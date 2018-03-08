Imports System.Data
Imports System.Data.SqlClient

Partial Class Fare_Calculator_InterChangeProhibitRoute
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
        End If
    End Sub
    Protected Sub btnShowAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnShowAll.Click
        Me.txtSearch.Text = ""
    End Sub

    Protected Sub GridView1_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.DataBound
        GridViewPageInfo.GetGridViewInfo(GridView1, Me.Page, Me.sdsInterChangeRouteList)
    End Sub
    Protected Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand
        If e.CommandName = "EditProject" Then
            Me.MultiView1.ActiveViewIndex = 1
            Dim UID As Integer = e.CommandArgument
            Session("EditKey") = e.CommandArgument
            Using dr As SqlDataReader = ClassDB.GetDataReaderParam("Select ProjectID, StartIC, Routes, RouteName, Notes, IsOnline from InterchangeProhibitRoute where UID = @UID", New SqlParameter("@UID", UID))
                Try
                    If dr IsNot Nothing Then
                        If dr.Read Then
                            Me.cbIsOnline.Checked = dr("IsOnline")
                            Me.ddlProject.SelectedValue = dr("ProjectID")
                            Me.lblRouteList.Text = (dr("RouteName"))
                            If Not CheckSqlInjectionWording(dr("Routes")) Then
                                Me.hidRouteList.Value = dr("Routes")
                            Else
                                Me.hidRouteList.Value = (dr("Routes"))
                            End If
                            Me.txtNotes.Text = (dr("Notes").ToString)
                            Me.dlICList.DataBind()
                            Me.btnInsert.Visible = False
                            Me.btnUpdate.Visible = True
                        End If
                    End If
                Catch ex As Exception
                Finally

                End Try
            End Using
        End If
    End Sub
    Protected Sub Repeater_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs)
        If e.CommandName.Equals("ICSelect") Then
            Dim arg() As String = e.CommandArgument.ToString.Split(",")
            If (Me.hidRouteList.Value & ",").IndexOf("," & arg(0) & ",") >= 0 Then
                ScriptManager.RegisterClientScriptBlock(Me.UpdatePanel1, GetType(UpdatePanel), "warning", "alert('交流道已存在');", True)
                Exit Sub
            End If
            Dim ddd As Integer = Me.hidRouteList.Value.Split(",").Length
            If Me.hidRouteList.Value.Split(",").Length > 1 Then
                Dim dt As DataTable = ClassDB.RunReturnDataTable("Select UID from InterChangeRelation where StartIC = @StartIC and StopIC = @StopID", New SqlParameter("@StartIC", Me.hidRouteList.Value.Split(",")(Me.hidRouteList.Value.Split(",").Length - 1)), New SqlParameter("@StopID", CInt(arg(0))))
                If dt.Rows.Count = 0 Then
                    ScriptManager.RegisterClientScriptBlock(Me.UpdatePanel1, GetType(UpdatePanel), "warning", "alert('與前一個交流道沒有連接');", True)
                    Exit Sub
                End If
            End If
            Me.lblRouteList.Text &= "," & "(" & arg(2) & ")" & arg(1)
            Me.hidRouteList.Value &= "," & arg(0)
        End If
    End Sub
    Protected Sub btnClear_Click(sender As Object, e As System.EventArgs) Handles btnClear.Click
        Me.lblRouteList.Text = ""
        Me.hidRouteList.Value = ""
    End Sub

    Protected Sub dlICList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs) Handles dlICList.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim lblHWName As Label = CType(e.Item.FindControl("lblHWName"), Label)
            lblHWName.Text = (e.Item.DataItem("HWName")) 'Me.dlICList.DataKeys(e.Item.ItemIndex)
            Dim sdsICListdl As SqlDataSource = CType(e.Item.FindControl("sdsICListdl"), SqlDataSource)
            sdsICListdl.SelectParameters("HWUID").DefaultValue = Me.dlICList.DataKeys(e.Item.ItemIndex)
            Dim rptRouteList As Repeater = CType(e.Item.FindControl("rptRouteList"), Repeater)
            rptRouteList.DataSourceID = sdsICListdl.ID
            sdsICListdl.SelectParameters("HWUID").DefaultValue = Me.dlICList.DataKeys(e.Item.ItemIndex)
            rptRouteList.DataBind()
        End If
    End Sub

    Protected Sub rptRouteList_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim btnIC As Button = CType(e.Item.FindControl("btnIC"), Button)
            btnIC.Text = (e.Item.DataItem("ICName"))
            btnIC.CommandArgument = e.Item.DataItem("UID") & "," & e.Item.DataItem("ICName") & "," & e.Item.DataItem("HWName")
        End If
    End Sub
    Protected Sub GridView1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView1.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            If e.Row.RowState = DataControlRowState.Alternate Or e.Row.RowState = DataControlRowState.Edit Or e.Row.RowState = DataControlRowState.Normal Or e.Row.RowState = DataControlRowState.Selected Then
                Dim RowIndex As Integer = e.Row.RowIndex
                Dim DataKeys As String = Me.GridView1.DataKeys(RowIndex).Value().ToString
                Dim BtnEdit As Button = CType(e.Row.FindControl("BtnEdit"), Button)
                BtnEdit.CommandArgument = DataKeys
                Dim lblStartICName As Label = CType(e.Row.FindControl("lblStartICName"), Label)
                lblStartICName.Text = (e.Row.DataItem("ICName"))
            End If
        End If
    End Sub
    Protected Sub GridView1_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridView1.RowDeleting

        Dim UID As Integer = Me.GridView1.DataKeys(e.RowIndex).Value
        Using dr As SqlDataReader = ClassDB.GetDataReader("Select StartIC, Routes from InterChangeProhibitRoute where UID =" & UID.ToString)
            If dr.Read Then
                ClassDB.UpdateDB("Delete from InterchangeBestFare where StartIC in (" & dr("StartIC").ToString & "," & dr("Routes").ToString & ") or StopIC in (" & dr("StartIC").ToString & "," & dr("Routes").ToString & ")")
            End If
        End Using
    End Sub

    Protected Sub GridView1_RowDeleted(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeletedEventArgs) Handles GridView1.RowDeleted
        If e.AffectedRows > 0 Then
            Dim TotalValues As Integer = e.Values.Count
            Dim i As Integer

            Dim DeleteVales As String = ""
            For i = 0 To TotalValues - 1
                DeleteVales += e.Values.Item(i) & ","
            Next

            'Me.lblMessage.Text = GridViewPageInfo.Message_Delete_OK(Me.Page, "禁止通行路徑資料-" & DeleteVales.Substring(0, 5) & "...")
            Me.lblMessage.Text = GridViewPageInfo.Message_Delete_OK(Me.Page, "")

            Dim Datakey As Integer = GridViewPageInfo.GetGridViewRowDeletedDataKey(e)


            'ClassDB.UpdateDB("delete from FreewayRelationEIC where DepartmentID='" & Datakey & "'")
        Else
            Me.lblMessage.Text = GridViewPageInfo.Message_Delete_False(Me.Page, "")

        End If
    End Sub

    Protected Sub btnCreate_Click(sender As Object, e As System.EventArgs) Handles btnCreate.Click
        Me.dlICList.DataBind()
        Me.cbIsOnline.Checked = True
        Me.ddlProject.SelectedIndex = 0
        Me.lblRouteList.Text = ""
        Me.hidRouteList.Value = ""
        Me.txtNotes.Text = ""
        Me.btnInsert.Visible = True
        Me.btnUpdate.Visible = False
        Me.MultiView1.ActiveViewIndex = 1
    End Sub

    Protected Sub btnUpdate_Click(sender As Object, e As System.EventArgs) Handles btnUpdate.Click
        Try
            If Me.hidRouteList.Value.Length = 0 Then
                ScriptManager.RegisterClientScriptBlock(Me.UpdatePanel1, GetType(UpdatePanel), "warning", "alert('沒有選擇交流道');", True)
                Exit Sub
            End If
            Dim Routes As String = Me.hidRouteList.Value
            Dim RouteName As String = Me.lblRouteList.Text
            'If Routes.Length > 0 Then
            '    Routes = Routes.Substring(1)
            '    RouteName = RouteName.Substring(1)
            'End If
            If IsNumeric(Session("EditKey")) Then
                If Routes.StartsWith(",") Then
                    Routes = Routes.Substring(1)
                End If
                If Routes.EndsWith(",") Then
                    Routes = Routes.Substring(0, Routes.Length - 1)
                End If
                ClassDB.UpdateDB("Delete from InterchangeBestFare where StartIC in (" & Routes.ToString & ") or StopIC in (" & Routes.ToString & ")")

                Me.sdsInterChangeRouteList.UpdateParameters("StartIC").DefaultValue = Routes.Split(",")(0)
                Me.sdsInterChangeRouteList.UpdateParameters("UID").DefaultValue = Session("EditKey").ToString
                Me.sdsInterChangeRouteList.UpdateParameters("Routes").DefaultValue = Routes
                Me.sdsInterChangeRouteList.UpdateParameters("RouteName").DefaultValue = RouteName
                Me.sdsInterChangeRouteList.Update()
                ClassDB.UpdateDB("Delete from InterchangeBestFare where StartIC in (" & Routes & ") or StopIC in (" & Routes & ")")
            End If
        Catch ex As Exception
            Dim eee = ex.Message
        End Try
        '自成一個群組

        Me.MultiView1.ActiveViewIndex = 0
    End Sub
    Protected Sub btnInsert_Click(sender As Object, e As System.EventArgs) Handles btnInsert.Click
        Try
            If Me.hidRouteList.Value.Length = 0 Then
                ScriptManager.RegisterClientScriptBlock(Me.UpdatePanel1, GetType(UpdatePanel), "warning", "alert('沒有選擇交流道');", True)
                Exit Sub
            End If

            Dim Routes As String = Me.hidRouteList.Value
            Dim RouteName As String = Me.lblRouteList.Text
            If Routes.Length > 0 Then
                Routes = Routes.Substring(1)
                RouteName = RouteName.Substring(1)
            End If

            ClassDB.UpdateDB("Delete from InterchangeBestFare where StartIC in (" & Routes.ToString & ") or StopIC in (" & Routes.ToString & ")")

            Me.sdsInterChangeRouteList.InsertParameters("StartIC").DefaultValue = Routes.Split(",")(0)
            Me.sdsInterChangeRouteList.InsertParameters("ProjectID").DefaultValue = Me.ddlProject.SelectedValue
            Me.sdsInterChangeRouteList.InsertParameters("Routes").DefaultValue = Routes
            Me.sdsInterChangeRouteList.InsertParameters("RouteName").DefaultValue = RouteName
            Me.sdsInterChangeRouteList.Insert()

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
