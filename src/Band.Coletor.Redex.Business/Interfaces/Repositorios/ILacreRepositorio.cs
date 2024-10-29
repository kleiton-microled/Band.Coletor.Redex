using Band.Coletor.Redex.Business.Models;
using System.Collections.Generic;

namespace Band.Coletor.Redex.Business.Interfaces.Repositorios
{
    public interface ILacreRepositorio
    {
        IEnumerable<Lacre> BuscarLacresPorConteiner(string idConteiner);

        void AtualizarLacres(Lacre lacre);

        void AtualizarLacres(int patio, string usuario, int validado);

        IEnumerable<string> BuscarLacresPorPatio(int patio);
    }
}