using Band.Coletor.Redex.Business.DTO;
using Band.Coletor.Redex.Business.Models;
using Band.Coletor.Redex.Business.Models.ModelsLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Band.Coletor.Redex.Business.Interfaces.Repositorios
{
    public interface IArmazensRepositorio 
    {
        IEnumerable<ArmazensDTO> GetComboArmazens(int patio);

    }
}
