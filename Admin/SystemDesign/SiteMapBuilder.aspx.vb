Imports System.Web.Security
Imports System.Data
Imports System.Data.SQLClient
Imports ComponentArt.Web.UI
Imports System.IO

Partial Class SystemDesign_SiteMapBuilder
    'Inherits System.Web.UI.Page
    Inherits PageBase

    
    Private _ShowCheckBox As Boolean = False
    Private SiteMapTable As String = "SiteMap"
    Private TreeCategory As Integer = 3

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Dim AllNodeListString As String = CType(Request.Form("__NodeIndex"), String)
        'Me.lblMessage_Add.Text = AllNodeListString

        If Not IsPostBack Then
            Session("EditSiteMap") = Nothing
            Session("SiteID") = Nothing
        End If
        If Session("R1") <> "" Then
            RadioButtonList1.SelectedValue = Session("R1")
            ddlSites.SelectedValue = Session("ddlSites").ToString
            Session("SiteID") = Session("ddlSites").ToString
            Session("EditSiteMap") = Nothing
            buildTree()
            Session("R1") = ""
            Session("ddlSites") = ""
        End If
        If Session("DelError") <> "" Or Session("DelError2") <> "" Then
            If Session("DelError") <> "" Then
                Err.Text = "<font color=red>失敗訊息：" & Session("DelError") & "刪除失敗!尚有相關子階層!</font>"
            End If
            If Session("DelError2") <> "" Then
                Err.Text = "<font color=red>失敗訊息：" & Session("DelError2") & "</font>"
            End If
            Session("DelError") = ""
            Session("DelError2") = ""
            Session("R1") = ""
            Session("ddlSites") = ""
        Else
            Err.Text = ""
        End If
        '判別前台網站地圖，或後台管理選單
        If ModulePathManager.GetPathKeyName(Me).ToLower.Equals("adminmenubuilder") Then
            SiteMapTable = "AdminMenu"
            TreeCategory = 1
        End If

        '保存樹狀資料的隱藏欄位
        Me.ClientScript.RegisterHiddenField("__NodeMove", "")
        Me.ClientScript.RegisterHiddenField("__NodeReName", "")
        Me.ClientScript.RegisterHiddenField("__NodeIndex", "")

        '隱藏按鈕-送出表單後執行伺服器事件的物件
        Dim BtnVirturlClientID As String = Me.BtnVirturl.ClientID
        'Dim BtnVirturlClientID As String = "ctl00$ContentPlaceHolder1$BtnVirturl"

        '如果是contentpage則其ClientID必須更換
        If Not Me.Master.Page Is Nothing Then
            BtnVirturlClientID = BtnVirturlClientID.Replace("_", "$")
        End If

        Me.TreeView1.CausesValidation = True

        Dim TreeView1ClientID As String = Me.TreeView1.ClientID
        'Me.lblMessage_Add.Text = TreeView1ClientID

        'TreeView物件Client Script
        Dim TreeViewScript As String
        TreeViewScript = "function nodeMove(sourceNode, targetNode)" & vbCrLf
        TreeViewScript += "	{" & vbCrLf
        TreeViewScript += "		document.getElementById('__NodeMove').value +=	sourceNode.Value + "","" +  targetNode.Value + "",""; " & vbCrLf
        TreeViewScript += "		return true; " & vbCrLf
        TreeViewScript += "	}" & vbCrLf

        TreeViewScript += "	function nodeRename(sourceNode,newName)" & vbCrLf
        TreeViewScript += "	{ " & vbCrLf
        TreeViewScript += "		document.getElementById('__NodeReName').value +=  sourceNode.Value + "","" + newName + "",""; " & vbCrLf
        TreeViewScript += "		" & TreeView1ClientID & ".SelectedNode.Text = newName;" & vbCrLf
        TreeViewScript += "		return true; " & vbCrLf
        TreeViewScript += "	}" & vbCrLf

        TreeViewScript += "	function startTheLoop()" & vbCrLf
        TreeViewScript += "	{" & vbCrLf
        TreeViewScript += "		var nodes = " & TreeView1ClientID & ".Nodes();" & vbCrLf
        TreeViewScript += "		document.getElementById('__NodeIndex').value ="""";" & vbCrLf
        TreeViewScript += "		for (var i = 0; i<nodes.length; i++)" & vbCrLf
        TreeViewScript += "	{" & vbCrLf
        TreeViewScript += "		myRecursiveFunction(nodes[i]);" & vbCrLf
        TreeViewScript += "	}" & vbCrLf
        TreeViewScript += "		__doPostBack('" & BtnVirturlClientID & "','')		//這裡可能要傳值,如果有其他按鈕" & vbCrLf
        TreeViewScript += "	}" & vbCrLf


        TreeViewScript += "	function myRecursiveFunction(node)" & vbCrLf
        TreeViewScript += "	{" & vbCrLf
        TreeViewScript += "		document.getElementById('__NodeIndex').value += node.Value + "","";" & vbCrLf
        TreeViewScript += "		var nodes = node.Nodes();" & vbCrLf
        TreeViewScript += "		for (var i = 0; i<nodes.length; i++)" & vbCrLf
        TreeViewScript += "		{" & vbCrLf
        TreeViewScript += "			myRecursiveFunction(nodes[i]);" & vbCrLf
        TreeViewScript += "		}" & vbCrLf
        TreeViewScript += "	}" & vbCrLf

        TreeViewScript += "	function expandAll()" & vbCrLf
        TreeViewScript += "	{" & vbCrLf
        TreeViewScript += "		" & TreeView1ClientID & ".ExpandAll();" & vbCrLf
        TreeViewScript += "	}" & vbCrLf

        TreeViewScript += "	function collapseAll()" & vbCrLf
        TreeViewScript += "	{" & vbCrLf
        TreeViewScript += "		" & TreeView1ClientID & ".CollapseAll();" & vbCrLf
        TreeViewScript += "	}" & vbCrLf

        Me.ClientScript.RegisterClientScriptBlock(GetType(UI.Page), "treeview", TreeViewScript, True)
        'Me.ClientScript.RegisterStartupScript(GetType(UI.Page), "treeview", TreeViewScript, True)

        'Me.Master.Page.ClientScript.RegisterHiddenField("__NodeMove", "")
        'Me.Master.Page.ClientScript.RegisterHiddenField("__NodeReName", "")
        'Me.Master.Page.ClientScript.RegisterHiddenField("__NodeIndex", "")

        If Not IsPostBack Then
            Session("EditSiteMap") = Nothing
            Session("SiteID") = Nothing
        End If

        If IsPostBack Then
            KeepMoveRaname()
        End If
        ModeChange()
    End Sub


    Protected Sub ddlSites_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlSites.SelectedIndexChanged
        Session("EditSiteMap") = Nothing
        Session("SiteID") = Me.ddlSites.SelectedValue
        buildTree()
    End Sub

#Region " 選單建構區域 "
    '====================================================================================================
    '樹狀選單區域
    '====================================================================================================

    Private Function GetDs() As DataSet

        Dim SiteID As Integer = Session("SiteID")
        Dim ds As DataSet = Session("EditSiteMap")
        If ds Is Nothing Then
            ds = ClassDB.RunReturnDataSet("SELECT * FROM " & SiteMapTable & " WHERE SiteID = " & Session("SiteID").ToString & " ORDER BY NodeOrder, NodeId", SiteMapTable)
        End If
        Session("SiteMapTable") = ds
        Return ds
    End Function

    Public Sub buildTree(Optional ByVal CheckedNode As String = "")
        Dim ds As DataSet = Session(SiteMapTable)
        If ds Is Nothing Then
            ds = GetDs()
        End If


        If ds Is Nothing And Me.ddlSites.SelectedIndex = 0 Then
            Me.lblMessage_Add.Text = "系統訊息：請選擇一個網站！"
            Exit Sub
        End If

        If Not ds.Relations.Contains("NodeRelation") Then
            ds.Relations.Add("NodeRelation", ds.Tables(0).Columns("NodeId"), ds.Tables(0).Columns("ParentNodeId"))
        End If

        Me.TreeView1.Nodes.Clear()

        For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
            Dim dbRow As System.Data.DataRow = ds.Tables(0).Rows(i)
            If (dbRow.IsNull("ParentNodeId")) Then
                Dim newNode As ComponentArt.Web.UI.TreeViewNode
                newNode = CreateNode(dbRow("Text").ToString(), dbRow("ImageUrl").ToString(), True, dbRow("NodeID").ToString(), dbRow("NavigateUrl").ToString)
                TreeView1.Nodes.Add(newNode)
                PopulateSubTree(dbRow, newNode)
            End If
        Next
    End Sub

    Private Sub PopulateSubTree(ByVal dbRow As System.Data.DataRow, ByVal node As ComponentArt.Web.UI.TreeViewNode)

        For i As Integer = 0 To dbRow.GetChildRows("NodeRelation").Count - 1
            Dim childRow As System.Data.DataRow = dbRow.GetChildRows("NodeRelation")(i)
            Dim childNode As ComponentArt.Web.UI.TreeViewNode = CreateNode(childRow("Text").ToString(), childRow("ImageUrl").ToString(), False, childRow("NodeID").ToString(), childRow("NavigateUrl").ToString)
            node.Nodes.Add(childNode)
            PopulateSubTree(childRow, childNode)
        Next
    End Sub

    '----------------------------------------------------------------------------------------------------
    '以下CreateNode Function視需要，傳入屬性
    '----------------------------------------------------------------------------------------------------
    Private Function CreateNode(ByVal text As String, ByVal imageurl As String, ByVal expanded As Boolean, ByVal NodeID As String, ByVal NavigateUrl As String) As ComponentArt.Web.UI.TreeViewNode
        Dim node As New ComponentArt.Web.UI.TreeViewNode
        node.Text = text
        node.ImageUrl = imageurl
        node.Expanded = expanded
        node.AutoPostBackOnCheckChanged = False

        node.Value = NodeID
        node.IsMultipleSelected = False
        node.ShowCheckBox = False


        Return node
    End Function



#End Region

#Region " 選單編輯區域 "

    ''' <summary>
    ''' 保留移動以及更名的編號
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub KeepMoveRaname()
        Dim MoveNodeString As String = CType(Request.Form("__NodeMove"), String)
        Dim ReNameNodeString As String = CType(Request.Form("__NodeReName"), String)

        Dim i As Integer

        If MoveNodeString.Length > 0 Then
            MoveNodeString = MoveNodeString.Substring(0, MoveNodeString.Length - 1)
            Dim MovedNodeArray() As String = Split(MoveNodeString, ",")

            For i = 0 To UBound(MovedNodeArray) Step 2
                'Dim a() As String = Split(MovedNodeArray(i), ",")
                Session("MoveNodeIDs") += MovedNodeArray(i) & ","
                Session("MoveNodePNodeIDs") += MovedNodeArray(i + 1) & ","
            Next
        End If

        Dim ReNameNodeArray() As String
        If ReNameNodeString.Length > 0 Then
            ReNameNodeString = ReNameNodeString.Substring(0, ReNameNodeString.Length - 1)
            ReNameNodeArray = Split(ReNameNodeString, ",")

            For i = 0 To UBound(ReNameNodeArray) Step 2
                'Dim b() As String = Split(ReNameNodeArray(i), ",")
                Session("ReNameNodeIDs") += ReNameNodeArray(i) & ","
                Session("ReNameNodeValues") += ReNameNodeArray(i + 1) & ","
            Next
        End If
    End Sub

    Private Sub RadioButtonList1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButtonList1.SelectedIndexChanged
        ModeChange()
    End Sub


    Private Sub ModeChange()
        Select Case RadioButtonList1.SelectedValue
            Case 1
                '//ReName and Move
                Me.TreeView1.AutoPostBackOnSelect = False
                Me.TreeView1.DragAndDropEnabled = True
                Me.TreeView1.NodeEditingEnabled = True
                Me.TreeView1.DropSiblingEnabled = True
                Me.TreeView1.ClientSideOnNodeRename = "nodeRename"
                Me.TreeView1.ClientSideOnNodeMove = "nodeMove"
                Me.TreeView1.KeyboardEnabled = True
                Me.TreeView1.DropRootEnabled = True

            Case 2
                Me.TreeView1.AutoPostBackOnSelect = True
                Me.TreeView1.DragAndDropEnabled = False
                Me.TreeView1.NodeEditingEnabled = False
            Case 3
                Me.TreeView1.AutoPostBackOnSelect = True
                Me.TreeView1.DragAndDropEnabled = False
                Me.TreeView1.NodeEditingEnabled = False
        End Select
    End Sub

    ''' <summary>
    ''' 當TreeView Node被點選時執行新增、或刪除功能
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub TreeView1_NodeSelected(ByVal sender As Object, ByVal e As ComponentArt.Web.UI.TreeViewNodeEventArgs) Handles TreeView1.NodeSelected
        'Me.Literal4.Text = e.Node.ParentNode.Value
        Select Case RadioButtonList1.SelectedValue
            Case "2"
                If Session("AddNodeID") Is Nothing Then
                    Session("AddNodeID") = 99999
                Else
                    Session("AddNodeID") = CInt(Session("AddNodeID")) + 1
                End If

                Dim newNode As New ComponentArt.Web.UI.TreeViewNode
                newNode.Text = "Copy of " & e.Node.Text
                newNode.ImageUrl = e.Node.ImageUrl
                newNode.Value = Session("AddNodeID")
                e.Node.ParentNode.Nodes.Add(newNode)
                Session("AddNodeIDs") += Session("AddNodeID") & ","
                Session("AddNodePNodeIDs") += e.Node.ParentNode.Value & ","
                Session("AddNodeSourceIDs") += e.Node.Value & ","
                'Me.Literal4.Text += "<br>Added <b>" & newNode.Text & "</b>. And New NodeID:" & newNode.Value

            Case "3"
                e.Node.ParentNode.Nodes.Remove(e.Node)
                Session("RemoveNodeIDs") += e.Node.Value & ","

                'Me.Literal4.Text = "Removed <b>" & e.Node.Text & "</b>."
            Case Else

        End Select
    End Sub

    ''' <summary>
    ''' 按下存檔後由虛擬按鈕呼叫本函式
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Sub BtnVirturl_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'test
        'WriteLog("test", "BtnVirturl_Click")
        Session("MaxNodeID") = GetMaxNodeID()
        Dim i As Integer

        '*//按下存檔的動作順序?
        '*--1.新增：
        '/////條件：依Session("AddNodeIDs")有沒有記截資料
        '/////動作：複製一份被複製者的Node的所有屬性，NodeID先以9999來起算
        '*--2.刪除：
        '/////條件：依Session("RemoveNodeIDs")有沒有記截資料
        '/////動作：複製一份被複製者的Node的所有屬性，NodeID先以9999來起算

        '//新增
        If Not Session("AddNodeIDs") Is Nothing Then
            Dim AddNodeSourceIDs As String = CType(Session("AddNodeSourceIDs"), String).Substring(0, CType(Session("AddNodeSourceIDs"), String).Length - 1)
            Dim AddNodeIDs As String = CType(Session("AddNodeIDs"), String).Substring(0, CType(Session("AddNodeIDs"), String).Length - 1)
            Dim AddNodePNodeIDs As String = CType(Session("AddNodePNodeIDs"), String).Substring(0, CType(Session("AddNodePNodeIDs"), String).Length - 1)

            Dim Add_NodeSourceIDArray() As String = Split(AddNodeSourceIDs, ",")
            Dim Add_NodeArray() As String = Split(AddNodeIDs, ",")
            Dim Add_NodePNodeArray() As String = Split(AddNodePNodeIDs, ",")

            For i = 0 To UBound(Add_NodeArray)
                Using dr As SqlDataReader = ClassDB.GetDataReader("SELECT * FROM " & SiteMapTable & " WHERE NodeID='" & Convert.ToInt32(Add_NodeSourceIDArray(i)) & "'")
                    Select Case SiteMapTable
                        Case "SiteMap"                       '(前台樹狀選單)
                            Try
                                If dr.Read() Then
                                    Dim retVal As Integer
                                    '2010/0826 修改 sitemap error 
                                    '從型別() 'DBNull' 至型別 'Integer' 的轉換是無效的。Microsoft.VisualBasic   於 Microsoft.VisualBasic.CompilerServices.Conversions.ToInteger(Object Value)
                                    '於 ClassDB.RunSPReturnInteger(String strSP, SqlParameter[] commandParameters)@retVal-->0
                                    retVal = ClassDB.RunSPReturnInteger("SiteMap_Node_Add",
                                    New SqlParameter("@NodeID", Convert.ToInt32(Add_NodeArray(i))),
                                    New SqlParameter("@ParentNodeId", Convert.ToInt32(Add_NodePNodeArray(i))),
                                    New SqlParameter("@Text", "CopyOf" & (dr.Item("Text"))),
                                    New SqlParameter("@NodeOrder", "0"),
                                    New SqlParameter("@AdminModule", IIf(IsDBNull(dr.Item("AdminModule")), "1", dr.Item("AdminModule"))),
                                    New SqlParameter("@IsDefaultNavigateUrl", dr.Item("IsDefaultNavigateUrl")),
                                    New SqlParameter("@NavigateUrl", dr.Item("NavigateUrl")),
                                    New SqlParameter("@Target", dr.Item("Target")),
                                    New SqlParameter("@CssFile", IIf(IsDBNull(dr.Item("CssFile")), "", dr.Item("CssFile"))),
                                    New SqlParameter("@RefPath", dr.Item("RefPath")),
                                    New SqlParameter("@NodeLevel", IIf(IsDBNull(dr.Item("NodeLevel")), 0, dr.Item("NodeLevel"))),
                                    New SqlParameter("@HaveChildNode", dr.Item("HaveChildNode")),
                                    New SqlParameter("@ReviseSET", IIf(IsDBNull(dr.Item("ReviseSET")), 0, dr.Item("ReviseSET"))),
                                    New SqlParameter("@GroupID", IIf(IsDBNull(dr.Item("GroupID")), 0, dr.Item("GroupID"))),
                                    New SqlParameter("@SiteID", IIf(IsDBNull(dr.Item("SiteID")), 1, dr.Item("SiteID"))),
                                    New SqlParameter("@ImageUrl", dr.Item("ImageUrl")),
                                    New SqlParameter("@ShowCheckBox", dr.Item("ShowCheckBox")),
                                    New SqlParameter("@ModuleID", IIf(IsDBNull(dr.Item("ModuleID")), 0, dr.Item("ModuleID"))),
                                    New SqlParameter("@HomeList", "0"))
                                End If

                            Catch ex As Exception
                                WriteErrLog(ex, Me)
                            Finally

                            End Try


                        Case "AdminMenu"                         '(後台選單)
                            Try
                                If dr.Read() Then
                                    Dim sql As String = "INSERT INTO " & SiteMapTable & " (NodeID,ParentNodeId,Text,NodeOrder,AdminPermissions,SuperAdmin,LoginType,NavigateUrl,UserNavigateUrl,ImageUrl)"
                                    sql += " VALUES ('" & Convert.ToInt32(Add_NodeArray(i)) & "','" & Convert.ToInt32(Add_NodePNodeArray(i)) & "','" & "CopyOf" & (dr.Item("Text")) & "',0,'" & dr.Item("AdminPermissions") & "','"
                                    sql += IIf(CType(dr.Item("SuperAdmin"), Boolean) = True, "1", "0") & "',1,'','','" & dr.Item("Imageurl") & "')"
                                    ClassDB.UpdateDB(sql)
                                End If
                            Catch ex As Exception
                                WriteErrLog(ex, Me)
                            Finally
                            End Try
                    End Select
                End Using
            Next
        End If



        '//刪除
        'If Not Session("RemoveNodeIDs") Is Nothing Then
        '    Dim RemoveNodeIDs As String = CType(Session("RemoveNodeIDs"), String).Substring(0, CType(Session("RemoveNodeIDs"), String).Length - 1)
        '    Dim RemoveNodeIDArray() As String = Split(RemoveNodeIDs, ",")
        '    For i = 0 To UBound(RemoveNodeIDArray)
        '        ClassDb.UpdateDB("DELETE " & SiteMapTable & " WHERE NodeID='" & RemoveNodeIDArray(i) & "'")       '刪除Node資料表該節點資料
        '        ClassDb.UpdateDB("DELETE " & SiteMapTable & " WHERE ParentNodeID='" & RemoveNodeIDArray(i) & "'")   '刪除子單元
        '        ClassDb.UpdateDB("DELETE Accounts_Permissions WHERE NodeID='" & RemoveNodeIDArray(i) & "' AND Category=" & TreeCategory)
        '    Next
        'End If

        If Not Session("RemoveNodeIDs") Is Nothing Then
            Dim RemoveNodeIDs As String = CType(Session("RemoveNodeIDs"), String).Substring(0, CType(Session("RemoveNodeIDs"), String).Length - 1)
            RemoveNodeIDs = ProcessNodeIDs(RemoveNodeIDs)
            Dim RemoveNodeIDArray() As String = Split(RemoveNodeIDs, ",")
            Dim C As Integer = 0
            Using dr As SqlDataReader = ClassDB.GetDataReaderParam("select Count(NodeID) as c from SiteMap where SiteID=@SiteID and Not ParentNodeId is null", New SqlParameter("@SiteID", Session("SiteID")))
                Try
                    If dr IsNot Nothing Then
                        If dr.Read Then
                            C = dr("C").ToString
                        End If
                    End If
                Catch ex As Exception
                End Try
            End Using
            If C <> UBound(RemoveNodeIDArray) + 1 Then
                For i = 0 To UBound(RemoveNodeIDArray)
                    ClassDB.UpdateDB("DELETE " & SiteMapTable & " WHERE NodeID='" & Convert.ToInt32(RemoveNodeIDArray(i)) & "'")       '刪除Node資料表該節點資料
                    ClassDB.UpdateDB("DELETE " & SiteMapTable & " WHERE ParentNodeID='" & Convert.ToInt32(RemoveNodeIDArray(i)) & "'")   '刪除子單元
                    ClassDB.UpdateDB("DELETE Accounts_Permissions WHERE NodeID='" & Convert.ToInt32(RemoveNodeIDArray(i)) & "' AND Category=" & TreeCategory)
                Next
            Else
                Session("DelError2") = "不能刪除所有的節點"
            End If
        End If

        '//移動(在權限控管是否會有一個問題是節點被移到新的父ID,該父ID沒有被勾選，
        If Not Session("MoveNodeIDs") Is Nothing Then
            Dim MoveNodeIDs As String = CType(Session("MoveNodeIDs"), String).Substring(0, CType(Session("MoveNodeIDs"), String).Length - 1)
            Dim MoveNodePNodeIDs As String = CType(Session("MoveNodePNodeIDs"), String).Substring(0, CType(Session("MoveNodePNodeIDs"), String).Length - 1)
            Dim MoveNodeArray() As String = Split(MoveNodeIDs, ",")
            Dim MovePNodeArray() As String = Split(MoveNodePNodeIDs, ",")
            For i = 0 To UBound(MoveNodeArray)
                ClassDB.UpdateDB("UPDATE " & SiteMapTable & " SET ParentNodeId='" & Convert.ToInt32(MovePNodeArray(i)) & "' WHERE NodeID='" & Convert.ToInt32(MoveNodeArray(i)) & "'")
            Next
        End If

        '//更名
        If Not Session("ReNameNodeIDs") Is Nothing Then
            Dim ReNameNodeIDs As String = CType(Session("ReNameNodeIDs"), String).Substring(0, CType(Session("ReNameNodeIDs"), String).Length - 1)
            Dim ReNameNodeValues As String = CType(Session("ReNameNodeValues"), String).Substring(0, CType(Session("ReNameNodeValues"), String).Length - 1)
            Dim ReNameNodeIDArray() As String = Split(ReNameNodeIDs, ",")
            Dim ReNameNodeValuesArray() As String = Split(ReNameNodeValues, ",")

            Try
                For i = 0 To UBound(ReNameNodeIDArray)
                    ClassDB.UpdateDB("UPDATE " & SiteMapTable & " SET Text=N'" & ReNameNodeValuesArray(i) & "' WHERE NodeID='" & Convert.ToInt32(ReNameNodeIDArray(i)) & "'")
                Next
            Catch ex As Exception
                WriteErrLog(ex, Me)
            End Try

        End If

        '//Update Order 排序
        If Not Session("AddNodeIDs") Is Nothing Then
            Dim AddNodePNodeIDs As String = CType(Session("AddNodePNodeIDs"), String).Substring(0, CType(Session("AddNodePNodeIDs"), String).Length - 1)
            Dim AddNodeIDs As String = CType(Session("AddNodeIDs"), String).Substring(0, CType(Session("AddNodeIDs"), String).Length - 1)
            Dim Add_NodeArray() As String = Split(AddNodeIDs, ",")
            Dim Add_NodePNodeArray() As String = Split(AddNodePNodeIDs, ",")
            Try
                For i = 0 To UBound(Add_NodePNodeArray)
                    UpdateNodeOrder(Add_NodePNodeArray(i))
                Next
            Catch ex As Exception
                WriteErrLog(ex, Me)
            End Try

        End If

        '//Update Order 排序
        If Not Session("MoveNodeIDs") Is Nothing Then
            Dim MoveNodePNodeIDs As String = CType(Session("MoveNodePNodeIDs"), String).Substring(0, CType(Session("MoveNodePNodeIDs"), String).Length - 1)
            Dim MovePNodeArray() As String = Split(MoveNodePNodeIDs, ",")

            Try
                For i = 0 To UBound(MovePNodeArray)
                    UpdateNodeOrder(MovePNodeArray(i))
                Next
            Catch ex As Exception
                WriteErrLog(ex, Me)
            End Try

        End If

        '//重新把9999以上的NodeID變更NodeID
        If Not Session("AddNodeIDs") Is Nothing Then
            Dim AddNodeIDs As String = CType(Session("AddNodeIDs"), String).Substring(0, CType(Session("AddNodeIDs"), String).Length - 1)
            Dim Add_NodeArray() As String = Split(AddNodeIDs, ",")

            Try
                For i = 0 To UBound(Add_NodeArray)
                    ClassDB.UpdateDB("UPDATE " & SiteMapTable & " SET NodeID='" & CInt(Session("MaxNodeID")) + (i + 1) * 2 & "' WHERE NodeID='" & Convert.ToInt32(Add_NodeArray(i)) & "'")
                    ClassDB.UpdateDB("UPDATE " & SiteMapTable & " SET ParentNodeID='" & CInt(Session("MaxNodeID")) + (i + 1) * 2 & "' WHERE ParentNodeID='" & Convert.ToInt32(Add_NodeArray(i)) & "'")
                Next
            Catch ex As Exception
                WriteErrLog(ex, Me)
            End Try

        End If

        '重算層數、是否有子節點、節點小圖
        doBuilTreeDetailInfo()

        '重算全部節點排序
        ReBuildNodeOrder()

        ClearReload()
        Session("R1") = RadioButtonList1.SelectedValue
        Session("ddlSites") = ddlSites.SelectedValue
    End Sub
#Region "處理過的NodeIDs"
    Function ProcessNodeIDs(ByVal NodeIDs As String) As String
        Dim ArrNodeID() As String = Split(NodeIDs, ",")
        Dim R As String = ""
        Dim DelError As String = ""
        Dim i As Integer = 0
        For i = 0 To UBound(ArrNodeID)
            Using dr As SqlDataReader = ClassDB.GetDataReaderParam("select NodeID from SiteMap where ParentNodeID=@ParentNodeID", New SqlParameter("@ParentNodeID", ArrNodeID(i))) ' & ArrNodeID(i) & "'")
                Try
                    If dr IsNot Nothing Then
                        If dr.Read Then
                            If DelError = "" Then
                                DelError = GetText(ArrNodeID(i), NodeIDs)
                            Else
                                DelError = DelError & "、" & GetText(ArrNodeID(i), NodeIDs)
                            End If
                            If GetText(ArrNodeID(i), NodeIDs) = "" Then
                                If R = "" Then
                                    R = ArrNodeID(i)
                                Else
                                    R = R & "," & ArrNodeID(i)
                                End If
                            End If
                        Else
                            If R = "" Then
                                R = ArrNodeID(i)
                            Else
                                R = R & "," & ArrNodeID(i)
                            End If
                        End If
                    End If
                Catch ex As Exception
                End Try
            End Using
        Next
        If DelError <> "" Then
            Session("DelError") = DelError
        End If
        Return R
    End Function
    Function GetText(ByVal NodeID As Integer, ByVal NodeIDs As String) As String
        Dim R As String
        Using dr As SqlDataReader = ClassDB.GetDataReaderParam("select NodeID,Text from SiteMap where ParentNodeId=@ParentNodeID", New SqlParameter("ParentNodeID", NodeID)) ''" & NodeID & "'")
            If dr IsNot Nothing Then
                While dr.Read
                    If ChkData(dr("NodeID").ToString, NodeIDs) = False Then
                        R = dr("Text").ToString
                        Exit While
                    End If
                End While
            End If
        End Using
        Return R
    End Function
    Function ChkData(ByVal NodeID As Integer, ByVal NodeIDs As String) As Boolean
        Dim Chk As Boolean = False
        Dim ArrNodeIDs() As String = NodeIDs.Split(",")
        Dim i As Integer = 0
        For i = 0 To UBound(ArrNodeIDs)
            If NodeID = ArrNodeIDs(i) Then
                Chk = True
                Exit For
            End If
        Next
        Return Chk
    End Function
#End Region

    Private Sub ClearReload()
        '//更新完成重新導向本頁，重新讀取資料庫值
        Session("AddNodeIDs") = Nothing
        Session("AddNodePNodeIDs") = Nothing
        Session("AddNodeSourceIDs") = Nothing
        Session("RemoveNodeIDs") = Nothing
        Session("MoveNodeIDs") = Nothing
        Session("MoveNodePNodeIDs") = Nothing
        Session("ReNameNodeIDs") = Nothing
        Session("ReNameNodeValues") = Nothing
        Session("MaxNodeID") = Nothing

        Session(SiteMapTable) = Nothing
        If TreeCategory = 3 Then
            Cache.Remove("SiteMap")
        End If

        'Response.Redirect(Request.Path)
        Me.TreeView1.Nodes.Clear()
        buildTree()
    End Sub

    Private Function GetMaxNodeID() As Integer

        Dim MaxNodeID As Integer
        Using dr As SqlDataReader = ClassDB.GetDataReader("SELECT TOP 1 NodeID FROM " & SiteMapTable & " ORDER BY NodeID DESC")

            Try
                If dr IsNot Nothing Then
                    If dr.Read() Then
                        MaxNodeID = dr.Item("NodeID")
                    End If
                End If
            Catch ex As Exception
                WriteErrLog(ex, Me)
            Finally
            End Try
        End Using
        Return MaxNodeID
    End Function

    Private Sub UpdateNodeOrder(ByVal PNodeID As Integer)
        Dim AllNodeListString As String = CType(Request.Form("__NodeIndex"), String)
        Dim AllNodeListArray() As String = Split(AllNodeListString, ",")

        Dim pos As Integer = 0

        Using ds As DataSet = New DataSet
            Using dt As DataTable = New DataTable
                ds.Tables.Add(dt)

                Dim myColumn As DataColumn = New DataColumn
                myColumn.ColumnName = "Position"
                myColumn.DataType = System.Type.GetType("System.Int32")
                ds.Tables(0).Columns.Add(myColumn)

                Dim myColumn2 As DataColumn = New DataColumn
                myColumn2.ColumnName = "NodeID"
                ds.Tables(0).Columns.Add(myColumn2)
                Using dr As SqlDataReader = ClassDB.GetDataReader("SELECT NodeID FROM " & SiteMapTable & " WHERE ParentNodeId='" & PNodeID & "' ORDER BY NodeOrder, NodeID")
                    Try
                        While dr.Read
                            Dim nRow As DataRow = ds.Tables(0).NewRow
                            nRow("Position") = Array.IndexOf(AllNodeListArray, CType(dr.Item("NodeID"), String))
                            nRow("NodeID") = dr.Item("NodeID")
                            ds.Tables(0).Rows.Add(nRow)
                        End While
                    Catch ex As Exception
                        WriteErrLog(ex, Me)
                    Finally
                    End Try
                End Using
                Dim SelectRow() As DataRow
                SelectRow = ds.Tables(0).Select("Position > 0", "Position ASC")

                Try
                    For i As Integer = 0 To SelectRow.Count - 1
                        Dim eRow As DataRow = SelectRow(i)
                        pos += 1
                        ClassDB.UpdateDB("UPDATE " & SiteMapTable & " SET NodeOrder='" & pos * 2 & "' WHERE NodeID='" & Convert.ToInt32(eRow("NodeID")) & "'")
                    Next
                Catch ex As Exception
                    WriteErrLog(ex, Me)
                End Try
            End Using
        End Using
        'End If
    End Sub

    Private Sub btn_clearreload_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_clearreload.Click
        ClearReload()
    End Sub

#End Region


#Region " 選單建構存檔完成後,其他數值計算 "

    ''' <summary>
    ''' 重算節點的層數、是否有子節點、變換節點的小圖
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub doBuilTreeDetailInfo()

        Using ds As DataSet = ClassDB.RunReturnDataSet("SELECT * FROM " & SiteMapTable & " ORDER BY NodeOrder, NodeId", SiteMapTable)

            If Not ds.Relations.Contains("NodeRelation") Then
                ds.Relations.Add("NodeRelation", ds.Tables(0).Columns("NodeId"), ds.Tables(0).Columns("ParentNodeId"))
            End If


            For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                Dim drow As DataRow = ds.Tables(0).Rows(i)
                Dim imageurl As String = ""
                Dim Level As Integer = GetLevel(drow, 0)
                Dim HaveChildNode As Integer = GetHaveChildRow(drow)
                Dim NodeID As String = drow("NodeID")
                If Not IsNumeric(NodeID) Then
                    NodeID = "0"
                End If
                Select Case Level
                    Case 0
                        imageurl = "root.gif"
                    Case 1
                        imageurl = "folders.gif"
                    Case 2
                        imageurl = "journal.gif"
                    Case 3
                        imageurl = "notes.gif"
                    Case 4
                        imageurl = "notes.gif"
                    Case 5
                        imageurl = "notes.gif"
                    Case 6
                        imageurl = "notes.gif"
                End Select

                Try
                    ClassDB.UpdateDB("UPDATE " & SiteMapTable & " SET HaveChildNode='" & HaveChildNode & "', NodeLevel='" & Level & "', imageurl='" & imageurl & "' WHERE NodeID='" & Convert.ToInt32(NodeID) & "'")
                Catch ex As Exception
                    WriteErrLog(ex, Me)
                End Try
            Next
        End Using
    End Sub

    ''' <summary>
    ''' 副程式：取得是否有子節點
    ''' </summary>
    ''' <param name="drow"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetHaveChildRow(ByVal drow As DataRow) As Integer

        Dim i As Integer = drow.GetChildRows("NodeRelation").Count
        'For j As Integer = 0 To drow.GetChildRows("NodeRelation").Count - 1
        '    Dim childRow As System.Data.DataRow = drow.GetChildRows("NodeRelation")(j)
        '    i += 1
        'Next
        If i = 0 Then
            Return 0
        Else
            Return 1
        End If
    End Function

    ''' <summary>
    ''' 副程式：取得節點往上算至根節點的層數(迴圈設計)
    ''' </summary>
    ''' <param name="drow"></param>
    ''' <param name="level"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetLevel(ByVal drow As DataRow, ByVal level As Integer) As Integer


        For i As Integer = 0 To drow.GetParentRows("NodeRelation").Count - 1
            Dim childRow As System.Data.DataRow = drow.GetParentRows("NodeRelation")(i)
            level += 1
            level = GetLevel(childRow, level)
        Next

        Return level
    End Function

#End Region

#Region " 選單建構存檔完成後,重新計算全部節點順序 "

    Private AllNodeCount As Integer = 1

    ''' <summary>
    ''' 重算所有節點的總順序
    ''' </summary>
    ''' <param name="CheckedNode"></param>
    ''' <remarks></remarks>
    Public Sub ReBuildNodeOrder(Optional ByVal CheckedNode As String = "")
        SiteMapTable = RemoveSQLInjection(SiteMapTable)
        Using ds As DataSet = ClassDB.RunReturnDataSet("SELECT * FROM " & SiteMapTable & " ORDER BY NodeOrder, NodeId", SiteMapTable)
            If Not ds.Relations.Contains("NodeRelation") Then
                ds.Relations.Add("NodeRelation", ds.Tables(0).Columns("NodeId"), ds.Tables(0).Columns("ParentNodeId"))
            End If
            For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                Dim dbRow As System.Data.DataRow = ds.Tables(0).Rows(i)
                If (dbRow.IsNull("ParentNodeId")) Then
                    Dim NodeID As String = dbRow("NodeID")
                    If Not IsNumeric("NodeID") Then
                        NodeID = "0"
                    End If
                    WriteNodeCount(dbRow("NodeID").ToString())
                    PopulateSubNode(dbRow)
                End If
            Next
        End Using
    End Sub

    ''' <summary>
    ''' 重算所有節點的總順序(迴圈設計)
    ''' </summary>
    ''' <param name="dbRow"></param>
    ''' <remarks></remarks>
    Private Sub PopulateSubNode(ByVal dbRow As System.Data.DataRow)

        For i As Integer = 0 To dbRow.GetChildRows("NodeRelation").Count - 1
            Dim childRow As System.Data.DataRow = dbRow.GetChildRows("NodeRelation")(i)
            WriteNodeCount(childRow("NodeID").ToString())
            'WriteLog("PopulateSubNode", "NodeID=" & childRow("NodeID").ToString() & " Text=" & childRow("Text").ToString())
            PopulateSubNode(childRow)
        Next
    End Sub

    ''' <summary>
    ''' 重算所有節點的總順序：寫入資料庫
    ''' </summary>
    ''' <param name="NodeID"></param>
    ''' <remarks></remarks>
    Private Sub WriteNodeCount(ByVal NodeID As String)

        ClassDB.UpdateDB("UPDATE " & SiteMapTable & " SET AllNodeOrder='" & Convert.ToInt32(AllNodeCount) & "' WHERE NodeID='" & Convert.ToInt32(NodeID) & "'")

        'WriteLog("WriteNodeCount", "NodeID=" & NodeID & "AllNodeCount=" & AllNodeCount)

        AllNodeCount += 1

    End Sub
#End Region

End Class
