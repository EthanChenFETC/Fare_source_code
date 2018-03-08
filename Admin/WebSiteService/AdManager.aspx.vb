Imports System.Data
Imports System.Data.SQLClient
Imports System.IO

Partial Class WebSiteService_AdManager
    Inherits PageBase

    Dim AdvertisementsPath As String = System.Configuration.ConfigurationManager.AppSettings("UploadPath").ToString & "Advertisements/"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        '客戶端欄位檢查
        Me.btnAddNew.Attributes("onmousedown") = "javascript: if (" & Me.txtCaption.ClientID & ".value=='')" & "window.alert('請輸入名稱!')" & ";if (" & Me.txtAlternateText.ClientID & ".value=='') window.alert('請輸入註解!');" & _
        "if (" & Me.txtNavigateUrl.ClientID & ".value=='')" & "window.alert('請輸入連結!')" & ";if (" & Me.FileUpload1.ClientID & ".value=='') window.alert('請選擇上傳的圖檔!');"
        Me.txtCaption.Attributes("onblur") = "javascript:" & Me.txtAlternateText.ClientID & ".value = this.value;"
    End Sub

    Private Function GetAdPath() As String
        Dim ADPath As String = Server.MapPath(AdvertisementsPath)
        If Not ADPath.EndsWith("/") Then ADPath = ADPath & "/"
        Return RemoveXSS(ADPath)
    End Function

    '----------------------------------------------------------------------------------------------------
    '增加一個新的廣告
    '----------------------------------------------------------------------------------------------------
    Private Sub btnAddNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddNew.Click
        '伺服器端檢查欄位檢查
        Dim PostDate As String = Now.ToString("yyyy/MM/dd HH:mm:ss", System.Globalization.DateTimeFormatInfo.InvariantInfo)

        'GetHttpPostFile
        Dim HttpPostedFile1 As HttpPostedFile = Me.FileUpload1.PostedFile
        'GetFileName
        Dim FileName As String = RemoveXSS(Me.FileUpload1.FileName)


        Dim retVal As Integer

        If HttpPostedFile1.ContentLength = Nothing Then
            Me.lblAddMessage.Text = "請選擇上傳的檔案..."
        Else
            Try
                retVal = ClassDB.RunSPReturnInteger("Net2_Advertisements_Insert",
                New SqlParameter("@SiteID", CInt(Me.ddlSite_List.SelectedValue)),
                New SqlParameter("@ImageUrl", RemoveSQLInjection(FileName)),
                New SqlParameter("@Caption", RemoveSQLInjection(Me.txtCaption.Text)),
                New SqlParameter("@AlternateText", RemoveSQLInjection(Me.txtAlternateText.Text) & "(另開新視窗)"),
                New SqlParameter("@NavigateUrl", RemoveSQLInjection(Me.txtNavigateUrl.Text)),
                New SqlParameter("@Keyword", RemoveSQLInjection(Me.txtKeyword.Text)),
                New SqlParameter("@impressions", RemoveSQLInjection(Me.txtImpressions.Text)),
                New SqlParameter("@UserID", CInt(Session("UserID").ToString)),
                New SqlParameter("@DepartmentID", CInt(Session("DepartmentID").ToString)),
                New SqlParameter("@GroupID", CInt((Me.ddlGroup_List.SelectedValue))))

                Me.FileUpload1.SaveAs(GetAdPath() & FileName)
            Catch ex As Exception
                ModuleWriteLog.WriteLog(Request.Path, ex.Message & ex.StackTrace & ex.Source, Me)
            End Try
           

            If retVal = 1 Then
                Me.lblAddMessage.Text = "系統訊息：新增-廣告成功"
                WriteAdminLog(Me, "新增-廣告管理")
            Else
                Me.lblAddMessage.Text = "系統訊息：新增-廣告失敗，請重新操作。"
                WriteAdminLog(Me, "新增-廣告管理", False)
            End If
        End If

        Me.GridView1.DataBind()
        CacheObject.RemoveCacheKey(CacheObject.CacheKey_Ad)  '移除前台AD快取
    End Sub



    Protected Sub GridView1_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.DataBound
        GridViewPageInfo.GetGridViewInfo(Me.GridView1, Me, Me.SDS_AD_List)
    End Sub

    Protected Sub ddlSite_List_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlSite_List.DataBound
        Me.sds_Group_List.SelectParameters.Item("SiteID").DefaultValue = Me.ddlSite_List.SelectedValue
        Me.sds_Group_List.DataBind()
    End Sub
    Protected Sub ddlSite_List_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlSite_List.SelectedIndexChanged
        Me.sds_Group_List.SelectParameters.Item("SiteID").DefaultValue = Me.ddlSite_List.SelectedValue
        Me.sds_Group_List.DataBind()
    End Sub
    Private SiteValue As Integer = 0
    Private GroupValue As Integer = 0
    Protected Sub ddlSite_List2_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ddl As DropDownList = sender
        Me.sds_Group_List2.SelectParameters.Item("SiteID").DefaultValue = ddl.SelectedValue
        Me.sds_Group_List2.DataBind()
    End Sub
    Protected Sub ddlSite_List2_DataBound(ByVal sender As Object, ByVal e As System.EventArgs)
        SiteValue = sender.SelectedValue
        Me.sds_Group_List2.SelectParameters("SiteID").DefaultValue = SiteValue
        Me.sds_Group_List2.DataBind()
    End Sub
    Protected Sub ddlGroup_List2_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
        ' Me.sds_Group_List2.SelectParameters(0).DefaultValue = SiteValue
    End Sub
    Protected Sub ddlGroup_List2_DataBound(ByVal sender As Object, ByVal e As System.EventArgs)
        'Dim ddl As DropDownList = CType(sender, DropDownList)
        'ddl.SelectedIndex = 0
    End Sub

    Protected Sub GridView1_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles GridView1.RowUpdating
        Dim dv As GridView = sender
        Dim rv As DataRowView = CType(dv.Rows(e.RowIndex).DataItem, DataRowView)
        Dim ddl As DropDownList = CType(dv.Rows(e.RowIndex).FindControl("ddlGroup_List2"), DropDownList)
        Dim ddl2 As DropDownList = CType(dv.Rows(e.RowIndex).FindControl("ddlSite_List2"), DropDownList)
        Dim TextBox1 As TextBox = CType(dv.Rows(e.RowIndex).FindControl("TextBox1"), TextBox)
        Dim TextBox2 As TextBox = CType(dv.Rows(e.RowIndex).FindControl("TextBox2"), TextBox)
        Dim TextBox3 As TextBox = CType(dv.Rows(e.RowIndex).FindControl("TextBox3"), TextBox)
        Me.SDS_AD_List.UpdateParameters("AlternateText").DefaultValue = RemoveSQLInjection(TextBox1.Text)
        Me.SDS_AD_List.UpdateParameters("Caption").DefaultValue = RemoveSQLInjection(TextBox2.Text)
        Me.SDS_AD_List.UpdateParameters("NavigateUrl").DefaultValue = RemoveSQLInjection(TextBox3.Text)

        Me.SDS_AD_List.UpdateParameters("SiteID").DefaultValue = CInt(ddl2.SelectedValue)
        Me.SDS_AD_List.UpdateParameters("GroupID").DefaultValue = CInt(ddl.SelectedValue)
    End Sub

    Protected Sub GridView1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView1.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim rv As DataRowView = CType(e.Row.DataItem, DataRowView)
            If e.Row.RowState = 0 OrElse e.Row.RowState = 1 OrElse e.Row.RowState = 2 OrElse e.Row.RowState = 3 Then
                Dim Image1 As Image = CType(e.Row.FindControl("Image1"), Image)
                Image1.ImageUrl = AdvertisementsPath & rv.Item("ImageUrl").ToString
                Image1.ToolTip = rv.Item("AlternateText").ToString
                Dim Label1 As Label = CType(e.Row.FindControl("Label1"), Label)
                Dim Label2 As Label = CType(e.Row.FindControl("Label2"), Label)
                Dim Label3 As Label = CType(e.Row.FindControl("Label3"), Label)
                Dim Label4 As Label = CType(e.Row.FindControl("Label4"), Label)
                Label1.Text = rv("SiteName")
                Label2.Text = rv("Caption")
                Label3.Text = rv("NavigateUrl")
                Label4.Text = rv("GroupName")
            End If
            If e.Row.RowState = DataControlRowState.Edit Or e.Row.RowState = 5 Then
                Dim ddl As DropDownList = CType(e.Row.FindControl("ddlGroup_List2"), DropDownList)
                Dim ddlSite_List2 As DropDownList = CType(e.Row.FindControl("ddlSite_List2"), DropDownList)

                If ddlSite_List2.SelectedValue <> rv.Item("SiteID").ToString Then
                    ddl.SelectedIndex = 0
                Else
                    ddl.SelectedValue = rv.Item("GroupID").ToString
                End If
                Dim TextBox2 As TextBox = CType(e.Row.FindControl("TextBox2"), TextBox)
                Dim TextBox3 As TextBox = CType(e.Row.FindControl("TextBox3"), TextBox)
                Dim TextBox1 As TextBox = CType(e.Row.FindControl("TextBox1"), TextBox)
                TextBox1.Text = rv("AlternateText")
                TextBox2.Text = rv("Caption")
                TextBox3.Text = rv("NavigateUrl")
            End If
        End If
    End Sub

    Protected Sub btnShowAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnShowAll.Click
        Me.txtSearch.Text = ""
    End Sub

    Protected Sub GridView1_RowDeleted(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeletedEventArgs) Handles GridView1.RowDeleted
        If e.AffectedRows > 0 Then
            CacheObject.RemoveCacheKey(CacheObject.CacheKey_Ad)  '移除前台AD快取
        End If
    End Sub

 
    Protected Sub GridView1_RowUpdated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdatedEventArgs) Handles GridView1.RowUpdated
        If e.AffectedRows > 0 Then
            CacheObject.RemoveCacheKey(CacheObject.CacheKey_Ad)  '移除前台AD快取
        End If
    End Sub
End Class
