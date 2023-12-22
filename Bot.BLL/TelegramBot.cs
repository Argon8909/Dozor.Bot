using System.Collections.Concurrent;
using System.Diagnostics;
using Bot.WebApp;
using Bot.Ws.Models;
using Bot.Ws.Models;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Timers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Timer = System.Timers.Timer;


namespace Bot.Ws;

public class TelegramBot : IHostedService, IDisposable
{
    private readonly IBotService _botService;
    private readonly Timer _timer;
    private readonly CancellationTokenSource _cancellationTokenSource;

    public TelegramBot(IBotService botService, IOptions<BotSettings> options)
    {
        _cancellationTokenSource = new CancellationTokenSource();
        _botService = botService;
        _timer = new Timer();
        _timer.Interval = TimeSpan.FromMilliseconds(options.Value.UpdateTime).TotalMilliseconds;
        _timer.Elapsed += OnTimerElapsed;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer.Start();
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer.Stop();
        _cancellationTokenSource.Cancel();
        return Task.CompletedTask;
    }

    private void OnTimerElapsed(object sender, ElapsedEventArgs e)
    {
        // Логика для таймера
        // Console.WriteLine("Сработал таймер ");
        _botService.InputMessagesHandler(_cancellationTokenSource.Token);
    }

    public void Dispose()
    {
        _timer.Dispose();
        _cancellationTokenSource.Dispose();
    }
}

/*
 public class TelegramBot : IHostedService, IDisposable
{
    private readonly IBotService _botService;
    private readonly Timer _timer;
    private readonly CancellationTokenSource _cancellationTokenSource;

    public TelegramBot(IBotService botService)
    {
        _botService = botService;
        _timer = new Timer();
        _timer.Interval = TimeSpan.FromSeconds(5).TotalMilliseconds;
        _timer.Elapsed += OnTimerElapsed;

        _cancellationTokenSource = new CancellationTokenSource();
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer.Start();
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer.Stop();
        _cancellationTokenSource.Cancel();
        return Task.CompletedTask;
    }

    private void OnTimerElapsed(object sender, ElapsedEventArgs e)
    {
        // Логика для таймера
        Console.WriteLine("Сработал таймер ");
        _botService.OnInputMessagesHandler(_cancellationTokenSource.Token);
    }

    public void Dispose()
    {
        _timer.Dispose();
        _cancellationTokenSource.Dispose();
    }
}
*/