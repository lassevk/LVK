using Serilog;

namespace LVK.Extensions.Logging.Serilog;

public class SerilogConfiguration
{
    public bool Enabled { get; set; }
    public string? Path { get; set; }
    public SerilogLevel? MinLevel { get; set; }
    public RollingInterval? RollingInterval { get; set; }
    public long? FileSizeLimit { get; set; }
    public int? RetentionDays { get; set; }

    public bool IsValidAndEnabled() => Enabled && !string.IsNullOrWhiteSpace(Path);
}