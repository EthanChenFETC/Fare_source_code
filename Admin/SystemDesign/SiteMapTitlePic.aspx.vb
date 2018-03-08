Imports System.Data
Imports System.Data.SqlClient
Partial Class SystemDesign_SiteMapTitlePic
    'Inherits System.Web.UI.Page
    Inherits PageBase

    Dim TItlePicPath As String = "TitlePic/"

    Protected Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand
        If e.CommandName = "ufTitlePic" Then
            Dim key() As String = Split(e.CommandArgument, ",")
            Me.MultiView1.ActiveViewIndex = 1
            Me.lblNodeID.Text = key(0)
            Me.lblNodeText.Text = Me.GridView1.DataKeys(key(1))("Text").ToString
            Me.Uploader1A.SingleUploadFilePath = Me.GridView1.DataKeys(key(1))("TitlePic").ToString
            Me.Uploader1B.SingleUploadFilePath = Me.GridView1.DataKeys(key(1))("TitlePic2").ToString
            Me.Uploader1C.SingleUploadFilePath = Me.GridView1.DataKeys(key(1))("TitlePic3").ToString
        End If
        'If e.CommandName = "ufTitlePic" Then
        '    Dim key() As String = Split(e.CommandArgument, ",")
        '    Dim FileUploadTitlePic As FileUpload = CType(Me.GridView1.Rows(CInt(key(1))).FindControl("FileUploadTitlePic"), FileUpload)

        '    If FileUploadTitlePic.HasFile Then
        '        Dim fileName As String = FileUploadTitlePic.FileName


        '        'Check dir exits
        '        If Not FileIO.FileSystem.DirectoryExists(Server.MapPath(ModulePathManager.GetUploadPath & TItlePicPath)) Then
        '            FileIO.FileSystem.CreateDirectory(Server.MapPath(ModulePathManager.GetUploadPath & TItlePicPath))
        '        End If

        '        'Upload
        '        FileUploadTitlePic.SaveAs(Server.MapPath(ModulePathManager.GetUploadPath & TItlePicPath & fileName))

        '        'Update DB
        '        Me.SDS_UpdateTitlePic.UpdateParameters("TitlePic").DefaultValue = ModulePathManager.GetUploadPath & TItlePicPath & fileName
        '        Me.SDS_UpdateTitlePic.UpdateParameters("NodeID").DefaultValue = key(0)
        '        Me.SDS_UpdateTitlePic.Update()

        '        Me.GridView1.DataBind()

        '    Else
        '        Me.ltlMessage.Text = "<span style=""color:#FF0000"">系統訊息：請選擇上傳圖檔！</span>"
        '    End If
        'End If
    End Sub




    Protected Sub GridView1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView1.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then

            Dim drowView As System.Data.DataRowView = CType(e.Row.DataItem, System.Data.DataRowView)

            If e.Row.RowType = DataControlRowType.DataRow Then
                '檢視模式
                'If ((e.Row.RowState = DataControlRowState.Normal) OrElse (e.Row.RowState = DataControlRowState.Alternate)) Then
                If e.Row.RowState = 0 OrElse e.Row.RowState = 1 Then
                    '指定Datakey給上傳圖檔的Button
                    Dim btnTitlePicUpload As Button = CType(e.Row.FindControl("btnTitlePicUpload"), Button)
                    btnTitlePicUpload.CommandName = "ufTitlePic"
                    btnTitlePicUpload.CommandArgument = Me.GridView1.DataKeys(e.Row.RowIndex).Value.ToString & "," & e.Row.RowIndex
                    btnTitlePicUpload.CausesValidation = False
                    btnTitlePicUpload.ValidationGroup = e.Row.RowIndex
                    'btnTitlePicUpload.Text += "RowIndex=" & e.Row.RowIndex.ToString
                    Dim Image1 As Image = CType(e.Row.FindControl("Image1"), Image)
                    Dim Label1 As Label = CType(e.Row.FindControl("Label1"), Label)
                    Image1.CssClass = "AdminMenuImage" & drowView("NodeLevel").ToString
                    Image1.ImageUrl = "Images/" & drowView("ImageUrl").ToString
                    Label1.CssClass = "AdminNode" & drowView("NodeLevel").ToString
                    Label1.Text = drowView("Text").ToString
                End If

            End If


        End If
    End Sub



#Region "Edit"

  

    ''' <summary>
    ''' 更新上架媒體-資料庫
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btn_Update_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Update.Click
        If Me.Page.IsValid Then

            Me.sds_TitlePic.UpdateParameters("NodeID").DefaultValue = Me.lblNodeID.Text
            Me.sds_TitlePic.UpdateParameters("TitlePic").DefaultValue = IIf(IsDBNull(Me.Uploader1A.SingleUploadFilePath), "", Me.Uploader1A.SingleUploadFilePath) 'Chris 20080602
            Me.sds_TitlePic.UpdateParameters("TitlePic2").DefaultValue = IIf(IsDBNull(Me.Uploader1B.SingleUploadFilePath), "", Me.Uploader1B.SingleUploadFilePath) 'Chris 20080602
            Me.sds_TitlePic.UpdateParameters("TitlePic3").DefaultValue = IIf(IsDBNull(Me.Uploader1C.SingleUploadFilePath), "", Me.Uploader1C.SingleUploadFilePath) 'Chris 20080602
            Me.sds_TitlePic.Update()
        End If
    End Sub

    ''' <summary>
    ''' 資料庫來源物件-更新完成事作
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub sds_TitlePic_Updated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.SqlDataSourceStatusEventArgs) Handles sds_TitlePic.Updated
        If e.AffectedRows > 0 Then
            Me.lbMessage.Text = GridViewPageInfo.Message_Update_OK(Me.Page, "選單標頭上傳")
        Else
            Me.lbMessage.Text = GridViewPageInfo.Message_Update_False(Me.Page, "選單標頭上傳")
        End If
        Me.MultiView1.ActiveViewIndex = 0
        Me.GridView1.DataBind()
    End Sub
    Protected Sub btn_Cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Cancel.Click
        Me.MultiView1.ActiveViewIndex = 0
    End Sub

#End Region

End Class
