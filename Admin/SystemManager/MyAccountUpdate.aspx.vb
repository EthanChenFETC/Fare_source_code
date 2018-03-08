Imports System.Data
Imports System.Data.SqlClient

''' <summary>
''' 本頁面是提供給需要變換密碼時,所以不列在PageBase來管理,但以偵測是否有登入來保護系統(20080225mw)
''' </summary>
''' <remarks></remarks>
Partial Class SystemManager_MyAccountUpdate
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("UserID") Is Nothing OrElse Session("DepartmentID") Is Nothing Then
            Me.FormView1.Visible = False
            Me.Visible = False
            Exit Sub
        End If
        If Session("NeedToChangePassword") = True Then
            Dim PWValidDays As Integer = 180
            If System.Configuration.ConfigurationManager.AppSettings("PWValidDays").ToString IsNot Nothing Then
                'Web.Config取得密碼有效期
                PWValidDays = CInt(System.Configuration.ConfigurationManager.AppSettings("PWValidDays").ToString)
            End If
            Me.lbMessage.Text = "您是第一次登入系統，或" & PWValidDays & "天未更換密碼！請即刻進行密碼變更！"
        End If
    End Sub


    Protected Sub FormView1_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles FormView1.DataBound
        If Me.FormView1.DefaultMode = FormViewMode.Edit Then
            Dim rv As DataRowView = CType(Me.FormView1.DataItem, DataRowView)
            Dim Password As String = rv.DataView.Table.Rows(0).Item("Password").ToString()
            Me.ViewState.Add("Tempword", Password)
            Dim Label2 As Label = CType(Me.FormView1.FindControl("lbPasswordWarning"), Label)
            Label2.Text = (rv("Name"))
            Dim EngNameTextBox As TextBox = CType(Me.FormView1.FindControl("EngNameTextBox"), TextBox)
            EngNameTextBox.Text = RemoveXSS(IIf(IsDBNull(rv("EngName")), "", rv("EngName")))
            Dim TitleTextBox As TextBox = CType(Me.FormView1.FindControl("TitleTextBox"), TextBox)
            TitleTextBox.Text = RemoveXSS(IIf(IsDBNull(rv("Title")), "", rv("Title")))
            Dim DropDownList1 As DropDownList = CType(Me.FormView1.FindControl("DropDownList1"), DropDownList)
            DropDownList1.DataBind()
            DropDownList1.SelectedValue = rv("DepartmentID")
            Dim TelNoTextBox As TextBox = CType(Me.FormView1.FindControl("TelNoTextBox"), TextBox)
            TelNoTextBox.Text = RemoveXSS(IIf(IsDBNull(rv("TelNo")), "", rv("TelNo")))
            Dim AddressTextBox As TextBox = CType(Me.FormView1.FindControl("AddressTextBox"), TextBox)
            AddressTextBox.Text = RemoveXSS(IIf(IsDBNull(rv("Address")), "", rv("Address")))
            Dim EmailTextBox As TextBox = CType(Me.FormView1.FindControl("EmailTextBox"), TextBox)
            EmailTextBox.Text = RemoveXSS(IIf(IsDBNull(rv("Email")), "", rv("Email")))
            Dim TextBox1 As TextBox = CType(Me.FormView1.FindControl("TextBox1"), TextBox)
            TextBox1.Text = RemoveXSS(IIf(IsDBNull(rv("IDnumber")), "", rv("IDnumber")))
            Dim Label1 As Label = CType(Me.FormView1.FindControl("Label1"), Label)
            Label1.Text = RemoveXSS(rv("UserName"))

            '強制更新密碼

            If Session("NeedToChangePassword") = True Then
                Try
                    Dim lbPasswordWarning As Label = CType(Me.FormView1.FindControl("lbPasswordWarning"), Label)
                    lbPasswordWarning.Text = "<font color=""#ff0000"">請輸入新的密碼！</font>"

                    '啟動驗證
                    Dim rfvPassword As RequiredFieldValidator = CType(Me.FormView1.FindControl("rfvPassword"), RequiredFieldValidator)
                    rfvPassword.Enabled = True
                Catch ex As Exception
                    WriteErrLog(ex, Me.Page)
                End Try
            End If
        End If
    End Sub

    Protected Sub FormView1_ItemUpdated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.FormViewUpdatedEventArgs) Handles FormView1.ItemUpdated
        If e.AffectedRows > 0 Then
            Session("NeedToChangePassword") = Nothing
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, GetType(UpdatePanel), "waring", "alert('您的密碼變更完成，系統將自動登出請以新的密碼登入，謝謝！');parent.location='" & ModulePathManager.GetAdminPath(Me.Page) & "Logout.aspx';", True)
        Else
            Me.lbMessage.Text = "系統訊息：" & (e.Exception.Message.ToString)
        End If
    End Sub


    Protected Sub FormView1_ItemUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.FormViewUpdateEventArgs) Handles FormView1.ItemUpdating
        Dim PasswordTextBox As TextBox = CType(Me.FormView1.FindControl("PasswordTextBox"), TextBox)
        If Session("NeedToChangePassword") = True And PasswordTextBox.Text = "" Then
            Me.lbMessage.Text = "請輸入新的密碼！"
            e.Cancel = True
        Else
            If PasswordTextBox.Text = "" Then
                Me.SDS_MyAccount.UpdateParameters("Password").DefaultValue = Me.ViewState("Tempword")
            Else
                If PasswordStrength.doPasswordCheck(PasswordTextBox.Text) = False Then
                    Me.lbMessage.Text = "您的密碼強度不夠,請填足8碼包含英數字,謝謝!"
                    e.Cancel = True
                End If

                Dim pw As String = FormsAuthentication.HashPasswordForStoringInConfigFile(PasswordTextBox.Text, "SHA1")
                Me.SDS_MyAccount.UpdateParameters("Password").DefaultValue = pw

                Dim EngNameTextBox As TextBox = CType(Me.FormView1.FindControl("EngNameTextBox"), TextBox)
                Dim TitleTextBox As TextBox = CType(Me.FormView1.FindControl("TitleTextBox"), TextBox)
                Dim TelNoTextBox As TextBox = CType(Me.FormView1.FindControl("TelNoTextBox"), TextBox)
                Dim AddressTextBox As TextBox = CType(Me.FormView1.FindControl("AddressTextBox"), TextBox)
                Dim EmailTextBox As TextBox = CType(Me.FormView1.FindControl("EmailTextBox"), TextBox)
                Dim TextBox1 As TextBox = CType(Me.FormView1.FindControl("TextBox1"), TextBox)

                Me.SDS_MyAccount.UpdateParameters("Title").DefaultValue = TitleTextBox.Text.ToString
                Me.SDS_MyAccount.UpdateParameters("EngName").DefaultValue = EngNameTextBox.Text.ToString
                Me.SDS_MyAccount.UpdateParameters("TelNo").DefaultValue = TelNoTextBox.Text.ToString
                Me.SDS_MyAccount.UpdateParameters("Address").DefaultValue = AddressTextBox.Text.ToString
                Me.SDS_MyAccount.UpdateParameters("Email").DefaultValue = EmailTextBox.Text.ToString
                Me.SDS_MyAccount.UpdateParameters("IDNumber").DefaultValue = TextBox1.Text.ToString

            End If
        End If
    End Sub



End Class
