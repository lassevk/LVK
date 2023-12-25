namespace LVK.Data;

public interface IDataProtectionPasswordProvider
{
    string? TryGetPassword(string passwordName);
}