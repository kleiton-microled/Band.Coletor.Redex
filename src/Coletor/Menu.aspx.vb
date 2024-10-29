Public Class WebForm2

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
                ScriptManager.RegisterClientScriptBlock(Me, [GetType](), "script", "<script>alert('" & ex.Message & "');</script>", False)
                ' Response.Redirect("Index.aspx")
            End Try
        End If

        ' Descomentar quando for enviar par ao cliente

        'Me.btIni6.Visible = Login.Valida_Acesso_Botao(Session("USUARIO"), "BTDESCARGA_EXP")
        'Me.btIni.Visible = Login.Valida_Acesso_Botao(Session("USUARIO"), "BTDESOVA")
        'Me.btIni5.Visible = Login.Valida_Acesso_Botao(Session("USUARIO"), "BTESTUFAGEM")
        'Me.btIni3.Visible = Login.Valida_Acesso_Botao(Session("USUARIO"), "BTIDENTIFICA_LOTE")
        'Me.btIni4.Visible = Login.Valida_Acesso_Botao(Session("USUARIO"), "BTMARCANTES")
        'Me.btIni0.Visible = Login.Valida_Acesso_Botao(Session("USUARIO"), "BTMOVIMENTA_CS")
        'Me.btIni2.Visible = Login.Valida_Acesso_Botao(Session("USUARIO"), "BTMOVIMENTA_CNTR")
        'Me.btIni7.Visible = Login.Valida_Acesso_Botao(Session("USUARIO"), "BTCARREGAMENTO")
        'Me.btIni8.Visible = Login.Valida_Acesso_Botao(Session("USUARIO"), "BTINVENTARIO")



        txtOpcao.Focus()

    End Sub

    Protected Sub btIni_Click(sender As Object, e As EventArgs) Handles btIni.Click
        'Response.Redirect("DesovaDatas.aspx")
        Response.Redirect("TalieColetorDescarga.aspx?dd=0&cd=1")

    End Sub

    Protected Sub btIni0_Click(sender As Object, e As EventArgs) Handles btIni0.Click
        'If Session("FLAG_OB_MARCANTE") = 0 Then
        Response.Redirect("InventarioCS.aspx")
        'Else
        ' Response.Redirect("InventarioCSF2_new.aspx")
        'End If

    End Sub

    Protected Sub btIni1_Click(sender As Object, e As EventArgs) Handles btIni1.Click

        Try
            Session.Abandon()
            Session.RemoveAll()
            HttpContext.Current.Request.Cookies.Clear()
        Catch ex As Exception
        End Try

        Response.Redirect("Index.aspx")

    End Sub

    Private Sub form1_Load(sender As Object, e As System.EventArgs) Handles form1.Load
        txtTerminal.Text = Session("PATIO")
        txtUsuario.Text = Session("USUARIO")
    End Sub

    Protected Sub btIni3_Click(sender As Object, e As EventArgs) Handles btIni3.Click
        Session("AUTONUMCNTR") = 0
        Response.Redirect("Operacao.aspx")
    End Sub

    Protected Sub btIni2_Click(sender As Object, e As EventArgs) Handles btIni2.Click
        Response.Redirect("InventarioCNTR.aspx")
    End Sub

    Protected Sub btIni4_Click(sender As Object, e As EventArgs) Handles btIni4.Click
        Response.Redirect("frmMarcante.aspx")
    End Sub

    Protected Sub btIni5_Click(sender As Object, e As EventArgs) Handles btIni5.Click
        Response.Redirect("Estufagem.aspx")
    End Sub

    Protected Sub btIni6_Click(sender As Object, e As EventArgs) Handles btIni6.Click
        'Response.Redirect("Talie_Descarga_Registro.aspx")
        'Response.Redirect("TalieColetorDescarga.aspx")
        Response.Redirect("TalieColetorDescarga.aspx?dd=1&cd=0")
    End Sub

    Protected Sub btIni7_Click(sender As Object, e As EventArgs) Handles btIni7.Click
        Response.Redirect("CarregaCS.aspx")
    End Sub

    Protected Sub btIni8_Click(sender As Object, e As EventArgs) Handles btIni8.Click
        Response.Redirect("InventarioCego.aspx")
    End Sub

    Protected Sub txtOpcao_TextChanged(sender As Object, e As EventArgs) Handles txtOpcao.TextChanged
        If txtOpcao.Text = "1" Then
            If Me.btIni7.Visible = True And Me.btIni7.Enabled = True Then
                Response.Redirect("CarregaCS.aspx")
            End If
        ElseIf txtOpcao.Text = "2" Then
            If Me.btIni6.Visible = True And Me.btIni6.Enabled = True Then
                'Response.Redirect("Talie/TalieDescargaRegistro.aspx?patio=" & Session("AUTONUMPATIO").ToString() & "&usuario=" & Session("AUTONUMUSUARIO").ToString())
                Response.Redirect("Talie_Descarga_Registro.aspx")
            End If
        ElseIf txtOpcao.Text = "3" Then
            If Me.btIni.Visible = True And Me.btIni.Enabled = True Then
                Session("FLAG_DDC") = 0
                Response.Redirect("DesovaDatas.aspx")
            End If
        ElseIf txtOpcao.Text = "4" Then
            If Me.btIni5.Visible = True And Me.btIni5.Enabled = True Then
                Response.Redirect("Estufagem.aspx")
            End If
        ElseIf txtOpcao.Text = "5" Then
            If Me.btIni3.Visible = True And Me.btIni3.Enabled = True Then
                Session("AUTONUMCNTR") = 0
                Response.Redirect("Operacao.aspx")
            End If
        ElseIf txtOpcao.Text = "6" Then
            If Me.btIni8.Visible = True And Me.btIni8.Enabled = True Then
                Response.Redirect("InventarioCego.aspx")
            End If
        ElseIf txtOpcao.Text = "7" Then
            If Me.btIni4.Visible = True And Me.btIni4.Enabled = True Then
                Response.Redirect("frmMarcante.aspx")
            End If
        ElseIf txtOpcao.Text = "8" Then
            If Me.btIni0.Visible = True And Me.btIni0.Enabled = True Then
                'If Session("FLAG_OB_MARCANTE") = 0 Then
                Response.Redirect("InventarioCS.aspx")
                'Else
                ' Response.Redirect("InventarioCSF2_new.aspx")
                'End If
            End If
        ElseIf txtOpcao.Text = "9" Then
            If Me.btIni2.Visible = True And Me.btIni2.Enabled = True Then
                Response.Redirect("InventarioCNTR.aspx")
            End If
        ElseIf txtOpcao.Text = "10" Then
            If Me.btIni.Visible = True And Me.btIni.Enabled = True Then
                Session("FLAG_DDC") = 1
                Response.Redirect("DesovaDatas.aspx")
            End If
        ElseIf txtOpcao.Text = "0" Then
            Try
                Session.Abandon()
                Session.RemoveAll()
                HttpContext.Current.Request.Cookies.Clear()
            Catch ex As Exception
            End Try

            Response.Redirect("Index.aspx")
        End If

    End Sub

    Protected Sub btIni9_Click(sender As Object, e As EventArgs) Handles btIni9.Click
        Session("FLAG_DDC") = 1
        Response.Redirect("DesovaDatas.aspx")
    End Sub

    'Protected Sub btIniTalie_Click(sender As Object, e As EventArgs) Handles btIniTalie.Click
    '    Session("AUTONUMCNTR") = 0
    '    Response.Redirect("OperacaoSemCores.aspx")
    'End Sub
End Class