using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LVK.Data.EntityFramework.Postgres;

public static class Extensions
{
    public static IHostApplicationBuilder AddPostgresSqlDbContext<TContext>(this IHostApplicationBuilder builder, string? configurationName = null, string? connectionString = null)
        where TContext : DbContext
    {
        connectionString = builder.GetConnectionString(configurationName, connectionString);

        bool isDevelopment = builder.Environment.IsDevelopment();
        builder.Services.AddDbContextFactory<TContext>(configureDbContext);
        builder.Services.AddDbContext<TContext>(configureDbContext);

        return builder;

        void configureDbContext(DbContextOptionsBuilder options)
        {
            options = options.UseNpgsql(connectionString);
            options.ConfigureDefaults(isDevelopment);
        }
    }
}