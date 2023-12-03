using Bot.Ws.Models;
using Bot.Ws.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;


namespace Bot.Ws;

public class TelegramBot : BackgroundService
{
    private readonly string _botToken = "6312399390:AAFp9ahKllgC93T16KD2sA2q39CAIMwyJ3w";
    private readonly HttpClient _httpClient;
    public Queue<ResultJson> _inputMessagesQueue = new Queue<ResultJson>();
    
    public TelegramBot(
        // IOptions<TelegramBotOptions> options
    )
    {
        //_botToken = options.Value.BotToken;
        _httpClient = new HttpClient();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var offset = 0;

        while (!stoppingToken.IsCancellationRequested)
        {
            var updates = await GetUpdatesAsync(offset, stoppingToken);
            Console.WriteLine(updates.Ok);


            foreach (var update in updates.Results)
            {
                // Обработка входящего сообщения
                ProcessUpdate(update);
                _inputMessagesQueue.Enqueue(update);
                
                Console.WriteLine(update.Message.Text);
                // Увеличиваем offset, чтобы не получать обновления повторно
                offset = update.UpdateId + 1;
            }

            await Task.Delay(1000, stoppingToken);
        }
    }

/// ///////////////////////////////////////

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
/// //////////////////////////////////

    private async Task<MessagesJson> GetUpdatesAsync(int offset, CancellationToken cancellationToken) // List<Update>
    {
        try
        {
            var apiUrl = $"https://api.telegram.org/bot{_botToken}/getUpdates?offset={offset}";
            var response = await _httpClient.GetStringAsync(apiUrl, cancellationToken);
            var updates = JsonConvert.DeserializeObject<MessagesJson>(response);

            return updates;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task SendMessageAsync(long chatId, string text)
    {
        var apiUrl = $"https://api.telegram.org/bot{_botToken}/sendMessage?chat_id={chatId}&text={text}";
        var res = await _httpClient.GetStringAsync(apiUrl);
        Console.WriteLine("res: " + res);
    }

    public Queue<ResultJson> GetMessages(long chatId)
    {
        return new Queue<ResultJson>(_inputMessagesQueue.Where(msg => msg.Message.Chat.Id == chatId));
    }
}