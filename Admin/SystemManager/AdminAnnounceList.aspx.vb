
Partial Class SystemManager_AdminAnnounceList
    Inherits PageBase

    Protected Sub btnShowAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnShowAll.Click
        Me.txtSearch.Text = ""
    End Sub

    Protected Sub GridView1_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.DataBound
        GridViewPageInfo.GetGridViewInfo(Me.GridView1, Me.Page, Me.SDS_AdminAnnoce_List)
    End Sub

    Protected Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand

        If e.CommandName = "EditPublish" Then
            Dim index As Integer = CInt(e.CommandArgument)
            Dim UID As Integer = CInt(GridView1.DataKeys(index).Value)  '取得主鍵，轉到編輯頁面

            Session("PublishMode") = "Edit"
            Session("PublisKey") = UID

            InterfaceBuilder.TabGoNext(Me.Page)    'ChangeUserControlByPassFileName(Me.Page, "AdminAnnounceEdit.ascx", "ascx")

        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Session("PublishMode") = Nothing
        Session("PublisKey") = Nothing
    End Sub
End Class
