using Band.Coletor.Redex.Business.DTO;
using Band.Coletor.Redex.Business.Extensions;
using Band.Coletor.Redex.Business.Interfaces.Repositorios;
using Band.Coletor.Redex.Business.Models;
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
    public class PatiosRepositorio: IPatiosRepositorio
    {
        public PatiosDTO GetPatioByID(int id)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" SELECT  ");
                    sb.AppendLine(" ISNULL(Flag_Truck_Mov_Coletor,0) as qual   ");
                    sb.AppendLine(" FROM  ");
                    sb.AppendLine(" REDEX.TB_PATIOS ");
                    sb.AppendLine(" WHERE ");
                    sb.AppendLine(" AUTONUM =  " + id);
                        
                    var query = _db.Query<PatiosDTO>(sb.ToString()).FirstOrDefault();

                    return query;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
