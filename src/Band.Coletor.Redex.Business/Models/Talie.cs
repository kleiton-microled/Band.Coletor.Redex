using Band.Coletor.Redex.Business.Helpers;
using Band.Coletor.Redex.Business.Models.Entities;
using FluentValidation;
using System;

namespace Band.Coletor.Redex.Business.Models
{
    public class Talie : Entidade<Talie>
    {
        private string conteiner;
        private int talieId;

        public Talie()
        {
        }

        public Talie(
            int registroId,
            string placa,
            DateTime? inicio,
            bool crossDocking,
            int conferenteId,
            int equipeId,
            int bookingId,
            string operacaoId,
            int gateId,
            string observacoes)
        {
            RegistroId = registroId;
            Placa = placa;
            Inicio = inicio;
            CrossDocking = crossDocking;
            ConferenteId = conferenteId;
            EquipeId = equipeId;
            BookingId = bookingId;
            OperacaoId = operacaoId;
            GateId = gateId;
            Observacoes = observacoes;
        }

        public Talie(
            string conteinerId,
            DateTime? inicio,
            bool crossDocking,
            int bookingId,
            string operacaoId,
            int conferenteId,
            int equipeId)
        {
            ConteinerId = conteinerId;
            Inicio = inicio;
            CrossDocking = crossDocking;
            BookingId = bookingId;
            OperacaoId = operacaoId;
            ConferenteId = conferenteId;
            EquipeId = equipeId;
        }

        public Talie(string conteiner, DateTime? inicio, bool crossDocking, int bookingId, string operacaoId, int conferenteId, int equipeId, int talieId)
        {
            this.conteiner = conteiner;
            Inicio = inicio;
            CrossDocking = crossDocking;
            BookingId = bookingId;
            OperacaoId = operacaoId;
            ConferenteId = conferenteId;
            EquipeId = equipeId;
            this.talieId = talieId;
        }

        public int RegistroId { get; set; }

        public int Patio { get; set; }
        public DateTime? Inicio { get; set; }

        public DateTime? Termino { get; set; }

        public string DtInicio { get; set; }

        public bool CrossDocking { get; set; }

        public int ConferenteId { get; set; }

        public int EquipeId { get; set; }

        public string ConteinerId { get; set; }

        public int BookingId { get; set; }

        public string OperacaoId { get; set; }
        public string Placa { get; set; }

        public int GateId { get; set; }

        public bool EhEstufagem { get; set; }
        public string Observacoes { get; set; }

        public int Lote { get; set; }
        public int AUTONUM_TALIE { get; set; }

        public int AUTONUM_REG { get; set; }

        public int AUTONUM_PATIO { get; set; }

        public int FLAG_CARREGAMENTO { get; set; }
        public int FLAG_ESTUFAGEM { get; set; }
        public int FLAG_DESCARGA { get; set; }

        public void Alterar(Talie talie)
        {
            Inicio = talie.Inicio;
            CrossDocking = talie.CrossDocking;
            ConferenteId = talie.ConferenteId;
            EquipeId = talie.EquipeId;
            OperacaoId = talie.OperacaoId;
            Observacoes = talie.Observacoes;
        }

        public static Talie InsertCommand(int autonumTalie, DateTime inicio, int conferente, int equipe, int autonumBooking, string operacao,
                                                string placa, int autonumGate, int autonumRegistro, string obs)
        {
            var entity = new Talie();
            entity.AUTONUM_TALIE = autonumTalie;
            entity.Inicio = inicio;
            entity.ConferenteId = conferente;
            entity.EquipeId = equipe;
            entity.BookingId = autonumBooking;
            entity.OperacaoId = operacao;
            entity.Placa = placa;
            entity.GateId = autonumGate;
            entity.RegistroId = autonumRegistro;
            entity.Observacoes = obs;

            return entity;
        }

        public override void Validar()
        {
            RuleFor(c => c.Lote)
                .GreaterThan(0)
                .WithMessage("Lote não informado");

            RuleFor(c => c.Placa)
                .NotNull()
                .WithMessage("A placa é obrigatória")
                .Length(8).WithMessage("Placa inválida");

            //RuleFor(c => c.Fechado)
            //    .Must(c => c == false)
            //    .WithMessage("O Talie já está fechado");

            RuleFor(c => c.Inicio)
                .NotNull()
                .WithMessage("A Data de início é obrigatória");

            RuleFor(c => c.Inicio)
                .Must(c => DateTimeHelpers.IsDate(c))
                .WithMessage("Data de início inválida");

            RuleFor(c => c.ConferenteId)
                .GreaterThan(0)
                .WithMessage("Conferente não informado");

            RuleFor(c => c.EquipeId)
                .GreaterThan(0)
                .WithMessage("Equipe não informada");

            RuleFor(c => c.BookingId)
                .GreaterThan(0)
                .WithMessage("Booking não informado");

            RuleFor(c => c.OperacaoId)
                .NotEmpty()
                .WithMessage("Operação não informada");

            RuleFor(c => c.GateId)
                .GreaterThan(0)
                .WithMessage("Gate não informado");

            ValidationResult = Validate(this);
        }
    }
}