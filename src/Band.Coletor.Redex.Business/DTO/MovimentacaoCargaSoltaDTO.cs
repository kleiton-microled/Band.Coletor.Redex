using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Band.Coletor.Redex.Business.DTO
{
    public class MovimentacaoCargaSoltaDTO
    {
        public string LOTE_STR { get; set; }
        public string MERCADORIA { get; set; }
        public string MARCA { get; set; }
        public string DATA_ENTRADA { get; set; }
        public string IMPORTADOR { get; set; }
        public string CNTR_DESOVA { get; set; }
        public int ID_PATIO { get; set; }
        public string VOLUME { get; set; }
        public string TIPO_DOC { get; set; }
        public string CANAL_ALF { get; set; }
        public string MOTIVO_PROX_MVTO { get; set; }
        public string IMO { get; set; }
        public string NVOCC { get; set; }
        public string BL { get; set; }
        public int LOTE { get; set; }
        public string SISTEMA { get; set; }
        public string ANVISA { get; set; }
        public int FLAG_ANVISA { get; set; }
        public string ONU { get; set; }
        public int QUANTIDADE { get; set; }
        public string LOCAL { get; set; }
        public int SALDO { get; set; }
        public int AUTONUM_CS { get; set; }
        public string REFERENCE { get; set; }
        public string EMBALAGEM { get; set; }
        public string NUM_NF { get; set; }
    }
    public class MovimentacaoCargaSoltaItemDTO
    {
        public int QTDE { get; set; }
        public string EMBALAGEM { get; set; }
        public string LOCAL { get; set; }
        public string YARD { get; set; }
        public int AUTONUM { get; set; }
        public string DISPLAY { get; set; }

    }
    public class InserirNovaMovimentacaoCargaSoltaDTO
    {
        public int AUTONUM { get; set; }
        public int AUTONUM_CS { get; set; }
        public string ARMAZEM { get; set; }
        public string YARD { get; set; }
        public int ORIGEM { get; set; }
        public string QUANTIDADE { get; set; }
        public string QUANTIDADE_POS { get; set; }
        public int MOTIVO_COL { get; set; }
        public string SISTEMA { get; set; }
        public int ID_MARCANTE { get; set; }
        public int ID_USUARIO { get; set; }
        public string LOTE { get; set; }
        public int ITEM_CS { get; set; }
        public int ARMADOR { get; set; }
        public string LOCAL_POS { get; set; }
        public string TIPO { get; set; }
        public string ETIQUETA { get; set; }
    }
}
