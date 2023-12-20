using System.Collections.Concurrent;
using System.Diagnostics;
using Bot.WebApp;
using Bot.Ws.Models;
using Bot.Ws.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;


namespace Bot.Ws;

public class TelegramBot : BackgroundService
{
    private readonly IBotService _botService;
    
    public TelegramBot(IBotService botService)
    {
        _botService = botService;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            Console.WriteLine("qqqqqqqqqqqqqqqqqqqqq");
            await _botService.InputMessagesHandler(cancellationToken);
            await Task.Delay(1000, cancellationToken);
        }
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