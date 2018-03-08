Imports System.Data
Imports System.Data.SqlClient

Partial Class SystemManager_MyAccount
    Inherits PageBase
    Protected Sub SDS_MyAccount_ItemUpdated(ByVal sender As Object, ByVal e As SqlDataSourceStatusEventArgs) Handles SDS_MyAccount.Updated
        If e.AffectedRows > 0 Then
            Me.lbMessage.Text = "系統訊息：您的資料已經更新。"

            '如果為強制變換密碼
            If Session("NeedToChangePassword") = True Then
                Me.lbMessage.Text = "系統訊息：您的密碼變更完成，下次登入請使用新的密碼，謝謝！。"
                Session("NeedToChangePassword") = Nothing
            End If
        Else
            Me.lbMessage.Text = "系統訊息：" & e.Exception.Message.ToString
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim UserID As Integer = CInt(Session("UserID"))
            Dim str As String = SDS_MyAccount.SelectCommand '.Replace("@UserID", UserID.ToString)
            Using dr As SqlDataReader = ClassDB.GetDataReaderParam(str, New SqlParameter("@UserID", UserID))
                Try
                    If dr IsNot Nothing Then
                        If dr.Read Then
                            Dim Password As String = dr("Password").ToString
                            Me.ViewState.Add("Tempword", Password)
                            Me.NameTextBox.Text = (dr("Name").ToString)
                            Me.EngNameTextBox.Text = (dr("EngName").ToString)
                            Me.TitleTextBox.Text = (dr("Title").ToString)
                            DropDownList1.DataBind()
                            Me.DropDownList1.SelectedValue = dr("DepartmentID").ToString
                            Me.TelNoTextBox.Text = (dr("TelNo").ToString)
                            Me.AddressTextBox.Text = (dr("Address").ToString)
                            Me.EmailTextBox.Text = dr("Email").ToString
                            Me.IDNumberTextBox.Text = dr("IDNumber").ToString
                            Me.Label1.Text = ((dr("UserName").ToString))
                            Me.PasswordTextBox.Text = dr("IDNumber").ToString
                        End If
                    End If
                Catch ex As Exception
                End Try
            End Using
            If Session("NeedToChangePassword") = True Then
                Dim PWValidDays As Integer = 180
                If System.Configuration.ConfigurationManager.AppSettings("PWValidDays").ToString IsNot Nothing Then
                    'Web.Config取得密碼有效期
                    PWValidDays = CInt(System.Configuration.ConfigurationManager.AppSettings("PWValidDays").ToString)
                End If
                Me.lbMessage.Text = "您是第一次登入系統，或" & PWValidDays & "天未更換密碼！請即刻進行密碼變更！"
                '啟動驗證
                lbPasswordWarning.Text = "<font color=""#ff0000"">請輸入新的密碼！</font>"
            End If
        End If
    End Sub

    Protected Sub ButtonUpdate_Click(sender As Object, e As EventArgs) Handles ButtonUpdate.Click

        If Session("NeedToChangePassword") = True And PasswordTextBox.Text = "" Then
            Me.lbMessage.Text = "請輸入新的密碼！"
        Else
            If PasswordTextBox.Text = "" Then
                Me.SDS_MyAccount.UpdateParameters("Password").DefaultValue = Me.ViewState("Tempword")
            Else
                If PasswordStrength.doPasswordCheck(PasswordTextBox.Text) = False Then
                    Me.lbMessage.Text = "您的密碼強度不夠,請填足8碼包含英數字,謝謝!"
                End If
                Dim pw As String = FormsAuthentication.HashPasswordForStoringInConfigFile(PasswordTextBox.Text, "SHA1")
                Me.SDS_MyAccount.UpdateParameters("Password").DefaultValue = pw
            End If
            Me.SDS_MyAccount.UpdateParameters("Title").DefaultValue = Me.TitleTextBox.Text.ToString
            Me.SDS_MyAccount.UpdateParameters("Name").DefaultValue = Me.NameTextBox.Text.ToString
            Me.SDS_MyAccount.UpdateParameters("EngName").DefaultValue = Me.EngNameTextBox.Text.ToString
            Me.SDS_MyAccount.UpdateParameters("TelNo").DefaultValue = Me.TelNoTextBox.Text.ToString
            Me.SDS_MyAccount.UpdateParameters("Address").DefaultValue = Me.AddressTextBox.Text.ToString
            Me.SDS_MyAccount.UpdateParameters("Email").DefaultValue = Me.EmailTextBox.Text.ToString
            Me.SDS_MyAccount.UpdateParameters("DepartmentID").DefaultValue = Me.DropDownList1.SelectedValue.ToString
            Me.SDS_MyAccount.UpdateParameters("IDNumber").DefaultValue = Me.IDNumberTextBox.Text.ToString

            Me.SDS_MyAccount.Update()
        End If

    End Sub
End Class
