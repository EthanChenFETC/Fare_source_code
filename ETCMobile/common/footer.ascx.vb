Imports System.Data
Imports System.Data.SqlClient
Partial Class footer
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        setCount()
        If ConfigurationManager.AppSettings("PCURL") IsNot Nothing Then
            Dim url As String = ConfigurationManager.AppSettings("PCURL")
            hypc.NavigateUrl = url
        End If
    End Sub
    Public Sub setCount()

        Using dr As SqlDataReader = ClassDB.GetDataReader("Select sum(yy.CustomCount)+sum(yy.SuggestCount)+sum(yy.MapCount)+sum(yy.MCustomCount)+sum(yy.MSuggestCount) as TotalCount from (select distinct CustomCount, SuggestCount, MapCount, MCustomCount, MSuggestCount, LastUpdate  from FareCalculateCount) as yy")
            Try
                If dr IsNot Nothing Then
                    If dr.Read Then
                        Me.lblCount.Text = CInt(dr("TotalCount")).ToString()
                    End If
                End If
            Catch ex As Exception
            Finally

            End Try
        End Using
    End Sub
End Class
