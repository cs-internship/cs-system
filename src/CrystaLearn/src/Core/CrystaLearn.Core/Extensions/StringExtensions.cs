using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CrystaLearn.Core.Extensions;
public static class StringExtensions
{
    public static string Sha(this string text)
    {
        StringBuilder sb = new StringBuilder();

        using (SHA256 hash = SHA256.Create())
        {
            Encoding enc = Encoding.UTF8;
            var result = hash.ComputeHash(enc.GetBytes(text));

            foreach (var b in result)
                sb.Append(b.ToString("x2"));
        }

        return sb.ToString();
    }
}
