Imports System.Data.SqlClient
Imports System.Web.Services
Imports Newtonsoft.Json

Public Class TalieColetorDescarga
    Inherits System.Web.UI.Page

    Dim Sql As String = String.Empty
    Private logDAO As New Log

    Public Sub New()

    End Sub

    Private Function ObterDD() As Integer
        If Request.QueryString("dd") IsNot Nothing Then
            Return Val(Request.QueryString("dd").ToString())
        End If
    End Function


    Private Function ObterCD() As Integer
        If Request.QueryString("cd") IsNot Nothing Then
            Return Val(Request.QueryString("cd").ToString())
        End If
    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("LOGADO") Is Nothing Or Session("USUARIO") Is Nothing Or Session("SENHA") Is Nothing Or Session("EMPRESA") Is Nothing Or Session("PATIO") Is Nothing Or Session("AUTONUMPATIO") Is Nothing Then
            Try

                If 1 = 1 Then
                    Console.Write("True")
                End If
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
                Session("FLAG_OB_MARCANTE") = Server.HtmlEncode(Request.Cookies("FLAG_OB_MARCANTE").Value)
                Session("FLAG_FILMA_DESOVA") = Server.HtmlEncode(Request.Cookies("FLAG_FILMA_DESOVA").Value)
                Session("FLAG_REDEX_SEM_MARCANTE") = Server.HtmlEncode(Request.Cookies("FLAG_REDEX_SEM_MARCANTE").Value)
                Session("LARGURATELA") = Server.HtmlEncode(Request.Cookies("LARGURATELA").Value)
            Catch ex As Exception
                Log.InserirLog(Estufagem.ObterPagina(Page), ex.Message, ex.StackTrace, Session("AUTONUMUSUARIO"))
                ModelState.AddModelError(String.Empty, $"Ocorreu um erro ao carregar a página!")
                ViewState("Sucesso") = False
                Exit Sub
            End Try
        End If

        If Me.IsPostBack Then
            TabName.Value = Request.Form(TabName.UniqueID)
        End If

        If Not Page.IsPostBack Then
            SetEstiloPnlLimpar()
            LimparTalie()
            CarregarRegistro()

            SetTipoDescarga()

            If ObterDD() > 0 Then
                pnlCrossDocking.Visible = False
                pnlConteiner.Visible = False
            End If

            ConsultarTalies()

            If (Request.QueryString("registro").ToInt() > 0) Then
                CarregarDadosPorRegistro(Request.QueryString("registro"))
                Exit Sub
            End If

        End If
    End Sub

    Private Sub CarregarRegistro()
        Try

            Sql = " SELECT "
            Sql = Sql & "    DISTINCT "
            Sql = Sql & "    B.AUTONUM_REG AS REGISTRO "
            Sql = Sql & " FROM "
            Sql = Sql & "    " + Banco.BancoRedex + "TB_GATE_NEW A "
            Sql = Sql & " INNER JOIN "
            Sql = Sql & "    " + Banco.BancoRedex + "TB_REGISTRO B ON A.AUTONUM = B.AUTONUM_GATE "
            Sql = Sql & " INNER JOIN "
            Sql = Sql & "    " + Banco.BancoRedex + "TB_REGISTRO_CS RCS ON RCS.AUTONUM_REG = B.AUTONUM_REG "
            Sql = Sql & " LEFT JOIN "
            Sql = Sql & "    " + Banco.BancoRedex + "TB_TALIE T ON B.AUTONUM_REG = T.AUTONUM_REG "
            Sql = Sql & " WHERE   "
            Sql = Sql & " isnull(t.FLAG_FECHADO, 0) = 0 And a.DT_GATE_IN > GETDATE() - 60  AND A.FLAG_GATE_IN = 1 and B.tipo_registro='E'"
            Sql = Sql & " ORDER BY "
            Sql = Sql & "    b.autonum_reg DESC "



            Me.cbRegistro.Items.Clear()
            Me.cbRegistro.DataTextField = "REGISTRO"
            Me.cbRegistro.DataValueField = "REGISTRO"
            Me.cbRegistro.DataSource = Banco.List(Sql.ToString())
            Me.cbRegistro.DataBind()

            cbRegistro.Items.Insert(0, New ListItem("Selecione um Regitro", 0))
            cbRegistro.SelectedIndex = 0
        Catch ex As Exception
            Log.InserirLog(Estufagem.ObterPagina(Page), ex.Message, ex.StackTrace, Page.Session("AUTONUMUSUARIO"))
            ModelState.AddModelError(String.Empty, $"Ocorreu um erro ao carregar itens da NF: {ex.Message}")
            Exit Sub

        End Try
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
            Log.InserirLog(Estufagem.ObterPagina(Page), ex.Message, ex.StackTrace, Page.Session("AUTONUMUSUARIO"))
            ModelState.AddModelError(String.Empty, $"Ocorreu um erro ao carregar conferentes: {ex.Message}")
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
            Log.InserirLog(Estufagem.ObterPagina(Page), ex.Message, ex.StackTrace, Page.Session("AUTONUMUSUARIO"))
            ModelState.AddModelError(String.Empty, $"Ocorreu um erro ao carregar equipes: {ex.Message}")
            Exit Sub
        End Try

    End Sub

    Private Sub CarregarModo()
        Me.cbModo.Items.Clear()
        Me.cbModo.Items.Insert(0, "Selecione o Modo")
        Me.cbModo.Items.Insert(1, "Automatizada")
        Me.cbModo.Items.Insert(2, "Manual")
    End Sub

    Private Sub ConsultarTalies(Optional talie As String = "", Optional registro As String = "")
        Try
            Sql = "       SELECT "
            Sql = Sql & "    a.autonum_reg, "
            Sql = Sql & "    c.id_conteiner, "
            Sql = Sql & "    b.reference, "
            Sql = Sql & "    b.instrucao, "
            Sql = Sql & "    d.fantasia, "
            Sql = Sql & "    b.autonum_parceiro, "
            Sql = Sql & "    a.AUTONUM_TALIE as Id, "
            Sql = Sql & "    a.AUTONUM_PATIO, "
            Sql = Sql & "    a.Placa, "
            Sql = Sql & "    a.Inicio, "
            Sql = Sql & "    a.TERMINO, "
            Sql = Sql & "    a.FLAG_DESCARGA, "
            Sql = Sql & "    a.FLAG_ESTUFAGEM, "
            Sql = Sql & "    a.CROSSDOCKING, "
            Sql = Sql & "    a.CONFERENTE, "
            Sql = Sql & "    a.EQUIPE, "
            Sql = Sql & "    a.AUTONUM_BOO, "
            Sql = Sql & "    a.FLAG_CARREGAMENTO, "
            Sql = Sql & "    A.AUTONUM_GATE, "
            Sql = Sql & "    a.flag_fechado, "
            Sql = Sql & "    A.FLAG_COMPLETO, "
            Sql = Sql & "    a.forma_operacao"
            Sql = Sql & " FROM"
            Sql = Sql & "    " & Banco.BancoRedex & "tb_talie a"
            Sql = Sql & " inner join "
            Sql = Sql & "    " & Banco.BancoRedex & "tb_booking b on a.autonum_boo = b.autonum_boo"
            Sql = Sql & " left join "
            Sql = Sql & "    " & Banco.BancoRedex & "tb_patio c on a.autonum_patio = c.autonum_patio"
            Sql = Sql & " inner join "
            Sql = Sql & "    " & Banco.BancoRedex & "tb_cad_parceiros d on b.autonum_parceiro = d.autonum "
            Sql = Sql & " where  "
            Sql = Sql & "    1=1 AND a.flag_fechado = 0 "
            Sql = Sql & IIf(String.IsNullOrEmpty(talie), "", $" AND a.AUTONUM_TALIE = {talie} ")
            Sql = Sql & IIf(String.IsNullOrEmpty(registro), "", $" AND  a.autonum_reg = {registro} ")
            Sql = Sql & "    AND flag_descarga= " & IIf(hddnTipoDescarga.Value.Equals(OpcoesDescarga.DA.ToString()), 1, 0) & "  "
            Sql = Sql & "    AND crossdocking=" & IIf(hddnTipoDescarga.Value.Equals(OpcoesDescarga.CD.ToString()), 1, 0) & " "
            Sql = Sql & " order by "
            Sql = Sql & "    a.autonum_reg desc"

            Dim dt As New DataTable
            dt = Banco.Consultar(String.Format(Sql.ToString(), ID))

            Me.grdTalie.DataSource = dt
            Me.grdTalie.DataBind()

            If dt.Rows.Count > 0 Then
                grdTalie.Enabled = (dt.Rows(0)("flag_descarga").ToString() = 1 Or dt.Rows(0)("crossdocking").ToString() = 1)
            End If
        Catch ex As Exception
            Log.InserirLog(Estufagem.ObterPagina(Page), ex.Message, ex.StackTrace, Session("AUTONUMUSUARIO"))
            ModelState.AddModelError(String.Empty, $"Ocorreu um erro ao consultar talie: {ex.Message.RemoverCaracteresEspeciaisAlert()} ")
            Exit Sub

        End Try
    End Sub

    Protected Sub grdTalie_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdTalie.RowDataBound

        If e.Row.RowType = DataControlRowType.Header Then

            e.Row.TableSection = TableRowSection.TableHeader
        End If

        If e.Row.RowType = DataControlRowType.DataRow Then

            Dim ID = Me.grdTalie.DataKeys(e.Row.RowIndex).Value.ToString().ToInt()

        End If
    End Sub

    Private Sub LimparTalie()

        hddnTalieId.Value = "0"
        hddnTalieItensSelIdx.Value = "0"
        hddnNFItensSelIdx.Value = "0"
        hddnConteinerId.Value = "0"
        hddnGateId.Value = "0"
        hddnReserva.Value = "0"
        hddnNFId.Value = "0"
        hddnCrossDocking.Value = "0"
        hddnNFItemId.Value = "0"
        hddnNovoNFItem.Value = "0"
        hddnEmbalagemId.Value = "0"
        hddnProdutoId.Value = "0"
        hddnClienteId.Value = "0"
        hddnReservaId.Value = "0"
        hddnTalieItemId.Value = "0"

        cbConferente.Items.Clear()
        cbEquipe.Items.Clear()
        cbModo.Items.Clear()

        chkTalieFechado.Checked = False
        chkCrossdocking.Checked = False
        chkEstufagemCompleta.Checked = False

        lblTalieNumero.Text = String.Empty
        txtInicio.Text = String.Empty
        txtFIM.Text = String.Empty
        txtCliente.Text = String.Empty
        txtPlaca.Text = String.Empty
        txtConteiner.Text = String.Empty
        txtReserva.Text = String.Empty

        CarregarConferentes()
        CarregarEquipes()
        CarregarModo()

    End Sub

    Private Sub LimparTalieItens()

        cbNF.Items.Clear()
        '        cbLocal.Items.Clear()
        cbModo.Items.Clear()

        chkTalieFechado.Checked = False
        chkCrossdocking.Checked = False
        chkEstufagemCompleta.Checked = False

        txtEmbalagem.Text = String.Empty
        txtProduto.Text = String.Empty
        txtQtdeNF.Text = String.Empty
        txtQtdeDescarga.Text = String.Empty
        txtLargura.Text = String.Empty
        txtAltura.Text = String.Empty
        txtComprimento.Text = String.Empty
        txtPeso.Text = String.Empty
        txtLocal.Text = String.Empty

        pnlEdicaoItem.Visible = False

    End Sub

    Private Sub LimparGridItens()
        grdItens.DataSource = Nothing
        grdItens.DataBind()
        ConsultarTalieItens(-1)
    End Sub

    Protected Sub cbRegistro_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbRegistro.SelectedIndexChanged

        CarregarDadosPorRegistro(cbRegistro.SelectedValue)
    End Sub
    Private Sub CarregarDadosPorRegistro(registro As String)
        If String.IsNullOrEmpty(registro) Or registro.ToInt <= 0 Then
            ModelState.AddModelError(String.Empty, $"Informe um registro")
            Exit Sub
        End If

        LimparTalie()
        LimparTalieItens()
        LimparGridItens()

        ObterDadosPorRegistro(registro)
    End Sub

    Protected Sub ObterDadosPorRegistro(registroConsultar As String)


        Try

            Sql = " SELECT
                        A.AUTONUM_TALIE AS Id,
                        A.AUTONUM_REG AS Registro,
                        C.ID_CONTEINER AS ConteinerId,
                        B.REFERENCE AS Referencia,
                        B.INSTRUCAO AS Instrucao,
                        D.FANTASIA as Exportador,
                        B.AUTONUM_EXPORTADOR AS ExportadorId,
                        A.AUTONUM_PATIO PatioId,
                        A.PLACA,
                        CONVERT(VarChar(10), a.inicio, 103) + ' ' + CONVERT(VarChar(8),  a.inicio, 108) INICIO,
                        CONVERT(VarChar(10), a.termino, 103) + ' ' + CONVERT(VarChar(8), a.termino, 108) TERMINO,
                        ISNULL(a.FLAG_DESCARGA,0) as Descarga,
                        ISNULL(a.FLAG_ESTUFAGEM,0) as Estufagem,
                        a.CROSSDOCKING,
                        a.CONFERENTE as ConferenteId,
                        a.EQUIPE as EquipeId,
                        a.AUTONUM_BOO as BookingId,
                        ISNULL(a.FLAG_CARREGAMENTO,0) as Carregamento,
                        A.AUTONUM_GATE as GateId,
                        ISNULL(A.FLAG_FECHADO,0) as Fechado,
                        B.AUTONUM_PATIOS as Patio,
                        A.FORMA_OPERACAO as OperacaoId,
                        Conf.FANTASIA AS Conferente,
                        Eq.FANTASIA as Equipe,
                        case when  A.FORMA_OPERACAO = 'A' then 'Automático' else 'Manual' end Operacao
                    FROM
                     " & Banco.BancoRedex & "TB_TALIE A
                    INNER JOIN
                     " & Banco.BancoRedex & "TB_BOOKING B ON A.AUTONUM_BOO = B.AUTONUM_BOO
                    LEFT JOIN
                     " & Banco.BancoRedex & "TB_PATIO C ON A.AUTONUM_PATIO = C.AUTONUM_PATIO
                    INNER JOIN
                     " & Banco.BancoRedex & "TB_CAD_PARCEIROS D ON B.AUTONUM_PARCEIRO = D.AUTONUM
                    LEFT JOIN
                        " & Banco.BancoRedex & "tb_cad_parceiros Conf on a.CONFERENTE = CONF.AUTONUM
                    LEFT JOIN
                        " & Banco.BancoRedex & "tb_cad_parceiros Eq on a.EQUIPE = Eq.AUTONUM
                    WHERE
                     A.AUTONUM_REG=" & registroConsultar & "
                    ORDER BY A.INICIO DESC, D.FANTASIA, C.ID_CONTEINER, B.REFERENCE, B.INSTRUCAO"

            Dim dadosTalie As DataTable = Banco.List(Sql)

            CarregarConferentes()
            CarregarEquipes()
            CarregarModo()

            If dadosTalie.Rows.Count > 0 Then
                lblTalieNumero.Text = dadosTalie.Rows(0)("Id").ToString()
                cbRegistro.Text = dadosTalie.Rows(0)("Registro").ToString()
                txtConteiner.Text = dadosTalie.Rows(0)("ConteinerId").ToString()
                hddnReserva.Value = dadosTalie.Rows(0)("BookingId").ToString()
                hddnReservaId.Value = dadosTalie.Rows(0)("BookingId").ToString()
                txtReserva.Text = dadosTalie.Rows(0)("Referencia").ToString()

                txtPlaca.Text = dadosTalie.Rows(0)("Placa").ToString()
                txtInicio.Text = dadosTalie.Rows(0)("Inicio").ToString()
                txtFIM.Text = dadosTalie.Rows(0)("Termino").ToString()
                chkCrossdocking.Checked = dadosTalie.Rows(0)("CrossDocking").ToString() > 0
                hddnGateId.Value = dadosTalie.Rows(0)("GateId").ToString()
                hddnClienteId.Value = dadosTalie.Rows(0)("ExportadorId").ToString()
                txtCliente.Text = dadosTalie.Rows(0)("Exportador").ToString()

                hddnConteinerId.Value = dadosTalie.Rows(0)("PatioId").ToString()

                cbConferente.SelectedIndex = cbConferente.Items.IndexOf((cbConferente.Items.FindByValue(dadosTalie.Rows(0)("ConferenteId").ToString())))
                cbEquipe.SelectedIndex = cbEquipe.Items.IndexOf((cbEquipe.Items.FindByValue(dadosTalie.Rows(0)("EquipeId").ToString())))
                cbModo.SelectedIndex = cbModo.Items.IndexOf((cbModo.Items.FindByValue(dadosTalie.Rows(0)("OperacaoId").ToString())))

                If "A".Equals(dadosTalie.Rows(0)("OperacaoId").ToString()) Then
                    cbModo.SelectedValue = "Automatizada"
                ElseIf "M".Equals(dadosTalie.Rows(0)("OperacaoId").ToString()) Then
                    cbModo.SelectedValue = "Manual"
                End If

                Dim talieFechado As Boolean = dadosTalie.Rows(0)("Fechado").ToString()
                chkTalieFechado.Checked = talieFechado

                pnlFinalizar.Visible = Not talieFechado
                pnlGravar.Visible = Not talieFechado
                SetEstiloPnlLimpar()

                ConsultarTalieItens(lblTalieNumero.Text)
                SetTipoDescarga()
            Else
                Sql = " SELECT
                        A.AUTONUM AS GateId,
                        B.PLACA,
                        E.AUTONUM_EXPORTADOR as ExportadorId,
                        CP.RAZAO AS Exportador,
                        E.REFERENCE AS Referencia,
                        E.AUTONUM_BOO as Reserva,
                        B.AUTONUM_REG as Registro,
                        B.PATIO,
                        getdate() as Inicio,
                        B.DT_LIB_ENT_CAM
                    FROM
                        " & Banco.BancoRedex & "TB_GATE_NEW A
                    INNER JOIN
                        " & Banco.BancoRedex & "TB_REGISTRO B ON A.AUTONUM = B.AUTONUM_GATE
                    INNER JOIN
                        " & Banco.BancoRedex & "TB_BOOKING E ON B.AUTONUM_BOO = E.AUTONUM_BOO
                    INNER JOIN
                        " & Banco.BancoRedex & "TB_CAD_PARCEIROS CP ON E.AUTONUM_EXPORTADOR=CP.AUTONUM
                    WHERE
                        B.AUTONUM_REG= " & registroConsultar & "
                    ORDER BY
                        A.AUTONUM DESC"

                Dim registroBusca As DataTable = Banco.List(Sql)

                If registroBusca.Rows.Count = 0 Then
                    ModelState.AddModelError(String.Empty, $"Registro não encontrado")
                    cbRegistro.SelectedIndex = -1
                    Exit Sub
                End If

                If (String.IsNullOrEmpty(registroBusca.Rows(0)("DT_LIB_ENT_CAM").ToString())) Then
                    ModelState.AddModelError(String.Empty, $"Veículo não possui liberação de descarga")
                    cbRegistro.SelectedIndex = -1
                    Exit Sub
                End If

                hddnGateId.Value = registroBusca.Rows(0)("GateId").ToString()
                txtPlaca.Text = registroBusca.Rows(0)("PLACA").ToString()

                hddnClienteId.Value = registroBusca.Rows(0)("ExportadorId").ToString()
                txtCliente.Text = registroBusca.Rows(0)("Exportador").ToString()

                hddnReserva.Value = registroBusca.Rows(0)("Reserva").ToString()

                hddnReservaId.Value = registroBusca.Rows(0)("Reserva").ToString()
                txtReserva.Text = registroBusca.Rows(0)("Referencia").ToString()

                cbRegistro.Text = registroBusca.Rows(0)("Registro").ToString()
                txtInicio.Text = Now.ToString("dd/MM/yyyy HH:mm")

                pnlFinalizar.Visible = False
                SetEstiloPnlLimpar()
                SetTipoDescarga()
            End If
            ConsultarTalies()
        Catch ex As Exception
            Log.InserirLog(Estufagem.ObterPagina(Page), ex.Message, ex.StackTrace, Page.Session("AUTONUMUSUARIO"))
            ModelState.AddModelError(String.Empty, $"Ocorreu um erro ao obter dados por registro!")
            Exit Sub

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

        'pnlLimpar.CssClass = pnlLimpar.CssClass.Replace("offset-md-6 offset-lg-6", "")
        'pnlLimpar.CssClass = pnlLimpar.CssClass.Replace("offset-md-8 offset-lg-8", "")
        'pnlLimpar.Attributes.Add("class", IIf(pnlFinalizar.Visible, "offset-md-6 offset-lg-6 ", "offset-md-8 offset-lg-8"))
    End Sub

    Protected Sub btnTalieGravar_Click(sender As Object, e As EventArgs) Handles btnTalieGravar.Click
        Try

            If String.IsNullOrEmpty(txtInicio.Text) Then
                ModelState.AddModelError(String.Empty, $"Informe a data de início")
            End If

            If Not IsDate(txtInicio.Text) Then
                ModelState.AddModelError(String.Empty, $"Data de início inválida")
            End If

            If cbConferente.SelectedIndex <= 0 Then
                ModelState.AddModelError(String.Empty, $"Informe o conferente")
            End If

            If cbEquipe.SelectedIndex <= 0 Then
                ModelState.AddModelError(String.Empty, $"Informe a equipe")
            End If
            If cbModo.SelectedIndex <= 0 Then
                ModelState.AddModelError(String.Empty, $"Informe o modo de operação")
            End If
            If hddnTipoDescarga.Value.Equals(OpcoesDescarga.CD.ToString()) AndAlso String.IsNullOrEmpty(txtConteiner.Text) Then
                ModelState.AddModelError(String.Empty, $"Informe o contêiner")
            End If

            If String.IsNullOrEmpty(txtPlaca.Text) Then
                ModelState.AddModelError(String.Empty, $"Informe a Placa do veículo descarregado")
            End If
            Sql = "select isnull(max(p.autonum_patio),0) from redex..tb_patio p"
            Sql = Sql & " where isnull(p.flag_historico,0)=0 and p.id_conteiner='" & txtConteiner.Text & "'"
            Dim registroBusca As Long
            registroBusca =Banco.ExecuteScalar(Sql).ToInt()


            If hddnTipoDescarga.Value.Equals(OpcoesDescarga.CD.ToString()) AndAlso registroBusca = 0 Then
                ModelState.AddModelError(String.Empty, $"Informe um contêiner válido")
            End If

            If (Not ModelState.IsValid) Then
                ViewState("Sucesso") = False
                Exit Sub
            End If




            If String.IsNullOrEmpty(lblTalieNumero.Text) Then

                Sql = "Insert into " & Banco.BancoRedex & "tb_talie ( "
                Sql = Sql & "autonum_patio,inicio,termino,flag_descarga,flag_estufagem,flag_carregamento"
                Sql = Sql & ",crossdocking,conferente,equipe,autonum_boo,forma_operacao,placa,AUTONUM_GATE,FLAG_COMPLETO,AUTONUM_REG"
                Sql = Sql & ") values ( "
                If hddnTipoDescarga.Value.Equals(OpcoesDescarga.CD.ToString()) Then
                    Sql = Sql & registroBusca
                Else
                    Sql = Sql & "NULL"
                End If
                Sql = Sql & ",convert(datetime,'" & String.Format(txtInicio.Text, "dd/MM/yyyy HH:mm") & "',103)"
                Sql = Sql & ",NULL"
                Sql = Sql & ",1"
                Sql = Sql & "," & IIf(hddnTipoDescarga.Value.Equals(OpcoesDescarga.CD.ToString()), 1, 0)
                Sql = Sql & "," & IIf(hddnTipoDescarga.Value.Equals(OpcoesDescarga.CCS.ToString()), 1, 0)
                Sql = Sql & "," & chkCrossdocking.Checked.ToInt()
                Sql = Sql & "," & cbConferente.SelectedValue
                Sql = Sql & "," & cbEquipe.SelectedValue
                Sql = Sql & "," & hddnReserva.Value
                Sql = Sql & ",'" & UCase(Left(cbModo.Text, 1)) & "'"
                If Not String.IsNullOrEmpty(txtPlaca.Text) Then
                    Sql = Sql & ",'" & txtPlaca.Text & "'"
                    Sql = Sql & "," & hddnGateId.Value
                Else
                    Sql = Sql & ",null"
                    Sql = Sql & ",0"
                End If
                Sql = Sql & "," & chkEstufagemCompleta.Checked.ToInt()
                Sql = Sql & "," & IIf(String.IsNullOrEmpty(cbRegistro.Text), 0, cbRegistro.Text)
                Sql = Sql & ")"
                Banco.ExecuteScalar(Sql)

                Sql = "SELECT IDENT_CURRENT('REDEX.DBO.TB_TALIE')"




                'Dim NT As String
                ' Dim conn As Connection
                ' conn = BD.StringConexao
                'Dim retorno As New ADODB.Recordset
                'retorno.Open(Sql.ToUpper(), BD.Conexao, ADODB.CursorTypeEnum.adOpenKeyset, ADODB.LockTypeEnum.adLockOptimistic)
                'retorno = retorno.NextRecordset
                'MsgBox(retorno.Fields(0).Value)

                Dim NT = Banco.ExecuteScalar(Sql)

                'Dim sqlTest = $"select max(AUTONUM_TALIE) ID from MATIRA.REDEX.dbo.tb_talie where AUTONUM_REG = {IIf(String.IsNullOrEmpty(cbRegistro.Text), 0, cbRegistro.Text)}" ' or Identity
                'Dim rst As New ADODB.Recordset
                'rst.Open(sqlTest, BD.Conexao, 1, 1)

                'NT = rst.Fields("ID").Value
                lblTalieNumero.Text = NT
                grdTalie.SelectedIndex = Value2Idx(grdTalie, "AUTONUM_TALIE", NT).ToString()

                'GRAVAR TALIE_ITEM
                ' gera descarga automatica
                GerarDescargaAutomatica()

                lblTalieNumero.Text = NT
                pnlFinalizar.Visible = True
                SetEstiloPnlLimpar()
            Else

                If chkTalieFechado.Checked Then
                    ModelState.AddModelError(String.Empty, $"Talie Fechado, alteração não permitida")
                    ViewState("Sucesso") = False
                    Exit Sub
                End If

                Sql = $" update
                            {Banco.BancoRedex}tb_talie
                        set
                            autonum_patio = {IIf(hddnTipoDescarga.Value.Equals(OpcoesDescarga.CD.ToString()), hddnConteinerId.Value, "null")},
                            inicio=convert(datetime,'{String.Format(txtInicio.Text, "dd/MM/yyyy HH:mm") }',103),
                            flag_estufagem={IIf(hddnTipoDescarga.Value.Equals(OpcoesDescarga.CD.ToString()), 1, 0)},
                            flag_carregamento={ IIf(hddnTipoDescarga.Value.Equals(OpcoesDescarga.CCS.ToString()), 1, 0)},
                            crossdocking={chkCrossdocking.Checked.ToInt()},
                            FLAG_COMPLETO={chkEstufagemCompleta.Checked.ToInt()},
                            conferente={cbConferente.SelectedValue},
                            equipe={cbEquipe.SelectedValue},
                            autonum_boo={ hddnReserva.Value},
                            forma_operacao='{UCase(Left(cbModo.Text, 1))}',
                            AUTONUM_REG={IIf(String.IsNullOrEmpty(cbRegistro.Text), 0, cbRegistro.Text)}
                        where
                            autonum_talie = {lblTalieNumero.Text}"
                If Banco.BeginTransaction(Sql.ToUpper()) Then
                    grdTalie.SelectedIndex = Value2Idx(grdTalie, "AUTONUM_TALIE", lblTalieNumero.Text).ToString()

                    Sql = $"SELECT
                                    count(*) TOTAL
                                 FROM
                                 {Banco.BancoRedex}tb_talie g
                                 inner join {Banco.BancoRedex}tb_talie_item a on a.autonum_talie = g.autonum_talie
                                 where a.autonum_talie={lblTalieNumero.Text}"

                    Dim itemBusca = Banco.ExecuteScalar(Sql)

                    If (Val(itemBusca) = 0) Then

                        GerarDescargaAutomatica()

                        'Sql = $"INSERT INTO
                        '            {Banco.BancoRedex}tb_talie_item (
                        '                autonum_talie,
                        '                nf,
                        '                qtde_descarga,
                        '                tipo_descarga,
                        '                diferenca,
                        '                obs,
                        '                qtde_disponivel,
                        '                peso,
                        '                qtde_estufagem,
                        '                AUTONUM_REGCS)
                        '            SELECT
                        '                {lblTalieNumero.Text} AS AUTONUM_TALIE,
                        '                NFI.AUTONUM_NF,
                        '                rcs.quantidade,
                        '                'TOTAL',
                        '                0,
                        '                '',
                        '                RCS.QUANTIDADE,
                        '                BCG.PESO_BRUTO,
                        '                0
                        '            FROM
                        '                {Banco.BancoRedex}tb_registro_cs rcs
                        '            INNER JOIN
                        '                {Banco.BancoRedex}tb_booking_carga bcg on rcs.autonum_bcg=bcg.autonum_bcg
                        '            INNER JOIN
                        '                {Banco.BancoRedex}TB_NOTAS_FISCAIS NF ON (RCS.AUTONUM_REG=NF.AUTONUM_REG And RCS.NF=NF.NUM_NF)
                        '            INNER JOIN
                        '                {Banco.BancoRedex}TB_NOTAS_ITENS NFI ON NF.AUTONUM_NF = NFI.AUTONUM_NF
                        '            WHERE
                        '                RCS.AUTONUM_REG= {cbRegistro.Text}"
                        'Banco.BeginTransaction(Sql)

                    End If

                End If
            End If

            If Not String.IsNullOrEmpty(txtConteiner.Text) And hddnConteinerId.Value <> "0" Then
                If chkEstufagemCompleta.Checked Then
                    Sql = "update " & Banco.BancoRedex & "tb_patio set ef='F' where autonum_patio = " & hddnConteinerId.Value
                Else
                    Sql = "update " & Banco.BancoRedex & "tb_patio set ef='E' where autonum_patio = " & hddnConteinerId.Value
                End If
                Banco.BeginTransaction(Sql.ToUpper)
            End If

            ConsultarTalieItens(lblTalieNumero.Text)


            ViewState("Sucesso") = True
            lblMensagem.Text = $"Talie {IIf(String.IsNullOrEmpty(lblTalieNumero.Text), "cadastrado", "atualizado")} com sucesso!"

        Catch ex As Exception
            Log.InserirLog(Estufagem.ObterPagina(Page), ex.Message, ex.StackTrace, Page.Session("AUTONUMUSUARIO"))
            ModelState.AddModelError(String.Empty, $"Ocorreu um erro: {ex.Message}!")
            Exit Sub
        End Try

    End Sub

    Private Sub GerarDescargaAutomatica()
        Dim IdNF As Long = 0
        Dim dtRegistros As DataTable
        Sql = $"SELECT 
                            rcs.*, 
                            bcg.qtde as qtde_manifestada, 
                            (isnull(BCG.PESO_bruto,0) / bcg.qtde) as peso_manifestado,
                            bcg.imo,
                            bcg.imo2,
                            bcg.imo3,
                            bcg.imo4,
                            bcg.uno,
                            bcg.uno2,
                            bcg.uno3,
                            bcg.uno4,
                            BCG.AUTONUM_PRO,
                            BCG.AUTONUM_EMB
                        FROM 
                            {Banco.BancoRedex}tb_registro reg 
                        INNER JOIN 
                            {Banco.BancoRedex}tb_registro_cs rcs on reg.autonum_reg=rcs.autonum_reg
                        INNER JOIN 
                            {Banco.BancoRedex}tb_booking_carga bcg on rcs.autonum_bcg=bcg.autonum_bcg
                        WHERE 
                            reg.autonum_reg={cbRegistro.Text}"

        dtRegistros = Banco.Consultar(Sql)

        For Each registro As DataRow In dtRegistros.Rows

            Dim dtNFs As New DataTable
            Sql = $"SELECT max(a.AUTONUM_NF) AUTONUM_NF FROM REDEX.dbo.tb_NOTAS_FISCAIS A where A.NUM_NF = '{registro("NF").ToString()}'"
            IdNF = Banco.ExecuteScalar(Sql).ToInt()

            Dim PesoNF As Double
            PesoNF = 0
            If IdNF <> 0 Then
                PesoNF = Banco.ExecuteScalar($"select max(isnull(peso_bruto,0)) from redex.dbo.tb_notas_fiscais where autonum_nf={IdNF}").ToDouble()
            End If

            Sql = $"INSERT INTO 
                                {Banco.BancoRedex}TB_TALIE_ITEM 
                                    (
                                        AUTONUM_TALIE,
                                        AUTONUM_REGCS,
                                        QTDE_DESCARGA,
                                        TIPO_DESCARGA,
                                        DIFERENCA,
                                        OBS,
                                        QTDE_DISPONIVEL,
                                        COMPRIMENTO,
                                        LARGURA,ALTURA,
                                        PESO,
                                        QTDE_ESTUFAGEM,
                                        MARCA,
                                        REMONTE,
                                        FUMIGACAO,
                                        FLAG_FRAGIL,
                                        FLAG_MADEIRA,
                                        YARD,
                                        ARMAZEM,
                                        AUTONUM_NF,
                                        NF,
                                        IMO,
                                        UNO,
                                        IMO2,
                                        UNO2,
                                        IMO3,
                                        UNO3,
                                        IMO4,
                                        UNO4,
                                        AUTONUM_EMB,
                                        AUTONUM_PRO
                                    ) values (
                                        {lblTalieNumero.Text},
                                        {registro("Autonum_Regcs").ToString},
                                        {registro("quantidade").ToString},
                                        'TOTAL',
                                        '0',
                                        '',
                                        0,
                                        0,
                                        0,
                                        0,
                                        {PPonto(PesoNF)},
                                        0,
                                        '',
                                        0,
                                        '',
                                        0,
                                        0,
                                        null,
                                        null,
                                        {IdNF},
                                        '{registro("NF").ToString}',
                                        '{IIf(String.IsNullOrEmpty(registro("IMO").ToString()), "", registro("IMO").ToString())}',
                                        '{IIf(String.IsNullOrEmpty(registro("UNO").ToString()), "", registro("UNO").ToString())}',
                                        '{IIf(String.IsNullOrEmpty(registro("imo2").ToString()), "", registro("imo2").ToString())}',
                                        '{IIf(String.IsNullOrEmpty(registro("uno2").ToString()), "", registro("uno2").ToString())}',
                                        '{IIf(String.IsNullOrEmpty(registro("imo3").ToString()), "", registro("imo3").ToString())}',
                                        '{IIf(String.IsNullOrEmpty(registro("uno3").ToString()), "", registro("uno3").ToString())}',
                                        '{IIf(String.IsNullOrEmpty(registro("imo4").ToString()), "", registro("imo4").ToString())}',
                                        '{IIf(String.IsNullOrEmpty(registro("uno4").ToString()), "", registro("uno4").ToString())}',
                                        {IIf(String.IsNullOrEmpty(registro("AUTONUM_EMB").ToString()), 0, registro("AUTONUM_EMB").ToString())},
                                        {IIf(String.IsNullOrEmpty(registro("AUTONUM_PRO").ToString()), 0, registro("AUTONUM_EMB").ToString())}
                                    )"
            Banco.ExecuteScalar(Sql)
        Next
    End Sub

    Private Sub SetTipoDescarga()
        If ObterDD() > 0 Then
            hddnTipoDescarga.Value = OpcoesDescarga.DA.ToString()
            Exit Sub
        End If

        hddnTipoDescarga.Value = OpcoesDescarga.CD.ToString()

    End Sub

    Private Sub ConsultarTalieItens(id As Integer)

        Try
            Sql = $"SELECT
                        a.AUTONUM_PRO,
                        a.obs,
                        b.qtde,
                        a.qtde_estufagem,
                        a.autonum_ti,
                        c.num_nf,
                        c.serie_nf,
                        d.lote,
                        a.qtde_descarga,
                        a.tipo_descarga,
                        a.diferenca,
                        f.descricao_emb,
                        e.desc_produto,
                        d.instrucao,
                        b.item,
                        a.comprimento,
                        a.largura,
                        a.altura,
                        a.peso,
                        a.armazem,
                        a.YARD,
                        g.autonum_talie,
                        b.autonum_nfi,
                        b.autonum_nf,
                        e.autonum_pro AS AUTONUM_PRODUTO,
                        f.autonum_emb AS AUTONUM_EMBALAGEM
                    FROM
                       {Banco.BancoRedex}tb_talie g
                    inner join
                       {Banco.BancoRedex}tb_talie_item a on a.autonum_talie = g.autonum_talie
					inner join
						{Banco.BancoRedex}TB_REGISTRO_CS rcs on a.AUTONUM_REGCS = rcs.AUTONUM_REGCS
                    left join
                       {Banco.BancoRedex}tb_notas_fiscais c on a.AUTONUM_NF = c.AUTONUM_NF
                    left join
                       {Banco.BancoRedex}tb_notas_itens b on C.autonum_nf = b.autonum_nf
                    inner join
                       {Banco.BancoRedex}tb_booking_carga d on rcs.autonum_bcg = d.autonum_bcg
                    left join
                       {Banco.BancoRedex}tb_cad_produtos e on d.autonum_pro = e.autonum_pro
                    left join
                       {Banco.BancoRedex}tb_cad_embalagens f on d.autonum_emb = f.autonum_emb
                    where
                       a.autonum_talie={id}
                    order by
                       d.instrucao,
                       c.num_nf,
                       b.item"

            Me.grdItens.DataSource = Banco.List(Sql)
            Me.grdItens.DataBind()
        Catch ex As Exception
            Log.InserirLog(Estufagem.ObterPagina(Page), ex.Message, ex.StackTrace, Session("AUTONUMUSUARIO"))
            ModelState.AddModelError(String.Empty, $"Ocorreu um erro ao consultar itens: {ex.Message} !")
            Exit Sub

        End Try
    End Sub

    Protected Sub grdItens_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdItens.RowCommand

        Dim Index As Integer = e.CommandArgument

        Dim ID As String = Me.grdItens.DataKeys(Index)("AUTONUM_TI").ToString()

        Select Case e.CommandName
            Case "EDITAR"
                Try
                    hddnTalieItemId.Value = grdItens.DataKeys(Index)("AUTONUM_TI")
                    EditarDescarga(grdItens.DataKeys(Index)("AUTONUM_TI"))
                    pnlEdicaoItem.Visible = True
                Catch ex As Exception
                    Log.InserirLog(Estufagem.ObterPagina(Page), ex.Message, ex.StackTrace, Page.Session("AUTONUMUSUARIO"))
                    ModelState.AddModelError(String.Empty, $"Ocorreu um erro ao habilitar item talie para edição: {ex.Message} ")
                    Exit Sub

                End Try
            Case "EXCLUIR"
                If GetFlagFechado(ID) = 1 Then
                    ModelState.AddModelError(String.Empty, $"Não é possivel excluir talie com fechamento confirmado")
                    Exit Sub
                End If
        End Select

    End Sub
    Private Sub EditarDescarga(talieItemId As Integer)

        Try
            Sql = $"SELECT
                        a.autonum_pro,
                        a.obs,
                        c.volumes as qtde,
                        a.qtde_estufagem,
                        a.autonum_ti,
                        c.num_nf,
                        c.serie_nf,
                        d.lote,
                        a.qtde_descarga,
                        a.tipo_descarga,
                        a.diferenca,
                        f.descricao_emb,
                        e.desc_produto,
                        d.instrucao,
                        a.comprimento,
                        a.largura,
                        a.altura,
                        a.peso,
                        a.armazem,
                        a.yard,
                        g.autonum_talie,
                        a.autonum_nf,
                        e.autonum_pro as AUTONUM_PRODUTO,
                        f.autonum_emb as AUTONUM_EMBALAGEM
                    FROM
                        {Banco.BancoRedex}tb_talie g
                    inner join
                        {Banco.BancoRedex}tb_talie_item a on a.autonum_talie = g.autonum_talie
                    inner join
						{Banco.BancoRedex}TB_REGISTRO_CS rcs on a.AUTONUM_REGCS = rcs.AUTONUM_REGCS
                    left join
                       {Banco.BancoRedex}tb_notas_fiscais c on a.AUTONUM_NF = c.AUTONUM_NF
                    inner join
                       {Banco.BancoRedex}tb_booking_carga d on rcs.autonum_bcg = d.autonum_bcg
                    left join
                       {Banco.BancoRedex}tb_cad_produtos e on d.autonum_pro = e.autonum_pro
                    left join
                       {Banco.BancoRedex}tb_cad_embalagens f on d.autonum_emb = f.autonum_emb
                    where
                        a.autonum_ti={talieItemId}
                    order by
                        d.instrucao,
                        c.num_nf"

            Dim talieItem As DataTable = Banco.List(Sql)

            If talieItem.Rows.Count > 0 Then

                LimparDescarga()
                CarregarNFs()
                '                CarregarArmazens()

                cbNF.SelectedIndex = cbNF.Items.IndexOf(cbNF.Items.FindByValue(Val(talieItem.Rows(0)("AUTONUM_NF").ToString())))
                txtEmbalagem.Text = talieItem.Rows(0)("DESCRICAO_EMB").ToString()
                txtProduto.Text = talieItem.Rows(0)("DESC_PRODUTO").ToString()
                txtQtdeNF.Text = talieItem.Rows(0)("QTDE").ToString()
                txtQtdeDescarga.Text = talieItem.Rows(0)("QTDE_DESCARGA").ToString()
                txtLargura.Text = talieItem.Rows(0)("LARGURA").ToString()
                txtAltura.Text = talieItem.Rows(0)("ALTURA").ToString()
                txtComprimento.Text = talieItem.Rows(0)("COMPRIMENTO").ToString()
                txtPeso.Text = talieItem.Rows(0)("PESO").ToString()
                '                cbLocal.SelectedIndex = cbLocal.Items.IndexOf(cbLocal.Items.FindByValue(Val(talieItem.Rows(0)("ARMAZEM").ToString())))
                txtLocal.Text = talieItem.Rows(0)("YARD").ToString()
                cbNF.Enabled = False
            End If
        Catch ex As Exception
            Log.InserirLog(Estufagem.ObterPagina(Page), ex.Message, ex.StackTrace, Session("AUTONUMUSUARIO"))
            ModelState.AddModelError(String.Empty, $"Ocorreu um erro ao consultar itens: {ex.Message}")
            Exit Sub

        End Try

    End Sub



    Protected Sub grdItens_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdItens.RowDataBound

        If e.Row.RowType = DataControlRowType.Header Then

            e.Row.TableSection = TableRowSection.TableHeader
        End If

        If e.Row.RowType = DataControlRowType.DataRow Then

            Dim ID = Me.grdTalie.DataKeys(e.Row.RowIndex).Value.ToString().ToInt()

        End If
    End Sub

    Protected Function GetFlagFechado(talieId As Integer) As Integer
        Dim retorno As Integer = 0

        Try
            Sql = $"SELECT
                        ISNULL(flag_fechado,0)  as flag_fechado
                    from
                        {Banco.BancoRedex}tb_talie
                    where
                        autonum_talie={talieId}"
            Dim tb1 As DataTable = Banco.List(Sql)
            If tb1.Rows.Count > 0 Then
                retorno = IIf(tb1.Rows(0) Is Nothing, 0, tb1.Rows(0)("flag_fechado"))
            End If
        Catch ex As Exception
            Log.InserirLog(Estufagem.ObterPagina(Page), ex.Message, ex.StackTrace, Page.Session("AUTONUMUSUARIO"))
            ModelState.AddModelError(String.Empty, $"Ocorreu um erro ao obter status talie: {ex.Message}")

        End Try
        Return retorno
    End Function

    Private Sub LimparDescarga()

        cbNF.SelectedIndex = -1
        '        cbLocal.SelectedIndex = -1
        txtEmbalagem.Text = String.Empty
        txtProduto.Text = String.Empty
        txtQtdeNF.Text = String.Empty
        txtQtdeDescarga.Text = String.Empty
        txtAltura.Text = String.Empty
        txtComprimento.Text = String.Empty
        txtLargura.Text = String.Empty
        txtPeso.Text = String.Empty
        txtLocal.Text = String.Empty
        LimparGridItens()
    End Sub

    Private Sub CarregarNFs()
        Try

            Sql = $"SELECT
                        'NF: ' + NF.NUM_NF + ' Qtde: ' + cast( NF.VOLUMES as varchar) DISPLAY,
                        NF.AUTONUM_NF as AUTONUM
                    FROM
                        {Banco.BancoRedex}tb_NOTAS_FISCAIS NF
                    INNER JOIN
                        {Banco.BancoRedex}TB_TALIE_ITEM TI ON NF.AUTONUM_NF = T.AUTONUM_NF
                    INNER JOIN
                        {Banco.BancoRedex}TB_TALIE T ON TI.AUTONUM_TALIE = T.AUTONUM_TALIE
                    INNER JOIN
                        {Banco.BancoRedex}tb_registro r on T.autonum_reg=r.autonum_reg
                    WHERE
                        NF.autonum_reg = {cbRegistro.Text}
                    ORDER BY
                        DISPLAY "


            Sql = $"SELECT
                        'NF: ' + NF.NUM_NF + ' Qtde: ' + cast( NF.VOLUMES as varchar) DISPLAY,
                        NF.AUTONUM_NF as AUTONUM
                    FROM
                        {Banco.BancoRedex}tb_NOTAS_FISCAIS NF
                    WHERE
                        NF.autonum_reg = {cbRegistro.Text}
                    ORDER BY
                        DISPLAY "

            Me.cbNF.Items.Clear()
            Me.cbNF.DataTextField = "DISPLAY"
            Me.cbNF.DataValueField = "AUTONUM"
            Me.cbNF.DataSource = Banco.List(Sql.ToString())
            Me.cbNF.DataBind()

            cbNF.Items.Insert(0, New ListItem("--Selecione uma NF--", 0))
            cbNF.SelectedIndex = 0
        Catch ex As Exception
            Log.InserirLog(Estufagem.ObterPagina(Page), ex.Message, ex.StackTrace, Page.Session("AUTONUMUSUARIO"))
            ModelState.AddModelError(String.Empty, $"Ocorreu um erro ao carregar NFs: {ex.Message}")
            Exit Sub
        End Try
    End Sub

    'Private Sub CarregarArmazens()
    '    Try

    '        Sql = $"SELECT
    '                    Autonum as autonum,
    '                    Descr AS DISPLAY
    '                FROM
    '                    {Banco.BancoSgipa}tb_armazens_ipa
    '                WHERE
    '                    patio = 1
    '                ORDER BY
    '                    DESCR"

    '        Me.cbLocal.Items.Clear()
    '        Me.cbLocal.DataTextField = "DISPLAY"
    '        Me.cbLocal.DataValueField = "AUTONUM"
    '        Me.cbLocal.DataSource = Banco.List(Sql.ToString())
    '        Me.cbLocal.DataBind()

    '        cbLocal.Items.Insert(0, New ListItem("--Selecione um Local--", 0))
    '        cbLocal.SelectedIndex = 0
    '    Catch ex As Exception
    '        Log.InserirLog(Estufagem.ObterPagina(Page), ex.Message, ex.StackTrace, Page.Session("AUTONUMUSUARIO"))
    '        ModelState.AddModelError(String.Empty, $"Ocorreu um erro ao carregar armazens: {ex.Message}")
    '        Exit Sub

    '    End Try
    'End Sub

    'Protected Sub cbLocal_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbLocal.SelectedIndexChanged
    'Try
    '    If Not cbLocal.Text.Equals(String.Empty) Then
    '        Dim sql As String
    '        sql = $"SELECT
    '                    flag_ct,
    '                    ISNULL(YARD,'') YARD
    '                FROM
    '                    {Banco.BancoSgipa}tb_armazens_ipa
    '                WHERE
    '                    autonum={Me.cbLocal.SelectedValue}"

    '        Dim TbA As DataTable = Banco.List(sql)

    '        If TbA.Rows.Count > 0 Then
    '            If TbA.Rows(0)("flag_ct").ToString() = 0 Then
    '                itemLocalArmazem.Visible = True
    '                txtLocal.Text = TbA.Rows(0)("YARD")
    '            Else
    '                itemLocalArmazem.Visible = False
    '                Me.txtLocal.Text = ""
    '            End If
    '        End If
    '    End If
    'Catch ex As Exception
    '    Log.InserirLog(Estufagem.ObterPagina(Page), ex.Message, ex.StackTrace, Page.Session("AUTONUMUSUARIO"))
    '    ModelState.AddModelError(String.Empty, $"Ocorreu um erro ao selecionar local!")
    '    Exit Sub
    'End Try

    'End Sub

    Protected Sub btnItemGravar_Click(sender As Object, e As EventArgs) Handles btnItemGravar.Click
        If hddnTalieItemId.Value > 0 Then

            Dim QTD As Long = 0
            Sql = $"select QTDE_DESCARGA from {Banco.BancoRedex}tb_talie_item where autonum_ti={hddnTalieItemId.Value}"
            QTD = CLng(Banco.ExecuteScalar(Sql))

            If Val(txtQtdeDescarga.Text) > QTD Then
                ModelState.AddModelError(String.Empty, $"Qtde descarregada não pode ser superior ao saldo existente")
                Exit Sub
            End If
            If Val(txtQtdeDescarga.Text) < QTD Then
                Dim DIF As Long = QTD - Val(txtQtdeDescarga.Text)
                Sql = $"insert into {Banco.BancoRedex}TB_TALIE_ITEM (AUTONUM_TALIE, NF, QTDE_DESCARGA, AUTONUM_EMB, QTDE_ESTUFAGEM, LOTE,
                        AUTONUM_PRO, AUTONUM_NF, PESO, COMPRIMENTO, LARGURA, ALTURA, DIFERENCA, TIPO_DESCARGA, OBS, QTDE_DISPONIVEL,
                        MARCA, FLAG_MADEIRA, FLAG_FRAGIL, REMONTE, FUMIGACAO, YARD, AUTONUM_REGCS, ARMAZEM, IMO, IMO2, IMO3, IMO4, IMO5,
                        UNO, UNO2, UNO3, UNO4, UNO5, CODIGO_CARGA, carimbo, data_liberacao, CARGA_NUMERADA)
                        select AUTONUM_TALIE,NF,{DIF},AUTONUM_EMB,QTDE_ESTUFAGEM,LOTE,
                        AUTONUM_PRO,AUTONUM_NF,PESO,COMPRIMENTO,LARGURA,ALTURA,DIFERENCA,TIPO_DESCARGA,OBS,QTDE_DISPONIVEL,
                        MARCA,FLAG_MADEIRA,FLAG_FRAGIL,REMONTE,FUMIGACAO,YARD,AUTONUM_REGCS,ARMAZEM,IMO,IMO2,IMO3,IMO4,IMO5,
                        UNO,UNO2,UNO3,UNO4,UNO5,CODIGO_CARGA,carimbo,data_liberacao,CARGA_NUMERADA
                        from redex.dbo.TB_TALIE_ITEM where autonum_ti={hddnTalieItemId.Value}"
                Try
                    Banco.BeginTransaction(Sql)
                Catch ex As Exception
                    Log.InserirLog(Estufagem.ObterPagina(Page), ex.Message, ex.StackTrace, Page.Session("AUTONUMUSUARIO"))
                    ModelState.AddModelError(String.Empty, $"Ocorreu um erro ao desdobrar o saldo do item: {ex.Message}")
                    Exit Sub
                End Try
            End If

            Sql = $"UPDATE
                        {Banco.BancoRedex}TB_TALIE_ITEM
                    SET
                        QTDE_DESCARGA =  {txtQtdeDescarga.Text.ToInt()},
                        LARGURA =   {txtLargura.Text.ToDecimalEnUs()},
                        ALTURA =  {txtAltura.Text.ToDecimalEnUs()},
                        COMPRIMENTO =  {txtComprimento.Text.ToDecimalEnUs()},
                        PESO =  {txtPeso.Text.ToDecimalEnUs()},
                        YARD =  '{txtLocal.Text}'
                    WHERE
                        AUTONUM_TI= {hddnTalieItemId.Value}"

            Try
                    Banco.BeginTransaction(Sql)

                    ViewState("Sucesso") = True
                    lblMensagem.Text = "Item atualizado com sucesso"

                    ConsultarTalieItens(lblTalieNumero.Text)
                    pnlEdicaoItem.Visible = False
                Catch ex As Exception
                    Log.InserirLog(Estufagem.ObterPagina(Page), ex.Message, ex.StackTrace, Page.Session("AUTONUMUSUARIO"))
                    ModelState.AddModelError(String.Empty, $"Ocorreu um erro ao atualizar o item: {ex.Message}")
                    Exit Sub

                End Try

            End If

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
        Me.grdTalie.Dispose()
        Me.grdItens.Dispose()

        Response.Redirect(Login.ObterURIMenu())
    End Sub

    Private Function Value2Idx(ByRef grd As System.Web.UI.WebControls.GridView, dtKey As String, valor As String) As Integer
        Try
            Dim i As Integer = -1
            For i = 0 To i < grd.Rows.Count

                grd.SelectedIndex = i
                If grd.SelectedDataKey(dtKey).ToString() = valor Then
                    Return i
                End If
            Next
            Return -1
        Catch
            Return -1
        End Try
    End Function

    Private Sub MakeGridViewPrinterFriendly(gridView As GridView)
        If gridView.Rows.Count > 0 Then
            gridView.UseAccessibleHeader = True
            gridView.HeaderRow.TableSection = TableRowSection.TableHeader
        End If
    End Sub

    Private Sub TalieColetorDescarga_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        MakeGridViewPrinterFriendly(grdTalie)
        MakeGridViewPrinterFriendly(grdItens)
    End Sub

    Protected Sub btnDescargaCancelar_Click1(sender As Object, e As EventArgs)
        LimparDescarga()
        ConsultarTalieItens(lblTalieNumero.Text)
    End Sub

    Private Sub btnTalieCancelar_Click(sender As Object, e As EventArgs) Handles btnTalieCancelar.Click
        cbRegistro.SelectedIndex = -1
        LimparTalie()
        LimparDescarga()
    End Sub

    Private Sub btnHome_Click(sender As Object, e As EventArgs) Handles btnHome.Click
        Response.Redirect(Login.ObterURIMenu())
    End Sub

    Private Sub btnLogout_Click(sender As Object, e As EventArgs) Handles btnLogout.Click
        Response.Redirect(Login.ObterURIMenu() & "home/logout")
    End Sub

    <WebMethod>
    Public Shared Function ExcluirItem(id As String) As String

        If (String.IsNullOrEmpty(id)) Then
            Throw New Exception("Selecione um item na lista")
        End If

        Try
            Dim Sql As String = String.Empty
            Sql = "delete from " & Banco.BancoRedex & "tb_talie_item"
            Sql = Sql & " where"
            Sql = Sql & " autonum_ti=" & id
            Banco.BeginTransaction(Sql.ToUpper())

            Return "Item excluído com sucesso!"
        Catch ex As Exception

            Log.InserirLog("Estufagem", ex.Message, ex.StackTrace, Nothing)
            Throw New Exception("Erro ao efetuar descarga")

        End Try
    End Function

    <WebMethod>
    Public Shared Function ExcluirTalie(id As String) As String

        If (String.IsNullOrEmpty(id)) Then
            Throw New Exception("Selecione um item na lista")
        End If

        Try

            Dim Sql1 As String = String.Empty
            Sql1 = $"DELETE FROM 
                        {Banco.BancoRedex}tb_talie_item
                    WHERE
                        autonum_talie ={id}"

            Dim Sql2 As String = String.Empty
            Sql2 = $"DELETE FROM
                        {Banco.BancoRedex}tb_talie
                    where
                        autonum_talie={id}"

            Banco.ExecuteTransaction(Sql1, Sql2)
            Return "Talie excluído com sucesso!"

        Catch ex As Exception
            Log.InserirLog("Estufagem", ex.Message, ex.StackTrace, 0)
            Throw New Exception("Erro. Tente novamente." & ex.Message())
        End Try

    End Function

    <WebMethod>
    Public Shared Function ObterTotalItensPorTalie(id As String) As String

        If (String.IsNullOrEmpty(id)) Then
            Throw New Exception("Selecione um item na lista")
        End If

        Try
            Return Banco.ExecuteScalar($"select count(*) from {Banco.BancoRedex}tb_talie_item where autonum_talie={id}")
        Catch ex As Exception
            Log.InserirLog("Estufagem", ex.Message, ex.StackTrace, 0)
            Throw New Exception("Erro. Tente novamente." & ex.Message())
        End Try

    End Function

    <WebMethod>
    Public Shared Function ValidarFecharTalie(idTalie As String, tipoDescarga As String, conteinerId As String, conteiner As String, usuarioId As String) As String
        Try
            Dim Sql As String = String.Empty
            Dim DataFechamento As String
            Dim Gate As Long
            Dim boo As Long

            If (String.IsNullOrEmpty(idTalie)) Then
                Throw New Exception("Selecione um item na lista")
            End If

            If (String.IsNullOrEmpty(idTalie)) Then
                Throw New Exception("Finalize somente após o lançamento do talie")
            End If

            If Banco.ExecuteScalar($"select count(*) from {Banco.BancoRedex}tb_talie_item where autonum_talie={idTalie}").ToInt() = 0 Then
                Throw New Exception("Finalize o talie somente após o lançamento da descarga")
            End If

            If tipoDescarga.Equals(OpcoesDescarga.CD.ToString()) Then
                If (String.IsNullOrEmpty(conteinerId)) Then
                    Throw New Exception("Descarga com Unitização é obrigatório informar contêiner")
                End If

                Sql = $"select count(*) from {Banco.BancoRedex}tb_talie_item where autonum_talie={idTalie} and isnull(autonum_nf,0)=0"

                If (Banco.ExecuteScalar(Sql).ToInt()) <> 0 Then
                    Throw New Exception($"Consta item sem vínculo de NF.{vbCr}Favor providenciar o cadastro da NF.") ' em : Documentação/Notas Fiscias/Lançamento"
                End If
            End If

            Sql = $"SELECT 
                        NF.NUM_NF 
                    FROM 
                        {Banco.BancoRedex}tb_talie_item ti
                    INNER JOIN 
                        {Banco.BancoRedex}TB_TALIE T ON TI.AUTONUM_TALIE = T.AUTONUM_TALIE
                    INNER JOIN 
                        {Banco.BancoRedex}TB_BOOKING BOO ON T.AUTONUM_BOO=BOO.AUTONUM_BOO
                    INNER JOIN 
                        {Banco.BancoRedex}TB_NOTAS_FISCAIS NF ON TI.AUTONUM_NF=NF.AUTONUM_NF
                    WHERE 
                        TI.AUTONUM_TALIE = {idTalie} AND ISNULL(TI.AUTONUM_NF,0)<>0 AND ISNULL(NF.AUTONUM_REG,0)=0 AND ISNULL(BOO.FLAG_BAGAGEM,0)=0"

            Dim dtNFs As New DataTable
            dtNFs = Banco.Consultar(Sql)

            Dim wNFs As String
            wNFs = ""

            For Each nf In dtNFs.Rows
                wNFs = wNFs & IIf(String.IsNullOrEmpty(nf("num_NF")), "1", String.IsNullOrEmpty(nf("num_NF")))
                If wNFs <> "" Then wNFs = wNFs & ","
            Next

            If wNFs <> "" Then
                Throw New Exception($"A(s) NF(s) abaixo estão sem o vinculo do registro de entrada e isso gera impacto no processo de automatização da DUE{vbCrLf}{wNFs}")
            End If

            Dim etiquetas As Integer
            Dim pendencias As Integer
            Dim totalItens As Integer

            totalItens = Banco.ExecuteScalar($"select count(*) from {Banco.BancoRedex}tb_talie_item where autonum_talie={idTalie}").ToInt()

            If totalItens <> 0 Then
                Sql = $"SELECT 
                                COUNT(*) FROM
                            {Banco.BancoRedex}TB_TALIE T
                            INNER JOIN 
                                {Banco.BancoRedex}TB_TALIE_ITEM TI ON T.AUTONUM_TALIE = TI.AUTONUM_TALIE
                            INNER JOIN 
                                {Banco.BancoRedex}ETIQUETAS E ON TI.AUTONUM_REGCS = E.AUTONUM_RCS
                            WHERE 
                                T.AUTONUM_TALIE = {idTalie}"
                etiquetas = Banco.ExecuteScalar(Sql).ToInt()


                Sql = $"SELECT 
                            COUNT(*) 
                        FROM
                            {Banco.BancoRedex}TB_TALIE T
                        INNER JOIN 
                            {Banco.BancoRedex}TB_TALIE_ITEM TI ON T.AUTONUM_TALIE = TI.AUTONUM_TALIE
                        INNER JOIN 
                            {Banco.BancoRedex}ETIQUETAS E ON TI.AUTONUM_REGCS = E.AUTONUM_RCS
                        WHERE 
                            T.AUTONUM_TALIE = {idTalie} AND EMISSAO IS NULL"

                pendencias = Banco.ExecuteScalar(Sql).ToInt()
            End If

            Dim etiquetasEpendencias = New DescargaValidacao With {
                .Etiquetas = etiquetas,
                .Pendencias = pendencias,
                .TotalItens = totalItens
            }

            Return JsonConvert.SerializeObject(etiquetasEpendencias)
        Catch ex As Exception
            Log.InserirLog("TalieColetorDescarga", ex.Message, ex.StackTrace, usuarioId)
            Throw New Exception(ex.Message)
        End Try

    End Function

    <WebMethod>
    Public Shared Function FinalizarTalie(idTalie As String, inicio As String, estufagemCompleta As String, tipoDescarga As String, conteinerId As String, conteiner As String, usuarioId As String, atualizarAlertaEtiqueta1 As Integer, atualizarAlertaEtiqueta2 As Integer, etiquetas As Integer, pendencias As Integer, totalItens As Integer) As String

        Dim Sql As String = String.Empty
        Sql = "Select FORMA_OPERACAO from tb_talie where autonum_talie = " & idTalie
        Dim forma_Operacao As String = Banco.ExecuteScalar(Sql)


        Using connection As SqlConnection = New SqlConnection(Banco.ConnectionString())
            connection.Open()
            Dim command As SqlCommand = connection.CreateCommand()
            Dim transaction As SqlTransaction
            transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted)
            command.Transaction = transaction

            Try
                '        command.CommandText = SQL1
                '        command.ExecuteNonQuery()
                '        command.CommandText = SQL2
                '        command.ExecuteNonQuery()
                '        transaction.Commit()
                '    Catch e As Exception
                '        transaction.Rollback()
                '    End Try
                'End Using


                Dim DataFechamento As String
                Dim Gate As Long
                Dim boo As Long
                'Dim listaSQLTransacao As New List(Of String)

                If Banco.ExecuteScalar($"select count(*) from {Banco.BancoRedex}tb_talie_item where autonum_talie={idTalie}").ToInt() <> 0 Then

                    'VERIFICA EMISSAO DE ETIQUETAS PARA DESCARGA ARMAZEM
                    If (OpcoesDescarga.DA.ToString().Equals(tipoDescarga)) Then

                        If (etiquetas = 0) Then
                            'If MsgBox("Não Consta geração de etiquetas deste registro" & vbCr & "Deseja continuar assim mesmo ?", vbQuestion + vbYesNo) = vbNo Then
                            '    MsgBox "Operação Cancelada"
                            '    Exit Sub
                            'End If
                            If (atualizarAlertaEtiqueta1 = 0) Then
                                Return String.Empty
                            End If
                            Banco.ExecuteScalar($"UPDATE REDEX.dbo.tb_TALIE SET ALERTA_ETIQUETA=1 WHERE AUTONUM_TALIE={idTalie}")
                        End If


                        If pendencias <> 0 Then
                            'If MsgBox("Consta pendencia de emissão de etiquetas deste registro" & vbCr & "Deseja continuar assim mesmo ?", vbQuestion + vbYesNo) = vbNo Then
                            '    MsgBox "Operação Cancelada"
                            '        Exit Sub
                            'End If
                            If (atualizarAlertaEtiqueta2 = 0) Then
                                Return String.Empty
                            End If
                            Banco.ExecuteScalar($"UPDATE REDEX.dbo.tb_TALIE SET ALERTA_ETIQUETA=2 WHERE AUTONUM_TALIE={idTalie}")
                        End If
                    End If


                    Dim rS As New ADODB.Recordset

                    Sql = $"SELECT 
                            d.autonum_bcg, 
                            b.qtde_descarga AS quantidade, 
                            b.autonum_emb,
                            b.autonum_pro, 
                            b.marca, 
                            b.qtde_estufagem, 
                            a.termino, 
                            b.comprimento, 
                            b.largura, 
                            b.altura, 
                            b.peso, 
                            c.autonum_nf, 
                            a.autonum_boo, 
                            b.autonum_ti,
                            a.autonum_gate, 
                            a.autonum_talie, 
                            b.yard,
                            b.armazem,
                            b.autonum_emb, 
                            d.autonum_emb as emb_reserva, 
                            etq.codproduto,
                            a.forma_operacao, 
                            b.imo,
                            b.uno, 
                            b.imo2,
                            b.uno2, 
                            b.imo3,
                            b.uno3, 
                            b.imo4,
                            b.uno4,
                            b.autonum_regcs,
                            B.CODIGO_CARGA,
                            boo.fcl_lcl,
                            boo.AUTONUM_PATIOS
                        FROM 
                            {Banco.BancoRedex}tb_talie a  
                        INNER JOIN 
                            {Banco.BancoRedex}tb_talie_item b ON a.autonum_talie = b.autonum_talie 
                        LEFT JOIN 
                            {Banco.BancoRedex}tb_notas_fiscais c ON b.autonum_nf = c.autonum_nf 
                        INNER JOIN 
                            {Banco.BancoRedex}tb_registro_cs e ON e.autonum_regcs = b.autonum_regcs 
                        INNER JOIN 
                            {Banco.BancoRedex}tb_booking_carga d ON d.autonum_bcg = e.autonum_bcg 
                        INNER JOIN 
                            {Banco.BancoRedex}tb_booking boo on d.autonum_boo=boo.autonum_boo
                        left join 
                            (select autonum_rcs, substring(codproduto,1,8) codproduto from {Banco.BancoRedex}etiquetas group by autonum_rcs, substring(codproduto,1,8)) etq on e.autonum_regcs = etq.autonum_rcs
                        where a.autonum_talie={idTalie}"

                    Dim itens = Banco.Consultar(Sql)
                    Dim dataRegistro As String
                    Dim balanca As Integer
                    If (itens IsNot Nothing) Then
                        If (itens.Rows.Count > 0) Then
                            'If (String.IsNullOrEmpty(itens.Rows(0)("termino").ToString())) Then
                            '    Throw New Exception("Grave a data de termino antes de finalizar")
                            'End If

                            dataRegistro = Banco.ExecuteScalar($"SELECT getdate()").ToDateTime.DataHoraMinutosSegundosFormatada() 'itens.Rows(0)("termino").ToString().ToDateTime().DataHoraMinutosSegundosFormatada()

                            Dim bruto = Banco.ExecuteScalar($"SELECT BRUTO FROM {Banco.BancoRedex}TB_GATE_NEW WHERE AUTONUM={itens.Rows(0)("Autonum_gate")}")
                            balanca = IIf(String.IsNullOrEmpty(bruto), 0, bruto)
                            Gate = itens.Rows(0)("Autonum_gate")

                            If Not IsDate(dataRegistro) Then
                                Throw New Exception("Data de termino invalida ou estava em branco quando o talie foi carregado. Grave a data de termino e carregue o talie novamente")
                            End If
                        End If
                    End If

                    For Each item In itens.Rows

                        Dim Id As Long
                        '                        Sql = "INSERT INTO REDEX.DBO.SEQ_TB_PATIO_CS (data) values (Convert(Char(10),GetDate(),121))"
                        Sql = "INSERT INTO REDEX.DBO.SEQ_TB_PATIO_CS (data) values (GetDate())"
                        Banco.ExecuteScalar(Sql)

                        Sql = "Select IDENT_CURRENT('REDEX.DBO.SEQ_TB_PATIO_CS')"

                        Id = CLng(Banco.ExecuteScalar(Sql))




                        Sql = $"INSERT INTO {Banco.BancoRedex}TB_PATIO_CS (
                                autonum_pcs,
                                AUTONUM_BCG,
                                QTDE_ENTRADA,
                                AUTONUM_EMB,
                                autonum_pro,
                                MARCA,
                                VOLUME_DECLARADO,
                                COMPRIMENTO,
                                LARGURA,
                                ALTURA,
                                BRUTO,
                                qtde_unit,
                                DT_PRIM_ENTRADA,
                                FLAG_HISTORICO,
                                AUTONUM_REGCS,
                                AUTONUM_NF,
                                talie_descarga,
                                QTDE_ESTUFAGEM,
                                YARD,
                                ARMAZEM,
                                AUTONUM_PATIOS,
                                PATIO,
                                imo,
                                uno,
                                imo2,
                                uno2,
                                imo3,
                                uno3,
                                imo4,
                                uno4,
                                codproduto,
                                codigo_carga
                            ) Values (
                            {Id},
                            {item("autonum_bcg").ToString()},
                            {item("quantidade").ToString().ToInt()},
                            {IIf(item("AUTONUM_EMB").ToString().ToInt() <> 0, item("AUTONUM_EMB").ToString(), item("emb_reserva").ToString())},
                            {item("AUTONUM_PRO").ToString().ToInt()},
                           '{IIf(String.IsNullOrEmpty(item("Marca").ToString()), "", item("Marca").ToString().ToInt())}',
                            {Replace(item("Comprimento").ToString() * item("Largura").ToString() * item("Altura").ToString(), ",", ".")},
                            {Replace(IIf(String.IsNullOrEmpty(item("Comprimento").ToString()), 0, item("Comprimento").ToString()), ",", ".")},
                            {Replace(IIf(String.IsNullOrEmpty(item("Largura").ToString()), 0, item("Largura").ToString()), ",", ".")},
                            {Replace(IIf(String.IsNullOrEmpty(item("Altura").ToString()), 0, item("Altura").ToString()), ",", ".")},
                            {PPonto(IIf(String.IsNullOrEmpty(item("Peso").ToString()), 0, item("Peso").ToString()))},
                            {IIf(IIf(String.IsNullOrEmpty(item("fcl_lcl").ToString()), 1, item("fcl_lcl").ToString()).Equals("FCL"), item("quantidade").ToString().ToInt(), "1")},
                            convert(DATETIME,'{dataRegistro}',103),
                            0,
                            {item("Autonum_Regcs").ToString().ToInt()},
                            {item("Autonum_NF").ToString().ToInt()},
                            {item("autonum_ti").ToString()},
                            {item("qtde_estufagem").ToString().ToInt()},
                            '{IIf(String.IsNullOrEmpty(item("Yard").ToString()), 1, item("Yard").ToString().ToInt())}',
                            {item("ARMAZEM").ToString().ToInt()},
                            {item("AUTONUM_PATIOS").ToString().ToInt()},
                            {item("AUTONUM_PATIOS").ToString().ToInt()},
                            '{IIf(String.IsNullOrEmpty(item("IMO").ToString()), "", item("IMO").ToString().ToInt())}',
                            '{IIf(String.IsNullOrEmpty(item("UNO").ToString()), "", item("UNO").ToString().ToInt())}',
                            '{IIf(String.IsNullOrEmpty(item("imo2").ToString()), "", item("imo2").ToString().ToInt())}',
                            '{IIf(String.IsNullOrEmpty(item("uno2").ToString()), "", item("uno2").ToString().ToInt())}',
                            '{IIf(String.IsNullOrEmpty(item("imo3").ToString()), "", item("imo3").ToString().ToInt())}',
                            '{IIf(String.IsNullOrEmpty(item("uno3").ToString()), "", item("uno3").ToString().ToInt())}',
                            '{IIf(String.IsNullOrEmpty(item("imo4").ToString()), "", item("imo4").ToString().ToInt())}',
                            '{IIf(String.IsNullOrEmpty(item("uno4").ToString()), "", item("uno4").ToString().ToInt())}',
                            '{IIf(String.IsNullOrEmpty(item("codproduto").ToString()), "", item("codproduto").ToString().ToInt())}',
                            '{IIf(String.IsNullOrEmpty(item("codigo_carga").ToString()), "", item("codigo_carga").ToString().ToInt())}'
                            )"

                        command.CommandText = Sql.ToUpper()
                        command.ExecuteNonQuery()


                        Dim imo = IIf(String.IsNullOrEmpty(item("IMO").ToString()), "", item("IMO").ToString())
                        If Not imo.Equals("") Then
                            command.CommandText = $"update {Banco.BancoRedex}TB_BOOKING_CARGA set imo='{imo}' where autonum_bcg={item("autonum_bcg")}"
                            command.ExecuteNonQuery()
                        End If

                        Sql = $"INSERT INTO {Banco.BancoSgipa}TB_CARGA_SOLTA_YARD (
                                Autonum_CS,
                                Armazem,
                                yard,
                                Origem,
                                DATA,
                                AUDIT_94,
                                FL_FRENTE,
                                FL_FUNDO,
                                FL_LE,
                                FL_LD
                            ) VALUES (
                                {Val(Id)},
                                {item("ARMAZEM").ToString().ToInt()},
                                '{IIf(String.IsNullOrEmpty(item("Yard").ToString()), 1, item("Yard").ToString().ToInt())}',
                                'R',
                                GETDATE(),
                                0,
                                0,
                                0,
                                0,
                                0
                            )"

                        command.CommandText = Sql.ToUpper()
                        command.ExecuteNonQuery()


                        Dim RsSeq As New ADODB.Recordset

                        Sql = "insert into redex.dbo.seq_tb_amr_gate (data) values (GetDate())"
                        Banco.ExecuteScalar(Sql)
                        Sql = "SELECT ident_current('redex.dbo.seq_tb_amr_gate')"
                        Dim idAMR As Long = CLng(Banco.ExecuteScalar(Sql))

                        Sql = $"insert into {Banco.BancoRedex}tb_amr_gate(
                                autonum,
                                gate,
                                cntr_rdx,
                                cs_rdx,
                                peso_entrada,
                                peso_saida,
                                data,
                                id_booking,
                                id_oc,
                                funcao_gate,
                                flag_historico
                            ) values (
                                {idAMR},
                                {item("Autonum_gate").ToString()},
                                0,
                                {Id},
                                {balanca},
                                0,
                                convert(DATETIME,'{dataRegistro}',103),
                                {item("autonum_boo").ToString()},
                                0,
                                203,
                                0
                            )"

                        command.CommandText = Sql.ToUpper()
                        command.ExecuteNonQuery()


                        command.CommandText = $"update {Banco.BancoRedex}TB_PATIO_CS SET PCS_PAI = {Id} where autonum_pcs ={Id}".ToUpper()
                        command.ExecuteNonQuery()

                        boo = item("autonum_boo").ToString()

                    Next

                End If

                Sql = $"SELECT 
                        COUNT(1) 
                    FROM 
                        {Banco.BancoRedex}tb_patio_cs pcs 
                    INNER JOIN 
                        {Banco.BancoRedex}tb_talie_item ti on pcs.talie_descarga = ti.autonum_ti
                    WHERE 
                        ti.autonum_talie = {idTalie}"


                command.CommandText = Sql
                Dim totalItensPatio = command.ExecuteNonQuery()

                If totalItensPatio.ToString().ToInt() = 0 Then
                    Throw New Exception($"Falha no processo de fechamento{vbCr}Carga não foi transferida para o estoque{vbCr}Contate o TI assim que possível")
                    'DB.RollbackTrans
                End If

                command.CommandText = $"update {Banco.BancoRedex}tb_talie set flag_fechado=1, dt_fechamento=getdate(),  TERMINO=getdate() where autonum_talie={idTalie}"
                command.ExecuteNonQuery()

                'verifica toda carga entrada da reserva
                Sql = $"SELECT 
                        sum(bcg.qtde) 
                    FROM 
                        {Banco.BancoRedex}tb_booking boo
                    inner join 
                        {Banco.BancoRedex}tb_booking_carga bcg on boo.autonum_boo = bcg.autonum_boo
                    where 
                        boo.autonum_boo = {boo} and bcg.flag_cs=1"

                'Dim QR = Banco.ExecuteScalar(Sql.ToUpper()).ToString().ToInt()
                command.CommandText = Sql
                Dim QR = command.ExecuteScalar()

                Sql = $"SELECT 
                        sum(pcs.qtde_entrada) 
                    FROM 
                        {Banco.BancoRedex}tb_booking boo
                    inner join 
                        tb_booking_carga bcg on boo.autonum_boo = bcg.autonum_boo
                    inner join 
                        tb_patio_cs pcs on bcg.autonum_bcg = pcs.autonum_bcg and boo.autonum_boo = {boo}"

                command.CommandText = Sql

                Dim QE = command.ExecuteNonQuery().ToString().ToInt()

                If QR = QE And QE <> 0 Then
                    command.CommandText = $"update REDEX.dbo.tb_booking set flag_finalizado=1 where autonum_boo = {boo}"
                    command.ExecuteNonQuery()


                End If

                command.CommandText = $"update REDEX.dbo.tb_booking set status_reserva=2 where autonum_boo = {boo}"
                command.ExecuteNonQuery()

                '=====================================

                '   Para casos de crossdocking gera estufagem automatica
                If (OpcoesDescarga.CD.ToString().Equals(tipoDescarga)) Then

                    If conteinerId = "" Or conteinerId = "0" Then
                        Sql = "SELECT AUTONUM_PATIO FROM " & Banco.BancoRedex & "TB_TALIE WHERE AUTONUM_TALIE=" & idTalie
                        command.CommandText = Sql
                        conteinerId = command.ExecuteScalar
                    End If


                    'If Combo1 = "" Then Sql = Sql & ",''"
                    'If Combo1 = "Manual" Then Sql = Sql & ",'M'"
                    'If Combo1 = "Mecanizada" Then Sql = Sql & ",'A'"
                    'If Combo1 = "Parcial" Then Sql = Sql & ",'P'"

                    'Sql = "Select FORMA_OPERACAO from tb_talie where autonum_talie = " & idTalie
                    'Dim forma_Operacao As String = UCase(Left(cbModo.Text, 1))


                    Sql = $"SELECT 
                            pcs.* 
                        FROM 
                            {Banco.BancoRedex}tb_patio_cs pcs 
                        inner join 
                            {Banco.BancoRedex}tb_talie_item ti on pcs.talie_descarga = ti.autonum_ti
                        where 
                            ti.autonum_talie = {idTalie} "
                    '                    Dim pcs As DataTable = Banco.Consultar(Sql)
                    '                    command.CommandText = Sql
                    Dim PCS As New DataTable
                    command.CommandText = Sql
                    Dim Rdr As SqlDataReader = command.ExecuteReader()
                    If Rdr.HasRows Then PCS.Load(Rdr)


                    'If pcs IsNot Nothing Then
                    '    If pcs.Rows.Count > 0 Then
                    command.CommandText = $"update {Banco.BancoRedex}tb_patio set ef='F' where autonum_patio = {conteinerId}".ToUpper()
                    command.ExecuteNonQuery()





                    '    End If
                    'End If

                    Dim Reserva_CC As Long
                    Sql = $"SELECT 
                            bcg.autonum_boo 
                        from 
                            {Banco.BancoRedex}tb_patio cc
                        inner join 
                            {Banco.BancoRedex}tb_booking_carga bcg on cc.autonum_bcg=bcg.autonum_bcg
                        where 
                            cc.autonum_patio={conteinerId}"

                    command.CommandText = Sql
                    Reserva_CC = command.ExecuteScalar.ToString().ToInt()

                    'Gera planejamento automatico
                    Dim ID As Long = CLng(Banco.ExecuteScalar($"select autonum_ro from {Banco.BancoRedex}tb_romaneio where autonum_patio={conteinerId}"))
                    If ID = 0 Then

                        Banco.ExecuteScalar("insert into seq_tb_romaneio (data) values (GetDate())")
                        ID = CLng(Banco.ExecuteScalar("SELECT ident_current('redex.dbo.seq_tb_romaneio')"))

                        Sql = $"INSERT INTO {Banco.BancoRedex}tb_romaneio (
                                autonum_ro,
                                data_inclusao,
                                usuario,
                                autonum_patio,
                                data_programacao,
                                obs,
                                autonum_boo,
                                VISIT_CODE,
                                DATA_AGENDAMENTO,
                                SEM_NF,
                                flag_historico,
                                crossdocking
                            ) values (
                                {ID},
                                getdate(),
                                {usuarioId},
                                {conteinerId},
                                getdate(),
                                '',
                                {Reserva_CC},
                                '',
                                NULL,
                                0,
                                1,
                                1
                            )"

                        command.CommandText = Sql
                        command.ExecuteNonQuery()
                    End If

                    For Each pc In PCS.Rows
                        Sql = $"INSERT INTO {Banco.BancoRedex}TB_ROMANEIO_CS (
                                autonum_rcs,
                                autonum_ro,
                                autonum_pcs, 
                                qtde, 
                                volume
                            ) values (
                                {Banco.ExecuteScalar("insert into seq_tb_romaneio_cs (data) values (GetDate())  SELECT ident_current('seq_tb_romaneio_cs') as IDrcs").ToString().ToInt()},
                                {ID},
                                {pc("autonum_pcs").ToString().ToInt()},
                                {pc("qtde_entrada").ToString().ToInt()},
                                0
                            )"

                        command.CommandText = Sql
                        command.ExecuteNonQuery()

                    Next
                    ' ------------------------------------------------------------------------------------


                    ' Gera Talie automatico
                    Dim TalieCarregamento As Long
                    Dim DtInicioEstuf As String
                    Dim DtTerminoEstuf As String


                    Sql = $"SELECT Min(inicio) FROM REDEX.DBO.TB_TALIE WHERE AUTONUM_PATIO={conteinerId} AND FLAG_DESCARGA=1"
                    command.CommandText = Sql
                    DtInicioEstuf = command.ExecuteScalar.ToString().ToDateTime().DataHoraMinutosSegundosFormatada()


                    Sql = $"SELECT Min(TERMINO) FROM REDEX.DBO.TB_TALIE WHERE AUTONUM_PATIO={conteinerId} AND FLAG_DESCARGA=1"
                    command.CommandText = Sql
                    DtTerminoEstuf = command.ExecuteScalar.ToString().ToDateTime().DataHoraMinutosSegundosFormatada()


                    Dim pEquip As String = "0"
                    Dim pConf As String = "0"
                    Dim pModo As String = "A"

                    Sql = $"SELECT EQUIPE FROM REDEX.DBO.TB_TALIE WHERE AUTONUM_PATIO={conteinerId} AND FLAG_DESCARGA=1"
                    command.CommandText = Sql
                    pEquip = command.ExecuteScalar.ToString()

                    Sql = $"SELECT CONFERENTE FROM REDEX.DBO.TB_TALIE WHERE AUTONUM_PATIO={conteinerId} AND FLAG_DESCARGA=1"
                    command.CommandText = Sql
                    pConf = command.ExecuteScalar.ToString()

                    Sql = $"SELECT forma_operacao FROM REDEX.DBO.TB_TALIE WHERE AUTONUM_PATIO={conteinerId} AND FLAG_DESCARGA=1"
                    command.CommandText = Sql
                    pModo = command.ExecuteScalar.ToString()

                    Sql = $"select COUNT(*) from {Banco.BancoRedex}tb_talie where autonum_patio={conteinerId} and flag_carregamento=1"
                    If command.ExecuteScalar.ToString.ToInt = 0 Then
                        Sql = $"INSERT INTO {Banco.BancoRedex}tb_talie (
                                autonum_patio,
                                inicio,
                                termino,
                                flag_estufagem,
                                crossdocking,
                                autonum_boo,
                                forma_operacao,
                                conferente,
                                equipe,
                                flag_descarga,
                                flag_carregamento,
                                obs,
                                autonum_ro,
                                autonum_gate,
                                flag_fechado
                            ) values (
                                {conteinerId},
                                convert(datetime,'{DtInicioEstuf}',103),
                                convert(datetime,'{DtTerminoEstuf}',103),
                                1,
                                1,
                                {Reserva_CC},
                                '{pModo}',
                                {pConf},
                                {pEquip},
                                0,
                                1,
                                '',
                                {ID},
                                0,
                                1
                        )"

                        command.CommandText = Sql.ToUpper()
                        command.ExecuteNonQuery()

                        'Banco.ExecuteScalar("insert into SEQ_TB_TALIE (data) values (GetDate())")
                        'TalieCarregamento = CLng(Banco.ExecuteScalar("SELECT ident_current('redex.dbo.SEQ_TB_TALIE')"))
                        TalieCarregamento = CLng(Banco.ExecuteScalar("SELECT ident_current('redex.dbo.TB_TALIE')"))

                        command.CommandText = $"update {Banco.BancoRedex}tb_romaneio set autonum_talie={TalieCarregamento} where autonum_ro = {ID}"
                        command.ExecuteNonQuery()
                    Else
                        Sql = $"update {Banco.BancoRedex}tb_talie set
                        inicio  = convert(datetime,'{DtInicioEstuf}',103),
                        termino = convert(datetime,'{DtTerminoEstuf}',103),
                        where autonum_patio={conteinerId} and flag_carregamento=1"

                        command.CommandText = Sql
                        command.ExecuteNonQuery()
                    End If

                    ' ----------------------------------------------------
                    For Each pc In PCS.Rows

                        Sql = $"INSERT INTO {Banco.BancoRedex}TB_AMR_NF_SAIDA (AUTONUM_PATIO, AUTONUM_NFI, QTDE_ESTUFADA) VALUES(
                            {conteinerId},{pc("Autonum_NF").ToString()},{pc("qtde_entrada").ToString()})"
                        command.CommandText = Sql
                        command.ExecuteNonQuery()

                        'atualiza o campo qtde estufada

                        Sql = $" UPDATE {Banco.BancoRedex}TB_NOTAS_ITENS SET QTDE_ESTUFADA = {pc("qtde_entrada").ToString().ToInt()}  WHERE AUTONUM_NFI = {pc("Autonum_NF").ToString()}"

                        command.CommandText = Sql
                        command.ExecuteNonQuery()


                        Dim iDcs As Long = 0
                        Sql = "insert into " & Banco.BancoRedex & "seq_saida_carga (data) values (GetDate())"
                        Banco.ExecuteScalar(Sql)

                        Sql = "SELECT ident_current('redex.dbo.seq_saida_carga')"
                        iDcs = CLng(Banco.ExecuteScalar(Sql))

                        Sql = $"INSERT INTO {Banco.BancoRedex}TB_SAIDA_CARGA (autonum_sc,AUTONUM_PCS
                            ,QTDE_SAIDA,AUTONUM_EMB,PESO_BRUTO,ALTURA,COMPRIMENTO,LARGURA,VOLUME,autonum_patio,ID_CONTEINER
                            ,MERCADORIA,DATA_ESTUFAGEM,autonum_nfi,autonum_talie,autonum_ro
                            ) VALUES (
                            {iDcs}, 
                            {pc("autonum_pcs").ToString()},
                            {pc("qtde_entrada").ToString().ToInt()},
                            {pc("AUTONUM_EMB").ToString()},
                            {PPonto(pc("bruto").ToString().ToInt())},
                            {PPonto(pc("Altura").ToString().ToInt())},
                            {PPonto(pc("Comprimento").ToString().ToInt())},
                            {PPonto(pc("Largura").ToString().ToInt())},
                            {PPonto(pc("VOLUME_declarado").ToString().ToInt())},
                            {conteinerId},
                            '{conteiner}',
                            '',
                            convert(datetime,'{DtInicioEstuf}',103),
                            {pc("Autonum_NF").ToString()},
                            {TalieCarregamento},
                            {ID}
                            )"
                        command.CommandText = Sql
                        command.ExecuteNonQuery()

                        Dim QS As Long
                        Sql = $"select sum(qtde_saida) from {Banco.BancoRedex}tb_saida_carga where autonum_pcs={pc("autonum_pcs")}"
                        command.CommandText = Sql
                        QS = CLng(command.ExecuteScalar().ToString())
                        If QS >= CLng(pc("qtde_entrada").ToString()) Then
                            command.CommandText = $"update {Banco.BancoRedex}tb_patio_cs set flag_historico=1 where autonum_pcs={pc("autonum_pcs").ToString()}"
                            command.ExecuteNonQuery()
                        End If
                    Next

                End If

                transaction.Commit()

                Return "Carga Transferida para o estoque"


            Catch ex As Exception
                transaction.Rollback()
                Log.InserirLog("TalieColetorDescarga", ex.Message, ex.StackTrace, usuarioId)
                Throw New Exception($"Erro durante  processo de Fechamento.{vbCr}Processo Cancelado!")
            End Try
        End Using

    End Function
    Protected Sub grdTalie_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdTalie.RowCommand

        Dim Index As Integer = e.CommandArgument

        Dim ID As String = Me.grdTalie.DataKeys(Index)("AUTONUM_REG").ToString()

        Select Case e.CommandName
            Case "EDITAR"
                Try
                    Dim idx = cbRegistro.Items.IndexOf(cbRegistro.Items.FindByValue(Val(ID)))
                    If (idx < 0) Then
                        cbRegistro.Items.Insert(cbRegistro.Items.Count, New ListItem(ID, ID))
                        idx = cbRegistro.Items.IndexOf(cbRegistro.Items.FindByValue(Val(ID)))
                    End If
                    cbRegistro.SelectedIndex = idx
                    If cbRegistro.SelectedValue > 0 Then
                        ObterDadosPorRegistro(cbRegistro.SelectedValue)
                    End If
                Catch ex As Exception
                    Log.InserirLog(Estufagem.ObterPagina(Page), ex.Message, ex.StackTrace, Page.Session("AUTONUMUSUARIO"))
                    ModelState.AddModelError(String.Empty, $"Ocorreu um erro ao habilitar item talie para edição: {ex.Message}")
                    Exit Sub
                End Try

        End Select

    End Sub

    Private Sub btnItemCancelar_Click(sender As Object, e As EventArgs) Handles btnItemCancelar.Click
        LimparTalieItens()
    End Sub

    Protected Sub btnPesquisarTalie_Click(sender As Object, e As EventArgs)
        ConsultarTalies(txtFiltroTalie.Text, txtFiltroRegistro.Text)


    End Sub
    Protected Sub btnConsultarTalies_Click(sender As Object, e As EventArgs)
        txtFiltroRegistro.Text = String.Empty
        txtFiltroTalie.Text = String.Empty

        ConsultarTalies(txtFiltroTalie.Text, txtFiltroRegistro.Text)
    End Sub

    Protected Sub cbModo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbModo.SelectedIndexChanged

    End Sub

    Protected Sub cbNF_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbNF.SelectedIndexChanged

    End Sub

    Protected Sub grdItens_SelectedIndexChanged(sender As Object, e As EventArgs) Handles grdItens.SelectedIndexChanged

    End Sub

    Protected Sub txtLocal_TextChanged(sender As Object, e As EventArgs) Handles txtLocal.TextChanged

    End Sub
End Class
