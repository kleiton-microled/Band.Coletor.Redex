Imports System
Imports System.Globalization
Imports System.Runtime.CompilerServices


Module ObjectExtensions
    <Extension()>
    Function ToInt(ByVal valor As Object) As Integer
        If valor Is Nothing Then Return 0
        Dim resultado As Integer = 0
        If Int32.TryParse(valor.ToString(), resultado) Then Return resultado
        Return 0
    End Function


End Module
