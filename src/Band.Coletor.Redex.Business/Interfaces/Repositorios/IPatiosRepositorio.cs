using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Band.Coletor.Redex.Business.DTO;

namespace Band.Coletor.Redex.Business.Interfaces.Repositorios
{
    public interface IPatiosRepositorio
    {
        PatiosDTO GetPatioByID(int id);
    }
}
