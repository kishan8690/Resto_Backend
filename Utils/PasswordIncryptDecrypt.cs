using System.Text;

namespace Resto_Backend.Utils
{
    public static class PasswordIncryptDecrypt
    {

        public static string ConvertToEncrypt(string password)
        {
            if (string.IsNullOrEmpty(password)) return "";
            string passwordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(password, workFactor: 13);
            return passwordHash;
        }
        public static bool ConvertToDecrypt(string password, string hashPassword)
        {
            if (string.IsNullOrEmpty(hashPassword) && string.IsNullOrEmpty(password)) return false;
            return BCrypt.Net.BCrypt.EnhancedVerify(password, hashPassword);

        }
    }
}

