using Dapper;
using Ecoporto.Coletor.Service.Enums;
using Ecoporto.Coletor.Service.Models;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;

namespace Ecoporto.Coletor.Service.DAO
{
    public class UploadDAO
    {
        private static readonly string StringConexao
            = ConfigurationManager.ConnectionStrings["StringConexaoOracle"].ConnectionString;

        public int Cadastrar(UploadRequest upload)
        {
            using (OracleConnection con = new OracleConnection(StringConexao))
            {
                var parametros = new DynamicParameters();

                parametros.Add(name: "ProcessoId", value: upload.ProcessoId, direction: ParameterDirection.Input);
                parametros.Add(name: "Arquivo", value: upload.Arquivo, direction: ParameterDirection.Input);
                parametros.Add(name: "TipoArquivo", value: upload.TipoArquivo, direction: ParameterDirection.Input);
                parametros.Add(name: "UsuarioId", value: upload.UsuarioId, direction: ParameterDirection.Input);

                parametros.Add(name: "Id", dbType: DbType.Int32, direction: ParameterDirection.Output);

                con.Execute(@"
                    INSERT INTO
	                    REDEX.TB_COLETOR_FOTOS
                            (
		                        AUTONUM,
		                        AUTONUM_PROCESSO,
		                        ARQUIVO,
		                        TIPO,
		                        USUARIO
                            ) VALUES (
                                REDEX.SEQ_COLETOR_FOTOS.NEXTVAL,
                                :ProcessoId,
                                :Arquivo,
                                :TipoArquivo,
                                :UsuarioId) RETURNING AUTONUM INTO :Id", parametros);

                return parametros.Get<int>("Id");
            }
        }

        public void Excluir(int id)
        {
            using (OracleConnection con = new OracleConnection(StringConexao))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "Id", value: id, direction: ParameterDirection.Input);

                con.Execute(@"DELETE FROM REDEX.TB_COLETOR_FOTOS WHERE AUTONUM = :Id", parametros);
            }
        }

        public UploadResult ObterImagemPorId(int id)
        {
            using (OracleConnection con = new OracleConnection(StringConexao))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "Id", value: id, direction: ParameterDirection.Input);

                return con.Query<UploadResult>(@"SELECT ARQUIVO, DT_INCLUSAO As DataInclusao FROM REDEX.TB_COLETOR_FOTOS WHERE AUTONUM = :Id", parametros).FirstOrDefault();
            }
        }

        public IEnumerable<UploadResult> ObterImagens(int processoId, TipoArquivo tipoArquivo)
        {
            using (OracleConnection con = new OracleConnection(StringConexao))
            {
                var parametros = new DynamicParameters();

                parametros.Add(name: "ProcessoId", value: processoId, direction: ParameterDirection.Input);
                parametros.Add(name: "TipoArquivo", value: tipoArquivo, direction: ParameterDirection.Input);

                return con.Query<UploadResult>(@"
                    SELECT 
                        AUTONUM As Id,
                        AUTONUM_PROCESSO As ProcessoId,
                        ARQUIVO, 
                        TIPO, 
                        DT_INCLUSAO As DataInclusao 
                    FROM 
                        REDEX.TB_COLETOR_FOTOS 
                    WHERE 
                        AUTONUM_PROCESSO = :ProcessoId 
                    AND 
                        Tipo = :TipoArquivo
                    ORDER BY
                        DT_INCLUSAO DESC", parametros);
            }
        }
    }
}