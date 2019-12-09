using System;
using System.Security.Cryptography;
using System.Text;

namespace Suites.Utils.System
{
    public static class HashExtensions
    {
        public static string ToMd5Hash(this string source)
        {
            StringNullOrEmptyException.Assert(source, nameof(source));

            // Create a new instance of the MD5CryptoServiceProvider object.
            using var hasher = new MD5CryptoServiceProvider();

            // Convert the input string to a byte array and compute the hash.
            var data = hasher.ComputeHash(Encoding.UTF8.GetBytes(source));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            var sb = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (var i = 0; i < data.Length; i++)
            {
                sb.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sb.ToString();
        }
    }
}
