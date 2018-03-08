Imports System.Data
Imports System.Data.SqlClient

Partial Class SystemDesign_Sites
    Inherits PageBase

    Private _SiteRootNodeID As Integer

    Protected Sub SqlDS_SitesAdd_Inserted(ByVal sender As Object, ByVal e As SqlDataSourceStatusEventArgs) Handles SqlDS_SitesAdd.Inserted
        If e.AffectedRows > 0 Then
            Dim newSiteName As String = e.Command.Parameters("@SiteName").Value

            ''插入一筆資料到SiteMape裡()
            Dim SiteID As Integer
            Using dr As SqlDataReader = ClassDB.GetDataReader("SELECT TOP 1 SiteID FROM Sites ORDER BY SiteID DESC")
                Try
                    If dr.Read Then
                        SiteID = CInt(dr("SiteID"))
                    End If
                Catch ex As Exception
                    WriteErrLog(ex, Me.Page)
                Finally

                End Try
            End Using
            'ClassDB.UpdateDB("INSERT INTO SiteMap (NodeID, Text, NodeOrder, NodeLevel, SiteID, ImageUrl) VALUES ('" & _SiteRootNodeID & "', '" & newSiteName & "', '0', '0','" & SiteID & "','root.gif')")
            Dim NodeOrderMax As Integer = 2
            Using dr As SqlDataReader = ClassDB.GetDataReader("select max(NodeOrder) as MaxOrder from SiteMap where ParentNodeId is null")
                If dr.Read Then
                    NodeOrderMax = dr("MaxOrder") + 2
                End If
            End Using
            Dim AllNodeOrderMax As Integer = 2
            Using dr As SqlDataReader = ClassDB.GetDataReader("select max(AllNodeOrder) as MaxOrder from SiteMap ")
                If dr.Read Then
                    AllNodeOrderMax = dr("MaxOrder") + 2
                End If
            End Using
            ClassDB.UpdateDB("INSERT INTO SiteMap (NodeID, Text, NodeOrder, AdminModule, IsDefaultNavigateUrl, PublishType, Target, RefPath, AllNodeOrder, NodeLevel, HaveChildNode, ReviseSet, GroupID, SiteID, ImageUrl, ShowCheckBox, ModuleID, HomeList, DocCount, ViewCount, NavigateUrl, DeadlineDays) VALUES ('" & _SiteRootNodeID & "', '" & newSiteName & "', " & NodeOrderMax & ",1,1,1,'_parent','Publish.aspx', " & AllNodeOrderMax & ", '0', 1, 1,1, '" & SiteID & "','root.gif',0,32,0,0,0,'Publish.aspx',0)")
            '新增第一層        
            ClassDB.UpdateDB("INSERT INTO SiteMap (NodeID,ParentNodeId, Text, NodeOrder, AdminModule, IsDefaultNavigateUrl, PublishType, Target, RefPath, AllNodeOrder, NodeLevel, HaveChildNode, ReviseSet, GroupID, SiteID, ImageUrl, ShowCheckBox, ModuleID, HomeList, DocCount, ViewCount, NavigateUrl, DeadlineDays) VALUES ('" & _SiteRootNodeID + 1 & "','" & _SiteRootNodeID & "','" & newSiteName & "第一層" & "', 2,1,1,1,'_parent','Publish.aspx', " & AllNodeOrderMax + 2 & ", '1', 1, 1,1, '" & SiteID & "','folders.gif',0,32,0,0,0,'Publish.aspx',0)")
            '新增第二層        
            ClassDB.UpdateDB("INSERT INTO SiteMap (NodeID,ParentNodeId, Text, NodeOrder, AdminModule, IsDefaultNavigateUrl, PublishType, Target, RefPath, AllNodeOrder, NodeLevel, HaveChildNode, ReviseSet, GroupID, SiteID, ImageUrl, ShowCheckBox, ModuleID, HomeList, DocCount, ViewCount, NavigateUrl, DeadlineDays) VALUES ('" & _SiteRootNodeID + 2 & "','" & _SiteRootNodeID + 1 & "','" & newSiteName & "第二層" & "', 2,1,1,1,'_parent','Publish.aspx', " & AllNodeOrderMax + 4 & ", '2', 0, 1,1, '" & SiteID & "','journal.gif',1,32,0,0,0,'Publish.aspx',0)")

            Me.lblMessage.Text = "系統訊息：資料新增完成！"
        Else
            Me.lblMessage.Text = "系統訊息：資料新增失敗！"
        End If
    End Sub


    ''' <summary>
    ''' Delete Site will Del All SiteMap Node with this Site
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub GridView1_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.DataBound
        GridViewPageInfo.GetGridViewInfo(Me.GridView1, Me.Page, Me.SqlDS_Sites)
    End Sub
    ''' <summary>
    ''' Delete Site will Del All SiteMap Node with this Site
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub GridView1_RowDeleted(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeletedEventArgs) Handles GridView1.RowDeleted
        If e.AffectedRows > 0 Then
            Dim key As Integer = GridViewPageInfo.GetGridViewRowDeletedDataKey(e)

        End If
    End Sub
    ''' <summary>
    ''' SiteMap Node with this Site RowUpdated
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub GridView1_RowUpdated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdatedEventArgs) Handles GridView1.RowUpdated
        Dim NewValuesCount As Integer = e.NewValues.Count
        Dim i As Integer

        Dim NewValues As String = ""
        For i = 0 To NewValuesCount - 1
            NewValues += e.NewValues.Item(i) & ","
        Next

        Dim Oldvalues As String = ""
        For i = 0 To NewValuesCount - 1
            Oldvalues += e.OldValues.Item(i) & ","
        Next

        WriteAdminLog(Me.Page, "更新單位資料-舊資料『" & Oldvalues & "』 新資料『" & NewValues & "』")

        Me.lblMessage.Text = " 系統訊息：資料已更新！"
    End Sub



    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        '取得SiteMap的最大NodeID
        Dim SiteRootNodeID As Integer = 0


        Using dr As SqlDataReader = ClassDB.GetDataReader("SELECT TOP 1 NodeID FROM SiteMap ORDER BY NodeID DESC")
            Try
                If dr.Read Then
                    If CInt(dr("NodeID")) > 0 Then
                        SiteRootNodeID = CInt(dr("NodeID")) + 10
                    Else
                        SiteRootNodeID = 1
                    End If
                Else
                    SiteRootNodeID = 1
                End If
            Catch ex As Exception
                WriteErrLog(ex, Me.Page)
            Finally

            End Try
        End Using
        '記錄NodeID
        _SiteRootNodeID = SiteRootNodeID

        Me.SqlDS_SitesAdd.InsertParameters.Add("SiteRootNodeID", SiteRootNodeID)
        SqlDS_SitesAdd.Insert()
    End Sub
    Protected Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        SiteNameTextBox.Text = ""
        SiteShortNameTxt.Text = ""
        DescriptionTextBox.Text = ""
    End Sub
End Class
