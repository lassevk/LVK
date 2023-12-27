using Microsoft.Extensions.Configuration;

namespace LVK.Data.Protection.PasswordProviders;

public class ConfigurationDataProtectionPasswordProvider : IDataProtectionPasswordProvider
{
    private readonly IConfigurationSection _configuration;

    public ConfigurationDataProtectionPasswordProvider(IConfiguration configuration)
    {
        _configuration = configuration.GetSection("DataProtection:Passwords");
    }

    public string? TryGetPassword(string passwordName) => _configuration[passwordName];
}