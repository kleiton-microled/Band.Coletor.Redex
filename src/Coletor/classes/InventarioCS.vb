Imports System.Data.OleDb
Imports System.Data.SqlClient

Public Class InventarioCS
    Inherits System.Web.UI.Page

    Public _Mercadoria As String
    Public _Marca As String
    Public _Entrada As String
    Public _Cliente As String
    Public _Conteiner As String

    Public _Canal As String
    Public _Volume As String
    Public _Doc As String
    Public _Movimento As String
    Public _Imo As String


    Public _Quantidade As String
    Public _Embalagem As String
    Public _Local As String
    Public _Patio As String
    Public _Nvocc As String
    Public _Bl As String
    Public Property Nvocc() As String
        Get
            Return Me._Nvocc
        End Get
        Set(ByVal value As String)
            Me._Nvocc = value
        End Set
    End Property

    Public Property BL() As String
        Get
            Return Me._Bl
        End Get
        Set(ByVal value As String)
            Me._Bl = value
        End Set
    End Property

    Public Property Canal() As String
        Get
            Return Me._Canal
        End Get
        Set(ByVal value As String)
            Me._Canal = value
        End Set
    End Property

    Public Property Volume() As String
        Get
            Return Me._Volume
        End Get
        Set(ByVal value As String)
            Me._Volume = value
        End Set
    End Property


    Public Property Doc() As String
        Get
            Return Me._Doc
        End Get
        Set(ByVal value As String)
            Me._Doc = value
        End Set
    End Property

    Public Property Movimento() As String
        Get
            Return Me._Movimento
        End Get
        Set(ByVal value As String)
            Me._Movimento = value
        End Set
    End Property

    Public Property IMO() As String
        Get
            Return Me._Imo
        End Get
        Set(ByVal value As String)
            Me._Imo = value
        End Set
    End Property


    Public Property Patio() As String
        Get
            Return Me._Patio
        End Get
        Set(ByVal value As String)
            Me._Patio = value
        End Set
    End Property

    Public Property Mercadoria() As String
        Get
            Return Me._Mercadoria
        End Get
        Set(ByVal value As String)
            Me._Mercadoria = value
        End Set
    End Property
    Public Property Quantidade() As String
        Get
            Return Me._Quantidade
        End Get
        Set(ByVal value As String)
            Me._Quantidade = value
        End Set
    End Property

    Public Property Embalagem() As String
        Get
            Return Me._Embalagem
        End Get
        Set(ByVal value As String)
            Me._Embalagem = value
        End Set
    End Property

    Public Property Local() As String
        Get
            Return Me._Local
        End Get
        Set(ByVal value As String)
            Me._Local = value
        End Set
    End Property

    Public Property Marca() As String
        Get
            Return Me._Marca
        End Get
        Set(ByVal value As String)
            Me._Marca = value
        End Set
    End Property


    Public Property Entrada() As String
        Get
            Return Me._Entrada
        End Get
        Set(ByVal value As String)
            Me._Entrada = value
        End Set
    End Property

    Public Property Cliente() As String
        Get
            Return Me._Cliente
        End Get
        Set(ByVal value As String)
            Me._Cliente = value
        End Set
    End Property

    Public Property Conteiner() As String
        Get
            Return Me._Conteiner
        End Get
        Set(ByVal value As String)
            Me._Conteiner = value
        End Set
    End Property


    Public Shared Function PopulaDadosLote(QualLote As String, QualPatio As Integer, Optional SoRedex As Boolean = False) As InventarioCS_f2

        Dim rsCs As New DataTable
        Dim SQL As New StringBuilder

        Using p_connect As New SqlConnection(Banco.ConnectionString())
            Using p_cmd As New SqlCommand()

                If IIf(SoRedex, 1, 0) = 1 Then

                    SQL.Append("   SELECT PATIO AS ID_PATIO, ")
                    SQL.Append("          BL , ")
                    SQL.Append("          LOTE, ")
                    SQL.Append("          MERCADORIA, ")
                    SQL.Append("          MARCA, ")
                    SQL.Append("          IMPORTADOR, ")
                    SQL.Append("          DT_ENTRADA AS DATA_ENTRADA, ")
                    SQL.Append("          CNTR_DESOVA, ")
                    SQL.Append("          VOLUME, ")
                    SQL.Append("          FINALITY, ")
                    SQL.Append("          TIPO_DOC, ")
                    SQL.Append("          CANAL_ALF, ")
                    SQL.Append("          PROX_MOVIMENTO AS MOTIVO_PROX_MVTO, ")
                    SQL.Append("          SISTEMA, ")
                    SQL.Append("          IMO, ")
                    SQL.Append("          NVOCC, ")
                    SQL.Append("          ANVISA, ")
                    SQL.Append("          FLAG_ANVISA, ")
                    SQL.Append("          LOTE_STR ")
                    SQL.Append("     FROM (SELECT a.patio, ")
                    SQL.Append("                  C.REFERENCE AS BL, ")
                    SQL.Append("                  0 AS lote, ")
                    SQL.Append("                  B.DESC_PRODUTO AS MERCADORIA, ")
                    SQL.Append("                  A.MARCA, ")
                    SQL.Append("                  H.FANTASIA AS IMPORTADOR, ")
                    SQL.Append("                  A.DT_PRIM_ENTRADA AS DT_ENTRADA, ")
                    SQL.Append("                  '' AS CNTR_DESOVA, ")
                    SQL.Append("                  A.VOLUME_DECLARADO AS VOLUME, ")
                    SQL.Append("                  'RDX' AS finality, ")
                    SQL.Append("                  ISNULL (SDBOO.DOC_EXP, '-') AS tipo_doc, ")
                    SQL.Append("                  0 AS canal_alf, ")
                    SQL.Append("                  '' AS PROX_MOVIMENTO, ")
                    SQL.Append("                  'R' AS SISTEMA, ")
                    SQL.Append("                  A.IMO, ")
                    SQL.Append("                  '' AS NVOCC, ")
                    SQL.Append("                  '-' AS ANVISA, ")
                    SQL.Append("                  0 AS FLAG_ANVISA, ")
                    SQL.Append("                  C.os AS LOTE_STR, ")
                    SQL.Append("                    A.QTDE_ENTRADA ")
                    SQL.Append("                  - ISNULL ( (SELECT SUM (qtde_saida) ")
                    SQL.Append("                             FROM REDEX.dbo.tb_saida_carga w ")
                    SQL.Append("                            WHERE w.autonum_pcs = a.autonum_pcs), ")
                    SQL.Append("                         0) ")
                    SQL.Append("                     AS SALDO ")
                    SQL.Append("             FROM REDEX.dbo.TB_PATIO_CS A ")
                    SQL.Append("                  INNER JOIN REDEX.dbo.TB_BOOKING_CARGA D ")
                    SQL.Append("                     ON A.AUTONUM_BCG = D.AUTONUM_BCG ")
                    SQL.Append("                  INNER JOIN REDEX.dbo.TB_BOOKING C ")
                    SQL.Append("                     ON D.AUTONUM_BOO = C.AUTONUM_BOO ")
                    SQL.Append("                  LEFT JOIN REDEX.dbo.TB_CAD_PRODUTOS B ")
                    SQL.Append("                     ON d.AUTONUM_PRO = B.AUTONUM_PRO ")
                    SQL.Append("                  LEFT JOIN REDEX.dbo.TB_CAD_PARCEIROS H ")
                    SQL.Append("                     ON C.AUTONUM_PARCEIRO = H.AUTONUM ")
                    SQL.Append("                  LEFT JOIN REDEX.dbo.TB_booking BM ")
                    SQL.Append("                     ON C.reserva_master = bm.autonum_boo ")
                    SQL.Append("                  LEFT JOIN REDEX.dbo.TB_booking BS ")
                    SQL.Append("                     ON C.reserva_pai = bs.autonum_boo ")
                    SQL.Append("                  LEFT JOIN (  SELECT MIN (td.descricao) AS DOC_EXP, ")
                    SQL.Append("                                      sdb.autonum_boo ")
                    SQL.Append("                                 FROM redex.dbo.tb_cad_sd sd ")
                    SQL.Append("                                      INNER JOIN redex.dbo.tb_cad_tipo_documento td ")
                    SQL.Append("                                         ON sd.autonum_tipo_doc = td.autonum ")
                    SQL.Append("                                      LEFT JOIN redex.dbo.tb_cad_sd_boo sdb ")
                    SQL.Append("                                         ON sd.autonum_sd = sdb.autonum_sd ")
                    SQL.Append("                                      LEFT JOIN redex.dbo.tb_booking boo ")
                    SQL.Append("                                         ON sdb.autonum_boo = boo.autonum_boo ")
                    SQL.Append("                             GROUP BY sdb.autonum_boo) SDBOO ")
                    SQL.Append("                     ON D.AUTONUM_BOO = SDBOO.AUTONUM_BOO ")
                    SQL.Append("            WHERE   C.OS='" & QualLote & "' AND a.patio = " & QualLote & " AND  d.flag_cs = 1 ")
                    SQL.Append("                  AND ISNULL (a.flag_historico, 0) = 0 ")
                    SQL.Append("                  ) ")
                    SQL.Append("    WHERE SALDO > 0 ")

                Else

                    SQL.Append("      SELECT c.patio AS ID_PATIO, ")
                    SQL.Append("              b.numero AS BL, ")
                    SQL.Append("              b.autonum AS LOTE, ")
                    SQL.Append("              c.mercadoria AS MERCADORIA, ")
                    SQL.Append("              c.marca AS MARCA, ")
                    SQL.Append("              p.razao AS importador, ")
                    SQL.Append("              CASE WHEN cntr.dt_entrada IS NULL THEN c.dt_entrada ELSE cntr.dt_entrada END AS DATA_ENTRADA, ")
                    SQL.Append("              cntr.id_conteiner AS cntr_desova, ")
                    SQL.Append("              ISNULL (c.volume_m3, 0) AS volume, ")
                    SQL.Append("              'IPA-CS' AS finality, ")
                    SQL.Append("              ISNULL (td.descr, 'NOT DEFINED YET') AS tipo_doc, ")
                    SQL.Append("              ISNULL (b.canal_siscomex, 9) AS canal_alf, ")
                    SQL.Append("              vw3.motivo AS motivo_prox_mvto, ")
                    SQL.Append("              'I' AS sistema, ")
                    SQL.Append("              CASE WHEN c.imo = '0' THEN '' ELSE c.imo end AS IMO, ")
                    SQL.Append("              vw2.razao AS nvocc, ")
                    SQL.Append("              ISNULL (ANV.DESCR, '-') AS ANVISA, ")
                    SQL.Append("              ISNULL (B.FLAG_ANVISA, 0) AS FLAG_ANVISA, ")
                    SQL.Append("              CAST(b.autonum AS VARCHAR) AS LOTE_STR ")
                    SQL.Append("         FROM SGIPA.dbo.tb_carga_solta c ")
                    SQL.Append("              INNER JOIN sgipa.dbo.tb_bl b ")
                    SQL.Append("                 ON c.bl = b.autonum ")
                    SQL.Append("              LEFT JOIN sgipa.dbo.tb_cntr_bl cntr ")
                    SQL.Append("                 ON c.cntr = cntr.autonum ")
                    SQL.Append("              LEFT JOIN sgipa.dbo.tb_cad_parceiros p ")
                    SQL.Append("                 ON b.importador = p.autonum ")
                    SQL.Append("              LEFT JOIN sgipa.dbo.tb_cad_parceiros vw2 ")
                    SQL.Append("                 ON b.captador = vw2.autonum ")
                    SQL.Append("              LEFT JOIN sgipa.dbo.tb_tipos_documentos td ")
                    SQL.Append("                 ON b.tipo_documento = td.code ")
                    SQL.Append("              LEFT JOIN sgipa.dbo.vw_prox_mov_cs_un vw3 ")
                    SQL.Append("                 ON b.autonum = vw3.lote ")
                    SQL.Append("              LEFT JOIN SGIPA.dbo.TB_BL_ANVISA BANV ")
                    SQL.Append("                 ON C.BL = BANV.LOTE ")
                    SQL.Append("              LEFT JOIN SGIPA.dbo.TB_CAD_ANVISA ANV ")
                    SQL.Append("                 ON BANV.COD_ANVISA = ANV.AUTONUM ")
                    SQL.Append("        WHERE B.AUTONUM = " & QualLote & " AND c.patio = " & QualPatio & " AND c.flag_terminal = 1 AND c.flag_historico = 0 ")

                    SQL.Append(" UNION ")

                    SQL.Append("   SELECT PATIO AS ID_PATIO, ")
                    SQL.Append("          BL AS BL, ")
                    SQL.Append("          LOTE, ")
                    SQL.Append("          MERCADORIA, ")
                    SQL.Append("          MARCA, ")
                    SQL.Append("          IMPORTADOR, ")
                    SQL.Append("          DT_ENTRADA AS DATA_ENTRADA, ")
                    SQL.Append("          CNTR_DESOVA, ")
                    SQL.Append("          VOLUME, ")
                    SQL.Append("          FINALITY, ")
                    SQL.Append("          TIPO_DOC, ")
                    SQL.Append("          CANAL_ALF, ")
                    SQL.Append("          PROX_MOVIMENTO AS MOTIVO_PROX_MVTO, ")
                    SQL.Append("          SISTEMA, ")
                    SQL.Append("          IMO, ")
                    SQL.Append("          NVOCC, ")
                    SQL.Append("          ANVISA, ")
                    SQL.Append("          FLAG_ANVISA, ")
                    SQL.Append("          LOTE_STR ")
                    SQL.Append("     FROM (SELECT a.patio, ")
                    SQL.Append("                  C.REFERENCE AS BL, ")
                    SQL.Append("                  0 AS lote, ")
                    SQL.Append("                  B.DESC_PRODUTO AS MERCADORIA, ")
                    SQL.Append("                  A.MARCA, ")
                    SQL.Append("                  H.FANTASIA AS IMPORTADOR, ")
                    SQL.Append("                  A.DT_PRIM_ENTRADA AS DT_ENTRADA, ")
                    SQL.Append("                  '' AS CNTR_DESOVA, ")
                    SQL.Append("                  A.VOLUME_DECLARADO AS VOLUME, ")
                    SQL.Append("                  'RDX' AS finality, ")
                    SQL.Append("                  ISNULL (SDBOO.DOC_EXP, '-') AS tipo_doc, ")
                    SQL.Append("                  0 AS canal_alf, ")
                    SQL.Append("                  '' AS PROX_MOVIMENTO, ")
                    SQL.Append("                  'R' AS SISTEMA, ")
                    SQL.Append("                  A.IMO, ")
                    SQL.Append("                  '' AS NVOCC, ")
                    SQL.Append("                  '-' AS ANVISA, ")
                    SQL.Append("                  0 AS FLAG_ANVISA, ")
                    SQL.Append("                  C.os AS LOTE_STR, ")
                    SQL.Append("                    A.QTDE_ENTRADA ")
                    SQL.Append("                  - ISNULL ( (SELECT SUM (qtde_saida) ")
                    SQL.Append("                             FROM REDEX.dbo.tb_saida_carga w ")
                    SQL.Append("                            WHERE w.autonum_pcs = a.autonum_pcs), ")
                    SQL.Append("                         0) ")
                    SQL.Append("                     AS SALDO ")
                    SQL.Append("             FROM REDEX.dbo.TB_PATIO_CS A ")
                    SQL.Append("                  INNER JOIN REDEX.dbo.TB_BOOKING_CARGA D ")
                    SQL.Append("                     ON A.AUTONUM_BCG = D.AUTONUM_BCG ")
                    SQL.Append("                  INNER JOIN REDEX.dbo.TB_BOOKING C ")
                    SQL.Append("                     ON D.AUTONUM_BOO = C.AUTONUM_BOO ")
                    SQL.Append("                  LEFT JOIN REDEX.dbo.TB_CAD_PRODUTOS B ")
                    SQL.Append("                     ON d.AUTONUM_PRO = B.AUTONUM_PRO ")
                    SQL.Append("                  LEFT JOIN REDEX.dbo.TB_CAD_PARCEIROS H ")
                    SQL.Append("                     ON C.AUTONUM_PARCEIRO = H.AUTONUM ")
                    SQL.Append("                  LEFT JOIN REDEX.dbo.TB_booking BM ")
                    SQL.Append("                     ON C.reserva_master = bm.autonum_boo ")
                    SQL.Append("                  LEFT JOIN REDEX.dbo.TB_booking BS ")
                    SQL.Append("                     ON C.reserva_pai = bs.autonum_boo ")
                    SQL.Append("                  LEFT JOIN (  SELECT MIN (td.descricao) AS DOC_EXP, ")
                    SQL.Append("                                      sdb.autonum_boo ")
                    SQL.Append("                                 FROM redex.dbo.tb_cad_sd sd ")
                    SQL.Append("                                      INNER JOIN redex.dbo.tb_cad_tipo_documento td ")
                    SQL.Append("                                         ON sd.autonum_tipo_doc = td.autonum ")
                    SQL.Append("                                      LEFT JOIN redex.dbo.tb_cad_sd_boo sdb ")
                    SQL.Append("                                         ON sd.autonum_sd = sdb.autonum_sd ")
                    SQL.Append("                                      LEFT JOIN redex.dbo.tb_booking boo ")
                    SQL.Append("                                         ON sdb.autonum_boo = boo.autonum_boo ")
                    SQL.Append("                             GROUP BY sdb.autonum_boo) SDBOO ")
                    SQL.Append("                     ON D.AUTONUM_BOO = SDBOO.AUTONUM_BOO ")
                    SQL.Append("            WHERE   C.OS='" & QualLote & "' AND a.patio = " & QualPatio & " AND  d.flag_cs = 1 ")
                    SQL.Append("                  AND ISNULL (a.flag_historico, 0) = 0 ")
                    SQL.Append("                  )Q ")
                    SQL.Append("    WHERE SALDO > 0 ")

                End If

                rsCs = Banco.Consultar(SQL.ToString())

                If rsCs IsNot Nothing Then
                    If rsCs.Rows.Count > 0 Then

                        Dim OrdensOBJ As New InventarioCS_f2

                        OrdensOBJ.Mercadoria = rsCs.Rows(0)("MERCADORIA").ToString
                        OrdensOBJ.Marca = rsCs.Rows(0)("MARCA").ToString
                        OrdensOBJ.Entrada = rsCs.Rows(0)("DATA_ENTRADA").ToString
                        OrdensOBJ.Cliente = rsCs.Rows(0)("IMPORTADOR").ToString
                        OrdensOBJ.Conteiner = rsCs.Rows(0)("CNTR_DESOVA").ToString
                        OrdensOBJ.Patio = rsCs.Rows(0)("ID_PATIO").ToString
                        OrdensOBJ.Volume = rsCs.Rows(0)("VOLUME").ToString
                        OrdensOBJ.Doc = rsCs.Rows(0)("TIPO_DOC").ToString
                        OrdensOBJ.Canal = rsCs.Rows(0)("CANAL_ALF").ToString
                        OrdensOBJ.Movimento = rsCs.Rows(0)("MOTIVO_PROX_MVTO").ToString
                        OrdensOBJ.IMO = rsCs.Rows(0)("IMO").ToString
                        OrdensOBJ.Nvocc = rsCs.Rows(0)("NVOCC").ToString
                        OrdensOBJ.BL = rsCs.Rows(0)("BL").ToString
                        OrdensOBJ.Sistema = rsCs.Rows(0)("FINALITY").ToString
                        OrdensOBJ.ANVISA = rsCs.Rows(0)("ANVISA").ToString
                        OrdensOBJ.FlagAnvisa = rsCs.Rows(0)("FLAG_ANVISA").ToString
                        Return OrdensOBJ

                    End If
                End If

            End Using
        End Using

        Return Nothing

    End Function

    Public Shared Function PopulaItem(Id_Gravacao As String) As InventarioCS_f2

        Dim Sql As String
        Dim SqlF As String

        If Microsoft.VisualBasic.Mid(Id_Gravacao, 1, 2) <> "RV" Then

            Dim rsCs As New DataTable

            Dim iSQL As New StringBuilder
            Dim PAutonumCs As String = Id_Gravacao.Replace("IU", "").Replace("RU", "").Replace("RV", "").Replace("C", "").Replace("Y", "")

            If Mid(Id_Gravacao, 3, 1) = "Y" And Mid(Id_Gravacao, 1, 1) = "I" Then

                iSQL.Append("SELECT  QTDE,EMBALAGEM,(DESCR_ARMAZEM + ' ' + POSICAO) AS LOCAL, POSICAO AS YARD  ")
                iSQL.Append("  FROM ( ")
                iSQL.Append("    SELECT a.descr as DESCR_ARMAZEM, ")
                iSQL.Append("          ISNULL (m.volumes, 0) AS QTDE, ")
                iSQL.Append("          e.descr AS EMBALAGEM, ")
                iSQL.Append("          y.yard AS POSICAO, ")
                iSQL.Append("          'I' AS SISTEMA, ")
                iSQL.Append("          'Y' + LTRIM (CAST(y.autonum as varchar)) AS ID_GRAVACAO, ")
                iSQL.Append("          c.quantidade AS QTDE_CAPTADA, ")
                iSQL.Append("          m.autonum AS MARCANTE, ")
                iSQL.Append("          CAST(b.autonum as varchar) AS LOTE_STR, ")
                iSQL.Append("          B.NUMERO AS BL, ")
                iSQL.Append("          A.AUTONUM AS AUTONUM_ARMAZEM ")
                iSQL.Append("     FROM sgipa.dbo.tb_carga_solta c ")
                iSQL.Append("          INNER JOIN sgipa.dbo.tb_bl b ")
                iSQL.Append("             ON c.bl = b.autonum ")
                iSQL.Append("          INNER JOIN sgipa.dbo.tb_carga_solta_yard y ")
                iSQL.Append("             ON c.autonum = y.autonum_cs ")
                iSQL.Append("          INNER JOIN sgipa.dbo.tb_marcantes m ")
                iSQL.Append("             ON c.autonum = m.autonum_carga AND y.autonum = m.autonum_cs_yard ")
                iSQL.Append("          INNER JOIN sgipa.dbo.tb_armazens_ipa a ")
                iSQL.Append("             ON y.armazem = a.autonum ")
                iSQL.Append("          LEFT JOIN sgipa.dbo.dte_tb_embalagens e ")
                iSQL.Append("             ON c.embalagem = e.code ")
                iSQL.Append("    WHERE ")
                iSQL.Append("            Y.AUTONUM = " & PAutonumCs & " ")
                iSQL.Append("            AND c.flag_terminal = 1 ")
                iSQL.Append("          AND b.flag_ativo = 1 ")
                iSQL.Append("          AND c.flag_historico = 0 ")
                iSQL.Append("          AND m.volumes > 0  ")
                iSQL.Append("    UNION   ")
                iSQL.Append("   SELECT i.descr as DESCR_ARMAZEM, ")
                iSQL.Append("          y.quantidade - ISNULL (M.VOLUMES, 0) AS QTDE, ")
                iSQL.Append("          e.descr AS EMBALAGEM, ")
                iSQL.Append("          y.yard AS POSICAO, ")
                iSQL.Append("          'I' AS SISTEMA, ")
                iSQL.Append("          'Y' + LTRIM (CAST(y.autonum as varchar)) AS ID_GRAVACAO, ")
                iSQL.Append("          c.quantidade AS QTDE_CAPTADA, ")
                iSQL.Append("          '000000000000' AS MARCANTE, ")
                iSQL.Append("          cast(b.autonum as varchar) AS LOTE_STR, ")
                iSQL.Append("          B.NUMERO AS BL, ")
                iSQL.Append("          I.AUTONUM AS AUTONUM_ARMAZEM ")
                iSQL.Append("     FROM sgipa.dbo.tb_carga_solta c ")
                iSQL.Append("          INNER JOIN sgipa.dbo.tb_bl b ")
                iSQL.Append("             ON c.bl = b.autonum ")
                iSQL.Append("          INNER JOIN sgipa.dbo.tb_carga_solta_yard y ")
                iSQL.Append("             ON c.autonum = y.autonum_cs ")
                iSQL.Append("          INNER JOIN sgipa.dbo.tb_armazens_ipa i ")
                iSQL.Append("             ON y.armazem = i.autonum ")
                iSQL.Append("          LEFT JOIN operador.dbo.vw_carga_solta_mc mc ")
                iSQL.Append("             ON c.autonum = mc.autonum_cs ")
                iSQL.Append("          LEFT JOIN sgipa.dbo.tb_marcantes m ")
                iSQL.Append("             ON y.autonum = m.autonum_cs_yard ")
                iSQL.Append("          LEFT JOIN sgipa.dbo.dte_tb_embalagens e ")
                iSQL.Append("             ON c.embalagem = e.code ")
                iSQL.Append("    WHERE      ")
                iSQL.Append("         Y.AUTONUM = " & PAutonumCs & " ")
                iSQL.Append("         AND b.flag_ativo = 1 ")
                iSQL.Append("         AND c.flag_terminal = 1 ")
                iSQL.Append("          AND c.flag_historico = 0 ")
                iSQL.Append("          AND y.quantidade > 0 ")
                iSQL.Append("          AND (  ISNULL (c.quantidade_real, 0) ")
                iSQL.Append("               - ISNULL (c.quantidade_saida, 0) ")
                iSQL.Append("               - ISNULL (mc.quantidade, 0)) > 0 ")
                iSQL.Append("          AND (m.autonum IS NULL OR ISNULL (M.VOLUMES, 0) <> Y.QUANTIDADE) )Q ")

            End If

            If Mid(Id_Gravacao, 3, 1) = "C" And Mid(Id_Gravacao, 1, 1) = "I" Then

                iSQL.Append("    SELECT  QTDE,EMBALAGEM,(DESCR_ARMAZEM + ' ' + POSICAO) AS LOCAL, POSICAO AS YARD  ")
                iSQL.Append("    FROM (    ")
                iSQL.Append("   SELECT i.descr as DESCR_ARMAZEM, ")
                iSQL.Append("          ISNULL (m.volumes, 0) AS QTDE, ")
                iSQL.Append("          e.descr AS EMBALAGEM, ")
                iSQL.Append("          '-' AS POSICAO, ")
                iSQL.Append("          'I' AS SISTEMA, ")
                iSQL.Append("          'C' + LTRIM (CAST(c.autonum AS VARCHAR)) AS ID_GRAVACAO, ")
                iSQL.Append("          c.quantidade AS QTDE_CAPTADA, ")
                iSQL.Append("          m.autonum AS MARCANTE, ")
                iSQL.Append("          CAST(b.autonum as varchar) AS LOTE_STR, ")
                iSQL.Append("          B.NUMERO AS BL , ")
                iSQL.Append("          I.AUTONUM AS AUTONUM_ARMAZEM ")
                iSQL.Append("     FROM sgipa.dbo.tb_carga_solta c ")
                iSQL.Append("          INNER JOIN sgipa.dbo.tb_bl b ")
                iSQL.Append("             ON c.bl = b.autonum ")
                iSQL.Append("          INNER JOIN sgipa.dbo.tb_marcantes m ")
                iSQL.Append("             ON c.autonum = m.autonum_carga ")
                iSQL.Append("          INNER JOIN sgipa.dbo.tb_armazens_ipa i ")
                iSQL.Append("             ON c.patio = i.patio ")
                iSQL.Append("          LEFT JOIN sgipa.dbo.dte_tb_embalagens e ")
                iSQL.Append("             ON c.embalagem = e.code ")
                iSQL.Append("    WHERE      ")
                iSQL.Append("         C.AUTONUM = " & PAutonumCs & " ")
                iSQL.Append("         AND b.flag_ativo = 1 ")
                iSQL.Append("         AND c.flag_terminal = 1 ")
                iSQL.Append("         AND c.flag_historico = 0 ")
                iSQL.Append("         AND i.flag_padrao_patio = 1 ")
                iSQL.Append("         AND m.volumes > 0 ")
                iSQL.Append("         AND m.autonum_cs_yard = 0 ")
                iSQL.Append("    UNION    ")
                iSQL.Append("   SELECT i.descr as DESCR_ARMAZEM, ")
                iSQL.Append("          (  ISNULL (c.quantidade_real, 0) ")
                iSQL.Append("           - ISNULL (c.quantidade_saida, 0) ")
                iSQL.Append("           - ISNULL (vw.quantidade, 0)) ")
                iSQL.Append("             AS QTDE, ")
                iSQL.Append("          e.descr AS EMBALAGEM, ")
                iSQL.Append("          '-' AS POSICAO, ")
                iSQL.Append("          'I' AS SISTEMA, ")
                iSQL.Append("          'C' + LTRIM (CAST(c.autonum as varchar)) AS ID_GRAVACAO, ")
                iSQL.Append("          c.quantidade AS QTDE_CAPTADA, ")
                iSQL.Append("          '000000000000' AS MARCANTE, ")
                iSQL.Append("          CAST(b.autonum as varchar) AS LOTE_STR, ")
                iSQL.Append("          B.NUMERO AS BL, ")
                iSQL.Append("          I.AUTONUM AS AUTONUM_ARMAZEM ")
                iSQL.Append("     FROM sgipa.dbo.tb_carga_solta c ")
                iSQL.Append("          INNER JOIN sgipa.dbo.tb_bl b ")
                iSQL.Append("             ON c.bl = b.autonum ")
                iSQL.Append("          INNER JOIN sgipa.dbo.tb_armazens_ipa i ")
                iSQL.Append("             ON c.patio = i.patio ")
                iSQL.Append("          LEFT JOIN operador.dbo.vw_carga_solta_pos vw ")
                iSQL.Append("             ON c.autonum = vw.autonum_cs ")
                iSQL.Append("          LEFT JOIN operador.dbo.vw_carga_solta_mc vwmc ")
                iSQL.Append("             ON c.autonum = vwmc.autonum_cs ")
                iSQL.Append("          LEFT JOIN sgipa.dbo.dte_tb_embalagens e ")
                iSQL.Append("             ON c.embalagem = e.code ")
                iSQL.Append("    WHERE      ")
                iSQL.Append("         C.AUTONUM = " & PAutonumCs & " ")
                iSQL.Append("        AND b.flag_ativo = 1 ")
                iSQL.Append("         AND c.flag_terminal = 1 ")
                iSQL.Append("          AND c.flag_historico = 0 ")
                iSQL.Append("          AND i.flag_padrao_patio = 1 ")
                iSQL.Append("          AND (  ISNULL (c.quantidade_real, 0) ")
                iSQL.Append("               - ISNULL (c.quantidade_saida, 0) ")
                iSQL.Append("               - ISNULL (vw.quantidade, 0) ")
                iSQL.Append("               - ISNULL (vwmc.quantidade, 0)) > 0 ) VW WHERE QTDE>0  ")

            End If

            If Mid(Id_Gravacao, 3, 1) = "C" And Mid(Id_Gravacao, 1, 1) = "R" Then

                iSQL.Append("    SELECT  QTDE,EMBALAGEM,(DESCR_ARMAZEM + ' ' + POSICAO) AS LOCAL, POSICAO AS YARD    ")
                iSQL.Append("    FROM (  ")
                iSQL.Append(" SELECT DESCR AS DESCR_ARMAZEM,  ")
                iSQL.Append("          SALDO AS QTDE,  ")
                iSQL.Append("          EMBALAGEM,  ")
                iSQL.Append("          POSICAO,  ")
                iSQL.Append("          SISTEMA,  ")
                iSQL.Append("          ID_GRAVACAO,  ")
                iSQL.Append("          QTDE_CAPTADA,  ")
                iSQL.Append("          MARCANTE,  ")
                iSQL.Append("          LOTE_STR,  ")
                iSQL.Append("          BL,  ")
                iSQL.Append("          AUTONUM_ARMAZEM  ")
                iSQL.Append("     FROM (SELECT /*+ ORDERED */  ")
                iSQL.Append("                 i.descr,  ")
                iSQL.Append("                    A.QTDE_ENTRADA  ")
                iSQL.Append("                  - ISNULL ( (SELECT SUM (qtde_saida)  ")
                iSQL.Append("                             FROM REDEX.dbo.tb_saida_carga w  ")
                iSQL.Append("                            WHERE w.autonum_pcs = a.autonum_pcs),  ")
                iSQL.Append("                         0)  ")
                iSQL.Append("                  - ISNULL ( (SELECT SUM (quantidade)  ")
                iSQL.Append("                             FROM REDEX.dbo.tb_carga_solta_yard kk  ")
                iSQL.Append("                            WHERE kk.autonum_patiocs = a.autonum_pcs),  ")
                iSQL.Append("                         0)  ")
                iSQL.Append("                     AS SALDO,  ")
                iSQL.Append("                  G.DESCRICAO_EMB AS EMBALAGEM,  ")
                iSQL.Append("                  '-' AS posicao,  ")
                iSQL.Append("                  'R' AS SISTEMA,  ")
                iSQL.Append("                  'C' + LTRIM (cast(A.autonum_PCS as varchar)) AS ID_GRAVACAO,  ")
                iSQL.Append("                  A.QTDE_ENTRADA AS QTDE_CAPTADA,  ")
                iSQL.Append("                  '000000000000' AS MARCANTE,  ")
                iSQL.Append("                  C.os AS LOTE_STR,  ")
                iSQL.Append("                  '' AS BL,  ")
                iSQL.Append("                  I.AUTONUM AS AUTONUM_ARMAZEM  ")
                iSQL.Append("             FROM REDEX.dbo.TB_PATIO_CS A  ")
                iSQL.Append("                  INNER JOIN sgipa.dbo.tb_armazens_ipa I  ")
                iSQL.Append("                     ON A.patio = i.patio  ")
                iSQL.Append("                  INNER JOIN REDEX.dbo.TB_BOOKING_CARGA D  ")
                iSQL.Append("                     ON A.AUTONUM_BCG = D.AUTONUM_BCG  ")
                iSQL.Append("                  INNER JOIN REDEX.dbo.TB_BOOKING C  ")
                iSQL.Append("                     ON D.AUTONUM_BOO = C.AUTONUM_BOO  ")
                iSQL.Append("                  LEFT JOIN REDEX.dbo.TB_CAD_PRODUTOS B  ")
                iSQL.Append("                     ON d.AUTONUM_PRO = B.AUTONUM_PRO  ")
                iSQL.Append("                  LEFT JOIN REDEX.dbo.TB_CAD_EMBALAGENS G  ")
                iSQL.Append("                     ON A.AUTONUM_EMB = G.AUTONUM_EMB  ")
                iSQL.Append("                  LEFT JOIN REDEX.dbo.TB_CAD_PARCEIROS H  ")
                iSQL.Append("                     ON C.AUTONUM_PARCEIRO = H.AUTONUM  ")
                iSQL.Append("                  LEFT JOIN REDEX.dbo.TB_booking BM  ")
                iSQL.Append("                     ON C.reserva_master = bm.autonum_boo  ")
                iSQL.Append("                  LEFT JOIN REDEX.dbo.TB_booking BS  ")
                iSQL.Append("                     ON C.reserva_pai = bs.autonum_boo  ")
                iSQL.Append("                  LEFT JOIN (  SELECT MIN (td.descricao) AS DOC_EXP,  ")
                iSQL.Append("                                      sdb.autonum_boo  ")
                iSQL.Append("                                 FROM redex.dbo.tb_cad_sd sd  ")
                iSQL.Append("                                      INNER JOIN redex.dbo.tb_cad_tipo_documento td  ")
                iSQL.Append("                                         ON sd.autonum_tipo_doc = td.autonum  ")
                iSQL.Append("                                      LEFT JOIN redex.dbo.tb_cad_sd_boo sdb  ")
                iSQL.Append("                                         ON sd.autonum_sd = sdb.autonum_sd  ")
                iSQL.Append("                                      LEFT JOIN redex.dbo.tb_booking boo  ")
                iSQL.Append("                                         ON sdb.autonum_boo = boo.autonum_boo  ")
                iSQL.Append("                             GROUP BY sdb.autonum_boo) SDBOO  ")
                iSQL.Append("                     ON D.AUTONUM_BOO = SDBOO.AUTONUM_BOO  ")
                iSQL.Append("            WHERE       ")
                iSQL.Append("                  A.autonum_PCS= " & PAutonumCs & "  ")
                iSQL.Append("                  AND d.flag_cs = 1  ")
                iSQL.Append("                  AND i.flag_padrao_patio = 1  ")
                iSQL.Append("                  AND ISNULL (a.flag_historico, 0) = 0)Q)Q   ")

            End If

            If Mid(Id_Gravacao, 3, 1) = "Y" And Mid(Id_Gravacao, 1, 1) = "R" Then

                iSQL.Append("    SELECT  QTDE,EMBALAGEM,(DESCR_ARMAZEM + ' ' + POSICAO) AS LOCAL, POSICAO AS YARD    ")
                iSQL.Append("    FROM (     ")
                iSQL.Append("   SELECT DESCR AS DESCR_ARMAZEM,  ")
                iSQL.Append("          QUANTIDADE AS QTDE,  ")
                iSQL.Append("          EMBALAGEM,  ")
                iSQL.Append("          POSICAO,  ")
                iSQL.Append("          SISTEMA,  ")
                iSQL.Append("          ID_GRAVACAO,  ")
                iSQL.Append("          QTDE_CAPTADA,  ")
                iSQL.Append("          MARCANTE,  ")
                iSQL.Append("          LOTE_STR,  ")
                iSQL.Append("          AUTONUM_ARMAZEM  ")
                iSQL.Append("     FROM (SELECT i.descr,  ")
                iSQL.Append("                  Y.QUANTIDADE,  ")
                iSQL.Append("                    A.QTDE_ENTRADA  ")
                iSQL.Append("                  - ISNULL ( (SELECT SUM (qtde_saida)  ")
                iSQL.Append("                             FROM REDEX.dbo.tb_saida_carga w  ")
                iSQL.Append("                            WHERE w.autonum_pcs = a.autonum_pcs),  ")
                iSQL.Append("                         0)  ")
                iSQL.Append("                     AS SALDO,  ")
                iSQL.Append("                  G.DESCRICAO_EMB AS EMBALAGEM,  ")
                iSQL.Append("                  Y.YARD AS posicao,  ")
                iSQL.Append("                  'R' AS SISTEMA,  ")
                iSQL.Append("                  'Y' + LTRIM (CAST(Y.autonum as varchar)) AS ID_GRAVACAO,  ")
                iSQL.Append("                  A.QTDE_ENTRADA AS QTDE_CAPTADA,  ")
                iSQL.Append("                  '000000000000' AS MARCANTE,  ")
                iSQL.Append("                  C.os AS LOTE_STR,  ")
                iSQL.Append("                  I.AUTONUM AS AUTONUM_ARMAZEM  ")
                iSQL.Append("             FROM REDEX.dbo.TB_PATIO_CS A  ")
                iSQL.Append("                  INNER JOIN REDEX.dbo.TB_BOOKING_CARGA D  ")
                iSQL.Append("                     ON A.AUTONUM_BCG = D.AUTONUM_BCG  ")
                iSQL.Append("                  INNER JOIN REDEX.dbo.TB_BOOKING C  ")
                iSQL.Append("                     ON D.AUTONUM_BOO = C.AUTONUM_BOO  ")
                iSQL.Append("                  INNER JOIN REDEX.dbo.TB_CARGA_SOLTA_YARD Y  ")
                iSQL.Append("                     ON A.AUTONUM_PCS = Y.AUTONUM_PATIOCS  ")
                iSQL.Append("                  INNER JOIN sgipa.dbo.tb_armazens_ipa I  ")
                iSQL.Append("                     ON Y.ARMAZEM = i.AUTONUM  ")
                iSQL.Append("                  LEFT JOIN REDEX.dbo.TB_CAD_PRODUTOS B  ")
                iSQL.Append("                     ON d.AUTONUM_PRO = B.AUTONUM_PRO  ")
                iSQL.Append("                  LEFT JOIN REDEX.dbo.TB_CAD_EMBALAGENS G  ")
                iSQL.Append("                     ON A.AUTONUM_EMB = G.AUTONUM_EMB  ")
                iSQL.Append("                  LEFT JOIN REDEX.dbo.TB_CAD_PARCEIROS H  ")
                iSQL.Append("                     ON C.AUTONUM_PARCEIRO = H.AUTONUM  ")
                iSQL.Append("                  LEFT JOIN REDEX.dbo.TB_booking BM  ")
                iSQL.Append("                     ON C.reserva_master = bm.autonum_boo  ")
                iSQL.Append("                  LEFT JOIN REDEX.dbo.TB_booking BS  ")
                iSQL.Append("                     ON C.reserva_pai = bs.autonum_boo  ")
                iSQL.Append("                  LEFT JOIN (  SELECT MIN (td.descricao) AS DOC_EXP,  ")
                iSQL.Append("                                      sdb.autonum_boo  ")
                iSQL.Append("                                 FROM redex.dbo.tb_cad_sd sd  ")
                iSQL.Append("                                      INNER JOIN redex.dbo.tb_cad_tipo_documento td  ")
                iSQL.Append("                                         ON sd.autonum_tipo_doc = td.autonum  ")
                iSQL.Append("                                      LEFT JOIN redex.dbo.tb_cad_sd_boo sdb  ")
                iSQL.Append("                                         ON sd.autonum_sd = sdb.autonum_sd  ")
                iSQL.Append("                                      LEFT JOIN redex.dbo.tb_booking boo  ")
                iSQL.Append("                                         ON sdb.autonum_boo = boo.autonum_boo  ")
                iSQL.Append("                             GROUP BY sdb.autonum_boo) SDBOO  ")
                iSQL.Append("                     ON D.AUTONUM_BOO = SDBOO.AUTONUM_BOO  ")
                iSQL.Append("            WHERE Y.autonum = " & PAutonumCs & "  AND d.flag_cs = 1 AND ISNULL (a.flag_historico, 0) = 0)Q  ")
                iSQL.Append("    )Q  ")

            End If

            If iSQL.ToString() <> String.Empty Then

                rsCs = Banco.Consultar(iSQL.ToString())

                If rsCs IsNot Nothing Then
                    If rsCs.Rows.Count > 0 Then

                        Dim OrdensOBJ As New InventarioCS_f2

                        OrdensOBJ.Quantidade = rsCs.Rows(0)("QTDE").ToString
                        OrdensOBJ.Embalagem = rsCs.Rows(0)("EMBALAGEM").ToString
                        OrdensOBJ.Local = rsCs.Rows(0)("LOCAL").ToString
                        OrdensOBJ.Yard = rsCs.Rows(0)("YARD").ToString
                        Return OrdensOBJ

                    End If
                End If
            End If

        Else

            Sql = "SELECT "
            Sql = Sql & " SUM(A.QTDE) AS QTDE,"
            Sql = Sql & " MIN(A.EMBALAGEM) AS EMBALAGEM ,"
            Sql = Sql & " (  MIN(A.DESCR_ARMAZEM) + ' ' + MIN(A.POSICAO)) AS LOCAL, "
            Sql = Sql & " MIN(A.POSICAO) AS YARD"
            Sql = Sql & " FROM " & Banco.BancoOperador & "VW_INVENT_ARMAZEM_ITEM A INNER JOIN " & Banco.BancoOperador & "VW_INVENT_ARMAZEM_ITEM B   "
            Sql = Sql & " ON A.EMBALAGEM=B.EMBALAGEM AND A.POSICAO=B.POSICAO AND A.LOTE_STR=B.LOTE_STR AND A.MARCANTE=B.MARCANTE WHERE A.ID_GRAVACAO='{0}' AND A.QTDE=1 "
            Sql = Sql & " GROUP BY A.LOTE_STR,A.EMBALAGEM,A.DESCR_ARMAZEM,A.POSICAO, A.MARCANTE "

            SqlF = String.Format(Sql, Id_Gravacao.Replace("IU", "").Replace("RU", "").Replace("RV", ""))

            Dim dtInventario = Banco.Consultar(SqlF)

            If dtInventario IsNot Nothing Then
                If dtInventario.Rows.Count > 0 Then
                    Dim OrdensOBJ2 As New InventarioCS_f2
                    OrdensOBJ2.Quantidade = dtInventario.Rows(0)("QTDE").ToString
                    OrdensOBJ2.Embalagem = dtInventario.Rows(0)("EMBALAGEM").ToString
                    OrdensOBJ2.Local = dtInventario.Rows(0)("LOCAL").ToString
                    OrdensOBJ2.Yard = dtInventario.Rows(0)("YARD").ToString
                    Return OrdensOBJ2
                End If
            End If
        End If

        Return Nothing

    End Function

    Public Shared Function ConsultarItensLote(ByVal QualLote As String, FlagRedex As Boolean, QualPatio As Integer) As DataTable

        Dim rsCs As New DataTable
        Dim SQL As New StringBuilder

        SQL.Append("SELECT ")
        SQL.Append("'IU' + VW.ID_GRAVACAO as AUTONUM,  ")
        SQL.Append("(VW.QTDE +  '/' + VW.QTDE_CAPTADA + ' ' + VW.EMBALAGEM + ' ' + VW.DESCR_ARMAZEM + ' ' + VW.POSICAO  ) AS DISPLAY   ")
        SQL.Append("  FROM (   ")
        SQL.Append("    SELECT a.descr as DESCR_ARMAZEM, ")
        SQL.Append("          CAST(ISNULL (m.volumes, 0) AS VARCHAR) AS QTDE, ")
        SQL.Append("          e.descr AS EMBALAGEM, ")
        SQL.Append("          y.yard AS POSICAO, ")
        SQL.Append("          'I' AS SISTEMA, ")
        SQL.Append("          'Y' + LTRIM (CAST(y.autonum AS VARCHAR)) AS ID_GRAVACAO, ")
        SQL.Append("          c.quantidade AS QTDE_CAPTADA, ")
        SQL.Append("          m.autonum AS MARCANTE, ")
        SQL.Append("          CAST(b.autonum AS VARCHAR) AS LOTE_STR, ")
        SQL.Append("          B.NUMERO AS BL, ")
        SQL.Append("          A.AUTONUM AS AUTONUM_ARMAZEM ")
        SQL.Append("     FROM sgipa.dbo.tb_carga_solta c ")
        SQL.Append("          INNER JOIN sgipa.dbo.tb_bl b ")
        SQL.Append("             ON c.bl = b.autonum ")
        SQL.Append("          INNER JOIN sgipa.dbo.tb_carga_solta_yard y ")
        SQL.Append("             ON c.autonum = y.autonum_cs ")
        SQL.Append("          INNER JOIN sgipa.dbo.tb_marcantes m ")
        SQL.Append("             ON c.autonum = m.autonum_carga AND y.autonum = m.autonum_cs_yard ")
        SQL.Append("          INNER JOIN sgipa.dbo.tb_armazens_ipa a ")
        SQL.Append("             ON y.armazem = a.autonum ")
        SQL.Append("          LEFT JOIN sgipa.dbo.dte_tb_embalagens e ")
        SQL.Append("             ON c.embalagem = e.code ")
        SQL.Append("    WHERE      ")
        SQL.Append("            B.AUTONUM = " & QualLote & " ")
        SQL.Append("            AND B.PATIO = " & QualPatio & " ")
        SQL.Append("            AND c.flag_terminal = 1 ")
        SQL.Append("          AND b.flag_ativo = 1 ")
        SQL.Append("          AND c.flag_historico = 0 ")
        SQL.Append("          AND m.volumes > 0           ")
        SQL.Append("   UNION ")
        SQL.Append("   SELECT i.descr as DESCR_ARMAZEM, ")
        SQL.Append("          cast(ISNULL (m.volumes, 0) AS VARCHAR) AS QTDE, ")
        SQL.Append("          e.descr AS EMBALAGEM, ")
        SQL.Append("          '-' AS POSICAO, ")
        SQL.Append("          'I' AS SISTEMA, ")
        SQL.Append("          'C' + LTRIM (CAST(c.autonum AS VARCHAR)) AS ID_GRAVACAO, ")
        SQL.Append("          c.quantidade AS QTDE_CAPTADA, ")
        SQL.Append("          m.autonum AS MARCANTE, ")
        SQL.Append("          CAST(b.autonum as varchar) AS LOTE_STR, ")
        SQL.Append("          B.NUMERO AS BL , ")
        SQL.Append("          I.AUTONUM AS AUTONUM_ARMAZEM ")
        SQL.Append("     FROM sgipa.dbo.tb_carga_solta c ")
        SQL.Append("          INNER JOIN sgipa.dbo.tb_bl b ")
        SQL.Append("             ON c.bl = b.autonum ")
        SQL.Append("          INNER JOIN sgipa.dbo.tb_marcantes m ")
        SQL.Append("             ON c.autonum = m.autonum_carga ")
        SQL.Append("          INNER JOIN sgipa.dbo.tb_armazens_ipa i ")
        SQL.Append("             ON c.patio = i.patio ")
        SQL.Append("          LEFT JOIN sgipa.dbo.dte_tb_embalagens e ")
        SQL.Append("             ON c.embalagem = e.code ")
        SQL.Append("    WHERE  ")
        SQL.Append("         B.AUTONUM = " & QualLote & " ")
        SQL.Append("         AND B.PATIO = " & QualPatio & " ")
        SQL.Append("         AND b.flag_ativo = 1 ")
        SQL.Append("         AND c.flag_terminal = 1 ")
        SQL.Append("         AND c.flag_historico = 0 ")
        SQL.Append("         AND i.flag_padrao_patio = 1 ")
        SQL.Append("         AND m.volumes > 0 ")
        SQL.Append("         AND m.autonum_cs_yard = 0  ")
        SQL.Append("   UNION ")
        SQL.Append("   SELECT i.descr as DESCR_ARMAZEM, ")
        SQL.Append("         CAST( y.quantidade - ISNULL (M.VOLUMES, 0) AS VARCHAR) AS QTDE, ")
        SQL.Append("          e.descr AS EMBALAGEM, ")
        SQL.Append("          y.yard AS POSICAO, ")
        SQL.Append("          'I' AS SISTEMA, ")
        SQL.Append("          'Y' + LTRIM (CAST(y.autonum AS VARCHAR)) AS ID_GRAVACAO, ")
        SQL.Append("          c.quantidade AS QTDE_CAPTADA, ")
        SQL.Append("          '000000000000' AS MARCANTE, ")
        SQL.Append("          cast(b.autonum as varchar) AS LOTE_STR, ")
        SQL.Append("          B.NUMERO AS BL, ")
        SQL.Append("          I.AUTONUM AS AUTONUM_ARMAZEM ")
        SQL.Append("     FROM sgipa.dbo.tb_carga_solta c ")
        SQL.Append("          INNER JOIN sgipa.dbo.tb_bl b ")
        SQL.Append("             ON c.bl = b.autonum ")
        SQL.Append("          INNER JOIN sgipa.dbo.tb_carga_solta_yard y ")
        SQL.Append("             ON c.autonum = y.autonum_cs ")
        SQL.Append("          INNER JOIN sgipa.dbo.tb_armazens_ipa i ")
        SQL.Append("             ON y.armazem = i.autonum ")
        SQL.Append("          LEFT JOIN operador.dbo.vw_carga_solta_mc mc ")
        SQL.Append("             ON c.autonum = mc.autonum_cs ")
        SQL.Append("          LEFT JOIN sgipa.dbo.tb_marcantes m ")
        SQL.Append("             ON y.autonum = m.autonum_cs_yard ")
        SQL.Append("          LEFT JOIN sgipa.dbo.dte_tb_embalagens e ")
        SQL.Append("             ON c.embalagem = e.code ")
        SQL.Append("    WHERE      ")
        SQL.Append("         B.AUTONUM = " & QualLote & " ")
        SQL.Append("         AND B.PATIO = " & QualPatio & " ")
        SQL.Append("         AND b.flag_ativo = 1 ")
        SQL.Append("         AND c.flag_terminal = 1 ")
        SQL.Append("          AND c.flag_historico = 0 ")
        SQL.Append("          AND y.quantidade > 0 ")
        SQL.Append("          AND (  ISNULL (c.quantidade_real, 0) ")
        SQL.Append("               - ISNULL (c.quantidade_saida, 0) ")
        SQL.Append("               - ISNULL (mc.quantidade, 0)) > 0 ")
        SQL.Append("          AND (m.autonum IS NULL OR ISNULL (M.VOLUMES, 0) <> Y.QUANTIDADE) ")
        SQL.Append("   UNION ")
        SQL.Append("   SELECT i.descr as DESCR_ARMAZEM, ")
        SQL.Append("          CAST(  ISNULL (c.quantidade_real, 0) ")
        SQL.Append("           - ISNULL (c.quantidade_saida, 0) ")
        SQL.Append("           - ISNULL (vw.quantidade, 0) AS VARCHAR) ")
        SQL.Append("             AS QTDE, ")
        SQL.Append("          e.descr AS EMBALAGEM, ")
        SQL.Append("          '-' AS POSICAO, ")
        SQL.Append("          'I' AS SISTEMA, ")
        SQL.Append("          'C' + LTRIM (CAST(c.autonum AS VARCHAR)) AS ID_GRAVACAO, ")
        SQL.Append("          c.quantidade AS QTDE_CAPTADA, ")
        SQL.Append("          '000000000000' AS MARCANTE, ")
        SQL.Append("          CAST(b.autonum AS VARCHAR) AS LOTE_STR, ")
        SQL.Append("          B.NUMERO AS BL, ")
        SQL.Append("          I.AUTONUM AS AUTONUM_ARMAZEM ")
        SQL.Append("     FROM sgipa.dbo.tb_carga_solta c ")
        SQL.Append("          INNER JOIN sgipa.dbo.tb_bl b ")
        SQL.Append("             ON c.bl = b.autonum ")
        SQL.Append("          INNER JOIN sgipa.dbo.tb_armazens_ipa i ")
        SQL.Append("             ON c.patio = i.patio ")
        SQL.Append("          LEFT JOIN operador.dbo.vw_carga_solta_pos vw ")
        SQL.Append("             ON c.autonum = vw.autonum_cs ")
        SQL.Append("          LEFT JOIN operador.dbo.vw_carga_solta_mc vwmc ")
        SQL.Append("             ON c.autonum = vwmc.autonum_cs ")
        SQL.Append("          LEFT JOIN sgipa.dbo.dte_tb_embalagens e ")
        SQL.Append("             ON c.embalagem = e.code ")
        SQL.Append("    WHERE      ")
        SQL.Append("         B.AUTONUM = " & QualLote & " ")
        SQL.Append("         AND B.PATIO = " & QualPatio & " ")
        SQL.Append("         AND b.flag_ativo = 1 ")
        SQL.Append("         AND c.flag_terminal = 1 ")
        SQL.Append("          AND c.flag_historico = 0 ")
        SQL.Append("          AND i.flag_padrao_patio = 1 ")
        SQL.Append("          AND (  ISNULL (c.quantidade_real, 0) ")
        SQL.Append("               - ISNULL (c.quantidade_saida, 0) ")
        SQL.Append("               - ISNULL (vw.quantidade, 0) ")
        SQL.Append("               - ISNULL (vwmc.quantidade, 0)) > 0 ) VW WHERE QTDE>0 ")


        Return Banco.Consultar(SQL.ToString())

    End Function

    Public Shared Function ConsultarArmazens(ByVal QualPatio As String) As DataTable


        Dim Sql As String

        Sql = " select Autonum as autonum, Descr AS DISPLAY "
        Sql = Sql & " FROM " & Banco.BancoSgipa & "tb_armazens_ipa "
        Sql = Sql & " WHERE dt_saida is null and flag_historico=0 and "
        Sql = Sql & " patio=" & QualPatio
        Sql = Sql & " ORDER BY DESCR "


        Return Banco.Consultar(Sql)

    End Function

    Public Shared Function ConsultarYard(ByVal QualArmazem As String) As DataTable


        Dim Sql As String

        Sql = " select YARD as autonum, YARD AS DISPLAY "
        Sql = Sql & " FROM " & Banco.BancoSgipa & "tb_YARD_CS "
        Sql = Sql & " WHERE "
        Sql = Sql & " ARMAZEM=" & QualArmazem
        Sql = Sql & " ORDER BY YARD  "


        Return Banco.Consultar(Sql)

    End Function

    Public Shared Function ConsultarMotivo() As DataTable


        Dim Sql As String

        Sql = " select Autonum as autonum, Descricao AS DISPLAY "
        Sql = Sql & " FROM " & Banco.BancoOperador & "tb_cad_motivo where ISNULL(flag_coletor,0) = 1"
        Sql = Sql & " ORDER BY descricao  "


        Return Banco.Consultar(Sql)

    End Function
End Class
