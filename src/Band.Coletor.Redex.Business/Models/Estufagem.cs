using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Band.Coletor.Redex.Business.Models
{
    public class Estufagem
    {
        public string NF { get; set; }
        public int Quantidade { get; set; }
        public int SomaQuant { get; set; }
        public int QuantNota { get; set; }
        public int EstufadaInd { get; set; }
        public string Reserva { get; set; }
        public string Conteiner { get; set; }
        public string Embalagem { get; set; }
        public string Produto { get; set; }
        public string Peso { get; set; }
        public decimal Volume { get; set; }
        public string Cliente { get; set; }
        public string Navio { get; set; }
        public string Viagem { get; set; }
        public string NavioViagem { get; set; }

        public string CodigoBarra { get; set; }

        public int PatioCargaId { get; set; }

        public int AUTONUM_BOO { get; set; }
        public int AUTONUM_PATIO { get; set; }
        public int AUTONUM_RO { get; set; }
        public string FANTASIA { get; set; }
        public int AUTONUM_CLIENTE { get; set; }
        public int AUTONUM { get; set; }
        public int SALDO { get; set; }
        public int FLAG_FECHADO { get; set; }
        public string Mercadoria { get; set; }
        public int AUTONUM_SC { get; set; }
        public int AUTONUM_RCS { get; set; }
        public string DISPLAY { get; set; }
        public string CLIENTE { get; set; }
        public DateTime? INICIO { get; set; }
        public DateTime? TERMINO { get; set; }
        public string CONFERENTE { get; set; }
        public int EQUIPE { get; set; }
        public string FORMA_OPERACAO { get; set; }
        public int Fechado { get; set; }
        public int AUTONUM_TALIE { get; set; }
        public int AUTONUM_NFI { get; set; }
        public string RESERVA { get; set; }
        public int QTDE { get; set; }
        public int QTDE_SAIDA { get; set; }
        public string OS { get; set; }
        public string ID_CONTEINER { get; set; }
        public int AUTONUM_PCS { get; set; }
        public int AUTONUM_EMB { get; set; }
        public int BRUTO { get; set; }
        public decimal COMPRIMENTO { get; set; }
        public decimal LARGURA { get; set; }
        public decimal ALTURA { get; set; }
        public int AUTONUM_PRO { get; set; }
        public string CODPRODUTO { get; set; }
        public decimal VOLUME { get; set; }
        public decimal VOLUME_TOTAL { get; set;}
        public bool CrossDocking { get; set; }
        public int ConferenteId { get; set; }
        public int BookingId { get; set; }
        public int EquipeId { get; set; }
        public int ConteinerId { get;set; } 
        public string OperacaoId { get; set; }
        //public string Termino { get; set; }       
        public int autonum_nf { get; set; }
        public string NUM_NF { get; set; }
        public int flag_CARREGAMENTO { get; set; }
        public int flag_fechado { get; set; }
        public int FLAG_ESTUFAGEM { get; set; }
        public string Obs { get; set; }
    }
}
