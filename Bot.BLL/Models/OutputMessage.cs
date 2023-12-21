using System.Text.Json.Serialization;

namespace Bot.BLL.Models;


// Root myDeserializedClass = JsonSerializer.Deserialize<Root>(myJsonResponse);
public class Chat
{
    [JsonPropertyName("id")]
    public long id { get; set; }

    [JsonPropertyName("first_name")]
    public string first_name { get; set; }

    [JsonPropertyName("username")]
    public string username { get; set; }

    [JsonPropertyName("type")]
    public string type { get; set; }
}

public class From
{
    [JsonPropertyName("id")]
    public long id { get; set; }

    [JsonPropertyName("is_bot")]
    public bool is_bot { get; set; }

    [JsonPropertyName("first_name")]
    public string first_name { get; set; }

    [JsonPropertyName("username")]
    public string username { get; set; }
}

public class Result
{
    [JsonPropertyName("message_id")]
    public int message_id { get; set; }

    [JsonPropertyName("from")]
    public From from { get; set; }

    [JsonPropertyName("chat")]
    public Chat chat { get; set; }

    [JsonPropertyName("date")]
    public int date { get; set; }

    [JsonPropertyName("text")]
    public string text { get; set; }
}

public class Root
{
    [JsonPropertyName("ok")]
    public bool ok { get; set; }

    [JsonPropertyName("result")]
    public Result result { get; set; }
}

