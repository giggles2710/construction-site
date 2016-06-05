using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace _170516.Utility
{
    public static class EncryptionHelper
    {
        public static string HashPassword(string password, string saltToken)
        {
            var hmacSHA1 = new HMACSHA1(System.Text.Encoding.UTF8.GetBytes(saltToken));
            var saltedHash = hmacSHA1.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

            return Convert.ToBase64String(saltedHash);
        }
    }
}