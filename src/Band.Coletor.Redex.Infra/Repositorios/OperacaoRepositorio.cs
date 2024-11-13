using Band.Coletor.Redex.Business.Interfaces.Repositorios;
using Band.Coletor.Redex.Business.Models;
using Band.Coletor.Redex.Infra.Configuracao;
using Dapper;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Runtime.Caching;

namespace Band.Coletor.Redex.Infra.Repositorios
{
    public class OperacaoRepositorio :BaseRepositorio<Operacao>, IOperacaoRepositorio
    {
        public OperacaoRepositorio(string connectionString) : base(connectionString)
        {
        }

        public IEnumerable<Operacao> ObterOperacoes()
        {
            MemoryCache cache = MemoryCache.Default;

            var operacoes = cache["Operacao.ObterOperacoes"] as IEnumerable<Operacao>;

            if (operacoes == null)
            {
                using (SqlConnection con = new SqlConnection(Config.StringConexao()))
                {
                    operacoes = con.Query<Operacao>(@"SELECT 'A' As Sigla, 'Automatizada' As Descricao UNION ALL SELECT 'M' As Sigla, 'Manual' As Descricao  UNION ALL SELECT 'P' As Sigla, 'Parcial' As Descricao ");
                }

                cache["Operacao.ObterOperacoes"] = operacoes;
            }

            return operacoes;
        }
    }
}
