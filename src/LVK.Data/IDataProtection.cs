namespace LVK.Data;

public interface IDataProtection
{
    byte[] Protect(byte[] unprotectedData, string passwordName);

    byte[] Unprotect(byte[] protectedData, string passwordName);
}