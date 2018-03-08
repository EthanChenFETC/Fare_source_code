
Partial Class SystemDesign_SiteMapGroupCatgry
    Inherits PageBase

    Protected Sub GridView1_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.DataBound
        GridViewPageInfo.GetGridViewInfo(Me.GridView1, Me.Page, Me.sds_Categry)
        Me.Page.Form.Enctype = "multipart/form-data"

    End Sub

    Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click
        Me.MultiView1.ActiveViewIndex = 0
        Me.txtGroupName.Text = ""
    End Sub

    Protected Sub btn_Insert_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Insert.Click
        If Page.IsValid Then
            Me.sds_Categry.Insert()
        End If
    End Sub

    Protected Sub sds_Categry_Deleted(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.SqlDataSourceStatusEventArgs) Handles sds_Categry.Deleted
        If e.AffectedRows > 0 Then
            Me.lbMessage.Text = GridViewPageInfo.Message_Delete_OK(Me.Page, "後台管理-選單群組代碼管理")
        Else
            Me.lbMessage.Text = GridViewPageInfo.Message_Delete_False(Me.Page, "後台管理-選單群組代碼管理")
        End If
        Me.MultiView1.ActiveViewIndex = -1
    End Sub

    Protected Sub sds_Categry_Inserted(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.SqlDataSourceStatusEventArgs) Handles sds_Categry.Inserted
        If e.AffectedRows > 0 Then
            Me.lbMessage.Text = GridViewPageInfo.Message_Insert_OK(Me.Page, "後台管理-選單群組代碼管理")
        Else
            Me.lbMessage.Text = GridViewPageInfo.Message_Insert_False(Me.Page, "後台管理-選單群組代碼管理")
        End If
        Me.MultiView1.ActiveViewIndex = -1
        'Me.GridView1.DataBind()
    End Sub

    Protected Sub sds_Categry_Updated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.SqlDataSourceStatusEventArgs) Handles sds_Categry.Updated
        If e.AffectedRows > 0 Then
            Me.lbMessage.Text = GridViewPageInfo.Message_Update_OK(Me.Page, "後台管理-選單群組代碼管理")
        Else
            Me.lbMessage.Text = GridViewPageInfo.Message_Update_False(Me.Page, "後台管理-選單群組代碼管理")
        End If
        Me.MultiView1.ActiveViewIndex = -1
        'Me.GridView1.DataBind()
    End Sub

    Protected Sub btn_Cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Cancel.Click
        Me.MultiView1.ActiveViewIndex = -1
    End Sub

    Protected Sub btnShowAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnShowAll.Click
        Me.txtSearch.Text = ""
    End Sub
    Dim TItlePicPath As String = "TitlePic/"
    Protected Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand

        If e.CommandName = "ufTitlePic" Then

            Dim key() As String = Split(e.CommandArgument, ",")
            Dim FileUploadTitlePic As FileUpload = CType(Me.GridView1.Rows(CInt(key(1))).FindControl("FileUploadTitlePic"), FileUpload)
            Dim dd As String = FileUploadTitlePic.FileName

            If FileUploadTitlePic.HasFile Then
                Dim fileName As String = FileUploadTitlePic.FileName


                'Check dir exits
                If Not FileIO.FileSystem.DirectoryExists(Server.MapPath(ModulePathManager.GetUploadPath & TItlePicPath)) Then
                    FileIO.FileSystem.CreateDirectory(Server.MapPath(ModulePathManager.GetUploadPath & TItlePicPath))
                End If

                'Upload
                FileUploadTitlePic.SaveAs(Server.MapPath(RemoveXSS(ModulePathManager.GetUploadPath & TItlePicPath & fileName)))

                'Update DB
                Me.SDS_UpdateTitlePic.UpdateParameters("TitlePic").DefaultValue = ModulePathManager.GetUploadPath & TItlePicPath & fileName
                Me.SDS_UpdateTitlePic.UpdateParameters("GroupID").DefaultValue = key(0)
                Me.SDS_UpdateTitlePic.Update()

                Me.GridView1.DataBind()

            Else
                Me.lbMessage.Text = "<span style=""color:#FF0000"">系統訊息：請選擇上傳圖檔！</span>"
            End If
        End If
        If e.CommandName = "UpdateItem" Then
            Dim key() As String = e.CommandArgument.ToString.Split(",")
            Dim SiteID As DropDownList = CType(GridView1.Rows(key(1)).FindControl("ddl_Site_List"), DropDownList)
            Me.sds_Categry.UpdateParameters("SiteID").DefaultValue = SiteID.SelectedValue
            Me.GridView1.UpdateRow(key(1), False)
        End If
    End Sub
    Protected Sub GridView1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView1.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim drowView As System.Data.DataRowView = CType(e.Row.DataItem, System.Data.DataRowView)
            If e.Row.RowType = DataControlRowType.DataRow Then
                '檢視模式
                If e.Row.RowState = 0 OrElse e.Row.RowState = 1 Then
                    '指定Datakey給上傳圖檔的Button
                    Dim btnTitlePicUpload As Button = CType(e.Row.FindControl("btnTitlePicUpload"), Button)
                    btnTitlePicUpload.CommandName = "ufTitlePic"
                    btnTitlePicUpload.CommandArgument = Me.GridView1.DataKeys(e.Row.RowIndex).Value.ToString & "," & e.Row.RowIndex
                    btnTitlePicUpload.CausesValidation = True
                    btnTitlePicUpload.ValidationGroup = e.Row.RowIndex
                    'btnTitlePicUpload.Text += "RowIndex=" & e.Row.RowIndex.ToString

                    Dim RequiredFieldValidator1 As RequiredFieldValidator = CType(e.Row.FindControl("RequiredFieldValidator1"), RequiredFieldValidator)
                    RequiredFieldValidator1.ValidationGroup = e.Row.RowIndex
                    Dim SiteName As Label = CType(e.Row.FindControl("SiteName"), Label)
                    SiteName.Text = (drowView("SiteName").ToString)
                ElseIf e.Row.RowState = DataControlRowState.Edit Then
                    Dim ddl_Site_List As DropDownList = CType(e.Row.FindControl("ddl_Site_List"), DropDownList)
                    ddl_Site_List.DataBind()
                    ddl_Site_List.SelectedValue = drowView("SiteID").ToString
                    Dim Button1 As Button = CType(e.Row.FindControl("Button1"), Button)
                    Button1.CommandArgument = Me.GridView1.DataKeys(e.Row.RowIndex).Value.ToString & "," & e.Row.RowIndex
                End If
            End If
        End If
    End Sub
    Protected Sub SDS_UpdateTitlePic_Updated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.SqlDataSourceStatusEventArgs) Handles SDS_UpdateTitlePic.Updated
        If e.AffectedRows > 0 Then
            Me.lbMessage.Text = "<span style=""color:#FF0000"">系統訊息：上傳圖檔完成！</span>"
        End If
    End Sub

End Class
