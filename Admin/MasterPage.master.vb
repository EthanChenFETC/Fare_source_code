Imports System.Data
Imports System.Data.SqlClient
'Imports ClassSecurity.SecurityBase64


Partial Class MasterPage
    Inherits System.Web.UI.MasterPage
    '以下這個函數的程式碼，要點選過 GridView 的頁碼換頁過後，再按瀏覽器的「上一頁」，才會被觸發。
    Protected Sub History1_Navigate(ByVal sender As Object, ByVal args As Microsoft.Web.Preview.UI.Controls.HistoryEventArgs)
        Dim dd As String = Request.Url.ToString
        If dd.IndexOf("Survey/Intra_Survey.aspx") < 0 Then
            'Response.Redirect(Request.Url.ToString)
        End If
    End Sub
    ''' <summary>
    ''' 頁面讀取事件，本事件處理公版事項(20080225mw)
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        'Dim Injection As Sql_Injection = New Sql_Injection
        'Injection.Check_Sql_Injection(Request, Response, Me.Page)
        'Injection.Check_InputInteger(Request, Response, Me.Page)
    End Sub

    ''' <summary>
    ''' 頁面讀取事件，本事件處理公版事項(20080225mw)
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then

            Dim NowNodeID As Integer = GetNowNodeID()

            '驗證有沒有頁面存取權限,若為-1找不到節點則轉到沒有權限(20080225mw)
            If NowNodeID = -1 Then

                Response.Redirect(ModulePathManager.GetAdminPath(Me.Page) & "NoPermission.aspx")
            End If

            '選單UserControl
            InterfaceBuilder.buildMenu(Me.Page, Me.Menu1)

            '導覽列
            Me.AdminNavBar.Text = ModulePermissions.BuildNavBar(Me.Page, NowNodeID)

            ''標纖
            InterfaceBuilder.BuildTab(Me.Page, Me.TabStrip1, NowNodeID)

        Else
            History1.AddHistoryPoint("myPage", 0)
        End If

        '頁面標題
        Me.Page.Title = GetPageTitle()
        '20130209 Chris 這個時候應該要把 session 清掉了吧
        'Session.Remove("ReqPar")
    End Sub

    ''' <summary>
    ''' 取得現在作用中的NodeID(20080225mw)
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetNowNodeID() As Integer
        Dim NodeID As Integer = -1

        If Me.ViewState("NowNodeID") IsNot Nothing Then
            NodeID = CInt(Me.ViewState("NowNodeID"))

        Else

            NodeID = ModulePermissions.GetAdminMenuNodeID(Me.Page)
        End If

        Me.ViewState.Add("NowNodeID", NodeID)
        Return NodeID
    End Function

    Private Function GetPageTitle() As String
        Dim PageTitle As String = ""
        If Me.ViewState("PageTitle") IsNot Nothing Then
            PageTitle = Me.ViewState("PageTitle").ToString
        Else
            PageTitle = ModulePermissions.GetAdminMenuName(Me.Page, GetNowNodeID)
        End If

        If PageTitle <> "" Then
            Me.ViewState.Add("PageTitle", PageTitle)
        End If

        Return PageTitle

    End Function


    '''' <summary>
    '''' 剛登入時預設為Default.ascx
    '''' </summary>
    '''' <returns></returns>
    '''' <remarks></remarks>
    'Protected Function GetHomeUrlValue() As String
    '    Dim UrlValue As String = "1000,~/Intranet/Default.aspx,0,True"
    '    Return UrlValue
    'End Function

    ''' <summary>
    ''' ComponentART Menu Menu1_ItemSelected 選單按下
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Menu1_ItemSelected(ByVal sender As Object, ByVal e As ComponentArt.Web.UI.MenuItemEventArgs) Handles Menu1.ItemSelected
        '如果連結字串為空，表示還有子單元,但仍得限制在4層以內
        Dim url As String = e.Item.Value
        Dim NodeID As Integer = CInt(e.Item.ID)
        Dim i As Integer
        Do While url = "" And i <= 2
            url = ModulePermissions.GetAdminMenuFirstChildNodeUrl(Me.Page, NodeID)
            i += 1
        Loop
        url = RemoveXSS(url.Replace("http://", "").Replace("https://", ""))
        If url <> "" Then
            Response.Redirect(url)
        End If
    End Sub

    Protected Sub TabStrip1_ItemSelected(ByVal sender As Object, ByVal e As ComponentArt.Web.UI.TabStripTabEventArgs) Handles TabStrip1.ItemSelected
        Response.Redirect(e.Tab.Value)
    End Sub

    Protected Sub lkbHome_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lkbHome.Click
        Response.Redirect("~/Default.aspx")
    End Sub


    Protected Sub ScriptManager1_AsyncPostBackError(ByVal sender As Object, ByVal e As System.Web.UI.AsyncPostBackErrorEventArgs) Handles ScriptManager1.AsyncPostBackError
        Me.ScriptManager1.AsyncPostBackErrorMessage = e.Exception.Message & vbCrLf & e.Exception.Source & vbCrLf & e.Exception.StackTrace
        WriteLog("Ajax System Errog Report", e.Exception.Message & vbCrLf & e.Exception.Source & vbCrLf & e.Exception.StackTrace, Me.Page)
    End Sub

End Class

