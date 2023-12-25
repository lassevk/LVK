namespace LVK.Extensions.Logging.SmartInspect;

public sealed class SmartInspectLoggerConfiguration
{
    public string? ApplicationName { get; set; }

    public string? Connections { get; set; }
    public bool Enabled { get; set; }
    public SmartInspectMonitorsConfiguration Monitors { get; } = new();

    public Dictionary<string, SmartInspectLoggerSessionConfiguration> Sessions { get; } = new(StringComparer.InvariantCultureIgnoreCase);
}

public class SmartInspectMonitorsConfiguration
{
    public bool Cpu { get; set; }
    public bool GC { get; set; }
    public bool Memory { get; set; }
}

public class SmartInspectLoggerSessionConfiguration
{
    public bool? Enabled { get; set; }
    public string? Color { get; set; }
    public string? Level { get; set; }
}