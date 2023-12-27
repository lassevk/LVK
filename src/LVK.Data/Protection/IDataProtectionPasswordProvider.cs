namespace LVK.Data.Protection;

public interface IDataProtectionPasswordProvider
{
    string? TryGetPassword(string passwordName);
}