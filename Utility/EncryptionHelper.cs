using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    public static class EncryptionHelper
    {
        private static string SecretKey = "9TZTi6PdCcG0JurXNjH6ww==";
        private static string IvKey = "TZJbkD968uwJ2ZVEDmyPiw==";

        public static byte[] Encrypt(string value, byte[] key, byte[] iv)
        {
            byte[] encrypted;
            using (var rijAlg = Rijndael.Create())
            {
                rijAlg.Key = key;
                rijAlg.IV = iv;

                // Create a decrytor to perform the stream transform.
                var encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for encryption.
                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(value);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            return encrypted;
        }

        public static string HashPassword(string password, string saltToken)
        {
            var hmacSHA512 = new HMACSHA512(System.Text.Encoding.UTF8.GetBytes(saltToken));
            var saltedHash = hmacSHA512.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

            return Convert.ToBase64String(saltedHash);
        }
    }
}
