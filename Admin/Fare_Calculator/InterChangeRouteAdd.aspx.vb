Imports System.Data
Imports System.Data.SqlClient

Partial Class Fare_Calculator_InterChangeRouteAdd
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Me.ddlHWInsert.DataBind()
            Me.ddlHWInsert.SelectedIndex = 0
        End If
    End Sub


    Protected Sub btnShowAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnShowAll.Click
        Me.ddlHWList.SelectedIndex = 0
        Me.txtSearch.Text = ""
        Me.UpdatePanel1.Update()
    End Sub
    Protected Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand
        '更新
        If e.CommandName = "UpdateItem" Then
            Try

                Me.sdsInterChangeRouteList.UpdateParameters("UID").DefaultValue = Me.GridView1.DataKeys(e.CommandArgument).Value
                Me.sdsInterChangeRouteList.UpdateParameters("AxisX").DefaultValue = CType(Me.GridView1.Rows(e.CommandArgument).Cells(5).Controls(0), TextBox).Text
                Me.sdsInterChangeRouteList.UpdateParameters("AxisY").DefaultValue = CType(Me.GridView1.Rows(e.CommandArgument).Cells(6).Controls(0), TextBox).Text
                Me.sdsInterChangeRouteList.UpdateParameters("ItemOrder").DefaultValue = CType(Me.GridView1.Rows(e.CommandArgument).Cells(7).Controls(0), TextBox).Text
                Me.sdsInterChangeRouteList.UpdateParameters("IsOnline").DefaultValue = CType(Me.GridView1.Rows(e.CommandArgument).FindControl("ckbIsOnline"), CheckBox).Checked
                Me.sdsInterChangeRouteList.Update()
                Me.GridView1.Rows(e.CommandArgument).RowState = DataControlRowState.Normal
                Me.GridView1.EditIndex = -1
            Catch ex As Exception

            End Try
        End If

    End Sub

    Protected Sub GridView1_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.DataBound
        GridViewPageInfo.GetGridViewInfo(GridView1, Me.Page, Me.sdsInterChangeRouteList)
    End Sub


    Protected Sub GridView1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView1.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim drowView As System.Data.DataRowView = CType(e.Row.DataItem, System.Data.DataRowView)
            If e.Row.RowState = 0 Or e.Row.RowState = 1 Or e.Row.RowState = 2 Or e.Row.RowState = 3 Then
                Dim RowIndex As Integer = e.Row.RowIndex
                Dim DataKeys As String = Me.GridView1.DataKeys(RowIndex).Value().ToString
                Dim lblHWName As Label = CType(e.Row.FindControl("lblHWName"), Label)
                lblHWName.Text = (drowView("HWName"))
                Dim lblStartICName As Label = CType(e.Row.FindControl("lblStartICName"), Label)
                lblStartICName.Text = (drowView("StartICName"))
                Dim lblStopICName As Label = CType(e.Row.FindControl("lblStopICName"), Label)
                lblStopICName.Text = (drowView("StopICName"))
                Dim ckbIsOnline As CheckBox = CType(e.Row.FindControl("ckbIsOnline"), CheckBox)
                ckbIsOnline.Checked = CType(drowView("IsOnline"), Boolean)

            End If
            If e.Row.RowState = DataControlRowState.Edit Or e.Row.RowState = 5 Then    'In the Edit Mode in Normal & Alt Rows
                Try
                    Dim RowIndex As Integer = e.Row.RowIndex
                    Dim btn As Button = CType(e.Row.FindControl("btnUpdate"), Button)
                    btn.CommandArgument = e.Row.RowIndex
                    Dim ckbIsOnline As CheckBox = CType(e.Row.FindControl("ckbIsOnline"), CheckBox)
                    ckbIsOnline.Checked = CType(drowView("IsOnline"), Boolean)

                Catch ex As Exception
                    WriteErrLog(ex, Me.Page)
                End Try
            End If
        End If
    End Sub

    Protected Sub GridView1_RowDeleted(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeletedEventArgs) Handles GridView1.RowDeleted
        If e.AffectedRows > 0 Then
            Dim TotalValues As Integer = e.Values.Count
            Dim i As Integer

            Dim DeleteVales As String = ""
            For i = 0 To TotalValues - 1
                DeleteVales += e.Values.Item(i) & ","
            Next

            Me.lblMessage.Text = GridViewPageInfo.Message_Delete_OK(Me.Page, "路徑路線-" & DeleteVales.Substring(0, 5) & "...")

            Dim Datakey As Integer = GridViewPageInfo.GetGridViewRowDeletedDataKey(e)


            'ClassDB.UpdateDB("delete from FreewayRelationEIC where DepartmentID='" & Datakey & "'")
        Else
            Me.lblMessage.Text = GridViewPageInfo.Message_Delete_False(Me.Page, "路徑路線")

        End If
    End Sub


    Protected Sub GridView1_RowUpdated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdatedEventArgs) Handles GridView1.RowUpdated
        If e.AffectedRows > 0 Then
            Dim editIndex As Integer = GridView1.EditIndex
            Dim KeyID As String = GridView1.Rows(editIndex).Cells(1).Text
            Dim dd As String = Me.sdsInterChangeRouteList.UpdateParameters("UID").DefaultValue
            Dim Oldvalues As String = CType(GridView1.Rows(editIndex).FindControl("lblStartICName"), Label).Text
            'ClassDB.UpdateDB("Insert into FreewayRelationEIC(EicDepartmentID,DepartmentID)values('" & EicDepartmentID & "','" & KeyID & "')")
            Me.lblMessage.Text = GridViewPageInfo.Message_Update_OK(Me.Page, "路徑路線-" & Oldvalues & "...")
        Else
            Me.lblMessage.Text = GridViewPageInfo.Message_Update_False(Me.Page, "路徑路線")
        End If

    End Sub


    Protected Sub btnCreate_Click(sender As Object, e As System.EventArgs) Handles btnCreate.Click
        Try
            If Me.ddlStartIC.SelectedIndex < 0 Or Me.ddlStopIC.SelectedIndex < 0 Then
                Me.lblMessage.Text = GridViewPageInfo.Message_Insert_False(Me.Page, "請選擇起訖點交流道")
            End If

            Me.sdsInterChangeRouteList.Insert()
            Me.lblMessage.Text = GridViewPageInfo.Message_Insert_OK(Me.Page, "(" & RemoveSQLInjection(Me.ddlHWInsert.SelectedItem.Text) & ")" & RemoveSQLInjection(Me.ddlStartIC.SelectedItem.Text) & " 至 ")

        Catch ex As Exception
            Dim eee = ex.Message
        End Try
    End Sub
End Class
