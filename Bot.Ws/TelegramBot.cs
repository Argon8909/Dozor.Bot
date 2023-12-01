using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Bot.Ws;

public class TelegramBot : BackgroundService
{
    private readonly string _botToken = "6312399390:AAFp9ahKllgC93T16KD2sA2q39CAIMwyJ3w";
    private readonly HttpClient _httpClient;

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


            foreach (var update in updates.Result)
            {
                // Обработка входящего сообщения
                // ProcessUpdate(update);
                Console.WriteLine(update.Message.Text);
                // Увеличиваем offset, чтобы не получать обновления повторно
                offset = update.UpdateId + 1;
            }

            await Task.Delay(1000, stoppingToken);
        }
    }
/*
    private void ProcessUpdate(Update update)
    {
        if (update.Message != null)
        {
            var message = update.Message;
            Console.WriteLine($"Received message from {message.Chat.Id}: {message.Text}");

            // Здесь обрабатывай сообщение, например, отправляй ответ
            SendMessage(message.Chat.Id, "Привет! Я бот на .NET без библиотек.");
        }
    }
*/
    private async Task<MessagesRoot> GetUpdatesAsync(int offset, CancellationToken cancellationToken) // List<Update>
    {
        try
        {
            var apiUrl = $"https://api.telegram.org/bot{_botToken}/getUpdates?offset={offset}";
            var response = await _httpClient.GetStringAsync(apiUrl, cancellationToken);
            var updates = JsonConvert.DeserializeObject<MessagesRoot>(response);

            return updates;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private void SendMessage(int chatId, string text)
    {
        var apiUrl = $"https://api.telegram.org/bot{_botToken}/sendMessage?chat_id={chatId}&text={text}";
        _httpClient.GetStringAsync(apiUrl);
    }
}