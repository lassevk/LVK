namespace LVK.Notifications.Pushover;

internal class PushoverNotificationOptions
{
    public string? DefaultUser { get; set; }
    public required string ApiToken { get; set; }
}