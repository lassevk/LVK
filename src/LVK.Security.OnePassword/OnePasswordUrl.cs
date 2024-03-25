using System.Text.Json.Serialization;

namespace LVK.Security.OnePassword;

public record OnePasswordUrl : OnePasswordItemElement
{
    [JsonPropertyName("label")]
    public string? Label { get; init; }

    [JsonPropertyName("href")]
    public required string Address { get; init; }

    [JsonPropertyName("primary")]
    public bool IsPrimary { get; init; }
}