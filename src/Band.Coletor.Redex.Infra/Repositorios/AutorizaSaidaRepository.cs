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
    public class AutorizaSaidaRepository : IAutorizaSaidaRepository
    {
        IDbConnection _db;

        public int Verifica_Saida(string cntr)
        {
            try
            {
                using (_db = new SqlConnection(Config.StringConexao()))
                {

                    var sb = new StringBuilder();

                    sb.AppendLine(" SELECT count(1) from REDEX.DBO.VW_AUTORIZA_SAIDA  ");

                    if (!string.IsNullOrEmpty(cntr))
                    {
                        sb.AppendLine(" WHERE CARGA LIKE '%" + cntr + "%'");
                    }

                    int count = _db.Query<int>(sb.ToString()).FirstOrDefault();



                    return count;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int GetAutonumGate(string placa)
        {
            try
            {
                using (_db = new SqlConnection(Config.StringConexao()))
                {
                    var sb = new StringBuilder();

                    sb.AppendLine(" SELECT ");
                    sb.AppendLine(" isnull(max(AUTONUM),0) as AUTONUM  ");
                    sb.AppendLine(" FROM  ");
                    sb.AppendLine(" REDEX..TB_GATE_NEW ");
                    sb.AppendLine(" WHERE  ");
                    sb.AppendLine(" FLAG_GATE_IN = 1 ");
                    sb.AppendLine(" AND  ");
                    sb.AppendLine(" iSNULL(FLAG_GATE_OUT, 0) = 0 ");
                    //sb.AppendLine(" AND  ");
                    //sb.AppendLine(" FUNCAO_GATE_ENTRADA = 201 ");
                    sb.AppendLine(" AND ");
                    sb.AppendLine(" PLACA = '" + placa + "' ");

                    int id = _db.Query<int>(sb.ToString()).FirstOrDefault();

                    return id;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int GetIDFuncaoGateIN(string placa)
        {
            try
            {
                using (_db = new SqlConnection(Config.StringConexao()))
                {
                    var sb = new StringBuilder();

                    sb.AppendLine(" SELECT ");
                    sb.AppendLine(" FUNCAO_GATE_ENTRADA  ");
                    sb.AppendLine(" FROM  ");
                    sb.AppendLine(" REDEX..TB_GATE_NEW ");
                    sb.AppendLine(" WHERE  ");
                    sb.AppendLine(" PLACA = '" + placa + "' and isnull(flag_gate_out,0)=0");

                    int id = _db.Query<int>(sb.ToString()).FirstOrDefault();

                    return id;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IEnumerable<AutorizaSaidaDTO> GetDadosVerificaSaida(int gate)
        {
            try
            {
                using (_db = new SqlConnection(Config.StringConexao()))
                {
                    var sb = new StringBuilder();

                    sb.AppendLine(" SELECT  ");
                    sb.AppendLine(" qtde_saida ");
                    sb.AppendLine(" ,carga ");
                    sb.AppendLine(" ,imp_exp ");
                    sb.AppendLine(" ,FE ");
                    sb.AppendLine(" ,REFERENCE ");
                    sb.AppendLine(" ,fantasia ");
                    sb.AppendLine(" ,autonum_ro ");
                    sb.AppendLine(" ,autonum_reg ");
                    sb.AppendLine(" ,autonum_talie ");
                    sb.AppendLine(" ,autonum_patio ");
                    sb.AppendLine(" ,autonum_boo ");
                    sb.AppendLine(" ,carga ");
                    sb.AppendLine(" FROM  ");
                    sb.AppendLine(" VW_AUTORIZA_SAIDA_COLETOR ");
                    sb.AppendLine(" where ");
                    sb.AppendLine(" AUTONUM_GATE_SAIDA =  " + gate);

                    var query = _db.Query<AutorizaSaidaDTO>(sb.ToString()).AsEnumerable();

                    return query;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public AutorizaSaidaDTO GetDadosVerificaSaidaFD(string cntr)
        {
            try
            {
                using (_db = new SqlConnection(Config.StringConexao()))
                {
                    var sb = new StringBuilder();

                    sb.AppendLine(" SELECT  ");
                    sb.AppendLine(" qtde_saida ");
                    sb.AppendLine(" ,carga ");
                    sb.AppendLine(" ,imp_exp ");
                    sb.AppendLine(" ,FE ");
                    sb.AppendLine(" ,REFERENCE ");
                    sb.AppendLine(" ,fantasia ");
                    sb.AppendLine(" ,autonum_ro ");
                    sb.AppendLine(" ,autonum_reg ");
                    sb.AppendLine(" ,autonum_talie ");
                    sb.AppendLine(" ,autonum_patio ");
                    sb.AppendLine(" ,carga ");
                    sb.AppendLine(" FROM  ");
                    sb.AppendLine(" VW_AUTORIZA_SAIDA ");
                    sb.AppendLine(" where ");
                    sb.AppendLine(" carga like  '%" + cntr + "%'");

                    var query = _db.Query<AutorizaSaidaDTO>(sb.ToString()).FirstOrDefault();

                    return query;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public AutorizaSaidaDTO GetDadosRomaneioById(int id)
        {
            try
            {
                using (_db = new SqlConnection(Config.StringConexao()))
                {
                    var sb = new StringBuilder();
                    sb.AppendLine(" SELECT  ");
                    sb.AppendLine(" autonum_ro  ");
                    sb.AppendLine(" ,autonum_patio ");
                    sb.AppendLine(" FROM  ");
                    sb.AppendLine(" REDEX..TB_ROMANEIO ");
                    sb.AppendLine(" WHERE  ");
                    sb.AppendLine(" autonum_ro =  " + id);

                    var query = _db.Query<AutorizaSaidaDTO>(sb.ToString()).FirstOrDefault();

                    return query;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public AutorizaSaidaDTO GetAutonumReg(int id)
        {
            try
            {
                using (_db = new SqlConnection(Config.StringConexao()))
                {
                    var sb = new StringBuilder();

                    sb.AppendLine(" SELECT  ");
                    sb.AppendLine(" autonum_reg ");
                    sb.AppendLine(" FROM  ");
                    sb.AppendLine(" REDEX..tb_registro  ");
                    sb.AppendLine(" WHERE  ");
                    sb.AppendLine(" autonum_ro  = " + id);

                    var query = _db.Query<AutorizaSaidaDTO>(sb.ToString()).FirstOrDefault();

                    return query;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdatePatioGateById(int gate, int autonum_patio)
        {
            try
            {
                using (_db = new SqlConnection(Config.StringConexao()))
                {
                    var sb = new StringBuilder();

                    sb.AppendLine(" UPDATE REDEX..TB_PATIO SET   ");
                    sb.AppendLine(" FLAG_SAIDA = 1 ");
                    sb.AppendLine(" ,DT_CARREGA = GETDATE() ");
                    sb.AppendLine(" ,AUTONUM_GATE_SAIDA =  " + gate);
                    sb.AppendLine(" WHERE ");
                    sb.AppendLine(" AUTONUM_PATIO =  " + autonum_patio);

                    _db.Query<object>(sb.ToString()).FirstOrDefault();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateSaidaCargaByRomaneioId(int id)
        {
            try
            {
                using (_db = new SqlConnection(Config.StringConexao()))
                {
                    var sb = new StringBuilder();

                    sb.AppendLine(" UPDATE REDEX..TB_SAIDA_CARGA SET  ");
                    sb.AppendLine(" FLAG_SAIDA = 1 ");
                    sb.AppendLine(" WHERE  ");
                    sb.AppendLine(" AUTONUM_RO = " + id);


                    _db.Query<object>(sb.ToString()).FirstOrDefault();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateRomaneioById(int id)
        {
            try
            {
                using (_db = new SqlConnection(Config.StringConexao()))
                {
                    var sb = new StringBuilder();

                    sb.AppendLine(" UPDATE REDEX..TB_ROMANEIO set  ");
                    sb.AppendLine(" FLAG_HISTORICO = 1 ");
                    sb.AppendLine(" WHERE  ");
                    sb.AppendLine(" AUTONUM_RO =  " + id);


                    _db.Query<object>(sb.ToString()).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public GateDTO GetDadosGateNewById(int id)
        {
            try
            {
                using (_db = new SqlConnection(Config.StringConexao()))
                {
                    var sb = new StringBuilder();

                    sb.AppendLine(" SELECT ");
                    sb.AppendLine(" AUTONUM ");
                    sb.AppendLine(" ,PLACA  ");
                    sb.AppendLine(" ,CARRETA ");
                    sb.AppendLine(" ,Id_Motorista ");
                    sb.AppendLine(" ,ID_TRANSPORTADORA ");
                    sb.AppendLine(" ,ISNULL(TARA,0) AS TARA ");
                    sb.AppendLine(" ,funcao_gate_entrada ");
                    sb.AppendLine(" ,ISNULL(BRUTO, 0) as BRUTO ");
                    sb.AppendLine(" ,DT_GATE_IN  ");
                    sb.AppendLine(" FROM  ");
                    sb.AppendLine(" REDEX..TB_GATE_NEW ");
                    sb.AppendLine(" WHERE  ");
                    sb.AppendLine(" AUTONUM = " + id);

                    var query = _db.Query<GateDTO>(sb.ToString()).FirstOrDefault();

                    return query;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateDadosTbRegistro(GateDTO gateDTO)
        {
            try
            {
                using (_db = new SqlConnection(Config.StringConexao()))
                {
                    var sb = new StringBuilder();
                    sb.AppendLine(" UPDATE REDEX..TB_REGISTRO SET  ");
                    sb.AppendLine(" PLACA =  '" + gateDTO.PLACA + "' ");
                    sb.AppendLine(" ,CARRETA =  '" + gateDTO.CARRETA + "' ");
                    sb.AppendLine(" ,AUTONUM_MOT = " + gateDTO.Id_Motorista);
                    sb.AppendLine(" ,AUTONUM_TRANSP = " + gateDTO.ID_TRANSPORTADORA);
                    sb.AppendLine(" ,AUTONUM_GATE =  " + gateDTO.AUTONUM_GATE);
                    sb.AppendLine(" ,TARA_VEICULO = " + gateDTO.TARA);
                    sb.AppendLine(" WHERE  ");
                    sb.AppendLine(" AUTONUM_REG = " + gateDTO.AUTONUM_REG);


                    _db.Query<object>(sb.ToString()).FirstOrDefault();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IEnumerable<GateDTO> GetDadosPatioBooCntrByIdGate(int id)
        {
            try
            {
                using (_db = new SqlConnection(Config.StringConexao()))
                {
                    var sb = new StringBuilder();

                    sb.AppendLine(" SELECT  ");
                    sb.AppendLine(" bcg.autonum_boo  ");
                    sb.AppendLine(" ,CC.autonum_patio ");
                    sb.AppendLine(" ,isnull(tara, 0) AS TARA ");
                    sb.AppendLine(" ,cc.id_conteiner  ");
                    sb.AppendLine(" from  ");
                    sb.AppendLine(" REDEX..TB_PATIO CC ");
                    sb.AppendLine(" INNER JOIN REDEX..TB_BOOKING_carga bcg on cc.autonum_bcg = bcg.autonum_bcg ");
                    sb.AppendLine(" LEFT JOIN REDEX..TB_DADOS_CONTEINER dc on cc.id_conteiner = dc.id_conteiner ");
                    sb.AppendLine(" WHERE  ");
                    sb.AppendLine(" CC.autonum_gate_saida = " + id);

                    var query = _db.Query<GateDTO>(sb.ToString()).AsEnumerable();

                    return query;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateDadosTbGateNew(GateDTO gateDTO)
        {
            try
            {
                using (_db = new SqlConnection(Config.StringConexao()))
                {
                    var sb = new StringBuilder();

                    sb.Clear();
                    sb.AppendLine(" UPDATE REDEX..TB_GATE_NEW SET  ");
                    sb.AppendLine(" DT_GATE_OUT =  CONVERT(DATETIME,'" + gateDTO.DT_GATE_OUT + "',103) ");
                    sb.AppendLine(" ,BRUTO =  " + gateDTO.BRUTO);
                    sb.AppendLine(" ,FLAG_GATE_OUT = 1 ");
                    sb.AppendLine(" ,USUARIO_GATE_OUT =  " + gateDTO.UsuarioId);
                    sb.AppendLine(" ,FUNCAO_GATE_SAIDA = 204 ");
                    sb.AppendLine(" ,TICKET_SAIDA = 0 ");
                    sb.AppendLine(" ,GATE_MANUAL = 0 ");
                    sb.AppendLine(" WHERE  ");
                    sb.AppendLine(" AUTONUM =  " + gateDTO.AUTONUM);

                    _db.Query<object>(sb.ToString()).FirstOrDefault();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public long Amr_Gate_RDX(long Gate, long Cntr_Rdx, long Peso_Entrada, long Peso_Saida, string Data, long Booking, long IdReg, string Funcao_Gate, byte Historico)
        {
            try
            {
                using (_db = new SqlConnection(Config.StringConexao()))
                {

                    var sb0 = new StringBuilder();

                    sb0.AppendLine(" INSERT INTO REDEX..SEQ_TB_AMR_GATE  ");
                    sb0.AppendLine(" (DATA) values (GETDATE())  SELECT CAST(SCOPE_IDENTITY() AS bigint)  ");

                    long id = _db.Query<long>(sb0.ToString()).FirstOrDefault();

                    var sb = new StringBuilder();

                    DateTime dt = Convert.ToDateTime(Data);

                    sb.AppendLine(" INSERT INTO REDEX..TB_AMR_GATE  ");
                    sb.AppendLine(" ( ");
                    sb.AppendLine(" autonum ");
                    sb.AppendLine(" ,gate");
                    sb.AppendLine(" ,cntr_rdx");
                    sb.AppendLine(" ,peso_entrada");
                    sb.AppendLine(" ,peso_saida");
                    sb.AppendLine(" ,data");
                    sb.AppendLine(" ,id_booking");
                    sb.AppendLine(" ,funcao_gate ");
                    sb.AppendLine(" ,flag_historico ");
                    sb.AppendLine(" ) VALUES ( ");
                    sb.AppendLine(" " + id);
                    sb.AppendLine(" ," + Gate);
                    sb.AppendLine(" ," + Cntr_Rdx);
                    sb.AppendLine(" ," + Peso_Entrada);
                    sb.AppendLine(" ," + Peso_Saida);
                    sb.AppendLine(" ,convert(datetime,'" + Data + "',103)");
                    sb.AppendLine(" ," + Booking);
                    sb.AppendLine(" ," + Funcao_Gate);
                    sb.AppendLine(" ," + Historico);
                    sb.AppendLine(" ) ");

                    _db.Query<object>(sb.ToString()).FirstOrDefault();


                    return id;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateTbPatio207(GateDTO gateDTO)
        {
            try
            {
                using (_db = new SqlConnection(Config.StringConexao()))
                {
                    var sb = new StringBuilder();

                    sb.AppendLine(" UPDATE REDEX..TB_PATIO  SET ");
                    sb.AppendLine(" EF_SAIDA = '" + gateDTO.EF_SAIDA + "' ");
                    sb.AppendLine(" ,DT_SAIDA = '" + gateDTO.DATE.ToString("yyyy/MM/dd HH:mm:ss") + "' ");
                    sb.AppendLine(" ,FLAG_HISTORICO = 1 ");
                    sb.AppendLine(" WHERE  ");
                    sb.AppendLine(" AUTONUM_PATIO = " + gateDTO.AUTONUM_PATIO);

                    _db.Query<object>(sb.ToString()).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateTbPatio(GateDTO gateDTO)
        {
            try
            {
                using (_db = new SqlConnection(Config.StringConexao()))
                {
                    var sb = new StringBuilder();

                    sb.AppendLine(" UPDATE REDEX..TB_PATIO  SET ");
                    sb.AppendLine(" EF_SAIDA = '" + gateDTO.EF_SAIDA + "' ");
                    sb.AppendLine(" ,DT_SAIDA = '" + gateDTO.DATE.ToString("yyyy/MM/dd HH:mm:ss") + "' ");
                    sb.AppendLine(" ,FLAG_HISTORICO = 1 ");
                    sb.AppendLine(" ,AUTONUM_GATE_SAIDA =  " + gateDTO.AUTONUM);
                    sb.AppendLine(" WHERE  ");
                    sb.AppendLine(" AUTONUM_PATIO = " + gateDTO.AUTONUM_PATIO);

                    _db.Query<object>(sb.ToString()).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateTbRegistroByPlaca(string placa)
        {
            try
            {
                using (_db = new SqlConnection(Config.StringConexao()))
                {
                    var sb = new StringBuilder();

                    sb.AppendLine(" UPDATE REDEX..TB_REGISTRO SET ");
                    sb.AppendLine("  SAIDA_REDEX = GETDATE()  ");
                    sb.AppendLine(" WHERE  ");
                    sb.AppendLine(" PLACA  = '" + placa + "' ");
                    sb.AppendLine(" ENTRADA_REDEX is NOT NULL  ");
                    sb.AppendLine(" AND   ");
                    sb.AppendLine(" SAIDA_REDEX is null ");

                    _db.Query<object>(sb.ToString()).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateTbRegistroByConteiner(string op, string unidades, string placa)
        {
            try
            {
                using (_db = new SqlConnection(Config.StringConexao()))
                {
                    var sb = new StringBuilder();


                    sb.AppendLine(" UPDATE REDEX..TB_REGISTRO SET ");
                    sb.AppendLine("  CONTEINER_SAIDA = '" + op + "  " + unidades + "'");
                    sb.AppendLine(" WHERE  ");
                    sb.AppendLine(" PLACA  = '" + placa + "' ");
                    sb.AppendLine(" DATA_SAIDA is NOT NULL  ");


                    _db.Query<object>(sb.ToString()).FirstOrDefault();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IEnumerable<GateDTO> GetDadosPatioBooByIdGate(int id)
        {
            try
            {
                using (_db = new SqlConnection(Config.StringConexao()))
                {
                    var sb = new StringBuilder();
                    sb.AppendLine(" SELECT  ");
                    sb.AppendLine(" BCG.AUTONUM_BOO ");
                    sb.AppendLine(" ,CC.AUTONUM_PATIO ");
                    sb.AppendLine(" ,CC.id_conteier ");
                    sb.AppendLine(" FROM  ");
                    sb.AppendLine(" REDEX..TB_PATIO CC  ");
                    sb.AppendLine(" INNER JOIN REDEX..TB_BOOKING_CARGA BCG  ");
                    sb.AppendLine(" ON CC.AUTONUM_BCG = BCG.AUTONUM_BCG ");
                    sb.AppendLine(" WHERE  ");
                    sb.AppendLine(" CC.AUTONUM_GATE_SAIDA =  " + id);

                    var query = _db.Query<GateDTO>(sb.ToString()).AsEnumerable();

                    return query;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public decimal GetPesoTaraByGateNew(int id)
        {
            try
            {
                using (_db = new SqlConnection(Config.StringConexao()))
                {
                    var sb = new StringBuilder();

                    sb.AppendLine(" SELECT  ");
                    sb.AppendLine(" ISNULL(GN.bruto,0)-ISNULL(GN.TARA,0) PESO  ");
                    sb.AppendLine(" FROM ");
                    sb.AppendLine(" REDEX..TB_GATE_NEW GN ");
                    sb.AppendLine(" INNER JOIN REDEX..TB_AMR_GATE AG on gn.AUTONUM = ag.GATE ");
                    sb.AppendLine(" WHERE  ");
                    sb.AppendLine(" AG.CNTR_RDX = " + id);
                    sb.AppendLine(" AND  ");
                    sb.AppendLine(" FUNCAO_GATE In(205,206,216,225) ");
                    sb.AppendLine(" ORDER BY  ");
                    sb.AppendLine(" GN.AUTONUM DESC  ");


                    var query = _db.Query<decimal>(sb.ToString()).FirstOrDefault();

                    return query;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateDadosTbGateNew207(GateDTO gateDTO)
        {
            try
            {
                using (_db = new SqlConnection(Config.StringConexao()))
                {
                    var sb = new StringBuilder();

                    sb.AppendLine(" UPDATE REDEX..TB_GATE_NEW SET  ");
                    sb.AppendLine(" DT_GATE_OUT =  '" + gateDTO.DT_GATE_OUT.ToString("yyyy/MM/dd HH:mm:ss") + "' ");
                    sb.AppendLine(" ,BRUTO = isnull(BRUTO,0) + " + gateDTO.BRUTO);
                    sb.AppendLine(" ,FLAG_GATE_OUT = 1 ");
                    sb.AppendLine(" ,USUARIO_GATE_OUT =  " + gateDTO.UsuarioId);
                    sb.AppendLine(" ,FUNCAO_GATE_SAIDA = 207 ");
                    sb.AppendLine(" ,TICKET_SAIDA = 0 ");
                    sb.AppendLine(" ,GATE_MANUAL = 0 ");
                    sb.AppendLine(" WHERE  ");
                    sb.AppendLine(" AUTONUM =  " + gateDTO.AUTONUM);

                    _db.Query<object>(sb.ToString()).FirstOrDefault();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateTbTalieByIdPatio(int gate, int autonum_talie)
        {
            try
            {
                using (_db = new SqlConnection(Config.StringConexao()))
                {
                    var sb = new StringBuilder();

                    sb.AppendLine(" UPDATE REDEX..TB_TALIE SET ");
                    sb.AppendLine("  AUTONUM_GATE =  " + gate);
                    sb.AppendLine(" WHERE  ");
                    sb.AppendLine(" AUTONUM_TALIE =  " + autonum_talie);

                    _db.Query<object>(sb.ToString()).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateTbRomaneioByIdPatio(int gate, int autonum_ro)
        {
            try
            {
                using (_db = new SqlConnection(Config.StringConexao()))
                {
                    var sb = new StringBuilder();

                    sb.AppendLine(" UPDATE REDEX..TB_ROMANEIO SET ");
                    sb.AppendLine(" FLAG_HISTORICO = 1 ");
                    sb.AppendLine(" ,AUTONUM_GATE_SAIDA =  " + gate);
                    sb.AppendLine(" WHERE  ");
                    sb.AppendLine(" AUTONUM_RO =  " + autonum_ro);

                    _db.Query<object>(sb.ToString()).FirstOrDefault();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateTbSaidaCargaByIdPatio(int gate, string data_saida, int autonum_patio)
        {
            try
            {
                using (_db = new SqlConnection(Config.StringConexao()))
                {
                    var sb = new StringBuilder();

                    sb.AppendLine(" UPDATE REDEX..TB_SAIDA_CARGA SET  ");
                    sb.AppendLine(" AUTONUM_GATE_SAIDA = " + gate);
                    sb.AppendLine(" ,DT_SAIDA =  '" + Convert.ToDateTime(data_saida) + "' ");
                    sb.AppendLine(" WHERE  ");
                    sb.AppendLine(" AUTONUM_PATIO = " + autonum_patio);

                    _db.Query<object>(sb.ToString()).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IEnumerable<GateDTO> GetAutonumPcsByIdPatio(int id)
        {
            try
            {
                using (_db = new SqlConnection(Config.StringConexao()))
                {
                    var sb = new StringBuilder();

                    sb.AppendLine(" SELECT AUTONUM_PCS FROM REDEX..TB_SAIDA_CARGA where AUTONUM_PATIO = " + id);

                    var query = _db.Query<GateDTO>(sb.ToString()).AsEnumerable();

                    return query;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateUltSaidaTbPatioCsById(int id, string data)
        {
            try
            {
                using (_db = new SqlConnection(Config.StringConexao()))
                {
                    var sb = new StringBuilder();

                    sb.AppendLine(" UPDATE REDEX..tb_patio_cs SET DT_Ult_Saida = '" + Convert.ToDateTime(data) + "' WHERE AUTONUM = " + id);

                    _db.Query<object>(sb.ToString()).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdatePrimSaidaTbPatioCsById(int id, string data)
        {
            try
            {
                using (_db = new SqlConnection(Config.StringConexao()))
                {
                    var sb = new StringBuilder();

                    sb.AppendLine(" UPDATE REDEX..tb_patio_cs SET DT_prim_Saida = '" + Convert.ToDateTime(data) + "' WHERE AUTONUM = " + id);

                    _db.Query<object>(sb.ToString()).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public RomaneioDTO GetDadosRomaneioByPatioId(int id)
        {
            try
            {
                using (_db = new SqlConnection(Config.StringConexao()))
                {
                    var sb = new StringBuilder();

                    sb.AppendLine(" SELECT  ");
                    sb.AppendLine(" ro.autonum_gate_saida AS  AUTONUM_GATE_SAIDA ");
                    sb.AppendLine(" ,ro.crossdocking  ");
                    sb.AppendLine(" ,ro.autonum_patio ");
                    sb.AppendLine(" ,ro.autonum_ro ");
                    sb.AppendLine(" ,ro.autonum_talie  ");
                    sb.AppendLine(" ,ro.autonum_boo ");
                    sb.AppendLine(" ,boo.flag_op_full_cntr ");
                    sb.AppendLine(" FROM  ");
                    sb.AppendLine(" REDEX..tb_romaneio ro  ");
                    sb.AppendLine(" INNER JOIN REDEX..tb_booking boo on ro.autonum_ro = boo.autonum_boo ");
                    sb.AppendLine(" WHERE  ");
                    sb.AppendLine(" ro.autonum_patio = " + id);
                    sb.AppendLine(" AND  ");
                    sb.AppendLine(" ISNULL(ro.autonum_talie, 0) = 0 ");
                    sb.AppendLine(" AND  ");
                    sb.AppendLine(" boo.flag_op_full_cntr=1 ");
                    sb.AppendLine(" AND  ");
                    sb.AppendLine(" ISNULL(boo.flag_cs,0)=0 ");


                    var query = _db.Query<RomaneioDTO>(sb.ToString()).FirstOrDefault();

                    return query;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public long InsertNewTalie(RomaneioDTO romaneio)
        {
            try
            {
                using (_db = new SqlConnection(Config.StringConexao()))
                {
                    var sb = new StringBuilder();

                    sb.AppendLine(" INSERT INTO REDEX..TB_TALIE ");
                    sb.AppendLine(" ( ");
                    sb.AppendLine(" autonum_patio ");
                    sb.AppendLine(" ,inicio ");
                    sb.AppendLine(" ,termino ");
                    sb.AppendLine(" ,flag_estufagem ");
                    sb.AppendLine(" ,crossdocking  ");
                    sb.AppendLine(" ,autonum_boo ");
                    sb.AppendLine(" ,forma_operacao ");
                    sb.AppendLine(" ,conferente ");
                    sb.AppendLine(" ,equipe  ");
                    sb.AppendLine(" ,flag_descarga ");
                    sb.AppendLine(" ,flag_carregamento ");
                    sb.AppendLine(" ,obs  ");
                    sb.AppendLine(" ,autonum_ro ");
                    sb.AppendLine(" ,autonum_gate ");
                    sb.AppendLine(" ,flagfechado ");
                    sb.AppendLine(" ) VALUES ( ");
                    sb.AppendLine(" " + romaneio.autonum_patio);
                    sb.AppendLine(" ,Convert(datetime('" + romaneio.DT_GATE_IN + "') ");
                    sb.AppendLine(" ,GetDate() ");
                    sb.AppendLine(" ,0 ");
                    sb.AppendLine(" ,0 ");
                    sb.AppendLine(" ," + romaneio.autonum_boo);
                    sb.AppendLine(" ," + string.Empty);
                    sb.AppendLine(" ,null ");
                    sb.AppendLine(" ,null ");
                    sb.AppendLine(" ,0 ");
                    sb.AppendLine(" ,1 ");
                    sb.AppendLine(" ," + string.Empty);
                    sb.AppendLine(" , " + romaneio.autonum_ro);
                    sb.AppendLine(" , " + romaneio.autonum_gate_entrada);
                    sb.AppendLine(" ,1 ");
                    sb.AppendLine("  ) select cast(scope_identity as big int) ");

                    long id = _db.Query<int>(sb.ToString()).FirstOrDefault();

                    return id;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateTbRomaneioTalieById(int talie_id, int id)
        {
            try
            {
                using (_db = new SqlConnection(Config.StringConexao()))
                {
                    var sb = new StringBuilder();

                    sb.AppendLine(" UPDATE REDEX..TB_ROMANEIO SET ");
                    sb.AppendLine(" FLAG_HISTORICO = 1  ");
                    sb.AppendLine(" ,AUTONUM_TALIE = " + talie_id);
                    sb.AppendLine(" WHERE   ");
                    sb.AppendLine(" AUTONUM_RO = " + id);

                    _db.Query<object>(sb.ToString()).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int GetQuantidadeEntrada(int id)
        {
            try
            {
                using (_db = new SqlConnection(Config.StringConexao()))
                {
                    var sb = new StringBuilder();

                    sb.AppendLine(" select isnull(qtde_entrada, 0) from REDEX..tb_patio_cs where autonum_pcs =  " + id);

                    int qtt = _db.Query<int>(sb.ToString()).FirstOrDefault();

                    return qtt;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int GetQuantidadesSaida(int id)
        {
            try
            {
                using (_db = new SqlConnection(Config.StringConexao()))
                {
                    var sb = new StringBuilder();

                    sb.AppendLine(" select isnull(qtde_saida, 0) from REDEX..tb_patio_cs where autonum_pcs =  " + id);

                    int qts = _db.Query<int>(sb.ToString()).FirstOrDefault();

                    qts = _db.Query<int>(sb.ToString()).FirstOrDefault();

                    return qts;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateTbPatioFlagHistorico(int id)
        {
            try
            {
                using (_db = new SqlConnection(Config.StringConexao()))
                {
                    var sb = new StringBuilder();

                    sb.AppendLine(" UPDATE REDEX..TB_PATIO_CS SET  ");
                    sb.AppendLine(" FLAG_HISTORICO = 1 ");
                    sb.AppendLine(" WHERE ");
                    sb.AppendLine(" AUTONUM_PCS = " + id);

                    _db.Query<object>(sb.ToString()).FirstOrDefault();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateTbPregistroByPlaca(string placa)
        {
            try
            {
                using (_db = new SqlConnection(Config.StringConexao()))
                {
                    var sb = new StringBuilder();

                    sb.AppendLine(" update REDEX..TB_PRE_REGISTRO SET   ");
                    sb.AppendLine(" saida_redex = GETDATE() ");
                    sb.AppendLine(" where   ");
                    sb.AppendLine("  placa = '" + placa + "' ");
                    sb.AppendLine(" and  ");
                    sb.AppendLine(" entrada_redex is not null  ");
                    sb.AppendLine(" and  ");
                    sb.AppendLine(" saida_redex is null  ");

                    _db.Query<object>(sb.ToString()).FirstOrDefault();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateTbRegistroCntrSaida(string operacaoUnidades, string placa)
        {
            try
            {
                using (_db = new SqlConnection(Config.StringConexao()))
                {
                    var sb = new StringBuilder();

                    sb.AppendLine(" update REDEX..TB_PRE_REGISTRO SET ");
                    sb.AppendLine(" conteiner_saida = '" + operacaoUnidades + "' ");
                    sb.AppendLine(" where placa = '" + placa + "' ");
                    sb.AppendLine(" and  ");
                    sb.AppendLine(" data_saida is null ");

                    _db.Query<object>(sb.ToString()).FirstOrDefault();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
