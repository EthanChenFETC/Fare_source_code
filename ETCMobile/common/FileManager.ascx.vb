Imports System.IO
Imports System.Data
Imports System.Data.SqlClient

''' <summary>
''' 廣告區塊(使用者控制項)
''' </summary>
''' <remarks></remarks>
Partial Class common_FileManager
    Inherits System.Web.UI.UserControl

    Private _AttFiles_RepeatLayout_Horizontal As Boolean = False
    Private _AttFiles_RepeatColum As Integer = 1
    Private _AttFiles_Table As Boolean = False
    Private _AttFiles_Editable As Boolean = False
    Private _AttFile_IsShowTitle As Boolean = True
    Private _AttFile_IsShowEmptyWord As Boolean = True
    Private _AttFile_WordLimit As Integer = 0
    Private _CounterEnable As Integer = 0 '20100604Chris
    'Private _AttFilesID As String

    Dim _SiteDomain As String = PathManager.FullDomainUrl    ' System.Configuration.ConfigurationManager.AppSettings("SiteDomainName").ToString  '"http://" & Request.ServerVariables("SERVER_NAME")
    Dim _UploadPath As String = PathManager.GetUploadPath().ToString


    ''' <summary>
    ''' 自資料庫中取得資料，並將資料繫結至頁面上DataList控制項
    ''' </summary>
    ''' <param name="AttFilesID">資料庫附件資料表之附件檔案之主鍵ID</param>
    ''' <remarks></remarks>
    Public Sub doDataBind(ByVal AttFilesID As String)
        'Me.lbNoFile.Text = "abc"
        If Not AttFilesID = Nothing Then
            If AttFilesID.Length > 0 Then
                Dim ds As DataTable
                If Not Regex.IsMatch(AttFilesID.Replace(",", ""), "^[0-9]+$") Then
                    Exit Sub
                End If
                If Not IsNumeric(AttFilesID.Replace(",", "")) Then
                    Exit Sub
                End If
                Dim sqlString As String = "SELECT AttFileID,FileName,FilePath,FileTitle,FileSize,FileMime, UpdateDateTime,DownloadCount FROM AttachFiles WHERE AttFileID IN ( @AttFilesID ) order by ItemOrder" '增加排序功能20081029
                ds = ClassDB.RunReturnDataTable(sqlString.Replace("@AttFilesID", AttFilesID))
                ds.Columns.Add("ToolTipsWording")
                Dim dt As DataTable = ds.Clone
                For i As Integer = 0 To ds.Rows.Count - 1
                    Dim dRow As DataRow = ds.Rows(i)
                    dRow("ToolTipsWording") = "File Name:" & dRow("FileName") & vbCrLf & "File Size:" & CType(dRow("FileSize"), String) & "Byte" & vbCrLf & "Last Updated:" & dRow("UpdateDateTime")
                Next
                '限制顯示字數
                If _AttFile_WordLimit > 0 Then
                    For i As Integer = 0 To ds.Rows.Count - 1
                        Dim dRow As DataRow = ds.Rows(i)
                        dRow("FileTitle") = ModuleMisc.LimitWord(dRow("FileTitle"), _AttFile_WordLimit)
                    Next
                End If
                If ds.Rows.Count = 0 Then
                    Me.dl.Visible = False
                    Me.lbNoFile.Text = GlobalResourcesCulture.GetGlobalResourcesString("LabelForDownloadNoFile").ToString
                Else
                    Dim dtt As DataTable = New DataTable
                    dtt = ds.Clone
                    For i As Integer = 0 To ds.Rows.Count - 1
                        Dim newrow As DataRow = dtt.NewRow
                        Dim dRow As DataRow = ds.Rows(i)
                        newrow("AttFileID") = CInt(dRow("AttFileID"))
                        newrow("FileName") = (dRow("FileName"))
                        newrow("FilePath") = (dRow("FilePath"))
                        newrow("FileTitle") = (dRow("FileTitle"))
                        newrow("ToolTipsWording") = ("ToolTipsWording")
                        dtt.Rows.Add(newrow)
                    Next
                    Me.dl.DataSource = dtt.DefaultView
                    Me.dl.DataBind()
                End If

                'text
                Me.lbDownloadText.Text = GlobalResourcesCulture.GetGlobalResourcesString("LabelForDownloadText").ToString

                '是否顯示標題(附件下載)
                Me.lbDownloadText.Visible = _AttFile_IsShowTitle

                Me.lbNoFile.Visible = _AttFile_IsShowEmptyWord
            End If
        End If
    End Sub


    ''' <summary>
    ''' DataList控制項資料繫結
    ''' </summary>
    ''' <param name="sender">觸發本事件控制項</param>
    ''' <param name="e">觸發本事件帶進之參數</param>
    ''' <remarks>檔案下載在頁面的呈現設定</remarks>
    Protected Sub dl_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs) Handles dl.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem OrElse e.Item.ItemType = ListItemType.Item Then
            Dim rowView As DataRowView = CType(e.Item.DataItem, DataRowView)
            '下載次數(文字)
            Dim lbdlCountTxt As Label = CType(e.Item.FindControl("lbDlCountTxt"), Label)
            lbdlCountTxt.Text = (GlobalResourcesCulture.GetGlobalResourcesString("LabelForDownloadCount").ToString)

            '下載次數
            Dim lbDlCount As Label = CType(e.Item.FindControl("lbDlCount"), Label)
            lbDlCount.Text = (rowView.Row("DownloadCount").ToString)

            Dim lb_AttFile As HyperLink = CType(e.Item.FindControl("lb_AttFile"), HyperLink)
            'lb_AttFile.ImageUrl = GetIconPath(rowView.Row("FileName").ToString)
            lb_AttFile.NavigateUrl = Page.ResolveUrl("~/Download_File.ashx?id=" & CInt(rowView("AttFileID")).ToString)
            lb_AttFile.ToolTip = (rowView("ToolTipsWording"))
            lb_AttFile.Text = "<img alt=""" & (rowView("FileTitle")) & """ src=""" & GetIconPath(rowView.Row("FileName").ToString) & """/> " & (rowView("FileTitle"))
        End If
    End Sub


    ''' <summary>
    ''' 下載檔案命令
    ''' </summary>
    ''' <param name="sender">觸發本事件控制項</param>
    ''' <param name="e">觸發本事件帶進之參數</param>
    ''' <remarks>使用者點選頁面中的檔案超連結會觸發本事件</remarks>
    Sub GetAttFile_Command(ByVal sender As Object, ByVal e As CommandEventArgs)
        'Dim fs As Stream

        'Dim strContentType As String
        Dim strPath As String = GetUploadPath()
        Dim strFileName As String = e.CommandArgument
        Dim DisplayFileName As String = PathManager.GetAppNameByPath(strFileName)

        Dim FileName As String = RemoveXSS(strPath & strFileName)
        If doCheckFile(strPath & strFileName) = False Then
            Dim script As String = "alert('Sorry~This file does not Exits！\nThat could be Lost!');"
            Me.Page.ClientScript.RegisterClientScriptBlock(Me.Page.GetType, "Waring", script, True)
            ModuleWriteLog.WriteLog(Request.Path, "Attach File Lost Link :" & strPath & strFileName, Me.Page)
            Exit Sub
        End If

        '更新下載次數
        ClassDB.UpdateDB("Net2_F_FileDownload_AddCount", New SqlParameter("@AttFileID", CInt(e.CommandName)))
        Dim bytBytes(10) As Byte
        Using fs As FileStream = File.Open(FileName, FileMode.Open)
            ReDim bytBytes(fs.Length)
            fs.Read(bytBytes, 0, fs.Length)
        End Using
        'Dim bBytes(bytBytes.Length) As Byte
        'bytBytes.CopyTo(bBytes, 0)
        Response.ClearHeaders()
        Response.Clear()
        Response.Expires = 0
        Response.Buffer = True
        'Response.AddHeader("Accept-Language", "zh-tw")
        Response.AddHeader("Content-disposition", "attachment; filename=" & Chr(34) & Server.UrlPathEncode(DisplayFileName) & Chr(34))
        Response.ContentType = "application/octet-stream"
        Response.BinaryWrite(bytBytes)
        Response.End()
    End Sub

    'Function ConvertFileName(ByVal FileName As String) As String
    '    ConvertFileName = Replace(Timer, ".", "") & Right(FileName, InStr(StrReverse(FileName), "."))
    'End Function

    ''' <summary>
    ''' 取得檔案下載的實體路徑
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetUploadPath() As String
        Dim UploadPath As String = _UploadPath

        If Not UploadPath.EndsWith("/") Then     '//<----這裡把路徑變成以/結尾
            UploadPath += "/"
        End If

        If Not UploadPath.Substring(1, 1) = ":" Then
            UploadPath = Server.MapPath(UploadPath)
        End If

        Return UploadPath
    End Function


    ''' <summary>
    ''' 取得小圖示之實體檔案位置
    ''' </summary>
    ''' <param name="FileExtention">檔型(即檔案之副檔名)</param>
    ''' <returns>傳回實體檔案路徑字串</returns>
    ''' <remarks></remarks>
    Private Function GetIconPath(ByVal FileExtention As String) As String
        '取得網站網址(考量設置在次目錄),文章連結必需能符合路徑
        Dim SiteDomainPath As String = _SiteDomain

        If SiteDomainPath.EndsWith("/") Then
            SiteDomainPath = SiteDomainPath.Substring(0, SiteDomainPath.Length - 1)
        End If

        SiteDomainPath = SiteDomainPath & "/Template/Style1/AttFileIcons/"    'docText.gif"
        FileExtention = PathManager.GetExtentionName(FileExtention)
        Select Case FileExtention
            Case "doc"
                SiteDomainPath += "icon_word.gif"

            Case "xls"
                SiteDomainPath += "icon_excel.gif"

            Case "ppt"
                SiteDomainPath += "icon_ppt.gif"

            Case "pdf"
                SiteDomainPath += "icon_pdf.gif"

            Case Else
                SiteDomainPath += "icon_normal.gif"
        End Select

        Return SiteDomainPath

    End Function


    'Public Property AttFiles_RepeatLayout_Horizontal() As Boolean
    '    Get
    '        Return _AttFiles_RepeatLayout_Horizontal
    '    End Get
    '    Set(ByVal Value As Boolean)
    '        _AttFiles_RepeatLayout_Horizontal = Value
    '    End Set
    'End Property

    ''' <summary>
    ''' 檔案在頁面上呈現的欄位數
    ''' </summary>
    ''' <value>數量</value>
    ''' <returns>數量</returns>
    ''' <remarks></remarks>
    Public Property AttFiles_RepeatColum() As Integer
        Get
            Return _AttFiles_RepeatColum
        End Get
        Set(ByVal Value As Integer)
            _AttFiles_RepeatColum = Value
        End Set
    End Property

    'Public Property AttFiles_Table() As Boolean
    '    Get
    '        Return _AttFiles_Table
    '    End Get
    '    Set(ByVal Value As Boolean)
    '        _AttFiles_Table = Value
    '    End Set
    'End Property

    ''' <summary>
    ''' 設定頁面上是否提供編輯的功能
    ''' </summary>
    ''' <value>布林值(是/否)</value>
    ''' <returns>布林值(是/否)</returns>
    ''' <remarks></remarks>
    Public Property AttFiles_Editable() As Boolean
        Get
            Return _AttFiles_Editable
        End Get
        Set(ByVal Value As Boolean)
            _AttFiles_Editable = Value
        End Set
    End Property

    ''' <summary>
    ''' 設定頁面上呈現下載清單中是否呈現欄位標題
    ''' </summary>
    ''' <value>布林值(是/否)</value>
    ''' <remarks></remarks>
    Public WriteOnly Property AttFile_IsShowTitle() As Boolean
        Set(ByVal value As Boolean)
            _AttFile_IsShowTitle = value
        End Set
    End Property

    ''' <summary>
    ''' 設定頁面上是否呈現空字串
    ''' </summary>
    ''' <value>布林值(是/否)</value>
    ''' <remarks></remarks>
    Public WriteOnly Property AttFile_IsShowEmptyWord() As Boolean
        Set(ByVal value As Boolean)
            _AttFile_IsShowEmptyWord = value
        End Set
    End Property

    ''' <summary>
    ''' 設定頁面上呈現出檔名的長度限制
    ''' </summary>
    ''' <value>字數</value>
    ''' <remarks></remarks>
    Public WriteOnly Property AttFile_WordLimit() As Integer
        Set(ByVal value As Integer)
            _AttFile_WordLimit = value
        End Set
    End Property
    ''' <summary>
    ''' 設定頁面上下載次數是否出現 Chris 20100604
    ''' </summary>
    ''' <value>字數</value>
    ''' <remarks></remarks>
    Public WriteOnly Property CounterEnable() As Integer
        Set(ByVal value As Integer)
            _CounterEnable = value
        End Set
    End Property
    ''' <summary>
    ''' 取得實體檔案是否存在
    ''' </summary>
    ''' <param name="AttFile">檔案實體位址字串</param>
    ''' <returns>布林值(是/否)</returns>
    ''' <remarks></remarks>
    Private Function doCheckFile(ByVal AttFile As String) As Boolean
        If Not FileIO.FileSystem.FileExists(AttFile) Then
            Return False
        Else
            Return True
        End If
    End Function




End Class
