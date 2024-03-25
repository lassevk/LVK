using System.Text.Json;
using System.Text.Json.Serialization;

namespace LVK.Security.OnePassword;

public record OnePasswordItemElement
{
    [JsonExtensionData]
    public Dictionary<string, JsonElement>? ExtensionData { get; set; }
}