using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Band.Coletor.Redex.Business.Models;

namespace Band.Coletor.Redex.Site.Models
{
    public class PreRegistroViewModel
    {
        
        [Display(Name = "Nº Protocolo")]
        public string ProtocoloPesquisa { get; set; }

        [Display(Name = "Ano Protocolo")]
        public string AnoProtocoloPesquisa { get { return DateTime.Now.Year.ToString(); }  }

        [Display(Name = "Placa")]
        public string PlacaPesquisa { get; set; }

        [Display(Name = "Placa Carreta")]
        public string PlacaCarretaPesquisa { get; set; }

        [Display(Name = "Ticket Entrada")]
        public string TicketPesquisa { get; set; }

        [Display(Name = "Protocolo")]
        public string Protocolo { get; set; }

        public string Placa { get; set; }

        [Display(Name = "Placa Carreta")]
        public string PlacaCarreta { get; set; }

        public string Ticket { get; set; }

        [Display(Name = "Período")]
        public string Periodo { get; set; }

        [Display(Name = "Período Inicial")]
        public string PeriodoInicial { get; set; }

        [Display(Name = "Período Final")]
        public string PeriodoFinal { get; set; }

        public string Motorista { get; set; }

        public string CNH { get; set; }

        [Display(Name = "Finalidade")]
        public string FinalidadeId { get; set; }

        [Display(Name = "Pátio destino")]
        public int PatioDestinoId { get; set; }
        public string DescPatioDestino { get; set; }
        public bool PesquisarPorProtocolo { get; set; }
        public List<PreRegistro> PatioDestino { get; set; }

    }
}
