using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SimpleFlashCards.Helpers
{
    public class Secure
    {
        private static string Salt = new AppSettings().GetSetting("Hash:Salt");
        public string Generate(string value, string salt)
        {
            StringBuilder sb = new StringBuilder();
            using (SHA256 hash = SHA256.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] valueEncode = hash.ComputeHash(enc.GetBytes(value));
                byte[] saltEncode = hash.ComputeHash(enc.GetBytes(salt));
                byte[] allBytes = valueEncode.Concat(saltEncode).ToArray();

                foreach (byte b in allBytes)
                    sb.Append(b.ToString("x2"));
            }
            string result = sb.ToString();
            return result;
        }
        public string GenerateSHA256(string value)
        {
            StringBuilder sb = new StringBuilder();
            using (SHA256 hash = SHA256.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] valueEncode = hash.ComputeHash(enc.GetBytes(value));

                foreach (byte b in valueEncode)
                    sb.Append(b.ToString("x2"));
            }
            string result = sb.ToString();
            return result;
        }

        public string Encrypt(string text)
        {
            byte[] clearBytes = Encoding.Unicode.GetBytes(text);
            byte[] saltBytes = new byte[]
            {
                0x49, 0x76, 0x61, 0x6e,
                0x20, 0x4d, 0x65, 0x64,
                0x76, 0x65, 0x64, 0x65,
                0x76
            };
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(Salt, saltBytes);
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(),
                        CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    text = Convert.ToBase64String(ms.ToArray());
                }
            }
            return text;
        }

        public string Decrypt(string text)
        {
            text = text.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(text);
            byte[] saltBytes = new byte[]
            {
                0x49, 0x76, 0x61, 0x6e,
                0x20, 0x4d, 0x65, 0x64,
                0x76, 0x65, 0x64, 0x65,
                0x76
            };
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(Salt, saltBytes);
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(),
                        CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    text = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return text;
        }
    }
}
