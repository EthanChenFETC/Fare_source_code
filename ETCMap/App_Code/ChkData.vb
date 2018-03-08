Imports Microsoft.VisualBasic

Public Module ChkData
    Public Function ChkIsNumeric(ByVal Str As String) As Boolean
        Dim Chk As Boolean = True
        If Str <> "" Then
            Dim s As String = "0123456789"
            Dim i As Integer
            For i = 0 To Str.Length - 1
                Dim o As String = Str.Substring(i, 1)
                If s.IndexOf(o) = -1 Then
                    Chk = False
                    Exit For
                End If
            Next
        Else
            Chk = False
        End If
        Return Chk
    End Function
End Module
