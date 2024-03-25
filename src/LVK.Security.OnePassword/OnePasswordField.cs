using System.Text.Json.Serialization;

namespace LVK.Security.OnePassword;

public record OnePasswordField : OnePasswordItemElement
{
    [JsonPropertyName("id")]
    public required string Id { get; init; }

    [JsonPropertyName("type")]
    public required string Type { get; init; }

    [JsonPropertyName("label")]
    public string? Label { get; init; }

    [JsonPropertyName("purpose")]
    public string? Purpose { get; init; }

    [JsonPropertyName("value")]
    public string? Value { get; init; }

    [JsonPropertyName("reference")]
    public required string Reference { get; init; }

    [JsonPropertyName("entropy")]
    public double? Entropy { get; init; }

    [JsonPropertyName("password_details")]
    public OnePasswordItemPasswordDetails? PasswordDetails { get; init; }
}