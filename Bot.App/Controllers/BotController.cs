using System.Text;
using Bot.Ws;
using Microsoft.AspNetCore.Mvc;

namespace Bot.WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BotController : Controller
{
    private readonly IBotService _botService;
    
    public BotController(IBotService botService)
    {
        _botService = botService;
    }
    
    [HttpGet]
    public IActionResult Get()
    {
        var msg =_botService.GetMessages();
        var sb = new StringBuilder();
        
        foreach (var resultJson in msg)
        {
            sb.Append(resultJson.Message.Text + "\t" + resultJson.Message.From.Username + "\n");
        }
        
        return Ok(sb.ToString());
    }
}