Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Xml
Imports System.Xml.XPath
Partial Class _Default
    Inherits InjectionPage
    Dim i As Integer = 0
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim cnid As String = Request.QueryString.Item("cnid")
        If String.IsNullOrEmpty(cnid) Then
            Dim Injection As Sql_Injection = New Sql_Injection
            Injection.Check_Sql_Injection(Request, Response, Me.Page)

        End If


        Dim SiteID As Integer = CInt(ConfigurationManager.AppSettings("SiteID"))

        If Session("SessionID") Is Nothing Then
            Session.Add("SessionID", Session.SessionID)
            ClassDB.UpdateDB("FareCalculatorPageCount", New SqlParameter("SiteID", SiteID))
        End If
        availableTagst.Value = IIf(ConfigurationManager.AppSettings("MUrl") Is Nothing, "http://fare.fetc.net.tw/mobile/", RemoveXSS(ConfigurationManager.AppSettings("MUrl")))
        'Me.hlMobile.NavigateUrl = availableTagst.Value
        Try
            If Request.UrlReferrer.ToString.Trim.Equals(availableTagst.Value) Or Request.UrlReferrer.ToString.IndexOf(Context.Request.ServerVariables("SERVER_NAME").ToString) >= 0 Then
                availableTagst.Value = ""
            End If
        Catch ex As Exception
        End Try
        If Not IsPostBack Then
            Dim HomePageNews As String = (ConfigurationManager.AppSettings("HomePageNews"))
            If HomePageNews Is Nothing Then
                HomePageNews = "MobileNews"
            End If
            Dim HomePageMarqee As String = (ConfigurationManager.AppSettings("HomePageMarqee"))
            If HomePageMarqee Is Nothing Then
                HomePageMarqee = "MobileMarqee"
            End If
            Dim HomePageAnnounce As String = (ConfigurationManager.AppSettings("HomePageAnnounce"))
            If HomePageAnnounce Is Nothing Then
                HomePageAnnounce = "MobileHome"
            End If
            Dim HomePageAd As String = (ConfigurationManager.AppSettings("HomePageAd"))
            If HomePageAd Is Nothing Then
                HomePageAd = "HomePageAd"
            End If
            Dim HomePageMd As String = (ConfigurationManager.AppSettings("HomePageMd"))
            If HomePageMd Is Nothing Then
                HomePageMd = "HomePageMd"
            End If
            Dim TopMenuGroup As String = (ConfigurationManager.AppSettings("TopMenuGroup"))
            If TopMenuGroup Is Nothing Then
                TopMenuGroup = "MobileMenu"
            End If
            Me.HomePageBlock1.SiteMapGroupID = (HomePageNews)
            Me.HomeMarqeeBlock1.SiteMapGroupID = (HomePageMarqee)
            Me.PublishBlock1.BlockID = (HomePageAnnounce)
            Me.PublishBlock2.BlockID = (HomePageAd)
            Me.PublishBlock3.BlockID = (HomePageMd)
            Me.TopArea1.SiteMapGroupID = (TopMenuGroup)
        End If
    End Sub
End Class
