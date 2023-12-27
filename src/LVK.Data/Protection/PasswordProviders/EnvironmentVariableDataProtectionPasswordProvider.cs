using System.Text;

using LVK.Core.Caching;

namespace LVK.Data.Protection.PasswordProviders;

public class EnvironmentVariableDataProtectionPasswordProvider : IDataProtectionPasswordProvider
{
    private readonly WeakCache<string, string> _variableNameCache = new();

    public string? TryGetPassword(string passwordName)
    {
        string variableName = _variableNameCache.GetOrAddValue(passwordName, CalculateVariableName)!;
        string? value = Environment.GetEnvironmentVariable(variableName);
        return string.IsNullOrWhiteSpace(value) ? null : value;
    }

    private string CalculateVariableName(string passwordName)
    {
        var sb = new StringBuilder("DATA_PROTECTION_SCOPE_");

        foreach (char c in passwordName.Where(c => c is >= 'A' and <= 'Z' or >= 'a' and <= 'z' or >= '0' and <= '9'))
            sb.Append(c);

        return sb.ToString();
    }
}