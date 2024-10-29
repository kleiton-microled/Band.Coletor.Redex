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


namespace Band.Coletor.Redex.Infra.Repositorios
{
    public class PreRegistroRepositorio : IPreRegistroRepositorio
    {
        public int Cadastrar(PreRegistro preRegistro)
        {
            using (SqlConnection con = new SqlConnection(Config.StringConexao()))
            {
                var parametros = new DynamicParameters();

                parametros.Add(name: "Protocolo", value: preRegistro.Protocolo, direction: ParameterDirection.Input);
                parametros.Add(name: "Placa", value: preRegistro.Placa, direction: ParameterDirection.Input);
                parametros.Add(name: "Carreta", value: preRegistro.PlacaCarreta, direction: ParameterDirection.Input);
                parametros.Add(name: "Ticket", value: preRegistro.Ticket, direction: ParameterDirection.Input);
                parametros.Add(name: "LocalPatio", value: preRegistro.LocalPatio, direction: ParameterDirection.Input);
                parametros.Add(name: "DataChegada", value: preRegistro.DataChegada, direction: ParameterDirection.Input);
                parametros.Add(name: "DataChegadaDeicPatio", value: preRegistro.DataChegadaDeicPatio, direction: ParameterDirection.Input);
                parametros.Add(name: "FlagDeicPatio", value: preRegistro.FlagDeicPatio.ToInt(), direction: ParameterDirection.Input);

                var temestacionamento = con.Query<Agendamento>($@"
                    SELECT 
	                    TOP 1 AUTONUM  AS AgendamentoId 
                    FROM 
	                    REDEX..TB_PRE_REGISTRO 
                    WHERE 
	                    PLACA = @Placa
                    AND 
	                    (DATA_CHEGADA IS NULL   AND SAIDA_DEIC_PATIO IS NOT  NULL AND DATA_CHEGADA_DEIC_PATIO IS NOT NULL) 
                    ORDER BY 
	                    AUTONUM DESC", parametros).FirstOrDefault();
                if (temestacionamento != null)
                {
                    parametros.Add(name: "ID", value: temestacionamento.AgendamentoId, direction: ParameterDirection.Input);

                    return con.Query<int>(@"
                    UPDATE 
	                    REDEX..TB_PRE_REGISTRO
                            SET DATA_CHEGADA=@DataChegada,
                                LOCAL=@LocalPatio where autonum=@id", parametros).FirstOrDefault();

                }
                else
                { 
                    return con.Query<int>(@"
                    INSERT INTO
	                    REDEX..TB_PRE_REGISTRO
                            (
		                        PROTOCOLO,
		                        PLACA,
		                        CARRETA,
                                Ticket,  
		                        DATA_CHEGADA,
                                LOCAL,
                                DATA_CHEGADA_DEIC_PATIO,
                                FLAG_DEIC_PATIO
                            ) VALUES (
                                @Protocolo,
                                @Placa,
                                @Carreta,
                                @Ticket,  
                                @DataChegada,
                                @LocalPatio,
                                @DataChegadaDeicPatio,
                                @FlagDeicPatio);  SELECT CAST(SCOPE_IDENTITY() AS INT)", parametros).FirstOrDefault();
                 }
            }
        }

        public Agendamento ObterDadosAgendamento( string protocolo, string ano, string placa, string placaCarreta, string sistema)
        {
            using (SqlConnection con = new SqlConnection(Config.StringConexao()))
            {
                var parametros = new DynamicParameters();
                var where = " WHERE  1=1 AND PERIODO_INICIAL >= DATEADD(d,-2,GETDATE()) ";

                //if (!protocolo.IsNullOrEmptyOrWhiteSpace()) {
                //    parametros.Add(name: "Ano", value: ano, direction: ParameterDirection.Input);
                //    parametros.Add(name: "Protocolo", value: protocolo, direction: ParameterDirection.Input);
                //    where += " AND AnoProtocolo = @Ano AND NumProtocolo = @Protocolo ";

                //}

                if (!placa.IsNullOrEmptyOrWhiteSpace())
                {
                    parametros.Add(name: "Placa", value: placa, direction: ParameterDirection.Input);
                    parametros.Add(name: "PlacaCarreta", value: placaCarreta, direction: ParameterDirection.Input);
                    where += " AND PLACA_CAVALO = @Placa AND PLACA_CARRETA = @PlacaCarreta ";
                }

                if (sistema == "R")
                {
                    return con.Query<Agendamento>($@"
                    SELECT
                        ID_AGENDAMENTO as AgendamentoId,
	                    PROTOCOLO,
	                    PERIODO,
	                    MOTORISTA,
	                    CNH,
	                    PLACA_CAVALO as Placa,
	                    PLACA_CARRETA as PlacaCarreta,
	                    PERIODO_INICIAL as PeriodoInicial,
	                    PERIODO_FINAL as PeriodoFinal,
	                    TIPO
                    FROM
                        REDEX..VW_AGENDAMENTOS_REDEX 
                    {where}
                    ", parametros).FirstOrDefault();
                }
                else
                //if (sistema == "R")
                {
                    return con.Query<Agendamento>($@"
               
                        SELECT   AUTONUM_AG_CNTR as AgendamentoId,
	                    PROTOCOLO,
	                    PERIODO,
	                    NOME_MOTORISTA MOTORISTA,
	                    CNH,
	                    PLACA_CAVALO as Placa,
	                    PLACA_CARRETA as PlacaCarreta,
	                    PERIODO_INICIAL as PeriodoInicial,
	                    PERIODO_FINAL as PeriodoFinal,
	                    ''  TIPO                      
                    FROM
                        SGIPA..VW_AGENDA_CNTR 
					 
  
                    {where}
                    ", parametros).FirstOrDefault();
                }
            }
        }

        public Agendamento GetDadosAgendamento(PreRegistro preRegistro)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    var sb = new StringBuilder();

                    if (preRegistro.Sistema == "R")
                    {
                        sb.AppendLine("  SELECT top 1 ");
                        sb.AppendLine("  ID_AGENDAMENTO as AgendamentoId, ");
                        sb.AppendLine("  PROTOCOLO as Protocolo,  ");
                        sb.AppendLine("  PERIODO as Periodo, ");
                        sb.AppendLine("  MOTORISTA as Motorista,  ");
                        sb.AppendLine("  CNH,  ");
                        sb.AppendLine("  PLACA_CAVALO as Placa, ");
                        sb.AppendLine("  PLACA_CARRETA as PlacaCarreta, ");
                        sb.AppendLine("  PERIODO_INICIAL as Periodo_Inicial,   ");
                        sb.AppendLine("  PERIODO_FINAL as Periodo_Final, ");
                        sb.AppendLine("  TIPO as Tipo ");
                        sb.AppendLine("  FROM  ");
                        sb.AppendLine("  REDEX..VW_AGENDAMENTOS_REDEX ");
                        sb.AppendLine("  WHERE  ");
                        sb.AppendLine("   1=1  ");
                        sb.AppendLine("  AND  ");
                        sb.AppendLine("  PERIODO_INICIAL >= DATEADD(d,-80,GETDATE()) ");

                        if (!string.IsNullOrEmpty(preRegistro.Placa))
                        {
                            sb.AppendLine(" AND  ");
                            sb.AppendLine(" PLACA_CAVALO = '" + preRegistro.Placa + "' "); 
                        }

                        if (!string.IsNullOrEmpty(preRegistro.PlacaCarreta))
                        {
                            sb.AppendLine(" AND  ");
                            sb.AppendLine(" PLACA_CARRETA = '" + preRegistro.PlacaCarreta + "' "); 
                        }


                        var query = _db.Query<Agendamento>(sb.ToString()).FirstOrDefault();

                        return query;
                    }
                    else
                    {
                        sb.Clear(); 
                        sb.AppendLine("  SELECT ");
                        sb.AppendLine("  AUTONUM_AG_CNTR as AgendamentoId, ");
                        sb.AppendLine("  PROTOCOLO,  ");
                        sb.AppendLine("  PERIODO, ");
                        sb.AppendLine("  NOME_MOTORISTA MOTORISTA, ");
                        sb.AppendLine("  CNH, ");
                        sb.AppendLine("  PLACA_CAVALO as Placa, ");
                        sb.AppendLine("  PLACA_CARRETA as PlacaCarreta, ");
                        sb.AppendLine("  PERIODO_INICIAL as Periodo_Inicial, ");
                        sb.AppendLine("  PERIODO_FINAL as Periodo_Final, ");
                        sb.AppendLine(" ''  TIPO ");
                        sb.AppendLine(" FROM ");
                        sb.AppendLine(" SGIPA..VW_AGENDA_CNTR ");
                        sb.AppendLine(" WHERE  "); 
                        sb.AppendLine("   1=1  ");
                        sb.AppendLine("  AND  ");

                        if (string.IsNullOrEmpty(preRegistro.Placa))
                        {
                            sb.AppendLine(" AND  ");
                            sb.AppendLine(" PLACA_CAVALO = '" + preRegistro.Placa + "' ");
                        }

                        if (!string.IsNullOrEmpty(preRegistro.PlacaCarreta))
                        {
                            sb.AppendLine(" AND  ");
                            sb.AppendLine(" PLACA_CARRETA = '" + preRegistro.PlacaCarreta + "' ");
                        }


                        var query = _db.Query<Agendamento>(sb.ToString()).FirstOrDefault();

                        return query;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Agendamento PendenciaDeSaida(string placa)
        {
            //
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
		                    Saida_Patio As DataSaidaPatio , Ticket
                    FROM 
	                    REDEX..TB_PRE_REGISTRO 
                    WHERE 
	                    PLACA = @Placa
                    AND 
	                    (SAIDA_DEIC_PATIO IS NULL OR DATA_SAIDA IS NULL) 
                    ORDER BY 
	                    AUTONUM DESC", parametros).FirstOrDefault();
            }
        }

        public Agendamento PendenciaDeSaidaEstacionamento(string placa)
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
		                    Saida_Patio As DataSaidaPatio ,Ticket
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
        public Agendamento TemEstacionamento(string placa)
        {
            using (SqlConnection con = new SqlConnection(Config.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "Placa", value: placa, direction: ParameterDirection.Input);

                return con.Query<Agendamento>($@"
                    SELECT 
	                    TOP 1 AUTONUM,
		                    DATA_CHEGADA_DEIC_PATIO As DataChegadaDeicPatio,
		                    Saida_Deic_Patio As DataSaidaDeicPatio,
		                    DATA_CHEGADA As DataChegadaPatio,
		                    Saida_Patio As DataSaidaPatio ,Ticket
                    FROM 
	                    REDEX..TB_PRE_REGISTRO 
                    WHERE 
	                    PLACA = @Placa
                    AND 
	                    (DATA_CHEGADA IS NULL ULL AND SAIDA_DEIC_PATIO IS NOT  NULL AND DATA_CHEGADA_DEIC_PATIO IS NOT NULL) 
                    ORDER BY 
	                    AUTONUM DESC", parametros).FirstOrDefault();
            }
        }
        public Agendamento PendenciaDeSaidaPatio(string placa)
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
		                    Saida_Patio As DataSaidaPatio ,Ticket
                    FROM 
	                    REDEX..TB_PRE_REGISTRO 
                    WHERE 
	                    PLACA = @Placa
                    AND 
	                    (Data_Saida IS NULL AND DATA_CHEGADA IS NOT NULL) 
                    ORDER BY 
	                    AUTONUM DESC", parametros).FirstOrDefault();
            }
        }

        public int PendenciaEntrada(string placa)
        {
            using (SqlConnection con = new SqlConnection(Config.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "Placa", value: placa, direction: ParameterDirection.Input);

                return con.Query<int>($@"
                    SELECT 
	                    ISNULL(MAX(AUTONUM),0) AUTONUM
                    FROM 
	                    REDEX..TB_PRE_REGISTRO 
                    WHERE 
	                    PLACA = @Placa
                    AND 
	                    DATA_CHEGADA_DEIC_PATIO > GETDATE() - 2
                    AND 
	                    (SAIDA_DEIC_PATIO IS NOT NULL AND SAIDA_PATIO IS NULL)", parametros).Single();
            }
        }

        public void AtualizarDataChegada(int id)
        {
            using (SqlConnection con = new SqlConnection(Config.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "Id", value: id, direction: ParameterDirection.Input);

                con.Execute($@"UPDATE REDEX..TB_PRE_REGISTRO SET DATA_CHEGADA = GetDate(), Local = 1, FLAG_DEIC_PATIO = 0 WHERE AUTONUM = @Id", parametros);
            }
        }
        public IEnumerable<PreRegistro> GetDadosPatio()
        {
            try
            {
                using (var _Db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.Append(" select AUTONUM as PatioDestinoId, case when autonum =7 then 'IPA' ELSE DESCR END as DescPatioDestino from OPERADOR..tb_patios where AUTONUM in (7, 2, 3) ");

                    var query = _Db.Query<PreRegistro>(sb.ToString());

                    return query;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public int GetPendenciasSaidaPlaca(string placa)
        {
            int count = 0;

            try
            {
                using (var _Db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.Append(" SELECT  ");
                    sb.Append(" count(1)  ");
                    sb.Append(" FROM  ");
                    sb.Append(" REDEX..TB_PRE_REGISTRO  ");
                    sb.Append(" WHERE  ");
                    sb.Append(" PLACA = '"+  placa + "'   ");
                    sb.Append(" AND  ");
                    sb.Append(" (Data_Saida IS NULL AND DATA_CHEGADA IS NOT NULL)  ");                    

                    count = _Db.Query<int>(sb.ToString()).FirstOrDefault();

                    return count;
                }
            }
            catch (Exception ex)
            {
                return count;
            }
        }
    }
}