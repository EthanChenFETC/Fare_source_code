Imports System.Data
Imports System.Data.SqlClient

Partial Class Publish_PublishList
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Session("Administrator") Is Nothing And Session("SuperAdmin") Is Nothing Then
                Me.ddlDepartment.DataBind()
                Me.ddlDepartment.SelectedValue = Session("DepartmentID")
                Me.ddlUsers.Items.Clear()
                Me.ddlUsers.Items.Add(New ListItem("全部", "0"))
                Me.ddlUsers.DataSourceID = "sdsUsers"
                Me.ddlUsers.DataBind()
                Me.ddlUsers.DataSourceID = ""
                Me.ddlUsers.SelectedValue = Session("UserID")
                Me.ddlDepartment.Enabled = False
                Me.ddlUsers.Enabled = False
            End If
        End If
        If Session("Administrator") = True Or Session("SuperAdmin") = True Then
            Me.SqlDS_Net2_Publication_Admin_GetList.SelectParameters("Admin").DefaultValue = 1
            Me.SqlDS_Net2_Publication_Admin_GetList.SelectParameters("SuperAdmin").DefaultValue = 1
        Else
            Me.SqlDS_Net2_Publication_Admin_GetList.SelectParameters("Admin").DefaultValue = 0
            Me.SqlDS_Net2_Publication_Admin_GetList.SelectParameters("SuperAdmin").DefaultValue = 0
        End If

        'BuildSiteTree()

        Session("PublishMode") = Nothing
        Session("publishkey") = Nothing

        '網站管理員身份-文章清單不篩選個人上稿,重新指派SqlDataSource
        'If Session("Administrator") = True Or Session("SuperAdmin") = True Then
        '    Me.GridView1.DataSourceID = "Net2_Publication_Admin_GetList_Adminstrator"
        'End If
    End Sub

#Region "上方功能"
    Protected Sub ddlSites_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlSites.SelectedIndexChanged
        BuildSiteTree()
    End Sub


    Protected Sub ddlDepartment_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlDepartment.SelectedIndexChanged
        Me.ddlUsers.Items.Clear()
        Me.ddlUsers.Items.Add(New ListItem("全部", "0"))
        Me.ddlUsers.DataSourceID = "sdsUsers"
        Me.ddlUsers.DataBind()
        Me.ddlUsers.DataSourceID = ""
    End Sub

    Protected Sub btnShowAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnShowAll.Click
        Me.txtSearch.Text = ""
        Me.txtEdate.Text = "3000/01/01"
        Me.txtSdate.Text = Date.Now.Year.ToString & "/01/01"
        Me.ddlSites.SelectedIndex = 0
        Me.ddlSiteMap.SelectedIndex = 0
        Me.ddlDepartment.SelectedIndex = 0
        Me.ddlUsers.SelectedIndex = 0
        If Session("Administrator") = True Or Session("SuperAdmin") = True Then
            Me.SqlDS_Net2_Publication_Admin_GetList.SelectParameters("Admin").DefaultValue = 1
            Me.SqlDS_Net2_Publication_Admin_GetList.SelectParameters("SuperAdmin").DefaultValue = 1
        Else
            Me.SqlDS_Net2_Publication_Admin_GetList.SelectParameters("Admin").DefaultValue = 0
            Me.SqlDS_Net2_Publication_Admin_GetList.SelectParameters("SuperAdmin").DefaultValue = 0
        End If
        Me.GridView1.DataBind()
    End Sub

    ''' <summary>
    ''' 取得樹狀選單，有複雜的過濾條件
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub BuildSiteTree()

        '取得GroupID(System can let User hvae more than 2 Group
        Dim MyGroupID As String = Session("GroupID")
        If MyGroupID.IndexOf("_") >= 0 Then
            MyGroupID = MyGroupID.Replace("_", ",")
        End If

        '先取得擁有的SiteID
        Dim SiteID As String = ""
        'If ddlSites.SelectedIndex > 0 Then
        '    SiteID = " and SiteID = " & CInt(ddlSites.SelectedValue).ToString
        'End If
        'SiteID = ""
        Dim Str As String = "SELECT DISTINCT SiteID FROM  Accounts_GroupSitePermission WHERE  (ISNULL((SELECT GroupID FROM Accounts_Group WHERE  GroupID in (@MyGroupID) and GroupName = 'Administrator'), '') <> '' or GroupID IN (@MyGroupID))  and (@SiteID <= 0 or @SiteID = SiteID)"
        Using dr As SqlDataReader = ClassDB.GetDataReaderParam(Str.Replace("@MyGroupID", MyGroupID).Replace("@MyGroupID", MyGroupID), New SqlParameter("@SiteID", CInt(ddlSites.SelectedValue)))
            Try
                If dr IsNot Nothing Then
                    While dr.Read
                        SiteID += CInt(dr("SiteID")).ToString & ","
                    End While
                End If
            Catch ex As Exception
                WriteErrLog(ex, Me.Page)
            End Try
        End Using
        If SiteID.Length > 0 Then SiteID = SiteID.Substring(0, SiteID.Length - 1)

        '取得NodeID
        Dim NodeIDs As String = ""
        Using dr As SqlDataReader = ClassDB.GetDataReaderParam("SELECT DISTINCT NodeID FROM  Accounts_Permissions WHERE  (GroupID IN (@MyGroupID)) AND (Category = 3)".Replace("@MyGroupID", MyGroupID), New SqlParameter("@MyGroupID", MyGroupID))
            Try
                If dr IsNot Nothing Then
                    While dr.Read
                        NodeIDs += CInt(dr("NodeID")).ToString & ","
                    End While
                End If
            Catch ex As Exception
                WriteErrLog(ex, Me.Page)
            Finally

            End Try
        End Using
        If NodeIDs.Length > 0 Then NodeIDs = NodeIDs.Substring(0, NodeIDs.Length - 1)
        If Not Regex.IsMatch(SiteID.Replace(",", ""), "^[0-9]+$") Then
            Exit Sub
        End If
        If Not Regex.IsMatch(NodeIDs.Replace(",", ""), "^[0-9]+$") Then
            Exit Sub
        End If

        '取得樹狀資料
        Dim dt As DataTable = New DataTable
        If Session("Administrator") = True Then
            dt = ClassDB.RunReturnDataTable("SELECT * FROM SITEMAP WHERE SiteID in(@SiteID) AND NodeId IN (@NodeIDs) AND (PublishType = 1 ) ORDER BY AllNodeOrder".Replace("@NodeIDs", NodeIDs).Replace("@SiteID", SiteID), New SqlParameter("@SiteID", SiteID), New SqlParameter("@NodeIDs", NodeIDs))
        Else
            dt = ClassDB.RunReturnDataTable("SELECT * FROM SITEMAP WHERE SiteID in(@SiteID) AND NodeId IN (@NodeIDs) AND (PublishType = 1 ) ORDER BY AllNodeOrder".Replace("@NodeIDs", NodeIDs).Replace("@SiteID", SiteID), New SqlParameter("@SiteID", SiteID), New SqlParameter("@NodeIDs", NodeIDs))
        End If
        Dim tid As Integer = Me.ddlSiteMap.SelectedValue
        Me.ddlSiteMap.Items.Clear()
        Me.ddlSiteMap.Items.Add(New ListItem("全部單元", "0"))
        For i As Integer = 0 To dt.Rows.Count - 1
            Dim drow As DataRow = dt.Rows(i)
            Dim Text As String = ReturnSpace(CInt(drow("NodeLevel"))) & drow("Text").ToString

            Dim NodeID As String = drow("NodeID")
            If Not IsNumeric(NodeID) Then
                NodeID = "0"
            End If

            Me.ddlSiteMap.Items.Add(New ListItem(Text, NodeID))
        Next
        Me.ddlSiteMap.SelectedValue = tid
    End Sub

    Private Function ReturnSpace(ByVal i As Integer) As String
        Dim j As Integer
        Dim SpaceString As String = ""
        For j = 0 To i
            SpaceString += "　"
        Next
        'SpaceString += "-"
        Return SpaceString
    End Function

#End Region


#Region "GridView1"

    ''' <summary>
    ''' 清單總量、頁數資料
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub GridView1_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.DataBound
        '網站管理員身份,Change the Default SDS
        'If Session("Administrator") = True Or Session("SuperAdmin") = True Then
        '    'If False = True Then
        '    GridViewPageInfo.GetGridViewInfo(Me.GridView1, Me.Page, Me.Net2_Publication_Admin_GetList_Adminstrator)
        'Else
        '    GridViewPageInfo.GetGridViewInfo(Me.GridView1, Me.Page, Me.SqlDS_Net2_Publication_Admin_GetList)
        'End If
        GridViewPageInfo.GetGridViewInfo(Me.GridView1, Me.Page, Me.SqlDS_Net2_Publication_Admin_GetList)
        Using dt As DataTable = IIf(Session("Administrator") = True Or Session("SuperAdmin") = True, CType(Me.SqlDS_Net2_Publication_Admin_GetList.Select(New DataSourceSelectArguments), DataView).Table, CType(Me.SqlDS_Net2_Publication_Admin_GetList.Select(New DataSourceSelectArguments), DataView).Table)
            If Not dt Is Nothing Then
                Dim Title() As String = {"網站", "單元名稱", "文章標題", "上稿日期", "上架日期", "下架日期", "最後更新日期", "單位", "資料最後更新人員"}
                Using dtt As DataTable = dt.Clone
                    For i As Integer = 0 To dtt.Columns.Count - 1
                        Dim colname As String = dtt.Columns.Item(i).ColumnName
                        If i < Title.Length Then
                            dt.Columns.Item(i).ColumnName = Title(i)
                        Else
                            dt.Columns.Remove(colname)
                        End If
                    Next
                End Using
                Exportbutton1.Dataview = dt.DefaultView
                Exportbutton1.FileNameToExport = "Publish-Report-" & Date.Now.ToString("yyyyMMddhhmmss") & ".xls"
                Exportbutton1.ExportType = PNayak.Web.UI.WebControls.ExportButton.ExportTypeEnum.Excel
                Exportbutton1.Separator = PNayak.Web.UI.WebControls.ExportButton.SeparatorTypeEnum.TAB
                Exportbutton1.CharSet = "utf-8"
                Exportbutton1.Delimiter = """"
            End If
        End Using
    End Sub


    ''' <summary>
    ''' 清單命令
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand
        '往上排序
        If e.CommandName = "EditPublish" Then
            'Dim index As Integer = CInt(e.CommandArgument)
            'Dim UID As Integer = CInt(GridView1.DataKeys(index).Value)  '取得主鍵，轉到編輯頁面
            Dim UID As Integer = CInt(e.CommandArgument)

            Session("PublishMode") = "Edit"
            Session("publishkey") = UID

            InterfaceBuilder.TabGoNext(Me.Page)
        End If
        If e.CommandName = "BtnMeta" Then
            Dim index As Integer = CInt(e.CommandArgument)
            'Dim UID As Integer = CInt(GridView1.DataKeys(index).Value)  '取得主鍵，轉到編輯頁面

            Session("PublishMode") = "BtnMeta"
            Session("publishkey") = index

            InterfaceBuilder.TabGoNext(Me.Page)
        End If

        If e.CommandName = "BtnPublish" Then
            Dim index As Integer = CInt(e.CommandArgument)
            'Dim UID As Integer = CInt(GridView1.DataKeys(index).Value)  '取得主鍵，轉到編輯頁面

            Session("PublishMode") = "BtnPublish"
            Session("publishkey") = index
            Session("publishlistkey") = index

            InterfaceBuilder.TabGoNext(Me.Page)
        End If

        If e.CommandName = "DocDelete" Then
            Me.ViewState.Add("DelPublicID", e.CommandArgument.ToString)

            'Me.lblMessage.Text = e.CommandArgument.ToString

            'WriteLog("e.CommandArgument.ToString=", e.CommandArgument.ToString)

            'If Session("Administrator") = True Then
            '    Me.sds_Publish_Administrator.DeleteParameters("PublicID").DefaultValue = e.CommandArgument.ToString
            '    Me.sds_Publish_Administrator.Delete()
            'Else
            '    Me.sds_Publish.DeleteParameters("PublicID").DefaultValue = e.CommandArgument.ToString
            '    Me.sds_Publish.Delete()
            'End If

            Me.sds_Publish.DeleteParameters("PublicID").DefaultValue = e.CommandArgument.ToString
            Me.sds_Publish.Delete()
        End If

        If e.CommandName = "BtnLink" Then
            Dim URLs As String = e.CommandArgument.ToString
            If Not IsDBNull(URLs) Then
                If URLs.Length > 0 Then
                    Dim script As String = "window.open('" & URLs & "','','','');"
                    ScriptManager.RegisterClientScriptBlock(Me.Page, GetType(Page), "openwindow", script, True)
                End If
            End If
        End If
    End Sub

    Protected Sub GridView1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView1.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim drowView As System.Data.DataRowView = CType(e.Row.DataItem, System.Data.DataRowView)
            If e.Row.RowState = 0 OrElse e.Row.RowState = 1 OrElse e.Row.RowState = 2 OrElse e.Row.RowState = 3 Then
                '標題文字繫結
                Dim LinkButton1 As LinkButton = CType(e.Row.FindControl("LinkButton1"), LinkButton)
                Dim Subject As String = drowView.Item("Subject").ToString
                '超連結的標題
                If Subject.StartsWith("<a href") Then
                    Dim si As Integer
                    Dim ei As Integer
                    si = Subject.IndexOf(">")
                    ei = Subject.LastIndexOf("<")
                    'WriteLog("Publish", "Subject=" & Subject & " si=" & si & " ei=" & ei)
                    Subject = Subject.Substring(si + 1, ei - si - 1)

                End If
                LinkButton1.Text = ModuleMisc.LimitWord(Subject, 20)
                LinkButton1.ToolTip = Subject
                LinkButton1.CommandArgument = drowView("PublicID").ToString
                Dim ImageAttFile As Image = CType(e.Row.FindControl("ImageAttFile"), Image)
                If Not IsDBNull(drowView.Item("AttFiles")) Then
                    If drowView.Item("AttFiles").ToString.Length > 0 Then
                        ImageAttFile.Visible = True
                        Dim a() As String = Split(drowView.Item("AttFiles").ToString, ",")
                        ImageAttFile.ToolTip = "已上傳" & a.Length & "附檔!"
                    End If
                End If
                'Chris 20090708 新增超連結
                Dim ImageLinkFile As ImageButton = CType(e.Row.FindControl("ImageLinkFile"), ImageButton)
                If Not IsDBNull(drowView.Item("SubjectLink")) Then
                    If drowView.Item("SubjectLink").ToString.Length > 0 Then
                        ImageLinkFile.Visible = True
                        ImageLinkFile.ToolTip = RemoveXSS(drowView.Item("SubjectLink").ToString)
                        'ImageLinkFile.PostBackUrl = drowView.Item("SubjectLink").ToString

                    End If
                End If
                Dim rv As DataRowView = CType(e.Row.DataItem, DataRowView)
                If e.Row.RowState = 0 OrElse e.Row.RowState = 1 Then
                    Dim BtnPublish As Button = CType(e.Row.FindControl("BtnPublish"), Button)
                    BtnPublish.CommandArgument = drowView("PublicID")
                    Dim ButtonMeta As Button = CType(e.Row.FindControl("BtnMeta"), Button)
                    ButtonMeta.CommandArgument = drowView("PublicID")
                    Using dr As SqlDataReader = ClassDB.GetDataReaderParam("select MetaID from PublicationRelationMeta where PublicID=@PublicID", New SqlParameter("@PublicID", rv.Item("PublicID")))
                        If dr IsNot Nothing Then
                            If dr.Read = False Then
                                ButtonMeta.Text = "新增檢索"
                            Else
                                ButtonMeta.Text = "修改檢索"
                            End If
                        End If
                    End Using
                End If
                Dim HiddenField_AttFiles As HiddenField = CType(e.Row.FindControl("HiddenField_AttFiles"), HiddenField)
                HiddenField_AttFiles.Value = drowView("AttFiles")


            ElseIf e.Row.RowState = 4 OrElse e.Row.RowState = 5 Then
                Dim SubjectLink As TextBox = CType(e.Row.FindControl("SubjectLink"), TextBox)
                SubjectLink.Text = drowView("SubjectLink")
                Dim TextBox1 As TextBox = CType(e.Row.FindControl("TextBox1"), TextBox)
                TextBox1.Text = drowView("AttFiles")
            End If
        End If
    End Sub
    Protected Sub ImageLinkFile_OnClick(ByVal sender As Object, ByVal e As System.EventArgs)

    End Sub


    ''' <summary>
    ''' 文章刪除後續處理-統計資料更新
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub GridView1_RowDeleted(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeletedEventArgs) Handles GridView1.RowDeleted
        If e.AffectedRows > 0 Then

            Dim key As String = GridViewPageInfo.GetGridViewRowDeletedDataKey(e)

            '取得資料提供者/部門編號

            Dim ResponUser As Integer
            Dim ResponDepartment As Integer
            Dim NodeID As Integer
            Dim SiteID As Integer
            Using dr As SqlDataReader = ClassDB.GetDataReaderSP("Net2_Publish_GetDocInfo", New SqlParameter("@PublicID", CInt(key)))
                Try
                    If dr IsNot Nothing Then
                        If dr.Read Then
                            ResponUser = CInt(dr("ResponUser").ToString)
                            ResponDepartment = CInt(dr("ResponDepartment").ToString)
                            NodeID = CInt(dr("NodeId").ToString)
                            SiteID = CInt(dr("SiteID").ToString)
                        End If
                    End If
                Catch ex As Exception
                    WriteErrLog(ex, Me.Page)
                Finally

                End Try
            End Using

            '發行文章計數器減少(2008/1/18改成從Publication資料表中取得，所以不用再另外記錄)
            ModuleCounter.DocCount_Update(SiteID, NodeID, CInt(Session("UserID").ToString), CInt(Session("DepartmentID").ToString), ResponUser, ResponDepartment, -1)

            '異動追蹤
            Me.lblMessage.Text = GridViewPageInfo.Message_Delete_OK(Me.Page, "多向上稿文章，文章編號：" & key)
            ModuleAuditLog.WriteAuditLog(Me.Page, 3, "刪除-多向上稿文章，文章編號：" & key, "0", key, "")

        End If

    End Sub

    ''' <summary>
    ''' 文章刪除前
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub GridView1_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridView1.RowDeleting
        '取得附檔的ID字串
        Dim HiddenField_AttFiles As HiddenField = CType(Me.GridView1.Rows(e.RowIndex).FindControl("HiddenField_AttFiles"), HiddenField)
        Dim AttFiles As String = HiddenField_AttFiles.Value

        '如有資料呼叫刪除Function
        If AttFiles.Length > 0 Then
            ModuleMisc.AttachFilesDelete(AttFiles, Me.Page)
        End If

        Dim index As Integer = e.RowIndex
        Dim key As String = Me.GridView1.DataKeys(index).Value.ToString
        Me.SqlDS_Net2_Publication_Admin_GetList.DeleteParameters("PublicID").DefaultValue = key
        Me.Net2_Publication_Admin_GetList_Adminstrator.DeleteParameters("PublicID").DefaultValue = key
    End Sub

#End Region

End Class
