using Band.Coletor.Redex.Business.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Band.Coletor.Redex.Business.Interfaces.Repositorios
{
    public interface IEquipeRepositorio
    {
        IEnumerable<Equipe> ObterEquipes();
        Task<IEnumerable<Equipe>> GetAllEquipes();
    }
}