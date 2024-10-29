using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Band.Coletor.Redex.Business.DTO
{
    public class IventarioCNTRDTO
    {
        public string ID_CONTEINER { get; set; }
        public string ID_CONTEINER_D { get; set; }
        public int COUNT_CONTEINER { get; set; }
        public string YARD { get; set; }
        public string YARD_D { get; set; }
        public string IMO1 { get; set; }
        public string IMO2 { get; set; }
        public string IMO3 { get; set; }
        public string IMO4 { get; set; }
        public string IMO1_D { get; set; }
        public string IMO2_D { get; set; }
        public string IMO3_D { get; set; }
        public string IMO4_D { get; set; }
        public int DIST_DELTA { get; set; }

        public int SEGREGACAO { get; set; }
        public string CLASS1 { get; set; }
        public string CLASS2 { get; set; }

        public string IMPEXP { get; set; }
        public string DESCRICAO { get; set; }
        public string EF { get; set; }
        public string DATA_ENT_TEMP { get; set; }
        public string SCALE { get; set; }
        public int BRUTO { get; set; }
        public string NOME { get; set; }
        public decimal TAMANHO { get; set; }
        public string TEMPERATURE { get; set; }
        public string TIPOBASICO { get; set; }
        public string DESCR_MOTIVO_POSIC { get; set; }
        public string FINALITY { get; set; }
        public string FLAG_OOG { get; set; }
        public string FLAG_SPC { get; set; }
        public string DLV_TERM { get; set; }
        public int AUTONUM { get; set; }
        public string SISTEMA { get; set; }
        public int COUNT_YARD { get; set; }
        public string YARD_ATUAL { get; set; }

        public int CNTR { get; set; }
        public string ORIGEM { get; set; }
        public string DESTINO { get; set; }
        public DateTime Data { get; set; }
        public int MOTIVO { get; set; }
        public string TIPO { get; set; }
        public int USUARIO { get; set; }
        public int ID_TRANSPORTADORA { get; set; }
        public int AUTONUM_FROTA_CARRETA { get; set; }
        public int AUTONUM_FROTA_EMPILHADEIRA { get; set; }
        public DateTime? Inicio { get; set;  }

       
    }
}

