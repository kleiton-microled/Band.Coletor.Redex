Imports System
Imports System.Collections.Generic
Imports System.Data.SqlClient
Imports System.Data
Imports System.Reflection

Public Class DB
    Private Shared Property _Server As String
    Private Shared Property _User As String
    Private Shared Property _Password As String
    Private Shared Property _SchemaA As String
    Private Shared Property _Base As String

    Public Shared Property Server() As String
        Get
            Return _Server
        End Get
        Set(ByVal value As String)
            _Server = value
        End Set
    End Property

    Public Shared Property SchemaA() As String
        Get
            Return _SchemaA
        End Get
        Set(ByVal value As String)
            _SchemaA = value
        End Set
    End Property

    Public Shared Property User() As String
        Get
            Return _User
        End Get
        Set(ByVal value As String)
            _User = value
        End Set
    End Property

    Public Shared Property Password() As String
        Get
            Return _Password
        End Get
        Set(ByVal value As String)
            _Password = value
        End Set
    End Property
    Public Shared Function ExecuteScalar(ByVal SQL As String) As String

        Dim Result As Object = Nothing

        Using Con As SqlConnection = New SqlConnection(ConnectionString())

            Using Cmd As SqlCommand = New SqlCommand(SQL, Con)

                Try
                    Cmd.CommandTimeout = 1200
                    Con.Open()
                    Result = Cmd.ExecuteScalar()
                    Con.Close()
                Catch ex As Exception
                    'Util.GravarLog(ex.Message, "DB.cs - ExecuteScalar - " & ex.ToString(), SQL)
                    Throw New Exception("Banco de Dados RJ indiponível no momento. " & ex.Message)
                End Try
            End Using
        End Using

        Return If(Result Is Nothing, Nothing, Result.ToString())
    End Function

    Public Shared Function BeginTransaction(ByVal SQL As String, ByVal Optional checkSimulador As Boolean = False) As Boolean

        Dim Success As Boolean = False

        Using Con As SqlConnection = New SqlConnection(ConnectionString())

            Using Cmd As SqlCommand = New SqlCommand(SQL, Con)
                Dim Transaction As SqlTransaction
                Cmd.CommandTimeout = 1200
                Con.Open()
                Transaction = Con.BeginTransaction()
                Cmd.Transaction = Transaction

                Try
                    Cmd.ExecuteNonQuery()
                    Transaction.Commit()
                    Success = True
                Catch ex As Exception
                    'Util.GravarLog(ex.Message, "DB.cs - BeginTransaction " & ex.ToString(), SQL)
                    Transaction.Rollback()
                End Try

                Return Success
            End Using
        End Using
    End Function

    Public Shared Function List(ByVal SQL As String, ByVal Optional checkSimulador As Boolean = False) As DataTable

        Dim Ds As DataSet = New DataSet()

        Using Con As SqlConnection = New SqlConnection(ConnectionString())

            Using Cmd As SqlCommand = New SqlCommand()
                Cmd.CommandTimeout = 12000
                Cmd.Connection = Con
                Cmd.CommandType = CommandType.Text
                Cmd.CommandText = SQL

                Using Adp As SqlDataAdapter = New SqlDataAdapter(Cmd)
                    Adp.SelectCommand.CommandTimeout = 18800

                    Try
                        Adp.Fill(Ds)
                    Catch ex As Exception
                        ' Util.GravarLog(ex.Message, "DB.cs - List " & ex.ToString(), SQL)
                        Return Nothing
                    End Try

                    Return Ds.Tables(0)
                End Using
            End Using
        End Using
    End Function

    Public Shared Function Reader(ByVal SQL As String, ByVal Optional checkSimulador As Boolean = False) As String()
        Dim lista As List(Of String) = New List(Of String)()

        Using Con As SqlConnection = New SqlConnection(ConnectionString())

            Using Cmd As SqlCommand = New SqlCommand(SQL, Con)

                Try
                    Cmd.CommandTimeout = 1200
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
                Catch ex As Exception
                    ' Util.GravarLog(ex.Message, "DB.cs - Reader " & ex.ToString(), SQL)
                End Try
            End Using
        End Using

        Return lista.ToArray()
    End Function
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
        Return String.Format("Server={0};Initial Catalog={1};User ID={2};Password={3}", Server, SchemaA, User, Password)
    End Function
    Shared Sub New()

        Server = ConfigurationManager.AppSettings("Servidor").ToString()
        SchemaA = ConfigurationManager.AppSettings("Schema").ToString()
        User = ConfigurationManager.AppSettings("Usuario").ToString()
        Password = ConfigurationManager.AppSettings("Senha").ToString()

    End Sub
    Public Shared Function Teste() As String
        Dim teste1 As String = Assembly.GetExecutingAssembly().FullName
        Dim teste2 As String = "OtherMethod called from: " & Assembly.GetCallingAssembly().FullName
        Return ""
    End Function
End Class
