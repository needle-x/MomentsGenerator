using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Microsoft.Extensions.Logging;

namespace ContentGeneratorApp
{
    public partial class MainForm : Form
    {
        private readonly AzureImagePoetryClient _client;

        public MainForm(AzureImagePoetryClient client)
        {
            InitializeComponent();
            this.ClientSize = new Size(800, 600);
            this.Text = "文案生成器";

            _client = client;

            // 填充风格选择框
            comboBoxStyle.Items.AddRange(new string[] { "古诗词", "文言文", "散文", "经典电影台词" });
        }

        // 点击生成按钮时触发
        private async void buttonGenerateContent_Click(object sender, EventArgs e)
        {
            string keyword = textBoxKeyword.Text.Trim();
            string style = comboBoxStyle.SelectedItem?.ToString()?.Trim();
            string imagePath = textBoxImagePath.Text.Trim();

            if (string.IsNullOrWhiteSpace(keyword) || string.IsNullOrWhiteSpace(style))
            {
                MessageBox.Show("请提供关键词和风格。");
                return;
            }

            try
            {
                // 在输出框中显示“正在生成”
                textBoxOutput.Text = "正在生成...";
                textBoxOutput.Text = string.Empty;

                // 如果提供了图片路径，则生成图文结合的内容
                if (!string.IsNullOrWhiteSpace(imagePath) && File.Exists(imagePath))
                {
                    await foreach (var part in _client.GenerateContentStreamAsync(imagePath, keyword, style))
                    {
                        textBoxOutput.Text += part;
                    }
                }
                else
                {
                    // 如果没有图片，生成纯文本内容
                    await foreach (var part in _client.GenerateTextOnlyContentStreamAsync(keyword, style))
                    {
                        textBoxOutput.Text += part;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"错误: {ex.Message}");
            }
        }

        // 浏览文件按钮，选择图片
        private void buttonBrowseImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                textBoxImagePath.Text = openFileDialog.FileName;
            }
        }
    }
}
