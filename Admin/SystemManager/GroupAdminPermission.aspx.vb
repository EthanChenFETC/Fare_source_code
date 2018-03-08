Imports System.Web.Security
Imports System.Data
Imports System.Data.SQLClient
Imports ComponentArt.Web.UI
Imports System.IO

''' <summary>
''' 一、設定後台選單權限
''' 1.請取全部現有的後台選單
''' 
''' 二、設定前台選單權限
''' 1.取得全部的SiteMap Node
''' 2.勾選原群組擁有的Node
''' 3.將
''' </summary>
''' <remarks></remarks>
Partial Class SystemManager_GroupAdminPermission
    'Inherits System.Web.UI.Page
    Inherits PageBase

    'Admin
    Private _ThisGroupCheckNode_OldValue As String = ""
    Private _LoginUserAdminMenuList As String = ""
    Private _ThisGroupCheckNode_NewValue As String = ""
    Private _arrayThisGroupCheckNode_NewValue(0) As String

    'Site
    Private _LoginuserTreeNodeList As String = ""
    Private _ThisGroupTreeCheckNode_OldValue As String = ""

    Private _ThisGroupCheckNode_NewValue2 As String = ""
    Private _arrayThisGroupCheckNode_NewValue2(0) As String
    Private _ThisGroup_SiteIDList As String = ""


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            If Session("SettingGroupID") Is Nothing Then
                Response.Redirect("~/SystemManager/Group.aspx")   '(20080225mw)
            Else

                'Show the Setting GroupName
                RenderGroupName()

                Try
                    '建立樹狀
                    BuildAdminTree()

                    BuildSiteTree()

                    Me.TreeView1.ExpandAll()
                    Me.TreeView2.ExpandAll()
                Catch ex As Exception
                    WriteErrLog(ex, Me) '防止節點關聯有問題
                End Try

            End If

        End If
    End Sub


    ''' <summary>
    ''' 'Show the Setting GroupName
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub RenderGroupName()
        'Dim GroupName As String = ""

        Using dr As SqlDataReader = ClassDB.GetDataReaderParam("SELECT GroupName FROM Accounts_Group WHERE (GroupID=@GroupID)", New SqlParameter("GroupID", CInt(Session("SettingGroupID"))))
            Try
                If dr IsNot Nothing Then
                    If dr.Read Then
                        Me.LabelSettingGroupName.Text = (CStr(dr("GroupName")))
                        'WriteLog(Me.Page.Title & "RenderGroupName", CStr(dr("GroupName")))
                    End If
                End If
            Catch ex As Exception
                WriteErrLog(ex, Me)
            Finally

            End Try
        End Using
    End Sub

  


#Region " SiteTree選單建構區域 "


    ''' <summary>
    ''' 取得本群組之網站IDs
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GetThisGroup_SiteIDList()

        Dim ThisGroup_SiteList As String = ""
        Using dr As SqlDataReader = ClassDB.GetDataReaderSP("Net2_SiteID_List_byGroupID", New SqlParameter("@GroupID", CInt(Session("SettingGroupID"))))
            Try
                If dr IsNot Nothing Then
                    While dr.Read
                        ThisGroup_SiteList += CInt(dr("SiteID")).ToString & ","
                    End While
                End If
            Catch ex As Exception
                WriteErrLog(ex, Me)
            Finally

            End Try
        End Using
        '20091217修改為0的錯誤
        If ThisGroup_SiteList.Length > 0 Then
            ThisGroup_SiteList = ThisGroup_SiteList.Substring(0, ThisGroup_SiteList.Length - 1)
        End If

        _ThisGroup_SiteIDList = ThisGroup_SiteList

    End Sub


    ''' <summary>
    ''' 取得本群組之前台樹狀權限
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GetThisGroup_TreeNodeList()


        Dim Output As String = ""
        Using dr As SqlDataReader = ClassDB.GetDataReaderSP("Net2_Accounts_Permissions_NodeID_byGroupID",
        New SqlParameter("@GroupID", CInt(Session("SettingGroupID"))),
        New SqlParameter("Category", 3))
            Try
                If dr IsNot Nothing Then
                    While dr.Read
                        Output += CInt(dr("NodeID")).ToString & ","
                        'WriteLog("GetThisGroup_TreeNodeList", dr("NodeID") & ",")
                    End While
                End If
            Catch ex As Exception
                WriteErrLog(ex, Me)
            Finally

            End Try
        End Using
        If Output.Length > 0 Then
            Output = Output.Substring(0, Output.Length - 1)
        End If

        _ThisGroupTreeCheckNode_OldValue = Output
    End Sub


    ''' <summary>
    ''' 取得登入者所擁有的前台網站的NodeID,以","分隔，這組資料將提供打開樹狀、並且顯示CheckBox與文字功能
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GetLoginUserSiteNodeID()


        Dim Output As String = ""
        Using dr As SqlDataReader = ClassDB.GetDataReaderSP("Net2_Accounts_Permissions_NodeID_byUserID",
        New SqlParameter("@UserID", CInt(Session("UserID"))),
        New SqlParameter("@Category", 3))

            Try
                If dr IsNot Nothing Then
                    While dr.Read
                        Output += CInt(dr("NodeID")).ToString & ","
                    End While
                End If
            Catch ex As Exception
                WriteErrLog(ex, Me)
            Finally

            End Try
        End Using
        If Output.Length > 0 Then
            Output = Output.Substring(0, Output.Length - 1)
        End If

        _LoginuserTreeNodeList = Output

    End Sub


    ''' <summary>
    ''' AdminTree建立樹狀選單
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub BuildSiteTree()

        GetThisGroup_SiteIDList()

        GetLoginUserSiteNodeID()    '取得並設定登入者擁有的SiteMap節點清單

        GetThisGroup_TreeNodeList() '取得並設定本設定中的群組者擁有的SiteMap節點清單


        'Dim sql As String = "SELECT  NodeId, ParentNodeId, Text, ImageUrl FROM SiteMap WHERE  (SiteID IN (" & GetThisGroup_SiteIDList() & "))"
        'Dim sql As String = "SELECT  NodeId, ParentNodeId, Text, ImageUrl FROM SiteMap"
        Using ds As DataSet = ClassDB.RunSPReturnDataSet("Net2_SiteMap_List_Permissions", "SiteMapList")
            If Not ds.Relations.Contains("NodeRelation") Then
                Try
                    ds.Relations.Add("NodeRelation", ds.Tables(0).Columns("NodeId"), ds.Tables(0).Columns("ParentNodeId"))
                Catch ex As Exception
                    WriteErrLog(ex, Me)
                End Try
            End If

            'WriteLog("BuildSiteTree", "RowCount=" & ds.Tables(0).Rows.Count)

            Me.TreeView2.Nodes.Clear()

            For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                Dim dbRow As System.Data.DataRow = ds.Tables(0).Rows(i)
                If _ThisGroup_SiteIDList.IndexOf(dbRow("SiteID").ToString) >= 0 Then
                    If (dbRow.IsNull("ParentNodeId")) Then
                        Dim newNode As ComponentArt.Web.UI.TreeViewNode
                        newNode = SiteCreateNode(dbRow("Text").ToString(), dbRow("ImageUrl").ToString(), True, dbRow("NodeID").ToString())
                        TreeView2.Nodes.Add(newNode)
                        SitePopulateSubTree(dbRow, newNode)
                    End If
                End If
            Next
        End Using
    End Sub


    ''' <summary>
    ''' 建立樹狀選單迴圈取得節點
    ''' </summary>
    ''' <param name="dbRow"></param>
    ''' <param name="node"></param>
    ''' <remarks></remarks>
    Private Sub SitePopulateSubTree(ByVal dbRow As System.Data.DataRow, ByVal node As ComponentArt.Web.UI.TreeViewNode)

        For i As Integer = 0 To dbRow.GetChildRows("NodeRelation").Count - 1
            Dim childRow As System.Data.DataRow = dbRow.GetChildRows("NodeRelation")(i)
            If _ThisGroup_SiteIDList.IndexOf(childRow("SiteID").ToString) >= 0 Then
                Dim childNode As ComponentArt.Web.UI.TreeViewNode = SiteCreateNode(childRow("Text").ToString(), childRow("ImageUrl").ToString(), False, childRow("NodeID").ToString())
                node.Nodes.Add(childNode)
                SitePopulateSubTree(childRow, childNode)
            End If
        Next
    End Sub

    ''' <summary>
    ''' 建立樹狀選單:節點實體化
    ''' </summary>
    ''' <param name="text"></param>
    ''' <param name="imageurl"></param>
    ''' <param name="expanded"></param>
    ''' <param name="NodeID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function SiteCreateNode(ByVal text As String, ByVal imageurl As String, ByVal expanded As Boolean, ByVal NodeID As String) As ComponentArt.Web.UI.TreeViewNode
        If Not IsNumeric(NodeID) Then
            NodeID = "0"
        End If

        Dim node As New ComponentArt.Web.UI.TreeViewNode
        node.Text = text
        node.ImageUrl = imageurl
        node.Expanded = expanded
        node.AutoPostBackOnCheckChanged = False
        node.ID = NodeID
        node.Value = NodeID
        node.ShowCheckBox = False
        node.Visible = False

        If Session("SuperAdmin") = True Then

            '提供超級管理員可以全部打開樹狀選單
            node.ShowCheckBox = True
            node.Visible = True

        Else

            '設定登入者自己擁有權限的，才開啟CheckBox
            If _LoginuserTreeNodeList.Length > 0 Then
                Dim ArrayCheckNode() As String = Split(_LoginuserTreeNodeList, ",")
                Dim i As Integer
                For i = 0 To UBound(ArrayCheckNode)
                    If NodeID = ArrayCheckNode(i) Then
                        node.ShowCheckBox = True
                        node.Visible = True
                    End If
                Next
            End If

        End If


        '將原本此群組擁有的NODE Checked
        'WriteLog("_ThisGroupTreeCheckNode_OldValue Length", _ThisGroupTreeCheckNode_OldValue.Length)
        If _ThisGroupTreeCheckNode_OldValue.Length > 0 Then
            'WriteLog("_ThisGroupTreeCheckNode_OldValue", _ThisGroupTreeCheckNode_OldValue)
            Dim ArrayCheckNode() As String = Split(_ThisGroupTreeCheckNode_OldValue, ",")
            Dim i As Integer
            For i = 0 To UBound(ArrayCheckNode)
                If NodeID = ArrayCheckNode(i) Then
                    node.ShowCheckBox = True
                    node.Checked = True
                Else

                End If
            Next
        End If
        Return node
    End Function


#End Region


#Region " AdminMenu選單建構副屬 "

    ''' <summary>
    ''' 取得操作者擁有的權限節點
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GetLoginUserAdminMenuList()
        '登入中Cache中的權限表
        Using ds As DataSet = ModulePermissions.GetPermissionsTable(Me)
            Dim LoginUserAdminMenuList As String = ""

            For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                Dim dbRow As System.Data.DataRow = ds.Tables(0).Rows(i)
                LoginUserAdminMenuList += dbRow("NodeID") & ","
                'If dbRow("SuperAdmin") = False Then
                '    LoginUserAdminMenuList += dbRow("NodeID") & ","
                'End If
            Next
            LoginUserAdminMenuList = LoginUserAdminMenuList.Substring(0, LoginUserAdminMenuList.Length - 1)

            '設定
            _LoginUserAdminMenuList = LoginUserAdminMenuList
        End Using
    End Sub

    ''' <summary>
    ''' 取得設定中群組的權限節點
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GetThisGroupAdminMenuList()
        '登入中Cache中的權限表

        Using ds As DataSet = ClassDB.RunSPReturnDataSet("Net2_Accounts_Permissions_NodeID", "This_Accounts_Permissions_NodeID", New SqlParameter("@GroupID", Session("SettingGroupID")))
            If ds.Tables(0).Rows.Count > 0 Then
                Dim ThisGroupAdminMenuList As String = ""

                For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                    Dim dbRow As System.Data.DataRow = ds.Tables(0).Rows(i)
                    ThisGroupAdminMenuList += dbRow("NodeID") & ","
                Next
                ThisGroupAdminMenuList = ThisGroupAdminMenuList.Substring(0, ThisGroupAdminMenuList.Length - 1)

                '設定
                _ThisGroupCheckNode_OldValue = ThisGroupAdminMenuList

                '_ThisGroupID = CInt(Session("SettingGroupID"))
                'Session("SettingGroupID") = Nothing


            End If
        End Using
    End Sub

#End Region


#Region " AdminTree選單建構區域 "
    '====================================================================================================
    '樹狀選單區域
    '====================================================================================================

    ''' <summary>
    ''' AdminTree建立樹狀選單
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub BuildAdminTree()

        '取得操作者擁有的權限節點
        GetLoginUserAdminMenuList()

        '取得設定中群組的權限節點
        GetThisGroupAdminMenuList()



        Using ds As DataSet = ClassDB.RunSPReturnDataSet("Net2_Get_AdminMenu_List", "AdminMenuList")

            'Dim ds As DataSet = ModulePermissions.GetPermissionsTable(Me)

            If Not ds.Relations.Contains("NodeRelation") Then
                Try
                    ds.Relations.Add("NodeRelation", ds.Tables(0).Columns("NodeId"), ds.Tables(0).Columns("ParentNodeId"))
                Catch ex As Exception
                    WriteErrLog(ex, Me)
                End Try

            End If

            'WriteLog("BuildAdminTree", "RowCount=" & ds.Tables(0).Rows.Count)

            Me.TreeView1.Nodes.Clear()

            For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                Dim dbRow As System.Data.DataRow = ds.Tables(0).Rows(i)
                If (dbRow.IsNull("ParentNodeId")) Then
                    Dim newNode As ComponentArt.Web.UI.TreeViewNode
                    newNode = CreateNode(dbRow("Text").ToString(), dbRow("ImageUrl").ToString(), True, dbRow("NodeID").ToString())
                    TreeView1.Nodes.Add(newNode)
                    PopulateSubTree(dbRow, newNode)
                End If
            Next
        End Using
    End Sub

    ''' <summary>
    ''' 建立樹狀選單迴圈取得節點
    ''' </summary>
    ''' <param name="dbRow"></param>
    ''' <param name="node"></param>
    ''' <remarks></remarks>
    Private Sub PopulateSubTree(ByVal dbRow As System.Data.DataRow, ByVal node As ComponentArt.Web.UI.TreeViewNode)

        For i As Integer = 0 To dbRow.GetChildRows("NodeRelation").Count - 1
            Dim childRow As System.Data.DataRow = dbRow.GetChildRows("NodeRelation")(i)
            Dim childNode As ComponentArt.Web.UI.TreeViewNode = CreateNode(childRow("Text").ToString(), childRow("ImageUrl").ToString(), False, childRow("NodeID").ToString())
            node.Nodes.Add(childNode)
            PopulateSubTree(childRow, childNode)
        Next
    End Sub

    ''' <summary>
    ''' 建立樹狀選單:節點實體化
    ''' </summary>
    ''' <param name="text"></param>
    ''' <param name="imageurl"></param>
    ''' <param name="expanded"></param>
    ''' <param name="NodeID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CreateNode(ByVal text As String, ByVal imageurl As String, ByVal expanded As Boolean, ByVal NodeID As String) As ComponentArt.Web.UI.TreeViewNode
        If Not IsNumeric(NodeID) Then
            NodeID = "0"
        End If

        Dim node As New ComponentArt.Web.UI.TreeViewNode
        node.Text = text
        node.ImageUrl = imageurl
        node.Expanded = expanded
        node.AutoPostBackOnCheckChanged = False
        node.ID = NodeID
        node.Value = NodeID
        node.ShowCheckBox = False
        node.Visible = False


        '設定自己擁有權限的，才開啟CheckBox
        If _LoginUserAdminMenuList.Length > 0 Then
            Dim ArrayCheckNode() As String = Split(_LoginUserAdminMenuList, ",")
            Dim i As Integer
            For i = 0 To UBound(ArrayCheckNode)
                If NodeID = ArrayCheckNode(i) Then
                    node.ShowCheckBox = True
                    node.Visible = True
                End If
            Next
        End If

        '將原本此群組擁有的NODE Checked
        If _ThisGroupCheckNode_OldValue.Length > 0 Then
            Dim ArrayCheckNode() As String = Split(_ThisGroupCheckNode_OldValue, ",")
            Dim i As Integer
            For i = 0 To UBound(ArrayCheckNode)
                If NodeID = ArrayCheckNode(i) Then
                    node.ShowCheckBox = True
                    node.Checked = True
                Else

                End If
            Next
        End If
        Return node
    End Function

#End Region

    ''' <summary>
    ''' Update the Data into Database the User Check on TreeView1
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btn_Update_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Update.Click


        '===================================================================================================
        'AdminMenu TreeView1
        '===================================================================================================

        '##Got the NodeID User Checked

        For i As Integer = 0 To Me.TreeView1.CheckedNodes.Count - 1
            Dim Node As ComponentArt.Web.UI.TreeViewNode = Me.TreeView1.CheckedNodes(i)
            'WriteLog("btn_Update_Click-depth", Node.Depth)

            '核選的Node把ID記起來
            _ThisGroupCheckNode_NewValue += Node.Value & ","
            _arrayThisGroupCheckNode_NewValue = Split(_ThisGroupCheckNode_NewValue, ",")

            'If this Node Have ParentNode Should be Check
            If Not Node.ParentNode Is Nothing Then

                If Array.IndexOf(_arrayThisGroupCheckNode_NewValue, Node.ParentNode.Value) = -1 Then
                    _ThisGroupCheckNode_NewValue += Node.ParentNode.Value & ","
                    _arrayThisGroupCheckNode_NewValue = Split(_ThisGroupCheckNode_NewValue, ",")
                End If

            End If
        Next

        '##Update to DataBase
        '1.Delete the old Vale in Accounts_Permissions
        Dim GroupID As Integer = Session("SettingGroupID")

        ClassDB.UpdateDBText("DELETE FROM Accounts_Permissions WHERE GroupID=@GroupID And Category=1", New SqlParameter("@GroupID", GroupID))

        '2.Update the New Value to Database

        For i As Integer = 0 To UBound(_arrayThisGroupCheckNode_NewValue) - 1  '有多一個","
            ClassDB.UpdateDB("Net2_Accounts_Permissions_Add",
                   New SqlParameter("@GroupID", CInt(Session("SettingGroupID"))),
                   New SqlParameter("@NodeID", CInt(_arrayThisGroupCheckNode_NewValue(i))),
                   New SqlParameter("@Category", 1),
                   New SqlParameter("@UpdateUser", CInt(Session("UserID"))))
        Next

        '3.Update the Group Infomation About how much Node it have the permission
        ClassDB.UpdateDBText("UPDATE Accounts_Group SET AdminPermissionCount=" & UBound(_arrayThisGroupCheckNode_NewValue) - 1 & " WHERE GroupID=@GroupID", New SqlParameter("@GroupID", GroupID))





        '===================================================================================================
        'SiteTree TreeView1
        '===================================================================================================
        Try

            '##Got the NodeID User Checked
            For i As Integer = 0 To Me.TreeView2.CheckedNodes.Count - 1
                Dim Node As ComponentArt.Web.UI.TreeViewNode = Me.TreeView2.CheckedNodes(i)

                '核選的Node把ID記起來
                _ThisGroupCheckNode_NewValue2 += Node.Value & ","
                _arrayThisGroupCheckNode_NewValue2 = Split(_ThisGroupCheckNode_NewValue2, ",")

                'If this Node Have ParentNode Should be Check
                If Not Node.ParentNode Is Nothing Then

                    If Array.IndexOf(_arrayThisGroupCheckNode_NewValue2, Node.ParentNode.Value) = -1 Then
                        _ThisGroupCheckNode_NewValue2 += Node.ParentNode.Value & ","
                        _arrayThisGroupCheckNode_NewValue2 = Split(_ThisGroupCheckNode_NewValue2, ",")
                    End If

                End If
            Next

        Catch ex As Exception
            Dim dd As String = ex.Message
        End Try
        '##Update to DataBase
        '1.Delete the old Vale in Accounts_Permissions
        ClassDB.UpdateDBText("DELETE FROM Accounts_Permissions WHERE GroupID=@GroupID And Category=3", New SqlParameter("@GroupID", GroupID))

        '2.Update the New Value to Database
        For i As Integer = 0 To UBound(_arrayThisGroupCheckNode_NewValue2) - 1  '有多一個","
            ClassDB.UpdateDB("Net2_Accounts_Permissions_Add",
                   New SqlParameter("@GroupID", CInt(Session("SettingGroupID"))),
                   New SqlParameter("@NodeID", CInt(_arrayThisGroupCheckNode_NewValue2(i))),
                   New SqlParameter("@Category", 3),
                   New SqlParameter("@UpdateUser", CInt(Session("UserID"))))
        Next

        '3.Update the Group Infomation About how much Node it have the permission
        ClassDB.UpdateDBText("UPDATE Accounts_Group SET TreePermissionCount=@TreePermissionCount WHERE GroupID=@GroupID", New SqlParameter("@TreePermissionCount", UBound(_arrayThisGroupCheckNode_NewValue2) - 1), New SqlParameter("@GroupID", GroupID))




        ''===================================================================================================
        WriteAdminLog(Me, "設定群組後台權限，群組編號：" & Session("SettingGroupID"))

        'show message
        InterfaceBuilder.doPageActionMessage(Me, lblMessage)

       

        '清除在Cache中的permissions Data Table
        ModulePermissions.RemovePermissionsTable(Session("SettingGroupID"))

        Session("SettingGroupID") = Nothing

        InterfaceBuilder.TabGoPre(Me)

    End Sub

    Protected Sub btn_clearreload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_clearreload.Click
        Response.Redirect("GroupAdminPermission.aspx")
        'Me.TreeView1.Dispose()
        'Me.TreeView2.Dispose()
        'BuildAdminTree()
        'BuildSiteTree()
    End Sub
End Class
