using FluentValidation;

namespace Band.Coletor.Redex.Business.Models
{
    public class UsuarioLogin : Entidade<UsuarioLogin>
    {
        public UsuarioLogin()
        {
        }

        public UsuarioLogin(string login, string senha, string nome)
        {
            Login = login;
            Senha = senha;
            Nome = nome;
        }

        public string Login { get; set; }

        public string Senha { get; set; }

        public string Nome { get; set; }

        public int PatioColetorId { get; set; }

        public bool Ativo { get; set; }

        public override void Validar()
        {
            RuleFor(c => c.Login)
               .NotNull()
               .WithMessage("Login não informado");

            RuleFor(c => c.Senha)
              .NotNull()
              .WithMessage("Senha não informada");
        }
    }
}