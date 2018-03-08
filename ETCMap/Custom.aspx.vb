Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Xml
Imports System.Xml.XPath
Imports Microsoft.VisualBasic

Imports System.Net
Partial Class _Custom
    Inherits InjectionPage
    Dim SiteID As String = ConfigurationManager.AppSettings("SiteID").ToString
    Dim ProjectDefault As String = ConfigurationManager.AppSettings("ProjectDefault").ToString
    Dim Injection As Sql_Injection = New Sql_Injection
    Dim Remind As String = ConfigurationManager.AppSettings("CustomRemind").ToString

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.PublishBlock2.BlockID = Remind
        Dim SCAN As String = ConfigurationManager.AppSettings("SCAN")
        If IsNumeric(SCAN) Then
            sdsHWList.SelectParameters("IsScan").DefaultValue = SCAN
            sdsInterChangeList1.SelectParameters("IsScan").DefaultValue = SCAN
            sdsInterChangeList2.SelectParameters("IsScan").DefaultValue = SCAN
            sdsInterChangeList3.SelectParameters("IsScan").DefaultValue = SCAN
            sdsInterChangeList4.SelectParameters("IsScan").DefaultValue = SCAN
            sdsInterChangeList5.SelectParameters("IsScan").DefaultValue = SCAN
            sdsInterChangeList6.SelectParameters("IsScan").DefaultValue = SCAN
            sdsInterChangeList7.SelectParameters("IsScan").DefaultValue = SCAN
            sdsInterChangeList8.SelectParameters("IsScan").DefaultValue = SCAN
            sdsInterChangeList9.SelectParameters("IsScan").DefaultValue = SCAN
            sdsInterChangeList10.SelectParameters("IsScan").DefaultValue = SCAN
            sdsInterChangeList11.SelectParameters("IsScan").DefaultValue = SCAN
            sdsInterChangeList12.SelectParameters("IsScan").DefaultValue = SCAN
            sdsInterChangeList13.SelectParameters("IsScan").DefaultValue = SCAN
            sdsInterChangeList14.SelectParameters("IsScan").DefaultValue = SCAN
            sdsInterChangeList15.SelectParameters("IsScan").DefaultValue = SCAN
            sdsInterChangeList16.SelectParameters("IsScan").DefaultValue = SCAN
            sdsInterChangeList17.SelectParameters("IsScan").DefaultValue = SCAN
            sdsInterChangeList18.SelectParameters("IsScan").DefaultValue = SCAN
            sdsInterChangeList19.SelectParameters("IsScan").DefaultValue = SCAN
            sdsInterChangeList20.SelectParameters("IsScan").DefaultValue = SCAN
        End If
        Dim routesl() As String
        Dim para As Boolean = False
        Dim routes As String = Request.Params(datePicker.ClientID.Replace("_", "$"))
        If routes IsNot Nothing Then
            If routes.Trim.Length > 0 Then
                Try
                    routesl = Request.Params(datePicker.ClientID.Replace("_", "$")).ToString.ToLower.Replace("-", "").Replace("/", "").Split(",")
                    para = False
                    For i As Integer = 0 To routesl.Length - 1
                        Try
                            If routesl(i).Trim.Length = 0 Then
                                Continue For
                            End If
                            If Not IsNumeric(routesl(i)) Then
                                para = True
                                Exit For
                            End If
                        Catch ex As Exception
                            para = True
                            Exit For
                        End Try
                    Next
                    If para Then
                        ScriptManager.RegisterClientScriptBlock(Me.Page, GetType(Page), "warning", "alert('系統參數被竄改，請洽系統管理員');document.location.href='Default.aspx';", True)
                        Exit Sub
                    End If
                Catch ex As Exception
                End Try
            End If
        End If
        Me.MultiView1.ActiveViewIndex = -1
        If Not IsPostBack Then
            Me.datePicker.Text = Date.Today.ToString("yyyy/MM/dd")
            TextChanged(sender, e)
            Session.Remove("ICList")
            'Session.Add("ICList", Nothing)
            Session.Remove("ListCount")
            Session.Add("ListCount", 5)

        End If

        For i As Integer = 6 To Session("ListCount")
            CType(Me.lbtMore.Parent.FindControl("UpdatePanel" & i.ToString), UpdatePanel).Visible = True
        Next
        Me.ddlTimePediod.Attributes.Add("onchange", "javascript:document.getElementById('" & Me.hidd1.ClientID & "').value = this.value;__doPostBack('" & ddlTimePediod.ClientID.Replace("_", "$") & "','');")
        'Me.ddlTimePediod.Attributes.Add("onchange", "javascript:document.getElementById('" & Me.hidd1.ClientID & "').value = this.value;return true;")
        Me.imgCalculate.Attributes.Add("onkeypress", "javascript:__doPostBack('" & imgCalculate.ClientID.Replace("_", "$") & "','')")
        Me.CarType.Attributes.Add("onchange", "if(this.selectedIndex==1) {document.getElementById('" & FareType.ClientID & "').selectedIndex = 2;} ;return false")
        Me.FareType.Attributes.Add("onchange", "if(this.selectedIndex!=2 ) if(document.getElementById('" & CarType.ClientID & "').selectedIndex == 1) {document.getElementById('" & CarType.ClientID & "').selectedIndex = 0;} ;return false")

    End Sub

    ''' <summary>
    ''' 自訂路線下拉選單產生
    ''' </summary>
    ''' <param name="sender">觸發本事件控制項</param>
    ''' <param name="e">觸發本事件帶進之參數</param>
    ''' <remarks></remarks>
    ''' 
    Protected Sub TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim selectdate As DateTime = New Date(Me.datePicker.Text.Split("/")(0), Me.datePicker.Text.Split("/")(1), Me.datePicker.Text.Split("/")(2))
        Dim DateLastYear As Integer = Convert.ToInt32(DateAdd(DateInterval.Year, -1, Date.Now).ToString("yyyyMM")) * 100 + 1

        Dim DateNow As Integer = Convert.ToInt32(selectdate.ToString("yyyyMMdd"))
        If IsDate(Me.datePicker.Text) Then
            If DateNow < DateAdd(DateInterval.Year, -1, Date.Now).ToString("yyyyMMdd") Then
                ScriptManager.RegisterClientScriptBlock(Me.Page, GetType(Page), "warning", "alert('日期區間不能早於距今日起一年以前!!');", True)
                Exit Sub
            End If
        Else
            ScriptManager.RegisterClientScriptBlock(Me.Page, GetType(Page), "warning", "alert('日期格式錯誤!!');", True)
            Exit Sub
        End If
        If IsNumeric(ConfigurationManager.AppSettings("SCAN")) Then
            If ConfigurationManager.AppSettings("SCAN") = 1 Then
                Me.imgCalculate.Enabled = False
                Me.ddlTimePediod.Enabled = False
                datePicker.Enabled = False
                Me.CarType.Items.Clear()
                Me.FareType.Items.Clear()
                Me.CarType.Items.Add(New ListItem("小型車", "1"))
                Me.FareType.Items.Add(New ListItem("eTag用戶", "1"))
                Me.CarType.SelectedIndex = 0
                Me.FareType.SelectedIndex = 0
                Exit Sub
            End If
        End If
        Dim dtProject As DataTable = Context.Cache("InterChangeFareProject")
        If dtProject Is Nothing Then dtProject = Fare.GetInterChangeFareProject()

        Dim dt As DataTable = New DataTable ' ClassDB.RunReturnDataTable("Select (convert(varchar(2),StartHour) + ':00 至 ' + convert(varchar(2),EndHour) + ':00' ) as TimePeriod, UID as ProjectUID from InterchangeFareProject where  Convert(varchar(20),StartDate,112) <= Convert(varchar(20),convert(datetime,'" & Me.datePicker.Text & "'),112) and Convert(varchar(20),EndDate,112) >= Convert(varchar(20),convert(datetime,'" & Me.datePicker.Text & "'),112) and IsOnline>0 order by StartHour")
        Dim col1 As DataColumn = New DataColumn("TimePeriod")
        Dim col2 As DataColumn = New DataColumn("ProjectUID")
        Dim col3 As DataColumn = New DataColumn("IsFree")
        dt.Columns.Clear()
        dt.Columns.Add(col1)
        dt.Columns.Add(col2)
        dt.Columns.Add(col3)
        'Dim selectdate As DateTime = New Date(Me.datePicker.Text.Split("/")(0), Me.datePicker.Text.Split("/")(1), Me.datePicker.Text.Split("/")(2))
        'Dim DateLastYear As Integer = Convert.ToInt32(DateAdd(DateInterval.Year, -1, Date.Now).ToString("yyyyMM")) * 100 + 1
        'Dim DateNow As Integer = Convert.ToInt32(selectdate.ToString("yyyyMMdd"))
        'If DateNow < DateLastYear Then
        '    ScriptManager.RegisterClientScriptBlock(Me.Page, GetType(Page), "warning", "alert('日期區間不能早於距今日起一年以前!!');", True)
        '    Exit Sub
        'End If
        Dim drow() As DataRow = dtProject.Select(" StartDate <= '#" & Me.datePicker.Text & "#' and EndDate >= '#" & Me.datePicker.Text & "#' and IsOnline>0 and UID <> " & ProjectDefault & "", "StartHour")
        If drow.Length = 0 Then
            Dim newr As DataRow = dt.NewRow
            newr("TimePeriod") = "0:00 至 24:00"
            newr("ProjectUID") = ProjectDefault
            newr("IsFree") = False
            dt.Rows.Add(newr)
        Else
            For i As Integer = 0 To drow.Length - 1
                Dim newr As DataRow = dt.NewRow
                newr("TimePeriod") = drow(i)("StartHour").ToString & ":00 至 " & (drow(i)("EndHour")).ToString & ":00"
                newr("ProjectUID") = drow(i)("UID").ToString
                newr("IsFree") = IIf(drow(i)("IsFree"), True, False)
                dt.Rows.Add(newr)
            Next
        End If
        Me.ddlTimePediod.Items.Clear()
        Me.ddlTimePediod.DataSource = dt
        Me.ddlTimePediod.SelectedIndex = -1

        Me.ddlTimePediod.DataBind()

        Dim selectindex As Integer = -1
        For i As Integer = 0 To dt.Rows.Count - 1
            If dt.Rows(i)("IsFree").ToString.ToLower.Equals("true") Then
                ddlTimePediod.Items(i).Text &= "(暫停收費時段)"
                ddlTimePediod.Items(i).Attributes.Add("disabled", "disabled")
            ElseIf selectindex = -1 Then
                selectindex = i
            End If
        Next

        Me.ddlTimePediod.SelectedIndex = selectindex
        If Session("SelectedDate") Is Nothing Then
            Session.Add("SelectedDate", Me.datePicker.Text)
        Else
            Try
                If Me.datePicker.Text = Session("SelectedDate") Then
                    Me.ddlTimePediod.SelectedValue = Me.hidd1.Value
                Else
                    Session.Remove("SelectedDate")
                    Session.Add("SelectedDate", Me.datePicker.Text)
                End If
            Catch ex As Exception
                Session.Remove("SelectedDate")
            End Try
        End If
        Me.ltlProjectInform.Text = "<font color=red>【" & dtProject.Select(" UID=" & Me.ddlTimePediod.SelectedValue.ToString)(0)("ProjectName").ToString & "】</font>"
        Me.UpdatePanelDate.Update()
        Me.MultiView1.ActiveViewIndex = -1
    End Sub
    Protected Sub ddlTimePediod_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlTimePediod.SelectedIndexChanged
        Dim dtProject As DataTable = Context.Cache("InterChangeFareProject")
        If dtProject Is Nothing Then dtProject = Fare.GetInterChangeFareProject()
        Me.ltlProjectInform.Text = "<font color=red>【" & dtProject.Select(" UID=" & ddlTimePediod.SelectedValue.ToString)(0)("ProjectName").ToString & "】</font>"
    End Sub
    Protected Sub HWList_OnDataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles HWList1.DataBound
        Dim HWList As DropDownList = CType(sender, DropDownList)

        If Not IsPostBack Then
            If HWList.ID = "HWList1" Then
                HWList.SelectedIndex = 1
            End If
        End If
    End Sub
    Protected Sub HWList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim HWList As DropDownList = CType(sender, DropDownList)
        Dim ICListFrom As DropDownList = CType(HWList.Parent.FindControl("ICListFrom" & HWList.ID.Replace("HWList", "")), DropDownList)
        Dim ICListTo As DropDownList = CType(HWList.Parent.FindControl("ICListTo" & HWList.ID.Replace("HWList", "")), DropDownList)
        'ICListFrom.DataBind()
        'Dim i As Integer = HWList.ID.Replace("HWList", "")
        'If i > 5 Then
        '    CType(Me.lbtMore.Parent.FindControl("UpdatePanel" & i.ToString), UpdatePanel).Update()
        'End If
    End Sub
    Protected Sub ICListFrom_OnDataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles ICListFrom1.DataBound, ICListFrom2.DataBound, ICListFrom3.DataBound, ICListFrom4.DataBound, ICListFrom5.DataBound, ICListFrom6.DataBound, ICListFrom7.DataBound, ICListFrom8.DataBound, ICListFrom9.DataBound, ICListFrom10.DataBound, ICListFrom11.DataBound, ICListFrom12.DataBound, ICListFrom13.DataBound, ICListFrom14.DataBound, ICListFrom15.DataBound, ICListFrom16.DataBound, ICListFrom17.DataBound, ICListFrom18.DataBound, ICListFrom19.DataBound, ICListFrom20.DataBound
        Dim ICListFrom As DropDownList = CType(sender, DropDownList)
        Dim HWList As DropDownList = CType(ICListFrom.Parent.FindControl("HWList" & ICListFrom.ID.Replace("ICListFrom", "")), DropDownList)
        If HWList.SelectedValue > 0 Then
            BindICListTo(ICListFrom, HWList.SelectedValue)
        End If
    End Sub

    Private Sub BindICListTo(ByVal ICListFrom As DropDownList, ByVal HWUID As Integer)
        Dim ICListto As DropDownList = CType(ICListFrom.Parent.FindControl("ICListTo" & ICListFrom.ID.Replace("ICListFrom", "")), DropDownList)
        'ICListto.DataSource = Fare.GetICListFromHWID(HWUID)
        ICListto.DataBind()
        If HWUID > 0 Then
            DoDisplayICListTo(ICListFrom, ICListto, Fare.GetInterChangeList, HWUID)
        End If
    End Sub
    Private Sub DoDisplayICListTo(ByVal ICListFrom As DropDownList, ByVal ICListTo As DropDownList, ByVal dt As DataTable, ByVal HWUID As Integer)
        Dim DTDDL As DataTable = Fare.GetICListFromHWID(HWUID)
        For i As Integer = 0 To ICListFrom.Items.Count - 1
            Dim FName As String = DTDDL.Select("UID=" & ICListFrom.Items(i).Value.ToString)(0)("ICName")
            Dim TName As String = DTDDL.Select("UID=" & ICListTo.Items(i).Value.ToString)(0)("ICName")
            If Not dt.Select(" UID=" & ICListFrom.Items(i).Value.ToString)(0)("InSouth") And Not dt.Select(" UID=" & ICListFrom.Items(i).Value.ToString)(0)("InNorth") Then
                ICListFrom.Items(i).Enabled = False
            ElseIf i = 0 Or (dt.Select(" UID=" & ICListFrom.Items(i).Value.ToString)(0)("InSouth") And Not dt.Select(" UID=" & ICListFrom.Items(i).Value.ToString)(0)("InNorth")) Then
                If HWUID Mod 2 > 0 Then
                    ICListFrom.Items(i).Text = FName & "(限南下)"
                Else
                    ICListFrom.Items(i).Text = FName & "(限往東)"
                End If
            ElseIf (Not dt.Select(" UID=" & ICListFrom.Items(i).Value.ToString)(0)("InSouth") And dt.Select(" UID=" & ICListFrom.Items(i).Value.ToString)(0)("InNorth")) Then
                If HWUID Mod 2 > 0 Then
                    ICListFrom.Items(i).Text = FName & "(限北上)"
                Else
                    ICListFrom.Items(i).Text = FName & "(限往西)"
                End If
            End If

            Dim inSouth As Boolean = dt.Select(" UID=" & ICListFrom.SelectedItem.Value.ToString)(0)("InSouth")
            Dim inNorth As Boolean = dt.Select(" UID=" & ICListFrom.SelectedItem.Value.ToString)(0)("inNorth")
            Dim OutSouth As Boolean = dt.Select(" UID=" & ICListTo.Items(i).Value.ToString)(0)("OutSouth")
            Dim OutNorth As Boolean = dt.Select(" UID=" & ICListTo.Items(i).Value.ToString)(0)("OutNorth")
            Dim iItemOrder As Integer = dt.Select("UID=" & ICListFrom.SelectedItem.Value)(0)("ItemOrder")
            Dim oItemOrder As Integer = dt.Select("UID=" & ICListTo.Items(i).Value)(0)("ItemOrder")
            Dim IsEnabled As Boolean = True
            'If OutSouth And Not OutNorth Then
            '    If HWUID Mod 2 > 0 Then
            '        ICListTo.Items(i).Text = TName & "(限南下)"
            '    Else
            '        ICListTo.Items(i).Text = TName & "(限往東)"
            '    End If
            'ElseIf OutNorth And Not OutSouth Then
            '    If HWUID Mod 2 > 0 Then
            '        ICListTo.Items(i).Text = TName & "(限北上)"
            '    Else
            '        ICListTo.Items(i).Text = TName & "(限往西)"
            '    End If
            'End If
            If ICListFrom.SelectedValue = ICListTo.Items(i).Value Then
                IsEnabled = False
            ElseIf (Not inSouth And oItemOrder > iItemOrder) Or (Not inNorth And oItemOrder < iItemOrder) Then
                IsEnabled = False
            ElseIf inSouth And OutSouth = False And oItemOrder > iItemOrder Then
                IsEnabled = False
            ElseIf inNorth And OutNorth = False And oItemOrder < iItemOrder Then
                IsEnabled = False
            Else

            End If
            If ICListFrom.Items(i).Text.IndexOf("系統") >= 0 Then
                ICListFrom.Items(i).Attributes.Add("style", "color:red;")
                ICListTo.Items(i).Attributes.Add("style", "color:red;")
            End If
            If Not IsEnabled Then
                ICListTo.Items(i).Attributes.Add("disabled", "disabled")
                If ICListTo.SelectedIndex = i Then
                    ICListTo.SelectedIndex += 1
                    If i = ICListTo.Items.Count - 1 Then
                        ICListTo.SelectedIndex = 0
                    End If

                End If
            Else
                ICListTo.Items(i).Attributes.Remove("disabled")
            End If
        Next
    End Sub

    Protected Sub ICList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ICListFrom As DropDownList = CType(sender, DropDownList)
        Dim ICListTo As DropDownList = CType(ICListFrom.Parent.FindControl("ICListTo" & ICListFrom.ID.Replace("ICListFrom", "")), DropDownList)
        Dim HWList As DropDownList = CType(ICListFrom.Parent.FindControl("HWList" & ICListFrom.ID.Replace("ICListFrom", "")), DropDownList)
        If HWList.SelectedValue > 0 Then

            DoDisplayICListTo(ICListFrom, ICListTo, Fare.GetInterChangeList, HWList.SelectedValue)
        End If
        'ICListTo.SelectedIndex = 0
        'If ICListTo.Items(ICListTo.SelectedIndex).Enabled = False Then
        '    If ICListTo.SelectedIndex = ICListTo.Items.Count - 1 Then
        '        ICListTo.SelectedIndex = 0
        '    Else
        '        ICListTo.SelectedIndex += 1
        '    End If
        'End If
        'UpdatePanel1.Update()
    End Sub
    Private Function CheckInput(ByVal btn As ImageButton) As Boolean
        Dim ret As Boolean = False
        For i As Integer = 1 To 20
            If CType(btn.Parent.FindControl("HWList" & i.ToString), DropDownList).SelectedIndex > 0 Then
                ret = True
                Exit For
            End If
        Next
        Return ret
    End Function

    Protected Sub imgCalculate_Click2(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgCalculate.Click
        If Not CheckInput(CType(sender, ImageButton)) Then
            Dim scripts As String = "alert('請至少選擇一個行程!!');"
            ScriptManager.RegisterClientScriptBlock(Me.Page, GetType(Page), "start", scripts, True)
        End If
        Dim selectdate As DateTime = New Date(Me.datePicker.Text.Split("/")(0), Me.datePicker.Text.Split("/")(1), Me.datePicker.Text.Split("/")(2))
        Dim DateLastYear As Integer = Convert.ToInt32(DateAdd(DateInterval.Year, -1, Date.Now).ToString("yyyyMM")) * 100 + 1
        Dim DateNow As Integer = Convert.ToInt32(selectdate.ToString("yyyyMMdd"))
        If DateNow < DateLastYear Then
            ScriptManager.RegisterClientScriptBlock(Me.Page, GetType(Page), "warning", "alert('日期區間不能早於距今日起一年以前!!');", True)
            Exit Sub
        End If

        '抓取這個計劃的費率表
        Dim FareList As DataTable = Fare.GetFareListByProjectID(Me.ddlTimePediod.SelectedValue)
        Dim FreeList As DataTable = Fare.GetInterChangeStartFree()
        Dim dt As DataTable = Fare.GetInterChangeList()
        Dim ProjectList As DataTable = Fare.GetProjectByProjectID(Me.ddlTimePediod.SelectedValue)
        Dim RoadList As String = ""
        Dim TotalMiles As Double = 0
        Dim TotalFare As Double = 0
        Dim TotalFareM As Double = 0
        Dim TotalFareG As Double = 0
        Dim TotalFareDiff As Double = 0
        Dim TotalFareMDiff As Double = 0
        Dim TotalFareGDiff As Double = 0
        Dim TotalFareAdd As Double = 0
        Dim TotalFareMAdd As Double = 0
        Dim TotalFareGAdd As Double = 0
        Dim TripMiles As Double = 0

        For i As Integer = 1 To 20
            Dim ICFrom As DropDownList = CType(imgCalculate.Parent.FindControl("ICListFrom" & i.ToString), DropDownList)
            Dim ICTo As DropDownList = CType(imgCalculate.Parent.FindControl("ICListTo" & i.ToString), DropDownList)
            Dim HWList As DropDownList = CType(imgCalculate.Parent.FindControl("HWList" & i.ToString), DropDownList)
            If HWList.SelectedIndex = 0 Then
                Continue For
            End If
            '交流道(起點)與交流道(訖點)不能相同!
            If ICFrom.SelectedValue = ICTo.SelectedValue Then
                Dim scripts As String = " alert('交流道(起點)與交流道(訖點)不能相同!!');"
                ScriptManager.RegisterClientScriptBlock(Me.Page, GetType(Page), "Error", scripts, True)
                Exit Sub
            End If
            '如果是暫停收費時段，就根本不用算
            If Not ProjectList.Rows(0)("IsFree") Then
                Dim ICList As DataTable = Fare.GetICListFromHWID(HWList.SelectedIndex)
                Dim tMiles As Double = 0
                Dim cMiles As Double = 0
                Dim cFares As Double = 0
                Dim cFaresM As Double = 0
                Dim cFaresG As Double = 0
                Dim cFaresDiff As Double = 0
                Dim cFaresMDiff As Double = 0
                Dim cFaresGDiff As Double = 0
                Dim cFaresAdd As Double = 0
                Dim cFaresMAdd As Double = 0
                Dim cFaresGAdd As Double = 0
                Dim StartItemOrder As Integer = dt.Select("UID=" & ICFrom.SelectedValue)(0)("ItemOrder")
                Dim StopItemOrder As Integer = dt.Select("UID=" & ICTo.SelectedValue)(0)("ItemOrder")
                Dim StartIndex As Integer = StartItemOrder
                Dim StopIndex As Integer = StopItemOrder

                If StartItemOrder > StopItemOrder Then
                    StartIndex = StopItemOrder
                    StopIndex = StartItemOrder
                    '處理起點和經過免費的問題
                    '如果方向北上    如果起始點往北一個交流道免費的話，結束指標減一
                    If FreeList.Select(" StartIC=" & ICFrom.SelectedValue & " and StopIC = " & dt.Select(" ItemOrder=" & (StartItemOrder - 1).ToString)(0)("UID").ToString & " and FreeType in (0,3,4)").Length > 0 Then
                        StopIndex -= 1
                    End If
                    '處理終點和經過免費的問題
                    '如果方向北上    如果往南一個交流道到終點免費的話，起始指標加一
                    If FreeList.Select(" StartIC=" & dt.Select(" ItemOrder=" & (StopItemOrder + 1).ToString)(0)("UID").ToString & " and StopIC = " & ICTo.SelectedValue & " and FreeType in (1,2,4)").Length > 0 Then
                        StartIndex += 1
                    End If
                Else
                    Try
                        '處理起點和經過免費的問題
                        '如果方向南下    如果起始點往南一個交流道免費的話，起始指標加一
                        If FreeList.Select(" StartIC=" & ICFrom.SelectedValue & " and StopIC = " & dt.Select(" HWUID=" & HWList.SelectedValue & " and ItemOrder=" & (StopItemOrder).ToString)(0)("UID").ToString & " and FreeType in (0,3,4)").Length > 0 Then
                            StartIndex += 1
                        End If
                        '處理終點和經過免費的問題
                        '如果方向北上    如果往南一個交流道到終點免費的話，結束指標減一
                        If FreeList.Select(" StartIC=" & dt.Select(" HWUID=" & HWList.SelectedValue & " and ItemOrder=" & (StopItemOrder - 1).ToString)(0)("UID").ToString & " and StopIC = " & ICTo.SelectedValue & " and FreeType in (1,2,4)").Length > 0 Then
                            StopIndex -= 1
                        End If
                    Catch ex As Exception

                    End Try
                End If
                If StartIndex >= StopIndex Then
                    Continue For
                End If

                For j As Integer = StartIndex To StopIndex - 1

                    Dim ICUID As Integer = 0
                    Dim ICNext As Integer = 0
                    If dt.Select("HWUID = " & HWList.SelectedValue & " and ItemOrder=" & j).Length > 0 Then
                        ICUID = dt.Select("HWUID = " & HWList.SelectedValue & " and ItemOrder=" & j)(0)("UID")
                    Else
                        Continue For
                    End If
                    Dim k As Integer = 0
                    For k = j To StopIndex - 1
                        If dt.Select("HWUID = " & HWList.SelectedValue & " and ItemOrder=" & (k + 1)).Length > 0 Then
                            ICNext = dt.Select("HWUID = " & HWList.SelectedValue & " and ItemOrder=" & (k + 1))(0)("UID")
                            j = k
                            Exit For
                        Else
                            Continue For
                        End If
                    Next
                    If k = StopIndex Then
                        Continue For
                    End If
                    If FreeList.Select(" StartIC=" & ICUID & " and StopIC = " & ICNext.ToString & " and FreeType = 4").Length <= 0 And FreeList.Select(" StartIC=" & ICNext & " and StopIC = " & ICUID & " and FreeType = 4").Length <= 0 Then
                        If StartItemOrder > StopItemOrder Then
                            If FreeList.Select(" StartIC=" & ICNext.ToString & " and StopIC = " & ICUID & " and FreeType = 3").Length <= 0 And (ICFrom.SelectedValue <> ICNext Or FreeList.Select(" StartIC=" & ICNext.ToString & " and StopIC = " & ICUID & " and FreeType = 0").Length <= 0) And (ICTo.SelectedValue <> ICUID Or FreeList.Select(" StartIC=" & ICNext.ToString & " and StopIC = " & ICUID & " and FreeType = 1").Length = 0) Then
                                tMiles += Double.Parse(dt.Select("UID=" & ICNext)(0)("ICMilesN").ToString) - Double.Parse(dt.Select("UID=" & ICUID)(0)("ICMilesN").ToString)
                                cMiles += Double.Parse(dt.Select("UID=" & ICNext)(0)("ICMilesBetweenN").ToString)
                                cFares += Double.Parse(FareList.Select("ICUID=" & ICNext)(0)("FareSN").ToString)
                                cFaresM += Double.Parse(FareList.Select("ICUID=" & ICNext)(0)("FareMN").ToString)
                                cFaresG += Double.Parse(FareList.Select("ICUID=" & ICNext)(0)("FareGN").ToString)
                                cFaresDiff += Double.Parse(FareList.Select("ICUID=" & ICNext)(0)("FareSDiffN").ToString)
                                cFaresMDiff += Double.Parse(FareList.Select("ICUID=" & ICNext)(0)("FareMDiffN").ToString)
                                cFaresGDiff += Double.Parse(FareList.Select("ICUID=" & ICNext)(0)("FareGDiffN").ToString)
                                cFaresAdd += Double.Parse(FareList.Select("ICUID=" & ICNext)(0)("FareSAddN").ToString)
                                cFaresMAdd += Double.Parse(FareList.Select("ICUID=" & ICNext)(0)("FareMAddN").ToString)
                                cFaresGAdd += Double.Parse(FareList.Select("ICUID=" & ICNext)(0)("FareGAddN").ToString)
                            End If
                        Else
                            If FreeList.Select(" StartIC=" & ICUID.ToString & " and StopIC = " & ICNext & " and FreeType = 2").Length <= 0 And (ICFrom.SelectedValue <> ICUID Or FreeList.Select(" StartIC=" & ICUID.ToString & " and StopIC = " & ICNext & " and FreeType = 0").Length <= 0) And (ICTo.SelectedValue <> ICNext Or FreeList.Select(" StartIC=" & ICUID.ToString & " and StopIC = " & ICNext & " and FreeType = 1").Length = 0) Then
                                tMiles += Double.Parse(dt.Select("UID=" & ICNext)(0)("ICMilesN").ToString) - Double.Parse(dt.Select("UID=" & ICUID)(0)("ICMilesN").ToString)
                                cMiles += Double.Parse(dt.Select("UID=" & ICNext)(0)("ICMilesBetween").ToString)
                                cFares += Double.Parse(FareList.Select("ICUID=" & ICNext)(0)("FareS").ToString)
                                cFaresM += Double.Parse(FareList.Select("ICUID=" & ICNext)(0)("FareM").ToString)
                                cFaresG += Double.Parse(FareList.Select("ICUID=" & ICNext)(0)("FareG").ToString)
                                cFaresDiff += Double.Parse(FareList.Select("ICUID=" & ICNext)(0)("FareSDiff").ToString)
                                cFaresMDiff += Double.Parse(FareList.Select("ICUID=" & ICNext)(0)("FareMDiff").ToString)
                                cFaresGDiff += Double.Parse(FareList.Select("ICUID=" & ICNext)(0)("FareGDiff").ToString)
                                cFaresAdd += Double.Parse(FareList.Select("ICUID=" & ICNext)(0)("FareSAdd").ToString)
                                cFaresMAdd += Double.Parse(FareList.Select("ICUID=" & ICNext)(0)("FareMAdd").ToString)
                                cFaresGAdd += Double.Parse(FareList.Select("ICUID=" & ICNext)(0)("FareGAdd").ToString)
                            End If
                        End If

                    End If
                Next

                TripMiles += tMiles
                TotalMiles += cMiles
                TotalFare += cFares
                TotalFareM += cFaresM
                TotalFareG += cFaresG
                TotalFareDiff += cFaresDiff
                TotalFareMDiff += cFaresMDiff
                TotalFareGDiff += cFaresGDiff
                TotalFareAdd += cFaresAdd
                TotalFareMAdd += cFaresMAdd
                TotalFareGAdd += cFaresGAdd
            End If
            RoadList += "路段" & i.ToString & ": " & "從" & ICFrom.Items(IIf(ICFrom.SelectedIndex < 0, ICFrom.SelectedIndex = 0, ICFrom.SelectedIndex)).Text.Replace("(限往東)", "").Replace("(限南下)", "").Replace("(限往東西)", "").Replace("(限北上)", "") & " 到 " & ICTo.Items(ICTo.SelectedIndex).Text & "&nbsp。<br>" & vbCrLf '(計費里程：" & IIf(Format(cMiles, "##.#") = "", "0.0", Format(cMiles, "##.#")) & "公里;費用: " & IIf(Format(cFares, "##.#") = "", "0.0", Format(cFares, "##.#")) & "元)。<br>" & vbCrLf
        Next
        Dim RoadTrip As String = RoadList

        Dim FareNormal As Double = ProjectList.Rows(0)("FareS")

        Dim FareDiscount As Double = ProjectList.Rows(0)("FareSDiscount")
        If Me.CarType.Value = "2" Then
            FareType.Value = 3
        ElseIf Me.CarType.Value = "1" Then
        ElseIf Me.CarType.Value = "3" Then
            TotalFare = TotalFareM
            TotalFareDiff = TotalFareMDiff
            TotalFareAdd = TotalFareMAdd
            FareNormal = ProjectList.Rows(0)("FareM")
            FareDiscount = ProjectList.Rows(0)("FareMDiscount")
        ElseIf Me.CarType.Value = "4" Then
            TotalFare = TotalFareG
            TotalFareDiff = TotalFareGDiff
            TotalFareAdd = TotalFareGAdd
            FareNormal = ProjectList.Rows(0)("FareG")
            FareDiscount = ProjectList.Rows(0)("FareGDiscount")
        End If

        '長途折扣 = ( 總計通行費 - (正常費率 * 長途優惠里程) ) * (正常費率 - 優惠費率) / 正常費率  四捨五入至小數點一位，也就是 (總費用 - 長途折扣以內里程的費用 ) = 超過長途折扣里程的費用 , 
        ' 然後 用這個(超過長途折扣里程的費用) * (正常費率 - 長途折扣優惠費率) / 正常費率 (這個部分就是 超過長途折扣里程的費用  - 長途折扣費率和正常費率的比率)
        Dim sDiscount As Double = 0
        Dim sFreeMile As Double = ProjectList.Rows(0)("FreeMiles") * FareNormal

        '如果計畫設定沒有長途折扣里程
        If ProjectList.Rows(0)("DiscountMiles") > 0 Or (CarType.Value = 2 And ProjectList.Rows(0)("IsTaxi")) Then
            sDiscount = IIf(TotalFare > ProjectList.Rows(0)("DiscountMiles") * FareNormal, Fix(((TotalFare - ProjectList.Rows(0)("DiscountMiles") * FareNormal) * (FareNormal - FareDiscount) / FareNormal) * 10 + 0.005) / 10, 0)
        End If

        Dim sTotalFare As Double
        '如果是計程車就沒有免費里程、長途折扣、差別費率、預儲帳戶九折、4到6日繳費9折和ETC9折優惠
        If ProjectList.Rows(0)("IsFree") Then
            sTotalFare = 0
        ElseIf Me.CarType.Value = 2 And ProjectList.Rows(0)("IsTaxi") Then
            sTotalFare = TotalFare
        Else
            sTotalFare = TotalFare
            If ProjectList.Rows(0)("IsLongDistant") Then
                sTotalFare -= sDiscount
            End If
            If ProjectList.Rows(0)("IsFreeMiles") Then
                sTotalFare -= sFreeMile
            End If
            If ProjectList.Rows(0)("IsDifferent") Then
                sTotalFare -= TotalFareDiff
            End If
            If ProjectList.Rows(0)("IsAdd") Then
                sTotalFare += TotalFareAdd
            End If
        End If
        RoadTrip = RoadTrip & "總計費里程: <font color=blue >" & IIf(Format(TotalMiles, "##.#") = "", "0.0", Format(TotalMiles, "##.#")) & "</font>公里"
        Me.ltlRoadTrip.Text = RoadTrip

        '免費里程
        Me.ltlFreeFare.Text = IIf(sFreeMile = 0, "0.0", Format(sFreeMile, "##.#"))
        '長途折扣
        Me.ltlDiscount.Text = IIf(sDiscount = 0, "0.0", Format(sDiscount, "##.#"))
        '差別費率, 如果設定不顯示加價費率代表差別費率和加價費率加總顯示於差別費率，設定顯示則將差別費率和加價費率拆開兩列

        Me.ltlDiff.Text = IIf(TotalFareDiff = 0, "0.0", Format(TotalFareDiff, "##.#"))
        Me.ltlAdd.Text = IIf(TotalFareAdd = 0, "0.0", Format(TotalFareAdd, "##.#"))
        '原始通行費
        Me.ltlFare.Text = IIf(TotalFare = 0, "0.0", Format(TotalFare, "##.#"))
        '合計通行費= 
        Me.TotalFare.Text = IIf(sTotalFare <= 0, "0.0", Format(sTotalFare, "##.#"))
        '調整整數的時候要加上 小數一位的 零
        If Me.ltlFare.Text.IndexOf(".") < 0 Then
            Me.ltlFare.Text &= ".0"
        End If
        If Me.ltlDiscount.Text.IndexOf(".") < 0 Then
            Me.ltlDiscount.Text &= ".0"
        End If
        If Me.ltlFreeFare.Text.IndexOf(".") < 0 Then
            Me.ltlFreeFare.Text &= ".0"
        End If
        If Me.ltlDiff.Text.IndexOf(".") < 0 Then
            Me.ltlDiff.Text &= ".0"
        End If
        If Me.ltlAdd.Text.IndexOf(".") < 0 Then
            Me.ltlAdd.Text &= ".0"
        End If
        If Me.TotalFare.Text.IndexOf(".") < 0 Then
            Me.TotalFare.Text &= ".0"
        End If

        '調整如果值小於1,要在前面加個整數位 0 
        If Me.ltlFare.Text.IndexOf(".") = 0 Then
            Me.ltlFare.Text = "0" & Me.ltlFare.Text
        End If
        If Me.ltlDiscount.Text.IndexOf(".") = 0 Then
            Me.ltlDiscount.Text = "0" & Me.ltlDiscount.Text
        End If
        If Me.ltlFreeFare.Text.IndexOf(".") = 0 Then
            Me.ltlFreeFare.Text = "0" & Me.ltlFreeFare.Text
        End If
        If Me.ltlDiff.Text.IndexOf(".") = 0 Then
            Me.ltlDiff.Text = "0" & Me.ltlDiff.Text
        End If
        If Me.ltlAdd.Text.IndexOf(".") = 0 Then
            Me.ltlAdd.Text = "0" & Me.ltlAdd.Text
        End If
        If Me.TotalFare.Text.IndexOf(".") = 0 Then
            Me.TotalFare.Text = "0" & Me.TotalFare.Text
        End If

        '計畫設定要不顯示的關掉
        Me.trFareDiff.Visible = ProjectList.Rows(0)("IsDifferent")
        Me.trFareAdd.Visible = ProjectList.Rows(0)("IsAdd")
        Me.trLongDistant.Visible = ProjectList.Rows(0)("IsLongDistant")
        Me.trFreeMiles.Visible = ProjectList.Rows(0)("IsFreeMiles")
        Me.trETag.Visible = ProjectList.Rows(0)("IsETag")
        Me.trReserved.Visible = ProjectList.Rows(0)("IsReserved")
        Me.tr46day.Visible = ProjectList.Rows(0)("IsJustInTime")
        Me.tr7day.Visible = ProjectList.Rows(0)("IsJustInTime")
        '選擇繳費方法
        If CarType.Value = 2 And ProjectList.Rows(0)("IsTaxi") Then
            Me.TotalFare.Text = IIf(Double.Parse(Me.TotalFare.Text) = 0, "0", Format(Double.Parse(Me.TotalFare.Text), "##"))
            Me.ltlTotal.Visible = True
            Me.trTotal.Visible = True
            Me.trLongDistant.Visible = False
            Me.trFreeMiles.Visible = False
            Me.trFareDiff.Visible = False
            Me.trFareAdd.Visible = False
            Me.trETag.Visible = False
            Me.trReserved.Visible = False
            Me.tr46day.Visible = False
            Me.tr7day.Visible = False
        Else
            If FareType.Value = 1 Then
                lblIsETag.Text = "預儲帳戶足餘額" & (ProjectList.Rows(0)("RateETag") * 10).ToString & "折扣款"
                Dim Fare90 As Double = Fix((Double.Parse(Me.TotalFare.Text) * ProjectList.Rows(0)("RateETag") * 10 + 5) / 10)
                Me.ltlETag.Text = IIf(Fare90 = 0, "0", Format(Fare90, "##"))

                If ProjectList.Rows(0)("IsETag") Then
                    Me.ltlTotal.Visible = False
                Else
                    Me.TotalFare.Text = IIf(Double.Parse(Me.TotalFare.Text) = 0, "0", Format(Double.Parse(Me.TotalFare.Text), "##"))
                    Me.ltlTotal.Visible = True
                    Me.trETag.Visible = False
                End If
                Me.trReserved.Visible = False
                Me.tr46day.Visible = False
                Me.tr7day.Visible = False
            ElseIf FareType.Value = 2 Then
                lblReserve.Text = "預約用戶帳戶足餘額" & (ProjectList.Rows(0)("RateReserved") * 10).ToString & "折扣款"
                Dim Fare90 As Double = Fix((Double.Parse(Me.TotalFare.Text) * ProjectList.Rows(0)("RateReserved") * 10 + 5) / 10)
                Me.ltlReserved.Text = IIf(Fare90 = 0, "0", Format(Fare90, "##"))
                If ProjectList.Rows(0)("IsReserved") Then
                    Me.ltlTotal.Visible = False
                Else
                    Me.TotalFare.Text = IIf(Double.Parse(Me.TotalFare.Text) = 0, "0", Format(Double.Parse(Me.TotalFare.Text), "##"))
                    Me.ltlTotal.Visible = True
                    Me.trReserved.Visible = False
                End If
                Me.trETag.Visible = False
                Me.tr46day.Visible = False
                Me.tr7day.Visible = False
            ElseIf FareType.Value = 3 Then
                Dim Fare46day As Double = Fix((Double.Parse(Me.TotalFare.Text) * ProjectList.Rows(0)("RateNoReserved") * 10 + 5) / 10)
                Me.ltl46day.Text = IIf(Fare46day = 0, "0", Format(Fare46day, "##"))
                Me.ltl7day.Text = IIf(Me.TotalFare.Text = "0.0", "0", Format(Double.Parse(Me.TotalFare.Text), "##"))
                lbl46.Text = "通行日起第4~6天主動繳費" & (ProjectList.Rows(0)("RateNoReserved") * 10).ToString & "折"
                If ProjectList.Rows(0)("IsJustInTime") Then
                    Me.ltlTotal.Visible = False
                Else
                    Me.TotalFare.Text = IIf(Double.Parse(Me.TotalFare.Text) = 0, "0", Format(Double.Parse(Me.TotalFare.Text), "##"))
                    Me.ltlTotal.Visible = True
                    Me.tr46day.Visible = False
                    Me.tr7day.Visible = False
                End If
                ltlTotal.Visible = False
                trETag.Visible = False
                trReserved.Visible = False
            End If
        End If

        '計畫時段說明
        Dim Year As String = "2016"
        Dim Months As Integer = "05"
        Dim Days As Integer = "01"
        If IsNumeric(Me.datePicker.Text.Replace("/", "")) Then
            Dim ymd() As String = Me.datePicker.Text.Split("/")
            Year = ymd(0)
            Months = ymd(1)
            Days = ymd(2)
        End If
        Me.ltlDate.Text = (CInt(Year) - 1911).ToString & "年" & Months & "月" & Days & "日"
        Me.ltlTimePeriod.Text = ProjectList.Rows(0)("StartHour").ToString & " ~ " & ProjectList.Rows(0)("EndHour").ToString & "時"
        Me.ltlFareS.Text = ProjectList.Rows(0)("FareS").ToString
        Me.ltlFareM.Text = ProjectList.Rows(0)("FareM").ToString
        Me.ltlFareG.Text = ProjectList.Rows(0)("FareG").ToString
        Me.ltlProjectName.Text = "(費率計畫：<font color=red>" & ProjectList.Rows(0)("MainProjectName").ToString & "</font>)"
        If Not IsDBNull(ProjectList.Rows(0)("Notes")) Then
            If ProjectList.Rows(0)("Notes").ToString.Trim.Length > 0 Then
                Me.ltlProjectNotes.Text = "<p class=""time"">【收費說明：" & ProjectList.Rows(0)("Notes").ToString & "】</p>"
            End If
        End If
        '紀錄LOG

        Dim scriptss As String = "document.location.href='#buttom'"
        Dim UserIP As String = Request.ServerVariables("REMOTE_ADDR")
        If Not CheckSqlInjectionWording(UserIP.Replace(":", "").Replace(".", "").Replace("-", "").ToString.Trim) And Regex.IsMatch(UserIP.Replace(":", "").Replace(".", "").Replace("-", "").ToString.Trim, "^[0-9]+$") Then
            UserIP = RemoveSqlInjection(UserIP)
            Dim SessionID As String = Session("SessionID")
            SessionID = RemoveSqlInjection(SessionID)
            'ClassDB.UpdateDB("farecalculatorLogInsert",
            '                 New SqlParameter("SiteID", 1),
            '                 New SqlParameter("UserType", 0),
            '                 New SqlParameter("UserIP", UserIP),
            '                 New SqlParameter("SessionID", SessionID),
            '                 New SqlParameter("RoadTrip", RemoveSqlInjection(RoadTrip))
            '                 )
            ClassDB.UpdateDB("farecalculatorLogInsert",
                             New SqlParameter("SiteID", 1),
                             New SqlParameter("UserType", 0),
                             New SqlParameter("UserIP", ""),
                             New SqlParameter("SessionID", ""),
                             New SqlParameter("RoadTrip", "")
                             )
            Me.MultiView1.ActiveViewIndex = 0

            'Dim sqlStr As String = "update FareCalculateCount set CustomCount = CustomCount + 1, LastUpdateCustom = getdate() where SiteID =  1 and convert(varchar(20), getdate(), 112) = LastUpdateDateTime"
            'ClassDB.UpdateDB(sqlStr)
            Fare.FindFooter(Me.Controls)

        Else
            scriptss = "alert('請不要攻擊本網站!!');"
        End If
        ScriptManager.RegisterClientScriptBlock(Me.Page, GetType(Page), "start", scriptss, True)
    End Sub
   

#Region "Print"

    Protected Sub ibtPrint_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)

        imgCalculate_Click2(sender, e)
        Me.hdFareNormal.Value = Me.ltlFare.Text
        Me.hdFareDiscount.Value = Me.ltlDiscount.Text
        Me.hdFreeFare.Value = Me.ltlFreeFare.Text
        Me.hdRoadTrip.Value = Me.ltlRoadTrip.Text

    End Sub
    ''' <summary>
    ''' 取得網頁原始碼
    ''' </summary>
    ''' <param name="Url">網址</param>
    ''' <param name="key">主鍵</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetHtml(ByVal Url As String, ByVal key As Integer) As String
        '取得完整的執行中網址(包含特殊的Port)

        ' Create a request for the URL.                 
        Dim SiteDomainName As String = ConfigurationManager.AppSettings("SiteDomainName").ToString
        Dim request As WebRequest = WebRequest.Create(SiteDomainName & Url) '& key)

        request.Credentials = CredentialCache.DefaultCredentials
        ' Get the response.
        Dim responseFromServer As String = ""
        Using response As HttpWebResponse = CType(request.GetResponse(), HttpWebResponse)
            '' Display the status.
            ' Get the stream containing content returned by the server.
            Using dataStream As Stream = response.GetResponseStream()
                ' Open the stream using a StreamReader for easy access.
                Using reader As New StreamReader(dataStream)
                    ' Read the content.
                    responseFromServer = reader.ReadToEnd()
                    '' Display the content.
                    ' Cleanup the streams and the response.
                End Using
            End Using
        End Using
        Return responseFromServer
    End Function

#End Region

    Protected Sub lbtMore_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtMore.Click
        Try
            Dim ListCount As Integer = Session("ListCount")
            If ListCount < 20 Then
                ListCount += 1
                For i As Integer = 6 To ListCount
                    CType(CType(sender, Button).Parent.FindControl("UpdatePanel" & i.ToString), UpdatePanel).Visible = True
                Next
                Session.Remove("ListCount")
                Session.Add("ListCount", ListCount)
            Else
                Dim script As String = "alert('已達最多查詢路段數量!!');"
                ScriptManager.RegisterClientScriptBlock(Me.Page, GetType(Page), "alert", script, True)
            End If
        Catch ex As Exception
            Dim exx As String = ex.Message
        End Try
    End Sub


End Class
