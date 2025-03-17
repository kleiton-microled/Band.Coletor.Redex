using System;
using System.Threading.Tasks;
using Band.Coletor.Redex.Business.Interfaces.Repositorios;
using Band.Coletor.Redex.Business.Models;
using Dapper;

namespace Band.Coletor.Redex.Infra.Repositorios
{
    public class MarcanteRepositorio : BaseRepositorio<Marcante>, IMarcanteRepositorio
    {
        public MarcanteRepositorio(string connectionString) : base(connectionString)
        {
        }

        public async Task<Marcante> BuscarMarcante(string marcante)
        {
            using (var connection = Connection)
            {
                string query = @"SELECT * FROM REDEX.dbo.TB_MARCANTES tm";

               
                try
                {
                   return await connection.QueryFirstOrDefaultAsync<Marcante>(query);
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }
    }
}
