using Band.Coletor.Redex.Business.DTO;
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
    public class IventarioCNTRRepositorio : IIventarioCNTRRepositorio
    {
        public IEnumerable<MotivosDTO> GetListarMotivos()
        {
            try
            {
                using (var _Db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" SELECT ");
                    sb.AppendLine(" AUTONUM, DESCRICAO  ");
                    sb.AppendLine(" FROM  ");
                    sb.AppendLine(" OPERADOR..TB_CAD_MOTIVO ");
                    sb.AppendLine(" ORDER BY DESCRICAO ");

                    var query = _Db.Query<MotivosDTO>(sb.ToString()).AsEnumerable();

                    return query;

                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public IEnumerable<EmpilhadeirasDTO> GetListarEmpilhadeiras(int id)
        {
            try
            {
                using (var _Db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" SELECT ");
                    sb.AppendLine(" AUTONUM,  ");
                    sb.AppendLine(" IDENTIFICACAO ");
                    sb.AppendLine(" FROM  ");
                    sb.AppendLine(" OPERADOR..TB_Frota ");
                    sb.AppendLine(" WHERE  ");
                    sb.AppendLine(" PATIO =  " + id);
                    sb.AppendLine("  AND ");
                    sb.AppendLine(" TIPO_VEICULO = 'E' ");
                    sb.AppendLine(" AND  ");
                    sb.AppendLine(" FLAG_ATIVO = 1 ");
                    sb.AppendLine(" ORDER BY IDENTIFICACAO ");


                    var query = _Db.Query<EmpilhadeirasDTO>(sb.ToString()).AsEnumerable();

                    return query;
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public IEnumerable<VeiculosDTO> GetListarVeiculos(int id)
        {
            try
            {
                using (var _Db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" SELECT ");
                    sb.AppendLine(" AUTONUM, IDENTIFICACAO ");
                    sb.AppendLine(" FROM  ");
                    sb.AppendLine(" REDEX.TB_Frota ");
                    sb.AppendLine(" WHERE  ");
                    sb.AppendLine(" PATIO =  " + id);
                    sb.AppendLine("  AND ");
                    sb.AppendLine(" TIPO_VEICULO = 'C' ");
                    sb.AppendLine(" AND  ");
                    sb.AppendLine(" FLAG_ATIVO = 1 ");
                    sb.AppendLine(" ORDER BY IDENTIFICACAO ");


                    var query = _Db.Query<VeiculosDTO>(sb.ToString()).AsEnumerable();

                    return query;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public int GetFlagTruckMovColetor(int id)
        {
            int truckMov = 0;
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" SELECT ISNULL(Flag_Truck_Mov_Coletor,0) as Qual FROM OPERADOR..TB_PATIOS WHERE AUTONUM = " + id);

                    truckMov = _db.Query<int>(sb.ToString()).FirstOrDefault();

                    return truckMov;                        
                }
            }
            catch (Exception ex)
            {
                return truckMov;
            }
        }
        public IventarioCNTRDTO GetIDContainerSubstring(int patio, string container)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" SELECT ");
                    sb.AppendLine("  COUNT(0) as COUNT_CONTEINER ");
                    sb.AppendLine("  ,MAX(ID_CONTEINER) AS ID_CONTEINER  ");
                    sb.AppendLine("  FROM ");
                    sb.AppendLine(" OPERADOR..VW_INVENT_SISTEMAS_PY ");
                    sb.AppendLine(" WHERE  ");
                    sb.AppendLine(" PATIO =  " + patio);
                    sb.AppendLine(" AND  ");
                    sb.AppendLine(" REPLACE(SUBSTRING(ID_CONTEINER, 8, 5), '-', '') =  '" + container.Replace("-", "") + "' ");


                    var query = _db.Query<IventarioCNTRDTO>(sb.ToString()).FirstOrDefault();

                    return query;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public IventarioCNTRDTO GetIDContainer(int patio, string container)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" SELECT ");
                    sb.AppendLine("  COUNT(0) as COUNT_CONTEINER ");
                    sb.AppendLine("  ,MAX(ID_CONTEINER) AS ID_CONTEINER  ");
                    sb.AppendLine("  FROM ");
                    sb.AppendLine(" OPERADOR..VW_INVENT_SISTEMAS_PY ");
                    sb.AppendLine(" WHERE  ");
                    sb.AppendLine(" PATIO =  " + patio);
                    sb.AppendLine(" AND  ");
                    sb.AppendLine(" ID_CONTEINER =  '" + container + "' ");
                    //sb.AppendLine(" REPLACE(ID_CONTEINER,'-','') =  '" + container.Replace("-", "") + "' ");


                    var query = _db.Query<IventarioCNTRDTO>(sb.ToString()).FirstOrDefault();

                    return query;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
         }
        public IventarioCNTRDTO InserirCTNR(IventarioCNTRDTO obj)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();


                    _db.Query<IventarioCNTRDTO>(sb.ToString()).FirstOrDefault();

                    return null;                   
                        
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public IventarioCNTRDTO UpdateCTNRBL(string yard, int id)
        {
            try
            {
                using (var _Db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" UPDATE SGIPA..TB_CNTR_BL Set YARD = '" + yard + "' WHERE AUTONUM =  " + id);

                    _Db.Query<IventarioCNTRDTO>(sb.ToString()).FirstOrDefault();

                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public IventarioCNTRDTO UpdatePatioByYard(string yard, int id)
        {
            try
            {
                using (var _Db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" UPDATE REDEX..TB_PATIO Set YARD= '" + yard + "' WHERE AUTONUM_PATIO = " + id);

                    _Db.Query<IventarioCNTRDTO>(sb.ToString()).FirstOrDefault();

                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public IventarioCNTRDTO UpdatePatioById(string yard, int id)
        {
            try
            {
                using (var _Db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" UPDATE REDEX..TB_PATIO Set YARD= '" + yard + "' WHERE AUTONUM = " + id);

                    _Db.Query<IventarioCNTRDTO>(sb.ToString()).FirstOrDefault();

                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public IventarioCNTRDTO UpdateArmazemIPAByYard(string yard, int id)
        {
            try
            {
                using (var _Db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" UPDATE SGIPA..TB_ARMAZENS_IPA Set YARD = '" + yard + "' WHERE AUTONUM =  " + id);

                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public IventarioCNTRDTO Insert_HIST_SHIFITING(IventarioCNTRDTO obj)
        {
            try
            {
                using (var _Db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" INSERT INTO OPERADOR..TB_HIST_SHIFTING ");
                    sb.AppendLine(" ( ");
                    sb.AppendLine(" CNTR, ");
                    sb.AppendLine(" ORIGEM,  ");
                    sb.AppendLine(" DESTINO,  ");
                    sb.AppendLine(" Data, ");
                    sb.AppendLine(" MOTIVO, ");
                    sb.AppendLine(" TIPO, ");
                    sb.AppendLine(" USUARIO, ");
                    sb.AppendLine(" ID_TRANSPORTADORA,  ");
                    sb.AppendLine(" AUTONUM_FROTA_CARRETA, ");
                    sb.AppendLine(" AUTONUM_FROTA_EMPILHADEIRA ");
                    sb.AppendLine(" ) ");
                    sb.AppendLine(" VALUES ");
                    sb.AppendLine(" ( ");
                    sb.AppendLine(" @CNTR, ");
                    sb.AppendLine(" @ORIGEM,  ");
                    sb.AppendLine(" @DESTINO,  ");
                    sb.AppendLine(" GetDate(), ");
                    sb.AppendLine(" @MOTIVO, ");
                    sb.AppendLine(" @TIPO, ");
                    sb.AppendLine(" @USUARIO, ");
                    sb.AppendLine(" @ID_TRANSPORTADORA,  ");
                    sb.AppendLine(" @AUTONUM_FROTA_CARRETA, ");
                    sb.AppendLine(" @AUTONUM_FROTA_EMPILHADEIRA ");
                    sb.AppendLine(" ) ");

                    _Db.Query<IventarioCNTRDTO>(sb.ToString(), new
                    {
                        CNTR = obj.CNTR,
                        ORIGEM = obj.ORIGEM, 
                        DESTINO = obj.DESTINO, 
                        MOTIVO = obj.MOTIVO, 
                        TIPO = obj.TIPO, 
                        USUARIO = obj.USUARIO, 
                        ID_TRANSPORTADORA = obj.ID_TRANSPORTADORA,
                        AUTONUM_FROTA_CARRETA  = obj.AUTONUM_FROTA_CARRETA,
                        AUTONUM_FROTA_EMPILHADEIRA = obj.AUTONUM_FROTA_EMPILHADEIRA, 

                    }).FirstOrDefault();

                    return null;

                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public IventarioCNTRDTO GetBuscaCTNR(string id)
        {
            try
            {
                using (var _Db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" SELECT  ");
                    sb.AppendLine(" IMPEXP ");
                    sb.AppendLine(" ,DESCRICAO ");
                    sb.AppendLine(" ,EF ");
                    sb.AppendLine(" ,DATA_ENT_TEMP ");
                    sb.AppendLine(" ,SCALE ");
                    sb.AppendLine(" ,BRUTO ");
                    sb.AppendLine(" ,IMO1 ");
                    sb.AppendLine(" ,NOME ");
                    sb.AppendLine(" ,YARD ");
                    sb.AppendLine(" ,TAMANHO ");
                    sb.AppendLine(" ,TEMPERATURE ");
                    sb.AppendLine(" ,TIPOBASICO ");
                    sb.AppendLine(" ,DESCR_MOTIVO_POSIC ");
                    sb.AppendLine(" ,FINALITY ");
                    sb.AppendLine(" ,FLAG_OOG ");
                    sb.AppendLine(" ,FLAG_SPC ");
                    sb.AppendLine(" ,DLV_TERM ");
                    sb.AppendLine(" ,AUTONUM ");
                    sb.AppendLine(" ,SISTEMA ");
                    sb.AppendLine(" FROM ");
                    sb.AppendLine(" OPERADOR..VW_INVENT_SISTEMAS_P1 ");
                    sb.AppendLine(" WHERE  ");
                    sb.AppendLine(" ID_CONTEINER =   '" + id  + "' ");

                    var query = _Db.Query<IventarioCNTRDTO>(sb.ToString()).FirstOrDefault();

                    return query;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public ParametrosDTO GetEspacoMin()
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" Select ISNULL(DIST_IMO_SEGREG1,0) AS DIST_IMO_SEGREG1,ISNULL(DIST_IMO_SEGREG2,0) AS DIST_IMO_SEGREG2,ISNULL(DIST_IMO_SEGREG3,0) AS DIST_IMO_SEGREG3,ISNULL(DIST_IMO_SEGREG4,0) AS DIST_IMO_SEGREG4  from  REDEX.dte_tb_parametros ");

                    var query = _db.Query<ParametrosDTO>(sb.ToString()).FirstOrDefault();

                    return query;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public IEnumerable<IventarioCNTRDTO> GetDadosDistDelta(int idPatio, string idConteiner)
        {
            try
            {
                using (var _Db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" SELECT  ");
                    sb.AppendLine(" ID_CONTEINER ");
                    sb.AppendLine(" ID_CONTEINER_D ");
                    sb.AppendLine(" ,ISNULL(IMO1,'') as IMO1 ");
                    sb.AppendLine(" ,ISNULL(IMO2,'') as IMO2 ");
                    sb.AppendLine(" ,ISNULL(IMO3,'') as IMO3 ");
                    sb.AppendLine(" ,ISNULL(IMO4,'') as IMO4 ");
                    sb.AppendLine(" ,ISNULL(IMO1_D,'') as IMO1_D ");
                    sb.AppendLine(" ,ISNULL(IMO2_D,'') as IMO2_D ");
                    sb.AppendLine(" ,ISNULL(IMO3_D,'') as IMO3_D ");
                    sb.AppendLine(" ,ISNULL(IMO4_D,'') as IMO4_D ");
                    sb.AppendLine(" ,ISNULL(DIST_DELTA,0) as DIST_DELTA ");
                    sb.AppendLine(" ,YARD_D ");
                    sb.AppendLine(" FROM  ");
                    sb.AppendLine(" VW_DIST_IMO ");
                    sb.AppendLine(" WHERE ");
                    sb.AppendLine(" PATIO = " + idPatio);
                    sb.AppendLine(" AND ");
                    sb.AppendLine(" ID_CONTEINER = " + idConteiner);
                    sb.AppendLine(" ORDER BY DIST_DELTA ");

                    var query = _Db.Query<IventarioCNTRDTO>(sb.ToString()).AsEnumerable();

                    return query;

                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public YardDTO GetValidaYard(int idPatio, string Yard)
        {
            try
            {
                using (var _Db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" SELECT YARD, ISNULL(VALIDA,0) AS VALIDA, ISNULL(FLAG_RUA,0) AS FLAG_RUA, ISNULL(FLAG_BLOQUEIO,1) AS FLAG_BLOQUEIO FROM REDEX.TB_YARD WHERE PATIO =  " + idPatio + " AND  YARD =  '" + Yard + "'  ");

                    var query = _Db.Query<YardDTO>(sb.ToString()).FirstOrDefault();

                    return query;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public IventarioCNTRDTO GetInventSistemasXYZ(string id)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" SELECT ");
                    sb.AppendLine(" ID_CONTEINER ");
                    sb.AppendLine(" ,ISNULL(IMO1,'') AS IMO1 ");
                    sb.AppendLine(" ,ISNULL(IMO2,'') AS IMO2 ");
                    sb.AppendLine(" ,ISNULL(IMO3,'') AS IMO3 ");
                    sb.AppendLine(" ,ISNULL(IMO4,'') AS IMO4 ");
                    sb.AppendLine(" FROM  ");
                    sb.AppendLine(" REDEX.VW_INVENT_SISTEMAS_IMO_XYZ ");
                    sb.AppendLine(" WHERE ");
                    sb.AppendLine(" ID_CONTEINER = '" + id + "' ");

                    var query = _db.Query<IventarioCNTRDTO>(sb.ToString()).FirstOrDefault();

                    return query;

                }
            }
            catch (Exception ex)
            {
                return null;

            }
        }
        public IventarioCNTRDTO GetRegrasImoQuadra(int idPatio, string Yard)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" SELECT  ");
                    sb.AppendLine(" YARD FROM INTELOPER..TB_IP_ATRIBUTO_YARD  ");
                    sb.AppendLine(" WHERE PATIO=" + idPatio + " AND AUTONUM_ATR=21 AND VLR_ATRIB=1 AND STATUS_ATRIB='R' ");
                    sb.AppendLine(" AND DT_INI_ATR < GETDATE() AND (DT_FIM_ATR IS NULL OR DT_FIM_ATR> GETDATE() ) ");
                    sb.AppendLine(" AND (LEN(YARD)=1 OR LEN(YARD)=2)  ");
                    sb.AppendLine(" AND (SUBSTRING(YARD,1,1)='" + Yard.Substring(1, 1) + "' OR SUBSTRING(YARD,1,2)='" + Yard.Substring(1, 2) + "') ");
                    sb.AppendLine(" UNION  ");
                    sb.AppendLine(" SELECT  ");
                    sb.AppendLine(" YARD FROM INTELOPER..TB_IP_ATRIBUTO_YARD  ");
                    sb.AppendLine(" WHERE PATIO=" + idPatio + " AND AUTONUM_ATR=21 AND VLR_ATRIB=0 AND STATUS_ATRIB='E' ");
                    sb.AppendLine(" AND DT_INI_ATR < GETDATE() AND (DT_FIM_ATR IS NULL OR DT_FIM_ATR> GETDATE() ) ");
                    sb.AppendLine(" AND (LEN(YARD)=1 OR LEN(YARD)=2)  ");
                    sb.AppendLine(" AND (SUBSTRING(YARD,1,1)='" + Yard.Substring(1, 1) + "' OR SUBSTRING(YARD,1,2)='" + Yard.Substring(1, 2) + "') ");

                    var query = _db.Query<IventarioCNTRDTO>(sb.ToString()).FirstOrDefault();

                    return query;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public IventarioCNTRDTO GetRegrasImoPilhas(int idPatio, string Yard, int tamanho)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" SELECT  ");
                    sb.AppendLine(" YARD FROM INTELOPER..TB_IP_ATRIBUTO_YARD  ");
                    sb.AppendLine(" WHERE PATIO=" + idPatio + " AND AUTONUM_ATR=21 AND VLR_ATRIB=1 AND STATUS_ATRIB='R' ");
                    sb.AppendLine(" AND DT_INI_ATR < GETDATE() AND (DT_FIM_ATR IS NULL OR DT_FIM_ATR> GETDATE() ) ");
                    if (tamanho == 20)
                    {
                        sb.AppendLine(" AND SUBSTRING(YARD,1,6)= '" + Yard.Substring(1, 6) + "' ");
                    }
                    else
                    {
                        //Colocar o else aqui com as validacoes necessarias 
                        //Sql = Sql & " AND (SUBSTRING(YARD,1,6)='" & Mid$(Yard, 1, 2) & Format(Val(Mid$(Yard, 3, 2)) + 1, "00") & Mid$(Yard, 5, 2) & "' OR SUBSTRING(YARD,1,6)='" & Mid$(Yard, 1, 2) & Format(Val(Mid$(Yard, 3, 2)) - 1, "00") & Mid$(Yard, 5, 2) & "')"
                    }
                    sb.AppendLine(" UNION  ");
                    sb.AppendLine(" SELECT  ");
                    sb.AppendLine(" YARD FROM INTELOPER..TB_IP_ATRIBUTO_YARD  ");
                    sb.AppendLine(" WHERE PATIO=" + idPatio + " AND AUTONUM_ATR=21 AND VLR_ATRIB=0 AND STATUS_ATRIB='E' ");
                    sb.AppendLine(" AND DT_INI_ATR < GETDATE() AND (DT_FIM_ATR IS NULL OR DT_FIM_ATR> GETDATE() ) ");
                    sb.AppendLine(" AND (LEN(YARD)=1 OR LEN(YARD)=2)  ");
                    if (tamanho == 20)
                    {
                        sb.AppendLine(" AND SUBSTRING(YARD,1,6)= '" + Yard.Substring(1, 6) + "' ");
                    }
                    else
                    {
                        //Colocar o else aqui com as validacoes necessarias  como na linha abaixo 

                        //Sql = Sql & " AND (SUBSTRING(YARD,1,6)='" & Mid$(Yard, 1, 2) & Format(Val(Mid$(Yard, 3, 2)) + 1, "00") & Mid$(Yard, 5, 2) & "' OR SUBSTRING(YARD,1,6)='" & Mid$(Yard, 1, 2) & Format(Val(Mid$(Yard, 3, 2)) - 1, "00") & Mid$(Yard, 5, 2) & "')"
                    }

                    var query = _db.Query<IventarioCNTRDTO>(sb.ToString()).FirstOrDefault();

                    return query;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public IventarioCNTRDTO GetRegrasConteinerRua(int idPatio, string Yard)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" SELECT  ");
                    sb.AppendLine(" YARD FROM INTELOPER..TB_IP_ATRIBUTO_YARD  ");
                    sb.AppendLine(" WHERE PATIO=" + idPatio + " AND AUTONUM_ATR=21 AND VLR_ATRIB=1 AND STATUS_ATRIB='R' ");
                    sb.AppendLine(" AND DT_INI_ATR < GETDATE() AND (DT_FIM_ATR IS NULL OR DT_FIM_ATR> GETDATE() ) ");
                    sb.AppendLine(" AND YARD = '" + Yard + "' ");

                    sb.AppendLine(" UNION  ");
                    sb.AppendLine(" SELECT  ");
                    sb.AppendLine(" YARD FROM INTELOPER..TB_IP_ATRIBUTO_YARD  ");
                    sb.AppendLine(" WHERE PATIO=" + idPatio + " AND AUTONUM_ATR=21 AND VLR_ATRIB=0 AND STATUS_ATRIB='E' ");
                    sb.AppendLine(" AND DT_INI_ATR < GETDATE() AND (DT_FIM_ATR IS NULL OR DT_FIM_ATR> GETDATE() ) ");
                    sb.AppendLine(" AND YARD = '" + Yard + "' ");

                    var query = _db.Query<IventarioCNTRDTO>(sb.ToString()).FirstOrDefault();

                    return query;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public IventarioCNTRDTO getUpdateCNTR_BL(string yard, int id)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" UPDATE SGIPA..TB_CNTR_BL Set YARD= '" + yard + "' WHERE AUTONUM = " + id);

                    _db.Query<IventarioCNTRDTO>(sb.ToString()).FirstOrDefault();

                    return null;

                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public IventarioCNTRDTO getUpdatePatio(string yard, int id)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" UPDATE REDEX..TB_PATIO SET YARD = '" + yard + "' WHERE AUTONUM_PATIO = " + id);

                    _db.Query<IventarioCNTRDTO>(sb.ToString()).FirstOrDefault();

                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public IventarioCNTRDTO GetUpdateArmazens(string yard, int id)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine("  UPDATE SGIPA..TB_ARMAZENS_IPA SET YARD = '" + yard + "' WHERE AUTONUM =  " + id);

                    _db.Query<IventarioCNTRDTO>(sb.ToString()).FirstOrDefault();

                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public IventarioCNTRDTO CountYardByID(int idPatio, string Yard)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" SELECT  ");
                    sb.AppendLine(" COUNT(AUTONUM) as COUNT_YARD ");
                    sb.AppendLine(" FROM ");
                    sb.AppendLine(" OPERADOR.DBO.TB_YARD  ");
                    sb.AppendLine(" WHERE  ");
                    sb.AppendLine(" PATIO = " + idPatio);
                    sb.AppendLine(" AND ");
                    sb.AppendLine(" YARD =  '" + Yard + "' ");
                    sb.AppendLine(" AND ");
                    sb.AppendLine(" VALIDA = 0 ");
                    sb.AppendLine(" AND ");
                    sb.AppendLine(" FLAG_RUA = 0 ");

                    var query = _db.Query<IventarioCNTRDTO>(sb.ToString()).FirstOrDefault();

                    return query;

                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
