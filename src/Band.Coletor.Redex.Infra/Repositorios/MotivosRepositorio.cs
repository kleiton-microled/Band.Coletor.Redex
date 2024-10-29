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
    public class MotivosRepositorio : IMotivosRepositorio
    {
        public IDbConnection _db;
        public StringBuilder sb;
        public IEnumerable<MotivosDTO> GetComboMotivos()
        {
            try
            {
                using (_db = new SqlConnection(Config.StringConexao()))
                {
                    sb = new StringBuilder();

                    sb.AppendLine(" select Autonum as AUTONUM, Descricao AS DESCRICAO  ");
                    sb.AppendLine(" FROM operador..tb_cad_motivo where ISNULL(flag_coletor,0) = 1 ");
                    sb.AppendLine(" ORDER BY DESCRICAO ");

                    string sql = sb.ToString();

                    var query = _db.Query<MotivosDTO>(sql.ToString()).AsEnumerable();


                    return query;
                }
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
        public IEnumerable<MotivosDTO> GetComboMotivosUtilitarios(string placa)
        {
            try
            {
                using (_db = new SqlConnection(Config.StringConexao()))
                {
                    sb = new StringBuilder();


                    if (!string.IsNullOrEmpty(placa))
                    {
                        sb.AppendLine(" select count(*) from REDEX..tb_frota where placa_cavalo = '" + placa + "' and flag_ativo = 1");

                        int count = _db.Query<int>(sb.ToString()).FirstOrDefault();

                        sb.Clear();

                        if (count > 0)
                        {
                            sb.AppendLine(" select autonum as AUTONUM, descricao as DESCRICAO from tb_motivo_utilitario where FLAG_FROTA=1 ");
                        }
                        else
                        {
                            sb.AppendLine(" select autonum as AUTONUM, descricao as DESCRICAO from tb_motivo_utilitario where isnull(FLAG_FROTA, 0) = 0  ");
                        }

                        sb.AppendLine(" order by descricao ");
                    }
                    else
                    {
                        sb.Clear();

                        sb.AppendLine(" select 0 as autonum as AUTONUM, 'Selecione um motivo' as descricao as DESCRICAO");
                    }

                    var query = _db.Query<MotivosDTO>(sb.ToString()).AsEnumerable();

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

