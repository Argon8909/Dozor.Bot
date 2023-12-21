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

    [HttpPost]
    public IActionResult Post(string chatId, string message)
    {
        _botService.SendMessageAsync(chatId, message);
        
        return Ok();
    }

    [HttpGet]
    public IActionResult Get()
    {
        var msg = _botService.GetMessages();
        var sb = new StringBuilder();

        foreach (var resultJson in msg)
        {
            sb.Append(resultJson.Message.Chat.Id + "\t" + resultJson.Message.From.Username + "\n" +
                      resultJson.Message.Text + "\n");
        }

        return Ok(sb.ToString());
    }
}