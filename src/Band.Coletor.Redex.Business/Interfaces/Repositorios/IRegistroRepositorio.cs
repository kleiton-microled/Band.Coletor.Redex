using Band.Coletor.Redex.Application.ViewModel;
using System.Threading.Tasks;
using Band.Coletor.Redex.Business.Models;
using Band.Coletor.Redex.Business.DTO;

namespace Band.Coletor.Redex.Business.Interfaces.Repositorios
{
    public interface IRegistroRepositorio
    {
        Registro ObterRegistroPorLote(int lote);
        Task<RegistroDTO> CarregarRegistro(int registro);
        bool ValidarNotaCadastrada(int codigoRegistro);
        void GeraDescargaAutomatica(int codigoRegistro, int autonumTalie);
        int ValidarDanfe(int codigoRegistro);
        int SaveOrUpdate(RegistroViewModel registro);
        void GravarObservacao(string observacao, long talie);

    }
}