using FluentValidation;

namespace Band.Coletor.Redex.Business.Models
{
    public class Usuario : Entidade<Usuario>
    {
        public Usuario()
        {
        }

        public Usuario(string login, string loginWorkflow, string nome, string email, int cargoId, bool administrador, bool ativo)
        {
            Login = login;
            LoginWorkflow = loginWorkflow;
            Nome = nome;
            Email = email;
            CargoId = cargoId;
            Administrador = administrador;
            Ativo = ativo;
        }

        public string Login { get; set; }

        public string LoginWorkflow { get; set; }

        public string Nome { get; set; }

        public string Email { get; set; }

        public int CargoId { get; set; }

        public bool Administrador { get; set; }

        public bool Ativo { get; set; }

        public override void Validar()
        {
            RuleFor(c => c.Login)
                .NotEmpty()
                .WithMessage("O Login do usuário é obrigatório");

            RuleFor(c => c.Login)
                .NotEmpty()
                .WithMessage("O Nome do usuário é obrigatório");

            RuleFor(c => c.CargoId)
                .GreaterThan(0)
                .WithMessage("O Cargo do usuário é obrigatório");

            ValidationResult = Validate(this);
        }
    }
}