using Band.Coletor.Redex.Business.Interfaces.Repositorios;
using Band.Coletor.Redex.Business.Models;
using Band.Coletor.Redex.Infra.Configuracao;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Band.Coletor.Redex.Infra.Repositorios
{
    public class LacreRespositorio : ILacreRepositorio
    {
        public void AtualizarLacres(Lacre lacre)
        {
        }

        public void AtualizarLacres(int patio, string usuario, int validado)
        {
            using (SqlConnection con = new SqlConnection(Config.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "Patio", value: patio.ToString(), direction: ParameterDirection.Input);
                parametros.Add(name: "Data", value: DateTime.Now, direction: ParameterDirection.Input);
                parametros.Add(name: "Usuario", value: usuario, direction: ParameterDirection.Input);
                parametros.Add(name: "Validado", value: validado, direction: ParameterDirection.Input);

                int t = con.Execute(@"
                        UPDATE REDEX.TB_PATIO_LACRES
                            SET DATA = :Data,
                            USUARIO = :Usuario,
                            FLAG_VALIDADO = :Validado

                        WHERE
                              AUTONUM_PATIO = :Patio ", parametros);
            }
        }

        public IEnumerable<Lacre> BuscarLacresPorConteiner(string idConteiner)
        {
            IEnumerable<Lacre> lacres;

            using (SqlConnection con = new SqlConnection(Config.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "Conteiner", value: "%" + idConteiner.ToString() + "%", direction: ParameterDirection.Input);

                lacres = con.Query<Lacre>(@"
                        SELECT
                            P.ID_CONTEINER as IdConteiner,
                            PL.LACRE as LacreDescricao,
                            PL.AUTONUM_PATIO as Patio,
                            PL.DATA as Data,
                            PL.USUARIO as Usuario,
                            PL.FLAG_VALIDADO as Validado
                        FROM REDEX.TB_PATIO_LACRES PL
                            INNER JOIN REDEX.TB_PATIO P ON PL.AUTONUM_PATIO = P.AUTONUM_PATIO
                        WHERE
                              P.ID_CONTEINER like :Conteiner
                        ORDER BY PL.LACRE", parametros);
            }

            return lacres;
        }

        public IEnumerable<string> BuscarLacresPorPatio(int patio)
        {
            using (SqlConnection con = new SqlConnection(Config.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "Patio", value: patio.ToString(), direction: ParameterDirection.Input);

                return con.Query<string>(@"
                        SELECT
                            PL.LACRE as LacreDescricao
                        FROM REDEX.TB_PATIO_LACRES PL
                        WHERE
                              PL.AUTONUM_PATIO = :Patio ", parametros);
            }
        }
    }
}