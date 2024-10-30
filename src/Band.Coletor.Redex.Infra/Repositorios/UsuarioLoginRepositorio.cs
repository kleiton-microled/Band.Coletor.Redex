using Band.Coletor.Redex.Business.Interfaces.Repositorios;
using Band.Coletor.Redex.Business.Models;
using Band.Coletor.Redex.Infra.Configuracao;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Band.Coletor.Redex.Infra.Repositorios
{
    public class UsuarioLoginRepositorio : IUsuarioLoginRepositorio
    {
        public UsuarioLogin Busca(string login, string senha)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Config.StringConexao()))
                {
                    var parametros = new DynamicParameters();
                    parametros.Add(name: "Login", value: login, direction: ParameterDirection.Input);
                    parametros.Add(name: "Senha", value: senha, direction: ParameterDirection.Input);
                    string query = @"
                    SELECT
                        A.AUTONUM_USU As Id,
                        A.USUARIO As Login,
                        A.SENHA,
                        A.NOMECOMPLETO As Nome,
                        A.FLAG_ATIVO As ATIVO,
                        B.AUTONUM As PatioColetorId
                    FROM
                        REDEX..TB_CAD_USUARIOS A
                    LEFT JOIN
                        REDEX..TB_EMPRESAS B ON A.COD_EMPRESA = B.AUTONUM
                    WHERE
	                     USUARIO = @Login AND SENHA = @Senha";

                var usuario = con.Query<UsuarioLogin>(query, param: parametros).FirstOrDefault();
                    return usuario;
                }
            }
            catch (System.Exception ex)
            {

                throw;
            }
           
        }
    }
}