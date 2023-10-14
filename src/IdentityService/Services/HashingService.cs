using System.Security.Cryptography;
using System.Text;

namespace IdentityService.Services
{
    public interface IHashingService
    {
        (string Salt, string Hash) GenerateSaltAndHash(string password);
        string HashPassword(string password, string salt);
    }

    public class HashingService : IHashingService
    {
        public (string Salt, string Hash) GenerateSaltAndHash(string password)
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] saltBytes = new byte[32];
                rng.GetBytes(saltBytes);
                string salt = Convert.ToBase64String(saltBytes);

                string hashedPassword = HashPassword(password, salt);

                return (salt, hashedPassword);
            }
        }

        public string HashPassword(string password, string salt)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] saltBytes = Convert.FromBase64String(salt);
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] combinedBytes = new byte[saltBytes.Length + passwordBytes.Length];

                Buffer.BlockCopy(saltBytes, 0, combinedBytes, 0, saltBytes.Length);
                Buffer.BlockCopy(passwordBytes, 0, combinedBytes, saltBytes.Length, passwordBytes.Length);

                byte[] hashBytes = sha256.ComputeHash(combinedBytes);
                return Convert.ToBase64String(hashBytes);
            }
        }
    }
}
