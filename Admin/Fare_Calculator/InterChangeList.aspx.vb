Imports System.Data
Imports System.Data.SqlClient

Partial Class Fare_Calculator_InterChangeList
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
        End If
    End Sub
    Protected Sub GridView1_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridView1.RowDeleting

        Dim UID As Integer = Me.GridView1.DataKeys(e.RowIndex).Value
        ClassDB.UpdateDB("Delete from InterchangeBestFare where StartIC in (" & UID & ") or StopIC in (" & UID & ")")
    End Sub

    Protected Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand
        If e.CommandName = "DeleteItem" Then
            Dim key As Integer = e.CommandArgument
            sdsInterChangeList.DeleteParameters("ICUID").DefaultValue = key
            sdsInterChangeList.Delete()
            GridView1.DataBind()
        End If

    End Sub

    Protected Sub btnShowAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnShowAll.Click
        Me.txtSearch.Text = ""
    End Sub

    Protected Sub GridView1_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.DataBound
        GridViewPageInfo.GetGridViewInfo(GridView1, Me.Page, Me.sdsInterChangeList)
    End Sub


    Protected Sub GridView1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView1.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            'Turn Off the Permission to Edit the Department Self
            Dim drow As DataRowView = CType(e.Row.DataItem, DataRowView)
            If e.Row.RowState = 0 Or e.Row.RowState = 1 Or e.Row.RowState = 2 Or e.Row.RowState = 3 Then
                Dim RowIndex As Integer = e.Row.RowIndex
                Dim DataKeys As String = Me.GridView1.DataKeys(RowIndex).Value().ToString
                If DataKeys.Equals(CStr(Session("DepartmentID"))) And Not Session("Administrator") And Not Session("SuperAdmin") Then
                    Dim btn1 As Button = CType(e.Row.FindControl("BtnEdit"), Button)
                    Dim btn2 As Button = CType(e.Row.FindControl("BtnDel"), Button)
                    btn1.Enabled = False
                    btn2.Enabled = False
                End If
                Dim lblHWName As Label = CType(e.Row.FindControl("lblHWName"), Label)
                Dim lblAlias As Label = CType(e.Row.FindControl("lblAlias"), Label)
                ' Dim lblNotes As Label = CType(e.Row.FindControl("lblNotes"), Label)
                lblHWName.Text = (drow("HWName"))
                lblAlias.Text = (drow("AliasICName"))
                'lblNotes.Text = (IIf(IsDBNull(drow("Notes")), "", drow("Notes").ToString))
            ElseIf e.Row.RowState = 5 Or e.Row.RowState = DataControlRowState.Edit Then
                Dim ddlHWList As DropDownList = CType(e.Row.FindControl("ddlHWList"), DropDownList)
                'Dim ddlICList As DropDownList = CType(e.Row.FindControl("ddlICList"), DropDownList)
                'Dim txtNotes As TextBox = CType(e.Row.FindControl("txtNotes"), TextBox)
                ddlHWList.DataBind()
                ' ddlICList.DataBind()
                ddlHWList.SelectedValue = drow("HWUID")
                'txtNotes.Text = Server.HtmlEncode(IIf(IsDBNull(drow("Notes")), "", drow("Notes").ToString))

            End If
        End If

    End Sub

    Protected Sub GridView1_RowDeleted(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeletedEventArgs) Handles GridView1.RowDeleted
        If e.AffectedRows > 0 Then
            Me.lblMessage.Text = "交流道刪除成功，並同時刪除相關於該交流道之費率表資料、交流道關聯、禁止通行路徑，請至各管理功能調整相關設定"
        Else
            Me.lblMessage.Text = GridViewPageInfo.Message_Delete_False(Me.Page, "交流道資料")
        End If

    End Sub


    Protected Sub GridView1_RowUpdated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdatedEventArgs) Handles GridView1.RowUpdated
        If e.AffectedRows > 0 Then
            Dim editIndex As Integer = GridView1.EditIndex
            Dim KeyID As String = GridView1.Rows(editIndex).Cells(1).Text
            Dim Oldvalues As String = ""
            Dim UID As Integer = KeyID
            ClassDB.UpdateDB("Delete from InterchangeBestFare where StartIC = " & UID.ToString & " or StopIC = " & UID.ToString)
            'ClassDB.UpdateDB("Insert into FreewayRelationEIC(EicDepartmentID,DepartmentID)values('" & EicDepartmentID & "','" & KeyID & "')")
            Me.lblMessage.Text = GridViewPageInfo.Message_Update_OK(Me.Page, "交流道資料-")
        Else
            Me.lblMessage.Text = GridViewPageInfo.Message_Update_False(Me.Page, "交流道資料")
        End If

    End Sub

    Protected Sub GridView1_RowUpdatingd(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles GridView1.RowUpdating

        Dim editIndex As Integer = e.RowIndex
        Dim KeyID As String = GridView1.Rows(editIndex).Cells(1).Text
        Dim ddlHWList As DropDownList = CType(Me.GridView1.Rows(editIndex).FindControl("ddlHWList"), DropDownList)
        Dim ddlICList As DropDownList = CType(Me.GridView1.Rows(editIndex).FindControl("ddlICList"), DropDownList)
        'Dim txtNotes As TextBox = CType(Me.GridView1.Rows(editIndex).FindControl("txtNotes"), TextBox)
        Me.sdsInterChangeList.UpdateParameters("HWUID").DefaultValue = ddlHWList.SelectedValue
        Me.sdsInterChangeList.UpdateParameters("ICAlias").DefaultValue = ddlICList.SelectedValue
        'Me.sdsInterChangeList.UpdateParameters("Notes").DefaultValue = txtNotes.Text
        Me.sdsInterChangeList.Update()
    End Sub


    Protected Sub btnCreate_Click(sender As Object, e As System.EventArgs) Handles btnCreate.Click
        Me.txtCName.Text = ""
        Me.txtMiles.Text = "0"
        Me.txtMilesBetween.Text = "0"
        Me.txtMilesN.Text = "0"
        Me.txtMilesBetweenN.Text = "0"
        Me.txtItemOrder.Text = "1"
        Me.ddlHWList.DataBind()
        Me.ddlHWList.SelectedIndex = 0
        Me.ddlICGroup.Items.Clear()
        Me.ddlICGroup.Items.Add(New ListItem("獨立群組", 0))
        Me.ddlICGroup.DataBind()
        Me.ddlICGroup.SelectedIndex = 0
        Me.ckbOutSouth.Checked = True
        Me.ckbOutNorth.Checked = True
        Me.ckbInSouth.Checked = True
        Me.ckbInNorth.Checked = True
        Me.cbIsOnline.Checked = True
        Me.cbIsVirture.Checked = True
        Me.txtMapCoords.Text = "1,1,1"
        Me.txtNotes.Text = ""
        Me.MultiView1.ActiveViewIndex = 1
    End Sub

    Protected Sub btnInsert_Click(sender As Object, e As System.EventArgs) Handles btnInsert.Click
        Try
            Me.sdsInterChangeList.Insert()
            Me.lblMessage.Text = "交流道新增成功，請至費率表管理修訂本交流道相關費率，並請至交流道關聯功能指定本交流道關聯之路徑"
            Me.MultiView1.ActiveViewIndex = 0
        Catch ex As Exception
            Dim eee = ex.Message
        End Try
    End Sub
    Protected Sub btnCancel_Click(sender As Object, e As System.EventArgs) Handles btnCancel.Click
        Me.MultiView1.ActiveViewIndex = 0
    End Sub
End Class
