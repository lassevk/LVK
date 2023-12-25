namespace LVK.Extensions.Logging.SmartInspect;

public sealed class SmartInspectLoggerConfiguration
{
    public string? ApplicationName { get; set; }

    public string? Connections { get; set; }
    public bool Enabled { get; set; }
    public SmartInspectMonitorsConfiguration Monitors { get; } = new();

    public Dictionary<string, SmartInspectLoggerSessionConfiguration> Sessions { get; } = new(StringComparer.InvariantCultureIgnoreCase);
}