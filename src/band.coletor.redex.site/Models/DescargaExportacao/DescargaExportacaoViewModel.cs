using Band.Coletor.Redex.Application.ViewModel;
using System.Collections.Generic;

namespace Band.Coletor.Redex.Site.Models.DescargaExportacao
{
    // ViewModel para carregar dados dos ComboBox
    public class DescargaExportacaoViewModel
    {
       public IEnumerable<ConferenteViewModel> Conferentes { get; set; }
        public IEnumerable<EquipeViewModel> Equipes { get; set; }
        public IEnumerable<OperacaoViewModel> Operacoes { get; set; }
        public IEnumerable<ItensDescarregadosViewModel> Itens { get; set; }
    }

}
