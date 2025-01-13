using Band.Coletor.Redex.Application.ViewModel;
using Band.Coletor.Redex.Business.Classes.ServiceResult;
using System.Threading.Tasks;

namespace Band.Coletor.Redex.Business.Interfaces.Business
{
    public interface ITalieBusiness
    {
        Task<TalieViewModel> ObterDadosTaliePorRegistro(int registro);
        Task<ServiceResult<int>> Save(TalieViewModel talieViewModel);
        Task<ServiceResult<bool>> Update(TalieViewModel talieViewModel);
        ServiceResult<int> ObterNotaFiscal(string numeroNotaFiscal, string codigoBooking, string codigoRegistro);
        Task<ServiceResult<TalieItemViewModel>> ObterItensNotaFiscal(string numeroNotaFiscal, string codigoRegistro);
    }
}
