Imports System.Data
Imports System.Data.SqlClient

Partial Class Fare_Calculator_InterChangeRelation
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Me.ddlICList1.Items.Add(New ListItem("全部", "0"))
            Me.ddlICList1.DataBind()
            Me.ddlICList1.SelectedIndex = 0
        Else
            If Me.ddlICList1.Items.Count = 0 Then
                Me.ddlICList1.Items.Add(New ListItem("全部", "0"))
                Me.ddlICList1.DataBind()
                Me.ddlICList1.SelectedIndex = 0
            End If
        End If
    End Sub


    Protected Sub btnShowAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnShowAll.Click
        Me.ddlHWList1.SelectedIndex = 0
        Me.ddlICList1.DataBind()
        Me.ddlICList1.SelectedIndex = 0
        Me.txtSearch.Text = ""
    End Sub
    Protected Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand
        '更新
        If e.CommandName = "Update" Then
            Dim ddlHWList As DropDownList = CType(Me.GridView1.Rows(e.CommandArgument).FindControl("ddlHWList"), DropDownList)
            Dim ddlICList As DropDownList = CType(Me.GridView1.Rows(e.CommandArgument).FindControl("ddlICList"), DropDownList)
            Dim rblDirectionStart As RadioButtonList = CType(Me.GridView1.Rows(e.CommandArgument).FindControl("rblDirectionStart"), RadioButtonList)
            Dim rblDirectionStop As RadioButtonList = CType(Me.GridView1.Rows(e.CommandArgument).FindControl("rblDirectionStop"), RadioButtonList)
            Dim ckbIsOnline As CheckBox = CType(Me.GridView1.Rows(e.CommandArgument).FindControl("ckbIsOnline"), CheckBox)
            Dim UID As Integer = Me.GridView1.DataKeys(e.CommandArgument).Item("UID")
            Dim StartIC As Integer = Me.GridView1.DataKeys(e.CommandArgument).Item("StartIC")
            Me.sdsInterChangeList.UpdateParameters("UID").DefaultValue = UID
            Me.sdsInterChangeList.UpdateParameters("StartIC").DefaultValue = StartIC
            Me.sdsInterChangeList.UpdateParameters("StopIC").DefaultValue = ddlICList.SelectedValue
            Me.sdsInterChangeList.UpdateParameters("DirectionStart").DefaultValue = rblDirectionStart.SelectedValue
            Me.sdsInterChangeList.UpdateParameters("DirectionStop").DefaultValue = rblDirectionStop.SelectedValue
            Me.sdsInterChangeList.UpdateParameters("IsOnline").DefaultValue = ckbIsOnline.Checked
            Me.sdsInterChangeList.Update()
        End If
    End Sub

    Protected Sub GridView1_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.DataBound
        GridViewPageInfo.GetGridViewInfo(GridView1, Me.Page, Me.sdsInterChangeList)
    End Sub


    Protected Sub GridView1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView1.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            'Turn Off the Permission to Edit the Department Self
            Dim rv As DataRowView = e.Row.DataItem
            If e.Row.RowState = 0 Or e.Row.RowState = 1 Or e.Row.RowState = 2 Or e.Row.RowState = 3 Then
                Dim RowIndex As Integer = e.Row.RowIndex
                Dim DataKeys As String = Me.GridView1.DataKeys(RowIndex).Value().ToString
                Dim btn1 As Button = CType(e.Row.FindControl("BtnEdit"), Button)
                btn1.CommandArgument = RowIndex
                'Dim btn2 As Button = CType(e.Row.FindControl("BtnDel"), Button)
                'btn1.Enabled = False
                'btn2.Enabled = False
                Dim lblHWName As Label = CType(e.Row.FindControl("lblHWName"), Label)
                lblHWName.Text = (rv("HWNameD"))
                Dim lblICName As Label = CType(e.Row.FindControl("lblICName"), Label)
                lblICName.Text = (rv("ICNameD"))
                Dim rblDirectionStart As RadioButtonList = CType(e.Row.FindControl("rblDirectionStart"), RadioButtonList)
                rblDirectionStart.SelectedValue = CInt(rv("DirectionStart"))
                Dim rblDirectionStop As RadioButtonList = CType(e.Row.FindControl("rblDirectionStop"), RadioButtonList)
                rblDirectionStop.SelectedValue = CInt(rv("DirectionStop"))
                Dim ckbIsOnline As CheckBox = CType(e.Row.FindControl("ckbIsOnline"), CheckBox)
                ckbIsOnline.Checked = CType(rv("IsOnline"), Boolean)
            End If

            '關閉非管理者時,單位變更到民意信箱
            If e.Row.RowState = DataControlRowState.Edit Or e.Row.RowState = 5 Then    'In the Edit Mode in Normal & Alt Rows
                Try
                    Dim RowIndex As Integer = e.Row.RowIndex
                    Dim drowView As System.Data.DataRowView = CType(e.Row.DataItem, System.Data.DataRowView)
                    Dim btn1 As Button = CType(e.Row.FindControl("Button1"), Button)
                    btn1.CommandArgument = RowIndex
                    Dim ddlHWList As DropDownList = CType(e.Row.FindControl("ddlHWList"), DropDownList)
                    Dim ddlICList As DropDownList = CType(e.Row.FindControl("ddlICList"), DropDownList)
                    ddlHWList.DataBind()
                    ddlHWList.SelectedValue = drowView.Item("HWUIDD")
                    Me.sdsICList4.SelectParameters("HWUID").DefaultValue = ddlHWList.SelectedValue
                    ddlICList.Items.Clear()
                    ddlICList.DataBind()
                    ddlICList.SelectedValue = drowView.Item("StopIC")
                    Dim rblDirectionStart As RadioButtonList = CType(e.Row.FindControl("rblDirectionStart"), RadioButtonList)
                    rblDirectionStart.SelectedValue = CInt(rv("DirectionStart"))
                    Dim rblDirectionStop As RadioButtonList = CType(e.Row.FindControl("rblDirectionStop"), RadioButtonList)
                    rblDirectionStop.SelectedValue = CInt(rv("DirectionStop"))
                    Dim ckbIsOnline As CheckBox = CType(e.Row.FindControl("ckbIsOnline"), CheckBox)
                    ckbIsOnline.Checked = CType(rv("IsOnline"), Boolean)
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

            Me.lblMessage.Text = GridViewPageInfo.Message_Delete_OK(Me.Page, "單位資料-" & DeleteVales.Substring(0, 5) & "...")

            Dim Datakey As Integer = GridViewPageInfo.GetGridViewRowDeletedDataKey(e)


            'ClassDB.UpdateDB("delete from FreewayRelationEIC where DepartmentID='" & Datakey & "'")
        Else
            Me.lblMessage.Text = GridViewPageInfo.Message_Delete_False(Me.Page, "單位資料")

        End If
    End Sub


    Protected Sub GridView1_RowUpdated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdatedEventArgs) Handles GridView1.RowUpdated
        If e.AffectedRows > 0 Then
            Dim editIndex As Integer = GridView1.EditIndex
            Dim KeyID As String = GridView1.Rows(editIndex).Cells(1).Text

            Dim Oldvalues As String = "11111"
            'ClassDB.UpdateDB("Insert into FreewayRelationEIC(EicDepartmentID,DepartmentID)values('" & EicDepartmentID & "','" & KeyID & "')")
            Me.lblMessage.Text = GridViewPageInfo.Message_Update_OK(Me.Page, "單位資料-" & Oldvalues.Substring(0, 5) & "...")

        Else
            Me.lblMessage.Text = GridViewPageInfo.Message_Update_False(Me.Page, "單位資料")
        End If

    End Sub


    Protected Sub btnCreate_Click(sender As Object, e As System.EventArgs) Handles btnCreate.Click
        'Me.ddlHWList2.DataBind()
        'Me.ddlHWList2.SelectedIndex = 0
        'Me.ddlICList2.DataBind()
        'Me.ddlICList2.SelectedIndex = 0
        'Me.ddlHWList3.DataBind()
        'Me.ddlHWList3.SelectedIndex = 0
        'Me.ddlICList3.DataBind()
        'Me.ddlICList3.SelectedIndex = 0
        Me.MultiView1.ActiveViewIndex = 1
    End Sub

    Protected Sub btnInsert_Click(sender As Object, e As System.EventArgs) Handles btnInsert.Click
        Try
            ClassDB.UpdateDB("Delete from InterchangeBestFare where StartIC in (" & ddlICList2.SelectedValue.ToString & "," & ddlICList3.SelectedValue.ToString & ") or StopIC in (" & ddlICList2.SelectedValue.ToString & "," & ddlICList3.SelectedValue.ToString & ")")

            Me.sdsInterChangeList.Insert()
        Catch ex As Exception
            Dim eee = ex.Message
        End Try
        Me.MultiView1.ActiveViewIndex = 0
    End Sub
    Protected Sub btnCancel_Click(sender As Object, e As System.EventArgs) Handles btnCancel.Click
        Me.MultiView1.ActiveViewIndex = 0
    End Sub
    Protected Sub ddlHWList4_SelectedIndexChanged(sender As Object, e As System.EventArgs)
        Dim rowindex As Integer = CType(CType(sender, DropDownList).Parent.Parent, GridViewRow).RowIndex
        Me.sdsICList4.SelectParameters("HWUID").DefaultValue = CType(Me.GridView1.Rows(rowindex).FindControl("ddlHWList"), DropDownList).SelectedValue
        Dim ddlICList As DropDownList = CType(Me.GridView1.Rows(rowindex).FindControl("ddlICList"), DropDownList)
        ddlIClist.Items.Clear()
        ddlICList.DataBind()
        ddlICList.SelectedIndex = 0
    End Sub

    Protected Sub ddlHWList1_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlHWList1.SelectedIndexChanged
        Me.ddlICList1.Items.Clear()
        Me.ddlICList1.Items.Add(New ListItem("全部", "0"))
        Me.ddlICList1.DataBind()
        Me.ddlICList1.SelectedIndex = 0
    End Sub

    Protected Sub ddlHWList_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlHWList2.SelectedIndexChanged
        Me.ddlICList2.Items.Clear()
        Me.ddlICList2.DataBind()
        Me.ddlICList2.SelectedIndex = 0

    End Sub

    Protected Sub ddlHWListD_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlHWList3.SelectedIndexChanged
        Me.ddlICList3.Items.Clear()
        Me.ddlICList3.DataBind()
        Me.ddlICList3.SelectedIndex = 0
    End Sub
End Class
