Public Class Login

    Private _ConsultaReefer As Integer
    Public Shared AutonumPatio As Integer
    Public Shared NomePatio As String
    Public Shared NomeUsuario As String



    Public Property ConsultaReefer() As String
        Get
            Return _ConsultaReefer
        End Get
        Set(ByVal value As String)
            _ConsultaReefer = value
        End Set
    End Property

    Public Function EfetuarLogin(ByVal Usuario As String, ByVal Senha As String) As Long

        Try

            'If BD.ControleSenha = "0" Then
            If Senha <> "" Then
                Dim sql = $"SELECT AUTONUM_USU FROM {Banco.BancoRedex}TB_CAD_USUARIOS WHERE USUARIO='{Usuario}' AND SENHA='{Senha}' AND FLAG_ATIVO=1"

                Return Banco.ExecuteScalar(sql)
            Else
                Dim sql = $"SELECT AUTONUM_USU FROM {Banco.BancoRedex}TB_CAD_USUARIOS WHERE USUARIO='{Usuario}'  AND FLAG_ATIVO=1"
                Return Banco.ExecuteScalar(sql)
            End If

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

    End Function

    Public Shared Function Valida_Acesso_Botao(QualUsuario As String, QualNomeBotao As String) As Boolean
        Dim Sql As String = String.Empty

        Sql = "SELECT 1 "
        Sql = Sql & " FROM"
        Sql = Sql & " " & Banco.BancoOperador & "TB_SYS_FUNCOES"
        Sql = Sql & " ," & Banco.BancoOperador & "TB_SYS_GRP_PERMISSOES"
        Sql = Sql & " ," & Banco.BancoOperador & "TB_SYS_USER_GRUPOS "
        Sql = Sql & " ," & Banco.BancoOperador & "TB_CAD_USUARIOS "
        Sql = Sql & " WHERE"
        Sql = Sql & "((tb_cad_usuarios.autonum = tb_sys_user_grupos.autonumuser) "
        Sql = Sql & " AND (tb_sys_funcoes.codfunc = tb_sys_grp_permissoes.codfunc) "
        Sql = Sql & " AND (tb_sys_grp_permissoes.codgrupo = tb_sys_user_grupos.codgrupo) "
        Sql = Sql & " AND (UPPER(tb_cad_usuarios.usuario) = '" & QualUsuario & "') "
        Sql = Sql & " AND (tb_sys_funcoes.sistema = 'COLETORWEB') "
        Sql = Sql & " AND (UPPER(tb_sys_funcoes.Nomeobj) = '" & QualNomeBotao & "')) "
        Sql = Sql & " and (tb_sys_grp_permissoes.codtipoperm=5) "


        Dim tbUA = Banco.Consultar(Sql)
        If tbUA IsNot Nothing Then
            Return tbUA.Rows.Count > 0
        End If

        Return False
    End Function


    Public Function UsuarioConsultaReefer(ByVal Usuario As String) As Boolean

        Dim sql = $"SELECT 
                        ISNULL(FLAG_CON_REEFER,0) FLAG_CON_REEFER 
                    FROM 
                        {Banco.BancoOperador}TB_CAD_USUARIOS 
                    WHERE 
                        USUARIO='{Usuario}'"
        Dim Rst = Banco.Consultar(sql)


        Return Banco.ExecuteScalar(sql).ToInt() = 1


    End Function

    Public Function ConsultarPatio(ByVal Usuario As String) As String


        dim sql = $"SELECT 
                        B.DESCR_RESUMIDO 
                    FROM 
                        {Banco.BancoOperador}TB_CAD_USUARIOS A 
                    LEFT JOIN 
                        {Banco.BancoOperador}TB_PATIOS B ON A.PATIO_COLETOR = B.AUTONUM 
                    WHERE 
                        A.USUARIO='{Usuario}'"

        Return Banco.ExecuteScalar(sql)

    End Function

    Public Function ConsultarAutonumPatio(ByVal Usuario As String) As String

        Dim sql = $"SELECT 
                        B.AUTONUM 
                    FROM 
                        {Banco.BancoOperador}TB_CAD_USUARIOS A 
                    LEFT JOIN 
                        {Banco.BancoOperador}TB_PATIOS B ON A.PATIO_COLETOR = B.AUTONUM 
                    WHERE 
                        A.USUARIO='{Usuario}'"

        Return Banco.ExecuteScalar(sql)

    End Function

    Public Function ConsultarEmpresa(ByVal Usuario) As String


        Dim sql = $"SELECT 
                        B.RAZAO_SOCIAL 
                    FROM 
                        {Banco.BancoOperador}TB_CAD_USUARIOS A 
                    LEFT JOIN 
                        {Banco.BancoRedex}TB_EMPRESAS B ON A.COD_EMPRESA = B.AUTONUM 
                    WHERE 
                        A.USUARIO='{Usuario}'"

        Return Banco.ExecuteScalar(sql)


    End Function
    Public Shared Function ObterURIMenu() As String
        Return ConfigurationManager.AppSettings("ColetorSitePath")
    End Function

End Class
