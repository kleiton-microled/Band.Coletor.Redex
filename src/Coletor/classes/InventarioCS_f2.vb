Imports System.Data.OleDb
Public Class InventarioCS_f2
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
    Public _Yard As String
    Public _Patio As String
    Public _Nvocc As String
    Public _Bl As String
    Public _Lote As String
    Public _Sistema As String
    Public _ANVISA As String
    Public _FlagAnvisa As String

    Public Property Nvocc() As String
        Get
            Return Me._Nvocc
        End Get
        Set(ByVal value As String)
            Me._Nvocc = value
        End Set
    End Property

    Public Property ANVISA() As String
        Get
            Return Me._ANVISA
        End Get
        Set(ByVal value As String)
            Me._ANVISA = value
        End Set
    End Property
    Public Property FlagAnvisa() As String
        Get
            Return Me._FlagAnvisa
        End Get
        Set(ByVal value As String)
            Me._FlagAnvisa = value
        End Set
    End Property

    Public Property Sistema() As String
        Get
            Return Me._Sistema
        End Get
        Set(ByVal value As String)
            Me._Sistema = value
        End Set
    End Property

    Public Property Yard() As String
        Get
            Return Me._Yard
        End Get
        Set(ByVal value As String)
            Me._Yard = value
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
    Public Property Lote() As String
        Get
            Return Me._Lote
        End Get
        Set(ByVal value As String)
            Me._Lote = value
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


    Public Shared Function PopulaDados(QualMarcante As String, QualPatio As Integer) As InventarioCS_f2

        Dim Rst As New DataTable
        Dim Rst2 As New DataTable
        Dim Sql As String
        Dim SqlF As String


        If BD.BancoEmUso = "ORACLE" Then


            Sql = "SELECT BL FROM " & Banco.BancoSgipa & "TB_MARCANTES WHERE AUTONUM=" & Val(QualMarcante)
            Rst2 = Banco.Consultar(Sql)

            If Rst2 IsNot Nothing Then
                If Rst2.Rows.Count > 0 Then

                    Sql = "SELECT "
                    Sql = Sql & " MERCADORIA,"
                    Sql = Sql & " MARCA,"
                    Sql = Sql & " CONVERT(VARCHAR,DATA_ENTRADA,103) AS DATA_ENTRADA,"
                    Sql = Sql & " IMPORTADOR,"
                    Sql = Sql & " CNTR_DESOVA, "
                    Sql = Sql & " ID_PATIO,"
                    Sql = Sql & " TIPO_DOC,"
                    Sql = Sql & " DECODE(CANAL_ALF,0,'AMARELO',1,'VERMELHO',2,'VERDE',3,'CINZA',9,'') AS CANAL_ALF,"
                    Sql = Sql & " MOTIVO_PROX_MVTO AS MOTIVO_PROX_MVTO ,"
                    Sql = Sql & " VOLUME,"
                    Sql = Sql & " IMO,"
                    Sql = Sql & " NVOCC,"
                    Sql = Sql & " BL,"
                    Sql = Sql & " LOTE_STR AS LOTE,"
                    Sql = Sql & " FINALITY, ANVISA, FLAG_ANVISA "
                    Sql = Sql & " FROM " & Banco.BancoOperador & "VW_INVENT_ARMAZEM_COL_P" & QualPatio
                    Sql = Sql & " WHERE LOTE_STR='" & Rst2.Rows(0)("BL").ToString & "'"


                    SqlF = String.Format(Sql, QualMarcante)

                    Rst = Banco.Consultar(SqlF)

                Else
                    Return Nothing
                End If
            Else
                Return Nothing
            End If

        Else

                Return Nothing

        End If

        If Rst2 IsNot Nothing Then
            If Rst2.Rows.Count > 0 Then

                Dim OrdensOBJ As New InventarioCS_f2

                OrdensOBJ.Mercadoria = Rst.Rows(0)("MERCADORIA").ToString
                OrdensOBJ.Marca = Rst.Rows(0)("MARCA").ToString
                OrdensOBJ.Entrada = Rst.Rows(0)("DATA_ENTRADA").ToString
                OrdensOBJ.Cliente = Rst.Rows(0)("IMPORTADOR").ToString
                OrdensOBJ.Conteiner = Rst.Rows(0)("CNTR_DESOVA").ToString
                OrdensOBJ.Patio = Rst.Rows(0)("ID_PATIO").ToString
                OrdensOBJ.Volume = Rst.Rows(0)("VOLUME").ToString
                OrdensOBJ.Doc = Rst.Rows(0)("TIPO_DOC").ToString
                OrdensOBJ.Canal = Rst.Rows(0)("CANAL_ALF").ToString
                OrdensOBJ.Movimento = Rst.Rows(0)("MOTIVO_PROX_MVTO").ToString
                OrdensOBJ.IMO = Rst.Rows(0)("IMO").ToString
                OrdensOBJ.Nvocc = Rst.Rows(0)("NVOCC").ToString
                OrdensOBJ.BL = Rst.Rows(0)("BL").ToString
                OrdensOBJ.Lote = Rst.Rows(0)("Lote").ToString
                OrdensOBJ.Sistema = Rst.Rows(0)("FINALITY").ToString
                OrdensOBJ.ANVISA = Rst.Rows(0)("ANVISA").ToString
                OrdensOBJ.FlagAnvisa = Rst.Rows(0)("FLAG_ANVISA").ToString
                Return OrdensOBJ

            End If
        End If

        Return Nothing



    End Function

    Public Shared Function PopulaItem(QualMarcante As String) As InventarioCS_f2

        Dim Rst As New DataTable
        Dim Sql As String
        Dim SqlF As String



        If BD.BancoEmUso = "ORACLE" Then

            Sql = "SELECT "
            Sql = Sql & " QTDE,"
            Sql = Sql & " EMBALAGEM,"
            Sql = Sql & " (DESCR_ARMAZEM + ' ' + POSICAO) AS LOCAL, "
            Sql = Sql & " POSICAO AS YARD "
            Sql = Sql & " "
            Sql = Sql & " FROM " & Banco.BancoOperador & "VW_INVENT_ARMAZEM_ITEM"
            Sql = Sql & " WHERE MARCANTE='{0}'"


            SqlF = String.Format(Sql, QualMarcante)

            Rst = Banco.Consultar(SqlF)

        Else

            Sql = ""

        End If

        If Rst IsNot Nothing Then
            If Rst.Rows.Count > 0 Then
                Dim OrdensOBJ As New InventarioCS_f2

                OrdensOBJ.Quantidade = Rst.Rows(0)("QTDE").ToString
                OrdensOBJ.Embalagem = Rst.Rows(0)("EMBALAGEM").ToString
                OrdensOBJ.Local = Rst.Rows(0)("LOCAL").ToString
                OrdensOBJ.Yard = Rst.Rows(0)("YARD").ToString
                Return OrdensOBJ

            End If
        End If

        Return Nothing



    End Function
    Public Shared Function ConsultarItensLote(ByVal QualLote As String, QualMarcante As String) As DataTable

        Dim Sql As String

        Sql = " select ID_GRAVACAO as autonum, (QTDE +  '/' + QTDE_CAPTADA + ' ' + EMBALAGEM + ' ' + DESCR_ARMAZEM + ' ' + POSICAO  ) AS DISPLAY "
        Sql = Sql & " FROM " & Banco.BancoOperador & "VW_INVENT_ARMAZEM_ITEM "
        Sql = Sql & " WHERE "
        Sql = Sql & " LOTE_STR='" & QualLote & "'"
        Sql = Sql & " AND MARCANTE='" & QualMarcante & "'"
        Sql = Sql & " AND QTDE>0 ORDER BY DESCR_ARMAZEM,POSICAO "

        Return Banco.Consultar(Sql)


    End Function

    Public Shared Function ConsultarArmazens(ByVal QualPatio As String) As DataTable

        Dim Rst As New DataTable

        Dim Sql As String

        Sql = " select Autonum as autonum, Descr AS DISPLAY "
        Sql = Sql & " FROM " & Banco.BancoSgipa & "tb_armazens_ipa "
        Sql = Sql & " WHERE dt_saida is null and flag_historico=0 "
        If (Not String.IsNullOrEmpty(QualPatio)) Then
            Sql = Sql & "and  patio=" & QualPatio
        End If
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
        Sql = Sql & " FROM " & Banco.BancoOperador & "tb_cad_motivo " ' where ISNULL(flag_coletor,0) = 1"
        Sql = Sql & " ORDER BY descricao  "

        Return Banco.Consultar(Sql)



    End Function
End Class
