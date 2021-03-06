Imports System.Text
Imports System.Data
Imports System.Data.SqlClient

''' <summary>
''' 動態產生及管理JavaScript版本選單
''' </summary>
''' <remarks></remarks>
Public Module WebMenuJs

    Dim TotalMenuCount As Integer = 0
    Dim Context As HttpContext = HttpContext.Current
    Dim CurrentNodeID As Integer = Context.Request.Params("CurrentNodeID")
    Dim Level As Integer = Context.Request.Params("Level")
    Dim HomeNodeID As Integer   '本站首頁的NodeID ==> RootNodeID

    Dim Sub1 As Integer = 1
    Dim OpenSub1 As Boolean = False
    Dim Sub2 As Integer = 1
    Dim OpenSub2 As Boolean = False
    Dim Sub3 As Integer = 1
    Dim OpenSub3 As Boolean = False
    Dim Sub4 As Integer = 1
    Dim OpenSub4 As Boolean = False
    Dim Sub5 As Integer = 1


    Dim SubWord As String = "abcdefghijk"

    Private CacheMinute As Integer = 15


#Region " BildMenu "

    ''' <summary>
    ''' 取得作用中的選單各階層的NodeID(數字編號)
    ''' </summary>
    ''' <param name="GetLevel"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CetLevelNodeID(ByVal GetLevel As Integer) As Integer


        Dim ds As DataSet = Context.Cache("SiteMap")
        If ds Is Nothing Then ds = GetSiteMapds()

        If GetLevel = Level Then
            Return CurrentNodeID
            Exit Function
        End If

        Dim Output As Integer
        If Level > 1 Then

            Dim i As Integer

            Dim TempID As Integer

            '階層往上找尋各父層的NodeID
            For i = Level - 1 To 1 Step -1
                Level -= 1   '設定Level值與i相同
                For j As Integer = 0 To ds.Tables(0).Rows.Count - 1
                    Dim dRow As DataRow = ds.Tables(0).Rows(j)
                    If IsDBNull(dRow("NodeID")) Then
                        Continue For
                    End If
                    If CInt(dRow("NodeID")).Equals(CurrentNodeID) Then
                        TempID = CInt(dRow("ParentNodeID"))
                        'WriteLog("Level", Level & "-TempID" & TempID, , False)
                        If Level = GetLevel Then    '找到要索取的階層
                            Output = TempID '取得NodeID
                        End If
                    End If
                Next
                CurrentNodeID = TempID
            Next
        Else
            Output = CurrentNodeID
        End If

        Return Output

    End Function



#Region "選單資料來源計算"


    ''' <summary>
    ''' 取得以逗號分隔的NodeID字串
    ''' </summary>
    ''' <param name="GroupID">選單群組(預設左選單:值1)</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetNodeString(ByVal GroupID As Integer) As String

        '先從稍早的選單DataTable快取中取得資料
        Dim s As String = Context.Cache("NodeString" & GroupID)

        If s = "" OrElse s = Nothing OrElse s = String.Empty Then
            Dim SiteID As String = System.Configuration.ConfigurationManager.AppSettings("SiteID").ToString
            If Not IsNumeric(SiteID) Then
                SiteID = "0"
            End If
            Dim sql As String = ""    'below had be fix 2008/2/27 - new select is select only nodelevel <=3, if not do this will find the node in this string but never exit in left menu! will show js error.
            sql += "SELECT  SiteMap_Group.NodeID, SiteMap_Group.GroupID, SiteMap_Group.SiteID, SiteMap.NodeLevel "
            sql += "FROM      SiteMap_Group INNER JOIN "
            sql += "SiteMap ON SiteMap_Group.NodeID = SiteMap.NodeId "
            sql += "WHERE   (SiteMap_Group.GroupID = @GroupID) AND (SiteMap_Group.SiteID =  @SiteID) " 'AND (SiteMap.NodeLevel <= 3)"
            Using dr As SqlDataReader = ClassDB.GetDataReaderParam(sql, New SqlParameter("@SiteID", SiteID), New SqlParameter("@GroupID", GroupID))
                Try
                    If dr IsNot Nothing Then
                        While dr.Read
                            s &= ", " & CInt(dr("NodeID")).ToString
                        End While
                    End If
                Catch ex As Exception
                    WriteLog("WebMenuJs", ex.Message & vbCrLf & ex.StackTrace)
                Finally

                End Try
            End Using
            If s IsNot Nothing Then
                If s.Length > 0 Then
                    s = s.Substring(1)
                    Context.Cache.Insert("NodeString" & GroupID, s, Nothing, DateTime.Now.AddMinutes(CacheMinute), System.Web.Caching.Cache.NoSlidingExpiration)
                Else
                    Context.Cache.Remove("NodeString" & GroupID)
                End If
            Else
                Context.Cache.Remove("NodeString" & GroupID)
            End If
        End If
        Return s

    End Function


    ''' <summary>
    ''' 重新計算網站地圖資料表,屬於這個選單群組的節點的資料(包含各層,有無子節點,同父節點的數量)
    ''' </summary>
    ''' <param name="GroupID">選單群組ID</param>
    ''' <returns>DataTable物件</returns>
    ''' <remarks></remarks>
    Public Function doPhaseDataTable(ByVal GroupID As Integer) As DataTable

        '先從稍早的選單DataTable快取中取得資料
        Dim dt As DataTable = Context.Cache("MenuStringDataTable")

        If dt Is Nothing Then
            Dim MenuGroupString As String = GetNodeString(GroupID)
            If MenuGroupString Is Nothing Then
                Return dt
            End If
            MenuGroupString = "," & MenuGroupString & ", "
            'Dim dt As DataTable
            Dim ds As DataSet = Context.Cache("SiteMap")
            If ds Is Nothing Then ds = GetSiteMapds()
            dt = ds.Tables(0).Clone

            '先製作一份有效的資料列的datatable,只包含有在本群組的選單項目
            For j As Integer = 0 To ds.Tables(0).Rows.Count - 1
                Dim dRow As DataRow = ds.Tables(0).Rows(j)
                If IsDBNull(drow("NodeID")) Then
                    Continue For
                End If
                Dim dd As String = drow("HaveChildNode").ToString
                Dim ddd As String = drow("Text").ToString
                If MenuGroupString.IndexOf(", " & drow("NodeID").ToString & ", ") >= 0 Then  '只有在群組內的選單節點才加入選單的DataTable
                    Dim nrow As DataRow = dt.NewRow
                    nrow("NodeID") = drow("NodeID")
                    nrow("ParentNodeID") = drow("ParentNodeID")
                    nrow("Text") = drow("Text")
                    nrow("RefPath") = drow("RefPath")
                    nrow("NodeLevel") = drow("NodeLevel")
                    nrow("HaveChildNode") = drow("HaveChildNode")
                    nrow("PublishType") = drow("PublishType")
                    nrow("Target") = drow("Target")
                    dt.Rows.Add(nrow)
                End If
            Next

            '重新計算是否確實擁有子Node(因後台選單群組設定，可自由設定，可能沒有把子單元勾選進來)
            For j As Integer = 0 To dt.Rows.Count - 1
                Dim dRow As DataRow = dt.Rows(j)
                If IsDBNull(dRow("NodeID")) Then
                    Continue For
                End If
                'WriteLog("doPhaseDataTable", "NodeID" & drow("NodeID") & " " & drow("HaveChildNode"))
                If CType(dRow("HaveChildNode"), Boolean) = True Then
                    Dim ddddd As String = dRow("HaveChildNode").ToString
                    Dim dddddd As String = dRow("Text").ToString
                    dRow("HaveChildNode") = GetHaveChildNode(dt, dRow("NodeID").ToString)
                    Dim dddd As String = dRow("HaveChildNode").ToString
                End If
            Next

            '計算與本節點擁有共同父節點的Node數量，包含本節點自己
            dt.Columns.Add("SameParentNodeCount")
            For j As Integer = 0 To dt.Rows.Count - 1
                Dim dRow As DataRow = dt.Rows(j)
                dRow("SameParentNodeCount") = GetTheSameParentNodeCount(dt, drow("ParentNodeID").ToString)
            Next

            '計算本節點的子節點數量
            dt.Columns.Add("ChildNodeCount")
            For j As Integer = 0 To dt.Rows.Count - 1
                Dim dRow As DataRow = dt.Rows(j)
                If IsDBNull(drow("NodeID")) Then
                    Continue For
                End If
                drow("ChildNodeCount") = GetTheChildNodeCount(dt, drow("NodeID").ToString)
            Next

            '取得相同父ID的節點,並且有子節點的節點,其數量有幾個(呼叫)
            dt.Columns.Add("SameParentNodeHaveChildNodeCount")
            For j As Integer = 0 To dt.Rows.Count - 1
                Dim dRow As DataRow = dt.Rows(j)
                dRow("SameParentNodeHaveChildNodeCount") = GetTheSameParentNodeHaveChildNodeCount(dt, drow("ParentNodeID").ToString)
            Next

            '取得相同屬性,本節點順眼位(屬性：相同父ID的節點,並且有子節點的節點
            dt.Columns.Add("SameParentNodeHaveChildThisNodeOrder")
            For j As Integer = 0 To dt.Rows.Count - 1
                Dim dRow As DataRow = dt.Rows(j)
                If IsDBNull(drow("NodeID")) Then
                    Continue For
                End If
                drow("SameParentNodeHaveChildThisNodeOrder") = GetSameParentNodeHaveChildThisNodeOrder(dt, drow("ParentNodeID").ToString, drow("NodeID").ToString)
            Next

            '放入快取中60分鐘
            'Context.Cache.Insert("MenuString", MenuString, Nothing, Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(60))
            Context.Cache.Insert("MenuStringDataTable", dt, Nothing, DateTime.Now.AddMinutes(CacheMinute), System.Web.Caching.Cache.NoSlidingExpiration)
        End If

        Return dt
    End Function

    ''' <summary>
    ''' 驗證是否真有子節點(呼叫)
    ''' </summary>
    ''' <param name="dt">DataTable物件</param>
    ''' <param name="NodeID">單元節點主鍵值NodeID</param>
    ''' <returns>布林值(True/False)</returns>
    ''' <remarks></remarks>
    Private Function GetHaveChildNode(ByVal dt As DataTable, ByVal NodeID As String) As Boolean
        Dim HaveChild As Boolean = False

        For i As Integer = 0 To dt.Rows.Count - 1
            Dim d As DataRow = dt.Rows(i)
            If d("ParentNodeID").ToString = NodeID Then '找到有父ID為本NODEID的,所以確實有子單元
                HaveChild = True
                Exit For
            End If
        Next

        Return HaveChild
    End Function

    ''' <summary>
    ''' 取得相同父ID的節點有幾個(呼叫)
    ''' </summary>
    ''' <param name="dt">DataTable物件</param>
    ''' <param name="ParentNodeID">父單元節點主鍵值NodeID</param>
    ''' <returns>數量</returns>
    ''' <remarks></remarks>
    Private Function GetTheSameParentNodeCount(ByVal dt As DataTable, ByVal ParentNodeID As String) As Integer
        Dim i As Integer = 0
        For j As Integer = 0 To dt.Rows.Count - 1
            Dim d As DataRow = dt.Rows(j)

            If d("ParentNodeID").ToString = ParentNodeID Then '找到有父ID為本NODEID的,所以確實有子單元
                i += 1
            End If
        Next

        Return i
    End Function


    ''' <summary>
    ''' 計算有多少個子節點(呼叫)
    ''' </summary>
    ''' <param name="dt">DataTable物件</param>
    ''' <param name="NodeID">現在這個節點主鍵值NodeID</param>
    ''' <returns>子節點數量</returns>
    ''' <remarks></remarks>
    Private Function GetTheChildNodeCount(ByVal dt As DataTable, ByVal NodeID As String) As Integer
        Dim i As Integer = 0
        For j As Integer = 0 To dt.Rows.Count - 1
            Dim d As DataRow = dt.Rows(j)
            If d("ParentNodeID").ToString = NodeID Then '找到有父ID為本NODEID的,所以確實有子單元
                i += 1
            End If
        Next

        Return i
    End Function



    ''' <summary>
    ''' 取得相同父ID的節點,並且有子節點的節點,其數量有幾個(呼叫)
    ''' </summary>
    ''' <param name="dt">DataTable物件</param>
    ''' <param name="ParentNodeID">父單元節點主鍵值NodeID</param>
    ''' <returns>數量</returns>
    ''' <remarks></remarks>
    Private Function GetTheSameParentNodeHaveChildNodeCount(ByVal dt As DataTable, ByVal ParentNodeID As String) As Integer

        Dim i As Integer = 0
        For j As Integer = 0 To dt.Rows.Count - 1
            Dim d As DataRow = dt.Rows(j)
            If d("ParentNodeID").ToString = ParentNodeID Then '找到有父ID為本NODEID的,所以確實有子單元
                If CType(d("HaveChildNode"), Boolean) = True Then
                    i += 1
                End If
            End If
        Next

        Return i
    End Function

    ''' <summary>
    ''' 取得同屬性之節點順位(屬性：相同父ID的節點,並且有子節點的節點)
    ''' </summary>
    ''' <param name="dt">DataTable物件</param>
    ''' <param name="ParentNodeID">父單元節點主鍵值NodeID</param>
    ''' <returns>數量</returns>
    ''' <remarks></remarks>
    Private Function GetSameParentNodeHaveChildThisNodeOrder(ByVal dt As DataTable, ByVal ParentNodeID As String, ByVal thisNodeID As String) As Integer
        Dim i As Integer = 0
        For j As Integer = 0 To dt.Rows.Count - 1
            Dim d As DataRow = dt.Rows(j)
            If d("ParentNodeID").ToString = ParentNodeID Then '找到有父ID為本NODEID的,所以確實有子單元
                If CType(d("HaveChildNode"), Boolean) = True Then
                    i += 1
                    If d("NodeID").ToString.Equals(thisNodeID) Then
                        Exit For
                    End If
                End If
            End If
        Next

        Return i
    End Function

#End Region



    ''' <summary>
    ''' 取得選單字串,Javascript版
    ''' </summary>
    ''' 
    ''' <returns>選單HTML原始碼</returns>
    ''' <remarks></remarks>
    Public Function BuidMenu() As String
        'Public Function BuidMenu(Optional ByVal GroupID As Integer = 1) As String
        Dim GroupID As Integer = CInt(System.Configuration.ConfigurationManager.AppSettings("MainMenuGroupID").ToString)
        'doPhaseDataTable(GroupID)

        '先從稍早的選單快取中取得資料
        Dim MenuString As String = Context.Cache("MenuString")



        If MenuString Is Nothing Or MenuString = "" Then
            Dim path As String = System.Web.HttpContext.Current.Request.ApplicationPath
            If Not path.EndsWith("/") Then
                path += "/"
            End If
            '快取中無資料時重新計算

            If System.Configuration.ConfigurationManager.AppSettings("CacheMinute") IsNot Nothing Then
                CacheMinute = CInt(System.Configuration.ConfigurationManager.AppSettings("CacheMinute").ToString)
            End If

            Dim dt As DataTable = doPhaseDataTable(GroupID)
            If dt Is Nothing Then
                Context.Cache.Remove("MenuString")
                Return ""
            End If
            TotalMenuCount = 0

            Dim iaccesskey As Integer = 0

            '追蹤第1層選單有多少個,在傳入Javascript時可以給予適當回圈
            Dim level1Count As Integer = 0

            Dim sb As StringBuilder = New StringBuilder



            'HomeNodeID本站首頁的NodeID ==> RootNodeID
            For i As Integer = 0 To dt.Rows.Count - 1
                Dim dRow As DataRow = dt.Rows(i)
                If IsDBNull(dRow("NodeID")) Then
                    Continue For
                End If
                If (dRow.IsNull("ParentNodeID")) Then
                    HomeNodeID = CInt(dRow("NodeID").ToString)
                    Exit For
                End If
            Next

            Sub1 = 1
            Sub2 = 1
            Sub3 = 1
            OpenSub1 = False
            OpenSub2 = False
            OpenSub3 = False
            Try

                sb.Append(" <ul>" & vbCrLf)
                For i As Integer = 0 To dt.Rows.Count - 1
                    Dim dRow As DataRow = dt.Rows(i)
                    If IsDBNull(dRow("NodeID")) Then
                        Continue For
                    End If
                    '有在本選單群組的項目

                    Dim NodeText As String = Trim(dRow("Text").ToString)
                    '20100818 Chris 新增替代說明
                    'Dim NodeText_Title As String = IIf(IsDBNull(dRow("TextEn")), Trim(dRow("Text").ToString), Trim(dRow("TextEn").ToString)) 'NodeText
                    Dim NodeText_Title As String = IIf(IsDBNull(dRow("TextEn")), Trim(dRow("Text").ToString), IIf(Trim(dRow("TextEn").ToString).Equals(""), Trim(dRow("Text").ToString), Trim(dRow("TextEn").ToString))) 'NodeText
                    If NodeText.Length > 23 Then NodeText = NodeText.Substring(0, 23) & "..."

                    Dim RefPath As String = Trim(dRow("RefPath").ToString)
                    Dim NodePath As String = RefPath
                    'add by Chris 20090211 for URL連結+URL內容帶有http://或https://等字串的時候(或者應該是://代表外連某個網路的應用程式
                    '如果系統設計設定為URL連結加上http://, https://, mms://, mailto:等等都可以使用
                    'If dRow("PublishType").ToString = "2" And (RefPath.IndexOf("http://".ToLower) = 0 Or RefPath.IndexOf("https://".ToLower) = 0 Or RefPath.IndexOf("mailto:".ToLower) = 0) Then
                    '其實如果是多上向稿也無所謂, 只要檢查是不是URL裡面有帶完整URL就好了呢
                    If dRow("PublishType").ToString = "2" And (RefPath.IndexOf("http://".ToLower) = 0 Or RefPath.IndexOf("https://".ToLower) = 0 Or RefPath.IndexOf("mailto:".ToLower) = 0) Then

                    Else
                        RefPath = RefPath & "?cnid=" & dRow("NodeID").ToString
                        NodePath = Nothing
                    End If
                    '====
                    Dim Target As String = ""
                    '強迫另開視窗為URL連結而非另開視窗, 這樣子其實有爭議, 因為多向上稿就不能另開視窗, 或許也是對的
                    '因為多向上稿另開視窗以後, 還是會有完整的網頁(包含左邊選單)這樣子會一直另開視窗, 很無聊
                    '但是如果有人就是這麼無聊要這樣設定呢?
                    '那就可以把下面那一行改成
                    'If dRow("Target").ToString = ("_blank")  Then
                    If dRow("Target").ToString = ("_blank") And dRow("PublishType").ToString = "2" Then
                        Target = " Target=""_blank"" "
                    End If
                    Select Case CInt(dRow("NodeLevel").ToString)
                        Case 1
                            '--------------------------------------------------------------------------------------------
                            Sub2 = 1    'Reset to 0
                            If OpenSub2 = True Then
                                sb.Append(" </ul>" & vbCrLf)
                                OpenSub2 = False
                            End If

                            If OpenSub1 = True Then
                                sb.Append("</li>" & vbCrLf & vbCrLf)
                                OpenSub1 = False
                            End If
                            Dim sbstr As StringBuilder = New StringBuilder
                            If NodePath Is Nothing Then
                                sbstr.Append("<li><a id=""TopMenuNode" & dRow("NodeID").ToString & """ title=""" & NodeText_Title & """ href=""" & RefPath & """" & Target & " >")
                            Else
                                sbstr.Append("<li><a id=""TopMenuNode" & dRow("NodeID").ToString & """ title=""" & NodeText_Title & """ href=""" & NodePath & """" & Target & " >")
                            End If
                            If CType(dRow("HaveChildNode"), Boolean) = True Then
                                sbstr = New StringBuilder
                                sbstr.Append("<li class=""active has-sub""><a id=""TopMenuNode" & dRow("NodeID").ToString & """>")
                            End If
                            sb.Append(sbstr)
                            sb.Append(NodeText)
                            sb.Append("</a>")

                            If CType(dRow("HaveChildNode").ToString(), Boolean) = True Then
                                '有子單元->使用JS
                                level1Count += 1
                                OpenSub1 = True
                                Sub1 += 1

                            Else
                                sb.Append("</li>" & vbCrLf)
                                OpenSub1 = False
                                OpenSub2 = False
                                '==================
                            End If


                        Case 2
                            '--------------------------------------------------------------------------------------------
                            If OpenSub2 = False Then
                                sb.Append("<ul>" & vbCrLf)
                            End If
                            OpenSub2 = True
                            sb.Append(" <li>")
                            Dim sbstr As StringBuilder = New StringBuilder
                            If NodePath Is Nothing Then
                                sbstr.Append("<a id=""TopMenuNode" & dRow("NodeID").ToString & """ title=""" & NodeText_Title & """ href=""" & RefPath & """" & Target & " >")
                            Else
                                sbstr.Append("<a id=""TopMenuNode" & dRow("NodeID").ToString & """ title=""" & NodeText_Title & """ href=""" & NodePath & """" & Target & " >")
                            End If
                            Dim dd As String = dRow("HaveChildNode").ToString
                            If CType(dRow("HaveChildNode"), Boolean) = True Then
                                sbstr = New StringBuilder
                                sbstr.Append("<a id=""TopMenuNode" & dRow("NodeID").ToString & """ title=""" & NodeText_Title & """ href=""" & RefPath & """ " & Target & " >")
                            End If
                            sb.Append(sbstr)

                            sb.Append(NodeText)
                            sb.Append("</a>")
                            sb.Append("</li>" & vbCrLf)
                    End Select

                Next

                '回圈完了如果有未關閉的div
                If OpenSub2 = True Then
                    sb.Append(" </ul>" & vbCrLf)
                    OpenSub2 = False
                End If

                If OpenSub1 = True Then
                    sb.Append("</li>" & vbCrLf & vbCrLf)
                    OpenSub1 = False
                End If

            Catch ex As Exception
                ModuleWriteLog.WriteLog("WebMenuJs.vb", ex.Message & ex.Source & ex.StackTrace)
            End Try

            sb.Append("</ul>" & vbCrLf & vbCrLf)
            MenuString = sb.ToString
            Context.Cache.Insert("MenuString", MenuString, Nothing, DateTime.Now.AddMinutes(CacheMinute), System.Web.Caching.Cache.NoSlidingExpiration)

        End If
        
        Return MenuString
    End Function
    ''' <summary>
    ''' 取得加註在menuString之後的Javascript,以自動展開目前的作用節點
    ''' </summary>
    ''' <param name="dt">DataTable物件</param>
    ''' <param name="Level1NodeID">層級1之單元主鍵NodeID</param>
    ''' <returns>HTML字串</returns>
    ''' <remarks></remarks>
    Private Function GetCheckSpanString(ByVal dt As DataTable, ByVal Level1NodeID As String) As String

        Dim s As String = ""
        For j As Integer = 0 To dt.Rows.Count - 1
            Dim d As DataRow = dt.Rows(j)
            If IsDBNull(d("NodeID")) Then
                Continue For
            End If
            If d("NodeID").ToString = Level1NodeID Then
                s = "<script>CheckSpan('menu" & d("SameParentNodeHaveChildThisNodeOrder").ToString & "'," & d("SameParentNodeHaveChildNodeCount").ToString & ");</script>"
                Exit For
            End If
        Next

        Return s
    End Function

    ''' <summary>
    ''' 取得加註在menuString之後的Javascript,以自動展開目前的作用節點(NodeLevel3者呼叫)
    ''' </summary>
    ''' <param name="dt">DataTable物件</param>
    ''' <param name="Level2NodeID">層級1之單元主鍵NodeID</param>
    ''' <param name="Level1NodeID">層級2之單元主鍵NodeID</param>
    ''' <returns>HTML字串</returns>
    ''' <remarks></remarks>
    Private Function GetCheckSpan2String(ByVal dt As DataTable, ByVal Level2NodeID As String, ByVal Level1NodeID As String) As String

        Dim Level1NodeOrder As String = "1"
        For j As Integer = 0 To dt.Rows.Count - 1
            Dim d As DataRow = dt.Rows(j)
            If IsDBNull(d("NodeID")) Then
                Continue For
            End If
            If d("NodeID").ToString = Level1NodeID Then  '找到資料列
                Level1NodeOrder = d("SameParentNodeHaveChildThisNodeOrder").ToString
                Exit For
            End If
        Next
        Dim path As String = System.Web.HttpContext.Current.Request.ApplicationPath
        If Not path.EndsWith("/") Then
            path += "/"
        End If
        Dim s As String = ""
        For j As Integer = 0 To dt.Rows.Count - 1
            Dim d As DataRow = dt.Rows(j)
            If IsDBNull(d("NodeID")) Then
                Continue For
            End If
            If d("NodeID").ToString = Level2NodeID Then  '找到資料列
                s = "<script>CheckSpan2('menu" & SubWord.Substring(Level1NodeOrder - 1, 1) & "'," & d("SameParentNodeHaveChildThisNodeOrder").ToString & "," & d("SameParentNodeHaveChildNodeCount").ToString & ",'" & path & "');</script>"
                Exit For
            End If
        Next

        Return s
    End Function
    ''' <summary>
    ''' 取得現在節點之父層(第1層的Node)
    ''' </summary>
    ''' <param name="dt">DataTable物件</param>
    ''' <param name="Level2NodeID">層級2之單元主鍵NodeID</param>
    ''' <returns>父層單元主鍵NodeID</returns>
    ''' <remarks></remarks>
    Private Function GetLevel1NodeID(ByVal dt As DataTable, ByVal Level2NodeID As String) As String

        Dim Level1NodeID As String = ""
        For j As Integer = 0 To dt.Rows.Count - 1
            Dim d As DataRow = dt.Rows(j)
            If d("NodeID").ToString = Level2NodeID Then
                Level1NodeID = d("ParentNodeID")
                Exit For
            End If
        Next

        Return Level1NodeID
    End Function

    ''' <summary>
    ''' 取得子單元之資料列字串
    ''' </summary>
    ''' <param name="dRow">DataRow資料列</param>
    ''' <param name="Level">層數</param>
    ''' <returns>HTML字串</returns>
    ''' <remarks></remarks>
    Private Function GetChildRowsString(ByVal dRow As DataRow, ByVal Level As Integer) As String
        Dim sb As StringBuilder = New StringBuilder
        Level += 1

        sb.Append(vbCrLf & "<ul>")

        For i As Integer = 0 To dRow.GetChildRows("NodeRelation2").Count - 1
            Dim childRow As DataRow = dRow.GetChildRows("NodeRelation2")(i)
            'sb.Append("<li title=""" & childRow("Text") & """>")
            sb.Append("<li title=""" & IIf(IsDBNull(childRow("TextEn")), childRow("Text"), IIf(childRow("TextEn").ToString.Equals(""), childRow("Text"), childRow("TextEn"))) & """>")

            '//判斷Target
            Dim rp As String
            If CType(childRow("RefPath"), String).EndsWith("Target=_blank") Then
                rp = "target=""_blank"" href=""" & childRow("RefPath") & """>"

            ElseIf CType(childRow("RefPath"), String).EndsWith("Target=_parent") Then
                rp = "target=""_parent"" href=""" & childRow("RefPath") & """>"
            Else
                rp = " href=""" & childRow("RefPath") & """>"
            End If

            If childRow("NodeID") = CetLevelNodeID(Level) Then
                sb.Append("<a class=""active" & Level & """ id=""ItemID" & childRow("NodeID") & """ " & rp)
            Else
                sb.Append("<a class=""subMenu" & Level & """ id=""ItemID" & childRow("NodeID") & """ " & rp)
            End If

            If Level < 3 Then
                sb.Append(IIf(CStr(childRow("Text")).Length > 25, Left(childRow("Text"), 23) & "..", childRow("Text")))
            Else
                sb.Append(IIf(CStr(childRow("Text")).Length > 22, Left(childRow("Text"), 21) & "..", childRow("Text")))
            End If

            sb.Append("</a>")

            If childRow("NodeID") = CetLevelNodeID(Level) Then
                If GetChildRowsCount(childRow, Level) Then
                    sb.Append(GetChildRowsString(childRow, Level))
                End If
            End If
            sb.Append("</li>" & vbCrLf)
            TotalMenuCount += 1
        Next
        sb.Append("</ul>")

        Return sb.ToString
    End Function


    ''' <summary>
    ''' 取得是否有子節點
    ''' </summary>
    ''' <param name="dRow">DataRow物件</param>
    ''' <param name="Level">層數</param>
    ''' <returns>布林值(True/False)</returns>
    ''' <remarks></remarks>
    Private Function GetChildRowsCount(ByVal dRow As DataRow, ByVal Level As Integer) As Boolean
        Level += 1
        Dim i As Integer = 0

        For j As Integer = 0 To dRow.GetChildRows("NodeRelation2").Count - 1
            Dim childRow As DataRow = dRow.GetChildRows("NodeRelation2")(j)
            i += 1
        Next
        If i = 0 Then
            Return False
        Else
            Return True
        End If
    End Function

    ''' <summary>
    ''' 取得選單字串,Javascript版
    ''' </summary>
    ''' 
    ''' <returns>選單HTML原始碼</returns>
    ''' <remarks></remarks>
    Public Function BuidButtomMenu() As String
        'Public Function BuidMenu(Optional ByVal GroupID As Integer = 1) As String
        Dim GroupID As Integer = CInt(RemoveSqlInjection(System.Configuration.ConfigurationManager.AppSettings("ButtomMenuGroupID")).ToString)
        'doPhaseDataTable(GroupID)

        '先從稍早的選單快取中取得資料
        Dim MenuString As String = Context.Cache("ButtomMenuString")

        If MenuString Is Nothing Or MenuString = "" Then
            Dim path As String = System.Web.HttpContext.Current.Request.ApplicationPath
            If Not path.EndsWith("/") Then
                path += "/"
            End If
            '快取中無資料時重新計算

            If System.Configuration.ConfigurationManager.AppSettings("CacheMinute") IsNot Nothing Then
                CacheMinute = CInt(System.Configuration.ConfigurationManager.AppSettings("CacheMinute").ToString)
            End If

            Dim dt As DataTable = doPhaseDataTableButtom(GroupID)
            If dt Is Nothing Then
                Context.Cache.Remove("ButtomMenuString")
                Return ""
            End If
            TotalMenuCount = 0

            Dim iaccesskey As Integer = 0

            '追蹤第1層選單有多少個,在傳入Javascript時可以給予適當回圈
            Dim level1Count As Integer = 0

            Dim sb As StringBuilder = New StringBuilder


            'HomeNodeID本站首頁的NodeID ==> RootNodeID
            For i As Integer = 0 To dt.Rows.Count - 1
                Dim dRow As DataRow = dt.Rows(i)

                If IsDBNull(dRow("NodeID")) Then
                    Continue For
                End If
                If (dRow.IsNull("ParentNodeID")) Then
                    HomeNodeID = CInt(dRow("NodeID").ToString)
                    Exit For
                End If
            Next

            Sub1 = 1
            Sub2 = 1
            Sub3 = 1
            OpenSub1 = False
            OpenSub2 = False
            OpenSub3 = False

            Try


                For i As Integer = 0 To dt.Rows.Count - 1
                    Dim dRow As DataRow = dt.Rows(i)
                    If IsDBNull(dRow("NodeID")) Then
                        Continue For
                    End If
                    '有在本選單群組的項目

                    Dim NodeText As String = Trim(dRow("Text").ToString)
                    '20100818 Chris 新增替代說明
                    'Dim NodeText_Title As String = IIf(IsDBNull(dRow("TextEn")), Trim(dRow("Text").ToString), Trim(dRow("TextEn").ToString)) 'NodeText
                    Dim NodeText_Title As String = IIf(IsDBNull(dRow("TextEn")), Trim(dRow("Text").ToString), IIf(Trim(dRow("TextEn").ToString).Equals(""), Trim(dRow("Text").ToString), Trim(dRow("TextEn").ToString))) 'NodeText
                    If NodeText.Length > 23 Then NodeText = NodeText.Substring(0, 23) & "..."

                    Dim RefPath As String = Trim(dRow("RefPath").ToString)
                    Dim NodePath As String = RefPath
                    'add by Chris 20090211 for URL連結+URL內容帶有http://或https://等字串的時候(或者應該是://代表外連某個網路的應用程式
                    '如果系統設計設定為URL連結加上http://, https://, mms://, mailto:等等都可以使用
                    'If dRow("PublishType").ToString = "2" And (RefPath.IndexOf("http://".ToLower) = 0 Or RefPath.IndexOf("https://".ToLower) = 0 Or RefPath.IndexOf("mailto:".ToLower) = 0) Then
                    '其實如果是多上向稿也無所謂, 只要檢查是不是URL裡面有帶完整URL就好了呢
                    If dRow("PublishType").ToString = "2" And (RefPath.IndexOf("http://".ToLower) = 0 Or RefPath.IndexOf("https://".ToLower) = 0 Or RefPath.IndexOf("mailto:".ToLower) = 0) Then

                    Else
                        RefPath = RefPath & "?cnid=" & dRow("NodeID").ToString
                        NodePath = Nothing
                    End If
                    '====
                    Dim Target As String = ""
                    '強迫另開視窗為URL連結而非另開視窗, 這樣子其實有爭議, 因為多向上稿就不能另開視窗, 或許也是對的
                    '因為多向上稿另開視窗以後, 還是會有完整的網頁(包含左邊選單)這樣子會一直另開視窗, 很無聊
                    '但是如果有人就是這麼無聊要這樣設定呢?
                    '那就可以把下面那一行改成
                    'If dRow("Target").ToString = ("_blank")  Then
                    If dRow("Target").ToString = ("_blank") And dRow("PublishType").ToString = "2" Then
                        Target = " Target=""_blank"" "
                    End If
                    Select Case CInt(dRow("NodeLevel").ToString)
                        Case 1
                            '--------------------------------------------------------------------------------------------
                            Sub2 = 1    'Reset to 0
                            If OpenSub1 = True Then
                                sb.Append("</div>" & vbCrLf & vbCrLf)
                                OpenSub1 = False
                            End If
                            sb.Append("<div class=""downmenu"">" & vbCrLf)
                            'If NodePath Is Nothing Then
                            '    sb.Append("<a id=""BotMenuNode" & dRow("NodeID").ToString & """ title=""" & NodeText_Title & """ href=""" & RefPath & """" & Target & """ >")
                            'Else
                            '    sb.Append("<a id=""BotMenuNode" & dRow("NodeID").ToString & """ title=""" & NodeText_Title & """ href=""" & NodePath & """" & Target & """ >")
                            'End If
                            Dim sbstr As StringBuilder = New StringBuilder
                            If NodePath Is Nothing Then
                                sbstr.Append("<a id=""BotMenuNode" & dRow("NodeID").ToString & """ title=""" & NodeText_Title & """ href=""" & RefPath & """" & Target & " >")
                            Else
                                sbstr.Append("<a id=""BotMenuNode" & dRow("NodeID").ToString & """ title=""" & NodeText_Title & """ href=""" & NodePath & """" & Target & " >")
                            End If
                            If CType(dRow("HaveChildNode"), Boolean) = True Then
                                sbstr = New StringBuilder
                                sbstr.Append("<a id=""BotMenuNode" & dRow("NodeID").ToString & """ title=""" & NodeText_Title & """ href=""" & PathManager.FullDomainUrl & "MenuList.aspx?cnid=" & dRow("NodeID").ToString & """ >")
                            End If
                            sb.Append(sbstr)
                            sb.Append("<span class=""downmenutitle"">" & NodeText & "</span>")    '& "-" & dRow("NodeLevel").ToString)
                            sb.Append("</a>")

                            If CType(dRow("HaveChildNode").ToString(), Boolean) = True Then
                                '有子單元->使用JS
                                level1Count += 1
                                OpenSub1 = True
                                Sub1 += 1

                            Else
                                sb.Append("</div>" & vbCrLf)
                                OpenSub1 = False
                                OpenSub2 = False
                                '==================
                            End If


                        Case 2
                            '--------------------------------------------------------------------------------------------
                            sb.Append("<p>")
                            If NodePath Is Nothing Then
                                sb.Append("<a id=""BotMenuNode" & dRow("NodeID").ToString & """ title=""" & NodeText_Title & """ href=""" & RefPath & """" & Target & " >")
                            Else
                                sb.Append("<a id=""BotMenuNode" & dRow("NodeID").ToString & """ title=""" & NodeText_Title & """ href=""" & NodePath & """" & Target & " >")
                            End If
                            Dim sbstr As StringBuilder = New StringBuilder
                            If NodePath Is Nothing Then
                                sbstr.Append("<a id=""BotMenuNode" & dRow("NodeID").ToString & """ title=""" & NodeText_Title & """ href=""" & RefPath & """" & Target & " >")
                            Else
                                sbstr.Append("<a id=""BotMenuNode" & dRow("NodeID").ToString & """ title=""" & NodeText_Title & """ href=""" & NodePath & """" & Target & " >")
                            End If
                            If CType(dRow("HaveChildNode"), Boolean) = True Then
                                sbstr = New StringBuilder
                                sbstr.Append("<a id=""BotMenuNode" & dRow("NodeID").ToString & """ title=""" & NodeText_Title & """ href=""" & PathManager.FullDomainUrl & "MenuList2.aspx?cnid=" & dRow("NodeID").ToString & """ >")
                            End If
                            sb.Append(sbstr)
                            sb.Append(NodeText)    '& "-" & dRow("NodeLevel").ToString)
                            sb.Append("</a>")
                            sb.Append("</p>" & vbCrLf)
                    End Select

                Next

                '回圈完了如果有未關閉的div
                If OpenSub1 = True Then
                    sb.Append("</div>" & vbCrLf & vbCrLf)
                    OpenSub1 = False
                End If

            Catch ex As Exception
                ModuleWriteLog.WriteLog("WebMenuJs.vb", ex.Message & ex.Source & ex.StackTrace)
            End Try

            MenuString = sb.ToString
            'MenuString = MenuString.Replace("######", level1Count)  '替換掉正確的第1層數量

            '放入快取中60分鐘
            'Context.Cache.Insert("MenuString", MenuString, Nothing, Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(60))
            Context.Cache.Insert("ButtomMenuString", MenuString, Nothing, DateTime.Now.AddMinutes(CacheMinute), System.Web.Caching.Cache.NoSlidingExpiration)

        End If


        ''選單停駐點

        ''如果使用本Module最上方之共用參數,都取到同一個cnid,所以在這重新取得一次
        'Dim CurrentNodeID As String = HttpContext.Current.Request.Params("cnid")

        'If CurrentNodeID <> "" Then
        '    '有網址參數才執行

        '    Dim NodeString As String = GetNodeString(GroupID)
        '    Dim IsInCurrentMenu As Integer = NodeString.IndexOf(CurrentNodeID)
        '    If IsInCurrentMenu >= 0 Then
        '        '網址參數的NodeID有在左方選單當中時

        '        Dim dt As DataTable = doPhaseDataTableButtom(GroupID)
        '        Dim drow As DataRow
        '        For Each drow In dt.Rows
        '            If drow("NodeID").ToString = CurrentNodeID Then
        '                'WriteLog("webMenuJs", "CurrentNodeID=" & CurrentNodeID & " dt=" & dt.Rows.Count & " NodeLevel=" & drow("NodeLevel"), , False)
        '                Select Case CInt(drow("NodeLevel"))
        '                    Case 1
        '                        '在第1層直接focus即可

        '                        '加入註點(20080227-如果Level為0則絶對不能加駐點否則會javascript error)
        '                        MenuString += "<script>var obj = document.getElementById(""BotMenuNode" & CurrentNodeID & """);obj.focus();obj.className ='selected';</script>"
        '                    Case 2
        '                        '加入註點
        '                        MenuString += "<script>var obj = document.getElementById(""BotMenuNode" & drow("ParentNodeID").ToString & """);obj.focus();obj.className ='selected';</script>"
        '                End Select

        '                Exit For
        '            End If
        '        Next

        '    Else
        '        '網址參數的NodeID不在左方選單當中時,重新去SiteMap的DataTable找尋父ID一直到找到有在選單當中

        '        Do While GetNodeString(GroupID).IndexOf(CurrentNodeID) < 0
        '            '這裡處理較深層的選單沒有出現在左方選單時,無法做focus會出現Javascript錯誤之處理!
        '            CurrentNodeID = Sitemap.GetParentNodeID(CurrentNodeID)
        '        Loop

        '        Dim dt As DataTable = doPhaseDataTableButtom(GroupID)
        '        Dim drow As DataRow
        '        For Each drow In dt.Rows
        '            If drow("NodeID").ToString = CurrentNodeID Then
        '                'WriteLog("webMenuJs", "CurrentNodeID=" & CurrentNodeID & " dt=" & dt.Rows.Count & " NodeLevel=" & drow("NodeLevel"), , False)
        '                Select Case CInt(drow("NodeLevel"))
        '                    Case 1
        '                        '在第1層直接focus即可
        '                        '加入註點(20080227-如果Level為0則絶對不能加駐點否則會javascript error)
        '                        MenuString += "<script>var obj = document.getElementById(""BotMenuNode" & CurrentNodeID & """);obj.focus();obj.className ='selected';</script>"
        '                    Case 2
        '                        '加入註點focus
        '                        MenuString += "<script>var obj = document.getElementById(""BotMenuNode" & drow("ParentNodeID").ToString & """);obj.focus();obj.className ='selected';</script>"
        '                End Select
        '                Exit For
        '            End If
        '        Next
        '    End If
        'End If
        Return MenuString
    End Function

    ''' <summary>
    ''' 重新計算網站地圖資料表,屬於這個選單群組的節點的資料(包含各層,有無子節點,同父節點的數量)
    ''' </summary>
    ''' <param name="GroupID">選單群組ID</param>
    ''' <returns>DataTable物件</returns>
    ''' <remarks></remarks>
    Public Function doPhaseDataTableButtom(ByVal GroupID As Integer) As DataTable

        '先從稍早的選單DataTable快取中取得資料
        Dim dt As DataTable = Context.Cache("ButtomMenuStringDataTable")

        If dt Is Nothing Then
            Dim MenuGroupString As String = GetNodeString(GroupID)
            If MenuGroupString Is Nothing Then
                Return dt
            End If
            MenuGroupString = "," & MenuGroupString & ","
            'Dim dt As DataTable
            Dim ds As DataSet = Context.Cache("SiteMap")
            If ds Is Nothing Then ds = GetSiteMapds()
            dt = ds.Tables(0).Clone

            '先製作一份有效的資料列的datatable,只包含有在本群組的選單項目

            For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                Dim drow As DataRow = ds.Tables(0).Rows(i)
                If IsDBNull(dRow("NodeID")) Then
                    Continue For
                End If
                If MenuGroupString.IndexOf("," & dRow("NodeID").ToString & ",") >= 0 Then  '只有在群組內的選單節點才加入選單的DataTable
                        Dim nrow As DataRow = dt.NewRow
                        nrow("NodeID") = dRow("NodeID")
                        nrow("ParentNodeID") = dRow("ParentNodeID")
                        nrow("Text") = dRow("Text")
                        nrow("RefPath") = dRow("RefPath")
                        nrow("NodeLevel") = dRow("NodeLevel")
                        nrow("HaveChildNode") = dRow("HaveChildNode")
                        nrow("PublishType") = dRow("PublishType")
                        nrow("Target") = dRow("Target")
                        dt.Rows.Add(nrow)
                    End If
                Next

            '重新計算是否確實擁有子Node(因後台選單群組設定，可自由設定，可能沒有把子單元勾選進來)
            For i As Integer = 0 To dt.Rows.Count - 1
                Dim drow As DataRow = dt.Rows(i)

                If IsDBNull(drow("NodeID")) Then
                    Continue For
                End If
                Dim dddddd As Boolean = CType(drow("HaveChildNode"), Boolean)
                'WriteLog("doPhaseDataTable", "NodeID" & drow("NodeID") & " " & drow("HaveChildNode"))
                If CType(drow("HaveChildNode"), Boolean) = True Then
                    drow("HaveChildNode") = GetHaveChildNode(dt, drow("NodeID").ToString)
                End If
            Next

            '計算與本節點擁有共同父節點的Node數量，包含本節點自己
            dt.Columns.Add("SameParentNodeCount")
            For i As Integer = 0 To dt.Rows.Count - 1
                Dim drow As DataRow = dt.Rows(i)
                drow("SameParentNodeCount") = GetTheSameParentNodeCount(dt, drow("ParentNodeID").ToString)
            Next

            '計算本節點的子節點數量
            dt.Columns.Add("ChildNodeCount")
            For i As Integer = 0 To dt.Rows.Count - 1
                Dim drow As DataRow = dt.Rows(i)
                If IsDBNull(drow("NodeID")) Then
                    Continue For
                End If
                drow("ChildNodeCount") = GetTheChildNodeCount(dt, drow("NodeID").ToString)
            Next

            '取得相同父ID的節點,並且有子節點的節點,其數量有幾個(呼叫)
            dt.Columns.Add("SameParentNodeHaveChildNodeCount")
            For i As Integer = 0 To dt.Rows.Count - 1
                Dim drow As DataRow = dt.Rows(i)
                drow("SameParentNodeHaveChildNodeCount") = GetTheSameParentNodeHaveChildNodeCount(dt, drow("ParentNodeID").ToString)
            Next

            '取得相同屬性,本節點順眼位(屬性：相同父ID的節點,並且有子節點的節點
            dt.Columns.Add("SameParentNodeHaveChildThisNodeOrder")
            For i As Integer = 0 To dt.Rows.Count - 1
                Dim drow As DataRow = dt.Rows(i)
                If IsDBNull(drow("NodeID")) Then
                    Continue For
                End If
                drow("SameParentNodeHaveChildThisNodeOrder") = GetSameParentNodeHaveChildThisNodeOrder(dt, drow("ParentNodeID").ToString, drow("NodeID").ToString)
            Next

            '放入快取中60分鐘
            'Context.Cache.Insert("MenuString", MenuString, Nothing, Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(60))
            Context.Cache.Insert("ButtomMenuStringDataTable", dt, Nothing, DateTime.Now.AddMinutes(CacheMinute), System.Web.Caching.Cache.NoSlidingExpiration)
        End If

        Return dt
    End Function

    ''' <summary>
    ''' 取得選單字串,Javascript版
    ''' </summary>
    ''' 
    ''' <returns>選單HTML原始碼</returns>
    ''' <remarks></remarks>
    Public Function BuidSubMenu(ByVal WebForm As System.Web.UI.Page, Optional ByVal NodeID As Integer = 0) As String
        'Public Function BuidMenu(Optional ByVal GroupID As Integer = 1) As String
        Dim ds As DataSet = Context.Cache("SiteMap")
        If ds Is Nothing Then ds = GetSiteMapds()
        NodeID = GetNodeID_Auto(WebForm)
        Dim CurrentNodeID As String = NodeID.ToString()
        Dim GroupID As Integer = 0

        For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
            Dim drow1 As DataRow = ds.Tables(0).Rows(i)
            If IsDBNull(drow1("NodeID")) Then
                Continue For
            End If
            If NodeID = drow1("NodeID") And drow1("NodeLevel") = 1 Then
                GroupID = NodeID
                Exit For
            End If
            If drow1("NodeLevel") = 1 Then
                GroupID = drow1("NodeID")
                Continue For
            End If
            If NodeID <> drow1("NodeID") Then
                Continue For
            Else
                Exit For
            End If
        Next

        'doPhaseDataTable(GroupID)

        '先從稍早的選單快取中取得資料
        Dim MenuString As String = ""
        '快取中無資料時重新計算

        Dim dt As DataTable = doPhaseDataTableSub(GroupID, ds)
        If dt Is Nothing Then
            Return ""
        End If
        Dim path As String = System.Web.HttpContext.Current.Request.ApplicationPath
        If Not path.EndsWith("/") Then
            path += "/"
        End If

        If MenuString Is Nothing Or MenuString = "" Then
            TotalMenuCount = 0

            Dim iaccesskey As Integer = 0

            '追蹤第1層選單有多少個,在傳入Javascript時可以給予適當回圈
            Dim level1Count As Integer = 0

            Dim sb As StringBuilder = New StringBuilder



            'HomeNodeID本站首頁的NodeID ==> RootNodeID
            For i As Integer = 0 To dt.Rows.Count - 1
                Dim dRow As DataRow = dt.Rows(i)
                If (dRow.IsNull("ParentNodeID")) Then
                    HomeNodeID = CInt(dRow("NodeID").ToString)
                    Exit For
                End If
            Next

            Dim SubLevel() As Integer = {0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1}
            Dim OpenSub() As Boolean = {False, False, False, False, False, False, False, False, False, False, False}
            Dim ParentNodeId() As Integer = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
            Try


                For i As Integer = 0 To dt.Rows.Count - 1
                    Dim dRow As DataRow = dt.Rows(i)
                    If IsDBNull(dRow("NodeID")) Then
                        Continue For
                    End If
                    '有在本選單群組的項目

                    Dim NodeText As String = Trim(dRow("Text").ToString)

                    '20100818 Chris 新增替代說明
                    'Dim NodeText_Title As String = IIf(IsDBNull(dRow("TextEn")), Trim(dRow("Text").ToString), Trim(dRow("TextEn").ToString)) 'NodeText
                    Dim NodeText_Title As String = IIf(IsDBNull(dRow("TextEn")), Trim(dRow("Text").ToString), IIf(Trim(dRow("TextEn").ToString).Equals(""), Trim(dRow("Text").ToString), Trim(dRow("TextEn").ToString))) 'NodeText
                    If NodeText.Length > 23 Then NodeText = NodeText.Substring(0, 23) & "..."

                    Dim RefPath As String = Trim(dRow("RefPath").ToString)
                    Dim NodePath As String = RefPath
                    'add by Chris 20090211 for URL連結+URL內容帶有http://或https://等字串的時候(或者應該是://代表外連某個網路的應用程式
                    '如果系統設計設定為URL連結加上http://, https://, mms://, mailto:等等都可以使用
                    'If dRow("PublishType").ToString = "2" And (RefPath.IndexOf("http://".ToLower) = 0 Or RefPath.IndexOf("https://".ToLower) = 0 Or RefPath.IndexOf("mailto:".ToLower) = 0) Then
                    '其實如果是多上向稿也無所謂, 只要檢查是不是URL裡面有帶完整URL就好了呢
                    If dRow("PublishType").ToString = "2" And (RefPath.IndexOf("http://".ToLower) = 0 Or RefPath.IndexOf("https://".ToLower) = 0 Or RefPath.IndexOf("mailto:".ToLower) = 0) Then

                    Else
                        RefPath = RefPath & "?cnid=" & dRow("NodeID").ToString
                        NodePath = Nothing
                    End If
                    '====
                    Dim Target As String = ""
                    '強迫另開視窗為URL連結而非另開視窗, 這樣子其實有爭議, 因為多向上稿就不能另開視窗, 或許也是對的
                    '因為多向上稿另開視窗以後, 還是會有完整的網頁(包含左邊選單)這樣子會一直另開視窗, 很無聊
                    '但是如果有人就是這麼無聊要這樣設定呢?
                    '那就可以把下面那一行改成
                    'If dRow("Target").ToString = ("_blank")  Then
                    If dRow("Target").ToString = ("_blank") And dRow("PublishType").ToString = "2" Then
                        Target = " Target=""_blank"" "
                    End If
                    Select Case CInt(dRow("NodeLevel").ToString)
                        Case 1
                            '--------------------------------------------------------------------------------------------
                            Dim mm As Integer = SubLevel.Length - 1
                            While mm > 0
                                If OpenSub(mm) Then
                                    If mm = 1 Then
                                        sb.Append("</div>" & vbCrLf & vbCrLf)
                                    Else
                                        sb.Append("</div><div id=""left_down""></div>" & vbCrLf & vbCrLf)
                                    End If
                                    OpenSub(mm) = False
                                End If
                                SubLevel(mm) = 1
                                OpenSub(mm) = False
                                ParentNodeId(mm) = 0
                                mm = mm - 1
                            End While
                            ParentNodeId(dRow("NodeLevel")) = dRow("ParentNodeId")
                            sb.Append("<div id=""left_top"">" & NodeText & "</div>" & vbCrLf & "<div id=""left_bg"">" & vbCrLf)

                            If CType(dRow("HaveChildNode").ToString(), Boolean) = True Then
                                '有子單元->
                                level1Count += 1
                                SubLevel(1) += 1
                                OpenSub(1) = True
                            Else
                                sb.Append("</div>" & vbCrLf)
                                mm = SubLevel.Length - 1
                                While mm > 0
                                    SubLevel(mm) = 1
                                    OpenSub(mm) = False
                                    mm = mm - 1
                                End While
                                '==================
                            End If

                        Case Else
                            '--------------------------------------------------------------------------------------------
                            '--------------------------------------------------------------------------------------------
                            Dim mm As Integer = SubLevel.Length - 1
                            While mm > dRow("NodeLevel") - 1
                                If OpenSub(mm) Then
                                    sb.Append("</div>" & vbCrLf & vbCrLf)
                                    OpenSub(mm) = False
                                End If
                                If Not OpenSub(mm - 1) Then
                                    SubLevel(mm) = 1
                                    ParentNodeId(mm) = 0
                                End If
                                If mm = SubLevel.Length - 1 Then
                                    SubLevel(mm) = 1
                                Else
                                    SubLevel(mm + 1) = 1
                                End If
                                mm = mm - 1
                            End While
                            ParentNodeId(dRow("NodeLevel")) = dRow("ParentNodeId")
                            If CType(dRow("HaveChildNode").ToString(), Boolean) = True Then
                                If dRow("NodeLevel") = 2 Then
                                    sb.Append("<div class=""menu_1"">" & vbCrLf)
                                Else
                                    sb.Append("<div class=""menu_sub" & (dRow("NodeLevel") - 1).ToString & """>" & vbCrLf)
                                End If
                                sb.Append("<img id=""MenuSub" & dRow("ParentNodeID").ToString & "_" & SubLevel(CInt(dRow("NodeLevel").ToString())) & "_icon"" class=""menu_sub2_icon"" height=""9"" alt=""icon"" src=""" & PathManager.ApplicationUrl & "images/icon_07.gif"" width=""9"" />")
                                If NodePath Is Nothing Then
                                    sb.Append("<a id=""MenuNode" & dRow("NodeID").ToString & """ title=""" & NodeText_Title & """ href=""#"" onclick=""CheckSpan2('MenuSub" & dRow("ParentNodeID").ToString & "_" & "'," & SubLevel(CInt(dRow("NodeLevel").ToString())).ToString & "," & dRow("SameParentNodeHaveChildNodeCount").ToString & ",'" & path & "');"" onkeypress=""CheckSpan2('MenuSub" & dRow("ParentNodeID").ToString & "_" & "'," & SubLevel(CInt(dRow("NodeLevel").ToString())).ToString & "," & dRow("SameParentNodeHaveChildNodeCount").ToString & ",'" & path & "');"">")
                                Else
                                    sb.Append("<a id=""MenuNode" & dRow("NodeID").ToString & """ title=""" & NodeText_Title & """ href=""#"" onclick=""CheckSpan2('MenuSub" & dRow("ParentNodeID").ToString & "_" & "'," & SubLevel(CInt(dRow("NodeLevel").ToString())).ToString & "," & dRow("SameParentNodeHaveChildNodeCount").ToString & ",'" & path & "');"" onkeypress=""CheckSpan2('MenuSub" & dRow("ParentNodeID").ToString & "_" & "'," & SubLevel(CInt(dRow("NodeLevel").ToString())).ToString & "," & dRow("SameParentNodeHaveChildNodeCount").ToString & ",'" & path & "');"">")
                                    'sb.Append("<a id=""MenuNode" & dRow("NodeID").ToString & """ title=""" & NodeText_Title & """ href=""" & RefPath & """" & Target & " onclick=""CheckSpan2('MenuSub" & dRow("ParentNodeID").ToString & "_" & "'," & SubLevel(CInt(dRow("NodeLevel").ToString())).ToString & "," & dRow("SameParentNodeHaveChildNodeCount").ToString & ",'" & path & "');"" onkeypress=""CheckSpan2('MenuSub" & dRow("ParentNodeID").ToString & "_" & "'," & SubLevel(CInt(dRow("NodeLevel").ToString())).ToString & "," & dRow("SameParentNodeHaveChildNodeCount").ToString & ",'" & path & "');"">")
                                End If
                                sb.Append(NodeText)    '& "-" & dRow("NodeLevel").ToString)
                                sb.Append("</a>" & vbCrLf)

                                '******無障礙*******'
                                sb.Append("<noscript>")
                                sb.Append("<a id=""NSMenuNode" & dRow("NodeID").ToString & """ title=""" & NodeText_Title & """ href=""" & RefPath & """" & Target & ">")
                                sb.Append(NodeText)
                                sb.Append("</a>")
                                sb.Append("</noscript>" & vbCrLf)
                                '******無障礙*******'

                                sb.Append("</div>" & vbCrLf)
                                '有子單元->使用JS
                                sb.Append(" <div style=""display: none"" id=""MenuSub" & dRow("ParentNodeID").ToString & "_" & SubLevel(CInt(dRow("NodeLevel").ToString())) & """>" & vbCrLf)
                                OpenSub(CInt(dRow("NodeLevel").ToString())) = True

                                SubLevel(CInt(dRow("NodeLevel").ToString())) += 1
                            Else
                                '無子單元->用Link
                                If dRow("NodeLevel") = 2 Then
                                    sb.Append("<div class=""menu_1"">" & vbCrLf)
                                Else
                                    sb.Append("<div class=""menu_sub" & (CInt(dRow("NodeLevel").ToString()) - 1).ToString & """>" & vbCrLf)
                                End If
                                sb.Append("<img class=""menu_sub2_icon"" height=""9"" alt=""icon"" src=""" & PathManager.ApplicationUrl & "images/icon_08.gif"" width=""9"" />")
                                sb.Append("<a id=""MenuNode" & dRow("NodeID").ToString & """ title=""" & NodeText_Title & """ href=""" & RefPath & """" & Target & ">")
                                sb.Append(NodeText)    '& "-" & dRow("NodeLevel").ToString)
                                sb.Append("</a>" & vbCrLf)


                                sb.Append("</div>" & vbCrLf)
                                '20100412 既然沒有子單元，就已經是完整的DIV不用再判斷了
                                OpenSub(CInt(dRow("NodeLevel").ToString())) = False
                                '==================
                            End If



                    End Select

                Next

                '回圈完了如果有未關閉的div
                Dim nn As Integer = SubLevel.Length - 1
                While nn > 0
                    If OpenSub(nn) Then
                        If nn = 1 Then
                            sb.Append("</div><div id=""left_down""></div>" & vbCrLf & vbCrLf)
                        Else
                            sb.Append("</div>" & vbCrLf & vbCrLf)
                        End If
                        OpenSub(nn) = False
                    End If
                    SubLevel(nn) = 1
                    OpenSub(nn) = False
                    ParentNodeId(nn) = 0
                    nn = nn - 1
                End While


            Catch ex As Exception
                ModuleWriteLog.WriteLog("WebMenuJs.vb", ex.Message & ex.Source & ex.StackTrace)
            End Try

            MenuString = sb.ToString
            'MenuString = MenuString.Replace("######", level1Count)  '替換掉正確的第1層數量

        End If


        '選單停駐點

        '如果使用本Module最上方之共用參數,都取到同一個cnid,所以在這重新取得一次
        'Dim CurrentNodeID As String = HttpContext.Current.Request.Params("cnid")

        If CurrentNodeID <> "" Then
            '有網址參數才執行
            Dim NodeString As String = MenuString
            Dim IsInCurrentMenu As Integer = NodeString.IndexOf("""MenuNode" & CurrentNodeID & """")
            If IsInCurrentMenu >= 0 Then
                '網址參數的NodeID有在左方選單當中時
                For i As Integer = 0 To dt.Rows.Count - 1
                    Dim dRow As DataRow = dt.Rows(i)
                    If IsDBNull(drow("NodeID")) Then
                        Continue For
                    End If
                    If drow("NodeID").ToString = CurrentNodeID Then
                        Select Case CInt(drow("NodeLevel"))
                            Case 1
                                MenuString += "<script>var obj = document.getElementById(""MenuNode" & CurrentNodeID & """);obj.focus(); </script>"
                            Case Else
                                '在第2層直接focus即可
                                '加入註點
                                MenuString += "<script>"

                                If CInt(drow("NodeLevel").ToString()) > 2 Then
                                    Dim nodelevel1 As Integer = CInt(drow("NodeLevel").ToString())
                                    Dim parentid As Integer = drow("ParentNodeID").ToString
                                    Dim SameIDOrder As Integer = drow("SameParentNodeHaveChildThisNodeOrder").ToString
                                    While nodelevel1 > 2

                                        For j As Integer = 0 To dt.Rows.Count - 1
                                            Dim drow2 As DataRow = dt.Rows(j)
                                            If drow2("NodeID") = parentid Then '找到資料列

                                                SameIDOrder = drow2("SameParentNodeHaveChildThisNodeOrder").ToString

                                                For k As Integer = 0 To dt.Rows.Count - 1
                                                    Dim drow3 As DataRow = dt.Rows(k)
                                                    If drow3("NodeID") = parentid Then '找到資料列
                                                        nodelevel1 = CInt(drow2("NodeLevel").ToString())
                                                        parentid = drow3("ParentNodeID").ToString
                                                        Exit For
                                                    End If
                                                Next
                                                If nodelevel1 < 2 Then
                                                    Exit For
                                                End If
                                                MenuString += "document.getElementById(""MenuSub" & parentid & "_" & SameIDOrder & """).style.display=""block"";"
                                                MenuString += "document.getElementById(""MenuSub" & parentid & "_" & SameIDOrder & "_icon"").src=""" & path & "images/icon_08.gif" & """;"
                                                nodelevel1 = CInt(drow2("NodeLevel").ToString())
                                                parentid = drow2("ParentNodeID").ToString
                                                'SameIDOrder = drow2("SameParentNodeHaveChildThisNodeOrder").ToString
                                            End If
                                        Next
                                    End While
                                End If

                                If CType(drow("HaveChildNode").ToString(), Boolean) = True Then
                                    MenuString += "document.getElementById(""MenuSub" & drow("ParentNodeID").ToString & "_" & drow("SameParentNodeHaveChildThisNodeOrder").ToString() & """).style.display=""block"";"
                                End If
                                MenuString += "var obj = document.getElementById(""MenuNode" & CurrentNodeID & """);obj.className ='selected';"
                                'MenuString += "obj.focus();</script>"
                                MenuString += "</script>"
                        End Select

                        Exit For
                    End If
                Next

            Else
                '網址參數的NodeID不在左方選單當中時,重新去SiteMap的DataTable找尋父ID一直到找到有在選單當中
                Dim NodeLevel As Integer

                Do While NodeString.IndexOf("""MenuNode" & CurrentNodeID & """") < 0
                    '這裡處理較深層的選單沒有出現在左方選單時,無法做focus會出現Javascript錯誤之處理!
                    NodeLevel = 0
                    CurrentNodeID = Sitemap.GetParentNodeID(CurrentNodeID)

                    For j As Integer = 0 To ds.Tables(0).Rows.Count - 1
                        Dim dRow2 As DataRow = ds.Tables(0).Rows(j)
                        If dRow2("NodeID") = CurrentNodeID Then
                            NodeLevel = CInt(dRow2("NodeLevel"))
                            Exit For
                        End If
                    Next
                    If NodeLevel < 2 Then
                        Exit Do
                    End If
                Loop
                If NodeLevel > 1 Then
                    'Dim dt As DataTable = doPhaseDataTable(GroupID)
                    For i As Integer = 0 To dt.Rows.Count - 1
                        Dim dRow As DataRow = dt.Rows(i)
                        If drow("NodeID").ToString = CurrentNodeID Then
                            'WriteLog("webMenuJs", "CurrentNodeID=" & CurrentNodeID & " dt=" & dt.Rows.Count & " NodeLevel=" & drow("NodeLevel"), , False)
                            Select Case CInt(drow("NodeLevel"))
                                Case 1
                                    MenuString += "<script>var obj = document.getElementById(""MenuNode" & CurrentNodeID & """);obj.focus(); </script>"
                                Case Else
                                    '在第2層直接focus即可
                                    '加入註點
                                    MenuString += "<script>"

                                    If CInt(drow("NodeLevel").ToString()) > 2 Then
                                        Dim nodelevel1 As Integer = CInt(drow("NodeLevel").ToString())
                                        Dim parentid As Integer = drow("ParentNodeID").ToString
                                        Dim SameIDOrder As Integer = drow("SameParentNodeHaveChildThisNodeOrder").ToString
                                        While nodelevel1 > 2

                                            For j As Integer = 0 To dt.Rows.Count - 1
                                                Dim drow2 As DataRow = dt.Rows(j)
                                                If drow2("NodeID") = parentid Then '找到資料列

                                                    SameIDOrder = drow2("SameParentNodeHaveChildThisNodeOrder").ToString

                                                    For mm As Integer = 0 To dt.Rows.Count - 1
                                                        Dim drow3 As DataRow = dt.Rows(mm)
                                                        If drow3("NodeID") = parentid Then '找到資料列
                                                            nodelevel1 = CInt(drow2("NodeLevel").ToString())
                                                            parentid = drow3("ParentNodeID").ToString
                                                            Exit For
                                                        End If
                                                    Next
                                                    If nodelevel1 < 2 Then
                                                        Exit For
                                                    End If
                                                    MenuString += "document.getElementById(""MenuSub" & parentid & "_" & SameIDOrder & """).style.display=""block"";"
                                                    MenuString += "document.getElementById(""MenuSub" & parentid & "_" & SameIDOrder & "_icon"").src=""" & path & "images/icon_08.gif" & """;"
                                                    nodelevel1 = CInt(drow2("NodeLevel").ToString())
                                                    parentid = drow2("ParentNodeID").ToString
                                                    'SameIDOrder = drow2("SameParentNodeHaveChildThisNodeOrder").ToString
                                                End If
                                            Next
                                        End While
                                    End If

                                    If CType(drow("HaveChildNode").ToString(), Boolean) = True Then
                                        MenuString += "document.getElementById(""MenuSub" & drow("ParentNodeID").ToString & "_" & drow("SameParentNodeHaveChildThisNodeOrder").ToString() & """).style.display=""block"";"
                                    End If
                                    MenuString += "var obj = document.getElementById(""MenuNode" & CurrentNodeID & """);obj.className ='selected';"
                                    'MenuString += "obj.focus();</script>"
                                    MenuString += "</script>"
                            End Select
                            Exit For
                        End If
                    Next
                Else
                    MenuString += "<script>var obj = document.getElementById(""left_top"");obj.innerHTML='" & RemoveSqlInjection(System.Configuration.ConfigurationManager.AppSettings("NavigationHomeText").ToString) & "'</script>"
                End If

            End If
        End If

        Return MenuString
    End Function

    ''' <summary>
    ''' 重新計算網站地圖資料表,屬於這個選單群組的節點的資料(包含各層,有無子節點,同父節點的數量)
    ''' </summary>
    ''' <param name="GroupID">選單群組ID</param>
    ''' <returns>DataTable物件</returns>
    ''' <remarks></remarks>
    Public Function doPhaseDataTableSub(ByVal GroupID As Integer, ByVal ds As DataSet) As DataTable

        If ds Is Nothing Then ds = GetSiteMapds()
        Dim dt As DataTable = ds.Tables(0).Clone
        '先製作一份有效的資料列的datatable,只包含有在本群組的選單項目
        Dim thisnode As Boolean = False
        For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
            Dim drow As DataRow = ds.Tables(0).Rows(i)
            If IsDBNull(drow("NodeID")) Then
                Continue For
            End If
            If GroupID = drow("NodeID") Then
                thisnode = True
                Dim nrow As DataRow = dt.NewRow
                nrow("NodeID") = drow("NodeID")
                nrow("ParentNodeID") = drow("ParentNodeID")
                nrow("Text") = drow("Text")
                nrow("RefPath") = drow("RefPath")
                nrow("NodeLevel") = drow("NodeLevel")
                nrow("HaveChildNode") = drow("HaveChildNode")
                nrow("PublishType") = drow("PublishType")
                nrow("Target") = drow("Target")
                dt.Rows.Add(nrow)
                Continue For
            End If
            If thisnode Then
                Dim nrow As DataRow = dt.NewRow
                If drow("NodeLevel") < 2 Then
                    Exit For
                End If
                nrow("NodeID") = drow("NodeID")
                nrow("ParentNodeID") = drow("ParentNodeID")
                nrow("Text") = drow("Text")
                nrow("RefPath") = drow("RefPath")
                nrow("NodeLevel") = drow("NodeLevel")
                nrow("HaveChildNode") = drow("HaveChildNode")
                nrow("PublishType") = drow("PublishType")
                nrow("Target") = drow("Target")
                dt.Rows.Add(nrow)
            End If
        Next

        '重新計算是否確實擁有子Node(因後台選單群組設定，可自由設定，可能沒有把子單元勾選進來)
        For i As Integer = 0 To dt.Rows.Count - 1
            Dim dRow As DataRow = dt.Rows(i)
            If IsDBNull(drow("NodeID")) Then
                Continue For
            End If
            If CType(drow("HaveChildNode"), Boolean) = True Then
                drow("HaveChildNode") = GetHaveChildNode(dt, drow("NodeID").ToString)
            End If
        Next

        '計算與本節點擁有共同父節點的Node數量，包含本節點自己
        dt.Columns.Add("SameParentNodeCount")
        For i As Integer = 0 To dt.Rows.Count - 1
            Dim dRow As DataRow = dt.Rows(i)
            dRow("SameParentNodeCount") = GetTheSameParentNodeCount(dt, drow("ParentNodeID").ToString)
        Next

        '計算本節點的子節點數量
        dt.Columns.Add("ChildNodeCount")
        For i As Integer = 0 To dt.Rows.Count - 1
            Dim dRow As DataRow = dt.Rows(i)
            If IsDBNull(drow("NodeID")) Then
                Continue For
            End If
            drow("ChildNodeCount") = GetTheChildNodeCount(dt, drow("NodeID").ToString)
        Next

        '取得相同父ID的節點,並且有子節點的節點,其數量有幾個(呼叫)
        dt.Columns.Add("SameParentNodeHaveChildNodeCount")
        For i As Integer = 0 To dt.Rows.Count - 1
            Dim dRow As DataRow = dt.Rows(i)
            dRow("SameParentNodeHaveChildNodeCount") = GetTheSameParentNodeHaveChildNodeCount(dt, drow("ParentNodeID").ToString)
        Next

        '取得相同屬性,本節點順眼位(屬性：相同父ID的節點,並且有子節點的節點
        dt.Columns.Add("SameParentNodeHaveChildThisNodeOrder")
        For i As Integer = 0 To dt.Rows.Count - 1
            Dim dRow As DataRow = dt.Rows(i)
            If IsDBNull(drow("NodeID")) Then
                Continue For
            End If
            drow("SameParentNodeHaveChildThisNodeOrder") = GetSameParentNodeHaveChildThisNodeOrder(dt, drow("ParentNodeID").ToString, drow("NodeID").ToString)
        Next
        Return dt
    End Function
#End Region

End Module
