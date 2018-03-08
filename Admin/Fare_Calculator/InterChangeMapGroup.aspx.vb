Imports System.Data
Imports System.Data.SqlClient

Partial Class Fare_Calculator_InterChangeMapGroup
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
        End If
    End Sub


    Protected Sub btnShowAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnShowAll.Click
        Me.txtSearch.Text = ""
    End Sub

    Protected Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand

        If e.CommandName = "EditProject" Then
            Using dr As SqlDataReader = ClassDB.GetDataReaderParam("Select * from InterChangeMapGroup where UID = @UID", New SqlParameter("@UID", e.CommandArgument))
                Try
                    If dr IsNot Nothing Then
                        If dr.Read Then
                            Me.txtCName.Text = dr("GroupName").ToString
                            Me.txtItemOrder.Text = dr("ItemOrder")
                            Me.cbIsOnline.Checked = dr("IsOnline")
                            Me.btnInsert.Visible = False
                            Me.btnUpdate.Visible = True
                            Session.Remove("EditKey")
                            Session.Add("EditKey", e.CommandArgument)
                            Me.MultiView1.ActiveViewIndex = 1
                        Else
                            Me.lblMessage.Text = "找不到詳細資料"
                        End If
                    End If
                Catch ex As Exception
                    Me.lblMessage.Text = "系統發生錯誤"
                Finally

                End Try
            End Using
        End If

    End Sub

    Protected Sub GridView1_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.DataBound
        GridViewPageInfo.GetGridViewInfo(GridView1, Me.Page, Me.sdsInterChangeFareProject)
    End Sub


    Protected Sub GridView1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView1.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            If e.Row.RowState = DataControlRowState.Alternate Or e.Row.RowState = DataControlRowState.Edit Or e.Row.RowState = DataControlRowState.Normal Or e.Row.RowState = DataControlRowState.Selected Then
                'Turn Off the Permission to Edit the Department Self
                Dim RowIndex As Integer = e.Row.RowIndex
                Dim DataKeys As String = Me.GridView1.DataKeys(RowIndex).Value().ToString
                If e.Row.RowState = DataControlRowState.Normal Or e.Row.RowState = DataControlRowState.Alternate Or e.Row.RowState = DataControlRowState.Selected Then
                    Dim BtnEdit As Button = CType(e.Row.FindControl("BtnEdit"), Button)
                    BtnEdit.CommandArgument = e.Row.DataItem("UID")

                End If
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

            Me.lblMessage.Text = GridViewPageInfo.Message_Delete_OK(Me.Page, "地圖群組資料-" & DeleteVales.Substring(0, 5) & "...")

            Dim Datakey As Integer = GridViewPageInfo.GetGridViewRowDeletedDataKey(e)


            'ClassDB.UpdateDB("delete from FreewayRelationEIC where DepartmentID='" & Datakey & "'")
        Else
            Me.lblMessage.Text = GridViewPageInfo.Message_Delete_False(Me.Page, "地圖群組")

        End If
    End Sub


    Protected Sub GridView1_RowUpdated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdatedEventArgs) Handles GridView1.RowUpdated
        If e.AffectedRows > 0 Then
            Dim editIndex As Integer = GridView1.EditIndex
            Dim KeyID As String = GridView1.Rows(editIndex).Cells(1).Text

            Dim Oldvalues As String = "11111"
            'ClassDB.UpdateDB("Insert into FreewayRelationEIC(EicDepartmentID,DepartmentID)values('" & EicDepartmentID & "','" & KeyID & "')")
            Me.lblMessage.Text = GridViewPageInfo.Message_Update_OK(Me.Page, "地圖群組-" & "...")

        Else
            Me.lblMessage.Text = GridViewPageInfo.Message_Update_False(Me.Page, "地圖群組")
        End If

    End Sub


    Protected Sub btnCreate_Click(sender As Object, e As System.EventArgs) Handles btnCreate.Click
        Me.txtCName.Text = ""
        Me.txtItemOrder.Text = "1"
        Me.cbIsOnline.Checked = False
        Me.btnInsert.Visible = True
        Me.btnUpdate.Visible = False
        Me.MultiView1.ActiveViewIndex = 1
    End Sub

    Protected Sub btnUpdate_Click(sender As Object, e As System.EventArgs) Handles btnUpdate.Click
        Try
            Me.sdsInterChangeFareProject.UpdateParameters("UID").DefaultValue = Session("EditKey").ToString
            Me.sdsInterChangeFareProject.Update()
        Catch ex As Exception
            Dim eee = ex.Message
        End Try
        '自成一個群組

        Me.MultiView1.ActiveViewIndex = 0
    End Sub
    Protected Sub btnInsert_Click(sender As Object, e As System.EventArgs) Handles btnInsert.Click
        Try
            Me.sdsInterChangeFareProject.Insert()

        Catch ex As Exception
            Dim eee = ex.Message
        End Try
        '自成一個群組

        Me.MultiView1.ActiveViewIndex = 0
    End Sub
    Protected Sub btnCancel_Click(sender As Object, e As System.EventArgs) Handles btnCancel.Click
        Me.MultiView1.ActiveViewIndex = 0
    End Sub
End Class
