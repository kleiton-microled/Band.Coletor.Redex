Imports System.Data.SqlClient

Public Class BD

    Private Shared _Servidor As String
    Private Shared _Schema As String
    Private Shared _Usuario As String
    Private Shared _Senha As String
    Private Shared _BancoEmUso As String
    Private Shared _BancoOperador As String
    Private Shared _BancoSgipa As String
    Private Shared _Conexao As ADODB.Connection
    Private Shared _Proximo_ID As Long

    Public Shared LinhasAfetadas As Integer



    Public Shared Property Servidor() As String
        Get
            Return _Servidor
        End Get
        Set(ByVal value As String)
            _Servidor = value
        End Set
    End Property

    Public Shared Property Schema() As String
        Get
            Return _Schema
        End Get
        Set(ByVal value As String)
            _Schema = value
        End Set
    End Property

    Public Shared Property Usuario() As String
        Get
            Return _Usuario
        End Get
        Set(ByVal value As String)
            _Usuario = value
        End Set
    End Property

    Public Shared Property Senha() As String
        Get
            Return _Senha
        End Get
        Set(ByVal value As String)
            _Senha = value
        End Set
    End Property

    Public Shared Property BancoEmUso() As String
        Get
            Return _BancoEmUso
        End Get
        Set(ByVal value As String)
            _BancoEmUso = value
        End Set
    End Property

    Public Shared ReadOnly Property BancoOperador() As String
        Get
            If BancoEmUso = "ORACLE" Then
                Return "OPERADOR."
            Else
                Return "OPERADOR.DBO."
            End If
        End Get
    End Property

    Public Shared ReadOnly Property BancoSgipa() As String
        Get
            If BancoEmUso = "ORACLE" Then
                Return "SGIPA."
            Else
                Return "SGIPA.DBO."
            End If
        End Get
    End Property

    Public Shared ReadOnly Property BancoRedex() As String
        Get
            If BancoEmUso = "ORACLE" Then
                Return "REDEX."
            Else
                Return ConfigurationManager.AppSettings("BANCO_REDEX")
            End If
        End Get
    End Property

    Public Shared ReadOnly Property StringConexao
        Get
            If BancoEmUso = "ORACLE" Then
                Return String.Format("Provider=OraOLEDB.ORACLE.1;Data Source={0};User ID={1};Password={2};Unicode=True;PLSQLRSet=true", Servidor, Usuario, Senha)
            Else
                Return String.Format("Provider=SQLOLEDB.1;Persiste Security Info = false; Data Source={0};Initial Catalog={1};User ID={2};Password={3}", Servidor, Schema, Usuario, Senha)
                'Return String.Format("Data Source={0};Initial Catalog={1};User ID={2};Password={3}", Servidor, Schema, Usuario, Senha)
            End If
        End Get
    End Property


    Public Shared ReadOnly Property StringConexaoSemProvider
        Get
            If BancoEmUso = "ORACLE" Then
                Return String.Format("Provider=OraOLEDB.ORACLE.1;Data Source={0};User ID={1};Password={2};Unicode=True;PLSQLRSet=true", Servidor, Usuario, Senha)
            Else
                'Return String.Format("Provider=SQLOLEDB.1;Data Source={0};Initial Catalog={1};User ID={2};Password={3}", Servidor, Schema, Usuario, Senha)
                Return String.Format("Data Source={0};Initial Catalog={1};User ID={2};Password={3}", Servidor, Schema, Usuario, Senha)
            End If
        End Get
    End Property

    Public Shared Function Conexao() As ADODB.Connection
        Try
            If _Conexao Is Nothing Then
                _Conexao = New ADODB.Connection
                _Conexao.CursorLocation = ADODB.CursorLocationEnum.adUseClient
                _Conexao.Open(StringConexao)
            End If
            Return _Conexao
        Catch ex As Exception
            _Conexao = New ADODB.Connection
            _Conexao.CursorLocation = ADODB.CursorLocationEnum.adUseClient
            _Conexao.Open(StringConexao)
            Return _Conexao
        End Try

    End Function
    Public Shared Function Proximo_ID(Base As String, Origem As String, Optional Ultimo_ID As Boolean = True) As Long

        Dim pSQL As String
        If Not Ultimo_ID Then
            If BancoEmUso = "ORACLE" Then
                pSQL = "SELECT count(*) from " & Base & "sysobjects where name='" & Origem & "' and type='U'"
                If BD.Conexao.Execute(pSQL).Fields(0).Value Then
                    pSQL = "create table " & Base & Origem & " (NEXTVAL  NUMERIC(6) IDENTITY(1,1),Data datetime)"
                    BD.Conexao.Execute(pSQL)
                End If
                pSQL = "INSERT INTO " & Base & Origem & " (Data) values (getdate())"
                BD.Conexao.Execute(pSQL)
                pSQL = "SELECT IDENT_CURRENT('" & Base & Origem & "') AS ID"
                Proximo_ID = BD.Conexao.Execute(pSQL).Fields(0).Value
            Else
                If Right(Base, 1) <> "." Then Base = Base & "."
                pSQL = "SELECT count(*) from " & Base & "sysobjects where name='" & Origem & "' and type='U'"
                If BD.Conexao.Execute(pSQL).Fields(0).Value = 0 Then
                    pSQL = "create table " & Base & Origem & " (Nextval numeric(6) IDENTITY(1,1), data datetime)"
                    BD.Conexao.Execute(pSQL)
                End If
                pSQL = "insert into " & Base & Origem & " (data) values (getdate())"
                BD.Conexao.Execute(pSQL)

                pSQL = "SELECT IDENT_CURRENT('" & Base & Origem & "') AS ID"
                Proximo_ID = BD.Conexao.Execute(pSQL).Fields(0).Value
            End If
        Else
            pSQL = "SELECT IDENT_CURRENT('" & Base & Origem & "') AS ID"
            Proximo_ID = BD.Conexao.Execute(pSQL).Fields(0).Value
        End If



    End Function
    Public Shared Function Proximo_ID_DATE(Base As String, Origem As String, Optional Ultimo_ID As Boolean = True) As Long

        Dim pSQL As String
        If Not Ultimo_ID Then
            If Right(Base, 1) <> "." Then Base = Base & "."
            pSQL = "SELECT count(*) from " & Base & "sysobjects where name='" & Origem & "' and type='U'"

            pSQL = "insert into " & Base & Origem & " (data) values (Convert(Char(10),GetDate(),121))"
            BD.Conexao.Execute(pSQL)
            pSQL = "SELECT IDENT_CURRENT('" & Base & Origem & "') AS ID"
            Proximo_ID_DATE = BD.Conexao.Execute(pSQL).Fields(0).Value
        Else
            pSQL = "SELECT IDENT_CURRENT('" & Base & Origem & "') AS ID"
            Proximo_ID_DATE = BD.Conexao.Execute(pSQL).Fields(0).Value
        End If



    End Function

    Shared Sub New()

        Servidor = ConfigurationManager.AppSettings("Servidor").ToString()
        Schema = ConfigurationManager.AppSettings("Schema").ToString()
        Usuario = ConfigurationManager.AppSettings("Usuario").ToString()
        Senha = ConfigurationManager.AppSettings("Senha").ToString()
        BancoEmUso = ConfigurationManager.AppSettings("Banco").ToString()

    End Sub

    Public Shared Function List(ByVal SQL As String) As DataTable


        Using Con As SqlConnection = New SqlConnection(StringConexaoSemProvider())

            Using Adp As New SqlDataAdapter()
                Using Cmd As New SqlCommand()

                    Conexao()

                    Dim Ds As New DataSet()

                    Cmd.CommandType = CommandType.Text
                    Cmd.CommandText = SQL
                    Cmd.Connection = Con

                    Try
                        Adp.SelectCommand = Cmd
                        Adp.Fill(Ds)
                        Return Ds.Tables(0)
                    Catch ex As Exception
                        Throw New Exception(String.Format("Falha na Conexão. {0}", ex.Message))
                    End Try

                End Using
            End Using
        End Using

        Return Nothing

    End Function
    Public Shared Function ExecuteScalar(ByVal SQL As String) As String
        Dim Result As Object = Nothing

        Using Con As SqlConnection = New SqlConnection(StringConexaoSemProvider())

            Using Cmd As SqlCommand = New SqlCommand(SQL, Con)

                Try
                    Cmd.CommandTimeout = 1200
                    Con.Open()
                    Result = Cmd.ExecuteScalar()
                    Con.Close()
                Catch ex As Exception
                    Throw New Exception(ex.Message + "DB.cs - ExecuteScalar - ")
                End Try
            End Using
        End Using

        Return If(Result Is Nothing, Nothing, Result.ToString())
    End Function


    'Public Shared Function ConnectionString() As String
    '    Return String.Format("Server={0};Initial Catalog={1};User ID={2};Password={3}", Server, Schema, User, Password)
    'End Function



End Class
