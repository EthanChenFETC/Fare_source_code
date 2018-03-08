Imports System.Data
Imports System.Data.SqlClient

Partial Class SystemManager_DepartmentsAdd
    Inherits PageBase



    Protected Sub FormView1_ItemInserted(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.FormViewInsertedEventArgs) Handles FormView1.ItemInserted
        If e.AffectedRows > 0 Then
            Me.lbl_Message.Text = "系統訊息：資料新增完成！"
        Else
            Me.lbl_Message.Text = "系統訊息：資料新增錯誤。"
        End If

    End Sub

    ''' <summary>
    ''' 完成新增單位,取得key
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub SDS_Department_Insert_Inserted(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.SqlDataSourceStatusEventArgs) Handles SDS_Department_Insert.Inserted
        If e.AffectedRows > 0 Then
            '進行群組關聯作業
            Dim command As Common.DbCommand
            command = e.Command
            '取得SqlDataSource_GroupAdd新增後的主鍵
            Dim CreateDepID As Integer = CInt(command.Parameters("@retVal").Value.ToString())
            '副程式進行關聯
            DepartmentRelationAdd(CreateDepID)

            'SqlDS_Group_SetSelectCommand() '設定登入者-擁有的單位編號(依建立結構取得)

        End If
    End Sub

    ''' <summary>
    ''' 新增群組關聯-提供權限繼承功能
    ''' </summary>
    ''' <param name="CreateDepID"></param>
    ''' <remarks></remarks>
    Private Sub DepartmentRelationAdd(ByVal CreateDepID As Integer)
        Dim MyParentDepartments() As String = Split(ModulePermissions.GetMyParentDepartments(Session("DepartmentID").ToString), ",")
        Dim i As Integer
        For i = 0 To UBound(MyParentDepartments)

            Dim value As String = MyParentDepartments(i)
            Dim NeedUpdate As Boolean = False

            'Some ParentUser could be dupcat, blow check after this value non other the same value
            If i = Array.LastIndexOf(MyParentDepartments, value) Then
                NeedUpdate = True
            End If

            If NeedUpdate = True Then
                Try
                    '加入GroupRelation
                    Dim OwnerDepartmentID As String = MyParentDepartments(i)   '取得登入者的Department and Parent Departments
                    Dim UpdateUser As String = CStr(Session("UserID"))

                    '設定插入SDS參數
                    Me.SDS_DepartmentRelation_Insert.InsertParameters("UpdateUser").DefaultValue = UpdateUser  '操作者
                    Me.SDS_DepartmentRelation_Insert.InsertParameters("CreateDepID").DefaultValue = CreateDepID    '被建造者
                    Me.SDS_DepartmentRelation_Insert.InsertParameters("OwnerDepID").DefaultValue = OwnerDepartmentID
                    Me.SDS_DepartmentRelation_Insert.Insert()  '新增一筆關聯紀錄

                Catch ex As Exception
                    WriteErrLog(ex, Me.Page)
                End Try
            End If
        Next
    End Sub

    Protected Sub FormView1_ItemInserting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.FormViewInsertEventArgs) Handles FormView1.ItemInserting
        If Not Page.IsValid Then
            e.Cancel = True
        Else
            Dim DepartmentNameTextBox As TextBox = CType(FormView1.FindControl("DepartmentNameTextBox"), TextBox)
            Dim DepartmentEngNameTextBox As TextBox = CType(FormView1.FindControl("DepartmentEngNameTextBox"), TextBox)
            SDS_Department_Insert.InsertParameters("DepartmentName").DefaultValue = DepartmentNameTextBox.Text
            SDS_Department_Insert.InsertParameters("DepartmentEngName").DefaultValue = DepartmentEngNameTextBox.Text
        End If
    End Sub
End Class
