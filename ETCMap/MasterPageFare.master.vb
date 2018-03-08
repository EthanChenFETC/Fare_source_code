Imports Microsoft.Security.Application
Imports System.Data
Imports System.Data.SqlClient
''' <summary>
''' 中文一般人員網頁編排主版
''' </summary>
''' <remarks></remarks>
Partial Class MasterPageFare
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
        Dim Injection As Sql_Injection = New Sql_Injection
        Injection.Check_Sql_Injection(Request, Response, Me.Page)
        Injection.Check_InputInteger(Request, Response, Me.Page)

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
        If PageTitle.Length > 0 Then
            Me.imgMenuTitle.ToolTip = PageTitle  '執行頁面標題顯示
            Me.imgMenuTitle.AlternateText = PageTitle  '執行頁面標題顯示
            Dim dt As DataTable = Sitemap.GetNodeTitlePic(nodeid)
            If dt.Rows.Count > 0 Then
                Me.imgMenuTitle.ImageUrl = PathManager.ApplicationUrl & PathManager.GetUploadPath & dt.Rows(0)("TitlePic")
            End If
        End If
        'If Not Request.Path.EndsWith("Publish.aspx") Then
        '    '非多向上稿單元
        '    PageTitle = Sitemap.GetNodeText_Auto(Me.Page)
        '    If PageTitle.Length > 0 Then
        '        Me.lbPageTitle.Text = "<h2>" & PageTitle & "</h2>"  '執行頁面標題顯示
        '    End If
        'Else
        '    '多向上稿單元
        '    'Me.hyForward.NavigateUrl = "~/MailForward.aspx?" & Request.Params.ToString
        '    'Me.hyForward.Visible = True
        '    '以上8/31因為怕轉寄黑函所以停用
        '    CheckPIDExist(Request) '20100531 Chris forever loop
        'End If

        Me.Page.Title = System.Configuration.ConfigurationManager.AppSettings("SiteName").ToString

        If Not IsPostBack Then
            ModuleCounter.WriteCounter(Me.Page)
        End If

        Me.TopArea1.SiteMapGroupID = IIf(ConfigurationManager.AppSettings("TopMenuGroup") Is Nothing, "ChineseTop", RemoveSqlInjection(ConfigurationManager.AppSettings("TopMenuGroup")))
        If nodeid > 0 Then
            Using dr As SqlDataReader = ClassDB.GetDataReaderParam("Select RefPath from SiteMap where NodeID = @NodeID", New SqlParameter("NodeID", nodeid))
                Try
                    If dr IsNot Nothing Then
                        If dr.Read Then
                            If dr("RefPath").ToString.ToLower.Equals("custom.aspx") Then
                                'Me.imgMenuTitle.ImageUrl = "~/images/custom_01.gif"
                                Me.HyperLink1.ToolTip = "地圖化查詢"
                                Me.HyperLink1.NavigateUrl = "SuggestedMap.aspx"
                                Me.Image1.ImageUrl = "~/images/btn_SuggestedMap2.gif"
                                Me.Image1.BorderWidth = "0"
                                Me.Image1.Attributes.Add("onmouseover", "this.src='images/btn_SuggestedMap2a.gif'")
                                Me.Image1.Attributes.Add("onfocus", "this.src='images/btn_SuggestedMap2.gif'")
                                Me.Image1.Attributes.Add("onmouseout", "this.src='images/btn_SuggestedMap2.gif'")
                                Me.Image1.Attributes.Add("onblur", "this.src='images/btn_SuggestedMap2a.gif'")

                                Me.HyperLink2.ToolTip = "最低費用查詢"
                                Me.HyperLink2.NavigateUrl = "Suggested.aspx"
                                Me.Image2.ImageUrl = "~/images/btn_Suggested2.gif"
                                Me.Image2.BorderWidth = "0"
                                Me.Image2.Attributes.Add("onmouseover", "this.src='images/btn_Suggested2a.gif'")
                                Me.Image2.Attributes.Add("onfocus", "this.src='images/btn_Suggested2.gif'")
                                Me.Image2.Attributes.Add("onmouseout", "this.src='images/btn_Suggested2.gif'")
                                Me.Image2.Attributes.Add("onblur", "this.src='images/btn_Suggested2a.gif'")

                            ElseIf dr("RefPath").ToString.ToLower.Equals("suggested.aspx") Then
                                'Me.imgMenuTitle.ImageUrl = "~/images/suggested_01.gif"
                                Me.HyperLink1.ToolTip = "地圖化查詢"
                                Me.HyperLink1.NavigateUrl = "SuggestedMap.aspx"
                                Me.Image1.ImageUrl = "~/images/btn_SuggestedMap2.gif"
                                Me.Image1.BorderWidth = "0"
                                Me.Image1.Attributes.Add("onmouseover", "this.src='images/btn_SuggestedMap2a.gif'")
                                Me.Image1.Attributes.Add("onfocus", "this.src='images/btn_SuggestedMap2.gif'")
                                Me.Image1.Attributes.Add("onmouseout", "this.src='images/btn_SuggestedMap2.gif'")
                                Me.Image1.Attributes.Add("onblur", "this.src='images/btn_SuggestedMap2a.gif'")

                                Me.HyperLink2.ToolTip = "自訂路線查詢"
                                Me.HyperLink2.NavigateUrl = "Custom.aspx"
                                Me.Image2.ImageUrl = "~/images/btn_Custom2.gif"
                                Me.Image2.BorderWidth = "0"
                                Me.Image2.Attributes.Add("onmouseover", "this.src='images/btn_Custom2a.gif'")
                                Me.Image2.Attributes.Add("onfocus", "this.src='images/btn_Custom2.gif'")
                                Me.Image2.Attributes.Add("onmouseout", "this.src='images/btn_Custom2.gif'")
                                Me.Image2.Attributes.Add("onblur", "this.src='images/btn_Custom2a.gif'")

                            ElseIf dr("RefPath").ToString.ToLower.Equals("suggestedmap.aspx") Then
                                'Me.imgMenuTitle.ImageUrl = "~/images/suggestedMaptit.gif"
                                Me.HyperLink1.ToolTip = "最低費用查詢"
                                Me.HyperLink1.NavigateUrl = "Suggested.aspx"
                                Me.Image1.ImageUrl = "~/images/btn_Suggested2.gif"
                                Me.Image1.BorderWidth = "0"
                                Me.Image1.Attributes.Add("onmouseover", "this.src='images/btn_Suggested2a.gif'")
                                Me.Image1.Attributes.Add("onfocus", "this.src='images/btn_Suggested2.gif'")
                                Me.Image1.Attributes.Add("onmouseout", "this.src='images/btn_Suggested2.gif'")
                                Me.Image1.Attributes.Add("onblur", "this.src='images/btn_Suggested2a.gif'")

                                Me.HyperLink2.ToolTip = "自訂路線查詢"
                                Me.HyperLink2.NavigateUrl = "Custom.aspx"
                                Me.Image2.ImageUrl = "~/images/btn_Custom2.gif"
                                Me.Image2.BorderWidth = "0"
                                Me.Image2.Attributes.Add("onmouseover", "this.src='images/btn_Custom2a.gif'")
                                Me.Image2.Attributes.Add("onfocus", "this.src='images/btn_Custom2.gif'")
                                Me.Image2.Attributes.Add("onmouseout", "this.src='images/btn_Custom2.gif'")
                                Me.Image2.Attributes.Add("onblur", "this.src='images/btn_Custom2a.gif'")

                            End If
                        End If
                    End If
                Catch ex As Exception

                End Try
            End Using
        End If
    End Sub

    ''' <summary>
    ''' 檢查多向上稿pid與cnid是否對應
    ''' </summary>
    ''' <param name="webrequest">觸發本事件控制項</param>
    ''' <remarks></remarks>
    Private Function CheckPIDExist(ByVal webrequest As HttpRequest) As Boolean
        Dim ret As Boolean = True
        If Not IsNumeric(webrequest.Params("p")) And Not IsNumeric(webrequest.Params("cnid")) Then
            Dim cnid As Integer = 0
            Dim p As Integer = 0
            Dim sqlStr As String = "Select nodeid from PublicationMenuRelation where nodeid=@NodeID and PublicID = @PublicID"
            Using dr As SqlDataReader = ClassDB.GetDataReaderParam(sqlStr, New SqlParameter("@NodeID", cnid), New SqlParameter("@PublicID", p))
                Try
                    If dr IsNot Nothing Then
                        If Not dr.Read Then
                            Me.Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "waring", "alert('本單元並無此文章，請重新選擇!\n');document.location.href = '" & PathManager.ApplicationUrl() & "Default.aspx';", True)
                            'Response.End()
                            ret = False
                        End If
                    End If
                Catch ex As Exception
                    Dim exx As String = ex.Message
                Finally
                End Try
            End Using
        End If
        Return ret
    End Function
End Class

