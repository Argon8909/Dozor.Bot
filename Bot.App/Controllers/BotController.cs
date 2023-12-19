using System.Text;
using Bot.Ws;
using Microsoft.AspNetCore.Mvc;

namespace Bot.WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BotController : Controller
{
    private readonly TelegramBot _telegramBot;
    
    public BotController(TelegramBot telegramBot)
    {
        _telegramBot = telegramBot;
    }
    
    [HttpGet]
    public IActionResult Get()
    {
        var msg =_telegramBot.GetMessages();
        var sb = new StringBuilder();
        
        foreach (var resultJson in msg)
        {
            sb.Append(resultJson.Message.Text + "\t" + resultJson.Message.From.Username + "\n");
        }
        
        return Ok(sb.ToString());
    }
}