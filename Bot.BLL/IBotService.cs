using System.Collections.Concurrent;
using Bot.Ws.Models;

namespace Bot.Ws;

public interface IBotService
{
    Task InputMessagesHandler(CancellationToken cancellationToken);
    Task<MessagesJson?> GetUpdatesAsync(int offset, CancellationToken cancellationToken);
    void SendMessageAsync(string chatId, string text);
    ConcurrentQueue<ResultJson> GetMessages();
}