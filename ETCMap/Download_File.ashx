<%@ WebHandler Language="VB" Class="Download_File" %>

Imports System
Imports System.Web
Imports System.IO
Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.UI.Page
Public Class Download_File : Implements IHttpHandler
    Dim _UploadPath As String = PathManager.GetUploadPath().ToString
    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        Dim AttFileID As String = context.Request.QueryString.Item("id")
        If Not IsNumeric(AttFileID) Then
            Exit Sub
        End If
        Dim AttFiles As Integer = CInt(AttFileID)
        Dim script As String = ""
        If AttFiles > 0 Then
            Dim strFileName As String = ""
            Dim sqlString As String = "SELECT AttFileID,FileName,FilePath,FileTitle,FileSize,FileMime,IdentityCode, UpdateDateTime,DownloadCount FROM AttachFiles WHERE AttFileID = @AttFileID" ''" & AttFileID & "'" '增加排序功能20081029
            Using dr As SqlDataReader = ClassDB.GetDataReaderParam(sqlString, New SqlParameter("@AttFileID", AttFiles))
                Try
                    If dr IsNot Nothing Then
                        If dr.Read Then
                            Dim FilePath As String = dr("FilePath")
                            Dim FileNamet As String = dr("FileName")
                            If FilePath Is Nothing Or FileNamet Is Nothing Then
                                script = "alert('檔案找不到！\n請洽系統管理員!');"
                                context.Response.Write("<script>" & script & "history.back();</script>")
                                Exit Sub
                            End If
                            If FilePath.IndexOf("..") >= 0 Or FileNamet.IndexOf("..") >= 0 Then
                                script = "alert('檔案路徑錯誤！\n請洽系統管理員!');"
                                context.Response.Write("<script>" & script & "history.back();</script>")
                                Exit Sub
                            End If
                            FilePath = FilePath & FileNamet
                            strFileName = (FilePath)

                        End If
                    End If
                Catch ex As Exception
                Finally
                End Try
            End Using
            If script.Length > 0 Then
                Exit Sub
            End If
            'Dim fs As Stream

            'Dim strContentType As String
            Dim strPath As String = GetUploadPath(context)
            Dim DisplayFileName As String = (PathManager.GetAppNameByPath(strFileName))
            Dim FileName As String = strPath & strFileName
            FileName = (FileName)
            If doCheckFile(FileName) = False Then
                script = "alert('檔案找不到！\n請洽系統管理員!');"
                context.Response.Write("<script>" & script & "history.back();</script>")
            Else
                '更新下載次數
                ClassDB.UpdateDB("Net2_F_FileDownload_AddCount", New SqlParameter("@AttFileID", AttFiles))
                Try
                    Dim bytBytes(10) As Byte
                    Using fs As FileStream = File.OpenRead(FileName)
                        ReDim bytBytes(fs.Length - 1)
                        fs.Read(bytBytes, 0, fs.Length)
                    End Using
                    Dim bBytes(bytBytes.Length - 1) As Byte
                    bytBytes.CopyTo(bBytes, 0)
                    bytBytes = Nothing
                    context.Response.ClearHeaders()
                    context.Response.Clear()
                    context.Response.Expires = 0
                    context.Response.Buffer = True
                    'Response.AddHeader("Accept-Language", "zh-tw")
                    context.Response.AddHeader("Content-disposition", "attachment; filename=""" & DisplayFileName & """")
                    context.Response.ContentType = "application/octet-stream"
                    context.Response.BinaryWrite(bBytes)
                    context.Response.End()
                    context.ApplicationInstance.CompleteRequest()
                Catch ex As Exception
                Finally
                End Try
            End If
        End If
    End Sub

    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property
    ''' <summary>
    ''' 取得實體檔案是否存在
    ''' </summary>
    ''' <param name="AttFile">檔案實體位址字串</param>
    ''' <returns>布林值(是/否)</returns>
    ''' <remarks></remarks>
    Private Function doCheckFile(ByVal AttFile As String) As Boolean
        If Not FileIO.FileSystem.FileExists(AttFile) Then
            'If Not File.
            Return False
        Else
            Return True
        End If
    End Function
    ''' <summary>
    ''' 取得檔案下載的實體路徑
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetUploadPath(ByVal context As HttpContext) As String
        Dim UploadPath As String = _UploadPath

        If Not UploadPath.EndsWith("/") Then     '//<----這裡把路徑變成以/結尾
            UploadPath += "/"
        End If

        If Not UploadPath.Substring(1, 1) = ":" Then
            UploadPath = context.Server.MapPath(UploadPath)
        End If

        Return UploadPath
    End Function
End Class