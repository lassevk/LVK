using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace LVK.Data.EntityFramework;

public static class HostApplicationBuilderExtensions
{
    public static string GetConnectionString(this IHostApplicationBuilder builder, string? configurationName, string? connectionString)
    {
        if (connectionString is null)
        {
            configurationName ??= "Default";
            connectionString = builder.Configuration.GetConnectionString(configurationName);
            if (connectionString is null)
            {
                throw new InvalidOperationException($"No connection string defined with the name '{configurationName}'");
            }
        }

        return connectionString;
    }
}