Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Xml
Imports System.Xml.XPath
Partial Class _SuggestedMap
    Inherits InjectionPage
    Public availableTagst As String = ""
    Dim SiteID As String = ConfigurationManager.AppSettings("SiteID").ToString
    Dim ProjectDefault As String = ConfigurationManager.AppSettings("ProjectDefault").ToString
    Dim Injection As Sql_Injection = New Sql_Injection
    Dim Remind As String = ConfigurationManager.AppSettings("SuggestMapRemind").ToString

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        availableTagst = ""

        Dim dtc As DataTable = Fare.GetInterChangeList
        For i As Integer = 0 To dtc.Rows.Count - 1
            availableTagst = availableTagst & ",'(" & dtc.Rows(i)("HWName") & ")" & dtc.Rows(i)("ICName") & "'"
        Next
        If availableTagst.ToString.Length > 0 Then
            availableTagst = availableTagst.Substring(1)
        End If
    End Sub

    Private Function CheckChar(ByVal Injection_chk As String) As Boolean
        Dim InputStr As String = " ,!,$,&,+,=,?"
        Dim chk() As String = InputStr.Split(",")
        Dim i As Integer
        Dim chk2 As String = "true"
        '檢查字串原始型態、urldecode 一次，urldecode 二次 是不是有select inject 字串, 為什麼要兩次呢? 因為有可能會收到被兩次urlencode 的字串
        For i = 0 To UBound(chk)
            If UCase(Injection_chk).Replace(UCase(chk(i)), "") <> UCase(Injection_chk) Or UCase(Server.UrlDecode(Injection_chk)).Replace(UCase(chk(i)), "") <> UCase(Server.UrlDecode(Injection_chk)) Or UCase(Server.UrlDecode(Server.UrlDecode(Injection_chk))).Replace(UCase(chk(i)), "") <> UCase(Server.UrlDecode(Server.UrlDecode(Injection_chk))) Then
                Return False
                chk2 = "false"
            End If
        Next
        If chk2 = "true" Then Return True
    End Function
    Private Function CheckHidden() As Boolean
        If Not Me.routeLines.Value Is String.Empty Then
            If Me.routeLines.Value.Trim.Length > 0 Then
                If Not Regex.IsMatch(Me.routeLines.Value.Replace(",", "").ToString.Trim, "^[0-9]+$") Then
                    Return True
                End If
            End If
        End If
        If Not Me.hdAxisX.Value Is String.Empty Then
            If Me.hdAxisX.Value.Trim.Length > 0 Then
                If Not Regex.IsMatch(Me.hdAxisX.Value.Replace(",", "").ToString.Trim, "^[0-9]+$") Then
                    Return True
                End If
            End If
        End If
        If Not Me.hdAxisY.Value Is String.Empty Then
            If Me.hdAxisY.Value.Trim.Length > 0 Then
                If Not Regex.IsMatch(Me.hdAxisY.Value.Replace(",", "").ToString.Trim, "^[0-9]+$") Then
                    Return True
                End If
            End If
        End If
        'If Not availableTagst Is String.Empty Then
        '    If availableTagst.Trim.Length > 0 Then
        '        If availableTagst.ToString.IndexOf("()") >= 0 Or availableTagst.ToString.IndexOf(":") >= 0 Or availableTagste.ToString.IndexOf(";") >= 0 Then
        '            Return True
        '        End If
        '        If Not CheckChar(availableTagst) Then
        '            Return True
        '        End If
        '    End If
        'End If
        If Not Me.Hidinit.Value Is String.Empty Then
            If Me.Hidinit.Value.Trim.Length > 0 Then
                If Hidinit.Value.Replace("mousedragend();", "").Length > 0 Then
                    Return True
                End If
            End If
        End If


        Return False
    End Function
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.PublishBlock3.BlockID = Remind

        If CheckHidden() Then
            ScriptManager.RegisterClientScriptBlock(Me.Page, GetType(Page), "warning", "alert('系統參數被竄改，請洽系統管理員');document.href='Default.aspx';", True)
            Exit Sub
        End If
        Dim routesl() As String
        Dim para As Boolean = False
        Try
            routesl = Request.Params(routeLines.ClientID.Replace("_", "$")).ToString.ToLower.Replace("%2c", "").Split(",")
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
            Dim dd As String = ex.Message
        End Try
        Try
            routesl = Request.Params("hdAxisX").ToString.ToLower.Replace("%2c", "").Split(",")
            para = False
            For i As Integer = 0 To routesl.Length - 1
                Try
                    If Not IsNumeric(routesl(i)) Then
                        para = True
                        Exit For
                    End If
                    If routesl(i).Trim.Length = 0 Then
                        Continue For
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
            Dim exx As String = ex.Message
        End Try
        Try
            routesl = Request.Params("hdAxisY").ToString.ToLower.Replace("%2c", "").Split(",")
            para = False
            For i As Integer = 0 To routesl.Length - 1
                Try
                    If Not IsNumeric(routesl(i)) Then
                        para = True
                        Exit For
                    End If
                    If routesl(i).Trim.Length = 0 Then
                        Continue For
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
        Try
            routesl = Request.Params(datePicker.ClientID.Replace("_", "$")).ToString.ToLower.Replace("-", "").Replace("/", "").Split(",")
            para = False
            For i As Integer = 0 To routesl.Length - 1
                Try
                    If Not IsNumeric(routesl(i)) Then
                        para = True
                        Exit For
                    End If
                    If routesl(i).Trim.Length = 0 Then
                        Continue For
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
        Dim dtICList As DataTable = Fare.GetInterChangeList()
        If Not IsPostBack Then
            hdAxisX.Value = ""
            hdAxisY.Value = ""
            hidd1.Value = ""
            hid.Value = ""
            Hidinit.Value = ""
            hdFareNormal.Value = ""
            hdFareDiscount.Value = ""
            hdFreeFare.Value = ""
            hdRoadTrip.Value = ""
            hdapDiv5.Value = ""
            hdapDiv6.Value = ""
            hdapDiv7.Value = ""
            routeLines.Value = "0,1,0,"
            Me.MultiView1.ActiveViewIndex = -1

            Me.hdAxisX.Value = "0"
            Me.hdAxisY.Value = "0"
            For i As Integer = 1 To dtICList.Select(" 1=1 ", "UID desc")(0)("UID")
                Dim drow() As DataRow = dtICList.Select(" UID=" & i.ToString)
                If drow.Length > 0 Then
                    Me.hdAxisX.Value &= "," & dtICList.Select(" UID=" & i.ToString)(0)("AxisX")
                    Me.hdAxisY.Value &= "," & dtICList.Select(" UID=" & i.ToString)(0)("AxisY")
                Else
                    Me.hdAxisX.Value &= ",0"
                    Me.hdAxisY.Value &= ",0"
                End If
            Next
            Me.datePicker.Text = Date.Today.ToString("yyyy/MM/dd")
            TextChanged(sender, e)
            Me.Hidinit.Value = "mousedragend();"
        Else
            Me.Hidinit.Value = ""
        End If
        '==========================


        '========================

        Me.MultiView1.ActiveViewIndex = -1

        Me.imgCalculate.Attributes.Add("onkeypress", "javascript:__doPostBack('" & imgCalculate.ClientID.Replace("_", "$") & "','')")
        Me.txtFrom.Attributes.Add("onblur", "if (this.value==''){ this.value='請輸入交流道名稱';}")
        Me.txtFrom.Attributes.Add("onfocus", "if (this.value=='請輸入交流道名稱'){ this.value='';}")
        Me.txtFrom.Attributes.Add("onchange", "textChange(this);")
        Me.txtFrom.Attributes.Add("onkeyup", "textChange(this);")
        Me.txtTo.Attributes.Add("onblur", "if (this.value==''){ this.value='請輸入交流道名稱';}")
        Me.txtTo.Attributes.Add("onfocus", "if (this.value=='請輸入交流道名稱'){ this.value='';}")
        Me.txtTo.Attributes.Add("onchange", "textChange(this);")
        Me.txtTo.Attributes.Add("onkeyup", "textChange(this);")
        Me.txtTransit.Attributes.Add("onblur", "if (this.value==''){ this.value='請輸入交流道名稱';}")
        Me.txtTransit.Attributes.Add("onfocus", "if (this.value=='請輸入交流道名稱'){ this.value='';}")
        Me.txtTransit.Attributes.Add("onchange", "textChange(this);")
        Me.txtTransit.Attributes.Add("onkeyup", "textChange(this);")
        Me.ddlTimePediod.Attributes.Add("onchange", "document.getElementById('" & Me.hidd1.ClientID & "').value = this.value;__doPostBack('" & ddlTimePediod.ClientID.Replace("_", "$") & "','');")
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
        Me.MultiView1.ActiveViewIndex = -1
    End Sub

    Private Function GetHighWay(ByVal HWNameTo As String) As DataTable
        Dim HWStop As Integer = 0
        If HWNameTo.IndexOf("國道一高架") >= 0 Or HWNameTo.IndexOf("國道1高架") >= 0 Or HWNameTo.IndexOf("國一高架") >= 0 Or HWNameTo.IndexOf("國1高架") >= 0 Then
            HWNameTo = HWNameTo.Replace("(國1高架)", "").Replace("國道一高架", "").Replace("國道1高架1", "").Replace("國一高架", "").Replace("國1高架", "")
            HWStop = 11
        ElseIf HWNameTo.IndexOf("國道十") >= 0 Or HWNameTo.IndexOf("國道10") >= 0 Or HWNameTo.IndexOf("國十") >= 0 Or HWNameTo.IndexOf("國10") >= 0 Then
            HWNameTo = HWNameTo.Replace("(國道10)", "").Replace("國道十", "").Replace("國道10", "").Replace("國十", "").Replace("國10", "")
            HWStop = 10
        ElseIf HWNameTo.IndexOf("國道一") >= 0 Or HWNameTo.IndexOf("國道1") >= 0 Or HWNameTo.IndexOf("國一") >= 0 Or HWNameTo.IndexOf("國1") >= 0 Then
            HWNameTo = HWNameTo.Replace("(國道1)", "").Replace("國道一", "").Replace("國道1", "").Replace("國一", "").Replace("國1", "")
            HWStop = 1
        ElseIf HWNameTo.IndexOf("國道二") >= 0 Or HWNameTo.IndexOf("國道2") >= 0 Or HWNameTo.IndexOf("國二") >= 0 Or HWNameTo.IndexOf("國2") >= 0 Then
            HWNameTo = HWNameTo.Replace("(國道2)", "").Replace("國道二", "").Replace("國道2", "").Replace("國二", "").Replace("國2", "")
            HWStop = 2
        ElseIf HWNameTo.IndexOf("國道三甲") >= 0 Or HWNameTo.IndexOf("國道3甲") >= 0 Or HWNameTo.IndexOf("國三甲") >= 0 Or HWNameTo.IndexOf("國3甲") >= 0 Then
            HWNameTo = HWNameTo.Replace("(國3甲)", "").Replace("國道三甲", "").Replace("國道3甲", "").Replace("國三甲", "").Replace("國3甲", "")
            HWStop = 12
        ElseIf HWNameTo.IndexOf("國道三") >= 0 Or HWNameTo.IndexOf("國道3") >= 0 Or HWNameTo.IndexOf("國三") >= 0 Or HWNameTo.IndexOf("國3") >= 0 Then
            HWNameTo = HWNameTo.Replace("(國道3)", "").Replace("國道三", "").Replace("國道3", "").Replace("國三", "").Replace("國3", "")
            HWStop = 3
        ElseIf HWNameTo.IndexOf("國道四") >= 0 Or HWNameTo.IndexOf("國道4") >= 0 Or HWNameTo.IndexOf("國四") >= 0 Or HWNameTo.IndexOf("國4") >= 0 Then
            HWNameTo = HWNameTo.Replace("(國道4)", "").Replace("國道四", "").Replace("國道4", "").Replace("國四", "").Replace("國4", "")
            HWStop = 4
        ElseIf HWNameTo.IndexOf("國道五") >= 0 Or HWNameTo.IndexOf("國道5") >= 0 Or HWNameTo.IndexOf("國五") >= 0 Or HWNameTo.IndexOf("國5") >= 0 Then
            HWNameTo = HWNameTo.Replace("(國道5)", "").Replace("國道五", "").Replace("國道5", "").Replace("國五", "").Replace("國5", "")
            HWStop = 5
        ElseIf HWNameTo.IndexOf("國道六") >= 0 Or HWNameTo.IndexOf("國道6") >= 0 Or HWNameTo.IndexOf("國六") >= 0 Or HWNameTo.IndexOf("國6") >= 0 Then
            HWNameTo = HWNameTo.Replace("(國道6)", "").Replace("國道六", "").Replace("國道6", "").Replace("國六", "").Replace("國6", "")
            HWStop = 6
        ElseIf HWNameTo.IndexOf("國道八") >= 0 Or HWNameTo.IndexOf("國道8") >= 0 Or HWNameTo.IndexOf("國八") >= 0 Or HWNameTo.IndexOf("國8") >= 0 Then
            HWNameTo = HWNameTo.Replace("(國道8)", "").Replace("國道八", "").Replace("國道8", "").Replace("國八", "").Replace("國8", "")
            HWStop = 8
        End If
        HWNameTo = HWNameTo.Replace(" ", "").Replace("<", "").Replace(">", "").Replace("/", "").Replace("-", "").Replace("_", "") '.replace("(","").replace(")","")
        Dim dt As DataTable = New DataTable
        dt.Columns.Add(New DataColumn("HWName"))
        dt.Columns.Add(New DataColumn("HWValue"))
        Dim drow As DataRow = dt.NewRow
        drow("HWName") = HWNameTo
        drow("HWValue") = HWStop
        dt.Rows.Add(drow)
        Return dt
    End Function
    Protected Sub lkbtCalculate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lkbtCalculate.Click
        DoCalculate()
    End Sub
    Protected Sub imgCalculate_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgCalculate.Click
        DoCalculate()
    End Sub
    Private Sub DoCalculate()

        Dim scriptsa As String = ""
        If Me.txtFrom.Text.Trim().Length = 0 Or Me.txtFrom.Text.Equals("請輸入交流道名稱") Then
            scriptsa = "alert('請輸入起點交流道名稱!');"
        ElseIf Me.txtTo.Text.Trim.Length = 0 Or Me.txtTo.Text.Equals("請輸入交流道名稱") Then
            scriptsa = "alert('請輸入訖點交流道名稱!');"
            'ElseIf Me.txtTransit.Text.Trim.Length = 0 Or Me.txtTransit.Text.Equals("請輸入交流道名稱") Then
            '    scriptsa = "alert('請輸入經過點交流道名稱!');"
            'ElseIf Me.FareType.SelectedIndex = 0 Then
            '    scriptsa = "alert('請選擇繳費方式!');"
        End If
        If scriptsa.Length > 0 Then
            ScriptManager.RegisterClientScriptBlock(Me.Page, GetType(Page), "warning", scriptsa, True)
            Exit Sub
        End If
        Dim selectdate As DateTime = New Date(Me.datePicker.Text.Split("/")(0), Me.datePicker.Text.Split("/")(1), Me.datePicker.Text.Split("/")(2))
        Dim DateLastYear As Integer = Convert.ToInt32(DateAdd(DateInterval.Year, -1, Date.Now).ToString("yyyyMM")) * 100 + 1
        Dim DateNow As Integer = Convert.ToInt32(selectdate.ToString("yyyyMMdd"))
        If DateNow < DateLastYear Then
            ScriptManager.RegisterClientScriptBlock(Me.Page, GetType(Page), "warning", "alert('日期區間不能早於距今日起一年以前!!');", True)
            Exit Sub
        End If

        Dim strSql As String = "select * from interchangeFare where StartIC = @StartIC and StopIC = @StopIC"
        Dim HWStart As Integer = 0
        Dim HWStop As Integer = 0
        Dim HWTo As Integer = 0
        Dim StartIC As Integer = 0
        Dim StopIC As Integer = 0
        Dim TotalMiles As Double = 0
        Dim ChargeMiles As Double = 0
        Dim RoadTrip As String = ""
        Dim i As Integer = 0
        Dim HWNameFrom As String = Me.txtFrom.Text
        Dim HWNameTo As String = Me.txtTo.Text
        Dim ICHWNameFrom As String = ""
        Dim ICNameFrom As String = ""


        Dim TransitIC As Integer = 0
        Dim HWTransit As Integer = 0
        Dim HWNameTransit As String = Me.txtTransit.Text

        Dim dtHW As DataTable = GetHighWay(HWNameFrom)
        HWNameFrom = dtHW.Rows(0)("HWName")
        HWStart = dtHW.Rows(0)("HWValue")
        dtHW = GetHighWay(HWNameTo)
        HWNameTo = dtHW.Rows(0)("HWName")
        HWStop = dtHW.Rows(0)("HWValue")
        If HWNameTransit.Length > 0 And HWNameTransit.IndexOf("請輸入") < 0 Then
            dtHW = GetHighWay(HWNameTransit)
            HWNameTransit = dtHW.Rows(0)("HWName")
            HWTransit = dtHW.Rows(0)("HWValue")
        Else
            HWNameTransit = ""
            HWTransit = 0
        End If

        Dim FilterFrom As String = ""
        Dim FilterTo As String = ""
        Dim FilterTransit As String = ""

        Dim dtIC As DataTable = Fare.GetInterChangeList()

        If HWStart <> 0 Then
            FilterFrom = FilterFrom & " HWUID = " & HWStart.ToString & " and ICName = '" & HWNameFrom & "'" '.Replace(")", "").Replace("(", "").Replace(" ", "").Replace("_", "").Replace("-", "").Replace(":", "") & "%'"
        Else
            FilterFrom = " ICName = '" & HWNameFrom & "'"
        End If
        If HWStop <> 0 Then
            FilterTo = FilterTo & " HWUID = " & HWStop.ToString & " and ICName = '" & HWNameTo & "'" '.Replace(")", "").Replace("(", "").Replace(" ", "").Replace("_", "").Replace("-", "").Replace(":", "") & "%'"
        Else
            FilterTo = " ICName = '" & HWNameTo & "'"
        End If

        If HWTransit <> 0 Then
            FilterTransit = FilterTransit & " HWUID = " & HWTransit.ToString & " and ICName = '" & HWNameTransit & "'" '.Replace(")", "").Replace("(", "").Replace(" ", "").Replace("_", "").Replace("-", "").Replace(":", "") & "%'"
        Else
            FilterTransit = " ICName = '" & HWNameTransit & "'"
        End If

        Dim drowFrom() As DataRow = dtIC.Select(FilterFrom)
        If drowFrom.Length > 1 Then
            drowFrom = dtIC.Select(FilterFrom.Replace("LIKE", "=").Replace("%'", "'"))
            If drowFrom Is Nothing Or drowFrom.Length = 0 Then
                drowFrom = dtIC.Select(FilterFrom)
            End If
        End If

        Dim drowTo() As DataRow = dtIC.Select(FilterTo)
        Dim drowTransit() As DataRow = dtIC.Select(FilterTransit)

        Dim streng As String = ""
        If drowFrom.Length = 0 Then
            streng = ",交流道(起點)"
        End If
        If drowTo.Length = 0 Then
            streng &= ",交流道(訖點)"
        End If
        'If drowTransit.Length = 0 Then
        '    streng &= ",交流道(經過點)"
        'End If
        If streng.Length > 0 Then
            streng = " alert('" & streng.Substring(1).Replace(",", "與") & "無法辨識，請重新輸入。請參考提示框建議!!');"
            ScriptManager.RegisterClientScriptBlock(Me.Page, GetType(Page), "Error", streng, True)
            Exit Sub
        End If
        StartIC = drowFrom(0)("UID")
        HWStart = drowFrom(0)("HWUID")
        StopIC = drowTo(0)("UID")
        HWStop = drowTo(0)("HWUID")
        Try
            TransitIC = drowTransit(0)("UID")
            HWTransit = drowTransit(0)("HWUID")

        Catch ex As Exception

        End Try

        If TransitIC = 0 And StartIC = StopIC Then
            ScriptManager.RegisterClientScriptBlock(Me.Page, GetType(Page), "same", "alert('交流道(起點)與交流道(訖點)不能相同');", True)
            Exit Sub
        End If
        If TransitIC = StartIC Or TransitIC = StopIC Then
            ScriptManager.RegisterClientScriptBlock(Me.Page, GetType(Page), "same", "alert('交流道(起點或訖點)與交流道(經過點)不能相同');", True)
            Exit Sub
        End If
        If dtIC.Select(" UID = " & StartIC.ToString & " and (InSouth =0 and InNorth = 0)").Length > 0 Then
            ScriptManager.RegisterClientScriptBlock(Me.Page, GetType(Page), "same", "alert('交流道起點無法通行。請重新輸入!!');", True)
            Exit Sub
        End If
        If dtIC.Select(" UID = " & StopIC.ToString & " and (OutSouth =0 and OutNorth = 0)").Length > 0 Then
            ScriptManager.RegisterClientScriptBlock(Me.Page, GetType(Page), "same", "alert('交流道訖點點無法通行。請重新輸入!!');", True)
            Exit Sub
        End If

        Dim FullRoad As String = ""
        Dim routeConnectName As String = ""

        Dim TotalFare As Double = 0
        Dim TotalFareM As Double = 0
        Dim TotalFareG As Double = 0
        Dim TotalFareDiff As Double = 0
        Dim TotalFareMDiff As Double = 0
        Dim TotalFareGDiff As Double = 0
        Dim TotalFareAdd As Double = 0
        Dim TotalFareMAdd As Double = 0
        Dim TotalFareGAdd As Double = 0
        Dim Route1 As String = ""
        Dim Route2 As String = ""
        Dim RealStart As Integer = StartIC
        Dim strSqlt As String = ""
        Dim dt As DataTable = New DataTable
        Dim FareList As DataTable = Fare.GetFareListByProjectID(Me.ddlTimePediod.SelectedValue)
        Dim FreeList As DataTable = Fare.GetInterChangeStartFree()
        Dim ProjectList As DataTable = Fare.GetProjectByProjectID(Me.ddlTimePediod.SelectedValue)

        If TransitIC <> 0 Then
            dt = FareCalcaulate(StartIC, TransitIC, HWStart, HWTransit)
            If dt.Rows.Count = 0 Then
                Dim scripts As String = "alert('" & "起點交流道無法到達經過點交流道" & "\n故未能抵達，請重新選擇路徑!!')"
                ScriptManager.RegisterClientScriptBlock(Me.Page, GetType(Page), "error", scripts, True)
                Exit Sub
            End If
            TotalMiles += dt.Rows(0)("TotalMiles")
            ChargeMiles += dt.Rows(0)("ChargeMiles")
            Dim BestRouteName1 As String = ""
            Dim BestConnectName1 As String = ""
            BestRouteName1 = getRouteName(dt.Rows(0)("Route"))
            BestRouteName1 = BestRouteName1.Split(",")(0)
            BestRouteName1 = "從" & HWName(dtIC.Select("UID=" & StartIC)(0)("HWUID")) & " " & dtIC.Select("UID=" & StartIC)(0)("ICName") & " 到 " & HWName(dtIC.Select("UID=" & TransitIC)(0)("HWUID")) & dtIC.Select("UID=" & TransitIC)(0)("ICName") & "<br/> 詳細路線:<br/>" & BestRouteName1.Substring("&#8594;".Length) & "<br/>" & "路段二："
            RoadTrip &= "路段一：" & BestRouteName1 ' dt.Rows(0)("RouteName").Replace("(,", "<br />(").Replace("&#8594;)", ")").Replace("&#8594;,", "&#8594;").Replace("->)", ")").Replace("->,", "->").Replace("->", "&#8594;")
            RoadTrip = "從" & HWName(dtIC.Select("UID=" & StartIC)(0)("HWUID")) & " " & dtIC.Select("UID=" & StartIC)(0)("ICName") & " 經過 " & HWName(dtIC.Select("UID=" & TransitIC)(0)("HWUID")) & dtIC.Select("UID=" & TransitIC)(0)("ICName") & " 到 " & HWName(dtIC.Select("UID=" & StopIC)(0)("HWUID")) & dtIC.Select("UID=" & StopIC)(0)("ICName") & "<br/> <br/>" & RoadTrip

            TotalFare += dt.Rows(0)("BestFare")
            TotalFareM += dt.Rows(0)("BestFareM")
            TotalFareG += dt.Rows(0)("BestFareG")
            TotalFareDiff += dt.Rows(0)("BestFareDiff")
            TotalFareMDiff += dt.Rows(0)("BestFareMDiff")
            TotalFareGDiff += dt.Rows(0)("BestFareGDiff")
            TotalFareAdd += dt.Rows(0)("BestFareAdd")
            TotalFareMAdd += dt.Rows(0)("BestFareMAdd")
            TotalFareGAdd += dt.Rows(0)("BestFareGAdd")
            StartIC = TransitIC
            HWStart = HWTransit
            Route1 = dt.Rows(0)("Route")
        End If
        RouteFares = Fare.GetMaxFare()
        dt = FareCalcaulate(StartIC, StopIC, HWStart, HWStop)
        If dt.Rows.Count = 0 Then
            If StartIC = TransitIC Then
                Dim scripts As String = "alert('" & "經過點交流道無法到達訖點點交流道" & "\n故未能抵達，請重新選擇路徑!!')"
                ScriptManager.RegisterClientScriptBlock(Me.Page, GetType(Page), "error", scripts, True)
                Exit Sub
            Else
                Dim scripts As String = "alert('" & "起點交流道無法到達訖點點交流道" & "\n故未能抵達，請重新選擇路徑!!')"
                ScriptManager.RegisterClientScriptBlock(Me.Page, GetType(Page), "error", scripts, True)
                Exit Sub
            End If
        End If

        TotalMiles += dt.Rows(0)("TotalMiles")
        ChargeMiles += dt.Rows(0)("ChargeMiles")
        'routeConnectName &= dt.Rows(0)("RouteName").Replace("(,", "<br />(").Replace("&#8594;)", ")").Replace("&#8594;,", "&#8594;").Replace("->)", ")").Replace("->,", "->").Replace("->", "&#8594;")
        'RoadTrip &= dt.Rows(0)("RouteName").Replace("(,", "<br />(").Replace("&#8594;)", ")").Replace("&#8594;,", "&#8594;").Replace("->)", ")").Replace("->,", "->").Replace("->", "&#8594;")
        '路徑
        Dim BestRouteName As String = ""
        Dim BestConnectName As String = ""
        BestRouteName = getRouteName(dt.Rows(0)("Route"))
        BestRouteName = BestRouteName.Split(",")(0)
        BestRouteName = "從" & HWName(dtIC.Select("UID=" & StartIC)(0)("HWUID")) & " " & dtIC.Select("UID=" & StartIC)(0)("ICName") & " 到 " & HWName(dtIC.Select("UID=" & StopIC)(0)("HWUID")) & dtIC.Select("UID=" & StopIC)(0)("ICName") & "<br/> 詳細路線:<br/>" & BestRouteName.Substring("&#8594;".Length) & "<br/><br/>"
        RoadTrip &= BestRouteName ' dt.Rows(0)("RouteName").Replace("(,", "<br />(").Replace("&#8594;)", ")").Replace("&#8594;,", "&#8594;").Replace("->)", ")").Replace("->,", "->").Replace("->", "&#8594;")

        TotalFare += dt.Rows(0)("BestFare")
        TotalFareM += dt.Rows(0)("BestFareM")
        TotalFareG += dt.Rows(0)("BestFareG")
        TotalFareDiff += dt.Rows(0)("BestFareDiff")
        TotalFareMDiff += dt.Rows(0)("BestFareMDiff")
        TotalFareGDiff += dt.Rows(0)("BestFareGDiff")
        TotalFareAdd += dt.Rows(0)("BestFareAdd")
        TotalFareMAdd += dt.Rows(0)("BestFareMAdd")
        TotalFareGAdd += dt.Rows(0)("BestFareGAdd")

        Dim FareNormal As Double = ProjectList.Rows(0)("FareS")
        Dim FareDiscount As Double = ProjectList.Rows(0)("FareSDiscount")
        Dim RoadList As String = BestRouteName

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
        RoadTrip = RoadTrip & "總計費里程: <font color=blue >" & IIf(Format(ChargeMiles, "##.#") = "", "0.0", Format(ChargeMiles, "##.#")) & "</font>公里"
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
        'Me.ltlProjectInform.Text = "<font color=red>【" & ProjectList.Rows(0)("MainProjectName").ToString & "】</font>"
        If Not IsDBNull(ProjectList.Rows(0)("Notes")) Then
            If ProjectList.Rows(0)("Notes").ToString.Trim.Length > 0 Then
                Me.ltlProjectNotes.Text = "<p class=""time"">【收費說明：" & ProjectList.Rows(0)("Notes").ToString & "】</p>"
            End If
        End If

        '紀錄LOG
        Dim UserIP As String = Request.ServerVariables("REMOTE_ADDR")
        Dim scriptss As String = "document.location.href='#buttom'"
        If Not CheckSqlInjectionWording(UserIP.Replace(":", "").Replace(".", "").Replace("-", "").ToString.Trim) And Regex.IsMatch(UserIP.Replace(":", "").Replace(".", "").Replace("-", "").ToString.Trim, "^[0-9]+$") Then
            UserIP = RemoveSqlInjection(UserIP)
            Dim SessionID As String = Session("SessionID")
            SessionID = RemoveSqlInjection(SessionID)
            'ClassDB.UpdateDB("farecalculatorLogInsert",
            '                 New SqlParameter("SiteID", 1),
            '                 New SqlParameter("UserType", 2),
            '                 New SqlParameter("UserIP", UserIP),
            '                 New SqlParameter("SessionID", SessionID),
            '                 New SqlParameter("RoadTrip", RemoveSqlInjection(RoadTrip))
            '                 )
            ClassDB.UpdateDB("farecalculatorLogInsert",
                             New SqlParameter("SiteID", 1),
                             New SqlParameter("UserType", 2),
                             New SqlParameter("UserIP", ""),
                             New SqlParameter("SessionID", ""),
                             New SqlParameter("RoadTrip", "")
                             )
            Me.MultiView1.ActiveViewIndex = 0
            'Dim sqlStr As String = "update FareCalculateCount set MapCount = MapCount + 1, LastUpdateMap = getdate() where SiteID =  1 and convert(varchar(20), getdate(), 112) = LastUpdateDateTime"
            'ClassDB.UpdateDB(sqlStr)
            Fare.FindFooter(Me.Controls)
            ReturnRouteLines(Route1 & IIf(Route1.Length = 0, "", ",") & dt.Rows(0)("Route"))
            Me.hid.Value = RealStart.ToString
            Me.hdapDiv5.Value = (dtIC.Select("UID=" & RealStart.ToString)(0)("AxisX")).ToString & "," & (dtIC.Select("UID=" & RealStart.ToString)(0)("AxisY")).ToString
            Me.hdapDiv6.Value = (dtIC.Select("UID=" & StopIC.ToString)(0)("AxisX")).ToString & "," & (dtIC.Select("UID=" & StopIC.ToString)(0)("AxisY")).ToString
            If TransitIC <> 0 Then
                Me.hdapDiv7.Value = (dtIC.Select("UID=" & TransitIC.ToString)(0)("AxisX")).ToString & "," & (dtIC.Select("UID=" & TransitIC.ToString)(0)("AxisY")).ToString
            Else
                Me.hdapDiv7.Value = ""
            End If
        Else
            scriptss = "alert('請不要攻擊本網站!!');"
        End If
        ScriptManager.RegisterClientScriptBlock(Me.UpdatePanel2, GetType(UpdatePanel), "start", scriptss, True)
    End Sub
#Region "計算路徑"
    Function ReturnRouteLines(ByVal Route As String) As String
        Dim dtIC As DataTable = Fare.GetInterChangeList

        Dim routeline As String = ""
        'Dim Route As String = dt.Rows(0)("Route")
        Dim routes() As String = Route.Split(",")
        Dim StartIC As Integer = 0
        Dim dtBetween As DataTable = Context.Cache("InterChangeRouteList")
        If dtBetween Is Nothing Then dtBetween = Fare.GetInterChangeRouteList()
        For i As Integer = 0 To routes.Length - 1
            Dim RouteBetween As String = ""
            Dim StopIC As Integer = routes(i)
            Dim sIC As Integer = StartIC
            Dim eIC As Integer = StopIC
            Dim StartItemOrder As Integer = 0
            Dim StartHW As Integer = 0
            If i > 0 Then
                StartHW = dtIC.Select("UID=" & StartIC.ToString)(0)("HWUID")
                StartItemOrder = dtIC.Select("UID=" & StartIC.ToString)(0)("ItemOrder")
            End If
            Dim StopItemOrder As Integer = dtIC.Select("UID=" & StopIC.ToString)(0)("ItemOrder")
            Dim StopHW As Integer = dtIC.Select("UID=" & StopIC.ToString)(0)("HWUID")
            Dim sOrder As Integer = StartItemOrder
            Dim eOrder As Integer = StopItemOrder
            If StartIC <> 0 Then
                If StartHW = StopHW Then
                    If StartItemOrder > StopItemOrder Then
                        sOrder = StopItemOrder
                        eOrder = StartItemOrder
                        sIC = StopIC
                        eIC = StartIC
                    End If
                    Dim tRoutes() As DataRow = dtIC.Select("HWUID=" & StartHW.ToString & " and ItemOrder >" & sOrder.ToString & " and ItemOrder < " & eOrder.ToString, " ItemOrder" & IIf(StartItemOrder > StopItemOrder, " desc", ""))
                    For k As Integer = 0 To tRoutes.Length - 1
                        Try
                            Dim bRoutes() As DataRow = dtBetween.Select("StartIC = " & sIC.ToString & " and StopIC = " & tRoutes(k)("UID").ToString, "ItemOrder" & IIf(StartItemOrder > StopItemOrder, " desc", ""))
                            For j As Integer = 0 To bRoutes.Length - 1
                                If bRoutes(j)("AxisX") > 0 Or bRoutes(j)("AxisX") > 0 Then
                                    routeline += "," & bRoutes(j)("AxisX").ToString
                                    routeline += "," & bRoutes(j)("AxisY").ToString
                                End If
                            Next
                        Catch ex As Exception
                        End Try
                        If tRoutes(k)("AxisX") > 0 Or tRoutes(k)("AxisY") > 0 Then
                            routeline += "," & tRoutes(k)("AxisX").ToString
                            routeline += "," & tRoutes(k)("AxisY").ToString
                        End If
                        sIC = tRoutes(k)("UID")
                    Next
                End If
            End If
            Try
                Dim dRoutes() As DataRow = dtBetween.Select("StartIC = " & sIC.ToString & " and StopIC = " & eIC.ToString, "ItemOrder" & IIf(StartItemOrder > StopItemOrder, " desc", ""))
                For j As Integer = 0 To dRoutes.Length - 1
                    If dRoutes(j)("AxisX") > 0 Or dRoutes(j)("AxisX") > 0 Then
                        routeline += "," & dRoutes(j)("AxisX").ToString
                        routeline += "," & dRoutes(j)("AxisY").ToString
                    End If
                Next
            Catch ex As Exception
            End Try
            routeline += "," & dtIC.Select(" UID =" & StopIC.ToString)(0)("AxisX").ToString
            routeline += "," & dtIC.Select(" UID =" & StopIC.ToString)(0)("AxisY").ToString
            StartIC = StopIC
            StartItemOrder = StopItemOrder
            StartHW = StopHW
        Next
        routeline = "0,1,0," & ((routeline.Split(",").Length - 1) / 2).ToString & routeline
        routeLines.Value = routeline
        Return routeline
    End Function
#End Region
#Region "計算費率"
    Dim RoutePass As String = ""
    Dim RouteFares As Double = Fare.GetMaxFare()
    Dim RouteSearch As Double = 0
    Dim ICTurn As String = ","
    Dim HWTurn As String = ","

    Private HWName() As String = Fare.HWListArray
    Private Function FareCalcaulate(ByVal StartIC As Integer, ByVal StopIC As Integer, ByVal StartHW As Integer, ByVal StopHW As Integer) As DataTable
        RoutePass = ""
        '先把所有的交流道選出來

        Dim dtIC As DataTable = Fare.GetInterChangeListwithHWNameandByProjectID(Me.ddlTimePediod.SelectedValue.ToString)
        Dim dtRelation As DataTable = Fare.GetInterChangeRelationDistint
        Dim dtProhibit As DataTable = Fare.GetInterChangeProhibitRoute
        Dim prohibitStr As String = Fare.GetInterChangeProhibitRouteString()
        Dim i As Integer = 0
        Dim StartTime As Date = Date.Now
        Dim StopTime As Date = Date.Now
        RouteSearch = 0
        StartTime = Date.Now
        RoutePass = ""
        '找出起訖點的國道代碼和公里數和排序
        Dim dtSelectStartIC() As DataRow = dtIC.Select(" UID = " & StartIC.ToString)
        Dim dtSelectStopIC() As DataRow = dtIC.Select(" UID = " & StartIC.ToString)
        Dim StartICMiles As Double = IIf(dtSelectStartIC.Length > 0, dtSelectStartIC(0)("ICMiles"), 0)
        Dim StopICMiles As Double = IIf(dtSelectStopIC.Length > 0, dtSelectStopIC(0)("ICMiles"), 0)
        Dim StartItemOrder As Integer = IIf(dtSelectStartIC.Length > 0, dtSelectStartIC(0)("ItemOrder"), 0)
        Dim StopItemOrder As Integer = IIf(dtSelectStopIC.Length > 0, dtSelectStopIC(0)("ItemOrder"), 0)

        Dim Route As String = "," & StartIC & ","
        Dim RouteF As Double = 0
        Dim isBestFare As Boolean = False
        'Using dr As SqlDataReader = ClassDB.GetDataReaderParam("Select Routes from InterchangeBestFare where StartIC = @StartIC and StopIC = @StopIC", New SqlParameter("@StartIC", StartIC), New SqlParameter("@StopIC", StopIC))
        '    Try
        '        If dr.Read Then
        '            isBestFare = True
        '            RoutePass = "#," & dr("Routes").ToString & ","
        '        End If
        '    Catch ex As Exception
        '    End Try
        'End Using
        If Not isBestFare Then
            Try

                Dim Starts As DateTime = Date.Now
                getNextRoute(StartIC, StopIC, StartIC, dtRelation, dtIC, prohibitStr, Route, RouteF)
                Dim Ends As DateTime = Date.Now
                Dim ts As TimeSpan = Ends - Starts
                Dim Times As String = Format((ts.TotalMilliseconds / 1000), "##.###")
                If Times.IndexOf(".") = 0 Then
                    Times = "0" & Times
                End If
                Me.ltlSearchTime.Text = "查詢耗時：" & Times & "秒"

                'maxsearch += 1
                If RoutePass.Length = 0 Then
                    Fare.SetMaxFare(RouteFares)
                    RouteFares = Fare.GetMaxFare()
                End If
                'End While
            Catch ex As Exception
                Return New DataTable
            Finally
            End Try
        End If
        Dim RouteTable As DataTable = Fare.FareColumn()

        If RoutePass.Length = 0 Then
            Return New DataTable
        End If
        Dim FareList As DataTable = Fare.GetFareListByProjectID(Me.ddlTimePediod.SelectedValue)
        Dim FreeList As DataTable = Fare.GetInterChangeStartFree()
        Dim ProjectList As DataTable = Fare.GetProjectByProjectID(Me.ddlTimePediod.SelectedValue)

        Dim RouteArray() As String = RoutePass.Substring(1).Split("#")
        i = 1
        For jj As Integer = 0 To RouteArray.Length - 1
            Route = RouteArray(jj)
            If RouteArray(jj).IndexOf("," & StartIC & ",") < 0 Or RouteArray(jj).IndexOf("," & StopIC & ",") < 0 Then
                Continue For
            End If
            Dim InterChangeList() As String = RouteArray(jj).Substring(1, RouteArray(jj).Length - 2).Split(",")
            If InterChangeList.Length < 2 Then
                Continue For
            End If


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
            Dim str As String = ""

            For kk As Integer = 0 To InterChangeList.Length - 2
                Dim oHW As Integer = dtIC.Select(" UID=" & InterChangeList(kk))(0)("HWUID")
                Dim sHW As Integer = dtIC.Select(" UID=" & InterChangeList(kk + 1))(0)("HWUID")
                Dim sICUID As Integer = dtIC.Select(" UID=" & InterChangeList(kk))(0)("UID")
                Dim eICUID As Integer = dtIC.Select(" UID=" & InterChangeList(kk + 1))(0)("UID")
                Dim StartOrder As Integer = dtIC.Select(" UID=" & InterChangeList(kk))(0)("ItemOrder")
                Dim EndOrder As Integer = dtIC.Select(" UID=" & InterChangeList(kk + 1))(0)("ItemOrder")
                If oHW = sHW Then
                    str &= sICUID & ","
                    If FreeList.Select(" StartIC=" & sICUID & " and StopIC = " & eICUID.ToString & " and FreeType = 4").Length <= 0 And FreeList.Select(" StartIC=" & eICUID & " and StopIC = " & sICUID & " and FreeType = 4").Length <= 0 Then
                        Dim srow As DataRow = dtIC.Select(" UID = " & sICUID.ToString)(0)
                        Dim erow As DataRow = dtIC.Select(" UID = " & eICUID.ToString)(0)
                        If StartOrder > EndOrder Then
                            If FreeList.Select(" StartIC=" & sICUID.ToString & " and StopIC = " & eICUID & " and FreeType = 3").Length <= 0 And (StartIC <> sICUID Or FreeList.Select(" StartIC=" & sICUID.ToString & " and StopIC = " & eICUID & " and FreeType = 0").Length <= 0) And (StopIC <> eICUID Or FreeList.Select(" StartIC=" & sICUID.ToString & " and StopIC = " & eICUID & " and FreeType = 1").Length = 0) Then
                                tMiles += Math.Abs(Double.Parse(srow("ICMilesN").ToString) - Double.Parse(erow("ICMilesN").ToString))
                                cMiles += Double.Parse(dtIC.Select("UID=" & sICUID)(0)("ICMilesBetweenN").ToString)
                                cFares += srow("FareSN")
                                cFaresM += srow("FareMN")
                                cFaresG += srow("FareGN")
                                cFaresDiff += srow("FareSDiffN")
                                cFaresMDiff += srow("FareMDiffN")
                                cFaresGDiff += srow("FareGDiffN")
                                cFaresAdd += srow("FareSAddN")
                                cFaresMAdd += srow("FareMAddN")
                                cFaresGAdd += srow("FareGAddN")
                            End If
                        Else
                            If FreeList.Select(" StartIC=" & sICUID.ToString & " and StopIC = " & eICUID & " and FreeType = 2").Length <= 0 And (StartIC <> sICUID Or FreeList.Select(" StartIC=" & sICUID.ToString & " and StopIC = " & eICUID & " and FreeType = 0").Length <= 0) And (StopIC <> eICUID Or FreeList.Select(" StartIC=" & sICUID.ToString & " and StopIC = " & eICUID & " and FreeType = 1").Length = 0) Then
                                tMiles += Math.Abs(Double.Parse(srow("ICMiles").ToString) - Double.Parse(erow("ICMiles").ToString))
                                cMiles += Double.Parse(erow("ICMilesBetween").ToString)
                                cFares += erow("FareS")
                                cFaresM += erow("FareM")
                                cFaresG += erow("FareG")
                                cFaresDiff += erow("FareSDiff")
                                cFaresMDiff += erow("FareMDiff")
                                cFaresGDiff += erow("FareGDiff")
                                cFaresAdd += erow("FareSAdd")
                                cFaresMAdd += erow("FareMAdd")
                                cFaresGAdd += erow("FareGAdd")
                            End If
                        End If
                    End If
                End If
            Next
            Dim drows As DataRow = RouteTable.NewRow
            drows("UID") = i
            drows("Route") = RouteArray(jj)
            drows("TotalMiles") = Math.Round(tMiles, 4)
            drows("ChargeMiles") = Math.Round(cMiles, 2)
            drows("BestFare") = Math.Round(cFares, 2)
            drows("BestFareM") = Math.Round(cFaresM, 2)
            drows("BestFareG") = Math.Round(cFaresG, 2)
            drows("BestFareDiff") = Math.Round(cFaresDiff, 2)
            drows("BestFareMDiff") = Math.Round(cFaresMDiff, 2)
            drows("BestFareGDiff") = Math.Round(cFaresGDiff, 2)
            drows("BestFareAdd") = Math.Round(cFaresAdd, 2)
            drows("BestFareMAdd") = Math.Round(cFaresMAdd, 2)
            drows("BestFareGAdd") = Math.Round(cFaresGAdd, 2)
            drows("BestTotalFare") = Math.Round(cFares - cFaresDiff + cFaresGAdd, 2)
            RouteTable.Rows.Add(drows)
            i += 1
        Next

        If RouteTable.Rows.Count = 0 Then
            Return RouteTable
        End If
        Dim BestTotalFare As Double = IIf(RouteTable.Rows.Count = 0, 0, RouteTable.Compute("min(BestTotalFare)", String.Empty))
        Dim UID As Double = IIf(RouteTable.Rows.Count = 0, 0, RouteTable.Select("BestTotalFare=" & BestTotalFare)(0)("UID"))
        Dim ChargeFares As Double = IIf(RouteTable.Rows.Count = 0, 0, RouteTable.Rows(UID - 1)("BestFare"))
        Dim ChargeFaresM As Double = IIf(RouteTable.Rows.Count = 0, 0, RouteTable.Rows(UID - 1)("BestFareM"))
        Dim ChargeFaresG As Double = IIf(RouteTable.Rows.Count = 0, 0, RouteTable.Rows(UID - 1)("BestFareG"))
        Dim ChargeMiles As Double = IIf(RouteTable.Rows.Count = 0, 0, RouteTable.Rows(UID - 1)("ChargeMiles"))
        '還原牌價
        ChargeFares = IIf(RouteTable.Rows.Count = 0, 0, RouteTable.Rows(UID - 1)("BestFare"))

        Dim ChargeFaresDiff As Double = IIf(RouteTable.Rows.Count = 0, 0, RouteTable.Rows(UID - 1)("BestFareDiff"))
        Dim ChargeFaresMDiff As Double = IIf(RouteTable.Rows.Count = 0, 0, RouteTable.Rows(UID - 1)("BestFareMDiff"))
        Dim ChargeFaresGDiff As Double = IIf(RouteTable.Rows.Count = 0, 0, RouteTable.Rows(UID - 1)("BestFareGDiff"))
        Dim ChargeFaresAdd As Double = IIf(RouteTable.Rows.Count = 0, 0, RouteTable.Rows(UID - 1)("BestFareAdd"))
        Dim ChargeFaresMAdd As Double = IIf(RouteTable.Rows.Count = 0, 0, RouteTable.Rows(UID - 1)("BestFareMAdd"))
        Dim ChargeFaresGAdd As Double = IIf(RouteTable.Rows.Count = 0, 0, RouteTable.Rows(UID - 1)("BestFareGAdd"))

        Dim BestRoute As String = IIf(RouteTable.Rows.Count = 0, "", RouteTable.Rows(UID - 1)("Route"))
        BestRoute = BestRoute.Substring(1, BestRoute.Length - 2)
        If Not isBestFare Then
            UpdateDBText("Insert into InterchangeBestFare (StartIC, StopIC, Routes) values(@StartIC, @StopIC, @Routes)", New SqlParameter("@StartIC", StartIC), New SqlParameter("@StopIC", StopIC), New SqlParameter("@Routes", BestRoute))
        End If

        Dim TotalMiles As Double = IIf(RouteTable.Rows.Count = 0, 0, RouteTable.Rows(UID - 1)("TotalMiles"))
        Dim BestRouteName As String = ""
        Dim BestConnectName As String = ""

        BestRouteName = getRouteName(BestRoute)
        BestConnectName = BestRouteName.Split(",")(1)
        BestRouteName = BestRouteName.Split(",")(0)
        BestRouteName = "從" & HWName(dtIC.Select("UID=" & StartIC)(0)("HWUID")) & " " & dtIC.Select("UID=" & StartIC)(0)("ICName") & " 到 " & HWName(dtIC.Select("UID=" & StopIC)(0)("HWUID")) & dtIC.Select("UID=" & StopIC)(0)("ICName") & "<br/> 詳細路線:<br/>" & BestRouteName.Substring("&#8594;".Length) & "<br/><br/>"
        BestConnectName = "從" & HWName(dtIC.Select("UID=" & StartIC)(0)("HWUID")) & " " & dtIC.Select("UID=" & StartIC)(0)("ICName") & " 到 " & HWName(dtIC.Select("UID=" & StopIC)(0)("HWUID")) & dtIC.Select("UID=" & StopIC)(0)("ICName") & IIf(BestConnectName.Length > 0, "  <br/>(" & BestConnectName & ")<br/>", "")
        Dim dt As DataTable = Fare.FareDT(FareList.Rows(0)("FareListID").ToString, StartIC, StopIC, BestRoute, BestRouteName, BestConnectName, TotalMiles, ChargeMiles, ChargeFares, ChargeFaresM, ChargeFaresG, ChargeFaresDiff, ChargeFaresMDiff, ChargeFaresGDiff, ChargeFaresAdd, ChargeFaresMAdd, ChargeFaresGAdd, BestTotalFare, StartTime, Date.Now, RouteArray.Length, RouteSearch)
        Return dt
    End Function
    Private Function getRouteName(ByVal RouteList As String) As String

        Dim dtIC As DataTable = Fare.GetInterChangeListwithHWNameandByProjectID(ddlTimePediod.SelectedValue.ToString)
        Dim dtrelation As DataTable = Fare.GetInterChangeRelation()

        Dim tRoute() As String = RouteList.Split(",")
        Dim BestRouteName As String = ""
        Dim BestConnectName As String = ""
        Dim hwPrev As Integer = 0
        Dim icOrder As Integer = 0
        For i As Integer = 0 To tRoute.Count - 1
            Dim tr As String = tRoute(i)
            BestRouteName += "&#8594;"
            Dim BestRouteHW As Integer = dtIC.Select("UID=" & tr)(0)("HWUID")
            If dtrelation.Select(" HWStart <> HWStop and StartIC = " & tr).Length > 0 Then
                BestConnectName += HWName(BestRouteHW) & " " & dtIC.Select("UID=" & tr)(0)("ICName") & "&#8594;"
            End If
            If hwPrev <> dtIC.Select(" UID=" & tr)(0)("HWUID") Then
                BestRouteName += "<font color=blue>" & HWName(BestRouteHW) & "</font>"
                icOrder = 0
            End If
            Dim thisOrder As Integer = dtIC.Select("UID=" & tr)(0)("ItemOrder")
            If icOrder = 0 Then
                icOrder = thisOrder
            End If
            If Math.Abs(thisOrder - icOrder) > 1 Then
                While icOrder <> thisOrder
                    If icOrder > thisOrder Then
                        icOrder = icOrder - 1
                        If icOrder > thisOrder Then
                            If dtIC.Select("HWUID=" & BestRouteHW & " and ItemOrder = " & (icOrder).ToString).Length > 0 Then
                                BestRouteName += " " & dtIC.Select("HWUID=" & BestRouteHW & " and ItemOrder = " & (icOrder).ToString)(0)("ICName") & "&#8594;"
                            End If
                        End If
                    Else
                        icOrder = icOrder + 1
                        If icOrder < thisOrder Then
                            If dtIC.Select("HWUID=" & BestRouteHW & " and ItemOrder = " & (icOrder).ToString).Length > 0 Then
                                BestRouteName += " " & dtIC.Select("HWUID=" & BestRouteHW & " and ItemOrder = " & (icOrder).ToString)(0)("ICName") & "&#8594;"
                            End If
                        End If
                    End If
                End While
            End If
            hwPrev = dtIC.Select("UID=" & tr)(0)("HWUID")
            icOrder = thisOrder
            BestRouteName += " " & dtIC.Select("HWUID=" & BestRouteHW & " and ItemOrder = " & (icOrder).ToString)(0)("ICName")
        Next
        Return BestRouteName & "," & BestConnectName
    End Function
    Dim TurnState As Integer = 0
    Dim TurnIC As Integer = 0
    Private Sub getNextRoute(ByVal StartIC As Integer, ByVal StopIC As Integer, ByVal StartID As Integer, ByVal dtRelation As DataTable, ByVal dtIC As DataTable, ByVal dtProhibit As String, ByVal Route As String, ByVal RouteFare As Double)
        RouteSearch += 1
        Dim RouteN() As String = Route.Split(",")
        '如果走了一百個交流道還到不了，就太誇張了
        If RouteN.Length > 120 Then
            Exit Sub
        End If
        '不能走的路線
        If (Route & StopIC.ToString & ",").IndexOf("," & dtProhibit & ",") >= 0 Then
            Exit Sub
        End If
        Dim StartIDRow As DataRow = dtIC.Select("UID=" & StartID.ToString)(0)
        Dim StopICRow As DataRow = dtIC.Select("UID=" & StopIC.ToString)(0)
        Dim StartHW As Integer = StartIDRow("HWUID")
        Dim StopHW As Integer = StopICRow("HWUID")
        '是不是迴轉點
        Dim IsTurn As Integer = dtRelation.Select(" StartIC = " & StartID & " and StartIC = StopIC").Length
        Dim rowp() As String = dtProhibit.Split("#")
        '如果下一個點是終點，就結束
        Dim dtselect() As DataRow = dtRelation.Select(" StartIC = " & StartID & " and StopIC = " & StopIC)
        '如果不是迴轉點，不能迴轉
        If dtselect.Length > 0 And (RouteN.Length < 3 Or Not (RouteN(RouteN.Length - 3) = StopIC.ToString And IsTurn = 0)) Then
            Dim stopsign As Boolean = False
            '如果沒有入口
            If StartIC = StartID Then
                If StartIDRow("ItemOrder") < StopICRow("ItemOrder") Then
                    If StartIDRow("InSouth") = True Then
                        stopsign = True
                    End If
                Else
                    If StartIDRow("InNorth") = True Then
                        stopsign = True
                    End If
                End If
            End If
            '如果沒有出口
            If StartIDRow("ItemOrder") < StopICRow("ItemOrder") Then
                If StopICRow("OutSouth") = True Then
                    stopsign = True
                Else
                    stopsign = False
                End If
            Else
                If StopICRow("OutNorth") = True Then
                    stopsign = True
                Else
                    stopsign = False
                End If
            End If

            'Dim dtrelationlist As DataTable = Fare.GetInterChangeRelation()
            'Dim row As DataRow = dtrelationlist.Select("StartIC=" & StartID & " and StopIC = " & StopIC)(0)
            ''如果起訖點就在隔壁，要先檢查能不能過去
            'If Route.Replace(",", "").Equals(StartIC.ToString) And StartHW = StopHW Then
            '    If Not ((StartIDRow("InSouth") And row("DirectionStop") = 0) Or (StartIDRow("InNorth") And row("DirectionStop") = 1)) Then
            '        stopsign = True
            '    End If
            'End If
            If stopsign Then

                For Each row As String In rowp
                    If (Route & StopIC.ToString & ",").IndexOf(row) >= 0 Then
                        stopsign = False
                        Exit For
                    End If
                Next
                'If (Route & StopIC.ToString & ",").IndexOf(dtProhibit) >= 0 Then
                '    stopsign = False
                'End If
                'For Each drow As DataRow In dtProhibit.Rows
                '    If (Route & StopIC.ToString & ",").IndexOf("," & drow("Routes") & ",") >= 0 Then
                '        stopsign = True
                '        Exit For
                '    End If
                'Next
            End If
            'If Not stopsign Then
            '    If (RouteN(RouteN.Length - 3) = StopIC.ToString) Then
            '        stopsign = True
            '    End If
            'End If
            '國三轉國一不能下汐止
            If (Route & StopIC.ToString & ",").IndexOf(",91,6,208,") >= 0 And StopIC = 208 Then
                stopsign = False
            End If
            If stopsign Then
                '看一下是不是有這個出口方向
                ' If (StopICRow("OutSouth") And Row("DirectionStop") = 0) Or (StopICRow("OutNorth") And Row("DirectionStop") = 1) Then
                '，就直接給他過去了，幹嘛客氣，其他點根本就不要走
                If StartIDRow("HWUID") = StopICRow("HWUID") Then
                    If StartIDRow("ItemOrder") > StopICRow("ItemOrder") Then
                        RouteFare += StartIDRow("FareS") - StartIDRow("FareSDiffN") + StartIDRow("FareSAddN")
                    Else
                        RouteFare += StopICRow("FareS") - StopICRow("FareSDiff") + StopICRow("FareSAdd")
                    End If
                End If

                If RouteFares > RouteFare Then
                    RouteFares = RouteFare
                    RoutePass &= "#" & Route & StopIC & "," & "#"
                    RoutePass = RoutePass.Replace("##", "#")
                End If
                'RoutePass += "#" & Route
                Exit Sub
                'End If
            End If
        End If

        Dim RouteS As String = Route
        Dim RFare As Double = RouteFare
        Dim i As Integer = 0
        If StartHW = StopHW Then

            Dim dtSameHW() As DataRow = dtRelation.Select(" StartIC = " & StartID & " and HWStart = HWStop", " StopItemOrder desc")
            Dim dtDiffHW() As DataRow = dtRelation.Select(" StartIC = " & StartID & " and HWStart <> HWStop", " StopItemOrder desc")

            If dtDiffHW.Length > 0 Then
                Dim dtt As DataTable = dtRelation.Clone
                For Each row As DataRow In dtSameHW
                    dtt.ImportRow(row)
                Next
                For Each row As DataRow In dtDiffHW
                    dtt.ImportRow(row)
                Next
                dtselect = dtt.Select()
            Else
                dtselect = dtSameHW
            End If
        Else
            dtselect = dtRelation.Select(" StartIC = " & StartID, " HWStop, StopItemOrder ")
        End If
        Dim IsHWTurn As Integer = dtRelation.Select("StartIC=" & StartID.ToString & " and  HWStart <> HWStop").Length
        'For i = 0 To dtselect.Length - 1
        For i = 0 To dtselect.Length - 1
            Dim stopsign As Boolean = False
            Route = RouteS
            RouteFare = RFare
            Dim drow As DataRow = dtselect(i)
            Dim StopID As Integer = drow("StopIC")
            StopICRow = dtIC.Select("UID=" & StopID.ToString)(0)
            'If Route.IndexOf(",7,8,9,10,11,12,13,12,") >= 0 Then
            '    Route = Route
            'End If
            '如果沒有入口
            If StartIC = StartID And Route = "," & StartIC.ToString & "," Then
                If StartIDRow("ItemOrder") < StopICRow("ItemOrder") Then
                    If StartIDRow("InSouth") = False Then
                        Continue For
                    End If
                Else
                    If StartIDRow("InNorth") = False Then
                        Continue For
                    End If
                End If
            End If

            ''如果在禁止通行路徑，當然不能走
            For Each row As String In rowp
                If (Route & StopIC.ToString & ",").IndexOf(row) >= 0 Then
                    stopsign = True
                    Exit For
                End If
            Next
            If stopsign = True Then
                Continue For
            End If
            '如果禁止通行
            'If ("," & dtProhibit & ",").IndexOf("#," & StartID & "," & StopID.ToString & ",#") >= 0 Then
            '    Continue For
            'End If
            'If ("," & dtProhibit & ",").IndexOf("#," & Route.Split(",")(Route.Split(",").Length - 2) & "," & StartID & "," & StopID.ToString & ",#") >= 0 Then
            '    Continue For
            'End If
            'If Route.Split(",").Length > 3 Then
            '    If ("," & dtProhibit & ",").IndexOf("#," & Route.Split(",")(Route.Split(",").Length - 3) & Route.Split(",")(Route.Split(",").Length - 2) & StartID & "," & StopID.ToString & ",#") >= 0 Then
            '        Continue For
            '    End If
            'End If

            '如果同一交流道走過2次，就丟掉
            If Route.Replace(",", ",,").Replace("," & StopID.ToString & ",", ",,").Length < Route.Replace(",", ",,").Length - (StopID.ToString.ToString.Length) * 1 Then
                Continue For
                'Exit Sub
            End If
            If RouteSearch > 80000 Then
                Exit Sub
            End If
            '如果跨國道


            'If (Route & StopID.ToString & ",").IndexOf(dtProhibit) >= 0 Then
            '    Continue For
            'End If
            '如果起點終點不在國5,6,11，下一個點在國5,6,11就不要去
            '如果訖點都不在國一高架就不要走國一高架，因為費率和里程國一高架都比照國一，所以走國一就好，不要走國一高架
            If (StopHW <> 5 And drow("HWStart") <> 5 And drow("HWStop") = 5) Or (StopHW <> 6 And drow("HWStart") <> 6 And drow("HWStop") = 6) Or (StopHW <> 11 And drow("HWStart") <> 11 And drow("HWStop") = 11) Then
                Continue For
            End If
            '如果相同路徑已經走過一次，就不能再走一次
            If Route.IndexOf("," & StartID.ToString & "," & StopID.ToString & ",") >= 0 Then
                Continue For
            End If
            '如果是現在是起點，下一個點如果不在可行的方向,就離開
            If Route.Replace(",", "").Equals(StartIC.ToString) And StartHW = StopHW Then
                If StartIDRow("ItemOrder") > dtIC.Select("UID=" & StopID.ToString)(0)("ItemOrder") Then
                    If StartIDRow("InNorth") = 0 Then
                        Continue For
                    End If
                Else
                    If StartIDRow("InSouth") = 0 Then
                        Continue For
                    End If
                End If
            End If

            '如果不是迴轉點，不能迴轉
            If (RouteN(RouteN.Length - 3) = StopID.ToString And IsTurn = 0) Then
                Continue For
            End If
            '如果變換國道，又換回來的時候，如果不是由原來的路徑反過來回去，就不太應該，因為不需要走這一趟，直接從換國道的起始點直接走下去就好了，沒有必要繞一趟另外那個國道
            '如果是迴轉點，這個迴轉點只應該要迴轉一次，以後不能再經過這個點了
            '如果這個點是換國道的點去的時候和回來的時候應該要反方向的路徑，不能走到別的地方去，例如 h1c1, rw, h2c2,  h2c3. 迴轉, r2c3, rw, h1c1 才對h1c1, rw, h2c2,  h2c3. 迴轉, r2c3, rw, h1c4就不對了
            '如果下一個迴轉點，就把startid 記起來，等下看看有沒有迴轉
            '檢查起始點是不是迴轉
            '如果起訖點是同一個點，就繼續
            If StartID = StopID Then
                '如果之前StartID 已經迴轉過，就不能再轉一次了
                For j As Integer = 0 To dtselect.Count - 1
                    Dim drows As DataRow = dtselect(j)
                    If Route.IndexOf("," & drows("StopIC").ToString & "," & StartID.ToString & "," & drows("StopIC").ToString) >= 0 Then
                        stopsign = True
                        Exit For
                    End If
                Next
                If stopsign Then
                    Exit For
                End If
                Continue For
            End If
            '如果前三個點是迴轉，drow("StopIC") 跟前四個點不一樣，這樣是不行的，例如 16,17,18,17,85 這樣不行，因為直接 16,17,85就好了幹嘛還要繞一圈
            If RouteN.Length > 5 Then
                If StartID = RouteN(RouteN.Length - 4) And RouteN(RouteN.Length - 5) <> StopID Then
                    If dtProhibit.IndexOf("," & RouteN(RouteN.Length - 5) & "," & StartID & "," & StopID.ToString & ",") >= 0 Then
                    Else
                        Continue For
                    End If
                End If
            End If
            '如果在同一國道才要加費率，轉換國道時不用調整費用
            If drow("HWStart") = drow("HWStop") Then
                '如果前一個點是國道轉接點，而且以前已經穿越過這兩個國道，現在就一定要走原來的路回去，如果不是，就不行
                '如果起始點是穿越國道點
                If IsHWTurn > 0 Then
                    Dim errorRoute As Boolean = False
                    '檢查這個點以前有沒有被穿越過，每個穿越都要檢查
                    For j As Integer = 0 To dtselect.Count - 1
                        Dim drows As DataRow = dtselect(j)
                        If drows("HWStart") = drows("HWStop") Then
                            Continue For
                        End If
                        '檢查這個點以前有沒有被穿越過
                        If Route.IndexOf("," & drows("StopIC").ToString & "," & StartID.ToString & "," & drows("StopIC").ToString & ",") > 0 Then
                            If Route.IndexOf("," & StopID.ToString & "," & StartID.ToString & ",") < 0 Then
                                errorRoute = True
                                Exit For
                            End If
                        End If
                    Next
                    If errorRoute Then
                        Continue For
                    End If
                End If

                If drow("StartItemOrder") > drow("StopItemOrder") Then
                    RouteFare += StartIDRow("FareS") - StartIDRow("FareSDiffN") + StartIDRow("FareSAddN")
                Else
                    RouteFare += dtIC.Select("UID=" & StopID.ToString)(0)("FareS") - dtIC.Select("UID=" & StopID.ToString)(0)("FareSDiff") + dtIC.Select("UID=" & StopID.ToString)(0)("FareSAdd")
                End If
                '如果大於最小費率，就不要再算下去了
                If RouteFare > RouteFares Then
                    Fare.SetMaxFare(RouteFare)
                    Continue For
                End If
                ''如果在禁止通行路徑，當然不能走
                'If (Route & StopIC.ToString & ",").IndexOf(dtProhibit) >= 0 Then
                '    Continue For
                'End If
                'For Each drows As DataRow In dtProhibit.Rows
                '    If (Route & drow("StopIC").ToString & ",").IndexOf("," & drows("Routes") & ",") >= 0 Then
                '        stopsign = True
                '        Exit For
                '    End If
                'Next
                'If stopsign Then
                '    Continue For
                'End If
                'End If
            End If
            getNextRoute(StartIC, StopIC, StopID.ToString, dtRelation, dtIC, dtProhibit, Route & StopID & ",".ToString, RouteFare)
        Next

        'If Route.IndexOf("," & StopIC.ToString & ",") >= 0 Then
        '    Return Route
        'Else
        '    Route = RouteS
        'End If
        'Return Route
    End Sub

#End Region
    Public Function getICName(ByVal ICUID As Integer) As String
        Dim ICName As String = ""
        Dim dtIC As DataTable = Fare.GetInterChangeList
        Try
            ICName = dtIC.Select("UID=" & ICUID.ToString)(0)("ICNAME").ToString
        Catch ex As Exception
            ICName = "交流道"
        End Try
        Return ICName
    End Function

    Public Function getAreaCoords(ByVal ICUID As Integer) As String
        Dim ret As String = ""
        Dim Coords As String = Fare.GetICoords(ICUID)
        Dim dtIC As DataTable = Fare.GetInterChangeList
        Try
            If dtIC.Select("UID=" & ICUID.ToString).Length > 0 Then

                ret = " coords=""" & Coords.ToString & """ onClick=""MM_showHideLayers('" & ICUID.ToString & "','apDiv3','" & Fare.GetICName(ICUID) & "','show')"" onMouseOver=""MM_showHideLayers('" & ICUID.ToString & "','apDiv2','" & Fare.GetICName(ICUID) & "','show')"" onMouseOut=""MM_showHideLayers('" & ICUID.ToString & "','apDiv2','" & Fare.GetICName(ICUID) & "','hide')"""
            Else
                ret = "onClick=""alert('本交流道尚未開放!!');"""
            End If
        Catch ex As Exception
            ret = ""
        End Try
        Return ret
    End Function



    ''' <summary>
    ''' DataList控制項資料繫結設定
    ''' </summary>
    ''' <param name="sender">觸發本事件控制項</param>
    ''' <param name="e">觸發本事件帶進之參數</param>
    ''' <remarks></remarks>
    Protected Sub rptMap_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptMap.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem OrElse e.Item.ItemType = ListItemType.Item Then
            Dim rowView As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim ltlMap As Literal = CType(e.Item.FindControl("ltlMap"), Literal)
            Dim ltlArea As Literal = CType(e.Item.FindControl("ltlArea"), Literal)
            ltlArea.Text = ""
            Using dr As SqlDataReader = ClassDB.GetDataReaderParam("Select l.UID, ISNULL(MapCoords, '') as MapCoords, ICName, HWName from InterchangeList l inner join HWList h on h.UID = l.HWUID where l.IsVirture > 0 and l.UID in (Select ICUID from InterchangeMapGroupRelation where GroupUID=@GroupUID)", New SqlParameter("@GroupUID", rowView("UID").ToString))
                Try
                    ltlMap.Text = "<map name=""Map" & CInt(rowView("ItemOrder")).ToString & """>"
                    If dr IsNot Nothing Then
                        While dr.Read
                            If dr("MapCoords").ToString.Trim.Length > 0 Then
                                Dim Coord As String = dr("MapCoords")

                                If Regex.IsMatch(Coord.Replace(";", "").Replace(",", "").ToString.Trim, "^[0-9]+$") Then
                                    Dim Coords() As String = Coord.ToString.Split(";")
                                    Dim CoordsList As String = ""
                                    Dim UID As String = dr("UID")
                                    UID = Convert.ToInt32(UID).ToString
                                    Dim HWName As String = dr("HWName")
                                    Dim ICName As String = dr("ICName")

                                    CoordsList = Coords(0)
                                    If CoordsList.ToString.Split(",").Length > 3 Then
                                        ltlArea.Text &= "<area shape=""rect"" href=""#"" coords=""" & CoordsList & """ onClick=""MM_showHideLayers('" & UID.ToString & "','apDiv3','" & "(" & HWName.ToString & ")" & ICName.ToString & "','show')"" onMouseOver=""MM_showHideLayers('" & UID.ToString & "','apDiv2','" & "(" & HWName.ToString & ")" & ICName & "','show')"" onMouseOut=""MM_showHideLayers('" & UID.ToString & "','apDiv2','" & "(" & HWName.ToString & ")" & ICName.ToString & "','hide')""/>" & vbCrLf
                                    Else
                                        ltlArea.Text &= vbCrLf & "<area shape=""circle"" href=""#"" coords=""" & CoordsList & """ onClick=""MM_showHideLayers('" & UID.ToString & "','apDiv3','" & "(" & HWName.ToString & ")" & ICName.ToString & "','show')"" onMouseOver=""MM_showHideLayers('" & UID.ToString & "','apDiv2','" & "(" & HWName.ToString & ")" & ICName & "','show')"" onMouseOut=""MM_showHideLayers('" & UID.ToString & "','apDiv2','" & "(" & HWName.ToString & ")" & ICName.ToString & "','hide')""/>" & vbCrLf
                                    End If
                                    If Coords.Length > 1 Then
                                        CoordsList = Coords(1)
                                        If CoordsList.ToString.Split(",").Length > 3 Then
                                            ltlArea.Text &= "<area shape=""rect"" href=""#"" coords=""" & CoordsList & """ onClick=""MM_showHideLayers('" & UID.ToString & "','apDiv3','" & "(" & HWName.ToString & ")" & ICName.ToString & "','show')"" onMouseOver=""MM_showHideLayers('" & UID.ToString & "','apDiv2','" & "(" & HWName.ToString & ")" & ICName & "','show')"" onMouseOut=""MM_showHideLayers('" & UID.ToString & "','apDiv2','" & "(" & HWName.ToString & ")" & ICName.ToString & "','hide')""/>" & vbCrLf
                                        Else
                                            ltlArea.Text &= vbCrLf & "<area shape=""circle"" href=""#"" coords=""" & CoordsList & """ onClick=""MM_showHideLayers('" & UID.ToString & "','apDiv3','" & "(" & HWName.ToString & ")" & ICName.ToString & "','show')"" onMouseOver=""MM_showHideLayers('" & UID.ToString & "','apDiv2','" & "(" & HWName.ToString & ")" & ICName & "','show')"" onMouseOut=""MM_showHideLayers('" & UID.ToString & "','apDiv2','" & "(" & HWName.ToString & ")" & ICName.ToString & "','hide')""/>" & vbCrLf
                                        End If
                                    End If
                                End If
                            End If
                        End While
                    End If
                Catch ex As Exception
                Finally
                End Try
            End Using
        End If
    End Sub


End Class
