using System.Timers;
using Bot.Ws;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Timer = System.Timers.Timer;


namespace Bot.BLL.TelegramLogic;

public class TgUpdateLoop : IHostedService, IDisposable
{
    private readonly IBotService _botService;
    private readonly Timer _timer;
    private readonly CancellationTokenSource _cancellationTokenSource;

    public TgUpdateLoop(IBotService botService, IOptions<BotSettings> options)
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
 
*/