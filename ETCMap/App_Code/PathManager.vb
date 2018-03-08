Imports Microsoft.VisualBasic

''' <summary>
''' 資訊網路徑應用服務
''' </summary>
''' <remarks></remarks>
Public Module PathManager

    ''' <summary>
    ''' 取得系統設定中的上傳路徑
    ''' </summary>
    ''' <returns>虛擬上傳路徑(沒有做ServerMap並沒有提供實體路徑)</returns>
    ''' <remarks></remarks>
    Public Function GetUploadPath() As String
        Dim UploadPath As String = System.Configuration.ConfigurationManager.AppSettings("UploadPath").ToString.Replace("~/", "")

        Return UploadPath
    End Function


    ''' <summary>
    ''' 由路徑中取得其檔名
    ''' </summary>
    ''' <param name="paths">路徑字串</param>
    ''' <returns>字串(即Publication.aspx,Faq.aspx...等等字串，將字串前面所有路徑過濾掉)</returns>
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
    ''' 由檔名中中取得附檔名(檔型)
    ''' </summary>
    ''' <param name="FileName">檔案名稱或路徑字串</param>
    ''' <returns>附檔名字串</returns>
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
    ''' 取得進行中的應用程式之網址
    ''' </summary>
    ''' <returns>以http開頭之完整網址，如有特殊的Port將一起輸出)</returns>
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
    ''' 取得目前執行本應用程式之路徑(例~/)
    ''' </summary>
    ''' <returns>程式之路徑</returns>
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

            '放進快取中
            Context.Cache.Insert("ApplicationUrl", ApplicationPath, Nothing, Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(60))
            _ApplicationPath = ApplicationPath
        End If

        Return _ApplicationPath
    End Function


    ''' <summary>
    ''' 取得現在的網址檔名，但去除副檔名
    ''' </summary>
    ''' <param name="Webform">Webform表單物件</param>
    ''' <returns>網址檔名字串</returns>
    ''' <remarks></remarks>
    Public Function GetPathFileNameNoExten(ByVal Webform As System.Web.UI.Page) As String
        Dim OutPut As String = ""
        Dim PathFileName As String = RemoveXSS(GetPathFileName(Webform))
        Dim i As Integer = PathFileName.LastIndexOf(".")
        OutPut = PathFileName.Substring(0, i)
        Return OutPut
    End Function


    ''' <summary>
    ''' 取得現在的網址檔名，如Default.aspx或Link.aspx
    ''' </summary>
    ''' <param name="Webform">Webform表單物件</param>
    ''' <returns>網址檔名字串</returns>
    ''' <remarks></remarks>
    Public Function GetPathFileName(ByVal Webform As System.Web.UI.Page) As String
        Dim PathFileName As String = ""
        Dim tmpAppName As String = RemoveXSS(Webform.Request.Path)
        Dim i As Integer = tmpAppName.LastIndexOf("/")

        PathFileName = tmpAppName.Substring(i + 1, tmpAppName.Length - i - 1)

        Return PathFileName
    End Function


End Module
