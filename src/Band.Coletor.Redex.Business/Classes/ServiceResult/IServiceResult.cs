using System.Collections.Generic;

namespace Band.Coletor.Redex.Business.Classes.ServiceResult
{
    public interface IServiceResult<T>
    {
        bool Status { get; set; }
        IList<string> Mensagens { get; set; }
    }
}
