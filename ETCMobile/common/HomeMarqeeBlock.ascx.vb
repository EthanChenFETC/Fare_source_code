Imports System.Data
Imports System.Data.SqlClient

''' <summary>
''' 廣告媒體區模組
''' </summary>
''' <remarks></remarks>
Partial Class common_HomeMarqeeBlock
    Inherits System.Web.UI.UserControl

    Private _GroupIDs As String
    Private _SiteMapGroupID As String
    Private _CallMode As Boolean
    Private _GroupID As String

    ''' <summary>
    ''' 區塊ID，即在資料表中區塊主鍵
    ''' </summary>
    ''' <value>ID值</value>
    ''' <remarks></remarks>
    WriteOnly Property GroupIDs() As String
        Set(ByVal value As String)
            _GroupIDs = value
        End Set
    End Property
    WriteOnly Property SiteMapGroupID() As String
        Set(ByVal value As String)
            _SiteMapGroupID = value
        End Set
    End Property

    WriteOnly Property GroupID() As String
        Set(ByVal value As String)
            _GroupID = value
        End Set
    End Property
    WriteOnly Property CallMode() As Boolean
        Set(ByVal value As Boolean)
            _CallMode = value
        End Set
    End Property

    Private _newsNewDate As Integer = 3 '顯示new的天數
    Private _newsWordLimit As Integer = 26  '字數限制

    '''' <summary>
    '''' 取得媒體主題
    '''' </summary>
    '''' <value></value>
    '''' <returns></returns>
    '''' <remarks></remarks>
    'ReadOnly Property MediaSubject() As String
    '    Get
    '        Return _MediaSubject
    '    End Get
    'End Property

    ''' <summary>
    ''' 頁面讀取事件
    ''' </summary>
    ''' <param name="sender">觸發本事件控制項</param>
    ''' <param name="e">觸發本事件帶進之參數</param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'If System.Configuration.ConfigurationManager.AppSettings("BlockWordLimit") IsNot Nothing Then
        '    _newsWordLimit = System.Configuration.ConfigurationManager.AppSettings("BlockWordLimit").ToString
        'End If
        If _CallMode = False Then
        Else
        End If
        Dim SiteID As String = System.Configuration.ConfigurationManager.AppSettings("SiteID")
        If Not IsNumeric(SiteID) Then
            SiteID = "2"
        End If
        getGroupID(SiteID)
        Me.sds_news_marquee.SelectParameters("SiteID").DefaultValue = SiteID
    End Sub
    ''' <summary>
    ''' 政策公告-頭條專區
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub getGroupID(ByVal SiteID As Integer)
        Dim UploadPath As String = PathManager.GetUploadPath().ToString
        'Dim PublishInfomationID As Integer = PublishInfomation.doGetPublishInfomationIDCookie(Me)
        'If InfoID > 0 Then PublishInfomationID = InfoID
        '要修成為關鍵字(非NodeID)
        Dim sqlStr As String = "SELECT TOP 1 GroupID, ItemCount FROM SiteMapGroupCatgry WHERE SiteID=@SiteID and SiteMapGroupID = @SiteMapGroupID and IsOnline = 1 order by Updatedatetime desc"
        Using dr As SqlDataReader = ClassDB.GetDataReaderParam(sqlStr, New SqlParameter("@SiteID", SiteID), New SqlParameter("@SiteMapGroupID", _SiteMapGroupID))
            Try
                If dr IsNot Nothing Then
                    If dr.Read Then
                        _GroupID = CInt(dr("GroupID"))
                        Me.sds_news_marquee.SelectParameters("GroupID").DefaultValue = _GroupID
                        Me.sds_news_marquee.SelectParameters("Qty").DefaultValue = CInt(dr("ItemCount"))
                    End If
                End If
            Catch ex As Exception
                'WriteErrLog(ex, Me)
            Finally

            End Try
        End Using
    End Sub
    ''' <summary>
    ''' 重大消息
    ''' </summary>
    ''' <param name="sender">觸發本事件控制項</param>
    ''' <param name="e">觸發本事件帶進之參數</param>
    ''' <remarks></remarks>
    Protected Sub Repeater1_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles Repeater1.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem OrElse e.Item.ItemType = ListItemType.Item Then
            If e.Item.ItemIndex > 0 Then
                'e.Item.Visible = False

            End If
            Dim dr As DataRowView = CType(e.Item.DataItem, DataRowView)
            '主旨
            Dim HyperLink1 As HyperLink = CType(e.Item.FindControl("hyperSubject"), HyperLink)
            Dim Subject As String = RemoveXSS(dr.Item("Subject").ToString) 'ModuleMisc.LimitWord(dr.Item("Subject").ToString, _newsWordLimit)
            HyperLink1.Text = (Subject)
            HyperLink1.ToolTip = Subject
            'Chris 20090708 如果標題設定HyperLink 就連到SubjectLink網址
            If Not IsDBNull(dr("SubjectLink")) Then
                If dr("SubjectLink").ToString.Length > 0 Then
                    HyperLink1.NavigateUrl = RemoveXSS(dr("SubjectLink").ToString)
                    HyperLink1.Attributes.Add("Target", "_Blank")
                Else
                    HyperLink1.NavigateUrl = String.Format("~/Publish.aspx?cnid={0}&p={1}", CInt(dr.Item("NodeID")).ToString, CInt(dr.Item("PublicID")).ToString)
                End If
            Else
                HyperLink1.NavigateUrl = String.Format("~/Publish.aspx?cnid={0}&p={1}", CInt(dr.Item("NodeID")).ToString, CInt(dr.Item("PublicID")).ToString)
            End If
        End If
    End Sub

End Class
