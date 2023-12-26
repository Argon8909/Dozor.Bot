using System.Collections.Concurrent;
using System.Text;
using Bot.BLL.Interfaces;
using Bot.BLL.Models;
using Bot.BLL.TelegramLogic;
using Microsoft.Extensions.Hosting;

namespace Bot.BLL.GameLogic;

public class Engine : IHostedService
{
    private ConcurrentQueue<ResultJson> InputMessagesQueue { get; } = new();
    private CancellationTokenSource _cancellationTokenSource;
    private readonly IBotService _botService;
    private const string FilePath = @"A:\data\dozor_backup.json"; // A:\data

    public Engine(IBotService botService)
    {
        _botService = botService;
        _botService.OnInputMessages += OnInputMessagesHandler;
    }

    private async Task GameControl(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            
            // логика проведения игры тут
            //
            //
            await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
        }
    }
    
    private void OnInputMessagesHandler(object? sender, InputMessagesEventArgs inputMessagesEvent)
    {
        Console.WriteLine("Событие обработано!");
        var messages = inputMessagesEvent.InputMessagesData;
        foreach (var message in messages)
        {
            InputMessagesQueue.Enqueue(message);
        }
        MessageHandler();
    }

// Логика метода написана для эксперимента.
    private void TestSendMessages()
    {
        var messages = BackupManager.ReadObjectsFromFile<ResultJson>(FilePath);
        foreach (var message in messages)
        {
            _botService.SendMessageAsync(message.Message.Chat.Id.ToString(), message.Message.Text);
        }
    }

    public Task MessageHandler()
    {
        var sb = new StringBuilder();
        while (true)
        {
            // Извлечение элемента из очереди
            if (InputMessagesQueue.TryDequeue(out ResultJson result))
            {
                BackupManager.WriteObjectToFile(result, FilePath);
                sb.Append(result.Message.Chat.Id + "\t" + result.Message.From.Username + "\n" +
                          result.Message.Text + "\n");
            }
            else
            {
                // Очередь была пуста, обработка ситуации
                Console.WriteLine("Очередь пуста.");
                break;
            }
        }

        TestSendMessages();
        Console.WriteLine(sb.ToString());
        return Task.CompletedTask;
    }
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _cancellationTokenSource = new CancellationTokenSource();

        Task.Run(async () => await GameControl(_cancellationTokenSource.Token), _cancellationTokenSource.Token);

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _cancellationTokenSource?.Cancel();
        return Task.CompletedTask;
    }
}