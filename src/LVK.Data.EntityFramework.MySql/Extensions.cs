using System.Data.Common;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LVK.Data.EntityFramework.MySql;

public static class Extensions
{
    public static IHostApplicationBuilder AddMySqlDbContext<TContext>(this IHostApplicationBuilder builder, string? configurationName = null, string? connectionString = null)
        where TContext : DbContext
    {
        connectionString = builder.GetConnectionString(configurationName, connectionString);

        var connectionStringBuilder = new DbConnectionStringBuilder
        {
            ConnectionString = connectionString,
        };

        var version = ServerVersion.Parse((string?)connectionStringBuilder["version"] ?? "10.11.6-MariaDB");
        connectionStringBuilder.Remove("version");

        connectionString = connectionStringBuilder.ConnectionString;

        bool isDevelopment = builder.Environment.IsDevelopment();
        builder.Services.AddDbContextFactory<TContext>(configureDbContext);
        builder.Services.AddDbContext<TContext>(configureDbContext);

        return builder;

        void configureDbContext(DbContextOptionsBuilder options)
        {
            options = options.UseMySql(connectionString, version);
            options.ConfigureDefaults(isDevelopment);
        }
    }
}