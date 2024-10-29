using FluentValidation;
using System;

namespace Band.Coletor.Redex.Business.Models
{
    public class Conteiner : Entidade<Conteiner>
    {
        public Conteiner()
        {
        }

        public Conteiner(string idConteiner, int tamanho, string status, string reserva, string referencia, string cliente, string armador, DateTime? deadline, string lacres, int lacreControle, DateTime? entrada, int totalProgramado, int aReceber, int entregues, int armPatio, int estufados, int cCSemMonitoramento)
        {
            IdConteiner = idConteiner;
            Tamanho = tamanho;
            Status = status;
            Reserva = reserva;
            Referencia = referencia;
            Cliente = cliente;
            Armador = armador;
            Deadline = deadline;
            Lacres = lacres;
            LacreControle = lacreControle;
            Entrada = entrada;
            TotalProgramado = totalProgramado;
            AReceber = aReceber;
            Entregues = entregues;
            ArmPatio = armPatio;
            Estufados = estufados;
            CCSemMonitoramento = cCSemMonitoramento;
        }

        public string IdConteiner { get; set; }

        public int Tamanho { get; set; }

        public string Status { get; set; }

        public string Reserva { get; set; }

        public string Referencia { get; set; }

        public string Cliente { get; set; }

        public string Armador { get; set; }

        public DateTime? Deadline { get; set; }

        public string Lacres { get; set; }

        public int LacreControle { get; set; }

        public DateTime? Entrada { get; set; }

        public int TotalProgramado { get; set; }

        public int AReceber { get; set; }

        public int Entregues { get; set; }

        public int ArmPatio { get; set; }

        public int Estufados { get; set; }

        public int CCSemMonitoramento { get; set; }

        public int Patio { get; set; }

        public override void Validar()
        {
            RuleFor(c => c.IdConteiner)
                .NotNull()
                .WithMessage("Contêiner não informado");

            ValidationResult = Validate(this);
        }

        public string ConteinerDeadline { get { return string.Format("{0}{1}{2}", IdConteiner, (Deadline != null ? " - " : ""), Deadline); } }
    }
}