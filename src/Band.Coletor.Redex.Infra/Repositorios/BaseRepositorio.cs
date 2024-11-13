using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Band.Coletor.Redex.Business.Interfaces.Repositorios;
using Band.Coletor.Redex.Business.Models;
using System.Linq;

namespace Band.Coletor.Redex.Infra.Repositorios
{
    public abstract class BaseRepositorio<T> : IBaseRepositorio<T> where T : class
    {
        private readonly string _connectionString;

        protected BaseRepositorio(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected IDbConnection Connection => new SqlConnection(_connectionString);

        public virtual async Task<IEnumerable<T>> ListAll(string customQuery = null)
        {
            using (var connection = Connection)
            {
                // Obtém o nome da tabela a partir do atributo ou do nome da classe
                var tableAttribute = typeof(T).GetCustomAttributes(typeof(TableAttribute), false).FirstOrDefault() as TableAttribute;
                string tableName = tableAttribute != null ? tableAttribute.Name : typeof(T).Name;

                // Usa a consulta personalizada, se fornecida; caso contrário, usa a consulta padrão
                string query = customQuery ?? $"SELECT * FROM {tableName}";

                return await connection.QueryAsync<T>(query);
            }
        }


        public T GetById(int id)
        {
            using (var connection = Connection)
            {
                string query = $"SELECT * FROM {typeof(T).Name} WHERE Id = @Id";
                return connection.QuerySingleOrDefault<T>(query, new { Id = id });
            }
        }

        public void Add(T entity)
        {
            using (var connection = Connection)
            {
                // Exemplo de um insert genérico - precisaria ser customizado para cada entidade
                connection.Execute($"INSERT INTO {typeof(T).Name} ... VALUES ...", entity);
            }
        }

        public void Update(T entity)
        {
            using (var connection = Connection)
            {
                // Exemplo de um update genérico - precisaria ser customizado para cada entidade
                connection.Execute($"UPDATE {typeof(T).Name} SET ... WHERE Id = @Id", entity);
            }
        }

        public void Delete(int id)
        {
            using (var connection = Connection)
            {
                string query = $"DELETE FROM {typeof(T).Name} WHERE Id = @Id";
                connection.Execute(query, new { Id = id });
            }
        }
    }
}
