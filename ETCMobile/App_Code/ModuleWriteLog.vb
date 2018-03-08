Imports Microsoft.VisualBasic
Imports System.IO
Imports System.Text
Imports System.Data
Imports System.Data.SqlClient

Public Module ModuleWriteLog

    '通用日誌記錄
    Public Sub WriteLog(ByVal whatPage As String, ByVal logtxt As String, Optional ByVal Webform As Page = Nothing, Optional ByVal SendtoMail As Boolean = True)
        Dim PostDate As String = Now.ToString("yyyy/MM/dd HH:mm:ss", System.Globalization.DateTimeFormatInfo.InvariantInfo)
        Dim PostDateS As String = Now.ToString("yyyy-MM-dd", System.Globalization.DateTimeFormatInfo.InvariantInfo)
        Dim logfile As String = ""
        Dim ErrorLogsPath As String = ConfigurationManager.AppSettings("ERRORPATH").ToString
        Try
            ErrorLogsPath = (ErrorLogsPath)
            If Not Directory.Exists(ErrorLogsPath) Then
                Directory.CreateDirectory(ErrorLogsPath)
            End If
        Catch ex As Exception
            Exit Sub
        End Try
        '//如不設路徑則不寫Log
        If ErrorLogsPath.Length > 0 Then
            ErrorLogsPath = ErrorLogsPath & "ErrorLog" & PostDateS & ".logfile"
            Try
                Using sw As StreamWriter = New StreamWriter(ErrorLogsPath, True)
                    Try
                        sw.WriteLine(PostDate & "==>" & whatPage & "==>" & logtxt & vbCrLf)
                        sw.Flush()
                        sw.Close()
                    Catch ex As Exception
                    End Try
                End Using
            Catch ex As Exception
            End Try
        End If
        '是否寄件通知
        If SendtoMail = False Then
            Exit Sub
        Else
            '  MailSender.SendMail("ErrReport-" & whatPage, logtxt, "chris@interweb.com.tw")
        End If
    End Sub

    '通用日誌記錄
    Public Sub wl(ByVal whatPage As String, ByVal logtxt As String, Optional ByVal Webform As Page = Nothing, Optional ByVal SendtoMail As Boolean = True)
        Dim PostDate As String = Now.ToString("yyyy/MM/dd HH:mm:ss", System.Globalization.DateTimeFormatInfo.InvariantInfo)
        Dim PostDateS As String = Now.ToString("yyyy-MM-dd", System.Globalization.DateTimeFormatInfo.InvariantInfo)
        Dim logfile As String = ""
        Dim ErrorLogsPath As String = ""
        Dim AppPath As String = ""
        Dim HttpContext As HttpContext = Web.HttpContext.Current
        AppPath = HttpContext.Request.ApplicationPath
        If AppPath.EndsWith("/") Then
            AppPath = AppPath & "Errorlogs/"
        Else
            AppPath = AppPath & "/Errorlogs/"
        End If
        'End If
        ErrorLogsPath = HttpContext.Server.MapPath(AppPath)
        Dim LogTexts As String = PostDate & "==>" & whatPage & "==>" & logtxt & vbCrLf
        If Not Webform Is Nothing Then
            LogTexts = "Time=       " & PostDate & vbCrLf
            LogTexts += "Page=       " & Webform.Request.Path & vbCrLf
            LogTexts += "Logs=       " & logtxt & vbCrLf
        End If
        '//如不設路徑則不寫Log
        If ErrorLogsPath.Length > 0 Then
            ErrorLogsPath = ErrorLogsPath & "ErrorLog" & PostDateS & ".logfile"
            Try
                Using sw As StreamWriter = New StreamWriter(ErrorLogsPath, True)
                    Try
                        sw.WriteLine(LogTexts)
                        sw.Flush()
                    Catch ex As Exception
                    End Try
                End Using
            Catch ex As Exception
            End Try
        End If
    End Sub

    ''後台的使用記錄
    'Public Sub WriteAdminLog(ByVal Webform As System.Web.UI.Page, ByVal Actions As String, Optional ByVal ActionsResult As Boolean = True, Optional ByVal SiteMenu As String = "")
    '    Dim UserIP As String = Webform.Request.ServerVariables("REMOTE_ADDR")
    '    'Dim Actions As String = "Edit"
    '    Dim AdminMenu As String = ModulePermissions.GetAdminMenuName(Webform)
    '    Dim UserName As String = Webform.Session("Name") & ""
    '    Dim DepartmentName As String = ""
    '    If Not Webform.Session("DepartmentName") Is Nothing Then DepartmentName = Webform.Session("DepartmentName")
    '    If UserName = "" Then
    '        If DepartmentName <> "" Then
    '            UserName = DepartmentName
    '        Else
    '            UserName = "未登入"
    '        End If
    '    End If

    '    
    '    ClassDB.UpdateDB("AdminLog_Add", _
    '      New SqlParameter("@Actions", Actions), _
    '      New SqlParameter("@ActionsResult", IIf(ActionsResult = True, "1", "0")), _
    '      New SqlParameter("@AdminMenu", AdminMenu), _
    '      New SqlParameter("@SiteMenu", SiteMenu), _
    '      New SqlParameter("@UserIP", UserIP), _
    '      New SqlParameter("@UpdateUser", UserName))

    '    
    'End Sub

    '錯誤捕捉專用日誌
    Public Sub WriteErrLog(ByVal ex As Exception, Optional ByVal Webform As Page = Nothing, Optional ByVal SendtoMail As Boolean = True)
        Dim PostDate As String = Now.ToString("yyyy/MM/dd HH:mm:ss", System.Globalization.DateTimeFormatInfo.InvariantInfo)
        Dim PostDateS As String = Now.ToString("yyyy-MM-dd", System.Globalization.DateTimeFormatInfo.InvariantInfo)
        Dim logfile As String = ""
        'Dim ErrorLogsPath As String = ModulePathManager.GetErrorLogsPath()
        Dim ErrorLogsPath As String = ConfigurationManager.AppSettings("ERRORPATH").ToString
        ErrorLogsPath = (ErrorLogsPath)
        If Not Directory.Exists(ErrorLogsPath) Then
            Directory.CreateDirectory(ErrorLogsPath)
        End If

        '//如不設路徑則不寫Log
        Dim errMessage As String = ""
        If ErrorLogsPath.Length > 0 Then
            ErrorLogsPath = ErrorLogsPath & "ErrorLog" & PostDateS & ".logfile"
            'Dim sw As StreamWriter = New StreamWriter(ErrorLogsPath, True)
            errMessage = PostDate & vbCrLf
            errMessage += "==>" & Webform.Request.ServerVariables("SERVER_NAME") & Webform.Request.Path & vbCrLf
            errMessage += "ex.UserIP->" & Webform.Request.ServerVariables("REMOTE_ADDR") & vbCrLf
            errMessage += "ex.Message->" & ex.Message & vbCrLf
            errMessage += "ex.Source->" & ex.Source & vbCrLf
            errMessage += "ex.StackTrace->" & ex.StackTrace & vbCrLf
            Try
                Using sw As StreamWriter = New StreamWriter(ErrorLogsPath, True)
                    sw.WriteLine(errMessage)
                    sw.Flush()
                End Using
            Catch exx As Exception
            End Try
            'Try
            '    sw.WriteLine(errMessage)
            '    sw.Flush()
            '    sw.Close()
            'Catch
            'End Try
        End If

        '是否寄件通知
        'If SendtoMail = False Then
        '    Exit Sub
        'Else
        'MailSender.SendMail("ErrReport-" & ex.Message, errMessage, "chris@interweb.com.tw")
        'End If
    End Sub

End Module