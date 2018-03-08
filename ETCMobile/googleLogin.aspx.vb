Imports System.IO
Imports System.Net
Imports System.Data
Imports System.Data.SqlClient
Imports System.Runtime.Serialization
Imports System.Runtime.Serialization.Json
Imports System.Text
Partial Class googleLogin
    Inherits System.Web.UI.Page
    'Dim GoogleAppSecret As String = System.Configuration.ConfigurationManager.AppSettings("GoogleAppSecret").ToString
    'Dim GoogleAppID As String = System.Configuration.ConfigurationManager.AppSettings("GoogleAppID").ToString
    Dim SiteDomainName As String = System.Configuration.ConfigurationManager.AppSettings("SiteDomainName").ToString
#Region "json class"
    <DataContract()> _
    Private Class JsonData

        <DataMember(Name:="id")> _
        Public ID As String
        <DataMember(Name:="name")> _
        Public Name As String
        <DataMember(Name:="email")> _
        Public email As String
    End Class
    <DataContract()> _
    Private Class TokenData

        <DataMember(Name:="access_token")> _
        Public access_token As String
    End Class
#End Region
    Private Function JsonProcess(ByVal Content As String) As String
        Dim Err As String
        Try
            Dim stream As New MemoryStream(Encoding.UTF8.GetBytes(Content))
            ' json read
            Dim json As New DataContractJsonSerializer(GetType(JsonData))
            Dim objJson As JsonData = DirectCast(json.ReadObject(stream), JsonData)

            ' print the data
            Dim KeyID As String = objJson.ID
            Dim UserName As String
            Dim email As String = objJson.email
            Dim name As String = objJson.Name
            'Dim dr As SqlDataReader = ClassDB.GetDataReader("select MemberID,UserName from Member where KeyID ='" & KeyID & "' and Kind=3")
            'If dr.Read Then
            '    ClassDB.UpdateDB("Update Member set Conts=Conts+1 where MemberID=" & dr("MemberID") & "")
            '    Session("MemberID") = dr("MemberID").ToString
            '    Session("UserName") = dr("UserName").ToString
            'Else
            '    Dim ArrData() As String = email.Split("@")
            '    UserName = ArrData(0)
            '    '新增
            '    Dim MemberID As Integer = ClassDB.RunSPReturnInteger("Net2_F_Member_Add", _
            '             New SqlParameter("@UID", ""), _
            '             New SqlParameter("@KeyID", KeyID), _
            '             New SqlParameter("@UserName", UserName), _
            '             New SqlParameter("@email", email), _
            '             New SqlParameter("@Kind", 3))
            '    If MemberID = 0 Then
            '        Err = "Google登入系統目前發生錯誤!無法登入"
            '    Else
            '        Session("MemberID") = MemberID
            '        Session("UserName") = UserName
            '    End If
            'End If
            'dr.Close()
            lblEmail.Text = email
            lblName.Text = name
            Dim IPAddress As String
            Try
                IPAddress = Request.ServerVariables("REMOTE_ADDR")
            Catch ex As Exception
                WriteErrLog(ex, Me)
            End Try
            'RecordClass.MemberLog(Session("MemberID"), IPAddress)
        Catch ex As Exception
            WriteErrLog(ex, Me)
            Err = "Facebook登入系統目前發生錯誤!無法登入"
        End Try
        Return Err

    End Function
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim access_token As String = ""
        For i As Integer = 0 To Request.Params.AllKeys.Length - 1
            access_token &= Request.Params.AllKeys(i).ToString & "###" & Request.Params(i).ToString & "!!!"
        Next
        Label1.Text = access_token
        'access_token = HttpWebRequestClass.PostBodyData(Url, PostData)

        'Dim stream As New MemoryStream(Encoding.UTF8.GetBytes(access_token))
        '        Dim json As New DataContractJsonSerializer(GetType(TokenData))
        '        Dim objJson As TokenData = DirectCast(json.ReadObject(stream), TokenData)
        '        Dim Content As String = HttpWebRequestClass.BodyData("https://www.googleapis.com/oauth2/v1/userinfo?access_token=" & objJson.access_token)
        'Dim errors As String = JsonProcess(Content)

    End Sub

End Class
