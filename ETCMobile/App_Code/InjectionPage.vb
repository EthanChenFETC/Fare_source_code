Imports Microsoft.VisualBasic

Public Class InjectionPage
    Inherits System.Web.UI.Page
    Dim Injection As Sql_Injection = New Sql_Injection
    Dim SiteDomainName As String = ConfigurationManager.AppSettings("SiteDomainName").ToString
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim Injection_data As String
        Injection_data = Request.Headers.Item("referer") & "," & Request.Url.Query
        If Request.Params("cnid") IsNot Nothing Then
            If Not IsNumeric(Request.Params("cnid")) Then
                Dim script As String = "alert('請勿使用駭客攻擊手法!!');location.href='" & SiteDomainName & "';"
                'Me.ClientScript.RegisterStartupScript(Me.Page.GetType, "waring", script, True)
                Response.Write("<script>" & script & "</script>")
                HttpContext.Current.ApplicationInstance.CompleteRequest()
                'Response.End()
            End If
        End If
        If Request.Params("p") IsNot Nothing Then
            If Not IsNumeric(Request.Params("p")) Then
                Dim script As String = "alert('請勿使用駭客攻擊手法!!');location.href='" & SiteDomainName & "';"
                'Me.ClientScript.RegisterStartupScript(Me.Page.GetType, "waring", script, True)
                Response.Write("<script>" & script & "</script>")
                'Response.Redirect(SiteDomainName)
                HttpContext.Current.ApplicationInstance.CompleteRequest()
                'Response.End()
            End If
        End If
        'If Injection.Check_Sql_Injection(Request, Response, Me.Page) Then
        '    Dim script As String = "alert('請勿使用駭客攻擊手法!!');location.href='" & SiteDomainName & "';"
        '    Response.Write("<script>" & script & "</script>")
        '    HttpContext.Current.ApplicationInstance.CompleteRequest()
        '    'Response.End()
        'End If
    End Sub

End Class
