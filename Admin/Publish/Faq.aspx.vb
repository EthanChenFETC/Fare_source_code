Imports System.Data
Imports System.Data.Sqlclient


Partial Class Publish_Faq
    Inherits PageBase

    Dim ThisNodeID() As Int32
    Dim ThisParentNodeID() As Int32
    Dim ThisSiteNodeID() As Int32
    Protected Sub Page_Load1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '    ModuleMisc.SearchFunction(Me.Literal1, Me.txtSearch, Me.btnSearch)
        'ScriptManager.RegisterClientScriptBlock(Me.UpdatePanel1, Me.UpdatePanel1.GetType, "SetPanelScroll", sScript, True)

        If IsPostBack Then
            If Session("PublishMode") = "BtnMeta" Then
                'Dim index As Integer = CInt(Session("PublisKey"))
                ''Dim UID As Integer = CInt(GridView1.DataKeys(index).Value)  '取得主鍵，轉到編輯頁面
                ''ReadBackContent(Session("PublisKey"))
                ''Me.UpdatePanel1.Update()
                'Dim dr As SqlDataReader = ClassDB.GetDataReader("select siteid, nodeid from sitemap where refpath='faq/default.aspx' and siteid in(select siteid from SitesAprelation where apkeyword = 'faq' and apuid in (SELECT categoryid From faq where  publicID =" & Session("PublisKey") & "))")
                'Dim i As Int16

                'While dr.Read
                '    Array.Resize(ThisNodeID, i + 1)
                '    ThisNodeID(i) = dr("NodeID")
                '    'ThisParentNodeID = dr("ParentNodeID")
                '    Array.Resize(ThisSiteNodeID, i + 1)
                '    ThisSiteNodeID(i) = dr("SiteID")
                '    i = i + 1
                'End While

                'ShowDataType1()
                'ShowDataType2()
                'ShowLang()
                ''抓Meta資料
                ''抓Meta資料
                'ShowMetaData()
                ''分類
                'BuildThemeTreeOld()
                'BuildCakeTreeOld()
                'BuildServiceTreeOld()
            ElseIf Session("PublishMode") Is Nothing Then
                Me.MultiView3.ActiveViewIndex = 0
            End If
        Else
            Me.MultiView3.ActiveViewIndex = 0
            If Session("AuditPara") IsNot Nothing Then
                If IsNumeric(Session("AuditPara").replace(",", "")) Then
                    Me.MultiView1.ActiveViewIndex = 0

                    Me.MultiView3.ActiveViewIndex = -1
                    Me.btn_Update.Visible = True
                    Me.btn_Insert.Visible = False
                    ReadBackContent(Session("AuditPara"))
                    Me.UpdatePanel1.Update()
                    'Me.Page.MaintainScrollPositionOnPostBack = True
                    Session("PublishMode") = "Edit"
                    Dim index As Integer = CInt(Session("AuditPara"))
                    Session("PublisKey") = index
                    'ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "FaqSet", "aspnetForm.action='Faq.aspx#FaqSet'; aspnetForm.submit();", True)
                End If
                Session("AuditPara") = Nothing
                End If
            End If
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.UpdatePanel1.Update()
    End Sub

    Protected Sub btnShowAll_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.txtSearch.Text = ""
        Me.UpdatePanel1.Update()
    End Sub

#Region "GridView1"


    Protected Sub GridView1_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.DataBound
        GridViewPageInfo.GetGridViewInfo(Me.GridView1, Me, Me.sds_Faq_List)
    End Sub

    ''' <summary>
    ''' 刪除資料前，先行刪除附檔資料與實體檔案
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub GridView1_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridView1.RowDeleting
        '取得附檔的ID字串
        Dim HiddenField_AttFiles As HiddenField = CType(Me.GridView1.Rows(e.RowIndex).FindControl("HiddenField_AttFiles"), HiddenField)
        Dim AttFiles As String = HiddenField_AttFiles.Value

        '如有資料呼叫刪除Function
        If AttFiles.Length > 0 Then
            ModuleMisc.AttachFilesDelete(AttFiles, Me)
        End If

    End Sub

    Protected Sub sds_Faq_List_Deleted(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.SqlDataSourceStatusEventArgs) Handles sds_Faq_List.Deleted
        doUpdateList()
        ModuleAuditLog.WriteAuditLog(Me.Page, 3, "刪除-常見問答!", "0", e.Command.Parameters("@PublicID").Value.ToString, "")
    End Sub

#End Region

#Region "新增區"

    ''' <summary>
    ''' 按下新增事件
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub BtnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnAddNew.Click
        Me.MultiView1.ActiveViewIndex = 0
        Me.MultiView3.ActiveViewIndex = -1
        Me.btn_Update.Visible = False
        Me.btn_Insert.Visible = True
        doClearForm()
        'Me.txtPublishDate.Text = Date.Today.ToString("yyyy/MM/dd", System.Globalization.DateTimeFormatInfo.InvariantInfo)
        Me.UpdatePanel1.Update()
    End Sub

    ''' <summary>
    ''' 確定新增
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btn_Insert_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Insert.Click
        Dim PublishExpireDate As Date = Date.Today.AddYears(500).ToString("yyyy/MM/dd", System.Globalization.DateTimeFormatInfo.InvariantInfo)
        If Me.DatePicker2.GetSelectDate <> Nothing Then
            PublishExpireDate = Me.DatePicker2.GetSelectDate
        End If

        Me.sds_Faq_List.InsertParameters("Subject").DefaultValue = RemoveSQLInjection(Me.txtSubject.Text)
        Me.sds_Faq_List.InsertParameters("Content").DefaultValue = Me.FCKeditor3.Value
        Me.sds_Faq_List.InsertParameters("PublishDate").DefaultValue = Me.DatePicker1.GetSelectDate
        Me.sds_Faq_List.InsertParameters("PublishExpireDate").DefaultValue = PublishExpireDate
        Me.sds_Faq_List.InsertParameters("AttFiles").DefaultValue = Me.Uploader1.MultiUploadValue
        Me.sds_Faq_List.Insert()

    End Sub


    Protected Sub sds_Faq_List_Inserted(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.SqlDataSourceStatusEventArgs) Handles sds_Faq_List.Inserted
        If e.AffectedRows > 0 Then
            Me.lbMessage.Text = GridViewPageInfo.Message_Insert_OK(Me, "常見問答")
            ModuleAuditLog.WriteAuditLog(Me.Page, 1, "新增-常見問答!", "0", e.Command.Parameters("@retVal").Value.ToString, e.Command.Parameters("@Subject").Value.ToString)
        Else
            Me.lbMessage.Text = GridViewPageInfo.Message_Insert_False(Me, "常見問答")
            ModuleAuditLog.WriteAuditLog(Me.Page, 1, "新增-常見問答!", "1", "", e.Command.Parameters("@Subject").Value.ToString)
        End If
        Me.MultiView1.ActiveViewIndex = -1
        Me.MultiView3.ActiveViewIndex = 0
        Me.UpdatePanel1.Update()
    End Sub


    Private Sub doClearForm()
        Me.txtSubject.Text = ""
        Me.FCKeditor3.Value = ""
        Me.Uploader1.UploadClear()

        Me.DatePicker1.SelectedDate = Date.Now
        Me.DatePicker2.SelectedDate = DateAdd(DateInterval.Year, 1, Date.Now)
    End Sub



    Protected Sub btn_Cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Cancel.Click
        Me.MultiView1.ActiveViewIndex = -1
        Me.MultiView3.ActiveViewIndex = 0
        Me.UpdatePanel1.Update()
    End Sub



    ''' <summary>
    ''' 更新清單
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub doUpdateList()
        Me.GridView1.DataBind()
        Me.UpdatePanel1.Update()
    End Sub

#End Region

#Region "編輯區"

    Protected Sub GridView1_PageIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.PageIndexChanged
        Me.UpdatePanel1.Update()
    End Sub

    Protected Sub GridView1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView1.RowDataBound
        Dim rv As DataRowView = CType(e.Row.DataItem, DataRowView)
        If e.Row.RowType = DataControlRowType.DataRow Then
            'Dim dr As SqlDataReader = ClassDB.GetDataReader("select siteid, nodeid from sitemap where refpath='faq/faq.aspx' and siteid in(select siteid from SitesAprelation where apkeyword = 'faq' and apuid in (SELECT categoryid From faq where  publicID =" & e.Row.DataItem("PublicID").ToString & "))")
            'Dim i As Int16

            'While dr.Read
            '    Array.Resize(ThisNodeID, i + 1)
            '    ThisNodeID(i) = dr("NodeID")
            '    'ThisParentNodeID = dr("ParentNodeID")
            '    Array.Resize(ThisSiteNodeID, i + 1)
            '    ThisSiteNodeID(i) = dr("SiteID")
            '    i = i + 1
            'End While

            If e.Row.RowState = 0 OrElse e.Row.RowState = 1 OrElse e.Row.RowState = 2 OrElse e.Row.RowState = 3 Then
                'Dim ChkData As Label = CType(e.Row.FindControl("ChkData"), Label)
                'Using dr As SqlDataReader = ClassDB.GetDataReader("select NodeID from FunctionRelationMeta where NodeID = " & ThisNodeID(0) & " and PublicID=" & rv.Item("PublicID") & "")
                '    If dr.Read = False Then
                '        ChkData.Text = "<font color=red>未設定</font>"
                '    Else
                '        ChkData.Text = "<font color=blue>已設定</font>"
                '    End If
                'End Using
                Dim Label1 As Label = CType(e.Row.FindControl("Label1"), Label)
                Label1.Text = CType(rv("PublishExpireDate"), DateTime).ToShortDateString
                Dim Label2 As Label = CType(e.Row.FindControl("Label2"), Label)
                Label2.Text = CType(rv("PostDate"), DateTime).ToShortDateString
                Dim LinkButton1 As LinkButton = CType(e.Row.FindControl("LinkButton1"), LinkButton)
                LinkButton1.CommandArgument = rv("PublicID")
                LinkButton1.Text = (rv("Subject"))
                Dim HiddenField_AttFiles As HiddenField = CType(e.Row.FindControl("HiddenField_AttFiles"), HiddenField)
                HiddenField_AttFiles.Value = IIf(IsDBNull(rv("AttFiles")), "", rv("AttFiles"))
            ElseIf e.Row.RowState = 4 OrElse e.Row.RowState = 5 Then
                Dim TextBox1 As TextBox = CType(e.Row.FindControl("TextBox1"), TextBox)
                TextBox1.Text = (rv("Subject"))
                Dim TextBox2 As TextBox = CType(e.Row.FindControl("TextBox2"), TextBox)
                TextBox1.Text = CType(rv("PostDate"), DateTime).ToShortDateString
                Dim TextBox3 As TextBox = CType(e.Row.FindControl("TextBox3"), TextBox)
                TextBox1.Text = CType(rv("PublishExpireDate"), DateTime).ToShortDateString
            End If
        End If
    End Sub

    Protected Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand
        If e.CommandName = "EditItem" Then
            Me.MultiView1.ActiveViewIndex = 0

            Me.MultiView3.ActiveViewIndex = -1
            Me.btn_Update.Visible = True
            Me.btn_Insert.Visible = False
            ReadBackContent(e.CommandArgument)
            Me.UpdatePanel1.Update()
            'Me.Page.MaintainScrollPositionOnPostBack = True
            Session("PublishMode") = "Edit"
            Dim index As Integer = CInt(e.CommandArgument)
            Session("PublisKey") = index
            'ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "FaqSet", "aspnetForm.action='Faq.aspx#FaqSet'; aspnetForm.submit();", True)
        End If


    End Sub

    'Protected Sub GridView1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.SelectedIndexChanged
    '    Me.MultiView1.ActiveViewIndex = 0
    '    Me.btn_Update.Visible = True
    '    Me.btn_Insert.Visible = False
    '    Dim PublicID As Integer = Me.GridView1.DataKeys(Me.GridView1.SelectedIndex).Value
    '    ReadBackContent(PublicID)
    '    Me.UpdatePanel1.Update()
    'End Sub

    ''' <summary>
    ''' 讀回資料
    ''' </summary>
    ''' <param name="PublicID"></param>
    ''' <remarks></remarks>
    Private Sub ReadBackContent(ByVal PublicID As Integer)
        Dim s As String = Me.sds_ReadBackContent.SelectCommand '.Replace("@PublicID", PublicID)
        Using dr As SqlDataReader = ClassDB.GetDataReaderParam(s, New SqlParameter("@PublicID", PublicID))
            Try
                If dr IsNot Nothing Then
                    While dr.Read
                        Me.rblCatgry.SelectedValue = dr("CateGoryID").ToString
                        Me.txtSubject.Text = (dr("Subject").ToString)
                        Me.FCKeditor3.Value = dr("Content").ToString
                        Me.DatePicker1.SelectedDate = CType(dr("PublishDate").ToString, Date)
                        Me.DatePicker2.SelectedDate = CType(dr("PublishExpireDate").ToString, Date)
                        'Me.DatePicker1.SetSelectDate(CType(dr("PublishDate").ToString, Date))
                        'Me.DatePicker2.SetSelectDate(CType(dr("PublishExpireDate").ToString, Date).ToString("yyyy/MM/dd", System.Globalization.DateTimeFormatInfo.InvariantInfo))
                        Me.ViewTextboxUser1.SetUserID = CInt(dr("ResponUser"))
                        Me.ViewTextboxDep1.SetDepartmentID = CInt(dr("ResponDepartment"))
                        Dim AttFiles As String = ""
                        If Not IsDBNull(dr("AttFiles")) Then
                            AttFiles = dr("AttFiles")
                        End If
                        Me.Uploader1.SetMultiUploadValue(AttFiles)
                    End While
                End If
            Catch ex As Exception
                WriteErrLog(ex, Me)
            Finally

            End Try
        End Using
        Me.ViewState.Add("PublicID", PublicID)
    End Sub

    Protected Sub btn_Update_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Update.Click

        Dim PublishExpireDate As Date = Date.Today.AddYears(500).ToString("yyyy/MM/dd", System.Globalization.DateTimeFormatInfo.InvariantInfo)
        If Me.DatePicker2.GetSelectDate <> Nothing Then
            PublishExpireDate = Me.DatePicker2.GetSelectDate
        End If

        Me.sds_Faq_List.UpdateParameters("PublicID").DefaultValue = Me.ViewState("PublicID")
        Me.sds_Faq_List.UpdateParameters("Subject").DefaultValue = RemoveSQLInjection(Me.txtSubject.Text)
        Me.sds_Faq_List.UpdateParameters("Content").DefaultValue = Me.FCKeditor3.Value
        Me.sds_Faq_List.UpdateParameters("AttFiles").DefaultValue = Me.Uploader1.MultiUploadValue
        Me.sds_Faq_List.UpdateParameters("PublishDate").DefaultValue = Me.DatePicker1.GetSelectDate
        WriteLog("btn_Update_Click", Me.DatePicker1.GetSelectDate)
        Me.sds_Faq_List.UpdateParameters("PublishExpireDate").DefaultValue = PublishExpireDate
        Me.sds_Faq_List.Update()
    End Sub

    Protected Sub sds_Faq_List_Updated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.SqlDataSourceStatusEventArgs) Handles sds_Faq_List.Updated
        If e.AffectedRows > 0 Then
            Me.lbMessage.Text = GridViewPageInfo.Message_Update_OK(Me, "常見問答")
            ModuleAuditLog.WriteAuditLog(Me.Page, 2, "更新-常見問答!", "0", Me.ViewState("PublicID"), e.Command.Parameters("@Subject").Value.ToString)
            Me.ViewState.Remove("PublicID")
        Else
            ModuleAuditLog.WriteAuditLog(Me.Page, 2, "更新-常見問答!", "1", Me.ViewState("PublicID"), e.Command.Parameters("@Subject").Value.ToString)
            'Me.lbMessage.Text = GridViewPageInfo.Message_Update_False(Me, "常見問答")
        End If

        Me.MultiView1.ActiveViewIndex = -1
        Me.MultiView3.ActiveViewIndex = 0
        doUpdateList()
    End Sub
#End Region


    ''' <summary>
    ''' 分類加註網站功能
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub rblCatgry_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles rblCatgry.DataBound

        For i As Integer = 0 To Me.rblCatgry.Items.Count - 1
            Dim l As ListItem = Me.rblCatgry.Items(i)
            'l.Text += MultiSites.GetItemSitesFullName(l.Value, MultiSites.GetNodeKeyword(Me))
            l.Text += MultiSites.GetItemSitesFullName(l.Value, "FaqCatgry")
        Next
    End Sub
End Class
