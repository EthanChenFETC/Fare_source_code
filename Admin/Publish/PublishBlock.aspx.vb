Imports System.Data
Imports System.Data.SqlClient
Partial Class Publish_PublishBlock
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Session("AuditPara") IsNot Nothing Then
                If IsNumeric(Session("AuditPara").replace(",", "")) Then
                    Me.GridView1.DataBind()
                    Dim i As Integer = 0
                    Dim j As Integer = 0
                    Dim find As Boolean = False
                    For j = 0 To Me.GridView1.PageCount - 1
                        For i = 0 To Me.GridView1.Rows.Count - 1
                            If Me.GridView1.DataKeys(i).Value = Session("AuditPara") Then
                                Me.GridView1.SelectedIndex = i
                                find = True
                                Exit For
                            End If
                        Next
                        If find Then
                            Exit For
                        End If
                        If j < Me.GridView1.PageCount - 1 Then
                            Me.GridView1.PageIndex += 1
                            Me.GridView1.DataBind()
                        End If
                    Next
                    Dim AuditPara As String = Session("AuditPara")
                    If Not IsNumeric(AuditPara) Then
                        AuditPara = "0"
                    End If
                    Dim dt As DataTable = ClassDB.RunReturnDataTable("SELECT Content, UpdateTime, BlockName, BlockID, Html FROM PublishBlock WHERE (UID = @UID)", New SqlParameter("@UID", CInt(AuditPara)))
                    Session("AuditPara") = Nothing
                    If dt.Rows.Count > 0 Then
                        Dim IsHtml As Boolean = CType(dt.Rows(0).Item("HTML"), Boolean)
                        Dim BlockName As String = (dt.Rows(0).Item("BlockName").ToString)
                        Dim Contents As String = dt.Rows(0).Item("Content").ToString
                        Contents = RemoveNope(Contents)
                        If IsHtml = True Then
                            Me.MultiView1.ActiveViewIndex = 1
                            Me.FCKeditor3.Value = Contents
                            Me.lbBlockSubject.Text = BlockName
                        Else
                            Me.MultiView1.ActiveViewIndex = 2
                            Me.txt_Content.Text = Contents
                            Me.lbBlockSubject2.Text = BlockName
                        End If
                    End If
                End If
            End If
        End If
    End Sub
#Region "View0-上方區域、新建區塊、搜尋"

    ''' <summary>
    ''' 點選新增區塊
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnNewBlock_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNewBlock.Click
        Me.sds_PublishBlock_List.Insert()
    End Sub

    ''' <summary>
    ''' 資料來源新增完成事件
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub sds_PublishBlock_List_Inserted(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.SqlDataSourceStatusEventArgs) Handles sds_PublishBlock_List.Inserted
        If e.AffectedRows > 0 Then
            Me.lbMessage.Text = "系統訊息：新增區塊成功!"
            WriteAdminLog(Me.Page, "新增區塊-" & RemoveSQLInjection(Me.txtBlockID.Text), "-" & RemoveSQLInjection(Me.txtBlockName.Text))
            Using dr As SqlClient.SqlDataReader = ClassDB.GetDataReader("select max(UID) as UID from PublishBlock")
                Try
                    If dr.Read Then
                        ModuleAuditLog.WriteAuditLog(Me.Page, 1, "新增-新增區塊成功!", "0", CInt(dr("UID")).ToString, e.Command.Parameters("@BlockName").Value.ToString)
                    End If
                Catch ex As Exception
                    ModuleAuditLog.WriteAuditLog(Me.Page, 1, "新增-新增區塊成功!", "1", "", e.Command.Parameters("@BlockName").Value.ToString)
                Finally

                End Try
            End Using
        Else
            ModuleAuditLog.WriteAuditLog(Me.Page, 1, "新增-新增區塊成功!", "1", "", e.Command.Parameters("@BlockName").Value.ToString)
        End If
    End Sub

    ''' <summary>
    ''' 清除搜尋
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btn_Clear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Clear.Click
        Me.txt_Search.Text = ""
        Me.GridView1.DataBind()
    End Sub
#End Region


#Region "View0-GridView1清單"

    Protected Sub GridView1_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.DataBound
        GridViewPageInfo.GetGridViewInfo(Me.GridView1, Me.Page, Me.sds_PublishBlock_List)
    End Sub

    Protected Sub GridView1_RowUpdatingd(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles GridView1.RowUpdating
        Dim editIndex As Integer = e.RowIndex
        Dim KeyID As String = GridView1.Rows(editIndex).Cells(1).Text
        Dim CheckBox1 As CheckBox = CType(Me.GridView1.Rows(editIndex).FindControl("CheckBox1"), CheckBox)
        Dim TextBox1 As TextBox = CType(Me.GridView1.Rows(editIndex).FindControl("TextBox1"), TextBox)
        'Dim txtNotes As TextBox = CType(Me.GridView1.Rows(editIndex).FindControl("txtNotes"), TextBox)
        Me.sds_PublishBlock_List.UpdateParameters("Html").DefaultValue = CheckBox1.Checked
        Me.sds_PublishBlock_List.UpdateParameters("BlockName").DefaultValue = TextBox1.Text
        Me.sds_PublishBlock_List.Update()
    End Sub
    Protected Sub GridView1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView1.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim rv As DataRowView = CType(e.Row.DataItem, DataRowView)
            If e.Row.RowState = 0 OrElse e.Row.RowState = 1 OrElse e.Row.RowState = 2 OrElse e.Row.RowState = 3 Then
                Dim b As LinkButton = CType(e.Row.FindControl("LinkButton1"), LinkButton)
                b.Text = RemoveXSS(rv.Item("BlockName").ToString)
                Dim CheckBox1 As CheckBox = CType(e.Row.FindControl("CheckBox1"), CheckBox)
                CheckBox1.Checked = CType(rv("Html"), Boolean)
            ElseIf e.Row.RowState = 4 OrElse e.Row.RowState = 5 Then
                Dim TextBox1 As TextBox = CType(e.Row.FindControl("TextBox1"), TextBox)
                TextBox1.Text = RemoveXSS(rv("BlockName").ToString)
                Dim CheckBox1 As CheckBox = CType(e.Row.FindControl("CheckBox1"), CheckBox)
                CheckBox1.Checked = CType(rv("Html"), Boolean)
            End If
        End If
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub GridView1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.SelectedIndexChanged
        Dim dt As DataTable = CType(Me.sds_Content.Select(New DataSourceSelectArguments), DataView).Table

        Dim IsHtml As Boolean = CType(dt.Rows(0).Item("HTML"), Boolean)
        If IsHtml = True Then
            Me.MultiView1.ActiveViewIndex = 1
            Me.FCKeditor3.Value = dt.Rows(0).Item("Content").ToString
            Me.lbBlockSubject.Text = RemoveXSS(dt.Rows(0).Item("BlockName").ToString)
        Else
            Me.MultiView1.ActiveViewIndex = 2
            Me.txt_Content.Text = dt.Rows(0).Item("Content").ToString
            Me.lbBlockSubject2.Text = RemoveXSS(dt.Rows(0).Item("BlockName").ToString)
        End If

    End Sub
    ''' <summary>
    ''' 刪除期刊資料列(同時會刪除掉專題SP)-訊息顯示
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub GridView1_RowDeleted(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeletedEventArgs) Handles GridView1.RowDeleted
        If e.AffectedRows > 0 Then
            Dim key As Integer = GridViewPageInfo.GetGridViewRowDeletedDataKey(e)
            ModuleAuditLog.WriteAuditLog(Me.Page, 3, "刪除-區塊成功!", "0", key, "")
        Else
            ModuleAuditLog.WriteAuditLog(Me.Page, 3, "刪除-區塊失敗!", "1", "", "")
        End If
        Me.GridView1.DataBind()
        'Me.UpdatePanel1.Update()
    End Sub

#End Region


#Region "View1-HTML內容編輯"

    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        Me.sds_Content.UpdateParameters("Content").DefaultValue = Me.FCKeditor3.Value
        Me.sds_Content.Update()
    End Sub


    Protected Sub sds_Content_Updated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.SqlDataSourceStatusEventArgs) Handles sds_Content.Updated
        If e.AffectedRows > 0 Then
            Me.lbMessage.Text = "系統訊息：更新成功!"
            WriteAdminLog(Me.Page, "更新首頁區塊-" & Me.GridView1.SelectedRow.Cells(2).Text)
            ModuleAuditLog.WriteAuditLog(Me.Page, 2, "更新-首頁區塊!", "0", Me.GridView1.SelectedValue.ToString, Me.lbBlockSubject.Text)
        Else
            Me.lbMessage.Text = "系統訊息：更新失敗!"
            WriteAdminLog(Me.Page, "更新首頁區塊-" & Me.GridView1.SelectedRow.Cells(2).Text, False)
            ModuleAuditLog.WriteAuditLog(Me.Page, 2, "更新-首頁區塊!", "1", Me.GridView1.SelectedValue.ToString, Me.lbBlockSubject.Text)
        End If
        Me.MultiView1.ActiveViewIndex = 0
    End Sub


    ''' <summary>
    ''' 取消返回清單
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btn_Cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Cancel.Click
        Me.MultiView1.ActiveViewIndex = 0
    End Sub

#End Region


#Region "View2-非html區塊編輯"

    ''' <summary>
    ''' 取消返回清單
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btn_Cancel2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Cancel2.Click
        Me.MultiView1.ActiveViewIndex = 0
    End Sub

    Protected Sub btn_Submit2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Submit2.Click
        Me.sds_Content.UpdateParameters("Content").DefaultValue = RemoveXSS(Me.txt_Content.Text)
        Me.sds_Content.Update()
    End Sub

#End Region
End Class
