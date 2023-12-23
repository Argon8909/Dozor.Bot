using System.Text;
using Bot.Ws;
using Bot.Ws.Models;
using Microsoft.Extensions.Hosting;

namespace Bot.BLL;

public class Engine : IHostedService  //, IDisposable
{
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
            
            
            
            await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
        }
    }

   
    // Логика метода написана для эксперимента.
    private void OnInputMessagesHandler()
    {
        Console.WriteLine("Событие обработано!");
        var sb = new StringBuilder();
        while (true)
        {
            // Извлечение элемента из очереди
            if (_botService.InputMessagesQueue.TryDequeue(out ResultJson result))
            {
                BackupManager.WriteObjectToFile<ResultJson>(result, FilePath );
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
    }

    private void TestSendMessages()
    {
      var messages =  BackupManager.ReadObjectsFromFile<ResultJson>(FilePath);
      foreach (var message in messages)
      {
          _botService.SendMessageAsync(message.Message.Chat.Id.ToString(), message.Message.Text);
      }
      
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