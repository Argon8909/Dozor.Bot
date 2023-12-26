namespace Bot.BLL.TelegramLogic;

public class BotSettings
{
    public string Token { get; set; }
    public int UpdateTime { get; set; }
    public List<string> Admins { get; set; }
    public List<string> PassWords { get; set; }
}