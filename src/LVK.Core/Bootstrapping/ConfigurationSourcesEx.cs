using Linqy;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using Microsoft.Extensions.Configuration.Json;

namespace LVK.Core.Bootstrapping;

public static class ConfigurationSourcesEx
{
    public static void AdjustConfigurationSources(IConfigurationManager configuration, string environmentName)
    {
        var jsonSources = configuration.Sources.OfType<JsonConfigurationSource>().ToList();
        foreach (JsonConfigurationSource source in jsonSources)
            configuration.Sources.Remove(source);

        EnvironmentVariablesConfigurationSource? finalEnvironmentVariableSource
            = configuration.Sources.OfType<EnvironmentVariablesConfigurationSource>().FirstOrDefault(source => source.Prefix is null);
        if (finalEnvironmentVariableSource != null)
            configuration.Sources.Remove(finalEnvironmentVariableSource);

        string hostname = Environment.MachineName.ToLowerInvariant();
        environmentName = environmentName.ToLowerInvariant();

        configuration.SetBasePath(AppContext.BaseDirectory);
        configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
        configuration.AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: true);
        configuration.AddJsonFile($"appsettings.{hostname}.json", optional: true, reloadOnChange: true);
        configuration.AddJsonFile($"appsettings.{hostname}.{environmentName}.json", optional: true, reloadOnChange: true);

        if (finalEnvironmentVariableSource != null)
            configuration.Sources.Add(finalEnvironmentVariableSource);
    }
}