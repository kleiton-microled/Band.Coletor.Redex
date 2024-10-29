Imports System.Globalization
Imports System.Runtime.CompilerServices

Module DecimalExtensions
    <Extension()>
    Function ToStringInvariantCulture(ByVal valor As Decimal) As String
        Return valor.ToString(CultureInfo.InvariantCulture)
    End Function
End Module
