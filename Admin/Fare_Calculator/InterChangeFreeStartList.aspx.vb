Imports System.Data
Imports System.Data.SqlClient

Partial Class Fare_Calculator_InterChangeFreeStartList
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
            Me.sdsInterChangeList.UpdateParameters("UID").DefaultValue = Me.GridView1.DataKeys(e.CommandArgument).Value
            Me.sdsInterChangeList.UpdateParameters("FreeType").DefaultValue = CInt(CType(Me.GridView1.Rows(e.CommandArgument).FindControl("rblFreeType"), RadioButtonList).SelectedValue)
            Me.sdsInterChangeList.UpdateParameters("IsOnline").DefaultValue = CType(Me.GridView1.Rows(e.CommandArgument).FindControl("ckbIsOnline"), CheckBox).Checked
            Me.sdsInterChangeList.Update()

            Dim UID As Integer = Me.GridView1.DataKeys(e.CommandArgument).Value
            Using dr As SqlDataReader = ClassDB.GetDataReader("Select StartIC, StopIC from InterChangeStartFree where UID =" & UID.ToString)
                If dr.Read Then
                    ClassDB.UpdateDB("Delete from InterchangeBestFare where StartIC in (" & dr("StartIC").ToString & "," & dr("StopIC").ToString & ") or StopIC in (" & dr("StartIC").ToString & "," & dr("StopIC").ToString & ")")
                End If
            End Using

            Me.GridView1.Rows(e.CommandArgument).RowState = DataControlRowState.Normal
            Me.GridView1.EditIndex = -1
        End If

    End Sub

    Protected Sub GridView1_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.DataBound
        GridViewPageInfo.GetGridViewInfo(GridView1, Me.Page, Me.sdsInterChangeList)
    End Sub


    Protected Sub GridView1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView1.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim drowView As System.Data.DataRowView = CType(e.Row.DataItem, System.Data.DataRowView)
            If e.Row.RowState = 0 Or e.Row.RowState = 1 Or e.Row.RowState = 2 Or e.Row.RowState = 3 Then
                Dim RowIndex As Integer = e.Row.RowIndex
                Dim DataKeys As String = Me.GridView1.DataKeys(RowIndex).Value().ToString
                Dim lblFreeType As Label = CType(e.Row.FindControl("lblFreeType"), Label)
                If e.Row.DataItem("FreeType") = 0 Then
                    lblFreeType.Text = "起始免費"
                ElseIf e.Row.DataItem("FreeType") = 1 Then
                    lblFreeType.Text = "終點免費"
                ElseIf e.Row.DataItem("FreeType") = 2 Then
                    lblFreeType.Text = "南下(東向)經過免費"
                ElseIf e.Row.DataItem("FreeType") = 3 Then
                    lblFreeType.Text = "北上(西向)經過免費"
                Else
                    lblFreeType.Text = "雙向經過免費"
                End If


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
                    Dim rblFreeType As RadioButtonList = CType(e.Row.FindControl("rblFreeType"), RadioButtonList)
                    rblFreeType.SelectedValue = e.Row.DataItem("FreeType")
                Catch ex As Exception
                    WriteErrLog(ex, Me.Page)
                End Try
                Dim ckbIsOnline As CheckBox = CType(e.Row.FindControl("ckbIsOnline"), CheckBox)
                ckbIsOnline.Checked = CType(drowView("IsOnline"), Boolean)

            End If
        End If
    End Sub
    Protected Sub GridView1_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridView1.RowDeleting

        Dim UID As Integer = Me.GridView1.DataKeys(e.RowIndex).Value
        Using dr As SqlDataReader = ClassDB.GetDataReader("Select StartIC, StopIC from InterChangeStartFree where UID =" & UID.ToString)
            If dr.Read Then
                ClassDB.UpdateDB("Delete from InterchangeBestFare where StartIC in (" & dr("StartIC").ToString & "," & dr("StopIC").ToString & ") or StopIC in (" & dr("StartIC").ToString & "," & dr("StopIC").ToString & ")")
            End If
        End Using
    End Sub

    Protected Sub GridView1_RowDeleted(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeletedEventArgs) Handles GridView1.RowDeleted
        If e.AffectedRows > 0 Then
            Dim TotalValues As Integer = e.Values.Count
            Dim i As Integer

            Dim DeleteVales As String = ""
            For i = 0 To TotalValues - 1
                DeleteVales += e.Values.Item(i) & ", "
            Next

            'Me.lblMessage.Text = GridViewPageInfo.Message_Delete_OK(Me.Page, "起點免費-" & DeleteVales.Substring(0, 5) & "...")
            Me.lblMessage.Text = GridViewPageInfo.Message_Delete_OK(Me.Page, "")
            Dim Datakey As Integer = GridViewPageInfo.GetGridViewRowDeletedDataKey(e)
            'ClassDB.UpdateDB("delete from FreewayRelationEIC where DepartmentID='" & Datakey & "'")
        Else
            Me.lblMessage.Text = GridViewPageInfo.Message_Delete_False(Me.Page, " ")

        End If
    End Sub


    Protected Sub GridView1_RowUpdated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdatedEventArgs) Handles GridView1.RowUpdated
        If e.AffectedRows > 0 Then
            Dim editIndex As Integer = GridView1.EditIndex
            Dim KeyID As String = GridView1.Rows(editIndex).Cells(1).Text
            Dim dd As String = Me.sdsInterChangeList.UpdateParameters("UID").DefaultValue
            Dim Oldvalues As String = CType(GridView1.Rows(editIndex).FindControl("lblStartICName"), Label).Text
            'ClassDB.UpdateDB("Insert into FreewayRelationEIC(EicDepartmentID,DepartmentID)values('" & EicDepartmentID & "','" & KeyID & "')")
            Me.lblMessage.Text = GridViewPageInfo.Message_Update_OK(Me.Page, "起點免費-" & Oldvalues & "...")

        Else
            Me.lblMessage.Text = GridViewPageInfo.Message_Update_False(Me.Page, "起點免費")
        End If

    End Sub


    Protected Sub btnCreate_Click(sender As Object, e As System.EventArgs) Handles btnCreate.Click
        Try
            If Me.ddlStartIC.SelectedIndex < 0 Or Me.rblStopIC.SelectedIndex < 0 Then
                Me.lblMessage.Text = GridViewPageInfo.Message_Insert_False(Me.Page, "請選擇起訖點交流道")
            End If
            Dim str As String = "Select UID from InterChangeStartFree where StartIC = @StartIC and StopIC = @StopIC and FreeType=@FreeType"
            Using dr As SqlDataReader = ClassDB.GetDataReaderParam(str, New SqlParameter("@StartIC", CInt(Me.ddlStartIC.SelectedValue)), New SqlParameter("@StopIC", CInt(rblStopIC.SelectedValue)), New SqlParameter("@FreeType", CInt(rblFreeType.SelectedValue)))
                If dr IsNot Nothing Then
                    If dr.Read Then
                        Me.lblMessage.Text = GridViewPageInfo.Message_Insert_False(Me.Page, "本資料已存在，請重新選擇")
                    Else
                        Me.sdsInterChangeList.Insert()
                        Me.lblMessage.Text = GridViewPageInfo.Message_Insert_OK(Me.Page, RemoveSQLInjection(Me.rblFreeType.Items(Me.rblFreeType.SelectedIndex).Text))
                    End If
                Else
                    Me.lblMessage.Text = GridViewPageInfo.Message_Insert_False(Me.Page, "資料發生錯誤，請重新選擇")
                End If
            End Using
        Catch ex As Exception

        End Try
    End Sub
End Class
