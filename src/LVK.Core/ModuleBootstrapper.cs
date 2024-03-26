using LVK.Core.Bootstrapping;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Hosting;

namespace LVK.Core;

public class ModuleBootstrapper : IModuleBootstrapper
{
    public void Bootstrap(IHostBootstrapper bootstrapper, IHostApplicationBuilder builder)
    {
        string? assemblyLocation = Path.GetDirectoryName(GetType().Assembly.Location);
        if (assemblyLocation == null)
            return;

        for (int index = builder.Configuration.Sources.Count - 1; index >= 0; index--)
        {
            var jcs = builder.Configuration.Sources[index] as JsonConfigurationSource;
            if (jcs is null)
                continue;

            if (!jcs.Path?.StartsWith("appsettings.", StringComparison.InvariantCultureIgnoreCase) ?? false)
                continue;
            if (!jcs.Path?.EndsWith(".json", StringComparison.InvariantCultureIgnoreCase) ?? false)
                continue;

            builder.Configuration.Sources.RemoveAt(index);
        }

        string[] configurationFiles =
        [
            "appsettings.json", $"appsettings.{builder.Environment.EnvironmentName}.json", $"appsettings.{builder.Environment.EnvironmentName}.json",
            $"appsettings.{Environment.MachineName.ToLowerInvariant()}.json", $"appsettings.{Environment.MachineName.ToLowerInvariant()}.{builder.Environment.EnvironmentName}.json"
        ];

        IConfigurationBuilder b = builder.Configuration.SetBasePath(assemblyLocation).AddJsonFile("appsettings.json");

        foreach (string configurationFile in configurationFiles)
            b = b.AddJsonFile(configurationFile, optional: true);
    }
}