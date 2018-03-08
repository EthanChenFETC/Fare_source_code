
Partial Class SystemManager_Section
    Inherits PageBase

#Region "View0-GridView1-科別清單"

    ''' <summary>
    ''' 設定GridVeiw統一樣式
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub GridView1_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.DataBound
        GridViewPageInfo.GetGridViewInfo(Me.GridView1, Me.Page, Me.sds_section)
    End Sub

    Protected Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand
        If e.CommandName = "UpdateItem" Then
            Dim RowIndex As Integer = e.CommandArgument.ToString
            Dim DropDownList1 As DropDownList = CType(Me.GridView1.Rows(RowIndex).FindControl("DropDownList1"), DropDownList)
            Me.sds_section.UpdateParameters("DepartmentID").DefaultValue = DropDownList1.SelectedValue
            Me.sds_section.Update()
            Me.GridView1.DataBind()
        End If
    End Sub
    Protected Sub GridView1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView1.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then

            Dim drowView As System.Data.DataRowView = CType(e.Row.DataItem, System.Data.DataRowView)

            '編輯模式
            If e.Row.RowState = 0 OrElse e.Row.RowState = 1 Or e.Row.RowState = 2 OrElse e.Row.RowState = 3 Then
                Dim Label1 As Label = CType(e.Row.FindControl("Label1"), Label)
                Label1.Text = (drowView("DepartmentName").ToString)
            ElseIf e.Row.RowState = 4 OrElse e.Row.RowState = 5 Then
                Dim DropDownList1 As DropDownList = CType(e.Row.FindControl("DropDownList1"), DropDownList)
                DropDownList1.DataBind()
                If DropDownList1.Items.Count > 0 Then
                    DropDownList1.SelectedValue = (drowView("DepartmentID"))
                End If
                Dim Button1 As Button = CType(e.Row.FindControl("Button1"), Button)
                Button1.CommandArgument = e.Row.RowIndex
            End If

        End If
    End Sub

    ''' <summary>
    ''' 清除搜尋
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btn_Clear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Clear.Click
        Me.txt_Search.Text = ""
        Me.GridView1.DataBind()
    End Sub


    ''' <summary>
    ''' 新增科別
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btn_NewSection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_NewSection.Click
        If Me.Page.IsValid Then
            Me.sds_section.Insert()
        End If
    End Sub


    ''' <summary>
    ''' 新增科別完成事件
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub sds_section_Inserted(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.SqlDataSourceStatusEventArgs) Handles sds_section.Inserted
        If e.AffectedRows > 0 Then
            GridViewPageInfo.Message_Insert_OK(Me.Page, "科別管理")
        Else
            GridViewPageInfo.Message_Insert_False(Me.Page, "科別管理")
        End If
    End Sub
#End Region

End Class
