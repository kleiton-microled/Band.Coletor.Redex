using Band.Coletor.Redex.Business.Interfaces.Repositorios;
using Band.Coletor.Redex.Business.Models;
using Band.Coletor.Redex.Infra.Configuracao;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Caching;

namespace Band.Coletor.Redex.Infra.Repositorios
{
    public class ConteinerRepositorio : IConteinerRepositorio
    {
        public IEnumerable<Conteiner> ConsultarConteinerPorNumero(string idConteiner)
        {
            IEnumerable<Conteiner> conteiners;

            using (SqlConnection con = new SqlConnection(Config.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "Conteiner", value: "%" + idConteiner.ToString() + "%", direction: ParameterDirection.Input);

                conteiners = con.Query<Conteiner>(@"
                        SELECT
                            V.ID_CONTEINER AS IdConteiner,
                            V.TAMANHO,
                            V.STATUS,
                            V.RESERVA,
                            V.REFERENCIA,
                            V.CLIENTE,
                            V.ARMADOR,
                            V.DEADLINE,
                            V.LACRES,
                            V.LACRE_CONTROLE
                        FROM REDEX.VW_CONSULTA_CNTR_COLETOR V
                        WHERE
                             REPLACE(V.ID_CONTEINER, '-', '') LIKE :Conteiner", parametros);
            }

            return conteiners;
        }

        public IEnumerable<Conteiner> ConsultarConteinersReserva(string numeroReserva)
        {
            IEnumerable<Conteiner> conteiners;

            using (SqlConnection con = new SqlConnection(Config.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "Reserva", value: numeroReserva.ToString(), direction: ParameterDirection.Input);

                conteiners = con.Query<Conteiner>(@"
                        SELECT
                            V.ID_CONTEINER AS IdConteiner,
                            V.STATUS,
                            V.RESERVA,
                             V.REFERENCIA,
                            V.ENTRADA,
                            V.LACRES
                        FROM REDEX.VW_CONSULTA_CNTR_COLETOR V
                        WHERE
                              V.RESERVA = :Reserva
                        ORDER BY V.ID_CONTEINER", parametros);
            }

            return conteiners;
        }

        public Conteiner ConsultarResumoGeralReserva(string numeroReserva)
        {
            using (SqlConnection con = new SqlConnection(Config.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "Reserva", value: numeroReserva.ToString(), direction: ParameterDirection.Input);

                return con.Query<Conteiner>(@"
                    SELECT
                        SUM( CASE WHEN BCG.FLAG_CNTR=1 THEN 1 ELSE 0 END) TotalProgramado,
                        SUM(CASE WHEN T.FLAG_ESTUFAGEM = 0 AND P.FLAG_ENTRADA = 0  THEN 1 ELSE 0 END) ARECEBER,
                        SUM(P.FLAG_HISTORICO) AS ENTREGUES,
                        SUM(CASE WHEN T.FLAG_ESTUFAGEM = 1 AND P.FLAG_ENTRADA = 1  THEN 1 ELSE 0 END) AS ARMPATIO,
                        COUNT(T.FLAG_ESTUFAGEM) AS ESTUFADOS,
                        0 AS CCSEMMONITORAMENTO
                    FROM REDEX.TB_BOOKING BOO
                        INNER JOIN REDEX.TB_BOOKING_CARGA BCG ON BOO.AUTONUM_BOO=BCG.AUTONUM_BOO
                        INNER JOIN REDEX.TB_TALIE T ON BOO.AUTONUM_BOO = T.AUTONUM_BOO
                        INNER JOIN REDEX.TB_PATIO P ON T.AUTONUM_PATIO = P.AUTONUM_PATIO
                    WHERE
                        BOO.REFERENCE=:Reserva", parametros).FirstOrDefault();
            }
        }

        public string ObterConteinerPorId(int conteinerId)
        {
            using (SqlConnection con = new SqlConnection(Config.StringConexao()))
            {
                var parametros = new DynamicParameters();

                parametros.Add(name: "ConteinerId", value: conteinerId, direction: ParameterDirection.Input);

                return con.Query<string>(@"
                    SELECT
                       ID_CONTEINER
                    FROM
	                    REDEX..TB_PATIO 
                    WHERE
	                    autonum_patio = @ConteinerId", parametros).FirstOrDefault();

            }
        }

        public Conteiner ObterConteinerPorNumero(string idConteiner)
        {
            using (SqlConnection con = new SqlConnection(Config.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "Conteiner", value: "%" + idConteiner.ToString() + "%", direction: ParameterDirection.Input);

                return con.Query<Conteiner>(@"
                        SELECT
                            V.ID_CONTEINER AS IdConteiner,
                            V.AUTONUM_PATIO AS Patio,
                            V.TAMANHO,
                            V.STATUS,
                            V.RESERVA,
                            V.REFERENCIA,
                            V.CLIENTE,
                            V.ARMADOR,
                            V.DEADLINE,
                            V.LACRES,
                            V.LACRE_CONTROLE
                        FROM REDEX.VW_CONSULTA_CNTR_COLETOR V
	                    WHERE
                            V.ID_CONTEINER like :Conteiner", parametros).FirstOrDefault();
            }
        }

        public Conteiner ObterDetalhesConteiner(string idConteiner)
        {
            using (SqlConnection con = new SqlConnection(Config.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "Conteiner", value: "'%" + idConteiner.ToString() + "%'", direction: ParameterDirection.Input);

                var conteiner = "%" + idConteiner.ToString() + "%";

                return con.Query<Conteiner>(@"
                    SELECT
                            V.ID_CONTEINER AS IdConteiner,
                            V.TAMANHO,
                            V.STATUS,
                            V.RESERVA,
                            V.REFERENCIA,
                            V.CLIENTE,
                            V.ARMADOR,
                            V.DEADLINE,
                            V.LACRES,
                            V.LACRE_CONTROLE
                        FROM REDEX.VW_CONSULTA_CNTR_COLETOR V
	                        V.ID_CONTEINER like :conteiner", new { conteiner }).FirstOrDefault();
            }
        }

        IEnumerable<Conteiner> IConteinerRepositorio.ObterConteiners()
        {
            MemoryCache cache = MemoryCache.Default;

            var conteiners = cache["Conteiner.ObterConteiners"] as IEnumerable<Conteiner>;

            if (conteiners == null)
            {
                using (SqlConnection con = new SqlConnection(Config.StringConexao()))
                {
                    conteiners = con.Query<Conteiner>(@"
                        SELECT ROWNUM row_num,
                            P.ID_CONTEINER IdConteiner,
                            P.TAMANHO,
                            'STATUS' AS  Staus,
                            BOO.REFERENCE AS Booking,
                            'REFERENCIA' AS Referencia,
                            'CLIENTE' AS Cliente,
                            'ARMADOR' AS Armador,
                            V.DT_DEAD_LINE Deadline,
                            'LACRE' AS Lacre,
                            1 AS StatusLacre
                        FROM REDEX.TB_PATIO P
                            LEFT JOIN REDEX.TB_BOOKING_CARGA BC ON P.AUTONUM_BCG = BC.AUTONUM_BCG
                            LEFT JOIN REDEX.TB_BOOKING BOO  ON BC.AUTONUM_BOO = BOO.AUTONUM_BOO
                            LEFT JOIN REDEX.TB_VIAGENS V ON BOO.AUTONUM_VIA = V.AUTONUM_VIA
                    where  ROWNUM <50
                    ");
                }

                cache["Conteiner.ObterConteiners"] = conteiners;
            }

            return conteiners;
        }
    }
}