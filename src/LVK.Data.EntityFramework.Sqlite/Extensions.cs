using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LVK.Data.EntityFramework.Sqlite;

public static class Extensions
{
    public static IHostApplicationBuilder AddSqliteDbContext<TContext>(this IHostApplicationBuilder builder, string? configurationName = null, string? connectionString = null)
        where TContext : DbContext
    {
        connectionString = builder.GetConnectionString(configurationName, connectionString);

        bool isDevelopment = builder.Environment.IsDevelopment();
        builder.Services.AddDbContextFactory<TContext>(configureDbContext);
        builder.Services.AddDbContext<TContext>(configureDbContext);

        return builder;

        void configureDbContext(DbContextOptionsBuilder options)
        {
            options = options.UseSqlite(connectionString);
            options.ConfigureDefaults(isDevelopment);
        }
    }
}