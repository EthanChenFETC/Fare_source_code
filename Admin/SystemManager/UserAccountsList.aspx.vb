Imports System.Data
Imports System.Data.SqlClient

Partial Class SystemManager_UserAccountsList
    Inherits PageBase


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'GridView1_SDS_ReSetSelectCommand()
    End Sub

#Region "GridView1"

    '設定分頁資訊
    Protected Sub GridView1_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.DataBound
        GridViewPageInfo.GetGridViewInfo(GridView1, Me.Page, Me.SqlDS_UserAccounts)
    End Sub

    Protected Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand
        If e.CommandName = "SetDel" Then
            '2007/12/6 Remove This and Add Below(sql), It's because IsOnline is Set to Offline, No need to SetDel=1
            'ClassDB.UpdateDB("Net2_Accounts_SetToDel", New SqlParameter("@UserID", e.CommandArgument))  

            '2007/12/6 Real Del
            Dim sql As String = "DELETE FROM Accounts_Users WHERE UserID = @UserID"
            ClassDB.UpdateDBText(sql, New SqlParameter("@UserID", CInt(e.CommandArgument)))

            '刪除原來User擁有的GroupID
            Dim retVal As Integer = ClassDB.RunSPReturnInteger("Net2_Accounts_GroupUser_DeleteByUser", New SqlParameter("@UserID", e.CommandArgument))
            Me.lbMessage.Text = GridViewPageInfo.Message_Delete_OK(Me.Page, "人員資料")
            Me.GridView1.DataBind()
            '刪除後台首頁個人化
            Me.sds_HomePageBlock.DeleteParameters("UserID").DefaultValue = e.CommandArgument
            Me.sds_HomePageBlock.Delete()

        End If

        '編輯使用者詳細資料
        If e.CommandName = "EditUser" Then
            Me.sds_UserDetail.SelectParameters("UserID").DefaultValue = CInt(e.CommandArgument)
            Me.ViewState.Add("UserID", CInt(e.CommandArgument))
            Me.MultiView1.ActiveViewIndex = 1
            'Me.DetailsView1.DefaultMode = DetailsViewMode.Edit
            Me.DetailsView1.ChangeMode(DetailsViewMode.Edit)
            Me.DetailsView1.DataBind()
        End If

    End Sub

    Protected Sub GridView1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView1.RowDataBound
        '設定篩選為資料列
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim drv As DataRowView = CType(e.Row.DataItem, DataRowView)
            '一般模式
            If e.Row.RowState = 0 Or e.Row.RowState = 1 Or e.Row.RowState = 2 Or e.Row.RowState = 3 Then
                Dim DataKeys As String = drv("UserID").ToString

                '權限群組的CheckBoxList
                Dim CheckBoxList1 As CheckBoxList = CType(e.Row.FindControl("CheckBoxList1"), CheckBoxList)
                BindCheckBoxList(CheckBoxList1, DataKeys)

                If Session("Administrator") Is Nothing And Session("SuperAdmin") Is Nothing Then
                    '非Administrator群組，自己的資料無法編輯
                    If DataKeys.Equals(CStr(Session("UserID"))) Then
                        CType(e.Row.FindControl("BtnEdit"), Button).Enabled = False
                        CType(e.Row.FindControl("BtnDel"), Button).Enabled = False
                        CType(e.Row.FindControl("lbtn_FullName"), LinkButton).Enabled = False
                    End If
                End If
                CType(e.Row.FindControl("BtnDel"), Button).CommandArgument = DataKeys
                CType(e.Row.FindControl("lbtn_FullName"), LinkButton).CommandArgument = DataKeys
                CType(e.Row.FindControl("lbtn_FullName"), LinkButton).Text = (drv("Name").ToString)
                CType(e.Row.FindControl("lbEngName"), Label).Text = (drv("EngName").ToString)
                CType(e.Row.FindControl("lbUserName"), Label).Text = (drv("UserName"))
                CType(e.Row.FindControl("lbDepartmentName"), Label).Text = (drv("DepartmentName").ToString)
                CType(e.Row.FindControl("lbTitle"), Label).Text = (drv("Title").ToString)
                CType(e.Row.FindControl("Label1"), Label).Text = (drv("TelNo").ToString)
                CType(e.Row.FindControl("Label2"), Label).Text = (drv("FaxNo").ToString)
                CType(e.Row.FindControl("Label6"), Label).Text = (drv("PermissionName").ToString)
            End If

            '''編輯模式
            'If e.Row.RowState = 4 Or e.Row.RowState = 5 Then    '(4+0=4,4+1=5)
            '    Dim RowIndex As Integer = e.Row.RowIndex
            '    Dim DataKeys As String = Me.GridView1.DataKeys(RowIndex).Value().ToString
            '    CType(e.Row.FindControl("txtName"), TextBox).Text = drv("Name")
            '    CType(e.Row.FindControl("txtEngName"), TextBox).Text = drv("EngName")
            '    CType(e.Row.FindControl("txtUserName"), TextBox).Text = drv("UserName")
            '    CType(e.Row.FindControl("TextBox1"), TextBox).Text = drv("Title")
            '    CType(e.Row.FindControl("TextBox2"), TextBox).Text = drv("TelNo")
            '    CType(e.Row.FindControl("txtTelExt"), TextBox).Text = drv("TelNo_Ext")
            '    CType(e.Row.FindControl("TextBox3"), TextBox).Text = drv("FaxNo")
            '    Dim ddlDepartment As DropDownList = CType(e.Row.FindControl("ddlDepartment"), DropDownList)
            '    ddlDepartment.DataBind()
            '    ddlDepartment.SelectedValue = drv("DepartmentID")

            'End If
        End If

    End Sub

    ''' <summary>
    ''' Bind在檢視狀態下的網站權限 CheckBoxList
    ''' </summary>
    ''' <param name="CheckBoxList1"></param>
    ''' <param name="DataKeys"></param>
    ''' <remarks></remarks>
    Private Sub BindCheckBoxList(ByVal CheckBoxList1 As CheckBoxList, ByVal DataKeys As String)

        '取得此資料列使用者所擁有的群組權限
        CheckBoxList1.Items.Clear()
        Using dr As SqlDataReader = ClassDB.GetDataReaderSP("Net2_Accounts_GroupUsers_GetGroupData", New SqlParameter("@UserID", CInt(DataKeys)))
            Try
                If dr IsNot Nothing Then
                    While dr.Read
                        '因為是複數群組，所以無法在ASPX中設定，在此把此使用者的權限列出(其餘不列)，並勾選。
                        CheckBoxList1.Items.Add(New ListItem((dr("GroupName")), CInt(dr("GroupID"))))
                        CheckBoxList1.Items(CheckBoxList1.Items.Count - 1).Selected = True
                    End While
                End If
            Catch ex As Exception
                WriteErrLog(ex, Me.Page)
            Finally
            End Try
        End Using
    End Sub

    ''' <summary>
    ''' 設定在編輯狀態下的網站權限CheckBoxlist
    ''' </summary>
    ''' <param name="CheckBoxList1"></param>
    ''' <param name="DataKeys"></param>
    ''' <remarks></remarks>
    Private Sub BindCheckBoxListEdit(ByVal CheckBoxList1 As CheckBoxList, ByVal DataKeys As String)

        '取得登入者，所擁有的群組清單
        Dim MyOwnGroups As String = ModulePermissions.GetMyOwnerGroups(Session("GroupID"))
        'Dim groupids() As String = MyOwnGroups.Split(",")
        'Dim sqlp(groupids.Length) As SqlParameter
        'For i As Integer = 0 To groupids.Length - 1
        '    Dim val As Integer = CInt(groupids(i)).ToString
        '    sqlp(i) = New SqlParameter("@GroupID", val)
        'Next
        Dim sql_MyOwnGroups As String = "SELECT GroupID, GroupName FROM Accounts_Group WHERE GroupID in (" & MyOwnGroups & ")"

        '系統管理員可以打開全部
        If Session("Administrator") IsNot Nothing Or Session("SuperAdmin") IsNot Nothing Then
            sql_MyOwnGroups = "SELECT GroupID, GroupName FROM Accounts_Group"
            'sqlp = Nothing
        End If

        CheckBoxList1.Items.Clear()
        Using dr As SqlDataReader = ClassDB.GetDataReaderParam(sql_MyOwnGroups)
            Try
                If dr IsNot Nothing Then
                    If dr IsNot Nothing Then
                        While dr.Read
                            Dim GroupName As String = dr("GroupName")

                            Dim GroupID As Integer = IIf(IsNumeric(dr("GroupID")), dr("GroupID"), 0)
                            CheckBoxList1.Items.Add(New ListItem(GroupName, GroupID.ToString))
                        End While
                    End If
                End If
            Catch ex As Exception
                WriteErrLog(ex, Me.Page)
            Finally

            End Try
        End Using

        '2.取得本列使用者所擁有的群組權限
        Using dr2 As SqlDataReader = ClassDB.GetDataReaderSP("Net2_Accounts_GroupUsers_GetGroupData", New SqlParameter("@UserID", CInt(DataKeys)))
            Try
                If dr2 IsNot Nothing Then
                    While dr2.Read
                        For i As Integer = 0 To CheckBoxList1.Items.Count - 1
                            Dim cb As ListItem = CheckBoxList1.Items(i)
                            If cb.Value = CStr(dr2("GroupID")) Then
                                cb.Selected = True
                            End If
                        Next
                    End While
                End If
            Catch ex As Exception
                WriteErrLog(ex, Me.Page)
            Finally

            End Try
        End Using
    End Sub


    ''' <summary>
    ''' 新增時取得權限群組（限登入者之權限，如為系統管理員會全開）
    ''' </summary>
    ''' <param name="CheckBoxList1"></param>
    ''' <remarks></remarks>
    Private Sub BindCheckBoxListInsert(ByVal CheckBoxList1 As CheckBoxList)

        '取得登入者，所擁有的群組清單
        Dim MyOwnGroups As String = ModulePermissions.GetMyOwnerGroups(Session("GroupID"))
        Dim sql_MyOwnGroups As String = "SELECT GroupID, GroupName FROM Accounts_Group WHERE @GroupID = '' or GroupID in(@GroupID)".Replace("@GroupID", MyOwnGroups)

        '系統管理員可以打開全部
        If Session("Administrator") = True Or Session("SuperAdmin") = True Then
            MyOwnGroups = ""
            'If Session("Administrator") = True Then
            sql_MyOwnGroups = "SELECT GroupID, GroupName FROM Accounts_Group"
            'End If
        End If

        CheckBoxList1.Items.Clear()
        Using dr As SqlDataReader = ClassDB.GetDataReaderParam(sql_MyOwnGroups)
            Try
                If dr IsNot Nothing Then
                    While dr.Read
                        Dim GroupName As String = dr("GroupName")

                        Dim GroupID As Integer = IIf(IsNumeric(dr("GroupID")), dr("GroupID"), 0)
                        CheckBoxList1.Items.Add(New ListItem(GroupName, GroupID.ToString))
                    End While
                End If
            Catch ex As Exception
                WriteErrLog(ex, Me.Page)
            Finally

            End Try
        End Using
    End Sub

    ''' <summary>
    ''' Department DropDownList Binding,It's cause some User doesn't hvae a department
    ''' </summary>
    ''' <param name="ddl"></param>
    ''' <param name="DataKey"></param>
    ''' <remarks></remarks>
    Private Sub BindDepartmentDDL(ByVal ddl As DropDownList, ByVal DataKey As String)
        ddl.Items.Clear()
        Dim sql As String = "SELECT [DepartmentID], [DepartmentName] FROM [Department]  WHERE  @MyOwnDepartment = '' or DepartmentID IN (@MyOwnDepartment2) order by ItemOrder"
        Dim MyOwnDepartment As String = ""
        If Session("Administrator") Is Nothing And Session("SuperAdmin") Is Nothing Then
            MyOwnDepartment = ModulePermissions.GetMyOwnerDepartments(Session("DepartmentID"))
            'sql += " WHERE DepartmentID IN (" & MyOwnDepartment & ") order by ItemOrder"
        Else
            'ddl.Items.Add(New ListItem("選擇部門", "0"))
            sql = "SELECT [DepartmentID], [DepartmentName] FROM [Department] order by ItemOrder"
        End If

        'Using dr As SqlDataReader = ClassDB.GetDataReaderParam(sql.Replace("@MyOwnDepartment", "'" & MyOwnDepartment & "'"), New SqlParameter("@MyOwnDepartment", MyOwnDepartment))
        Using dr As SqlDataReader = ClassDB.GetDataReaderParam(sql.Replace("@MyOwnDepartment2", MyOwnDepartment))
            Try
                If dr IsNot Nothing Then
                    While dr.Read
                        Dim DepartmentID As Integer = IIf(IsNumeric(dr("DepartmentID")), dr("DepartmentID"), 0)
                        Dim DepartmentName As String = dr("DepartmentName")

                        ddl.Items.Add(New ListItem(DepartmentName.ToString, DepartmentID.ToString))
                    End While
                End If
            Catch ex As Exception
                WriteErrLog(ex, Me.Page)
            Finally

            End Try
        End Using
        Try
            ddl.SelectedValue = DataKey
        Catch ex As Exception
            ddl.SelectedIndex = 0
        End Try
    End Sub
    ''' <summary>
    ''' Department DropDownList Binding,It's cause some User doesn't hvae a department
    ''' </summary>
    ''' <param name="ddl"></param>
    ''' <param name="DataKey"></param>
    ''' <remarks></remarks>
    Private Sub BindSectionDDL(ByVal ddl As DropDownList, ByVal DataKey As String)

        ddl.Items.Clear()
        Dim sql As String = "SELECT [SectionID], [SectionName] FROM [Section] WHERE (@DepartmentID = '' or DepartmentID IN (@DepartmentID)) "
        Dim DepartmentID As String = ""
        Dim SectionID As String = ""
        If Session("Administrator") IsNot Nothing Or Session("SuperAdmin") IsNot Nothing Then
            'ddl.Items.Add(New ListItem("選擇科別", "0"))
            DepartmentID = DataKey.ToString
            sql = "SELECT [SectionID], [SectionName] FROM [Section] WHERE DepartmentID IN (@DepartmentID) order by SectionID"
        Else
            Using drr As SqlDataReader = ClassDB.GetDataReaderParam("Select SectionID from Accounts_Users where UserID = @UserID", New SqlParameter("@UserID", Session("UserID").ToString))
                Try
                    If drr IsNot Nothing Then
                        If drr.Read Then
                            If drr("SectionID") <> 0 Then
                                sql = "SELECT [SectionID], [SectionName] FROM [Section] WHERE SectionID = @SectionID"
                                SectionID = CInt(drr("SectionID")).ToString
                            Else
                                sql = "SELECT [SectionID], [SectionName] FROM [Section] WHERE DepartmentID IN (@DepartmentID) order by SectionID"
                                DepartmentID = DataKey.ToString
                            End If
                        End If
                    End If
                Catch ex As Exception
                Finally

                End Try
            End Using
        End If
        Using dr As SqlDataReader = ClassDB.GetDataReaderParam(sql.Replace("@DepartmentID", DepartmentID).Replace("@SectionID", SectionID))
            Try
                If dr IsNot Nothing Then
                    While dr.Read
                        Dim SectionName As String = dr("SectionName")

                        Dim sectionIDs As Integer = dr("SectionID")
                        If IsNumeric(sectionIDs) Then
                            sectionIDs = CInt(sectionIDs).ToString
                        Else
                            sectionIDs = "0"
                        End If

                        ddl.Items.Add(New ListItem(SectionName, SectionID))
                    End While
                End If
            Catch ex As Exception
                WriteErrLog(ex, Me.Page)
            Finally

            End Try
        End Using
        Try
            ddl.SelectedValue = DataKey
        Catch ex As Exception
            ddl.SelectedIndex = 0
        End Try
    End Sub

#End Region


    ''' <summary>
    ''' 人員資料列，資料刪除(後)-完成
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub GridView1_RowDeleted(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeletedEventArgs) Handles GridView1.RowDeleted
        Dim TotalValues As Integer = e.Values.Count
        Dim i As Integer

        Dim DeleteVales As String = ""
        For i = 0 To TotalValues - 1
            DeleteVales += e.Values.Item(i) & ","
        Next
        'WriteAdmiLog(Me.Page, "刪除人員資料-『" & DeleteVales & "』")
        'Me.lblMessage.Text = " 系統訊息：資料已刪除！"
        'If e.AffectedRows > 0 Then

        '    Dim Datakey As Integer = GridViewPageInfo.GetGridViewRowDeletedDataKey(e)

        '    
        '    '刪除原來User擁有的GroupID
        '    Dim retVal As Integer = ClassDB.RunSPReturnInteger("Net2_Accounts_GroupUser_DeleteByUser", New SqlParameter("@UserID", Datakey))

        '   

        '    Me.lblMessage.Text = GridViewPageInfo.Message_Delete_OK(Me.Page, "人員資料" & DeleteVales.Substring(0, 5) & "...")
        'Else
        '    Me.lblMessage.Text = GridViewPageInfo.Message_Delete_False(Me.Page, "人員資料" & DeleteVales.Substring(0, 5) & "...")
        'End If
    End Sub

    ''' <summary>
    ''' 人員資料列，資料更新(後)-完成
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub GridView1_RowUpdated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdatedEventArgs) Handles GridView1.RowUpdated
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

        If e.AffectedRows > 0 Then

            Me.lbMessage.Text = GridViewPageInfo.Message_Update_OK(Me.Page, "人員資料" & Oldvalues.Substring(0, 5) & "...")
        Else
            Me.lbMessage.Text = GridViewPageInfo.Message_Update_False(Me.Page, "人員資料" & Oldvalues.Substring(0, 5) & "...")
        End If

    End Sub


    ''' <summary>
    ''' 人員資料列，資料更新(前)
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub GridView1_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles GridView1.RowUpdating
        ''密碼更新處理---------------------------------------------------------------------------------------------------------------------
        Dim i As Integer = e.RowIndex
        Dim GridViewRow1 As GridViewRow = Me.GridView1.Rows(i)

        Dim txtPW As TextBox = CType(GridViewRow1.Cells(3).FindControl("txtP"), TextBox)

        'WriteLog("GridViewRow1-txtPW.Text", txtPW.Text)    '成功捉到Password值，再來要作處理
        If Not txtPW.Text = String.Empty Then
            '沒有輸入密碼
            If PasswordStrength.doPasswordCheck(txtPW.Text) = False Then
                Me.lbMessage.Text = "您的密碼強度不夠,請填足8碼包含英數字,謝謝!"
                e.Cancel = True
            End If

            Dim SHA1Password As String = FormsAuthentication.HashPasswordForStoringInConfigFile(txtPW.Text, "SHA1")
            Me.SqlDS_UserAccounts.UpdateParameters.Add("Password", SHA1Password)    'Aspx沒有這組參數，這裡要用Add方法。
        Else
            '有輸入密碼
            Me.SqlDS_UserAccounts.UpdateParameters.Add("Password", String.Empty)  '實際傳入txtPW是空值，預存會判斷並不會更新此欄位
        End If



        '權限群組更新處理---------------------------------------------------------------------------------------------------------------------

        '取得本資料列的DataKey
        Dim DataKey As String = GridViewPageInfo.GetGridViewRowUpdatingDataKey(e)

        '先刪除原來User擁有的GroupID
        Dim retVal As Integer = ClassDB.RunSPReturnInteger("Net2_Accounts_GroupUser_DeleteByUser", New SqlParameter("@UserID", DataKey))

        '取得CheckBoxList資料
        Dim GridViewRow As GridViewRow = Me.GridView1.Rows(e.RowIndex)
        Dim CheckBoxList1 As CheckBoxList = CType(GridViewRow.FindControl("CheckBoxList2"), CheckBoxList)

        '重新寫入新的網站權限設定

        For j As Integer = 0 To CheckBoxList1.Items.Count - 1
            Dim cb As ListItem = CheckBoxList1.Items(j)
            If cb.Selected = True Then
                '寫入資料表
                ClassDB.UpdateDB("Net2_Accounts_GroupUser_Add",
                New SqlParameter("@UpdateUser", Session("UserID")),
                New SqlParameter("@GroupID", CInt(cb.Value)),
                New SqlParameter("@UserID ", CInt(DataKey)))
            End If
        Next

        'DepartmentID UpdateParameters
        Dim ddlDepartment As DropDownList = CType(GridViewRow1.Cells(4).FindControl("ddlDepartment"), DropDownList)
        Me.SqlDS_UserAccounts.UpdateParameters("DepartmentID").DefaultValue = ddlDepartment.SelectedValue

    End Sub



#Region "重設人員清單來源Select Command 及參數"


    Protected Sub SqlDS_UserAccounts_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles SqlDS_UserAccounts.Load
        GridView1_SDS_ReSetSelectCommand()
    End Sub


    ''' <summary>
    ''' 重新定義使用者列表SelectCommand陳述式
    ''' Confirm 2007-7-19
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GridView1_SDS_ReSetSelectCommand()
        Dim Addtion_SelectCommand As String = ""
        Dim Param As String = ""

        If Session("Administrator") = True Or Session("SuperAdmin") = True Then
            '網站管理員 Administrator
            Me.SqlDS_UserAccounts.SelectParameters("UserID").DefaultValue = 0
        Else

            '一般使用者群組(限制自己建立及其所建立的人員名單)
            '預存中自動取得OwnerID=UserID的資料＆自己的UserID
            Me.SqlDS_UserAccounts.SelectParameters("UserID").DefaultValue = CInt(Session("UserID").ToString)
        End If
    End Sub

#End Region



#Region "上方功能區"


    ''' <summary>
    ''' 切換至新增帳號的View
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btn_NewUser_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_NewUser.Click
        Me.MultiView1.ActiveViewIndex = 1

        Me.DetailsView1.ChangeMode(DetailsViewMode.Insert)

        Me.DetailsView1.DataBind()
        '系統管理員可以打開全部
        'Department Binding
        Dim ddlDepartment As DropDownList = CType(Me.DetailsView1.FindControl("ddl_Dep_List"), DropDownList)
        BindDepartmentDDL(ddlDepartment, 0)
        Dim ddl_Section As DropDownList = CType(Me.DetailsView1.FindControl("ddl_Section"), DropDownList)
        Dim ddls As Integer = IIf(IsNumeric(ddlDepartment.SelectedValue.ToString), ddlDepartment.SelectedValue.ToString, 0)
        BindSectionDDL(ddl_Section, ddls)


        Dim txt_P As TextBox = CType(Me.DetailsView1.FindControl("txt_P"), TextBox)
        txt_P.Text = ""
        Dim TextBox1 As TextBox = CType(Me.DetailsView1.FindControl("TextBox1"), TextBox)
        TextBox1.Text = ""

        Dim TextBox4 As TextBox = CType(Me.DetailsView1.FindControl("TextBox4"), TextBox)
        TextBox4.Text = ""


    End Sub

    ''' <summary>
    ''' 清除搜尋關鍵字，並重新繫結清單
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btn_Clear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Clear.Click
        Me.txt_Search.Text = ""
    End Sub

#End Region



#Region "部門／單位清單Select Command & 參數"

    ''' <summary>
    ''' 限制其單位擁有權
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub SqlDS_DepartmentList_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles SqlDS_DepartmentList.Load
        Dim sql As String = ""

        If Session("Administrator") = True Or Session("SuperAdmin") = True Then
            sql = "SELECT [DepartmentID], [DepartmentName] FROM [Department]"
        Else
            Dim MyOwnDepartment As String = ModulePermissions.GetMyOwnerDepartments(Session("DepartmentID"))
            sql = "SELECT [DepartmentID], [DepartmentName] FROM [Department] WHERE Accounts_Users.DepartmentID IN (" & MyOwnDepartment & ")"
        End If

        Me.SqlDS_DepartmentList.SelectCommand = sql
        'Dim DepartmentID As String = Session("DepartmentID")
        'If IsNumeric(DepartmentID) Then
        '    Me.SqlDS_DepartmentList.SelectParameters("DepartmentID").DefaultValue = DepartmentID
        'End If

    End Sub

#End Region


#Region "編輯使用者內容"


    'Protected Sub btn_Submit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Submit.Click
    '    If Me.DetailsView1.DefaultMode = DetailsViewMode.Edit Then
    '        me.DetailsView1.
    '    End If
    'End Sub

    ''' <summary>
    ''' 編輯／新增資料繫結
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub DetailsView1_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles DetailsView1.DataBound

        '共有的物件宣告(DetailView允許在不同Modue的Template中有相同ControlID的物件)
        '-------------------------------------------------------------------------------------------------------------------------------
        '科別下拉選單
        Dim ddl_Department As DropDownList = CType(Me.DetailsView1.Rows(0).FindControl("ddl_Dep_List"), DropDownList)
        '科別下拉選單
        Dim ddl_Section As DropDownList = CType(Me.DetailsView1.Rows(0).FindControl("ddl_Section"), DropDownList)
        '權限群組的CheckBoxList
        Dim cbl_Permissions As CheckBoxList = CType(Me.DetailsView1.Rows(0).FindControl("cbl_Permissions"), CheckBoxList)

        '編輯模式
        If Me.DetailsView1.CurrentMode = DetailsViewMode.Edit Then

            Dim dv As DataRowView = CType(Me.DetailsView1.DataItem, DataRowView)
            If Not dv Is Nothing Then
                Dim UserID As Integer = dv.Item("UserID").ToString
                BindDepartmentDDL(ddl_Department, dv.Item("DepartmentID").ToString)
                Me.sds_Section.SelectParameters("DepartmentID").DefaultValue = dv.Item("DepartmentID").ToString
                ReBindSection()

                '科別下拉選單處理（如為Null將會有錯誤的問題）
                Try
                    If Not IsDBNull(dv.Item("SectionID")) Then
                        ddl_Section.SelectedValue = dv.Item("SectionID")
                    End If
                Catch ex As Exception
                End Try

                '繫結權限群組清單、及該User的權限CheckBoxList勾選
                BindCheckBoxListEdit(cbl_Permissions, UserID)


                '密碼處理(先放在ViewStats)
                Me.ViewState.Add("pwt", dv.Item("Password"))
                Me.ViewState.Add("UserID", UserID)
            End If

            Dim txt_P As TextBox = CType(Me.DetailsView1.FindControl("txt_P"), TextBox)
            txt_P.Text = ""
            Dim TextBox1 As TextBox = CType(Me.DetailsView1.FindControl("TextBox1"), TextBox)
            TextBox1.Text = IIf(IsDBNull(dv.Item("TelNo")), "", dv.Item("TelNo"))

            Dim TextBox4 As TextBox = CType(Me.DetailsView1.FindControl("TextBox4"), TextBox)
            TextBox4.Text = IIf(IsDBNull(dv.Item("TelNo_Ext")), "", dv.Item("TelNo_Ext"))

        End If


        '新增模式
        If Me.DetailsView1.CurrentMode = DetailsViewMode.Insert Then

            '繫結權限群組清單
            BindCheckBoxListInsert(cbl_Permissions)

        End If
    End Sub

    ''' <summary>
    ''' 人員編輯／新增-取消返回清單
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub DetailsView1_ItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DetailsViewCommandEventArgs) Handles DetailsView1.ItemCommand
        If e.CommandName = "Cancel" Then
            Me.DetailsView1.ChangeMode(DetailsViewMode.ReadOnly)
            'Me.DetailsView1.DataBind()
            Me.MultiView1.ActiveViewIndex = 0
        End If
        If e.CommandName = "CancelEdit" Then
            Me.DetailsView1.ChangeMode(DetailsViewMode.ReadOnly)
            Me.DetailsView1.DataBind()
            Me.MultiView1.ActiveViewIndex = 0
        End If
    End Sub


    ''' <summary>
    ''' 資料來源-新增使用者完成事件
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>在此新增使者的權限群組</remarks>
    Protected Sub DetailsView1_ItemInserted(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DetailsViewInsertedEventArgs) Handles DetailsView1.ItemInserted
        If e.AffectedRows > 0 Then
            Dim UserID As Integer
            Dim sql As String = "SELECT MAX(UserID) AS UserID FROM Accounts_Users"
            Using dr As SqlDataReader = ClassDB.GetDataReader(sql)
                Try
                    If dr IsNot Nothing Then
                        If dr.Read Then
                            UserID = dr("UserID")
                        End If
                    End If
                Catch ex As Exception
                    WriteErrLog(ex, Me.Page)
                Finally

                End Try
            End Using
            ''增加使用者的權限群組
            'Dim cbl_Permissions As CheckBoxList = CType(Me.DetailsView1.Rows(0).FindControl("cbl_Permissions"), CheckBoxList) '權限群組的CheckBoxList
            'Dim l As ListItem
            'For Each l In cbl_Permissions.Items
            '    If l.Selected = True Then
            '        Me.sds_GroupUser.InsertParameters("GroupID").DefaultValue = l.Value
            '        Me.sds_GroupUser.InsertParameters("UserID").DefaultValue = UserID
            '        Me.sds_GroupUser.Insert()
            '    End If
            'Next
            'Chris Chu 20090723 新增default六宮格
            'Dim SiteID As String = System.Configuration.ConfigurationManager.AppSettings("SiteID").ToString
            'Me.sds_HomePageBlock.DeleteParameters("UserID").DefaultValue = UserID
            'Me.sds_HomePageBlock.InsertParameters("UserID").DefaultValue = UserID
            'Me.sds_HomePageBlock.InsertParameters("SiteID").DefaultValue = SiteID
            'Me.sds_HomePageBlock.Insert()

            Me.lbMessage.Text = GridViewPageInfo.Message_Insert_OK(Me.Page, "系統管理-人員管理")
        Else

            Me.lbMessage.Text = GridViewPageInfo.Message_Insert_False(Me.Page, "系統管理-人員管理")
        End If

                'Me.DetailsView1.ChangeMode(DetailsViewMode.ReadOnly)
                'Me.DetailsView1.DataBind()
                Me.MultiView1.ActiveViewIndex = 0
                Me.GridView1.DataBind()

    End Sub


    ''' <summary>
    ''' 加入密碼欄位
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub DetailsView1_ItemInserting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DetailsViewInsertEventArgs) Handles DetailsView1.ItemInserting
        Dim ddl_Section As DropDownList = CType(Me.DetailsView1.FindControl("ddl_Section"), DropDownList)
        Dim ddl_Dep_List As DropDownList = CType(Me.DetailsView1.FindControl("ddl_Dep_List"), DropDownList)
        Dim txt_P As TextBox = CType(Me.DetailsView1.FindControl("txt_P"), TextBox)
        Dim UserName As String = CType(Me.DetailsView1.Rows(3).Cells(1).Controls(0), TextBox).Text
        UserName = RemoveSQLInjection(UserName)
        Dim pwt As String = FormsAuthentication.HashPasswordForStoringInConfigFile(Trim(txt_P.Text), "SHA1")
        '如果有輸入字才作驗證,否則不更新
        '如果有同樣的username ,否則不更新
        Dim sqlStr As String = "select UserID as UserID from accounts_users where Username=@UserName"
        Using dr As SqlDataReader = ClassDB.GetDataReaderParam(sqlStr, New SqlParameter("@UserName", UserName))
            Try
                If dr IsNot Nothing Then
                    If dr.Read Then
                        Me.Label8.Text = "您的使用者帳號重複,請重新選擇，謝謝!"
                        e.Cancel = True
                    Else
                        pwt = FormsAuthentication.HashPasswordForStoringInConfigFile(Trim(txt_P.Text), "SHA1")
                        Me.sds_UserDetail.InsertParameters("Password").DefaultValue = pwt
                        Me.sds_UserDetail.InsertParameters("SectionID").DefaultValue = 0 'ddl_Section.SelectedValue
                        Me.sds_UserDetail.InsertParameters("DepartmentID").DefaultValue = ddl_Dep_List.SelectedValue
                    End If
                Else
                    Me.Label8.Text = "您的使用者帳號重複,請重新選擇，謝謝!"
                End If
            Catch ex As Exception
                Me.Label8.Text = "您的使用者帳號重複,請重新選擇，謝謝!"
            End Try
        End Using
    End Sub


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>在此更新使者的權限群組</remarks>
    Protected Sub DetailsView1_ItemUpdated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DetailsViewUpdatedEventArgs) Handles DetailsView1.ItemUpdated
        If e.AffectedRows > 0 Then
            '增加使用者的權限群組
            Me.sds_GroupUser.DeleteParameters("UserID").DefaultValue = Me.ViewState("UserID").ToString
            Me.sds_GroupUser.Delete()

            '權限群組的CheckBoxList
            Dim cbl_Permissions As CheckBoxList = CType(Me.DetailsView1.Rows(0).FindControl("cbl_Permissions"), CheckBoxList)

            For i As Integer = 0 To cbl_Permissions.Items.Count - 1
                Dim l As ListItem = cbl_Permissions.Items(i)
                If l.Selected = True Then
                    Me.sds_GroupUser.InsertParameters("GroupID").DefaultValue = l.Value
                    Me.sds_GroupUser.InsertParameters("UserID").DefaultValue = Me.ViewState("UserID").ToString
                    Me.sds_GroupUser.Insert()
                End If
            Next
            Me.lbMessage.Text = GridViewPageInfo.Message_Update_OK(Me.Page, "系統管理-人員管理")
        Else
            Me.lbMessage.Text = GridViewPageInfo.Message_Update_False(Me.Page, "系統管理-人員管理")
        End If

        Me.DetailsView1.ChangeMode(DetailsViewMode.ReadOnly)
        Me.DetailsView1.DataBind()
        Me.GridView1.DataBind()
        Me.MultiView1.ActiveViewIndex = 0

    End Sub


    ''' <summary>
    ''' 重新寫入密碼
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub DetailsView1_ItemUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DetailsViewUpdateEventArgs) Handles DetailsView1.ItemUpdating
        Dim pwt As String = Me.ViewState("pwt").ToString
        Dim txt_P As TextBox = CType(Me.DetailsView1.FindControl("txt_P"), TextBox)
        If txt_P.Text <> "" Then
            pwt = FormsAuthentication.HashPasswordForStoringInConfigFile(Trim(txt_P.Text), "SHA1")
        End If

        Me.sds_UserDetail.UpdateParameters("Password").DefaultValue = pwt
        Dim TextBox1 As TextBox = CType(Me.DetailsView1.FindControl("TextBox1"), TextBox)
        Dim TextBox4 As TextBox = CType(Me.DetailsView1.FindControl("TextBox4"), TextBox)
        Dim cbl_Permissions As CheckBoxList = CType(Me.DetailsView1.FindControl("cbl_Permissions"), CheckBoxList)
        Me.sds_UserDetail.UpdateParameters("TelNo").DefaultValue = RemoveSQLInjection(TextBox1.Text)
        Me.sds_UserDetail.UpdateParameters("TelNo_Ext").DefaultValue = RemoveSQLInjection(TextBox4.Text)


        Dim ddl_Section As DropDownList = CType(Me.DetailsView1.FindControl("ddl_Section"), DropDownList)
        Dim ddl_Dep_List As DropDownList = CType(Me.DetailsView1.FindControl("ddl_Dep_list"), DropDownList)
        Me.sds_UserDetail.UpdateParameters("SectionID").DefaultValue = 0 'RemoveSQLInjection(ddl_Section.SelectedValue)
        Me.sds_UserDetail.UpdateParameters("DepartmentID").DefaultValue = RemoveSQLInjection(ddl_Dep_List.SelectedValue)
    End Sub
#End Region


    Protected Sub ddl_Dep_List_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        ReBindSection()
    End Sub

    Private Sub ReBindSection()

        Dim ddl_Dep_List As DropDownList = CType(Me.DetailsView1.FindControl("ddl_Dep_List"), DropDownList)
        Dim ddl_Section As DropDownList = CType(Me.DetailsView1.FindControl("ddl_Section"), DropDownList)
        Dim DepartmentID As Integer = CInt((ddl_Dep_List.SelectedValue))

        If DepartmentID <> 0 Then
            Me.sds_Section.SelectParameters("DepartmentID").DefaultValue = DepartmentID
            BindSectionDDL(ddl_Section, DepartmentID)
            'ddl_Section.DataBind()
        Else
            ddl_Section.Items.Clear()

        End If
    End Sub

    Protected Sub ddl_Section_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
        'Dim ddl_Section As DropDownList = CType(Me.DetailsView1.FindControl("ddl_Section"), DropDownList)
        'ddl_Section.Items.Clear()
        'ddl_Section.Items.Add(New ListItem("請選擇科別", "0"))
    End Sub

    ''' <summary>
    ''' 完成新增User,取得key
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub sds_UserDetail_Inserted(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.SqlDataSourceStatusEventArgs) Handles sds_UserDetail.Inserted
        If e.AffectedRows > 0 Then

            '進行群組關聯作業
            Dim command As Common.DbCommand
            command = e.Command

            '取得新增後的主鍵
            Dim CreateDepID As Integer = 0
            Dim sqlStr As String = "select max(UserID) as UserID from accounts_users"
            Using dr As SqlDataReader = ClassDB.GetDataReader(sqlStr)
                Try
                    If dr.Read Then
                        CreateDepID = dr("UserID").ToString
                    End If
                Catch ex As Exception

                End Try
            End Using
            'Dim CreateDepID As Integer = CInt(command.Parameters("@retVal").Value.ToString())

            '副程式進行-使用者從屬關係關聯
            RelationAdd(CreateDepID)

            '副程式進行-使用者權限群組關聯
            GroupRelation(CreateDepID)

        End If
    End Sub

    ''' <summary>
    ''' 新增使用者關聯-提供權限繼承功能
    ''' </summary>
    ''' <param name="CreateID"></param>
    ''' <remarks></remarks>
    Private Sub RelationAdd(ByVal CreateID As Integer)
        Dim MyParentUsers() As String = Split(ModulePermissions.GetMyParentUsers(Session("UserID").ToString), ",")
        Dim i As Integer
        For i = 0 To UBound(MyParentUsers)

            Dim value As String = MyParentUsers(i)
            Dim NeedUpdate As Boolean = False

            'Some ParentUser could be dupcat, blow check after this value non other the same value
            If i = Array.LastIndexOf(MyParentUsers, value) Then
                NeedUpdate = True
            End If

            If NeedUpdate = True Then
                Try
                    '加入GroupRelation
                    Dim OwnerID As String = MyParentUsers(i)   '取得登入者的GroupID
                    Dim UpdateUser As String = CStr(Session("UserID"))

                    '設定插入SDS參數
                    Me.SDS_UsersRelation_Insert.InsertParameters("UpdateUser").DefaultValue = UpdateUser  '操作者
                    Me.SDS_UsersRelation_Insert.InsertParameters("CreateID").DefaultValue = CreateID    '被建造者
                    Me.SDS_UsersRelation_Insert.InsertParameters("OwnerID").DefaultValue = OwnerID
                    Me.SDS_UsersRelation_Insert.Insert()  '新增一筆關聯紀錄

                Catch ex As Exception
                    WriteErrLog(ex, Me.Page)
                End Try
            End If
        Next
    End Sub

    ''' <summary>
    ''' 使用者權限群組資料-新增資料
    ''' </summary>
    ''' <param name="NewUserID"></param>
    ''' <remarks></remarks>
    Private Sub GroupRelation(ByVal NewUserID As Integer)
        '權限群組的CheckBoxList
        Dim cbl_PermissionGroup As CheckBoxList = CType(Me.DetailsView1.Rows(0).FindControl("cbl_Permissions"), CheckBoxList)


        For i As Integer = 0 To cbl_PermissionGroup.Items.Count - 1
            Dim l As ListItem = cbl_PermissionGroup.Items(i)
            If l.Selected Then
                '寫入資料表

                ClassDB.UpdateDB("Net2_Accounts_GroupUser_Add",
                New SqlParameter("@UpdateUser", Session("UserID")),
                New SqlParameter("@GroupID", CInt(l.Value)),
                New SqlParameter("@UserID ", CInt(NewUserID)))

            End If
        Next
    End Sub
End Class

