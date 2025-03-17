namespace Band.Coletor.Redex.Application.ViewModel
{
    public class MarcanteViewModel : BaseViewModel
    {
        public int TalieId { get; set; }

        public int BookingId { get; set; }

        public int RegistroCsId { get; set; }

        public bool Registro { get; set; }

        public int PatioId { get; set; }

        public string CodigoMarcante { get; set; }

        public int Quantidade { get; set; }

        public int Volumes { get; set; }

        public int QuantidadeDescarregada { get; set; }

        public int QuantidadeAssociada { get; set; }

        public int ArmazemId { get; set; }

        public string Armazem { get; set; }

        public string Quadra { get; set; }

        public string Rua { get; set; }

        public string Fiada { get; set; }
    }
}
