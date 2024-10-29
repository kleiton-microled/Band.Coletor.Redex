using Band.Coletor.Redex.Business.Enums;
using System;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;

namespace Band.Coletor.Redex.Site.Extensions
{
    public static class UserExtensions
    {
        public static int ObterId(this IPrincipal user)
        {
            var claim = ((ClaimsIdentity)user.Identity).FindFirst("Id");

            return claim == null
                ? 0
                : claim.Value.ToInt();
        }

        public static int ObterPatioColetorId(this IPrincipal user)
        {
            var claim = ((ClaimsIdentity)user.Identity).FindFirst("PatioId");

            return claim == null
                ? 0
                : claim.Value.ToInt();
        }

        public static string ObterNome(this IPrincipal user)
        {
            var claim = ((ClaimsIdentity)user.Identity).FindFirst("Nome");

            return claim?.Value;
        }

        public static LocalPatio ObterLocalPatio(this IPrincipal user)
        {
            var claim = ((ClaimsIdentity)user.Identity).FindFirst("LocalPatio");

            return (LocalPatio)claim.Value.ToInt();
        }

        public static string ObterIdEncriptado(this IPrincipal user)
        {
            var claim = ((ClaimsIdentity)user.Identity).FindFirst("Id");
            var id = (claim == null ? 0 : claim.Value.ToInt()).ToString();

            byte[] data = UTF8Encoding.UTF8.GetBytes(id);
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes("MICROLED"));
                using (TripleDESCryptoServiceProvider tripDes = new TripleDESCryptoServiceProvider() { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                {
                    ICryptoTransform transform = tripDes.CreateEncryptor();
                    byte[] results = transform.TransformFinalBlock(data, 0, data.Length);
                    return Convert.ToBase64String(results, 0, results.Length);
                }
            }



            //byte[] Results;
            //System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();
            //MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
            //byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(senha));
            //TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();
            //TDESAlgorithm.Key = TDESKey;
            //TDESAlgorithm.Mode = CipherMode.ECB;
            //TDESAlgorithm.Padding = PaddingMode.PKCS7;
            //byte[] DataToEncrypt = UTF8.GetBytes(Message);
            //try
            //{
            //    ICryptoTransform Encryptor = TDESAlgorithm.CreateEncryptor();
            //    Results = Encryptor.TransformFinalBlock(DataToEncrypt, 0, DataToEncrypt.Length);
            //}
            //finally
            //{
            //    TDESAlgorithm.Clear();
            //    HashProvider.Clear();
            //}
            //return Convert.ToBase64String(Results);


        }

    }
}