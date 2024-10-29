using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Band.Coletor.Redex.Business.DTO
{
    public class MovimentacaoContainerDTO
    {
        public int AUTONUM { get; set; }
        public string IMPEXP { get; set; }
        public string DESCRICAO { get; set; }
        public string EF { get; set; }
        public DateTime DATA_ENT_TEMP { get; set; }
        public string BRUTO { get; set; }
        public string IMO1 { get; set; }
        public string YARD { get; set; }
        public string YARD_DESTINO { get; set; }
        public int TAMANHO { get; set; }
        public string TEMPERATURE { get; set; }
        public string TIPOBASICO { get; set; }
        public string DESCR_MOTIVO_POSIC { get; set; }
        public string dt_prevista_posic { get; set; }
        public string FINALITY { get; set; }
        public string VIAGEM { get; set; }
        public string SITUACAO_BL { get; set; }
        public string QTD { get; set; }
        public string FLAG_OOG { get; set; }
        public string FLAG_SPC { get; set; }
        public string DLV_TERM { get; set; }
        public string SISTEMA { get; set; }
        public string ID_CONTEINER { get; set; }
        public int AUTONUM_CNTR { get; set; }
        public int PATIO { get; set; }
        public int ID_MOTIVO { get; set; }
        public int ID_USUARIO { get; set; }
        public string CAMERA { get; set; }
    }
}
