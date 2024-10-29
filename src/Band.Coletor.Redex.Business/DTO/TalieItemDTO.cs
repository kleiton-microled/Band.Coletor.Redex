using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Band.Coletor.Redex.Business.DTO
{
    public class TalieItemDTO
    {
        public int AUTONUM_TI { get; set; }
        public int AUTONUM_TALIE { get; set; }
        public int AUTONUM_EMB { get; set; }

        public int AUTONUM_NF { get; set; }
        public int QTDE_DESCARGA { get; set; }
        public decimal COMPRIMENTO { get; set; }
        public decimal LARGURA { get; set; }
        public decimal ALTURA { get; set; }
        public decimal Peso { get; set; }
        public int TalieItemId { get; set; }
        public int QuantidadeEstufagem { get; set; }
        public string YARD { get; set; }
        public int Quantidade { get; set; }
        public int Carimbo { get; set; }
        public int FLAG_MADEIRA { get; set; }
        public int FLAG_FRAGIL { get; set; }
        public int REMONTE { get; set; }
        public int FLAG_AVARIA { get; set; }
        public int FLAG_REMONTE { get; set; }
        public int ItemTalie { get; set; }
        public string IMO { get; set; }
        public string IMO2 { get; set; }
        public string IMO3 { get; set; }
        public string IMO4 { get; set; }
        public string UNO { get; set; }
        public string UNO2 { get; set; }
        public string UNO3 { get; set; }
        public string UNO4 { get; set; }
        public string MARCACAO { get; set; }
        public string OBSERVACAO { get; set; }
        public int FLAG_NUMERADA { get; set; }

    }
}
