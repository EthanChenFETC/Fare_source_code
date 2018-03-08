
Partial Class SystemError
    Inherits InjectionPage

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Session("Title") = "高速公路局中文版"
        If Not IsPostBack Then
            If Request.Params("aspxerrorpath") <> "" Then
                Try
                    Dim AllInfos As StringBuilder = New StringBuilder
                    AllInfos.Append(vbCrLf & "使用者IP：" & Request.ServerVariables("REMOTE_ADDR") & vbCrLf)
                    AllInfos.Append("使用語言：" & Request.ServerVariables("HTTP_ACCEPT_LANGUAGE") & vbCrLf)
                    AllInfos.Append("瀏覽器：" & Request.ServerVariables("HTTP_USER_AGENT") & vbCrLf)
                    AllInfos.Append("作業系統：" & Request.Browser.Platform & vbCrLf)
                    AllInfos.Append("先前頁面：" & Request.ServerVariables("HTTP_REFERER"))
                    AllInfos.Append(vbCrLf)

                    ModuleWriteLog.WriteLog(Request.Path & " 重新導向之錯誤頁面==>" & Request.Params("aspxerrorpath"), AllInfos.ToString, Me, False)
                Catch ex As Exception

                End Try
            End If
        End If
    End Sub
End Class
