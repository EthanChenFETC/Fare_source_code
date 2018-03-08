Imports System.Data
Imports System.Data.SqlClient

Partial Class SystemManager_DepartmentsList
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Me.ClearChildControlState()
        End If
    End Sub


    Protected Sub btnShowAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnShowAll.Click
        Me.txtSearch.Text = ""
    End Sub

    Protected Sub GridView1_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.DataBound
        GridViewPageInfo.GetGridViewInfo(GridView1, Me.Page, Me.SqlDS_Departments)

        '排序功能按鈕
        If Me.GridView1.Rows.Count > 0 Then
            Dim btnOrderUp As Button = CType(Me.GridView1.Rows(0).FindControl("btnOrderUp"), Button)
            Dim btnOrderDn As Button = CType(Me.GridView1.Rows(Me.GridView1.Rows.Count - 1).FindControl("btnOrderDn"), Button)
            btnOrderUp.Enabled = False
            btnOrderDn.Enabled = False
        End If
    End Sub

    Protected Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand
        '排序往上
        If e.CommandName = "OrderUp" Then
            '取得主要的key
            Dim key As String = e.CommandArgument 'CInt(Me.GridView1.DataKeys(e.CommandArgument).Value)
            ''取得上一列的Key
            'Dim oKey As Integer = CInt(Me.GridView1.DataKeys(e.CommandArgument - 1).Value)

            '取得由上到下的ID主鍵順序
            Dim i As Integer
            Dim IDs As String = ""
            For i = 0 To Me.GridView1.Rows.Count - 1
                IDs += CInt(Me.GridView1.DataKeys(i).Value) & ","
            Next

            If IDs.Length > 0 Then
                IDs = IDs.Substring(0, IDs.Length - 1)
            End If

            '取得點選列的ID,在總順序中的順位
            Dim IDa() As String = Split(IDs, ",")
            Dim iIndex As Integer = Array.IndexOf(IDa, key)
            Dim UpValue As String = IDa(iIndex - 1)

            '對調順序
            IDa.SetValue(UpValue, iIndex)
            IDa.SetValue(key, iIndex - 1)

            For i = 0 To UBound(IDa)
                Dim sql As String = Me.sds_Order_Update.UpdateCommand
                'sql = sql.Replace("@DepartmentID", IDa(i))
                'sql = sql.Replace("@ItemOrder", i)
                ClassDB.UpdateDBText(sql, New SqlParameter("@DepartmentID", IDa(i)), New SqlParameter("@ItemOrder", i))
            Next

            Me.GridView1.DataBind()
            Me.lblMessage.Text = "系統訊息：排序更新完成！"
        End If

        '排序往下
        If e.CommandName = "OrderDn" Then
            '取得主要的key
            Dim key As String = e.CommandArgument 'CInt(Me.GridView1.DataKeys(e.CommandArgument).Value)

            '取得由上到下的ID主鍵順序
            Dim i As Integer
            Dim IDs As String = ""
            For i = 0 To Me.GridView1.Rows.Count - 1
                IDs += CInt(Me.GridView1.DataKeys(i).Value) & ","
            Next

            If IDs.Length > 0 Then
                IDs = IDs.Substring(0, IDs.Length - 1)
            End If

            '取得點選列的ID,在總順序中的順位
            Dim IDa() As String = Split(IDs, ",")
            Dim iIndex As Integer = Array.IndexOf(IDa, key)
            Dim UpValue As String = IDa(iIndex + 1)

            '對調順序
            IDa.SetValue(UpValue, iIndex)
            IDa.SetValue(key, iIndex + 1)

            For i = 0 To UBound(IDa)
                Dim sql As String = Me.sds_Order_Update.UpdateCommand
                'sql = sql.Replace("@DepartmentID", IDa(i))
                'sql = sql.Replace("@ItemOrder", i)
                ClassDB.UpdateDBText(sql, New SqlParameter("@DepartmentID", IDa(i)), New SqlParameter("@ItemOrder", i))
            Next

            Me.GridView1.DataBind()
            Me.lblMessage.Text = "系統訊息：排序更新完成！"
        End If
    End Sub

    Protected Sub GridView1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView1.RowDataBound

        If Session("Administrator") Is Nothing Then

            If e.Row.RowType = DataControlRowType.DataRow Then
                'Turn Off the Permission to Edit the Department Self
                If e.Row.RowState = 0 Or e.Row.RowState = 1 Or e.Row.RowState = 2 Or e.Row.RowState = 3 Then
                    Dim RowIndex As Integer = e.Row.RowIndex
                    Dim DataKeys As String = Me.GridView1.DataKeys(RowIndex).Value().ToString

                    If DataKeys.Equals(CStr(Session("DepartmentID"))) Then
                        Dim btn1 As Button = CType(e.Row.FindControl("BtnEdit"), Button)
                        Dim btn2 As Button = CType(e.Row.FindControl("BtnDel"), Button)
                        btn1.Enabled = False
                        btn2.Enabled = False
                    End If
                End If

                ''關閉非管理者時,單位變更到民意信箱
                'If e.Row.RowState = 4 Or e.Row.RowState = 5 Then    'In the Edit Mode in Normal & Alt Rows
                '    Try
                '        Dim DropDownList1 As DropDownList = CType(e.Row.FindControl("DropDownList1"), DropDownList)
                '        If Session("MailBoxID") IsNot Nothing Then
                '            DropDownList1.SelectedValue = Session("MailBoxID").ToString
                '        Else
                '            DropDownList1.SelectedIndex = 0
                '        End If

                '        DropDownList1.Enabled = False   'Disable to Change!!
                '    Catch ex As Exception
                '        WriteErrLog(ex, Me.Page)
                '    End Try
                'End If

                '排序按鈕(繫結RowIndex)
                Dim btnOrderUp As Button = CType(e.Row.FindControl("btnOrderUp"), Button)
                Dim btnOrderDn As Button = CType(e.Row.FindControl("btnOrderDn"), Button)
                btnOrderUp.CommandArgument = e.Row.RowIndex
                btnOrderDn.CommandArgument = e.Row.RowIndex

            End If
        End If

    End Sub

    Protected Sub GridView1_RowDeleted(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeletedEventArgs) Handles GridView1.RowDeleted
        If e.AffectedRows > 0 Then

            Dim TotalValues As Integer = e.Values.Count
            Dim i As Integer

            Dim DeleteVales As String = ""
            For i = 0 To TotalValues - 1
                DeleteVales += e.Values.Item(i) & ","
            Next
            If DeleteVales.Length > 5 Then
                Me.lblMessage.Text = GridViewPageInfo.Message_Delete_OK(Me.Page, "單位資料-" & DeleteVales.Substring(0, 5) & "...")
            Else
                Me.lblMessage.Text = GridViewPageInfo.Message_Delete_OK(Me.Page, "單位資料-" & DeleteVales)
            End If

            Dim Datakey As Integer = GridViewPageInfo.GetGridViewRowDeletedDataKey(e)


            ClassDB.UpdateDB("Net2_Department_Delete", New SqlParameter("@DepartmentID", Datakey))  'Kill All the Relation Tables have this Department


        Else
            Me.lblMessage.Text = GridViewPageInfo.Message_Delete_False(Me.Page, "單位資料")

        End If
    End Sub


    Protected Sub GridView1_RowUpdated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdatedEventArgs) Handles GridView1.RowUpdated
        If e.AffectedRows > 0 Then
            Dim NewValuesCount As Integer = e.NewValues.Count
            Dim i As Integer

            Dim NewValues As String = ""
            For i = 0 To NewValuesCount - 1
                NewValues += e.NewValues.Item(i) & ","
            Next

            Dim Oldvalues As String = ""
            For i = 0 To NewValuesCount - 1
                Oldvalues += e.OldValues.Item(i) & ","
            Next
            '20091217 修改長度錯誤
            If Oldvalues.Length > 5 Then
                Me.lblMessage.Text = GridViewPageInfo.Message_Update_OK(Me.Page, "單位資料-" & Oldvalues.Substring(0, 5) & "...")
            Else
                Me.lblMessage.Text = GridViewPageInfo.Message_Update_OK(Me.Page, "單位資料-" & Oldvalues)
            End If

        Else
            Me.lblMessage.Text = GridViewPageInfo.Message_Update_False(Me.Page, "單位資料")
        End If

    End Sub



    ''' <summary>
    ''' 重設定資料列Select Command
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub SqlDS_Departments_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles SqlDS_Departments.Load
        SqlDS_Departments_SetSelectCommand()
    End Sub

    ''' <summary>
    ''' 副程式,當Gridview要DataBind前或剛新增完成一筆資料，取得使用者的群組擁有的群組權限
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SqlDS_Departments_SetSelectCommand()
        If Session("Administrator") = True Then
            Me.SqlDS_Departments.SelectParameters("DepartmentID").DefaultValue = 0
        Else
            Me.SqlDS_Departments.SelectParameters("DepartmentID").DefaultValue = CInt(Session("DepartmentID").ToString)
        End If

    End Sub


    ''' <summary>
    ''' 顯示單位排序-CheckBox
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub cbDoItemOrder_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If Me.cbDoItemOrder.Checked = True Then
            Me.GridView1.AllowPaging = False
            Me.GridView1.Columns(2).Visible = True
        Else
            Me.GridView1.AllowPaging = True
            Me.GridView1.Columns(2).Visible = False
        End If
    End Sub
End Class
