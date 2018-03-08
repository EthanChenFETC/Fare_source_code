Imports System.Data
Imports System.Data.SqlClient

Partial Class Fare_Calculator_InterChangeFareEdit
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If IsNumeric(Session("ProjectID")) Then
                Me.sdsFareListProject.SelectParameters("UID").DefaultValue = Session("ProjectID").ToString
                Me.sdsInterChangeFare.SelectParameters("FareListID").DefaultValue = Session("ProjectID").ToString
                GridView1.DataBind()
                Me.GridView2.DataBind()
                Me.UpdatePanel1.DataBind()
                Me.ddlHWList.Visible = True
                Me.ddlICList.Visible = True
                Me.GridView2.Visible = True
                Me.btnCancel.Visible = True
                Session.Remove("ProjectID")
            End If
        End If
    End Sub
    Protected Sub btnShowAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnShowAll.Click
        Me.txtSearch.Text = ""
    End Sub

    Protected Sub HWList_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlHWList.DataBound

    End Sub
    Protected Sub HWList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlHWList.SelectedIndexChanged
        ddlICList.Items.Clear()
        ddlICList.Items.Add(New ListItem("全部交流道", 0))
        ddlICList.DataBind()
    End Sub


    Protected Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand
        If e.CommandName = "FareEdit" Then
            Me.sdsFareListProject.SelectParameters("UID").DefaultValue = e.CommandArgument
            Me.GridView1.DataBind()
            Me.sdsInterChangeFare.SelectParameters("FareListID").DefaultValue = e.CommandArgument
            Session.Remove("EditKey")
            Session.Add("EditKey", e.CommandArgument)
            Me.GridView2.DataBind()
            'Me.UpdatePanel1.DataBind()
            Me.ddlHWList.Visible = True
            Me.ddlICList.Visible = True
            Me.GridView2.Visible = True
            'Me.btnAddFare.Visible = True
            Me.btnCancel.Visible = True
        End If

        If e.CommandName = "DeleteItem" Then
            Dim key As Integer = e.CommandArgument
            Dim isDelete As Boolean = True
            Using dr As SqlDataReader = ClassDB.GetDataReader("Select Top 1 UID from InterchangeFareProject where FareListID = " & key)
                If dr.Read Then
                    isDelete = False
                End If
            End Using
            If isDelete Then
                Dim sql As String = ""
                ClassDB.UpdateDB("delete from InterChangeFareListProject where UID = " & key)

                ClassDB.UpdateDB("delete from InterChangeFareList where FareListID = " & key)
                GridView1.DataBind()
                ScriptManager.RegisterClientScriptBlock(Me.Page, GetType(Page), "", "alert('刪除費率成功');", True)
            Else
                ScriptManager.RegisterClientScriptBlock(Me.Page, GetType(Page), "", "alert('尚有專案使用本費率，請將專案刪除後再行刪除費率');", True)
            End If

        End If

    End Sub
    Protected Sub GridView1_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.DataBound
        GridViewPageInfo.GetGridViewInfo(GridView1, Me.Page, Me.sdsFareListProject)
    End Sub
    Protected Sub GridView2_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView2.DataBound
        GridViewPageInfo.GetGridViewInfo(GridView2, Me.Page, Me.sdsInterChangeFare)
    End Sub

    'Protected Sub GridView1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.SelectedIndexChanged
    '    Dim indexs As Integer = Me.GridView1.SelectedIndex
    '    Me.GridView2.DataBind()
    '    Me.MultiView1.ActiveViewIndex = 0
    'End Sub
    Protected Sub GridView1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView1.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            'Turn Off the Permission to Edit the Department Self
            If e.Row.RowState = 0 Or e.Row.RowState = 1 Or e.Row.RowState = 2 Or e.Row.RowState = 3 Then
                Dim RowIndex As Integer = e.Row.RowIndex
                Dim DataKeys As String = Me.GridView1.DataKeys(RowIndex).Value().ToString
                Dim BtnDel As Button = CType(e.Row.FindControl("BtnDel"), Button)
                BtnDel.CommandArgument = DataKeys
                If Session("SuperAdmin") Then
                    BtnDel.Visible = True
                Else
                    BtnDel.Visible = False
                End If

                Dim BtnFare As Button = CType(e.Row.FindControl("BtnFare"), Button)
                BtnFare.CommandArgument = DataKeys


            End If
        End If
    End Sub

    Protected Sub GridView1_RowUpdated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdatedEventArgs) Handles GridView1.RowUpdated
        If e.AffectedRows > 0 Then
            Dim editIndex As Integer = GridView1.EditIndex
            Dim KeyID As String = GridView1.Rows(editIndex).Cells(1).Text

            Dim Oldvalues As String = ""
            'ClassDB.UpdateDB("Insert into FreewayRelationEIC(EicDepartmentID,DepartmentID)values('" & EicDepartmentID & "','" & KeyID & "')")
            Me.lblMessage.Text = GridViewPageInfo.Message_Update_OK(Me.Page, "費率表資料-" & KeyID)

        Else
            Me.lblMessage.Text = GridViewPageInfo.Message_Update_False(Me.Page, "費率表資料")
        End If

    End Sub
    Protected Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        If Me.txtFareProjectName.Text IsNot Nothing Then
            If Me.txtFareProjectName.Text.Trim.Length > 0 Then
                Dim FareListID As Integer = CInt(Me.ddlFareList.SelectedValue)
                Dim iCount As Integer = ClassDB.RunSPReturnInteger("InterChangeFareList_Add",
                                                                   New SqlParameter("FareName", RemoveSQLInjection(Me.txtFareProjectName.Text)),
                                                                   New SqlParameter("FareListID", FareListID),
                                                                   New SqlParameter("IsDefault", Me.ckbIsDefault.Checked),
                                                                   New SqlParameter("Notes", ""))
                If iCount <= 0 Then
                    Dim str As String = "新增費率表發生錯誤，請洽系統管理員"
                    ScriptManager.RegisterClientScriptBlock(Me.Page, GetType(Page), "Warning", str, True)
                Else
                    Me.txtFareProjectName.Text = ""
                    Me.ddlFareList.SelectedIndex = 0
                    Me.ckbIsDefault.Checked = False
                    lblMessage.Text = "新增費率表成功"

                    'Me.gridview2.databound()
                End If
            End If
        End If
    End Sub
    Protected Sub btnAddFare_Click(sender As Object, e As EventArgs) Handles btnAddFare.Click
        Dim EditKey As Integer = Session("EditKey")
        Dim dtDelete As DataTable = ClassDB.RunReturnDataTable("Select f.ICUID from InterchangeFareList f where f.FareListID = @FarListID and f.ICUID not in (Select i.UID from InterchangeList i where i.IsOnline > 0)", New SqlParameter("@FareListID", EditKey.ToString))
        For i As Integer = 0 To dtDelete.Rows.Count - 1
            Dim row As DataRow = dtDelete.Rows(i)
            Dim ICUID As Integer = row("ICUID")
            ClassDB.UpdateDBText("Delete from InterChangeFareList where ICUID = @ICUID and FareListID = @FareListID", New SqlParameter("@FareListID", EditKey.ToString), New SqlParameter("@ICUID", ICUID))
        Next
        Dim dtAdd As DataTable = ClassDB.RunReturnDataTable("Select i.UID from InterchangeList i where i.IsOnline > 0 and UID not in (Select ICUID from InterchangeFareList f where f.FareListID = @FareListID)", New SqlParameter("@FareListID", EditKey.ToString))
        For i As Integer = 0 To dtAdd.Rows.Count - 1
            Dim row As DataRow = dtAdd.Rows(i)
            ClassDB.UpdateDBText("insert into InterChangeFareList (FareListID, ICUID, FareS, FareM, FareG, FareSN, FareMN, FareGN, FareSDiff, FareMDiff, FareGDiff, FareSDiffN, FareMDiffN, FareGDiffN, FareSAdd, FareMAdd, FareGAdd, FareSAddN, FareMAddN, FareGAddN, UpdateDateTime) values (@FareListID, @UID, 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,getdate())", New SqlParameter("@FareListID", EditKey.ToString), New SqlParameter("@UID", row("UID")))
        Next
        Me.lblFareAddMessage.Text = "本次重整新增 " & dtAdd.Rows.Count.ToString & " 筆，刪除 " & dtDelete.Rows.Count & " 筆費率，請重新檢閱並調整費率"
        Me.GridView2.DataBind()
        Me.UpdatePanel1.DataBind()
    End Sub
    Protected Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.GridView2.Visible = False
        Me.ddlHWList.Visible = False
        Me.ddlICList.Visible = False
        Me.btnAddFare.Visible = False
        Me.btnCancel.Visible = False
        Me.lblFareAddMessage.Text = ""
        Session.Remove("EditKey")
        Me.sdsFareListProject.SelectParameters("UID").DefaultValue = -1
        Me.GridView1.DataBind()
    End Sub
    Protected Sub sdsInterChangeFare_Updated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.SqlDataSourceStatusEventArgs) Handles sdsInterChangeFare.Updated
        If e.AffectedRows > 0 Then
            Dim UID As Integer = e.Command.Parameters("@UID").Value
            Using dr As SqlDataReader = ClassDB.GetDataReader("Select ICUID from InterChangeFareList where UID =" & UID.ToString)
                If dr.Read Then
                    ClassDB.UpdateDB("Delete from InterchangeBestFare where StartIC = " & dr("ICUID").ToString & " or StopIC = " & dr("ICUID").ToString)
                End If
            End Using
        End If

    End Sub

End Class
