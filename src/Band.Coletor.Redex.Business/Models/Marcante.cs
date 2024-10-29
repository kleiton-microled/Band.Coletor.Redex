using FluentValidation;

namespace Band.Coletor.Redex.Business.Models
{
    public class Marcante : Entidade<Marcante>
    {
        public Marcante()
        {
        }

        public Marcante(
            int talieId,
            int quantidade,
            int volumes,
            string quadra,
            int quantidadeAssociada,
            int quantidadeDescarregada,
            bool registro,
            string codigoMarcante,
            int armazemId)
        {
            TalieId = talieId;
            Quantidade = quantidade;
            Volumes = volumes;
            Quadra = quadra;
            QuantidadeAssociada = quantidadeAssociada;
            QuantidadeDescarregada = quantidadeDescarregada;
            Registro = registro;
            CodigoMarcante = codigoMarcante;
            ArmazemId = armazemId;
        }

        public int TalieId { get; set; }

        public int BookingId { get; set; }

        public int RegistroCsId { get; set; }

        public bool Registro { get; set; }

        public int PatioId { get; set; }

        public string CodigoMarcante { get; set; }

        public int Quantidade { get; set; }

        public int Volumes { get; set; }

        public int QuantidadeDescarregada { get; set; }

        public int QuantidadeAssociada { get; set; }

        public int ArmazemId { get; set; }

        public string Armazem { get; set; }

        public string Quadra { get; set; }

        public string Rua { get; set; }

        public string Fiada { get; set; }

        public override void Validar()
        {
            RuleFor(c => c.CodigoMarcante)
                .NotNull()
                .WithMessage("Marcante não informado");

            RuleFor(c => c.Quantidade)
                .GreaterThan(0)
                .WithMessage("Quantidade não informada");

            RuleFor(c => c.TalieId)
                .GreaterThan(0)
                .WithMessage("Talie não informado");

            RuleFor(c => c.Quantidade)
               .GreaterThan(0)
               .WithMessage("Volume não informado");

            RuleFor(c => c.Volumes)
                .Must((c, r) =>
                {
                    return ValidarMarcanteAssociado(c.Volumes, c.Registro);
                })
                .WithMessage("Marcante já associado");

            RuleFor(c => c.Quantidade)
                .Must((c, r) =>
                {
                    return ValidarDivergenciaQuantidades(c.QuantidadeAssociada, c.Quantidade, c.QuantidadeDescarregada);
                })
                .WithMessage("Divergência de quantidades");

            ValidationResult = Validate(this);
        }

        private static bool ValidarMarcanteAssociado(int volumes, bool registro)
        {
            return !(volumes > 0 && registro == false);
        }

        private static bool ValidarDivergenciaQuantidades(int quantidadeAssociada, int quantidade, int quantidadeDescarregada)
        {
            return !((quantidadeAssociada + quantidade) > quantidadeDescarregada);
        }
    }
}