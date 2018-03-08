
Partial Class SystemErrors
    Inherits System.Web.UI.Page

    ''' <summary>
    ''' (20080225mw)
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub lkbHome_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lkbHome.Click
        Response.Redirect(ModulePathManager.GetApplicationPath & "Default.aspx")
    End Sub

    ''' <summary>
    ''' (20080225mw)
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.hyLogout.NavigateUrl = ModulePathManager.GetApplicationPath & "Logout.aspx"
    End Sub

   
End Class
