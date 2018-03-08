
Partial Class GetMp3
    'Inherits System.Web.UI.Page
    Inherits InjectionPage

    Protected Sub form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles form1.Load
        If Session("CheckCode") <> Nothing Then
            Response.ContentType = "audio/mpeg"
            Dim checkCode As String = Session("CheckCode")
            If checkCode.Length > 0 Then
                For i As Integer = 0 To checkCode.Length - 1
                    Response.WriteFile(Page.ResolveUrl("~/sound/" & checkCode(i) & ".mp3"))
                Next
            End If
        End If


    End Sub
End Class
