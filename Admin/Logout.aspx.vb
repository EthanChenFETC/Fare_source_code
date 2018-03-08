
Partial Class Logout
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim ReqPar = Session("ReqPar")
        Dim ReturnUrl As String = Session("ReturnUrl")
        Session.Abandon()
        Session.Add("ReqPar", ReqPar)
        Session.Add("ReturnUrl", ReturnUrl)
        Me.Server.Transfer("~/Login.aspx")
    End Sub
End Class
