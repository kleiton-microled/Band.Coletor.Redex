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
    public class IventarioCSRepositorio : IIventarioCSRepositorio
    {
        public IventarioCSDTO GetDadosPopulaLote(string Lote, int Patio, bool redex)
        {
            try
            {
                redex = true;

                using (var _Db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                   if (redex)
                    {
                        sb.AppendLine("   SELECT PATIO AS ID_PATIO, ");
                        sb.AppendLine("          BL , ");
                        sb.AppendLine("          LOTE, ");
                        sb.AppendLine("          MERCADORIA, ");
                        sb.AppendLine("          MARCA, ");
                        sb.AppendLine("          IMPORTADOR, ");
                        sb.AppendLine("          DT_ENTRADA AS DATA_ENTRADA, ");
                        sb.AppendLine("          CNTR_DESOVA, ");
                        sb.AppendLine("          VOLUME, ");
                        sb.AppendLine("          FINALITY, ");
                        sb.AppendLine("          TIPO_DOC, ");
                        sb.AppendLine("          CANAL_ALF, ");
                        sb.AppendLine("          PROX_MOVIMENTO AS MOTIVO_PROX_MVTO, ");
                        sb.AppendLine("          SISTEMA, ");
                        sb.AppendLine("          IMO, ");
                        sb.AppendLine("          NVOCC, ");
                        sb.AppendLine("          ANVISA, ");
                        sb.AppendLine("          FLAG_ANVISA, ");
                        sb.AppendLine("          LOTE_STR ");
                        sb.AppendLine("     FROM (SELECT a.patio, ");
                        sb.AppendLine("                  C.REFERENCE AS BL, ");
                        sb.AppendLine("                  0 AS lote, ");
                        sb.AppendLine("                  B.DESC_PRODUTO AS MERCADORIA, ");
                        sb.AppendLine("                  A.MARCA, ");
                        sb.AppendLine("                  H.FANTASIA AS IMPORTADOR, ");
                        sb.AppendLine("                  A.DT_PRIM_ENTRADA AS DT_ENTRADA, ");
                        sb.AppendLine("                  '' AS CNTR_DESOVA, ");
                        sb.AppendLine("                  A.VOLUME_DECLARADO AS VOLUME, ");
                        sb.AppendLine("                  'RDX' AS finality, ");
                        sb.AppendLine("                  ISNULL (SDBOO.DOC_EXP, '-') AS tipo_doc, ");
                        sb.AppendLine("                  0 AS canal_alf, ");
                        sb.AppendLine("                  '' AS PROX_MOVIMENTO, ");
                        sb.AppendLine("                  'R' AS SISTEMA, ");
                        sb.AppendLine("                  A.IMO, ");
                        sb.AppendLine("                  '' AS NVOCC, ");
                        sb.AppendLine("                  '-' AS ANVISA, ");
                        sb.AppendLine("                  0 AS FLAG_ANVISA, ");
                        sb.AppendLine("                  C.os AS LOTE_STR, ");
                        sb.AppendLine("                    A.QTDE_ENTRADA ");
                        sb.AppendLine("                  - ISNULL ( (SELECT SUM (qtde_saida) ");
                        sb.AppendLine("                             FROM REDEX.dbo.tb_saida_carga w ");
                        sb.AppendLine("                            WHERE w.autonum_pcs = a.autonum_pcs), ");
                        sb.AppendLine("                         0) ");
                        sb.AppendLine("                     AS SALDO ");
                        sb.AppendLine("             FROM REDEX.dbo.TB_PATIO_CS A ");
                        sb.AppendLine("                  INNER JOIN REDEX.dbo.TB_BOOKING_CARGA D ");
                        sb.AppendLine("                     ON A.AUTONUM_BCG = D.AUTONUM_BCG ");
                        sb.AppendLine("                  INNER JOIN REDEX.dbo.TB_BOOKING C ");
                        sb.AppendLine("                     ON D.AUTONUM_BOO = C.AUTONUM_BOO ");
                        sb.AppendLine("                  LEFT JOIN REDEX.dbo.TB_CAD_PRODUTOS B ");
                        sb.AppendLine("                     ON d.AUTONUM_PRO = B.AUTONUM_PRO ");
                        sb.AppendLine("                  LEFT JOIN REDEX.dbo.TB_CAD_PARCEIROS H ");
                        sb.AppendLine("                     ON C.AUTONUM_PARCEIRO = H.AUTONUM ");
                        sb.AppendLine("                  LEFT JOIN REDEX.dbo.TB_booking BM ");
                        sb.AppendLine("                     ON C.reserva_master = bm.autonum_boo ");
                        sb.AppendLine("                  LEFT JOIN REDEX.dbo.TB_booking BS ");
                        sb.AppendLine("                     ON C.reserva_pai = bs.autonum_boo ");
                        sb.AppendLine("                  LEFT JOIN (  SELECT MIN (td.descricao) AS DOC_EXP, ");
                        sb.AppendLine("                                      sdb.autonum_boo ");
                        sb.AppendLine("                                 FROM redex.dbo.tb_cad_sd sd ");
                        sb.AppendLine("                                      INNER JOIN redex.dbo.tb_cad_tipo_documento td ");
                        sb.AppendLine("                                         ON sd.autonum_tipo_doc = td.autonum ");
                        sb.AppendLine("                                      LEFT JOIN redex.dbo.tb_cad_sd_boo sdb ");
                        sb.AppendLine("                                         ON sd.autonum_sd = sdb.autonum_sd ");
                        sb.AppendLine("                                      LEFT JOIN redex.dbo.tb_booking boo ");
                        sb.AppendLine("                                         ON sdb.autonum_boo = boo.autonum_boo ");
                        sb.AppendLine("                             GROUP BY sdb.autonum_boo) SDBOO ");
                        sb.AppendLine("                     ON D.AUTONUM_BOO = SDBOO.AUTONUM_BOO ");
                        sb.AppendLine("            WHERE   C.OS='" + Lote + "' AND a.patio = " + Patio + " AND  d.flag_cs = 1 ");
                        sb.AppendLine("                  AND ISNULL (a.flag_historico, 0) = 0 ");
                        sb.AppendLine("                  ) as Q ");
                        sb.AppendLine("    WHERE SALDO > 0 ");


                    }
                    else
                    {
                        sb.AppendLine("      SELECT c.patio AS ID_PATIO, ");
                        sb.AppendLine("              b.numero AS BL, ");
                        sb.AppendLine("              b.autonum AS LOTE, ");
                        sb.AppendLine("              c.mercadoria AS MERCADORIA, ");
                        sb.AppendLine("              c.marca AS MARCA, ");
                        sb.AppendLine("              p.razao AS importador, ");
                        sb.AppendLine("              CASE WHEN cntr.dt_entrada IS NULL THEN c.dt_entrada ELSE cntr.dt_entrada END AS DATA_ENTRADA, ");
                        sb.AppendLine("              cntr.id_conteiner AS cntr_desova, ");
                        sb.AppendLine("              ISNULL (c.volume_m3, 0) AS volume, ");
                        sb.AppendLine("              'IPA-CS' AS finality, ");
                        sb.AppendLine("              ISNULL (td.descr, 'NOT DEFINED YET') AS tipo_doc, ");
                        sb.AppendLine("              ISNULL (b.canal_siscomex, 9) AS canal_alf, ");
                        sb.AppendLine("              vw3.motivo AS motivo_prox_mvto, ");
                        sb.AppendLine("              'I' AS sistema, ");
                        sb.AppendLine("              CASE WHEN c.imo = '0' THEN '' ELSE c.imo end AS IMO, ");
                        sb.AppendLine("              vw2.razao AS nvocc, ");
                        sb.AppendLine("              ISNULL (ANV.DESCR, '-') AS ANVISA, ");
                        sb.AppendLine("              ISNULL (B.FLAG_ANVISA, 0) AS FLAG_ANVISA, ");
                        sb.AppendLine("              CAST(b.autonum AS VARCHAR) AS LOTE_STR ");
                        sb.AppendLine("         FROM SGIPA.dbo.tb_carga_solta c ");
                        sb.AppendLine("              INNER JOIN sgipa.dbo.tb_bl b ");
                        sb.AppendLine("                 ON c.bl = b.autonum ");
                        sb.AppendLine("              LEFT JOIN sgipa.dbo.tb_cntr_bl cntr ");
                        sb.AppendLine("                 ON c.cntr = cntr.autonum ");
                        sb.AppendLine("              LEFT JOIN sgipa.dbo.tb_cad_parceiros p ");
                        sb.AppendLine("                 ON b.importador = p.autonum ");
                        sb.AppendLine("              LEFT JOIN sgipa.dbo.tb_cad_parceiros vw2 ");
                        sb.AppendLine("                 ON b.captador = vw2.autonum ");
                        sb.AppendLine("              LEFT JOIN sgipa.dbo.tb_tipos_documentos td ");
                        sb.AppendLine("                 ON b.tipo_documento = td.code ");
                        sb.AppendLine("              LEFT JOIN sgipa.dbo.vw_prox_mov_cs_un vw3 ");
                        sb.AppendLine("                 ON b.autonum = vw3.lote ");
                        sb.AppendLine("              LEFT JOIN SGIPA.dbo.TB_BL_ANVISA BANV ");
                        sb.AppendLine("                 ON C.BL = BANV.LOTE ");
                        sb.AppendLine("              LEFT JOIN SGIPA.dbo.TB_CAD_ANVISA ANV ");
                        sb.AppendLine("                 ON BANV.COD_ANVISA = ANV.AUTONUM ");
                        sb.AppendLine("        WHERE B.AUTONUM = " + Lote + " AND c.patio = " + Patio + " AND c.flag_terminal = 1 AND c.flag_historico = 0 ");

                        sb.AppendLine(" UNION ");

                        sb.AppendLine("   SELECT PATIO AS ID_PATIO, ");
                        sb.AppendLine("          BL AS BL, ");
                        sb.AppendLine("          LOTE, ");
                        sb.AppendLine("          MERCADORIA, ");
                        sb.AppendLine("          MARCA, ");
                        sb.AppendLine("          IMPORTADOR, ");
                        sb.AppendLine("          DT_ENTRADA AS DATA_ENTRADA, ");
                        sb.AppendLine("          CNTR_DESOVA, ");
                        sb.AppendLine("          VOLUME, ");
                        sb.AppendLine("          FINALITY, ");
                        sb.AppendLine("          TIPO_DOC, ");
                        sb.AppendLine("          CANAL_ALF, ");
                        sb.AppendLine("          PROX_MOVIMENTO AS MOTIVO_PROX_MVTO, ");
                        sb.AppendLine("          SISTEMA, ");
                        sb.AppendLine("          IMO, ");
                        sb.AppendLine("          NVOCC, ");
                        sb.AppendLine("          ANVISA, ");
                        sb.AppendLine("          FLAG_ANVISA, ");
                        sb.AppendLine("          LOTE_STR ");
                        sb.AppendLine("     FROM (SELECT a.patio, ");
                        sb.AppendLine("                  C.REFERENCE AS BL, ");
                        sb.AppendLine("                  0 AS lote, ");
                        sb.AppendLine("                  B.DESC_PRODUTO AS MERCADORIA, ");
                        sb.AppendLine("                  A.MARCA, ");
                        sb.AppendLine("                  H.FANTASIA AS IMPORTADOR, ");
                        sb.AppendLine("                  A.DT_PRIM_ENTRADA AS DT_ENTRADA, ");
                        sb.AppendLine("                  '' AS CNTR_DESOVA, ");
                        sb.AppendLine("                  A.VOLUME_DECLARADO AS VOLUME, ");
                        sb.AppendLine("                  'RDX' AS finality, ");
                        sb.AppendLine("                  ISNULL (SDBOO.DOC_EXP, '-') AS tipo_doc, ");
                        sb.AppendLine("                  0 AS canal_alf, ");
                        sb.AppendLine("                  '' AS PROX_MOVIMENTO, ");
                        sb.AppendLine("                  'R' AS SISTEMA, ");
                        sb.AppendLine("                  A.IMO, ");
                        sb.AppendLine("                  '' AS NVOCC, ");
                        sb.AppendLine("                  '-' AS ANVISA, ");
                        sb.AppendLine("                  0 AS FLAG_ANVISA, ");
                        sb.AppendLine("                  C.os AS LOTE_STR, ");
                        sb.AppendLine("                    A.QTDE_ENTRADA ");
                        sb.AppendLine("                  - ISNULL ( (SELECT SUM (qtde_saida) ");
                        sb.AppendLine("                             FROM REDEX.dbo.tb_saida_carga w ");
                        sb.AppendLine("                            WHERE w.autonum_pcs = a.autonum_pcs), ");
                        sb.AppendLine("                         0) ");
                        sb.AppendLine("                     AS SALDO ");
                        sb.AppendLine("             FROM REDEX.dbo.TB_PATIO_CS A ");
                        sb.AppendLine("                  INNER JOIN REDEX.dbo.TB_BOOKING_CARGA D ");
                        sb.AppendLine("                     ON A.AUTONUM_BCG = D.AUTONUM_BCG ");
                        sb.AppendLine("                  INNER JOIN REDEX.dbo.TB_BOOKING C ");
                        sb.AppendLine("                     ON D.AUTONUM_BOO = C.AUTONUM_BOO ");
                        sb.AppendLine("                  LEFT JOIN REDEX.dbo.TB_CAD_PRODUTOS B ");
                        sb.AppendLine("                     ON d.AUTONUM_PRO = B.AUTONUM_PRO ");
                        sb.AppendLine("                  LEFT JOIN REDEX.dbo.TB_CAD_PARCEIROS H ");
                        sb.AppendLine("                     ON C.AUTONUM_PARCEIRO = H.AUTONUM ");
                        sb.AppendLine("                  LEFT JOIN REDEX.dbo.TB_booking BM ");
                        sb.AppendLine("                     ON C.reserva_master = bm.autonum_boo ");
                        sb.AppendLine("                  LEFT JOIN REDEX.dbo.TB_booking BS ");
                        sb.AppendLine("                     ON C.reserva_pai = bs.autonum_boo ");
                        sb.AppendLine("                  LEFT JOIN (  SELECT MIN (td.descricao) AS DOC_EXP, ");
                        sb.AppendLine("                                      sdb.autonum_boo ");
                        sb.AppendLine("                                 FROM redex.dbo.tb_cad_sd sd ");
                        sb.AppendLine("                                      INNER JOIN redex.dbo.tb_cad_tipo_documento td ");
                        sb.AppendLine("                                         ON sd.autonum_tipo_doc = td.autonum ");
                        sb.AppendLine("                                      LEFT JOIN redex.dbo.tb_cad_sd_boo sdb ");
                        sb.AppendLine("                                         ON sd.autonum_sd = sdb.autonum_sd ");
                        sb.AppendLine("                                      LEFT JOIN redex.dbo.tb_booking boo ");
                        sb.AppendLine("                                         ON sdb.autonum_boo = boo.autonum_boo ");
                        sb.AppendLine("                             GROUP BY sdb.autonum_boo) SDBOO ");
                        sb.AppendLine("                     ON D.AUTONUM_BOO = SDBOO.AUTONUM_BOO ");
                        sb.AppendLine("            WHERE   C.OS='" + Lote +  "' AND a.patio = " + Patio + " AND  d.flag_cs = 1 ");
                        sb.AppendLine("                  AND ISNULL (a.flag_historico, 0) = 0 ) as Q ");
                        sb.AppendLine("    WHERE SALDO > 0 ");
                    }

                    var query = _Db.Query<IventarioCSDTO>(sb.ToString()).FirstOrDefault();

                    return query;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public IEnumerable<ArmazensDTO> GetConsultaArmazens(string patio)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" SELECT  ");
                    sb.AppendLine(" AUTONUM, DESCR AS DISPLAY ");
                    sb.AppendLine(" FROM  ");
                    sb.AppendLine(" SGIPA..tb_armazens_ipa ");
                    sb.AppendLine(" WHERE  ");
                    sb.AppendLine(" dt_saida is null   ");
                    sb.AppendLine(" AND  ");
                    sb.AppendLine(" flag_historico=0  ");

                    if (patio != null)
                    {
                        if (patio != "")
                        {
                            sb.AppendLine(" AND ");
                            sb.AppendLine(" PATIO =  '" + patio + "' ");
                        }                        
                    }
                    sb.AppendLine(" ORDER BY DESCR ");

                    var query = _db.Query<ArmazensDTO>(sb.ToString()).AsEnumerable();

                    return query;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public IventarioCSDTO GetPopulaItem(string id)
        {
            try
            {
                using (var _Db = new SqlConnection(Config.StringConexao()))
                {
                    string PAutonumCs = id.Replace("IU", "").Replace("RU", "").Replace("RV", "").Replace("C", "").Replace("Y", "");

                    StringBuilder sb = new StringBuilder();

                    if (id.Substring(1, 2) != "RV")
                    {
                        if (id.Substring(2, 1) == "Y" && id.Substring(0, 1) == "I")
                        {
                            sb.AppendLine("SELECT  QTDE,EMBALAGEM,(DESCR_ARMAZEM + ' ' + POSICAO) AS LOCAL, POSICAO AS YARD  ");
                            sb.AppendLine("  FROM ( ");
                            sb.AppendLine("    SELECT a.descr as DESCR_ARMAZEM, ");
                            sb.AppendLine("          ISNULL (m.volumes, 0) AS QTDE, ");
                            sb.AppendLine("          e.descr AS EMBALAGEM, ");
                            sb.AppendLine("          y.yard AS POSICAO, ");
                            sb.AppendLine("          'I' AS SISTEMA, ");
                            sb.AppendLine("          'Y' + LTRIM (CAST(y.autonum as varchar)) AS ID_GRAVACAO, ");
                            sb.AppendLine("          c.quantidade AS QTDE_CAPTADA, ");
                            sb.AppendLine("          m.autonum AS MARCANTE, ");
                            sb.AppendLine("          CAST(b.autonum as varchar) AS LOTE_STR, ");
                            sb.AppendLine("          B.NUMERO AS BL, ");
                            sb.AppendLine("          A.AUTONUM AS AUTONUM_ARMAZEM ");
                            sb.AppendLine("     FROM sgipa.dbo.tb_carga_solta c ");
                            sb.AppendLine("          INNER JOIN sgipa.dbo.tb_bl b ");
                            sb.AppendLine("             ON c.bl = b.autonum ");
                            sb.AppendLine("          INNER JOIN sgipa.dbo.tb_carga_solta_yard y ");
                            sb.AppendLine("             ON c.autonum = y.autonum_cs ");
                            sb.AppendLine("          INNER JOIN sgipa.dbo.tb_marcantes m ");
                            sb.AppendLine("             ON c.autonum = m.autonum_carga AND y.autonum = m.autonum_cs_yard ");
                            sb.AppendLine("          INNER JOIN sgipa.dbo.tb_armazens_ipa a ");
                            sb.AppendLine("             ON y.armazem = a.autonum ");
                            sb.AppendLine("          LEFT JOIN sgipa.dbo.dte_tb_embalagens e ");
                            sb.AppendLine("             ON c.embalagem = e.code ");
                            sb.AppendLine("    WHERE ");
                            sb.AppendLine("            Y.AUTONUM = " + PAutonumCs + " ");
                            sb.AppendLine("            AND c.flag_terminal = 1 ");
                            sb.AppendLine("          AND b.flag_ativo = 1 ");
                            sb.AppendLine("          AND c.flag_historico = 0 ");
                            sb.AppendLine("          AND m.volumes > 0  ");
                            sb.AppendLine("    UNION   ");
                            sb.AppendLine("   SELECT i.descr as DESCR_ARMAZEM, ");
                            sb.AppendLine("          y.quantidade - ISNULL (M.VOLUMES, 0) AS QTDE, ");
                            sb.AppendLine("          e.descr AS EMBALAGEM, ");
                            sb.AppendLine("          y.yard AS POSICAO, ");
                            sb.AppendLine("          'I' AS SISTEMA, ");
                            sb.AppendLine("          'Y' + LTRIM (CAST(y.autonum as varchar)) AS ID_GRAVACAO, ");
                            sb.AppendLine("          c.quantidade AS QTDE_CAPTADA, ");
                            sb.AppendLine("          '000000000000' AS MARCANTE, ");
                            sb.AppendLine("          cast(b.autonum as varchar) AS LOTE_STR, ");
                            sb.AppendLine("          B.NUMERO AS BL, ");
                            sb.AppendLine("          I.AUTONUM AS AUTONUM_ARMAZEM ");
                            sb.AppendLine("     FROM sgipa.dbo.tb_carga_solta c ");
                            sb.AppendLine("          INNER JOIN sgipa.dbo.tb_bl b ");
                            sb.AppendLine("             ON c.bl = b.autonum ");
                            sb.AppendLine("          INNER JOIN sgipa.dbo.tb_carga_solta_yard y ");
                            sb.AppendLine("             ON c.autonum = y.autonum_cs ");
                            sb.AppendLine("          INNER JOIN sgipa.dbo.tb_armazens_ipa i ");
                            sb.AppendLine("             ON y.armazem = i.autonum ");
                            sb.AppendLine("          LEFT JOIN operador.dbo.vw_carga_solta_mc mc ");
                            sb.AppendLine("             ON c.autonum = mc.autonum_cs ");
                            sb.AppendLine("          LEFT JOIN sgipa.dbo.tb_marcantes m ");
                            sb.AppendLine("             ON y.autonum = m.autonum_cs_yard ");
                            sb.AppendLine("          LEFT JOIN sgipa.dbo.dte_tb_embalagens e ");
                            sb.AppendLine("             ON c.embalagem = e.code ");
                            sb.AppendLine("    WHERE      ");
                            sb.AppendLine("         Y.AUTONUM = " + PAutonumCs + " ");
                            sb.AppendLine("         AND b.flag_ativo = 1 ");
                            sb.AppendLine("         AND c.flag_terminal = 1 ");
                            sb.AppendLine("          AND c.flag_historico = 0 ");
                            sb.AppendLine("          AND y.quantidade > 0 ");
                            sb.AppendLine("          AND (  ISNULL (c.quantidade_real, 0) ");
                            sb.AppendLine("               - ISNULL (c.quantidade_saida, 0) ");
                            sb.AppendLine("               - ISNULL (mc.quantidade, 0)) > 0 ");
                            sb.AppendLine("          AND (m.autonum IS NULL OR ISNULL (M.VOLUMES, 0) <> Y.QUANTIDADE) )Q ");
                        }
                        if (id.Substring(2, 1) == "C" && id.Substring(0, 1) == "I")
                        {
                            sb.AppendLine("    SELECT  QTDE,EMBALAGEM,(DESCR_ARMAZEM + ' ' + POSICAO) AS LOCAL, POSICAO AS YARD  ");
                            sb.AppendLine("    FROM (    ");
                            sb.AppendLine("   SELECT i.descr as DESCR_ARMAZEM, ");
                            sb.AppendLine("          ISNULL (m.volumes, 0) AS QTDE, ");
                            sb.AppendLine("          e.descr AS EMBALAGEM, ");
                            sb.AppendLine("          '-' AS POSICAO, ");
                            sb.AppendLine("          'I' AS SISTEMA, ");
                            sb.AppendLine("          'C' + LTRIM (CAST(c.autonum AS VARCHAR)) AS ID_GRAVACAO, ");
                            sb.AppendLine("          c.quantidade AS QTDE_CAPTADA, ");
                            sb.AppendLine("          m.autonum AS MARCANTE, ");
                            sb.AppendLine("          CAST(b.autonum as varchar) AS LOTE_STR, ");
                            sb.AppendLine("          B.NUMERO AS BL , ");
                            sb.AppendLine("          I.AUTONUM AS AUTONUM_ARMAZEM ");
                            sb.AppendLine("     FROM sgipa.dbo.tb_carga_solta c ");
                            sb.AppendLine("          INNER JOIN sgipa.dbo.tb_bl b ");
                            sb.AppendLine("             ON c.bl = b.autonum ");
                            sb.AppendLine("          INNER JOIN sgipa.dbo.tb_marcantes m ");
                            sb.AppendLine("             ON c.autonum = m.autonum_carga ");
                            sb.AppendLine("          INNER JOIN sgipa.dbo.tb_armazens_ipa i ");
                            sb.AppendLine("             ON c.patio = i.patio ");
                            sb.AppendLine("          LEFT JOIN sgipa.dbo.dte_tb_embalagens e ");
                            sb.AppendLine("             ON c.embalagem = e.code ");
                            sb.AppendLine("    WHERE      ");
                            sb.AppendLine("         C.AUTONUM = " + PAutonumCs + " ");
                            sb.AppendLine("         AND b.flag_ativo = 1 ");
                            sb.AppendLine("         AND c.flag_terminal = 1 ");
                            sb.AppendLine("         AND c.flag_historico = 0 ");
                            sb.AppendLine("         AND i.flag_padrao_patio = 1 ");
                            sb.AppendLine("         AND m.volumes > 0 ");
                            sb.AppendLine("         AND m.autonum_cs_yard = 0 ");
                            sb.AppendLine("    UNION    ");
                            sb.AppendLine("   SELECT i.descr as DESCR_ARMAZEM, ");
                            sb.AppendLine("          (  ISNULL (c.quantidade_real, 0) ");
                            sb.AppendLine("           - ISNULL (c.quantidade_saida, 0) ");
                            sb.AppendLine("           - ISNULL (vw.quantidade, 0)) ");
                            sb.AppendLine("             AS QTDE, ");
                            sb.AppendLine("          e.descr AS EMBALAGEM, ");
                            sb.AppendLine("          '-' AS POSICAO, ");
                            sb.AppendLine("          'I' AS SISTEMA, ");
                            sb.AppendLine("          'C' + LTRIM (CAST(c.autonum as varchar)) AS ID_GRAVACAO, ");
                            sb.AppendLine("          c.quantidade AS QTDE_CAPTADA, ");
                            sb.AppendLine("          '000000000000' AS MARCANTE, ");
                            sb.AppendLine("          CAST(b.autonum as varchar) AS LOTE_STR, ");
                            sb.AppendLine("          B.NUMERO AS BL, ");
                            sb.AppendLine("          I.AUTONUM AS AUTONUM_ARMAZEM ");
                            sb.AppendLine("     FROM sgipa.dbo.tb_carga_solta c ");
                            sb.AppendLine("          INNER JOIN sgipa.dbo.tb_bl b ");
                            sb.AppendLine("             ON c.bl = b.autonum ");
                            sb.AppendLine("          INNER JOIN sgipa.dbo.tb_armazens_ipa i ");
                            sb.AppendLine("             ON c.patio = i.patio ");
                            sb.AppendLine("          LEFT JOIN operador.dbo.vw_carga_solta_pos vw ");
                            sb.AppendLine("             ON c.autonum = vw.autonum_cs ");
                            sb.AppendLine("          LEFT JOIN operador.dbo.vw_carga_solta_mc vwmc ");
                            sb.AppendLine("             ON c.autonum = vwmc.autonum_cs ");
                            sb.AppendLine("          LEFT JOIN sgipa.dbo.dte_tb_embalagens e ");
                            sb.AppendLine("             ON c.embalagem = e.code ");
                            sb.AppendLine("    WHERE      ");
                            sb.AppendLine("         C.AUTONUM = " + PAutonumCs + " ");
                            sb.AppendLine("        AND b.flag_ativo = 1 ");
                            sb.AppendLine("         AND c.flag_terminal = 1 ");
                            sb.AppendLine("          AND c.flag_historico = 0 ");
                            sb.AppendLine("          AND i.flag_padrao_patio = 1 ");
                            sb.AppendLine("          AND (  ISNULL (c.quantidade_real, 0) ");
                            sb.AppendLine("               - ISNULL (c.quantidade_saida, 0) ");
                            sb.AppendLine("               - ISNULL (vw.quantidade, 0) ");
                            sb.AppendLine("               - ISNULL (vwmc.quantidade, 0)) > 0 ) VW WHERE QTDE>0  ");
                        }
                        if (id.Substring(2, 1) == "C" && id.Substring(0, 1) == "R")
                        {
                            sb.AppendLine("    SELECT  QTDE,EMBALAGEM,(DESCR_ARMAZEM + ' ' + POSICAO) AS LOCAL, POSICAO AS YARD    ");
                            sb.AppendLine("    FROM (  ");
                            sb.AppendLine(" SELECT DESCR AS DESCR_ARMAZEM,  ");
                            sb.AppendLine("          SALDO AS QTDE,  ");
                            sb.AppendLine("          EMBALAGEM,  ");
                            sb.AppendLine("          POSICAO,  ");
                            sb.AppendLine("          SISTEMA,  ");
                            sb.AppendLine("          ID_GRAVACAO,  ");
                            sb.AppendLine("          QTDE_CAPTADA,  ");
                            sb.AppendLine("          MARCANTE,  ");
                            sb.AppendLine("          LOTE_STR,  ");
                            sb.AppendLine("          BL,  ");
                            sb.AppendLine("          AUTONUM_ARMAZEM  ");
                            sb.AppendLine("     FROM (SELECT /*+ ORDERED */  ");
                            sb.AppendLine("                 i.descr,  ");
                            sb.AppendLine("                    A.QTDE_ENTRADA  ");
                            sb.AppendLine("                  - ISNULL ( (SELECT SUM (qtde_saida)  ");
                            sb.AppendLine("                             FROM REDEX.dbo.tb_saida_carga w  ");
                            sb.AppendLine("                            WHERE w.autonum_pcs = a.autonum_pcs),  ");
                            sb.AppendLine("                         0)  ");
                            sb.AppendLine("                  - ISNULL ( (SELECT SUM (quantidade)  ");
                            sb.AppendLine("                             FROM REDEX.dbo.tb_carga_solta_yard kk  ");
                            sb.AppendLine("                            WHERE kk.autonum_patiocs = a.autonum_pcs),  ");
                            sb.AppendLine("                         0)  ");
                            sb.AppendLine("                     AS SALDO,  ");
                            sb.AppendLine("                  G.DESCRICAO_EMB AS EMBALAGEM,  ");
                            sb.AppendLine("                  '-' AS posicao,  ");
                            sb.AppendLine("                  'R' AS SISTEMA,  ");
                            sb.AppendLine("                  'C' + LTRIM (cast(A.autonum_PCS as varchar)) AS ID_GRAVACAO,  ");
                            sb.AppendLine("                  A.QTDE_ENTRADA AS QTDE_CAPTADA,  ");
                            sb.AppendLine("                  '000000000000' AS MARCANTE,  ");
                            sb.AppendLine("                  C.os AS LOTE_STR,  ");
                            sb.AppendLine("                  '' AS BL,  ");
                            sb.AppendLine("                  I.AUTONUM AS AUTONUM_ARMAZEM  ");
                            sb.AppendLine("             FROM REDEX.dbo.TB_PATIO_CS A  ");
                            sb.AppendLine("                  INNER JOIN sgipa.dbo.tb_armazens_ipa I  ");
                            sb.AppendLine("                     ON A.patio = i.patio  ");
                            sb.AppendLine("                  INNER JOIN REDEX.dbo.TB_BOOKING_CARGA D  ");
                            sb.AppendLine("                     ON A.AUTONUM_BCG = D.AUTONUM_BCG  ");
                            sb.AppendLine("                  INNER JOIN REDEX.dbo.TB_BOOKING C  ");
                            sb.AppendLine("                     ON D.AUTONUM_BOO = C.AUTONUM_BOO  ");
                            sb.AppendLine("                  LEFT JOIN REDEX.dbo.TB_CAD_PRODUTOS B  ");
                            sb.AppendLine("                     ON d.AUTONUM_PRO = B.AUTONUM_PRO  ");
                            sb.AppendLine("                  LEFT JOIN REDEX.dbo.TB_CAD_EMBALAGENS G  ");
                            sb.AppendLine("                     ON A.AUTONUM_EMB = G.AUTONUM_EMB  ");
                            sb.AppendLine("                  LEFT JOIN REDEX.dbo.TB_CAD_PARCEIROS H  ");
                            sb.AppendLine("                     ON C.AUTONUM_PARCEIRO = H.AUTONUM  ");
                            sb.AppendLine("                  LEFT JOIN REDEX.dbo.TB_booking BM  ");
                            sb.AppendLine("                     ON C.reserva_master = bm.autonum_boo  ");
                            sb.AppendLine("                  LEFT JOIN REDEX.dbo.TB_booking BS  ");
                            sb.AppendLine("                     ON C.reserva_pai = bs.autonum_boo  ");
                            sb.AppendLine("                  LEFT JOIN (  SELECT MIN (td.descricao) AS DOC_EXP,  ");
                            sb.AppendLine("                                      sdb.autonum_boo  ");
                            sb.AppendLine("                                 FROM redex.dbo.tb_cad_sd sd  ");
                            sb.AppendLine("                                      INNER JOIN redex.dbo.tb_cad_tipo_documento td  ");
                            sb.AppendLine("                                         ON sd.autonum_tipo_doc = td.autonum  ");
                            sb.AppendLine("                                      LEFT JOIN redex.dbo.tb_cad_sd_boo sdb  ");
                            sb.AppendLine("                                         ON sd.autonum_sd = sdb.autonum_sd  ");
                            sb.AppendLine("                                      LEFT JOIN redex.dbo.tb_booking boo  ");
                            sb.AppendLine("                                         ON sdb.autonum_boo = boo.autonum_boo  ");
                            sb.AppendLine("                             GROUP BY sdb.autonum_boo) SDBOO  ");
                            sb.AppendLine("                     ON D.AUTONUM_BOO = SDBOO.AUTONUM_BOO  ");
                            sb.AppendLine("            WHERE       ");
                            sb.AppendLine("                  A.autonum_PCS= " + PAutonumCs + "  ");
                            sb.AppendLine("                  AND d.flag_cs = 1  ");
                            sb.AppendLine("                  AND i.flag_padrao_patio = 1  ");
                            sb.AppendLine("                  AND ISNULL (a.flag_historico, 0) = 0)Q)Q   ");
                        }
                        if (id.Substring(2, 1) == "Y" && id.Substring(0, 1) == "R")
                        {
                            sb.AppendLine("    SELECT  QTDE,EMBALAGEM,(DESCR_ARMAZEM + ' ' + POSICAO) AS LOCAL, POSICAO AS YARD    ");
                            sb.AppendLine("    FROM (     ");
                            sb.AppendLine("   SELECT DESCR AS DESCR_ARMAZEM,  ");
                            sb.AppendLine("          QUANTIDADE AS QTDE,  ");
                            sb.AppendLine("          EMBALAGEM,  ");
                            sb.AppendLine("          POSICAO,  ");
                            sb.AppendLine("          SISTEMA,  ");
                            sb.AppendLine("          ID_GRAVACAO,  ");
                            sb.AppendLine("          QTDE_CAPTADA,  ");
                            sb.AppendLine("          MARCANTE,  ");
                            sb.AppendLine("          LOTE_STR,  ");
                            sb.AppendLine("          AUTONUM_ARMAZEM  ");
                            sb.AppendLine("     FROM (SELECT i.descr,  ");
                            sb.AppendLine("                  Y.QUANTIDADE,  ");
                            sb.AppendLine("                    A.QTDE_ENTRADA  ");
                            sb.AppendLine("                  - ISNULL ( (SELECT SUM (qtde_saida)  ");
                            sb.AppendLine("                             FROM REDEX.dbo.tb_saida_carga w  ");
                            sb.AppendLine("                            WHERE w.autonum_pcs = a.autonum_pcs),  ");
                            sb.AppendLine("                         0)  ");
                            sb.AppendLine("                     AS SALDO,  ");
                            sb.AppendLine("                  G.DESCRICAO_EMB AS EMBALAGEM,  ");
                            sb.AppendLine("                  Y.YARD AS posicao,  ");
                            sb.AppendLine("                  'R' AS SISTEMA,  ");
                            sb.AppendLine("                  'Y' + LTRIM (CAST(Y.autonum as varchar)) AS ID_GRAVACAO,  ");
                            sb.AppendLine("                  A.QTDE_ENTRADA AS QTDE_CAPTADA,  ");
                            sb.AppendLine("                  '000000000000' AS MARCANTE,  ");
                            sb.AppendLine("                  C.os AS LOTE_STR,  ");
                            sb.AppendLine("                  I.AUTONUM AS AUTONUM_ARMAZEM  ");
                            sb.AppendLine("             FROM REDEX.dbo.TB_PATIO_CS A  ");
                            sb.AppendLine("                  INNER JOIN REDEX.dbo.TB_BOOKING_CARGA D  ");
                            sb.AppendLine("                     ON A.AUTONUM_BCG = D.AUTONUM_BCG  ");
                            sb.AppendLine("                  INNER JOIN REDEX.dbo.TB_BOOKING C  ");
                            sb.AppendLine("                     ON D.AUTONUM_BOO = C.AUTONUM_BOO  ");
                            sb.AppendLine("                  INNER JOIN REDEX.dbo.TB_CARGA_SOLTA_YARD Y  ");
                            sb.AppendLine("                     ON A.AUTONUM_PCS = Y.AUTONUM_PATIOCS  ");
                            sb.AppendLine("                  INNER JOIN sgipa.dbo.tb_armazens_ipa I  ");
                            sb.AppendLine("                     ON Y.ARMAZEM = i.AUTONUM  ");
                            sb.AppendLine("                  LEFT JOIN REDEX.dbo.TB_CAD_PRODUTOS B  ");
                            sb.AppendLine("                     ON d.AUTONUM_PRO = B.AUTONUM_PRO  ");
                            sb.AppendLine("                  LEFT JOIN REDEX.dbo.TB_CAD_EMBALAGENS G  ");
                            sb.AppendLine("                     ON A.AUTONUM_EMB = G.AUTONUM_EMB  ");
                            sb.AppendLine("                  LEFT JOIN REDEX.dbo.TB_CAD_PARCEIROS H  ");
                            sb.AppendLine("                     ON C.AUTONUM_PARCEIRO = H.AUTONUM  ");
                            sb.AppendLine("                  LEFT JOIN REDEX.dbo.TB_booking BM  ");
                            sb.AppendLine("                     ON C.reserva_master = bm.autonum_boo  ");
                            sb.AppendLine("                  LEFT JOIN REDEX.dbo.TB_booking BS  ");
                            sb.AppendLine("                     ON C.reserva_pai = bs.autonum_boo  ");
                            sb.AppendLine("                  LEFT JOIN (  SELECT MIN (td.descricao) AS DOC_EXP,  ");
                            sb.AppendLine("                                      sdb.autonum_boo  ");
                            sb.AppendLine("                                 FROM redex.dbo.tb_cad_sd sd  ");
                            sb.AppendLine("                                      INNER JOIN redex.dbo.tb_cad_tipo_documento td  ");
                            sb.AppendLine("                                         ON sd.autonum_tipo_doc = td.autonum  ");
                            sb.AppendLine("                                      LEFT JOIN redex.dbo.tb_cad_sd_boo sdb  ");
                            sb.AppendLine("                                         ON sd.autonum_sd = sdb.autonum_sd  ");
                            sb.AppendLine("                                      LEFT JOIN redex.dbo.tb_booking boo  ");
                            sb.AppendLine("                                         ON sdb.autonum_boo = boo.autonum_boo  ");
                            sb.AppendLine("                             GROUP BY sdb.autonum_boo) SDBOO  ");
                            sb.AppendLine("                     ON D.AUTONUM_BOO = SDBOO.AUTONUM_BOO  ");
                            sb.AppendLine("            WHERE Y.autonum = " + PAutonumCs + "  AND d.flag_cs = 1 AND ISNULL (a.flag_historico, 0) = 0)Q  ");
                            sb.AppendLine("    )Q  ");
                        }
                    }
                    else
                    {
                        sb.AppendLine(" SELECT  ");
                        sb.AppendLine(" SUM(A.QTDE) AS QTDE, ");
                        sb.AppendLine(" MIN(A.EMBALAGEM) AS EMBALAGEM , ");
                        sb.AppendLine("  (  MIN(A.DESCR_ARMAZEM) + ' ' + MIN(A.POSICAO)) AS LOCAL, ");
                        sb.AppendLine(" MIN(A.POSICAO) AS YARD ");
                        sb.AppendLine("   FROM OPERADOR..VW_INVENT_ARMAZEM_ITEM A  ");
                        sb.AppendLine(" INNER JOIN  OPERADOR..VW_INVENT_ARMAZEM_ITEM B     ");
                        sb.AppendLine(" ON A.EMBALAGEM=B.EMBALAGEM AND A.POSICAO=B.POSICAO AND A.LOTE=B.LOTE AND A.MARCANTE=B.MARCANTE WHERE A.ID_GRAVACAO='" + id + "' AND A.QTDE=1 ");

                    }

                    var query = _Db.Query<IventarioCSDTO>(sb.ToString()).FirstOrDefault();

                    return query;

                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public IventarioCSDTO GetSetaOcupacao(Int64 id)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine("  SELECT ISNULL(FLAG_CT,0) AS FLAG_CT,ISNULL(PERC_OCUPACAO_CT,'0%') AS OCUPACAO_CT FROM SGIPA..TB_ARMAZENS_IPA WHERE AUTONUM=" + id);

                    var query = _db.Query<IventarioCSDTO>(sb.ToString()).FirstOrDefault();

                    return query;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public IEnumerable<IventarioCSDTO> GetDadosGravacao(string id, string UV)
        {
            try
            {               

                using (var _Db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    if (UV == "U")
                    {
                        sb.AppendLine(" SELECT 0 AS QTDE, '" + id + "' AS ID_GRAVACAO ");
                    }
                    else
                    {
                        sb.AppendLine(" SELECT  ");
                        sb.AppendLine(" b.ID_GRAVACAO,b.QTDE  ");
                        sb.AppendLine(" FROM ");
                        sb.AppendLine(" OPERADOR.DBO.vw_invent_armazem_item  a ");
                        sb.AppendLine(" INNER JOIN  ");
                        sb.AppendLine(" OPERADOR.DBO.vw_invent_armazem_item b ");
                        sb.AppendLine(" on a.lote_str=b.lote_str and a.sistema=b.sistema ");
                        sb.AppendLine(" and a.EMBALAGEM = b.embalagem and a.MARCANTE=b.marcante ");
                        sb.AppendLine(" and a.DESCR_ARMAZEM=b.descr_armazem ");
                        sb.AppendLine(" and a.POSICAO=b.posicao where ");
                        sb.AppendLine("  a.id_gravacao='" + id + "' ");
                        sb.AppendLine(" and a.qtde=1 ");
                        sb.AppendLine(" order by a.ID_GRAVACAO ");
                    }



                    var query = _Db.Query<IventarioCSDTO>(sb.ToString()).AsEnumerable();

                    return query;

                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public int getDadosCargaSoltaYard(long id)
        {
            int autonum = 0;
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" SELECT AUTONUM_CS FROM SGIPA.DBO.TB_CARGA_SOLTA_YARD WHERE AUTONUM= " + id);

                    autonum = _db.Query<int>(sb.ToString()).FirstOrDefault();

                    return autonum;
                }
            }
            catch (Exception ex)
            {
                return autonum;
            }
        }
        public int getDadosCargaSoltaYardByIDPatio(long id)
        {
            int autonum = 0;
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" SELECT AUTONUM_PATIOCS AS AUTONUM_CS FROM REDEX.dbo.TB_CARGA_SOLTA_YARD WHERE AUTONUM= " + id);

                    autonum = _db.Query<int>(sb.ToString()).FirstOrDefault();

                    return autonum;
                }
            }
            catch (Exception ex)
            {
                return autonum;
            }
        }
        public int getDadosQuantidadeAlocada(long id)
        {
            int soma = 0;
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" SELECT ISNULL(sum(quantidade),0) QUANT_ALOCADA FROM  SGIPA.DBO.TB_CARGA_SOLTA_YARD WHERE AUTONUM_CS= " + id);

                    soma = _db.Query<int>(sb.ToString()).FirstOrDefault();

                    return soma;
                }

            }
            catch (Exception ex)
            {
                return soma;
            }
        }
        public int getDadosQuantidadeEstoque(long id)
        {
            int soma = 0;
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" SELECT (quantidade_real-quantidade_saida) QUANT_ESTOQUE FROM  SGIPA.DBO.TB_CARGA_SOLTA WHERE AUTONUM_CS= " + id);

                    soma = _db.Query<int>(sb.ToString()).FirstOrDefault();

                    return soma;
                }
            }
            catch (Exception ex)
            {
                return soma;
            }
        }
        public int getIdCargaSolta(long autonumCs, int armazem, string yard, string banco)
        {
            int id = 0;
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" SELECT AUTONUM FROM "+ banco + "..TB_CARGA_SOLTA_YARD WHERE AUTONUM_PATIOCS=" + autonumCs);

                    if (armazem != 0)
                    {
                        sb.AppendLine(" AND  ARMAZEM = " + armazem);

                    }
                    if (yard != null)
                    {
                        sb.AppendLine(" AND YARD = '" + yard  + "' ");
                    }

                    id = _db.Query<int>(sb.ToString()).FirstOrDefault();

                    return id;
                }
            }
            catch (Exception ex)
            {
                return id;
            }
        }
        public IventarioCSDTO InserirCargaSoltaYARD(IventarioCSDTO obj, string sistema)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" INSERT INTO " + sistema + "..TB_CARGA_SOLTA_YARD ");
                    sb.AppendLine(" ( ");
                    sb.AppendLine(" AUTONUM_CS ");
                    sb.AppendLine(" ,ARMAZEM ");
                    sb.AppendLine(" ,YARD  ");
                    sb.AppendLine(" ,ORIGEM ");
                    sb.AppendLine(" ,QUANTIDADE ");
                    sb.AppendLine(" ) ");
                    sb.AppendLine(" VALUES ");
                    sb.AppendLine(" ( ");
                    sb.AppendLine(" @AUTONUM_CS ");
                    sb.AppendLine(" ,@ARMAZEM ");
                    sb.AppendLine(" ,@YARD  ");
                    sb.AppendLine(" ,'I' ");
                    sb.AppendLine(" ,@QUANTIDADE ");
                    sb.AppendLine(" ) ");

                    _db.Query<IventarioCSDTO>(sb.ToString(), new
                    {
                        AUTONUM_CS = obj.AUTONUM_CS,
                        ARMAZEM = obj.ARMAZEM,
                        YARD = obj.YARD,
                        ORIGEM = obj.ORIGEM,
                        QUANTIDADE = obj.QUANTIDADE,

                    }).FirstOrDefault();

                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public IventarioCSDTO InserirCargaSoltaPatioCSYARD(IventarioCSDTO obj, string sistema)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(" INSERT INTO "+ sistema +"..TB_CARGA_SOLTA_YARD ");
                    sb.AppendLine(" ( ");
                    sb.AppendLine(" AUTONUM_PATIOCS ");
                    sb.AppendLine(" ,ARMAZEM ");
                    sb.AppendLine(" ,YARD  ");
                    sb.AppendLine(" ,ORIGEM ");
                    sb.AppendLine(" ,QUANTIDADE ");
                    sb.AppendLine(" ) ");
                    sb.AppendLine(" VALUES ");
                    sb.AppendLine(" ( ");
                    sb.AppendLine(" @AUTONUM_PATIOCS ");
                    sb.AppendLine(" ,@ARMAZEM ");
                    sb.AppendLine(" ,@YARD  ");
                    sb.AppendLine(" ,@ORIGEM ");
                    sb.AppendLine(" ,@QUANTIDADE ");
                    sb.AppendLine(" ) ");

                    _db.Query<IventarioCSDTO>(sb.ToString(), new
                    {
                        AUTONUM_PATIOCS = obj.AUTONUM_PATIOCS,
                        ARMAZEM = obj.ARMAZEM,
                        YARD = obj.YARD,
                        ORIGEM = obj.ORIGEM,
                        QUANTIDADE = obj.QUANTIDADE,

                    }).FirstOrDefault();

                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public IventarioCSDTO UpdateCargaSoltaYARD(IventarioCSDTO obj, string schema, string soma)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    int quantidade = obj.QUANTIDADE;
                    sb.AppendLine(" UPDATE " + schema + " ..TB_CARGA_SOLTA_YARD SET QUANTIDADE = QUANTIDADE " + soma + " @QUANTIDADE WHERE AUTONUM = @AUTONUM  ");

                    if (quantidade >= 2)
                    {
                        sb.AppendLine(" AND QUANTIDADE >=@QUANTIDADE  ");
                    }

                    _db.Query<IventarioCSDTO>(sb.ToString(), new
                    {
                        QUANTIDADE = obj.QUANTIDADE,
                        AUTONUM = obj.AUTONUM,

                    }).FirstOrDefault();

                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public IventarioCSDTO UpdateArmazemIPA(string ocupacao, int id)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" UPDATE SGIPA.dbo.TB_ARMAZENS_IPA SET PERC_OCUPACAO_CT =  '" + ocupacao + "' WHERE AUTONUM = " + id);

                    _db.Query<IventarioCSDTO>(sb.ToString()).FirstOrDefault();

                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        
        public IventarioCSDTO GetArmazemYARD(string id)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(" SELECT ARMAZEM FROM  SGIPA.DBO.TB_YARD_CS WHERE YARD= '" + id + "' ");

                    var query = _db.Query<IventarioCSDTO>(sb.ToString()).FirstOrDefault();

                    return query;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public int countYard(string id)
        {
            int result = 0;

            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine("SELECT 1 FROM SGIPA..TB_YARD_CS ");
                    sb.AppendLine("  WHERE ");
                    sb.AppendLine(" YARD = '" + id + "' ");
                    sb.AppendLine(" AND ");
                    sb.AppendLine(" VALIDA = 0 ");

                    result = _db.Query<int>(sb.ToString()).FirstOrDefault();

                    return result;
                }
            }
            catch (Exception ex)
            {
                return result;
            }
        }
        public IEnumerable<ArmazensDTO> GetConsultarItensLote(string lote, int patio)
        {
            try
            {
                using (var _db = new SqlConnection(Config.StringConexao()))
                {
                    StringBuilder SQL = new StringBuilder();

                    SQL.AppendLine(" SELECT ");
                    SQL.AppendLine(" 'IU' + VW.ID_GRAVACAO as AUTONUM,  ");
                    //SQL.AppendLine(" (VW.QTDE +  '/' + VW.QTDE_CAPTADA + ' ' + VW.EMBALAGEM + ' ' + VW.DESCR_ARMAZEM + ' ' + VW.POSICAO  ) AS DISPLAY   ");
                    SQL.AppendLine(" (VW.EMBALAGEM + ' ' + VW.DESCR_ARMAZEM + ' ' + VW.POSICAO  ) AS DISPLAY   ");
                    SQL.AppendLine("  FROM (   ");
                    SQL.AppendLine("    SELECT a.descr as DESCR_ARMAZEM, ");
                    SQL.AppendLine("          CAST(ISNULL (m.volumes, 0) AS VARCHAR) AS QTDE, ");
                    SQL.AppendLine("          e.descr AS EMBALAGEM, ");
                    SQL.AppendLine("          y.yard AS POSICAO, ");
                    SQL.AppendLine("          'I' AS SISTEMA, ");
                    SQL.AppendLine("          'Y' + LTRIM (CAST(y.autonum AS VARCHAR)) AS ID_GRAVACAO, ");
                    SQL.AppendLine("          c.quantidade AS QTDE_CAPTADA, ");
                    SQL.AppendLine("          m.autonum AS MARCANTE, ");
                    SQL.AppendLine("          CAST(b.autonum AS VARCHAR) AS LOTE_STR, ");
                    SQL.AppendLine("          B.NUMERO AS BL, ");
                    SQL.AppendLine("          A.AUTONUM AS AUTONUM_ARMAZEM ");
                    SQL.AppendLine("     FROM sgipa.dbo.tb_carga_solta c ");
                    SQL.AppendLine("          INNER JOIN sgipa.dbo.tb_bl b ");
                    SQL.AppendLine("             ON c.bl = b.autonum ");
                    SQL.AppendLine("          INNER JOIN sgipa.dbo.tb_carga_solta_yard y ");
                    SQL.AppendLine("             ON c.autonum = y.autonum_cs ");
                    SQL.AppendLine("          INNER JOIN sgipa.dbo.tb_marcantes m ");
                    SQL.AppendLine("             ON c.autonum = m.autonum_carga AND y.autonum = m.autonum_cs_yard ");
                    SQL.AppendLine("          INNER JOIN sgipa.dbo.tb_armazens_ipa a ");
                    SQL.AppendLine("             ON y.armazem = a.autonum ");
                    SQL.AppendLine("          LEFT JOIN sgipa.dbo.dte_tb_embalagens e ");
                    SQL.AppendLine("             ON c.embalagem = e.code ");
                    SQL.AppendLine("    WHERE      ");
                    //SQL.AppendLine("            B.AUTONUM = " + lote + " ");
                    //SQL.AppendLine("            AND B.PATIO = " + patio + " ");
                    //SQL.AppendLine("            AND  ");
                    SQL.AppendLine("          c.flag_terminal = 1 ");
                    SQL.AppendLine("          AND b.flag_ativo = 1 ");
                    SQL.AppendLine("          AND c.flag_historico = 0 ");
                    SQL.AppendLine("          AND m.volumes > 0           ");
                    SQL.AppendLine("   UNION ");
                    SQL.AppendLine("   SELECT i.descr as DESCR_ARMAZEM, ");
                    SQL.AppendLine("          cast(ISNULL (m.volumes, 0) AS VARCHAR) AS QTDE, ");
                    SQL.AppendLine("          e.descr AS EMBALAGEM, ");
                    SQL.AppendLine("          '-' AS POSICAO, ");
                    SQL.AppendLine("          'I' AS SISTEMA, ");
                    SQL.AppendLine("          'C' + LTRIM (CAST(c.autonum AS VARCHAR)) AS ID_GRAVACAO, ");
                    SQL.AppendLine("          c.quantidade AS QTDE_CAPTADA, ");
                    SQL.AppendLine("          m.autonum AS MARCANTE, ");
                    SQL.AppendLine("          CAST(b.autonum as varchar) AS LOTE_STR, ");
                    SQL.AppendLine("          B.NUMERO AS BL , ");
                    SQL.AppendLine("          I.AUTONUM AS AUTONUM_ARMAZEM ");
                    SQL.AppendLine("     FROM sgipa.dbo.tb_carga_solta c ");
                    SQL.AppendLine("          INNER JOIN sgipa.dbo.tb_bl b ");
                    SQL.AppendLine("             ON c.bl = b.autonum ");
                    SQL.AppendLine("          INNER JOIN sgipa.dbo.tb_marcantes m ");
                    SQL.AppendLine("             ON c.autonum = m.autonum_carga ");
                    SQL.AppendLine("          INNER JOIN sgipa.dbo.tb_armazens_ipa i ");
                    SQL.AppendLine("             ON c.patio = i.patio ");
                    SQL.AppendLine("          LEFT JOIN sgipa.dbo.dte_tb_embalagens e ");
                    SQL.AppendLine("             ON c.embalagem = e.code ");
                    SQL.AppendLine("    WHERE  ");
                    //SQL.AppendLine("         B.AUTONUM = " + lote + " ");
                    //SQL.AppendLine("         AND B.PATIO = " + patio + " ");
                    //SQL.AppendLine("         AND ");
                    SQL.AppendLine(" b.flag_ativo = 1  ");
                    SQL.AppendLine("         AND c.flag_terminal = 1 ");
                    SQL.AppendLine("         AND c.flag_historico = 0 ");
                    SQL.AppendLine("         AND i.flag_padrao_patio = 1 ");
                    SQL.AppendLine("         AND m.volumes > 0 ");
                    SQL.AppendLine("         AND m.autonum_cs_yard = 0  ");
                    SQL.AppendLine("   UNION ");
                    SQL.AppendLine("   SELECT i.descr as DESCR_ARMAZEM, ");
                    SQL.AppendLine("         CAST( y.quantidade - ISNULL (M.VOLUMES, 0) AS VARCHAR) AS QTDE, ");
                    SQL.AppendLine("          e.descr AS EMBALAGEM, ");
                    SQL.AppendLine("          y.yard AS POSICAO, ");
                    SQL.AppendLine("          'I' AS SISTEMA, ");
                    SQL.AppendLine("          'Y' + LTRIM (CAST(y.autonum AS VARCHAR)) AS ID_GRAVACAO, ");
                    SQL.AppendLine("          c.quantidade AS QTDE_CAPTADA, ");
                    SQL.AppendLine("          '000000000000' AS MARCANTE, ");
                    SQL.AppendLine("          cast(b.autonum as varchar) AS LOTE_STR, ");
                    SQL.AppendLine("          B.NUMERO AS BL, ");
                    SQL.AppendLine("          I.AUTONUM AS AUTONUM_ARMAZEM ");
                    SQL.AppendLine("     FROM sgipa.dbo.tb_carga_solta c ");
                    SQL.AppendLine("          INNER JOIN sgipa.dbo.tb_bl b ");
                    SQL.AppendLine("             ON c.bl = b.autonum ");
                    SQL.AppendLine("          INNER JOIN sgipa.dbo.tb_carga_solta_yard y ");
                    SQL.AppendLine("             ON c.autonum = y.autonum_cs ");
                    SQL.AppendLine("          INNER JOIN sgipa.dbo.tb_armazens_ipa i ");
                    SQL.AppendLine("             ON y.armazem = i.autonum ");
                    SQL.AppendLine("          LEFT JOIN operador.dbo.vw_carga_solta_mc mc ");
                    SQL.AppendLine("             ON c.autonum = mc.autonum_cs ");
                    SQL.AppendLine("          LEFT JOIN sgipa.dbo.tb_marcantes m ");
                    SQL.AppendLine("             ON y.autonum = m.autonum_cs_yard ");
                    SQL.AppendLine("          LEFT JOIN sgipa.dbo.dte_tb_embalagens e ");
                    SQL.AppendLine("             ON c.embalagem = e.code ");
                    SQL.AppendLine("    WHERE      ");
                    //SQL.AppendLine("         B.AUTONUM = " + lote + " ");
                    //SQL.AppendLine("         AND B.PATIO = " + patio + " ");
                    //SQL.AppendLine("         AND ");
                    SQL.AppendLine("  b.flag_ativo = 1  ");
                    SQL.AppendLine("         AND c.flag_terminal = 1 ");
                    SQL.AppendLine("          AND c.flag_historico = 0 ");
                    SQL.AppendLine("          AND y.quantidade > 0 ");
                    SQL.AppendLine("          AND (  ISNULL (c.quantidade_real, 0) ");
                    SQL.AppendLine("               - ISNULL (c.quantidade_saida, 0) ");
                    SQL.AppendLine("               - ISNULL (mc.quantidade, 0)) > 0 ");
                    SQL.AppendLine("          AND (m.autonum IS NULL OR ISNULL (M.VOLUMES, 0) <> Y.QUANTIDADE) ");
                    SQL.AppendLine("   UNION ");
                    SQL.AppendLine("   SELECT i.descr as DESCR_ARMAZEM, ");
                    SQL.AppendLine("          CAST(  ISNULL (c.quantidade_real, 0) ");
                    SQL.AppendLine("           - ISNULL (c.quantidade_saida, 0) ");
                    SQL.AppendLine("           - ISNULL (vw.quantidade, 0) AS VARCHAR) ");
                    SQL.AppendLine("             AS QTDE, ");
                    SQL.AppendLine("          e.descr AS EMBALAGEM, ");
                    SQL.AppendLine("          '-' AS POSICAO, ");
                    SQL.AppendLine("          'I' AS SISTEMA, ");
                    SQL.AppendLine("          'C' + LTRIM (CAST(c.autonum AS VARCHAR)) AS ID_GRAVACAO, ");
                    SQL.AppendLine("          c.quantidade AS QTDE_CAPTADA, ");
                    SQL.AppendLine("          '000000000000' AS MARCANTE, ");
                    SQL.AppendLine("          CAST(b.autonum AS VARCHAR) AS LOTE_STR, ");
                    SQL.AppendLine("          B.NUMERO AS BL, ");
                    SQL.AppendLine("          I.AUTONUM AS AUTONUM_ARMAZEM ");
                    SQL.AppendLine("     FROM sgipa.dbo.tb_carga_solta c ");
                    SQL.AppendLine("          INNER JOIN sgipa.dbo.tb_bl b ");
                    SQL.AppendLine("             ON c.bl = b.autonum ");
                    SQL.AppendLine("          INNER JOIN sgipa.dbo.tb_armazens_ipa i ");
                    SQL.AppendLine("             ON c.patio = i.patio ");
                    SQL.AppendLine("          LEFT JOIN operador.dbo.vw_carga_solta_pos vw ");
                    SQL.AppendLine("             ON c.autonum = vw.autonum_cs ");
                    SQL.AppendLine("          LEFT JOIN operador.dbo.vw_carga_solta_mc vwmc ");
                    SQL.AppendLine("             ON c.autonum = vwmc.autonum_cs ");
                    SQL.AppendLine("          LEFT JOIN sgipa.dbo.dte_tb_embalagens e ");
                    SQL.AppendLine("             ON c.embalagem = e.code ");
                    SQL.AppendLine("    WHERE      ");
                    //SQL.AppendLine("         B.AUTONUM = " + lote + " ");
                    //SQL.AppendLine("         AND B.PATIO = " + patio + " ");
                    //SQL.AppendLine("         AND  ");
                    SQL.AppendLine(" b.flag_ativo = 1 ");
                    SQL.AppendLine("         AND c.flag_terminal = 1 ");
                    SQL.AppendLine("          AND c.flag_historico = 0 ");
                    SQL.AppendLine("          AND i.flag_padrao_patio = 1 ");
                    SQL.AppendLine("          AND (  ISNULL (c.quantidade_real, 0) ");
                    SQL.AppendLine("               - ISNULL (c.quantidade_saida, 0) ");
                    SQL.AppendLine("               - ISNULL (vw.quantidade, 0) ");
                    SQL.AppendLine("               - ISNULL (vwmc.quantidade, 0)) > 0 ) VW ");
                    //SQL.AppendLine("            WHERE QTDE>0  ");

                    var query = _db.Query<ArmazensDTO>(SQL.ToString()).AsEnumerable();

                    return query;
                }                
                
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        //public hist_SHIFTINGDTO InsertDados(hist_SHIFTINGDTO obj)
        //{
        //    try
        //    {
        //        using (var _db = new SqlConnection(Config.StringConexao()))
        //        { 

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}
    }
}
