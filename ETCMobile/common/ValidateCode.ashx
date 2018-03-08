<%@ WebHandler Language="VB" Class="ValidateCode" %>

Imports System
Imports System.Web
Imports System.Drawing
Imports System.Web.SessionState

Public Class ValidateCode
    Implements IHttpHandler
    Implements IRequiresSessionState

    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        CreateCheckCodeImage(GenerateCheckCode(context), context)
    End Sub

    Private Function GenerateCheckCode(ByVal context As HttpContext) As String
        Dim number As Integer
        Dim code As String = ""
        Dim xx As String = ""
        Dim checkCode As String = [String].Empty
        'Dim seed As UInteger = RandomString()
        'Dim random As System.Random = New Random(seed)
        'For i As Integer = 0 To 5
        '    number = random.Next
        '    'If number Mod 2 = 0 Then
        '    '    code = (number Mod 10).ToString
        '    'Else
        '    '    code = ChrW((number Mod 26) + Asc("A")).ToString
        '    'End If
        '    code = (number Mod 10).ToString
        '    'checkCode += code
        'Next
        Dim randoms As System.Security.Cryptography.RandomNumberGenerator = System.Security.Cryptography.RandomNumberGenerator.Create()
        For i As Integer = 0 To 5
            Dim bytes(4) As Byte
            randoms.GetBytes(bytes)
            number = BitConverter.ToUInt16(bytes, 0)

            'If number Mod 2 = 0 Then
            '    code = (number Mod 10).ToString
            'Else
            '    code = ChrW((number Mod 26) + Asc("A")).ToString
            'End If
            code = (number Mod 10).ToString
            checkCode += code
        Next
        context.Session("Number") = checkCode

        '儲存在session 
        context.Session("CheckCode") = checkCode

        Return checkCode
    End Function

    Private Sub CreateCheckCodeImage(ByVal checkCode As String, ByVal context As HttpContext)
        If checkCode Is Nothing Then
            Return
        End If
        If checkCode.Trim = String.Empty Then
            Return
        End If
        Dim iWidth As Integer = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(checkCode.Length * 18)))
        '設定圖片寬高
        Using image As System.Drawing.Bitmap = New System.Drawing.Bitmap(iWidth, 30)
            Using g As Graphics = Graphics.FromImage(image)
                '隨机 
                Dim bytes(3) As Byte
                Dim Number As UInteger
                Dim random As System.Security.Cryptography.RandomNumberGenerator = System.Security.Cryptography.RandomNumberGenerator.Create()
                '清空圖片背景色 
                g.Clear(Color.White)
                For i As Integer = 0 To 24
                    random.GetBytes(bytes)
                    Number = BitConverter.ToUInt16(bytes, 0)
                    '圖片的背景噪訊 
                    Dim x1 As Integer = Number
                    random.GetBytes(bytes)
                    Number = BitConverter.ToUInt16(bytes, 0)
                    Dim x2 As Integer = Number
                    random.GetBytes(bytes)
                    Number = BitConverter.ToUInt16(bytes, 0)
                    Dim y1 As Integer = Number
                    random.GetBytes(bytes)
                    Number = BitConverter.ToUInt16(bytes, 0)
                    Dim y2 As Integer = Number

                    g.DrawLine(Pens.Silver, x1, y1, x2, y2)
                Next
                '字型大小及顏色
                Dim rect As System.Drawing.Rectangle = New System.Drawing.Rectangle(0, 0, image.Width, image.Height)
                Using font As Font = New System.Drawing.Font("Arial", 16, (System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic))
                    Using brush As System.Drawing.Drawing2D.LinearGradientBrush = New System.Drawing.Drawing2D.LinearGradientBrush(rect, Color.Black, Color.DarkBlue, 1.2F, True)
                        g.DrawString(checkCode, font, brush, 2, 2)
                        For i As Integer = 0 To 99
                            '圖片的前景雜訊 
                            Number = RandomInteger(0, image.Width - 1)
                            Dim x As Integer = Number
                            Number = RandomInteger(0, image.Height - 1)
                            Dim y As Integer = Number
                            random.GetBytes(bytes)
                            Number = BitConverter.ToUInt16(bytes, 0)
                            image.SetPixel(x, y, Color.FromArgb(Number))
                        Next
                        '圖片的框 
                        g.DrawRectangle(Pens.Silver, 0, 0, image.Width - 1, image.Height - 1)
                        Using ms As New System.IO.MemoryStream()
                            image.Save(ms, System.Drawing.Imaging.ImageFormat.Gif)
                            context.Response.ClearContent()
                            context.Response.ContentType = "image/Gif"
                            context.Response.BinaryWrite(ms.ToArray())
                        End Using
                    End Using
                End Using
            End Using
        End Using
    End Sub

    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class