
Imports System
Imports System.Runtime.CompilerServices


Module EnumExtensions
        <Extension()>
        Function ToEnum(Of T)(ByVal valor As String) As T
            Return CType([Enum].Parse(GetType(T), valor), T)
        End Function
    End Module
