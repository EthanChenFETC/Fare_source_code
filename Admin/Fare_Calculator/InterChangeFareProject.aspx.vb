Imports System.Data
Imports System.Data.SqlClient

Partial Class Fare_Calculator_InterChangeFareProject
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
        End If
    End Sub


    Protected Sub btnShowAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnShowAll.Click
        Me.txtSearch.Text = ""
    End Sub

    Protected Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand
        If e.CommandName = "BaseFare" Then
            Session.Remove("ProjectID")
            Session.Add("ProjectID", e.CommandArgument)
            'Response.Redirect("InterChangeFareEdit.aspx")
            'Server.Execute("InterChangeFareEdit.aspx")
            'HttpContext.Current.ApplicationInstance.CompleteRequest()
            InterfaceBuilder.ChangeUserControlByPassFileName(Me.Page, "InterChangeFareEdit.aspx", "aspx")
        End If
        If e.CommandName = "EditProject" Then
            Using dr As SqlDataReader = ClassDB.GetDataReaderParam("Select * from InterchangeFareProject where UID = @UID", New SqlParameter("@UID", e.CommandArgument))
                Try
                    If dr IsNot Nothing Then
                        If dr.Read Then
                            Me.txtCName.Text = dr("ProjectName").ToString
                            Me.CalendarPopupStartDate.SelectedValue = dr("StartDate")
                            Me.CalendarPopupEndDate.SelectedValue = dr("EndDate")
                            BuildDDL()
                            Try
                                Me.ddlFareList.SelectedValue = dr("ProjectAlias").ToString
                            Catch ex As Exception
                                Me.ddlFareList.SelectedValue = e.CommandArgument
                            End Try
                            Try
                                Me.ddlFareListProject.SelectedValue = dr("FareListID").ToString
                            Catch ex As Exception
                                Me.ddlFareListProject.SelectedIndex = 0
                            End Try
                            Try
                                Me.ddlStartHour.SelectedValue = dr("StartHour").ToString
                            Catch ex As Exception
                                Me.ddlStartHour.SelectedIndex = 0
                            End Try
                            Try
                                Me.ddlEndHour.SelectedValue = dr("EndHour").ToString
                            Catch ex As Exception
                                Me.ddlEndHour.SelectedIndex = 23
                            End Try
                            Me.txtFareS.Text = dr("FareS").ToString
                            Me.txtFareM.Text = dr("FareM").ToString
                            Me.txtFareG.Text = dr("FareG").ToString
                            Me.txtFareSDiscount.Text = dr("FareSDiscount").ToString
                            Me.txtFareMDiscount.Text = dr("FareMDiscount").ToString
                            Me.txtFareGDiscount.Text = dr("FareGDiscount").ToString
                            Me.txtFreeMiles.Text = dr("FreeMiles").ToString
                            Me.txtDiscountMiles.Text = dr("DiscountMiles").ToString
                            Me.txtRateS2M.Text = dr("RateS2M").ToString
                            Me.txtRateS2G.Text = dr("RateS2G").ToString
                            Me.txtRateLongDistant.Text = dr("RateLongDistant").ToString
                            Me.txtRateETag.Text = dr("RateETag").ToString
                            Me.txtRateReserved.Text = dr("RateReserved").ToString
                            Me.txtRateNoReserved.Text = dr("RateNoReserved").ToString
                            Me.txtItemOrder.Text = dr("ItemOrder").ToString
                            Me.cbIsOnline.Checked = dr("IsOnline")
                            Me.cbIsFree.Checked = dr("IsFree")
                            Me.cbIsLongDistant.Checked = dr("IsLongDistant")
                            Me.cbIsFreeMiles.Checked = dr("IsFreeMiles")
                            Me.cbIsDifferent.Checked = dr("IsDifferent")
                            Me.cbIsAdd.Checked = dr("IsAdd")
                            Me.cbIsReserved.Checked = dr("IsReserved")
                            Me.cbIsJustInTime.Checked = dr("IsJustInTime")
                            Me.cbIsTaxi.Checked = dr("IsTaxi")
                            Me.txtNotes.Text = dr("Notes").ToString
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
        If e.CommandName = "ViewProject" Then
            Dim path As String = ConfigurationManager.AppSettings("FareURL")
            If path Is Nothing Then
                path = "http://fare.fetc.net.tw/"
            End If
            If Not path.Substring(path.Length - 1).Equals("/") Then
                path = path & "/"
            End If
            path = path & "SuggestedTest.aspx?id=" & e.CommandArgument
            Dim script As String = "window.open('" & path & "');"
            Me.Page.ClientScript.RegisterClientScriptBlock(Me.Page.GetType, "openwindow", script, True)
        End If
    End Sub
    Private Sub BuildDDL()
        Me.ddlFareList.Items.Clear()
        'Me.ddlFareListProject.Items.Clear()
        'Me.ddlFareList.Items.Add(New ListItem("無", "0"))
        'Me.ddlFareListProject.Items.Add(New ListItem("無", "0"))
        Me.ddlFareList.DataBind()
        Me.ddlFareListProject.DataBind()
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
                Dim drv As DataRowView = e.Row.DataItem
                If e.Row.RowState = DataControlRowState.Normal Or e.Row.RowState = DataControlRowState.Alternate Or e.Row.RowState = DataControlRowState.Selected Then
                    Dim BtnFare As LinkButton = CType(e.Row.FindControl("BtnFare"), LinkButton)
                    BtnFare.CommandArgument = e.Row.DataItem("FareListID") 'DataKeys
                    Dim BtnEdit As Button = CType(e.Row.FindControl("BtnEdit"), Button)
                    BtnEdit.CommandArgument = DataKeys
                    Dim BtnPreview As Button = CType(e.Row.FindControl("BtnPreview"), Button)
                    BtnPreview.CommandArgument = DataKeys
                    Dim lblProject As Label = CType(e.Row.FindControl("lblProject"), Label)
                    Dim lblStartDate As Label = CType(e.Row.FindControl("lblStartDate"), Label)
                    Dim lblEndDate As Label = CType(e.Row.FindControl("lblEndDate"), Label)
                    Dim lblNotes As Label = CType(e.Row.FindControl("lblNotes"), Label)
                    lblProject.Text = (IIf(IsDBNull(drv("aProjectName")), "", drv("aProjectName")))
                    lblStartDate.Text = (IIf(IsDBNull(drv("StartDate")), "", CType(drv("StartDate"), DateTime).ToString("yyyy/MM/dd")))
                    lblEndDate.Text = (IIf(IsDBNull(drv("EndDate")), "", CType(drv("EndDate"), DateTime).ToString("yyyy/MM/dd")))
                    lblNotes.Text = (IIf(IsDBNull(drv("Notes")), "", drv("Notes")))

                    BtnFare.Text = (drv("FareName"))

                ElseIf e.Row.RowState = DataControlRowState.Edit Then
                    Dim ddlFareListProject As DropDownList = CType(e.Row.FindControl("ddlFareListProject"), DropDownList)
                    Try
                        ddlFareListProject.SelectedValue = e.Row.DataItem("FareListID")
                    Catch ex As Exception
                        ddlFareListProject.SelectedIndex = 0
                    End Try

                    Dim ddlProject As DropDownList = CType(e.Row.FindControl("ddlProject"), DropDownList)
                    Try
                        ddlProject.SelectedValue = e.Row.DataItem("ProjectAlias")
                    Catch ex As Exception
                        ddlProject.SelectedIndex = 0
                    End Try
                    Dim BtnDel As Button = CType(e.Row.FindControl("BtnDel"), Button)
                    If BtnDel IsNot Nothing Then
                        If Session("SuperAdmin") Then
                            BtnDel.Visible = True
                        Else
                            BtnDel.Visible = False
                        End If
                    End If

                    Dim CalendarPopupStartDate As eWorld.UI.CalendarPopup = CType(e.Row.FindControl("CalendarPopupStartDate"), eWorld.UI.CalendarPopup)
                    CalendarPopupStartDate.SelectedDate = CType(drv("StartDate"), DateTime)
                    Dim CalendarPopupEndDate As eWorld.UI.CalendarPopup = CType(e.Row.FindControl("CalendarPopupEndDate"), eWorld.UI.CalendarPopup)
                    CalendarPopupEndDate.SelectedDate = CType(drv("EndDate"), DateTime)
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

            Me.lblMessage.Text = GridViewPageInfo.Message_Delete_OK(Me.Page, "專案資料-" & DeleteVales.Substring(0, 5) & "...")

            Dim Datakey As Integer = GridViewPageInfo.GetGridViewRowDeletedDataKey(e)


            'ClassDB.UpdateDB("delete from FreewayRelationEIC where DepartmentID='" & Datakey & "'")
        Else
            Me.lblMessage.Text = GridViewPageInfo.Message_Delete_False(Me.Page, "專案資料")

        End If
    End Sub


    Protected Sub GridView1_RowUpdated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdatedEventArgs) Handles GridView1.RowUpdated
        If e.AffectedRows > 0 Then
            Dim editIndex As Integer = GridView1.EditIndex
            Dim KeyID As String = GridView1.Rows(editIndex).Cells(1).Text

            Dim Oldvalues As String = "11111"
            'ClassDB.UpdateDB("Insert into FreewayRelationEIC(EicDepartmentID,DepartmentID)values('" & EicDepartmentID & "','" & KeyID & "')")
            Me.lblMessage.Text = GridViewPageInfo.Message_Update_OK(Me.Page, "專案資料-" & "...")

        Else
            Me.lblMessage.Text = GridViewPageInfo.Message_Update_False(Me.Page, "專案資料")
        End If

    End Sub


    Protected Sub btnCreate_Click(sender As Object, e As System.EventArgs) Handles btnCreate.Click
        Me.txtCName.Text = ""
        Me.CalendarPopupStartDate.SelectedValue = Date.Today
        Me.CalendarPopupEndDate.SelectedValue = Date.Today
        If Me.ddlFareListProject.Items.Count <= 0 Then
            Me.ddlFareListProject.DataBind()
        End If
        Me.ddlFareList.SelectedIndex = 0
        Me.ddlFareListProject.SelectedIndex = 0
        Me.ddlStartHour.SelectedValue = "0"
        Me.ddlEndHour.SelectedValue = "24"
        Me.txtFareS.Text = "1.2"
        Me.txtFareM.Text = "1.5"
        Me.txtFareG.Text = "1.8"
        Me.txtFareSDiscount.Text = "0.9"
        Me.txtFareMDiscount.Text = "1.12"
        Me.txtFareGDiscount.Text = "1.35"
        Me.txtFreeMiles.Text = "20"
        Me.txtDiscountMiles.Text = "200"
        Me.txtRateS2M.Text = "1.25"
        Me.txtRateS2G.Text = "1.5"
        Me.txtRateLongDistant.Text = "0.75"
        Me.txtRateETag.Text = "0.9"
        Me.txtRateReserved.Text = "0.9"
        Me.txtItemOrder.Text = "1"
        Me.cbIsOnline.Checked = False
        Me.cbIsFree.Checked = False
        Me.cbIsLongDistant.Checked = True
        Me.cbIsFreeMiles.Checked = True
        Me.cbIsDifferent.Checked = True
        Me.cbIsAdd.Checked = False
        Me.cbIsTaxi.Checked = False
        Me.cbIsReserved.Checked = True
        Me.cbIsJustInTime.Checked = True

        Me.txtNotes.Text = ""
        Me.btnInsert.Visible = True
        Me.btnUpdate.Visible = False
        Me.MultiView1.ActiveViewIndex = 1
    End Sub

    Protected Sub btnUpdate_Click(sender As Object, e As System.EventArgs) Handles btnUpdate.Click
        'If cbIsDifferent.Checked And cbIsAdd.Checked Then
        '    ScriptManager.RegisterClientScriptBlock(Me.Page, GetType(Page), "", "alert('顯示差別費率並加入計算/顯示加價費率並加入計算 僅能選擇一項!!');", True)
        '    Exit Sub
        'End If
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
            'If cbIsDifferent.Checked And cbIsAdd.Checked Then
            '    ScriptManager.RegisterClientScriptBlock(Me.Page, GetType(Page), "", "alert('顯示差別費率並加入計算/顯示加價費率並加入計算 僅能選擇一項!!');", True)
            '    Exit Sub
            'End If
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
    Private Sub BindProject()
        Me.ddlFareList.Items.Clear()
        Me.ddlFareList.Items.Add(New ListItem("無", "0"))
        Me.ddlFareListProject.Items.Clear()
        'Me.ddlFareListProject.Items.Add(New ListItem("無", "0"))
    End Sub
End Class
