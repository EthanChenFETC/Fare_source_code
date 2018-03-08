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
        SiteID = CInt(SiteID)
        If Session("SessionID") Is Nothing Then
            Session.Add("SessionID", Session.SessionID)
            ClassDB.UpdateDB("FareCalculatorPageCount", New SqlParameter("SiteID", SiteID))
        End If
        If Not IsPostBack Then
            Me.HomePageBlock1.SiteMapGroupID = IIf(ConfigurationManager.AppSettings("HomePageNews") Is Nothing, "MobileNews", (ConfigurationManager.AppSettings("HomePageNews")))
            Me.HomeMarqeeBlock1.SiteMapGroupID = IIf(ConfigurationManager.AppSettings("HomePageMarqee") Is Nothing, "MobileMarqee", (ConfigurationManager.AppSettings("HomePageMarqee")))
            Me.PublishBlock1.BlockID = IIf(ConfigurationManager.AppSettings("HomePageAnnounce") Is Nothing, "Announce", (ConfigurationManager.AppSettings("HomePageAnnounce")))
            Me.TopArea1.SiteMapGroupID = IIf(ConfigurationManager.AppSettings("TopMenuGroup") Is Nothing, "ChineseTop", (ConfigurationManager.AppSettings("TopMenuGroup")))

            Dim HomePageNews As String = ConfigurationManager.AppSettings("HomePageNews")
            If HomePageNews Is Nothing Then
                HomePageNews = "ChineseHomeNews"
            End If
            Dim HomePageMarqee As String = ConfigurationManager.AppSettings("HomePageMarqee")
            If HomePageMarqee Is Nothing Then
                HomePageMarqee = "ChineseHomeMarqee"
            End If
            Dim HomePageAnnounce As String = ConfigurationManager.AppSettings("HomePageAnnounce")
            If HomePageAnnounce Is Nothing Then
                HomePageAnnounce = "Announce"
            End If
            Dim TopMenuGroup As String = ConfigurationManager.AppSettings("TopMenuGroup")
            If TopMenuGroup Is Nothing Then
                TopMenuGroup = "ChineseTop"
            End If
            Me.HomePageBlock1.SiteMapGroupID = (HomePageNews)
            Me.HomeMarqeeBlock1.SiteMapGroupID = (HomePageMarqee)
            Me.PublishBlock1.BlockID = (HomePageAnnounce)
            Me.TopArea1.SiteMapGroupID = (TopMenuGroup)

        End If
    End Sub
End Class
