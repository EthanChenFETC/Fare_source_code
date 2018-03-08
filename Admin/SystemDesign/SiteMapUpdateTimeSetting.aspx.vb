
Partial Class SystemDesign_SiteMapUpdateTimeSetting
    Inherits PageBase

    Protected Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand
        If e.CommandName = "UpdateDeadline" Then
            Dim Value() As String = Split(e.CommandArgument.ToString, ",")
            Dim NodeID As Integer = CInt(Value(1))
            Dim RowIndex As Integer = CInt(Value(0))
            Dim txtDeadline As TextBox = CType(Me.GridView1.Rows(RowIndex).FindControl("txtDeadline"), TextBox)

            Me.SqlDataSource_AdminMenu.UpdateParameters("DeadlineDays").DefaultValue = txtDeadline.Text
            Me.SqlDataSource_AdminMenu.UpdateParameters("NodeID").DefaultValue = NodeID
            Me.SqlDataSource_AdminMenu.Update()
        End If
    End Sub


    Protected Sub GridView1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView1.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim drowView As System.Data.DataRowView = CType(e.Row.DataItem, System.Data.DataRowView)
            If e.Row.RowState = 0 OrElse e.Row.RowState = 1 OrElse e.Row.RowState = 2 OrElse e.Row.RowState = 31 Then

                Dim btnUpdateDeadline As Button = CType(e.Row.FindControl("btnUpdateDeadline"), Button)
                btnUpdateDeadline.CommandArgument = e.Row.RowIndex & "," & drowView.Item("NodeId").ToString

                Dim txtDeadline As TextBox = CType(e.Row.FindControl("txtDeadline"), TextBox)
                Dim lbDayWord As Label = CType(e.Row.FindControl("lbDayWord"), Label)
                txtDeadline.Text = drowView("DeadlineDays").ToString

                If CType(drowView.Item("HaveChildNode").ToString, Boolean) = True OrElse CInt((drowView.Item("PublishType").ToString)) > 1 Then
                    txtDeadline.Visible = False
                    btnUpdateDeadline.Visible = False
                    lbDayWord.Visible = False
                End If
                Dim Image1 As Image = CType(e.Row.FindControl("Image1"), Image)
                Image1.CssClass = "AdminMenuImage" & drowView("NodeLevel").ToString
                Image1.ImageUrl = "Images/" & drowView("ImageUrl").ToString
                Dim Label1 As Label = CType(e.Row.FindControl("Label1"), Label)
                Label1.Text = (drowView("Text").ToString)
                Label1.CssClass = "AdminNode" & drowView("NodeLevel").ToString
            ElseIf e.Row.RowState = 4 OrElse e.Row.RowState = 5 Then
                Dim TextBox1 As TextBox = CType(e.Row.FindControl("TextBox1"), TextBox)
                TextBox1.Text = drowView("Text").ToString
            End If

        End If
    End Sub

    Protected Sub SqlDataSource_AdminMenu_Updated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.SqlDataSourceStatusEventArgs) Handles SqlDataSource_AdminMenu.Updated
        If e.AffectedRows > 1 Then
            Me.lbMessage.Text = "系統訊息：更新完成!"
        End If
    End Sub

End Class
