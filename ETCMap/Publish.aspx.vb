''' <summary>
''' 多向上稿文章清單與內容頁(網頁表單)
''' </summary>
''' <remarks></remarks>
Partial Class Publish
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'If Not IsPostBack Then
        '    If Request.Params("cnid") IsNot Nothing Then
        '        Try
        '            Dim NodeID As Integer = CInt(Request.Params("cnid"))
        '        Catch ex As Exception
        '            Response.Redirect("~/Default.aspx")
        '        End Try
        '    End If

        '    If Request.Params("p") IsNot Nothing Then
        '        Try
        '            Dim PublicID As Integer = CInt(Request.Params("p"))
        '        Catch ex As Exception
        '            Response.Redirect("~/Default.aspx")
        '        End Try
        '    End If
        'End If

    End Sub
End Class
