using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

public class AzureImagePoetryClient
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly string _endpoint;
    private readonly string _deployment;

    public AzureImagePoetryClient(string apiKey, string endpoint, string deployment)
    {
        this._apiKey = apiKey;
        this._endpoint = endpoint;
        this._deployment = deployment;
        this._httpClient = new HttpClient();
        this._httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
    }

    public async IAsyncEnumerable<string> GenerateContentStreamAsync(string imagePath, string keyword, string style)
    {
        byte[] imageBytes = await File.ReadAllBytesAsync(imagePath);
        string imageBase64 = Convert.ToBase64String(imageBytes);

        string reference = style switch
        {
            "古诗词" => "《诗经》、《古诗十九首》、《乐府诗集》、《唐诗三百首》、《宋词三百首》等",
            "文言文" => "《左传》、《战国策》、《史记》、《汉书》、《古文观止》、《论语》、《庄子》等",
            "散文" => "朱自清《春》、鲁迅《朝花夕拾》、冰心《寄小读者》、林清玄《温一壶月光下酒》等",
            "经典电影台词" => "《霸王别姬》、《阳光灿烂的日子》、《大话西游》、《肖申克的救赎》、《阿甘正传》、《这个杀手不太冷》等",
            _ => ""
        };

        string prompt = $"""
            请仅参考已有的{style}作品（如{reference}），结合关键词“{keyword}”和这张图片，从中直接引用一段或数句贴切的原文，**不要进行任何创作或解释**。

            只需返回如下格式：
            **作者《作品名》**：
            “原文内容。”

            如果能标注出处更佳。
            """;

        var messages = new[]
        {
            new {
                role = "user",
                content = new object[]
                {
                    new { type = "text", text = prompt },
                    new {
                        type = "image_url",
                        image_url = new {
                            url = $"data:image/jpeg;base64,{imageBase64}"
                        }
                    }
                }
            }
        };

        await foreach (var chunk in this.GenerateStreamAsync(messages))
        {
            yield return chunk;
        }
    }

    public async IAsyncEnumerable<string> GenerateTextOnlyContentStreamAsync(string keyword, string style)
    {
        string prompt = $"请用{style}风格，围绕“{keyword}”写一段文案。";

        var messages = new[]
        {
            new {
                role = "user",
                content = prompt
            }
        };

        await foreach (var chunk in this.GenerateStreamAsync(messages))
        {
            yield return chunk;
        }
    }

    private async IAsyncEnumerable<string> GenerateStreamAsync(object messages)
    {
        var requestBody = new
        {
            messages = messages,
            temperature = 0.7,
            max_tokens = 800,
            stream = true
        };

        var requestJson = JsonSerializer.Serialize(requestBody);
        var url = $"{this._endpoint}openai/deployments/{this._deployment}/chat/completions?api-version=2025-01-01-preview";
        var request = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = new StringContent(requestJson, Encoding.UTF8, "application/json")
        };

        var response = await this._httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"请求失败: {response.StatusCode}\n{error}");
        }

        using var stream = await response.Content.ReadAsStreamAsync();
        using var reader = new StreamReader(stream);

        while (!reader.EndOfStream)
        {
            var line = await reader.ReadLineAsync();
            if (string.IsNullOrWhiteSpace(line) || !line.StartsWith("data:"))
            {
                continue;
            }

            var jsonPart = line.Substring("data:".Length).Trim();
            if (jsonPart == "[DONE]")
            {
                break;
            }

            using var jsonDoc = JsonDocument.Parse(jsonPart);
            if (jsonDoc.RootElement.TryGetProperty("choices", out var choices) &&
                choices.ValueKind == JsonValueKind.Array &&
                choices.GetArrayLength() > 0 &&
                choices[0].TryGetProperty("delta", out var delta) &&
                delta.TryGetProperty("content", out var contentElement))
            {
                var content = contentElement.GetString();
                if (!string.IsNullOrEmpty(content))
                {
                    yield return content;
                }
            }

        }
    }
}
