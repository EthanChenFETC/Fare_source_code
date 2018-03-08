Imports System.Data
Imports System.Data.SqlClient

Partial Class SystemDesign_SiteMapGroupSetting
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Dim dd = "aa"
    End Sub


    ''' <summary>
    ''' 取得樹狀選單，有複雜的過濾條件
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub BuildSiteTree()
        '取得樹狀資料
        Dim SiteID As Integer = Me.GridView1.DataKeys(Me.GridView1.SelectedIndex).Value.ToString
        Using ds As DataSet = ClassDB.RunReturnDataSet("SELECT * FROM SITEMAP WHERE SiteID in(@SiteID)", "SiteTreeDS", New SqlParameter("@SiteID", SiteID))
            Try
                Me.SiteTree1.BuildTree(ds)
            Catch ex As Exception
                WriteErrLog(ex, Me.Page)
            End Try
        End Using
    End Sub
    Protected Sub GridView1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView1.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim rv As DataRowView = CType(e.Row.DataItem, DataRowView)
            If e.Row.RowState = 0 OrElse e.Row.RowState = 1 OrElse e.Row.RowState = 2 OrElse e.Row.RowState = 3 Then
                Dim ddl As DropDownList = CType(e.Row.FindControl("ddlGroup_List"), DropDownList)
                Dim sql As String = "SELECT GroupID, GroupName, SiteMapGroupID FROM SiteMapGroupCatgry where SiteID = @SiteID and IsOnline=1"
                ddl.Items.Clear()
                Using dr As SqlDataReader = ClassDB.GetDataReaderParam(sql, New SqlParameter("@SiteID", rv.Item("SiteID").ToString))
                    Try
                        If dr IsNot Nothing Then
                            While dr.Read
                                Dim GroupName As String = dr("GroupName")

                                Dim GroupID As Integer = dr("GroupID")
                                If Not IsNumeric(GroupID) Then
                                    GroupID = "0"
                                End If
                                ddl.Items.Add(New ListItem(GroupName, GroupID))
                                If Me.GroupIDSelected.SelectedValue = GroupID Then
                                    ddl.Items.Item(ddl.Items.Count - 1).Selected = "true"
                                Else
                                    ddl.Items.Item(ddl.Items.Count - 1).Selected = "false"
                                End If
                            End While
                        End If
                    Catch ex As Exception
                        WriteErrLog(ex, Me.Page)
                    End Try
                End Using

            End If
        End If
    End Sub


    Protected Sub GridView1_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.DataBound
        GridViewPageInfo.GetGridViewInfo(Me.GridView1, Me.Page)
    End Sub


    Protected Sub GridView1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.SelectedIndexChanged
        Dim s As String = ""
        Dim SiteID As Integer = Me.GridView1.DataKeys(Me.GridView1.SelectedIndex).Value.ToString
        Dim gv As GridView = sender
        Dim ddl As DropDownList = CType(gv.SelectedRow.FindControl("ddlGroup_List"), DropDownList)
        If ddl.SelectedValue IsNot Nothing And IsNumeric(ddl.SelectedValue) Then
            Dim GroupID As Integer = CInt(ddl.SelectedValue)
            Me.GroupIDSelected.Items(0).Value = GroupID.ToString
            Dim sql As String = "SELECT NodeID, GroupID, SiteID FROM SiteMap_Group WHERE (SiteID = @SiteID) AND (GroupID = @GroupID )"
            Using dr As SqlDataReader = ClassDB.GetDataReaderParam(sql, New SqlParameter("@SiteID", SiteID), New SqlParameter("@GroupID", GroupID))
                Try
                    If dr IsNot Nothing Then
                        While dr.Read
                            s += dr("NodeID").ToString & ","
                        End While
                    End If
                Catch ex As Exception
                    WriteErrLog(ex, Me)
                Finally
                End Try
            End Using
        Else
            Me.GroupIDSelected.SelectedIndex = 0
        End If
        If s.Length > 0 Then s = s.Substring(0, s.Length - 1)
        Me.SiteTree1.SetNodeIDtoCheck = s
        BuildSiteTree()
    End Sub

    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        Dim GroupID As Integer = 0
        If IsNumeric(Me.GroupIDSelected.Text) Then
            GroupID = CInt(Me.GroupIDSelected.Text)
            Dim SiteID As Integer = CInt(Me.GridView1.DataKeys(Me.GridView1.SelectedIndex).Value.ToString)
            Dim Nodes As String = Me.SiteTree1.GetCheckedNode
            Try
                Me.sds_SiteMap_Group_Insert.Delete()
            Catch ex As Exception
                WriteErrLog(ex, Me.Page)
                Me.lbMessage.Text = "系統訊息：設定更新錯誤，請重新選取站台重新操作一次！"
                Exit Sub
            End Try
            If Nodes.Trim.Length = 0 Then
                BuildSiteTree()
            Else
                Dim Node() As String = Split(Nodes, ",")
                Dim i As Integer = 0
                Try
                    For i = 0 To UBound(Node)
                        Me.sds_SiteMap_Group_Insert.InsertParameters("NodeID").DefaultValue = CInt(Node(i))
                        Me.sds_SiteMap_Group_Insert.InsertParameters("GroupID").DefaultValue = GroupID
                        Me.sds_SiteMap_Group_Insert.InsertParameters("SiteID").DefaultValue = SiteID
                        Me.sds_SiteMap_Group_Insert.Insert()
                        'Dim uSql As String = "INSERT INTO SiteMap_Group SET (NodeID, GroupID, SiteID) VALUES (" & Node(i) & "," & Me.RadioButtonList1.SelectedValue & "," & SiteID & ") "
                        'ClassDB.UpdateDB(uSql)
                    Next
                Catch ex As Exception
                    WriteErrLog(ex, Me.Page)
                    'Exit Sub
                End Try
            End If
            Me.GridView1.DataBind()
            Me.lbMessage.Text = "系統訊息：設定更新完成！"
        Else
            Me.lbMessage.Text = "系統訊息：選單群組錯誤！"
        End If
    End Sub
End Class
