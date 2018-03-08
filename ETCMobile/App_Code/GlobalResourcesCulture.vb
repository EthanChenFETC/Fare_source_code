Imports Microsoft.VisualBasic

''' <summary>
''' 多語系字串服務
''' </summary>
''' <remarks>
''' 提供多語系設定檔字串取得的服務，請在Web.Config設定正確的文化區別(culture)
''' </remarks>
Public Module GlobalResourcesCulture

    ''' <summary>
    ''' 取得已設定好的多語系字詞
    ''' </summary>
    ''' <param name="ResourceKey">多語系關鍵字</param>
    ''' <returns>多語系字詞</returns>
    ''' <remarks></remarks>
    Public Function GetGlobalResourcesString(ByVal ResourceKey As String) As String
        Dim VerID As String = GetCurrentCulture()
        Dim GlobalResourceString As String = HttpContext.GetGlobalResourceObject(VerID, ResourceKey).ToString()
        Return GlobalResourceString
    End Function


    ''' <summary>
    ''' 自動由Web.Config取得正確的文化區別(culture)
    ''' </summary>
    ''' <returns>文化字串</returns>
    ''' <remarks></remarks>
    Public Function GetCurrentCulture() As String
        Dim VerID As String = System.Globalization.CultureInfo.CurrentCulture.ToString
        VerID = VerID.Replace("-", "")
        Return VerID
    End Function



    ''' <summary>
    ''' 自動取得本站的
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetCurrentCultureLanguage() As String
        Dim CultureInfo As String = ""
        Select Case GetCurrentCulture()
            Case "zhTW"
                CultureInfo = "中文"
            Case "enUS"
                CultureInfo = "英文"
            Case Else
                CultureInfo = "中文"
        End Select

        Return CultureInfo
    End Function

End Module
