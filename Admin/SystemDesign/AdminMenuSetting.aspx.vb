Imports System.Data


Partial Class SystemManager_AdminMenuSetting
    'Inherits System.Web.UI.Page
    Inherits PageBase



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            'BuildGridView1()
        End If

    End Sub
    Protected Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand
        If e.CommandName = "Update" Then
            Dim key As String = e.CommandArgument
            Dim ddlTabType As DropDownList = CType(Me.GridView1.Rows(key).FindControl("ddlTabType"), DropDownList)
            Me.SqlDataSource_AdminMenu.UpdateParameters("TabType").DefaultValue = ddlTabType.SelectedValue
            Dim DropDownList1 As DropDownList = CType(Me.GridView1.Rows(key).FindControl("DropDownList1"), DropDownList)
            Me.SqlDataSource_AdminMenu.UpdateParameters("PublishType").DefaultValue = DropDownList1.SelectedValue
            Dim TextBox1 As TextBox = CType(Me.GridView1.Rows(key).FindControl("TextBox1"), TextBox)
            Me.SqlDataSource_AdminMenu.UpdateParameters("Text").DefaultValue = TextBox1.Text
            Me.SqlDataSource_AdminMenu.Update()
        End If
    End Sub

    Protected Sub GridView1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView1.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then

            Dim drowView As System.Data.DataRowView = CType(e.Row.DataItem, System.Data.DataRowView)

            '編輯模式
            If e.Row.RowState = 0 OrElse e.Row.RowState = 1 Or e.Row.RowState = 2 OrElse e.Row.RowState = 3 Then
                Dim lblTabType As Label = CType(e.Row.FindControl("lblTabType"), Label)
                Dim Label2 As Label = CType(e.Row.FindControl("Label2"), Label)
                Dim Label1 As Label = CType(e.Row.FindControl("Label1"), Label)
                Dim Image1 As Image = CType(e.Row.FindControl("Image1"), Image)
                Label1.Text = (drowView("Text").ToString)
                Label1.CssClass = "AdminNode" & drowView("NodeLevel").ToString
                Image1.CssClass = "AdminMenuImage" & drowView("NodeLevel").ToString
                Label2.Text = (drowView("PublishTypeName").ToString)
                lblTabType.Text = (drowView("TabTypeName").ToString)
                Image1.ImageUrl = "Images/" & (drowView("ImageUrl"))
            ElseIf e.Row.RowState = 4 OrElse e.Row.RowState = 5 Then
                Dim ddlTabType As DropDownList = CType(e.Row.FindControl("ddlTabType"), DropDownList)
                ddlTabType.SelectedValue = drowView("TabType")
                Dim DropDownList1 As DropDownList = CType(e.Row.FindControl("DropDownList1"), DropDownList)
                DropDownList1.DataBind()
                DropDownList1.SelectedValue = drowView("PublishType")


                Dim TextBox1 As TextBox = CType(e.Row.FindControl("TextBox1"), TextBox)
                TextBox1.Text = Server.HtmlDecode(drowView("Text").ToString)


            End If

        End If
    End Sub


    Public Sub TEST()

    End Sub
End Class
