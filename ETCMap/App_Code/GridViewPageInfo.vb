Imports Microsoft.VisualBasic
Imports System.Data

''' <summary>
''' GridVeiw������Τ@�˦��]�w
''' </summary>
''' <remarks></remarks>
Public Module GridViewPageInfo

    ''' <summary>
    ''' �]�wGridView�`����/���X���
    ''' </summary>
    ''' <param name="GridView">���M�Ϊ�GridView���</param>
    ''' <param name="Webform">Webform��檫��</param>
    ''' <param name="SqldataSource">�M�Φb��GridView����ƨӷ�SqldataSource</param>
    ''' <param name="ShowPageInfo">�O�_�e�{���X���</param>
    ''' <remarks></remarks>
    Public Sub GetGridViewInfo(ByVal GridView As System.Web.UI.WebControls.GridView, ByVal Webform As System.Web.UI.Page, Optional ByVal SqldataSource As SqlDataSource = Nothing, Optional ByVal ShowPageInfo As Boolean = True)
        If GridView.Rows.Count > 0 Then
            If ShowPageInfo = True Then
                Dim bottomPagerRow As GridViewRow = GridView.FooterRow
                bottomPagerRow.Cells(0).Text = "<span class=""PageInfo"">�ثe�Ҧb���X(" & GridView.PageIndex + 1 & "/" & GridView.PageCount & ")  </span>"
                bottomPagerRow.Cells(0).ColumnSpan = GridView.Columns.Count

                Dim i As Integer
                For i = 1 To GridView.Columns.Count - 1
                    bottomPagerRow.Cells(i).Visible = False
                Next


                '���Ƥέ��X���
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
                    bottomPagerRow.Cells(0).Text = "<span class=""PageInfo"">���X " & GridView.PageIndex + 1 & " / " & TotalPageCount & " �� �`�p " & TotalRowCount & " �����</span>"
                End If
            End If

            'GridView.Caption = "<span class=""PageInfo"">�ثe�Ҧb���X(" & GridView.PageIndex + 1 & "/" & GridView.PageCount & ")  </span>"
            'GridView.CaptionAlign = TableCaptionAlign.Right

            ''���Ƥέ��X���
            'If SqlDataSource IsNot Nothing Then
            '    Dim dt As DataTable = CType(SqlDataSource.Select(New DataSourceSelectArguments), DataView).Table
            '    Dim PageSize As Integer = GridView.PageSize
            '    Dim TotalRowCount As Integer = dt.Rows.Count
            '    Dim TotalPageCount As Integer = Math.Ceiling(TotalRowCount / PageSize)
            '    GridView.Caption = "<span class=""PageInfo"">���X " & GridView.PageIndex + 1 & " / " & TotalPageCount & " �� �`�p " & TotalRowCount & " �����</span>"
            'End If

            SetCss(GridView, ShowPageInfo)
        End If
    End Sub
    ''' <summary>
    ''' ���oGridView�`����/���X��ơA�öi�����
    ''' </summary>
    ''' <param name="GridView">GridView����</param>
    ''' <param name="Webform">Webform������檫��</param>
    ''' <param name="SqlDataSource">��ƨӷ�����</param>
    ''' <param name="ShowPageInfo">�O�_�e�{���X���</param>
    ''' <remarks></remarks>
    Public Sub GetGridViewInfo2(ByVal GridView As System.Web.UI.WebControls.GridView, ByVal Webform As System.Web.UI.Page, Optional ByVal SqlDataSource As SqlDataSource = Nothing, Optional ByVal ShowPageInfo As Boolean = True, Optional ByVal SqlDataTable As DataTable = Nothing)
        If GridView.Rows.Count > 0 Then

            If ShowPageInfo = True Then

                Dim bottomPagerRow As GridViewRow = GridView.FooterRow
                bottomPagerRow.Cells(0).Text = "<span class=""PageInfo"">�ثe�Ҧb���X(" & GridView.PageIndex + 1 & "/" & GridView.PageCount & ")  </span>"
                bottomPagerRow.Cells(0).ColumnSpan = GridView.Columns.Count
                bottomPagerRow.Visible = True
                bottomPagerRow.Cells(0).Visible = True
                Dim i As Integer
                For i = 1 To GridView.Columns.Count - 1
                    bottomPagerRow.Cells(i).Visible = False
                Next


                '���Ƥέ��X���
                If SqlDataSource IsNot Nothing Then
                    Dim dt As DataTable = CType(SqlDataSource.Select(New DataSourceSelectArguments), DataView).Table
                    Dim PageSize As Integer = GridView.PageSize
                    '20100601 �׭q gridview �̭����椸�C���p��
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
                    bottomPagerRow.Cells(0).Text = "<span class=""PageInfo"">���X " & GridView.PageIndex + 1 & " / " & TotalPageCount & " �� �`�p " & TotalRowCount & " �����</span>"
                End If
                '���Ƥέ��X���
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
                    bottomPagerRow.Cells(0).Text = "<span class=""PageInfo"">���X " & GridView.PageIndex + 1 & " / " & TotalPageCount & " �� �`�p " & TotalRowCount & " �����</span>"
                End If
            End If

            'GridView.Caption = "<span class=""PageInfo"">�ثe�Ҧb���X(" & GridView.PageIndex + 1 & "/" & GridView.PageCount & ")  </span>"
            'GridView.CaptionAlign = TableCaptionAlign.Right

            ''���Ƥέ��X���
            'If SqlDataSource IsNot Nothing Then
            '    Dim dt As DataTable = CType(SqlDataSource.Select(New DataSourceSelectArguments), DataView).Table
            '    Dim PageSize As Integer = GridView.PageSize
            '    Dim TotalRowCount As Integer = dt.Rows.Count
            '    Dim TotalPageCount As Integer = Math.Ceiling(TotalRowCount / PageSize)
            '    GridView.Caption = "<span class=""PageInfo"">���X " & GridView.PageIndex + 1 & " / " & TotalPageCount & " �� �`�p " & TotalRowCount & " �����</span>"
            'End If

            SetCss2(GridView, ShowPageInfo)
        End If
    End Sub

    ''' <summary>
    ''' GridView�R�O�\��
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
    ''' �]�w���˦� Setting Css Style
    ''' </summary>
    ''' <param name="gv">GridView����</param>
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
        gv.EmptyDataText = "�ثe�S����ơA�αz���d�ߨS����ơC"
        gv.PagerSettings.Mode = PagerButtons.NumericFirstLast
        '20100415 <��1��> <�e10��><��10��><�̫�1��>
        gv.PagerSettings.FirstPageText = "<��1��>"
        gv.PagerSettings.LastPageText = "<�̲׭�>"
        gv.PagerSettings.NextPageText = "<�U��>"
        gv.PagerSettings.PreviousPageText = "<�e��>"
        Dim i, j, k, l, m As Integer
        '���opager row
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
                                        lkb.Text = "<�e10��>"
                                        p10Key = True
                                    ElseIf lkb.Text.Equals("...") Then
                                        lkb.Text = "<��10��>"
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
                    'tlkb.Text = "<�W1��>"
                    'tlkb.CommandName = "Page"
                    'tlkb.CommandArgument = gv.PageIndex - 1
                    'tlkb.OnClientClick = "javascript:__doPostBack('ctl00$ContentPlaceHolder1$GridView1','Page$" & gv.PageIndex - 1 & "')"
                    'tlkb.PostBackUrl = "javascript:__doPostBack('ctl00$ContentPlaceHolder1$GridView1','Page$" & gv.PageIndex - 1 & "')"
                    Dim tlkb As Literal = New Literal
                    tlkb.Text = "<a href=""javascript:__doPostBack('" & gv.ClientID.Replace("_", "$") & "','Page$" & gv.PageIndex & "')""><�W1��></a>"
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
                    tlkb.Text = "<a href=""javascript:__doPostBack('" & gv.ClientID.Replace("_", "$") & "','Page$" & gv.PageIndex + 2 & "')"">�U1��</a>"
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
                    tlkb.Text = "<a href=""javascript:__doPostBack('" & gv.ClientID.Replace("_", "$") & "','Page$" & gv.PageIndex & "')"">�W1��</a>"
                    tcp.Controls.Add(tlkb)
                    CType(gv.BottomPagerRow.Controls(0).Controls(0).Controls(0), TableRow).Controls.AddAt(0, tcp)
                End If
                If gv.PageIndex < gv.PageCount - 1 Then
                    Dim tcp As TableCell = New TableCell
                    tcp.BorderStyle = BorderStyle.None
                    tcp.BorderWidth = "0"
                    Dim tlkb As Literal = New Literal
                    tlkb.Text = "<a href=""javascript:__doPostBack('" & gv.ClientID.Replace("_", "$") & "','Page$" & gv.PageIndex + 2 & "')"">�U1��</a>"
                    tcp.Controls.Add(tlkb)
                    CType(gv.BottomPagerRow.Controls(0).Controls(0).Controls(0), TableRow).Controls.AddAt(CType(gv.BottomPagerRow.Controls(0).Controls(0).Controls(0), TableRow).Controls.Count, tcp)
                End If
            End If
        End If

        gv.ShowFooter = ShowFooter
    End Sub
    ''' <summary>
    ''' �]�w���˦� Setting Css Style
    ''' </summary>
    ''' <param name="gv">GridView����</param>
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
        gv.EmptyDataText = "�ثe�S����ơA�αz���d�ߨS����ơC"
        gv.PagerSettings.Mode = PagerButtons.NumericFirstLast
        '20100415 <��1��> <�e10��><��10��><�̫�1��>
        gv.PagerSettings.FirstPageText = "<��1��>"
        gv.PagerSettings.LastPageText = "<�̲׭�>"
        gv.PagerSettings.NextPageText = "<�U��>"
        gv.PagerSettings.PreviousPageText = "<�e��>"
        Dim i, j, k, l, m As Integer
        '���opager row
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
                                        lkb.Text = "<�e10��>"
                                        p10Key = True
                                    ElseIf lkb.Text.Equals("...") Then
                                        lkb.Text = "<��10��>"
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
                    'tlkb.Text = "<�W1��>"
                    'tlkb.CommandName = "Page"
                    'tlkb.CommandArgument = gv.PageIndex - 1
                    'tlkb.OnClientClick = "javascript:__doPostBack('ctl00$ContentPlaceHolder1$GridView1','Page$" & gv.PageIndex - 1 & "')"
                    'tlkb.PostBackUrl = "javascript:__doPostBack('ctl00$ContentPlaceHolder1$GridView1','Page$" & gv.PageIndex - 1 & "')"
                    Dim tlkb As Literal = New Literal
                    tlkb.Text = "<a href=""javascript:__doPostBack('" & gv.ClientID.Replace("_", "$") & "','Page$" & gv.PageIndex & "')""><�W1��></a>"
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
                    tlkb.Text = "<a href=""javascript:__doPostBack('" & gv.ClientID.Replace("_", "$") & "','Page$" & gv.PageIndex + 2 & "')"">�U1��</a>"
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
                    tlkb.Text = "<a href=""javascript:__doPostBack('" & gv.ClientID.Replace("_", "$") & "','Page$" & gv.PageIndex & "')"">�W1��</a>"
                    tcp.Controls.Add(tlkb)
                    CType(gv.BottomPagerRow.Controls(0).Controls(0).Controls(0), TableRow).Controls.AddAt(0, tcp)
                End If
                If gv.PageIndex < gv.PageCount - 1 Then
                    Dim tcp As TableCell = New TableCell
                    Dim tlkb As Literal = New Literal
                    tlkb.Text = "<a href=""javascript:__doPostBack('" & gv.ClientID.Replace("_", "$") & "','Page$" & gv.PageIndex + 2 & "')"">�U1��</a>"
                    tcp.Controls.Add(tlkb)
                    CType(gv.BottomPagerRow.Controls(0).Controls(0).Controls(0), TableRow).Controls.AddAt(CType(gv.BottomPagerRow.Controls(0).Controls(0).Controls(0), TableRow).Controls.Count, tcp)
                End If
            End If
        End If

        gv.ShowFooter = ShowFooter
    End Sub
    ''' <summary>
    ''' ���oGridVeiw�����s�᪺�D���
    ''' </summary>
    ''' <param name="e">GridViewUpdateEventArgs</param>
    ''' <returns>�D��Ȧr��</returns>
    ''' <remarks></remarks>
    Public Function GetGridViewRowUpdatingDataKey(ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) As String

        Dim keysArray(e.Keys.Count - 1) As DictionaryEntry
        e.Keys.CopyTo(keysArray, 0) '�N�Ƚƻs��DictionaryEntry�@������
        Dim DataKey As Integer = 0

        '���o�w���key�D���
        For i As Integer = 0 To keysArray.Count - 1
            Dim entry As DictionaryEntry = keysArray(i)
            If entry.Key IsNot Nothing Then
                DataKey = CInt(entry.Value.ToString())
            End If
        Next

        Return DataKey
    End Function


    ''' <summary>
    ''' ���oGridVeiw�����s�᪺�D���
    ''' </summary>
    ''' <param name="e">GridViewUpdateEventArgs</param>
    ''' <returns>�D��Ȧr��</returns>
    ''' <remarks></remarks>
    Public Function GetGridViewRowUpdatedDataKey(ByVal e As System.Web.UI.WebControls.GridViewUpdatedEventArgs) As String

        Dim keysArray(e.Keys.Count - 1) As DictionaryEntry
        e.Keys.CopyTo(keysArray, 0) '�N�Ƚƻs��DictionaryEntry�@������
        Dim DataKey As Integer = 0

        '���o�w���key�D���
        For i As Integer = 0 To keysArray.Count - 1
            Dim entry As DictionaryEntry = keysArray(i)
            If entry.Key IsNot Nothing Then
                DataKey = CInt(entry.Value.ToString())
            End If
        Next

        Return DataKey
    End Function


    ''' <summary>
    ''' ���oGridVeiw����R����ƫ᪺�D���
    ''' </summary>
    ''' <param name="e">GridViewUpdateEventArgs</param>
    ''' <returns>�D��Ȧr��</returns>
    ''' <remarks></remarks>
    Public Function GetGridViewRowDeletedDataKey(ByVal e As System.Web.UI.WebControls.GridViewDeletedEventArgs) As String

        Dim keysArray(e.Keys.Count - 1) As DictionaryEntry
        e.Keys.CopyTo(keysArray, 0) '�N�Ƚƻs��DictionaryEntry�@������
        Dim DataKey As Integer = 0

        '���o�w���key�D���
        For i As Integer = 0 To keysArray.Count - 1
            Dim entry As DictionaryEntry = keysArray(i)
            If entry.Key IsNot Nothing Then
                DataKey = CInt(entry.Value.ToString())
            End If
        Next

        Return DataKey
    End Function

    ''' <summary>
    ''' ���oGridView�`����/���X��ơA�öi�����
    ''' </summary>
    ''' <param name="GridView">GridView����</param>
    ''' <param name="Webform">Webform������檫��</param>
    ''' <param name="SqlDataSource">��ƨӷ�����</param>
    ''' <param name="ShowPageInfo">�O�_�e�{���X���</param>
    ''' <remarks></remarks>
    Public Sub GetGridViewInfo3(ByVal GridView As System.Web.UI.WebControls.GridView, ByVal Webform As System.Web.UI.Page, Optional ByVal SqlDataSource As SqlDataSource = Nothing, Optional ByVal ShowPageInfo As Boolean = True, Optional ByVal SqlDataTable As DataTable = Nothing)
        If GridView.Rows.Count > 0 Then

            'If ShowPageInfo = True Then

            '    Dim bottomPagerRow As GridViewRow = GridView.FooterRow
            '    bottomPagerRow.Cells(0).Text = "<span class=""PageInfo"">�ثe�Ҧb���X(" & GridView.PageIndex + 1 & "/" & GridView.PageCount & ")  </span>"
            '    bottomPagerRow.Cells(0).ColumnSpan = GridView.Columns.Count
            '    bottomPagerRow.Visible = True
            '    bottomPagerRow.Cells(0).Visible = True
            '    Dim i As Integer
            '    For i = 1 To GridView.Columns.Count - 1
            '        bottomPagerRow.Cells(i).Visible = False
            '    Next


            '    '���Ƥέ��X���
            '    If SqlDataSource IsNot Nothing Then
            '        Dim dt As DataTable = CType(SqlDataSource.Select(New DataSourceSelectArguments), DataView).Table
            '        Dim PageSize As Integer = GridView.PageSize
            '        '20100601 �׭q gridview �̭����椸�C���p��
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
            '        bottomPagerRow.Cells(0).Text = "<span class=""PageInfo"">���X " & GridView.PageIndex + 1 & " / " & TotalPageCount & " �� �`�p " & TotalRowCount & " �����</span>"
            '    End If
            '    '���Ƥέ��X���
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
            '        bottomPagerRow.Cells(0).Text = "<span class=""PageInfo"">���X " & GridView.PageIndex + 1 & " / " & TotalPageCount & " �� �`�p " & TotalRowCount & " �����</span>"
            '    End If
            'End If

            'GridView.Caption = "<span class=""PageInfo"">�ثe�Ҧb���X(" & GridView.PageIndex + 1 & "/" & GridView.PageCount & ")  </span>"
            'GridView.CaptionAlign = TableCaptionAlign.Right

            '���Ƥέ��X���
            If SqlDataSource IsNot Nothing Then
                Dim dt As DataTable = CType(SqlDataSource.Select(New DataSourceSelectArguments), DataView).Table
                Dim PageSize As Integer = GridView.PageSize
                Dim TotalRowCount As Integer = dt.Rows.Count
                Dim TotalPageCount As Integer = Math.Ceiling(TotalRowCount / PageSize)
                GridView.Caption = "<span class=""PageInfo"">���X " & GridView.PageIndex + 1 & " / " & TotalPageCount & " �� �`�p " & TotalRowCount & " �����</span>"
            End If

            SetCss3(GridView, ShowPageInfo)
        End If
    End Sub
    ''' <summary>
    ''' �]�w���˦� Setting Css Style
    ''' </summary>
    ''' <param name="gv">GridView����</param>
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
        gv.EmptyDataText = "�ثe�S����ơA�αz���d�ߨS����ơC"
        gv.PagerSettings.Mode = PagerButtons.NumericFirstLast
        '20100415 <��1��> <�e10��><��10��><�̫�1��>
        gv.PagerSettings.FirstPageText = "<<<"
        gv.PagerSettings.LastPageText = ">>>"
        gv.PagerSettings.NextPageText = ">"
        gv.PagerSettings.PreviousPageText = "<"
        Dim i, j, k, l, m As Integer
        '���opager row
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
                                        lkb.ToolTip = "<�e10��>"
                                        lkb.Text = "<<"
                                        p10Key = True
                                    ElseIf lkb.Text.Equals("...") Then
                                        lkb.ToolTip = "<��10��>"
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
                    'tlkb.Text = "<�W1��>"
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
    ''' SqlDataSource���J�s��ƫ�A���o�D���
    ''' </summary>
    ''' <param name="e">SqlDataSourceStatusEventArgs</param>
    ''' <returns>�D��Ȫ��r��</returns>
    ''' <remarks></remarks>
    Public Function GetSDSInsert_DataKey(ByVal e As System.Web.UI.WebControls.SqlDataSourceStatusEventArgs) As String
        '�i��s�����p�@�~
        Dim command As Common.DbCommand
        command = e.Command
        '���oSqlDataSource_GroupAdd�s�W�᪺�D��
        Dim key As Integer = CInt(command.Parameters("@retVal").Value.ToString())

        Return key
    End Function

#End Region


#Region "�ۭq����2008/3/12"
    ''' <summary>
    ''' �]�wGridView�`����/���X���
    ''' </summary>
    ''' <param name="GridView">���M�Ϊ�GridView���</param>
    ''' <param name="Webform">Webform��檫��</param>
    ''' <param name="SqldataSource">�M�Φb��GridView����ƨӷ�SqldataSource</param>
    ''' <param name="ShowPageInfo">�O�_�e�{���X���</param>
    ''' <remarks></remarks>
    Public Sub GetGridViewInfoAppTable(ByVal GridView As System.Web.UI.WebControls.GridView, ByVal Webform As System.Web.UI.Page, Optional ByVal SqldataSource As SqlDataSource = Nothing, Optional ByVal ShowPageInfo As Boolean = True)
        If GridView.Rows.Count > 0 Then
            If ShowPageInfo = True Then
                Dim bottomPagerRow As GridViewRow = GridView.FooterRow
                bottomPagerRow.Cells(0).Text = "<span class=""PageInfo"">�ثe�Ҧb���X(" & GridView.PageIndex + 1 & "/" & GridView.PageCount & ")  </span>"
                bottomPagerRow.Cells(0).ColumnSpan = GridView.Columns.Count

                Dim i As Integer
                For i = 1 To GridView.Columns.Count - 1
                    bottomPagerRow.Cells(i).Visible = False
                Next


                '���Ƥέ��X���
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
                    bottomPagerRow.Cells(0).Text = "<span class=""PageInfo"">���X " & GridView.PageIndex + 1 & " / " & TotalPageCount & " �� �`�p " & TotalRowCount & " �����</span>"
                End If
            End If

            'GridView.Caption = "<span class=""PageInfo"">�ثe�Ҧb���X(" & GridView.PageIndex + 1 & "/" & GridView.PageCount & ")  </span>"
            'GridView.CaptionAlign = TableCaptionAlign.Right

            ''���Ƥέ��X���
            'If SqlDataSource IsNot Nothing Then
            '    Dim dt As DataTable = CType(SqlDataSource.Select(New DataSourceSelectArguments), DataView).Table
            '    Dim PageSize As Integer = GridView.PageSize
            '    Dim TotalRowCount As Integer = dt.Rows.Count
            '    Dim TotalPageCount As Integer = Math.Ceiling(TotalRowCount / PageSize)
            '    GridView.Caption = "<span class=""PageInfo"">���X " & GridView.PageIndex + 1 & " / " & TotalPageCount & " �� �`�p " & TotalRowCount & " �����</span>"
            'End If

            SetCssAppTable(GridView, ShowPageInfo)
        End If
    End Sub
    ''' <summary>
    ''' �]�w���˦� Setting Css Style
    ''' </summary>
    ''' <param name="gv">GridView����</param>
    ''' <remarks></remarks>
    Private Sub SetCssAppTable(ByVal gv As GridView, ByVal ShowFooter As Boolean)
        gv.PagerStyle.CssClass = "DgPager"
        gv.GridLines = GridLines.None
        gv.EmptyDataText = "�ثe�S����ơA�αz���d�ߨS����ơC"
        gv.PagerSettings.Mode = PagerButtons.NumericFirstLast
        '20100415 <��1��> <�e10��><��10��><�̫�1��>
        gv.PagerSettings.FirstPageText = "<��1��>"
        gv.PagerSettings.LastPageText = "<�̲׭�>"
        gv.PagerSettings.NextPageText = "<�U��>"
        gv.PagerSettings.PreviousPageText = "<�e��>"
        Dim i, j, k, l, m As Integer
        '���opager row
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
                                        lkb.Text = "<�e10��>"
                                        p10Key = True
                                    ElseIf lkb.Text.Equals("...") Then
                                        lkb.Text = "<��10��>"
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
                    'tlkb.Text = "<�W1��>"
                    'tlkb.CommandName = "Page"
                    'tlkb.CommandArgument = gv.PageIndex - 1
                    'tlkb.OnClientClick = "javascript:__doPostBack('ctl00$ContentPlaceHolder1$GridView1','Page$" & gv.PageIndex - 1 & "')"
                    'tlkb.PostBackUrl = "javascript:__doPostBack('ctl00$ContentPlaceHolder1$GridView1','Page$" & gv.PageIndex - 1 & "')"
                    Dim tlkb As Literal = New Literal
                    tlkb.Text = "<a href=""javascript:__doPostBack('" & gv.ClientID.Replace("_", "$") & "','Page$" & gv.PageIndex & "')""><�W1��></a>"
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
                    tlkb.Text = "<a href=""javascript:__doPostBack('" & gv.ClientID.Replace("_", "$") & "','Page$" & gv.PageIndex + 2 & "')"">�U1��</a>"
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
                    tlkb.Text = "<a href=""javascript:__doPostBack('" & gv.ClientID.Replace("_", "$") & "','Page$" & gv.PageIndex & "')"">�W1��</a>"
                    tcp.Controls.Add(tlkb)
                    CType(gv.BottomPagerRow.Controls(0).Controls(0).Controls(0), TableRow).Controls.AddAt(0, tcp)
                End If
                If gv.PageIndex < gv.PageCount - 1 Then
                    Dim tcp As TableCell = New TableCell
                    Dim tlkb As Literal = New Literal
                    tlkb.Text = "<a href=""javascript:__doPostBack('" & gv.ClientID.Replace("_", "$") & "','Page$" & gv.PageIndex + 2 & "')"">�U1��</a>"
                    tcp.Controls.Add(tlkb)
                    CType(gv.BottomPagerRow.Controls(0).Controls(0).Controls(0), TableRow).Controls.AddAt(CType(gv.BottomPagerRow.Controls(0).Controls(0).Controls(0), TableRow).Controls.Count, tcp)
                End If
            End If
        End If

        gv.ShowFooter = ShowFooter
    End Sub
    ''' <summary>
    ''' �ۭq���X(�A�Ω�GridView�PSqlDataSource)
    ''' </summary>
    ''' <param name="Webform">Webform</param>
    ''' <param name="Pager">CutePager.Pager</param>
    ''' <param name="gv">GridView</param>
    ''' <param name="sds">SqlDataSource���ѭp���Ƽƨϥ�</param>
    ''' <remarks></remarks>
    Public Sub BindPager(ByVal Webform As System.Web.UI.Page, ByVal Pager As CutePager.Pager, ByVal gv As GridView, ByVal sds As SqlDataSource)
        If 1 = 1 Then
            '�ɨ� �W����pager info 2010/05/26 Chris
            GetGridViewInfoAppTable(gv, Webform, sds)
        Else  '���U���{�����n�����A�H��d���n�|��
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
                        'i���ѼƦb�̫e��
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
                '����GridView�������Ʀr����
                gv.BottomPagerRow.Visible = False
            End If
        End If

    End Sub

#End Region

End Module

