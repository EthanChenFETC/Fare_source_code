
Partial Class SystemManager_AdminLogs
    Inherits PageBase

    Protected Sub btnShowAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnShowAll.Click
        Me.txtSearch.Text = ""
    End Sub

    Protected Sub GridView1_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.DataBound

        GridViewPageInfo.GetGridViewInfo(GridView1, Me.Page, Me.SqlDS_AdminLog)

    End Sub

End Class
