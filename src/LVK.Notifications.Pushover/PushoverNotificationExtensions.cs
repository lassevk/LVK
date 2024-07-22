using Microsoft.AspNetCore.StaticFiles;

namespace LVK.Notifications.Pushover;

public static class PushoverNotificationExtensions
{
    public static PushoverNotification TargettingUser(this PushoverNotification notification, string userKey)
        => notification with
        {
            TargetUser = userKey,
        };

    public static PushoverNotification WithHtmlContent(this PushoverNotification notification)
        => notification with
        {
            EnableHtml = true,
        };

    public static PushoverNotification WithTitle(this PushoverNotification notification, string title)
        => notification with
        {
            Title = title,
        };

    public static PushoverNotification WithTimestamp(this PushoverNotification notification, DateTimeOffset timestamp)
        => notification with
        {
            Timestamp = timestamp,
        };

    public static PushoverNotification WithTimeToLive(this PushoverNotification notification, TimeSpan ttl)
        => notification with
        {
            TimeToLive = (int)ttl.TotalSeconds,
        };

    public static PushoverNotification WithTimeToLive(this PushoverNotification notification, int seconds)
        => notification with
        {
            TimeToLive = seconds,
        };

    public static PushoverNotification WithSound(this PushoverNotification notification, PushoverNotificationSound sound)
        => notification with
        {
            Sound = sound,
        };

    public static PushoverNotification WithSound(this PushoverNotification notification, string sound)
        => notification with
        {
            CustomSound = sound,
        };

    public static PushoverNotification WithLowestPriority(this PushoverNotification notification)
        => notification with
        {
            Priority = PushoverNotificationPriority.Lowest,
        };

    public static PushoverNotification WithLowPriority(this PushoverNotification notification)
        => notification with
        {
            Priority = PushoverNotificationPriority.Low,
        };

    public static PushoverNotification WithNormalPriority(this PushoverNotification notification)
        => notification with
        {
            Priority = PushoverNotificationPriority.Normal,
        };

    public static PushoverNotification WithHighPriority(this PushoverNotification notification)
        => notification with
        {
            Priority = PushoverNotificationPriority.High,
        };

    // public static PushoverNotification WithEmergencyPriority(this PushoverNotification notification, TimeSpan retryInterval, TimeSpan expirationDuration)
    //     => notification with
    //     {
    //         Priority = PushoverNotificationPriority.Emergency,
    //         EmergencyRetryInterval = (int)retryInterval.TotalSeconds,
    //         EmergencyExpireDuration = (int)expirationDuration.TotalSeconds,
    //     };
    //
    // public static PushoverNotification WithEmergencyPriority(this PushoverNotification notification)
    //     => WithEmergencyPriority(notification, 60, 3 * 60 * 60);
    //
    // public static PushoverNotification WithEmergencyPriority(this PushoverNotification notification, int retryIntervalInSeconds)
    //     => WithEmergencyPriority(notification, retryIntervalInSeconds, 3 * 60 * 60);
    //
    // public static PushoverNotification WithEmergencyPriority(this PushoverNotification notification, TimeSpan retryInterval)
    //     => WithEmergencyPriority(notification, retryInterval, TimeSpan.FromHours(3));
    //
    // public static PushoverNotification WithEmergencyPriority(this PushoverNotification notification, int retryIntervalInSeconds, int expirationDurationInSeconds)
    //     => notification with
    //     {
    //         Priority = PushoverNotificationPriority.Emergency,
    //         EmergencyRetryInterval = retryIntervalInSeconds,
    //         EmergencyExpireDuration = expirationDurationInSeconds,
    //     };

    public static PushoverNotification WithUrl(this PushoverNotification notification, string url, string title)
        => notification with
        {
            Url = url, UrlTitle = title,
        };

    public static PushoverNotification WithAttachment(this PushoverNotification notification, byte[] content, string mimeType)
        => notification with
        {
            AttachmentBase64 = Convert.ToBase64String(content), AttachmentMimeType = mimeType,
        };

    public static PushoverNotification WithAttachment(this PushoverNotification notification, Stream content, string mimeType)
    {
        using var temp = new MemoryStream();
        content.CopyTo(temp);

        return WithAttachment(notification, temp.ToArray(), mimeType);
    }

    public static PushoverNotification WithAttachmentFromFile(this PushoverNotification notification, string filePath)
    {
        new FileExtensionContentTypeProvider().TryGetContentType(filePath, out string? contentType);
        return WithAttachment(notification, File.ReadAllBytes(filePath), contentType ?? "application/octet-stream");
    }
}