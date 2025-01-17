using System.Collections.Generic;

namespace Band.Coletor.Redex.Application.ViewModel.View
{
    public class TalieViewModel
    {
        public int Id { get; set; }
        public string Inicio { get; set; }
        public string Termino { get; set; }
        public int Conferente { get; set; }
        public int Equipe { get; set; }
        public int Operacao { get; set; }
        public string Observacao { get; set; }
        public List<TalieItemViewModel> TalieItem { get; set; }

    }
}
