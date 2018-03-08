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
        Dim PageTitle As String = ""
        Dim nodeid As Integer = Sitemap.GetNodeID_Auto(Me.Page)
        Dim PageUrl As String = Request.RawUrl.ToLower
        PageTitle = Sitemap.GetNodeText_byNodeID(nodeid)
        'Sitemap.GetNodeText_Auto(Me.Page)

        Me.divfare.Visible = True
        Me.Page.Title &= "-" & PageTitle
        If PageUrl.ToString.Length > 0 Then

            If PageUrl.IndexOf("custom") >= 0 Then
                Me.divlink.Attributes("class") = "txt_title_01"
                Me.divbutton.Attributes("class") = "btn_suggest"
                Me.alink.HRef = PathManager.ApplicationUrl & "Suggested.aspx"
            ElseIf PageUrl.IndexOf("suggest") >= 0 Then
                Me.divlink.Attributes("class") = "txt_title_02"
                Me.divbutton.Attributes("class") = "btn_way"
                Me.alink.HRef = PathManager.ApplicationUrl & "Custom.aspx"
            ElseIf PageUrl.IndexOf("fare_calculatoropinion") >= 0 Then
                Me.divlink.Attributes("class") = "txt_title_03"
                Me.divbutton.Visible = False
                Me.divfare.Visible = False
            ElseIf PageUrl.IndexOf("publish") >= 0 Then
                Me.divlink.Attributes("class") = "txt_title_05"
                Me.divbutton.Visible = False
                Me.divfare.Visible = False
            Else
                Me.divcontent_title.Visible = False
                Me.divfare.Visible = False
                Me.btn_back.Visible = False
            End If
            Dim TitlePic As DataTable = New DataTable
            If nodeid > 0 Then
                TitlePic = GetNodeTitlePic(nodeid)
                If TitlePic.Rows.Count > 0 Then
                    Me.imgMenuTitle.ImageUrl = RemoveXSS(PathManager.ApplicationUrl & PathManager.GetUploadPath & TitlePic.Rows(0)("TitlePic"))
                End If
            Else
                Dim cnhb As Integer = IIf(IsNumeric(Request.Params("cnhb")), Request.Params("cnhb"), 0)
                If cnhb > 0 Then
                    TitlePic = GetcnhbTitlePic(cnhb)
                    If TitlePic.Rows.Count > 0 Then
                        Me.imgMenuTitle.ImageUrl = RemoveXSS(PathManager.ApplicationUrl & TitlePic.Rows(0)("TitlePic").replace("~/", ""))
                    End If
                End If
            End If
            Dim SiteID As Integer = (System.Configuration.ConfigurationManager.AppSettings("SiteID").ToString)
            If Session("SessionID") Is Nothing Then
                Session.Add("SessionID", Session.SessionID)
                ClassDB.UpdateDB("FareCalculatorPageCount", New SqlParameter("SiteID", SiteID))
            End If
            If Not IsPostBack Then
                UpdateCounter()
                ModuleCounter.WriteCounter(Me.Page)
            End If
        End If
    End Sub

    ''' <summary>
    ''' 政策公告-頭條專區
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub getGroupID(ByVal SiteID As Integer)
        Dim UploadPath As String = PathManager.GetUploadPath().ToString

        'Dim PublishInfomationID As Integer = PublishInfomation.doGetPublishInfomationIDCookie(Me)
        'If InfoID > 0 Then PublishInfomationID = InfoID

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
        Response.Cookies("faremobilevisited").Expires = DateTime.Now.AddMinutes(10)
        Response.Cookies("faremobilevisited").Value = "1"
    End Sub

    ''' <summary>
    ''' 計數器取得客戶端的Cookie
    ''' </summary>
    ''' <returns>布林值(True/False)</returns>
    ''' <remarks>判斷30分鐘內是否已經造訪過本站</remarks>
    Private Function CheckVisitedCookie() As Boolean
        If Request.Cookies("faremobilevisited") Is Nothing Then
            Return False
        Else
            Return True
        End If
    End Function

#End Region

End Class
