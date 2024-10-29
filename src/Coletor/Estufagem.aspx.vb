Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.Globalization
Imports System.IO
Imports System.Web.Services
Imports System.Web.UI.WebControls
Public Class Estufagem
    Inherits System.Web.UI.Page

    Dim Sql As String = String.Empty

    Private Shared CargaSuzano As Boolean = False
    Private Boo As Integer = 0
    Dim filtro As String = String.Empty
    Private Shared sc As New SaidaCarga()
    Dim mensagemConfirmacao As String = String.Empty
    Private logDAO As New Log
    Public Shared Function ObterPagina(pg As Page) As String
        Return Path.GetFileNameWithoutExtension(pg.AppRelativeVirtualPath)
    End Function
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
                Session("FLAG_REDEX_SEM_MARCANTE") = Server.HtmlEncode(Request.Cookies("FLAG_REDEX_SEM_MARCANTE").Value)
                Session("LARGURATELA") = Server.HtmlEncode(Request.Cookies("LARGURATELA").Value)
            Catch ex As Exception
                Log.InserirLog(ObterPagina(Page), ex.Message, ex.StackTrace, Page.Session("AUTONUMUSUARIO"))
                ModelState.AddModelError(String.Empty, $"Ocorreu um erro ao carregar a página")
                ViewState("Sucesso") = False
                Exit Sub

            End Try
        End If

        If Not Page.IsPostBack Then
            SetEstiloPnlLimpar()
            LimparCampos()
            LimparCamposCarga()
            CarregarEquipes()
            CarregarConferentes()
            CarregarModo()

            If Not String.IsNullOrEmpty(Request.QueryString("cntr")) Then
                txtConteiner.Text = Request.QueryString("cntr")
                CarregarDadosConteiner(txtConteiner.Text)

                Exit Sub
            End If
        End If

    End Sub

    Private Sub CarregarConferentes()
        Try

            Sql = "Select autonum_eqp as AUTONUM, nome_eqp as DISPLAY from " + Banco.BancoRedex + "tb_equipe where flag_ativo=1 and flag_conferente=1 order by nome_eqp "

            Me.cbConferente.Items.Clear()
            Me.cbConferente.DataTextField = "DISPLAY"
            Me.cbConferente.DataValueField = "AUTONUM"
            Me.cbConferente.DataSource = Banco.List(Sql.ToString())
            Me.cbConferente.DataBind()

            cbConferente.Items.Insert(0, New ListItem("Selecione um Conf.", 0))
            cbConferente.SelectedIndex = 0
        Catch ex As Exception
            Log.InserirLog(ObterPagina(Page), ex.Message, ex.StackTrace, Page.Session("AUTONUMUSUARIO"))
            ModelState.AddModelError(String.Empty, $"Ocorreu um erro ao carregar conferentes")
            ViewState("Sucesso") = False
            Exit Sub
        End Try

    End Sub
    Private Sub CarregarEquipes()
        Try

            Sql = "Select autonum_eqp as AUTONUM, nome_eqp as DISPLAY from " + Banco.BancoRedex + "tb_equipe where flag_ativo=1 and flag_Operador=1 order by nome_eqp "

            Me.cbEquipe.Items.Clear()
            Me.cbEquipe.DataTextField = "DISPLAY"
            Me.cbEquipe.DataValueField = "AUTONUM"
            Me.cbEquipe.DataSource = Banco.List(Sql.ToString())
            Me.cbEquipe.DataBind()

            cbEquipe.Items.Insert(0, New ListItem("Selecione uma Equipe", 0))
            cbEquipe.SelectedIndex = 0

        Catch ex As Exception
            Log.InserirLog(ObterPagina(Page), ex.Message, ex.StackTrace, Page.Session("AUTONUMUSUARIO"))
            ModelState.AddModelError(String.Empty, $"Ocorreu um erro ao carregar equipes")
            ViewState("Sucesso") = False
            Exit Sub
        End Try

    End Sub

    Private Sub CarregarModo()
        Me.cbModo.Items.Insert(0, New ListItem("Selecione o Modo", "0"))
        Me.cbModo.Items.Insert(1, New ListItem("Automatizada", "A"))
        Me.cbModo.Items.Insert(2, New ListItem("Manual", "M"))
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
        Me.cbConferente.Dispose()
        Me.cbEquipe.Dispose()
        Me.cbModo.Dispose()
        Me.grdCargaEstufada.Dispose()

        Response.Redirect(Login.ObterURIMenu())
    End Sub

    Sub CarregaItensEstufados(autonumPatio As String, autonumTalie As String)
        Try
            Sql = $"SELECT 
                        cc.id_conteiner, 
                        D.reference, 
                        E.descricao_emb, 
                        F.desc_produto, 
                        A.QTDE_SAIDA, 
						A.Mercadoria,
						A.PESO_BRUTO,
						A.VOLUME,
						A.autonum_sc,
                        B.autonum_pcs, 
                        G.razao, 
                        H.descricao_nav + '--' + I.num_viagem AS navio_viagem, 
                        NF.num_nf,
						F.CODIGO as COD_PRODUTO,
                        A.AUTONUM_NFI,
                        A.AUTONUM_PATIO
                    FROM   
	                    {Banco.BancoRedex}tb_saida_carga A
                    INNER JOIN
                        {Banco.BancoRedex}tb_patio_cs B ON A.autonum_pcs = B.autonum_pcs
                    INNER JOIN       
	                    {Banco.BancoRedex}tb_booking_carga C ON B.autonum_bcg = C.autonum_bcg 
                    INNER JOIN
	                    {Banco.BancoRedex}tb_booking D ON C.autonum_boo = D.autonum_boo 
                    LEFT JOIN
	                    {Banco.BancoRedex}tb_cad_embalagens E ON B.autonum_emb = E.autonum_emb 
                    LEFT JOIN
	                    {Banco.BancoRedex}tb_cad_produtos F ON B.autonum_pro = f.autonum_pro 
                    INNER JOIN
	                    {Banco.BancoRedex}tb_cad_parceiros G ON D.autonum_parceiro = G.autonum 
                    INNER JOIN
	                    {Banco.BancoRedex}tb_viagens i ON D.autonum_via = i.autonum_via 
                    INNER JOIN
	                    {Banco.BancoRedex}tb_cad_navios h ON i.autonum_nav = h.autonum_nav
                    INNER JOIN {Banco.BancoRedex}tb_notas_fiscais NF ON A.autonum_nfI = NF.autonum_nf 
                    RIGHT JOIN
	                    {Banco.BancoRedex}tb_patio CC ON A.autonum_patio = CC.autonum_patio
                    WHERE   
	                    a.autonum_patio = {autonumPatio} AND a.autonum_talie = {autonumTalie} "

            Me.grdCargaEstufada.DataSource = Banco.List(Sql)
            Me.grdCargaEstufada.DataBind()

            If grdCargaEstufada.Rows.Count > 0 Then

                grdCargaEstufada.UseAccessibleHeader = True
                grdCargaEstufada.HeaderRow.TableSection = TableRowSection.TableHeader
                grdCargaEstufada.FooterRow.TableSection = TableRowSection.TableFooter
            End If

        Catch ex As Exception
            Log.InserirLog(ObterPagina(Page), ex.Message, ex.StackTrace, Page.Session("AUTONUMUSUARIO"))
            ModelState.AddModelError(String.Empty, $"Ocorreu um erro ao carregar itens estufados")
            ViewState("Sucesso") = False
            Exit Sub
        End Try
    End Sub
    Private Sub Mostra_Estufagem()
        Try
            Sql = "select sum(qtde_saida) from " & Banco.BancoRedex & "tb_saida_carga"
            Sql = Sql & " where"
            Sql = Sql & " autonum_patio = " & hddnAutonumPatio.Value
        Catch ex As Exception
            Log.InserirLog(ObterPagina(Page), ex.Message, ex.StackTrace, Page.Session("AUTONUMUSUARIO"))
            ModelState.AddModelError(String.Empty, $"Ocorreu um erro ao buscar dados da estufagem")
            ViewState("Sucesso") = False
            Exit Sub
        End Try
    End Sub

    Private Sub CalcularVolume()

        Dim vol = sc.Largura * sc.Altura * sc.Comprimento
        vol = vol / 1000000
        sc.Volume = vol
        If txtQuantidade.Text > 0 Then
            vol = vol * txtQuantidade.Text
            sc.VolumeTotal = vol
        End If

    End Sub

    Protected Sub btnCarga_Click(sender As Object, e As EventArgs) Handles btnCarga.Click
        Try

            If txtQuantidade.Text.ToDecimal() > ObterSaldoAtualizado().ToString().ToDecimal() Then
                ModelState.AddModelError(String.Empty, $"Quantidade Insuficiente em estoque")
            End If

            If txtQuantidade.Text.ToDecimal() <= .0D Then
                ModelState.AddModelError(String.Empty, $"Quantidade deve ser superior a 0")
            End If

            If hddnAutonumPatio.Value.ToInt() = 0 Then
                ModelState.AddModelError(String.Empty, $"Informe o contêiner")
            End If

            If cbNFs.SelectedValue <= 0 Then
                ModelState.AddModelError(String.Empty, $"Informe a NF - Operação cancelada")
            End If

            If hddnAutonumTalie.Value.Equals("0") Then
                ModelState.AddModelError(String.Empty, $"Não consta talie para este conteiner")
            Else
                Sql = "select flag_fechado from redex.dbo.tb_talie where autonum_talie=" & hddnAutonumTalie.Value
                If Banco.ExecuteScalar(Sql) = "1" Then
                    ModelState.AddModelError(String.Empty, $"Talie Fechado - Estufagem não permitida")
                End If
            End If

            If (Not ModelState.IsValid) Then
                ViewState("Sucesso") = False
                Exit Sub
            End If

            sc.PesoTotal = txtQuantidade.Text * sc.Bruto
            CalcularVolume()
            '      CarregarDadosNF()
            'atualiza o campo qtde estufada

            Sql = String.Empty

            Dim idSaidaCarga = Banco.ExecuteScalar($" exec redex.dbo.sp_nextval 'seq_SAIDA_CARGA' ")

            Sql = "INSERT INTO " & Banco.BancoRedex & "TB_SAIDA_CARGA (AUTONUM_SC, AUTONUM_PCS,"
            Sql = Sql & " QTDE_SAIDA,AUTONUM_EMB,PESO_BRUTO,COMPRIMENTO,LARGURA,ALTURA,VOLUME,autonum_patio,ID_CONTEINER,MERCADORIA"
            Sql = Sql & ",DATA_ESTUFAGEM,autonum_nfi,autonum_ro,autonum_talie,AUTONUM_RCS,CODPRODUTO) VALUES ("
            Sql = Sql & idSaidaCarga
            Sql = Sql & "," & sc.PatioCSId
            Sql = Sql & "," & txtQuantidade.Text
            Sql = Sql & "," & sc.EmbalagemId
            Sql = Sql & ",'" & PPonto(sc.Bruto) & "'"
            Sql = Sql & "," & PPonto(sc.Comprimento)
            Sql = Sql & "," & PPonto(sc.Largura)
            Sql = Sql & "," & PPonto(sc.Altura)
            Sql = Sql & "," & PPonto(sc.Volume)
            Sql = Sql & "," & hddnAutonumPatio.Value
            If hddnAutonumPatio.Value.ToInt() <> 0 Then
                Sql = Sql & ",'" & txtConteiner.Text & "'"
            Else
                Sql = Sql & ",NULL"
            End If
            Sql = Sql & "," & sc.ProdutoId
            Sql = Sql & ",convert(datetime,'" & txtInicio.Text.ToDateTime().DataHoraFormatada() & "',103)"
            Sql = Sql & "," & sc.NFId
            Sql = Sql & "," & hddnAutonumRomaneio.Value
            Sql = Sql & "," & hddnAutonumTalie.Value
            Sql = Sql & "," & sc.AutonumRCS
            Sql = Sql & " ,'" & txtOS.Text & "'"
            Sql = Sql & ")"

            Banco.ExecuteScalar(Sql)
            txtQuantidade.Text = String.Empty
            CarregaItensEstufados(hddnAutonumPatio.Value, hddnAutonumTalie.Value)
            Call Mostra_Estufagem()

            txtOS.Text = ""
            cbNFs.SelectedIndex = -1
            txtQuantidade.Text = ""
            txtReservaCarga.Text = ""

        Catch ex As Exception
            Log.InserirLog(ObterPagina(Page), ex.Message, ex.StackTrace, Page.Session("AUTONUMUSUARIO"))
            ModelState.AddModelError(String.Empty, $"Ocorreu um erro ao inserir carga")
            ViewState("Sucesso") = False
        End Try
    End Sub
    Private Sub ReabrirEstufagm()
        Try
            txtFIM.Text = "__/__/____ __:__"
            Sql = "update " & Banco.BancoRedex & "tb_talie set termino=null,flag_fechado=0  where autonum_patio=" & hddnAutonumPatio.Value
            Banco.ExecuteScalar(Sql)

            Sql = "update " & Banco.BancoRedex & "tb_patio set ef='E' where autonum_patio = " & hddnAutonumPatio.Value
            Banco.ExecuteScalar(Sql)
        Catch ex As Exception
            Log.InserirLog(ObterPagina(Page), ex.Message, ex.StackTrace, Page.Session("AUTONUMUSUARIO"))
            ModelState.AddModelError(String.Empty, $"Ocorreu um erro ao reabrir estufagem")
            ViewState("Sucesso") = False
        End Try
    End Sub

    Protected Sub OnConfirm(sender As Object, e As EventArgs)
        Dim confirmValue As String = Request.Form("confirm_value")
        If confirmValue = "Yes" Then
            ClientScript.RegisterStartupScript(Me.[GetType](), "alert", "alert('You clicked YES!')", True)
        Else
            ClientScript.RegisterStartupScript(Me.[GetType](), "alert", "alert('You clicked NO!')", True)
        End If
    End Sub
    Protected Sub Submit(ByVal sender As Object, ByVal e As EventArgs)
        ClientScript.RegisterStartupScript(Me.GetType(), "alert", "alert('Form submitted.');", True)
    End Sub
    <WebMethod>
    Public Shared Function Descarga(ByVal autonumNFI As String, sc As String, patio As String, qtdeSaida As String, codProduto As String) As String
        If (String.IsNullOrEmpty(autonumNFI)) Then
            Throw New Exception("Selecione um item na lista")
        End If

        Try
            Dim Sql As String = String.Empty
            Sql = "delete from " & Banco.BancoRedex & "TB_AMR_NF_SAIDA"
            Sql = Sql & " where"
            Sql = Sql & " autonum_nfi=" & autonumNFI
            Sql = Sql & " and autonum_patio=" & patio
            Banco.ExecuteScalar(Sql.ToUpper())

            Sql = "UPDATE " & Banco.BancoRedex & "TB_NOTAS_ITENS SET QTDE_ESTUFADA = qtde_estufada - " & qtdeSaida.ToInt() & " WHERE AUTONUM_NFI = " & autonumNFI
            Banco.ExecuteScalar(Sql.ToUpper())

            Sql = "delete from  " & Banco.BancoRedex & "tb_saida_carga where autonum_sc=" & sc
            Banco.ExecuteScalar(Sql.ToUpper())


            If CargaSuzano Then
                Sql = "update " & Banco.BancoRedex & "tb_integra_carga set estufado=0 where codbarra='" & codProduto & "'"
                Banco.ExecuteScalar(Sql.ToUpper())
            End If

            Sql = "update " & Banco.BancoRedex & "tb_patio set ef='E' where autonum_patio = " & patio
            Banco.ExecuteScalar(Sql.ToUpper())

            Return "Item Descarregado com sucesso!"
        Catch ex As Exception

            Log.InserirLog("Estufagem", ex.Message, ex.StackTrace, Nothing)
            Throw New Exception("Erro ao efetuar descarga")
        End Try

    End Function
    Private Sub btnHome_Click(sender As Object, e As EventArgs) Handles btnHome.Click
        Response.Redirect(Login.ObterURIMenu())
    End Sub

    Private Sub btnLogout_Click(sender As Object, e As EventArgs) Handles btnLogout.Click
        Response.Redirect(Login.ObterURIMenu() & "home/logout")
    End Sub

    Private Sub btnGravar_Click(sender As Object, e As EventArgs) Handles btnGravar.Click
        Try

            If hddnAutonumRomaneio.Value <= 0 Then
                ModelState.AddModelError(String.Empty, $"Informe um contêiner válido")
            End If

            If hddnAutonumBOO.Value <= 0 Then
                ModelState.AddModelError(String.Empty, $"Informe uma reserva")
            End If

            If cbConferente.SelectedValue <= 0 Then
                ModelState.AddModelError(String.Empty, $"Informe o conferente")
            End If

            If cbEquipe.SelectedValue <= 0 Then
                ModelState.AddModelError(String.Empty, $"Informe a equipe")
            End If

            If cbModo.SelectedIndex <= 0 Then
                ModelState.AddModelError(String.Empty, $"Informe o modo de operação")
            End If

            If (Not ModelState.IsValid) Then
                ViewState("Sucesso") = False
                Exit Sub
            End If

            If Not IsNumeric(hddnAutonumRomaneio.Value) Then Exit Sub
            Sql = "select ro.autonum_gate_saida, ro.crossdocking, ro.autonum_patio, ro.autonum_ro, ro.autonum_talie, t.autonum_talie as talie, T.AUTONUM_PATIO AS CNTR_TALIE"
            Sql = Sql & " ,RO.AUTONUM_BOO AS BOO_RO, T.AUTONUM_BOO AS BOO_TALIE, ISNULL(T.FLAG_FECHADO,0) as Fechado "
            Sql = Sql & " from REDEX.dbo.tb_romaneio ro"
            Sql = Sql & " left join REDEX.dbo.tb_talie t on ro.autonum_talie=t.autonum_talie "
            Sql = Sql & " where ro.autonum_ro = " & hddnAutonumRomaneio.Value

            Dim dtRomaneio = Banco.Consultar(Sql)

            If dtRomaneio IsNot Nothing Then
                If dtRomaneio.Rows.Count > 0 Then
                    Dim RS = dtRomaneio.Rows(0)
                    If RS("Autonum_Talie").ToString().ToInt() = 0 Then
                        Dim Autonum_Talie = 0

                        hddnAutonumRomaneio.Value = RS("autonum_ro").ToString
                        If RS("Autonum_Patio").ToString().ToInt() <> 0 Then
                            Sql = "SELECT COUNT(1) FROM REDEX.dbo.tb_TALIE WHERE AUTONUM_PATIO=" & RS("Autonum_Patio") & " AND FLAG_CARREGAMENTO=1"
                            If Banco.ExecuteScalar(Sql).ToInt() <> 0 Then
                                ModelState.AddModelError(String.Empty, $"Já existe um talie talie para este contêiner - Operação cancelada")
                                ViewState("Sucesso") = False
                                hddnAutonumRomaneio.Value = String.Empty
                                Exit Sub
                            End If
                        End If

                        Sql = "SELECT COUNT(*) FROM REDEX.dbo.tb_TALIE WHERE AUTONUM_ro=" & RS("Autonum_Ro")
                        If Banco.ExecuteScalar(Sql).ToInt() <> 0 Then
                            ModelState.AddModelError(String.Empty, $"Foi encontrado um talie para este romaneio - Operação cancelada")
                            ViewState("Sucesso") = False
                            hddnAutonumRomaneio.Value = String.Empty
                            Exit Sub
                        End If

                        Sql = "Insert into " & Banco.BancoRedex & "tb_talie (" 'autonum_talie,"
                        Sql = Sql & "autonum_patio,inicio,flag_estufagem"
                        Sql = Sql & ",crossdocking,autonum_boo,forma_operacao,conferente,equipe,flag_descarga,flag_carregamento,obs,autonum_ro,autonum_gate"
                        Sql = Sql & ") values (" '& TxtRegistro & ","
                        Sql = Sql & "" & RS("Autonum_Patio").ToString().ToInt()
                        If (String.IsNullOrEmpty(txtInicio.Text)) Then
                            Sql = Sql & ",getdate()"
                        Else
                            Sql = Sql & ",convert(datetime,'" & txtInicio.Text.ToDateTime().DataHoraFormatada() & "',103)"
                        End If
                        'Sql = Sql & ",convert(datetime,'" & txtFIM.Text.ToDateTime().DataHoraFormatada() & "',103)"
                        If Banco.ExecuteScalar("select flag_op_full_cntr from REDEX.dbo.tb_booking where autonum_boo = " & RS("BOO_RO")).ToString().ToInt() = 1 Then
                            Sql = Sql & ",0"
                        Else

                            Dim EF = Banco.ExecuteScalar("select ef from REDEX.dbo.tb_registro where autonum_ro = " & hddnAutonumRomaneio.Value)
                            EF = IIf(String.IsNullOrEmpty(EF), "F", EF)
                            If EF = "F" Then
                                Sql = Sql & ",1"
                            Else
                                Sql = Sql & ",0"
                            End If

                        End If
                        Sql = Sql & ",0"
                        Sql = Sql & "," & hddnAutonumBOO.Value
                        Sql = Sql & ",'" & cbModo.SelectedValue & "' "

                        'If cbModo.SelectedValue = "" Then Sql = Sql & ",''"
                        'If cbModo.SelectedValue = "Manual" Then Sql = Sql & ",'M'"
                        'If cbModo.SelectedValue = "Mecanizada" Then Sql = Sql & ",'A'"
                        'If cbModo.SelectedValue = "Parcial" Then Sql = Sql & ",'P'"

                        Sql = Sql & "," & cbConferente.SelectedValue
                        Sql = Sql & "," & cbEquipe.SelectedValue
                        Sql = Sql & ",0"
                        Sql = Sql & ",1"
                        Sql = Sql & ",''"
                        Sql = Sql & "," & RS("Autonum_Ro").ToString().ToInt()
                        Sql = Sql & "," & RS("AUTONUM_GATE_SAIDA").ToString().ToInt()
                        Sql = Sql & ")"

                        Banco.ExecuteScalar(UCase(Sql))

                        '                        Dim TxtRegistro = Banco.ExecuteScalar($"INSERT INTO SEQ_TB_TALIE (data) values (Convert(Char(10),GetDate(),121));  Select SCOPE_IDENTITY() As ID")
                        Sql = "SELECT IDENT_CURRENT('REDEX.DBO.TB_TALIE') As ID"
                        Dim TxtRegistro = Banco.ExecuteScalar(Sql)

                        hddnAutonumTalie.Value = TxtRegistro.ToString().ToInt()



                        Sql = $"SELECT INICIO FROM {Banco.BancoRedex}TB_TALIE WHERE AUTONUM_TALIE = {hddnAutonumTalie.Value}"
                        Dim dtTalie = Banco.Consultar(Sql)
                        If (dtTalie IsNot Nothing) Then
                            If dtTalie.Rows.Count > 0 Then
                                txtInicio.Text = dtTalie.Rows(0)("INICIO").ToString().DataHoraFormatada
                            End If
                        End If

                        Banco.ExecuteScalar("update REDEX.dbo.tb_romaneio set autonum_talie=" & TxtRegistro & " where autonum_ro = " & RS("Autonum_Ro").ToString().ToInt())

                        If RS("CrossDocking").ToString().ToInt = 1 Then
                            Sql = "update REDEX.dbo.tb_saida_carga set autonum_talie=" & TxtRegistro & ", autonum_ro=" & RS!Autonum_Ro & " where autonum_patio = " & RS!Autonum_Patio & " and isnull(autonum_talie,0)=0"
                            Banco.ExecuteScalar(Sql)

                            Sql = "UPDATE REDEX.dbo.tb_TALIE SET FLAG_ESTUFAGEM=1 WHERE AUTONUM_TALIE=" & TxtRegistro
                            Banco.ExecuteScalar(Sql)
                        End If

                        'Call DataCombo1_LostFocus(0)
                        pnlFinalizar.Visible = True
                                SetEstiloPnlLimpar()

                                ViewState("Sucesso") = True
                            Else
                                hddnAutonumTalie.Value = RS("AUTONUM_TALIE").ToString().ToInt()
                                If (RS("Fechado")) Then
                                    ModelState.AddModelError(String.Empty, $"Talie Fechado, alteração não permitida")
                                    ViewState("Sucesso") = False
                                    Exit Sub
                                End If

                                Sql = "update " & Banco.BancoRedex & "tb_talie set"
                                Sql = Sql & " inicio=" & IIf(String.IsNullOrEmpty(txtInicio.Text), "getdate() ", " convert(datetime,'" & txtInicio.Text.ToDateTime().DataFormatada() & "',103)")
                                Sql = Sql & ",termino=" & IIf(String.IsNullOrEmpty(txtFIM.Text), "null", "convert(datetime,'" & txtFIM.Text.ToDateTime().DataFormatada() & "',103)")

                                Sql = Sql & ",conferente=" & cbConferente.SelectedValue
                                Sql = Sql & ",equipe=" & cbEquipe.SelectedValue
                                Sql = Sql & ",obs=''"

                                Sql = Sql & ",autonum_gate =" & RS("AUTONUM_GATE_SAIDA").ToString().ToInt()
                                If Banco.ExecuteScalar("select flag_op_full_cntr from REDEX.dbo.tb_booking where autonum_boo = " & hddnAutonumBOO.Value).ToInt() = 1 Then
                                    Sql = Sql & ",flag_estufagem=0"
                                Else
                                    If Banco.ExecuteScalar("select flag_cntr from REDEX.dbo.tb_booking where autonum_boo = " & hddnAutonumBOO.Value).ToInt() = 1 Then
                                        If Banco.ExecuteScalar("select count(1) from REDEX.dbo.tb_registro where autonum_ro = " & hddnAutonumRomaneio.Value).ToInt() <> 0 Then
                                            Dim EF = Banco.ExecuteScalar("select ef from REDEX.dbo.tb_registro where autonum_ro = " & hddnAutonumRomaneio.Value)
                                            'EF = IIf(String.IsNullOrEmpty(EF), 1, EF)
                                            If EF = "" Then
                                                If Banco.ExecuteScalar("SELECT COUNT(1) FROM REDEX.dbo.tb_saida_carga where autonum_patio=" & hddnAutonumPatio.Value).ToInt() <> 0 Then
                                                    Sql = Sql & ",flag_estufagem=1"
                                                Else
                                                    Sql = Sql & ",flag_estufagem=0"
                                                End If
                                            Else
                                                If EF = "F" Then
                                                    Sql = Sql & ",flag_estufagem=1"
                                                Else
                                                    Sql = Sql & ",flag_estufagem=0"
                                                End If
                                            End If
                                        Else
                                            If hddnAutonumPatio.Value <> 0 Then
                                                Sql = Sql & ",flag_estufagem=1"
                                            Else
                                                Sql = Sql & ",flag_estufagem=0"
                                            End If
                                        End If
                                    Else
                                        Sql = Sql & ",flag_estufagem=0"
                                    End If
                                End If
                                Sql = Sql & ",forma_operacao='" & cbModo.SelectedValue & "' "
                                'If cbModo.SelectedValue = "" Then Sql = Sql & "''"
                                'If cbModo.SelectedValue = "Manual" Then Sql = Sql & "'M'"
                                'If cbModo.SelectedValue = "Mecanizada" Then Sql = Sql & "'A'"
                                'If cbModo.SelectedValue = "Parcial" Then Sql = Sql & "'P'"

                                Sql = Sql & " where autonum_talie = " & RS("Autonum_Talie")
                                Banco.ExecuteScalar(UCase(Sql))
                                ViewState("Sucesso") = True
                            End If
                        End If
                    End If
        Catch ex As Exception
            Log.InserirLog("Estufagem", ex.Message, ex.StackTrace, 0)
            ModelState.AddModelError(String.Empty, $"'{"Erro. Tente novamente."}' ")
            ViewState("Sucesso") = False
            'Throw New Exception("Erro. Tente novamente." & ex.Message())
        End Try
    End Sub

    Private Sub LimparCampos()
        txtReserva.Text = String.Empty
        Me.txtCliente.Text = String.Empty
        txtInicio.Text = String.Empty
        txtFIM.Text = String.Empty
        cbConferente.SelectedIndex = 0
        cbEquipe.SelectedIndex = 0
        cbModo.SelectedIndex = 0
        hddnAutonumPatio.Value = String.Empty
        hddnAutonumBOO.Value = String.Empty
        hddnRegistro.Value = String.Empty
        hddnAutonumTalie.Value = String.Empty
        hddnAutonumRomaneio.Value = String.Empty
        hddnAutonumCliente.Value = String.Empty
    End Sub

    <WebMethod>
    Public Shared Function ValidarFechamento(talie As String, autonumRomaneio As String, autonumPatio As String, autonumBOO As String) As Integer

        Try

            Dim Motivo As String
            Dim Erro As Byte

            If talie = 0 Then
                Throw New Exception("Fechamento disponível somente após o lançamento da estufagem!")

            End If

            If autonumRomaneio = 0 Then
                Throw New Exception("Falha na busca do planejamento")

            End If

            Dim Sql = "select autonum_talie from REDEX.dbo.tb_romaneio where autonum_ro=" & autonumRomaneio

            If talie <> Banco.ExecuteScalar(Sql).ToInt() Then
                Throw New Exception("Já existe um talie para o romaneio selecionado - Operação cancelada")
                Exit Function
            End If

            Sql = "select autonum_ro from REDEX.dbo.tb_talie where autonum_talie=" & talie
            If autonumRomaneio <> Banco.ExecuteScalar(Sql).ToInt() Then
                Throw New Exception("Já existe um romaneio para o talie selecionado - Operação cancelada")
            End If

            'CONSISTE AS NOTAS FISCAIS
            Sql = "select count(1)"
            Sql = Sql & " from REDEX.dbo.tb_saida_carga sc"
            Sql = Sql & " inner join REDEX.dbo.tb_patio_cs pcs on sc.autonum_pcs = pcs.autonum_pcs"
            Sql = Sql & " inner join REDEX.dbo.tb_notas_fiscais nf on nf.autonum_nf = pcs.autonum_nf"
            Sql = Sql & " left join REDEX.dbo.tb_notas_itens nfi on nfi.autonum_nf = nf.autonum_nf"
            Sql = Sql & " where sc.autonum_ro = " & autonumRomaneio
            Sql = Sql & " and ISNULL(nfi.autonum_nf,0)=0"
            Sql = UCase(Sql)

            If Banco.ExecuteScalar(Sql).ToInt() <> 0 Then
                Throw New Exception("Carregamento / Estufagem possui NF sem item cadastrado")
            End If

            Dim RS = Banco.Consultar("select autonum_patio,flag_CARREGAMENTO, AUTONUM_BOO, flag_fechado, FLAG_ESTUFAGEM from REDEX.dbo.tb_talie where autonum_talie = " & talie)

            If RS Is Nothing Then

                Throw New Exception("Talie não Criado - Processo cancelado")

            End If
            If (RS.Rows.Count > 0) Then
                Return RS.Rows(0)("flag_fechado").ToString().ToInt()
            Else
                Throw New Exception("Talie não Criado - Processo cancelado")
            End If

        Catch ex As Exception
            Log.InserirLog("Estufagem", ex.Message, ex.StackTrace, 0)
            Throw New Exception(ex.Message())
        End Try

    End Function
    <WebMethod>
    Public Shared Function GerarCancelamento(talie As String, autonumRomaneio As String) As String

        Try

            Banco.ExecuteScalar("update REDEX.dbo.tb_talie set flag_fechado=0 where autonum_talie=" & talie)
            Banco.ExecuteScalar("update REDEX.dbo.tb_romaneio set flag_historico=0 where autonum_ro=" & autonumRomaneio)
            Banco.ExecuteScalar("DELETE FROM REDEX.dbo.tb_SAIDA_CARGA WHERE AUTONUM_TALIE=" & talie)

            Return "Cancelamento Gerado"

        Catch ex As Exception
            Log.InserirLog("Estufagem", ex.Message, ex.StackTrace, 0)
            Throw New Exception("Erro. Tente novamente." & ex.Message())
        End Try

    End Function
    <WebMethod>
    Public Shared Function ValidarFechamentoParte2(talie As Integer, autonumRomaneio As Integer, autonumPatio As Integer) As String

        Try

            Dim Motivo As String
            Dim Erro As Byte
            Dim sql As String

            Dim RS = Banco.Consultar("select autonum_patio,flag_CARREGAMENTO, AUTONUM_BOO, flag_fechado, FLAG_ESTUFAGEM from REDEX.dbo.tb_talie where autonum_talie = " & talie)

            If RS IsNot Nothing Then
                If RS.Rows.Count > 0 Then
                    If RS.Rows(0)("Autonum_Patio").ToString().ToInt() <> 0 Then
                        Dim Lacre As String
                        sql = "select lacre from REDEX.dbo.tb_patio_lacres where autonum_patio=" & autonumPatio & " and flag_ativo=1"
                        Lacre = Banco.ExecuteScalar(sql)
                        If Lacre = "" Then
                            sql = "select count(1) from REDEX.dbo.tb_patio where autonum_patio=" & autonumPatio & " and autonum_tpc in ('FR','OT')"
                            If Banco.ExecuteScalar(sql).ToInt() = 0 Then
                                Throw New Exception("Não consta Lacre para o conteiner estufado - Verifique controle de lacres")
                            End If
                        End If

                        Dim SQL1 = UCase("select sum(qtde_saida) from REDEX.dbo.tb_saida_carga where autonum_patio=" & RS.Rows(0)("Autonum_Patio").ToString().ToInt())
                        Dim SQL2 = UCase("select sum(qtde) from REDEX.dbo.tb_romaneio_cs where autonum_ro=" & autonumRomaneio)
                        If Banco.ExecuteScalar(SQL1).ToInt() <> Banco.ExecuteScalar(SQL2).ToInt() Then
                            Throw New Exception("Divergencia na quantidade planejada e quantidade estufada. Fechamento cancelado")
                        End If
                    End If

                    If RS.Rows(0)("Autonum_Patio").ToString().ToInt() <> 0 And RS.Rows(0)("flag_carregamento").ToString().ToInt() = 1 And RS.Rows(0)("flag_ESTUFAGEM").ToString().ToInt() = 1 Then
                        sql = $"SELECT 
                        count(1)
                    FROM   
	                    {Banco.BancoRedex}tb_saida_carga A
                    INNER JOIN
                        {Banco.BancoRedex}tb_patio_cs B ON A.autonum_pcs = B.autonum_pcs
                    INNER JOIN       
	                    {Banco.BancoRedex}tb_booking_carga C ON B.autonum_bcg = C.autonum_bcg 
                    INNER JOIN
	                    {Banco.BancoRedex}tb_booking D ON C.autonum_boo = D.autonum_boo 
                    LEFT JOIN
	                    {Banco.BancoRedex}tb_cad_embalagens E ON B.autonum_emb = E.autonum_emb 
                    LEFT JOIN
	                    {Banco.BancoRedex}tb_cad_produtos F ON B.autonum_pro = f.autonum_pro 
                    INNER JOIN
	                    {Banco.BancoRedex}tb_cad_parceiros G ON D.autonum_parceiro = G.autonum 
                    INNER JOIN
	                    {Banco.BancoRedex}tb_viagens i ON D.autonum_via = i.autonum_via 
                    INNER JOIN
	                    {Banco.BancoRedex}tb_cad_navios h ON i.autonum_nav = h.autonum_nav
                    INNER JOIN
	                    {Banco.BancoRedex}tb_notas_fiscais NF ON B.autonum_nf = NF.autonum_nf 
                    RIGHT JOIN
	                    {Banco.BancoRedex}tb_patio CC ON A.autonum_patio = CC.autonum_patio
                    WHERE   
	                    a.autonum_patio = {autonumPatio} AND a.autonum_talie = {talie} "
                        If Banco.ExecuteScalar(sql).ToInt() = 0 Then
                            Throw New Exception("Não foi encontrado registro de saida")
                        End If

                    End If
                End If
            End If
            Return String.Empty
        Catch ex As Exception
            Log.InserirLog("Estufagem", ex.Message, ex.StackTrace, 0)
            Throw New Exception(ex.Message())
        End Try

    End Function
    <WebMethod>
    Public Shared Function ConfirmaFechamento(talie As String, autonumRomaneio As String, autonumPatio As String) As Integer

        Using connection As SqlConnection = New SqlConnection(Banco.ConnectionString())
            connection.Open()
            Dim command As SqlCommand = connection.CreateCommand()
            Dim transaction As SqlTransaction
            transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted)
            command.Transaction = transaction

            Try

                command.CommandText = $"update REDEX.dbo.tb_talie set flag_fechado=1, flag_pacotes=1, termino = getdate() where autonum_talie={talie}"
                command.ExecuteNonQuery()

                command.CommandText = $"update REDEX.dbo.tb_romaneio set flag_historico=1   where autonum_ro={autonumRomaneio}"
                command.ExecuteNonQuery()

                transaction.Commit()

            Catch ex As Exception
                transaction.Rollback()
                Log.InserirLog("Estufagem", ex.Message, ex.StackTrace, 0)
                Throw New Exception($"Erro durante processo de Fechamento.{vbCr}Processo Cancelado!")
            End Try
        End Using

    End Function


    Sub LimparCamposCarga()
        cbNFs.Items.Clear()
        txtQuantidade.Text = String.Empty
        txtReservaCarga.Text = String.Empty
    End Sub

    'Private Sub cbNFs_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbNFs.SelectedIndexChanged
    '    Try

    '        If cbNFs.SelectedValue > 0 Then
    '            Dim OS = $"{txtOS.Text.Substring(1, 5)}/{txtOS.Text.Substring(6, 2)}"
    '            Sql = $"SELECT 
    '                        BOO.OS,
    '                        CC.ID_CONTEINER, 
    '                        BOO.AUTONUM_BOO,
    '                        BOO.REFERENCE AS RESERVA, 
    '                        NF.NUM_NF, ISNULL(SC.QTDE_SAIDA,0) QTDE_SAIDA, 
    '                        RCS.QTDE, 
    '                        RCS.QTDE - ISNULL(SC.QTDE_SAIDA,0) AS SALDO,
    '                        PCS.AUTONUM_PCS,
    '                        PCS.AUTONUM_EMB,
    '                        PCS.BRUTO, 
    '                        PCS.COMPRIMENTO,
    '                        PCS.LARGURA, 
    '                        PCS.ALTURA, 
    '                        PCS.AUTONUM_PRO,
    '                        RCS.AUTONUM_RCS,
    '                        PCS.CODPRODUTO
    '                    FROM 
    '                        TB_ROMANEIO RO
    '                    INNER JOIN 
    '                        TB_ROMANEIO_CS RCS ON RO.AUTONUM_RO=RCS.AUTONUM_RO
    '                    INNER JOIN 
    '                        TB_PATIO CC ON RO.AUTONUM_PATIO=CC.AUTONUM_PATIO
    '                    INNER JOIN 
    '                        TB_PATIO_CS PCS ON RCS.AUTONUM_PCS = PCS.AUTONUM_PCS
    '                    INNER JOIN 
    '                        TB_BOOKING_CARGA BCG ON PCS.AUTONUM_BCG = BCG.AUTONUM_BCG
    '                    INNER JOIN 
    '                        TB_BOOKING BOO ON BCG.AUTONUM_BOO = BOO.AUTONUM_BOO
    '                    INNER JOIN 
    '                        TB_NOTAS_FISCAIS NF ON PCS.AUTONUM_NF = NF.AUTONUM_NF
    '                    LEFT JOIN 
    '                        (SELECT AUTONUM_PCS,SUM(QTDE_SAIDA) AS QTDE_SAIDA FROM TB_SAIDA_CARGA GROUP BY AUTONUM_PCS) SC ON PCS.AUTONUM_PCS=SC.AUTONUM_PCS
    '                    WHERE 
    '                        ISNULL(RO.AUTONUM_PATIO,0)<>0 AND  BOO.OS = {OS} AND CC.ID_CONTEINER='{txtConteiner.Text}' and NF.AUTONUM_NF = {cbNFs.SelectedValue}
    '                    ORDER BY
    '                        RO.AUTONUM_RO DESC"

    '            Dim dtCarga = Banco.Consultar(Sql)
    '            If (dtCarga IsNot Nothing) Then
    '                If dtCarga.Rows.Count > 0 Then
    '                    sc = New SaidaCarga()
    '                    txtQuantidade.Text = dtCarga.Rows(0)("SALDO").ToString().ToInt()
    '                    txtReservaCarga.Text = dtCarga.Rows(0)("RESERVA").ToString()

    '                    sc.PatioCSId = dtCarga.Rows(0)("AUTONUM_PCS").ToString()
    '                    sc.Saldo = dtCarga.Rows(0)("SALDO").ToString().ToInt()
    '                    sc.Reserva = dtCarga.Rows(0)("RESERVA").ToString()
    '                    sc.EmbalagemId = dtCarga.Rows(0)("AUTONUM_EMB").ToString().ToInt()
    '                    sc.Bruto = dtCarga.Rows(0)("bruto").ToString().ToDecimal()
    '                    sc.Comprimento = dtCarga.Rows(0)("Comprimento").ToString().ToDecimal()
    '                    sc.Largura = dtCarga.Rows(0)("Largura").Value.ToString().ToDecimal()
    '                    sc.Altura = dtCarga.Rows(0)("Altura").Value.ToString().ToDecimal()
    '                    sc.ProdutoId = dtCarga.Rows(0)("AUTONUM_PRO").ToString().ToInt()
    '                    sc.AutonumRCS = dtCarga.Rows(0)("AUTONUM_RCS").ToString().ToInt()
    '                    sc.CodProduto = dtCarga.Rows(0)("CODPRODUTO").ToString().ToInt()
    '                    sc.PesoTotal = dtCarga.Rows(0)("saldo").ToString().ToDecimal() * dtCarga.Rows(0)("bruto").ToString().ToDecimal()
    '                    CalcularVolume()

    '                Else
    '                    ModelState.AddModelError(String.Empty, $"NF(s) não encontrada(s) para esta OS e este Contêiner")
    '                    ViewState("Sucesso") = False
    '                    Exit Sub
    '                End If
    '            End If

    '        End If
    '    Catch ex As Exception
    '        Log.InserirLog(ObterPagina(Page), ex.Message, ex.StackTrace, Page.Session("AUTONUMUSUARIO"))
    '        ModelState.AddModelError(String.Empty, $"Ocorreu um erro ao selecionar contêiner")
    '        ViewState("Sucesso") = False
    '    End Try
    'End Sub

    Protected Sub txtConteiner_TextChanged(sender As Object, e As EventArgs) Handles txtConteiner.TextChanged
        CarregarDadosConteiner(txtConteiner.Text)
    End Sub

    Private Sub CarregarDadosConteiner(conteiner As String)
        Try
            LimparCampos()
            Sql = String.Empty
            If String.IsNullOrEmpty(conteiner) Then
                ModelState.AddModelError(String.Empty, $"Informe um contêiner")
                ViewState("Sucesso") = False
                txtConteiner.Focus()
                Exit Sub
            End If

            'Carregar as Reservas
            Sql = $"SELECT DISTINCT 
                        BOO.AUTONUM_BOO,
                        BOO.REFERENCE AS RESERVA,
                        RO.AUTONUM_PATIO,
                        RO.AUTONUM_RO
                    FROM 
	                    {Banco.BancoRedex}TB_ROMANEIO RO
                    INNER JOIN 
                        TB_PATIO CC ON RO.AUTONUM_PATIO=CC.AUTONUM_PATIO
                    INNER JOIN 
                        TB_BOOKING BOO ON RO.AUTONUM_BOO = BOO.AUTONUM_BOO
                    INNER JOIN 
                            TB_BOOKING_CARGA BCGC ON CC.AUTONUM_BCG = BCGC.AUTONUM_BCG
                    INNER JOIN 
                            TB_BOOKING BOOCONTEINER ON BCGC.AUTONUM_BOO = BOOCONTEINER.AUTONUM_BOO
                    WHERE 
                        CC.ID_CONTEINER='{conteiner}'"

            Dim dtReserva = Banco.Consultar(Sql)

            If (dtReserva IsNot Nothing) Then
                If dtReserva.Rows.Count > 0 Then
                    txtReserva.Text = dtReserva.Rows(0)("RESERVA")
                    hddnAutonumBOO.Value = dtReserva.Rows(0)("AUTONUM_BOO")
                    hddnAutonumPatio.Value = dtReserva.Rows(0)("AUTONUM_PATIO")
                    hddnAutonumRomaneio.Value = dtReserva.Rows(0)("AUTONUM_RO")
                    hddnAutonumTalie.Value = Banco.ExecuteScalar($"SELECT AUTONUM_TALIE FROM {Banco.BancoRedex}TB_TALIE WHERE AUTONUM_PATIO={hddnAutonumPatio.Value}").ToInt()

                    Sql = $"SELECT 
                                AUTONUM, 
                                FANTASIA 
                            FROM
                                {Banco.BancoRedex}TB_BOOKING BOO 
                            INNER JOIN 
                                {Banco.BancoRedex}tb_cad_parceiros CP ON BOO.AUTONUM_PARCEIRO=CP.AUTONUM
                            WHERE 
                                BOO.REFERENCE='{txtReserva.Text}'"

                    Dim dtCliente = Banco.List(Sql.ToString())
                    If (dtCliente IsNot Nothing) Then
                        If (dtCliente.Rows.Count > 0) Then
                            txtCliente.Text = dtCliente.Rows(0)("FANTASIA")
                            hddnAutonumCliente.Value = dtCliente.Rows(0)("AUTONUM")
                        End If
                    End If

                    'Verificar se talie já existe e carregar os dados
                    Sql = "SELECT COUNT(1) FROM REDEX.dbo.tb_TALIE WHERE AUTONUM_PATIO=" & hddnAutonumPatio.Value & " AND FLAG_CARREGAMENTO=1"
                    If (Banco.ExecuteScalar(Sql).ToInt() > 0) Then
                        CarregarDadosTalie()

                        '   CarregaItensEstufados(hddnAutonumPatio.Value, hddnAutonumTalie.Value)
                        '  Call Mostra_Estufagem()

                    End If

                Else
                    ModelState.AddModelError(String.Empty, $"Contêiner não encontrado")
                    ViewState("Sucesso") = False
                    Exit Sub
                End If
            End If

        Catch ex As Exception
            Log.InserirLog(ObterPagina(Page), ex.Message, ex.StackTrace, Page.Session("AUTONUMUSUARIO"))
            ModelState.AddModelError(String.Empty, $"Ocorreu um erro ao buscar a reserva")
            ViewState("Sucesso") = False
        End Try
    End Sub

    Private Sub CarregarDadosTalie()
        Try
            'Carregar as Reservas
            Sql = $"SELECT 
	                    INICIO,
	                    TERMINO,
	                    CONFERENTE,
	                    EQUIPE,
	                    FORMA_OPERACAO,
                        ISNULL(FLAG_FECHADO,0) as Fechado
                    FROM 
	                    {Banco.BancoRedex}TB_TALIE
                    WHERE 
                        AUTONUM_TALIE = {hddnAutonumTalie.Value}"

            Dim dtTalie = Banco.Consultar(Sql)

            If (dtTalie IsNot Nothing) Then
                If dtTalie.Rows.Count > 0 Then
                    txtInicio.Text = dtTalie.Rows(0)("INICIO").ToString().DataHoraFormatada
                    txtFIM.Text = dtTalie.Rows(0)("TERMINO").ToString.DataHoraFormatada
                    cbConferente.SelectedIndex = cbConferente.Items.IndexOf(cbConferente.Items.FindByValue(dtTalie.Rows(0)("CONFERENTE").ToString()))
                    cbEquipe.SelectedIndex = cbEquipe.Items.IndexOf(cbEquipe.Items.FindByValue(dtTalie.Rows(0)("EQUIPE").ToString()))
                    cbModo.SelectedIndex = cbModo.Items.IndexOf(cbModo.Items.FindByValue(dtTalie.Rows(0)("FORMA_OPERACAO").ToString()))

                    pnlFinalizar.Visible = Not dtTalie.Rows(0)("Fechado").ToString() = 1
                    pnlGravar.Visible = Not dtTalie.Rows(0)("Fechado").ToString() = 1
                    SetEstiloPnlLimpar()
                    CarregaItensEstufados(hddnAutonumPatio.Value, hddnAutonumTalie.Value)
                Else
                    ModelState.AddModelError(String.Empty, $"Contêiner não encontrado")
                    ViewState("Sucesso") = False
                    Exit Sub
                End If
            End If

        Catch ex As Exception
            Log.InserirLog(ObterPagina(Page), ex.Message, ex.StackTrace, Page.Session("AUTONUMUSUARIO"))
            ModelState.AddModelError(String.Empty, $"Ocorreu um erro ao buscar a reserva")
            ViewState("Sucesso") = False
        End Try
    End Sub

    Private Sub SetEstiloPnlLimpar()
        Dim estilo = "form-group col-sm-12 col-md-2 col-lg-2"
        pnlLimpar.CssClass = pnlLimpar.CssClass.Replace("offset-md-6 offset-lg-6", "")
        pnlLimpar.CssClass = pnlLimpar.CssClass.Replace("offset-md-8 offset-lg-8", "")
        pnlLimpar.CssClass = pnlLimpar.CssClass.Replace("offset-md-10 offset-lg-10", "")
        If (pnlFinalizar.Visible And pnlGravar.Visible) Then
            estilo += "offset-md-6 offset-lg-6"
        ElseIf pnlFinalizar.Visible = False And pnlGravar.Visible Then
            estilo += "offset-md-8 offset-lg-8"
        Else
            estilo += "offset-md-10 offset-lg-10"
        End If

        pnlLimpar.Attributes.Add("class", estilo)
    End Sub

    Protected Sub txtOS_TextChanged(sender As Object, e As EventArgs) Handles txtOS.TextChanged
        Try
            LimparCamposCarga()
            Sql = String.Empty
            If String.IsNullOrEmpty(txtOS.Text) Then
                Exit Sub
            End If

            'If txtOS.Text.Length < 8 Then
            '    ModelState.AddModelError(String.Empty, $"Informe um número de OS válida")
            '    ViewState("Sucesso") = False
            '    Exit Sub
            'End If

            '            Dim OS = $"{txtOS.Text.Substring(0, 5)}/{txtOS.Text.Substring(6, 2)}"
            Dim OS As String = txtOS.Text

            'Carregar as NFs
            Sql = $"SELECT 
--                        NF.AUTONUM_NF, 
--                        NF.NUM_NF
                    rcs.autonum_rcs,
                    NF.NUM_NF + '    Qtde: ' + cast(RCS.QTDE as varchar) + '   (' + cast(PCS.COMPRIMENTO as varchar) + ' x ' + cast(PCS.LARGURA as varchar) + ' x ' + cast(PCS.ALTURA as varchar) + ' )' AS DISPLAY
                    FROM 
                        TB_ROMANEIO RO
                    INNER JOIN 
                        TB_ROMANEIO_CS RCS ON RO.AUTONUM_RO=RCS.AUTONUM_RO
                    INNER JOIN 
                        TB_PATIO CC ON RO.AUTONUM_PATIO=CC.AUTONUM_PATIO
                    INNER JOIN 
                        TB_PATIO_CS PCS ON RCS.AUTONUM_PCS = PCS.AUTONUM_PCS
                    INNER JOIN 
                        TB_BOOKING_CARGA BCG ON PCS.AUTONUM_BCG = BCG.AUTONUM_BCG
                    INNER JOIN 
                        TB_BOOKING BOO ON BCG.AUTONUM_BOO = BOO.AUTONUM_BOO
                    INNER JOIN 
                        TB_NOTAS_FISCAIS NF ON PCS.AUTONUM_NF = NF.AUTONUM_NF
                    WHERE 
--                        ISNULL(RO.AUTONUM_PATIO,0)<>0 AND  BOO.OS = '{OS}' AND CC.ID_CONTEINER='{txtConteiner.Text}' 
                        RO.AUTONUM_RO={hddnAutonumRomaneio.Value}
                        AND NF.NUM_NF='{OS}'

                    ORDER BY 
                        RO.AUTONUM_RO DESC"

            Dim dtNFs = Banco.Consultar(Sql)

            If (dtNFs IsNot Nothing) Then
                If dtNFs.Rows.Count > 0 Then

                    Me.cbNFs.Items.Clear()
                    Me.cbNFs.DataTextField = "display"
                    Me.cbNFs.DataValueField = "AUTONUM_rcs"
                    Me.cbNFs.DataSource = Banco.List(Sql.ToString())
                    Me.cbNFs.DataBind()
                    cbNFs.SelectedIndex = 0
                    CarregarDadosNF()

                Else
                    ModelState.AddModelError(String.Empty, $"NF(s) não encontrada(s) para esta OS e este Contêiner")
                    ViewState("Sucesso") = False
                    Exit Sub
                End If
            End If

        Catch ex As Exception
            Log.InserirLog(ObterPagina(Page), ex.Message, ex.StackTrace, Page.Session("AUTONUMUSUARIO"))
            ModelState.AddModelError(String.Empty, $"{"Ocorreu um erro ao buscar a OS"}")
            ViewState("Sucesso") = False
        End Try
    End Sub

    Protected Sub cbNFs_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbNFs.SelectedIndexChanged
        CarregarDadosNF()
    End Sub
    Private Sub CarregarDadosNF()
        Try

            If cbNFs.SelectedValue > 0 Then
                Dim OS = $"{txtOS.Text.Substring(0, 5)}/{txtOS.Text.Substring(6, 2)}"
                Sql = $"SELECT 
                                BOO.OS,
                                CC.ID_CONTEINER, 
                                BOO.AUTONUM_BOO,
                                BOO.REFERENCE AS RESERVA, 
                                nf.autonum_nf, NF.NUM_NF, ISNULL(SC.QTDE_SAIDA,0) QTDE_SAIDA, 
                                RCS.QTDE, 
                                RCS.QTDE - ISNULL(SC.QTDE_SAIDA,0) AS SALDO,
                                PCS.AUTONUM_PCS,
                                PCS.AUTONUM_EMB,
                                PCS.BRUTO, 
                                PCS.COMPRIMENTO,
                                PCS.LARGURA, 
                                PCS.ALTURA, 
                                PCS.AUTONUM_PRO,
                                RCS.AUTONUM_RCS,
                                PCS.CODPRODUTO
                            FROM 
                                TB_ROMANEIO RO
                            INNER JOIN 
                                TB_ROMANEIO_CS RCS ON RO.AUTONUM_RO=RCS.AUTONUM_RO
                            INNER JOIN 
                                TB_PATIO CC ON RO.AUTONUM_PATIO=CC.AUTONUM_PATIO
                            INNER JOIN 
                                TB_PATIO_CS PCS ON RCS.AUTONUM_PCS = PCS.AUTONUM_PCS
                            INNER JOIN 
                                TB_BOOKING_CARGA BCG ON PCS.AUTONUM_BCG = BCG.AUTONUM_BCG
                            INNER JOIN 
                                TB_BOOKING BOO ON BCG.AUTONUM_BOO = BOO.AUTONUM_BOO
                            INNER JOIN 
                                TB_NOTAS_FISCAIS NF ON PCS.AUTONUM_NF = NF.AUTONUM_NF
                            LEFT JOIN 
                                (SELECT AUTONUM_PCS,SUM(QTDE_SAIDA) AS QTDE_SAIDA FROM TB_SAIDA_CARGA GROUP BY AUTONUM_PCS) SC ON PCS.AUTONUM_PCS=SC.AUTONUM_PCS
                            WHERE 
                                rcs.autonum_rcs= {cbNFs.SelectedValue}
                            ORDER BY
                                RO.AUTONUM_RO DESC"

                '  ISNULL(RO.AUTONUM_PATIO,0)<>0 AND  BOO.OS = '{OS}' AND CC.ID_CONTEINER='{txtConteiner.Text}' and NF.AUTONUM_NF = {cbNFs.SelectedValue}


                Dim dtCarga = Banco.Consultar(Sql)
                If (dtCarga IsNot Nothing) Then
                    If dtCarga.Rows.Count > 0 Then
'                        txtQuantidade.Text = dtCarga.Rows(0)("SALDO").ToString().ToInt()
                        txtQuantidade.Text = 1
                        txtReservaCarga.Text = dtCarga.Rows(0)("RESERVA").ToString()

                        sc = New SaidaCarga()
                        sc.PatioCSId = dtCarga.Rows(0)("AUTONUM_PCS").ToString()
                        sc.Saldo = dtCarga.Rows(0)("SALDO").ToString().ToInt()
                        sc.Reserva = dtCarga.Rows(0)("RESERVA").ToString()
                        sc.EmbalagemId = dtCarga.Rows(0)("AUTONUM_EMB").ToString().ToInt()
                        sc.Bruto = dtCarga.Rows(0)("bruto").ToString().ToDecimal()
                        sc.Comprimento = dtCarga.Rows(0)("Comprimento").ToString().ToDecimal()
                        sc.Largura = dtCarga.Rows(0)("LARGURA").ToString().ToDecimal()
                        sc.Altura = dtCarga.Rows(0)("ALTURA").ToString().ToDecimal()
                        sc.ProdutoId = dtCarga.Rows(0)("AUTONUM_PRO").ToString().ToInt()
                        sc.AutonumRCS = dtCarga.Rows(0)("AUTONUM_RCS").ToString().ToInt()
                        sc.CodProduto = dtCarga.Rows(0)("CODPRODUTO").ToString().ToInt()
                        sc.PesoTotal = dtCarga.Rows(0)("saldo").ToString().ToDecimal() * dtCarga.Rows(0)("bruto").ToString().ToDecimal()
                        sc.NFId = dtCarga.Rows(0)("autonum_nf").ToString().ToInt()
                        CalcularVolume()

                    Else
                        ModelState.AddModelError(String.Empty, $"Carga não encontrada para esta NF")
                        ViewState("Sucesso") = False
                        Exit Sub
                    End If
                End If

            End If
        Catch ex As Exception
            Log.InserirLog(ObterPagina(Page), ex.Message, ex.StackTrace, Page.Session("AUTONUMUSUARIO"))
            ModelState.AddModelError(String.Empty, $"Ocorreu um erro ao selecionar contêiner")
            ViewState("Sucesso") = False
        End Try
    End Sub

    Private Function ObterSaldoAtualizado() As Integer
        If cbNFs.SelectedValue > 0 Then
            Dim OS = $"{txtOS.Text.Substring(0, 5)}/{txtOS.Text.Substring(6, 2)}"
            Sql = $"SELECT 
                        RCS.QTDE - ISNULL(SC.QTDE_SAIDA,0) AS SALDO
                    FROM 
                        TB_ROMANEIO RO
                    INNER JOIN 
                        TB_ROMANEIO_CS RCS ON RO.AUTONUM_RO=RCS.AUTONUM_RO
                    INNER JOIN 
                        TB_PATIO CC ON RO.AUTONUM_PATIO=CC.AUTONUM_PATIO
                    INNER JOIN 
                        TB_PATIO_CS PCS ON RCS.AUTONUM_PCS = PCS.AUTONUM_PCS
                    INNER JOIN 
                        TB_BOOKING_CARGA BCG ON PCS.AUTONUM_BCG = BCG.AUTONUM_BCG
                    INNER JOIN 
                        TB_BOOKING BOO ON BCG.AUTONUM_BOO = BOO.AUTONUM_BOO
                    INNER JOIN 
                        TB_NOTAS_FISCAIS NF ON PCS.AUTONUM_NF = NF.AUTONUM_NF
                    LEFT JOIN 
                        (SELECT AUTONUM_PCS,SUM(QTDE_SAIDA) AS QTDE_SAIDA FROM TB_SAIDA_CARGA GROUP BY AUTONUM_PCS) SC ON PCS.AUTONUM_PCS=SC.AUTONUM_PCS
                    WHERE 
                       RCS.AUTONUM_RCS = {cbNFs.SelectedValue}
                    ORDER BY
                        RO.AUTONUM_RO DESC"

            '                        ISNULL(RO.AUTONUM_PATIO,0)<>0 AND  BOO.OS = '{OS}' AND CC.ID_CONTEINER='{txtConteiner.Text}' and NF.AUTONUM_NF = {cbNFs.SelectedValue}


            Return Banco.ExecuteScalar(Sql).ToInt()

        End If
    End Function
    Private Function ObterPCS() As Long
        If cbNFs.SelectedValue > 0 Then
            Dim OS = $"{txtOS.Text.Substring(0, 5)}/{txtOS.Text.Substring(6, 2)}"
            Sql = $"SELECT 
                        PCS.AUTONUM_PCS
                    FROM 
                        TB_ROMANEIO RO
                    INNER JOIN 
                        TB_ROMANEIO_CS RCS ON RO.AUTONUM_RO=RCS.AUTONUM_RO
                    INNER JOIN 
                        TB_PATIO CC ON RO.AUTONUM_PATIO=CC.AUTONUM_PATIO
                    INNER JOIN 
                        TB_PATIO_CS PCS ON RCS.AUTONUM_PCS = PCS.AUTONUM_PCS
                    INNER JOIN 
                        TB_BOOKING_CARGA BCG ON PCS.AUTONUM_BCG = BCG.AUTONUM_BCG
                    INNER JOIN 
                        TB_BOOKING BOO ON BCG.AUTONUM_BOO = BOO.AUTONUM_BOO
                    INNER JOIN 
                        TB_NOTAS_FISCAIS NF ON PCS.AUTONUM_NF = NF.AUTONUM_NF
                    WHERE 
                        ISNULL(RO.AUTONUM_PATIO,0)<>0 AND  BOO.OS = '{OS}' AND CC.ID_CONTEINER='{txtConteiner.Text}' and NF.AUTONUM_NF = {cbNFs.SelectedValue}
                    "

            Return Convert.ToInt64(Banco.ExecuteScalar(Sql))

        End If
    End Function

    Protected Sub grdCargaEstufada_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdCargaEstufada.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then

            e.Row.TableSection = TableRowSection.TableHeader
        End If

        If e.Row.RowType = DataControlRowType.DataRow Then

            Dim ID = Me.grdCargaEstufada.DataKeys(e.Row.RowIndex).Value.ToString().ToInt()

        End If
    End Sub
    Private Sub MakeGridViewPrinterFriendly(gridView As GridView)
        If gridView.Rows.Count > 0 Then
            gridView.UseAccessibleHeader = True
            gridView.HeaderRow.TableSection = TableRowSection.TableHeader
        End If
    End Sub

    Private Sub TalieColetorDescarga_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        MakeGridViewPrinterFriendly(grdCargaEstufada)
    End Sub

    Private Sub pnlFinalizar_Load(sender As Object, e As EventArgs) Handles pnlFinalizar.Load

    End Sub

    Protected Sub txtQuantidade_TextChanged(sender As Object, e As EventArgs) Handles txtQuantidade.TextChanged

    End Sub

    Protected Sub txtReservaCarga_TextChanged(sender As Object, e As EventArgs) Handles txtReservaCarga.TextChanged

    End Sub
End Class