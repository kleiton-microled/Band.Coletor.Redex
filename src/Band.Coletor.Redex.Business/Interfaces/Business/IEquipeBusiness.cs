using Band.Coletor.Redex.Application.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Band.Coletor.Redex.Business.Interfaces.Business
{
    public interface IEquipeBusiness
    {
        Task<IEnumerable<EquipeViewModel>> ListAll();
    }
}
