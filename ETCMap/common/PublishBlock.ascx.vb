Imports System.Data
Imports System.Data.SqlClient

'''<summary>
'''本資訊網網頁上局部上稿區塊(使用者控制項)。
'''</summary>
'''<remarks>
'''</remarks>
Partial Class common_PublishBlock
    Inherits System.Web.UI.UserControl

    Private _BlockID As String

    ''' <summary>
    ''' 區塊ID，即在資料表中區塊主鍵
    ''' </summary>
    ''' <value>ID值</value>
    ''' <remarks></remarks>
    WriteOnly Property BlockID() As String
        Set(ByVal value As String)
            _BlockID = value
        End Set
    End Property

    ''' <summary>
    ''' 頁面讀取事件
    ''' </summary>
    ''' <param name="sender">觸發本事件控制項</param>
    ''' <param name="e">觸發本事件帶進之參數</param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'If Not IsPostBack Then
        Dim sql As String = "SELECT Content FROM PublishBlock WHERE (BlockID = @BlockID)" ''" & _BlockID & "')"
        Using dr As SqlDataReader = ClassDB.GetDataReaderParam(sql, New SqlParameter("@BlockID", _BlockID))
            Try
                If dr IsNot Nothing Then
                    If dr.Read Then
                        Dim Content As String = dr("Content").ToString
                        Me.lbContent.Text = Content
                    End If
                End If
            Catch ex As Exception
                WriteErrLog(ex, Me.Page)
            Finally

            End Try
        End Using
        'End If

    End Sub

End Class
