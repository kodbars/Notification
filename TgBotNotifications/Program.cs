using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text;
using Telegram.Bot;
using TgBotNotification.Configuration;
using TgBotNotification.Controllers;
using TgBotNotification;
using System;


public class Program
{
    private static async Task Main(string[] args)
    {
        Console.OutputEncoding = Encoding.Unicode;

        var hots = new HostBuilder()
            .ConfigureServices((hostContext, services) => ConfigureServices(services))
            .UseConsoleLifetime()
            .Build();


        Console.WriteLine("Сервис запущен");
        await hots.RunAsync();
        Console.WriteLine("Сервс остановлен");
    }

    static void ConfigureServices(IServiceCollection services)
    {
        AppSettings appSettings = BuildAppSettings();
        services.AddSingleton(appSettings);

        services.AddTransient<DefaultMessageController>();
        services.AddTransient<TextMessageController>();

        services.AddSingleton<ITelegramBotClient>(provider => new TelegramBotClient(appSettings.BotToken));
        
        services.AddHostedService<Bot>();
    }

    static AppSettings BuildAppSettings()
    {
        return new AppSettings()
        {
            BotToken = "7728526793:AAF8FY4tx3Yjrjde4DBwEmG5-2tlYxypN0Q",
        };
    }
}