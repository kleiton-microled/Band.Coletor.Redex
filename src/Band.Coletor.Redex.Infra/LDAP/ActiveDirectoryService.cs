using Band.Coletor.Redex.Business.Models;
using System.Collections.Generic;
using System.DirectoryServices;

namespace Band.Coletor.Redex.Infra.LDAP
{
    public class ActiveDirectoryService
    {
        private readonly string _servidorLDAP;
        private readonly string _usuario;
        private readonly string _senha;

        public ActiveDirectoryService(string servidorLDAP, string usuario, string senha)
        {
            _servidorLDAP = servidorLDAP;
            _usuario = usuario;
            _senha = senha;
        }

        public DirectorySearcher DirectorySearcher()
        {
            using (var root = new DirectoryEntry(_servidorLDAP, _usuario, _senha))
            {
                return new DirectorySearcher(root, "(&(objectClass=user)(objectCategory=person))");
            }
        }

        public IEnumerable<Usuario> ObterUsuarios()
        {
            var usuarios = new List<Usuario>();

            foreach (SearchResult usuario in DirectorySearcher().FindAll())
            {
                var usuarioAD = new Usuario
                {
                    Login = usuario.Properties["sAMAccountName"][0].ToString(),
                    Nome = usuario.Properties["cn"][0].ToString(),
                    Email = string.Empty
                };

                if (usuario.Properties.Contains("mail"))
                {
                    usuarioAD.Email = usuario.Properties["mail"][0].ToString();
                }

                usuarios.Add(usuarioAD);
            }

            return usuarios;
        }
    }
}