﻿using System.Text;

namespace Band.Coletor.Redex.Infra.Secure
{
    public static class Criptografia
    {
        public static string Encriptar(string texto)
        {
            if (string.IsNullOrEmpty(texto)) return "";
            var password = (texto += "|2d331cca-f6c0-40c0-bb43-6e32989c2881");
            var md5 = System.Security.Cryptography.MD5.Create();
            var data = md5.ComputeHash(Encoding.Default.GetBytes(password));
            var sbString = new StringBuilder();
            foreach (var t in data)
                sbString.Append(t.ToString("x2"));

            return sbString.ToString();
        }
    }
}