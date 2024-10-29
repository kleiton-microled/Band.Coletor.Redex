Public Class Log
    Public Shared Sub InserirLog(origem As String, mensagem As String, stTrace As String, usuario As String)

        Dim usuarioId As Integer = 0
        If Not String.IsNullOrEmpty(usuario) Then
            Integer.TryParse(usuario.Replace("AUTONUMUSUARIO=", ""), usuarioId)
        End If
        Dim SQL As New StringBuilder

        SQL.Append("INSERT INTO ")
        SQL.Append("	" + Banco.BancoRedex + "TB_LOG_COLETOR( ")
        SQL.Append("		ORIGEM, ")
        SQL.Append("		MENSAGEM, ")
        SQL.Append("		STACK_TRACE, ")
        SQL.Append("		USUARIO ")
        SQL.Append("	) VALUES ( ")
        SQL.Append("		'" & origem & "',")
        SQL.Append("		'" & mensagem.Replace("'", "") & "', ")
        SQL.Append("		'" & stTrace & "', ")
        SQL.Append("		" & usuarioId & " ")
        SQL.Append("	) ")

        Banco.ExecuteScalar(SQL.ToString())

    End Sub
End Class
