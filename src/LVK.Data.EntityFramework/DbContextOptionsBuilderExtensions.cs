using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LVK.Data.EntityFramework;

public static class Extensions
{
    public static void ConfigureDefaults(this DbContextOptionsBuilder optionsBuilder, bool isDevelopment)
    {
        if (!isDevelopment)
            return;

        optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information).EnableSensitiveDataLogging().EnableDetailedErrors();
    }
}