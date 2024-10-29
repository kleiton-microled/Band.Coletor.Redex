using Band.Coletor.Redex.Business.DTO;
using Band.Coletor.Redex.Business.Interfaces.Repositorios;
using Band.Coletor.Redex.Infra.Configuracao;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Band.Coletor.Redex.Infra.Repositorios
{
    public class ArmazensRepositorio : IArmazensRepositorio
    {
        public IEnumerable<ArmazensDTO> GetComboArmazens(int patio)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    var sb = new StringBuilder();

                    sb.AppendLine(" select Autonum as autonum, Descr AS DISPLAY ");
                    sb.AppendLine(" FROM SGIPA..TB_armazens_ipa ");
                    sb.AppendLine(" WHERE dt_saida is null and flag_historico=0 and ");
                    sb.AppendLine(" patio = " + patio);
                    sb.AppendLine(" ORDER BY DESCR ");

                    string sql = sb.ToString();


                    var query = _db.Query<ArmazensDTO>(sql).AsEnumerable();


                    return query;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}