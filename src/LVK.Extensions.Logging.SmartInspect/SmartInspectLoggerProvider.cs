using System.Collections.Concurrent;
using System.Diagnostics;
using System.Runtime.Versioning;

using Gurock.SmartInspect;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LVK.Extensions.Logging.SmartInspect;

[UnsupportedOSPlatform("browser")]
[ProviderAlias("SmartInspect")]
internal class SmartInspectLoggerProvider : ILoggerProvider
{
    private readonly IDisposable? _onChangeSubscription;
    private readonly ConcurrentDictionary<string, SmartInspectLogger> _loggers =
        new(StringComparer.OrdinalIgnoreCase);

    private SmartInspectLoggerConfiguration _configuration;

    public SmartInspectLoggerProvider(IOptionsMonitor<SmartInspectLoggerConfiguration> configurationMonitor)
    {
        _configuration = configurationMonitor.CurrentValue;
        _onChangeSubscription = configurationMonitor.OnChange(configuration =>
        {
            _configuration = configuration;
            ConfigureSmartInspect();
        });

        ConfigureSmartInspect();
    }

    private void ConfigureSmartInspect()
    {
        SiAuto.Si.AppName = _configuration.ApplicationName ?? GetProcessName();
        SiAuto.Si.Connections = _configuration.Connections ?? "";
        SiAuto.Si.Enabled = _configuration.Enabled;

        foreach (SmartInspectLogger logger in _loggers.Values.ToList())
            logger.ConfigureSession();
    }

    private string GetProcessName()
    {
        using var process = Process.GetCurrentProcess();
        if (string.IsNullOrEmpty(process.ProcessName))
            return "Process";

        return process.ProcessName;
    }

    public void Dispose()
    {
        _loggers.Clear();
        _onChangeSubscription?.Dispose();
    }

    public ILogger CreateLogger(string categoryName)
    {
        return _loggers.GetOrAdd(categoryName, name => new SmartInspectLogger(name, _configuration));
    }
}