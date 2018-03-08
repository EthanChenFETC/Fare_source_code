Imports System
Imports System.Net
Imports System.Net.Mail
Imports Microsoft.VisualBasic



Public Module MailSender

    Private mMailServer As String = System.Configuration.ConfigurationManager.AppSettings("MailServer").ToString
    Private mPort As String = System.Configuration.ConfigurationManager.AppSettings("MailServerPort").ToString
    Private mMailFrom As String = System.Configuration.ConfigurationManager.AppSettings("MailFrom").ToString
    Private mMailFromDisplayName As String = System.Configuration.ConfigurationManager.AppSettings("MailFromDisplayName").ToString

    Public Sub SendMail(
    ByVal MailSubject As String,
    ByVal MailBody As String,
    ByVal MailTo As String,
    Optional ByVal MailFrom As String = "",
    Optional ByVal MailCC As String = "",
    Optional ByVal MailBCC As String = "",
    Optional ByVal IsBodyHtml As Boolean = False)

        'No Send Mail when mail server is Not Ready
        If mMailServer Is Nothing OrElse mMailServer = "" OrElse mMailServer.Length = 0 Then Exit Sub



        Try

            Dim Mailmsg As New System.Net.Mail.MailMessage
            Mailmsg.IsBodyHtml = IsBodyHtml '為html內容格式
            Mailmsg.Subject = MailSubject
            Mailmsg.Body = MailBody

            If MailFrom = "" Then
                Mailmsg.From = New Net.Mail.MailAddress(mMailFrom, mMailFromDisplayName)
            Else
                Mailmsg.From = New Net.Mail.MailAddress(MailFrom, mMailFromDisplayName)
            End If
            If MailTo.IndexOf(";") = -1 Then
                Mailmsg.To.Add(MailTo)
            Else
                Dim strCC() As String = Split(MailTo, ";")

                For i As Integer = 0 To strCC.Count - 1
                    Dim strThisCC As String = strCC(i)
                    Mailmsg.To.Add(Trim(strThisCC))
                Next
            End If


            If MailCC <> "" Then
                Dim strCC() As String = Split(MailCC, ";")

                For i As Integer = 0 To strCC.Count - 1
                    Dim strThisCC As String = strCC(i)
                    Mailmsg.CC.Add(Trim(strThisCC))
                Next
            End If

            If MailBCC <> "" Then
                Dim strCC() As String = Split(MailBCC, ";")
                For i As Integer = 0 To strCC.Count - 1
                    Dim strThisCC As String = strCC(i)
                    Mailmsg.Bcc.Add(Trim(strThisCC))
                Next
            End If

            'If fileAttachments.HasFile Then
            'Dim attached As New 
            '    Attachment(Trim(fileAttachments.PostedFile.FileName.ToString()))
            '    message.Attachments.Add(attached)
            'End If

            Dim SmtpClient As New SmtpClient(mMailServer, mPort)
            SmtpClient.UseDefaultCredentials = False
            SmtpClient.Send(Mailmsg)

            Mailmsg = Nothing
            SmtpClient = Nothing

        Catch ex As FormatException
            WriteLog("MailSender-FormatException", ex.Message + vbCrLf + "SendMailError:" + "MailTo:" + MailTo + vbCrLf + "MailSubject:" + MailSubject)
        Catch ex As SmtpException
            WriteLog("MailSender-SMTP Exception", ex.Message + vbCrLf + "SendMailError:" + "MailTo:" + MailTo + vbCrLf + "MailSubject:" + MailSubject)
        Catch ex As Exception
            WriteLog("MailSender-General Exception", ex.Message + vbCrLf + "SendMailError:" + "MailTo:" + MailTo + vbCrLf + "MailSubject:" + MailSubject)
        End Try
    End Sub

End Module
