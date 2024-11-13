using Band.Coletor.Redex.Business.Interfaces.Repositorios;
using Band.Coletor.Redex.Business.Models;
using Band.Coletor.Redex.Infra.Repositorios.Sql;
using Dapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Band.Coletor.Redex.Infra.Repositorios
{
    public class EquipeRepositorio : BaseRepositorio<Equipe>, IEquipeRepositorio
    {
        public EquipeRepositorio(string connectionString) : base(connectionString)
        {
        }

        public async Task<IEnumerable<Equipe>> GetAllEquipes()
        {
            try
            {
                using (var connection = Connection)
                {
                    return await connection.QueryAsync<Equipe>(SqlQueries.BuscarEquipes);
                }
            }
            catch (System.Exception ex)
            {

                throw;
            }

        }

        public IEnumerable<Equipe> ObterEquipes()
        {
            using (var connection = Connection)
            {
                return connection.Query<Equipe>(@"
                SELECT
                    AUTONUM_EQP as Id,
                    NOME_EQP as Descricao
                FROM
                    REDEX..TB_EQUIPE
                WHERE
                    FLAG_ATIVO = 1 AND FLAG_OPERADOR = 1
                ORDER BY
                    NOME_EQP");
            }
        }
    }
}