Imports System.IO
Imports System.Data
Imports System.Data.SqlClient

''' <summary>
''' 廣告區塊(使用者控制項)
''' </summary>
''' <remarks></remarks>
Partial Class common_FileManagerCounter
    Inherits System.Web.UI.UserControl

    Private _AttFiles_RepeatLayout_Horizontal As Boolean = False
    Private _AttFiles_RepeatColum As Integer = 1
    Private _AttFiles_Table As Boolean = False
    Private _AttFiles_Editable As Boolean = False
    Private _AttFile_IsShowTitle As Boolean = True
    Private _AttFile_IsShowEmptyWord As Boolean = True
    Private _AttFile_WordLimit As Integer = 0
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
                If CheckSqlInjectionWording(AttFilesID) Then
                    Exit Sub
                End If
                If Not Regex.IsMatch(AttFilesID.Replace(",", ""), "^[0-9]+$") Then
                    Exit Sub
                End If
                Dim ds As DataSet
                Dim sqlString As String = "SELECT AttFileID,FileName,FilePath,FileTitle,FileSize,FileMime,IdentityCode, UpdateDateTime,DownloadCount FROM AttachFiles WHERE AttFileID IN ( @AttFilesID) order by ItemOrder" '增加排序功能20081029
                ds = ClassDB.RunReturnDataSet(sqlString.Replace("@AttFilesID", AttFilesID), "AttachFiles_GetSeries")


                ds.Tables(0).Columns.Add("ToolTipsWording")
                For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                    Dim dRow As DataRow = ds.Tables(0).Rows(i)
                    dRow("ToolTipsWording") = "File Name:" & dRow("FileName") & vbCrLf & "File Size:" & CType(dRow("FileSize"), String) & "Byte" & vbCrLf & "Last Updated:" & dRow("UpdateDateTime")
                Next

                '限制顯示字數
                If _AttFile_WordLimit > 0 Then
                    For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                        Dim dRow As DataRow = ds.Tables(0).Rows(i)
                        dRow("FileTitle") = ModuleMisc.LimitWord(dRow("FileTitle"), _AttFile_WordLimit)
                    Next
                End If

                If ds.Tables(0).Rows.Count = 0 Then
                    Me.dl.Visible = False
                Else
                    Dim dv As DataView = ds.Tables(0).DefaultView
                    Me.dl.DataSource = dv
                    Me.dl.DataBind()
                End If

                'text
                Me.lbDownloadText.Text = GlobalResourcesCulture.GetGlobalResourcesString("LabelForDownloadText").ToString

                '是否顯示標題(附件下載)
                Me.lbDownloadText.Visible = _AttFile_IsShowTitle
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
            '下載次數
            Dim lbDlCount As Label = CType(e.Item.FindControl("lbDlCount"), Label)
            lbDlCount.Text = rowView.Row("DownloadCount").ToString
        End If
    End Sub


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
