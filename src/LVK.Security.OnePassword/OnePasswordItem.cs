using System.Text.Json.Serialization;
// ReSharper disable CollectionNeverUpdated.Global

namespace LVK.Security.OnePassword;

public record OnePasswordItem : OnePasswordItemElement
{
    [JsonPropertyName("id")]
    public required string Id { get; init; }

    [JsonPropertyName("title")]
    public required string Title { get; init; }

    [JsonPropertyName("category")]
    public string? Category { get; init; }

    [JsonPropertyName("last_edited_by")]
    public string? LastEditedBy { get; init; }

    [JsonPropertyName("created_at")]
    public DateTimeOffset CreatedAt { get; init; }

    [JsonPropertyName("updated_at")]
    public DateTimeOffset UpdatedAt { get; init; }

    [JsonPropertyName("version")]
    public int Version { get; init; }

    [JsonPropertyName("additional_information")]
    public string? AdditionalInformation { get; init; }

    [JsonPropertyName("vault")]
    public OnePasswordVault? Vault { get; init; }

    [JsonPropertyName("urls")]
    public List<OnePasswordUrl> Urls { get; } = [];

    [JsonPropertyName("fields")]
    public List<OnePasswordField> Fields { get; } = [];

    [JsonPropertyName("tags")]
    public List<string> Tags { get; } = [];

    [JsonPropertyName("favorite")]
    public bool IsFavorite { get; init; }
}