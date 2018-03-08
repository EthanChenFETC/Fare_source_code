Imports System.Data
Imports System.Data.SqlClient

Partial Class _Default
    Inherits PageBase


    ''' <summary>
    ''' Login Infomation
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub DataList_UserInfo_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs) Handles DataList_UserInfo.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim drow As DataRowView = e.Item.DataItem
            Dim LoginIPLabel As Label = CType(e.Item.FindControl("LoginIPLabel"), Label)
            Dim UserIP As String = Request.ServerVariables("REMOTE_ADDR")
            If  Regex.IsMatch(UserIP.Replace(":", "").Replace(".", "").Replace("-", "").ToString.Trim, "^[0-9]+$") Then
                LoginIPLabel.Text = UserIP
            End If
            'Update This time User Login DB
            If Session("UserDataUpdate") IsNot Nothing Then
                ClassDB.UpdateDB("Net2_Accounts_LoginInfo_Update", New SqlParameter("@UserID", CInt(Session("UserID").ToString)))
                Session("UserDataUpdate") = Nothing
            End If
            Dim NameLabel As Label = CType(e.Item.FindControl("NameLabel"), Label)
            NameLabel.Text = (drow("Name").ToString)
            Dim LastLoginTimeLabel As Label = CType(e.Item.FindControl("LastLoginTimeLabel"), Label)
            LastLoginTimeLabel.Text = (drow("LastLoginTime"))
            Dim TotalLoginCountLabel As Label = CType(e.Item.FindControl("TotalLoginCountLabel"), Label)
            TotalLoginCountLabel.Text = (drow("TotalLoginCount"))
        End If
    End Sub

    ''' <summary>
    ''' Login Infomation
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub DataList_SiteInfo_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs) Handles DataList_SiteInfo.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim drow As DataRowView = e.Item.DataItem
            Dim Label1 As Label = CType(e.Item.FindControl("Label1"), Label)
            Label1.Text = (drow("SiteName"))
            Dim CounterLabel As Label = CType(e.Item.FindControl("CounterLabel"), Label)
            CounterLabel.Text = (drow("Counter"))
            Dim CounterTodayLabel As Label = CType(e.Item.FindControl("CounterTodayLabel"), Label)
            CounterTodayLabel.Text = (drow("CounterToday"))
            Dim CounterYesterdayLabel As Label = CType(e.Item.FindControl("CounterYesterdayLabel"), Label)
            CounterYesterdayLabel.Text = (drow("CounterYesterday"))
            Dim DocCountLabel As Label = CType(e.Item.FindControl("DocCountLabel"), Label)
            DocCountLabel.Text = (drow("DocCount"))
            Dim LastUpdateDateLabel As Label = CType(e.Item.FindControl("LastUpdateDateLabel"), Label)
            LastUpdateDateLabel.Text = (drow("SiteLastUpdate"))
        End If
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand

        If e.CommandName = "EditPublish" Then
            Dim index As Integer = CInt(e.CommandArgument)
            Dim UID As Integer = CInt(GridView1.DataKeys(index).Value)  '取得主鍵，轉到編輯頁面
            UID = CInt(UID)
            Dim sql As String = "SELECT * FROM AdminAnnounce WHERE (PublicID = @UID)"
            Using dr As SqlDataReader = ClassDB.GetDataReaderParam(sql, New SqlParameter("@UID", UID))
                Try
                    If dr.Read Then
                        Me.lbSubect.Text = (dr("Subject").ToString)
                        Dim Contents As String = dr("Content")
                        Contents = RemoveNope(Contents)
                        Me.lbContent.Text = Contents
                        'Me.ltScript.Text = "<script>set_float_bg_height()</script>"

                        Me.float_bg.Visible = True
                        Me.float_function.Visible = True

                        'Me.ltScript.Text = "<script>set_float_bg_height()</script>"
                    End If
                Catch ex As Exception
                    WriteErrLog(ex, Me.Page)
                Finally
                End Try
            End Using
            'InterfaceBuilder.ChangeUserControlByPassFileName(Me.Page, "AdminAnnounceEdit.ascx", "ascx")

        End If
    End Sub

    Protected Sub GridView1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView1.RowDataBound
        Dim rv As DataRowView = CType(e.Row.DataItem, DataRowView)
        If e.Row.RowType = DataControlRowType.DataRow Then
            If e.Row.RowState = 0 OrElse e.Row.RowState = 1 OrElse e.Row.RowState = 2 OrElse e.Row.RowState = 3 Then
                Dim Label1 As Label = CType(e.Row.FindControl("Label1"), Label)
                ' Label1.Text = (rv("Name"))
                Dim Label2 As Label = CType(e.Row.FindControl("Label2"), Label)
                'Label2.Text = (rv("DepartmentName"))
            ElseIf e.Row.RowState = 4 OrElse e.Row.RowState = 5 Then
                Dim TextBox1 As TextBox = CType(e.Row.FindControl("TextBox1"), TextBox)
                TextBox1.Text = (rv("Name").ToString)
            End If
        End If
    End Sub
    Protected Sub btnCloseMessage_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCloseMessage.Click
        Me.float_bg.Visible = False
        Me.float_function.Visible = False

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Me.Label3.Text = Me.float_function.ClientID.ToString
        'Me.Label3.Text = Me.float_bg.ClientID.ToString

        'WriteLog("Page_Load", Session("UserID").ToString & " " & Session("UrlValue").ToString)
    End Sub

End Class
