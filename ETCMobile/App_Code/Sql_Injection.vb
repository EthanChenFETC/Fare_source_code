''' <summary>
''' 畓翴苯磞家舱
''' </summary>
''' <remarks></remarks>
Public Class Sql_Injection
    Inherits System.Web.UI.Page
    Dim Injection As New String(System.Configuration.ConfigurationSettings.AppSettings("Injection"))
    Dim ValidateURL As New String(RemoveSqlInjection(System.Configuration.ConfigurationSettings.AppSettings("ValidateURL")))

    ''' <summary>
    ''' 浪琩SQL Injection﹃
    ''' </summary>
    ''' <param name="webrequest">HTTP Request</param>
    ''' <param name="webresponse">HTTP Response</param>
    ''' <returns>琌Τ獶猭﹃</returns>
    ''' <remarks></remarks>
    Public Function Check_Sql_Injection(ByVal webrequest As HttpRequest, ByVal webresponse As HttpResponse, ByVal prePage As Page) As Boolean


        Dim sqlInjection As Boolean = False
        Dim inputInjection As Boolean = False
        Try
            'If Not IsNumeric(Session("SCAN")) Then
            '    Session("SCAN") = ConfigurationManager.AppSettings("SCAN")
            '    If Not IsNumeric(Session("SCAN")) Then
            '        Session("SCAN") = 0
            '    End If
            'End If
            'If Session("SCAN") > 0 Then
            '    Return False
            'End If
            Dim i As Integer
            Try
                If (webrequest.Params("cnid") IsNot Nothing And Not IsNumeric(webrequest.Params("cnid"))) Or (webrequest.Params("cnhb") IsNot Nothing And Not IsNumeric(webrequest.Params("cnhb"))) Or (webrequest.Params("p") IsNot Nothing And Not IsNumeric(webrequest.Params("p"))) Then
                    inputInjection = True
                    WriteLog("SQL INJECTION", "cnid or cnhb or p is not integer")
                End If
                webrequest.ValidateInput()
            Catch ex As HttpException
                inputInjection = True
                WriteLog("SQL INJECTION cnid or cnhb or p", ex.Message)
            End Try
            If Not sqlInjection And Not inputInjection Then
                For i = 0 To webrequest.Cookies.Keys.Count - 1
                    Dim ss As String = webrequest.Cookies(webrequest.Cookies.Keys(i)).Value
                    Dim dd As String = webrequest.Cookies.Keys(i)
                    If dd Is Nothing Or ss Is Nothing Then
                        Continue For
                    End If

                    If Not Sql_Injection(ss) Then
                        inputInjection = True
                        WriteLog("request.QueryString", "獶猭把计嘿:" & dd & "\r\n獶猭把计ず甧:" & HttpUtility.HtmlEncode(ss), Nothing, True)
                        Exit For
                    End If
                Next
                For i = 0 To webrequest.QueryString.Keys.Count - 1
                    Dim ss As String = webrequest(webrequest.QueryString.Keys(i))
                    Dim dd As String = webrequest.QueryString.Keys(i)
                    If dd Is Nothing Or ss Is Nothing Then
                        Continue For
                    End If
                    If Not Sql_Injection(ss) Then
                        inputInjection = True
                        WriteLog("request.QueryString", "獶猭把计嘿:" & dd & "\r\n獶猭把计ず甧:" & HttpUtility.HtmlEncode(ss), Nothing, True)
                        Exit For
                    End If
                Next
                'For i = 0 To webrequest.Headers.AllKeys.Length - 1
                '    If webrequest.Headers.AllKeys(i).ToLower.IndexOf("Acunetix".ToLower) >= 0 Then
                '        sqlInjection = True
                '        WriteLog("request.Header", "畓翴苯磞:Acunetix", Nothing, True)
                '        Exit For
                '    End If
                'Next
                If Not inputInjection Then
                    For i = 0 To webrequest.Params.Keys.Count - 1
                        Dim ss As String = webrequest.Params.Item(i)
                        Dim dd As String = webrequest.Params.Keys(i)
                        If dd Is Nothing Or ss Is Nothing Then
                            Continue For
                        End If
                        'If dd.IndexOf("routeLines") >= 0 Or dd.IndexOf("hdAxisX") >= 0 Or dd.IndexOf("hdAxisY") >= 0 Then
                        '    If Not Regex.IsMatch(ss.Replace(",", "").Replace("%2c", "").ToString.Trim, "^[0-9]+$") Then
                        '        inputInjection = True
                        '        WriteLog("request.QueryString", "獶猭把计嘿:" & dd & "\r\n獶猭把计ず甧:" & HttpUtility.HtmlEncode(ss), Nothing, True)
                        '        Exit For
                        '    End If
                        'End If
                        'If dd.IndexOf("routeLines") >= 0 Or dd.IndexOf("hdAxisX") >= 0 Or dd.IndexOf("hdAxisY") >= 0 Then
                        '    If Not Regex.IsMatch(ss.Replace(",", "").Replace("%2c", "").ToString.Trim, "^[0-9]+$") Then
                        '        inputInjection = True
                        '        WriteLog("request.QueryString", "獶猭把计嘿:" & dd & "\r\n獶猭把计ず甧:" & HttpUtility.HtmlEncode(ss), Nothing, True)
                        '        Exit For
                        '    End If
                        'End If
                        If dd.IndexOf("ALL_") >= 0 Or dd.IndexOf("HTTP_") >= 0 Or dd.IndexOf("Hidinit") >= 0 Then
                            ss = ss.Replace(";", "")
                        ElseIf dd.IndexOf("availableTagst") >= 0 Then
                            ss = ss.Replace("'", "")
                        End If
                        If Not Sql_Injection(ss) Then
                            If webrequest.Params.Keys(i) = "ctl00_CPHolder1_TreeView1_Properties" Then
                                If webrequest.Headers.Item("referer") IsNot Nothing Then
                                    If webrequest.Headers.Item("referer").IndexOf(Context.Request.ServerVariables("SERVER_NAME")) >= 0 Then
                                        'dd = dd
                                    Else
                                        WriteLog("request.Params", "獶猭把计嘿:" & dd & "\r\n獶猭把计ず甧:" & HttpUtility.HtmlEncode(ss), Nothing, True)
                                        inputInjection = True
                                        Exit For
                                    End If
                                End If
                            Else
                                WriteLog("request.Params", "獶猭把计嘿:" & HttpUtility.HtmlEncode(dd) & "\r\n獶猭把计ず甧:" & HttpUtility.HtmlEncode(ss), Nothing, True)
                                inputInjection = True
                                Exit For
                            End If
                            'inputInjection = True
                            'Exit For
                        End If
                    Next
                    If Not inputInjection Then
                        '20140910  
                        Dim headersHost As String = webrequest.Headers.Item("Host").ToLower
                        Dim ValidateHost As String = IIf(ValidateURL Is Nothing, "", ValidateURL.ToLower).Trim
                        If ValidateHost.Length = 0 Then
                            ValidateHost = ValidateURL.Trim
                        End If
                        Dim VHosts() As String = ValidateHost.Split(",")
                        Dim k As Integer = 0
                        For k = 0 To VHosts.Length - 1
                            If headersHost.IndexOf(VHosts(k).Trim) >= 0 Then
                                Exit For
                            End If
                        Next
                        If k = VHosts.Length Then
                            inputInjection = True
                        End If

                        Dim QString As String = webrequest.QueryString.ToString
                        Dim URL As String = webrequest.Url.ToString
                        Dim pathinfo As String = webrequest.PathInfo

                        Dim headers As String = webrequest.Headers.ToString
                        Dim AcceptLanguage As String = webrequest.Headers.Item("Accept-Language")
                        Dim headersHTTPHost As String = webrequest.Headers.Item("HTTP_HOST")
                        k = 0
                        Try

                            If headersHTTPHost IsNot Nothing And inputInjection = False Then
                                headersHost = headersHTTPHost.ToLower.Trim
                                For k = 0 To VHosts.Length - 1
                                    If headersHost.IndexOf(VHosts(k).Trim) >= 0 Then
                                        Exit For
                                    End If
                                Next
                                If k = VHosts.Length Then
                                    inputInjection = True
                                End If

                            End If
                            headersHTTPHost = webrequest.Headers.Item("HOST")
                        Catch ex As Exception
                            WriteLog("HTTP_HOST", ex.Message & vbCrLf & ex.StackTrace)
                        End Try
                        k = 0
                        If headersHTTPHost IsNot Nothing And inputInjection = False Then
                            headersHost = headersHTTPHost.ToLower.Trim
                            For k = 0 To VHosts.Length - 1
                                If headersHost.IndexOf(VHosts(k).Trim) >= 0 Then
                                    Exit For
                                End If
                            Next
                            If k = VHosts.Length Then
                                inputInjection = True
                            End If

                        End If
                        headersHTTPHost = webrequest.Headers.Item("X-Forwarded-Host")
                        k = 0
                        Try
                            If headersHTTPHost IsNot Nothing And inputInjection = False Then
                                headersHost = headersHTTPHost.ToLower.Trim
                                For k = 0 To VHosts.Length - 1
                                    If headersHost.IndexOf(VHosts(k).Trim) >= 0 Then
                                        Exit For
                                    End If
                                Next
                                If k = VHosts.Length Then
                                    inputInjection = True
                                End If

                            End If
                        Catch ex As Exception
                            WriteLog("X-Forwarded-Host", ex.Message & vbCrLf & ex.StackTrace)
                        End Try
                        Try
                            headersHTTPHost = webrequest.ServerVariables("SERVER_NAME")
                            k = 0
                            If headersHTTPHost IsNot Nothing And inputInjection = False Then
                                headersHost = headersHTTPHost.ToLower.Trim
                                For k = 0 To VHosts.Length - 1
                                    If headersHost.IndexOf(VHosts(k).Trim) >= 0 Then

                                        Exit For
                                    End If
                                Next
                                If k = VHosts.Length Then
                                    inputInjection = True
                                End If

                            End If
                        Catch ex As Exception
                            WriteLog("SERVER_NAME", ex.Message & vbCrLf & ex.StackTrace)
                        End Try
                        If pathinfo Is Nothing Then
                            inputInjection = True
                        ElseIf Not pathinfo.Equals("") Then
                            inputInjection = True
                        ElseIf Not pathinfo.Trim.Length = 0 Then
                            inputInjection = True
                        End If

                        If Not Sql_InjectionReferer(webrequest.Headers.Item("Referer")) Then
                            inputInjection = True
                            WriteLog("request.Referer", "獶猭把计嘿:referer\r\n獶猭把计ず甧:" & HttpUtility.UrlEncode(webrequest.Headers.Item("Referer")), Nothing, True)
                        End If
                        If Not Sql_Injection(webrequest.Headers.Item("Referer")) Then
                            inputInjection = True
                            WriteLog("request.Referer", "獶猭把计嘿:referer\r\n獶猭把计ず甧:" & HttpUtility.UrlEncode(webrequest.Headers.Item("Referer")), Nothing, True)
                        End If
                        If Not Sql_Injection(webrequest.Headers.Item("URI")) Then
                            inputInjection = True
                            WriteLog("request.Url", "獶猭把计嘿:URI\r\n獶猭把计ず甧:" & HttpUtility.UrlEncode(webrequest.Headers.Item("URI")), Nothing, True)
                        End If
                        If Not Sql_Injection(URL) Then
                            inputInjection = True
                            WriteLog(" webrequest.Url", "獶猭把计嘿:URL\r\n獶猭把计ず甧:" & HttpUtility.UrlEncode(URL), Nothing, True)
                        End If
                        If Not Sql_Injection(webrequest.ServerVariables("REMOTE_ADDR")) Then
                            inputInjection = True
                            WriteLog("request.REMOTE_ADDR", "獶猭把计嘿:REMOTE_ADDR\r\n獶猭把计ず甧:" & HttpUtility.UrlEncode(webrequest.Headers.Item("URI")), Nothing, True)
                        End If
                        If Regex.Match(webrequest.Headers.ToString, "\(*\)\s+\{").Success Or webrequest.Headers.ToString.IndexOf("waitfor") > 0 Or webrequest.Headers.ToString.IndexOf("delay") >= 0 Or webrequest.Headers.ToString.IndexOf("sleep ") >= 0 Or webrequest.Headers.ToString.IndexOf("sleep(") >= 0 Then
                            WriteLog("request.Headers", "獶猭把计嘿:Headers" & "\r\n獶猭把计ず甧:Headers" & System.Web.HttpUtility.HtmlEncode(webrequest.Headers.ToString), Nothing, True)
                            inputInjection = True
                        End If


                    End If
                End If
            End If
            If inputInjection Then
                GoError(webrequest, webresponse)
            End If
        Catch ex As Exception
            WriteLog("checksqlinject-error", writeError(ex, webrequest))
        End Try

        Return inputInjection
    End Function
    Private Function writeError(ByVal ex As Exception, ByVal webrequest As HttpRequest) As String
        Dim contents As String = ex.Message & vbCrLf & ex.StackTrace & vbCrLf
        'contents &= "Header:" & HttpUtility.UrlEncode(webrequest.Headers.ToString) & vbCrLf
        'contents &= "Parmeters:" & HttpUtility.UrlEncode(webrequest.Params.ToString) & vbCrLf
        'contents &= "QueryString:" & HttpUtility.UrlEncode(webrequest.QueryString.ToString) & vbCrLf
        'contents &= "Cookies:" & HttpUtility.UrlEncode(webrequest.Cookies.ToString) & vbCrLf
        Return contents
    End Function


    Public Sub GoError(ByVal webrequest As HttpRequest, ByVal webresponse As HttpResponse)
        Try

            webresponse.Status = "200 You had enterred invalid parameters, please do not do that again!!"
            webresponse.AddHeader("Location", "http://fare.fetc.net.tw/")
            webresponse.Cookies.Clear()
            'Dim Script As String
            'If webrequest.UrlReferrer Is Nothing Then
            '    Script = PathManager.ApplicationUrl & "Default.aspx"
            'Else
            '    Script = webrequest.UrlReferrer.ToString
            'End If
            webresponse.Write("<noscript><a href='" & PathManager.ApplicationUrl & "Default.aspx'>叫浪琩眤块戈琌Τぃ猭じ籔才腹</a></noscript>")
            If webrequest.RawUrl.ToLower.IndexOf("/default.aspx") >= 0 Then
                webresponse.Write("<script>alert('叫浪琩眤块戈琌Τぃ猭じ籔才腹叫穝币笆聅凝竟璝Τ好拜叫╰参恨瞶!!');document.location.href='" & PathManager.ApplicationUrl & "Default.aspx?cnid=23" & "'</script>")
            Else
                webresponse.Write("<script>alert('叫浪琩眤块戈琌Τぃ猭じ籔才腹');document.location.href='" & PathManager.ApplicationUrl & "Default.aspx" & "'</script>")
            End If
            'webresponse.End()
            HttpContext.Current.Response.Flush()
            HttpContext.Current.Response.SuppressContent = True
            HttpContext.Current.ApplicationInstance.CompleteRequest()
        Catch ex As Exception
            WriteLog("GoError", ex.Message & vbCrLf & ex.StackTrace)
        End Try

    End Sub
    ''' <summary>
    ''' 浪琩SQL Injection﹃
    ''' </summary>
    ''' <param name="Injection_chk">浪琩﹃</param>
    ''' <returns>琌Τ獶猭﹃</returns>
    ''' <remarks></remarks>
    Public Function Sql_Injection(ByVal Injection_chk As String) As Boolean
        Dim chk() As String = Injection.Split(",")
        Dim i As Integer
        Dim chk2 As String = "true"
        '浪琩﹃﹍篈urldecode Ωurldecode Ω 琌ぃ琌Τselect inject ﹃, ぐ或璶ㄢΩ㎡? Τ穦Μ砆ㄢΩurlencode ﹃
        For i = 0 To UBound(chk)
            If UCase(Injection_chk).Replace(UCase(chk(i)), "") <> UCase(Injection_chk) Or UCase(Server.UrlDecode(Injection_chk)).Replace(UCase(chk(i)), "") <> UCase(Server.UrlDecode(Injection_chk)) Or UCase(Server.UrlDecode(Server.UrlDecode(Injection_chk))).Replace(UCase(chk(i)), "") <> UCase(Server.UrlDecode(Server.UrlDecode(Injection_chk))) Then
                Return False
                chk2 = "false"
            End If
        Next
        If chk2 = "true" Then Return True
    End Function
    ''' <summary>
    ''' 浪琩SQL Injection﹃
    ''' </summary>
    ''' <param name="Injection_chk">浪琩﹃</param>
    ''' <returns>琌Τ獶猭﹃</returns>
    ''' <remarks></remarks>
    Public Function Sql_InjectionReferer(ByVal Injection_chk As String) As Boolean
        Dim chk() As String = Injection.Split(",")
        Dim i As Integer
        Dim chk2 As String = "true"
        '浪琩﹃﹍篈urldecode Ωurldecode Ω 琌ぃ琌Τselect inject ﹃, ぐ或璶ㄢΩ㎡? Τ穦Μ砆ㄢΩurlencode ﹃
        For i = 0 To UBound(chk)
            If UCase(Injection_chk).Replace(UCase(chk(i)), "") <> UCase(Injection_chk) Or UCase(Server.UrlDecode(Injection_chk)).Replace(UCase(chk(i)), "") <> UCase(Server.UrlDecode(Injection_chk)) Or UCase(Server.UrlDecode(Server.UrlDecode(Injection_chk))).Replace(UCase(chk(i)), "") <> UCase(Server.UrlDecode(Server.UrlDecode(Injection_chk))) Then
                If UCase(chk(i)).Equals("DATA:") And (UCase(Injection_chk).Trim.IndexOf("X_CLIENT_DATA:") >= 0 Or UCase(Injection_chk).Trim.IndexOf("X-CLIENT-DATA:") >= 0) Then
                Else
                    Return False
                    chk2 = "false"
                End If

            End If
            Dim ChkCode As String = chk(i)
            If ChkCode.IndexOf("%09") >= 0 Then
                ChkCode = ChkCode.Replace("%09", vbTab)
            End If
            If UCase(Injection_chk).Replace(UCase(ChkCode), "") <> UCase(Injection_chk) Then
                If UCase(chk(i)).Equals("DATA:") And (UCase(Injection_chk).Trim.IndexOf("X_CLIENT_DATA:") >= 0 Or UCase(Injection_chk).Trim.IndexOf("X-CLIENT-DATA:") >= 0) Then
                Else
                    Return False
                    chk2 = "false"
                End If
            End If
        Next
        If chk2 = "true" Then Return True
    End Function
#Region "浪琩俱计把计琌岿粇"
    ''' <summary>
    ''' 浪琩俱计把计琌猭
    ''' </summary>
    ''' <param name="webrequest">HTTP Request</param>
    ''' <param name="webresponse">HTTP Response</param>
    ''' <returns>琌Τ獶猭﹃</returns>
    ''' <remarks></remarks>
    Public Function Check_InputInteger(ByVal webrequest As HttpRequest, ByVal webresponse As HttpResponse, ByVal prePage As Page) As Boolean
        Dim i As Integer
        Dim inputInjection As Boolean = False
        Dim inputInteger As New String(System.Configuration.ConfigurationManager.AppSettings("inputInteger"))
        Dim chk() As String = inputInteger.Split(",")

        For i = 0 To webrequest.QueryString.Keys.Count - 1
            Dim KeyName As String = webrequest.QueryString.Keys(i)
            Dim KeyValue As String = webrequest.QueryString.Item(i)
            If KeyValue IsNot Nothing Then
                Try
                    Dim k As Integer
                    For k = 0 To UBound(chk)
                        If UCase(KeyName).Equals(UCase(chk(k))) Then
                            Dim tempI As Integer = CInt(KeyValue)
                        End If
                    Next
                Catch ex As Exception
                    inputInjection = True
                    Exit For
                End Try

            End If
        Next

        If inputInjection Then
            GoError(webrequest, webresponse)

        End If
        Return inputInjection
    End Function
#End Region

    '#Region "浪琩CSRF ю阑"
    '    ''' <summary>
    '    ''' 璸计竟糶め狠Cookie
    '    ''' </summary>
    '    ''' <remarks>30だ牧筁戳</remarks>
    '    Public Function GetCSRFCookie(ByVal webform As Page) As String
    '        Dim SessionID As String = webform.Session.SessionID
    '        Return AESUtil.encrypt(SessionID, "interweb25016388")
    '    End Function

    '    ''' <summary>
    '    ''' 璸计竟眔め狠Cookie
    '    ''' </summary>
    '    ''' <returns>ガ狶(True/False)</returns>
    '    ''' <remarks>耞30だ牧ず琌竒硑砐筁セ</remarks>
    '    Public Function CheckCSRFCookie(ByVal webform As Page, ByVal csrfstr As String) As Integer
    '        Dim AntiCSRF As String = webform.Request.Params("AntiCSRF")
    '        Dim SessionID As String = webform.Session.SessionID
    '        If AntiCSRF Is Nothing Then
    '            Return 0
    '        Else
    '            If AESUtil.encrypt(SessionID, "interweb25016388").Equals(AntiCSRF) Then
    '                Return 1
    '            Else
    '                Return -1
    '            End If
    '        End If
    '    End Function
    '#End Region
End Class

