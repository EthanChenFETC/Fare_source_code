Imports System.Data
Imports System.Data.SqlClient

Partial Class Fare_Calculator_InterChangeProhibitRoute
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
        End If
    End Sub

    Protected Sub GridView1_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.DataBound
        GridViewPageInfo.GetGridViewInfo(GridView1, Me.Page, Me.sdsInterChangeRouteList)
    End Sub
    Protected Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand
        If e.CommandName = "EditProject" Then
            Me.MultiView1.ActiveViewIndex = 1
            Me.lblRouteList.Text = ""
            Dim UID As Integer = e.CommandArgument
            Session("EditKey") = e.CommandArgument
            Using dr As SqlDataReader = ClassDB.GetDataReaderParam("Select HWName, ICName, ICUID from InterChangeMapGroupRelation r inner join InterChangeMapGroup m on m.UID = r.GroupUID inner join InterChangeList l on l.UID = r.ICUID inner join HWList h on h.UID = l.HWUID where r.GroupUID = @UID", New SqlParameter("@UID", UID))
                Try
                    If dr IsNot Nothing Then
                        While dr.Read
                            Me.lblRouteList.Text &= "," & "(" & (dr("HWName")) & ")" & (dr("ICName"))
                            Me.hidRouteList.Value &= "," & CInt(dr("ICUID")).ToString
                        End While
                        If Me.lblRouteList.Text.Length > 0 Then
                            Me.lblRouteList.Text = Me.lblRouteList.Text.Substring(1)
                            Me.hidRouteList.Value = Me.hidRouteList.Value.Substring(1)
                        End If
                    End If
                Catch ex As Exception
                Finally

                End Try
            End Using
            Me.dlICList.DataBind()
            Me.btnUpdate.Visible = True
        End If
    End Sub
    Protected Sub Repeater_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs)
        If e.CommandName.Equals("ICSelect") Then
            Dim arg() As String = e.CommandArgument.ToString.Split(",")
            If (Me.hidRouteList.Value & ",").IndexOf("," & arg(0) & ",") >= 0 Then
                ScriptManager.RegisterClientScriptBlock(Me.UpdatePanel1, GetType(UpdatePanel), "warning", "alert('交流道已存在');", True)
                Exit Sub
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
            lblHWName.Text = e.Item.DataItem("HWName") 'Me.dlICList.DataKeys(e.Item.ItemIndex)
            Dim sdsICListdl As SqlDataSource = CType(e.Item.FindControl("sdsICListdl"), SqlDataSource)
            sdsICListdl.SelectParameters("HWUID").DefaultValue = Me.dlICList.DataKeys(e.Item.ItemIndex)
            Dim rptRouteList As Repeater = CType(e.Item.FindControl("rptRouteList"), Repeater)
            rptRouteList.DataSourceID = sdsICListdl.ID
            sdsICListdl.SelectParameters("HWUID").DefaultValue = Me.dlICList.DataKeys(e.Item.ItemIndex)
            rptRouteList.DataBind()

        End If
    End Sub
    Protected Sub Repeater_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        If e.Item.ItemType = 2 Or e.Item.ItemType = 3 Or e.Item.ItemType = 4 Or e.Item.ItemType = 5 Then
            Dim btnIC As Button = CType(e.Item.FindControl("btnIC"), Button)
            Dim drow As DataRowView = e.Item.DataItem
            btnIC.CommandArgument = RemoveXSS(drow("UID").ToString & "," & drow("ICName").ToString & "," & drow("HWName").ToString & "," & drow("ICName").ToString)
            btnIC.Text = RemoveXSS("(" & drow("HWName") & ")" & drow("ICName"))
        End If
    End Sub
    Protected Sub GridView1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView1.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            If e.Row.RowState = DataControlRowState.Alternate Or e.Row.RowState = DataControlRowState.Edit Or e.Row.RowState = DataControlRowState.Normal Or e.Row.RowState = DataControlRowState.Selected Then
                Dim RowIndex As Integer = e.Row.RowIndex
                Dim DataKeys As String = Me.GridView1.DataKeys(RowIndex).Value().ToString
                Dim BtnEdit As Button = CType(e.Row.FindControl("BtnEdit"), Button)
                BtnEdit.CommandArgument = DataKeys
                Dim drow As DataRowView = CType(e.Row.DataItem, DataRowView)
                Dim ltlICName As Literal = CType(e.Row.FindControl("ltlICName"), Literal)
                ltlICName.Text = ""
                Using dr As SqlDataReader = ClassDB.GetDataReaderParam("select HWName, ICName from InterChangeList i inner join HWList h on h.UID = i.HWUID where i.UID in (Select ICUID from InterchangeMapGroupRelation where GroupUID = @GroupUID)", New SqlParameter("@GroupUID", DataKeys.ToString))
                    Try
                        If dr IsNot Nothing Then
                            While dr.Read
                                ltlICName.Text &= "，" & "(" & (dr("HWName").ToString) & ")" & (dr("ICName").ToString)
                            End While
                        End If
                    Catch ex As Exception
                    Finally

                    End Try
                End Using
            End If
        End If
    End Sub



    Protected Sub btnUpdate_Click(sender As Object, e As System.EventArgs) Handles btnUpdate.Click
        Try
            If Me.hidRouteList.Value.Length = 0 Then
                ScriptManager.RegisterClientScriptBlock(Me.UpdatePanel1, GetType(UpdatePanel), "warning", "alert('沒有選擇交流道');", True)
                Exit Sub
            End If
            Dim Routes() As String = Me.hidRouteList.Value.Trim.Split(",")
            Dim RouteName As String = Me.lblRouteList.Text
            'If Me.hidRouteList.Value.Replace(",", "").Trim.Length = 0 Then
            '    ScriptManager.RegisterClientScriptBlock(Me.UpdatePanel1, GetType(UpdatePanel), "warning", "alert('沒有選擇交流道');", True)
            '    Exit Sub
            'End If
            Dim GroupUID As String = Session("EditKey").ToString
            If Not IsNumeric(GroupUID) Then
                GroupUID = "0"
            End If
            ClassDB.UpdateDBText("delete from InterchangeMapGroupRelation where GroupUID = @GroupUID", New SqlParameter("@GroupUID", GroupUID))
            For i As Integer = 0 To Routes.Length - 1
                Dim Route As String = Routes(i)
                If Not IsNumeric(Route) Then
                    Route = "0"
                End If
                If IsNumeric(Route) Then
                    ClassDB.UpdateDBText("Insert into InterchangeMapGroupRelation(GroupUID, ICUID) values(@GroupUID, @ICUID)", New SqlParameter("@GroupUID", GroupUID), New SqlParameter("@ICUID", Route))
                End If
            Next
        Catch ex As Exception
            Dim eee = ex.Message
        End Try
        '自成一個群組
        Me.GridView1.DataBind()
        Me.MultiView1.ActiveViewIndex = 0
    End Sub

    Protected Sub btnCancel_Click(sender As Object, e As System.EventArgs) Handles btnCancel.Click
        Me.MultiView1.ActiveViewIndex = 0
    End Sub

End Class
