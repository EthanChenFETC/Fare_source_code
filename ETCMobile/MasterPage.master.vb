Imports Microsoft.Security.Application
Imports System.Data
Imports System.Data.SqlClient
''' <summary>
''' 中文一般人員網頁編排主版
''' </summary>
''' <remarks></remarks>
Partial Class MasterPage
    Inherits System.Web.UI.MasterPage

    ''' <summary>
    ''' 頁面讀取事件
    ''' </summary>
    ''' <param name="sender">觸發本事件控制項</param>
    ''' <param name="e">觸發本事件帶進之參數</param>
    ''' <remarks>
    ''' 設定網頁標題文字、設定網頁Title文字、設定選單使用的Javascript註冊。
    ''' </remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Dim URL_REGEX As String = "^(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+=&amp;%\$#_]*)?$"
        'If Regex.IsMatch(Request.Params.ToString, URL_REGEX) Then
        '    WriteLog("Match", Request.Params.ToString)
        '    ' This is a valid URL so doing something with it
        'Else
        '    ' This is a potential attack!  Play it safe and error-out
        '    WriteLog("No Match", Request.Params.ToString)
        'End If
        '網頁畫面的標題
        Dim PageTitle As String = ""
        Dim nodeid As Integer = Sitemap.GetNodeID_Auto(Me.Page)
        PageTitle = Sitemap.GetNodeText_Auto(Me.Page)


        Me.Page.Title = System.Configuration.ConfigurationManager.AppSettings("SiteName").ToString

        If Not IsPostBack Then
            ModuleCounter.WriteCounter(Me.Page)
        End If
        Dim Injection As Sql_Injection = New Sql_Injection
        Injection.Check_Sql_Injection(Request, Response, Me.Page)
        Injection.Check_InputInteger(Request, Response, Me.Page)
        Me.TopArea1.SiteMapGroupID = IIf(ConfigurationManager.AppSettings("TopMenuGroup") Is Nothing, "ChineseTop", RemoveXSS(ConfigurationManager.AppSettings("TopMenuGroup")))

    End Sub

    '''' <summary>
    '''' 檢查多向上稿pid與cnid是否對應
    '''' </summary>
    '''' <param name="webrequest">觸發本事件控制項</param>
    '''' <remarks></remarks>
    'Private Function CheckPIDExist(ByVal webrequest As HttpRequest) As Boolean
    '    Dim ret As Boolean = True
    '    If Not IsNumeric(webrequest.Params("p")) And Not IsNumeric(webrequest.Params("cnid")) Then
    '        Dim cnid As Integer = 0
    '        Dim p As Integer = 0
    '        Dim sqlStr As String = "Select nodeid from PublicationMenuRelation where nodeid='" & cnid.ToString & "' and PublicID = '" & p.ToString & "'"
    '        Using dr As SqlDataReader = ClassDB.GetDataReader(sqlStr)
    '            Try
    '                If Not dr.Read Then
    '                    Me.Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "waring", "alert('本單元並無此文章，請重新選擇!\n');document.location.href = '" & PathManager.ApplicationUrl() & "Default.aspx';", True)
    '                    'Response.End()
    '                    ret = False
    '                End If
    '            Catch ex As Exception
    '                Dim exx As String = ex.Message
    '            Finally

    '            End Try
    '        End Using
    '    End If
    '    Return ret
    'End Function
End Class

