using Newtonsoft.Json;

namespace Bot.Ws;

// MessagesRoot myDeserializedClass = JsonConvert.DeserializeObject<MessagesRoot>(myJsonResponse);
public class Chat
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("first_name")]
    public string FirstName { get; set; }

    [JsonProperty("username")]
    public string Username { get; set; }

    [JsonProperty("type")]
    public string Type { get; set; }
}

public class Entity
{
    [JsonProperty("offset")]
    public int Offset { get; set; }

    [JsonProperty("length")]
    public int Length { get; set; }

    [JsonProperty("type")]
    public string Type { get; set; }
}

public class From
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("is_bot")]
    public bool IsBot { get; set; }

    [JsonProperty("first_name")]
    public string FirstName { get; set; }

    [JsonProperty("username")]
    public string Username { get; set; }

    [JsonProperty("language_code")]
    public string LanguageCode { get; set; }
}

public class Message
{
    [JsonProperty("message_id")]
    public int MessageId { get; set; }

    [JsonProperty("from")]
    public From From { get; set; }

    [JsonProperty("chat")]
    public Chat Chat { get; set; }

    [JsonProperty("date")]
    public int Date { get; set; }

    [JsonProperty("text")]
    public string Text { get; set; }

    [JsonProperty("entities")]
    public List<Entity> Entities { get; set; }
}

public class Result
{
    [JsonProperty("update_id")]
    public int UpdateId { get; set; }

    [JsonProperty("message")]
    public Message Message { get; set; }
}

public class MessagesRoot
{
    [JsonProperty("ok")]
    public bool Ok { get; set; }

    [JsonProperty("result")]
    public List<Result> Result { get; set; }
}

