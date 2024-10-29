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
    public class MovimentacaoCargaSoltaRepositorio : IMovimentacaoCargaSoltaRepositorio
    {
        public string GetLoteArmazemMarcante(string marcante, int patio)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    var sb = new StringBuilder();

                    sb.AppendLine(" SELECT AUTONUMCS FROM REDEX..P_INVENT_ARMAZEM_MARCANTE('" + marcante + "') ");

                    string autonumcs = _db.Query<string>(sb.ToString()).FirstOrDefault();

                    return autonumcs;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public MovimentacaoCargaSoltaDTO GetDadosInventArmazemCol_P(string marcante)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    var sb = new StringBuilder();
                    ///Campos comentados que não serão usados.

                    sb.AppendLine(" SELECT LOTE_STR,  ");
                    sb.AppendLine(" MERCADORIA,  ");
                    sb.AppendLine(" MARCA,  ");
                    sb.AppendLine(" DATA_ENTRADA, ");
                    sb.AppendLine(" IMPORTADOR,  ");
                    sb.AppendLine(" VOLUME,  ");
                    sb.AppendLine(" TIPO_DOC, ");
                    sb.AppendLine(" isnull(IMO, '') as IMO,  ");
                    sb.AppendLine("  BL,   ");
                    sb.AppendLine(" EMBALAGEM,  ");
                    sb.AppendLine(" SISTEMA,  ");
                    sb.AppendLine(" REFERENCE,  ");
                    sb.AppendLine(" isnull(ONU, '') as ONU, ");
                    sb.AppendLine(" QUANTIDADE,  ");
                    sb.AppendLine(" LOCAL,  ");
                    sb.AppendLine(" NUM_NF as NUM_NF ");
                    sb.AppendLine(" FROM  ");
                    sb.AppendLine(" REDEX..P_INVENT_ARMAZEM_COL_P('" + marcante + "') ");

                    var query = _db.Query<MovimentacaoCargaSoltaDTO>(sb.ToString()).FirstOrDefault();

                    return query;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public MovimentacaoCargaSoltaItemDTO GetDadosInventArmazemCol_P_Item(string marcante)
        {
            try
            {
                using (var _db = new SqlConnection())
                {
                    //using (var cmd = new SqlCommand("REDEX..P_INVENT_ARMAZEM_ITEM", _db))
                    //{
                    //    cmd.CommandType = CommandType.StoredProcedure;


                    //    cmd.Parameters.Add(new SqlParameter
                    //    {
                    //        ParameterName = "P_MARCANTE",
                    //        SqlDbType = SqlDbType.VarChar,
                    //        Direction = ParameterDirection.Input, 
                    //        Value = marcante
                    //    });

                    //    _db.Open();
                    //    var dr = cmd.ExecuteReader();


                    //    var movimentacaoCSItem = new MovimentacaoCargaSoltaItemDTO();

                    //    while (dr.Read())
                    //    {
                    //        movimentacaoCSItem.QTDE = Convert.ToInt32(dr["QTDE"].ToString());
                    //        movimentacaoCSItem.EMBALAGEM = dr["EMBALAGEM"].ToString();
                    //        movimentacaoCSItem.LOCAL = dr["LOCAL"].ToString();
                    //        movimentacaoCSItem.YARD = dr["YARD"].ToString(); 
                    //    }

                    //    return movimentacaoCSItem; 
                    //}

                    var sb = new StringBuilder();

                    sb.AppendLine(" SELECT  ");
                    sb.AppendLine(" QTDE, ");
                    sb.AppendLine(" EMBALAGEM, ");
                    sb.AppendLine(" LOCAL,  ");
                    sb.AppendLine(" YARD ");
                    sb.AppendLine(" FROM  ");
                    sb.AppendLine(" REDEX..P_INVENT_ARMAZEM_ITEM('" + marcante + "') ");

                    var query = _db.Query<MovimentacaoCargaSoltaItemDTO>(sb.ToString()).FirstOrDefault();

                    return query;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IEnumerable<MovimentacaoCargaSoltaItemDTO> GetDadosInventArmazemCol_P_Item_Lote(string lote, string marcante)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    //using (var cmd = new SqlCommand("REDEX..P_INVENT_ARMAZEM_ITEM_LM", _db))
                    //{
                    //    cmd.CommandType = CommandType.StoredProcedure;

                    //    cmd.Parameters.Add(new SqlParameter
                    //    {
                    //        ParameterName = "P_MARCANTE",
                    //        SqlDbType = SqlDbType.VarChar,
                    //        Direction = ParameterDirection.Input, 
                    //        Value = marcante
                    //    });

                    //    cmd.Parameters.Add(new SqlParameter
                    //    {
                    //        ParameterName = "P_LOTE",
                    //        SqlDbType = SqlDbType.VarChar,
                    //        Direction = ParameterDirection.Input,
                    //        Value = lote
                    //    });

                    //    _db.Open();
                    //    var dr = cmd.ExecuteReader(); 

                    //    var list_movimentacao_cs = new List<MovimentacaoCargaSoltaItemDTO>(); 


                    //    while (dr.Read())
                    //    {
                    //        var movimentacao_cs = new MovimentacaoCargaSoltaItemDTO();

                    //        movimentacao_cs.AUTONUM = Convert.ToInt32(dr["AUTONUM"].ToString());
                    //        movimentacao_cs.DISPLAY = dr["DISPLAY"].ToString();

                    //        list_movimentacao_cs.Add(movimentacao_cs); 
                    //    }

                    //    return list_movimentacao_cs;
                    //}

                    var sb = new StringBuilder();

                    sb.AppendLine(" SELECT  ");
                    sb.AppendLine(" AUTONUM, ");
                    sb.AppendLine(" DISPLAY ");
                    sb.AppendLine(" FROM  ");
                    sb.AppendLine(" REDEX..P_INVENT_ARMAZEM_ITEM_LM(" + marcante + ", '" + lote + "') ");

                    var query = _db.Query<MovimentacaoCargaSoltaItemDTO>(sb.ToString()).AsEnumerable();

                    return query;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int GetIDCsIpa(int id)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    var sb = new StringBuilder();

                    sb.AppendLine(" SELECT AUTONUM_CS FROM SGIPA..TB_CARGA_SOLTA_YARD WHERE AUTONUM= " + id);

                    string sql = sb.ToString();

                    int autonum = _db.Query<int>(sql).FirstOrDefault();

                    return autonum;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int GetIDCsRedex(int id)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    var sb = new StringBuilder();

                    sb.AppendLine(" Select AUTONUM_PATIOCS As AUTONUM_CS FROM REDEX..TB_CARGA_SOLTA_YARD WHERE AUTONUM= " + id);

                    string sql = sb.ToString();

                    int autonum = _db.Query<int>(sql).FirstOrDefault();

                    return autonum;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int GetAutonumCS(string marcante)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    //using (var cmd = new SqlCommand("REDEX..P_INVENT_ARMAZEM_MARCANTE", _db))
                    //{
                    //    cmd.CommandType = CommandType.StoredProcedure;

                    //    cmd.Parameters.Add(new SqlParameter()
                    //    {
                    //        ParameterName = "P_MARCANTE",
                    //        SqlDbType = SqlDbType.VarChar,
                    //        Direction = ParameterDirection.Input, 
                    //        Value = marcante 
                    //    });

                    //    _db.Open();
                    //    var dr = cmd.ExecuteReader();

                    //    while (dr.Read())
                    //    {
                    //        autonum = Convert.ToInt32(dr["AUTONUMCS"].ToString());
                    //    }

                    //    return autonum; 
                    //}

                    var sb = new StringBuilder();

                    sb.AppendLine(" SELECT AUTONUM_CS FROM  REDEX..P_INVENT_ARMAZEM_MARCANTE(" + marcante + ")");

                    var query = _db.Query<int>(sb.ToString()).FirstOrDefault();

                    return query;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int GetNextValIPA()
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    var sb = new StringBuilder();
                    sb.AppendLine(" INSERT INTO SGIPA..SEQ_CARGA_SOLTA_YARD(DATA)values(GETDATE() SELECT CAST(SCOPE_IDENTITY() AS int) ) ");

                    string sql = sb.ToString();

                    int id = _db.Query<int>(sql).FirstOrDefault();

                    return id;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int GetNextValRedex()
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    var sb = new StringBuilder();
                    sb.AppendLine(" INSERT INTO REDEX..SEQ_CARGA_SOLTA_YARD(DATA)values(GETDATE()) SELECT CAST(SCOPE_IDENTITY() AS int) ");

                    string sql = sb.ToString();

                    int id = _db.Query<int>(sql).FirstOrDefault();

                    return id;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int InsertCargaSoltaYard(InserirNovaMovimentacaoCargaSoltaDTO movimentacaoCargaSoltaDTO)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    var sb = new StringBuilder();

                    movimentacaoCargaSoltaDTO.SISTEMA = "REDEX..";



                    sb.AppendLine(" INSERT INTO " + movimentacaoCargaSoltaDTO.SISTEMA + "TB_CARGA_SOLTA_YARD ( ");
                    sb.AppendLine(" AUTONUM_PATIOCS ");
                    sb.AppendLine(" ,AUTONUM_USU ");
                    sb.AppendLine(" ,ARMAZEM,YARD ");
                    sb.AppendLine(" ,QUANTIDADE ");
                    sb.AppendLine(" ,MOTIVO_COL ");
                    sb.AppendLine(" ) Values ( ");
                    sb.AppendLine(" " + movimentacaoCargaSoltaDTO.AUTONUM_CS + " ");
                    sb.AppendLine(" ," + movimentacaoCargaSoltaDTO.ID_USUARIO);
                    sb.AppendLine(" ,'" + movimentacaoCargaSoltaDTO.ARMAZEM + "' ");
                    sb.AppendLine(" ,'" + movimentacaoCargaSoltaDTO.LOCAL_POS + "'");
                    sb.AppendLine(" ," + movimentacaoCargaSoltaDTO.QUANTIDADE + " ");
                    sb.AppendLine("," + movimentacaoCargaSoltaDTO.MOTIVO_COL + "");
                    sb.AppendLine(" ); SELECT CAST(SCOPE_IDENTITY() AS int)  ");

                    string sql = sb.ToString();

                    int id = _db.Query<int>(sql).FirstOrDefault();

                    sb.Clear();


                    if (id > 0)
                    {

                        sb.AppendLine(" SELECT AUTONUM FROM REDEX..TB_MARCANTES_RDX where AUTONUM_PCS = " + movimentacaoCargaSoltaDTO.AUTONUM_CS);


                        int marcante = _db.Query<int>(sb.ToString()).FirstOrDefault();

                        sb.Clear();

                        //O WHERE TÁ ERRADO TEM QUE SER PELO MARCANTE
                        sb.AppendLine(" UPDATE REDEX..TB_MARCANTES_RDX Set AUTONUM_CS_YARD=" + id + " WHERE AUTONUM = " + marcante + "  ");

                        sql = sb.ToString();

                        _db.Query<int>(sql).FirstOrDefault();

                    }

                    return id;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateArmazemIPA(int ocupacaoID, int armazemID)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    var sb = new StringBuilder();

                    sb.AppendLine(" UPDATE SGIPA..TB_ARMAZENS_IPA Set OCUPACAO_CT=" + ocupacaoID + " WHERE AUTONUM = " + armazemID);

                    string sql = sb.ToString();

                    _db.Query<int>(sql).FirstOrDefault();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int InsertCargaSoltaYardHist(InserirNovaMovimentacaoCargaSoltaDTO movimentacaoCargaSoltaDTO)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    var sb = new StringBuilder();

                    movimentacaoCargaSoltaDTO.SISTEMA = "REDEX..";

                    if (movimentacaoCargaSoltaDTO.YARD == "R")
                    {
                        movimentacaoCargaSoltaDTO.SISTEMA = "REDEX..";
                    }

                    sb.AppendLine(" INSERT INTO " + movimentacaoCargaSoltaDTO.SISTEMA + "TB_HIST_SHIFTING_CS ( ");
                    sb.AppendLine(" MARCANTE ");
                    sb.AppendLine(" ,ARMAZEM ");
                    sb.AppendLine(" ,YARD ");
                    sb.AppendLine(" ,DT_MOV ");
                    sb.AppendLine(" ,USUARIO ");
                    sb.AppendLine(" ,ORIGEM ");
                    sb.AppendLine(" ,QTDE ");
                    sb.AppendLine(" ,MOTIVO ");
                    sb.AppendLine(" ) Values ( ");
                    sb.AppendLine(" " + movimentacaoCargaSoltaDTO.ID_MARCANTE + " ");
                    sb.AppendLine(" ,'" + movimentacaoCargaSoltaDTO.ARMAZEM + "' ");
                    sb.AppendLine(" ,'" + movimentacaoCargaSoltaDTO.LOCAL_POS + "'");
                    sb.AppendLine(" ,GETDATE()");
                    sb.AppendLine(" ," + movimentacaoCargaSoltaDTO.ID_USUARIO + " ");
                    sb.AppendLine(" ,'" + movimentacaoCargaSoltaDTO.ORIGEM + " '");
                    sb.AppendLine(" ," + movimentacaoCargaSoltaDTO.QUANTIDADE + " ");
                    sb.AppendLine("," + movimentacaoCargaSoltaDTO.MOTIVO_COL + "");
                    sb.AppendLine(" ) SELECT CAST(SCOPE_IDENTITY() AS int)  ");

                    string sql = sb.ToString();

                    int id = _db.Query<int>(sql).FirstOrDefault();
                    //int id = 1;
                    sb.Clear();



                    return id;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool GetValida_Quantidade(int autonum_cs, double quantidade)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    var sb = new StringBuilder();

                    sb.AppendLine(" SELECT ISNULL(sum(quantidade),0) quant_alocada FROM SGIPA..TB_CARGA_SOLTA_YARD WHERE AUTONUM_CS= " + autonum_cs);

                    string sql = sb.ToString();

                    double quantidade_alocada = _db.Query<double>(sql).FirstOrDefault();

                    sb.Clear();

                    sb.AppendLine(" SELECT (quantidade_real-quantidade_saida) quant_estoque FROM SGIPA..TB_CARGA_SOLTA WHERE AUTONUM=  " + autonum_cs);

                    sql = sb.ToString();

                    double quant_estoque = _db.Query<double>(sql).FirstOrDefault();


                    if ((quantidade_alocada + quantidade) > quant_estoque)
                        return false;

                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int GetFlagCT(int id_armazem)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    var sb = new StringBuilder();

                    sb.AppendLine(" SELECT ");
                    sb.AppendLine(" ISNULL(FLAG_CT,0) AS FLAG_CT ");
                    sb.AppendLine(" FROM SGIPA..TB_ARMAZENS_IPA  ");
                    sb.AppendLine(" WHERE AUTONUM= " + id_armazem);

                    string sql = sb.ToString();

                    int flag = _db.Query<int>(sql).FirstOrDefault();

                    return flag;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public YardDTO GetDadosYardBloqueio(int armazem, string yard)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    var sb = new StringBuilder();


                    sb.AppendLine(" SELECT ");
                    sb.AppendLine(" AUTONUM, FLAG_BLOQUEIO ");
                    sb.AppendLine(" FROM SGIPA..TB_YARD_CS ");
                    sb.AppendLine(" WHERE ARMAZEM='" + armazem + "' AND UPPER(YARD)='" + yard + "'   ");
                    sb.AppendLine(" AND YARD NOT IN ('CAM','CANCC','GOUT')");

                    string sql = sb.ToString();

                    var query = _db.Query<YardDTO>(sql).FirstOrDefault();

                    return query;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int GetAutonumPatioCS(int id)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    var sb = new StringBuilder();

                    sb.AppendLine(" Select AUTONUM_PATIOCS As AUTONUM_CS FROM REDEX..TB_CARGA_SOLTA_YARD WHERE AUTONUM= " + id);

                    string sql = sb.ToString();

                    int autonum_cs = _db.Query<int>(sql).FirstOrDefault();

                    return autonum_cs;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int GetAutonumPatioCsStp(int marcante)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    var sb = new StringBuilder();

                    using (var cmd = new SqlCommand("REDEX..P_INVENT_ARMAZEM_MARCANTE", _db))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter
                        {
                            ParameterName = "P_MARCANTE",
                            SqlDbType = SqlDbType.Int,
                            Direction = ParameterDirection.Input,
                            Value = marcante
                        });

                        _db.Open();
                        var dr = cmd.ExecuteReader();

                        int autonum_cs = 0;

                        while (dr.Read())
                        {
                            autonum_cs = Convert.ToInt32(dr["AUTONUMCS"].ToString());
                        }

                        return autonum_cs;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string ValidaRegrasArmazem(long autonum_cs, string pilha, long autonum_armazem)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    using (var cmd = new SqlCommand("OPERADOR..fValidaRegrasArmazem", _db))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter
                        {
                            ParameterName = "p_QualCs",
                            SqlDbType = SqlDbType.VarChar,
                            Direction = ParameterDirection.Input,
                            Value = autonum_cs.ToString()
                        });

                        cmd.Parameters.Add(new SqlParameter
                        {
                            ParameterName = "p_QualAutonum_Armazem",
                            SqlDbType = SqlDbType.VarChar,
                            Direction = ParameterDirection.Input,
                            Value = autonum_armazem.ToString()
                        });


                        cmd.Parameters.Add(new SqlParameter
                        {
                            ParameterName = "p_QualPilha",
                            SqlDbType = SqlDbType.VarChar,
                            Direction = ParameterDirection.Input,
                            Value = pilha.ToString()
                        });

                        cmd.Parameters.Add(new SqlParameter
                        {
                            ParameterName = "l_result",
                            SqlDbType = SqlDbType.VarChar,
                            Direction = ParameterDirection.Output,

                        });

                        _db.Open();
                        var dr = cmd.ExecuteReader();

                        string l_result = string.Empty;

                        while (dr.Read())
                        {
                            l_result = dr["l_result"].ToString();
                        }

                        return l_result;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void GetAtualizaMarcante(int idCsYard, int marcante)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    var sb = new StringBuilder();

                    sb.AppendLine(" UPDATE REDEX..TB_MARCANTES_RDX Set AUTONUM_CS_YARD=  " + idCsYard + " WHERE AUTONUM = " + marcante);

                    string sql = sb.ToString();

                    _db.Query<int>(sql).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int getNextValHistShiftingCs()
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    var sb = new StringBuilder();

                    sb.AppendLine(" INSERT INTO REDEX..SEQ_HIST_SHIFTING_CS(DATA)values(GETDATE()) SELECT CAST(SCOPE_IDENTITY() AS int) ");

                    string sql = sb.ToString();

                    int id = _db.Query<int>(sql).FirstOrDefault();

                    return id;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int getInserHistShiftingCS(HIST_SHIFTING_CS hist_)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    var sb = new StringBuilder();

                    sb.AppendLine(" INSERT INTO REDEX..TB_HIST_SHIFTING_CS ");
                    sb.AppendLine(" ( ");
                    sb.AppendLine("MARCANTE, ");
                    sb.AppendLine("ARMAZEM,");
                    sb.AppendLine("YARD,");
                    sb.AppendLine("DT_MOV,");
                    sb.AppendLine("USUARIO,");
                    sb.AppendLine("ORIGEM,");
                    sb.AppendLine("LOTE,");
                    sb.AppendLine("MOTIVO");
                    sb.AppendLine(" ) VALUES ( ");
                    sb.AppendLine(" " + hist_.MARCANTE + " ");
                    sb.AppendLine(" " + hist_.ARMAZEM + " ");
                    sb.AppendLine(" '" + hist_.LOCAL_POS + "' ");
                    sb.AppendLine("  GETDATE() ");
                    sb.AppendLine(" " + hist_.USUARIO + " ");
                    sb.AppendLine(" " + hist_.YARD + " ");
                    sb.AppendLine(" " + hist_.LOTE + " ");
                    sb.AppendLine(" " + hist_.MOTIVO + "  ");
                    sb.AppendLine(" ) SELECT CAST(SCOPE_IDENTITY() AS int)  ");

                    string sql = sb.ToString();

                    int id = _db.Query<int>(sql).FirstOrDefault();

                    return id;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int getAutonumByEtiqueta(string pos)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    var sb = new StringBuilder();

                    //sb.AppendLine(" SELECT AUTONUM FROM SGIPA..TB_ARMAZENS_IPA WHERE DESCR='" + etiqueta + "' and flag_ct=1 ");

                    sb.AppendLine(" SELECT ARMAZEM FROM SGIPA..TB_YARD_CS where YARD =  '" + pos + "'");

                    int autonum = _db.Query<int>(sb.ToString()).FirstOrDefault();

                    return autonum;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IEnumerable<InventarioCegoDTO> GetDadosEtiquetaByCodBar(string cod_bar, string pos, string pos1)
        {
            try
            {
                using (var con = new SqlConnection(Config.StringConexao()))
                {
                    var sb = new StringBuilder();

                    sb.AppendLine("SELECT Y.AUTONUM as AUTONUM_YARD,Y.YARD,A.AUTONUM, ISNULL(A.PATIO,0) AS PATIO FROM SGIPA..TB_YARD_CS Y INNER JOIN SGIPA..TB_ARMAZENS_IPA A ");
                    sb.AppendLine(" On Y.ARMAZEM=A.AUTONUM WHERE ");
                    sb.AppendLine(" A.COD_BAR='" + cod_bar + "'");
                    sb.AppendLine(" AND ((Y.YARD='" + pos + "' AND Y.VALIDA=0) OR (Y.YARD='" + pos1 + "' AND Y.VALIDA=1)) ");

                    var query = con.Query<InventarioCegoDTO>(sb.ToString()).AsEnumerable();

                    return query;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
