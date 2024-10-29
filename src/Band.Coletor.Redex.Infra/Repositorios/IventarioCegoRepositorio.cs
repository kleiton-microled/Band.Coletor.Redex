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
    public class IventarioCegoRepositorio : IIventarioCegoRepositorio
    {
        public IEnumerable<InventarioCegoDTO> getDadosByIdPatio(int patio)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    var sb = new StringBuilder();

                    sb.AppendLine(" SELECT AUTONUM,ID_ARMAZEM,DESCR,DESCR_ARMAZEM,PRATELEIRA,HEAP FROM SGIPA.VW_INVENT_ABERTO WHERE PATIO = " + patio);
                    sb.AppendLine(" ORDER BY DESCR  ");

                    var query = _db.Query<InventarioCegoDTO>(sb.ToString()).AsEnumerable();

                    return query;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IEnumerable<InventarioCegoDTO> getDadosById(int id, int patio)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    var sb = new StringBuilder();

                    sb.AppendLine(" SELECT AUTONUM,ID_ARMAZEM,DESCR,DESCR_ARMAZEM,PRATELEIRA,HEAP FROM SGIPA.VW_INVENT_ABERTO WHERE AUTONUM=" + id + " AND PATIO=" + patio + " ORDER BY DESCR  ");

                    var query = _db.Query<InventarioCegoDTO>(sb.ToString()).AsEnumerable();

                    return query;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IEnumerable<InventarioCegoDTO> GetCarregaGridCarga(int id)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    var sb = new StringBuilder();

                    sb.AppendLine(" SELECT SUBSTR('000000000000' || MARCANTE ,-12) AS MARCANTE ,YARD, AUTONUM FROM SGIPA..TB_INVENTARIO_ITEM WHERE AUTONUM_INV= " + id + " ORDER BY AUTONUM DESC  ");

                    var query = _db.Query<InventarioCegoDTO>(sb.ToString()).AsEnumerable();

                    return query;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IEnumerable<InventarioCegoDTO> GetAutonumInventario(int id, int marcante)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    var sb = new StringBuilder();

                    sb.AppendLine(" Select autonum from SGIPA..TB_inventario_item where autonum_inv= " + id);
                    sb.AppendLine(" AND  ");
                    sb.AppendLine(" marcante = " + marcante);

                    var query = _db.Query<InventarioCegoDTO>(sb.ToString()).AsEnumerable();

                    return query;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void GetAtualizaDados(InventarioCegoDTO inventario)
        {
            try
            {
                using (var con = new SqlConnection(Config.StringConexao()))
                {
                    var sb = new StringBuilder();

                    sb.AppendLine(" UPDATE SGIPA..TB_INVENTARIO_ITEM SET ");
                    sb.AppendLine(" YARD='" + inventario.YARD + "',");
                    sb.AppendLine(" DT_INVENT=GETDATE(), ");
                    sb.AppendLine(" AUTONUM_USER= " + inventario.USUARIO);
                    sb.AppendLine(" WHERE AUTONUM=" + inventario.AUTONUM);


                    con.Query<InventarioCegoDTO>(sb.ToString()).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void GetInsertDados(InventarioCegoDTO inventario)
        {
            try
            {
                using (var con = new SqlConnection(Config.StringConexao()))
                {
                    var sb = new StringBuilder();

                    sb.AppendLine(" INSERT INTO SGIPA..TB_INVENTARIO_ITEM (AUTONUM, ");
                    sb.AppendLine(" AUTONUM_INV,  ");
                    sb.AppendLine(" MARCANTE, ");
                    sb.AppendLine(" YARD, ");
                    sb.AppendLine(" DT_INVENT, ");
                    sb.AppendLine(" AUTONUM_USER, ");
                    sb.AppendLine(" ) values ( ");
                    sb.AppendLine(" " + inventario.AUTONUM + " ");
                    sb.AppendLine(" " + inventario.AUTONUM_INV + " ");
                    sb.AppendLine(" " + inventario.MARCANTE + " ");
                    sb.AppendLine(" " + inventario.YARD + " ");
                    sb.AppendLine(" GETDATE() ");
                    sb.AppendLine(" " + inventario.USUARIO + " ");
                    sb.AppendLine(" ) ");

                    con.Query<int>(sb.ToString()).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int GetAutonumByTbArmazens(int armador)
        {
            try
            {
                using (var db = new SqlConnection(Config.StringConexao()))
                {
                    var sb = new StringBuilder();
                    sb.AppendLine(" SELECT autonum FROM SGIPA..TB_armazens_ipa where autonum=" + armador + " and flag_ct=1 ");

                    int autonum = db.Query<int>(sb.ToString()).FirstOrDefault();

                    return autonum;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int GetAutonumByTbYard(int armazem, int yard)
        {
            try
            {
                using (var con = new SqlConnection(Config.StringConexao()))
                {
                    var sb = new StringBuilder();
                    sb.AppendLine(" SELECT AUTONUM FROM SGIPA..TB_yard_cs where armazem=" + armazem + " and yard='" + yard + "' ");

                    int autonum = con.Query<int>(sb.ToString()).FirstOrDefault();

                    return autonum;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public InventarioCegoDTO GetDadosInventarioAberto(int id)
        {
            try
            {
                using (var con = new SqlConnection(Config.StringConexao()))
                {
                    var sb = new StringBuilder();

                    sb.AppendLine("  Select id_armazem,prateleira,heap from sgipa.vw_invent_aberto where autonum=  " + id);

                    var query = con.Query<InventarioCegoDTO>(sb.ToString()).FirstOrDefault();

                    return query;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IEnumerable<InventarioCegoDTO> GetDadosInventario(string cod_bar, string pos, string pos1)
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
