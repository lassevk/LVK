using System.Text.Json.Serialization;

namespace LVK.Security.OnePassword;

public record OnePasswordVault : OnePasswordItemElement
{
    [JsonPropertyName("id")]
    public required string Id { get; init; }

    [JsonPropertyName("name")]
    public required string Name { get; init; }
}