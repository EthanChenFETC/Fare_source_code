Imports System.Data
Imports System.Data.SqlClient

Partial Class Login
    Inherits System.Web.UI.Page


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim ReqPar = Session("ReqPar")
            Dim ReturnUrl As String = Session("ReturnUrl")
            Session.Abandon()
            ViewState.Add("ReqPar", ReqPar)
            ViewState.Add("ReturnUrl", ReturnUrl)
        End If
        Me.txtUserName.Focus()

    End Sub

    Private Sub btnLogin_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnLogin.Click

        doLogin()
        'If Page.IsValid Then
        '    doLogin()
        'End If
    End Sub

    Private Sub doLogin()
        Dim Username As String = Trim(Me.txtUserName.Text)
        Dim pStr As String = Trim(Me.txtP.Text)


        '放入不合法字元，不予通過，只有英數字
        If PasswordStrength.doLoginUserNameCheck(Username) = False OrElse PasswordStrength.doLoginPasswordCheck(pStr) = False Then
            Me.lblMessage.Text = "請勿使用非法的字元登入。"
            Exit Sub
        End If


        If IdentityObject.LoginIdentity(Username, pStr, Me) = True Then

            ModuleWriteLog.WriteAdminLog(Me, "登入-後台管理系統")
            '20130209 Chris 新增 ReturnUrl 帶參數 pid
            Dim ReturnUrl As String = IIf(ViewState("ReturnUrl") Is Nothing, "", ViewState("ReturnUrl"))
            Dim ReqPar = ViewState("ReqPar")
            If ReturnUrl.Trim().Length > 0 Then
                Session.Remove("ReturnUrl")
                Session.Add("ReqPar", ViewState("ReqPar"))

                Response.Redirect(ReturnUrl, False)
            Else

                Response.Redirect(ModulePathManager.GetApplicationPath & "Default.aspx")
                'ReqPar = System.Configuration.ConfigurationManager.AppSettings("DefaultRedirect")
                'If CStr(ReqPar) <> "" Then
                '    Response.Redirect(ReqPar)
                'Else
                '    Session("UrlValue") = "274,/Arc_Admin/Default.aspx,1,False"
                '    Response.Redirect(ModulePathManager.GetApplicationPath & "Default.aspx")
                '    '274,/Arc_AdminModule/Intranet/Default.aspx,1,False
                'End If
            End If

        Else
            WriteAdminLog(Me, "登入-帳號或密碼不正確！輸入的帳號：" & RemoveSQLInjection(Me.txtUserName.Text), False)
            Me.lblMessage.Text = "您的群組或帳號已經停用！或登入資料不正確！"
            'IdentityObject.doLoginCount(Me)
        End If
    End Sub

End Class