
Imports System
Imports System.Runtime.CompilerServices



Module DateTimeExtensions
        <Extension()>
        Function DataFormatada(ByVal valor As Object) As String
            If valor Is Nothing Then Return String.Empty
            Dim resultado As DateTime

            If DateTime.TryParse(valor.ToString(), resultado) Then
                If resultado <= DateTime.MinValue Then Return String.Empty
                Return resultado.ToString("dd/MM/yyyy")
            End If

            Return String.Empty
        End Function

    <Extension()>
    Function DataHoraFormatada(ByVal valor As Object) As String
        If valor Is Nothing Then Return String.Empty
        Dim resultado As DateTime

        If DateTime.TryParse(valor.ToString(), resultado) Then
            If resultado.Year = 1753 Then Return String.Empty
            Return Convert.ToDateTime(valor.ToString()).ToString("dd/MM/yyyy HH:mm")
        End If

        Return String.Empty
    End Function

    <Extension()>
    Function DataHoraMinutosSegundosFormatada(ByVal valor As Object) As String
        If valor Is Nothing Then Return String.Empty
        Dim resultado As DateTime

        If DateTime.TryParse(valor.ToString(), resultado) Then
            If resultado.Year = 1753 Then Return String.Empty
            Return Convert.ToDateTime(valor.ToString()).ToString("dd/MM/yyyy HH:mm:ss")
        End If

        Return String.Empty
    End Function
    <Extension()>
        Function ObterHorasMinutos(ByVal minutos As Integer) As String
            If minutos > 0 Then
                Dim totalMinutes As Integer = minutos
                Dim ts As TimeSpan = TimeSpan.FromMinutes(totalMinutes)
                Return String.Format("{0:00}:{1:00}", ts.Hours, ts.Minutes)
            End If

            Return String.Empty
        End Function
    End Module
