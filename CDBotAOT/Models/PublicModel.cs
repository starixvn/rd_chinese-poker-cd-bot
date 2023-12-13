using System.Text.Json.Serialization;

namespace Models;

public class PublicModel
{
    [JsonPropertyName("match")]
    public MatchModel? match { get; set; }

    [JsonPropertyName("is_have_output")]
    public List<bool>? IsHaveOutput { get; set; }
}

public class MatchModel
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("hash_slot_1")]
    public long HashSlot1 { get; set; }

    [JsonPropertyName("hash_slot_2")]
    public long HashSlot2 { get; set; }

    [JsonPropertyName("hash_slot_3")]
    public long HashSlot3 { get; set; }

    [JsonPropertyName("hash_slot_4")]
    public long HashSlot4 { get; set; }
}