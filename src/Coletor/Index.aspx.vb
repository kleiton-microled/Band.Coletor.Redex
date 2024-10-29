Imports System.Security.Cryptography

Public Class Index
    Inherits System.Web.UI.Page

    Dim Login As New Login

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim MsgErro As String = String.Empty

        lblERRO.Text = ""
        txtUsuario.Focus()

        Me.Label7.Visible = False
        Me.txtNovaSenha.Visible = False
        Me.btSalvar.Visible = False

        Me.Label2.Text = "VERSAO " & My.Application.Info.Version.ToString

        If Not Page.IsPostBack Then

            If Request.QueryString("EXTERNO") IsNot Nothing And Request.QueryString("CONTENT") IsNot Nothing Then

                Dim pagina As String = Request.QueryString("EXTERNO")

                Dim conteudo As String = Request.QueryString("CONTENT")

                If (Not String.IsNullOrEmpty(conteudo) And Not String.IsNullOrEmpty(pagina)) Then
                    Dim usuarioId = Decrypt(conteudo)
                    Session("AUTONUMUSUARIO") = usuarioId
                    Dim sql = $"SELECT 
                                    ISNULL(A.USUARIO,' ') USUARIO,
                                    ISNULL(A.SENHA,' ') SENHA,
                                    ISNULL(B.DESCR_RESUMIDO,' ') DESCR_RESUMIDO
                                FROM 
                                    {Banco.BancoRedex}TB_CAD_USUARIOS A 
                                LEFT JOIN 
                                    {Banco.BancoOperador}TB_PATIOS B ON A.PATIO = B.AUTONUM  
                                LEFT JOIN 
                                    {Banco.BancoRedex}TB_EMPRESAS C ON A.COD_EMPRESA = C.AUTONUM  
                                WHERE 
                                    A.AUTONUM_USU='{usuarioId}'"

                    Dim dtUsuario = Banco.Consultar(sql)
                    If dtUsuario IsNot Nothing Then
                        Dim patio = dtUsuario(0)("DESCR_RESUMIDO")
                        Dim usuario = dtUsuario(0)("USUARIO")
                        Dim senha = dtUsuario(0)("SENHA")
                        ConfiguracoesIniciais(usuario, senha, patio)
                    End If
                    Response.Redirect(pagina.Replace(";", "&"))
                End If
            Else
                Dim pagina As String = Login.ObterURIMenu()
                Response.Redirect(pagina)
            End If

            If Request.QueryString("err") IsNot Nothing Then

                MsgErro = Request.QueryString("err").ToString()

                If MsgErro.Equals("1") Then
                    txtUsuario.Focus()
                End If

                If MsgErro.Equals("2") Then
                    lblERRO.Text = Session("ERROLOGIN")

                    If Session("ERROLOGIN") = "SENHA EXPIRADA" Then
                        Me.txtUsuario.Text = Session("USUARIO")
                        txtUsuario.Enabled = False
                        Me.Label7.Visible = True
                        Me.txtNovaSenha.Visible = True
                        Me.btSalvar.Visible = True
                    End If

                End If

            End If
        End If

    End Sub

    Private Sub EscreverCookies()

        Dim Cookie1 As New HttpCookie("LOGADO")
        Cookie1.Values.Add("LOGADO", Session("LOGADO").ToString())
        Cookie1.Expires = DateTime.Now.AddDays(1)
        Response.Cookies.Add(Cookie1)

        Dim Cookie2 As New HttpCookie("USUARIO")
        Cookie1.Values.Add("USUARIO", Session("USUARIO").ToString())
        Cookie2.Expires = DateTime.Now.AddDays(1)
        Response.Cookies.Add(Cookie2)

        Dim Cookie3 As New HttpCookie("SENHA")
        Cookie1.Values.Add("SENHA", Session("SENHA").ToString())
        Cookie3.Expires = DateTime.Now.AddDays(1)
        Response.Cookies.Add(Cookie3)

        Dim Cookie4 As New HttpCookie("EMPRESA")
        Cookie4.Values.Add("EMPRESA", Session("EMPRESA").ToString())
        Cookie4.Expires = DateTime.Now.AddDays(1)
        Response.Cookies.Add(Cookie4)

        Dim Cookie5 As New HttpCookie("PATIO")
        Cookie5.Values.Add("PATIO", Session("PATIO").ToString())
        Cookie5.Expires = DateTime.Now.AddDays(1)
        Response.Cookies.Add(Cookie5)

        Dim Cookie6 As New HttpCookie("AUTONUMPATIO")
        Cookie6.Values.Add("AUTONUMPATIO", Session("AUTONUMPATIO").ToString())
        Cookie6.Expires = DateTime.Now.AddDays(1)
        Response.Cookies.Add(Cookie6)

        Dim Cookie7 As New HttpCookie("AUTONUMUSUARIO")
        Cookie7.Values.Add("AUTONUMUSUARIO", Session("AUTONUMUSUARIO").ToString())
        Cookie7.Expires = DateTime.Now.AddDays(1)
        Response.Cookies.Add(Cookie7)

        Dim Cookie8 As New HttpCookie("BROWSER_NAME")
        Cookie8.Values.Add("BROWSER_NAME", Session("BROWSER_NAME").ToString())
        Cookie8.Expires = DateTime.Now.AddDays(1)
        Response.Cookies.Add(Cookie8)

        Dim Cookie9 As New HttpCookie("BROWSER_VERSION")
        Cookie9.Values.Add("BROWSER_VERSION", Session("BROWSER_VERSION").ToString())
        Cookie9.Expires = DateTime.Now.AddDays(1)
        Response.Cookies.Add(Cookie9)

        Dim Cookie10 As New HttpCookie("MobileDeviceModel")
        Cookie10.Values.Add("MobileDeviceModel", Session("MobileDeviceModel").ToString())
        Cookie10.Expires = DateTime.Now.AddDays(1)
        Response.Cookies.Add(Cookie10)

        Dim Cookie11 As New HttpCookie("MobileDeviceManufacturer")
        Cookie11.Values.Add("MobileDeviceManufacturer", Session("MobileDeviceManufacturer").ToString())
        Cookie11.Expires = DateTime.Now.AddDays(1)
        Response.Cookies.Add(Cookie11)

        Dim Cookie12 As New HttpCookie("IsMobileDevice")
        Cookie12.Values.Add("IsMobileDevice", Session("IsMobileDevice").ToString())
        Cookie12.Expires = DateTime.Now.AddDays(1)
        Response.Cookies.Add(Cookie12)

        Dim Cookie15 As New HttpCookie("FLAG_REDEX_SEM_MARCANTE")
        Cookie15.Values.Add("FLAG_REDEX_SEM_MARCANTE", Session("FLAG_REDEX_SEM_MARCANTE").ToString())
        Cookie15.Expires = DateTime.Now.AddDays(1)
        Response.Cookies.Add(Cookie15)

        Dim Cookie16 As New HttpCookie("LARGURATELA")
        Cookie16.Values.Add("LARGURATELA", Session("LARGURATELA").ToString())
        Cookie16.Expires = DateTime.Now.AddDays(1)
        Response.Cookies.Add(Cookie16)

    End Sub

    Protected Sub btLogin_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btLogin.Click

        Dim strValidaVersao As String = ""
        Dim strValidaUsuario As String = ""

        Session("AUTONUMUSUARIO") = Login.EfetuarLogin(txtUsuario.Text.ToUpper(), txtSenha.Text)

        If Session("AUTONUMUSUARIO") > 0 Then

            ConfiguracoesIniciais(txtUsuario.Text.ToUpper(), txtSenha.Text, txtPatio.Text)

            Response.Redirect("Menu.aspx?pg=1")
        Else
            Response.Redirect("Index.aspx?err=1")
        End If

    End Sub

    Private Sub ConfiguracoesIniciais(usuario As String, senha As String, patio As String)

        Session("LOGADO") = True
        Session("USUARIO") = usuario
        Session("SENHA") = senha
        Dim sql = $"SELECT
                        ISNULL(B.DESCR_RESUMIDO, ' ') As DESCR_RESUMIDO,
                        ISNULL(C.RAZAO_SOCIAL, ' ') As RAZAO_SOCIAL,
                        ISNULL(B.AUTONUM, 0) As AUTONUM
                    FROM
                        {Banco.BancoRedex}TB_CAD_USUARIOS A
                    LEFT JOIN
                        {Banco.BancoOperador}TB_PATIOS B ON A.PATIO = B.AUTONUM
                    LEFT JOIN
                        {Banco.BancoRedex}TB_EMPRESAS C ON A.COD_EMPRESA = C.AUTONUM
                    WHERE
                        A.USUARIO='{usuario.ToUpper()}'"
        Dim dtUsuario = Banco.Consultar(sql)

        If dtUsuario IsNot Nothing Then
            If dtUsuario.Rows.Count > 0 Then
                Session("PATIO") = dtUsuario.Rows(0)("DESCR_RESUMIDO").ToString()
                Session("AUTONUMPATIO") = dtUsuario.Rows(0)("AUTONUM").ToString()
                Session("EMPRESA") = patio
            End If
        End If
        Me.txtPatio.Text = Session("PATIO")
        txtEmpresa.Text = Session("EMPRESA")

        Session("FLAG_REDEX_SEM_MARCANTE") = 0

        Dim tamTela As Integer = 800
        Try
            If Request.Form("HiddenField1") IsNot Nothing Then
                tamTela = Integer.Parse(Request.Form("HiddenField1").ToString())
            End If
        Catch
        End Try

        Session("LARGURATELA") = tamTela

        With Request.Browser
            Session("BROWSER_NAME") = .Browser
            Session("BROWSER_VERSION") = .Version
            Session("MobileDeviceModel") = .MobileDeviceModel
            Session("MobileDeviceManufacturer") = .MobileDeviceManufacturer
            Session("IsMobileDevice") = .IsMobileDevice

        End With

        EscreverCookies()
    End Sub

    Protected Sub txtUsuario_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtUsuario.TextChanged

        Me.txtUsuario.Text = UCase(Me.txtUsuario.Text)
        If Not txtUsuario.Text = String.Empty Then

            Dim sql = $"SELECT
                            B.DESCR_RESUMIDO,
                            C.RAZAO_SOCIAL,
                            B.AUTONUM
                        FROM
                            {Banco.BancoRedex}TB_CAD_USUARIOS A
                        LEFT JOIN
                            {Banco.BancoRedex}TB_PATIOS B ON A.PATIO = B.AUTONUM
                        LEFT JOIN
                            {Banco.BancoRedex}TB_EMPRESAS C ON A.COD_EMPRESA = C.AUTONUM
                        WHERE
                            A.USUARIO='{txtUsuario.Text.ToUpper()}'"

            Dim dtUsuario = Banco.Consultar(sql)

            If dtUsuario IsNot Nothing Then
                If dtUsuario.Rows.Count > 0 Then
                    txtPatio.Text = dtUsuario.Rows(0)("DESCR_RESUMIDO").ToString()
                    If txtPatio.Text <> String.Empty Then
                        Login.AutonumPatio = dtUsuario.Rows(0)("AUTONUM").ToString()
                        Login.NomePatio = txtPatio.Text
                        Login.NomeUsuario = txtUsuario.Text.ToUpper
                        txtEmpresa.Text = dtUsuario.Rows(0)("RAZAO_SOCIAL").ToString()
                        txtSenha.Focus()
                        btLogin.Enabled = True
                    End If
                Else
                    txtUsuario.Text = ""
                    txtEmpresa.Text = ""
                End If
            End If

        End If

    End Sub

    Protected Sub txtEmpresa_TextChanged(sender As Object, e As EventArgs) Handles txtEmpresa.TextChanged

    End Sub

    Protected Sub txtPatio_TextChanged(sender As Object, e As EventArgs) Handles txtPatio.TextChanged

    End Sub

    Protected Sub txtSenha_TextChanged(sender As Object, e As EventArgs) Handles txtSenha.TextChanged

    End Sub

    Protected Sub btSalvar_Click(sender As Object, e As EventArgs) Handles btSalvar.Click
        Dim StrTrocaSenha As String
        'StrTrocaSenha = ControleAcesso.alterarSenha(Me.txtNovaSenha.Text, "" & BD.BancoOperador & "TB_CAD_USUARIOS", Me.txtUsuario.Text.ToUpper)
        'If StrTrocaSenha <> "OK" Then
        '    Session("ERROLOGIN") = StrTrocaSenha
        '    Response.Redirect("Index.aspx?err=2")
        'Else
        '    txtUsuario.Enabled = True
        '    Me.txtUsuario.Text = ""
        '    Me.Label7.Visible = False
        '    Me.txtNovaSenha.Visible = False
        '    Me.btSalvar.Visible = False
        '    Me.txtNovaSenha.Text = ""
        '    ScriptManager.RegisterClientScriptBlock(Me, [GetType](), "script", "<script>alert('Senha alterada com sucesso!');</script>", False)
        'End If
    End Sub

    Protected Sub txtNovaSenha_TextChanged(sender As Object, e As EventArgs) Handles txtNovaSenha.TextChanged

    End Sub

    Private Sub form1_Load(sender As Object, e As EventArgs) Handles form1.Load

    End Sub

    Private Function Decrypt(conteudo As String) As String
        Dim data As Byte() = Convert.FromBase64String(conteudo)

        Using md5 As MD5CryptoServiceProvider = New MD5CryptoServiceProvider()
            Dim keys As Byte() = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes("MICROLED"))

            Using tripDes As TripleDESCryptoServiceProvider = New TripleDESCryptoServiceProvider() With {
                .Key = keys,
                .Mode = CipherMode.ECB,
                .Padding = PaddingMode.PKCS7
            }
                Dim transform As ICryptoTransform = tripDes.CreateDecryptor()
                Dim results As Byte() = transform.TransformFinalBlock(data, 0, data.Length)
                Return UTF8Encoding.UTF8.GetString(results)
            End Using
        End Using

    End Function

End Class