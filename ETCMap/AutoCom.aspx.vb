Imports System.IO
Imports System.Net
Imports System.Data
Imports System.Data.SqlClient

Partial Class AutoCom
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim sqlint As Sql_Injection = New Sql_Injection
        If Not sqlint.Check_Sql_Injection(Request, Response, Me.Page) Then
            Dim responseFromServer As String = ""
            Dim txt As String = Request.Params("txt")
            If txt Is Nothing Then
                txt = ""
            End If
            If txt.Length > 0 Then
                Try
                    Dim dtIC As DataTable = Fare.GetInterChangeList
                    Dim row() As DataRow = dtIC.Select("  ")
                    For i As Integer = 0 To row.Length - 1
                        responseFromServer = responseFromServer & ",(" & row(i)("HWName") & ")" & row(i)("ICName").ToString
                    Next
                    If responseFromServer.Length > 0 Then
                        responseFromServer = responseFromServer.Substring(1)
                    End If
                Catch ex As Exception
                    WriteErrLog(ex, Me)
                End Try
            End If
            Response.Write(responseFromServer)
        Else
            Response.Write("請不要用攻擊語法")
        End If
    End Sub
End Class
