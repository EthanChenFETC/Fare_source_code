Imports System.Data
Imports System.Data.SqlClient

Partial Class SystemManager_Group
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Me.Literal1.Text = "<label accesskey=""Q"" for=""" & Me.txtSearch.ClientID.ToString & """ > Search(Q):</label>"
        'Me.txtSearch.Attributes.Add("onkeypress", "if( event.keyCode == 13 )  { form1." & Me.btnSearch.ClientID & ".focus(); }")

        '搜尋區塊設定
        ModuleMisc.SearchFunction(Me.Literal1, Me.txtSearch, Me.btnSearch)
    End Sub

#Region "GridView1 Work"

    Protected Sub GridView1_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.DataBound
        '提供分頁資料呈現
        GridViewPageInfo.GetGridViewInfo(GridView1, Me.Page, Me.SqlDS_Group)
    End Sub

    ''' <summary>
    ''' 設定前後台樹狀權限連結
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand
        If e.CommandName = "SetGroupPermission" Then
            Session("SettingGroupID") = e.CommandArgument
            InterfaceBuilder.TabGoNext(Me.Page)
            'InterfaceBuilder.ChangeUserControlByPassFileName(Me.Page, "GroupAdminPermission.aspx", "ascx")
        End If

        '編輯群組設定
        If e.CommandName = "EditItem" Then
            doEditGroupItem(e.CommandArgument)
        End If

        '刪除群組
        If e.CommandName = "GroupDelete" Then
            Dim GroupID As Integer = CInt(e.CommandArgument)
            Try
                Dim sql As String = "DELETE FROM Accounts_Group WHERE (GroupID = @GroupID)" ' & GroupID & ")"
                ClassDB.UpdateDBText(sql, New SqlParameter("@GroupID", GroupID))
                sql = "DELETE FROM Accounts_GroupUsers WHERE (GroupID = @GroupID )"
                ClassDB.UpdateDBText(sql, New SqlParameter("@GroupID", GroupID))

                sql = "DELETE FROM Accounts_GroupSitePermission WHERE (GroupID = @GroupID)"
                ClassDB.UpdateDBText(sql, New SqlParameter("@GroupID", GroupID))

                sql = "DELETE FROM Accounts_GroupsRelation  WHERE (OwnerGroupID = @GroupID ) OR (CreateGroupID =@GroupID)"
                ClassDB.UpdateDBText(sql, New SqlParameter("@GroupID", GroupID))

                sql = "DELETE FROM Accounts_Permissions WHERE  (GroupID = @GroupID)"
                ClassDB.UpdateDBText(sql, New SqlParameter("@GroupID", GroupID))

            Catch ex As Exception
                WriteErrLog(ex, Me.Page)
            End Try

            SqlDS_Group_SetSelectCommand()
            Me.GridView1.DataBind()
            Me.lbMessage.Text = GridViewPageInfo.Message_Delete_OK(Me.Page, "權限群組")
        End If
    End Sub

    ''' <summary>
    ''' GridView1_RowDataBound
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub GridView1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView1.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim drowView As System.Data.DataRowView = CType(e.Row.DataItem, System.Data.DataRowView)
            If e.Row.RowState = 0 OrElse e.Row.RowState = 1 OrElse e.Row.RowState = 2 OrElse e.Row.RowState = 3 Then

                Dim DataKeys As Integer = CInt(drowView("GroupID"))

                Dim ImagePublishSites As Image = CType(e.Row.FindControl("ImagePublishSites"), Image)
                Dim LinkButton_Admin As LinkButton = CType(e.Row.FindControl("btnSetGroupPermission"), LinkButton)
                Dim btn1 As Button = CType(e.Row.FindControl("btn_EditItem"), Button)
                Dim BtnDel As Button = CType(e.Row.FindControl("BtnDel"), Button)

                If drowView("GroupName") = "Administrator" Then
                    ImagePublishSites.ToolTip = "網站系統管理員 本群組擁有所有的權限！"
                    LinkButton_Admin.ToolTip = "本群組無須設定樹狀選單！"
                    LinkButton_Admin.Enabled = False
                    btn1.Enabled = False
                    BtnDel.Enabled = False
                Else
                    LinkButton_Admin.CommandArgument = DataKeys
                    btn1.CommandArgument = DataKeys

                    BindGroupInfo(ImagePublishSites, DataKeys, LinkButton_Admin)
                End If


                '非Administrator時限制修改自己群組設定
                If Session("Administrator") Is Nothing Then
                    'disable LoginSelf' row can't edit
                    Dim GroupID As String = Session("GroupID").ToString
                    Dim ArrayGroupID() As String = Split(GroupID, "_")
                    Dim i As Integer
                    For i = 0 To UBound(ArrayGroupID)
                        If DataKeys = (ArrayGroupID(i)) Then
                            Dim btn2 As Button = CType(e.Row.FindControl("BtnDel"), Button)
                            btn1.Enabled = False
                            btn2.Enabled = False

                            LinkButton_Admin.Enabled = False
                        End If
                    Next
                End If
                BtnDel.CommandArgument = drowView("GroupID")

                '繫結編輯按鈕
                Dim btn_EditItem As Button = CType(e.Row.FindControl("btn_EditItem"), Button)
                btn_EditItem.CommandArgument = DataKeys
                Dim Label1 As Label = CType(e.Row.FindControl("Label1"), Label)
                Label1.Text = (drowView("Description"))
            ElseIf e.Row.RowState = DataControlRowState.Edit Or e.Row.RowState = DataControlRowState.Insert Then
                Dim TextBox1 As TextBox = CType(e.Row.FindControl("TextBox1"), TextBox)
                TextBox1.Text = drowView("Description")
            End If

        End If
    End Sub

    ''' <summary>
    ''' Bind在檢視狀態下的網站權限 CheckBoxList
    ''' </summary>
    ''' <param name="ImagePublishSites"></param>
    ''' <param name="DataKeys"></param>
    ''' <remarks></remarks>
    Private Sub BindGroupInfo(ByVal ImagePublishSites As Image, ByVal DataKeys As String, ByVal LinkButton_Admin As LinkButton)
        Dim info As String = ""
        ImagePublishSites.Visible = False
        Dim i As Integer = 1
        Dim GroupID As Integer = CInt(DataKeys)
        Using dr As SqlDataReader = ClassDB.GetDataReaderSP("Net2_SiteID_List_byGroupID", New SqlParameter("@GroupID", GroupID))
            Try
                If dr IsNot Nothing Then
                    While dr.Read
                        Dim sitename As String = dr("SiteName")

                        info &= i & "." & sitename & vbCrLf
                        i += 1
                    End While
                End If
            Catch ex As Exception
                WriteErrLog(ex, Me.Page)
            Finally
            End Try
        End Using
        If info.Length > 0 Then ImagePublishSites.Visible = True
        ImagePublishSites.ToolTip = info

        'Show The NodeCount
        Dim sql As String = "SELECT AdminPermissionCount, TreePermissionCount FROM Accounts_Group WHERE GroupID=@GroupID" ' & DataKeys
        Using dr2 As SqlDataReader = ClassDB.GetDataReaderParam(sql, New SqlParameter("@GroupID", GroupID))
            Try
                If dr2 IsNot Nothing Then
                    If dr2.Read Then
                        LinkButton_Admin.Text = "管理權限 (已開啟" & IIf(IsNumeric(dr2("AdminPermissionCount")), CInt(dr2("AdminPermissionCount")).ToString, 0) & "個後端與" & IIf(IsNumeric(dr2("TreePermissionCount")), CInt(dr2("TreePermissionCount")).ToString, 0) & "前端單元權限)"
                    End If
                End If
            Catch ex As Exception
                WriteErrLog(ex, Me.Page)
            Finally
            End Try
        End Using
    End Sub

#End Region


#Region "更新作業"

    ''' <summary>
    ''' 進入編輯介面
    ''' </summary>
    ''' <param name="GroupID"></param>
    ''' <remarks></remarks>
    Private Sub doEditGroupItem(ByVal GroupID As Integer)
        Me.MultiView1.ActiveViewIndex = 1
        'Dim GroupID As Integer = CInt(e.CommandArgument)
        Dim sql As String = "SELECT * FROM Accounts_Group WHERE GroupID=@GroupID" ' & GroupID
        Using dr As SqlDataReader = ClassDB.GetDataReaderParam(sql, New SqlParameter("@GroupID", GroupID))
            Try
                If dr IsNot Nothing Then
                    If dr.Read Then
                        Me.txt_GroupName2.Text = RemoveXSS(dr("GroupName").ToString)
                        Me.txt_Description2.Text = RemoveXSS(dr("Description").ToString)
                        Me.cbIsOnline.Checked = CType(dr("IsOnline"), Boolean)
                        BindCheckBoxListEdit(Me.cbl_SiteList, GroupID)
                    End If
                End If
            Catch ex As Exception
                WriteErrLog(ex, Me.Page)
            Finally
            End Try
        End Using
        Me.ViewState.Add("GroupID", GroupID)
    End Sub


    ''' <summary>
    ''' 設定在編輯狀態下的網站權限CheckBoxlist
    ''' </summary>
    ''' <param name="CheckBoxList1"></param>
    ''' <param name="DataKeys"></param>
    ''' <remarks></remarks>
    Private Sub BindCheckBoxListEdit(ByVal CheckBoxList1 As CheckBoxList, ByVal DataKeys As String)
        If Not Regex.IsMatch(Session("GroupID").Replace("_", ""), "^[0-9]+$") Then
            Exit Sub
        End If
        '1.取得目前所有後端已設立的網站
        Dim sql_AllSites As String = "SELECT SiteID, SiteName FROM Sites where IsOnline > 0 "
        Using dr As SqlDataReader = ClassDB.GetDataReader(sql_AllSites)
            CheckBoxList1.Items.Clear()
            Try
                While dr.Read
                    Dim siteid As String = dr("SiteID")

                    Dim sitename As String = dr("SiteName")

                    CheckBoxList1.Items.Add(New ListItem(sitename, siteid))
                    CheckBoxList1.Items(CheckBoxList1.Items.Count - 1).Enabled = False    '先設為灰色
                End While
            Catch ex As Exception
                WriteErrLog(ex, Me.Page)
            End Try
        End Using
        ''2.取得登入者所擁有的網站權限
        If Session("Administrator") Is Nothing Then
                'Mutil GroupID
                Dim LoginGroupID As String = CStr(Session("GroupID"))
            If LoginGroupID.IndexOf("_") <> -1 Then
                Dim ArrayGroupID() As String = Split(LoginGroupID, "_")
                Dim i As Integer
                For i = 0 To UBound(ArrayGroupID)
                    If IsNumeric(ArrayGroupID(i)) Then
                        Using dr As SqlDataReader = ClassDB.GetDataReaderSP("Net2_Accounts_GetSiteID_byGroupID", New SqlParameter("@GroupID", ArrayGroupID(i)))
                            Try
                                If dr IsNot Nothing Then
                                    While dr.Read
                                        For j As Integer = 0 To CheckBoxList1.Items.Count - 1
                                            Dim cb As ListItem = CheckBoxList1.Items(j)
                                            If cb.Value = CInt(dr("SiteID")) Then
                                                cb.Enabled = True
                                            End If
                                        Next
                                    End While
                                End If
                            Catch ex As Exception
                                WriteErrLog(ex, Me.Page)
                            End Try
                        End Using
                    End If
                Next
            Else
                'Singal GroupID

                Using dr As SqlDataReader = ClassDB.GetDataReaderSP("Net2_Accounts_GetSiteID_byGroupID", New SqlParameter("@GroupID", CInt(LoginGroupID)))
                    Try
                        If dr IsNot Nothing Then
                            While dr.Read
                                For j As Integer = 0 To CheckBoxList1.Items.Count - 1
                                    Dim cb As ListItem = CheckBoxList1.Items(j)
                                    If cb.Value = CInt(dr("SiteID")) Then
                                        cb.Enabled = True
                                    End If
                                Next
                            End While
                        End If
                    Catch ex As Exception
                        WriteErrLog(ex, Me.Page)
                    End Try
                End Using
            End If
            Else

            For j As Integer = 0 To CheckBoxList1.Items.Count - 1
                Dim cb As ListItem = CheckBoxList1.Items(j)
                cb.Enabled = True
            Next
        End If

        '取得本列群組的GroupID所擁有的SiteID
        Using dr As SqlDataReader = ClassDB.GetDataReaderSP("Net2_Accounts_GetSiteID_byGroupID", New SqlParameter("@GroupID", CInt(DataKeys)))
            Try
                If dr IsNot Nothing Then
                    While dr.Read
                        For j As Integer = 0 To CheckBoxList1.Items.Count - 1
                            Dim cb As ListItem = CheckBoxList1.Items(j)
                            If cb.Value = CInt(dr("SiteID")) Then
                                cb.Selected = True
                            End If
                        Next
                    End While
                End If
            Catch ex As Exception
                WriteErrLog(ex, Me.Page)
            Finally
            End Try
        End Using
    End Sub

    ''' <summary>
    ''' 確定更新群組
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btn_Update_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Update.Click
        doUpdate()
    End Sub

    ''' <summary>
    ''' 取消返回
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btn_Cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Cancel.Click
        Me.MultiView1.ActiveViewIndex = 0
    End Sub


    ''' <summary>
    ''' 群組設定更新-可用前台網站權限、名稱、簡述、是否有效
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub doUpdate()
        Dim GroupID As Integer = 0
        If Me.ViewState("GroupID") <> Nothing Then
            GroupID = CInt(Me.ViewState("GroupID"))
        End If
        Dim GroupName As String = Me.txt_GroupName2.Text

        If Me.CheckGroupNameExits(GroupName, GroupID) = True Then
            Me.lbMessage_Edit.Text = "系統訊息：目前系統內已經有相同之群組名稱，請重新命名！謝謝。"
            Exit Sub
        End If

        '取得DataKey後進行網站權限更新
        If GroupID <> 0 Then

            '先刪除原來Group擁有的SiteID
            Dim sql_Delete As String = "DELETE FROM Accounts_GroupSitePermission WHERE GroupID=@GroupID" ''" & GroupID & "'"
            ClassDB.UpdateDBText(sql_Delete, New SqlParameter("@GroupID", GroupID))

            '重新寫入新的網站權限設定

            For i As Integer = 0 To Me.cbl_SiteList.Items.Count - 1
                Dim cb As ListItem = Me.cbl_SiteList.Items(i)
                If cb.Selected = True Then
                    Dim sql_Add As String = "INSERT INTO Accounts_GroupSitePermission (GroupID, SiteID) VALUES  (@GroupID, @SiteID)"  '& GroupID & "','" & cb.Value & "')"
                    ClassDB.UpdateDBText(sql_Add, New SqlParameter("@GroupID", GroupID), New SqlParameter("@SiteID", CInt(cb.Value)))
                End If
            Next

            '群組資料更新
            Me.SqlDS_Group.UpdateParameters("GroupID").DefaultValue = GroupID
            Me.SqlDS_Group.Update()
        End If
    End Sub


    ''' <summary>
    ''' 完成群組編輯更新資料庫動作
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub SqlDS_Group_Updated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.SqlDataSourceStatusEventArgs) Handles SqlDS_Group.Updated
        If e.AffectedRows > 0 Then
            Me.lbMessage.Text = GridViewPageInfo.Message_Update_OK(Me.Page, "群組資料編輯")
        Else
            Me.lbMessage.Text = GridViewPageInfo.Message_Update_False(Me.Page, "群組資料編輯")
        End If
        Me.MultiView1.ActiveViewIndex = 0
    End Sub

#End Region


#Region " 新增群組，設定新增資料 "

    Private Function CheckGroupNameExits(ByVal GroupName As String, Optional ByVal GroupID As Integer = 0) As Boolean
        Dim sql As String = ""
        Dim IsExits As Boolean = False
        Select Case GroupID
            Case 0
                '檢查新增的
                sql = "SELECT GroupID FROM Accounts_Group WHERE (GroupName = @GroupName) and @GroupID = 0 "

            Case Else
                '檢查修改的
                sql = "SELECT GroupID FROM Accounts_Group WHERE (GroupName = @GroupName) AND (GroupID <> @GroupID )"
        End Select

        Using dr As SqlDataReader = ClassDB.GetDataReaderParam(sql, New SqlParameter("@GroupName", GroupName), New SqlParameter("@GroupID", GroupID))
            Try
                If dr IsNot Nothing Then
                    If dr.Read Then
                        WriteLog("CheckGroupNameExits", dr("GroupID"))
                        IsExits = True
                    End If
                End If
            Catch ex As Exception
                WriteErrLog(ex, Me.Page)
            Finally

            End Try
        End Using
        Return IsExits

    End Function

    ''' <summary>
    ''' 新增群組，設定新增資料
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnAddNewGroup_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNewGroup.Click

        Dim GroupName As String = RemoveSQLInjection(Me.txt_GroupName.Text)

        If CheckGroupNameExits(GroupName) = True Then
            Me.lbMessage.Text = "系統訊息：目前系統內已經有相同之群組名稱『" & (GroupName) & "』，請重新命名！謝謝。"
            Exit Sub
        End If

        Dim UpdateUser As String = (CInt(Session("UserID")))
        Dim Description As String = RemoveSQLInjection(Me.txt_Description.Text)

        '由資料來源物件執行插入資料庫動作
        Me.SqlDataSource_GroupAdd.InsertParameters("GroupName").DefaultValue = GroupName
        Me.SqlDataSource_GroupAdd.InsertParameters("Description").DefaultValue = Description
        Me.SqlDataSource_GroupAdd.InsertParameters("isOnline").DefaultValue = True
        Me.SqlDataSource_GroupAdd.InsertParameters("UpdateUser").DefaultValue = UpdateUser
        Me.SqlDataSource_GroupAdd.Insert()
    End Sub

    ''' <summary>
    ''' 插入1個群組至資料庫完成，進行群組關聯作業
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub SqlDataSource_GroupAdd_Inserted(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.SqlDataSourceStatusEventArgs) Handles SqlDataSource_GroupAdd.Inserted
        If e.AffectedRows > 0 Then

            Dim command As Common.DbCommand
            command = e.Command

            '取得SqlDataSource_GroupAdd新增後的主鍵
            Dim CreateGroupID As Integer = CInt(command.Parameters("@retVal").Value.ToString())

            '新增群組關聯-提供群組之從屬關係關聯
            GroupRelationAdd(CreateGroupID)

            SqlDS_Group_SetSelectCommand() '重新取得登入者擁有的群組編號
            Me.GridView1.DataBind()
            Me.lbMessage.Text = GridViewPageInfo.Message_Insert_OK(Me.Page, "權限群組")

        Else
            Me.lbMessage.Text = GridViewPageInfo.Message_Insert_False(Me.Page, "權限群組")
        End If

    End Sub

    ''' <summary>
    ''' 新增群組關聯-提供群組之從屬關係關聯(平面化的群組從屬關係,只會單層關係,沒有樹狀關聯)
    ''' </summary>
    ''' <param name="CreateGroupID">新建立的GroupID</param>
    ''' <remarks></remarks>
    Private Sub GroupRelationAdd(ByVal CreateGroupID As Integer)

        '取得操作者本身之權限群組之父群組編號(即可管理本群組的擁有者)，呼叫傳回的結果包含了目前的群組編號
        Dim Me_And_MyParentGroups() As String = Split(ModulePermissions.GetMyParentGroups(Session("GroupID").ToString), ",")
        Dim i As Integer
        For i = 0 To UBound(Me_And_MyParentGroups)
            '設定插入SDS參數
            Me.SqlDataSource_GroupRelationAdd.InsertParameters("UpdateUser").DefaultValue = CInt(Session("UserID").ToString)  '操作者
            Me.SqlDataSource_GroupRelationAdd.InsertParameters("CreateGroupID").DefaultValue = CreateGroupID    '被建造者
            Me.SqlDataSource_GroupRelationAdd.InsertParameters("OwnerGroupID").DefaultValue = Me_And_MyParentGroups(i)
            Me.SqlDataSource_GroupRelationAdd.Insert()  '回圈新增關聯紀錄
        Next
    End Sub

#End Region


#Region " SQLDataSource-SqlDs_Group "

    ''' <summary>
    ''' 動態設定資料來源(篩選群組)
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub SqlDS_Group_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles SqlDS_Group.Load
        '建立GridView
        SqlDS_Group_SetSelectCommand()
    End Sub


    ''' <summary>
    ''' 權限群組篩選
    ''' 1.Administrator(不篩選)
    ''' 2.一般人員(帶入登入者的群組)
    '''  .單數群組（由ModulePermissions=>取得自己群組所擁有可視的群組）
    '''  .複數群組（由ModulePermissions=>取得自己群組所擁有可視的群組）
    ''' 3.預存程序應刪除(Net2_Accounts_Group_List) 2007-7-18
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub SqlDS_Group_SetSelectCommand()
        Dim sql As String = ""
        Me.SqlDS_Group.SelectParameters.Clear()
        Dim MyOwnGroupIDs As String = ""
        sql = "SELECT  GroupID, GroupName, Description, isOnline, UpdateUser, UpdateDateTime FROM Accounts_Group "
        sql += " WHERE (GroupName LIKE N'%' + @Keyword + N'%') "
        If Session("Administrator") = False Then
            '網站管理員 Administrator
        Else
            '一般使用者群組
            Dim GroupID As String = Session("GroupID")
            If Not Regex.IsMatch(GroupID.Replace("_", ""), "^[0-9]+$") Then
                Exit Sub
            End If
            MyOwnGroupIDs = ModulePermissions.GetMyOwnerGroups(GroupID)
            If Not Regex.IsMatch(MyOwnGroupIDs.Replace(",", ""), "^[0-9]+$") Then
                Exit Sub
            End If
            Dim groupids() As String = MyOwnGroupIDs.Split(",")
            If groupids.Length > 0 Then
                sql &= "  AND ("
            End If

            For i As Integer = 0 To groupids.Length - 1
                sql &= " (GroupID = @GroupID" & i.ToString & ") or"
                Me.SqlDS_Group.SelectParameters.Add(New Parameter("GroupID" & i.ToString, TypeCode.Int32, groupids(i)))
            Next
            sql = sql.Substring(0, sql.Length - 2) & ")"
        End If
        'sql = sql.Replace("@MyOwnGroupIDs", "(" & MyOwnGroupIDs & ")")
        Me.SqlDS_Group.SelectCommand = sql & " ORDER BY GroupID"

        'Dim GroupIDs As New System.Web.UI.WebControls.Parameter
        'GroupIDs.Name = "MyOwnGroupIDs"
        'GroupIDs.Direction = ParameterDirection.Input
        'GroupIDs.Type = TypeCode.String
        'GroupIDs.DefaultValue = MyOwnGroupIDs
        'Me.SqlDS_Group.SelectParameters.Add(GroupIDs)

        Dim Keyword As New System.Web.UI.WebControls.Parameter
        Keyword.Name = "Keyword"
        Keyword.Direction = ParameterDirection.Input
        Keyword.Type = TypeCode.String
        Dim keywordstring As String = Me.txtSearch.Text
        If keywordstring.Trim.Length = 0 Then
            keywordstring = "%"
        Else
            keywordstring = RemoveSQLInjection(keywordstring)
        End If
        Keyword.DefaultValue = keywordstring
        Me.SqlDS_Group.SelectParameters.Add(Keyword)
    End Sub

#End Region


    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        SqlDS_Group_SetSelectCommand()
        Me.GridView1.DataBind()
    End Sub

    Protected Sub btnShowAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnShowAll.Click
        Me.txtSearch.Text = ""
        SqlDS_Group_SetSelectCommand()
        Me.GridView1.DataBind()
    End Sub
End Class
