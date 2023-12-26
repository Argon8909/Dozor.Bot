using System.Collections.Concurrent;
using System.Text;
using Bot.BLL.Interfaces;
using Bot.BLL.Models;
using Bot.BLL.TelegramLogic;
using Microsoft.Extensions.Options;

namespace Bot.BLL.GameLogic;

public class TeamManager : ITeamManager
{
    private ConcurrentQueue<ResultJson> InputMessagesQueue { get; } = new();
    private ConcurrentQueue<OutputMessageToPlayers> OutputMessagesQueue { get; } = new();
    private Team _adminTeam;
    private readonly IBotService _botService;
    public GameStatus GameStatus;
    private IOptions<BotSettings> _options;
    private List<Team> Teams = new();

    public TeamManager(IOptions<BotSettings> options, IBotService botService )
    {
        _options = options;
        _botService = botService;
        //GameStatus = gameStatus;
        _botService.OnInputMessages += InputMessagesHandler;
        // _adminTeam = new Team("Admins", TeamStatus.Admins) { Members = { } };

        OnInputMessages += TeamSettings;
    }

    private void TeamSettings()
    {
        if (GameStatus != GameStatus.TeamSettings)
        {
            OnInputMessages -= TeamSettings;
            return;
        }

        var passwords = _options.Value.PassWords;

        foreach (var message in InputMessagesQueue)
        {
            var inputText = message.Message.Text.Trim().ToLower();
            // string matchingPassword = passwords.FirstOrDefault(password => password == inputText);
            if (inputText == "/start")
            {
                // CreateNewTeam(message);
            }
        }
    }

    public string CreateNewTeam(string teamName, string password, TeamStatus teamStatus = TeamStatus.Players)
    {
        var team = new Team(teamName, teamStatus, password);
        Teams.Add(team);
        
        return default;
    }

    public string GetTeams()
    {
        var sb = new StringBuilder();
        foreach (var team in Teams)
        {
            sb.Append(team.Name + "\n");
        }

        return sb.ToString();
    }

    public Task AddToOutputMessages(OutputMessageToPlayers messageToPlayers)
    {
        OutputMessagesQueue.Enqueue(messageToPlayers);
        return Task.CompletedTask;
    }
    private void InputMessagesHandler(object? sender, InputMessagesEventArgs inputMessagesEvent)
    {
        Console.WriteLine("Событие обработано!");
        var messages = inputMessagesEvent.InputMessagesData;
        foreach (var message in messages)
        {
            InputMessagesQueue.Enqueue(message);
        }

        OnInputMessages?.Invoke();
    }

    public delegate void InputMessagesDelegate();

    public event InputMessagesDelegate OnInputMessages;
}

public enum GameStatus
{
    AdminSettings,
    TeamSettings,
    Started,
    GameOver,
}