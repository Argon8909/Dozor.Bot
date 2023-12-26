using Bot.BLL.TelegramLogic;

namespace Bot.BLL.Interfaces;

public interface IBotService
{
    Task InputMessagesHandler(CancellationToken cancellationToken);
    void SendMessageAsync(string chatId, string text);
    event EventHandler<InputMessagesEventArgs> OnInputMessages;
}