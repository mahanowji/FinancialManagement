using System.Security.Cryptography;

namespace Application.Utilities;

public static class SecurityHelper
{
    private const int SaltSize = 32;
    private const int KeySize = 64;
    private const int Iterations = 10000;

    public static string HashPassword(string password)
    {
        // Generate a random salt
        byte[] salt = new byte[SaltSize];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }

   
        using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations))
        {
            byte[] key = pbkdf2.GetBytes(KeySize);

            // Combine the salt and key into a single array
            byte[] hash = new byte[SaltSize + KeySize];
            Buffer.BlockCopy(salt, 0, hash, 0, SaltSize);
            Buffer.BlockCopy(key, 0, hash, SaltSize, KeySize);

            // Return the hashed password as a base64-encoded string
            return Convert.ToBase64String(hash);
        }
    }

    public static bool VerifyPassword(string password, string hashedPassword)
    {
        // Extract the salt and key from the hashed password
        byte[] hash = Convert.FromBase64String(hashedPassword);
        byte[] salt = new byte[SaltSize];
        Buffer.BlockCopy(hash, 0, salt, 0, SaltSize);
        byte[] key = new byte[KeySize];
        Buffer.BlockCopy(hash, SaltSize, key, 0, KeySize);

        // Derive a key from the password and salt using PBKDF2
        using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations))
        {
            byte[] testKey = pbkdf2.GetBytes(KeySize);

            // Compare the derived key with the stored key
            for (int i = 0; i < KeySize; i++)
            {
                if (key[i] != testKey[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}