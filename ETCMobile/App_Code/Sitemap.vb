Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient

''' <summary>
''' 提供前台單元節點服務
''' </summary>
''' <remarks></remarks>
Public Module Sitemap

    Private CacheKeyDic As String = PathManager.GetUploadPath().ToString & System.Configuration.ConfigurationManager.AppSettings("CacheKeyPath").ToString
    Private CacheMinute As Integer = 15

    Dim Context As HttpContext = HttpContext.Current
    Dim Meta_Kind As String = ConfigurationManager.AppSettings("Meta_Kind").ToString
    Public Function GetMeta(ByVal SiteID As Integer, ByVal Kind As String) As DataSet

        Dim ds As DataSet
        If Meta_Kind = 1 Then
            ds = ClassDB.RunSPReturnDataSet("Meta_GetAllList", Kind, New SqlParameter("@SiteID", SiteID))
        Else
            ds = ClassDB.RunSPReturnDataSet("Meta_GetAllListEN", Kind, New SqlParameter("@SiteID", SiteID))
        End If
        Try
            If Not ds.Relations.Contains("NodeRelation2") Then
                Try
                    ds.Relations.Add("NodeRelation2", ds.Tables(0).Columns("NodeId"), ds.Tables(0).Columns("ParentNodeId"))
                Catch ex1 As Exception
                    WriteLog("Sitemap.vb,Add Relations and Cache Relation error", ex1.Message)
                End Try
            End If

            Dim ApplicationUrl As String = PathManager.ApplicationUrl

            '建立完整的路徑表,避開站外連結

            For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                Dim d As DataRow = ds.Tables(0).Rows(i)
                If IsDBNull(d("RefPath")) Then
                    Continue For
                End If

                If Not CType(d("RefPath"), String).StartsWith("http://") Then
                    d("RefPath") = ApplicationUrl & d("RefPath")
                End If
            Next

            '放進快取中
            Context.Cache.Insert(Kind, ds, Nothing, Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(60))

        Catch ex As Exception
            WriteLog("Sitemap.vb,Add Relations and Cache", ex.Message)
        End Try


        Return ds
    End Function
    ''' <summary>
    ''' 自資料庫中取得網站單元節點資料
    ''' </summary>
    ''' <returns>DataSet物件</returns>
    ''' <remarks></remarks>
    Public Function GetSiteMapds() As DataSet

        If System.Configuration.ConfigurationManager.AppSettings("CacheMinute") IsNot Nothing Then
            CacheMinute = CInt(System.Configuration.ConfigurationManager.AppSettings("CacheMinute").ToString)
        End If

        Dim SiteID As String = CInt(System.Configuration.ConfigurationManager.AppSettings("SiteID"))
        'Dim ds As DataSet = ClassDB.RunSPReturnDataSet("SiteMap_GetAllList", "SiteMap", New SqlParameter("@SiteID", SiteID))
        '20090211 Chris Chu Sitemap裡面的選單應該要把沒有打勾的(也就是沒有啟用的單元)不要顯示在前端, 要做篩選
        'Dim ds As DataSet = ClassDB.RunSPReturnDataSet("SiteMap_GetAllList", "SiteMap", New SqlParameter("@SiteID", SiteID))
        Dim ds As DataSet = ClassDB.RunSPReturnDataSet("SiteMap_GetAllList", "SiteMap", New SqlParameter("@SiteID", SiteID))
        ds = clearSiteMapdsBrokenLink(ds)
        Try
            If Not ds.Relations.Contains("NodeRelation2") Then
                Try
                    ds.Relations.Add("NodeRelation2", ds.Tables(0).Columns("NodeId"), ds.Tables(0).Columns("ParentNodeId"))
                Catch ex1 As Exception
                    WriteLog("Sitemap.vb,Add Relations and Cache Relation error", ex1.Message)
                End Try
            End If

            Dim ApplicationUrl As String = PathManager.ApplicationUrl

            '建立完整的路徑表,避開站外連結
            For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                Dim d As DataRow = ds.Tables(0).Rows(i)
                If Not IsDBNull(d("RefPath")) Then
                    If (Not d("RefPath").ToString.StartsWith("http://")) And (Not d("RefPath").ToString.StartsWith("https://")) Then
                        d("RefPath") = ApplicationUrl & d("RefPath")
                    End If
                End If
            Next

            '放進快取中
            'Context.Cache.Insert("SiteMap", ds, Nothing, Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(60))
            Context.Cache.Insert("SiteMap", ds, Nothing, DateTime.Now.AddMinutes(CacheMinute), System.Web.Caching.Cache.NoSlidingExpiration)

        Catch ex As Exception
            WriteLog("Sitemap.vb,Add Relations and Cache", ex.Message)
        End Try

        Return ds
    End Function
    ''' <summary>
    ''' 自資料庫中取得網站單元節點資料
    ''' </summary>
    ''' <returns>DataSet物件</returns>
    ''' <remarks></remarks>
    Public Function GetSiteMapdsActive() As DataSet

        If System.Configuration.ConfigurationManager.AppSettings("CacheMinute") IsNot Nothing Then
            CacheMinute = CInt(System.Configuration.ConfigurationManager.AppSettings("CacheMinute").ToString)
        End If

        Dim SiteID As String = RemoveSqlInjection(System.Configuration.ConfigurationManager.AppSettings("SiteID"))
        'Dim ds As DataSet = ClassDB.RunSPReturnDataSet("SiteMap_GetAllList", "SiteMap", New SqlParameter("@SiteID", SiteID))
        '20090211 Chris Chu Sitemap裡面的選單應該要把沒有打勾的(也就是沒有啟用的單元)不要顯示在前端, 要做篩選
        'Dim ds As DataSet = ClassDB.RunSPReturnDataSet("SiteMap_GetAllActiveList", "SiteMap", New SqlParameter("@SiteID", SiteID))
        Dim ds As DataSet = ClassDB.RunSPReturnDataSet("SiteMap_GetAllActiveList", "SiteMap", New SqlParameter("@SiteID", SiteID))
        ds = clearSiteMapdsBrokenLink(ds)
        Try
            If Not ds.Relations.Contains("NodeRelation2") Then
                Try
                    ds.Relations.Add("NodeRelation2", ds.Tables(0).Columns("NodeId"), ds.Tables(0).Columns("ParentNodeId"))
                Catch ex1 As Exception
                    WriteLog("Sitemap.vb,Add Relations and Cache Relation error", ex1.Message)
                End Try
            End If

            Dim ApplicationUrl As String = PathManager.ApplicationUrl

            '建立完整的路徑表,避開站外連結
            For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                Dim d As DataRow = ds.Tables(0).Rows(i)
                If Not IsDBNull(d("RefPath")) Then
                    If (Not d("RefPath").ToString.StartsWith("http://")) And (Not d("RefPath").ToString.StartsWith("https://")) Then
                        d("RefPath") = ApplicationUrl & d("RefPath")
                    End If
                End If
            Next

            '放進快取中
            'Context.Cache.Insert("SiteMap", ds, Nothing, Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(60))
            Context.Cache.Insert("SiteMap", ds, Nothing, DateTime.Now.AddMinutes(CacheMinute), System.Web.Caching.Cache.NoSlidingExpiration)

        Catch ex As Exception
            WriteLog("Sitemap.vb,Add Relations and Cache", ex.Message)
        End Try

        Return ds
    End Function
    ''' <summary>
    ''' 自資料庫中取得網站單元節點資料
    ''' </summary>
    ''' <returns>DataSet物件</returns>
    ''' <remarks></remarks>
    Public Function clearSiteMapdsBrokenLink(ByVal ds As DataSet) As DataSet
        'Dim dset As DataSet = ds.Copy
        Dim dset As DataSet = ds.Clone
        Dim dt As DataTable = ds.Tables(0)

        Dim dtclone As DataTable = dt.Clone
        Dim i As Integer = 0
        Dim del As Integer = 0
        Dim menustr As String = ","
        For i = 0 To dt.Rows.Count - 1
            If dt.Rows(i)("NodeID") IsNot Nothing Then
                menustr = menustr & dt.Rows(i)("NodeID").ToString & ","
            End If
        Next

        For i = 0 To dt.Rows.Count - 1
            If dt.Rows(i)("ParentNodeID") IsNot Nothing Then
                If menustr.IndexOf("," & dt.Rows(i)("ParentNodeID").ToString & ",") < 0 Then
                    Continue For
                End If
            End If
            Dim newrow As DataRow = dtclone.NewRow
            For j As Integer = 0 To dt.Columns.Count - 1
                newrow(j) = dt.Rows(i)(j)
            Next
            dtclone.Rows.Add(newrow)
        Next
        dset.Tables.RemoveAt(0)
        dset.Tables.Add(dtclone)
        Return dset
    End Function
    ''' <summary>
    ''' 後台移除快取相依性檔案
    ''' </summary>
    ''' <param name="CacheKeyFileString">快取釋放之依據檔案實體路徑字串</param>
    ''' <remarks></remarks>
    Public Sub RemoveCacheKey(ByVal CacheKeyFileString As String)
        Dim Context As HttpContext = HttpContext.Current
        'Dim CacheKeyFile As Fileio.
        If FileIO.FileSystem.FileExists(Context.Server.MapPath(RemoveXSS(CacheKeyDic & CacheKeyFileString))) Then
            FileIO.FileSystem.DeleteFile(Context.Server.MapPath(RemoveXSS(CacheKeyDic & CacheKeyFileString)))
        End If
    End Sub


    ''' <summary>
    ''' 自動取得現行網頁表單之單元名稱
    ''' </summary>
    ''' <param name="WebForm">WebForm表單物件</param>
    ''' <returns>單元節點名稱</returns>
    ''' <remarks></remarks>
    Public Function GetNodeText_Auto(ByVal WebForm As System.Web.UI.Page) As String
        Dim NodeText As String = ""
        Dim NodeID As String = WebForm.Request.Params("cnid")
        Dim cnkw As String = WebForm.Request.Params("cnkw")
        Dim cnhb As String = WebForm.Request.Params("cnhb")

        If IsNumeric(NodeID) Then
            '取得此NOdeID的節點名稱
            NodeText = GetNodeText_byNodeID(NodeID)
            'ElseIf cnkw IsNot Nothing Then
            '    '取得此cnkw的節點名稱
            '    cnkw = RemoveSqlInjection(cnkw)
            '    NodeID = GetNodeID_byNodeKeyword(cnkw)
            '    NodeText = GetNodeText_byNodeID(NodeID)
        ElseIf IsNumeric(cnhb) Then
            '無NodeID,有單元關鍵字碼 
            NodeText = GetNodeText_byCNHB(cnhb.ToString)
            'NodeText = System.Configuration.ConfigurationManager.AppSettings("NavigationHomeText").ToString
        Else
            '以網址路徑取得名稱
            Dim Url As String = ""
            Url = WebForm.Request.Path
            'Dim i As Integer = Url.LastIndexOf("/")
            'Url = Url.Substring(i + 1, Url.Length - i - 1)
            NodeText = GetNodeText_byUrl(WebForm, Url)
        End If

        Return NodeText

    End Function


    ''' <summary>
    ''' 由NodeID取得節點名稱
    ''' </summary>
    ''' <param name="NodeID">單元節點資料之主鍵值NodeID</param>
    ''' <returns>單元節點名稱</returns>
    ''' <remarks></remarks>
    Public Function GetNodeText_byNodeID(ByVal NodeID As Integer) As String
        Dim ds As DataSet = Context.Cache("SiteMap")
        If ds Is Nothing Then
            ds = Sitemap.GetSiteMapds
        End If

        Dim NodeText As String = ""

        For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
            Dim r As DataRow = ds.Tables(0).Rows(i)
            If IsDBNull(r("NodeID")) Then
                Continue For
            End If
            If CInt(r("NodeID")).Equals(NodeID) Then
                NodeText = RemoveXSS(r("Text").ToString)
                Exit For
            End If
        Next

        Return NodeText
    End Function


    ''' <summary>
    ''' 由網址的字串取得節點名稱
    ''' </summary>
    ''' <param name="WebForm">WebForm表單物件</param>
    ''' <param name="Url">網址字串</param>
    ''' <returns>單元節點名稱</returns>
    ''' <remarks></remarks>
    Public Function GetNodeText_byUrl(ByVal WebForm As System.Web.UI.Page, ByVal Url As String) As String
        Dim ds As DataSet = Context.Cache("SiteMap")
        If ds Is Nothing Then
            ds = Sitemap.GetSiteMapds
        End If

        Dim NodeText As String = ""

        For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
            Dim r As DataRow = ds.Tables(0).Rows(i)
            If Not IsDBNull(r("RefPath")) Then
                If r("RefPath").ToString.Length > 0 Then
                    If Url.EndsWith(r("RefPath").ToString) Then
                        NodeText = RemoveXSS(r("Text").ToString)
                        Exit For
                    End If
                End If
            End If
        Next

        Return NodeText
    End Function


    ''' <summary>
    ''' 自動取得NodeID
    ''' </summary>
    ''' <param name="WebForm">WebForm網頁表單</param>
    ''' <returns>單元節點主鍵值NodeID</returns>
    ''' <remarks></remarks>
    Public Function GetNodeID_Auto(ByVal WebForm As System.Web.UI.Page) As Integer
        Dim NodeID As String = WebForm.Request.Params("cnid")
        Dim cnkw As String = WebForm.Request.Params("cnkw")
        Dim cnhb As String = WebForm.Request.Params("cnhb")
        If IsNumeric(NodeID) Then
            NodeID = CInt(NodeID)
            'ElseIf NodeID Is Nothing And cnkw IsNot Nothing Then
            '    cnkw = RemoveSqlInjection(cnkw)
            '    '無NodeID,有單元關鍵字碼
            '    NodeID = Sitemap.GetNodeID_byNodeKeyword(cnkw.ToString)
        ElseIf IsNumeric(cnhb) Then
            '無NodeID,有單元關鍵字碼
            NodeID = 0 'GetNodeID_byUrlString(PathManager.ApplicationUrl & "Default.aspx")
        Else

            '其他情況時以網址取得NodeID
            NodeID = Sitemap.GetNodeID_byUrl(WebForm)
        End If

        Return NodeID
    End Function



    '''' <summary>
    '''' 由NodeKeyword取得NodeID
    '''' </summary>
    '''' <returns>單元節點主鍵值NodeID</returns>
    '''' <remarks>呼叫者必須確定QueryString("cnkw") IsNot Nothing</remarks>
    'Public Function GetNodeID_byNodeKeyword(ByVal cnkw As String) As Integer

    '    Dim Keyword As String = cnkw
    '    Dim SiteID As String = CInt(System.Configuration.ConfigurationManager.AppSettings("SiteID")).ToString
    '    Dim NodeID As Integer = ClassDB.RunSPReturnInteger("Net2_SiteMap_GetNodeID_byNodeKeyword",
    '    New SqlParameter("@Keyword", Keyword),
    '    New SqlParameter("@SiteID", SiteID))

    '    Return NodeID
    'End Function

    ''' <summary>
    ''' 由NodeKeyword取得NodeID
    ''' </summary>
    ''' <param name="Url">Url</param>
    ''' <returns>單元節點主鍵值NodeID</returns>
    ''' <remarks>呼叫者必須確定QueryString("cnkw") IsNot Nothing</remarks>
    Public Function GetNodeID_byUrlString(ByVal Url As String) As Integer

        '以網址路徑取得名稱
        Dim ds As DataSet = Context.Cache("SiteMap")
        If ds Is Nothing Then
            ds = Sitemap.GetSiteMapds
        End If

        Dim NodeID As Integer = 0

        For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
            Dim r As DataRow = ds.Tables(0).Rows(i)
            If Not IsDBNull(r("Refpath")) Then
                If r("RefPath").ToString.Length > 0 Then
                    'If Url.EndsWith(r("RefPath").ToString) Then

                    If Url.ToLower.Equals(r("RefPath").ToString.ToLower) Then
                        NodeID = CInt(r("NodeID").ToString)
                        Exit For
                    End If
                End If
            End If
        Next

        Return NodeID
    End Function

    ''' <summary>
    ''' 由NodeKeyword取得NodeID
    ''' </summary>
    ''' <param name="WebForm">WebForm網頁表單</param>
    ''' <returns>單元節點主鍵值NodeID</returns>
    ''' <remarks>呼叫者必須確定QueryString("cnkw") IsNot Nothing</remarks>
    Public Function GetNodeID_byUrl(ByVal WebForm As System.Web.UI.Page) As Integer

        '以網址路徑取得名稱
        Dim Url As String = ""
        Url = WebForm.Request.Path

        Dim ds As DataSet = Context.Cache("SiteMap")
        If ds Is Nothing Then
            ds = Sitemap.GetSiteMapds
        End If

        Dim NodeID As Integer = 0
        Dim RootFlag As Boolean = False
        If Url.Replace(WebForm.Request.ApplicationPath, "").ToLower.Equals("/default.aspx") Then
            RootFlag = True
        End If

        For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
            Dim r As DataRow = ds.Tables(0).Rows(i)
            If IsDBNull(r("ParentNodeID")) And RootFlag Then
                NodeID = r("NodeID").ToString
                Exit For
            Else
                If Not IsDBNull(r("Refpath")) Then
                    If r("RefPath").ToString.Length > 0 Then
                        If Url.ToLower.Equals(r("RefPath").ToString.ToLower) Then
                            NodeID = CInt(r("NodeID").ToString)
                            Exit For
                        End If
                        If Url.EndsWith(r("RefPath").ToString) Then
                            NodeID = CInt(r("NodeID").ToString)
                            Exit For
                        End If
                    End If
                End If
            End If
        Next

        Return NodeID
    End Function


    ''' <summary>
    ''' 取得SiteMap中該節點的RefPath
    ''' </summary>
    ''' <param name="NodeID">單元節點主鍵值NodeID</param>
    ''' <returns>取得該節點之RefPath欄位資料字串</returns>
    ''' <remarks></remarks>
    Public Function GetNodeRefPath(ByVal NodeID As String) As String
        Dim ds_SiteMap As DataSet = CType(Context.Cache("SiteMap"), DataSet)
        If ds_SiteMap Is Nothing Then ds_SiteMap = GetSiteMapds()

        Dim NodeRefPath As String = ""

        For i As Integer = 0 To ds_SiteMap.Tables(0).Rows.Count - 1
            Dim dRow As DataRow = ds_SiteMap.Tables(0).Rows(i)
            If Not (dRow.IsNull("NodeID")) Then

                If CStr(dRow("NodeID")).Equals(NodeID) Then
                    NodeRefPath = CType(dRow("RefPath"), String)
                    Exit For
                End If
            End If
        Next
        Return NodeRefPath
    End Function


    ''' <summary>
    ''' 取得SiteMap中該節點的Target
    ''' </summary>
    ''' <param name="NodeID">單元節點主鍵值NodeID</param>
    ''' <returns>目標視窗Target字串</returns>
    ''' <remarks></remarks>
    Public Function GetNodeTarget(ByVal NodeID As String) As String
        Dim ds_SiteMap As DataSet = CType(Context.Cache("SiteMap"), DataSet)
        If ds_SiteMap Is Nothing Then ds_SiteMap = GetSiteMapds()

        Dim NodeTarget As String = ""
        For i As Integer = 0 To ds_SiteMap.Tables(0).Rows.Count - 1
            Dim dRow As DataRow = ds_SiteMap.Tables(0).Rows(i)
            If Not (dRow.IsNull("NodeID")) Then
                If CStr(dRow("NodeID")).Equals(NodeID) Then
                    NodeTarget = CType(dRow("Target"), String)
                    Exit For
                End If
            End If
        Next
        Return NodeTarget
    End Function


    ''' <summary>
    ''' 取得SiteMap中該節點的PublishType
    ''' </summary>
    ''' <param name="NodeID">單元節點主鍵值NodeID</param>
    ''' <returns>取得該節點之PublishType欄位資料數字</returns>
    ''' <remarks></remarks>
    Public Function GetNodePublishType(ByVal NodeID As String) As Integer
        Dim ds_SiteMap As DataSet = CType(Context.Cache("SiteMap"), DataSet)
        If ds_SiteMap Is Nothing Then ds_SiteMap = GetSiteMapds()

        Dim NodeTarget As String = ""
        For i As Integer = 0 To ds_SiteMap.Tables(0).Rows.Count - 1
            Dim dRow As DataRow = ds_SiteMap.Tables(0).Rows(i)
            If Not (dRow.IsNull("NodeID")) Then
                If CStr(dRow("NodeID")).Equals(NodeID) Then
                    NodeTarget = CInt(dRow("PublishType"))
                    Exit For
                End If
            End If
        Next
        Return NodeTarget
    End Function


    ''' <summary>
    ''' 判斷傳入的單元是否有子單元
    ''' </summary>
    ''' <param name="NodeID">欲查詢之單元節點主鍵值NodeID</param>
    ''' <returns>布林值(True/False)</returns>
    ''' <remarks></remarks>
    Public Function doCheckHaveChildNode(ByVal NodeID As String) As Boolean
        Dim ds As DataSet = Context.Cache("Cache")
        If ds Is Nothing Then ds = GetSiteMapds()
        Dim SelectRow() As DataRow
        SelectRow = ds.Tables(0).Select("NodeID =" & NodeID)
        Dim ret As Boolean = False
        For i As Integer = 0 To SelectRow.Count - 1
            Dim dRow As DataRow = SelectRow(i)
            If IsDBNull(dRow("HaveChildNode")) Then
                Continue For
            End If
            ret = dRow("HaveChildNode")
            Exit For
        Next
        Return ret
    End Function


    ''' <summary>
    ''' 取得傳入之NodeID的子NodeIDs字串
    ''' </summary>
    ''' <returns>子單元之主鍵值NodeID，以逗號分隔</returns>
    ''' <remarks></remarks>
    Public Function GetChildNodesID(ByVal NodeID As Integer) As String
        Dim ds_SiteMap As DataSet = CType(Context.Cache("SiteMap"), DataSet)
        If ds_SiteMap Is Nothing Then ds_SiteMap = Sitemap.GetSiteMapds()

        Dim ChildNodesID As String = ""

        For i As Integer = 0 To ds_SiteMap.Tables(0).Rows.Count - 1
            Dim dRow As DataRow = ds_SiteMap.Tables(0).Rows(i)
            If Not (dRow.IsNull("ParentNodeID")) Then
                If dRow("ParentNodeID") = NodeID Then
                    ChildNodesID += CType(dRow("NodeID"), String) & ","
                End If
            End If
        Next
        If ChildNodesID.Length > 0 Then
            ChildNodesID = ChildNodesID.Substring(0, ChildNodesID.Length - 1)
        End If

        Return ChildNodesID
    End Function


    ''' <summary>
    ''' 取得現在瀏覽的網頁單元，其在網站地圖SiteMap中的階層深度（首頁Level為0）
    ''' </summary>
    ''' <param name="WebForm"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetNowNodeLevel(ByVal WebForm As System.Web.UI.Page) As Integer
        Dim NodeLevel As Integer
        Dim ds_SiteMap As DataSet = CType(Context.Cache("SiteMap"), DataSet)
        If ds_SiteMap Is Nothing Then ds_SiteMap = Sitemap.GetSiteMapds()
        Dim NodeID As Integer = GetNodeID_Auto(WebForm)
        For i As Integer = 0 To ds_SiteMap.Tables(0).Rows.Count - 1
            Dim dRow As DataRow = ds_SiteMap.Tables(0).Rows(i)
            If IsDBNull(dRow("NodeLevel")) Then
                Continue For
            End If
            If dRow("NodeID") = NodeID Then
                NodeLevel = CInt(dRow("NodeLevel"))
                Exit For
            End If
        Next

        Return NodeLevel
    End Function


    ''' <summary>
    ''' 取得父資料列的NodeID
    ''' </summary>
    ''' <returns>取得父層NodeID</returns>
    ''' <remarks></remarks>
    Public Function GetParentNodeID(ByVal NodeID As Integer) As String
        Dim ds As DataSet = CType(Context.Cache("SiteMap"), DataSet)
        If ds Is Nothing Then ds = Sitemap.GetSiteMapds()
        Dim ParentNodeID As Integer = 0

        For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
            Dim r As DataRow = ds.Tables(0).Rows(i)
            If CInt(r("NodeID")) = NodeID Then
                If Not IsDBNull(r("ParentNodeID")) Then
                    ParentNodeID = CInt(r("ParentNodeID"))
                End If
                Exit For
            End If
        Next
        Return ParentNodeID
    End Function

#Region "Nav Bar"


    ''' <summary>
    ''' 建立後台橫向式-階層式導覽列。
    ''' </summary>
    Public Function BuildNavBar(ByVal Webform As System.Web.UI.Page) As String
        If Webform.Request.Params("cnhb") IsNot Nothing Then
            Dim HomeText As String = "首頁"
            If Not System.Configuration.ConfigurationManager.AppSettings("NavigationHomeText") Is Nothing Then
                HomeText = RemoveXSS(System.Configuration.ConfigurationManager.AppSettings("NavigationHomeText").ToString)
            End If
            Return "<a href=""" & PathManager.ApplicationUrl & "Default.aspx"" >" & HomeText & "</a>"
        End If
        Dim ds As DataSet = Context.Cache("SiteMap")
        If ds Is Nothing Then
            ds = Sitemap.GetSiteMapds
        End If

        Dim NodeID As Integer
        NodeID = Sitemap.GetNodeID_Auto(Webform)

        Dim NodeIDSeril As String = ""


        For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
            Dim dtRow As DataRow = ds.Tables(0).Rows(i)
            If IsDBNull(dtRow("NodeID")) Then
                Continue For
            End If
            If CInt(dtRow("NodeID")).Equals(NodeID) Then
                'NodeIDSeril = dtRow("Text").ToString & "||"
                'NodeIDSeril = "<a href=""" & dtRow("RefPath") & "?cnid=" & dtRow("NodeID").ToString & """ title=""" & IIf(IsDBNull(dtRow("TextEn")), dtRow("Text"), dtRow("TextEn")) & """>" & dtRow("Text") & "</a>"
                NodeIDSeril = "<a href=""" & dtRow("RefPath")
                If dtRow("RefPath").ToString.IndexOf("cnid") < 0 And dtRow("RefPath").ToString.IndexOf("http://") < 0 And dtRow("RefPath").ToString.IndexOf("https://") < 0 Then
                    If dtRow("RefPath").ToString.IndexOf("cnid") < 0 Then
                        If dtRow("RefPath").ToString.IndexOf("?") >= 0 Then
                            NodeIDSeril += "&cnid=" & dtRow("NodeID").ToString
                        Else
                            NodeIDSeril += "?cnid=" & dtRow("NodeID").ToString
                        End If
                    End If
                End If
                NodeIDSeril += """ title=""" & IIf(IsDBNull(dtRow("TextEn")), dtRow("Text"), IIf(Trim(dtRow("TextEn").ToString).Equals(""), dtRow("Text"), dtRow("TextEn"))) & """>" & dtRow("Text") & "</a>"
                If Not IsDBNull(dtRow("ParentNodeID")) Then
                    NodeIDSeril = GetNavBarParentRow(dtRow, ds, NodeIDSeril)
                End If
            End If
        Next
        Dim HomeText1 As String = "首頁"
        If NodeIDSeril = "" And Not System.Configuration.ConfigurationManager.AppSettings("NavigationHomeText") Is Nothing Then
            HomeText1 = System.Configuration.ConfigurationManager.AppSettings("NavigationHomeText").ToString
            NodeIDSeril = "<a href=""" & PathManager.ApplicationUrl & "Default.aspx"" >" & HomeText1 & "</a>"
        End If

        'NodeIDSeril = NodeIDSeril  '"&raquo;" & NodeIDSeril
        Return NodeIDSeril
    End Function



    ''' <summary>
    ''' 內部使用，取得父資料列。
    ''' </summary>
    ''' <param name="dtRow">DataRow物件</param>
    ''' <param name="ds">DataSet物件</param>
    ''' <param name="sb">StringBuilder物件</param>
    ''' <returns>以HTML輸出取得父層名稱，並提供其超連結字串</returns>
    ''' <remarks></remarks>
    Private Function GetNavBarParentRow(ByVal dtRow As DataRow, ByRef ds As DataSet, ByVal sb As String) As String

        For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
            Dim r As DataRow = ds.Tables(0).Rows(i)
            If r Is dtRow.GetParentRow("NodeRelation2") Then
                If Not CInt(r("NodeLevel").ToString) = 0 Then
                    Dim tmpsb As String = ""
                    tmpsb = "<a href=""" & r("RefPath")
                    If r("RefPath").ToString.IndexOf("cnid") < 0 And r("RefPath").ToString.IndexOf("http://") < 0 And r("RefPath").ToString.IndexOf("https://") < 0 Then
                        If r("RefPath").ToString.IndexOf("cnid") < 0 Then
                            If r("RefPath").ToString.IndexOf("?") >= 0 Then
                                tmpsb += "&cnid=" & r("NodeID").ToString
                            Else
                                tmpsb += "?cnid=" & r("NodeID").ToString
                            End If
                        End If
                    End If

                    'sb = "<a href=""" & r("RefPath") & "?cnid=" & r("NodeID").ToString & """ title=""" & IIf(IsDBNull(r("TextEn")), r("Text"), r("TextEn")) & """>" & r("Text") & "</a>" & "&nbsp;&raquo;" & sb
                    'sb = "<a href=""" & r("RefPath") & "?cnid=" & r("NodeID").ToString & """ title=""" & IIf(IsDBNull(r("TextEn")), r("Text"), IIf(Trim(r("TextEn").ToString).Equals(""), r("Text"), r("TextEn"))) & """>" & r("Text") & "</a>" & "&nbsp;&raquo;" & sb
                    sb = tmpsb & """ title=""" & IIf(IsDBNull(r("TextEn")), r("Text"), IIf(Trim(r("TextEn").ToString).Equals(""), r("Text"), r("TextEn"))) & """>" & r("Text") & "</a>" & "&nbsp;&raquo;" & sb
                    sb = GetNavBarParentRow(r, ds, sb)
                Else
                    Dim HomeText As String = "首頁"
                    If Not System.Configuration.ConfigurationManager.AppSettings("NavigationHomeText") Is Nothing Then
                        HomeText = RemoveXSS(System.Configuration.ConfigurationManager.AppSettings("NavigationHomeText").ToString)
                    End If
                    'sb = "<a href=""" & PathManager.ApplicationUrl & "Default.aspx"" title=""" & IIf(IsDBNull(r("TextEn")), r("Text"), r("TextEn")) & """>" & HomeText & "</a>" & "&nbsp;&raquo;" & sb
                    sb = "<a href=""" & PathManager.ApplicationUrl & "Default.aspx"" title=""" & IIf(IsDBNull(r("TextEn")), r("Text"), IIf(Trim(r("TextEn").ToString).Equals(""), r("Text"), r("TextEn"))) & """>" & HomeText & "</a>" & "&nbsp;&raquo;" & sb
                End If
            End If
        Next
        Return sb
    End Function


    ''' <summary>
    ''' 取得SiteMap中該節點的RefPath
    ''' </summary>
    ''' <param name="NodeID">單元節點主鍵值NodeID</param>
    ''' <returns>取得該節點之RefPath欄位資料字串</returns>
    ''' <remarks></remarks>
    Public Function GetNodeInfo(ByVal NodeID As String) As DataRow
        Dim ds_SiteMap As DataSet = CType(Context.Cache("SiteMap"), DataSet)
        If ds_SiteMap Is Nothing Then ds_SiteMap = GetSiteMapds()
        Dim NodeRefPath As DataRow
        For i As Integer = 0 To ds_SiteMap.Tables(0).Rows.Count - 1
            Dim dRow As DataRow = ds_SiteMap.Tables(0).Rows(i)
            If Not (dRow.IsNull("NodeID")) Then
                If CStr(dRow("NodeID")).Equals(NodeID) Then
                    NodeRefPath = dRow
                    Exit For
                End If
            End If
        Next
        Return NodeRefPath
    End Function
    ''' <summary>
    ''' 取得SiteMap中該節點的RefPath
    ''' </summary>
    ''' <param name="NodeID">單元節點主鍵值NodeID</param>
    ''' <returns>取得該節點之RefPath欄位資料字串</returns>
    ''' <remarks></remarks>
    Public Function GetNodeTitlePic(ByVal NodeID As Integer) As DataTable
        Dim sql As String = "select TitlePic, TitlePic2, TitlePic3 from SiteMap where NodeID = @NodeID"
        'sql = sql.Replace("@NodeID", "'" & NodeID & "'")
        Dim dt As DataTable = ClassDB.RunReturnDataTable(sql, New SqlParameter("@NodeID", NodeID))
        If dt.Rows.Count > 0 Then
            Return dt
        End If
        Return New DataTable()
    End Function
    ''' <summary>
    ''' 取得SiteMap中該節點的RefPath
    ''' </summary>
    ''' <param name="NodeID">單元節點主鍵值NodeID</param>
    ''' <returns>取得該節點之RefPath欄位資料字串</returns>
    ''' <remarks></remarks>
    Public Function GetcnhbTitlePic(ByVal GroupID As Integer) As DataTable
        Dim sql As String = "select TitlePic from SiteMapGroupCatgry where GroupID = @GroupID"
        'sql = sql.Replace("@NodeID", "'" & NodeID & "'")
        Dim dt As DataTable = ClassDB.RunReturnDataTable(sql, New SqlParameter("@GroupID", GroupID))
        If dt.Rows.Count > 0 Then
            Return dt
        End If
        Return New DataTable()
    End Function
    ''' <summary>
    ''' 由NodeID取得節點名稱
    ''' </summary>
    ''' <param name="NodeID">單元節點資料之主鍵值NodeID</param>
    ''' <returns>單元節點名稱</returns>
    ''' <remarks></remarks>
    Public Function GetNodeText_byCNHB(ByVal GroupID As Integer) As String

        Dim NodeText As String = ""
        Using dr As SqlDataReader = ClassDB.GetDataReaderParam("Select GroupName from SitemapGroupCatgry where GroupID = @GroupID", New SqlParameter("@GroupID", GroupID))
            Try
                If dr IsNot Nothing Then
                    If dr.Read Then
                        NodeText = RemoveXSS(dr("GroupName"))
                    End If
                End If
            Catch ex As Exception
            End Try
        End Using
        Return NodeText
    End Function
#End Region

    Dim ThisSiteID As String = CInt(ConfigurationManager.AppSettings("SiteID"))
    ''' <summary>
    ''' 檢查網頁是否存在
    ''' </summary>
    ''' <returns>取得父層webform</returns>
    ''' <remarks></remarks>
    Public Function CheckSiteMap(ByVal Webform As System.Web.UI.Page) As Boolean
        Dim ret As Boolean = False
        Dim cnhb As String = Webform.Request.Params("cnhb")
        Dim p As String = Webform.Request.Params("p")

        If IsNumeric(ThisSiteID) Then

            If IsNumeric(p) Then
                ret = True
            End If
            If IsNumeric(cnhb) Then
                Dim Str As String = "Select GroupID from SiteMapGroupCatgry where SiteID = @SiteID and GroupID = @GoupID"
                'Str = Str.Replace("@SiteID", ThisSiteID.ToString).Replace("@GoupID", cnhb.ToString)
                Using dr As SqlDataReader = ClassDB.GetDataReaderParam(Str, New SqlParameter("@SiteID", ThisSiteID), New SqlParameter("@GroupID", cnhb))
                    If dr IsNot Nothing Then
                        If dr.Read Then
                            ret = True
                        End If
                    End If
                End Using
            End If
            Dim cnid As String = Webform.Request.QueryString.Item("cnid")
            If Not IsNumeric(cnid) Then
                cnid = Sitemap.GetNodeID_Auto(Webform)
            End If
            If Not ret And cnid > 0 Then
                Dim Str As String = "Select NodeID from SiteMap where SiteID = @SiteID and NodeID = @NodeID"
                'Str = Str.Replace("@SiteID", ThisSiteID.ToString).Replace("@NodeID", cnid.ToString)
                Using dr As SqlDataReader = ClassDB.GetDataReaderParam(Str, New SqlParameter("@SiteID", ThisSiteID), New SqlParameter("@NodeID", cnid))
                    If dr IsNot Nothing Then
                        If dr.Read Then
                            ret = True
                        End If
                    End If
                End Using
            Else
                ret = True
            End If
        End If
        If Not ret Then
            ScriptManager.RegisterClientScriptBlock(Webform, GetType(Page), "warning", "alert('找不到這個單元!!');document.location.href='" & PathManager.ApplicationUrl & "Default.aspx';", True)
        End If
        Return ret
    End Function

End Module
