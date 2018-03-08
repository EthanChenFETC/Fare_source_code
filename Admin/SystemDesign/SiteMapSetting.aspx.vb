
Partial Class SystemDesign_SiteMapSetting
    Inherits PageBase
    Protected Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand
        If e.CommandName = "Update" Then
            Dim key As String = e.CommandArgument
            Dim rblPublishType As RadioButtonList = CType(Me.GridView1.Rows(key).FindControl("rblPublishType"), RadioButtonList)
            Me.SqlDataSource_AdminMenu.UpdateParameters("PublishType").DefaultValue = rblPublishType.SelectedValue
            Dim rbltxtURL As RadioButtonList = CType(Me.GridView1.Rows(key).FindControl("rbltxtURL"), RadioButtonList)
            Me.SqlDataSource_AdminMenu.UpdateParameters("Target").DefaultValue = rbltxtURL.SelectedValue
            Dim txtURL As TextBox = CType(Me.GridView1.Rows(key).FindControl("txtURL"), TextBox)
            Me.SqlDataSource_AdminMenu.UpdateParameters("RefPath").DefaultValue = txtURL.Text
            Dim TextBox1 As TextBox = CType(Me.GridView1.Rows(key).FindControl("TextBox1"), TextBox)
            Me.SqlDataSource_AdminMenu.UpdateParameters("Text").DefaultValue = TextBox1.Text
            Me.SqlDataSource_AdminMenu.Update()
        End If
    End Sub
    Protected Sub GridView1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView1.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then

            Dim drowView As System.Data.DataRowView = CType(e.Row.DataItem, System.Data.DataRowView)

            '編輯模式
            If e.Row.RowState = 0 OrElse e.Row.RowState = 1 Or e.Row.RowState = 2 OrElse e.Row.RowState = 3 Then
                Dim Label3 As Label = CType(e.Row.FindControl("Label3"), Label)
                Dim Label2 As Label = CType(e.Row.FindControl("Label2"), Label)
                Dim Label1 As Label = CType(e.Row.FindControl("Label1"), Label)
                Dim Image1 As Image = CType(e.Row.FindControl("Image1"), Image)
                Label1.Text = (drowView("Text").ToString)
                Label1.CssClass = "AdminNode" & drowView("NodeLevel").ToString
                Image1.CssClass = "AdminMenuImage" & drowView("NodeLevel").ToString
                Label2.Text = (drowView("PublishTypeName").ToString)
                Label3.Text = (drowView("RefPath").ToString)
                Image1.ImageUrl = "Images/" & (drowView("ImageUrl"))
            ElseIf e.Row.RowState = 4 OrElse e.Row.RowState = 5 Then
                Dim rblPublishType As RadioButtonList = CType(e.Row.FindControl("rblPublishType"), RadioButtonList)

                Dim rbltxtURL As RadioButtonList = CType(e.Row.FindControl("rbltxtURL"), RadioButtonList)
                rbltxtURL.DataBind()
                Dim txtURL As TextBox = CType(e.Row.FindControl("txtURL"), TextBox)


                For i As Integer = 0 To rblPublishType.Items.Count - 1
                    Dim rblItem As ListItem = rblPublishType.Items(i)
                    If rblItem.Value = "1" Then
                        '選擇多向上稿
                        rblItem.Attributes.Add("onclick", "document.all." & txtURL.ClientID & ".disabled=true;document.all." & rbltxtURL.ClientID & ".disabled=true;document.all." & txtURL.ClientID & ".value='Publish.aspx';")
                    Else
                        rblItem.Attributes.Add("onclick", "document.all." & txtURL.ClientID & ".disabled=false;document.all." & rbltxtURL.ClientID & ".disabled=false;document.all." & txtURL.ClientID & ".value='';")
                    End If
                Next

                Dim TextBox1 As TextBox = CType(e.Row.FindControl("TextBox1"), TextBox)
                TextBox1.Text = Server.HtmlDecode(drowView("Text").ToString)


                txtURL.Text = Server.HtmlDecode(drowView("RefPath").ToString)

                rbltxtURL.SelectedValue = drowView("Target")
                rblPublishType.SelectedValue = drowView("PublishType")
            End If

        End If
    End Sub


    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim rowindex As Integer = CInt(Request.Form("__EVENTARGUMENT").ToString)
        Dim fu As FileUpload = CType(Me.GridView1.Rows(rowindex).FindControl("FileUploadTitlePic"), FileUpload)
    End Sub
    Protected Sub GridView1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles GridView1.SelectedIndexChanged

    End Sub
End Class
