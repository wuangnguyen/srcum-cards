using Newtonsoft.Json;

public class CardItem
{
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("value")]
    public byte Value { get; set; }
}