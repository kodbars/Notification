using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TgBotNotification.Controllers
{
    public class DefaultMessageController
    {
        private readonly ITelegramBotClient _telegramBotClient;

        public DefaultMessageController(ITelegramBotClient telegramBotClient)
        {
            _telegramBotClient = telegramBotClient;
        }

        public async Task Handle(Message message, CancellationToken ct)
        {
            Console.WriteLine($"Контроллер {GetType().Name} получил сообщение");
            await _telegramBotClient.SendMessage(message.Chat.Id, "Дорогой друг&#128151;, отправь пожалуйста текст", parseMode: ParseMode.Html, cancellationToken: ct);
        }
    }
}
