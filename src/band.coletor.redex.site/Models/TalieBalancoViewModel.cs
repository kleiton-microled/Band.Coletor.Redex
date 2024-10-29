using Band.Coletor.Redex.Business.DTO;
using System.Collections.Generic;

namespace Band.Coletor.Redex.Site.Models
{
    public class TalieBalancoViewModel
    {
        public TalieBalancoViewModel()
        {
            VolumesDescarregados = new List<NotaFiscalDTO>();
            VolumesNaoDescarregados = new List<NotaFiscalDTO>();
        }

        public int TalieId { get; set; }

        public int QuantidadeNF { get; set; }

        public int Descarregados { get; set; }

        public string Mercadoria { get; set; }

        public string Embalagem { get; set; }

        public List<NotaFiscalDTO> VolumesDescarregados { get; set; }

        public List<NotaFiscalDTO> VolumesNaoDescarregados { get; set; }
    }
}