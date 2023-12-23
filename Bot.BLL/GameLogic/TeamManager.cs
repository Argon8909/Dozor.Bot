using System.Collections.Concurrent;
using Bot.BLL.Interfaces;
using Bot.BLL.Models;
using Bot.BLL.TelegramLogic;
using Bot.Ws;
using Bot.Ws.Models;
using Microsoft.Extensions.Options;

namespace Bot.BLL.GameLogic;

public class TeamManager : ITeamManager
{
    /*
    private ConcurrentQueue<ResultJson> InputMessagesQueue { get; } = new();
    private Team _adminTeam;
    private readonly IBotService _botService;
    public TeamManager(IOptions<BotSettings> options, IBotService botService)
    {
        _botService = botService;
        _botService.OnInputMessages += InputMessagesHandler;
        _adminTeam = new Team("Admins", TeamStatus.Admins)
        {
            Members = { }
        };
    }

    private List<Member> AddAdmins(IOptions<BotSettings> options)
    {
        foreach (var adminTgUsername in options.Value.Admins)
        {
            _botService.SendMessageAsync();
            var admin = new Member()
            {

            };
        }
    }

    private void InputMessagesHandler(object? sender, InputMessagesEventArgs inputMessagesEvent)
    {
        Console.WriteLine("Событие обработано!");
        var messages = inputMessagesEvent.InputMessagesData;
        foreach (var message in messages)
        {
            InputMessagesQueue.Enqueue(message);
        }
    }
*/
}