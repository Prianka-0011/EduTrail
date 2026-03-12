using System.Security.Cryptography;
using System.Text;
namespace EduTrail.Application.Shared
{
    public static class PasswordHasher
    {
        public static bool VerifyPassword(string password, string hash, string salt)
        {
            using var hmac = new HMACSHA512(Convert.FromBase64String(salt));
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(computedHash) == hash;
        }
    }
}
