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