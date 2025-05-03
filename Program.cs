using System;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Plugins;

namespace ContentGeneratorApp
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            var builder = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    var kernelSettings = KernelSettings.LoadSettings();

                    services
                        .AddSingleton<KernelSettings>(kernelSettings)
                        .AddTransient<Kernel>(serviceProvider =>
                        {
                            var kernelBuilder = Kernel.CreateBuilder();
                            kernelBuilder.Services.AddLogging(c => c.AddDebug().SetMinimumLevel(LogLevel.Information));
                            kernelBuilder.Services.AddChatCompletionService(kernelSettings);
                            kernelBuilder.Plugins.AddFromType<LightPlugin>();

                            return kernelBuilder.Build();
                        })
                        .AddSingleton<AzureImagePoetryClient>(serviceProvider =>
                        {
                            var kernelSettings = serviceProvider.GetRequiredService<KernelSettings>();
                            return new AzureImagePoetryClient(
                                kernelSettings.ApiKey,
                                kernelSettings.Endpoint,
                                kernelSettings.DeploymentId);
                        })
                        .AddSingleton<MainForm>();
                });

            var host = builder.Build();

            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(host.Services.GetRequiredService<MainForm>());
        }
    }
}
