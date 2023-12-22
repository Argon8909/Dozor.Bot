namespace Bot.BLL.Models;

public class InternalUpdate
{
    public int UpdateId { get; set; }
    public InternalMessage Message { get; set; }
}

public class InternalMessage
{
    public int MessageId { get; set; }
    public InternalFrom? From { get; set; }
    public InternalChat? Chat { get; set; }
    public int Date { get; set; }
    public string? Text { get; set; }
    public List<InternalEntity>? Entities { get; set; }
}

public class InternalFrom
{
    public long Id { get; set; }
    public bool IsBot { get; set; }
    public string? FirstName { get; set; }
    public string? Username { get; set; }
    public string? LanguageCode { get; set; }
}

public class InternalEntity
{
    public int Offset { get; set; }
    public int Length { get; set; }
    public string? Type { get; set; }
}

public class InternalChat
{
    public long Id { get; set; }
    public string? FirstName { get; set; }
    public string? Username { get; set; }
    public string? Type { get; set; }
}
