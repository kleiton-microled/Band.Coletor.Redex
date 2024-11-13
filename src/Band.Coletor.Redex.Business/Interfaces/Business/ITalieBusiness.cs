using Band.Coletor.Redex.Application.ViewModel;

namespace Band.Coletor.Redex.Business.Interfaces.Business
{
    public interface ITalieBusiness
    {
        TalieViewModel ObterDadosTaliePorRegistro(int registro);
    }
}
