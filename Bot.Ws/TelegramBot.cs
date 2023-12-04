using System.Collections.Concurrent;
using Bot.Ws.Models;
using Bot.Ws.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;


namespace Bot.Ws;

public class TelegramBot : BackgroundService
{
    private readonly string _botToken = "6312399390:AAFp9ahKllgC93T16KD2sA2q39CAIMwyJ3w";
    private readonly HttpClient _httpClient = new();
    // private ConcurrentQueue<ResultJson> _inputMessagesQueue = new();
    public ConcurrentQueue<ResultJson>? InputMessagesQueue { get; private set; }

/*
    public TelegramBot(
        // IOptions<TelegramBotOptions> options
    )
    {
        //_botToken = options.Value.BotToken;
        // _httpClient = new HttpClient();
    }
*/
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            await InputMessagesHandler(cancellationToken);
            await Task.Delay(1000, cancellationToken);
        }
    }

    private async Task InputMessagesHandler(CancellationToken cancellationToken)
    {
        var offset = 0;
        var updates = await GetUpdatesAsync(offset, cancellationToken);

        while (!cancellationToken.IsCancellationRequested && updates.Results.Count > 0)
        {
            updates = await GetUpdatesAsync(offset, cancellationToken);
            foreach (var update in updates.Results)
            {
                InputMessagesQueue.Enqueue(update);
                offset = update.UpdateId + 1;
            }

            await Task.Delay(100, cancellationToken);
        }
    }


    private async Task<MessagesJson?>? GetUpdatesAsync(int offset, CancellationToken cancellationToken) // List<Update>
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return null;
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

    public Queue<ResultJson> GetMessages(long chatId)
    {
        return new Queue<ResultJson>(InputMessagesQueue.Where(msg => msg.Message.Chat.Id == chatId));
    }
}

/*
 //////////////////////////////////////////
    private void ProcessUpdate(ResultJson update)
    {
        if (update.Message != null)
        {
            var message = update.Message;
            Console.WriteLine($"Received message from {message.Chat.Id}: {message.Text}");

            // Здесь обрабатывай сообщение, например, отправляй ответ
            SendMessageAsync(message.Chat.Id, "Привет! Я бот на .NET без библиотек.").Wait();
        }
    }

    /////////////////////////////////////
*/