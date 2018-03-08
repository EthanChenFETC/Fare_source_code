''' <summary>
''' 雜項模組服務
''' </summary>
''' <remarks></remarks>
Public Module ModuleMisc

    ''' <summary>
    ''' 提供去除HTML碼的字串轉換
    ''' </summary>
    ''' <param name="strHtml">HTML碼字串</param>
    ''' <param name="big5">是否為Big5中文文字</param>
    ''' <param name="WordQTY">是否指定最後輸出字數</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function htmlRemove(ByVal strHtml As String, Optional ByVal big5 As Boolean = True, Optional ByVal WordQTY As Integer = 0) As String
        Dim context As HttpContext = HttpContext.Current
        If strHtml Is Nothing Then
            Return ""
        End If
        Dim replaceHtml As String = strHtml
        If big5 = True Then
            replaceHtml = replaceHtml.Replace(" ", "")
        End If

        replaceHtml = RemoveVbCrLf(replaceHtml)
        replaceHtml = replaceHtml.Trim()
        replaceHtml = replaceHtml.Replace("<br>", vbCrLf)
        replaceHtml = replaceHtml.Replace("<br/>", vbCrLf).Replace("<BR/>", vbCrLf).Replace("<br />", vbCrLf).Replace("<BR />", vbCrLf).Replace("<br>", vbCrLf).Replace("<BR>", vbCrLf)
        replaceHtml = replaceHtml.Replace("<p>", vbNewLine).Replace("<P>", vbNewLine)
        'replaceHtml = replaceHtml.Replace("&nbsp;", vbNullString)
        replaceHtml = replaceHtml.Replace("&nbsp;", " ")
        replaceHtml = replaceHtml.Replace("&rdquo;", context.Server.HtmlDecode("&rdquo;"))
        replaceHtml = replaceHtml.Replace("&ldquo;", context.Server.HtmlDecode("&ldquo;"))
        replaceHtml = replaceHtml.Replace("""", "")
        replaceHtml = replaceHtml.Replace("#", "")
        'replaceHtml = replaceHtml.Replace("/", "")
        'replaceHtml = replaceHtml.Replace("=", "")


        Dim find1 As Integer = 0
        Dim find2 As Integer = 0
        Dim number As Integer = 0
        Dim Limit As Integer = WordQTY
        Dim EndIndex As Integer = 0
        If WordQTY = 0 Then
            Limit = replaceHtml.Length
        ElseIf replaceHtml.Length > WordQTY Then
            Limit = WordQTY
        Else
            Limit = replaceHtml.Length
        End If
        Dim Sindex As Integer = 0
        Dim Eindex As Integer = 0
        Dim tmpStr As String = ""
        Do While tmpStr.Length < Limit
            find1 = replaceHtml.IndexOf("<")
            If find1 < 0 Then
                Exit Do
            End If
            tmpStr &= replaceHtml.Substring(0, find1)
            If find1 + 1 >= replaceHtml.Length Then
                tmpStr &= replaceHtml.Substring(find1, 1)
                Exit Do
            End If
            replaceHtml = replaceHtml.Substring(find1 + 1)
            find1 = replaceHtml.IndexOf("<")
            find2 = replaceHtml.IndexOf(">")
            If find2 < 0 Then
                tmpStr &= replaceHtml
                Exit Do
            End If
            replaceHtml = replaceHtml.Substring(find2 + 1)
        Loop
        Return tmpStr
    End Function


    ''' <summary>
    ''' 去除字串中的斷行標記
    ''' </summary>
    ''' <param name="s"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function RemoveVbCrLf(ByVal s As String) As String
        Dim i As Integer = 1

        Do While i > 0
            i = InStr(s, vbCrLf, CompareMethod.Text)
            s = s.Replace(vbCrLf, "")
        Loop

        i = 1
        Do While i > 0
            i = InStr(s, vbLf, CompareMethod.Text)
            s = s.Replace(vbLf, "")
        Loop

        s = s.Replace("   ", "")

        Return s
    End Function

    ' Decode an HTML string to a regular ANSI string
    ' it strips down all special HTML sequences (eg "&lt;")
    ' however, it doesn't strip HTML tags

    Function HTMLDecode(ByVal html As String) As String
        'Dim context As HttpContext = HttpContext.Current
        Dim i As Integer

        HTMLDecode = html

        Do
            ' search the next ampersand, exit if no more
            i = InStr(i + 1, HTMLDecode, "&")
            If i = 0 Then Exit Do

            If StrComp(Mid$(HTMLDecode, i, 6), "&nbsp;", vbTextCompare) = 0 Then
                HTMLDecode = Left$(HTMLDecode, i - 1) & " " & Mid$(HTMLDecode, i + 6)
            ElseIf StrComp(Mid$(HTMLDecode, i, 6), "&quot;", vbTextCompare) = 0 Then
                HTMLDecode = Left$(HTMLDecode, i - 1) & """" & Mid$(HTMLDecode, _
                 i + 6)
            ElseIf StrComp(Mid$(HTMLDecode, i, 5), "&amp;", vbTextCompare) = 0 Then
                HTMLDecode = Left$(HTMLDecode, i - 1) & "&" & Mid$(HTMLDecode, _
                 i + 5)
            ElseIf StrComp(Mid$(HTMLDecode, i, 4), "&lt;", vbTextCompare) = 0 Then
                HTMLDecode = Left$(HTMLDecode, i - 1) & "<" & Mid$(HTMLDecode, _
                 i + 4)
            ElseIf StrComp(Mid$(HTMLDecode, i, 4), "&gt;", vbTextCompare) = 0 Then
                HTMLDecode = Left$(HTMLDecode, i - 1) & ">" & Mid$(HTMLDecode, _
                 i + 4)
            End If
        Loop
    End Function

    ''' <summary>
    ''' 移除單雙引號
    ''' </summary>
    ''' <param name="p"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function RemoveSqlInjection(ByVal p As String) As String
        Return p
        Try
            p = p.Replace("'", "")
            p = p.Replace("""", "")
            p = p.Replace(";", "")
            p = p.Replace(" ", "")
            p = p.Replace(vbCrLf, "")
            p = p.Replace(vbCr, "")
            p = p.Replace(vbLf, "")


        Catch ex As Exception

        End Try


        Return p
    End Function

    ''' <summary>
    ''' 移除單雙引號
    ''' </summary>
    ''' <param name="p"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function RemoveXSS(ByVal p As String) As String
        Return p
        Try
            p = p.Replace("<", "")
            p = p.Replace(">", "")
            p = p.Replace("""", "")
            p = p.Replace("'", "")
            p = p.Replace("&", "")
            p = p.Replace(vbCrLf, "")
            p = p.Replace(vbCr, "")
            p = p.Replace(vbLf, "")
            p = p.Replace("..", "")
        Catch ex As Exception

        End Try


        Return p
    End Function
    Public Function RemoveNope(ByVal p As String) As String

        Return p
    End Function
    Public Function CheckSqlInjectionWording(ByVal p As String) As Boolean
        Dim IsFind As Boolean = False
        Dim s As String = "abcfefghijklmnopqrstuvwxyz0123456789,"
        Dim i As Integer
        For i = 0 To p.Length - 1
            Dim o As String = p.Substring(i, 1).ToLower
            If s.IndexOf(o) = -1 Then
                IsFind = True
            End If
        Next
        Return IsFind
    End Function
    Public Function CheckInteger(ByVal p As String) As Boolean
        Dim IsFind As Boolean = False
        Dim s As String = "0123456789,_-"
        Dim t As String = p
        Dim i As Integer
        For i = 0 To s.Length - 1
            Dim o As String = s.Substring(i, 1).ToLower
            t.Replace(o, "")
        Next
        If t.Length > 0 Then
            Return False
        End If
        Return True
    End Function
    Public Sub AlertMsg(ByVal Webform As System.Web.UI.Page, ByVal Message As String)

        Dim script As String = "alert('" & Message & "');"
        Webform.ClientScript.RegisterStartupScript(Webform.Page.GetType, "waring", script, True)
    End Sub
    ''' <summary>
    ''' 轉換西元年至民國年
    ''' </summary>
    ''' <param name="d">西元年</param>
    ''' <returns>民國年</returns>
    ''' <remarks></remarks>
    Function GetChineseDate(ByVal d As Date) As Date
        d = d.AddYears(-1911)
        Return d
    End Function

    ''' <summary>
    ''' 西元年月日字串轉換為民國年月日
    ''' </summary>
    ''' <param name="d">西元年月日</param>
    ''' <returns>民國年月日</returns>
    ''' <remarks></remarks>
    Function GetChineseDotDate(ByVal d As Date) As String
        Dim dString As String = d.AddYears(-1911).ToString("yyy.MM.dd", System.Globalization.DateTimeFormatInfo.InvariantInfo)
        Return dString
    End Function

    ''' <summary>
    ''' 截斷過長字串, 以...代替
    ''' </summary>
    ''' <param name="oldword">原始字串</param>
    ''' <returns>截斷後字串</returns>
    ''' <remarks></remarks>
    Function LimitWord(ByVal oldWord As String, ByVal Limit As Integer) As String
        Dim n As String = oldWord
        If n.Length > Limit Then
            n = n.Substring(0, Limit) & "..."
        End If

        Return n
    End Function
    ''' <summary>
    ''' 截斷過長字串, 以...代替
    ''' </summary>
    ''' <param name="oldword">原始字串</param>
    ''' <returns>截斷後字串</returns>
    ''' <remarks></remarks>
    Function RandomString() As UInt16
        Dim s As UInt16
        Dim randomByte(4) As Byte
        Dim rng As System.Security.Cryptography.RNGCryptoServiceProvider = New System.Security.Cryptography.RNGCryptoServiceProvider
        rng.GetBytes(randomByte)
        s = BitConverter.ToUInt16(randomByte, 0)
        Return s
    End Function
    Function RandomInteger(ByVal min As Integer, ByVal max As Integer) As Integer
        Dim s As Integer

        Dim rng As System.Security.Cryptography.RNGCryptoServiceProvider = New System.Security.Cryptography.RNGCryptoServiceProvider

        Dim scale As UInt32 = UInt32.MaxValue
        While scale = UInt32.MaxValue
            Dim randomByte(4) As Byte
            rng.GetBytes(randomByte)
            scale = BitConverter.ToUInt32(randomByte, 0)
        End While
        s = (min + (max - min) * (scale / Convert.ToDouble(UInt32.MaxValue)))
        Return s
    End Function
End Module
