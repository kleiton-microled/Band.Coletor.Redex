using Band.Coletor.Redex.Application.ViewModel;
using System.Threading.Tasks;

namespace Band.Coletor.Redex.Business.Interfaces.Business
{
    public interface IMarcanteBusiness
    {
        Task<MarcanteViewModel> BuscarMarcante(string marcante);
    }
}
