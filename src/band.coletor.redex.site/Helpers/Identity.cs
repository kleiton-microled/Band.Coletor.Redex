using Band.Coletor.Redex.Business.DTO;
using Band.Coletor.Redex.Business.Models;
using Band.Coletor.Redex.Site.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;

namespace Band.Coletor.Redex.Site.Helpers
{
    public class Identity
    {
        public static void Autenticar()
        {
            var authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];

            if (authCookie != null)
            {
                var ticket = FormsAuthentication.Decrypt(authCookie.Value);

                FormsIdentity formsIdentity = new FormsIdentity(ticket);

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(formsIdentity);

                var usuario = JsonConvert.DeserializeObject<UsuarioAutenticado>(ticket.UserData);

                if (usuario != null)
                {
                    claimsIdentity.AddClaims(ObterClaims(usuario));
                }

                ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                HttpContext.Current.User = claimsPrincipal;
            }
        }

        public static List<Claim> ObterClaims(UsuarioAutenticado usuario)
        {
            var claims = new List<Claim>
            {
                new Claim("Id", usuario.Id.ToString()),
                new Claim("Nome", usuario.Nome),
                new Claim("Login", usuario.Login),
                new Claim("Ativo", usuario.Ativo.ToString()),
                new Claim("PatioId", usuario.PatioColetorId.ToString()),
                new Claim("LocalPatio", usuario.LocalPatio)
            };

            return claims;
        }
    }
}
