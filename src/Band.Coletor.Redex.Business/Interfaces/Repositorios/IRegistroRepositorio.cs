using Band.Coletor.Redex.Business.Models;

namespace Band.Coletor.Redex.Business.Interfaces.Repositorios
{
    public interface IRegistroRepositorio
    {
        Registro ObterRegistroPorLote(int lote);
    }
}