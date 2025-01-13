using Band.Coletor.Redex.Business.Classes.ServiceResult;
using Band.Coletor.Redex.Business.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Band.Coletor.Redex.Business.Interfaces.Repositorios
{
    public interface IConteinerRepositorio : IBaseRepositorio<ConteinerBL>
    {
        IEnumerable<Conteiner> ObterConteiners();

        Conteiner ObterConteinerPorNumero(string numeroConteiner);

        IEnumerable<Conteiner> ConsultarConteinerPorNumero(string idConteiner);

        IEnumerable<Conteiner> ConsultarConteinersReserva(string numeroReserva);

        Conteiner ConsultarResumoGeralReserva(string numeroReserva);

        string ObterConteinerPorId(int conteinerId);
        ServiceResult<IEnumerable<ConteinerBL>> ObterContainersMarcantes(string lote, int patio);
        ServiceResult<CargaConteiner> CarregarDadosContainer(string lote);
    }
}