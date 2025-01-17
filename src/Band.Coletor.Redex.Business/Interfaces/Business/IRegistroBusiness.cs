using Band.Coletor.Redex.Application.ViewModel;
using Band.Coletor.Redex.Business.Classes.ServiceResult;
using System.Threading.Tasks;

namespace Band.Coletor.Redex.Business.Interfaces.Business
{
    public interface IRegistroBusiness
    {
        Task<RegistroViewModel> CarregarRegistro(int registro);
        ServiceResult<int> SaveOrUpdate(RegistroViewModel registro);
        void GeraDescargaAutomatica(int codigoRegistro, int autonumTalie);
        bool ValidarNotaCadastrada(int codigoRegistro);
        int ValidarDanfe(int codigoRegistro);
        void GravarObservacao(string observacao, long talie);
    }
}
