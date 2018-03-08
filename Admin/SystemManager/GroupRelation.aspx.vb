Imports System.Data
Imports System.Data.SqlClient

Partial Class SystemManager_GroupRelation
    Inherits PageBase


    ''' <summary>
    ''' 頁碼資料
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub GridView1_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.DataBound
        GridViewPageInfo.GetGridViewInfo(GridView1, Me.Page, Me.SDS_Accounts_Group_List)
    End Sub


    Protected Sub GridView1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView1.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then

            If e.Row.RowState = 0 OrElse e.Row.RowState = 1 OrElse e.Row.RowState = 2 OrElse e.Row.RowState = 3 Then
                Dim drowView As System.Data.DataRowView = CType(e.Row.DataItem, System.Data.DataRowView)
                Dim UID As String = drowView.Item("GroupID").ToString

                Dim gvRow As GridViewRow = CType(e.Row, GridViewRow)

                '檢視設定

                For i As Integer = 0 To gvRow.Cells(3).Controls.Count - 1
                    Dim a As Control = gvRow.Cells(3).Controls(i)
                    If a.GetType.Name.ToString.Equals("DataControlButton") Then
                        Dim b As Button = CType(a, Button)
                        b.CommandArgument = UID & "," & e.Row.RowIndex.ToString
                    End If
                Next

                '提供已設定資訊
                Dim retVal As Integer
                Dim sql As String = "SELECT COUNT(CreateGroupID) AS MyOwneGroup FROM Accounts_GroupsRelation WHERE(OwnerGroupID = @UID)" '" & UID & ")"
                Using dr As SqlDataReader = ClassDB.GetDataReaderParam(sql, New SqlParameter("@UID", UID))
                    Try
                        If dr IsNot Nothing Then
                            If dr.Read Then
                                retVal = dr("MyOwneGroup").ToString
                            End If
                        End If
                    Catch ex As Exception
                        WriteErrLog(ex, Me.Page)
                    Finally

                    End Try
                End Using
                Dim lbInfo As Label = CType(e.Row.FindControl("lbInfo"), Label)
                If retVal = 0 Then
                    lbInfo.Text = "X"
                Else
                    lbInfo.Text = "已有 " & retVal & " 下屬群組"
                End If


            End If
        End If
    End Sub

    ''' <summary>
    ''' GridView RowCommand
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand

        If e.CommandName = "ViewItem" Then

            Dim c() As String = Split(e.CommandArgument, ",")
            Session("EditKey") = c(0)
            Me.GridView1.SelectedIndex = c(1)
        End If

        Me.cblMyOwnDep.DataBind()
    End Sub

    ''' <summary>
    ''' 勾選已設定的下屬單位
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub cblMyOwnDep_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cblMyOwnDep.DataBound

        If Not Session("EditKey") Is Nothing Then


            For i As Integer = 0 To Me.cblMyOwnDep.Items.Count - 1
                Dim lItem As ListItem = Me.cblMyOwnDep.Items(i)
                If lItem.Value = Session("EditKey").ToString Then
                    lItem.Enabled = False
                End If
            Next

            '取得本單位管理的下屬單位
            Dim sql As String = "SELECT CreateGroupID FROM Accounts_GroupsRelation WHERE (OwnerGroupID = " & Session("EditKey").ToString & ")"

            Dim OldMPSubscribe As String = ""
            Using dr As SqlDataReader = ClassDB.GetDataReader(sql)
                Try
                    While dr.Read

                        For i As Integer = 0 To Me.cblMyOwnDep.Items.Count - 1
                            Dim l As ListItem = Me.cblMyOwnDep.Items(i)
                            If l.Value = dr("CreateGroupID").ToString Then
                                l.Selected = True
                            End If
                        Next
                    End While
                Catch ex As Exception
                    WriteErrLog(ex, Me.Page)
                Finally

                End Try
            End Using
        End If
    End Sub


    ''' <summary>
    ''' 更新-單位的下屬單位設定
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btbSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btbSubmit.Click
        Dim GroupID As Integer = Session("EditKey")
        'Del old setting
        Dim sql As String = "DELETE FROM Accounts_GroupsRelation WHERE (OwnerGroupID = @GroupID)"
        'ClassDB.UpdateDB("Net2_Accounts_Group_MyOwnDep_Del", New SqlParameter("@OwnerDepID", Session("EditKey")))
        ClassDB.UpdateDBText(sql, New SqlParameter("@GroupID", GroupID))

        'Add new setting

        For i As Integer = 0 To Me.cblMyOwnDep.Items.Count - 1
            Dim l As ListItem = Me.cblMyOwnDep.Items(i)
            If l.Selected = True Then
                sql = "INSERT INTO Accounts_GroupsRelation (OwnerGroupID, CreateGroupID, UpdateDateTime, UpdateUser)"
                sql += " VALUES(@OwnerGroupID,@CreateGroupID, GETDATE(),@UserID)"
                ClassDB.UpdateDB(sql, New SqlParameter("@OwnerGroupID", CInt(Session("EditKey"))), New SqlParameter("@CreateGroupID", CInt(l.Value)), New SqlParameter("@UpdateUser", CInt(Session("UserID"))))
            End If
        Next

        ModuleWriteLog.WriteAdminLog(Me.Page, "更新-單位所屬設定")

        Me.lbMessage.Text = "系統訊息：設定完成！"
        Me.GridView1.DataBind()
    End Sub

    ''' <summary>
    ''' GV換頁動作
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub GridView1_PageIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.PageIndexChanged
        Me.GridView1.SelectedIndex = -1
        Session("EditKey") = Nothing
        Me.cblMyOwnDep.DataBind()
    End Sub
End Class

