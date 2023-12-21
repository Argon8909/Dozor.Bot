using System.Collections.Concurrent;
using Bot.WebApp;
using Bot.Ws.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Bot.Ws;

public class BotService : IBotService
{
    private readonly SemaphoreSlim _updateSemaphore = new(1, 1);
    private readonly string _botToken;
    private readonly object _inputMessagesHandlerLock = new object();
    private readonly HttpClient _httpClient;
    int offset = 0;
    public ConcurrentQueue<ResultJson> InputMessagesQueue { get; } = new ConcurrentQueue<ResultJson>();


    public BotService(IOptions<BotSettings> options)
    {
        _botToken = options.Value.Token;
        _httpClient = new HttpClient();
    }

    public async Task InputMessagesHandler(CancellationToken cancellationToken)
    {
        MessagesJson updates;

        // Используем SemaphoreSlim для синхронизации
        await _updateSemaphore.WaitAsync(cancellationToken);
        try
        {
            // Получаем обновления после захвата SemaphoreSlim
            updates = await GetUpdatesAsync(offset, cancellationToken);
            var tmpOffset = InputQueueHandler(updates, cancellationToken);
            if (tmpOffset > offset)
            {
                offset = tmpOffset;
            }
        }
        finally
        {
            _updateSemaphore.Release();
        }
    }

    private int InputQueueHandler(MessagesJson updates, CancellationToken cancellationToken)
    {
        int offset = 0;

        if (!cancellationToken.IsCancellationRequested && updates.Results.Count > 0)
        {
            foreach (var update in updates.Results)
            {
                InputMessagesQueue.Enqueue(update);
                offset = update.UpdateId + 1;
            }
        }

        return offset;
    }


    public async Task<MessagesJson?> GetUpdatesAsync(int offset, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return null;
        }

        try
        {
            var apiUrl = $"https://api.telegram.org/bot{_botToken}/getUpdates?offset={offset}";
            // Console.WriteLine("apiUrl ===> " + apiUrl);

            var response = await _httpClient.GetAsync(apiUrl, cancellationToken);
            var content = await response.Content.ReadAsStringAsync();

            // Console.WriteLine("response ===> " + content);

            var updates = JsonConvert.DeserializeObject<MessagesJson>(content);
            return updates;
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"HTTP Request Exception: {ex.Message}");
            return null;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }


    public void SendMessageAsync(string chatId, string text)
    {
        using (var httpClient = new HttpClient())
        {
            var apiUrl = $"https://api.telegram.org/bot{_botToken}/sendMessage?chat_id={chatId}&text={text}";
            Console.WriteLine("apiUrl ===> " + apiUrl);
            var res = httpClient.GetStringAsync(apiUrl);
            Console.WriteLine("res: " + res.Result.ToString());
        }
    }

    public ConcurrentQueue<ResultJson> GetMessages()
    {
        return InputMessagesQueue;
        // return new Queue<ResultJson>(InputMessagesQueue.Where(msg => msg.Message.Chat.Id == chatId));
    }
}