using System.Collections.Generic;
using System.Threading.Tasks;

namespace Band.Coletor.Redex.Business.Interfaces.Repositorios
{
    public interface IBaseRepositorio<T> where T : class
    {
        Task<IEnumerable<T>> ListAll(string customQuery = null);
        T GetById(int id);
        void Add(T entity);
        void Update(T entity);
        void Delete(int id);
    }

}
