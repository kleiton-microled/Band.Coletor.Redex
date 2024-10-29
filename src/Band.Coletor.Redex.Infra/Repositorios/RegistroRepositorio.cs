using Band.Coletor.Redex.Business.Interfaces.Repositorios;
using Band.Coletor.Redex.Business.Models;
using Band.Coletor.Redex.Infra.Configuracao;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Band.Coletor.Redex.Infra.Repositorios
{
    public class RegistroRepositorio : IRegistroRepositorio
    {
        public Registro ObterRegistroPorLote(int lote)
        {
            using (SqlConnection con = new SqlConnection(Config.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "lote", value: lote, direction: ParameterDirection.Input);

                return con.Query<Registro>(@"
                    SELECT
                        A.AUTONUM AS GateId,
                        B.PLACA,
                        E.AUTONUM_EXPORTADOR as ExportadorId,
                        CP.RAZAO AS Exportador,
                        E.REFERENCE AS Referencia,
                        E.AUTONUM_BOO as Reserva,
                        B.AUTONUM_REG as Lote,
                        B.PATIO,
                        GETDATE() as Inicio
                    FROM
                        TB_GATE_NEW A
                    INNER JOIN
                        TB_REGISTRO B ON A.AUTONUM = B.AUTONUM_GATE
                    INNER JOIN
                        TB_BOOKING E ON B.AUTONUM_BOO = E.AUTONUM_BOO
                    INNER JOIN
                        TB_CAD_PARCEIROS CP ON E.AUTONUM_EXPORTADOR=CP.AUTONUM
                    WHERE
                        B.AUTONUM_REG= @lote
                    ORDER BY
                        A.AUTONUM DESC", parametros).FirstOrDefault();
            }
        }
    }
}