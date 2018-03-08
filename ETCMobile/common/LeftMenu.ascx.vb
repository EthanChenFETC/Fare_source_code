Imports system.data
Imports System.Data.SqlClient

''' <summary>
''' 本資訊網上方選單項目
''' </summary>
''' <remarks></remarks>
Partial Class common_LeftMenu
    Inherits System.Web.UI.UserControl
    Dim TotalMenuCount As Integer = 0

    ''' <summary>
    ''' 頁面讀取事件
    ''' </summary>
    ''' <param name="sender">觸發本事件控制項</param>
    ''' <param name="e">觸發本事件帶進之參數</param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Dim Injection As Sql_Injection = New Sql_Injection
        'Injection.Check_Sql_Injection(Request, Response)
        '無障礙


        Dim Menu As String = WebMenuJs.BuidMenu
        Me.ltMenu.Text = Menu
    End Sub

End Class
