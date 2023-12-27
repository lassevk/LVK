using System.Security.Cryptography;

using LVK.Data.Encoding;
using LVK.Data.Protection;

namespace LVK.Data.Encryption;

public static class DataEncryption
{
    private const int _saltSize = 32; // 256 bits of salt

    public static byte[] Protect(byte[] unprotectedData, string password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(_saltSize);

        try
        {
            using Aes algorithm = CreateAlgorithm(password, salt);
            using var targetStream = new MemoryStream();
            using ICryptoTransform encryptor = algorithm.CreateEncryptor();
            using var encryptionStream = new CryptoStream(targetStream, encryptor, CryptoStreamMode.Write);

            DataEncoder.WriteInt32(targetStream, salt.Length);
            targetStream.Write(salt, 0, salt.Length);

            encryptionStream.Write(unprotectedData, 0, unprotectedData.Length);
            encryptionStream.FlushFinalBlock();

            return targetStream.ToArray();
        }
        catch (CryptographicException ex)
        {
            throw new DataProtectionException($"Unable to protect data: {ex.Message}", ex);
        }
    }

    public static byte[] Unprotect(byte[] protectedData, string password)
    {
        try
        {
            using var sourceStream = new MemoryStream(protectedData);
            int saltLength = DataEncoder.ReadInt32(sourceStream);
            if (saltLength < 8 || saltLength > 1024)
                throw new InvalidOperationException($"Invalid salt length, must be in the range 8..1024, but was {saltLength}");

            var salt = new byte[saltLength];
            sourceStream.ReadExactly(salt, 0, salt.Length);

            using Aes algorithm = CreateAlgorithm(password, salt);
            using ICryptoTransform decryptor = algorithm.CreateDecryptor();
            using var decryptionStream = new CryptoStream(sourceStream, decryptor, CryptoStreamMode.Read);
            using var targetStream = new MemoryStream();

            decryptionStream.CopyTo(targetStream);
            return targetStream.ToArray();
        }
        catch (ArgumentException ex)
        {
            throw new DataProtectionException($"Unable to unprotect data: {ex.Message}", ex);
        }
        catch (OverflowException ex)
        {
            throw new DataProtectionException($"Unable to unprotect data: {ex.Message}", ex);
        }
        catch (InvalidOperationException ex)
        {
            throw new DataProtectionException($"Unable to unprotect data: {ex.Message}", ex);
        }
        catch (CryptographicException ex)
        {
            throw new DataProtectionException($"Unable to unprotect data: {ex.Message}", ex);
        }
    }

    private static Aes CreateAlgorithm(string password, byte[] salt)
    {
        var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, salt, 4, HashAlgorithmName.SHA1);

        var algorithm = Aes.Create();
        algorithm.BlockSize = 128;
        algorithm.Mode = CipherMode.CBC;
        algorithm.Padding = PaddingMode.PKCS7;
        algorithm.Key = rfc2898DeriveBytes.GetBytes(algorithm.KeySize / 8);
        algorithm.IV = rfc2898DeriveBytes.GetBytes(algorithm.BlockSize / 8);

        return algorithm;
    }
}