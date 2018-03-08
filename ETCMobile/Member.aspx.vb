Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Net
Imports System.Threading
Partial Class Member
    'Inherits IsHttps
    Inherits System.Web.UI.Page



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load



    End Sub


    Protected Sub ImgGoogle_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnLog.Click



        Dim scope As String
        scope = "fullname,email"
        Dim scope2 As String
        scope2 = "guid,sid,titles,classes"
        Dim contents As String = ""
        contents &= "+'openid.ns=' + encodeURIComponent('http://specs.openid.net/auth/2.0')"
        contents &= "+'&openid.claimed_id=' + encodeURIComponent('http://specs.openid.net/auth/2.0/identifier_select')"
        contents &= "+'&openid.identity=' + encodeURIComponent('http://openid.matsu.edu.tw/')"
        'contents &= "+'&openid.return_to=' + encodeURIComponent('http://www.egame.kh.edu.tw/authOpenId')"
        'contents &= "+'&openid.realm=' + encodeURI('http://www.egame.kh.edu.tw/authOpenId')"
        contents &= "+'&openid.return_to=' + encodeURIComponent('http://tang-prizenomination.interweb.com.tw/googleLogin.aspx')"
        contents &= "+'&openid.realm=' + encodeURIComponent('http://tang-prizenomination.interweb.com.tw/googleLogin.aspx')"
        contents &= "+'&openid.assoc_handle=' + encodeURIComponent('{HMAC-SHA256}{5a0d7698}{dLrXWw==}')"
        contents &= "+'&openid.mode=' + encodeURIComponent('checkid_setup')"
        contents &= "+'&openid.ns.ext1=' + encodeURIComponent('http://openid.net/extensions/sreg/1.1')"
        contents &= "+'&openid.ext1.required=' + encodeURIComponent('" & scope & "')"
        contents &= "+'&openid.ns.ext2=' + encodeURIComponent('http://openid.net/srv/ax/1.0')"
        contents &= "+'&openid.ext2.mode=' + encodeURIComponent('fetch_request')"
        contents &= "+'&openid.ext2.type.guid=' + encodeURIComponent('http://axschema.edu.tw/person/guid')"
        contents &= "+'&openid.ext2.type.sid=' + encodeURIComponent('http://axschema.edu.tw/school/id')"
        contents &= "+'&openid.ext2.type.titles=' + encodeURIComponent('http://axschema.edu.tw/person/titles')"
        contents &= "+'&openid.ext2.type.classes=' + encodeURIComponent('http://axschema.matsu.edu.tw/school/classStr')"
        contents &= "+'&openid.ext2.required=' + encodeURIComponent('guid,sid,titles,classes')"




        Dim Url As String = "'http://openid.matsu.edu.tw/serve?'" & (contents)

        ScriptManager.RegisterClientScriptBlock(Me.Page, GetType(Page), "", "document.location.href=" & Url, True)


        'Response.Redirect(Url)
        'Response.Redirect("https://accounts.google.com/o/oauth2/auth?client_id=" & GoogleAppID & "&redirect_uri=" & Server.UrlEncode(SiteDomainName & "googlelogin.aspx") & "&scope=" & scope & "&response_type=code&state=/googlelogin")
    End Sub


End Class
