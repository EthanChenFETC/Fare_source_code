Imports System.Text
Imports System.Data
Imports System.Data.SqlClient

''' <summary>
''' 網站地圖(網頁表單)
''' </summary>
''' <remarks></remarks>
Partial Class WebSiteMap
    Inherits System.Web.UI.Page

    ''' <summary>
    ''' 頁面讀取事件
    ''' </summary>
    ''' <param name="sender">觸發本事件控制項</param>
    ''' <param name="e">觸發本事件帶進之參數</param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then

            'Me.ltl_Sitemap2.Text = BuildMenu2()
        End If
        Me.ltl_Sitemap1.Text = BuildMenu()
    End Sub

    ''' <summary>
    ''' 建立樹狀階層的HTML碼，呈現本網站地圖
    ''' </summary>
    ''' <returns>HTML碼</returns>
    ''' <remarks></remarks>
    Public Function BuildMenu() As String
        Dim ds As DataSet
        ds = Cache("SiteMap")
        If ds Is Nothing Then ds = Sitemap.GetSiteMapdsActive()

        Dim sb As StringBuilder = New StringBuilder

        sb.Append(vbCrLf & "<ul>")

        Dim i As Integer

        For j As Integer = 0 To ds.Tables(0).Rows.Count - 1
            Dim dRow As DataRow = ds.Tables(0).Rows(j)
            If Not (dRow.IsNull("ParentNodeID")) Then
                If dRow("NodeLevel") = "1" Then
                    i += 1
                    sb.Append("<li title=""" & dRow("Text") & """>")
                    If dRow("PublishType").ToString = "2" And (IIf(IsDBNull(dRow("RefPath")), "", dRow("RefPath")).ToString.IndexOf("http://".ToLower) = 0 Or IIf(IsDBNull(dRow("RefPath")), "", dRow("RefPath")).ToString.IndexOf("https://".ToLower) = 0 Or IIf(IsDBNull(dRow("RefPath")), "", dRow("RefPath")).ToString.IndexOf("mailto:".ToLower) = 0) Then
                        sb.Append("<a href=""" & dRow("RefPath") & """>")
                    Else
                        sb.Append("<a href=""" & dRow("RefPath") & "?cnid=" & dRow("NodeID").ToString & """>")
                    End If
                    '====
                    sb.Append(i & ". " & dRow("Text"))
                    sb.Append("</a>")
                    If GetChildRowsCount(dRow, 1) Then
                        sb.Append(GetChildRowsString(dRow, 1, i.ToString))
                    End If
                    sb.Append("</li>" & vbCrLf)
                End If

            End If
        Next
        sb.Append("</ul>")
        Return sb.ToString

    End Function


    ''' <summary>
    '''  取得並產生子資料列HTML碼
    ''' </summary>
    ''' <param name="dRow">DataRow物件</param>
    ''' <param name="Level">層數</param>
    ''' <param name="no">項目符號數</param>
    ''' <returns>HTML碼</returns>
    ''' <remarks></remarks>
    Private Function GetChildRowsString(ByVal dRow As DataRow, ByVal Level As Integer, ByVal no As String) As String
        Dim sb As StringBuilder = New StringBuilder
        Level += 1

        Dim i As Integer
        sb.Append(vbCrLf & "<ul>")

        For j As Integer = 0 To dRow.GetChildRows("NodeRelation2").Count - 1
            Dim childRow As DataRow = dRow.GetChildRows("NodeRelation2")(i)
            Try

                i += 1
                Dim ListItem As String
                ListItem = GetListItem(no, Level, i)
                sb.Append("<li title=""" & childRow("Text") & """>")
                'sb.Append("<a href=""" & childRow("RefPath") & "?cnid=" & childRow("NodeID").ToString & """>")
                'Chris Chu 20090210
                If childRow("PublishType").ToString = "2" And (IIf(IsDBNull(childRow("RefPath")), "", childRow("RefPath")).ToString.IndexOf("http://".ToLower) = 0 Or IIf(IsDBNull(childRow("RefPath")), "", childRow("RefPath")).ToString.IndexOf("https://".ToLower) = 0 Or IIf(IsDBNull(childRow("RefPath")), "", childRow("RefPath")).ToString.IndexOf("mailto:".ToLower) = 0) Then
                    sb.Append("<a href=""" & childRow("RefPath") & """>")
                Else
                    sb.Append("<a href=""" & childRow("RefPath") & "?cnid=" & childRow("NodeID").ToString & """>")
                End If
                sb.Append(ListItem & ". " & childRow("Text"))
                sb.Append("</a>")
                If GetChildRowsCount(childRow, Level) Then
                    sb.Append(GetChildRowsString(childRow, Level, ListItem))
                End If
                sb.Append("</li>" & vbCrLf)
            Catch ex As Exception
                Dim dd As String = ex.Message
            End Try
        Next
        sb.Append("</ul>")

        Return sb.ToString
    End Function


    ''' <summary>
    ''' 產生項目符號
    ''' </summary>
    ''' <param name="ListItem">項目符號</param>
    ''' <param name="level">層數</param>
    ''' <param name="last">現行列數</param>
    ''' <returns>項目符號</returns>
    ''' <remarks></remarks>
    Private Function GetListItem(ByVal ListItem As String, ByVal level As Integer, ByVal last As Integer) As String
        ListItem += "-" & last
        Return ListItem
    End Function


    ''' <summary>
    '''  取得是否有子資料列
    ''' </summary>
    ''' <param name="dRow">DataRow物件</param>
    ''' <param name="Level">層數</param>
    ''' <returns>布林值(True/False)</returns>
    ''' <remarks></remarks>
    Private Function GetChildRowsCount(ByVal dRow As DataRow, ByVal Level As Integer) As Boolean
        Level += 1
        Dim i As Integer = 0
        For j As Integer = 0 To dRow.GetChildRows("NodeRelation2").Count - 1
            Dim childRow As DataRow = dRow.GetChildRows("NodeRelation2")(i)
            i += 1
        Next
        If i = 0 Then
            Return False
        Else
            Return True
        End If
    End Function

End Class
