using System.ComponentModel;
using Bot.Ws;



IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<TelegramBot>();
        
            // services.Configure<TelegramBotOptions>(Configuration.GetSection("TelegramBot"));
    })
    .Build();

host.Run();