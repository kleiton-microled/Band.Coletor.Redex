using Band.Coletor.Redex.Application.ViewModel.View;

namespace Band.Coletor.Redex.Application.ViewModel
{
    public class RegistroViewModel
    {
        public int CodigoRegistro { get; set; }
        public View.TalieViewModel Talie { get; set; } = new View.TalieViewModel();
        public string Placa { get; set; }
        public string Reserva { get; set; }
        public string Cliente { get; set; }

    }
}
