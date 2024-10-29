Imports System.Runtime.CompilerServices

Module BooleanExtensions
    <Extension()>
    Function ToInt(ByVal valor As Boolean) As Integer
        Return IIf(valor, 1, 0)
    End Function
End Module