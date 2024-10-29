using Band.Coletor.Redex.Business.DTO;
using Band.Coletor.Redex.Business.Enums;
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
    public class SaidaCaminhaoRepositorio : ISaidaCaminhaoRepositorio
    {
        public void RegistrarSaida(int preRegistroId, LocalPatio localPatio)
        {
            using (SqlConnection con = new SqlConnection(Config.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "PreRegistroId", value: preRegistroId, direction: ParameterDirection.Input);

                if (localPatio == LocalPatio.Patio)
                {
                    con.Execute(@"
                    UPDATE
	                    REDEX..TB_PRE_REGISTRO
                        SET
                            DATA_SAIDA = GETDATE()
                        WHERE 
                            AUTONUM = @PreRegistroId", parametros);
                }

                if (localPatio == LocalPatio.Estacionamento)
                {
                    con.Execute(@"
                    UPDATE
	                    REDEX..TB_PRE_REGISTRO
                        SET                          
                            Saida_Deic_Patio = GetDate(),
                            LOCAL=2,
                            DATA_CHEGADA= GetDate()
                        WHERE 
                            AUTONUM = @PreRegistroId", parametros);
                }
            }
        }

        public RegistroSaidaCaminhaoDTO ObterDadosCaminhao(string protocolo, string ano, string placa, string placaCarreta , LocalPatio local)
        {
            using (SqlConnection con = new SqlConnection(Config.StringConexao()))
            {
                var parametros = new DynamicParameters();
                var where = " WHERE 1=1 ";


                if (local == LocalPatio.Patio)
                {
                    where = " WHERE  (PR.SAIDA_PATIO IS NULL AND PR.DATA_CHEGADA IS NOT NULL)  ";
                }

                if (local == LocalPatio.Patio)           
                   where = " WHERE  (PR.Saida_Deic_Patio IS NULL AND PR.DATA_CHEGADA_DEIC_PATIO IS NOT NULL)  ";

                if (!protocolo.IsNullOrEmptyOrWhiteSpace())
                {
                    var protocoloAno = $"{protocolo}/{ano}";
                    parametros.Add(name: "Protocolo", value: protocoloAno, direction: ParameterDirection.Input);
                    where += " AND PR.Protocolo = @Protocolo";

                }

                if (!placa.IsNullOrEmptyOrWhiteSpace())
                {
                    parametros.Add(name: "Placa", value: placa, direction: ParameterDirection.Input);
                    where += " AND PR.PLACA = @Placa";
                }

                if (!placaCarreta.IsNullOrEmptyOrWhiteSpace())
                {
                    parametros.Add(name: "Carreta", value: placaCarreta, direction: ParameterDirection.Input);
                    where += " AND PR.CARRETA = @Carreta ";
                }

                return con.Query<RegistroSaidaCaminhaoDTO>($@"
                    SELECT
                        PR.AUTONUM as PreRegistroId,
                        PR.PROTOCOLO,
	                    PR.PLACA as Placa,
	                    PR.CARRETA as PlacaCarreta,
                        PR.DATA_CHEGADA as DataChegada,
                        GN.FLAG_GATE_IN as GateIn,
                        GN.FLAG_GATE_OUT as GateOut,
                        GN.BRUTO AS PesoBruto, PR.TICKET
                    FROM 
                        REDEX..TB_PRE_REGISTRO PR
                    LEFT JOIN 
                        REDEX..TB_REGISTRO REG ON PR.AUTONUM_REG = REG.AUTONUM_REG
                    LEFT JOIN 
                        REDEX..TB_GATE_NEW GN ON REG.AUTONUM_GATE=GN.AUTONUM
                    {where}
                    ", parametros).FirstOrDefault();
            }
        }

        public RegistroSaidaCaminhaoDTO ObterDadosCaminhaoPorPreRegistroId(int preRegistroId)
        {
            using (SqlConnection con = new SqlConnection(Config.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "PreRegistroId", value: preRegistroId, direction: ParameterDirection.Input);

                return con.Query<RegistroSaidaCaminhaoDTO>($@"
                    SELECT
                        PR.AUTONUM as PreRegistroId,
                        PR.PROTOCOLO,
	                    PR.CAVALO as Placa,
	                    PR.CARRETA as PlacaCarreta,
                        PR.DATA_CHEGADA as DataChegada,
                        GN.FLAG_GATE_IN as GateIn,
                        GN.FLAG_GATE_OUT as GateOut,
                        GN.BRUTO AS PesoBruto, PR.TICKET
                    FROM 
                        REDEX..TB_PRE_REGISTRO PR
                    LEFT JOIN 
                        REDEX..TB_REGISTRO REG ON PR.AUTONUM_REG = REG.AUTONUM_REG
                    LEFT JOIN 
                        REDEX..TB_GATE_NEW GN ON REG.AUTONUM_GATE=GN.AUTONUM
                    WHERE
                        PR.AUTONUM = @PreRegistroId
                    ", parametros).FirstOrDefault();
            }
        }

        public Agendamento PendenciaDeSaida(string placa)
        {
            using (SqlConnection con = new SqlConnection(Config.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "Placa", value: placa, direction: ParameterDirection.Input);

                return con.Query<Agendamento>($@"
                    SELECT 
	                    TOP 1 
		                    DATA_CHEGADA_DEIC_PATIO As DataChegadaDeicPatio,
		                    Saida_Deic_Patio As DataSaidaDeicPatio,
		                    DATA_CHEGADA As DataChegadaPatio,
		                    Saida_Patio As DataSaidaPatio 
                    FROM 
	                    REDEX..TB_PRE_REGISTRO 
                    WHERE 
	                    PLACA = @Placa
                    AND 
	                    (SAIDA_DEIC_PATIO IS NULL AND DATA_CHEGADA_DEIC_PATIO IS NOT NULL) 
                    ORDER BY 
	                    AUTONUM DESC", parametros).FirstOrDefault();
            }
        }
       
        public Agendamento PendenciaSaida(string placa)
        {
            using (SqlConnection con = new SqlConnection(Config.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "Placa", value: placa, direction: ParameterDirection.Input);

                return con.Query<Agendamento>($@"
                    SELECT 
	                    PLACA
                    FROM 
	                    REDEX..TB_PRE_REGISTRO 
                    WHERE 
	                    PLACA = @Placa
                    AND 
	                    (SAIDA_PATIO IS NULL AND DATA_CHEGADA IS NOT NULL)", parametros).FirstOrDefault();
            }
        }
    }
}
