Imports System.Data
Imports System.Data.SqlClient

''' <summary>
''' 2008/2/19
''' 多向發佈的前台3.0版，因為OpenFind沒有辦法查到第2頁以後
''' 所以要自訂分頁
''' </summary>
''' <remarks></remarks>
Partial Class common_Publisher4
    Inherits System.Web.UI.UserControl

    Private _DisplayDateColumn As Boolean = False
    Private _DisplayTableStyle As Integer = 0
    Private _ErrorMessage As String = "<div id=""PublishContent""><br />" & GlobalResourcesCulture.GetGlobalResourcesString("InfoNoArticle") & "</div>"
    Private _HeaderText_Date As String = "Publish Date"
    Private _HeaderText_Subject As String = "Articles List"
    Private _DisplayShowHeader As Boolean = True
    Private _RowsCount As Integer = 10
    Private _SpecificNodeSource As String = ""
    Private _SpecificNodeStoredProcedure As String = ""
    Private _CssClassStartString As String = ""    '允許訂定額外的CssClass，藉由改變起始字串，方便在一個畫面內呈現多種樣式
    Private _More As String = GlobalResourcesCulture.GetGlobalResourcesString("InfoMoreArticle")
    Private _PublishInfoID As Integer
    Private _NewPageIndex As Integer
    Private _newsNewDate As Integer = 3 '顯示new的天數

    ''' <summary>
    ''' 頁面讀取事件
    ''' </summary>
    ''' <param name="sender">觸發本事件控制項</param>
    ''' <param name="e">觸發本事件帶進之參數</param>
    ''' <remarks></remarks>

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        '決定是否顯示日期欄位
        Me.gv.Attributes("summary") = "Article List Table"
        '//------屬性設定------------------------------------------------------------------------------
        'Me.gv.Columns(0).Visible = _DisplayDateColumn
        'Me.gv.BorderWidth = Unit.Pixel(_DisplayTableStyle)
        'Me.gv.Columns(0).HeaderText = _HeaderText_Date
        'Me.gv.Columns(1).HeaderText = _HeaderText_Subject
        Me.gv.ShowHeader = _DisplayShowHeader

        If Not IsPostBack Then
            '換頁
            If Not Request.Params("i") Is Nothing Then
                Try
                    Me.gv.PageIndex = CInt(Request.Params("i")) - 1
                Catch ex As Exception
                    Me.gv.PageIndex = 0
                End Try
            End If


            '文章清單/內容判斷與執行
            doBindPublish()
        End If

    End Sub


#Region "Publisher Area"

    ''' <summary>
    ''' 文章清單/內容判斷與執行
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub doBindPublish()
        ''wl("common_Publisher-doBindPublish", "_PublishInfoID=" & Me.DropDownList1.SelectedValue & "Request.Cookies(PublishInfoID)=" & Request.Cookies("PublishInfoID").Value)
        If Not IsNumeric(Request.Params("p")) And Not IsNumeric(Request.Params("cnid")) And Request.Params("cnkw") Is Nothing And Request.Params("cnhb") Is Nothing Then
            '在多向上稿前端，沒有以上3個參數者，強制返回首頁
            Server.Transfer("Default.aspx")
        Else
            Dim cnid As String = Request.Params("cnid")
            Dim pic As String = Request.Params("p")

            '在多向上稿前端，有參數
            If Request.Params("cnkw") IsNot Nothing And Not IsNumeric(cnid) Then    '只有在網址存在cnkw沒有cnid時才轉址
                '只有cnkw(NodeKeyword)，必須進行轉址，轉到正確的NodeID
                'doNodeKeywordTransfer()
                'Chris Chu 20090709 新增首頁區塊單元more功能
            ElseIf Request.Params("cnhb") IsNot Nothing And Not IsNumeric(cnid) Then    '只有在網址存在cnkw沒有cnid時才轉址
                Me.gv.AllowPaging = True
                _RowsCount = 10
                BindGridHomeBlockNode(RemoveXSS((Request.QueryString.Item("cnhb"))))

                Me.PublisherDetail.Visible = False
                Me.tblOne.Visible = False
                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''' 20090709
            Else
                If IsNumeric(pic) Then
                    '----------------------------------------------------------------
                    '有PublicID，是點選了文章，直接秀文章內容
                    '----------------------------------------------------------------
                    ReadOne()
                ElseIf IsNumeric(cnid) Then
                    cnid = CInt(cnid).ToString
                    '----------------------------------------------------------------
                    '無PublicID，是點選了選單
                    '----------------------------------------------------------------
                    If doCheckHaveChildNode(CInt(cnid)) Then
                        '------------------------------------------------------------
                        '子單元秀本單元文章清單、秀分區子單元文章清單
                        '------------------------------------------------------------

                        If getListCount() = 1 Then   '文章數 = 1
                            '同步顯示該單元與清單
                            ReadOne(cnid)
                            BindGridSpecificNode()  '這會讀取子單元列表
                        ElseIf getListCount() > 1 Then  '文章數 > 1 有多筆資料,則呈現本單元清單,SubNode Can be Click by MoreItem
                            Me.gv.AllowPaging = True
                            _RowsCount = 10
                            cnid = CInt(cnid).ToString
                            BindGridThisNode(cnid)
                            Me.PublisherDetail.Visible = False
                            Me.tblOne.Visible = False
                        Else
                            'wl("", "22222222222")
                            BindGridSpecificNode()
                            Me.PublisherDetail.Visible = False
                            Me.tblOne.Visible = False
                        End If
                    Else
                        '------------------------------------------------------------
                        '無子單元, 秀本單元文章清單
                        '------------------------------------------------------------
                        If getListCount() = 1 Then
                            ReadOne(CInt(cnid))
                        Else
                            Me.gv.AllowPaging = True

                            _RowsCount = 10
                            cnid = CInt(cnid).ToString
                            BindGridThisNode(cnid)
                            Me.PublisherDetail.Visible = False
                            Me.tblOne.Visible = False
                        End If
                    End If
                Else
                    '在多向上稿前端，沒有以上3個參數者，強制返回首頁
                    Server.Transfer("Default.aspx")
                End If
            End If
        End If
    End Sub


    '''' <summary>
    '''' 轉換多向上稿頁到NodeID
    '''' </summary>
    '''' <remarks></remarks>
    'Private Sub doNodeKeywordTransfer()
    '    Dim Keyword As String = Request.Params("cnkw").ToString
    '    Keyword = RemoveXSS(Keyword)
    '    Dim NodeID As Integer = Sitemap.GetNodeID_byNodeKeyword(Keyword)
    '    Dim Url As String = ""
    '    If NodeID > 0 Then
    '        Url = "~/Publish.aspx?cnid=" & NodeID
    '    Else
    '        Url = "~/Default.aspx"
    '    End If
    '    Response.Redirect(Url)
    'End Sub


    ''' <summary>
    ''' 取得該Node(網址參數)的文章清單數量
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function getListCount() As Integer
        Dim cnid As String = Request.Params("cnid")
        If IsNumeric(cnid) Then
            Dim retVal As Integer = ClassDB.RunSPReturnInteger("Net2_Get_DocCount_byNodeID",
        New SqlParameter("@NodeID", CInt(cnid)))
            Return retVal
        End If


        Return 0
    End Function


    '''' <summary>
    '''' 取得本NodeID所有的文章數
    '''' </summary>
    '''' <param name="NodeID"></param>
    '''' <returns></returns>
    '''' <remarks></remarks>
    'Private Function getListCountByNodeID(ByVal NodeID As String) As Integer

    '    Dim retVal As Integer = ClassDB.RunSPReturnInteger("Net2_Get_DocCount_byNodeID", _
    '    New SqlParameter("@NodeID", NodeID))

    '    Return retVal
    'End Function

    ''' <summary>
    ''' 文章清單-本節點
    ''' </summary>
    ''' <param name="ThisNodeSource"></param>
    ''' <remarks></remarks>
    Private Sub BindGridThisNode(ByVal ThisNodeSource As String)
        Dim pagestart As Integer = 0
        Dim j As Integer = 0
        '取得DateSet
        Me.gv.Visible = True
        Me.tblList.Visible = True
        Dim ds As DataSet = GetDsThisNode(ThisNodeSource)

        '如沒有資料(包含NodeID傳錯)須停止執行~
        If ds.Tables(0).Rows.Count = 0 Then
            Me.ltl_dgMessage.Text = _ErrorMessage
            Me.gv.Visible = False
            Me.tblList.Visible = False
            Exit Sub
        End If

        Dim dt As New DataTable
        dt = ds.Tables(0).Clone '建立一個欄位結構相同的DataTable

        '單元標題列
        Dim NodeRefPath() As String = Split(GetNodeRefPath(ThisNodeSource), ",")
        If NodeRefPath.Length < 2 Then
            Response.Redirect("~/Default.aspx")
        End If
        'Dim nRow0 As DataRow = dt.NewRow
        'nRow0("Subject") = GetNodeName(ThisNodeSource)
        'nRow0("RefPath") = NodeRefPath(0)   '"Publish.aspx?cnid=" & SpecificNodeIDs(i) '==>要作出單元的連結（多向與超連結）
        'nRow0("Target") = NodeRefPath(1)
        'nRow0("AttFiles") = ""
        'nRow0("UpdateDateTime") = Date.Today
        'nRow0("PublishDate") = Date.Today.AddDays(-16)
        'nRow0("ListType") = 1
        'dt.Rows.Add(nRow0)

        '文章清單列(由DataSet的資料來源複製到DataTable)
        For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
            Dim dRow As DataRow = ds.Tables(0).Rows(i)
            If IsDBNull(drow("Subject")) Then
                Continue For
            End If
            Dim nRow As DataRow = dt.NewRow
            pagestart += 1
            If pagestart Mod 10 = 1 Then
                'nRow("Subject") = "<img src=""images/icon_04.gif"" alt=""單元標題圖示"" style=""border-width:0px;"" /><a title=""" & GetNodeName(SpecificNodeIDs(i)) & """ href=""/Publish.aspx?cnid=" & SpecificNodeIDs(i) & """ target=""" & NodeRefPath(1) & """>" & GetNodeName(SpecificNodeIDs(i)) & " </a> <br />"
                nRow("NodeName") = "<img src=""images/icon_04.gif"" alt=""單元標題圖示"" style=""border-width:0px;"" /><a title=""" & GetNodeName(ThisNodeSource) & """ href=""Publish.aspx?cnid=" & ThisNodeSource & """ target=""" & NodeRefPath(1) & """>" & GetNodeName(ThisNodeSource) & "</a>"
                nRow("Subject") = RemoveXSS(nRow("Subject") & dRow("Subject").ToString) '& ds.Tables(0).Rows(j).Item("Subject").ToString
            Else
                nRow("Subject") = RemoveXSS(dRow("Subject").ToString) 'ds.Tables(0).Rows(j).Item("Subject").ToString
            End If

            nRow("SubjectLink") = RemoveXSS(dRow("SubjectLink").ToString) '標題
            Dim refpath As String = drow("RefPath").ToString.Substring(0, drow("RefPath").ToString.IndexOf("?"))
            '20090210 修改第四層選單為URL連結時加上cnid的錯誤
            nRow("RefPath") = refpath & "?cnid=" & ThisNodeSource & "&p=" & drow("PublicID").ToString 'drow("RefPath").ToString '路徑 2008/2/22修改為程式計算而非由資料庫取得，因多向有問題

            nRow("AttFiles") = drow("AttFiles").ToString  '附檔
            nRow("UpdateDateTime") = drow("UpdateDateTime").ToString
            nRow("PublishDate") = (Convert.ToDateTime(dRow("PublishDate")).Year - 1911) & Convert.ToDateTime(dRow("PublishDate")).ToString("-MM-dd")
            nRow("ListType") = 2
            dt.Rows.Add(nRow)
        Next

        Me.gv.DataSource = dt
        Me.gv.DataBind()

        BindPager(dt)
    End Sub

    ''' <summary>
    ''' 取得本單元文章清單
    ''' </summary>
    ''' <returns>DataSet</returns>
    ''' <remarks></remarks>
    Private Function GetDsThisNode(ByVal ThisNodeSource As String) As DataSet

        Dim ds As DataSet
        ds = ClassDB.RunSPReturnDataSet("Net2_Publisher_AllList_ThisNode", "ds_Publisher", _
        New SqlParameter("@CurrentNodeID", ThisNodeSource))

        'Subject, RefPath, UpdateDateTime, AttFiles, Text(單元文字）, ListType

        Return ds
    End Function


    ''' <summary>
    ''' 製作清單-取自特定的(數個)單元Node
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub BindGridSpecificNode()
        Dim ds As DataSet
        Dim SpecificNodeIDs() As String
        If _SpecificNodeSource <> "" Then
            '使用的頁面有指定特定的NodeID
            SpecificNodeIDs = Split(_SpecificNodeSource, ",")
        Else
            '自網址參數取得NodeId，再由副程式取得其子單元
            Dim cnid As String = Request.Params("cnid")
            If Not IsNumeric(Request.Params("cnid")) Then
                cnid = 0
            End If
            SpecificNodeIDs = Split(Sitemap.GetChildNodesID(CInt(cnid)), ",")
        End If
        Dim pagestart As Integer = 0


        Dim dt As New DataTable '整合不同單元的

        Dim i As Integer
        For i = 0 To UBound(SpecificNodeIDs)
            ds = GetDsThisNode(SpecificNodeIDs(i))
            If i = 0 Then dt = ds.Tables(0).Clone '先複製相同欄位

            Dim j As Integer

            '單元標題列
            Dim NodeRefPath() As String = Split(GetNodeRefPath(SpecificNodeIDs(i)), ",")
            If NodeRefPath.Length < 2 Then
                Response.Redirect("~/Default.aspx")
            End If
            'Dim nRow0 As DataRow = dt.NewRow
            'nRow0("Subject") = GetNodeName(SpecificNodeIDs(i))
            'nRow0("RefPath") = NodeRefPath(0)   '"Publish.aspx?cnid=" & SpecificNodeIDs(i) '==>要作出單元的連結（多向與超連結）
            'nRow0("Target") = NodeRefPath(1)
            'nRow0("AttFiles") = ""
            'nRow0("UpdateDateTime") = Date.Today
            'nRow0("ListType") = 1
            'nRow0("PublishDate") = Date.Today.AddDays(-16)
            'dt.Rows.Add(nRow0)

            '文章清單列
            'Dim drow As DataRow
            Dim ThisRowsCount As Integer
            If ds.Tables(0).Rows.Count > _RowsCount Then
                ThisRowsCount = _RowsCount
            Else
                ThisRowsCount = ds.Tables(0).Rows.Count
            End If
            For j = 0 To ThisRowsCount - 1
                If IsDBNull(ds.Tables(0).Rows(j).Item("Subject")) Then
                    Continue For
                End If
                Dim nRow As DataRow = dt.NewRow
                pagestart += 1
                If j = 0 Or pagestart Mod 10 = 1 Then
                    'nRow("Subject") = "<img src=""images/icon_04.gif"" alt=""單元標題圖示"" style=""border-width:0px;"" /><a title=""" & GetNodeName(SpecificNodeIDs(i)) & """ href=""/Publish.aspx?cnid=" & SpecificNodeIDs(i) & """ target=""" & NodeRefPath(1) & """>" & GetNodeName(SpecificNodeIDs(i)) & " </a> <br />"
                    nRow("NodeName") = "<img src=""images/icon_04.gif"" alt=""單元標題圖示"" style=""border-width:0px;"" /><a title=""" & GetNodeName(SpecificNodeIDs(i)) & """ href=""Publish.aspx?cnid=" & SpecificNodeIDs(i) & """ target=""" & NodeRefPath(1) & """>" & GetNodeName(SpecificNodeIDs(i)) & "</a>"
                    nRow("Subject") = RemoveXSS(nRow("Subject") & ds.Tables(0).Rows(j).Item("Subject").ToString)
                Else
                    nRow("Subject") = RemoveXSS(ds.Tables(0).Rows(j).Item("Subject").ToString)
                End If
                nRow("SubjectLink") = RemoveXSS(ds.Tables(0).Rows(j).Item("SubjectLink").ToString)
                'nRow("RefPath") = ds.Tables(0).Rows(j).Item("RefPath").ToString
                Dim refpath As String = ds.Tables(0).Rows(j).Item("RefPath").ToString.Substring(0, ds.Tables(0).Rows(j).Item("RefPath").ToString.IndexOf("?"))
                nRow("RefPath") = refpath & "?cnid=" & SpecificNodeIDs(i) & "&p=" & ds.Tables(0).Rows(j).Item("PublicID").ToString  '路徑 2008/2/22修改為程式計算而非由資料庫取得，因多向有問題

                nRow("AttFiles") = ds.Tables(0).Rows(j).Item("AttFiles").ToString
                nRow("UpdateDateTime") = ds.Tables(0).Rows(j).Item("UpdateDateTime").ToString
                nRow("PublishDate") = ds.Tables(0).Rows(j).Item("PublishDate").ToString
                nRow("ListType") = 2
                dt.Rows.Add(nRow)
            Next

            '更多內容
            '20100601 不要more了，用分頁來代替Chris
            'If ds.Tables(0).Rows.Count > _RowsCount Then
            '    Dim nRow1 As DataRow = dt.NewRow
            '    nRow1("Subject") = _More
            '    nRow1("RefPath") = NodeRefPath(0)
            '    nRow1("AttFiles") = ""
            '    nRow1("UpdateDateTime") = Date.Today
            '    nRow1("ListType") = 3
            '    nRow1("PublishDate") = Date.Today.AddDays(-16)
            '    dt.Rows.Add(nRow1)
            'End If

        Next

        Me.gv.DataSource = dt
        Me.gv.DataBind()

        BindPager(dt, False)
    End Sub
    ''' <summary>
    ''' 取自特定的單元名稱NodeName
    ''' </summary>
    ''' <param name="NodeID">單元編號</param>
    ''' <remarks></remarks>

    Private Function GetNodeName(ByVal NodeID As String) As String
        Dim ds As DataSet = CType(Cache("SiteMap"), DataSet)
        If ds Is Nothing Then ds = GetSiteMapds()

        Dim NodeName As String = ""
        For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
            Dim dRow As DataRow = ds.Tables(0).Rows(i)

            If Not (dRow.IsNull("NodeID")) Then
                If dRow("NodeID") = CType(NodeID, Integer) Then
                    'NodeName = CType(dRow("Text"), String) & IIf(IsDBNull(dRow("TextEn")), "", " (" & dRow("TextEn") & ")")
                    NodeName = RemoveXSS(CType(dRow("Text"), String)) '暫時不要上Chris 20100818 & IIf(IsDBNull(dRow("TextEn")), "", IIf(Trim(dRow("TextEn").ToString).Equals(""), "", " (" & dRow("TextEn") & ")"))

                End If
            End If
        Next
        Return NodeName
    End Function

    ''' <summary>
    ''' 取自特定的單元路徑
    ''' </summary>
    ''' <param name="NodeID">單元編號</param>
    ''' <remarks></remarks>
    Private Function GetNodeRefPath(ByVal NodeID As String) As String
        Dim ds As DataSet = CType(Cache("SiteMap"), DataSet)
        If ds Is Nothing Then ds = GetSiteMapds()

        Dim RefPath As String = "" ' {"", ""}  'RefPath, Target
        'Dim Target As String = ""

        For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
            Dim dRow As DataRow = ds.Tables(0).Rows(i)
            If Not (dRow.IsNull("NodeID")) Then
                If dRow("NodeID") = CType(NodeID, Integer) Then
                    'RefPath = Trim(dRow("RefPath").ToString) & "?cnid=" & NodeID & ","
                    'chris 20090210 選單URL連結
                    If dRow("PublishType").ToString = "2" And (Trim(dRow("RefPath").ToString).IndexOf("http://") = 0 Or Trim(dRow("RefPath").ToString).IndexOf("https://") = 0) Then
                        RefPath = (Trim(dRow("RefPath").ToString)) & ","
                    Else
                        RefPath = (Trim(dRow("RefPath").ToString)) & "?cnid=" & NodeID & ","
                    End If

                    If dRow("Target").ToString = ("_blank") And dRow("PublishType").ToString = "2" Then
                        RefPath += RemoveXSS(dRow("Target").ToString)
                    Else
                    End If
                End If
            End If
        Next


        Return RefPath
    End Function


    ''' <summary>
    ''' 副程式，讀取單篇文章標題及內容
    ''' </summary>
    ''' <param name="GetNewestArticleByNode"></param>
    ''' <remarks></remarks>
    Private Sub ReadOne(Optional ByVal GetNewestArticleByNode As String = "")
        Me.tblList.Visible = False
        PublisherDetail.Visible = True
        Me.tblOne.Visible = True
        Dim PublicID As Integer

        Dim cnid As String = Request.Params("cnid")
        Dim p As String = Request.Params("p")

        Dim str As String = "Net2_F_ReadOne_byNodeID"
        Dim pm As SqlParameter = New SqlParameter
        If GetNewestArticleByNode = "" Then
            '讀取大單元文章列表預存
            If Not IsNumeric(p) Then
                If IsNumeric(cnid) Then
                    str = "Net2_F_ReadOne_byNodeID"
                    pm = New SqlParameter("@NodeID", CInt(cnid))
                End If
            Else
                str = "Net2_F_ReadOne_byPublicID"
                pm = New SqlParameter("@PublicID", CInt(p))
            End If
        Else
            str = "Net2_F_ReadOne_byNodeID"
            pm = New SqlParameter("@NodeID", CInt(GetNewestArticleByNode))
        End If
        Using dr As SqlDataReader = ClassDB.GetDataReaderSP(str, pm)
            Try
                If dr IsNot Nothing Then
                    If dr.Read() Then
                        If GetNewestArticleByNode.Trim.Length = 0 And IsNumeric(cnid) Then
                            GetNewestArticleByNode = cnid
                        End If
                        'Me.ltSubject.Text = "<h2>" & dr("Subject").ToString & "</h2>"
                        Dim subject As String = dr("Subject").ToString
                        subject = (dr("Subject").ToString)
                        Me.ltSubject.Text = subject
                        Dim NodeName As String = GetNodeName(GetNewestArticleByNode)
                        'If dr("Subject").ToString.Equals(NodeName) Then
                        '    Me.ltSubject.Visible = False
                        'End If
                        Me.ltContent.Text = dr("Content").ToString
                        'Me.ltUpdateDateTime.Text = "<span class=""DateTime"">Last Updated Time:" & dr("UpdateDateTime").ToString & "</span>"
                        PublicID = CInt(dr("PublicID"))

                        'Me.Page.Title += "-" & dr("Subject").ToString  '2008/1/24小羅來電,檔案局確定A+取消這個只留網站名稱

                        Me.LabelForPublishDate.Text = GlobalResourcesCulture.GetGlobalResourcesString("LabelForPublishDate")
                        Me.LabelForLastUpdate.Text = GlobalResourcesCulture.GetGlobalResourcesString("LabelForLastUpdate")
                        Me.LabelForViewCount.Text = GlobalResourcesCulture.GetGlobalResourcesString("LabelForViewCount")

                        Dim PublishDate As Date = CType(dr("PublishDate"), Date)
                        Dim UpdateDateTime As Date = CType(dr("UpdateDateTime2"), Date)

                        If GlobalResourcesCulture.GetCurrentCulture() = "zhTW" Then
                            PublishDate = PublishDate.AddYears(-1911)
                            UpdateDateTime = UpdateDateTime.AddYears(-1911)
                            Me.lbPostDate.Text = PublishDate.ToString("yyy-MM-dd", System.Globalization.DateTimeFormatInfo.InvariantInfo)
                            'Me.lbLastUpdate.Text = UpdateDateTime.ToString("yyy.MM.dd tt hh:mm:ss", System.Globalization.DateTimeFormatInfo.InvariantInfo).Replace("AM", "上午").Replace("PM", "下午")
                            Me.lbLastUpdate.Text = UpdateDateTime.ToString("yyy-MM-dd", System.Globalization.DateTimeFormatInfo.InvariantInfo) 'Chris 20090522
                        Else
                            Me.lbPostDate.Text = PublishDate.ToString
                            Me.lbLastUpdate.Text = UpdateDateTime.ToString
                        End If
                        Me.lbViewCount.Text = (dr("ViewCount").ToString)
                        If dr("AttFiles").ToString.Trim.Equals("") Then
                            FileManager1.Visible = False
                        Else
                            If Not CheckSqlInjectionWording(dr.Item("AttFiles")) Then
                                FileManager1.doDataBind((dr.Item("AttFiles")).ToString)
                            End If
                        End If
                    Else
                        Me.ltContent.Text = _ErrorMessage
                    End If
                End If
            Catch ex As Exception
                WriteErrLog(ex, Me.Page)
                Me.ltContent.Text = _ErrorMessage
            Finally

            End Try
        End Using
        '//'//計數器增加
        Try
            If cnid IsNot Nothing Then
                If IsNumeric(p) Then
                    ModuleCounter.ViewCount_Add(PublicID, CInt(cnid))
                Else
                    ModuleCounter.ViewCount_Add(CInt(p), CInt(cnid))
                End If
            Else
                If IsNumeric(p) Then
                    ModuleCounter.ViewCount_Add(CInt(p), 0)
                End If
            End If
        Catch ex As Exception
        End Try

    End Sub

    ''' <summary>
    ''' 自訂頁碼
    ''' </summary>
    ''' <param name="dt">DataTable提供計算資料數使用</param>
    ''' <param name="Show">是否呈現自訂頁碼預設為是，但某些情況可以要求不要顯示</param>
    ''' <remarks></remarks>
    Private Sub BindPager(ByVal dt As DataTable, Optional ByVal Show As Boolean = True)
        If Show = Show Then
            'If Show = False Then
            Dim TotalPageCount As Integer = Math.Ceiling(dt.Rows.Count / Me.gv.PageSize)
            Dim endpage As Integer = IIf(TotalPageCount > 10, 10, TotalPageCount)
            Me.ltlPageCount.Text = "頁碼 " & (Me.gv.PageIndex + 1).ToString & " / " & TotalPageCount.ToString & " 頁 總計 " & dt.Rows.Count.ToString & " 筆資料"
            If Me.gv.PageIndex + 1 > 10 Then
                Me.ltlPager.Text = "<a href=""javascript:__doPostBack('" & gv.ClientID.Replace("_", "$") & "','Page$" & (Me.gv.PageIndex - (Me.gv.PageIndex Mod 10) + 1).ToString & "')"" title=""上十頁"" ><<</a>"
            Else
                Me.ltlPager.Text = ""
            End If
            If Me.gv.PageIndex > 0 Then
                Me.ltlPager.Text = "<a href=""javascript:__doPostBack('" & gv.ClientID.Replace("_", "$") & "','Page$" & (Me.gv.PageIndex - 1 + 1).ToString & "')"" title=""上一頁"" ><</a>"
            Else
                Me.ltlPager.Text = "<span class=""disabled"" title=""上一頁"" ><</span>"
            End If
            'Me.gv.PageIndex = IIf(Me.gv.PageIndex = 0, 1, Me.gv.PageIndex)

            For i As Integer = 1 To endpage
                If i - 1 = Me.gv.PageIndex Then
                    Me.ltlPager.Text &= "<span class=""disabled""><a  class=""current"" title=""第" & i.ToString & """ >" & (i).ToString & "</a/></span>"
                Else
                    Me.ltlPager.Text &= "<a href=""javascript:__doPostBack('" & gv.ClientID.Replace("_", "$") & "','Page$" & (i).ToString & "')"" title=""第" & i.ToString & "頁"" >" & (i).ToString & "</a>"
                End If
            Next
            If Me.gv.PageIndex + 1 = TotalPageCount Then
                Me.ltlPager.Text &= "<span class=""disabled"" title=""下一頁"" >></span>"
            Else
                Me.ltlPager.Text &= "<a href=""javascript:__doPostBack('" & gv.ClientID.Replace("_", "$") & "','Page$" & (Me.gv.PageIndex + 2).ToString & "')"" title=""下一頁"" >></a>"
            End If
            If Me.gv.PageIndex < TotalPageCount - (TotalPageCount Mod 10) + 1 And TotalPageCount > 10 Then
                Me.ltlPager.Text &= "<a href=""javascript:__doPostBack('" & gv.ClientID.Replace("_", "$") & "','ViewItem$" & (Me.gv.PageIndex + (10 - (Me.gv.PageIndex Mod 10)) + 1).ToString & "')"" title=""下十頁"" >>></a>"
            End If

            Exit Sub
        End If
        Dim PageIndex As Integer = 1
        If Request.Params("i") <> "" Then
            Try
                PageIndex = CInt(Request.Params("i"))
            Catch ex As Exception
                PageIndex = 1
            End Try
        End If

        Dim PageURLFormat As String = Request.Url.ToString
        If Request.Params("i") <> "" Then
            If PageURLFormat.IndexOf("?i=") >= 0 Then
                'i的參數在最前面
                PageURLFormat = PageURLFormat.Replace("i=" & Request.Params("i"), "")
            Else
                PageURLFormat = PageURLFormat.Replace("&i=" & Request.Params("i"), "")
            End If

        End If
        If PageURLFormat.IndexOf("?") >= 0 Then
            PageURLFormat = PageURLFormat & "&i={0}"
        Else
            PageURLFormat = PageURLFormat & "?i={0}"
        End If
    End Sub

    ''' <summary>
    ''' 頁面讀取事件-GridView標題列文章設定
    ''' </summary>
    ''' <param name="sender">觸發本事件控制項</param>
    ''' <param name="e">觸發本事件帶進之參數</param>
    ''' <remarks></remarks>
    Protected Sub gv_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles gv.DataBound
        '提供分頁資料呈現 2010/05/26 Chris Modified
        GridViewPageInfo.GetGridViewInfo3(gv, Me.Page, Nothing, True, Me.gv.DataSource)
        
    End Sub
    ''' <summary>
    ''' 問與答清單命令
    ''' </summary>
    ''' <param name="sender">觸發本事件控制項</param>
    ''' <param name="e">觸發本事件帶進之參數</param>
    ''' <remarks>重導頁面至詳細內容頁</remarks>
    Protected Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv.RowCommand
        If e.CommandName = "ViewItem" Then
            Try
                Me.gv.PageIndex = IIf(e.CommandArgument < 0, 0, e.CommandArgument)
                doBindPublish()
                BindPager(Me.gv.DataSource)
            Catch ex As Exception
            End Try
        End If
    End Sub

    ''' <summary>
    ''' GridView換頁(文章清單)
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub gv_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv.PageIndexChanging
        Me.gv.PageIndex = IIf(e.NewPageIndex < 0, 0, e.NewPageIndex)
        '_NewPageIndex = e.NewPageIndex
        doBindPublish()
        Me.gv.PageIndex = IIf(e.NewPageIndex < 0, 0, e.NewPageIndex)
        'Me.gv.DataBind()
        BindPager(Me.gv.DataSource)
    End Sub

    ''' <summary>
    ''' GridView(文章清單按鈕功能設定)
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub gv_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            If e.Row.RowState = DataControlRowState.Alternate OrElse e.Row.RowState = DataControlRowState.Normal Then

                Dim dRow As DataRowView = CType(e.Row.DataItem, DataRowView)

                '箭頭圖示
                Dim imgIcon As Image = New Image
                imgIcon.ImageUrl = "~/images/icon_pen.gif"
                imgIcon.AlternateText = "文章標題圖示"

                '文章標題
                Dim hl_Subject As HyperLink = CType(e.Row.FindControl("hl_Subject"), HyperLink)
                Dim wordlimit As Integer = 35
                If IsNumeric(System.Configuration.ConfigurationManager.AppSettings("PublishSubjectLimit")) Then
                    wordlimit = System.Configuration.ConfigurationManager.AppSettings("PublishSubjectLimit").ToString
                End If
                '20100726 Chris 修改 超連結沒有文章圖示的問題
                Dim subject As String = (dRow("Subject").ToString)
                If dRow("Subject").ToString.StartsWith("<a href") Then
                    hl_Subject.Text = subject
                Else
                    hl_Subject.Text = ModuleMisc.LimitWord(subject, wordlimit)
                    hl_Subject.ToolTip = subject
                End If

                'new圖示
                Dim imgIconNew As Image = New Image
                imgIconNew.ImageUrl = "~/images/new_icon.gif"
                imgIconNew.AlternateText = "最新文章圖示"
                'Dim PublishDate As Date = CType(dRow.Item("PublishDate"), Date)
                'If DateDiff(DateInterval.Day, PublishDate, Date.Today) <= _newsNewDate Then
                '    imgIconNew.Visible = True
                'Else
                '    imgIconNew.Visible = False
                'End If

                If dRow.Item("ListType") = "1" Then
                    '單元標題
                    hl_Subject.NavigateUrl = dRow("RefPath") '單元標題不用再加應用程式路徑

                    If dRow("Target").ToString = "_blank" Then
                        hl_Subject.Target = RemoveXSS(dRow("Target").ToString)
                    End If

                    Dim imgIconNode As Image = New Image
                    imgIconNode.ImageUrl = "~/images/icon_04.gif"
                    imgIconNode.AlternateText = "單元標題圖示"
                    e.Row.Cells(0).Controls.Add(imgIconNode)
                    e.Row.Cells(0).Controls.Add(hl_Subject)
                    e.Row.Cells(0).ColumnSpan = 5
                    e.Row.Cells(1).Visible = False
                    e.Row.Cells(2).Visible = False
                    e.Row.Cells(3).Visible = False
                    e.Row.Cells(4).Visible = False

                    e.Row.Cells(0).CssClass = "dg_List_cell_node"

                ElseIf dRow.Item("ListType") = "2" Then
                    e.Row.Cells(0).Text = dRow("NodeName").ToString

                    '文章標題
                    'e.Row.Cells(1).Controls.Add(imgIcon)
                    '20100726 Chris 修改 超連結沒有文章圖示的問題
                    '20100726 Chris 修改 超連結沒有文章圖示的問題
                    If dRow("Subject").ToString.StartsWith("<a href") Then
                        hl_Subject.NavigateUrl = dRow("Subject").ToString
                        e.Row.Cells(1).Controls.Remove(hl_Subject)
                        Dim LiteralSubject As Literal = CType(e.Row.FindControl("LiteralSubject"), Literal)
                        LiteralSubject.Text = dRow("Subject").ToString
                        e.Row.Cells(1).Controls.Add(LiteralSubject)
                    Else
                        hl_Subject.NavigateUrl = PathManager.ApplicationUrl & dRow("RefPath")
                        e.Row.Cells(1).Controls.Add(hl_Subject)
                    End If


                    'If dRow("Subject").ToString.StartsWith("<a href") Then
                    'Dim LiteralSubject As Literal = CType(e.Row.FindControl("LiteralSubject"), Literal)
                    'LiteralSubject.Text = dRow("Subject").ToString

                    'Else
                    'hl_Subject.NavigateUrl = PathManager.ApplicationUrl & dRow("RefPath")
                    'e.Row.Cells(1).Controls.Add(hl_Subject)
                    'End If
                    'e.Row.Cells(1).Controls.Add(imgIconNew)
                    'If Not IsDBNull(dRow.Item("AttFiles")) Then
                    '    If dRow.Item("AttFiles").ToString.Length > 0 Then
                    '        Dim FileManager2 As common_FileManager = CType(e.Row.FindControl("FileManager2"), UserControl)
                    '        If IsNumeric(System.Configuration.ConfigurationManager.AppSettings("PublishAttachLimit")) Then
                    '            FileManager2.AttFile_WordLimit = System.Configuration.ConfigurationManager.AppSettings("PublishAttachLimit").ToString
                    '        End If
                    '        FileManager2.doDataBind(dRow.Item("AttFiles").ToString)
                    '        Dim FileManagerCounter2 As common_FileManagerCounter = CType(e.Row.FindControl("FileManagerCounter2"), UserControl)
                    '        FileManagerCounter2.doDataBind(dRow.Item("AttFiles").ToString)
                    '    End If
                    'End If
                    'e.Row.Cells(3).Text = dRow.Item("UpdateDateTime").ToString.Substring(0, dRow.Item("UpdateDateTime").ToString.IndexOf(" ")) '4.27文芳要求修改只留日期，.ToString.LastIndexOf(":") 前端最後更新時間改成更新日期)
                    'e.Row.Cells(3).Text = dRow.Item("UpdateDateTime").ToString
                    Dim UpdateDateTime As Date = CType(dRow("UpdateDateTime"), Date) 'Chris Chu 20090522 民國年

                    If GlobalResourcesCulture.GetCurrentCulture() = "zhTW" Then
                        UpdateDateTime = UpdateDateTime.AddYears(-1911)
                        e.Row.Cells(4).Text = UpdateDateTime.ToString("yyy-MM-dd", System.Globalization.DateTimeFormatInfo.InvariantInfo)
                    Else
                        e.Row.Cells(4).Text = UpdateDateTime.ToString
                    End If

                    'e.Row.Cells(0).CssClass = "dg_List_cell0"
                    'e.Row.Cells(1).CssClass = "dg_List_cell1"
                    'e.Row.Cells(2).CssClass = "dg_List_cell2_20080427" '4.27文芳要求修改只留日期，dg_List_cell2 前端最後更新時間改成更新日期)
                    'e.Row.Cells(3).CssClass = "dg_List_cell4_20080427"
                    'e.Row.Cells(4).CssClass = "dg_List_cell3"
                Else
                    '更多文章
                    hl_Subject.NavigateUrl = dRow("RefPath")

                    e.Row.Cells(1).Controls.Add(hl_Subject)
                    e.Row.Cells(2).Text = ""
                    e.Row.Cells(3).Text = ""
                    e.Row.Cells(4).Text = ""

                    'e.Row.Cells(0).CssClass = "dg_List_cell0"
                    'e.Row.Cells(1).CssClass = "dg_List_more"
                    'e.Row.Cells(2).CssClass = "dg_List_cell2_20080427" '4.27文芳要求修改只留日期，dg_List_cell2 前端最後更新時間改成更新日期)
                    'e.Row.Cells(3).CssClass = "dg_List_cell4_20080427"
                    'e.Row.Cells(4).CssClass = "dg_List_cell3"
                End If
                'chris 20090709 如果是超連結直接hyperlink 另開視窗
                'Chris 20090708 如果標題設定HyperLink 就連到SubjectLink網址
                If Not IsDBNull(dRow("SubjectLink")) Then
                    If dRow("SubjectLink").ToString.Length > 0 Then
                        hl_Subject.NavigateUrl = dRow("SubjectLink").ToString
                        hl_Subject.Target = "_Blank"
                    End If
                End If
            End If
        End If
        'If e.Row.RowType = DataControlRowType.Header Then
        '    'e.Row.Cells(0).HorizontalAlign = HorizontalAlign.Center
        '    e.Row.Visible = False
        'End If
    End Sub


    ''' <summary>
    ''' 設定表格樣式 Setting Css Style
    ''' </summary>
    ''' <param name="gv"></param>
    ''' <remarks></remarks>
    Private Sub SetCss(ByVal gv As GridView)
        gv.CssClass = "newslist"
        gv.PagerSettings.Mode = PagerButtons.Numeric
        gv.PagerSettings.Position = PagerPosition.TopAndBottom
        'gv.FooterStyle.CssClass = "dg_List_Footer"
        'gv.HeaderStyle.CssClass = "dg_List_Header"
        'gv.RowStyle.CssClass = "GvItem"
        'gv.AlternatingRowStyle.CssClass = "GvAltItem"
        'gv.PagerStyle.CssClass = "GvPager"
        'gv.SelectedRowStyle.CssClass = "GvItem_edit"
        gv.HorizontalAlign = HorizontalAlign.Center
        gv.EmptyDataText = "目前沒有資料，或您的查詢沒有資料。"
        gv.CaptionAlign = TableCaptionAlign.Left
        gv.BorderWidth = 0
    End Sub

#End Region


#Region "本使用者控制項-屬性"

    WriteOnly Property DisplayDateColumn() As Boolean
        Set(ByVal Value As Boolean)
            _DisplayDateColumn = Value
        End Set
    End Property

    WriteOnly Property DisplayTableStyle() As Integer
        Set(ByVal Value As Integer)
            _DisplayTableStyle = Value
        End Set
    End Property

    WriteOnly Property HeaderText_Date() As String
        Set(ByVal Value As String)
            _HeaderText_Date = Value
        End Set
    End Property


    WriteOnly Property HeaderText_Subject() As String
        Set(ByVal Value As String)
            _HeaderText_Subject = Value
        End Set
    End Property

    WriteOnly Property DisplayShowHeader() As Boolean
        Set(ByVal Value As Boolean)
            _DisplayShowHeader = Value
        End Set
    End Property

    WriteOnly Property ListRowsCount() As Integer
        Set(ByVal Value As Integer)
            _RowsCount = Value
        End Set
    End Property

    WriteOnly Property SpecificNodeSource() As String
        Set(ByVal Value As String)
            _SpecificNodeSource = Value
        End Set
    End Property


    WriteOnly Property SpecificNodeStoredProcedure() As String
        Set(ByVal Value As String)
            _SpecificNodeStoredProcedure = Value
        End Set
    End Property

    WriteOnly Property CssClassStartString() As String
        Set(ByVal Value As String)
            _CssClassStartString = Value
        End Set
    End Property

#End Region
#Region "HomeBlockMore"
    ''' <summary>
    ''' 文章清單-首頁區塊More清單
    ''' </summary>
    ''' <param name="ThisNodeSource"></param>
    ''' <remarks></remarks>
    Private Sub BindGridHomeBlockNode(ByVal ThisNodeSource As String)
        '取得DateSet
        Me.gv.Visible = True
        Me.tblList.Visible = True
        Dim ds As DataSet = GetDsHomeBlockNode(ThisNodeSource)

        '如沒有資料(包含NodeID傳錯)須停止執行~
        If ds.Tables(0).Rows.Count = 0 Then
            Me.ltl_dgMessage.Text = _ErrorMessage
            Me.gv.Visible = False
            Me.tblList.Visible = False
            Exit Sub
        End If

        Dim dt As New DataTable
        dt = ds.Tables(0).Clone '建立一個欄位結構相同的DataTable

        '單元標題列
        'Dim nRow0 As DataRow = dt.NewRow
        'nRow0("Subject") = GetHomeBlockName(ThisNodeSource)
        'nRow0("SubjectLink") = ""
        'nRow0("RefPath") = ""   '"Publish.aspx?cnid=" & SpecificNodeIDs(i) '==>要作出單元的連結（多向與超連結）
        'nRow0("Target") = ""
        'nRow0("AttFiles") = ""
        'nRow0("UpdateDateTime") = Date.Today
        'nRow0("PublishDate") = Date.Today.AddDays(-16)
        'nRow0("ListType") = 1
        'dt.Rows.Add(nRow0)
        Dim ThisRowsCount As Integer
        ThisRowsCount = ds.Tables(0).Rows.Count
        Dim pagestart As Integer = 0
        '文章清單列(由DataSet的資料來源複製到DataTable)
        For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
            Dim dRow As DataRow = ds.Tables(0).Rows(i)
            If IsDBNull(drow("Subject")) Then
                Continue For
            End If
            Dim nRow As DataRow = dt.NewRow
            pagestart += 1

            If pagestart Mod 10 = 1 Then
                'nRow("Subject") = "<img src=""images/icon_04.gif"" alt=""單元標題圖示"" style=""border-width:0px;"" /><a title=""" & GetNodeName(SpecificNodeIDs(i)) & """ href=""/Publish.aspx?cnid=" & SpecificNodeIDs(i) & """ target=""" & NodeRefPath(1) & """>" & GetNodeName(SpecificNodeIDs(i)) & " </a> <br />"
                nRow("NodeName") = "<img src=""images/icon_04.gif"" alt=""單元標題圖示"" style=""border-width:0px;"" /><a title=""" & GetHomeBlockName(ThisNodeSource) & """ href=""Publish.aspx?cnhb=" & ThisNodeSource & """ >" & GetHomeBlockName(ThisNodeSource) & "</a>"
                nRow("Subject") = (nRow("Subject") & dRow("Subject").ToString)
            Else
                nRow("Subject") = (dRow("Subject").ToString)
            End If
            nRow("SubjectLink") = drow("SubjectLink").ToString '標題
            Dim refpath As String = drow("RefPath").ToString.Substring(0, drow("RefPath").ToString.IndexOf("?"))
            '20090210 修改第四層選單為URL連結時加上cnid的錯誤
            'nRow("RefPath") = refpath & "?cnid=" & ThisNodeSource & "&p=" & drow("PublicID").ToString 'drow("RefPath").ToString '路徑 2008/2/22修改為程式計算而非由資料庫取得，因多向有問題
            nRow("RefPath") = refpath & "?cnid=" & drow("NodeID").ToString & "&p=" & drow("PublicID").ToString 'drow("RefPath").ToString '路徑 2008/2/22修改為程式計算而非由資料庫取得，因多向有問題
            nRow("AttFiles") = drow("AttFiles").ToString  '附檔
            nRow("UpdateDateTime") = drow("UpdateDateTime").ToString
            'nRow0("PublishDate") = drow("PublishDate").ToString
            nRow("PublishDate") = drow("PublishDate").ToString
            nRow("ListType") = 2
            dt.Rows.Add(nRow)
        Next

        Me.gv.DataSource = dt
        Me.gv.DataBind()

        BindPager(dt)
    End Sub
    ''' <summary>
    ''' 取得本單元文章清單
    ''' </summary>
    ''' <returns>DataSet</returns>
    ''' <remarks></remarks>
    Private Function GetDsHomeBlockNode(ByVal ThisNodeSource As String) As DataSet
        Dim SiteID As String = CInt(System.Configuration.ConfigurationManager.AppSettings("SiteID").ToString)
        Dim ds As DataSet
        ds = ClassDB.RunSPReturnDataSet("Net2_F_PublichHomeBlock_List", "ds_homeblock",
        New SqlParameter("@PublishInfoID", 0),
        New SqlParameter("@GroupID", CInt(ThisNodeSource)),
        New SqlParameter("@SiteID", CInt(SiteID)))

        'Subject, RefPath, UpdateDateTime, AttFiles, Text(單元文字）, ListType

        Return ds
    End Function
    ''' <summary>
    ''' 取自特定的單元名稱NodeName
    ''' </summary>
    ''' <param name="NodeID">單元編號</param>
    ''' <remarks></remarks>

    Private Function GetHomeBlockName(ByVal NodeID As String) As String
        Dim NodeName As String = ""
        Using dr As SqlDataReader = ClassDB.GetDataReaderParam("Select GroupName from SiteMapGroupCatgry where GroupID = @GroupID", New SqlParameter("@GroupID", NodeID))
            Try
                If dr IsNot Nothing Then
                    If dr.Read Then
                        NodeName = (dr("GroupName"))
                    End If
                End If
            Catch ex As SqlException
            Finally
            End Try
        End Using
        Return NodeName
    End Function
#End Region

End Class
