Imports System.Data
Imports System.Data.SqlClient

Partial Class Publish_FaqCatgry
    Inherits PageBase


    Protected Sub Page_Load1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.sds_FaqCatgry_List.SelectParameters("ApKeyword").DefaultValue = MultiSites.GetNodeKeyword(Me)
    End Sub

    Protected Sub GridView1_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.DataBound
        GridViewPageInfo.GetGridViewInfo(Me.GridView1, Me.Page, Me.sds_FaqCatgry_List)
    End Sub



#Region "編輯功能"


    Protected Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand
        If e.CommandName = "EditCatgry" Then
            Me.MultiView1.ActiveViewIndex = 0
        End If
    End Sub

    Protected Sub GridView1_RowDeleted(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeletedEventArgs) Handles GridView1.RowDeleted
        If e.AffectedRows > 0 Then
            Dim ApUID As Integer = GridViewPageInfo.GetGridViewRowDeletedDataKey(e)
            Me.SiteCheckBoxList1.SitesApRelation_Delete(ApUID)   '新增上稿網站關聯
            Me.lbMessage.Text = GridViewPageInfo.Message_Delete_OK(Me, "常見問答-分類代碼管理 ")
        Else
            Me.lbMessage.Text = GridViewPageInfo.Message_Insert_False(Me, "常見問答-分類代碼管理 ")
        End If
    End Sub


    Protected Sub GridView1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.SelectedIndexChanged
        Me.MultiView1.ActiveViewIndex = 1
        Dim CategoryID As Integer = Me.GridView1.DataKeys(Me.GridView1.SelectedIndex).Value
        Dim s As String = Me.sds_Catgry_Detail.SelectCommand '.Replace("@CateGoryID", CategoryID)
        Using dr As SqlDataReader = ClassDB.GetDataReaderParam(s, New SqlParameter("@CategoryID", CategoryID))
            Try
                If dr IsNot Nothing Then
                    If dr.Read Then
                        Me.txtSubject_Edit.Text = dr("CateGoryName").ToString
                        Me.ViewTextboxDep1.SetDepartmentID = dr("ResponDepartment").ToString
                        Me.ViewTextboxUser1.SetUserID = dr("ResponUser").ToString
                        Me.SiteCheckBoxList2.ReadBackContent(CategoryID)
                    End If
                End If
            Catch ex As Exception
                WriteErrLog(ex, Me)
            Finally

            End Try
        End Using
    End Sub

    ''' <summary>
    ''' 確定更新事件
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btn_Update_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Update.Click
        Me.sds_Catgry_Detail.Update()
      
    End Sub

    ''' <summary>
    ''' DataSource更新完成 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub sds_Catgry_Detail_Updated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.SqlDataSourceStatusEventArgs) Handles sds_Catgry_Detail.Updated
        If e.AffectedRows > 0 Then
            Me.SiteCheckBoxList2.SitesApRelation_Update()   '更新關聯資料
            Me.lbMessage.Text = GridViewPageInfo.Message_Update_OK(Me, "常見問答分類代碼")
        Else
            Me.lbMessage.Text = GridViewPageInfo.Message_Update_False(Me, "常見問答分類代碼")
        End If
        Me.MultiView1.ActiveViewIndex = -1
        Me.GridView1.DataBind()
    End Sub

#End Region



#Region "新增功能"

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub BtnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnAddNew.Click
        Me.txtSubject.Text = ""
        Me.MultiView1.ActiveViewIndex = 0
    End Sub


    ''' <summary>
    ''' Insert-Button Event
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btn_Insert_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Insert.Click
        Me.sds_Catgry_Detail.Insert()
    End Sub

    ''' <summary>
    ''' Insert-Button Temp
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btn_Insert_temp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Insert_temp.Click
        Me.sds_Catgry_Detail.InsertParameters("IsOnline").DefaultValue = False
        Me.sds_Catgry_Detail.Insert()
    End Sub

    ''' <summary>
    ''' Check SiteCheckBoxList is Check?
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub sds_Catgry_Detail_Inserting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.SqlDataSourceCommandEventArgs) Handles sds_Catgry_Detail.Inserting
        If Me.SiteCheckBoxList1.GetIsValid = False Then
            ModuleMisc.AlertMsgInUpdatePanel(Me, "您沒有勾選欲上稿的網站!", Me.UpdatePanel1)
            e.Cancel = True
        End If
    End Sub

    ''' <summary>
    ''' 新增完分類資料
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub sds_Catgry_Detail_Inserted(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.SqlDataSourceStatusEventArgs) Handles sds_Catgry_Detail.Inserted
        If e.AffectedRows > 0 Then
            Dim NewApUID As Integer = GridViewPageInfo.GetSDSInsert_DataKey(e)
            Me.SiteCheckBoxList1.SitesApRelation_Insert(NewApUID)   '新增上稿網站關聯
            Me.lbMessage.Text = GridViewPageInfo.Message_Insert_OK(Me.Page, "常見問答分類代碼")
        Else
            Me.lbMessage.Text = GridViewPageInfo.Message_Insert_False(Me.Page, "常見問答分類代碼")
        End If

        Me.MultiView1.ActiveViewIndex = -1
        Me.GridView1.DataBind()
    End Sub


#End Region


#Region "一般區域"

    Protected Sub btn_CancelBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_CancelBack.Click, btn_Cancel.Click
        Me.MultiView1.ActiveViewIndex = -1
    End Sub

 
    Protected Sub btnShowAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnShowAll.Click
        Me.txtSearch.Text = ""
        Me.GridView1.DataBind()
    End Sub

    'Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click

    'End Sub


#End Region





#Region "清單網站篩選功能區-此段適用於所有要用於多站的AP"


    ''' <summary>
    ''' 初始化網站選擇
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub rbl_Sites_List_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbl_Sites_List.DataBound
        MultiSites.doMultiSite_DataBound(Me, Me.rbl_Sites_List)
    End Sub

    ''' <summary>
    ''' 更新清單
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub rbl_Sites_List_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbl_Sites_List.SelectedIndexChanged
        Me.GridView1.DataBind()
    End Sub

#End Region
End Class
