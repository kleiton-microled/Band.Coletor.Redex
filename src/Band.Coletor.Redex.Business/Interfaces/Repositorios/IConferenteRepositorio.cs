using Band.Coletor.Redex.Business.Models;
using System.Collections.Generic;

namespace Band.Coletor.Redex.Business.Interfaces.Repositorios
{
    public interface IConferenteRepositorio
    {
        IEnumerable<Conferente> ObterConferentes(int idConferente);
    }
}