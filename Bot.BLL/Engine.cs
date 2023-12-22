using System.Text;
using Bot.Ws;
using Bot.Ws.Models;
using Microsoft.Extensions.Hosting;

namespace Bot.BLL;

public class Engine : IHostedService//, IDisposable
{
    private readonly IBotService _botService;

    public Engine(IBotService botService)
    {
        _botService = botService;
        _botService.OnInputMessages += OnInputMessagesHandler;
    }

    public void OnInputMessagesHandler()
    {
        Console.WriteLine("Событие обработано!");
        // _botService.InputMessagesQueue.TryDequeue();
        var sb = new StringBuilder();
        while (true)
        {
            // Извлечение элемента из очереди
            if (_botService.InputMessagesQueue.TryDequeue(out ResultJson result))
            {
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
        Console.WriteLine(sb.ToString());
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        // Логика запуска сервиса
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        // Логика остановки сервиса
        return Task.CompletedTask;
    }


  
}