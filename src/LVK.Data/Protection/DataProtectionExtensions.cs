using System.Collections;
using System.Reflection;

namespace LVK.Data.Protection;

public static class DataProtectionExtensions
{
    public const string DefaultPasswordName = "Default";

    public static byte[] Protect(this IDataProtection dataProtection, byte[] unprotectedData)
    {
        if (dataProtection == null)
            throw new ArgumentNullException(nameof(dataProtection));

        return dataProtection.Protect(unprotectedData, DefaultPasswordName);
    }

    public static byte[] Unprotect(this IDataProtection dataProtection, byte[] protectedData)
    {
        if (dataProtection == null)
            throw new ArgumentNullException(nameof(dataProtection));

        return dataProtection.Unprotect(protectedData, DefaultPasswordName);
    }

    public static string Protect(this IDataProtection dataProtection, string unprotectedString, string passwordName)
    {
        if (dataProtection == null)
            throw new ArgumentNullException(nameof(dataProtection));

        if (unprotectedString == null)
            throw new ArgumentNullException(nameof(unprotectedString));

        var unprotectedBytes = System.Text.Encoding.UTF8.GetBytes(unprotectedString);
        var protectedBytes = dataProtection.Protect(unprotectedBytes, passwordName);
        return Convert.ToBase64String(protectedBytes);
    }

    public static string Unprotect(this IDataProtection dataProtection, string protectedString, string passwordName)
    {
        if (dataProtection == null)
            throw new ArgumentNullException(nameof(dataProtection));

        if (protectedString == null)
            throw new ArgumentNullException(nameof(protectedString));

        byte[] protectedBytes;
        try
        {
            protectedBytes = Convert.FromBase64String(protectedString);
        }
        catch (FormatException ex)
        {
            throw new DataProtectionException($"Unable to unprotect data: {ex.Message}", ex);
        }

        var unprotectedBytes = dataProtection.Unprotect(protectedBytes, passwordName);
        return System.Text.Encoding.UTF8.GetString(unprotectedBytes);
    }

    public static string Protect(this IDataProtection dataProtection, string unprotectedString)
    {
        if (dataProtection == null)
            throw new ArgumentNullException(nameof(dataProtection));

        if (unprotectedString == null)
            throw new ArgumentNullException(nameof(unprotectedString));

        return Protect(dataProtection, unprotectedString, DefaultPasswordName);
    }

    public static string TryUnprotect(this IDataProtection dataProtection, string protectedString, string passwordName = DefaultPasswordName)
    {
        try
        {
            return dataProtection.Unprotect(protectedString, passwordName);
        }
        catch (DataProtectionException)
        {
            return protectedString;
        }
        catch (EndOfStreamException)
        {
            return protectedString;
        }
    }

    public static string Unprotect(this IDataProtection dataProtection, string protectedString)
    {
        if (dataProtection == null)
            throw new ArgumentNullException(nameof(dataProtection));

        if (protectedString == null)
            throw new ArgumentNullException(nameof(protectedString));

        return Unprotect(dataProtection, protectedString, DefaultPasswordName);
    }

    public static void TryUnprotectObject(this IDataProtection dataProtection, object? instance, string passwordName = DefaultPasswordName)
        => TryUnprotectObject(dataProtection, instance, passwordName, new HashSet<object>());

    private static void TryUnprotectObject(IDataProtection dataProtection, object? instance, string passwordName, HashSet<object> done)
    {
        if (instance is null)
            return;

        if (!done.Add(instance))
            return;

        TryUnprotectProperties(dataProtection, instance, passwordName, done);
        if (instance is not IEnumerable ie)
            return;

        foreach (var child in ie)
            TryUnprotectObject(dataProtection, child, passwordName, done);
    }

    private static void TryUnprotectProperties(IDataProtection dataProtection, object instance, string passwordName, HashSet<object> done)
    {
        PropertyInfo[] properties = instance.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
        foreach (PropertyInfo property in properties)
            TryUnprotectProperty(dataProtection, instance, passwordName, done, property);
    }

    private static void TryUnprotectProperty(IDataProtection dataProtection, object instance, string passwordName, HashSet<object> done, PropertyInfo property)
    {
        if (property is not { CanRead: true, CanWrite: true })
            return;

        if (property.GetIndexParameters().Length > 0)
            return;

        var value = property.GetValue(instance);

        if (value is null)
            return;

        if (value is string s)
        {
            s = TryUnprotect(dataProtection, s, passwordName);
            property.SetValue(instance, s);

            return;
        }

        if (value.GetType().IsClass)
            TryUnprotectObject(dataProtection, value, passwordName, done);
    }
}