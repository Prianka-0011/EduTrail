using System.Security.Cryptography;
using System.Text;

namespace EduTrail.Application.Shared
{
    public static class PasswordHasher
    {
         public static void CreatePasswordHash(string password, out string hash, out string salt)
        {
            using var hmac = new HMACSHA512();

            salt = Convert.ToBase64String(hmac.Key);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            hash = Convert.ToBase64String(computedHash);
        }        public static bool VerifyPassword(string password, string hash, string salt)
        {
            using var hmac = new HMACSHA512(Convert.FromBase64String(salt));
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(computedHash) == hash;
        }
    }
}
