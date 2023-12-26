using System.Collections.Concurrent;
using System.Text;
using Bot.BLL.GameLogic;
using Bot.BLL.Interfaces;
using Bot.BLL.Models;
using Bot.BLL.TelegramLogic;
using Microsoft.AspNetCore.Mvc;

namespace Bot.WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BotController : Controller
{
    private readonly IBotService _botService;
   // private static ConcurrentQueue<ResultJson> _inputMessagesQueue  = new();
   private readonly ITeamManager _teamManager;
    public BotController(IBotService botService, ITeamManager teamManager)
    {
        _botService = botService;
        _teamManager = teamManager;
        // _botService.OnInputMessages += InputMessagesHandler;
    }

    [HttpPost("CreateNewTeam")]
    public IActionResult CreateNewTeam(string teamName, string password)
    {
        _teamManager.CreateNewTeam(teamName, password);
        
        return Ok();
    }

    [HttpGet("Teams")]
    public IActionResult Teams()
    {
        return Ok(_teamManager.GetTeams());
    }
    
    [HttpPost]
    public IActionResult Post(string chatId, string message)
    {
        _botService.SendMessageAsync(chatId, message);
        
        return Ok();
    }
/*
    [HttpGet]
    public IActionResult Get()
    {
        var sb = new StringBuilder();
        while (true)
        {
            // Извлечение элемента из очереди
            if (_inputMessagesQueue.TryDequeue(out ResultJson result))
            {
                //BackupManager.WriteObjectToFile(result, FilePath);
                sb.Append(result.Message.Chat.Id + "\t" + result.Message.From.Username + "\n" +
                          result.Message.Text + "\n");
            }
            else
            {
                break;
            }
        }

        return Ok(sb.ToString());
    }
    
   
    private void InputMessagesHandler(object? sender, InputMessagesEventArgs inputMessagesEvent)
    {
        Console.WriteLine("Событие обработано!");
        var messages = inputMessagesEvent.InputMessagesData;
        foreach (var message in messages)
        {
            _inputMessagesQueue.Enqueue(message);
        }
    }
    */
}