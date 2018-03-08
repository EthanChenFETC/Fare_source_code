Imports System.Data
Imports System.Data.SqlClient

''' <summary>
''' ���y��T���I��p�Ƨ�s�A��
''' </summary>
''' <remarks></remarks>
Public Module ModuleCounter


    ''' <summary>
    ''' �I�\�v�W�[
    ''' </summary>
    ''' <param name="PublicID">�o���峹���D���</param>
    ''' <param name="NodeID">�o���峹���椸�D���</param>
    ''' <remarks></remarks>
    Public Sub ViewCount_Add(ByVal PublicID As Integer, Optional ByVal NodeID As Integer = 0)
        Dim YearDate As String = Now.ToString("yyyyMM", System.Globalization.DateTimeFormatInfo.InvariantInfo)

        '//Get UserID, DepartmentID Info By PubliID
        Dim UserID As Integer
        Dim DepartmentID As Integer
        Dim ResUserID As Integer
        Dim ResDepartmentID As Integer


        '���o���峹�W�Z�H���B�������
        Using dr As SqlDataReader = ClassDB.GetDataReaderSP("ViewCount_GetIDs_ByPublicID", New SqlParameter("@PublicID", PublicID))
            Try
                If dr IsNot Nothing Then
                    If dr.Read() Then
                        UserID = CInt(dr.Item("UserID"))
                        DepartmentID = CInt(dr.Item("DepartmentID"))
                        ResUserID = CInt(dr.Item("ResponUser"))
                        ResDepartmentID = CInt(dr.Item("ResponDepartment"))
                        'WriteLog("NotError GetData", "UserID=" & UserID & " DepartmentID=" & DepartmentID & " ResUserID=" & ResUserID & " ResDepartmentID=" & ResDepartmentID)
                    End If
                End If
            Catch ex As Exception
                'WriteLog("ModuleCounter", ex.Message & ex.Source)
            Finally
            End Try
        End Using
        '//���o�椸�s���A���F��sSitemap�ӧO�椸�\Ū��
        If NodeID = 0 Then
            Using dr2 As SqlDataReader = ClassDB.GetDataReaderSP("ViewCount_Get_NodeID_ByPublicID", New SqlParameter("@PublicID", PublicID))
                Try
                    If dr2.Read Then
                        NodeID = CInt(dr2("NodeID"))
                    End If
                Catch ex As Exception
                    'WriteLog("ModuleCounter", ex.Message & ex.Source)
                Finally

                End Try
            End Using
        End If

        Try
            '//�`�\Ū�q�W�[
            ClassDB.UpdateDB("ViewCount_Add", _
            New SqlParameter("@NodeID", NodeID), _
            New SqlParameter("@PublicID", PublicID), _
            New SqlParameter("@UserID", UserID), _
            New SqlParameter("@DepartmentID", DepartmentID), _
            New SqlParameter("@ResUserID", ResUserID), _
            New SqlParameter("@ResDepartmentID", ResDepartmentID))

            '//�̤���\Ū�q�W�[
            ClassDB.UpdateDB("ViewCount_Add_ByMonth", _
             New SqlParameter("@YearDate", YearDate), _
             New SqlParameter("@PublicID", PublicID), _
             New SqlParameter("@UserID", UserID), _
             New SqlParameter("@DepartmentID", DepartmentID), _
             New SqlParameter("@ResUserID", ResUserID), _
             New SqlParameter("@ResDepartmentID", ResDepartmentID))
        Catch ex As Exception
            'WriteLog("ViewCount_Add", ex.Message & ex.Source & ex.StackTrace)
        End Try

        'WriteLog("==>", "YearDate" & YearDate & " PublicID=" & PublicID & " UserID=" & UserID & " DepartmentID=" & DepartmentID & " ResUserID=" & ResUserID & " ResDepartmentID=" & ResDepartmentID)
    End Sub


    ''' <summary>
    ''' ��s�椸���s���H���A�D�W�Z�椸����s
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub ViewCount_Add_NonePublish(ByVal NodeID As Integer)
        'Net2_F_SiteMap_ViewCount_Add
        'Dim NodeID As String = Request.Params("cnid").ToString
        

        ClassDB.UpdateDB("Net2_F_SiteMap_ViewCount_Add", _
                New SqlParameter("@NodeID", NodeID))
    End Sub
    ''' <summary>
    ''' ��s�椸���s���H���A�D�W�Z�椸����s
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub WriteCounter(ByVal Webform As System.Web.UI.Page)
        Dim SiteID As Integer = CInt(System.Configuration.ConfigurationManager.AppSettings("SiteID"))
        Dim SelectDate As String = ((Now.ToShortDateString).Split("/")(0) * 10000 + (Now.ToShortDateString).Split("/")(1) * 100 + (Now.ToShortDateString).Split("/")(2)).ToString
        Dim NodeID As String = "0"
        'If IsNumeric(Webform.Request.Params("cnid")) Or IsNumeric(Webform.Request.Params("cnkw")) Or IsNumeric(Webform.Request.Params("cnhb")) Then
        '    If Webform.Request.Params("cnid") IsNot Nothing Then
        '        NodeID = Webform.Request.Params("cnid").ToString
        '    ElseIf Webform.Request.Params("cnkw") IsNot Nothing Then
        '        Dim Keyword As String = Webform.Request.Params("cnkw").ToString
        '        NodeID = Sitemap.GetNodeID_byNodeKeyword(Keyword)
        '    ElseIf Webform.Request.Params("cnhb") IsNot Nothing Then
        '        NodeID = "99999"
        '    End If
        'Else
        '    Dim url As String = Webform.Request.RawUrl

        'End If
        NodeID = Sitemap.GetNodeID_Auto(Webform)
        If IsNumeric(SiteID) And IsNumeric(NodeID) Then
            'ClassDB.UpdateDB("delete from ClickCount where UID < (select (max(UID)-100000) as maxcount from CLickCount) ")
            Dim strSql As String = "Select UID from ClickCount where SiteID=@SiteID and NodeID = @NodeID And Convert(varchar(20), UpdateDateTime, 112) = @UpdateDateTime"
            Using dr As SqlDataReader = ClassDB.GetDataReaderParam(strSql, New SqlParameter("@SiteID", SiteID), New SqlParameter("@NodeID", NodeID), New SqlParameter("@UpdateDateTime", SelectDate))
                Try
                    If dr IsNot Nothing Then
                        If dr.Read Then
                            Dim UID As String = CInt(dr("UID")).ToString
                            ClassDB.UpdateDBText("Update ClickCount set Counter = Counter + 1 , UpdateDateTime = getdate() where UID=@UID", New SqlParameter("@UID", UID))
                        Else
                            ClassDB.UpdateDBText("Insert into ClickCount (SiteID,NodeID,SelectDate,UpdateDateTime, Counter) Values(@SiteID, @NodeID, @UpdateDateTime, getdate(), 1)", New SqlParameter("@SiteID", SiteID), New SqlParameter("NodeID", NodeID), New SqlParameter("@UpdateDateTime", Convert.ToDateTime(SelectDate)))
                        End If
                    End If
                Catch ex As Exception
                Finally

                End Try
            End Using
        End If
    End Sub
End Module
