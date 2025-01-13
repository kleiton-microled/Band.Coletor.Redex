using Band.Coletor.Redex.Application.ViewModel;
using Band.Coletor.Redex.Business.Classes.ServiceResult;
using System.Collections.Generic;

namespace Band.Coletor.Redex.Business.Interfaces.Business
{
    public interface IConteinerBusiness
    {
        ServiceResult<IEnumerable<ConteinerViewModel>> ObterContainersMarcantes(string lote, int patio);
        ServiceResult<CargaConteinerViewModel> CarregarDadosContainer(string lote);
    }
}
