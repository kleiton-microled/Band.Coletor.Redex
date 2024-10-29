using Band.Coletor.Redex.Business.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Band.Coletor.Redex.Site.Models
{
    public class TalieViewModel
    {
        public TalieViewModel()
        {
           
            Equipes = new List<Equipe>();
            Operacoes = new List<Operacao>();
            Conteiners = new List<TalieConteiner>();
        }

        [Display(Name = "Talie")]
        public int? TalieNumero { get; set; }

        public int TalieId { get; set; }

        public string ConteinerId { get; set; }

        [Display(Name = "Início")]
        public DateTime? Inicio { get; set; }

        [Display(Name = "Término")]
        public DateTime? Termino { get; set; }

        [Display(Name = "Lote")]
        public string Registro { get; set; }

        public string Placa { get; set; }

        public int BookingId { get; set; }

        public string Reserva { get; set; }

        public string Cliente { get; set; }

        public int GateId { get; set; }

        [Display(Name = "Conferente")]
        public int ConferenteId { get; set; }

        [Display(Name = "Equipe")]
        public int EquipeId { get; set; }

        [Display(Name = "Operação")]
        public string OperacaoId { get; set; }

        [Display(Name = "Cross Docking")]
        public bool CrossDocking { get; set; }

        [Display(Name = "Observações")]
        public string Observacoes { get; set; }

        public int Conferentes { get; set; }

        public List<Equipe> Equipes { get; set; }

        public List<Operacao> Operacoes { get; set; }

        public List<TalieConteiner> Conteiners { get; set; }
    }
}