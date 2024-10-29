Imports System.Data.SqlClient

Public Class Banco

    Private Shared _Servidor As String
    Private Shared _Usuario As String
    Private Shared _Senha As String
    Private Shared _Schema As String
    Private Shared _Base As String
    Private Shared _BancoOperador As String
    Private Shared _BancoSgipa As String
    Private Shared _BancoRedex As String
    Private Shared _StringConexao As String

    Shared Sub New()

        Dim ConfigurationAppSettings As System.Configuration.AppSettingsReader = New System.Configuration.AppSettingsReader()

        Servidor = ConfigurationManager.AppSettings("Servidor").ToString()
        Schema = ConfigurationManager.AppSettings("Schema").ToString()
        Usuario = ConfigurationManager.AppSettings("Usuario").ToString()
        Senha = ConfigurationManager.AppSettings("Senha").ToString()

    End Sub

    Public Shared Property Servidor() As String
        Get
            Return _Servidor
        End Get
        Set(value As String)
            _Servidor = value
        End Set
    End Property

    Public Shared Property Usuario() As String
        Get
            Return _Usuario
        End Get
        Set(value As String)
            _Usuario = value
        End Set
    End Property

    Public Shared Property Senha() As String
        Get
            Return _Senha
        End Get
        Set(value As String)
            _Senha = value
        End Set
    End Property

    Public Shared Property Schema() As String
        Get
            Return _Schema
        End Get
        Set(value As String)
            _Schema = value
        End Set
    End Property

    Public Shared Property Base() As String
        Get
            Return _Base
        End Get
        Set(value As String)
            _Base = value
        End Set
    End Property
    Public Shared ReadOnly Property BancoOperador() As String
        Get
            Return "OPERADOR.."
        End Get
    End Property

    Public Shared ReadOnly Property BancoSgipa() As String
        Get
            Return "SGIPA.."
        End Get
    End Property

    Public Shared ReadOnly Property BancoRedex() As String
        Get
            Return ConfigurationManager.AppSettings("BANCO_REDEX").ToString()
        End Get
    End Property


    Public Shared Function ExecuteScalar(SQL As String) As String

        Dim Result As Object = Nothing

        Using Con As New SqlConnection(ConnectionString())
            Using Cmd As New SqlCommand(SQL, Con)
                Try
                    Con.Open()
                    Result = Cmd.ExecuteScalar()
                    Con.Close()
                Catch generatedExceptionName As Exception
                    Throw New Exception($"Erro: {generatedExceptionName.Message}")
                    Result = Nothing
                End Try
            End Using
        End Using

        Return If(Result Is Nothing, Nothing, Result.ToString())

    End Function

    Public Shared Function BeginTransaction(SQL As String) As Boolean

        Dim Success As Boolean = False

        Using Con As New SqlConnection(ConnectionString())
            Using Cmd As New SqlCommand(SQL, Con)

                Dim Transaction As SqlTransaction

                Con.Open()
                Transaction = Con.BeginTransaction()
                Cmd.Transaction = Transaction

                Try
                    Cmd.ExecuteNonQuery()
                    Transaction.Commit()
                    Success = True
                Catch generatedExceptionName As Exception
                    Transaction.Rollback()
                    Success = False
                End Try

            End Using
        End Using

        Return Success

    End Function

    Public Shared Function List(SQL As String) As DataTable

        Dim Ds As New DataSet()

        Using Con As New SqlConnection(ConnectionString())
            Using Adp As New SqlDataAdapter(New SqlCommand(SQL, Con))

                Try
                    Adp.SelectCommand.CommandTimeout = 18000
                    Adp.Fill(Ds)
                Catch generatedExceptionName As Exception
                    Return Nothing
                End Try

                Return Ds.Tables(0)

            End Using
        End Using

    End Function

    Public Shared Function ListDs(SQL As String) As DataSet

        Dim Ds As New DataSet()

        Using Con As New SqlConnection(ConnectionString())
            Using Adp As New SqlDataAdapter(New SqlCommand(SQL, Con))

                Try
                    Adp.Fill(Ds)
                Catch generatedExceptionName As Exception
                    Return Nothing
                End Try

                Return Ds

            End Using
        End Using

    End Function

    Public Shared Function Reader(SQL As String) As String()

        Dim lista As New List(Of String)()

        Using Con As New SqlConnection(ConnectionString())
            Using Cmd As New SqlCommand(SQL, Con)

                Dim dr As SqlDataReader
                Con.Open()
                dr = Cmd.ExecuteReader()

                If dr.HasRows Then
                    While dr.Read()
                        lista.Add(dr(0).ToString())
                    End While
                End If

                dr.Close()
                Con.Close()

            End Using
        End Using

        Return lista.ToArray()

    End Function
    Public Shared Function Consultar(ByVal SQL As String) As DataTable

        Dim Ds As New DataTable

        Using Connection As New SqlConnection(ConnectionString())
            Using Command As New SqlCommand(SQL, Connection)

                Connection.Open()
                Dim Rdr As SqlDataReader = Command.ExecuteReader(CommandBehavior.CloseConnection)
                If Rdr.HasRows Then Ds.Load(Rdr)
                Connection.Close()

            End Using
        End Using

        Return Ds

    End Function
    Public Shared Sub ExecuteTransaction(SQL1 As String, SQL2 As String)
        Using connection As SqlConnection = New SqlConnection(ConnectionString())
            connection.Open()
            Dim command As SqlCommand = connection.CreateCommand()
            Dim transaction As SqlTransaction
            transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted)
            command.Transaction = transaction

            Try
                command.CommandText = SQL1
                command.ExecuteNonQuery()
                command.CommandText = SQL2
                command.ExecuteNonQuery()
                transaction.Commit()
            Catch e As Exception
                transaction.Rollback()
            End Try
        End Using
    End Sub

    Public Shared Sub ExecuteTransactionList(SQLs As List(Of String))
        Using connection As SqlConnection = New SqlConnection(ConnectionString())
            connection.Open()
            Dim command As SqlCommand = connection.CreateCommand()
            Dim transaction As SqlTransaction
            transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted)
            command.Transaction = transaction

            Try
                For Each uSQL In SQLs
                    command.CommandText = uSQL
                    command.ExecuteNonQuery()
                Next

                transaction.Commit()
            Catch e As Exception
                transaction.Rollback()
            End Try
        End Using
    End Sub

    Public Shared Function ExecuteProcedure(sql As String) As System.Data.DataSet
        Using Con As SqlConnection = New SqlConnection(ConnectionString())

            Using Cmd As SqlCommand = New SqlCommand(sql, Con)
                Try
                    Cmd.CommandTimeout = 1200
                    Dim adapter As SqlDataAdapter = New SqlDataAdapter()
                    Dim ds As DataSet = New DataSet()
                    Con.Open()
                    Cmd.CommandText = Banco.BancoRedex & "sp_Coletor_REDEX_Identity"
                    Cmd.CommandType = System.Data.CommandType.StoredProcedure
                    Cmd.Parameters.Clear()
                    Cmd.CommandTimeout = 0
                    Cmd.Parameters.Add("@select", SqlDbType.VarChar).Value = sql
                    adapter.SelectCommand = Cmd
                    adapter.Fill(ds)
                    Return ds
                Catch ex As Exception
                    Return Nothing
                Finally
                    If (Cmd IsNot Nothing) Then Cmd.Dispose()
                    Con.Close()
                End Try
            End Using
        End Using

    End Function
    Public Shared Function ConnectionString() As String
        Return ConfigurationManager.ConnectionStrings("StringConexaoSqlServer").ConnectionString
    End Function

End Class
