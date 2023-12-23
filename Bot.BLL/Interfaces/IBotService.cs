using System.Collections.Concurrent;
using Bot.BLL.TelegramLogic;
using Bot.Ws.Models;

namespace Bot.Ws;

public interface IBotService
{
   // ConcurrentQueue<ResultJson> InputMessagesQueue { get; }

    Task InputMessagesHandler(CancellationToken cancellationToken);

    // Task<MessagesJson?> GetUpdatesAsync(int offset, CancellationToken cancellationToken);
    void SendMessageAsync(string chatId, string text);

    ConcurrentQueue<ResultJson> GetMessages();

    // delegate void InputMessagesDelegate();
   // event BotService.InputMessagesDelegate OnInputMessages;
    event EventHandler<InputMessagesEventArgs> OnInputMessages;
}