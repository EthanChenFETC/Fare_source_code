Imports Microsoft.VisualBasic

''' <summary>
''' ��T�����|���ΪA��
''' </summary>
''' <remarks></remarks>
Public Module PathManager

    ''' <summary>
    ''' ���o�t�γ]�w�����W�Ǹ��|
    ''' </summary>
    ''' <returns>�����W�Ǹ��|(�S����ServerMap�èS�����ѹ�����|)</returns>
    ''' <remarks></remarks>
    Public Function GetUploadPath() As String
        Dim UploadPath As String = System.Configuration.ConfigurationManager.AppSettings("UploadPath").ToString.Replace("~/", "")

        Return UploadPath
    End Function


    ''' <summary>
    ''' �Ѹ��|�����o���ɦW
    ''' </summary>
    ''' <param name="paths">���|�r��</param>
    ''' <returns>�r��(�YPublication.aspx,Faq.aspx...�����r��A�N�r��e���Ҧ����|�L�o��)</returns>
    ''' <remarks></remarks>
    Public Function GetAppNameByPath(ByVal paths As String) As String
        If paths.Length = 0 Then
            Return Nothing
        Else
            Dim AppName As String = ""
            Dim i As Integer = 0
            i = paths.LastIndexOf("/")

            If i >= 0 Then
                AppName = paths.Substring(i + 1, paths.Length - i - 1)
            Else
                AppName = paths
            End If

            Return AppName
        End If
    End Function

    ''' <summary>
    ''' ���ɦW�������o���ɦW(�ɫ�)
    ''' </summary>
    ''' <param name="FileName">�ɮצW�٩θ��|�r��</param>
    ''' <returns>���ɦW�r��</returns>
    ''' <remarks></remarks>
    Public Function GetExtentionName(ByVal FileName As String) As String
        If FileName.Length = 0 Then
            Return ""
        Else
            Dim AppName As String = ""
            Dim i As Integer = 0
            i = FileName.LastIndexOf(".")

            If i >= 0 Then
                AppName = FileName.Substring(i + 1, FileName.Length - i - 1)
            Else
                AppName = FileName
            End If

            Return AppName
        End If
    End Function



    ''' <summary>
    ''' ���o�i�椤�����ε{�������}
    ''' </summary>
    ''' <returns>�Hhttp�}�Y��������}�A�p���S��Port�N�@�_��X)</returns>
    ''' <remarks></remarks>
    Public Function FullDomainUrl() As String
        Dim Context As HttpContext = HttpContext.Current
        Dim ThisDomain As String = ""
        Dim servername As String = ConfigurationManager.AppSettings("SiteDomainName").ToString '
        servername = RemoveXSS(servername)
        servername = servername.Replace(";", "").Replace(",", "")
        ThisDomain = servername '"http://" & servername
        If Context.Request.Url.ToString.ToLower.IndexOf("https://") >= 0 Then
            ThisDomain = ThisDomain.Replace("http://", "https://")
        End If

        Return ThisDomain
    End Function


    ''' <summary>
    ''' ���o�ثe���楻���ε{�������|(��~/)
    ''' </summary>
    ''' <returns>�{�������|</returns>
    ''' <remarks></remarks>
    Public Function ApplicationUrl() As String
        Dim appurl As String = HttpRuntime.AppDomainAppVirtualPath
        Dim Context As HttpContext = HttpContext.Current
        Dim _ApplicationPath As String = ""

        If Not Context.Cache("ApplicationUrl") Is Nothing Then
            _ApplicationPath = CStr(Context.Cache("ApplicationUrl"))
        Else

            ' Dim ApplicationPath As String = RemoveXSS(Context.Request.ApplicationPath)
            Dim ApplicationPath As String = HttpRuntime.AppDomainAppVirtualPath
            If Not ApplicationPath.EndsWith("/") Then ApplicationPath = ApplicationPath & "/"

            '��i�֨���
            Context.Cache.Insert("ApplicationUrl", ApplicationPath, Nothing, Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(60))
            _ApplicationPath = ApplicationPath
        End If

        Return _ApplicationPath
    End Function


    ''' <summary>
    ''' ���o�{�b�����}�ɦW�A���h�����ɦW
    ''' </summary>
    ''' <param name="Webform">Webform��檫��</param>
    ''' <returns>���}�ɦW�r��</returns>
    ''' <remarks></remarks>
    Public Function GetPathFileNameNoExten(ByVal Webform As System.Web.UI.Page) As String
        Dim OutPut As String = ""
        Dim PathFileName As String = RemoveXSS(GetPathFileName(Webform))
        Dim i As Integer = PathFileName.LastIndexOf(".")
        OutPut = PathFileName.Substring(0, i)
        Return OutPut
    End Function


    ''' <summary>
    ''' ���o�{�b�����}�ɦW�A�pDefault.aspx��Link.aspx
    ''' </summary>
    ''' <param name="Webform">Webform��檫��</param>
    ''' <returns>���}�ɦW�r��</returns>
    ''' <remarks></remarks>
    Public Function GetPathFileName(ByVal Webform As System.Web.UI.Page) As String
        Dim PathFileName As String = ""
        Dim tmpAppName As String = RemoveXSS(Webform.Request.Path)
        Dim i As Integer = tmpAppName.LastIndexOf("/")

        PathFileName = tmpAppName.Substring(i + 1, tmpAppName.Length - i - 1)

        Return PathFileName
    End Function


End Module
