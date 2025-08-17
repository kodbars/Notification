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
            .ConfigureServices((hostContext, services) => ConfigureServices(services)) // Задаем конфигурацию
            .UseConsoleLifetime() // Позволяет поддерживать приложение активным в консоли
            .Build(); // Собираем


        Console.WriteLine("Сервис запущен");
        await hots.RunAsync(); // Запускаем сервис
        Console.WriteLine("Сервс остановлен");
    }

    static void ConfigureServices(IServiceCollection services)
    {
        AppSettings appSettings = BuildAppSettings();
        services.AddSingleton(appSettings);

        // Подключаем контроллеры сообщений и кнопок
        services.AddTransient<DefaultMessageController>();
        services.AddTransient<TextMessageController>();

        // Регистрируем объект TelegramBotClient c токеном подключения
        services.AddSingleton<ITelegramBotClient>(provider => new TelegramBotClient(appSettings.BotToken));
        // Регистрируем постоянно активный сервис бота
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