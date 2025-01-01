using System.Security.Cryptography;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace Resto_Backend.Utils
{
    public static class PasswordIncryptDecrypt
    {
        private static readonly string EncryptionKey = "pjsGLNYrMqU6wny4";

        public static string ConvertToEncrypt(string text)
        {
             
            using (var aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(EncryptionKey);
                aesAlg.IV = new byte[16]; // Initialize the IV to 0s

                var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    using (var swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(text);
                    }

                    return Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
        }
        public static string ConvertToDecrypt(string encryptedText)
        {
            if (string.IsNullOrEmpty(encryptedText))
                throw new ArgumentException("The encrypted text cannot be null or empty.", nameof(encryptedText));

            try
            {
                // Validate if the string is a potential Base64 string
                if (!IsBase64String(encryptedText))
                    throw new FormatException("The input string is not a valid Base64-encoded string.");

                using (var aesAlg = Aes.Create())
                {
                    aesAlg.Key = Encoding.UTF8.GetBytes(EncryptionKey);
                    aesAlg.IV = new byte[16]; // Initialize the IV to 0s

                    var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                    using (var msDecrypt = new MemoryStream(Convert.FromBase64String(encryptedText)))
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    using (var srDecrypt = new StreamReader(csDecrypt))
                    {
                        return srDecrypt.ReadToEnd();
                    }
                }
            }
            catch (FormatException ex)
            {
                throw new InvalidOperationException("The encrypted text is not in a valid Base64 format. Please verify the input.", ex);
            }
            catch (CryptographicException ex)
            {
                throw new InvalidOperationException("An error occurred during decryption. Ensure the encryption key and IV are correct.", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An unexpected error occurred during decryption.", ex);
            }
        }

        // Helper method to validate Base64 strings
        private static bool IsBase64String(string base64)
        {
            Span<byte> buffer = new Span<byte>(new byte[base64.Length]);
            return Convert.TryFromBase64String(base64, buffer, out _);
        }

    }
}

