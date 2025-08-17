using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;
using System.Net.Http.Json;
using NotificationsDb.Data;


namespace TgBotNotification.Controllers
{
    public class TextMessageController
    {
        private readonly ITelegramBotClient _telegramBotClient;
        private static readonly HttpClient _httpClient = new HttpClient();
        private const string ApiUrl = "http://localhost:5264/api/notifications";

        public TextMessageController(ITelegramBotClient telegramBotClient)
        {
            _telegramBotClient = telegramBotClient;
        }

        public async Task Handle(Message message, CancellationToken ct)
        {
                    
                    var response = await _httpClient.PostAsJsonAsync(ApiUrl, new Notification()
                    {
                        Message = message.Text
                    });
                    
                    await _telegramBotClient.SendMessage(message.Chat.Id, $"StatusCode: {response.EnsureSuccessStatusCode().StatusCode}\n Запись в бд прошла успешно", parseMode: ParseMode.Html, cancellationToken: ct);
        }
    }
}
