using System.Text.Json.Serialization;

namespace Models;

public class MoveRequestModel
{
    [JsonPropertyName("private")]
    public PrivateModel? Private { get; set; }

    [JsonPropertyName("public")]
    public PublicModel? Public { get; set; }
}