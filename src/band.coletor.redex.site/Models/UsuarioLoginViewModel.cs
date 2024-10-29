using Band.Coletor.Redex.Business.Enums;
using System.ComponentModel.DataAnnotations;

namespace Band.Coletor.Redex.Site.Models
{
    public class UsuarioLoginViewModel
    {
        public UsuarioLoginViewModel()
        {
        }

        [Display(Name = "Local Pátio")]
        public LocalPatio LocalPatio { get; set; }

        [Display(Name = "Login")]
        public string Login { get; set; }

        [Display(Name = "Senha")]
        public string Senha { get; set; }

        [Display(Name = "Nome")]
        public string Nome { get; set; }

        public string Ativo { get; set; }

        public bool Lembrar { get; set; }

        public string ReturnUrl { get; set; }
    }
}