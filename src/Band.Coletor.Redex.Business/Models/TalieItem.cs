using FluentValidation;

namespace Band.Coletor.Redex.Business.Models
{
    public class TalieItem : Entidade<TalieItem>
    {
        public TalieItem()
        {
        }

        public TalieItem(
            int talieId,
            int registroCsId,
            decimal? quantidade,
            int patioId,
            decimal? comprimento,
            decimal? largura,
            decimal? altura,
            decimal peso,
            string remonte,
            string fumigacao,
            bool fragil,
            bool madeira,
            bool avariado,
            string yard,
            int notaFiscalId,
            string notaFiscal,
            string iMO1,
            string iMO2,
            string iMO3,
            string iMO4,
            string uNO1,
            string uNO2,
            string uNO3,
            string uNO4,
            int embalagemId)
        {
            TalieId = talieId;
            RegistroCsId = registroCsId;
            Quantidade = quantidade;
            PatioId = patioId;
            Comprimento = comprimento;
            Largura = largura;
            Altura = altura;
            Peso = peso;
            Remonte = remonte;
            Fumigacao = fumigacao;
            Fragil = fragil;
            Madeira = madeira;
            Avariado = avariado;
            Yard = yard;
            NotaFiscalId = notaFiscalId;
            NotaFiscal = notaFiscal;
            IMO1 = iMO1;
            IMO2 = iMO2;
            IMO3 = iMO3;
            IMO4 = iMO4;
            UNO1 = uNO1;
            UNO2 = uNO2;
            UNO3 = uNO3;
            UNO4 = uNO4;
            EmbalagemId = embalagemId;
        }

        public int TalieId { get; set; }

        public int RegistroCsId { get; set; }

        public string Descricao { get; set; }

        public decimal? Quantidade { get; set; }

        public int PatioId { get; set; }

        public decimal? Comprimento { get; set; }

        public decimal? Largura { get; set; }

        public decimal? Altura { get; set; }

        public decimal Peso { get; set; }

        public string Remonte { get; set; }

        public string Fumigacao { get; set; }

        public bool Fragil { get; set; }

        public bool Madeira { get; set; }

        public bool Avariado { get; set; }

        public string Yard { get; set; }

        public int NotaFiscalId { get; set; }

        public string NotaFiscal { get; set; }

        public string IMO1 { get; set; }

        public string IMO2 { get; set; }

        public string IMO3 { get; set; }

        public string IMO4 { get; set; }

        public string UNO1 { get; set; }

        public string UNO2 { get; set; }

        public string UNO3 { get; set; }

        public string UNO4 { get; set; }

        public string EmbalagemSigla { get; set; }

        public int EmbalagemId { get; set; }

        public string Embalagem { get; set; }

        public decimal QuantidadeDescarga { get; set; }

        public void Alterar(TalieItem item)
        {
            Quantidade = item.Quantidade;
            PatioId = item.PatioId;
            Comprimento = item.Comprimento;
            Largura = item.Largura;
            Altura = item.Altura;
            Peso = item.Peso;
            Remonte = item.Remonte;
            Fumigacao = item.Fumigacao;
            Fragil = item.Fragil;
            Madeira = item.Madeira;
            Avariado = item.Avariado;
            Yard = item.Yard;
            NotaFiscalId = item.NotaFiscalId;
            NotaFiscal = item.NotaFiscal;
            IMO1 = item.IMO1;
            IMO2 = item.IMO2;
            IMO3 = item.IMO3;
            IMO4 = item.IMO4;
            UNO1 = item.UNO1;
            UNO2 = item.UNO2;
            UNO3 = item.UNO3;
            UNO4 = item.UNO4;
            EmbalagemId = item.EmbalagemId;
        }

        public override void Validar()
        {
            RuleFor(c => c.Quantidade)
                .GreaterThan(0)
                .WithMessage("Quantidade não informada");

            RuleFor(c => c.EmbalagemId)
                .GreaterThan(0)
                .WithMessage("Embalagem não informada");

            RuleFor(c => c.NotaFiscal)
                .NotEmpty()
                .WithMessage("Nota Fiscal não informada");

            RuleFor(c => c.RegistroCsId)
                .GreaterThan(0)
                .WithMessage("Item de descarga não informado");

            RuleFor(c => c.Remonte)
                .MaximumLength(3)
                .WithMessage("Remonte inválido");

            ValidationResult = Validate(this);
        }
    }
}