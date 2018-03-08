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
    Private _DisplayShowHeader As Boolean = False
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
        If Not IsPostBack Then
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
            '在多向上稿前端，有參數
            Dim cnid As String = Request.Params("cnid")
            Dim pic As String = Request.Params("p")
            If Request.Params("cnkw") IsNot Nothing And Not IsNumeric(cnid) Then    '只有在網址存在cnkw沒有cnid時才轉址
                '只有cnkw(NodeKeyword)，必須進行轉址，轉到正確的NodeID
                'doNodeKeywordTransfer()
                'Chris Chu 20090709 新增首頁區塊單元more功能
            ElseIf IsNumeric(Request.Params("cnhb")) And Not IsNumeric(cnid) Then    '只有在網址存在cnkw沒有cnid時才轉址
                _RowsCount = 10
                BindGridHomeBlockNode(RemoveXSS(Request.QueryString.Item("cnhb")))
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
                    '----------------------------------------------------------------
                    '無PublicID，是點選了選單
                    '----------------------------------------------------------------
                    If doCheckHaveChildNode(CInt(cnid)) Then
                        '------------------------------------------------------------
                        '子單元秀本單元文章清單、秀分區子單元文章清單
                        '------------------------------------------------------------
                        cnid = CInt(cnid).ToString
                        If getListCount() = 1 Then   '文章數 = 1
                            '同步顯示該單元與清單
                            ReadOne(CInt(cnid))
                            BindGridSpecificNode()  '這會讀取子單元列表

                        ElseIf getListCount() > 1 Then  '文章數 > 1 有多筆資料,則呈現本單元清單,SubNode Can be Click by MoreItem
                            _RowsCount = 10
                            BindGridThisNode(CInt(cnid))
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
                            _RowsCount = 10
                            BindGridThisNode(CInt(cnid))
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
    '        'Url = "~/Publish.aspx?cnkw=" & Keyword & "&cnid=" & NodeID
    '        Url = "~/Publish.aspx?cnid=" & NodeID
    '    Else
    '        Url = "~/Default.aspx"
    '    End If

    '    'Server.Transfer(Url)
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
    'Private Function getListCountByNodeID(ByVal NodeID As Integer) As Integer

    '    Dim retVal As Integer = ClassDB.RunSPReturnInteger("Net2_Get_DocCount_byNodeID",
    '    New SqlParameter("@NodeID", NodeID))

    '    Return retVal
    'End Function

    ''' <summary>
    ''' 文章清單-本節點
    ''' </summary>
    ''' <param name="ThisNodeSource"></param>
    ''' <remarks></remarks>
    Private Sub BindGridThisNode(ByVal ThisNodeSource As Integer)
        Dim pagestart As Integer = 0
        Dim j As Integer = 0
        '取得DateSet
        Me.tblList.Visible = True
        Dim ds As DataSet = GetDsThisNode(ThisNodeSource)
        '如沒有資料(包含NodeID傳錯)須停止執行~
        If ds.Tables(0).Rows.Count = 0 Then
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
        '文章清單列(由DataSet的資料來源複製到DataTable)
        For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
            Dim drow As DataRow = ds.Tables(0).Rows(i)
            Dim nRow As DataRow = dt.NewRow
            pagestart += 1
            If pagestart Mod 10 = 1 Then
                'nRow("Subject") = "<img src=""images/icon_04.gif"" alt=""單元標題圖示"" style=""border-width:0px;"" /><a title=""" & GetNodeName(SpecificNodeIDs(i)) & """ href=""/Publish.aspx?cnid=" & SpecificNodeIDs(i) & """ target=""" & NodeRefPath(1) & """>" & GetNodeName(SpecificNodeIDs(i)) & " </a> <br />"
                nRow("NodeName") = "<img src=""images/icon_04.gif"" alt=""單元標題圖示"" style=""border-width:0px;"" /><a title=""" & GetNodeName(ThisNodeSource) & """ href=""Publish.aspx?cnid=" & ThisNodeSource & """ target=""" & NodeRefPath(1) & """>" & GetNodeName(ThisNodeSource) & "</a>"
                nRow("Subject") = RemoveXSS(nRow("Subject") & drow("Subject").ToString) '& ds.Tables(0).Rows(j).Item("Subject").ToString
            Else
                nRow("Subject") = RemoveXSS(drow("Subject").ToString) 'ds.Tables(0).Rows(j).Item("Subject").ToString
            End If

            nRow("SubjectLink") = drow("SubjectLink").ToString '標題
            Dim refpath As String = drow("RefPath").ToString.Substring(0, drow("RefPath").ToString.IndexOf("?"))
            '20090210 修改第四層選單為URL連結時加上cnid的錯誤
            nRow("RefPath") = refpath & "?cnid=" & ThisNodeSource & "&p=" & drow("PublicID").ToString 'drow("RefPath").ToString '路徑 2008/2/22修改為程式計算而非由資料庫取得，因多向有問題

            nRow("AttFiles") = drow("AttFiles").ToString  '附檔
            nRow("UpdateDateTime") = drow("UpdateDateTime").ToString
            nRow("PublishDate") = drow("PublishDate").ToString
            nRow("ListType") = 2
            dt.Rows.Add(nRow)
        Next

        Me.rpt.DataSource = dt
        Me.rpt.DataBind()

    End Sub

    ''' <summary>
    ''' 取得本單元文章清單
    ''' </summary>
    ''' <returns>DataSet</returns>
    ''' <remarks></remarks>
    Private Function GetDsThisNode(ByVal ThisNodeSource As Integer) As DataSet

        Dim ds As DataSet
        ds = ClassDB.RunSPReturnDataSet("Net2_Publisher_AllList_ThisNode", "ds_Publisher",
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
            SpecificNodeIDs = Split(Sitemap.GetChildNodesID(CInt(Request.Params("cnid"))), ",")
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


            '文章清單列
            'Dim drow As DataRow
            Dim ThisRowsCount As Integer
            If ds.Tables(0).Rows.Count > _RowsCount Then
                ThisRowsCount = _RowsCount
            Else
                ThisRowsCount = ds.Tables(0).Rows.Count
            End If
            For j = 0 To ThisRowsCount - 1
                Dim nRow As DataRow = dt.NewRow
                pagestart += 1
                If j = 0 Or pagestart Mod 10 = 1 Then
                    'nRow("Subject") = "<img src=""images/icon_04.gif"" alt=""單元標題圖示"" style=""border-width:0px;"" /><a title=""" & GetNodeName(SpecificNodeIDs(i)) & """ href=""/Publish.aspx?cnid=" & SpecificNodeIDs(i) & """ target=""" & NodeRefPath(1) & """>" & GetNodeName(SpecificNodeIDs(i)) & " </a> <br />"
                    nRow("NodeName") = "<img src=""images/icon_04.gif"" alt=""單元標題圖示"" style=""border-width:0px;"" /><a title=""" & GetNodeName(SpecificNodeIDs(i)) & """ href=""Publish.aspx?cnid=" & SpecificNodeIDs(i) & """ target=""" & NodeRefPath(1) & """>" & GetNodeName(SpecificNodeIDs(i)) & "</a>"
                    nRow("Subject") = RemoveXSS(nRow("Subject") & ds.Tables(0).Rows(j).Item("Subject").ToString)
                Else
                    nRow("Subject") = RemoveXSS(ds.Tables(0).Rows(j).Item("Subject").ToString)
                End If
                nRow("SubjectLink") = ds.Tables(0).Rows(j).Item("SubjectLink").ToString
                'nRow("RefPath") = ds.Tables(0).Rows(j).Item("RefPath").ToString
                Dim refpath As String = ds.Tables(0).Rows(j).Item("RefPath").ToString.Substring(0, ds.Tables(0).Rows(j).Item("RefPath").ToString.IndexOf("?"))
                nRow("RefPath") = refpath & "?cnid=" & SpecificNodeIDs(i) & "&p=" & ds.Tables(0).Rows(j).Item("PublicID").ToString  '路徑 2008/2/22修改為程式計算而非由資料庫取得，因多向有問題

                nRow("AttFiles") = ds.Tables(0).Rows(j).Item("AttFiles").ToString
                nRow("UpdateDateTime") = ds.Tables(0).Rows(j).Item("UpdateDateTime").ToString
                nRow("PublishDate") = ds.Tables(0).Rows(j).Item("PublishDate").ToString
                nRow("ListType") = 2
                dt.Rows.Add(nRow)
            Next

        Next

        Me.rpt.DataSource = dt
        Me.rpt.DataBind()

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
                    NodeName = CType(dRow("Text"), String) '暫時不要上Chris 20100818 & IIf(IsDBNull(dRow("TextEn")), "", IIf(Trim(dRow("TextEn").ToString).Equals(""), "", " (" & dRow("TextEn") & ")"))

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
                        RefPath = Trim(dRow("RefPath").ToString) & ","
                    Else
                        RefPath = Trim(dRow("RefPath").ToString) & "?cnid=" & NodeID & ","
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

                        Dim PublishDate As Date = CType(dr("PublishDate"), Date)
                        Dim UpdateDateTime As Date = CType(dr("UpdateDateTime2"), Date)

                        Me.ltUpdateDateTime.Text = (UpdateDateTime.Year - 1911) & "-" & UpdateDateTime.ToString("MM-dd")


                        If dr("AttFiles").ToString.Trim.Equals("") Then
                            FileManager3.Visible = False
                        Else
                            If Not CheckSqlInjectionWording(dr.Item("AttFiles")) Then
                                FileManager3.doDataBind(dr.Item("AttFiles").ToString)
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
            If IsNumeric(cnid) Then
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
    ''' GridView(文章清單按鈕功能設定)
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub gv_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rpt.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.SelectedItem Or e.Item.ItemType = ListItemType.SelectedItem Then
            Dim dRow As DataRowView = CType(e.Item.DataItem, DataRowView)
            '文章標題
            Dim hl_Subject As HyperLink = CType(e.Item.FindControl("hl_Subject"), HyperLink)
            Dim wordlimit As Integer = 35
            If IsNumeric(System.Configuration.ConfigurationManager.AppSettings("PublishSubjectLimit")) Then
                wordlimit = System.Configuration.ConfigurationManager.AppSettings("PublishSubjectLimit").ToString
            End If
            Dim ltldate As Literal = CType(e.Item.FindControl("ltldate"), Literal)
            ltldate.Text = (CType(dRow("PublishDate"), Date).Year - 1911).ToString & "-" & CType(dRow("PublishDate"), Date).ToString("MM-dd")
            '20100726 Chris 修改 超連結沒有文章圖示的問題
            Dim subject As String = dRow("Subject").ToString
            If subject.StartsWith("<a href") Then
                hl_Subject.Text = subject
            Else
                hl_Subject.Text = ModuleMisc.LimitWord(subject, wordlimit)
                hl_Subject.ToolTip = subject
            End If
            'Dim hl_Date As HyperLink = CType(e.Row.FindControl("hl_Date"), HyperLink)
            'hl_Date.Text = (CType(dRow("PublishDate"), Date).Year - 1911).ToString & "-" & CType(dRow("PublishDate"), Date).ToString("MM-dd")
            If dRow.Item("ListType") = "1" Then
                '單元標題
                hl_Subject.NavigateUrl = dRow("RefPath") '單元標題不用再加應用程式路徑
                If dRow("Target").ToString = "_blank" Then
                    hl_Subject.Target = RemoveXSS(dRow("Target").ToString)
                End If
                Dim UpdateDateTime As Date = CType(dRow("UpdateDateTime"), Date) 'Chris Chu 20090522 民國年
            ElseIf dRow.Item("ListType") = "2" Then
                hl_Subject.NavigateUrl = PathManager.ApplicationUrl & dRow.Item("RefPath").ToString  '單元標題不用再加應用程式路徑
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
        Me.rpt.Visible = True
        Me.tblList.Visible = True
        Dim ds As DataSet = GetDsHomeBlockNode(ThisNodeSource)

        '如沒有資料(包含NodeID傳錯)須停止執行~
        If ds.Tables(0).Rows.Count = 0 Then
            Me.tblList.Visible = False
            Exit Sub
        End If

        Dim dt As New DataTable
        dt = ds.Tables(0).Clone '建立一個欄位結構相同的DataTable


        Dim ThisRowsCount As Integer
        ThisRowsCount = ds.Tables(0).Rows.Count
        Dim pagestart As Integer = 0
        '文章清單列(由DataSet的資料來源複製到DataTable)
        For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
            Dim drow As DataRow = ds.Tables(0).Rows(i)
            Dim nRow As DataRow = dt.NewRow
            pagestart += 1

            If pagestart Mod 10 = 1 Then
                'nRow("Subject") = "<img src=""images/icon_04.gif"" alt=""單元標題圖示"" style=""border-width:0px;"" /><a title=""" & GetNodeName(SpecificNodeIDs(i)) & """ href=""/Publish.aspx?cnid=" & SpecificNodeIDs(i) & """ target=""" & NodeRefPath(1) & """>" & GetNodeName(SpecificNodeIDs(i)) & " </a> <br />"
                nRow("NodeName") = "<img src=""images/icon_04.gif"" alt=""單元標題圖示"" style=""border-width:0px;"" /><a title=""" & GetHomeBlockName(ThisNodeSource) & """ href=""Publish.aspx?cnhb=" & ThisNodeSource & """ >" & GetHomeBlockName(ThisNodeSource) & "</a>"
                nRow("Subject") = RemoveXSS(nRow("Subject") & drow("Subject").ToString)
            Else
                nRow("Subject") = RemoveXSS(drow("Subject").ToString)
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

        Me.rpt.DataSource = dt
        Me.rpt.DataBind()

    End Sub
    ''' <summary>
    ''' 取得本單元文章清單
    ''' </summary>
    ''' <returns>DataSet</returns>
    ''' <remarks></remarks>
    Private Function GetDsHomeBlockNode(ByVal ThisNodeSource As String) As DataSet
        Dim SiteID As String = CInt(System.Configuration.ConfigurationManager.AppSettings("SiteID")).ToString
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

    Private Function GetHomeBlockName(ByVal NodeID As Integer) As String
        Dim NodeName As String = ""
        Using dr As SqlDataReader = ClassDB.GetDataReaderParam("Select GroupName from SiteMapGroupCatgry where GroupID = @NodeID", New SqlParameter("@NodeID", NodeID))
            Try
                If dr IsNot Nothing Then
                    If dr.Read Then
                        NodeName = RemoveXSS(dr("GroupName"))
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
