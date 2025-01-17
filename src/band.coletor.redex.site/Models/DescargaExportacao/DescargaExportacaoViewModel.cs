using Band.Coletor.Redex.Application.ViewModel;
using System.Collections.Generic;

namespace Band.Coletor.Redex.Site.Models.DescargaExportacao
{
    // ViewModel para carregar dados dos ComboBox
    public class DescargaExportacaoViewModel
    {
        public Application.ViewModel.RegistroViewModel Registro { get; set; }
        public IEnumerable<ConferenteViewModel> Conferentes { get; set; }
        public IEnumerable<EquipeViewModel> Equipes { get; set; }
        public IEnumerable<OperacaoViewModel> Operacoes { get; set; }
        public IEnumerable<ItensDescarregadosViewModel> Itens { get; set; }
        public IEnumerable<ArmazemViewModel> Armazems { get; set; }
    }


}
