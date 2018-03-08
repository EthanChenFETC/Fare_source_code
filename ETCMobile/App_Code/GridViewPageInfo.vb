Imports Microsoft.VisualBasic
Imports System.Data

''' <summary>
''' GridVeiw控制項之統一樣式設定
''' </summary>
''' <remarks></remarks>
Public Module GridViewPageInfo

    ''' <summary>
    ''' 設定GridView總頁數/頁碼資料
    ''' </summary>
    ''' <param name="GridView">欲套用的GridView控制項</param>
    ''' <param name="Webform">Webform表單物件</param>
    ''' <param name="SqldataSource">套用在該GridView的資料來源SqldataSource</param>
    ''' <param name="ShowPageInfo">是否呈現頁碼資料</param>
    ''' <remarks></remarks>
    Public Sub GetGridViewInfo(ByVal GridView As System.Web.UI.WebControls.GridView, ByVal Webform As System.Web.UI.Page, Optional ByVal SqldataSource As SqlDataSource = Nothing, Optional ByVal ShowPageInfo As Boolean = True)
        If GridView.Rows.Count > 0 Then
            If ShowPageInfo = True Then
                Dim bottomPagerRow As GridViewRow = GridView.FooterRow
                bottomPagerRow.Cells(0).Text = "<span class=""PageInfo"">目前所在頁碼(" & GridView.PageIndex + 1 & "/" & GridView.PageCount & ")  </span>"
                bottomPagerRow.Cells(0).ColumnSpan = GridView.Columns.Count

                Dim i As Integer
                For i = 1 To GridView.Columns.Count - 1
                    bottomPagerRow.Cells(i).Visible = False
                Next


                '筆數及頁碼資料
                If SqlDataSource IsNot Nothing Then
                    Dim dt As DataTable = CType(SqlDataSource.Select(New DataSourceSelectArguments), DataView).Table
                    Dim PageSize As Integer = GridView.PageSize
                    Dim TotalRowCount As Integer = dt.Rows.Count
                    Dim TotalPageCount As Integer
                    If GridView.AllowPaging Then
                        TotalPageCount = Math.Ceiling(TotalRowCount / PageSize)
                    Else
                        TotalPageCount = 1
                    End If
                    bottomPagerRow.Cells(0).Text = "<span class=""PageInfo"">頁碼 " & GridView.PageIndex + 1 & " / " & TotalPageCount & " 頁 總計 " & TotalRowCount & " 筆資料</span>"
                End If
            End If

            'GridView.Caption = "<span class=""PageInfo"">目前所在頁碼(" & GridView.PageIndex + 1 & "/" & GridView.PageCount & ")  </span>"
            'GridView.CaptionAlign = TableCaptionAlign.Right

            ''筆數及頁碼資料
            'If SqlDataSource IsNot Nothing Then
            '    Dim dt As DataTable = CType(SqlDataSource.Select(New DataSourceSelectArguments), DataView).Table
            '    Dim PageSize As Integer = GridView.PageSize
            '    Dim TotalRowCount As Integer = dt.Rows.Count
            '    Dim TotalPageCount As Integer = Math.Ceiling(TotalRowCount / PageSize)
            '    GridView.Caption = "<span class=""PageInfo"">頁碼 " & GridView.PageIndex + 1 & " / " & TotalPageCount & " 頁 總計 " & TotalRowCount & " 筆資料</span>"
            'End If

            SetCss(GridView, ShowPageInfo)
        End If
    End Sub
    ''' <summary>
    ''' 取得GridView總頁數/頁碼資料，並進行顯示
    ''' </summary>
    ''' <param name="GridView">GridView物件</param>
    ''' <param name="Webform">Webform網頁表單物件</param>
    ''' <param name="SqlDataSource">資料來源物件</param>
    ''' <param name="ShowPageInfo">是否呈現頁碼資料</param>
    ''' <remarks></remarks>
    Public Sub GetGridViewInfo2(ByVal GridView As System.Web.UI.WebControls.GridView, ByVal Webform As System.Web.UI.Page, Optional ByVal SqlDataSource As SqlDataSource = Nothing, Optional ByVal ShowPageInfo As Boolean = True, Optional ByVal SqlDataTable As DataTable = Nothing)
        If GridView.Rows.Count > 0 Then

            If ShowPageInfo = True Then

                Dim bottomPagerRow As GridViewRow = GridView.FooterRow
                bottomPagerRow.Cells(0).Text = "<span class=""PageInfo"">目前所在頁碼(" & GridView.PageIndex + 1 & "/" & GridView.PageCount & ")  </span>"
                bottomPagerRow.Cells(0).ColumnSpan = GridView.Columns.Count
                bottomPagerRow.Visible = True
                bottomPagerRow.Cells(0).Visible = True
                Dim i As Integer
                For i = 1 To GridView.Columns.Count - 1
                    bottomPagerRow.Cells(i).Visible = False
                Next


                '筆數及頁碼資料
                If SqlDataSource IsNot Nothing Then
                    Dim dt As DataTable = CType(SqlDataSource.Select(New DataSourceSelectArguments), DataView).Table
                    Dim PageSize As Integer = GridView.PageSize
                    '20100601 修訂 gridview 裡面的單元列不計數
                    Dim MenuCount As Integer = 0

                    For j As Integer = 0 To dt.Rows.Count - 1
                        Dim drow As DataRow = dt.Rows(j)
                        If drow("ListType").ToString = "1" Then
                            MenuCount += 1
                        End If
                    Next
                    Dim TotalRowCount As Integer = dt.Rows.Count - MenuCount

                    Dim TotalPageCount As Integer
                    If GridView.AllowPaging Then
                        TotalPageCount = Math.Ceiling(TotalRowCount / PageSize)
                    Else
                        TotalPageCount = 1
                    End If
                    bottomPagerRow.Cells(0).Text = "<span class=""PageInfo"">頁碼 " & GridView.PageIndex + 1 & " / " & TotalPageCount & " 頁 總計 " & TotalRowCount & " 筆資料</span>"
                End If
                '筆數及頁碼資料
                If SqlDataTable IsNot Nothing Then
                    Dim dt As DataTable = SqlDataTable
                    Dim PageSize As Integer = GridView.PageSize
                    Dim TotalRowCount As Integer = dt.Rows.Count
                    Dim TotalPageCount As Integer
                    If GridView.AllowPaging Then
                        TotalPageCount = Math.Ceiling(TotalRowCount / PageSize)
                    Else
                        TotalPageCount = 1
                    End If
                    bottomPagerRow.Cells(0).Text = "<span class=""PageInfo"">頁碼 " & GridView.PageIndex + 1 & " / " & TotalPageCount & " 頁 總計 " & TotalRowCount & " 筆資料</span>"
                End If
            End If

            'GridView.Caption = "<span class=""PageInfo"">目前所在頁碼(" & GridView.PageIndex + 1 & "/" & GridView.PageCount & ")  </span>"
            'GridView.CaptionAlign = TableCaptionAlign.Right

            ''筆數及頁碼資料
            'If SqlDataSource IsNot Nothing Then
            '    Dim dt As DataTable = CType(SqlDataSource.Select(New DataSourceSelectArguments), DataView).Table
            '    Dim PageSize As Integer = GridView.PageSize
            '    Dim TotalRowCount As Integer = dt.Rows.Count
            '    Dim TotalPageCount As Integer = Math.Ceiling(TotalRowCount / PageSize)
            '    GridView.Caption = "<span class=""PageInfo"">頁碼 " & GridView.PageIndex + 1 & " / " & TotalPageCount & " 頁 總計 " & TotalRowCount & " 筆資料</span>"
            'End If

            SetCss2(GridView, ShowPageInfo)
        End If
    End Sub

    ''' <summary>
    ''' GridView命令功能
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Sub PrevNextClick(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim gv As GridView = CType(CType(sender, LinkButton).Parent.Parent.Parent, GridView)
        gv.PageIndex = 1
        gv.DataBind()
    End Sub
    ''' <summary>
    ''' 設定表格樣式 Setting Css Style
    ''' </summary>
    ''' <param name="gv">GridView物件</param>
    ''' <remarks></remarks>
    Private Sub SetCss(ByVal gv As GridView, ByVal ShowFooter As Boolean)
        gv.CssClass = "Dg"
        gv.GridLines = GridLines.None

        gv.RowStyle.CssClass = "DgItem"
        gv.AlternatingRowStyle.CssClass = "DgAltItem"

        gv.HeaderStyle.CssClass = "DgHeader"
        gv.FooterStyle.CssClass = "DgFooter"
        gv.PagerStyle.CssClass = "DgPager"

        gv.SelectedRowStyle.CssClass = "DgItem_edit"
        gv.EmptyDataText = "目前沒有資料，或您的查詢沒有資料。"
        gv.PagerSettings.Mode = PagerButtons.NumericFirstLast
        '20100415 <第1頁> <前10頁><後10頁><最後1頁>
        gv.PagerSettings.FirstPageText = "<第1頁>"
        gv.PagerSettings.LastPageText = "<最終頁>"
        gv.PagerSettings.NextPageText = "<下頁>"
        gv.PagerSettings.PreviousPageText = "<前頁>"
        Dim i, j, k, l, m As Integer
        '取得pager row
        Dim PagerRow As GridViewRow = gv.BottomPagerRow
        Dim pKey As Boolean = False
        Dim bKey As Boolean = False
        Dim p10Key As Boolean = False
        Dim b10Key As Boolean = False
        If PagerRow IsNot Nothing Then

            For i = 0 To PagerRow.Cells.Count - 1
                For k = 0 To PagerRow.Cells(i).Controls.Count - 1
                    Dim trtable As Control = PagerRow.Controls(i).Controls(k)
                    For l = 0 To trtable.Controls.Count - 1
                        Dim trPager As TableRow = trtable.Controls(l)
                        For j = 0 To trPager.Controls.Count - 1
                            Dim tcpager As TableCell = trPager.Controls(j)
                            tcpager.BorderStyle = BorderStyle.None
                            tcpager.BorderWidth = "0"
                            For m = 0 To tcpager.Controls.Count - 1
                                If tcpager.Controls(m).ToString.Equals("System.Web.UI.WebControls.DataControlPagerLinkButton") Then
                                    Dim lkb As LinkButton = tcpager.Controls(m)
                                    If lkb.Text.Equals("...") And j < 10 Then
                                        lkb.Text = "<前10頁>"
                                        p10Key = True
                                    ElseIf lkb.Text.Equals("...") Then
                                        lkb.Text = "<後10頁>"
                                        b10Key = True
                                    End If
                                End If
                            Next
                        Next
                    Next
                Next
            Next
            If gv.PageCount > 10 Then
                If gv.PageIndex > 0 Then
                    Dim tcp As TableCell = New TableCell
                    tcp.BorderStyle = BorderStyle.None
                    tcp.BorderWidth = "0"
                    'Dim tlkb As LinkButton = New LinkButton
                    'tlkb.Text = "<上1頁>"
                    'tlkb.CommandName = "Page"
                    'tlkb.CommandArgument = gv.PageIndex - 1
                    'tlkb.OnClientClick = "javascript:__doPostBack('ctl00$ContentPlaceHolder1$GridView1','Page$" & gv.PageIndex - 1 & "')"
                    'tlkb.PostBackUrl = "javascript:__doPostBack('ctl00$ContentPlaceHolder1$GridView1','Page$" & gv.PageIndex - 1 & "')"
                    Dim tlkb As Literal = New Literal
                    tlkb.Text = "<a href=""javascript:__doPostBack('" & gv.ClientID.Replace("_", "$") & "','Page$" & gv.PageIndex & "')""><上1頁></a>"
                    tcp.Controls.Add(tlkb)
                    If Not p10Key Then
                        CType(gv.BottomPagerRow.Controls(0).Controls(0).Controls(0), TableRow).Controls.AddAt(0, tcp)
                    Else
                        CType(gv.BottomPagerRow.Controls(0).Controls(0).Controls(0), TableRow).Controls.AddAt(2, tcp)
                    End If
                End If
                If gv.PageIndex < gv.PageCount - 1 Then
                    Dim tcp As TableCell = New TableCell
                    tcp.BorderStyle = BorderStyle.None
                    tcp.BorderWidth = "0"
                    Dim tlkb As Literal = New Literal
                    tlkb.Text = "<a href=""javascript:__doPostBack('" & gv.ClientID.Replace("_", "$") & "','Page$" & gv.PageIndex + 2 & "')"">下1頁</a>"
                    tcp.Controls.Add(tlkb)
                    If Not b10Key Then
                        CType(gv.BottomPagerRow.Controls(0).Controls(0).Controls(0), TableRow).Controls.AddAt(CType(gv.BottomPagerRow.Controls(0).Controls(0).Controls(0), TableRow).Controls.Count, tcp)
                    Else
                        CType(gv.BottomPagerRow.Controls(0).Controls(0).Controls(0), TableRow).Controls.AddAt(CType(gv.BottomPagerRow.Controls(0).Controls(0).Controls(0), TableRow).Controls.Count - 2, tcp)
                    End If
                End If
            Else
                If gv.PageIndex > 0 Then
                    Dim tcp As TableCell = New TableCell
                    tcp.BorderStyle = BorderStyle.None
                    tcp.BorderWidth = "0"
                    Dim tlkb As Literal = New Literal
                    tlkb.Text = "<a href=""javascript:__doPostBack('" & gv.ClientID.Replace("_", "$") & "','Page$" & gv.PageIndex & "')"">上1頁</a>"
                    tcp.Controls.Add(tlkb)
                    CType(gv.BottomPagerRow.Controls(0).Controls(0).Controls(0), TableRow).Controls.AddAt(0, tcp)
                End If
                If gv.PageIndex < gv.PageCount - 1 Then
                    Dim tcp As TableCell = New TableCell
                    tcp.BorderStyle = BorderStyle.None
                    tcp.BorderWidth = "0"
                    Dim tlkb As Literal = New Literal
                    tlkb.Text = "<a href=""javascript:__doPostBack('" & gv.ClientID.Replace("_", "$") & "','Page$" & gv.PageIndex + 2 & "')"">下1頁</a>"
                    tcp.Controls.Add(tlkb)
                    CType(gv.BottomPagerRow.Controls(0).Controls(0).Controls(0), TableRow).Controls.AddAt(CType(gv.BottomPagerRow.Controls(0).Controls(0).Controls(0), TableRow).Controls.Count, tcp)
                End If
            End If
        End If

        gv.ShowFooter = ShowFooter
    End Sub
    ''' <summary>
    ''' 設定表格樣式 Setting Css Style
    ''' </summary>
    ''' <param name="gv">GridView物件</param>
    ''' <remarks></remarks>
    Private Sub SetCss2(ByVal gv As GridView, ByVal ShowFooter As Boolean)
        gv.CssClass = "dg_List"
        gv.GridLines = GridLines.None

        gv.RowStyle.CssClass = "DgItem"
        gv.AlternatingRowStyle.CssClass = "DgAltItem"

        gv.HeaderStyle.CssClass = "dg_List_Header"
        gv.FooterStyle.CssClass = "dg_List_Footer"
        gv.PagerStyle.CssClass = "DgPager"

        gv.SelectedRowStyle.CssClass = "DgItem_edit"
        gv.EmptyDataText = "目前沒有資料，或您的查詢沒有資料。"
        gv.PagerSettings.Mode = PagerButtons.NumericFirstLast
        '20100415 <第1頁> <前10頁><後10頁><最後1頁>
        gv.PagerSettings.FirstPageText = "<第1頁>"
        gv.PagerSettings.LastPageText = "<最終頁>"
        gv.PagerSettings.NextPageText = "<下頁>"
        gv.PagerSettings.PreviousPageText = "<前頁>"
        Dim i, j, k, l, m As Integer
        '取得pager row
        Dim PagerRow As GridViewRow = gv.BottomPagerRow

        Dim pKey As Boolean = False
        Dim bKey As Boolean = False
        Dim p10Key As Boolean = False
        Dim b10Key As Boolean = False
        If PagerRow IsNot Nothing Then
            PagerRow.Visible = True
            For i = 0 To PagerRow.Cells.Count - 1
                For k = 0 To PagerRow.Cells(i).Controls.Count - 1
                    Dim trtable As Control = PagerRow.Controls(i).Controls(k)
                    For l = 0 To trtable.Controls.Count - 1
                        Dim trPager As TableRow = trtable.Controls(l)
                        For j = 0 To trPager.Controls.Count - 1
                            Dim tcpager As TableCell = trPager.Controls(j)
                            For m = 0 To tcpager.Controls.Count - 1
                                If tcpager.Controls(m).ToString.Equals("System.Web.UI.WebControls.DataControlPagerLinkButton") Then
                                    Dim lkb As LinkButton = tcpager.Controls(m)
                                    If lkb.Text.Equals("...") And j < 10 Then
                                        lkb.Text = "<前10頁>"
                                        p10Key = True
                                    ElseIf lkb.Text.Equals("...") Then
                                        lkb.Text = "<後10頁>"
                                        b10Key = True
                                    End If
                                End If
                            Next
                        Next
                    Next
                Next
            Next
            If gv.PageCount > 10 Then
                If gv.PageIndex > 0 Then
                    Dim tcp As TableCell = New TableCell
                    'Dim tlkb As LinkButton = New LinkButton
                    'tlkb.Text = "<上1頁>"
                    'tlkb.CommandName = "Page"
                    'tlkb.CommandArgument = gv.PageIndex - 1
                    'tlkb.OnClientClick = "javascript:__doPostBack('ctl00$ContentPlaceHolder1$GridView1','Page$" & gv.PageIndex - 1 & "')"
                    'tlkb.PostBackUrl = "javascript:__doPostBack('ctl00$ContentPlaceHolder1$GridView1','Page$" & gv.PageIndex - 1 & "')"
                    Dim tlkb As Literal = New Literal
                    tlkb.Text = "<a href=""javascript:__doPostBack('" & gv.ClientID.Replace("_", "$") & "','Page$" & gv.PageIndex & "')""><上1頁></a>"
                    tcp.Controls.Add(tlkb)
                    If Not p10Key Then
                        CType(gv.BottomPagerRow.Controls(0).Controls(0).Controls(0), TableRow).Controls.AddAt(0, tcp)
                    Else
                        CType(gv.BottomPagerRow.Controls(0).Controls(0).Controls(0), TableRow).Controls.AddAt(2, tcp)
                    End If
                End If
                If gv.PageIndex < gv.PageCount - 1 Then
                    Dim tcp As TableCell = New TableCell
                    Dim tlkb As Literal = New Literal
                    tlkb.Text = "<a href=""javascript:__doPostBack('" & gv.ClientID.Replace("_", "$") & "','Page$" & gv.PageIndex + 2 & "')"">下1頁</a>"
                    tcp.Controls.Add(tlkb)
                    If Not b10Key Then
                        CType(gv.BottomPagerRow.Controls(0).Controls(0).Controls(0), TableRow).Controls.AddAt(CType(gv.BottomPagerRow.Controls(0).Controls(0).Controls(0), TableRow).Controls.Count, tcp)
                    Else
                        CType(gv.BottomPagerRow.Controls(0).Controls(0).Controls(0), TableRow).Controls.AddAt(CType(gv.BottomPagerRow.Controls(0).Controls(0).Controls(0), TableRow).Controls.Count - 2, tcp)
                    End If
                End If
            Else
                If gv.PageIndex > 0 Then
                    Dim tcp As TableCell = New TableCell
                    Dim tlkb As Literal = New Literal
                    tlkb.Text = "<a href=""javascript:__doPostBack('" & gv.ClientID.Replace("_", "$") & "','Page$" & gv.PageIndex & "')"">上1頁</a>"
                    tcp.Controls.Add(tlkb)
                    CType(gv.BottomPagerRow.Controls(0).Controls(0).Controls(0), TableRow).Controls.AddAt(0, tcp)
                End If
                If gv.PageIndex < gv.PageCount - 1 Then
                    Dim tcp As TableCell = New TableCell
                    Dim tlkb As Literal = New Literal
                    tlkb.Text = "<a href=""javascript:__doPostBack('" & gv.ClientID.Replace("_", "$") & "','Page$" & gv.PageIndex + 2 & "')"">下1頁</a>"
                    tcp.Controls.Add(tlkb)
                    CType(gv.BottomPagerRow.Controls(0).Controls(0).Controls(0), TableRow).Controls.AddAt(CType(gv.BottomPagerRow.Controls(0).Controls(0).Controls(0), TableRow).Controls.Count, tcp)
                End If
            End If
        End If

        gv.ShowFooter = ShowFooter
    End Sub
    ''' <summary>
    ''' 取得GridVeiw控制項更新後的主鍵值
    ''' </summary>
    ''' <param name="e">GridViewUpdateEventArgs</param>
    ''' <returns>主鍵值字串</returns>
    ''' <remarks></remarks>
    Public Function GetGridViewRowUpdatingDataKey(ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) As String

        Dim keysArray(e.Keys.Count - 1) As DictionaryEntry
        e.Keys.CopyTo(keysArray, 0) '將值複製到DictionaryEntry一維之中
        Dim DataKey As Integer = 0

        '取得已資料key主鍵值
        For i As Integer = 0 To keysArray.Count - 1
            Dim entry As DictionaryEntry = keysArray(i)
            If entry.Key IsNot Nothing Then
                DataKey = CInt(entry.Value.ToString())
            End If
        Next

        Return DataKey
    End Function


    ''' <summary>
    ''' 取得GridVeiw控制項更新後的主鍵值
    ''' </summary>
    ''' <param name="e">GridViewUpdateEventArgs</param>
    ''' <returns>主鍵值字串</returns>
    ''' <remarks></remarks>
    Public Function GetGridViewRowUpdatedDataKey(ByVal e As System.Web.UI.WebControls.GridViewUpdatedEventArgs) As String

        Dim keysArray(e.Keys.Count - 1) As DictionaryEntry
        e.Keys.CopyTo(keysArray, 0) '將值複製到DictionaryEntry一維之中
        Dim DataKey As Integer = 0

        '取得已資料key主鍵值
        For i As Integer = 0 To keysArray.Count - 1
            Dim entry As DictionaryEntry = keysArray(i)
            If entry.Key IsNot Nothing Then
                DataKey = CInt(entry.Value.ToString())
            End If
        Next

        Return DataKey
    End Function


    ''' <summary>
    ''' 取得GridVeiw控制項刪除資料後的主鍵值
    ''' </summary>
    ''' <param name="e">GridViewUpdateEventArgs</param>
    ''' <returns>主鍵值字串</returns>
    ''' <remarks></remarks>
    Public Function GetGridViewRowDeletedDataKey(ByVal e As System.Web.UI.WebControls.GridViewDeletedEventArgs) As String

        Dim keysArray(e.Keys.Count - 1) As DictionaryEntry
        e.Keys.CopyTo(keysArray, 0) '將值複製到DictionaryEntry一維之中
        Dim DataKey As Integer = 0

        '取得已資料key主鍵值
        For i As Integer = 0 To keysArray.Count - 1
            Dim entry As DictionaryEntry = keysArray(i)
            If entry.Key IsNot Nothing Then
                DataKey = CInt(entry.Value.ToString())
            End If
        Next

        Return DataKey
    End Function

    ''' <summary>
    ''' 取得GridView總頁數/頁碼資料，並進行顯示
    ''' </summary>
    ''' <param name="GridView">GridView物件</param>
    ''' <param name="Webform">Webform網頁表單物件</param>
    ''' <param name="SqlDataSource">資料來源物件</param>
    ''' <param name="ShowPageInfo">是否呈現頁碼資料</param>
    ''' <remarks></remarks>
    Public Sub GetGridViewInfo3(ByVal GridView As System.Web.UI.WebControls.GridView, ByVal Webform As System.Web.UI.Page, Optional ByVal SqlDataSource As SqlDataSource = Nothing, Optional ByVal ShowPageInfo As Boolean = True, Optional ByVal SqlDataTable As DataTable = Nothing)
        If GridView.Rows.Count > 0 Then

            'If ShowPageInfo = True Then

            '    Dim bottomPagerRow As GridViewRow = GridView.FooterRow
            '    bottomPagerRow.Cells(0).Text = "<span class=""PageInfo"">目前所在頁碼(" & GridView.PageIndex + 1 & "/" & GridView.PageCount & ")  </span>"
            '    bottomPagerRow.Cells(0).ColumnSpan = GridView.Columns.Count
            '    bottomPagerRow.Visible = True
            '    bottomPagerRow.Cells(0).Visible = True
            '    Dim i As Integer
            '    For i = 1 To GridView.Columns.Count - 1
            '        bottomPagerRow.Cells(i).Visible = False
            '    Next


            '    '筆數及頁碼資料
            '    If SqlDataSource IsNot Nothing Then
            '        Dim dt As DataTable = CType(SqlDataSource.Select(New DataSourceSelectArguments), DataView).Table
            '        Dim PageSize As Integer = GridView.PageSize
            '        '20100601 修訂 gridview 裡面的單元列不計數
            '        Dim MenuCount As Integer = 0
            '        Dim drow As DataRow
            '        For Each drow In dt.Rows
            '            If drow("ListType").ToString = "1" Then
            '                MenuCount += 1
            '            End If
            '        Next
            '        Dim TotalRowCount As Integer = dt.Rows.Count - MenuCount

            '        Dim TotalPageCount As Integer
            '        If GridView.AllowPaging Then
            '            TotalPageCount = Math.Ceiling(TotalRowCount / PageSize)
            '        Else
            '            TotalPageCount = 1
            '        End If
            '        bottomPagerRow.Cells(0).Text = "<span class=""PageInfo"">頁碼 " & GridView.PageIndex + 1 & " / " & TotalPageCount & " 頁 總計 " & TotalRowCount & " 筆資料</span>"
            '    End If
            '    '筆數及頁碼資料
            '    If SqlDataTable IsNot Nothing Then
            '        Dim dt As DataTable = SqlDataTable
            '        Dim PageSize As Integer = GridView.PageSize
            '        Dim TotalRowCount As Integer = dt.Rows.Count
            '        Dim TotalPageCount As Integer
            '        If GridView.AllowPaging Then
            '            TotalPageCount = Math.Ceiling(TotalRowCount / PageSize)
            '        Else
            '            TotalPageCount = 1
            '        End If
            '        bottomPagerRow.Cells(0).Text = "<span class=""PageInfo"">頁碼 " & GridView.PageIndex + 1 & " / " & TotalPageCount & " 頁 總計 " & TotalRowCount & " 筆資料</span>"
            '    End If
            'End If

            'GridView.Caption = "<span class=""PageInfo"">目前所在頁碼(" & GridView.PageIndex + 1 & "/" & GridView.PageCount & ")  </span>"
            'GridView.CaptionAlign = TableCaptionAlign.Right

            '筆數及頁碼資料
            If SqlDataSource IsNot Nothing Then
                Dim dt As DataTable = CType(SqlDataSource.Select(New DataSourceSelectArguments), DataView).Table
                Dim PageSize As Integer = GridView.PageSize
                Dim TotalRowCount As Integer = dt.Rows.Count
                Dim TotalPageCount As Integer = Math.Ceiling(TotalRowCount / PageSize)
                GridView.Caption = "<span class=""PageInfo"">頁碼 " & GridView.PageIndex + 1 & " / " & TotalPageCount & " 頁 總計 " & TotalRowCount & " 筆資料</span>"
            End If

            SetCss3(GridView, ShowPageInfo)
        End If
    End Sub
    ''' <summary>
    ''' 設定表格樣式 Setting Css Style
    ''' </summary>
    ''' <param name="gv">GridView物件</param>
    ''' <remarks></remarks>
    Private Sub SetCss3(ByVal gv As GridView, ByVal ShowFooter As Boolean)
        gv.CssClass = "newslist"
        gv.GridLines = GridLines.None

        'gv.RowStyle.CssClass = "DgItem"
        'gv.AlternatingRowStyle.CssClass = "DgAltItem"

        'gv.HeaderStyle.CssClass = "dg_List_Header"
        gv.FooterStyle.CssClass = ""
        gv.PagerStyle.CssClass = "scott"

        'gv.SelectedRowStyle.CssClass = "DgItem_edit"
        gv.EmptyDataText = "目前沒有資料，或您的查詢沒有資料。"
        gv.PagerSettings.Mode = PagerButtons.NumericFirstLast
        '20100415 <第1頁> <前10頁><後10頁><最後1頁>
        gv.PagerSettings.FirstPageText = "<<<"
        gv.PagerSettings.LastPageText = ">>>"
        gv.PagerSettings.NextPageText = ">"
        gv.PagerSettings.PreviousPageText = "<"
        Dim i, j, k, l, m As Integer
        '取得pager row
        Dim PagerRow As GridViewRow = gv.BottomPagerRow

        Dim pKey As Boolean = False
        Dim bKey As Boolean = False
        Dim p10Key As Boolean = False
        Dim b10Key As Boolean = False
        If PagerRow IsNot Nothing Then
            PagerRow.Visible = True
            For i = 0 To PagerRow.Cells.Count - 1
                For k = 0 To PagerRow.Cells(i).Controls.Count - 1
                    Dim trtable As Control = PagerRow.Controls(i).Controls(k)
                    For l = 0 To trtable.Controls.Count - 1
                        Dim trPager As TableRow = trtable.Controls(l)
                        For j = 0 To trPager.Controls.Count - 1
                            Dim tcpager As TableCell = trPager.Controls(j)
                            For m = 0 To tcpager.Controls.Count - 1
                                If tcpager.Controls(m).ToString.Equals("System.Web.UI.WebControls.DataControlPagerLinkButton") Then
                                    Dim lkb As LinkButton = tcpager.Controls(m)
                                    If lkb.Text.Equals("...") And j < 10 Then
                                        lkb.ToolTip = "<前10頁>"
                                        lkb.Text = "<<"
                                        p10Key = True
                                    ElseIf lkb.Text.Equals("...") Then
                                        lkb.ToolTip = "<後10頁>"
                                        lkb.Text = ">>"
                                        b10Key = True

                                    End If
                                ElseIf tcpager.Controls(m).ToString.Equals("System.Web.UI.WebControls.Label") Then
                                    Dim lkb As Label = tcpager.Controls(m)
                                    lkb.CssClass = "disabled"

                                End If
                            Next
                        Next
                    Next
                Next
            Next
            If gv.PageCount > 10 Then
                If gv.PageIndex > 0 Then
                    Dim tcp As TableCell = New TableCell
                    'Dim tlkb As LinkButton = New LinkButton
                    'tlkb.Text = "<上1頁>"
                    'tlkb.CommandName = "Page"
                    'tlkb.CommandArgument = gv.PageIndex - 1
                    'tlkb.OnClientClick = "javascript:__doPostBack('ctl00$ContentPlaceHolder1$GridView1','Page$" & gv.PageIndex - 1 & "')"
                    'tlkb.PostBackUrl = "javascript:__doPostBack('ctl00$ContentPlaceHolder1$GridView1','Page$" & gv.PageIndex - 1 & "')"
                    Dim tlkb As Literal = New Literal
                    tlkb.Text = "<a href=""javascript:__doPostBack('" & gv.ClientID.Replace("_", "$") & "','Page$" & gv.PageIndex & "')""><</a>"
                    tcp.Controls.Add(tlkb)
                    If Not p10Key Then
                        CType(gv.BottomPagerRow.Controls(0).Controls(0).Controls(0), TableRow).Controls.AddAt(0, tcp)
                    Else
                        CType(gv.BottomPagerRow.Controls(0).Controls(0).Controls(0), TableRow).Controls.AddAt(2, tcp)
                    End If
                End If
                If gv.PageIndex < gv.PageCount - 1 Then
                    Dim tcp As TableCell = New TableCell
                    Dim tlkb As Literal = New Literal
                    tlkb.Text = "<a href=""javascript:__doPostBack('" & gv.ClientID.Replace("_", "$") & "','Page$" & gv.PageIndex + 2 & "')"">></a>"
                    tcp.Controls.Add(tlkb)
                    If Not b10Key Then
                        CType(gv.BottomPagerRow.Controls(0).Controls(0).Controls(0), TableRow).Controls.AddAt(CType(gv.BottomPagerRow.Controls(0).Controls(0).Controls(0), TableRow).Controls.Count, tcp)
                    Else
                        CType(gv.BottomPagerRow.Controls(0).Controls(0).Controls(0), TableRow).Controls.AddAt(CType(gv.BottomPagerRow.Controls(0).Controls(0).Controls(0), TableRow).Controls.Count - 2, tcp)
                    End If
                End If
            Else
                If gv.PageIndex > 0 Then
                    Dim tcp As TableCell = New TableCell
                    Dim tlkb As Literal = New Literal
                    tlkb.Text = "<a href=""javascript:__doPostBack('" & gv.ClientID.Replace("_", "$") & "','Page$" & gv.PageIndex & "')""><</a>"
                    tcp.Controls.Add(tlkb)
                    CType(gv.BottomPagerRow.Controls(0).Controls(0).Controls(0), TableRow).Controls.AddAt(0, tcp)
                End If
                If gv.PageIndex < gv.PageCount - 1 Then
                    Dim tcp As TableCell = New TableCell
                    Dim tlkb As Literal = New Literal
                    tlkb.Text = "<a href=""javascript:__doPostBack('" & gv.ClientID.Replace("_", "$") & "','Page$" & gv.PageIndex + 2 & "')"">></a>"
                    tcp.Controls.Add(tlkb)
                    CType(gv.BottomPagerRow.Controls(0).Controls(0).Controls(0), TableRow).Controls.AddAt(CType(gv.BottomPagerRow.Controls(0).Controls(0).Controls(0), TableRow).Controls.Count, tcp)
                End If
            End If
        End If

        gv.ShowFooter = ShowFooter
        gv.ShowHeader = False
    End Sub
#Region "SDS"

    ''' <summary>
    ''' SqlDataSource插入新資料後，取得主鍵值
    ''' </summary>
    ''' <param name="e">SqlDataSourceStatusEventArgs</param>
    ''' <returns>主鍵值的字串</returns>
    ''' <remarks></remarks>
    Public Function GetSDSInsert_DataKey(ByVal e As System.Web.UI.WebControls.SqlDataSourceStatusEventArgs) As String
        '進行群組關聯作業
        Dim command As Common.DbCommand
        command = e.Command
        '取得SqlDataSource_GroupAdd新增後的主鍵
        Dim key As Integer = CInt(command.Parameters("@retVal").Value.ToString())

        Return key
    End Function

#End Region


#Region "自訂分頁2008/3/12"
    ''' <summary>
    ''' 設定GridView總頁數/頁碼資料
    ''' </summary>
    ''' <param name="GridView">欲套用的GridView控制項</param>
    ''' <param name="Webform">Webform表單物件</param>
    ''' <param name="SqldataSource">套用在該GridView的資料來源SqldataSource</param>
    ''' <param name="ShowPageInfo">是否呈現頁碼資料</param>
    ''' <remarks></remarks>
    Public Sub GetGridViewInfoAppTable(ByVal GridView As System.Web.UI.WebControls.GridView, ByVal Webform As System.Web.UI.Page, Optional ByVal SqldataSource As SqlDataSource = Nothing, Optional ByVal ShowPageInfo As Boolean = True)
        If GridView.Rows.Count > 0 Then
            If ShowPageInfo = True Then
                Dim bottomPagerRow As GridViewRow = GridView.FooterRow
                bottomPagerRow.Cells(0).Text = "<span class=""PageInfo"">目前所在頁碼(" & GridView.PageIndex + 1 & "/" & GridView.PageCount & ")  </span>"
                bottomPagerRow.Cells(0).ColumnSpan = GridView.Columns.Count

                Dim i As Integer
                For i = 1 To GridView.Columns.Count - 1
                    bottomPagerRow.Cells(i).Visible = False
                Next


                '筆數及頁碼資料
                If SqldataSource IsNot Nothing Then
                    Dim dt As DataTable = CType(SqldataSource.Select(New DataSourceSelectArguments), DataView).Table
                    Dim PageSize As Integer = GridView.PageSize
                    Dim TotalRowCount As Integer = dt.Rows.Count
                    Dim TotalPageCount As Integer
                    If GridView.AllowPaging Then
                        TotalPageCount = Math.Ceiling(TotalRowCount / PageSize)
                    Else
                        TotalPageCount = 1
                    End If
                    bottomPagerRow.Cells(0).Text = "<span class=""PageInfo"">頁碼 " & GridView.PageIndex + 1 & " / " & TotalPageCount & " 頁 總計 " & TotalRowCount & " 筆資料</span>"
                End If
            End If

            'GridView.Caption = "<span class=""PageInfo"">目前所在頁碼(" & GridView.PageIndex + 1 & "/" & GridView.PageCount & ")  </span>"
            'GridView.CaptionAlign = TableCaptionAlign.Right

            ''筆數及頁碼資料
            'If SqlDataSource IsNot Nothing Then
            '    Dim dt As DataTable = CType(SqlDataSource.Select(New DataSourceSelectArguments), DataView).Table
            '    Dim PageSize As Integer = GridView.PageSize
            '    Dim TotalRowCount As Integer = dt.Rows.Count
            '    Dim TotalPageCount As Integer = Math.Ceiling(TotalRowCount / PageSize)
            '    GridView.Caption = "<span class=""PageInfo"">頁碼 " & GridView.PageIndex + 1 & " / " & TotalPageCount & " 頁 總計 " & TotalRowCount & " 筆資料</span>"
            'End If

            SetCssAppTable(GridView, ShowPageInfo)
        End If
    End Sub
    ''' <summary>
    ''' 設定表格樣式 Setting Css Style
    ''' </summary>
    ''' <param name="gv">GridView物件</param>
    ''' <remarks></remarks>
    Private Sub SetCssAppTable(ByVal gv As GridView, ByVal ShowFooter As Boolean)
        gv.PagerStyle.CssClass = "DgPager"
        gv.GridLines = GridLines.None
        gv.EmptyDataText = "目前沒有資料，或您的查詢沒有資料。"
        gv.PagerSettings.Mode = PagerButtons.NumericFirstLast
        '20100415 <第1頁> <前10頁><後10頁><最後1頁>
        gv.PagerSettings.FirstPageText = "<第1頁>"
        gv.PagerSettings.LastPageText = "<最終頁>"
        gv.PagerSettings.NextPageText = "<下頁>"
        gv.PagerSettings.PreviousPageText = "<前頁>"
        Dim i, j, k, l, m As Integer
        '取得pager row
        Dim PagerRow As GridViewRow = gv.BottomPagerRow
        Dim pKey As Boolean = False
        Dim bKey As Boolean = False
        Dim p10Key As Boolean = False
        Dim b10Key As Boolean = False
        If PagerRow IsNot Nothing Then

            For i = 0 To PagerRow.Cells.Count - 1
                For k = 0 To PagerRow.Cells(i).Controls.Count - 1
                    Dim trtable As Control = PagerRow.Controls(i).Controls(k)
                    For l = 0 To trtable.Controls.Count - 1
                        Dim trPager As TableRow = trtable.Controls(l)
                        For j = 0 To trPager.Controls.Count - 1
                            Dim tcpager As TableCell = trPager.Controls(j)
                            For m = 0 To tcpager.Controls.Count - 1
                                If tcpager.Controls(m).ToString.Equals("System.Web.UI.WebControls.DataControlPagerLinkButton") Then
                                    Dim lkb As LinkButton = tcpager.Controls(m)
                                    If lkb.Text.Equals("...") And j < 10 Then
                                        lkb.Text = "<前10頁>"
                                        p10Key = True
                                    ElseIf lkb.Text.Equals("...") Then
                                        lkb.Text = "<後10頁>"
                                        b10Key = True
                                    End If
                                End If
                            Next
                        Next
                    Next
                Next
            Next
            If gv.PageCount > 10 Then
                If gv.PageIndex > 0 Then
                    Dim tcp As TableCell = New TableCell
                    'Dim tlkb As LinkButton = New LinkButton
                    'tlkb.Text = "<上1頁>"
                    'tlkb.CommandName = "Page"
                    'tlkb.CommandArgument = gv.PageIndex - 1
                    'tlkb.OnClientClick = "javascript:__doPostBack('ctl00$ContentPlaceHolder1$GridView1','Page$" & gv.PageIndex - 1 & "')"
                    'tlkb.PostBackUrl = "javascript:__doPostBack('ctl00$ContentPlaceHolder1$GridView1','Page$" & gv.PageIndex - 1 & "')"
                    Dim tlkb As Literal = New Literal
                    tlkb.Text = "<a href=""javascript:__doPostBack('" & gv.ClientID.Replace("_", "$") & "','Page$" & gv.PageIndex & "')""><上1頁></a>"
                    tcp.Controls.Add(tlkb)
                    If Not p10Key Then
                        CType(gv.BottomPagerRow.Controls(0).Controls(0).Controls(0), TableRow).Controls.AddAt(0, tcp)
                    Else
                        CType(gv.BottomPagerRow.Controls(0).Controls(0).Controls(0), TableRow).Controls.AddAt(2, tcp)
                    End If
                End If
                If gv.PageIndex < gv.PageCount - 1 Then
                    Dim tcp As TableCell = New TableCell
                    Dim tlkb As Literal = New Literal
                    tlkb.Text = "<a href=""javascript:__doPostBack('" & gv.ClientID.Replace("_", "$") & "','Page$" & gv.PageIndex + 2 & "')"">下1頁</a>"
                    tcp.Controls.Add(tlkb)
                    If Not b10Key Then
                        CType(gv.BottomPagerRow.Controls(0).Controls(0).Controls(0), TableRow).Controls.AddAt(CType(gv.BottomPagerRow.Controls(0).Controls(0).Controls(0), TableRow).Controls.Count, tcp)
                    Else
                        CType(gv.BottomPagerRow.Controls(0).Controls(0).Controls(0), TableRow).Controls.AddAt(CType(gv.BottomPagerRow.Controls(0).Controls(0).Controls(0), TableRow).Controls.Count - 2, tcp)
                    End If
                End If
            Else
                If gv.PageIndex > 0 Then
                    Dim tcp As TableCell = New TableCell
                    Dim tlkb As Literal = New Literal
                    tlkb.Text = "<a href=""javascript:__doPostBack('" & gv.ClientID.Replace("_", "$") & "','Page$" & gv.PageIndex & "')"">上1頁</a>"
                    tcp.Controls.Add(tlkb)
                    CType(gv.BottomPagerRow.Controls(0).Controls(0).Controls(0), TableRow).Controls.AddAt(0, tcp)
                End If
                If gv.PageIndex < gv.PageCount - 1 Then
                    Dim tcp As TableCell = New TableCell
                    Dim tlkb As Literal = New Literal
                    tlkb.Text = "<a href=""javascript:__doPostBack('" & gv.ClientID.Replace("_", "$") & "','Page$" & gv.PageIndex + 2 & "')"">下1頁</a>"
                    tcp.Controls.Add(tlkb)
                    CType(gv.BottomPagerRow.Controls(0).Controls(0).Controls(0), TableRow).Controls.AddAt(CType(gv.BottomPagerRow.Controls(0).Controls(0).Controls(0), TableRow).Controls.Count, tcp)
                End If
            End If
        End If

        gv.ShowFooter = ShowFooter
    End Sub
    ''' <summary>
    ''' 自訂頁碼(適用於GridView與SqlDataSource)
    ''' </summary>
    ''' <param name="Webform">Webform</param>
    ''' <param name="Pager">CutePager.Pager</param>
    ''' <param name="gv">GridView</param>
    ''' <param name="sds">SqlDataSource提供計算資料數使用</param>
    ''' <remarks></remarks>
    Public Sub BindPager(ByVal Webform As System.Web.UI.Page, ByVal Pager As CutePager.Pager, ByVal gv As GridView, ByVal sds As SqlDataSource)
        If 1 = 1 Then
            '導到 上面的pager info 2010/05/26 Chris
            GetGridViewInfoAppTable(gv, Webform, sds)
        Else  '底下的程式不要移除，以後搞不好會用
            Dim dt As DataTable = CType(sds.Select(New DataSourceSelectArguments), DataView).Table

            If dt.Rows.Count = 0 Then
                Pager.Visible = False
            Else
                Pager.Visible = True
                Dim PageIndex As Integer = 1
                If Webform.Request.Params("i") <> "" Then
                    Try
                        PageIndex = CInt(Webform.Request.Params("i"))
                    Catch ex As Exception
                        PageIndex = 1
                    End Try
                End If

                Dim PageURLFormat As String = Webform.Request.Url.ToString
                If Webform.Request.Params("i") <> "" Then
                    If PageURLFormat.IndexOf("?i=") >= 0 Then
                        'i的參數在最前面
                        PageURLFormat = PageURLFormat.Replace("i=" & Webform.Request.Params("i"), "")
                    Else
                        PageURLFormat = PageURLFormat.Replace("&i=" & Webform.Request.Params("i"), "")
                    End If

                End If
                If PageURLFormat.IndexOf("?") >= 0 Then
                    PageURLFormat = PageURLFormat & "&i={0}"
                Else
                    PageURLFormat = PageURLFormat & "?i={0}"
                End If

                Pager.PageSize = gv.PageSize
                Pager.PageURLFormat = PageURLFormat
                Pager.CurrentIndex = PageIndex
                Pager.ItemCount = dt.Rows.Count
                Pager.ShowFirstLast = True
                '把原來GridView的分頁數字拿掉
                gv.BottomPagerRow.Visible = False
            End If
        End If

    End Sub

#End Region

End Module

