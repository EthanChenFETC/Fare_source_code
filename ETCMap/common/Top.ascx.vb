Imports System.Data
Imports System.Data.SqlClient
''' <summary>
''' 本資訊網上方選單項目
''' </summary>
''' <remarks></remarks>
Partial Class common_TopArea
    Inherits System.Web.UI.UserControl
    Private _GroupIDs As String
    Private _SiteMapGroupID As String = "ChineseTop"
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
    ''' <summary>
    ''' 頁面讀取事件
    ''' </summary>
    ''' <param name="sender">觸發本事件控制項</param>
    ''' <param name="e">觸發本事件帶進之參數</param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Dim Injection As Sql_Injection = New Sql_Injection
        'Injection.Check_Sql_Injection(Request, Response)
        '無障礙
        Dim Menu As String = WebMenuJs.BuidMenu
        Me.ltMenu.Text = Menu
        Dim SiteID As Integer = (System.Configuration.ConfigurationManager.AppSettings("SiteID").ToString)
        getGroupID(SiteID)
        Me.sds_TopMenu.SelectParameters("SiteID").DefaultValue = SiteID
        If Session("SessionID") Is Nothing Then
            Session.Add("SessionID", Session.SessionID)
            ClassDB.UpdateDB("FareCalculatorPageCount", New SqlParameter("SiteID", SiteID))
        End If
        If Not IsPostBack Then
            UpdateCounter()
            ModuleCounter.WriteCounter(Me.Page)
        End If
    End Sub
    ''' <summary>
    ''' 重大消息
    ''' </summary>
    ''' <param name="sender">觸發本事件控制項</param>
    ''' <param name="e">觸發本事件帶進之參數</param>
    ''' <remarks></remarks>
    Protected Sub Repeater1_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles Repeater1.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem OrElse e.Item.ItemType = ListItemType.Item Then
            Dim dr As DataRowView = CType(e.Item.DataItem, DataRowView)
            '主旨
            Dim HyperLink1 As HyperLink = CType(e.Item.FindControl("hyperSubject"), HyperLink)

            Dim Subject As String = dr.Item("Text").ToString
            HyperLink1.Text = (Subject)
            HyperLink1.ToolTip = Subject
            'Chris 20090708 如果標題設定HyperLink 就連到SubjectLink網址
            If dr("RefPath").ToString.IndexOf("http://") >= 0 Or dr("RefPath").ToString.IndexOf("https://") >= 0 Then
                HyperLink1.NavigateUrl = RemoveXSS(dr("RefPath").ToString)
            Else
                HyperLink1.NavigateUrl = RemoveXSS(PathManager.ApplicationUrl & dr("RefPath").ToString & "?cnid=" & dr("NodeID").ToString)
            End If
            HyperLink1.Attributes.Add("Target", RemoveXSS(dr("Target").ToString))
        End If
    End Sub
    ''' <summary>
    ''' 政策公告-頭條專區
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub getGroupID(ByVal SiteID As Integer)
        Dim UploadPath As String = RemoveXSS(PathManager.GetUploadPath().ToString)

        'Dim PublishInfomationID As Integer = PublishInfomation.doGetPublishInfomationIDCookie(Me)
        'If InfoID > 0 Then PublishInfomationID = InfoID


        '要修成為關鍵字(非NodeID)
        Dim sqlStr As String = "SELECT TOP 1 GroupID, ItemCount FROM SiteMapGroupCatgry WHERE SiteID=@SiteID and SiteMapGroupID = @SiteMapGroupID and IsOnline = 1 order by Updatedatetime desc"
        Using dr As SqlDataReader = ClassDB.GetDataReaderParam(sqlStr, New SqlParameter("@SiteID", SiteID), New SqlParameter("@SiteMapGroupID", _SiteMapGroupID))
            Try
                If dr IsNot Nothing Then
                    If dr.Read Then
                        _GroupID = dr("GroupID")
                        Me.sds_TopMenu.SelectParameters("GroupID").DefaultValue = RemoveXSS(dr("GroupID"))
                        Me.sds_TopMenu.SelectParameters("Qty").DefaultValue = RemoveXSS(dr("ItemCount"))
                    End If
                End If
            Catch ex As Exception
                'WriteErrLog(ex, Me)
            Finally

            End Try
        End Using
    End Sub

#Region "計數器"


    ''' <summary>
    ''' 計數器寫入客戶端的Cookie
    ''' </summary>
    ''' <remarks>30分鐘後過期</remarks>
    Private Sub UpdateCounter()
        Dim SiteID As String = System.Configuration.ConfigurationManager.AppSettings("SiteID").ToString
        If Not IsNumeric(SiteID) Then
            Exit Sub
        End If
        If CheckVisitedCookie() = False Then    '新進人次
            ClassDB.UpdateDB("Net2_F_SiteCounter_Update", New SqlParameter("@SiteID", CInt(SiteID)))
            AddVisitedCookie()
        End If
    End Sub

    ''' <summary>
    ''' 計數器寫入客戶端的Cookie
    ''' </summary>
    ''' <remarks>30分鐘後過期</remarks>
    Private Sub AddVisitedCookie()
        Response.Cookies("farenormalvisited").Expires = DateTime.Now.AddMinutes(10)
        Response.Cookies("farenormalvisited").Value = "1"
    End Sub

    ''' <summary>
    ''' 計數器取得客戶端的Cookie
    ''' </summary>
    ''' <returns>布林值(True/False)</returns>
    ''' <remarks>判斷30分鐘內是否已經造訪過本站</remarks>
    Private Function CheckVisitedCookie() As Boolean
        If Request.Cookies("farenormalvisited") Is Nothing Then
            Return False
        Else
            Return True
        End If
    End Function

#End Region

End Class
