using Band.Coletor.Redex.Business.DTO;
using System.Collections.Generic;

namespace Band.Coletor.Redex.Business.Interfaces.Repositorios
{
    public interface IArmazensRepositorio 
    {
        IEnumerable<ArmazensDTO> GetComboArmazens(int patio);

    }
}
