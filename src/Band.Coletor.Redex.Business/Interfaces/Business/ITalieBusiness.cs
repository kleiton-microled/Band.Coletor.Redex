using Band.Coletor.Redex.Application.ViewModel;
using System.Threading.Tasks;

namespace Band.Coletor.Redex.Business.Interfaces.Business
{
    public interface ITalieBusiness
    {
        Task<TalieViewModel> ObterDadosTaliePorRegistro(int registro);
        int Gravar(TalieViewModel talieViewModel);
        Task<bool> UpdateTalie(TalieViewModel talieViewModel);
    }
}
