using Band.Coletor.Redex.Business.Interfaces.Repositorios;
using Band.Coletor.Redex.Business.Models;
using Band.Coletor.Redex.Infra.Configuracao;
using Dapper;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Band.Coletor.Redex.Infra.Repositorios
{
    public class ConferenteRepositorio :BaseRepositorio<Conferente>, IConferenteRepositorio
    {
        public ConferenteRepositorio(string connectionString) : base(connectionString)
        {
        }

        public IEnumerable<Conferente> ObterConferentes(int idConferente)
        {
            using (var _db = new SqlConnection(Config.StringConexao()))
            {
                StringBuilder sb = new StringBuilder();

                sb.Clear();
                sb.AppendLine("SELECT AUTONUM_EQP as Id, NOME_EQP as Descricao ");
                sb.AppendLine(" from REDEX..TB_EQUIPE  WHERE "); 
                sb.AppendLine("FLAG_ATIVO=1 AND FLAG_CONFERENTE=1 and id_login = " + idConferente + " ");

              var query = _db.Query<Conferente>(sb.ToString()).AsEnumerable();

                return query;
            }
        }
    }
}