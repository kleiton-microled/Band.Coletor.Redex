using Band.Coletor.Redex.Business.Enums;
using Band.Coletor.Redex.Business.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Band.Coletor.Redex.Site.Models
{
    public class RegistroViewModel
    {
        public RegistroViewModel()
        {
          
            Equipes = new List<Equipe>();
            Operacoes = new List<Operacao>();
            Talies = new List<TalieConteiner>();
        }

        public int GateId { get; set; }

        public int Lote { get; set; }

        [Display(Name = "Início")]
        public DateTime? Inicio { get; set; }

        [Display(Name = "Término")]
        public DateTime? Termino { get; set; }

        [Display(Name = "Pátio")]
        public int Patio { get; set; }

        public int PatioId { get; set; }

        public string Placa { get; set; }

        [Display(Name = "Contêiner")]
        public string Conteiner { get; set; }

        public int ExportadorId { get; set; }

        public string Exportador { get; set; }

        public int Reserva { get; set; }

        public string Referencia { get; set; }

        [Display(Name = "Conferente")]
        public int ConferenteId { get; set; }

        [Display(Name = "Equipe")]
        public int EquipeId { get; set; }

        [Display(Name = "Cross Docking")]
        public bool CrossDocking { get; set; }

        [Display(Name = "Observações")]
        public string Observacoes { get; set; }

        [Display(Name = "Tipo Operação")]
        public TipoDescarga TipoDescarga { get; set; }

        [Display(Name = "Modo")]
        public string OperacaoId { get; set; }

        [Display(Name = "Talie")]
        public int TalieId { get; set; }

        public int Conferentes { get; set; }

        public List<Equipe> Equipes { get; set; }

        public List<Operacao> Operacoes { get; set; }

        public List<TalieConteiner> Talies { get; set; }
    }
}