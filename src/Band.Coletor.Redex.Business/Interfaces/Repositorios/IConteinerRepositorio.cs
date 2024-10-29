using Band.Coletor.Redex.Business.Models;
using System.Collections.Generic;

namespace Band.Coletor.Redex.Business.Interfaces.Repositorios
{
    public interface IConteinerRepositorio
    {
        IEnumerable<Conteiner> ObterConteiners();

        Conteiner ObterConteinerPorNumero(string numeroConteiner);

        IEnumerable<Conteiner> ConsultarConteinerPorNumero(string idConteiner);

        IEnumerable<Conteiner> ConsultarConteinersReserva(string numeroReserva);

        Conteiner ConsultarResumoGeralReserva(string numeroReserva);

        string ObterConteinerPorId(int conteinerId);
    }
}