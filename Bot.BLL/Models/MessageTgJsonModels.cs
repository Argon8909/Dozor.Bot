using Newtonsoft.Json;

namespace Bot.Ws.Models;

// MessagesJson myDeserializedClass = JsonConvert.DeserializeObject<MessagesJson>(myJsonResponse);
public class ChatJson
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("first_name")]
    public string? FirstName { get; set; }

    [JsonProperty("username")]
    public string? Username { get; set; }

    [JsonProperty("type")]
    public string? Type { get; set; }
}

public class EntityJson
{
    [JsonProperty("offset")]
    public int Offset { get; set; }

    [JsonProperty("length")]
    public int Length { get; set; }

    [JsonProperty("type")]
    public string? Type { get; set; }
}

public class FromJson
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("is_bot")]
    public bool IsBot { get; set; }

    [JsonProperty("first_name")]
    public string? FirstName { get; set; }

    [JsonProperty("username")]
    public string? Username { get; set; }

    [JsonProperty("language_code")]
    public string? LanguageCode { get; set; }
}

public class MessageJson
{
    [JsonProperty("message_id")]
    public int MessageId { get; set; }

    [JsonProperty("from")]
    public FromJson? From { get; set; }

    [JsonProperty("chat")]
    public ChatJson? Chat { get; set; }

    [JsonProperty("date")]
    public int Date { get; set; }

    [JsonProperty("text")]
    public string? Text { get; set; }

    [JsonProperty("entities")]
    public List<EntityJson>? Entities { get; set; }
}

public class ResultJson
{
    [JsonProperty("update_id")]
    public int UpdateId { get; set; }

    [JsonProperty("message")]
    public MessageJson? Message { get; set; }
}

public class MessagesJson
{
    [JsonProperty("ok")]
    public bool Ok { get; set; }

    [JsonProperty("result")]
    public List<ResultJson>? Results { get; set; }
}

