using LVK.Data.Encryption;
using LVK.Data.Protection.PasswordProviders;

namespace LVK.Data.Protection;

public class DataProtection : IDataProtection
{
    private readonly List<IDataProtectionPasswordProvider> _dataProtectionPasswordProviders;

    public static IDataProtection CreateDefault() => new DataProtection([new EnvironmentVariableDataProtectionPasswordProvider()]);

    public DataProtection(IEnumerable<IDataProtectionPasswordProvider> dataProtectionPasswordProviders)
    {
        if (dataProtectionPasswordProviders == null)
            throw new ArgumentNullException(nameof(dataProtectionPasswordProviders));

        _dataProtectionPasswordProviders = dataProtectionPasswordProviders.ToList();
    }

    public byte[] Protect(byte[] unprotectedData, string passwordName)
    {
        string? password = TryGetPassword(passwordName);
        if (password == null)
            throw new InvalidOperationException($"No data protection scope defined for '{passwordName}'");

        return DataEncryption.Protect(unprotectedData, password);
    }

    public byte[] Unprotect(byte[] protectedData, string passwordName)
    {
        string? password = TryGetPassword(passwordName);
        if (password == null)
            throw new InvalidOperationException($"No data protection scope defined for '{passwordName}'");

        return DataEncryption.Unprotect(protectedData, password);
    }

    private string? TryGetPassword(string passwordName)
        => _dataProtectionPasswordProviders
           .Select(provider => provider.TryGetPassword(passwordName))
           .FirstOrDefault(password => !string.IsNullOrWhiteSpace(password));
}