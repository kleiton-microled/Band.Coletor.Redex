using Band.Coletor.Redex.Application.ViewModel;
using System.Collections.Generic;

namespace Band.Coletor.Redex.Site.Models.MarcanteCargaSolta
{
    public class MarcanteCargaSoltaViewModel
    {
        public int Lote { get; set; }
        public IEnumerable<ConteinerViewModel> Containers { get; set; }
        public string Volume { get; set; }
        public int QuantidadeMarcantes { get; set; }
        public string Marcante { get; set; }
    }
}