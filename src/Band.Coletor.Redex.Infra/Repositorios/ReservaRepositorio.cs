using Band.Coletor.Redex.Business.Interfaces.Repositorios;
using Band.Coletor.Redex.Business.Models;
using Band.Coletor.Redex.Infra.Configuracao;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Band.Coletor.Redex.Infra.Repositorios
{
    public class ReservaRepositorio : IReservaRepositorio
    {
        public Reserva ObterDadosReservaPorConteiner(string conteiner)
        {
            using (SqlConnection con = new SqlConnection(Config.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "conteiner", value: conteiner, direction: ParameterDirection.Input);

                return con.Query<Reserva>(@"
                    SELECT
	                    P.AUTONUM_PATIO as PatioId,
	                    P.AUTONUM_BCG as CargaId,
	                    BOO.AUTONUM_PARCEIRO as ParceiroId,
	                    BOO.AUTONUM_BOO as ReservaId
                    FROM
	                    TB_PATIO P
                    INNER JOIN
	                    TB_BOOKING_CARGA BCG ON P.AUTONUM_BCG = BCG.AUTONUM_BCG
                    INNER JOIN
	                    TB_BOOKING BOO ON BCG.AUTONUM_BOO = BOO.AUTONUM_BOO
                    WHERE
	                    ISNULL(P.FLAG_HISTORICO,0)=0 AND P.ID_CONTEINER=@conteiner", parametros).FirstOrDefault();
            }
        }

        public int ObterParceiroPorIdReserva(int idReserva)
        {
            using (SqlConnection con = new SqlConnection(Config.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "reserva", value: idReserva, direction: ParameterDirection.Input);

                return con.Query<int>(@"
                    SELECT
                        AUTONUM_PARCEIRO
                    FROM
                        TB_BOOKING
                    WHERE
                        AUTONUM_BOO=@reserva", parametros).FirstOrDefault();
            }
        }
    }
}