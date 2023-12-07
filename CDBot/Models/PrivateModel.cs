using System.Text.Json.Serialization;
using ChinesePoker.Models;

namespace Models;

public class PrivateModel
{
    [JsonPropertyName("player_id")]
    public string? PlayerId { get; set;}

    [JsonPropertyName("arrangement_id")]
    public string? ArrangemnetId { get; set; }

    [JsonPropertyName("input_hash")]
    public long InputHash { get; set; }

    [JsonPropertyName("input")]
    public List<int>? Input { get; set; }

    [JsonPropertyName("output")]
    public SegmentCards? Output { get; set; }
}