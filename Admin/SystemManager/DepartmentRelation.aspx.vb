Imports System.Data
Imports System.Data.SqlClient

Partial Class SystemManager_DepartmentRelation
    Inherits PageBase
    ''' <summary>
    ''' 頁碼資料
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub GridView1_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.DataBound
        GridViewPageInfo.GetGridViewInfo(GridView1, Me.Page, Me.SDS_Department_List)
    End Sub


    Protected Sub GridView1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView1.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then

            If e.Row.RowState = 0 OrElse e.Row.RowState = 1 OrElse e.Row.RowState = 2 OrElse e.Row.RowState = 3 Then
                Dim drowView As System.Data.DataRowView = CType(e.Row.DataItem, System.Data.DataRowView)
                Dim UID As String = drowView.Item("DepartmentID").ToString

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


                Dim retVat As Integer = ClassDB.RunSPReturnInteger("Net2_Department_GetMyOwnDep_Count", New SqlParameter("@DepartmentID", UID))

                If retVat = 0 Then
                    gvRow.Cells(2).Text = "X"
                Else
                    gvRow.Cells(2).Text = "已有 " & retVat & " 下屬單位"
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


            '取得本單位管理的下屬單位
            Using dr As SqlDataReader = ClassDB.GetDataReaderSP("Net2_Department_GetMyOwnDep", New SqlParameter("@DepartmentID", CInt(Session("EditKey")).ToString))
                Dim OldMPSubscribe As String = ""
                Try
                    If dr IsNot Nothing Then
                        While dr.Read

                            For i As Integer = 0 To Me.cblMyOwnDep.Items.Count - 1
                                Dim l As ListItem = Me.cblMyOwnDep.Items(i)
                                If l.Value = dr("CreateDepID").ToString Then
                                    l.Selected = True

                                End If
                            Next
                        End While
                    End If
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

        'Del old setting
        ClassDB.UpdateDB("Net2_Department_MyOwnDep_Del", New SqlParameter("@OwnerDepID", CInt(Session("EditKey"))))

        'Add new setting

        For i As Integer = 0 To Me.cblMyOwnDep.Items.Count - 1
            Dim l As ListItem = Me.cblMyOwnDep.Items(i)
            If l.Selected = True Then
                ClassDB.UpdateDB("Net2_Department_MyOwnDep_Update",
                New SqlParameter("@OwnerDepID", CInt(Session("EditKey"))),
                New SqlParameter("@CreateDepID", CInt(l.Value)))
            End If
        Next

        ModuleWriteLog.WriteAdminLog(Me.Page, "更新-單位所屬設定")

        Me.lbMessage.Text = "系統訊息：設定完成！"
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
