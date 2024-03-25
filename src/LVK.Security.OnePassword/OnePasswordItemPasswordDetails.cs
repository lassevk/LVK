using System.Text.Json.Serialization;

namespace LVK.Security.OnePassword;

public record OnePasswordItemPasswordDetails : OnePasswordItemElement
{
    [JsonPropertyName("entropy")]
    public int? Entropy { get; init; }

    [JsonPropertyName("generated")]
    public bool Generated { get; init; }

    [JsonPropertyName("strength")]
    public string? Strength { get; init; }

    [JsonPropertyName("history")]
    public List<string> History { get; } = new();
}