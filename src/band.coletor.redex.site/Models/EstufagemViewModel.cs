using Band.Coletor.Redex.Business.DTO;
using Band.Coletor.Redex.Business.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Band.Coletor.Redex.Site.Models
{
    public class EstufagemViewModel
    {
        public EstufagemViewModel()
        {
            Clientes = new List<ClienteDTO>();
            Conteineres = new List<Conteiner>();
            Estufagens = new List<Estufagem>();
        }

        public int TalieId { get; set; }

        public string Reserva { get; set; }

        [Display(Name = "Cliente")]
        public int ClienteId { get; set; }

        [Display(Name = "Contêiner")]
        public int ConteinerId { get; set; }

        public string Conteiner { get; set; }

        [Display(Name = "Início")]
        public DateTime? Inicio { get; set; }

        [Display(Name = "Término")]
        public DateTime? Termino { get; set; }

        [Display(Name = "Conferente")]
        public int ConferenteId { get; set; }

        [Display(Name = "Equipe")]
        public int EquipeId { get; set; }

        [Display(Name = "Modo")]
        public string OperacaoId { get; set; }

        public string Marcante { get; set; }

        public int Quantidade { get; set; }

        [Display(Name ="Item NF.")]
        public int itemNF { get; set; }

        public int NF { get; set; }

        public string Lote { get; set; }

        public bool CargaSuzano { get; set; }
        public bool CrossDocking { get; set; }

        public int BookingId { get; set; }

        public List<ClienteDTO> Clientes { get; set; }
        public List<Conteiner> Conteineres { get; set; }
        public List<Conferente> Conferentes { get; set; }
        public List<Equipe> Equipes { get; set; }
        public List<Operacao> Operacoes { get; set; }

        public List<Estufagem> Estufagens { get; set; }

    }
}
