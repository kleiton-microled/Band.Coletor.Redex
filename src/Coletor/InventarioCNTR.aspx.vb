Imports System.Data.OleDb
Imports System.Data.SqlClient

Public Class WebFormICNTR
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session("LOGADO") Is Nothing Or Session("USUARIO") Is Nothing Or Session("SENHA") Is Nothing Or Session("EMPRESA") Is Nothing Or Session("PATIO") Is Nothing Or Session("AUTONUMPATIO") Is Nothing Then
            Try
                Session("LOGADO") = Server.HtmlEncode(Request.Cookies("LOGADO").Value)
                Session("USUARIO") = Server.HtmlEncode(Request.Cookies("USUARIO").Value)
                Session("SENHA") = Server.HtmlEncode(Request.Cookies("SENHA").Value)
                Session("EMPRESA") = Server.HtmlEncode(Request.Cookies("EMPRESA").Value)
                Session("PATIO") = Server.HtmlEncode(Request.Cookies("PATIO").Value)
                Session("AUTONUMPATIO") = Server.HtmlEncode(Request.Cookies("AUTONUMPATIO").Value)
                Session("AUTONUMUSUARIO") = Server.HtmlEncode(Request.Cookies("AUTONUMUSUARIO").Value)
                Session("BROWSER_NAME") = Server.HtmlEncode(Request.Cookies("BROWSER_NAME").Value)
                Session("BROWSER_VERSION") = Server.HtmlEncode(Request.Cookies("BROWSER_VERSION").Value)
                Session("MobileDeviceModel") = Server.HtmlEncode(Request.Cookies("MobileDeviceModel").Value)
                Session("MobileDeviceManufacturer") = Server.HtmlEncode(Request.Cookies("MobileDeviceManufacturer").Value)
                Session("IsMobileDevice") = Server.HtmlEncode(Request.Cookies("IsMobileDevice").Value)
                'Session("FLAG_OB_MARCANTE") = Server.HtmlEncode(Request.Cookies("FLAG_OB_MARCANTE").Value)
                'Session("FLAG_FILMA_DESOVA") = Server.HtmlEncode(Request.Cookies("FLAG_FILMA_DESOVA").Value)
                Session("FLAG_REDEX_SEM_MARCANTE") = Server.HtmlEncode(Request.Cookies("FLAG_REDEX_SEM_MARCANTE").Value)
                Session("LARGURATELA") = Server.HtmlEncode(Request.Cookies("LARGURATELA").Value)
            Catch ex As Exception
                Response.Redirect("Index.aspx")
            End Try
        End If

        If Not Page.IsPostBack Then
            Limpa()

            CarregarMotivos()
            CarregarEmpilhadeiras()
            CarregarVeiculos()


            Dim Sql As String
            Sql = $"SELECT ISNULL(Flag_Truck_Mov_Coletor,0) as Qual FROM {Banco.BancoOperador}TB_PATIOS {IIf(Not String.IsNullOrEmpty(Session("AUTONUMPATIO")), "WHERE AUTONUM = '" + Session("AUTONUMPATIO") + "'", "")}"

            Session("FLAG_TRUCK_MOV_COLETOR") = Banco.ExecuteScalar(Sql)

        End If

    End Sub


    Private Sub CarregarMotivos()

        Dim Sql As String

        Sql = "SELECT AUTONUM,DESCRICAO FROM " & BD.BancoOperador & "TB_CAD_MOTIVO ORDER BY DESCRICAO"

        Me.cbMotivoPos.DataSource = BD.List(Sql)
        Me.cbMotivoPos.DataTextField = "DESCRICAO"
        Me.cbMotivoPos.DataValueField = "AUTONUM"
        Me.cbMotivoPos.DataBind()   

        Me.cbMotivoPos.Items.Insert(0, "--Motivo--")
        Me.cbMotivoPos.SelectedIndex = 0

    End Sub

    Private Sub CarregarEmpilhadeiras()


        Dim Sql As String

        Sql = " SELECT AUTONUM AS AUTONUM,IDENTIFICACAO FROM " & BD.BancoOperador & "TB_FROTA WHERE " + IIf(Not String.IsNullOrEmpty(Session("AUTONUMPATIO")), "PATIO=" & Session("AUTONUMPATIO") & " AND ", "") + " TIPO_VEICULO='E' AND FLAG_ATIVO=1 ORDER BY IDENTIFICACAO"

        Me.cbEmpilhadeira.DataSource = BD.List(Sql)
        Me.cbEmpilhadeira.DataTextField = "IDENTIFICACAO"
        Me.cbEmpilhadeira.DataValueField = "AUTONUM"
        Me.cbEmpilhadeira.DataBind()


        Me.cbEmpilhadeira.Items.Insert(0, "--Empilhadeira--")
        Me.cbEmpilhadeira.SelectedIndex = 0



    End Sub

    Private Sub CarregarVeiculos()

        Dim Sql As String

        Sql = " SELECT AUTONUM AS AUTONUM,IDENTIFICACAO FROM " & BD.BancoOperador & "TB_FROTA WHERE " + IIf(Not String.IsNullOrEmpty(Session("AUTONUMPATIO")), " PATIO=" & Session("AUTONUMPATIO") & " AND ", "") + " TIPO_VEICULO='C' AND FLAG_ATIVO=1 ORDER BY IDENTIFICACAO"

        Me.cbVeiculo.DataSource = BD.List(Sql)
        Me.cbVeiculo.DataTextField = "IDENTIFICACAO"
        Me.cbVeiculo.DataValueField = "AUTONUM"
        Me.cbVeiculo.DataBind()


        Me.cbVeiculo.Items.Insert(0, "--Veiculo--")
        Me.cbVeiculo.SelectedIndex = 0



    End Sub

    Protected Sub btSair_Click(sender As Object, e As EventArgs) Handles btSair.Click

        Dim Controle As Control
        Dim Subcontrol As Control
        For Each Controle In Page.Controls
            For Each Subcontrol In Controle.Controls
                Subcontrol.Dispose()
            Next
            Controle.Dispose()
        Next

        Response.Redirect(Login.ObterURIMenu())
    End Sub

    Sub Limpa()


        Me.txtCATEG.Text = ""
        Me.txtRegime.Text = ""

        Me.txtEF.Text = ""
        Me.txtEntrada.Text = ""
        Me.txtEscala.Text = ""

        Me.txtGWT.Text = ""
        Me.txtIMO.Text = ""


        Me.txtNavio.Text = ""


        Me.txtPosicionamento.Text = ""
        Me.txtTAM.Text = ""
        Me.txtTemp.Text = ""
        Me.txtTiPO.Text = ""

        Me.txtYard.Text = ""
        Me.txtYardAtual.Text = ""
        Me.txtSistema.Text = ""
        Me.txtAutonum.Text = ""
        Me.cbEmpilhadeira.SelectedIndex = -1
        Me.cbMotivoPos.SelectedIndex = -1
        Me.cbVeiculo.SelectedIndex = -1


        Me.txtCNTR4.Focus()



    End Sub

    Protected Sub btFiltra_Click(sender As Object, e As EventArgs) Handles btFiltra.Click

        BuscaCntr()
    End Sub

    Sub BuscaCntr()

        'Dim TbCntr As New DataTable
        'Dim TbQtde As New DataTable
        Dim Sql As String = String.Empty

        If Trim(Me.txtCNTR4.Text) = "" And Trim(Me.txtCNTR.Text) = "" Then
            'ScriptManager.RegisterClientScriptBlock(Me, [GetType](), "script", "<script>alert('Nenhum conteiner informado');</script>", False)
            btSalvar.Focus()
            Exit Sub
        End If


        Dim Q As Long = 0

        If Trim(Me.txtCNTR4.Text) <> "" And Trim(Me.txtCNTR.Text) = "" Then
            If Len(Me.txtCNTR4.Text).ToString.Replace("-", "") <> 4 Then
                ScriptManager.RegisterClientScriptBlock(Me, [GetType](), "script", "<script>alert('Conteiner informado em formato incorreto');</script>", False)
                Exit Sub
            End If

            Sql = "SELECT COUNT(0) as q ,MAX(ID_CONTEINER) AS ID_CONTEINER FROM "
            Sql = Sql & " " & BD.BancoOperador & "VW_INVENT_SISTEMAS_PY "
            Sql = Sql & " WHERE " + IIf(Not String.IsNullOrEmpty(Session("AUTONUMPATIO")), " PATIO=" & Session("AUTONUMPATIO") & " And ", "") + " REPLACE(SUBSTRING(ID_CONTEINER,8,5),'-','')='" & Me.txtCNTR4.Text.Replace("-", "") & "'"
            Dim rsQ As DataTable
            rsQ = BD.List(Sql)

            Q = rsQ.Rows(0)("q")

            If Q = 0 Then
                ScriptManager.RegisterClientScriptBlock(Me, [GetType](), "script", "<script>alert('Nenhum Conteiner encontrado neste patio com este final');</script>", False)
            ElseIf Q > 1 Then
                ScriptManager.RegisterClientScriptBlock(Me, [GetType](), "script", "<script>alert('Mais de um Conteiner encontrado neste patio com este final. Informe a sigla completa');</script>", False)
            ElseIf Q = 1 Then
                Me.txtCNTR.Text = rsQ.Rows(0)("ID_CONTEINER").ToString

                Carrega_Dados_CNTR(Me.txtCNTR.Text)
            End If

            rsQ.Dispose()

        ElseIf Trim(Me.txtCNTR4.Text) = "" And Trim(Me.txtCNTR.Text) <> "" Then

            Dim Rst As DataTable

            Sql = "SELECT COUNT(0) as q ,MAX(ID_CONTEINER) AS ID_CONTEINER FROM " & BD.BancoOperador & "VW_INVENT_SISTEMAS_PY WHERE " + +IIf(Not String.IsNullOrEmpty(Session("AUTONUMPATIO")), " PATIO=" & Session("AUTONUMPATIO") & " AND ", "") + " REPLACE(ID_CONTEINER,'-','')='" & Me.txtCNTR.Text.Replace("-", "").ToUpper & "'"
            Rst = BD.List(Sql)

            If Rst.Rows(0)("q") = 0 Then
                Rst.Dispose()
                ScriptManager.RegisterClientScriptBlock(Me, [GetType](), "script", "<script>alert('Conteiner nao encontrado');</script>", False)
            ElseIf Rst.Rows(0)("q") = 1 Then
                Me.txtCNTR.Text = Rst.Rows(0)("id_conteiner").ToString
                Me.txtCNTR4.Text = ""
                Rst.Dispose()
                Carrega_Dados_CNTR(Me.txtCNTR.Text)
            End If

        End If

    End Sub

    Sub Carrega_Dados_CNTR(ByVal QualCNTR As String)


        Limpa()

        Dim _PosPend As String = String.Empty
        Dim _PosPlan As String = String.Empty
        Dim _SpecialCare As String = String.Empty
        Dim Term_Destino As String = String.Empty


        Dim Flag_Postergado As Integer = 0
        Dim FlagCSI As Integer = 0
        Dim FlagMAPA As Integer = 0
        Dim FlagBloqueio As Integer = 0
        Dim FlagBloqueioManual As Integer = 0
        Dim FlagDestinadoDesova As Integer = 0
        Dim FlagScannear As Integer = 0
        Dim FlagSuspeita As Integer = 0
        Dim FlagBloqueioScanner As Integer = 0
        Dim FlagScannerAtivo As Integer = 0
        Dim Flag_Truck_Mov_Coletor As Integer = 0
        Dim Patio_Coletor As Integer = Session("AUTONUMPATIO")

        Dim Sql As String
        Dim Obs As String = String.Empty

        Sql = "SELECT ISNULL(Flag_Truck_Mov_Coletor,0) as qual FROM " & BD.BancoOperador & "TB_PATIOS WHERE AUTONUM = " & Patio_Coletor
        Dim RsP As New DataTable
        RsP = BD.List(Sql)
        Flag_Truck_Mov_Coletor = RsP.Rows(0)("Qual")

        'Dim dr As OleDbDataReader
        Dim rsCntr As New DataTable
        Sql = "SELECT * FROM " & BD.BancoOperador & "VW_INVENT_SISTEMAS_P1 WHERE ID_CONTEINER='" & QualCNTR & "'"
        rsCntr = BD.List(Sql)

        If rsCntr.Rows.Count > 0 Then

            Me.txtCATEG.Text = rsCntr.Rows(0)("IMPEXP").ToString
            Me.txtRegime.Text = rsCntr.Rows(0)("DESCRICAO").ToString
            Me.txtEF.Text = rsCntr.Rows(0)("EF").ToString
            Me.txtEntrada.Text = rsCntr.Rows(0)("DATA_ENT_TEMP").ToString
            Me.txtEscala.Text = rsCntr.Rows(0)("SCALE").ToString

            Me.txtGWT.Text = rsCntr.Rows(0)("BRUTO").ToString
            Me.txtIMO.Text = rsCntr.Rows(0)("IMO1").ToString

            Me.txtNavio.Text = rsCntr.Rows(0)("NOME").ToString

            Me.txtYardAtual.Text = rsCntr.Rows(0)("YARD").ToString
            Me.txtTAM.Text = rsCntr.Rows(0)("TAMANHO").ToString
            Me.txtTemp.Text = rsCntr.Rows(0)("TEMPERATURE").ToString
            Me.txtTiPO.Text = rsCntr.Rows(0)("TIPOBASICO").ToString
            Me.txtPosicionamento.Text = rsCntr.Rows(0)("DESCR_MOTIVO_POSIC").ToString
            Me.txtCATEG.Text = rsCntr.Rows(0)("FINALITY").ToString

            If rsCntr.Rows(0)("FLAG_OOG").ToString = "Y" Then
                Obs = Obs & " OOG CARGO"
            End If
            If rsCntr.Rows(0)("FLAG_SPC").ToString = "Y" Then
                Obs = Obs & " SPECIAL CARGO"
            End If
            If rsCntr.Rows(0)("DLV_TERM").ToString <> "" Then
                Obs = Obs & " DLV TERM: " & rsCntr.Rows(0)("DLV_TERM").ToString
            End If

            Me.txtAutonum.Text = rsCntr.Rows(0)("AUTONUM").ToString
            Me.txtSistema.Text = rsCntr.Rows(0)("SISTEMA").ToString

        End If

        txtYard.Focus()


    End Sub

    Function Verifica_Regras_Seg_Imo_Delta(Id_Conteiner As String, Yard As String, Patio As Integer) As String

        Dim Retorno As String
        Retorno = ""

        Dim EspacoMin(4) As Single
        Dim Sql As String
        Sql = "Select ISNULL(DIST_IMO_SEGREG1,0) AS DIST_IMO_SEGREG1,ISNULL(DIST_IMO_SEGREG2,0) AS DIST_IMO_SEGREG2,ISNULL(DIST_IMO_SEGREG3,0) AS DIST_IMO_SEGREG3,ISNULL(DIST_IMO_SEGREG4,0) AS DIST_IMO_SEGREG4  from " & BD.BancoSgipa & "dte_tb_parametros "

        Dim tb1 = New DataTable
        tb1 = BD.List(Sql)
        EspacoMin(1) = tb1.Rows(0)("DIST_IMO_SEGREG1")
        EspacoMin(2) = tb1.Rows(0)("DIST_IMO_SEGREG2")
        EspacoMin(3) = tb1.Rows(0)("DIST_IMO_SEGREG3")
        EspacoMin(4) = tb1.Rows(0)("DIST_IMO_SEGREG4")
        tb1.Dispose()

        Dim Mensagem(4) As String
        Mensagem(1) = "Espaço inferior a " & EspacoMin(1) & "m "
        Mensagem(2) = "Espaço inferior a " & EspacoMin(2) & "m "
        Mensagem(3) = "Espaço inferior a " & EspacoMin(3) & "m "
        Mensagem(4) = "Espaço inferior a " & EspacoMin(4) & "m "

        Sql = "SELECT ID_CONTEINER,ISNULL(IMO1,'') as IMO1,ISNULL(IMO2,'') AS IMO2,ISNULL(IMO3,'') AS IMO3,ISNULL(IMO4,'') AS IMO4, "
        Sql = Sql & " ID_CONTEINER_D, YARD_D, ISNULL(IMO1_D,'') AS IMO1_D, ISNULL(IMO2_D,'') AS IMO2_D, ISNULL(IMO3_D,'') AS IMO3_D, ISNULL(IMO4_D,'') AS IMO4_D, ISNULL(DIST_DELTA,0) as DIST_DELTA "
        Sql = Sql & " FROM " & BD.BancoOperador & "VW_DIST_IMO "
        Sql = Sql & " WHERE PATIO=" & Patio & " AND ID_CONTEINER='" & Id_Conteiner & "' ORDER BY DIST_DELTA"

        tb1 = BD.List(Sql)
        Dim II As Integer

        If tb1.Rows.Count > 0 Then
            For II = 0 To tb1.Rows.Count - 1
                For i = 1 To 4
                    For j = 1 To 4
                        If Trim(tb1.Rows(II)("IMO" & i).ToString) <> "" And Trim(tb1.Rows(II)("IMO" & j & "_D").ToString) <> "" Then
                            If Trim(tb1.Rows(II)("IMO" & i).ToString) <> "0" And Trim(tb1.Rows(II)("IMO" & j & "_D").ToString) <> "0" Then
                                Sql = "Select SEGREGACAO from " & BD.BancoOperador & "tb_segregacao where CLASS1='" & Replace(Trim((tb1.Rows(II)("IMO" & i).ToString)), ".", "") & "'"
                                Sql = Sql & " AND CLASS2='" & Replace(Trim(tb1.Rows(II)("IMO" & j & "_D").ToString), ".", "") & "'"
                                Sql = Sql & " AND SEGREGACAO IN ('1','2','3','4') "
                                Sql = Sql & " UNION "
                                Sql = Sql & " Select SEGREGACAO from " & BD.BancoOperador & "tb_segregacao where CLASS2='" & Replace(Trim(tb1.Rows(II)("IMO" & i).ToString), ".", "") & "'"
                                Sql = Sql & " AND CLASS1='" & Replace(Trim(tb1.Rows(II)("IMO" & j & "_D").ToString), ".", "") & "'"
                                Sql = Sql & " AND SEGREGACAO IN ('1','2','3','4') "

                                Dim tbBusca = New DataTable
                                tbBusca = BD.List(Sql)
                                If tbBusca.Rows.Count > 0 Then
                                    If tb1.Rows(II)("dist_delta") < EspacoMin(Val(tbBusca.Rows(0)("SEGREGACAO"))) Then
                                        Retorno = Retorno & Mensagem(Val(tbBusca.Rows(0)("SEGREGACAO"))) & " Conteiner: " & tb1.Rows(II)("ID_CONTEINER_D").ToString & " Posicao: " & tb1.Rows(II)("Yard_D").ToString & " Imo:" & Trim(tb1.Rows(II)("IMO" & j & "_D").ToString)
                                        Return Retorno
                                    End If
                                End If
                            End If
                        End If
                    Next j
                Next i

            Next II
        End If

        Return Retorno
    End Function

    Function Valida_Aloca_Imo(Id_Conteiner As String, Yard As String, Tamanho As Integer, Patio As Integer) As String
        Dim Retorno As String
        Retorno = ""

        Dim Valida As Boolean
        Valida = False

        Dim Sql As String
        Sql = "SELECT ISNULL(valida,0) AS VALIDA FROM " & BD.BancoOperador & "TB_YARD WHERE PATIO=" & Patio & " AND YARD='" & Yard & "'"
        If BD.ExecuteScalar(Sql) = 0 Then
            Valida = False
        Else
            Valida = True
        End If

        Dim tb1 As DataTable

        Sql = "SELECT ID_CONTEINER,ISNULL(IMO1,'') AS IMO1,ISNULL(IMO2,'') AS IMO2, ISNULL(IMO3,'') AS IMO3,ISNULL(IMO4,'') AS IMO4 FROM " & BD.BancoOperador & "VW_INVENT_SISTEMAS_IMO_XYZ "
        Sql = Sql & " WHERE ID_CONTEINER='" & Id_Conteiner & "'"

        tb1 = BD.List(Sql)
        If tb1.Rows.Count > 0 Then
            'Verifica se pode pelas regras de IMO ou nao
            If Valida Then
                'A QUADRA TODA
                Sql = " SELECT YARD FROM INTELOPER..TB_IP_ATRIBUTO_YARD"
                Sql = Sql & " WHERE PATIO=" & Patio & " AND AUTONUM_ATR=21 AND VLR_ATRIB=1 AND STATUS_ATRIB='R'"
                Sql = Sql & " AND DT_INI_ATR < GETDATE() AND (DT_FIM_ATR IS NULL OR DT_FIM_ATR> GETDATE() )"
                Sql = Sql & " AND (LEN(YARD)=1 OR LEN(YARD)=2) "
                Sql = Sql & " AND (SUBSTRING(YARD,1,1)='" & Left(Yard, 1) & "' OR SUBSTRING(YARD,1,2)='" & Left(Yard, 2) & "')"
                Sql = Sql & " Union"
                Sql = Sql & " SELECT YARD FROM INTELOPER..TB_IP_ATRIBUTO_YARD"
                Sql = Sql & " WHERE PATIO = " & Patio & " AND AUTONUM_ATR=21 AND VLR_ATRIB=0 AND STATUS_ATRIB='E'"
                Sql = Sql & " AND DT_INI_ATR < GETDATE() AND (DT_FIM_ATR IS NULL OR DT_FIM_ATR > GETDATE())"
                Sql = Sql & " AND (LEN(YARD)=1 OR LEN(YARD)=2) "
                Sql = Sql & " AND (SUBSTRING(YARD,1,1)='" & Left(Yard, 1) & "' OR SUBSTRING(YARD,1,2)='" & Left(Yard, 2) & "')"

                Dim tbv1 As DataTable
                tbv1 = BD.List(Sql)
                If tbv1.Rows.Count > 0 Then
                    tbv1.Dispose()
                    Retorno = "Quadra não permite conteiner IMO"
                    Return Retorno
                End If
                tbv1.Dispose()


                Sql = " SELECT YARD FROM INTELOPER..TB_IP_ATRIBUTO_YARD"
                Sql = Sql & " WHERE PATIO=" & Patio & " AND AUTONUM_ATR=21 AND VLR_ATRIB=1 AND STATUS_ATRIB='R'"
                Sql = Sql & " AND DT_INI_ATR < GETDATE() AND (DT_FIM_ATR IS NULL OR DT_FIM_ATR>GETDATE())"
                If Tamanho = 20 Then
                    Sql = Sql & " AND SUBSTRING(YARD,1,6)='" & Mid$(Yard, 1, 6) & "'"
                Else
                    Sql = Sql & " AND (SUBSTRING(YARD,1,6)='" & Mid$(Yard, 1, 2) & Format(Val(Mid$(Yard, 3, 2)) + 1, "00") & Mid$(Yard, 5, 2) & "' OR SUBSTRING(YARD,1,6)='" & Mid$(Yard, 1, 2) & Format(Val(Mid$(Yard, 3, 2)) - 1, "00") & Mid$(Yard, 5, 2) & "')"
                End If

                Sql = Sql & " Union"
                Sql = Sql & " SELECT YARD FROM INTELOPER..TB_IP_ATRIBUTO_YARD"
                Sql = Sql & " WHERE PATIO=" & Patio & " AND AUTONUM_ATR=21 AND VLR_ATRIB=0 AND STATUS_ATRIB='E'"
                Sql = Sql & " AND DT_INI_ATR < GETDATE() AND (DT_FIM_ATR IS NULL OR DT_FIM_ATR>GETDATE())"
                If Tamanho = 20 Then
                    Sql = Sql & " AND SUBSTRING(YARD,1,6)='" & Mid$(Yard, 1, 6) & "'"
                Else
                    Sql = Sql & " AND (SUBSTRING(YARD,1,6)='" & Mid$(Yard, 1, 2) & Format(Val(Mid$(Yard, 3, 2)) + 1, "00") & Mid$(Yard, 5, 2) & "' OR SUBSTRING(YARD,1,6)='" & Mid$(Yard, 1, 2) & Format(Val(Mid$(Yard, 3, 2)) - 1, "00") & Mid$(Yard, 5, 2) & "')"
                End If

                Dim tbv2 As DataTable
                tbv2 = BD.List(Sql)
                If tbv2.Rows.Count > 0 Then
                    tbv2.Dispose()
                    Retorno = "Pilha não permite conteiner IMO"
                    Return Retorno
                End If
                tbv2.Dispose()

            Else

                Sql = " SELECT YARD FROM INTELOPER..TB_IP_ATRIBUTO_YARD"
                Sql = Sql & " WHERE PATIO=" & Patio & " AND AUTONUM_ATR=21 AND VLR_ATRIB=1 AND STATUS_ATRIB='R'"
                Sql = Sql & " AND DT_INI_ATR < GETDATE() AND (DT_FIM_ATR IS NULL OR DT_FIM_ATR> GETDATE())"
                Sql = Sql & " AND YARD='" & Yard & "'"
                Sql = Sql & " Union"
                Sql = Sql & " SELECT YARD FROM INTELOPER..TB_IP_ATRIBUTO_YARD"
                Sql = Sql & " WHERE PATIO=" & Patio & " AND AUTONUM_ATR=21 AND VLR_ATRIB=0 AND STATUS_ATRIB='E'"
                Sql = Sql & " AND DT_INI_ATR < GETDATE() AND (DT_FIM_ATR IS NULL OR DT_FIM_ATR> GETDATE() )"
                Sql = Sql & " AND YARD='" & Yard & "'"

                Dim tbv3 As DataTable
                tbv3 = BD.List(Sql)
                If tbv3.Rows.Count > 0 Then
                    tbv3.Dispose()
                    Retorno = "Heap/Rua não permite conteiner IMO"
                    Return Retorno
                End If
                tbv3.Dispose()

            End If





            For i = 1 To 4
                If Trim(tb1.Rows(0)("IMO" & i).ToString) <> "" Then
                    If Trim(tb1.Rows(0)("IMO" & i).ToString) <> "0" Then
                        'Verifica se pode pela classe IMO
                        If Valida Then
                            'A QUADRA TODA
                            Sql = " SELECT YARD FROM INTELOPER..TB_IP_ATRIBUTO_YARD"
                            Sql = Sql & " WHERE PATIO=" & Patio & " AND AUTONUM_ATR=12 AND VLR_ATRIB='" & Trim(tb1.Rows(0)("IMO" & i).ToString) & "' AND STATUS_ATRIB='R'"
                            Sql = Sql & " AND DT_INI_ATR < GETDATE() AND (DT_FIM_ATR IS NULL OR DT_FIM_ATR>GETDATE())"
                            Sql = Sql & " AND (LEN(YARD)=1 OR LEN(YARD)=2) "
                            Sql = Sql & " AND (SUBSTRING(YARD,1,1)='" & Left(Yard, 1) & "' OR SUBSTRING(YARD,1,2)='" & Left(Yard, 2) & "')"
                            Sql = Sql & " Union"
                            Sql = Sql & " SELECT YARD FROM INTELOPER..TB_IP_ATRIBUTO_YARD"
                            Sql = Sql & " WHERE AUTONUM_ATR=12 AND VLR_ATRIB<>'" & Trim(tb1.Rows(0)("IMO" & i).ToString) & "' AND STATUS_ATRIB='E'"
                            Sql = Sql & " AND DT_INI_ATR < GETDATE() AND (DT_FIM_ATR IS NULL OR DT_FIM_ATR > GETDATE())"
                            Sql = Sql & " AND (LEN(YARD)=1 OR LEN(YARD)=2) "
                            Sql = Sql & " AND (SUBSTRING(YARD,1,1)='" & Left(Yard, 1) & "' OR SUBSTRING(YARD,1,2)='" & Left(Yard, 2) & "')"

                            Dim tbv4 As DataTable

                            tbv4 = BD.List(Sql)
                            If tbv4.Rows.Count > 0 Then
                                tbv4.Dispose()
                                Retorno = "Quadra não permite classe IMO " & Trim(tb1.Rows(0)("IMO" & i).ToString)
                                Return Retorno
                            End If
                            tbv4.Dispose()
                            'PILHA

                            Sql = " SELECT YARD FROM INTELOPER..TB_IP_ATRIBUTO_YARD"
                            Sql = Sql & " WHERE PATIO=" & Patio & " AND AUTONUM_ATR=12 AND VLR_ATRIB='" & Trim(tb1.Rows(0)("IMO" & i).ToString) & "' AND STATUS_ATRIB='R'"
                            Sql = Sql & " AND DT_INI_ATR < GETDATE() AND (DT_FIM_ATR IS NULL OR DT_FIM_ATR>GETDATE())"
                            If Tamanho = 20 Then
                                Sql = Sql & " AND SUBSTRING(YARD,1,6)='" & Mid$(Yard, 1, 6) & "'"
                            Else
                                Sql = Sql & " AND (SUBSTRING(YARD,1,6)='" & Mid$(Yard, 1, 2) & Format(Val(Mid$(Yard, 3, 2)) + 1, "00") & Mid$(Yard, 5, 2) & "' OR SUBSTRING(YARD,1,6)='" & Mid$(Yard, 1, 2) & Format(Val(Mid$(Yard, 3, 2)) - 1, "00") & Mid$(Yard, 5, 2) & "')"
                            End If
                            Sql = Sql & " Union"
                            Sql = Sql & " SELECT YARD FROM INTELOPER..TB_IP_ATRIBUTO_YARD"
                            Sql = Sql & " WHERE PATIO=" & Patio & " AND AUTONUM_ATR=12 AND VLR_ATRIB<>'" & Trim(tb1.Rows(0)("IMO" & i).ToString) & "' AND STATUS_ATRIB='E'"
                            Sql = Sql & " AND DT_INI_ATR < GETDATE() AND (DT_FIM_ATR IS NULL OR DT_FIM_ATR>GETDATE())"
                            If Tamanho = 20 Then
                                Sql = Sql & " AND SUBSTRING(YARD,1,6)='" & Mid$(Yard, 1, 6) & "'"
                            Else
                                Sql = Sql & " AND (SUBSTRING(YARD,1,6)='" & Mid$(Yard, 1, 2) & Format(Val(Mid$(Yard, 3, 2)) + 1, "00") & Mid$(Yard, 5, 2) & "' OR SUBSTRING(YARD,1,6)='" & Mid$(Yard, 1, 2) & Format(Val(Mid$(Yard, 3, 2)) - 1, "00") & Mid$(Yard, 5, 2) & "')"
                            End If

                            Dim tbv5 As DataTable
                            tbv5 = BD.List(Sql)
                            If tbv5.Rows.Count > 0 Then
                                tbv5.Dispose()
                                Retorno = "Pilha não permite Classe IMO " & Trim(tb1.Rows(0)("IMO" & i).ToString)
                                Return Retorno
                            End If
                            tbv5.Dispose()


                        Else


                            Sql = " SELECT YARD FROM INTELOPER..TB_IP_ATRIBUTO_YARD"
                            Sql = Sql & " WHERE PATIO=" & Patio & " AND AUTONUM_ATR=12 AND VLR_ATRIB='" & Trim(tb1.Rows(0)("IMO" & i).ToString) & "' AND STATUS_ATRIB='R'"
                            Sql = Sql & " AND DT_INI_ATR < GETDATE() AND (DT_FIM_ATR IS NULL OR DT_FIM_ATR>GETDATE())"
                            Sql = Sql & " AND YARD='" & Yard & "'"
                            Sql = Sql & " Union"
                            Sql = Sql & " SELECT YARD FROM INTELOPER..TB_IP_ATRIBUTO_YARD"
                            Sql = Sql & " WHERE AUTONUM_ATR=12 AND VLR_ATRIB<>'" & Trim(tb1.Rows(0)("IMO" & i).ToString) & "' AND STATUS_ATRIB='E'"
                            Sql = Sql & " AND DT_INI_ATR < GETDATE() AND (DT_FIM_ATR IS NULL OR DT_FIM_ATR>GETDATE())"
                            Sql = Sql & " AND YARD='" & Yard & "'"

                            Dim tbv6 As DataTable
                            tbv6 = BD.List(Sql)
                            If tbv6.Rows.Count > 0 Then
                                tbv6.Dispose()
                                Retorno = "Heap/Rua não permite Classe IMO " & Trim(tb1.Rows(0)("IMO" & i).ToString)
                                Return Retorno
                            End If
                            tbv6.Dispose()
                        End If

                    End If
                End If
            Next

        End If






        Return Retorno


    End Function

    Function Valida_NImo(Id_Conteiner As String, Yard As String, Tamanho As Integer, Patio As Integer) As String
        Dim Retorno As String
        Retorno = ""

        Dim Valida As Boolean
        Valida = False

        Dim Sql As String
        Sql = "SELECT ISNULL(valida,0) AS VALIDA FROM " & BD.BancoOperador & "TB_YARD WHERE PATIO=" & Patio & " AND YARD='" & Yard & "'"

        If BD.ExecuteScalar(Sql) = 1 Then
            Valida = True
        Else
            Valida = False
        End If


        If Valida Then
            'A QUADRA é APENAS PARA IMO
            Sql = " SELECT YARD FROM INTELOPER..TB_IP_ATRIBUTO_YARD"
            Sql = Sql & " WHERE PATIO=" & Patio & " AND AUTONUM_ATR=21 AND VLR_ATRIB=1 AND STATUS_ATRIB='E'"
            Sql = Sql & " AND DT_INI_ATR < GETDATE() AND (DT_FIM_ATR IS NULL OR DT_FIM_ATR> GETDATE() )"
            Sql = Sql & " AND (LEN(YARD)=1 OR LEN(YARD)=2) "
            Sql = Sql & " AND (SUBSTRING(YARD,1,1)='" & Left(Yard, 1) & "' OR SUBSTRING(YARD,1,2)='" & Left(Yard, 2) & "')"
            Sql = Sql & " Union"
            Sql = Sql & " SELECT YARD FROM INTELOPER..TB_IP_ATRIBUTO_YARD"
            Sql = Sql & " WHERE PATIO = " & Patio & " AND AUTONUM_ATR=21 AND VLR_ATRIB=0 AND STATUS_ATRIB='R'"
            Sql = Sql & " AND DT_INI_ATR < GETDATE() AND (DT_FIM_ATR IS NULL OR DT_FIM_ATR > GETDATE())"
            Sql = Sql & " AND (LEN(YARD)=1 OR LEN(YARD)=2) "
            Sql = Sql & " AND (SUBSTRING(YARD,1,1)='" & Left(Yard, 1) & "' OR SUBSTRING(YARD,1,2)='" & Left(Yard, 2) & "')"

            Dim tbv1 As DataTable
            tbv1 = BD.List(Sql)
            If tbv1.Rows.Count > 0 Then
                tbv1.Dispose()
                Retorno = "Quadra APENAS permite conteiner IMO"
                Return Retorno
            End If
            tbv1.Dispose()


            Sql = " SELECT YARD FROM INTELOPER..TB_IP_ATRIBUTO_YARD"
            Sql = Sql & " WHERE PATIO=" & Patio & " AND AUTONUM_ATR=21 AND VLR_ATRIB=1 AND STATUS_ATRIB='E'"
            Sql = Sql & " AND DT_INI_ATR < GETDATE() AND (DT_FIM_ATR IS NULL OR DT_FIM_ATR>GETDATE())"
            If Tamanho = 20 Then
                Sql = Sql & " AND SUBSTRING(YARD,1,6)='" & Mid$(Yard, 1, 6) & "'"
            Else
                Sql = Sql & " AND (SUBSTRING(YARD,1,6)='" & Mid$(Yard, 1, 2) & Format(Val(Mid$(Yard, 3, 2)) + 1, "00") & Mid$(Yard, 5, 2) & "' OR SUBSTRING(YARD,1,6)='" & Mid$(Yard, 1, 2) & Format(Val(Mid$(Yard, 3, 2)) - 1, "00") & Mid$(Yard, 5, 2) & "')"
            End If

            Sql = Sql & " Union"
            Sql = Sql & " SELECT YARD FROM INTELOPER..TB_IP_ATRIBUTO_YARD"
            Sql = Sql & " WHERE PATIO=" & Patio & " AND AUTONUM_ATR=21 AND VLR_ATRIB=0 AND STATUS_ATRIB='R'"
            Sql = Sql & " AND DT_INI_ATR < GETDATE() AND (DT_FIM_ATR IS NULL OR DT_FIM_ATR>GETDATE())"
            If Tamanho = 20 Then
                Sql = Sql & " AND SUBSTRING(YARD,1,6)='" & Mid$(Yard, 1, 6) & "'"
            Else
                Sql = Sql & " AND (SUBSTRING(YARD,1,6)='" & Mid$(Yard, 1, 2) & Format(Val(Mid$(Yard, 3, 2)) + 1, "00") & Mid$(Yard, 5, 2) & "' OR SUBSTRING(YARD,1,6)='" & Mid$(Yard, 1, 2) & Format(Val(Mid$(Yard, 3, 2)) - 1, "00") & Mid$(Yard, 5, 2) & "')"
            End If

            Dim tbv2 As DataTable
            tbv2 = BD.List(Sql)
            If tbv2.Rows.Count > 0 Then
                tbv2.Dispose()
                Retorno = "Pilha APENAS permite conteiner IMO"
                Return Retorno
            End If
            tbv2.Dispose()



        Else

            Sql = " SELECT YARD FROM INTELOPER..TB_IP_ATRIBUTO_YARD"
            Sql = Sql & " WHERE PATIO=" & Patio & " AND AUTONUM_ATR=21 AND VLR_ATRIB=1 AND STATUS_ATRIB='E'"
            Sql = Sql & " AND DT_INI_ATR < GETDATE() AND (DT_FIM_ATR IS NULL OR DT_FIM_ATR> GETDATE())"
            Sql = Sql & " AND YARD='" & Yard & "'"
            Sql = Sql & " Union"
            Sql = Sql & " SELECT YARD FROM INTELOPER..TB_IP_ATRIBUTO_YARD"
            Sql = Sql & " WHERE PATIO=" & Patio & " AND AUTONUM_ATR=21 AND VLR_ATRIB=0 AND STATUS_ATRIB='R'"
            Sql = Sql & " AND DT_INI_ATR < GETDATE() AND (DT_FIM_ATR IS NULL OR DT_FIM_ATR> GETDATE() )"
            Sql = Sql & " AND YARD='" & Yard & "'"

            Dim tbv3 As DataTable
            tbv3 = BD.List(Sql)
            If tbv3.Rows.Count > 0 Then
                tbv3.Dispose()
                Retorno = "Heap/Rua APENAS permite conteiner IMO"
                Return Retorno
            End If
            tbv3.Dispose()

        End If


        Return Retorno


    End Function



    Protected Sub btSalvar_Click(sender As Object, e As EventArgs) Handles btSalvar.Click

        Dim Retorno As String

        If ValidaCampos() Then

            If Me.txtIMO.Text.Trim <> "" Then
                Dim Ret As String
                Ret = ""
                Ret = Valida_Aloca_Imo(Me.txtCNTR.Text, Me.txtYard.Text.ToUpper, Val(Me.txtTAM.Text), Session("AUTONUMPATIO"))
                If Ret <> "" Then
                    ScriptManager.RegisterClientScriptBlock(Me, [GetType](), "script", "<script>alert('" & Ret & "');</script>", False)
                    Exit Sub
                End If
            Else

                Dim Ret2 As String
                Ret2 = ""
                Ret2 = Valida_NImo(Me.txtCNTR.Text, Me.txtYard.Text.ToUpper, Val(Me.txtTAM.Text), Session("AUTONUMPATIO"))
                If Ret2 <> "" Then
                    ScriptManager.RegisterClientScriptBlock(Me, [GetType](), "script", "<script>alert('" & Ret2 & "');</script>", False)
                    Exit Sub
                End If

            End If


            Try


                Dim con As New SqlConnection(BD.StringConexaoSemProvider)
                con.Open()
                Dim cmd As New SqlCommand("" & BD.BancoSgipa & "FN_MOV_CNTR2", con)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add(New SqlParameter("RESULT", SqlDbType.VarChar, 300, ParameterDirection.ReturnValue, True, 0, 0, "RESULT", DataRowVersion.Current, ""))
                cmd.Parameters.Add("wID_Conteiner", SqlDbType.VarChar, 12).Value = Me.txtCNTR.Text
                cmd.Parameters.Add("wAutonumCntr", SqlDbType.Int).Value = Val(Me.txtAutonum.Text)
                cmd.Parameters.Add("wSistema", SqlDbType.VarChar, 1).Value = Me.txtSistema.Text
                cmd.Parameters.Add("wTamanho", SqlDbType.Int).Value = Val(Me.txtTAM.Text)
                cmd.Parameters.Add("wPatio", SqlDbType.Int).Value = Session("AUTONUMPATIO")
                cmd.Parameters.Add("wYardOrigem", SqlDbType.VarChar, 12).Value = Me.txtYardAtual.Text.ToUpper
                cmd.Parameters.Add("wYardDestino", SqlDbType.VarChar, 12).Value = Me.txtYard.Text.ToUpper
                cmd.Parameters.Add("wMotivo", SqlDbType.Int).Value = Me.cbMotivoPos.SelectedValue
                cmd.Parameters.Add("wUsuario", SqlDbType.Int).Value = Session("AUTONUMUSUARIO")
                cmd.Parameters.Add("wVeiculo", SqlDbType.Int).Value = IIf(Me.cbVeiculo.SelectedIndex = 0, 0, Me.cbVeiculo.SelectedValue)
                cmd.Parameters.Add("wEmpilhadeira", SqlDbType.Int).Value = IIf(Me.cbEmpilhadeira.SelectedIndex = 0, 0, Me.cbEmpilhadeira.SelectedValue)
                cmd.Parameters.Add("wBROWSER_NAME", SqlDbType.VarChar, 50).Value = Session("BROWSER_NAME")
                cmd.Parameters.Add("wBROWSER_VERSION", SqlDbType.VarChar, 50).Value = Session("BROWSER_VERSION")
                cmd.Parameters.Add("wMOBILEDEVICEMODEL", SqlDbType.VarChar, 50).Value = " "
                cmd.Parameters.Add("wMOBILEDEVICEMANUFACTURER", SqlDbType.VarChar, 50).Value = " "
                cmd.Parameters.Add("wFLAG_MOBILE", SqlDbType.Int).Value = IIf(Session("ISMOBILEDEVICE") = False, 0, 1)

                cmd.ExecuteScalar()
                Retorno = (cmd.Parameters(0).Value.ToString)
                con.Close()



                If Retorno = "OK" Then

                    Dim Sql As String

                    If (Me.txtSistema.Text = "I") Then
                        Sql = "UPDATE SGIPA..TB_CNTR_BL Set YARD='" & Me.txtYard.Text.ToUpper & "' WHERE AUTONUM=" & Val(Me.txtAutonum.Text)
                        BD.ExecuteScalar(Sql)
                    End If

                    If (Me.txtSistema.Text = "R") Then
                        Sql = "UPDATE REDEX..TB_PATIO Set YARD='" & Me.txtYard.Text.ToUpper & "' WHERE AUTONUM_PATIO=" & Val(Me.txtAutonum.Text)
                        BD.ExecuteScalar(Sql)
                    End If

                    If (Me.txtSistema.Text = "Z" Or Me.txtSistema.Text = "A") Then
                        Sql = "UPDATE SGIPA..TB_ARMAZENS_IPA Set YARD='" & Me.txtYard.Text.ToUpper & "' WHERE AUTONUM=" & Val(Me.txtAutonum.Text)
                        BD.ExecuteScalar(Sql)
                    End If

                    Sql = "INSERT INTO OPERADOR..TB_HIST_SHIFTING (CNTR, ORIGEM, DESTINO, Data, MOTIVO, TIPO, USUARIO, ID_TRANSPORTADORA, AUTONUM_FROTA_CARRETA, AUTONUM_FROTA_EMPILHADEIRA)VALUES(" & Val(Me.txtAutonum.Text) & ", '" & Me.txtYardAtual.Text.ToUpper & "', '" & Me.txtYard.Text.ToUpper & "',GETDATE()," & Me.cbMotivoPos.SelectedValue & ", '" & Me.txtSistema.Text & "', " & Session("AUTONUMUSUARIO") & ",0," & IIf(Me.cbVeiculo.SelectedIndex = 0, 0, Me.cbVeiculo.SelectedValue) & "," & IIf(Me.cbEmpilhadeira.SelectedIndex = 0, 0, Me.cbEmpilhadeira.SelectedValue) & ")"
                    BD.ExecuteScalar(Sql)

                    If Me.txtIMO.Text.Trim <> "" Then
                        Dim RetSeg As String = ""
                        RetSeg = ""
                        RetSeg = Verifica_Regras_Seg_Imo_Delta(Me.txtCNTR.Text, Me.txtYard.Text.ToUpper, Session("AUTONUMPATIO"))
                        If RetSeg <> "" Then
                            Dim SqlVolta As String = ""
                            If Me.txtSistema.Text = "I" Then
                                SqlVolta = "UPDATE " & BD.BancoSgipa & "TB_CNTR_BL Set YARD='" & Me.txtYardAtual.Text & "' WHERE AUTONUM=" & Val(Me.txtAutonum.Text)
                            ElseIf Me.txtSistema.Text = "O" Then
                                SqlVolta = "UPDATE " & BD.BancoOperador & "TB_PATIO SET YARD='" & Me.txtYardAtual.Text & "' WHERE AUTONUM=" & Val(Me.txtAutonum.Text)
                            ElseIf Me.txtSistema.Text = "R" Then
                                SqlVolta = "UPDATE " & Banco.BancoRedex & "TB_PATIO SET YARD='" & Me.txtYardAtual.Text & "' WHERE AUTONUM_PATIO=" & Val(Me.txtAutonum.Text)
                            ElseIf Me.txtSistema.Text = "A" Then
                                SqlVolta = "UPDATE " & BD.BancoSgipa & "TB_ARMAZENS_IPA SET YARD='" & Me.txtYardAtual.Text & "' WHERE AUTONUM=" & Val(Me.txtAutonum.Text)
                            End If

                            BD.ExecuteScalar(SqlVolta)
                            ScriptManager.RegisterClientScriptBlock(Me, [GetType](), "script", "<script>alert('" & RetSeg & "');</script>", False)
                        Else
                            ScriptManager.RegisterClientScriptBlock(Me, [GetType](), "script", "<script>alert('" & Retorno & "');</script>", False)
                            Limpa()
                            Me.txtCNTR.Text = ""
                            Me.txtCNTR4.Text = ""
                            Me.txtCNTR4.Focus()
                        End If

                    Else
                        ScriptManager.RegisterClientScriptBlock(Me, [GetType](), "script", "<script>alert('Atualizado com Sucesso');</script>", False)
                        Limpa()
                        Me.txtCNTR.Text = ""
                        Me.txtCNTR4.Text = ""
                        Me.txtCNTR4.Focus()
                    End If

                Else
                    ScriptManager.RegisterClientScriptBlock(Me, [GetType](), "script", "<script>alert('" & Retorno & "');</script>", False)
                End If

            Catch ex As Exception

                ScriptManager.RegisterClientScriptBlock(Me, [GetType](), "script", "<script>alert('Erro. tente novamente');</script>", False)
            End Try

            btSalvar.Enabled = True

        End If

        btSalvar.Enabled = True

    End Sub


    Function ValidaCampos() As Boolean

        If Me.txtYard.Text = "" Then
            ScriptManager.RegisterClientScriptBlock(Me, [GetType](), "script", "<script>alert('Local não informado!');</script>", False)
            Return False
        End If

        If Me.txtYard.Text = Me.txtYardAtual.Text Then
            ScriptManager.RegisterClientScriptBlock(Me, [GetType](), "script", "<script>alert('Origem e Destino iguais!');</script>", False)
            Return False
        End If

        If Me.cbMotivoPos.SelectedIndex = 0 Then
            ScriptManager.RegisterClientScriptBlock(Me, [GetType](), "script", "<script>alert('Informe o motivo do movimento !');</script>", False)
            Return False
        End If

        If Session("FLAG_TRUCK_MOV_COLETOR") = "1" Then
            If Me.cbVeiculo.SelectedIndex = 0 Then
                ScriptManager.RegisterClientScriptBlock(Me, [GetType](), "script", "<script>alert('Informe o veículo para esta operação!');</script>", False)
                Return False
            End If
            Return False
        End If

        Return True
    End Function

    Protected Sub txtCNTR_TextChanged(sender As Object, e As EventArgs) Handles txtCNTR.TextChanged
        txtCNTR4.Text = ""
    End Sub

    Protected Sub txtCNTR4_TextChanged(sender As Object, e As EventArgs) Handles txtCNTR4.TextChanged

        txtCNTR.Text = ""

    End Sub

    Function ValidaYard(WIdConteiner As String, Wtamanho As Integer, WYard As String, WYardOrigem As String, WPatio As Integer) As Boolean

        Dim WYardValida As Integer = 0
        Dim WYardOrValida As Integer = 0
        Dim WFlagRua As Integer = 0
        Dim WFlagORRua As Integer = 0

        Dim wQuadra As String = String.Empty
        Dim wFiada As String = String.Empty
        Dim wCol As String = String.Empty
        Dim WAlt As String = String.Empty

        Dim wQuadraOr As String = String.Empty
        Dim wFiadaOr As String = String.Empty
        Dim wColOr As String = String.Empty
        Dim WAltOr As String = String.Empty

        Dim Sql As String = String.Empty
        Sql = "SELECT YARD, ISNULL(VALIDA,0) AS VALIDA, ISNULL(FLAG_RUA,0) AS FLAG_RUA, ISNULL(FLAG_BLOQUEIO,1) AS FLAG_BLOQUEIO FROM " & BD.BancoOperador & "TB_YARD WHERE YARD = '" & WYard & "' AND PATIO = " & WPatio
        Dim TB1 As New DataTable
        TB1 = Banco.Consultar(Sql)
        If TB1 IsNot Nothing Then
            If TB1.Rows.Count = 0 Then
                ScriptManager.RegisterClientScriptBlock(Me, [GetType](), "script", "<script>alert('Posição não encontrada!');</script>", False)
                Return False
            Else
                If (TB1.Rows.Count > 0) Then
                    If TB1.Rows(0)("FLAG_BLOQUEIO").ToString() = 1 Then
                        ScriptManager.RegisterClientScriptBlock(Me, [GetType](), "script", "<script>alert('Posição bloqueada!');</script>", False)
                        Return False
                    End If
                    WYardValida = TB1.Rows(0)("VALIDA").ToString()
                    WFlagRua = TB1.Rows(0)("FLAG_RUA").ToString()

                End If
            End If
        End If

        Sql = "SELECT YARD, ISNULL(VALIDA,0) AS VALIDA, ISNULL(FLAG_RUA,0) AS FLAG_RUA, ISNULL(FLAG_BLOQUEIO,1) AS FLAG_BLOQUEIO FROM " & BD.BancoOperador & "TB_YARD WHERE YARD = '" & WYardOrigem & "' AND PATIO = " & WPatio
        TB1 = Banco.Consultar(Sql)
        If TB1 IsNot Nothing Then
            If TB1.Rows.Count > 0 Then
                WYardOrValida = TB1.Rows(0)("VALIDA").ToString()
                WFlagORRua = TB1.Rows(0)("FLAG_RUA").ToString()
            End If
        End If


        If WYardOrValida = 1 Then

            wQuadraOr = Microsoft.VisualBasic.Mid(WYardOrigem, 1, 2)
            wFiadaOr = Microsoft.VisualBasic.Mid(WYardOrigem, 3, 2)
            wColOr = Microsoft.VisualBasic.Mid(WYardOrigem, 5, 2)
            WAltOr = Microsoft.VisualBasic.Mid(WYardOrigem, 7, 1)

            'Nao pode ter um cntr acima dele
            Sql = "SELECT ID_CONTEINER FROM " & BD.BancoOperador & "VW_INVENT_SISTEMAS_PY WHERE "
            Sql = Sql & " PATIO=" & WPatio & " AND YARD = '" & wQuadraOr & wFiadaOr & wColOr & Microsoft.VisualBasic.Format(Val(WAltOr) + 1, "0") & "'"
            TB1 = Banco.Consultar(Sql)
            If TB1 IsNot Nothing Then
                If TB1.Rows.Count > 0 Then
                    Dim M1 As String
                    M1 = " Existe conteiner " & TB1.Rows(0)("ID_CONTEINER").ToString() & " acima. Não pode mover! "
                    ScriptManager.RegisterClientScriptBlock(Me, [GetType](), "script", "<script>alert('" & M1 & "');</script>", False)
                    Return False
                End If
            End If

        End If

        If WYardValida = 1 Then

            wQuadra = Microsoft.VisualBasic.Mid(WYard, 1, 2)
            wFiada = Microsoft.VisualBasic.Mid(WYard, 3, 2)
            wCol = Microsoft.VisualBasic.Mid(WYard, 5, 2)
            WAltOr = Microsoft.VisualBasic.Mid(WYard, 7, 1)

            'Conteiner de 20 fiada Impar
            If Wtamanho = 20 Then
                If Val(wFiada) Mod 2 = 0 Then
                    ScriptManager.RegisterClientScriptBlock(Me, [GetType](), "script", "<script>alert('Conteiner de 20 de ser colocado em fiada Impar!');</script>", False)
                    Return False
                End If
            End If

            'Conteiner de 40 fiada Par
            If Wtamanho = 40 Then
                If Val(wFiada) Mod 2 = 1 Then
                    ScriptManager.RegisterClientScriptBlock(Me, [GetType](), "script", "<script>alert('Conteiner de 40 de ser colocado em fiada par!');</script>", False)
                    Return False
                End If
            End If

            'Nao pode ter outro conteiner na mesma Posicao
            Sql = "SELECT ID_CONTEINER FROM " & BD.BancoOperador & "VW_INVENT_SISTEMAS_PY WHERE "
            Sql = Sql & " PATIO=" & WPatio & " AND (YARD = '" & WYard & "'"
            If Wtamanho = 20 Then
                Sql = Sql & " or YARD='" & wQuadra & Microsoft.VisualBasic.Format(Val(wFiada) + 1, "00") & wCol & WAlt & "'"
                Sql = Sql & " or YARD='" & wQuadra & Microsoft.VisualBasic.Format(Val(wFiada) - 1, "00") & wCol & WAlt & "'"
            End If
            If Wtamanho = 40 Then
                Sql = Sql & " or YARD='" & wQuadra & Microsoft.VisualBasic.Format(Val(wFiada) + 1, "00") & wCol & WAlt & "'"
                Sql = Sql & " or YARD='" & wQuadra & Microsoft.VisualBasic.Format(Val(wFiada) - 1, "00") & wCol & WAlt & "'"
                Sql = Sql & " or YARD='" & wQuadra & Microsoft.VisualBasic.Format(Val(wFiada) + 2, "00") & wCol & WAlt & "'"
                Sql = Sql & " or YARD='" & wQuadra & Microsoft.VisualBasic.Format(Val(wFiada) - 2, "00") & wCol & WAlt & "'"
            End If
            Sql = Sql & ")"
            Sql = Sql & " And ID_CONTEINER <>'" & WIdConteiner & "'"
            TB1 = Banco.Consultar(Sql)
            If TB1 IsNot Nothing Then
                If TB1.Rows.Count > 0 Then
                    Dim M2 As String
                    M2 = " Existe OUTRO conteiner " & TB1.Rows(0)("ID_CONTEINER").Value.ToString & " nesta posição ! "
                    ScriptManager.RegisterClientScriptBlock(Me, [GetType](), "script", "<script>alert('" & M2 & "');</script>", False)
                    Return False
                End If
            End If

            'Se a altura>1 entao tem que ter um abaixo
            If WAlt > "1" Then
                Sql = "SELECT ID_CONTEINER FROM " & BD.BancoOperador & "VW_INVENT_SISTEMAS_PY WHERE "
                Sql = Sql & " PATIO=" & WPatio & " AND YARD = '" & wQuadra & wFiada & wCol & Microsoft.VisualBasic.Format(Val(WAlt) - 1, "0") & "'"

                If String.IsNullOrEmpty(Banco.ExecuteScalar(Sql)) Then
                    ScriptManager.RegisterClientScriptBlock(Me, [GetType](), "script", "<script>alert('Não existe conteiner na altura abaixo!');</script>", False)
                    Return False
                End If
            End If
        End If



        Return True

    End Function




    Protected Sub btSalvar0_Click(sender As Object, e As EventArgs) Handles btSalvar0.Click
        Limpa()
        Me.txtCNTR.Text = ""
        Me.txtCNTR4.Text = ""
    End Sub



    Protected Sub txtPosicionamento_TextChanged(sender As Object, e As EventArgs) Handles txtPosicionamento.TextChanged

    End Sub

    Protected Sub txtYard_TextChanged(sender As Object, e As EventArgs) Handles txtYard.TextChanged


        If Me.txtYard.Text.Trim = "SAIDA" Or Me.txtYard.Text.Trim = "TREM" Then
            Me.cbMotivoPos.SelectedValue = 8
            btSalvar.Focus()
            Exit Sub
        End If

        If Me.txtYard.Text.Trim = "BAL" Then
            Me.cbMotivoPos.SelectedValue = 6
            btSalvar.Focus()
            Exit Sub
        End If

        Dim Sql As String
        Sql = "SELECT 1 FROM " & BD.BancoOperador & "TB_YARD WHERE " + IIf(String.IsNullOrEmpty(Session("AUTONUMPATIO")), +" PATIO=" & Session("AUTONUMPATIO") + "  AND ", "")
        Sql = Sql & " YARD='" & Me.txtYard.Text & "'"
        Sql = Sql & " AND VALIDA=0 AND FLAG_RUA=0 "
        Dim tBm As New DataTable
        tBm = Banco.Consultar(Sql)
        If tBm IsNot Nothing Then
            If tBm.Rows.Count > 0 Then
                Me.cbMotivoPos.SelectedValue = 33
                btSalvar.Focus()
                Exit Sub
            End If
        End If


        If Me.txtYardAtual.Text.Trim = "" And Me.txtYard.Text <> "" Then
            If Me.cbMotivoPos.Items.Count >= 12 Then
                Me.cbMotivoPos.SelectedValue = 12
                btSalvar.Focus()
                Exit Sub
            End If
        End If

        If Me.txtYard.Text.Trim <> "" And Me.txtYardAtual.Text.Trim <> "" Then
            Me.cbMotivoPos.SelectedValue = 1
            btSalvar.Focus()
            Exit Sub
        End If

        btSalvar.Focus()

    End Sub

    Protected Sub txtRegime_TextChanged(sender As Object, e As EventArgs) Handles txtRegime.TextChanged

    End Sub
    Private Sub btnHome_Click(sender As Object, e As EventArgs) Handles btnHome.Click
        Response.Redirect(Login.ObterURIMenu())
    End Sub

    Private Sub btnLogout_Click(sender As Object, e As EventArgs) Handles btnLogout.Click
        Response.Redirect(Login.ObterURIMenu() & "home/logout")
    End Sub
End Class