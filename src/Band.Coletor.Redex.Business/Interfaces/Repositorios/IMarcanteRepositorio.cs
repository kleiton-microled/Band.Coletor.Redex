using System.Threading.Tasks;
using Band.Coletor.Redex.Business.Models;

namespace Band.Coletor.Redex.Business.Interfaces.Repositorios
{
    public interface IMarcanteRepositorio : IBaseRepositorio<Marcante>
    {
        Task<Marcante> BuscarMarcante(string marcante);
    }
}