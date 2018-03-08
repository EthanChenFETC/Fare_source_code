Imports System.Web.Security
Imports System.Data
Imports System.Data.SQLClient
Imports ComponentArt.Web.UI
Imports System.IO

Partial Class SystemDesign_AdminMenuBuilder
    Inherits PageBase

    Private SiteMapTable As String = "AdminMenu"
    Private TreeCategory As Integer = 1

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Dim AllNodeListString As String = CType(Request.Form("__NodeIndex"), String)
        'Me.lblMessage_Add.Text = AllNodeListString

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
            buildTree()
        End If

        If IsPostBack Then
            KeepMoveRaname()
        End If
        ModeChange()
    End Sub


#Region " 選單建構區域 "
    '====================================================================================================
    '樹狀選單區域
    '====================================================================================================

    Protected Sub ddlSites_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlSites.SelectedIndexChanged
        buildTree()
    End Sub


    Public Sub buildTree(Optional ByVal CheckedNode As String = "")

        Dim ds As DataSet
        Select Case CInt(Me.ddlSites.SelectedValue)
            Case 1
                ds = ClassDB.RunReturnDataSet("SELECT * FROM " & SiteMapTable & " WHERE NodeID <1000 ORDER BY NodeOrder, NodeId", SiteMapTable)
            Case 2
                ds = ClassDB.RunReturnDataSet("SELECT * FROM " & SiteMapTable & " WHERE NodeID >=1000 ORDER BY NodeOrder, NodeId", SiteMapTable)
            Case Else
                Me.TreeView1.Nodes.Clear()
                Exit Sub
        End Select

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
    ''' 保留[移動]以及[更名]的編號
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

    ''' <summary>
    ''' 變換編輯模式RadioButtonList
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub RadioButtonList1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButtonList1.SelectedIndexChanged
        ModeChange()
    End Sub

    ''' <summary>
    ''' 設定樹狀的編輯屬性配合不同編輯模式
    ''' </summary>
    ''' <remarks></remarks>
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
                'Add New Mode
                Me.TreeView1.AutoPostBackOnSelect = True
                Me.TreeView1.DragAndDropEnabled = False
                Me.TreeView1.NodeEditingEnabled = False

            Case 3
                'Del Mode
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
                '新增節點動作
                '--------------------------------------------------------------------------------------------------
                'Dim NewNodeID As Integer
                'If CInt(e.Node.Value) >= 1000 Then
                '    '局內網,直接給它最大的NodeID
                '    If Session("AddNodeID") Is Nothing Then
                '        Session("AddNodeID") = Me.GetMaxNodeID(True) + 1
                '    Else
                '        Session("AddNodeID") = CInt(Session("AddNodeID")) + 1
                '    End If
                'Else
                '    '全球後端管理,給它後端的最大值,但不得超過1000

                'End If

                If Session("AddNodeID") Is Nothing Then
                    Session("AddNodeID") = 9999
                Else
                    Session("AddNodeID") = CInt(Session("AddNodeID")) + 1
                End If
               
                Dim newNode As New ComponentArt.Web.UI.TreeViewNode
                newNode.Text = "Copy of " & e.Node.Text
                newNode.ImageUrl = e.Node.ImageUrl
                newNode.Value = Session("AddNodeID")
                e.Node.ParentNode.Nodes.Add(newNode)

                Session("AddNodeIDs") += Session("AddNodeID") & ","    '新增的NodeID字串,以逗號分隔
                Session("AddNodePNodeIDs") += e.Node.ParentNode.Value & ","
                Session("AddNodeSourceIDs") += e.Node.Value & ","
                'Me.Literal4.Text += "<br>Added <b>" & newNode.Text & "</b>. And New NodeID:" & newNode.Value

            Case "3"
                '刪除節點動作
                '--------------------------------------------------------------------------------------------------
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
        If Me.ddlSites.SelectedValue = 1 Then
            '後端
            Session("MaxNodeID") = GetMaxNodeID()
        Else
            '局內網
            Session("MaxNodeID") = GetMaxNodeID(True)
        End If

        Dim i As Integer


        '*//按下存檔的動作順序?
        '*--1.新增：
        '/////條件：依Session("AddNodeIDs")有沒有記截資料
        '/////動作：複製一份被複製者的Node的所有屬性，NodeID先以9999來起算
        '*--2.刪除：
        '/////條件：依Session("RemoveNodeIDs")有沒有記截資料
        '/////動作：複製一份被複製者的Node的所有屬性，NodeID先以9999來起算

        '//新增
        If Not Session("AddNodeIDs") Is Nothing Then  '存放在Session中的NewNodeIDs
            '1.存放在Session中的複製來源節點的NodeIDs
            Dim AddNodeSourceIDs As String = CType(Session("AddNodeSourceIDs"), String).Substring(0, CType(Session("AddNodeSourceIDs"), String).Length - 1)
            '2.存放在Session中的NewNodeIDs
            Dim AddNodeIDs As String = CType(Session("AddNodeIDs"), String).Substring(0, CType(Session("AddNodeIDs"), String).Length - 1)
            '3.存放在Session中的NewNodeIDs的父NodeIDs
            Dim AddNodePNodeIDs As String = CType(Session("AddNodePNodeIDs"), String).Substring(0, CType(Session("AddNodePNodeIDs"), String).Length - 1)

            '變成Array
            Dim Add_NodeSourceIDArray() As String = Split(AddNodeSourceIDs, ",")
            Dim Add_NodeArray() As String = Split(AddNodeIDs, ",")
            Dim Add_NodePNodeArray() As String = Split(AddNodePNodeIDs, ",")


            '以新增的節點量做迴圈進行插入資料庫
            For i = 0 To UBound(Add_NodeArray)
                '取得被複製來源的NodeID的資料
                If Not IsNumeric(Add_NodeSourceIDArray(i)) Then
                    Continue For
                End If
                Using dr As SqlDataReader = ClassDB.GetDataReader("SELECT * FROM " & SiteMapTable & " WHERE NodeID='" & Add_NodeSourceIDArray(i) & "'")
                    Try
                        If dr.Read Then
                            '插入資料庫,新的一個節點資料(但使用最大的NodeID值,並且文字也是用Copy of...)
                            Dim sql As String = "INSERT INTO " & SiteMapTable & " (NodeID,ParentNodeId,Text,NodeOrder,AdminPermissions,SuperAdmin,LoginType,NavigateUrl,ImageUrl)"
                            sql += " VALUES ('" & Convert.ToInt32(Add_NodeArray(i)) & "','" & Convert.ToInt32(Add_NodePNodeArray(i)) & "','" & "CopyOf" & (dr.Item("Text")) & "',0,'" & RemoveSQLInjection(dr.Item("AdminPermissions")) & "','"
                            sql += IIf(CType(dr.Item("SuperAdmin"), Boolean) = True, "1", "0") & "',1,'','" & RemoveSQLInjection(dr.Item("Imageurl")) & "')"
                            ClassDB.UpdateDB(sql)
                        End If
                    Catch ex As Exception
                        WriteErrLog(ex, Me)
                    Finally

                    End Try
                End Using
            Next
        End If



        '//刪除
        If Not Session("RemoveNodeIDs") Is Nothing Then
            Dim RemoveNodeIDs As String = CType(Session("RemoveNodeIDs"), String).Substring(0, CType(Session("RemoveNodeIDs"), String).Length - 1)
            Dim RemoveNodeIDArray() As String = Split(RemoveNodeIDs, ",")
            For i = 0 To UBound(RemoveNodeIDArray)
                ClassDB.UpdateDB("DELETE " & SiteMapTable & " WHERE NodeID='" & Convert.ToInt32(RemoveNodeIDArray(i)) & "'")       '刪除Node資料表該節點資料
                ClassDB.UpdateDB("DELETE " & SiteMapTable & " WHERE ParentNodeID='" & Convert.ToInt32(RemoveNodeIDArray(i)) & "'")   '刪除子單元
                ClassDB.UpdateDB("DELETE Accounts_Permissions WHERE NodeID='" & Convert.ToInt32(RemoveNodeIDArray(i)) & "' AND Category=" & Convert.ToInt32(TreeCategory))
            Next
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
                    ClassDB.UpdateDB("UPDATE " & SiteMapTable & " SET Text='" & (ReNameNodeValuesArray(i)) & "' WHERE NodeID='" & Convert.ToInt32(ReNameNodeIDArray(i)) & "'")
                Next
            Catch ex As Exception
                WriteErrLog(ex, Me)
            End Try

        End If

        '//Update Order 排序-新增的-(僅於此新增的節點相同父節點的範圍)
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

        '//Update Order 排序-移動的(僅於此新增的節點相同父節點的範圍)
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

    End Sub


    ''' <summary>
    ''' 更新完成重新導向本頁，重新讀取資料庫值
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ClearReload()
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
   
        Me.TreeView1.Nodes.Clear()
        Me.lblMessage_Add.Text = "系統訊息：更新完成！"
        buildTree()
    End Sub



    ''' <summary>
    ''' 取得最大的NodeID值
    ''' </summary>
    ''' <param name="IsIntranet">是否為局內網，一般取得編號1000以下,局內網則無限制</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetMaxNodeID(Optional ByVal IsIntranet As Boolean = False) As Integer
        Dim MaxNodeID As Integer
        Dim sql As String = ""

        If IsIntranet = False Then
            '一般後端
            sql = "SELECT TOP 1 NodeID FROM " & SiteMapTable & " WHERE NodeID <1000 ORDER BY NodeID DESC"
        Else
            '局內網
            sql = "SELECT TOP 1 NodeID FROM " & SiteMapTable & " ORDER BY NodeID DESC"
        End If
        Using dr As SqlDataReader = ClassDB.GetDataReader(sql)
            Try
                If dr.Read() Then
                    MaxNodeID = dr.Item("NodeID")
                End If
            Catch ex As Exception
                WriteErrLog(ex, Me)
            Finally

            End Try
        End Using
        Return MaxNodeID
    End Function

    ''' <summary>
    ''' 更新單一父節點下的所有節點順序
    ''' </summary>
    ''' <param name="PNodeID"></param>
    ''' <remarks></remarks>
    Private Sub UpdateNodeOrder(ByVal PNodeID As Integer)
        Dim AllNodeListString As String = CType(Request.Form("__NodeIndex"), String) '當頁面的VirtaulClick送出前會把整個樹狀的排序組合成一個字串
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

                End Select

                Try
                    ClassDB.UpdateDB("UPDATE " & SiteMapTable & " SET HaveChildNode='" & HaveChildNode & "', NodeLevel='" & Level & "', imageurl='" & imageurl & "' WHERE NodeID='" & NodeID & "'")
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

        Using ds As DataSet = ClassDB.RunReturnDataSet("SELECT * FROM " & SiteMapTable & " ORDER BY NodeOrder, NodeId", SiteMapTable)

            If Not ds.Relations.Contains("NodeRelation") Then
                ds.Relations.Add("NodeRelation", ds.Tables(0).Columns("NodeId"), ds.Tables(0).Columns("ParentNodeId"))
            End If


            For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                Dim dbRow As System.Data.DataRow = ds.Tables(0).Rows(i)
                If (dbRow.IsNull("ParentNodeId")) Then
                    Dim NodeID As String = dbRow("NodeID").ToString()
                    If Not IsNumeric(NodeID) Then
                        NodeID = "0"
                    End If
                    WriteNodeCount(NodeID)
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
            PopulateSubNode(childRow)
        Next
    End Sub

    ''' <summary>
    ''' 重算所有節點的總順序：寫入資料庫
    ''' </summary>
    ''' <param name="NodeID"></param>
    ''' <remarks></remarks>
    Private Sub WriteNodeCount(ByVal NodeID As String)
        If Not IsNumeric(NodeID) Then
            Exit Sub
        End If
        ClassDB.UpdateDBText("UPDATE AdminMenu SET AllNodeOrder=@AllNodeOrder WHERE NodeID=@NodeID", New SqlParameter("@AllNodeOrder", AllNodeCount), New SqlParameter("@NodeID", NodeID))

        AllNodeCount += 1

    End Sub
#End Region

    
End Class
