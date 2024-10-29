using Band.Coletor.Redex.Business.Models;

namespace Band.Coletor.Redex.Business.Interfaces.Repositorios
{
    public interface IReservaRepositorio
    {
        Reserva ObterDadosReservaPorConteiner(string conteiner);

        int ObterParceiroPorIdReserva(int idReserva);
    }
}