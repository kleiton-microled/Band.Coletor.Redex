using Band.Coletor.Redex.Business.Interfaces.Repositorios;
using Band.Coletor.Redex.Business.Models;
using Band.Coletor.Redex.Infra.Configuracao;
using Dapper;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Band.Coletor.Redex.Infra.Repositorios
{
    public class EquipeRepositorio : IEquipeRepositorio
    {
        public IEnumerable<Equipe> ObterEquipes()
        {
            using (SqlConnection con = new SqlConnection(Config.StringConexao()))
            {
                return con.Query<Equipe>(@"
                    SELECT
                        AUTONUM_EQP as Id,
                        NOME_EQP as Descricao
                    FROM
                        REDEX..TB_EQUIPE
                    WHERE
                        FLAG_ATIVO=1 AND FLAG_OPERADOR=1
                    ORDER BY
                        NOME_EQP");
            }
        }
    }
}