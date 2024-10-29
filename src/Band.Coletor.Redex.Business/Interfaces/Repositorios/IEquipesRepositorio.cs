using Band.Coletor.Redex.Business.Models;
using System.Collections.Generic;

namespace Band.Coletor.Redex.Business.Interfaces.Repositorios
{
    public interface IEquipeRepositorio
    {
        IEnumerable<Equipe> ObterEquipes();
    }
}