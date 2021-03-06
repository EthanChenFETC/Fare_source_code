Imports System.Text
Imports System.Data
Imports System.Data.SqlClient

''' <summary>
''' 動態產生及管理JavaScript版本選單
''' </summary>
''' <remarks></remarks>
Public Module FareTest

    Dim Context As HttpContext = HttpContext.Current
    Private CacheMinute As Integer = 15
    Public Function FindFooter(ByVal mControls As ControlCollection) As Control
        For i As Integer = 0 To mControls.Count - 1
            Dim newcontrol As Control = mControls(i)
            If newcontrol.ToString.Equals("ASP.common_footer_ascx") Then
                Dim footer As Object = newcontrol
                footer.setCount()
                Return Nothing
            End If
            If newcontrol.Controls.Count > 0 Then
                FindFooter(newcontrol.Controls)
            End If
        Next
        Return Nothing
    End Function
    Public Function HWListArraySmall() As String()
        Dim HWName() As String = {"", "國道1", "國道2", "國道3", "國道4", "國道5", "國道6", "國道7", "國道8", "國道9", "國道10", "國1高架", "國3甲"}
        'Dim HWList As DataTable = Context.Cache("HWList")
        'If HWList Is Nothing Then HWList = Fare.GetHWList()
        'Dim hwstr As String = ""
        'For Each drow As DataRow In HWList.Select(" 1=1", "UID")
        '    hwstr &= "," & drow("HWName").ToString
        'Next
        'Dim HWArray() As String = hwstr.Split(",")
        Return HWName
    End Function
    Public Function HWListArray() As String()
        'Dim HWName() As String = {"", "國一", "國二", "國三", "國四", "國五", "國六", "國七", "國八", "國九", "國十", "國一高架", "國三甲"}
        Dim HWName() As String = {"", "<img src=""images/no1.png"">", "<img src=""images/no2.png"">", "<img src=""images/no3.png"">", "<img src=""images/no4.png"">", "<img src=""images/no5.png"">", "<img src=""images/no6.png"">", "", "<img src=""images/no8.png"">", "", "<img src=""images/no10.png"">", "<img src=""images/no1a.png"">", "<img src=""images/no3a.png"">"}
        'Dim HWList As DataTable = Context.Cache("HWList")
        'If HWList Is Nothing Then HWList = Fare.GetHWList()
        'Dim hwstr As String = ""
        'For Each drow As DataRow In HWList.Select(" 1=1", "UID")
        '    hwstr &= "," & drow("HWName").ToString
        'Next
        'Dim HWArray() As String = hwstr.Split(",")
        Return HWName
    End Function
    Public Function GetICName(ByVal ICUID As String) As String
        Dim ICList As DataTable = Context.Cache("InterChangeList")
        If ICList Is Nothing Then ICList = GetInterChangeList()
        Dim ICName() As DataRow = ICList.Select(" UID=" & ICUID.ToString)
        If ICName.Length = 0 Then
            Return ""
        End If
        Return "(" & HWListArraySmall(ICName(0)("HWUID")) & ")" & ICName(0)("ICName")
    End Function
    Public Function GetICoords(ByVal ICUID As String) As String
        Dim ICList As DataTable = Context.Cache("InterChangeList")
        If ICList Is Nothing Then ICList = GetInterChangeList()
        Dim ICName() As DataRow = ICList.Select(" UID=" & ICUID.ToString)
        If ICName.Length = 0 Then
            Return ""
        End If
        Return ICName(0)("MAPCoords")
    End Function
    Public Function FareColumn() As DataTable
        Dim dc1 As DataColumn = New DataColumn("UID", GetType(System.Int32))
        Dim dc2 As DataColumn = New DataColumn("FareListID", GetType(System.Int32))
        Dim dc3 As DataColumn = New DataColumn("StartIC", GetType(System.Int32))
        Dim dc4 As DataColumn = New DataColumn("StopIC", GetType(System.Int32))
        Dim dc5 As DataColumn = New DataColumn("Route", GetType(System.String))
        Dim dc6 As DataColumn = New DataColumn("RouteName", GetType(System.String))
        Dim dc7 As DataColumn = New DataColumn("RouteConnectName", GetType(System.String))
        Dim dc8 As DataColumn = New DataColumn("TotalMiles", GetType(System.Double))
        Dim dc9 As DataColumn = New DataColumn("ChargeMiles", GetType(System.Double))
        Dim dc10 As DataColumn = New DataColumn("BestFare", GetType(System.Double))
        Dim dc11 As DataColumn = New DataColumn("BestFareM", GetType(System.Double))
        Dim dc12 As DataColumn = New DataColumn("BestFareG", GetType(System.Double))
        Dim dc13 As DataColumn = New DataColumn("BestFareDiff", GetType(System.Double))
        Dim dc14 As DataColumn = New DataColumn("BestFareMDiff", GetType(System.Double))
        Dim dc15 As DataColumn = New DataColumn("BestFareGDiff", GetType(System.Double))
        Dim dc16 As DataColumn = New DataColumn("BestFareAdd", GetType(System.Double))
        Dim dc17 As DataColumn = New DataColumn("BestFareMAdd", GetType(System.Double))
        Dim dc18 As DataColumn = New DataColumn("BestFareGAdd", GetType(System.Double))
        Dim dc19 As DataColumn = New DataColumn("BestTotalFare", GetType(System.Double))
        Dim dc20 As DataColumn = New DataColumn("StartTime", GetType(System.DateTime))
        Dim dc21 As DataColumn = New DataColumn("StopTime", GetType(System.DateTime))
        Dim dc22 As DataColumn = New DataColumn("TotalRoute", GetType(System.Double))
        Dim dc23 As DataColumn = New DataColumn("TotalSearch", GetType(System.Double))
        Dim dt As DataTable = New DataTable
        dt.Columns.Add(dc1)
        dt.Columns.Add(dc2)
        dt.Columns.Add(dc3)
        dt.Columns.Add(dc4)
        dt.Columns.Add(dc5)
        dt.Columns.Add(dc6)
        dt.Columns.Add(dc7)
        dt.Columns.Add(dc8)
        dt.Columns.Add(dc9)
        dt.Columns.Add(dc10)
        dt.Columns.Add(dc11)
        dt.Columns.Add(dc12)
        dt.Columns.Add(dc13)
        dt.Columns.Add(dc14)
        dt.Columns.Add(dc15)
        dt.Columns.Add(dc16)
        dt.Columns.Add(dc17)
        dt.Columns.Add(dc18)
        dt.Columns.Add(dc19)
        dt.Columns.Add(dc20)
        dt.Columns.Add(dc21)
        dt.Columns.Add(dc22)
        dt.Columns.Add(dc23)
        Return dt
    End Function

    Public Function FareDT(ByVal FareListID As Integer, ByVal StartIC As Integer, ByVal StopIC As Integer, ByVal Route As String, ByVal RouteName As String, ByVal RouteConnectName As String, ByVal TotalMiles As Double, ByVal ChargeMiles As Double, ByVal BestFare As Double, ByVal BestFareM As Double, ByVal BestFareG As Double, ByVal BestFareDiff As Double, ByVal BestFareMDiff As Double, ByVal BestFareGDiff As Double, ByVal BestFareAdd As Double, ByVal BestFareMAdd As Double, ByVal BestFareGAdd As Double, ByVal BestTotalFares As Double, ByVal StartTime As Date, ByVal StopTime As Date, ByVal TotalRoute As Integer, ByVal TotalSearch As Integer) As DataTable
        Dim dt As DataTable = Fare.FareColumn()
        Dim drow As DataRow = dt.NewRow
        drow("FareListID") = FareListID
        drow("StartIC") = StartIC
        drow("StopIC") = StopIC
        drow("Route") = Route
        drow("RouteName") = RouteName
        drow("RouteConnectName") = RouteConnectName
        drow("TotalMiles") = TotalMiles
        drow("ChargeMiles") = ChargeMiles
        drow("BestFare") = BestFare
        drow("BestFareM") = BestFareM
        drow("BestFareG") = BestFareG
        drow("BestFareDiff") = BestFareDiff
        drow("BestFareMDiff") = BestFareMDiff
        drow("BestFareGDiff") = BestFareGDiff
        drow("BestFareAdd") = BestFareAdd
        drow("BestFareMAdd") = BestFareMAdd
        drow("BestFareGAdd") = BestFareGAdd
        drow("StartTime") = StartTime
        drow("StopTime") = StopTime
        drow("TotalRoute") = TotalRoute
        drow("TotalSearch") = TotalSearch
        drow("BestTotalFare") = TotalSearch
        dt.Rows.Add(drow)
        Return dt
    End Function



#Region " 抓取資料表 國道、交流道 "

    ''' <summary>
    ''' 取得選單字串,Javascript版
    ''' </summary>
    ''' 
    ''' <returns>選單HTML原始碼</returns>
    ''' <remarks></remarks>
    Public Function GetInterChangeRelationDistint() As DataTable
        '先從稍早的選單快取中取得資料
        Dim dt As DataTable = Context.Cache("InterChangeRelation")
        If dt Is Nothing Then dt = GetInterChangeRelation()
        Dim dtList As DataTable = dt.DefaultView.ToTable(True, "StartIC", "StopIC", "HWStart", "HWStop", "StartItemOrder", "StopItemOrder")
        Return dtList
    End Function
    ''' <summary>
    ''' 取得選單字串,Javascript版
    ''' </summary>
    ''' 
    ''' <returns>選單HTML原始碼</returns>
    ''' <remarks></remarks>
    Public Function GetICListFromHWID(ByVal HWUID As Integer) As DataTable
        '先從稍早的選單快取中取得資料
        Dim dt As DataTable
        Dim ICList As DataTable = Context.Cache("InterChangeList")
        If ICList Is Nothing Then ICList = GetInterChangeList()

        Dim dtList As DataTable = ICList.Clone
        Dim dtrow() As DataRow = ICList.Select(" HWUID = " & HWUID.ToString, "ItemOrder")
        For i As Integer = 0 To dtrow.Count - 1
            Dim row As DataRow = dtrow(i)
            Try

                Dim drow As DataRow = dtList.NewRow
                drow("UID") = Row("UID")
                drow("HWUID") = Row("HWUID")
                drow("ICName") = Row("ICName")
                drow("ICMiles") = Row("ICMiles")
                drow("ICMilesBetween") = Row("ICMilesBetween")
                drow("ICMilesN") = Row("ICMiles")
                drow("ICMilesBetweenN") = Row("ICMilesBetween")
                drow("OutSouth") = Row("OutSouth")
                drow("OutNorth") = Row("OutNorth")
                drow("InSouth") = Row("InSouth")
                drow("InNorth") = Row("InNorth")
                drow("ItemOrder") = Row("ItemOrder")
                drow("AxisY") = Row("AxisY")
                drow("AxisX") = Row("AxisX")
                drow("MapCoords") = Row("MapCoords")
                drow("Notes") = Row("Notes")
                dtList.Rows.Add(drow)
            Catch ex As Exception

            End Try
        Next
        Return dtList
    End Function
    ''' <summary>
    ''' 重新計算FareList
    ''' </summary>
    ''' <returns>DataTable物件</returns>
    ''' <remarks></remarks>
    Public Function GetFareListByProjectID(ByVal ProjectID As Integer) As DataTable
        '先從稍早的選單快取中取得資料
        Return GetFareListByFareListID(GetFareListIDByProjectID(ProjectID))
    End Function
    ''' <summary>
    ''' 重新計算ICList
    ''' </summary>
    ''' <returns>DataTable物件</returns>
    ''' <remarks></remarks>
    Public Function GetFareListIDByProjectID(ByVal ProjectID As Integer) As Integer
        '先從稍早的選單快取中取得資料
        Dim FareListID As Integer = 0
        Dim ProjectList As DataTable = Context.Cache("IterchangeFareProject")
        If ProjectList Is Nothing Then ProjectList = GetInterChangeFareProject()
        Try
            FareListID = ProjectList.Select(" UID=" & ProjectID.ToString)(0)("FareListID")
        Catch ex As Exception
            FareListID = 0
        End Try
        Return FareListID
    End Function
    ''' <summary>
    ''' 重新計算ICList
    ''' </summary>
    ''' <returns>DataTable物件</returns>
    ''' <remarks></remarks>
    Public Function GetFareListByFareListID(ByVal FareListID As Integer) As DataTable
        '先從稍早的選單快取中取得資料
        Dim dt As DataTable = New DataTable
        Dim FareListAll As DataTable = Context.Cache("InterChangeFareList")
        If FareListAll Is Nothing Then FareListAll = GetInterChangeFareList()
        dt = FareListAll.Clone
        Dim FareList() As DataRow = FareListAll.Select(" FareListID=" & FareListID.ToString)
        For i As Integer = 0 To FareList.Count - 1
            Dim row As DataRow = FareList(i)
            dt.ImportRow(Row)
        Next
        Return dt
    End Function
    ''' <summary>
    ''' 重新計算FareList
    ''' </summary>
    ''' <returns>DataTable物件</returns>
    ''' <remarks></remarks>
    Public Function GetProjectByProjectID(ByVal ProjectID As Integer) As DataTable
        '先從稍早的選單快取中取得資料
        '先從稍早的選單快取中取得資料
        Dim dt As DataTable = New DataTable
        Dim InterChnageFareProject As DataTable = Context.Cache("InterChnageFareProject")
        If InterChnageFareProject Is Nothing Then InterChnageFareProject = GetInterChangeFareProject()
        dt = InterChnageFareProject.Clone
        Dim ProjectList() As DataRow = InterChnageFareProject.Select(" UID=" & ProjectID.ToString)
        For i As Integer = 0 To ProjectList.Count - 1
            Dim row As DataRow = ProjectList(i)
            dt.ImportRow(Row)
        Next

        Return dt
    End Function
    ''' <summary>
    ''' 重新計算FareList
    ''' </summary>
    ''' <returns>DataTable物件</returns>
    ''' <remarks></remarks>
    Public Function GetInterChangeListwithHWNameandByProjectID(ByVal ProjectID As Integer) As DataTable
        '先從稍早的選單快取中取得資料
        '先從稍早的選單快取中取得資料

        Dim FareListID As Integer = GetFareListIDByProjectID(ProjectID)
        Dim dt As DataTable = Context.Cache("InterChangeListwithHWNameandByFareListID" & FareListID.ToString)
        If dt Is Nothing Then dt = GetInterChangeListwithHWNameandByFareListID(FareListID)
        Return dt
    End Function
    ''' 重新計算HWList
    ''' </summary>
    ''' <returns>DataTable物件</returns>
    ''' <remarks></remarks>
    Public Function GetHWList() As DataTable
        If System.Configuration.ConfigurationManager.AppSettings("CacheMinute") IsNot Nothing Then
            CacheMinute = CInt(System.Configuration.ConfigurationManager.AppSettings("CacheMinute").ToString)
        End If
        'Dim dt As DataTable = ClassDB.RunReturnDataTable("select * from HWList where IsOnline > 0 order by ItemOrder")
        Dim dt As DataTable = ClassDB.RunReturnDataTable("select * from HWList order by ItemOrder")
        If dt.Rows.Count = 0 Then
            Context.Cache.Remove("HWList")
            Return New DataTable
        End If
        Context.Cache.Insert("HWList", dt, Nothing, DateTime.Now.AddMinutes(CacheMinute), System.Web.Caching.Cache.NoSlidingExpiration)
        Return dt
    End Function
    ''' <summary>
    ''' 重新計算ICList
    ''' </summary>
    ''' <returns>DataTable物件</returns>
    ''' <remarks></remarks>
    Public Function GetInterChangeList() As DataTable
        If System.Configuration.ConfigurationManager.AppSettings("CacheMinute") IsNot Nothing Then
            CacheMinute = CInt(System.Configuration.ConfigurationManager.AppSettings("CacheMinute").ToString)
        End If
        'Dim dt As DataTable = ClassDB.RunReturnDataTable("select i.*, h.HWName from InterChangeList i inner join HWList h on h.UID = i.HWUID where i.IsOnline > 0 order by h.ItemOrder, i.ItemOrder")
        Dim dt As DataTable = ClassDB.RunReturnDataTable("select i.*, h.HWName from InterChangeList i inner join HWList h on h.UID = i.HWUID order by h.ItemOrder, i.ItemOrder")
        If dt.Rows.Count = 0 Then
            Context.Cache.Remove("InterChangeList")
            Return New DataTable
        End If
        Context.Cache.Insert("InterChangeList", dt, Nothing, DateTime.Now.AddMinutes(CacheMinute), System.Web.Caching.Cache.NoSlidingExpiration)
        Return dt
    End Function
    ''' <summary>
    ''' 重新計算ICList
    ''' </summary>
    ''' <returns>DataTable物件</returns>
    ''' <remarks></remarks>
    Public Function GetInterChangeListwithHWNameandByFareListID(ByVal FareListID As Integer) As DataTable
        If System.Configuration.ConfigurationManager.AppSettings("CacheMinute") IsNot Nothing Then
            CacheMinute = CInt(System.Configuration.ConfigurationManager.AppSettings("CacheMinute").ToString)
        End If
        'Dim dt As DataTable = ClassDB.RunReturnDataTable("Select i.UID, ICName, HWUID, HWlist.HWName, i.ItemOrder, i.ICMiles, i.ICMilesBetween,i.ICMilesN, i.ICMilesBetweenN,i.OutSouth, i.OutNorth, i.InSouth, i.InNorth, f.FareS, FareM, f.FareG, f.FareSN, f.FareMN, f.FareGN, f.FareSDiff, FareMDiff, f.FareGDiff, f.FareSDiffN, f.FareMDiffN, f.FareGDiffN, f.FareSAdd, FareMAdd, f.FareGAdd, f.FareSAddN, f.FareMAddN, f.FareGAddN  from InterChangeList i inner join HWList on HWUID = HWList.UID inner join InterChangeFareList f on i.UID = f.ICUID and f.FareListID = " & FareListID.ToString & " where i.IsOnline > 0 order by HWList.ItemOrder, i.ItemOrder")
        Dim dt As DataTable = ClassDB.RunReturnDataTable("Select i.UID, ICName, HWUID, HWlist.HWName, i.ItemOrder, i.ICMiles, i.ICMilesBetween,i.ICMilesN, i.ICMilesBetweenN,i.OutSouth, i.OutNorth, i.InSouth, i.InNorth, f.FareS, FareM, f.FareG, f.FareSN, f.FareMN, f.FareGN, f.FareSDiff, FareMDiff, f.FareGDiff, f.FareSDiffN, f.FareMDiffN, f.FareGDiffN, f.FareSAdd, FareMAdd, f.FareGAdd, f.FareSAddN, f.FareMAddN, f.FareGAddN  from InterChangeList i inner join HWList on HWUID = HWList.UID inner join InterChangeFareList f on i.UID = f.ICUID and f.FareListID = @FareListID order by HWList.ItemOrder, i.ItemOrder", New SqlParameter("@FareListID", FareListID))
        If dt.Rows.Count = 0 Then
            Context.Cache.Remove("InterChangeListwithHWNameandByFareListID")
            Return New DataTable
        End If
        Context.Cache.Insert("InterChangeListwithHWNameandByFareListID" & FareListID.ToString, dt, Nothing, DateTime.Now.AddMinutes(CacheMinute), System.Web.Caching.Cache.NoSlidingExpiration)
        Return dt
    End Function

    ''' <summary>
    ''' 重新計算ICList
    ''' </summary>
    ''' <returns>DataTable物件</returns>
    ''' <remarks></remarks>
    Public Function GetInterChangeRelation() As DataTable
        If System.Configuration.ConfigurationManager.AppSettings("CacheMinute") IsNot Nothing Then
            CacheMinute = CInt(System.Configuration.ConfigurationManager.AppSettings("CacheMinute").ToString)
        End If
        'Dim dt As DataTable = ClassDB.RunReturnDataTable("Select r.UID, r.StartIC, r.StopIC, r.DirectionStart, r.DirectionStop, i1.HWUID as HWStart, i2.HWUID as HWStop, i1.ItemOrder as StartItemOrder, i2.ItemOrder as StopItemOrder from InterChangeRelation r inner join InterChangeList i1 on i1.UID = r.StartIC inner join InterChangeList i2 on i2.UID = r.StopIC where r.IsOnline > 0  order by r.StartIC,HWStop, i2.ItemOrder")
        Dim dt As DataTable = ClassDB.RunReturnDataTable("Select r.UID, r.StartIC, r.StopIC, r.DirectionStart, r.DirectionStop, i1.HWUID as HWStart, i2.HWUID as HWStop, i1.ItemOrder as StartItemOrder, i2.ItemOrder as StopItemOrder from InterChangeRelation r inner join InterChangeList i1 on i1.UID = r.StartIC inner join InterChangeList i2 on i2.UID = r.StopIC order by r.StartIC,HWStop, i2.ItemOrder")
        If dt.Rows.Count = 0 Then
            Context.Cache.Remove("InterChangeRelation")
            Return New DataTable
        End If
        Context.Cache.Insert("InterChangeRelation", dt, Nothing, DateTime.Now.AddMinutes(CacheMinute), System.Web.Caching.Cache.NoSlidingExpiration)
        Return dt
    End Function
    ''' 重新計算InterChangeFareProject
    ''' </summary>
    ''' <returns>DataTable物件</returns>
    ''' <remarks></remarks>
    Public Function GetInterChangeFareProject() As DataTable
        If System.Configuration.ConfigurationManager.AppSettings("CacheMinute") IsNot Nothing Then
            CacheMinute = CInt(System.Configuration.ConfigurationManager.AppSettings("CacheMinute").ToString)
        End If
        Dim dt As DataTable = ClassDB.RunReturnDataTable("select p.*, Case p.ProjectAlias when 0 then p.ProjectName else (Select Top 1 a.ProjectName from InterChangeFareProject a where a.UID=p.ProjectAlias) end as MainProjectName from InterChangeFareProject p  ")
        If dt.Rows.Count = 0 Then
            Context.Cache.Remove("InterChangeFareProject")
            Return New DataTable
        End If
        Context.Cache.Insert("InterChangeFareProject", dt, Nothing, DateTime.Now.AddMinutes(CacheMinute), System.Web.Caching.Cache.NoSlidingExpiration)
        Return dt
    End Function
    ''' 重新計算InterChangeFareList
    ''' </summary>
    ''' <returns>DataTable物件</returns>
    ''' <remarks></remarks>
    Public Function GetInterChangeFareList() As DataTable
        If System.Configuration.ConfigurationManager.AppSettings("CacheMinute") IsNot Nothing Then
            CacheMinute = CInt(System.Configuration.ConfigurationManager.AppSettings("CacheMinute").ToString)
        End If
        Dim dt As DataTable = ClassDB.RunReturnDataTable("select * from InterChangeFareList ")
        If dt.Rows.Count = 0 Then
            Context.Cache.Remove("InterChangeFareList")
            Return New DataTable
        End If
        Context.Cache.Insert("InterChangeFareList", dt, Nothing, DateTime.Now.AddMinutes(CacheMinute), System.Web.Caching.Cache.NoSlidingExpiration)
        Return dt
    End Function
    ''' 重新計算InterChangeFareProject
    ''' </summary>
    ''' <returns>DataTable物件</returns>
    ''' <remarks></remarks>
    Public Function GetInterChangeFareListProject() As DataTable
        If System.Configuration.ConfigurationManager.AppSettings("CacheMinute") IsNot Nothing Then
            CacheMinute = CInt(System.Configuration.ConfigurationManager.AppSettings("CacheMinute").ToString)
        End If
        'Dim dt As DataTable = ClassDB.RunReturnDataTable("select * from InterChangeFareListProject where IsOnline > 0 ")
        'Dim dt As DataTable = ClassDB.RunReturnDataTable("select * from InterChangeFareListProject where IsOnline > 0 ")
        Dim dt As DataTable = ClassDB.RunReturnDataTable("select * from InterChangeFareListProject ")
        If dt.Rows.Count = 0 Then
            Context.Cache.Remove("InterChangeFareListProject")
            Return New DataTable
        End If
        Context.Cache.Insert("InterChangeFareListProject", dt, Nothing, DateTime.Now.AddMinutes(CacheMinute), System.Web.Caching.Cache.NoSlidingExpiration)
        Return dt
    End Function

    ''' 重新計算InterChangeFareProject
    ''' </summary>
    ''' <returns>DataTable物件</returns>
    ''' <remarks></remarks>
    Public Function GetInterChangeProhibitRoute() As DataTable
        If System.Configuration.ConfigurationManager.AppSettings("CacheMinute") IsNot Nothing Then
            CacheMinute = CInt(System.Configuration.ConfigurationManager.AppSettings("CacheMinute").ToString)
        End If
        'Dim dt As DataTable = ClassDB.RunReturnDataTable("select * from InterChangeProhibitRoute where IsOnline > 0 ")
        Dim dt As DataTable = ClassDB.RunReturnDataTable("select * from InterChangeProhibitRoute ")
        If dt.Rows.Count = 0 Then
            Context.Cache.Remove("InterChangeProhibitRoute")
            Return New DataTable
        End If
        Context.Cache.Insert("InterChangeProhibitRoute", dt, Nothing, DateTime.Now.AddMinutes(CacheMinute), System.Web.Caching.Cache.NoSlidingExpiration)
        Return dt
    End Function

    ''' 重新計算InterChangeFareProject
    ''' </summary>
    ''' <returns>DataTable物件</returns>
    ''' <remarks></remarks>
    Public Function GetInterChangeProhibitRouteString() As String
        Dim dt As DataTable = GetInterChangeProhibitRoute()
        Dim route As String = ""
        For i As Integer = 0 To dt.Rows.Count - 1
            Dim drow As DataRow = dt.Rows(i)
            route &= "#," & drow("Routes") & ","
        Next
        If route.Length > 0 Then
            route = route.Substring(1)
        End If
        Return route
    End Function
    ''' 重新計算InterChangeRouteList
    ''' </summary>
    ''' <returns>DataTable物件</returns>
    ''' <remarks></remarks>
    Public Function GetInterChangeRouteList() As DataTable
        If System.Configuration.ConfigurationManager.AppSettings("CacheMinute") IsNot Nothing Then
            CacheMinute = CInt(System.Configuration.ConfigurationManager.AppSettings("CacheMinute").ToString)
        End If
        'Dim dt As DataTable = ClassDB.RunReturnDataTable("select * from InterChangeRouteList where IsOnline > 0 order by UID")
        Dim dt As DataTable = ClassDB.RunReturnDataTable("select * from InterChangeRouteList order by UID")
        If dt.Rows.Count = 0 Then
            Context.Cache.Remove("InterChangeRouteList")
            Return New DataTable
        End If
        Context.Cache.Insert("InterChangeRouteList", dt, Nothing, DateTime.Now.AddMinutes(CacheMinute), System.Web.Caching.Cache.NoSlidingExpiration)
        Return dt
    End Function
    ''' 重新計算InterChangeStartFree
    ''' </summary>
    ''' <returns>DataTable物件</returns>
    ''' <remarks></remarks>
    Public Function GetInterChangeStartFree() As DataTable
        Dim dt As DataTable = Context.Cache("InterChangeStartFree")
        If dt Is Nothing Then
            If System.Configuration.ConfigurationManager.AppSettings("CacheMinute") IsNot Nothing Then
                CacheMinute = CInt(System.Configuration.ConfigurationManager.AppSettings("CacheMinute").ToString)
            End If
            'dt = ClassDB.RunReturnDataTable("select * from InterChangeStartFree where IsOnline > 0 ")
            dt = ClassDB.RunReturnDataTable("select * from InterChangeStartFree ")
            If dt.Rows.Count = 0 Then
                Context.Cache.Remove("InterChangeStartFree")
                Return New DataTable
            End If
            Context.Cache.Insert("InterChangeStartFree", dt, Nothing, DateTime.Now.AddMinutes(CacheMinute), System.Web.Caching.Cache.NoSlidingExpiration)
            Return dt

        End If
        Return Context.Cache("InterChangeStartFree")
    End Function
    ''' 取得最高費率
    ''' </summary>
    ''' <returns>DataTable物件</returns>
    ''' <remarks></remarks>
    Public Function GetMaxFare() As Double
        Dim maxfare As Double = 500
        If Context.Cache("InterChangeStartMaxFare") Is Nothing Then
            If System.Configuration.ConfigurationManager.AppSettings("CacheMinute") IsNot Nothing Then
                CacheMinute = CInt(System.Configuration.ConfigurationManager.AppSettings("CacheMinute").ToString)
            End If
            Context.Cache.Insert("InterChangeStartMaxFare", maxfare, Nothing, DateTime.Now.AddMinutes(CacheMinute), System.Web.Caching.Cache.NoSlidingExpiration)
        Else
            maxfare = Context.Cache("InterChangeStartMaxFare")
        End If
        Return maxfare
    End Function
    ''' 重新計算最高費率
    ''' </summary>
    ''' <returns>DataTable物件</returns>
    ''' <remarks></remarks>
    Public Sub SetMaxFare(ByVal fares As Double)
        If fares > 500 Then
            If System.Configuration.ConfigurationManager.AppSettings("CacheMinute") IsNot Nothing Then
                CacheMinute = CInt(System.Configuration.ConfigurationManager.AppSettings("CacheMinute").ToString)
            End If
            If Context.Cache("InterChangeStartMaxFare") Is Nothing Then
                Context.Cache.Insert("InterChangeStartMaxFare", fares, Nothing, DateTime.Now.AddMinutes(CacheMinute), System.Web.Caching.Cache.NoSlidingExpiration)
            Else
                Dim maxfare As Double = Context.Cache("InterChangeStartMaxFare")
                If fares > maxfare Then
                    Context.Cache.Insert("InterChangeStartMaxFare", fares, Nothing, DateTime.Now.AddMinutes(CacheMinute), System.Web.Caching.Cache.NoSlidingExpiration)
                End If
            End If
        End If
    End Sub
#End Region

End Module
