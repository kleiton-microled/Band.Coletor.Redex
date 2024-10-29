using Band.Coletor.Redex.Business.DTO;
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
    public class MovimentacaoContainerRepositorio : IMovimentacaoContainerRepositorio
    {
        public int GetFlagTruckMovColetor(int patio)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    var sb = new StringBuilder();

                    sb.AppendLine(" SELECT ISNULL(Flag_Truck_Mov_Coletor,0) as qual FROM OPERADOR..TB_PATIOS WHERE AUTONUM  = " + patio);

                    string sql = sb.ToString();

                    int flag_truck = _db.Query<int>(sql).FirstOrDefault();

                    return flag_truck;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public MovimentacaoContainerDTO GetDadosCntr(string id_conteiner, int patio)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {

                    var sb = new StringBuilder();

                    sb.Clear();

                    sb.AppendLine(" SELECT  ");
                    sb.AppendLine(" AUTONUM, ");
                    sb.AppendLine(" IMPEXP,  ");
                    sb.AppendLine(" DESCRICAO,  ");
                    sb.AppendLine(" DATA_ENT_TEMP,  ");
                    sb.AppendLine(" BRUTO,  ");
                    sb.AppendLine(" IMO1,  ");
                    sb.AppendLine(" YARD,  ");
                    sb.AppendLine(" TAMANHO,  ");
                    sb.AppendLine(" TEMPERATURE,  ");
                    sb.AppendLine(" TIPOBASICO,  ");
                    sb.AppendLine(" DESCR_MOTIVO_POSIC,  ");
                    sb.AppendLine(" dt_prevista_posic,  ");
                    sb.AppendLine(" FINALITY,  ");
                    sb.AppendLine(" VIAGEM,  ");
                    sb.AppendLine(" SITUACAO_BL,  ");
                    sb.AppendLine(" QTD,  ");
                    sb.AppendLine(" FLAG_OOG,  ");
                    sb.AppendLine(" FLAG_SPC,  ");
                    sb.AppendLine(" DLV_TERM,  ");
                    sb.AppendLine(" ID_CONTEINER,  ");
                    sb.AppendLine(" QTD,  ");
                    sb.AppendLine(" EF ");
                    sb.AppendLine(" from SGIPA..P_CONSULTA_INVENT_PATIO('" + id_conteiner + "', " + patio + ")  ");

                    var query = _db.Query<MovimentacaoContainerDTO>(sb.ToString()).FirstOrDefault();

                    return query;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string GetQualCam(int patio)
        {
            try
            {
                using (var con = new SqlConnection(Config.StringConexao()))
                {
                    var sb = new StringBuilder();

                    sb.AppendLine(" SELECT ISNULL(MAX(C.DESCR),'-') AS QUALCAM FROM OPERADOR..TB_YARD P INNER JOIN OPERADOR..TB_CAMERAS C ON P.AUTONUM_CAMERA=C.AUTONUM WHERE P.PATIO= " + patio);

                    string sql = sb.ToString();

                    string cam = con.Query<string>(sql).FirstOrDefault();

                    return cam;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Reserva ConsultarReserva(string reserva)
        {
            try
            {
                using (var con = new SqlConnection(Config.StringConexao()))
                {
                    var sb = new StringBuilder();

                    sb.AppendLine("select b.autonum_boo, b.imp_exp from Redex..tb_booking b where b.reference='" + reserva + "' ");

                    var cam = con.Query<Reserva>(sb.ToString()).FirstOrDefault();

                    return cam;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Reserva ConsultarCarga20(string Boo)
        {
            try
            {
                using (var con = new SqlConnection(Config.StringConexao()))
                {
                    var sb = new StringBuilder();

                    sb.Clear();
                    sb.AppendLine("select autonum_bcg, autonum_tpc from Redex..tb_booking_carga  where autonum_boo=" + Boo + " and flag_cntr=1 and tamanho=20 ");

                    var cam = con.Query<Reserva>(sb.ToString()).FirstOrDefault();

                    return cam;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Reserva ConsultarCarga40(string Boo)
        {
            try
            {
                using (var con = new SqlConnection(Config.StringConexao()))
                {
                    var sb = new StringBuilder();

                    sb.AppendLine("select autonum_bcg, autonum_tpc from Redex..tb_booking_carga  where autonum_boo=" + Boo + " and flag_cntr=1 and tamanho=40 ");

                    var cam = con.Query<Reserva>(sb.ToString()).FirstOrDefault();

                    return cam;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int ConsultaCNTREstoque(string cntr)
        {
            try
            {
                using (var con = new SqlConnection(Config.StringConexao()))
                {
                    var sb = new StringBuilder();

                    sb.Clear();
                    sb.AppendLine("select count(*) from Redex..tb_patio  where id_conteiner='" + cntr + "' and flag_historico=0");

                    var cam = con.Query<int>(sb.ToString()).FirstOrDefault();

                    return cam;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int ConsultaBooking(int Boo)
        {
            try
            {
                using (var con = new SqlConnection(Config.StringConexao()))
                {
                    var sb = new StringBuilder();

                    sb.Clear();
                    sb.AppendLine("Select isnull(OS,0) OS FROM Redex..tb_BOOKING WHERE AUTONUM_BOO=" + Boo + "");

                    var cam = con.Query<int>(sb.ToString()).FirstOrDefault();

                    return cam;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public int CadastrarOS(string ano)
        {
            try
            {
                using (var con = new SqlConnection(Config.StringConexao()))
                {
                    var sb = new StringBuilder();

                    sb.AppendLine("INSERT INTO redex..seq_os_" + ano + " default values; SELECT  SCOPE_IDENTITY()  AS Id");

                    var cam = con.Query<int>(sb.ToString()).FirstOrDefault();

                    return cam;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public int Cadastrar(RegistrosViewModel viewModel)
        {
            try
            {
                using (var con = new SqlConnection(Config.StringConexao()))
                {
                    var sb = new StringBuilder();


                    sb.Clear();
                    sb.AppendLine(" insert into redex..tb_registro ( ");
                    sb.AppendLine(" autonum_frota, placa,carreta,reboque,autonum_mot,autonum_transp,dt_chegada,");
                    sb.AppendLine(" tipo_registro,flag_cs,flag_cr,autonum_boo,tara_veiculo,tipo_doc_transp,doc_transp ");
                    sb.AppendLine(" ,renavam,patio,protocolo,num_protocolo,tipo_nota, ");
                    sb.AppendLine(" FLAG_DTA_SD,DT_EMISSAO_DAT,FLAG_DTA,NUM_DOC_DAT,TIPO_DOCUMENTO_DAT,");
                    sb.AppendLine(" flag_liberado, status, DT_LIB_ENT_CAM, ARMAZEM_DESTINO) values ( ");
                    sb.AppendLine(" " + viewModel.ID_frota + ", ");
                    sb.AppendLine(" '" + viewModel.PlacaPesquisa + "',");
                    sb.AppendLine(" '" + viewModel.PlacaCarretaPesquisa + "', ");
                    sb.AppendLine(" '" + viewModel.Reboque + "', ");
                    sb.AppendLine(" '" + viewModel.Motorista + "', ");
                    sb.AppendLine(" 2,getdate(),'E',0,1, ");
                    sb.AppendLine(" " + viewModel.autonumBoo + ", ");
                    sb.AppendLine(" 0,null,null,'',1,null,null,null,0,null,0,'','',1,'L' ,getdate(),null); SELECT  SCOPE_IDENTITY()  AS Id ");


                    var cam = con.Query<int>(sb.ToString()).FirstOrDefault();

                    return cam;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public int UpdateRegistro(string boo, string IdConteiner, string Tam, string BCG, string TPC, string placa)
        {
            try
            {
                using (var con = new SqlConnection(Config.StringConexao()))
                {
                    var sb = new StringBuilder();

                    sb.Clear();
                    sb.AppendLine(" update redex..tb_registro set flag_liberado = 1, dt_lib_ent_cam = getdate(),status = 'L' where isnull(flag_liberado,0) = 0 and placa = '" + placa + "' ");



                    var cam = con.Query<int>(sb.ToString()).FirstOrDefault();

                    return cam;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public int AtualizarBooking(string os, string ano, string boo)
        {
            try
            {
                using (var con = new SqlConnection(Config.StringConexao()))
                {
                    var sb = new StringBuilder();

                    sb.AppendLine("update redex..tb_booking set os " + os + "0000 / " + ano + " where autonum_boo = " + boo + "");

                    var cam = con.Query<int>(sb.ToString()).FirstOrDefault();

                    return cam;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }



        public IEnumerable<ConteinerDTO> GetDadosConteiner(string id_conteiner)
        {
            try
            {
                using (var con = new SqlConnection(Config.StringConexao()))
                {
                    var sb = new StringBuilder();

                    sb.AppendLine(" SELECT DISTINCT E.AUTONUM, C.ID_CONTEINER  + ' - ' + YARD  + ' - LOTE:' + E.AUTONUM AS Conteiner    ");
                    sb.AppendLine("  FROM OPERADOR..TB_GATE_NEW A INNER JOIN OPERADOR..TB_AMR_GATE B ON A.AUTONUM = B.GATE  ");
                    sb.AppendLine(" INNER JOIN TB_CNTR_BL C ON B.CNTR_IPA = C.AUTONUM INNER JOIN TB_AMR_CNTR_BL D ON D.CNTR = C.AUTONUM  ");
                    sb.AppendLine(" INNER JOIN TB_BL E ON D.BL= E.AUTONUM WHERE B.FUNCAO_GATE = 11 AND A.DT_GATE_IN IS NOT NULL AND ID_CONTEINER = " + id_conteiner);

                    string sql = sb.ToString();

                    var query = con.Query<ConteinerDTO>(sql).AsEnumerable();

                    return query;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string Valida_Aloca_Imo(ConteinerDTO conteiner)
        {
            try
            {
                bool valida = true;
                string ret = string.Empty;

                using (var con = new SqlConnection(Config.StringConexao()))
                {
                    var sb = new StringBuilder();

                    sb.Clear();
                    sb.AppendLine(" SELECT ISNULL(valida,0) AS VALIDA FROM OPERADOR..TB_YARD WHERE PATIO= " + conteiner.PATIO + "  AND YARD = '" + conteiner.YARD + "' ");


                    string sql = sb.ToString();

                    int valida_int = con.Query<int>(sql).FirstOrDefault();

                    if (valida_int == 0)
                        valida = false;

                    sb.Clear();

                    sb.AppendLine(" SELECT ID_CONTEINER,ISNULL(IMO1,'') AS IMO1,ISNULL(IMO2,'') AS IMO2, ISNULL(IMO3,'') AS IMO3,ISNULL(IMO4,'') AS IMO4  ");
                    sb.AppendLine(" FROM  ");
                    sb.AppendLine("  OPERADOR..VW_INVENT_SISTEMAS_IMO_XYZ ");
                    sb.AppendLine(" WHERE ");
                    sb.AppendLine(" ID_CONTEINER '" + conteiner.IdConteiner + "' ");

                    sql = sb.ToString();

                    var imos = con.Query<ConteinerDTO>(sql).FirstOrDefault();

                    if (imos != null)
                    {
                        if (valida)
                        {
                            sb.Clear();

                            sb.AppendLine(" SELECT YARD FROM INTELOPER..TB_IP_ATRIBUTO_YARD ");
                            sb.AppendLine(" WHERE PATIO=" + conteiner.PATIO + " AND AUTONUM_ATR=21 AND VLR_ATRIB=1 AND STATUS_ATRIB='R'");
                            sb.AppendLine(" AND DT_INI_ATR < GETDATE() AND (DT_FIM_ATR IS NULL OR DT_FIM_ATR> GETDATE() )");
                            sb.AppendLine(" AND (LENGTH(YARD)=1 OR LENGTH(YARD)=2) ");
                            sb.AppendLine(" AND (SUBSTR(YARD,1,1)='" + conteiner.YARD.Substring(0, 1) + "' OR SUBSTR(YARD,1,2)='" + conteiner.YARD.Substring(0, 2) + "')");
                            sb.AppendLine(" UNION ");
                            sb.AppendLine(" SELECT YARD FROM INTELOPER..TB_IP_ATRIBUTO_YARD ");
                            sb.AppendLine(" WHERE PATIO = " + conteiner.PATIO + " AND AUTONUM_ATR=21 AND VLR_ATRIB=0 AND STATUS_ATRIB='E' ");
                            sb.AppendLine(" AND DT_INI_ATR < GETDATE() AND (DT_FIM_ATR IS NULL OR DT_FIM_ATR > GETDATE()) ");
                            sb.AppendLine(" AND (LENGTH(YARD)=1 OR LENGTH(YARD)=2) ");
                            sb.AppendLine(" AND (SUBSTR(YARD,1,1)='" + conteiner.YARD.Substring(0, 1) + "' OR SUBSTR(YARD,1,2)='" + conteiner.YARD.Substring(0, 2) + "') ");


                            sql = sb.ToString();

                            var yardDt = con.Query<ConteinerDTO>(sql).FirstOrDefault();

                            if (yardDt != null)
                            {
                                ret = "Quadra não permite conteiner IMO";

                                return ret;
                            }


                            sb.Clear();

                            sb.AppendLine(" SELECT YARD FROM INTELOPER..TB_IP_ATRIBUTO_YARD ");
                            sb.AppendLine(" WHERE PATIO=" + conteiner.PATIO + " AND AUTONUM_ATR=21 AND VLR_ATRIB=1 AND STATUS_ATRIB='R' ");
                            sb.AppendLine(" AND DT_INI_ATR < GETDATE() AND (DT_FIM_ATR IS NULL OR DT_FIM_ATR>GETDATE()) ");

                            if (conteiner.TAMANHO == 20)
                            {
                                sb.AppendLine(" AND SUBSTR(YARD,1,6)='" + conteiner.YARD.Substring(0, 5) + "' ");
                            }
                            else
                            {
                                sb.AppendLine(" AND SUBSTR(YARD,1,6)='" + conteiner.YARD.Substring(0, 1) + conteiner.YARD.Substring(2, 1) + 1 + conteiner.YARD.Substring(4, 1) + "' ");
                                sb.AppendLine(" OR ");
                                sb.AppendLine(" SUBSTR(YARD,1,6)='" + conteiner.YARD.Substring(0, 1) + conteiner.YARD.Substring(2, 1) + 1 + conteiner.YARD.Substring(4, 1) + "' ");
                            }

                            sql = sb.ToString();

                            var yardDT2 = con.Query<ConteinerDTO>(sql).FirstOrDefault();

                            if (yardDT2 != null)
                            {
                                ret = "A Pilha não permite conteiner IMO";

                                return ret;
                            }

                        }
                        else
                        {
                            sb.Clear();


                            if (string.IsNullOrEmpty(imos.IMO1))
                            {
                                sb.AppendLine(" SELECT YARD FROM INTELOPER..TB_IP_ATRIBUTO_YARD ");
                                sb.AppendLine(" WHERE PATIO=" + conteiner.PATIO + " AND AUTONUM_ATR=21 AND VLR_ATRIB=1 AND STATUS_ATRIB='R' ");
                                sb.AppendLine(" AND DT_INI_ATR < GETDATE() AND (DT_FIM_ATR IS NULL OR DT_FIM_ATR> GETDATE()) ");
                                sb.AppendLine(" AND YARD '" + conteiner.YARD + "' ");
                                sb.AppendLine(" Union ");
                                sb.AppendLine(" SELECT YARD FROM INTELOPER..B_IP_ATRIBUTO_YARD ");
                                sb.AppendLine(" WHERE PATIO=" + conteiner.PATIO + " AND AUTONUM_ATR=21 AND VLR_ATRIB=0 AND STATUS_ATRIB='E  ");
                                sb.AppendLine(" AND DT_INI_ATR < GETDATE() AND (DT_FIM_ATR IS NULL OR DT_FIM_ATR> GETDATE() )  ");
                                sb.AppendLine(" AND YARD '" + conteiner.YARD + "' ");

                                sql = sb.ToString();

                                var yardDT3 = con.Query<ConteinerDTO>(sql).FirstOrDefault();

                                if (yardDT3 != null)
                                {
                                    ret = "Heap/Rua não permite conteiner IMO";

                                    return ret;
                                }
                            }
                        }


                        string imo1 = imos.IMO1;
                        string imo2 = imos.IMO2;
                        string imo3 = imos.IMO3;
                        string imo4 = imos.IMO4;

                        if (string.IsNullOrEmpty(imo1))
                        {
                            if (imos.IMO1 != "0")
                            {
                                if (valida)
                                {
                                    sb.Clear();

                                    sb.AppendLine(" SELECT YARD FROM INTELOPER..TB_IP_ATRIBUTO_YARD ");
                                    sb.AppendLine(" WHERE PATIO=" + conteiner.PATIO + " AND AUTONUM_ATR=12 AND VLR_ATRIB='" + imo1.Trim() + "' AND STATUS_ATRIB='R' ");
                                    sb.AppendLine(" AND DT_INI_ATR < GETDATE() AND (DT_FIM_ATR IS NULL OR DT_FIM_ATR>GETDATE()) ");
                                    sb.AppendLine(" AND (LENGTH(YARD)=1 OR LENGTH(YARD)=2)  ");
                                    sb.AppendLine(" AND (SUBSTR(YARD,1,1)='" + conteiner.YARD.Substring(0, 0) + "' OR SUBSTR(YARD,1,2)='" + conteiner.YARD.Substring(0, 1) + "') ");
                                    sb.AppendLine(" UNION ");
                                    sb.AppendLine(" SELECT YARD FROM INTELOPER..TB_IP_ATRIBUTO_YARD ");
                                    sb.AppendLine(" WHERE AUTONUM_ATR=12 AND VLR_ATRIB<>'" + conteiner.IMO1.Trim() + "' AND STATUS_ATRIB='E' ");
                                    sb.AppendLine(" AND DT_INI_ATR < GETDATE() AND (DT_FIM_ATR IS NULL OR DT_FIM_ATR > GETDATE()) ");
                                    sb.AppendLine(" AND (LENGTH(YARD)=1 OR LENGTH(YARD)=2)  ");
                                    sb.AppendLine(" AND (SUBSTR(YARD,1,1)='" + conteiner.YARD.Substring(0, 1) + "' OR SUBSTR(YARD,1,2)='" + conteiner.YARD.Substring(0, 2) + "') ");

                                    sql = sb.ToString();

                                    var yardDT4 = con.Query<ConteinerDTO>(sql).FirstOrDefault();

                                    if (yardDT4 != null)
                                    {
                                        ret = "Quadra não permite classe IMO " + imo1;

                                        return ret;
                                    }

                                }
                                else
                                {
                                    sb.Clear();

                                    sb.AppendLine(" SELECT YARD FROM INTELOPER..TB_IP_ATRIBUTO_YARD ");
                                    sb.AppendLine(" WHERE PATIO=" + conteiner.PATIO + " AND AUTONUM_ATR=12 AND VLR_ATRIB='" + imo1.Trim() + "' AND STATUS_ATRIB='R' ");
                                    sb.AppendLine(" AND DT_INI_ATR < GETDATE() AND (DT_FIM_ATR IS NULL OR DT_FIM_ATR>GETDATE()) ");
                                    sb.AppendLine(" AND YARD='" + conteiner.YARD + "' ");
                                    sb.AppendLine(" UNION ");
                                    sb.AppendLine(" SELECT YARD FROM INTELOPER..TB_IP_ATRIBUTO_YARD ");
                                    sb.AppendLine(" WHERE AUTONUM_ATR=12 AND VLR_ATRIB<>'" + imo1.Trim() + "' AND STATUS_ATRIB='E' ");
                                    sb.AppendLine(" AND DT_INI_ATR < GETDATE() AND (DT_FIM_ATR IS NULL OR DT_FIM_ATR>GETDATE()) ");
                                    sb.AppendLine(" AND YARD='" + conteiner.YARD + "' ");

                                    sql = sb.ToString();

                                    var yardDT8 = con.Query<ConteinerDTO>(sql).FirstOrDefault();

                                    if (yardDT8 != null)
                                    {
                                        ret = "Heap/Rua não permite Classe IMO " + imo1;

                                        return ret;
                                    }

                                }
                            }
                        }

                        if (string.IsNullOrEmpty(imo2))
                        {
                            if (imo2 != "0")
                            {
                                if (valida)
                                {
                                    sb.Clear();

                                    sb.AppendLine(" SELECT YARD FROM INTELOPER..TB_IP_ATRIBUTO_YARD ");
                                    sb.AppendLine(" WHERE PATIO=" + conteiner.PATIO + " AND AUTONUM_ATR=12 AND VLR_ATRIB='" + imo2.Trim() + "' AND STATUS_ATRIB='R' ");
                                    sb.AppendLine(" AND DT_INI_ATR < GETDATE() AND (DT_FIM_ATR IS NULL OR DT_FIM_ATR>GETDATE()) ");
                                    sb.AppendLine(" AND (LENGTH(YARD)=1 OR LENGTH(YARD)=2)  ");
                                    sb.AppendLine(" AND (SUBSTR(YARD,1,1)='" + conteiner.YARD.Substring(0, 0) + "' OR SUBSTR(YARD,1,2)='" + conteiner.YARD.Substring(0, 1) + "') ");
                                    sb.AppendLine(" UNION ");
                                    sb.AppendLine(" SELECT YARD FROM INTELOPER..TB_IP_ATRIBUTO_YARD ");
                                    sb.AppendLine(" WHERE AUTONUM_ATR=12 AND VLR_ATRIB<>'" + conteiner.IMO1.Trim() + "' AND STATUS_ATRIB='E' ");
                                    sb.AppendLine(" AND DT_INI_ATR < GETDATE() AND (DT_FIM_ATR IS NULL OR DT_FIM_ATR > GETDATE()) ");
                                    sb.AppendLine(" AND (LENGTH(YARD)=1 OR LENGTH(YARD)=2)  ");
                                    sb.AppendLine(" AND (SUBSTR(YARD,1,1)='" + conteiner.YARD.Substring(0, 1) + "' OR SUBSTR(YARD,1,2)='" + conteiner.YARD.Substring(0, 2) + "') ");

                                    sql = sb.ToString();

                                    var yardDT5 = con.Query<ConteinerDTO>(sql).FirstOrDefault();

                                    if (yardDT5 != null)
                                    {
                                        ret = "Quadra não permite classe IMO " + imo2;

                                        return ret;
                                    }
                                }
                                else
                                {
                                    sb.Clear();

                                    sb.AppendLine(" SELECT YARD FROM INTELOPER..TB_IP_ATRIBUTO_YARD ");
                                    sb.AppendLine(" WHERE PATIO=" + conteiner.PATIO + " AND AUTONUM_ATR=12 AND VLR_ATRIB='" + imo2.Trim() + "' AND STATUS_ATRIB='R' ");
                                    sb.AppendLine(" AND DT_INI_ATR < GETDATE() AND (DT_FIM_ATR IS NULL OR DT_FIM_ATR>GETDATE()) ");
                                    sb.AppendLine(" AND YARD='" + conteiner.YARD + "' ");
                                    sb.AppendLine(" UNION ");
                                    sb.AppendLine(" SELECT YARD FROM INTELOPER..TB_IP_ATRIBUTO_YARD ");
                                    sb.AppendLine(" WHERE AUTONUM_ATR=12 AND VLR_ATRIB<>'" + imo1.Trim() + "' AND STATUS_ATRIB='E' ");
                                    sb.AppendLine(" AND DT_INI_ATR < GETDATE() AND (DT_FIM_ATR IS NULL OR DT_FIM_ATR>GETDATE()) ");
                                    sb.AppendLine(" AND YARD='" + conteiner.YARD + "' ");

                                    sql = sb.ToString();

                                    var yardDT9 = con.Query<ConteinerDTO>(sql).FirstOrDefault();

                                    if (yardDT9 != null)
                                    {
                                        ret = "Heap/Rua não permite Classe IMO " + imo2;

                                        return ret;
                                    }
                                }
                            }
                        }

                        if (string.IsNullOrEmpty(imo3))
                        {
                            if (imo3 != "0")
                            {
                                if (valida)
                                {
                                    sb.Clear();

                                    sb.AppendLine(" SELECT YARD FROM INTELOPER..TB_IP_ATRIBUTO_YARD ");
                                    sb.AppendLine(" WHERE PATIO=" + conteiner.PATIO + " AND AUTONUM_ATR=12 AND VLR_ATRIB='" + imo3.Trim() + "' AND STATUS_ATRIB='R' ");
                                    sb.AppendLine(" AND DT_INI_ATR < GETDATE() AND (DT_FIM_ATR IS NULL OR DT_FIM_ATR>GETDATE()) ");
                                    sb.AppendLine(" AND (LENGTH(YARD)=1 OR LENGTH(YARD)=2)  ");
                                    sb.AppendLine(" AND (SUBSTR(YARD,1,1)='" + conteiner.YARD.Substring(0, 0) + "' OR SUBSTR(YARD,1,2)='" + conteiner.YARD.Substring(0, 1) + "') ");
                                    sb.AppendLine(" UNION ");
                                    sb.AppendLine(" SELECT YARD FROM INTELOPER..TB_IP_ATRIBUTO_YARD ");
                                    sb.AppendLine(" WHERE AUTONUM_ATR=12 AND VLR_ATRIB<>'" + conteiner.IMO1.Trim() + "' AND STATUS_ATRIB='E' ");
                                    sb.AppendLine(" AND DT_INI_ATR < GETDATE() AND (DT_FIM_ATR IS NULL OR DT_FIM_ATR > GETDATE()) ");
                                    sb.AppendLine(" AND (LENGTH(YARD)=1 OR LENGTH(YARD)=2)  ");
                                    sb.AppendLine(" AND (SUBSTR(YARD,1,1)='" + conteiner.YARD.Substring(0, 1) + "' OR SUBSTR(YARD,1,2)='" + conteiner.YARD.Substring(0, 2) + "') ");

                                    sql = sb.ToString();

                                    var yardDT6 = con.Query<ConteinerDTO>(sql).FirstOrDefault();

                                    if (yardDT6 != null)
                                    {
                                        ret = "Quadra não permite classe IMO " + imo3;

                                        return ret;
                                    }
                                }
                                else
                                {
                                    sb.Clear();

                                    sb.AppendLine(" SELECT YARD FROM INTELOPER..TB_IP_ATRIBUTO_YARD ");
                                    sb.AppendLine(" WHERE PATIO=" + conteiner.PATIO + " AND AUTONUM_ATR=12 AND VLR_ATRIB='" + imo3.Trim() + "' AND STATUS_ATRIB='R' ");
                                    sb.AppendLine(" AND DT_INI_ATR < GETDATE() AND (DT_FIM_ATR IS NULL OR DT_FIM_ATR>GETDATE()) ");
                                    sb.AppendLine(" AND YARD='" + conteiner.YARD + "' ");
                                    sb.AppendLine(" UNION ");
                                    sb.AppendLine(" SELECT YARD FROM INTELOPER..TB_IP_ATRIBUTO_YARD ");
                                    sb.AppendLine(" WHERE AUTONUM_ATR=12 AND VLR_ATRIB<>'" + imo3.Trim() + "' AND STATUS_ATRIB='E' ");
                                    sb.AppendLine(" AND DT_INI_ATR < GETDATE() AND (DT_FIM_ATR IS NULL OR DT_FIM_ATR>GETDATE()) ");
                                    sb.AppendLine(" AND YARD='" + conteiner.YARD + "' ");

                                    sql = sb.ToString();

                                    var yardDT10 = con.Query<ConteinerDTO>(sql).FirstOrDefault();

                                    if (yardDT10 != null)
                                    {
                                        ret = "Heap/Rua não permite Classe IMO " + imo3;

                                        return ret;
                                    }
                                }
                            }
                        }

                        if (string.IsNullOrEmpty(imo4))
                        {
                            if (imo3 != "0")
                            {
                                if (valida)
                                {
                                    sb.Clear();

                                    sb.AppendLine(" SELECT YARD FROM INTELOPER..TB_IP_ATRIBUTO_YARD ");
                                    sb.AppendLine(" WHERE PATIO=" + conteiner.PATIO + " AND AUTONUM_ATR=12 AND VLR_ATRIB='" + imo4.Trim() + "' AND STATUS_ATRIB='R' ");
                                    sb.AppendLine(" AND DT_INI_ATR < GETDATE() AND (DT_FIM_ATR IS NULL OR DT_FIM_ATR>GETDATE()) ");
                                    sb.AppendLine(" AND (LENGTH(YARD)=1 OR LENGTH(YARD)=2)  ");
                                    sb.AppendLine(" AND (SUBSTR(YARD,1,1)='" + conteiner.YARD.Substring(0, 0) + "' OR SUBSTR(YARD,1,2)='" + conteiner.YARD.Substring(0, 1) + "') ");
                                    sb.AppendLine(" UNION ");
                                    sb.AppendLine(" SELECT YARD FROM INTELOPER..TB_IP_ATRIBUTO_YARD ");
                                    sb.AppendLine(" WHERE AUTONUM_ATR=12 AND VLR_ATRIB<>'" + conteiner.IMO1.Trim() + "' AND STATUS_ATRIB='E' ");
                                    sb.AppendLine(" AND DT_INI_ATR < GETDATE() AND (DT_FIM_ATR IS NULL OR DT_FIM_ATR > GETDATE()) ");
                                    sb.AppendLine(" AND (LENGTH(YARD)=1 OR LENGTH(YARD)=2)  ");
                                    sb.AppendLine(" AND (SUBSTR(YARD,1,1)='" + conteiner.YARD.Substring(0, 1) + "' OR SUBSTR(YARD,1,2)='" + conteiner.YARD.Substring(0, 2) + "') ");

                                    sql = sb.ToString();

                                    var yardDT7 = con.Query<ConteinerDTO>(sql).FirstOrDefault();

                                    if (yardDT7 != null)
                                    {
                                        ret = "Quadra não permite classe IMO " + imo4;

                                        return ret;
                                    }
                                }
                                else
                                {
                                    sb.Clear();

                                    sb.AppendLine(" SELECT YARD FROM INTELOPER..TB_IP_ATRIBUTO_YARD ");
                                    sb.AppendLine(" WHERE PATIO=" + conteiner.PATIO + " AND AUTONUM_ATR=12 AND VLR_ATRIB='" + imo4.Trim() + "' AND STATUS_ATRIB='R' ");
                                    sb.AppendLine(" AND DT_INI_ATR < GETDATE() AND (DT_FIM_ATR IS NULL OR DT_FIM_ATR>GETDATE()) ");
                                    sb.AppendLine(" AND YARD='" + conteiner.YARD + "' ");
                                    sb.AppendLine(" UNION ");
                                    sb.AppendLine(" SELECT YARD FROM INTELOPER..TB_IP_ATRIBUTO_YARD ");
                                    sb.AppendLine(" WHERE AUTONUM_ATR=12 AND VLR_ATRIB<>'" + imo4.Trim() + "' AND STATUS_ATRIB='E' ");
                                    sb.AppendLine(" AND DT_INI_ATR < GETDATE() AND (DT_FIM_ATR IS NULL OR DT_FIM_ATR>GETDATE()) ");
                                    sb.AppendLine(" AND YARD='" + conteiner.YARD + "' ");

                                    sql = sb.ToString();

                                    var yardDT11 = con.Query<ConteinerDTO>(sql = sb.ToString()).FirstOrDefault();

                                    if (yardDT11 != null)
                                    {
                                        ret = "Heap/Rua não permite Classe IMO " + imo4;

                                        return ret;
                                    }

                                }
                            }
                        }
                    }


                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string Valida_NImo(ConteinerDTO conteiner)
        {
            try
            {
                bool valida = true;
                string ret = string.Empty;

                using (var con = new SqlConnection(Config.StringConexao()))
                {
                    var sb = new StringBuilder();

                    sb.Clear();
                    sb.AppendLine(" SELECT ISNULL(valida,0) AS VALIDA FROM OPERADOR..TB_YARD WHERE PATIO= " + conteiner.PATIO + "  AND YARD = '" + conteiner.YARD + "' ");

                    string sql = sb.ToString();

                    int valida_int = con.Query<int>(sql).FirstOrDefault();

                    if (valida_int == 0)
                        valida = false;

                    if (valida)
                    {
                        sb.Clear();

                        sb.AppendLine(" SELECT YARD FROM INTELOPER..TB_IP_ATRIBUTO_YARD ");
                        sb.AppendLine(" WHERE PATIO=" + conteiner.PATIO + " AND AUTONUM_ATR=21 AND VLR_ATRIB=1 AND STATUS_ATRIB='E' ");
                        sb.AppendLine(" AND DT_INI_ATR < GETDATE() AND (DT_FIM_ATR IS NULL OR DT_FIM_ATR> GETDATE() ) ");
                        sb.AppendLine(" AND (LENGTH(YARD)=1 OR LENGTH(YARD)=2)  ");
                        sb.AppendLine(" AND (SUBSTR(YARD,1,1)='" + conteiner.YARD.Substring(0, 1) + "' OR SUBSTR(YARD,1,2)='" + conteiner.YARD.Substring(0, 2) + "') ");
                        sb.AppendLine(" Union ");
                        sb.AppendLine(" SELECT YARD FROM INTELOPER..TB_IP_ATRIBUTO_YARD ");
                        sb.AppendLine(" WHERE PATIO = " + conteiner.PATIO + " AND AUTONUM_ATR=21 AND VLR_ATRIB=0 AND STATUS_ATRIB='R' ");
                        sb.AppendLine(" AND DT_INI_ATR < GETDATE() AND (DT_FIM_ATR IS NULL OR DT_FIM_ATR > GETDATE()) ");
                        sb.AppendLine(" AND (LENGTH(YARD)=1 OR LENGTH(YARD)=2)  ");
                        sb.AppendLine(" AND (SUBSTR(YARD,1,1)='" + conteiner.YARD.Substring(0, 1) + "' OR SUBSTR(YARD,1,2)='" + conteiner.YARD.Substring(0, 2) + "') ");

                        sql = sb.ToString();

                        var yardDT = con.Query<ConteinerDTO>(sb.ToString()).FirstOrDefault();

                        if (yardDT != null)
                        {
                            ret = "Quadra APENAS permite conteiner IMO";

                            return ret;
                        }

                        sb.Clear();

                        sb.AppendLine(" SELECT YARD FROM INTELOPER..TB_IP_ATRIBUTO_YARD ");
                        sb.AppendLine(" WHERE PATIO=" + conteiner.PATIO + " AND AUTONUM_ATR=21 AND VLR_ATRIB=1 AND STATUS_ATRIB='E' ");
                        sb.AppendLine(" AND DT_INI_ATR < GETDATE() AND (DT_FIM_ATR IS NULL OR DT_FIM_ATR>GETDATE()) ");

                        if (conteiner.TAMANHO == 20)
                        {
                            sb.AppendLine(" AND SUBSTR(YARD,1,6)='" + conteiner.YARD.Substring(0, 5) + "'");
                        }
                        else
                        {
                            sb.AppendLine(" AND (SUBSTR(YARD,1,6)='" + conteiner.YARD.Substring(0, 1) + conteiner.YARD.Substring(2, 1) + 1 + conteiner.YARD.Substring(4, 1) + "' ");
                            sb.AppendLine(" OR SUBSTR(YARD,1,6)='" + conteiner.YARD.Substring(0, 1) + Convert.ToDecimal(conteiner.YARD.Substring(2, 1) + 1) + conteiner.YARD.Substring(4, 1) + "') ");
                        }


                        sb.AppendLine(" UNION ");

                        sb.AppendLine(" SELECT YARD FROM INTELOPER..TB_IP_ATRIBUTO_YARD ");
                        sb.AppendLine(" WHERE PATIO=" + conteiner.PATIO + " AND AUTONUM_ATR=21 AND VLR_ATRIB=0 AND STATUS_ATRIB='R' ");
                        sb.AppendLine(" AND DT_INI_ATR < GETDATE() AND (DT_FIM_ATR IS NULL OR DT_FIM_ATR>GETDATE()) ");

                        if (conteiner.TAMANHO == 20)
                        {
                            sb.AppendLine(" AND SUBSTR(YARD,1,6)='" + conteiner.YARD.Substring(0, 5) + "' ");
                            sb.AppendLine(" OR SUBSTR(YARD,1,6)='" + conteiner.YARD.Substring(0, 1) + Convert.ToDecimal(conteiner.YARD.Substring(2, 1) + 1) + conteiner.YARD.Substring(4, 1) + "') ");
                        }

                        sql = sb.ToString();

                        var yardDT1 = con.Query<ConteinerDTO>(sql).FirstOrDefault();

                        if (yardDT1 != null)
                        {
                            ret = "Pilha APENAS permite conteiner IMO";

                            return ret;
                        }
                    }
                    else
                    {
                        sb.AppendLine(" SELECT YARD FROM INTELOPER..TB_IP_ATRIBUTO_YARD ");
                        sb.AppendLine(" WHERE PATIO=" + conteiner.PATIO + " AND AUTONUM_ATR=21 AND VLR_ATRIB=1 AND STATUS_ATRIB='E' ");
                        sb.AppendLine(" AND DT_INI_ATR < GETDATE() AND (DT_FIM_ATR IS NULL OR DT_FIM_ATR> GETDATE()) ");
                        sb.AppendLine(" AND YARD='" + conteiner.YARD + "' ");
                        sb.AppendLine(" Union ");
                        sb.AppendLine(" SELECT YARD FROM INTELOPER..TB_IP_ATRIBUTO_YARD ");
                        sb.AppendLine(" WHERE PATIO=" + conteiner.PATIO + " AND AUTONUM_ATR=21 AND VLR_ATRIB=0 AND STATUS_ATRIB='R' ");
                        sb.AppendLine(" AND DT_INI_ATR < GETDATE() AND (DT_FIM_ATR IS NULL OR DT_FIM_ATR> GETDATE() ) ");
                        sb.AppendLine(" AND YARD='" + conteiner.YARD + "' ");

                        sql = sb.ToString();

                        var yardDT2 = con.Query<ConteinerDTO>(sql).FirstOrDefault();

                        if (yardDT2 != null)
                        {
                            ret = "Heap/Rua APENAS permite conteiner IMO";

                            return ret;
                        }
                    }

                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string ValidaRegrasPatio(string id_conteiner, string pilha)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    using (var cmd = new SqlCommand("OPERADOR..fValidaRegrasPatio", _db))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter
                        {
                            ParameterName = "p_QualCntr",
                            SqlDbType = SqlDbType.VarChar,
                            Direction = ParameterDirection.Input,
                            Value = id_conteiner
                        });

                        cmd.Parameters.Add(new SqlParameter
                        {
                            ParameterName = "p_QualPilha",
                            SqlDbType = SqlDbType.VarChar,
                            Direction = ParameterDirection.Input,
                            Value = pilha
                        });

                        cmd.Parameters.Add(new SqlParameter
                        {
                            ParameterName = "l_result",
                            SqlDbType = SqlDbType.VarChar,
                            Direction = ParameterDirection.Output,
                        });

                        _db.Open();

                        var dr = cmd.ExecuteReader();

                        string ret = string.Empty;

                        while (dr.Read())
                        {
                            ret = dr["l_result"].ToString();
                        }

                        return ret;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string Verifica_Regras_Seg_Imo_Delta(ConteinerDTO conteiner)
        {
            string ret = string.Empty;
            string espaco = string.Empty;
            var sbRet = new StringBuilder();
            try
            {

                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    var sb = new StringBuilder();

                    sb.Clear();

                    sb.AppendLine(" Select ISNULL(DIST_IMO_SEGREG1,0) As DIST_IMO_SEGREG1, ISNULL(DIST_IMO_SEGREG2, 0) As DIST_IMO_SEGREG2, ISNULL(DIST_IMO_SEGREG3, 0) As DIST_IMO_SEGREG3, ISNULL(DIST_IMO_SEGREG4, 0) As DIST_IMO_SEGREG4  from sgipa..dte_tb_parametros  ");

                    string sql = sb.ToString();

                    var imo_segreg = _db.Query<ParametrosDTO>(sql).FirstOrDefault();

                    sbRet.AppendLine(" Espaço inferior a " + imo_segreg.DIST_IMO_SEGREG1 + " m");
                    sbRet.AppendLine(" Espaço inferior a " + imo_segreg.DIST_IMO_SEGREG2 + " m");
                    sbRet.AppendLine(" Espaço inferior a " + imo_segreg.DIST_IMO_SEGREG3 + " m");
                    sbRet.AppendLine(" Espaço inferior a " + imo_segreg.DIST_IMO_SEGREG4 + " m");

                    sb.Clear();

                    sb.AppendLine(" Select ID_CONTEINER,ISNULL(IMO1,'') as IMO1,ISNULL(IMO2,'') AS IMO2,ISNULL(IMO3,'') AS IMO3,ISNULL(IMO4,'') AS IMO4, ");
                    sb.AppendLine("  ID_CONTEINER_D, YARD_D, ISNULL(IMO1_D,'') AS IMO1_D, ISNULL(IMO2_D,'') AS IMO2_D, ISNULL(IMO3_D,'') AS IMO3_D, ISNULL(IMO4_D,'') AS IMO4_D, ISNULL(DIST_DELTA,0) as DIST_DELTA ");
                    sb.AppendLine(" FROM OPERADOR..VW_DIST_IMO ");
                    sb.AppendLine(" WHERE PATIO=" + conteiner.PATIO + " AND ID_CONTEINER='" + conteiner.IdConteiner + "' ORDER BY DIST_DELTA");

                    sql = sb.ToString();

                    var query = _db.Query<ConteinerDTO>(sql).FirstOrDefault();

                    if (query != null)
                    {
                        //imo 1

                        sb.Clear();

                        if (string.IsNullOrEmpty(query.IMO1))
                        {
                            if (query.IMO1 != "0")
                            {
                                sb.AppendLine(" Select SEGREGACAO from OPERADOR..tb_segregacao where CLASS1='" + query.IMO1.Replace(".", "") + "' ");
                                sb.AppendLine(" AND CLASS2='" + query.IMO1_D.Replace(".", "") + "'");
                                sb.AppendLine(" AND SEGREGACAO IN ('1','2','3','4') ");
                                sb.AppendLine(" UNION ");
                                sb.AppendLine(" Select SEGREGACAO from OPERADOR..tb_segregacao where CLASS2='" + query.IMO1.Replace(".", "") + "'");
                                sb.AppendLine(" AND CLASS1='" + query.IMO1_D.Replace(".", "") + "'");
                                sb.AppendLine(" AND SEGREGACAO IN ('1','2','3','4') ");

                                sql = sb.ToString();

                                var segr = _db.Query<ConteinerDTO>(sql).FirstOrDefault();

                                if (segr != null)
                                {
                                    if (query.DIST_DELTA < segr.SEGREGACAO)
                                    {
                                        sbRet.AppendLine(" " + segr.SEGREGACAO + " ");
                                        sbRet.AppendLine(" Conteiner :" + query.ID_CONTEINER_D);
                                        sbRet.AppendLine(" Posição: " + query.YARD_D);
                                        sbRet.AppendLine(" Imo : " + query.IMO1_D);

                                        ret = sbRet.ToString();

                                        return ret;

                                    }
                                }
                            }
                        }

                        //imo 2
                        sb.Clear();

                        if (string.IsNullOrEmpty(query.IMO2))
                        {
                            if (query.IMO2 != "0")
                            {
                                sb.AppendLine(" Select SEGREGACAO from OPERADOR..tb_segregacao where CLASS1='" + query.IMO2.Replace(".", "") + "' ");
                                sb.AppendLine(" AND CLASS2='" + query.IMO2_D.Replace(".", "") + "'");
                                sb.AppendLine(" AND SEGREGACAO IN ('1','2','3','4') ");
                                sb.AppendLine(" UNION ");
                                sb.AppendLine(" Select SEGREGACAO from OPERADOR..tb_segregacao where CLASS2='" + query.IMO2.Replace(".", "") + "'");
                                sb.AppendLine(" AND CLASS1='" + query.IMO2_D.Replace(".", "") + "'");
                                sb.AppendLine(" AND SEGREGACAO IN ('1','2','3','4') ");

                                sql = sb.ToString();

                                var segr = _db.Query<ConteinerDTO>(sql).FirstOrDefault();

                                if (segr != null)
                                {
                                    if (query.DIST_DELTA < segr.SEGREGACAO)
                                    {
                                        sbRet.AppendLine(" " + segr.SEGREGACAO + " ");
                                        sbRet.AppendLine(" Conteiner :" + query.ID_CONTEINER_D);
                                        sbRet.AppendLine(" Posição: " + query.YARD_D);
                                        sbRet.AppendLine(" Imo : " + query.IMO2_D);

                                        ret = sbRet.ToString();

                                        return ret;
                                    }
                                }
                            }
                        }

                        //Imo3 

                        sb.Clear();

                        if (string.IsNullOrEmpty(query.IMO3))
                        {
                            if (query.IMO3 != "0")
                            {
                                sb.AppendLine(" Select SEGREGACAO from OPERADOR..tb_segregacao where CLASS1='" + query.IMO3.Replace(".", "") + "' ");
                                sb.AppendLine(" AND CLASS2='" + query.IMO3_D.Replace(".", "") + "'");
                                sb.AppendLine(" AND SEGREGACAO IN ('1','2','3','4') ");
                                sb.AppendLine(" UNION ");
                                sb.AppendLine(" Select SEGREGACAO from OPERADOR..tb_segregacao where CLASS2='" + query.IMO3.Replace(".", "") + "'");
                                sb.AppendLine(" AND CLASS1='" + query.IMO3_D.Replace(".", "") + "'");
                                sb.AppendLine(" AND SEGREGACAO IN ('1','2','3','4') ");

                                sql = sb.ToString();

                                var segr = _db.Query<ConteinerDTO>(sql).FirstOrDefault();

                                if (segr != null)
                                {
                                    if (query.DIST_DELTA < segr.SEGREGACAO)
                                    {
                                        sbRet.AppendLine(" " + segr.SEGREGACAO + " ");
                                        sbRet.AppendLine(" Conteiner :" + query.ID_CONTEINER_D);
                                        sbRet.AppendLine(" Posição: " + query.YARD_D);
                                        sbRet.AppendLine(" Imo : " + query.IMO3_D);

                                        ret = sbRet.ToString();

                                        return ret;

                                    }
                                }
                            }
                        }
                        //Imo4
                        sb.Clear();

                        if (string.IsNullOrEmpty(query.IMO4))
                        {
                            if (query.IMO3 != "0")
                            {
                                sb.AppendLine(" Select SEGREGACAO from OPERADOR..tb_segregacao where CLASS1='" + query.IMO4.Replace(".", "") + "' ");
                                sb.AppendLine(" AND CLASS2='" + query.IMO4_D.Replace(".", "") + "'");
                                sb.AppendLine(" AND SEGREGACAO IN ('1','2','3','4') ");
                                sb.AppendLine(" UNION ");
                                sb.AppendLine(" Select SEGREGACAO from OPERADOR..tb_segregacao where CLASS2='" + query.IMO4.Replace(".", "") + "'");
                                sb.AppendLine(" AND CLASS1='" + query.IMO4_D.Replace(".", "") + "'");
                                sb.AppendLine(" AND SEGREGACAO IN ('1','2','3','4') ");

                                sql = sb.ToString();

                                var segr = _db.Query<ConteinerDTO>(sql).FirstOrDefault();

                                if (segr != null)
                                {
                                    if (query.DIST_DELTA < segr.SEGREGACAO)
                                    {
                                        sbRet.AppendLine(" " + segr.SEGREGACAO + " ");
                                        sbRet.AppendLine(" Conteiner :" + query.ID_CONTEINER_D);
                                        sbRet.AppendLine(" Posição: " + query.YARD_D);
                                        sbRet.AppendLine(" Imo : " + query.IMO4_D);

                                        ret = sbRet.ToString();

                                        return ret;

                                    }
                                }
                            }
                        }
                    }
                }

                return ret;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string GetInserir(MovimentacaoContainerDTO conteiner)
        {
            try
            {
                var sb = new StringBuilder();

                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    var stp = "REDEX..FN_MOV_CNTR2";

                    DynamicParameters dp = new DynamicParameters();


                    dp.Add("ID_Conteiner", conteiner.ID_CONTEINER, DbType.String, ParameterDirection.Input);
                    dp.Add("AutonumCntr", conteiner.AUTONUM_CNTR, DbType.Int32, ParameterDirection.Input);
                    dp.Add("Sistema", "R", DbType.String, ParameterDirection.Input);
                    dp.Add("Tamanho", conteiner.TAMANHO, DbType.Int32, ParameterDirection.Input);
                    dp.Add("Patio", conteiner.PATIO, DbType.Int32, ParameterDirection.Input);

                    dp.Add("YardOrigem", conteiner.YARD, DbType.String, ParameterDirection.Input);
                    dp.Add("YardDestino", conteiner.YARD_DESTINO, DbType.String, ParameterDirection.Input);
                    dp.Add("Motivo", conteiner.ID_MOTIVO, DbType.Int32, ParameterDirection.Input);
                    dp.Add("Usuario", conteiner.ID_USUARIO, DbType.Int32, ParameterDirection.Input);
                    dp.Add("Veiculo", 0, DbType.Int32, ParameterDirection.Input);
                    dp.Add("Empilhadeira", 0, DbType.Int32, ParameterDirection.Input);

                    dp.Add("BROWSER_NAME", string.Empty, DbType.String, ParameterDirection.Input);
                    dp.Add("BROWSER_VERSION", string.Empty, DbType.String, ParameterDirection.Input);
                    dp.Add("MOBILEDEVICEMODEL", string.Empty, DbType.String, ParameterDirection.Input);
                    dp.Add("MOBILEDEVICEMANUFACTURER", string.Empty, DbType.String, ParameterDirection.Input);
                    dp.Add("FLAG_MOBILE", 0, DbType.Int32, ParameterDirection.Input);

                    dp.Add("RETORNO", dbType: DbType.String, direction: ParameterDirection.Output, size: 300);

                    var query = _db.Query<MovimentacaoContainerDTO>(stp, dp, commandType: CommandType.StoredProcedure).FirstOrDefault();


                    string ret = dp.Get<string>("RETORNO");

                    return ret;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int GetCampoValida(int patio, string yard)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    var sb = new StringBuilder();

                    sb.AppendLine(" SELECT ISNULL(valida,0) AS VALIDA FROM OPERADOR..TB_YARD WHERE PATIO=" + patio + " AND YARD = '" + yard + "' ");

                    string sql = sb.ToString();

                    int valida = _db.Query<int>(sql).FirstOrDefault();

                    return valida;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int BuscaCntrPorVeiculo(int valor)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    var sb = new StringBuilder();

                    sb.AppendLine(" SELECT C.AUTONUM,ID_CONTEINER FROM OPERADOR..TB_GATE_NEW ");
                    sb.AppendLine(" A INNER JOIN OPERADOR..TB_AMR_GATE B ON A.AUTONUM = B.GATE ");
                    sb.AppendLine(" INNER JOIN TB_CNTR_BL C ON B.CNTR_IPA = C.AUTONUM  ");
                    sb.AppendLine(" WHERE A.AUTONUM = " + valor);

                    string sql = sb.ToString();

                    int cntr = _db.Query<int>(sql).FirstOrDefault();

                    return cntr;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ConteinerDTO countCntr(int patio, string id_conteiner)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    var sb = new StringBuilder();

                    sb.AppendLine(" SELECT COUNT(0) as CCNTR ,MAX(ID_CONTEINER) AS ID_CONTEINER FROM  ");
                    sb.AppendLine(" REDEX..VW_INVENT_SISTEMAS_PY ");
                    sb.AppendLine(" WHERE 0 = 0 ");
                    sb.AppendLine(" AND  ");
                    sb.AppendLine(" PATIO = " + patio);
                    sb.AppendLine(" AND ");
                    sb.AppendLine(" (REPLACE(SUBSTRING(ID_CONTEINER,8,5),'-','')='" + id_conteiner + "'  or id_conteiner='" + id_conteiner + "')");

                    var count = _db.Query<ConteinerDTO>(sb.ToString()).FirstOrDefault();

                    return count;


                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IEnumerable<ConteinerDTO> getInfoLote(string id_conteiner)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    var sb = new StringBuilder();

                    sb.AppendLine(" SELECT DISTINCT E.AUTONUM, C.ID_CONTEINER  + ' - ' + YARD  + ' - LOTE:' + E.AUTONUM AS DESCR   FROM OPERADOR..TB_GATE_NEW A INNER JOIN OPERADOR..TB_AMR_GATE B ON A.AUTONUM = B.GATE INNER JOIN SGIPA..TB_CNTR_BL C ON B.CNTR_IPA = C.AUTONUM INNER JOIN SGIPA..TB_AMR_CNTR_BL D ON D.CNTR = C.AUTONUM INNER JOIN SGIPA..TB_BL E ON D.BL= E.AUTONUM WHERE B.FUNCAO_GATE = 11 AND A.DT_GATE_IN IS NOT NULL AND ID_CONTEINER =  '" + id_conteiner + "'");

                    string sql = sb.ToString();

                    var conteiner = _db.Query<ConteinerDTO>(sql).AsEnumerable();

                    return conteiner;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int GetAutonum(string id_conteiner)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    var sb = new StringBuilder();

                    sb.AppendLine(" SELECT AUTONUM FROM SGIPA..TB_CNTR_BL WHERE ID_CONTEINER =  '" + id_conteiner + "'");

                    string sql = sb.ToString();

                    int conteiner = _db.Query<int>(sql).FirstOrDefault();

                    return conteiner;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string ConsultarMotorista(int id)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    var sb = new StringBuilder();

                    sb.AppendLine(" SELECT nome from redex..tb_motoristas where autonum =  '" + id + "'");



                    var nome = _db.Query<string>(sb.ToString()).FirstOrDefault();

                    return nome;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Placa_Pendente(string placa)
        {
            int count = 0;
            try
            {
                using (var db = new SqlConnection(Config.StringConexao()))
                {
                    var sb = new StringBuilder();

                    sb.AppendLine(" SELECT COUNT(1) FROM  REDEX..TB_PRE_REGISTRO WHERE PLACA = '" + placa + "' AND DATA_SAIDA IS NULL ");

                    count = db.Query<int>(sb.ToString()).FirstOrDefault();


                    if (count != 0)
                        return true;

                    sb.Clear();
                    sb.AppendLine(" SELECT COUNT(1) FROM REDEX..TB_GATE_NEW WHERE PLACA = '" + placa + "' AND ISNULL(FLAG_GATE_OUT, 0 )  = 0");

                    count = db.Query<int>(sb.ToString()).FirstOrDefault();

                    if (count != 0)
                        return true;


                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
