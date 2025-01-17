using Band.Coletor.Redex.Business.Entity;

namespace Band.Coletor.Redex.Business.DTO
{
    public class RegistroDTO
    {
        public int Id { get; set; }
        public Talie Talie { get; set; }
        public string Placa { get; set; }
        public string Reserva { get; set; }
        public string Cliente { get; set; }
    }
}
