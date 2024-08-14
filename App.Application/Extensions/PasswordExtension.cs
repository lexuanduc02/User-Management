using System.Security.Cryptography;
using System.Text;

namespace App.Application.Extensions
{
    public class PasswordExtension
    {
        private const int keySize = 64;
        private const int iterations = 350000;
        public static string GenerateSalt()
        {
            byte[] saltBytes = RandomNumberGenerator.GetBytes(keySize);

            return Convert.ToBase64String(saltBytes);
        }

        public static string HashPassword(string password, string salt)
        {
            var saltBytes = Convert.FromBase64String(salt);
            HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;

            var hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                saltBytes,
                iterations,
                hashAlgorithm,
                keySize);
            return Convert.ToHexString(hash);
        }

        public static bool VerifyPassword(string password, string hashedPassword, string salt)
        {
            string newHashed = HashPassword(password, salt);
            return newHashed.Equals(hashedPassword);
        }
    }
}
