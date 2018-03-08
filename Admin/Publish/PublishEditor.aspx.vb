Imports System.Data
Imports System.Data.SqlClient
Imports System.Net
Imports System.IO

Partial Class Publish_PublishEditor2
    'Inherits System.Web.UI.Page
    Inherits PageBase

    Private _NodeIDtoCheck As String = ""
    Private _NodeIDtoSelect As String = ""
    Dim MetaSubjectName As String = ConfigurationManager.AppSettings("MetaSubjectName").ToString
    Dim ENMetaSubjectName As String = ConfigurationManager.AppSettings("ENMetaSubjectName").ToString
    Dim _Accessible_website As String = ConfigurationManager.AppSettings("Accessible_website").ToString
    Dim _Meta_Kind As String = ConfigurationManager.AppSettings("Meta_Kind").ToString
    Dim _Meta_Subject As String = ConfigurationManager.AppSettings("Meta_Subject").ToString
    Dim _Accessible_website_EN As String = ConfigurationManager.AppSettings("Accessible_website_EN").ToString
    Dim _Meta_Kind_EN As String = ConfigurationManager.AppSettings("Meta_Kind_EN").ToString
    Dim _Meta_Subject_EN As String = ConfigurationManager.AppSettings("Meta_Subject_EN").ToString

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Dim ApplicationPath As String = Request.ApplicationPath

        If Not IsPostBack Then
            If Session("AuditPara") IsNot Nothing Then
                If IsNumeric(Session("AuditPara").replace(",", "")) Then
                    Dim key() As String = Session("AuditPara").ToString.Split(",")
                    Session("AuditPara") = Nothing
                    If IsNumeric(key(1)) Then
                        Session("publishkey") = key(1)
                    End If

                    If key(0) = "0" Then
                        Session("PublishMode") = "BtnPublish"
                        'Else
                        '    Session("PublishMode") = "BtnMeta"
                    End If
                End If
            End If
            BindUpdateUserInfo()

            If Not Session("PublishMode") Is Nothing And Not Session("publishkey") Is Nothing Then
                If Session("PublishMode") = "BtnPublish" Or Session("PublishMode") = "Edit" Then
                    Session("PublishMode") = "BtnPublish"
                    show1.Visible = True

                    'This is EditMode
                    ReadBackContent()

                    BuildSiteTree()

                    'BuildSiteTreeInformation()

                    'BuildSiteTreeKnowledge()

                    Me.PublishButton.Text = "確定更新"

                ElseIf Session("PublishMode") = "Edit" Then
                    show1.Visible = True

                    PublishButton.Visible = False
                    TempInsertButton.Visible = False
                    PublishButton.Visible = True
                    'This is EditMode
                    ReadBackContent()

                    BuildSiteTree()

                    'BuildSiteTreeInformation()

                    'BuildSiteTreeKnowledge()

                    Me.PublishButton.Text = "下一步"
                End If
            Else
                If Session("PublishMode") = "Next" Then

                Else
                    show1.Visible = True

                    'This is NewMode
                    Session("PublishMode") = "Add"

                    BuildSiteTree()

                    'BuildSiteTreeInformation()

                    'BuildSiteTreeKnowledge()

                    Me.CalendarPopupPublishDate.SelectedValue = Date.Today
                    Me.CalendarPopupExpireDate.SelectedValue = DateAdd(DateInterval.Year, 10, Date.Today)
                End If
            End If
        End If

        'Me.FCKeditor3.ToolbarStartExpanded = False
        Me.PublishButton.OnClientClick = "javascript:__doPostBack('" & Me.PublishButton.ClientID.ToString.Replace("_", "$") & "','');"
        Me.btnPreview.OnClientClick = "javascript:__doPostBack('" & Me.btnPreview.ClientID.ToString.Replace("_", "$") & "','');"
    End Sub


    ''' <summary>
    ''' 編輯模式讀回資料
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ReadBackContent()

        Using dr As SqlDataReader = ClassDB.GetDataReaderSP("Net2_Publish_ReadBackContent", New SqlParameter("@PublicID", CInt(Session("publishkey"))))
            If dr IsNot Nothing Then
                If dr.Read() Then
                    Me.SubjectTextBox.Text = dr("Subject").ToString
                    Me.SubjectLink.Text = dr("SubjectLink").ToString 'Add by Chris 20090708 for Subject HyperLink
                    Me.FCKeditor3.Value = dr("Content").ToString
                    Me.KeywordTextBox.Text = dr("Keyword").ToString
                    Me.CalendarPopupPublishDate.SelectedDate = dr("PublishDate")
                    Me.CalendarPopupExpireDate.SelectedDate = dr("PublishExpireDate")
                    Me.HotCheckBox.Checked = CType(dr.Item("Hot"), Boolean)
                    Me.rblImportant.SelectedIndex = IIf(dr("Important"), 0, 1)
                    If Not IsDBNull(dr("HomePhoto")) Then
                        If dr("HomePhoto").ToString.Length > 0 Then
                            Dim UploadPath As String = System.Configuration.ConfigurationManager.AppSettings("UploadPath").ToString
                            Me.HeadImage.Visible = True
                            Me.HeadImage.ImageUrl = UploadPath & dr("HomePhoto").ToString
                            Me.ViewState.Add("HomePhoto", dr("HomePhoto").ToString)
                            Me.HeadImage.Width = Unit.Pixel(200)
                            'Me.HeadImage.ImageAlign = ImageAlign.Left
                            Me.ltlBR.Text = "<br />"
                        End If

                        If Not IsDBNull(dr("HomePhotoInfo")) Then
                            Me.HomePhotoInfoTextBox.Text = dr("HomePhotoInfo").ToString
                            Me.HeadImage.ToolTip = dr("HomePhotoInfo").ToString
                        End If
                    End If

                    'If Not IsDBNull(dr.Item("ResponDepartment")) Then Me.ddl_ResponDepartment.SelectedValue = dr.Item("ResponDepartment")
                    'If Not IsDBNull(dr.Item("ResponUser")) Then Me.ddl_ResponUser.SelectedValue = dr.Item("ResponUser")
                    If Not IsDBNull(dr.Item("PostDate")) Then Me.ltlPostDateTime.Text = CType(dr.Item("PostDate"), Date).ToString("yyyy/MM/dd hh:mm:ss")
                    If Not IsDBNull(dr.Item("UpdateDateTime")) Then Me.ltlUpdateDateTime.Text = CType(dr.Item("UpdateDateTime"), Date).ToString("yyyy/MM/dd hh:mm:ss")
                    Me.ltlPostUser.Text = (dr("PostUserName").ToString) & "(" & (dr("PostDepartmentName")).ToString & ")"
                    Me.ltlUpdateUser.Text = (dr("UpdateUserName")).ToString & "(" & (dr("UpdateDepartmentName")).ToString & ")"
                    'If CType(dr.Item("AttFiles"), String).Length > 0 Then Me.UploadFiles1.ReadAttFiles(dr.Item("AttFiles"))
                    If Not IsDBNull(dr("AttFiles")) Then
                        'If dr("AttFiles").ToString.Length > 0 Then Me.UploadFiles1.ReadAttFiles(dr.Item("AttFiles"))
                        If dr("AttFiles").ToString.Length > 0 Then
                            Me.UploaderR2a.SetMultiUploadValue(dr("AttFiles").ToString)
                            Me.UploaderR2a.PublishToDownload = CType(dr("PublishToDownload").ToString, Boolean)
                        End If

                    End If
                    'Me.Label1.Text = "Trace=" & dr("AttFiles")

                    'Me.UploadFiles1.PublishToDownload = CType(dr.Item("PublishToDownload"), Boolean)

                    Session("ReviseStatus") = dr("ReviseStatus").ToString
                    '//////
                    'Me.ltl_UpdateDateTime.Text += "ReviseStatus:" & dr.Item("ReviseStatus")
                    If CInt(dr.Item("ReviseStatus")) = 1 Then
                        Me.TempInsertButton.Visible = False
                    End If
                End If
            End If
        End Using


        '讀回樹狀資料
        Dim NodeIDs As String = ""
        Using dr As SqlDataReader = ClassDB.GetDataReaderSP("Publication_GetNodeID_byPublicID", New SqlParameter("@PublicID", CInt(Session("publishkey"))))
            'Dim NodeIdCount As Integer = 0
            Try
                If dr IsNot Nothing Then
                    While dr.Read
                        NodeIDs += dr.Item("NodeID") & ","
                        'NodeIdCount += 1
                    End While
                End If
            Catch ex As Exception
                WriteErrLog(ex, Me)
            End Try
        End Using
        If NodeIDs.Length > 0 Then
            NodeIDs = NodeIDs.Substring(0, NodeIDs.Length - 1)
        End If

        Me.SiteTree1.SetNodeIDtoCheck = NodeIDs
        Using dr As SqlDataReader = ClassDB.GetDataReaderParam("SELECT NodeID FROM PublishInformationRelation WHERE PublicID = @PublicID", New SqlParameter("@PublicID", Session("publishkey")))
            NodeIDs = ""
            Try
                If dr IsNot Nothing Then
                    While dr.Read
                        NodeIDs += dr.Item("NodeID") & ","
                    End While
                End If
            Catch ex As Exception
                WriteErrLog(ex, Me)
            End Try
        End Using



    End Sub

    Private Sub BindUpdateUserInfo()

        If Not Session("UserID") Is Nothing Then
            Me.ltlPostUser.Text = CStr(Session("Name"))
            Me.ltlPostDateTime.Text = Date.Now.ToString("yyyy/MM/dd hh:mm:ss")
            Me.ltlUpdateUser.Text = CStr(Session("Name"))
            Me.ltlUpdateDateTime.Text = Date.Now.ToString("yyyy/MM/dd hh:mm:ss")
            Using dr As SqlDataReader = ClassDB.GetDataReaderParam("SELECT DepartmentName FROM Department WHERE DepartmentID=@DepartmentID", New SqlParameter("@DepartmentID", CStr(Session("DepartmentID"))))
                Try
                    If dr IsNot Nothing Then
                        If dr.Read Then
                            Me.ltlPostUser.Text &= "(" & (dr("DepartmentName")) & ")"
                            Me.ltlUpdateUser.Text &= "(" & (dr("DepartmentName")) & ")"
                        End If

                    End If
                Catch ex As Exception
                    WriteErrLog(ex, Me)
                End Try
            End Using
        End If

    End Sub


    ''' <summary>
    ''' 取得樹狀選單，有複雜的過濾條件
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub BuildSiteTree()

        If Session("Administrator") = True Or Session("SuperAdmin") = True Then
            '取得樹狀資料
            'Dim ds As DataSet = ClassDB.RunReturnDataSet("SELECT * FROM SITEMAP WHERE PublishType = 1", "SiteTreeDS")
            Using ds As DataSet = ClassDB.RunReturnDataSet("SELECT * FROM SITEMAP WHERE PublishType = 1", "SiteTreeDS")
                Me.SiteTree1.BuildTree(ds)
            End Using

        Else

            '取得GroupID(System can let User hvae more than 2 Group
            Dim MyGroupID As String = Session("GroupID")
            If Not Regex.IsMatch(MyGroupID.Replace("_", ""), "^[0-9]+$") Then
                Exit Sub
            End If
            If MyGroupID.IndexOf("_") >= 0 Then
                MyGroupID = MyGroupID.Replace("_", ",")
            End If
            Dim SiteID As String = ""
            '先取得擁有的SiteID
            Using dr As SqlDataReader = ClassDB.GetDataReaderParam("SELECT DISTINCT SiteID FROM  Accounts_GroupSitePermission WHERE  (GroupID IN (@MyGroupID))".Replace("@MyGroupID", MyGroupID), New SqlParameter("@MyGroupID", MyGroupID))

                Try
                    If dr IsNot Nothing Then
                        While dr.Read
                            SiteID += dr("SiteID") & ","
                        End While
                    End If
                Catch ex As Exception
                    WriteErrLog(ex, Me)
                End Try
            End Using
            If SiteID.Length > 0 Then SiteID = SiteID.Substring(0, SiteID.Length - 1)

            '取得NodeID
            Dim NodeIDs As String = ""
            Using dr As SqlDataReader = ClassDB.GetDataReaderParam("SELECT DISTINCT NodeID FROM  Accounts_Permissions WHERE  (GroupID IN (@MyGroupID)) AND (Category = 3)".Replace("@MyGroupID", MyGroupID), New SqlParameter("@MyGroupID", MyGroupID))

                Try
                    If dr IsNot Nothing Then
                        While dr.Read
                            NodeIDs += dr("NodeID") & ","
                        End While
                    End If
                Catch ex As Exception
                    WriteErrLog(ex, Me)

                End Try
            End Using
            If NodeIDs.Length > 0 Then NodeIDs = NodeIDs.Substring(0, NodeIDs.Length - 1)
            If Not Regex.IsMatch(NodeIDs.Replace(",", ""), "^[0-9]+$") Then
                Exit Sub
            End If

            '取得樹狀資料
            Using ds As DataSet = ClassDB.RunReturnDataSet("SELECT * FROM SITEMAP WHERE SiteID in(@SiteID) AND (NodeId IN (@NodeIDs))".Replace("@NodeIDs", NodeIDs).Replace("@SiteID", SiteID), "SiteTreeDS", New SqlParameter("@SiteID", SiteID), New SqlParameter("@NodeIDs", NodeIDs))
                Me.SiteTree1.BuildTree(ds)
            End Using
        End If

    End Sub



    Protected Sub TempInsertButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles TempInsertButton.Click
        Publish_Insert(True)
    End Sub

    Protected Sub PublishButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles PublishButton.Click
        Publish_Insert()
    End Sub



    ''' <summary>
    ''' 新增/更新文章資料
    ''' </summary>
    ''' <param name="IsTempInsert">是否為暫存(1 is Public, 0 is Not Public, -1 is ReturnBack, -2 is TempSave</param>
    ''' <remarks></remarks>
    Private Sub Publish_Insert(Optional ByVal IsTempInsert As Boolean = False)
        '判斷有無勾選樹狀選單
        'If Me.SiteTree1.GetSelectNodeCount = 0 OrElse Me.SiteTreeKnow.GetSelectNodeCount = 0 Then
        '    ModuleMisc.AlertMsg(Me, "您沒有勾選樹狀單元或知識管理項目!")
        '    Exit Sub
        'End If

        Dim _NodeID As String = Me.SiteTree1.CheckedNodes()     '用戶選擇的上稿單元
        'Dim _NodeIDKnow As String = Me.SiteTreeKnow.CheckedNodes()
        'Dim _NodeIDInfo As String = Me.SiteTreeInfo.CheckedNodes()
        'Chris 20090709 新增文章時如果沒有選擇單元時會報錯
        If _NodeID Is Nothing Or _NodeID = "" Then
            Dim script As String = "alert('請勾選發布單元!!!');"
            ScriptManager.RegisterClientScriptBlock(Me.Page, GetType(Page), "ErrorMessage", script, True)
        Else

            '建立陣列物件,取得所選取之單元編號
            Dim arrNodeID() As String = Split(_NodeID, ",")
            'Dim arrNodeIDKnow() As String = Split(_NodeIDKnow, ",")
            'Dim arrNodeIDInfo() As String = Split(_NodeIDInfo, ",")

            '//取得附檔編號
            'Dim _AttFiles As String = Me.UploadFiles1.GetAttachFiles()
            Dim _AttFiles As String = Me.UploaderR2a.MultiUploadValue
            'WriteLog("Publish_Insert", "_AttFiles=" & Me.UploaderR2a.MultiUploadValue)
            ''//取得是否發佈至下載區
            Dim _PublishToDownload As Boolean   '= True
            If _AttFiles.Length > 0 Then
                _PublishToDownload = Me.UploaderR2a.PublishToDownload
            Else
                _PublishToDownload = False
            End If

            '//是否需要審稿
            '---------------------------------------------------------------------------------------
            Dim IsNeedToRevise As Boolean = False

            '//審稿資訊取得
            '---------------------------------------------------------------------------------------
            Dim ReviseStatus As Integer = 1
            Dim CurrentReviser As Integer = 0


            '暫存按鈕
            If IsTempInsert = True Then
                ReviseStatus = -2
            End If

            '上稿內文
            Dim Content As SqlParameter = New SqlParameter
            Content.ParameterName = "@Content"
            Content.SqlDbType = SqlDbType.NText
            Content.Value = Me.FCKeditor3.Value

            '取得SiteMap節點的RefPath字串值
            Dim NodeID As Integer = CInt(arrNodeID(0))
            Dim SiteID As Integer
            Dim RefPath As String = ""
            Using dr2 As SqlDataReader = ClassDB.GetDataReaderSP("SiteMap_GetOne", New SqlParameter("@NodeID", NodeID))
                If dr2 IsNot Nothing Then
                    If dr2.Read() Then
                        RefPath = dr2("RefPath").ToString
                        SiteID = CInt(dr2("SiteID").ToString)
                    End If
                End If
            End Using
            '文章參照網址
            RefPath = RefPath & "?cnid=" & NodeID & "&p="


            '文章欄位與資料
            '//**//**//還需要處理路徑,//**審稿
            Dim retVal As Integer

            Select Case CStr(Session("PublishMode"))
                Case "Add"


                    '新增分類設定20080515()
                    retVal = ClassDB.RunSPReturnInteger("Net2_Publish_Add",
                    Content,
                    New SqlParameter("@Subject", RemoveSQLInjection(Me.SubjectTextBox.Text)),
                    New SqlParameter("@SubjectLink", RemoveSQLInjection(Me.SubjectLink.Text)),
                    New SqlParameter("@Keyword", RemoveSQLInjection(Me.KeywordTextBox.Text)),
                    New SqlParameter("@PublishDate", Me.CalendarPopupPublishDate.SelectedDate),
                    New SqlParameter("@PublishExpireDate", IIf(Me.CalendarPopupExpireDate.SelectedValue.HasValue = False, "2030/12/31", Me.CalendarPopupExpireDate.SelectedDate.ToString("yyyy/MM/dd"))),
                    New SqlParameter("@Hot", IIf(Me.HotCheckBox.Checked, 1, 0)),
                    New SqlParameter("@Important", RemoveSQLInjection(Me.rblImportant.SelectedValue)),
                    New SqlParameter("@AttFiles", RemoveSQLInjection(_AttFiles)),
                    New SqlParameter("@DepartmentID", CInt(Session("DepartmentID"))),
                    New SqlParameter("@UserID", CInt(Session("UserID"))),
                    New SqlParameter("@ReviseStatus", 1),
                    New SqlParameter("@CurrentReviser", CInt(CurrentReviser)),
                    New SqlParameter("@ResponDepartment", CInt(Session("DepartmentID"))),
                    New SqlParameter("@ResponUser", CInt(Session("UserID"))),
                    New SqlParameter("@PublishToDownload ", IIf(_PublishToDownload = True, 1, 0)),
                    New SqlParameter("@UpdateUser", CInt(Session("UserID"))),
                    New SqlParameter("@HomePhoto", HomePhotoUploadPath()),
                    New SqlParameter("@HomePhotoInfo", RemoveSQLInjection(Me.HomePhotoInfoTextBox.Text)),
                    New SqlParameter("@RefPath", RemoveSQLInjection(RefPath)))
                    Session("publishkey") = retVal
                    '異動追蹤
                    ModuleWriteLog.WriteAdminLog(Me, "新增-多向上稿文章，文章編號：" & retVal, True, Me.SiteTree1.GetCheckedNodeText())

                    '發行文章計數器(2008/1/18改成從Publication資料表中取得，所以不用再另外記錄)
                    ModuleCounter.DocCount_Update(SiteID, NodeID, CInt(Session("UserID").ToString), CInt(Session("DepartmentID").ToString), CInt(Session("UserID").ToString), CInt(Session("DepartmentID").ToString), 1)
                Case "Edit"
                    Session("PublishMode") = "Next"
                    Session("SNSiteTree1") = SiteTree1.GetCheckedNodeText()
                    Session("SNIsTempInsert") = IsTempInsert
                    Session("SNNodeID") = _NodeID
                    'Session("SNNodeIDKnow") = _NodeIDKnow
                    'Session("SNNodeIDInfo") = _NodeIDInfo
                    Session("SNContent") = Convert.ToBase64String(Encoding.UTF8.GetBytes(Content.Value))
                    Session("SNAttFiles") = _AttFiles
                    Session("SNSubject") = Me.SubjectTextBox.Text
                    Session("SNSubjectLink") = Me.SubjectLink.Text 'add by Chris 20090709 for 新增標題超連結欄位
                    Session("SNKeyword") = Me.KeywordTextBox.Text
                    Session("SNPublishDate") = Me.CalendarPopupPublishDate.SelectedDate
                    Session("SNPublishExpireDate") = IIf(Me.CalendarPopupExpireDate.SelectedValue.HasValue = False, "3000/12/31", Me.CalendarPopupExpireDate.SelectedDate.ToString("yyyy/MM/dd"))
                    Session("SNHot") = IIf(Me.HotCheckBox.Checked, 1, 0)
                    Session("SNImportant") = Me.rblImportant.SelectedValue '國防部沒有
                    Session("SNAttFiles") = _AttFiles '國防部沒有
                    Session("SNReviseStatus") = ReviseStatus
                    Session("SNCurrentReviser") = CurrentReviser
                    Session("SNPublishToDownload") = IIf(_PublishToDownload = True, 1, 0)
                    Session("SNHomePhoto") = HomePhotoUploadPath()
                    Session("SNHomePhotoInfo") = Me.HomePhotoInfoTextBox.Text
                    Session("SNRefPath") = RefPath
                    '取得ReadBackContent()時存入的Session("ReviseStatus")
                    Dim ThisReviseStatus As Integer = CInt(Session("ReviseStatus"))

                    '如果回頭編輯被退回的文章,編輯過後即改回狀態0,再送編輯
                    If ThisReviseStatus = -1 Then
                        If IsTempInsert = False Then
                            ThisReviseStatus = 0
                        End If
                    ElseIf ThisReviseStatus = -2 Then   '原稿為暫存時
                        If IsTempInsert = False Then    '使用者若按下發佈則變更為1,否則仍為-2(仍暫存)
                            ThisReviseStatus = 1
                        End If
                    End If
                    Session("SNReviseStatus") = ThisReviseStatus
                    InterfaceBuilder.TabGoPre2(Me, "PublishEditor.aspx")
                    Exit Sub
                Case "BtnPublish"

                    '取得ReadBackContent()時存入的Session("ReviseStatus")
                    Dim ThisReviseStatus As Integer = CInt(Session("ReviseStatus"))

                    '如果回頭編輯被退回的文章,編輯過後即改回狀態0,再送編輯
                    If ThisReviseStatus = -1 Then
                        If IsTempInsert = False Then
                            ThisReviseStatus = 0
                        End If
                    ElseIf ThisReviseStatus = -2 Then   '原稿為暫存時
                        If IsTempInsert = False Then    '使用者若按下發佈則變更為1,否則仍為-2(仍暫存)
                            ThisReviseStatus = 1
                        End If
                    End If
                    If Me.SubjectLink.Text IsNot Nothing And Me.SubjectLink.Text.ToLower.Equals("http://") Then
                        Me.SubjectLink.Text = Nothing
                    End If
                    retVal = CInt(Session("publishkey"))
                    ClassDB.UpdateDB("Net2_Publish_Update",
                    Content,
                    New SqlParameter("@PublicID", CInt(Session("publishkey"))),
                    New SqlParameter("@Subject", RemoveSQLInjection(Me.SubjectTextBox.Text)),
                    New SqlParameter("@SubjectLink", RemoveSQLInjection(Me.SubjectLink.Text)),
                    New SqlParameter("@Keyword", RemoveSQLInjection(Me.KeywordTextBox.Text)),
                    New SqlParameter("@PublishDate", Me.CalendarPopupPublishDate.SelectedDate),
                    New SqlParameter("@PublishExpireDate", IIf(Me.CalendarPopupExpireDate.SelectedValue.HasValue = False, "3000/12/31", Me.CalendarPopupExpireDate.SelectedDate.ToString("yyyy/MM/dd"))),
                    New SqlParameter("@Hot", IIf(Me.HotCheckBox.Checked, 1, 0)),
                    New SqlParameter("@Important", CInt(Me.rblImportant.SelectedValue)),
                    New SqlParameter("@AttFiles", RemoveSQLInjection(_AttFiles)),
                    New SqlParameter("@ResponDepartment", CInt(Session("DepartmentID"))),
                    New SqlParameter("@ResponUser", CInt(Session("UserID"))),
                    New SqlParameter("@HomePhoto", RemoveSQLInjection(HomePhotoUploadPath())),
                    New SqlParameter("@HomePhotoInfo", RemoveSQLInjection(Me.HomePhotoInfoTextBox.Text)),
                    New SqlParameter("@ReviseStatus", 1),
                    New SqlParameter("@PublishToDownload", IIf(_PublishToDownload = True, 1, 0)),
                    New SqlParameter("@UpdateUser", CInt(Session("UserID"))),
                    New SqlParameter("@RefPath", RemoveSQLInjection(RefPath & retVal)))



                    '後台異動記錄
                    'ModuleWriteLog.WriteAdminLog(Me, "修改-多向上稿文章，文章編號：" & CInt(retVal).ToString, True, RemoveSQLInjection(Me.SiteTree1.GetCheckedNodeText()))
                    'ModuleAuditLog.WriteAuditLog(Me.Page, 2, "修改-多向上稿文章，文章編號：" & CInt(retVal).ToString, "0", "0," & CInt(retVal).ToString, RemoveSQLInjection(Me.SubjectTextBox.Text))
            End Select


            '寫入單元的關聯資料
            Dim i As Integer
            ClassDB.UpdateDB("Net2_PublishRelation_Delete", New SqlParameter("@PublicID", retVal))
            For i = 0 To UBound(arrNodeID)
                ClassDB.UpdateDB("Net2_PublishRelation_Update",
                New SqlParameter("@PublicID", (retVal)),
                New SqlParameter("@NodeID", CInt(arrNodeID(i))))

                '異動追蹤
                'ModuleWriteLog.WriteAdminLog(Me, "新增-多向上稿文章寫入單元，文章編號：" & CInt(retVal) & "單元編號：" & RemoveSQLInjection(arrNodeID(i)), True, RemoveSQLInjection(Me.SiteTree1.GetCheckedNodeText()))
            Next

            'ClassDB.UpdateDB("Net2_PublishKnowledgeRelation_Delete", New SqlParameter("@PublicID", retVal))
            'For i = 0 To UBound(arrNodeIDKnow)
            '    ClassDB.UpdateDB("Net2_PublishKnowledgeRelation_Update", _
            '    New SqlParameter("@PublicID", retVal), _
            '    New SqlParameter("@NodeID", arrNodeIDKnow(i)))

            '    '異動追蹤
            '    ModuleWriteLog.WriteAdminLog(Me, "新增-多向上稿文章寫入知識管理，文章編號：" & retVal & "單元編號：" & arrNodeIDKnow(i), True, Me.SiteTree1.GetCheckedNodeText())
            'Next

            'ClassDB.UpdateDB("Net2_PublishInformationRelation_Delete", New SqlParameter("@PublicID", retVal))
            'For i = 0 To UBound(arrNodeIDInfo)
            '    ClassDB.UpdateDB("Net2_PublishInformationRelation_Update", _
            '    New SqlParameter("@PublicID", retVal), _
            '    New SqlParameter("@NodeID", arrNodeIDInfo(i)))
            'Next

            'me.UploaderR2a.u
            '發佈到下載區的標題
            If _AttFiles.Length > 0 Then
                Dim subject As String = Me.SubjectTextBox.Text

                Me.UploaderR2a.UpdateDownloadSubject(subject, IIf(Me.CalendarPopupExpireDate.SelectedValue.HasValue = False, "3000/12/31", Me.CalendarPopupExpireDate.SelectedDate.ToString("yyyy/MM/dd")))
            End If

            Session("PublishMode") = Nothing    '清除Add或Edit模式，回到清單列表
            Session("PublishKey") = Nothing
            Session("ReviseStatus") = Nothing


            InterfaceBuilder.TabGoPre(Me)
        End If

    End Sub

    ''' <summary>
    ''' 上傳首頁主圖、並取得路徑+檔名
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function HomePhotoUploadPath() As String
        Dim o As String = ""

        Dim HomePhotoFileName As String = Me.FileUploadHomePhoto.FileName
        If HomePhotoFileName.Length > 0 Then
            '不管是新增文章或編輯某個文章,如果使用者上傳新圖即以新圖取代空的或舊有的首頁圖
            Dim FullUploadPath As String = Server.UrlPathEncode(Server.MapPath(ModulePathManager.GetUploadPath & "HomePhoto/"))


            'Check Dir exits
            If Not FileIO.FileSystem.DirectoryExists(FullUploadPath) Then
                FileIO.FileSystem.CreateDirectory(FullUploadPath)
            End If

            Try
                'Upload the File
                FileUploadHomePhoto.SaveAs(FullUploadPath & HomePhotoFileName)
            Catch ex As Exception
                WriteErrLog(ex, Me)
            End Try

            'DB Save Path
            o = "HomePhoto/" & HomePhotoFileName
        Else
            '如果圖檔欄位沒有資料，那麼Check是否原來已有首頁圖，若有則將值丟回
            If Me.ViewState("HomePhoto") IsNot Nothing Then
                o = Me.ViewState("HomePhoto").ToString
            End If
        End If

        Return o
    End Function

#Region "Meta新增修改"

#End Region

    Protected Sub btnPreview_Click(sender As Object, e As EventArgs) Handles btnPreview.Click
        Dim strform As StringBuilder = New StringBuilder
        strform.Append("<form id=""PostForm"" name=""PostForm"" action=""PublishPreview.aspx"" method=""POST"" Target=""_Block""> ")
        strform.Append("<input type=""hidden"" name=""Content"" value=""" & (Me.FCKeditor3.Value) & """>")
        strform.Append("<input type=""hidden"" name=""Subject"" value=""" & (Me.SubjectTextBox.Text) & """>")
        strform.Append("<input type=""hidden"" name=""FileID"" value=""" & (Me.UploaderR2a.MultiUploadValue) & """>")
        strform.Append("</form>")
        strform.Append("<script language='javascript'>")
        strform.Append("document.getElementById('PostForm').submit();")
        strform.Append("</script>")
        Page.Controls.Add(New LiteralControl(strform.ToString))

    End Sub
End Class
