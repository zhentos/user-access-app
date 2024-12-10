using System.Security.Cryptography;

namespace UserAccessApp.Infrastructure
{
    public class PasswordHasher : IPasswordHasher
    {
        private const int SaltSize = 16; // Size of salt in bytes
        private const int HashSize = 20; // Size of hash in bytes
        private const int Iterations = 100000; // Recommended number of iterations

        public (string hashedPassword, string salt) HashPassword(string password)
        {
            // Generate a salt
            var salt = new byte[SaltSize];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            // Hash the password with the salt using PBKDF2
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256))
            {
                var hash = pbkdf2.GetBytes(HashSize);
                return (Convert.ToBase64String(hash), Convert.ToBase64String(salt));
            }
        }

        public bool VerifyPassword(string enteredPassword, string storedHash, string storedSalt)
        {
            var salt = Convert.FromBase64String(storedSalt);
            using (var pbkdf2 = new Rfc2898DeriveBytes(enteredPassword, salt, Iterations, HashAlgorithmName.SHA256))
            {
                var hash = pbkdf2.GetBytes(HashSize);
                return Convert.ToBase64String(hash) == storedHash;
            }
        }
    }
}
