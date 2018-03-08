Imports System.Data
Imports System.Data.SqlClient

Partial Class Publish_PublishPreview
    Inherits System.Web.UI.Page
    Private _ErrorMessage As String = "<div id=""PublishContent""><br />目前您瀏覽的單元沒有文章！</div>"


    ''' <summary>
    ''' (20080225mw)
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim Contents As String = Request.Form.Get("Content")

        Me.ltlContent.Text = Contents.Replace(vbCrLf, "</br/>")
        Dim Subject As String = Request.Params("Subject")

        Me.ltlSubject.Text = Subject
        Dim FileID As String = Request.Form.Get("FileID")

        If Not CheckInteger(FileID.Replace(",", "")) Then
            ScriptManager.RegisterClientScriptBlock(Me.Page, GetType(Page), "warning", "alert('資料不正確');window.close();", True)
            Exit Sub
        Else
            doDataBind(FileID)
        End If
    End Sub
    ''' <summary>
    ''' 自資料庫中取得資料，並將資料繫結至頁面上DataList控制項
    ''' </summary>
    ''' <param name="AttFilesID">資料庫附件資料表之附件檔案之主鍵ID</param>
    ''' <remarks></remarks>
    Public Sub doDataBind(ByVal AttFilesID As String)
        'Me.lbNoFile.Text = "abc"
        If Not AttFilesID = Nothing Then
            If CheckSqlInjectionWording(AttFilesID.Replace(",", "")) Or Not Regex.IsMatch(AttFilesID.Replace(",", ""), "^[0-9]+$") Then
                Exit Sub
            End If
            Dim ds As DataSet
            Dim sqlString As String = "SELECT AttFileID,FileName,FilePath,FileTitle,FileSize,FileMime,IdentityCode, UpdateDateTime,DownloadCount FROM AttachFiles WHERE AttFileID IN ( @AttFilesID ) order by ItemOrder".Replace("@AttFilesID", AttFilesID) '增加排序功能20081029
            ds = ClassDB.RunReturnDataSet(sqlString, "AttachFiles_GetSeries", New SqlParameter("@AttFilesID", AttFilesID))
            ds.Tables(0).Columns.Add("ToolTipsWording")
            ds.Tables(0).Columns.Add("FullPath")
            For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                Dim dRow As DataRow = ds.Tables(0).Rows(i)
                dRow("ToolTipsWording") = "File Name:" & RemoveXSS(dRow("FileName")) & vbCrLf & "File Size:" & CType(dRow("FileSize"), String) & "Byte" & vbCrLf & "Last Updated:" & dRow("UpdateDateTime")
                dRow("FullPath") = RemoveXSS(ModulePathManager.GetFullDomainUrl & GetUploadPath() & dRow("FilePath") & dRow("FileName").ToString)
            Next

            '限制顯示字數
            For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                Dim dRow As DataRow = ds.Tables(0).Rows(i)
                dRow("FileTitle") = RemoveXSS(ModuleMisc.LimitWord(dRow("FileTitle"), 100))
            Next
            If ds.Tables(0).Rows.Count = 0 Then
                Me.dl.Visible = False
                Me.lbNoFile.Text = "沒有檔案"
                divdownload.Visible = False
            Else
                Dim dview As DataView = ds.Tables(0).DefaultView
                Me.dl.DataSource = dview
                Me.dl.DataBind()
            End If

            'text
            Me.lbDownloadText.Text = "附加檔案"

            '是否顯示標題(附件下載)
            Me.lbDownloadText.Visible = True

            Me.lbNoFile.Visible = True
        Else
            divdownload.Visible = False
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
            Dim img_AttFile As Image = CType(e.Item.FindControl("img_AttFile"), Image)
            img_AttFile.ImageUrl = GetIconPath(rowView.Row("FileName").ToString)
            img_AttFile.Height = Unit.Pixel(18)
            'img_AttFile.AlternateText = "File Name:" & rowView.Row("FileName") & vbCrLf & "File Size:" & rowView.Row("FileSize").ToString & "Byte" & vbCrLf & "Last Updated:" & rowView.Row("UpdateDateTime").ToString
            Dim lb_AttFile As LinkButton = CType(e.Item.FindControl("lb_AttFile"), LinkButton)
            lb_AttFile.ToolTip = RemoveXSS(rowView("ToolTipsWording"))
            lb_AttFile.Text = (IIf(IsDBNull(rowView("FileTitle")), "", rowView("FileTitle")))

        End If
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
        Dim UploadPath As String = ModulePathManager.GetUploadPath.Replace("~/", "")

        If Not UploadPath.EndsWith("/") Then     '//<----這裡把路徑變成以/結尾
            UploadPath += "/"
        End If

        'If Not UploadPath.Substring(1, 1) = ":" Then
        '    UploadPath = Server.MapPath(UploadPath)
        'End If

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
        Dim SiteDomainPath As String = ModulePathManager.GetFullDomainUrl()

        If SiteDomainPath.EndsWith("/") Then
            SiteDomainPath = SiteDomainPath.Substring(0, SiteDomainPath.Length - 1)
        End If

        SiteDomainPath = SiteDomainPath & "images/"    'docText.gif"
        FileExtention = ModulePathManager.GetExtentionName(FileExtention)
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
End Class
