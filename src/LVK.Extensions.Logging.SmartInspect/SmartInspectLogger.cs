using System.Drawing;

using Gurock.SmartInspect;

using LVK.Core;

using Microsoft.Extensions.Logging;

namespace LVK.Extensions.Logging.SmartInspect;

internal class SmartInspectLogger : ILogger
{
    private readonly Session _session;
    private readonly SmartInspectLoggerConfiguration _configuration;
    private readonly string _name;

    public SmartInspectLogger(string name, SmartInspectLoggerConfiguration configuration)
    {
        _name = name;
        _session = SiAuto.Si.GetSession(name) ?? SiAuto.Si.AddSession(name);
        _configuration = configuration;

        ConfigureSession();
    }

    public void ConfigureSession()
    {
        ConfigureSessionWithDefaults();

        if (_configuration.Sessions.TryGetValue("Default", out SmartInspectLoggerSessionConfiguration? defaultConfiguration))
            ConfigureSession(defaultConfiguration);

        if (_configuration.Sessions.TryGetValue(_name, out SmartInspectLoggerSessionConfiguration? sessionConfiguration))
            ConfigureSession(sessionConfiguration);
    }

    private void ConfigureSession(SmartInspectLoggerSessionConfiguration sessionConfiguration)
    {
        if (sessionConfiguration.Enabled != null)
            _session.Active = sessionConfiguration.Enabled ?? false;

        if (!string.IsNullOrWhiteSpace(sessionConfiguration.Color))
            _session.Color = Color.FromName(sessionConfiguration.Color);

        if (!string.IsNullOrWhiteSpace(sessionConfiguration.Level))
            _session.Level = LogLevelToLevel(Enum.Parse<LogLevel>(sessionConfiguration.Level));
    }

    private void ConfigureSessionWithDefaults()
    {
        _session.Active = true;
        _session.Level = Level.Message;
        _session.Color = Color.Transparent;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (!_session.Active)
            return;

        Level level = LogLevelToLevel(logLevel);

        if (!_session.IsOn(level))
            return;

        string formatted = formatter(state, exception);
        switch (level)
        {
            case Level.Debug:
                _session.LogDebug(formatted);
                break;

            case Level.Verbose:
                _session.LogVerbose(formatted);
                break;

            case Level.Message:
                _session.LogMessage(formatted);
                break;

            case Level.Warning:
                _session.LogWarning(formatted);
                break;

            case Level.Error:
                if (exception != null)
                    _session.LogException(formatted, exception);
                else
                    _session.LogError(formatted);
                break;

            case Level.Fatal:
                _session.LogFatal(formatted);
                break;

            default:
                _session.LogMessage(formatted);
                break;
        }
    }

    private static Level LogLevelToLevel(LogLevel logLevel) => logLevel switch
    {
        LogLevel.Trace       => Level.Verbose,
        LogLevel.Debug       => Level.Debug,
        LogLevel.Information => Level.Message,
        LogLevel.Warning     => Level.Warning,
        LogLevel.Error       => Level.Error,
        LogLevel.Critical    => Level.Fatal,
        LogLevel.None        => Level.Debug,
        _                    => throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, null)
    };

    public bool IsEnabled(LogLevel logLevel) => throw new NotImplementedException();

    public IDisposable? BeginScope<TState>(TState state)
        where TState : notnull
    {
        if (!_session.IsOn(Level.Debug))
            return default!;

        var scopeName = state.ToString();
        _session.EnterMethod(Level.Debug, scopeName);
        return new DelegateDisposable(() => _session.LeaveMethod(Level.Debug, scopeName));
    }
}