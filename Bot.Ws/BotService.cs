using System.Collections.Concurrent;
using Bot.WebApp;
using Bot.Ws.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Bot.Ws;

public class BotService : IBotService
{
     private readonly string _botToken; //= "6312399390:AAFp9ahKllgC93T16KD2sA2q39CAIMwyJ3w";
    private readonly HttpClient _httpClient = new();
    // private ConcurrentQueue<ResultJson> _inputMessagesQueue = new();
    public ConcurrentQueue<ResultJson> InputMessagesQueue { get; } = new ConcurrentQueue<ResultJson>();

    

    public BotService(IOptions<BotSettings> options)
    {
        _botToken = options.Value.Token;
         _httpClient = new HttpClient();
         Console.WriteLine($">{options.Value.Token.ToString()}<");
    }

    public async Task InputMessagesHandler(CancellationToken cancellationToken)
    {
        var offset = 0;
        var updates = await GetUpdatesAsync(offset, cancellationToken);

        while (!cancellationToken.IsCancellationRequested && updates.Results.Count > 0)
        {
            updates = await GetUpdatesAsync(offset, cancellationToken);

            if (updates != null)
                foreach (var update in updates.Results)
                {
                    InputMessagesQueue.Enqueue(update);
                    offset = update.UpdateId + 1;
                }

            await Task.Delay(100, cancellationToken);
        }
    }


    public async Task<MessagesJson?>? GetUpdatesAsync(int offset, CancellationToken cancellationToken) // List<Update>
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return null;
            // return await Task.FromResult<MessagesJson?>(null);
        }

        try
        {
            using (var httpClient = new HttpClient())
            {
                var apiUrl = $"https://api.telegram.org/bot{_botToken}/getUpdates?offset={offset}";
                var response = await httpClient.GetStringAsync(apiUrl, cancellationToken);
                var updates = JsonConvert.DeserializeObject<MessagesJson>(response);
                return updates;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task SendMessageAsync(long chatId, string text)
    {
        using (var httpClient = new HttpClient())
        {
            var apiUrl = $"https://api.telegram.org/bot{_botToken}/sendMessage?chat_id={chatId}&text={text}";
            var res = await httpClient.GetStringAsync(apiUrl);
            Console.WriteLine("res: " + res);
        }
    }

    public ConcurrentQueue<ResultJson> GetMessages()
    {
        return InputMessagesQueue;
        // return new Queue<ResultJson>(InputMessagesQueue.Where(msg => msg.Message.Chat.Id == chatId));
    }
}