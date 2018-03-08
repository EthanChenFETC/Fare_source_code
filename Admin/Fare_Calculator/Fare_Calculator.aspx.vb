Imports System.Data
Imports System.Data.SqlClient
Partial Class Fare_Calculator_Fare_Calculator
    Inherits PageBase
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Me.txtSdate.Text = "2013/10/01"
            Me.txtEdate.Text = "3000/12/31"
        End If
        If Me.txtSdate.Text.Trim.Length = 0 Then
            Me.txtSdate.Text = "2013/10/01"
        End If
        If Me.txtEdate.Text.Trim.Length = 0 Then
            Me.txtEdate.Text = "3000/12/31"
        End If
    End Sub

    ''' <summary>
    ''' 清單總量、頁數資料
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub GridView1_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.DataBound
        GridViewPageInfo.GetGridViewInfo(Me.GridView1, Me.Page, Me.SDS_Fare_Calculator_List)
    End Sub

  
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        GridView1.DataBind()
    End Sub
End Class
