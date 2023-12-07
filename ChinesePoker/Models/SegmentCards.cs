using System.Text.Json.Serialization;

namespace ChinesePoker.Models;

public class SegmentCards
{
    [JsonPropertyName("segment_1")]
    public List<int> Segment1 { get; set; } = [];
    
    [JsonPropertyName("segment_2")]
    public List<int> Segment2 { get; set; } = [];

    [JsonPropertyName("segment_3")]
    public List<int> Segment3 { get; set; } = [];
}