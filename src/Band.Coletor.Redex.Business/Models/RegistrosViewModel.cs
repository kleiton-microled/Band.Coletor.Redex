using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Band.Coletor.Redex.Business.Models
{
    public class RegistrosViewModel
    {

        [Display(Name = "Nº Protocolo")]
        public string ProtocoloPesquisa { get; set; }

        [Display(Name = "Ano Protocolo")]
        public string AnoProtocoloPesquisa { get { return DateTime.Now.Year.ToString(); } }
        [Display(Name = "Frota")]
        public string Frota { get; set; }
        public int ID_frota { get; set; }

        [Display(Name = "Placa")]
        public string PlacaPesquisa { get; set; }

        [Display(Name = "Reserva / DI")]
        public string Reserva { get; set; }


        [Display(Name = "Placa Carreta")]
        public string PlacaCarretaPesquisa { get; set; }

        [Display(Name = "Ticket Entrada")]
        public string TicketPesquisa { get; set; }

        public string Placa { get; set; }

        [Display(Name = "Placa Carreta")]
        public string PlacaCarreta { get; set; }

        [Display(Name = "Reboque")]
        public string Reboque { get; set; }

        public string Ticket { get; set; }

        [Display(Name = "Período")]
        public string Periodo { get; set; }

        [Display(Name = "Motorista")]
        public string Motorista { get; set; }

        [Display(Name = "Período Inicial")]
        public string PeriodoInicial { get; set; }

        [Display(Name = "Período Final")]
        public string PeriodoFinal { get; set; }
        public string Tipo_Carga { get; set; }

        [Display(Name = "Conteiner")]
        public string Id_Conteiner1 { get; set; }

        [Display(Name = "Lacre 1 ")]
        public string Lacre1 { get; set; }

        [Display(Name = "Conteiner")]
        public string Id_Conteiner2 { get; set; }

        [Display(Name = "Lacre 2")]
        public string Lacre2 { get; set; }

        [Display(Name = "Conteiner")]
        public string Id_Conteiner3 { get; set; }

        [Display(Name = "Lacre 3")]
        public string Lacre3 { get; set; }

        [Display(Name = "Conteiner")]
        public string Id_Conteiner4 { get; set; }

        [Display(Name = "Cheio/Vazio")]
        public string Full_Empty { get; set; }
        public string chk1 { get; set; }
        public string chk2 { get; set; }
        public string chk3 { get; set; }
        public string chk4 { get; set; }
        public string EF1 { get; set; }
        public string Descricao_Status { get; set; }
        public string EF2 { get; set; }
        public string Descricao_Status2 { get; set; }

        public string EF3 { get; set; }
        public string Descricao_Status3 { get; set; }

        public string EF4 { get; set; }
        public string Descricao_Status4 { get; set; }

        public string autonumBoo { get; set; }


    }
}
