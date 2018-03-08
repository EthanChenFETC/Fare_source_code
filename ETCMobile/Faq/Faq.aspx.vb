Imports System.Data
Imports System.Data.SqlClient

''' <summary>
''' 問與答網頁(網頁表單)
''' </summary>
''' <remarks></remarks>
Partial Class Faq_Default
    Inherits System.Web.UI.Page

    Private SiteID As Integer = CInt(System.Configuration.ConfigurationManager.AppSettings("SiteID")).ToString

    ''' <summary>
    ''' 頁面讀取事件
    ''' </summary>
    ''' <param name="sender">觸發本事件控制項</param>
    ''' <param name="e">觸發本事件帶進之參數</param>
    ''' <remarks>網址參數p如果有帶值即會呈現內容頁</remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.sds_FaqCatgry_List.SelectParameters("SiteID").DefaultValue = SiteID
        Me.sds_Faq_List.SelectParameters("SiteID").DefaultValue = SiteID

        If Not IsPostBack Then
            'If Request.Params("p") Is Nothing OrElse Request.Params("p") = "" Then
            '    Server.Transfer("~/Faq/Default.aspx")
            'Else
            '    Try
            '        PublicID = CInt(Request.Params("p"))
            '    Catch ex As Exception
            '        Server.Transfer("~/Faq/Default.aspx")
            '    End Try
            '    doBindData()
            'End If
            'Dim PublicID As Integer
            'If Request.Params("p") <> Nothing Then
            '    Try
            '        PublicID = CInt(Request.Params("p"))
            '        Me.MultiView1.ActiveViewIndex = 1
            '        doBindData()
            '    Catch ex As Exception
            '        Me.MultiView1.ActiveViewIndex = 0
            '    End Try
            'Else
            'add by Chris Chu 20090211 接受其他網站直接連結常見問答類別
            Dim cnid As String = Request.Form.Get("cnid")
            If IsNumeric(cnid) And Not CheckSqlInjectionWording(cnid) Then
                Dim sqlStr As String = "SELECT FaqCatgry.CateGoryID FROM FaqCatgry INNER JOIN SitesApRelation ON FaqCatgry.CateGoryID = SitesApRelation.ApUID WHERE (FaqCatgry.IsOnline = 1) AND (SitesApRelation.SiteID = @SiteID ) AND (SitesApRelation.ApKeyword = 'Faq') and ( FaqCatgry.CateGoryID = @CateGoryID)" '  & cnid.ToString & ")"
                Using dr As SqlDataReader = ClassDB.GetDataReaderParam(sqlStr, New SqlParameter("@SiteID", SiteID), New SqlParameter("@CateGoryID", CInt(cnid)))
                    Try
                        If dr IsNot Nothing Then
                            If dr.Read Then
                                Me.ddl1.SelectedValue = cnid.ToString
                            End If
                        End If
                    Catch ex As SqlException
                    Finally
                    End Try
                End Using
            End If
        End If
        'End If
    End Sub

#Region "Faq List"

    ''' <summary>
    ''' GridView標題列文章設定格式
    ''' </summary>
    ''' <param name="sender">觸發本事件控制項</param>
    ''' <param name="e">觸發本事件帶進之參數</param>
    ''' <remarks></remarks>
    Protected Sub GridView1_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.DataBound
        'GridViewPageInfo.GetGridViewInfo3(Me.GridView1, Me.Page, Me.sds_Faq_List, False)
        Dim dt As DataTable = CType(Me.sds_Faq_List.Select(New DataSourceSelectArguments), DataView).Table
        '        GridViewPageInfo.GetGridViewInfo3(GridView1, Me.Page, Nothing, True, dt)
        ''GridViewPageInfo.BindPager(Me.Page, Nothing, Me.GridView1, Me.sds_Faq_List)
        BindPager(dt)
        ''GridView標題列文章設定
        'If Me.GridView1.Rows.Count > 0 Then
        '    Dim drow As TableRow = GridView1.HeaderRow
        '    'drow.Cells(0).Text = GlobalResourcesCulture.GetGlobalResourcesString("TableHeadSNO")
        '    drow.Cells(0).Text = GlobalResourcesCulture.GetGlobalResourcesString("TableHeadCategory")
        '    drow.Cells(1).Text = GlobalResourcesCulture.GetGlobalResourcesString("TableHeadFaqQ")
        'End If
    End Sub

    ''' <summary>
    ''' 資料搜尋關鍵字清除-按鈕Click事件
    ''' </summary>
    ''' <param name="sender">觸發本事件控制項</param>
    ''' <param name="e">觸發本事件帶進之參數</param>
    ''' <remarks></remarks>
    Protected Sub ibtnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ibtnClear.Click
        Me.txtSearch.Text = ""
        Me.GridView1.DataBind()
    End Sub

    ''' <summary>
    ''' 自訂頁碼
    ''' </summary>
    ''' <param name="dt">DataTable提供計算資料數使用</param>
    ''' <param name="Show">是否呈現自訂頁碼預設為是，但某些情況可以要求不要顯示</param>
    ''' <remarks></remarks>
    Private Sub BindPager(ByVal dt As DataTable, Optional ByVal Show As Boolean = True)

        'If Show = False Then
        Dim TotalPageCount As Integer = Math.Ceiling(dt.Rows.Count / Me.GridView1.PageSize)
        Me.ltlPageCount.Text = "頁碼 " & (Me.GridView1.PageIndex + 1).ToString & " / " & TotalPageCount.ToString & " 頁 總計 " & dt.Rows.Count.ToString & " 筆資料"

        Dim endpage As Integer = IIf(TotalPageCount > 10, 10, TotalPageCount)
        If Me.GridView1.PageIndex + 1 > 10 Then
            Me.ltlPager.Text = "<a href=""javascript:__doPostBack('" & GridView1.ClientID.Replace("_", "$") & "','Page$" & (Me.GridView1.PageIndex - (Me.GridView1.PageIndex Mod 10) + 1).ToString & "')"" title=""上十頁"" ><<</a>"
        Else
            Me.ltlPager.Text = ""
        End If
        If Me.GridView1.PageIndex > 0 Then
            Me.ltlPager.Text = "<a href=""javascript:__doPostBack('" & GridView1.ClientID.Replace("_", "$") & "','Page$" & (Me.GridView1.PageIndex - 1 + 1).ToString & "')"" title=""上一頁"" ><</a>"
        Else
            Me.ltlPager.Text = "<span class=""disabled"" title=""上一頁"" ><</span>"
        End If
        'Me.GridView1.PageIndex = IIf(Me.GridView1.PageIndex = 0, 1, Me.GridView1.PageIndex)

        For i As Integer = 1 To endpage
            If i - 1 = Me.GridView1.PageIndex Then
                Me.ltlPager.Text &= "<span class=""disabled""><a  class=""current"" title=""第" & i.ToString & """ >" & (i).ToString & "</a/></span>"
            Else
                Me.ltlPager.Text &= "<a href=""javascript:__doPostBack('" & GridView1.ClientID.Replace("_", "$") & "','Page$" & (i).ToString & "')"" title=""第" & i.ToString & "頁"" >" & (i).ToString & "</a>"
            End If
        Next
        If Me.GridView1.PageIndex + 1 = TotalPageCount Then
            Me.ltlPager.Text &= "<span class=""disabled"" title=""下一頁"" >></span>"
        Else
            Me.ltlPager.Text &= "<a href=""javascript:__doPostBack('" & GridView1.ClientID.Replace("_", "$") & "','Page$" & (Me.GridView1.PageIndex + 2).ToString & "')"" title=""下一頁"" >></a>"
        End If
        If Me.GridView1.PageIndex < TotalPageCount - (TotalPageCount Mod 10) + 1 And TotalPageCount > 10 Then
            Me.ltlPager.Text &= "<a href=""javascript:__doPostBack('" & GridView1.ClientID.Replace("_", "$") & "','Page$" & (Me.GridView1.PageIndex + (10 - (Me.GridView1.PageIndex Mod 10)) + 1).ToString & "')"" title=""下十頁"" >>></a>"
        End If

        Exit Sub

    End Sub
    ''' <summary>
    ''' 問與答清單命令
    ''' </summary>
    ''' <param name="sender">觸發本事件控制項</param>
    ''' <param name="e">觸發本事件帶進之參數</param>
    ''' <remarks>重導頁面至詳細內容頁</remarks>
    Protected Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand
        If e.CommandName = "ViewItem" Then
            'Chris Chu 20090417 I不知道為什麼要用response redirect 直接用postback不就好了嗎
            Dim PublicID As Integer
            Try
                PublicID = e.CommandArgument
                Me.MultiView1.ActiveViewIndex = 1
                doBindData(PublicID)
            Catch ex As Exception
                Me.MultiView1.ActiveViewIndex = 0
            End Try

        End If
        If e.CommandName = "ViewPager" Then
            Try
                Me.GridView1.PageIndex = IIf(e.CommandArgument < 0, 0, e.CommandArgument)
                Me.GridView1.DataBind()
                BindPager(Me.GridView1.DataSource)
            Catch ex As Exception
            End Try
        End If
        'If e.CommandName = "ViewItem" Then
        '    If Request.RawUrl.IndexOf("?") = -1 Then
        '        Response.Redirect(Request.RawUrl & "?p=" & e.CommandArgument)
        '    Else
        '        Response.Redirect(Request.RawUrl & "&p=" & e.CommandArgument)
        '    End If
        'End If
    End Sub


    ''' <summary>
    ''' 問與答清單項目資料繫結設定
    ''' </summary>
    ''' <param name="sender">觸發本事件控制項</param>
    ''' <param name="e">觸發本事件帶進之參數</param>
    ''' <remarks>設定清單中的ToolTip內文及其格式</remarks>
    Protected Sub GridView1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView1.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then
            'dim datacol as d
        End If
        If e.Row.RowType = DataControlRowType.DataRow Then
            If ((e.Row.RowState = DataControlRowState.Normal) OrElse (e.Row.RowState = DataControlRowState.Alternate)) Then
                Dim drowView As System.Data.DataRowView = CType(e.Row.DataItem, System.Data.DataRowView)
                Dim lblIndex As Label = CType(e.Row.FindControl("lblIndex"), Label)
                lblIndex.Text = "Q" & (e.Row.RowIndex + 1).ToString
                Dim lbtn1 As LinkButton = CType(e.Row.FindControl("lbtn1"), LinkButton)
                Dim Content As String = drowView.Item("Content").ToString
                Content = Content.Replace("<p>", vbCrLf)
                Content = ModuleMisc.htmlRemove(Content)
                lbtn1.ToolTip = Content
                lbtn1.Text = markKeyWord((drowView("Subject")).ToString, "red") 'add Chris Chu 20090520 for search text marked
                lbtn1.CommandArgument = drowView("PublicID").ToString
            End If
        End If

    End Sub

    ''' <summary>
    ''' 下拉選單之多語系字詞設定
    ''' </summary>
    ''' <param name="sender">觸發本事件控制項</param>
    ''' <param name="e">觸發本事件帶進之參數</param>
    ''' <remarks></remarks>
    Protected Sub ddl1_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddl1.DataBound
        If Me.ddl1.Items.Count > 0 Then
            Me.ddl1.Items(0).Text = GlobalResourcesCulture.GetGlobalResourcesString("DropDownListIndex0")
        End If
    End Sub

#End Region



#Region "Detail"


    ''' <summary>
    ''' 繫結詳細資料頁面
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub doBindData(ByVal PublicID As Integer)

        'Dim PublicID As Integer = CInt(Request.Params("p"))

        Me.Label5.Text = GlobalResourcesCulture.GetGlobalResourcesString("LabelForFaqCatgry")
        Me.Label6.Text = GlobalResourcesCulture.GetGlobalResourcesString("LabelForFaqQ")
        Me.Label7.Text = GlobalResourcesCulture.GetGlobalResourcesString("LabelForFaqA")
        Me.Label8.Text = GlobalResourcesCulture.GetGlobalResourcesString("LabelForFaqSource")

        Me.LabelForPublishDate.Text = GlobalResourcesCulture.GetGlobalResourcesString("LabelForPublishDate")
        Me.LabelForLastUpdate.Text = GlobalResourcesCulture.GetGlobalResourcesString("LabelForLastUpdate")
        Me.LabelForViewCount.Text = GlobalResourcesCulture.GetGlobalResourcesString("LabelForViewCount")


        Dim sql As String = Me.sds_Detail.SelectCommand '.Replace("@PublicID", PublicID)
        Using dr As SqlDataReader = ClassDB.GetDataReaderParam(sql, New SqlParameter("@PublicID", PublicID))
            Try
                If dr IsNot Nothing Then
                    If dr.Read Then
                        'Me.ltSubject.Text = dr("Subject").ToString
                        'Me.ltContent.Text = dr("Content").ToString
                        'Me.ltUpdateDateTime.Text = "<span class=""DateTime"">Last Updated Time:" & dr("UpdateDateTime").ToString & "</span>"
                        PublicID = CInt(dr("PublicID"))

                        'Me.Page.Title += "-" & dr("Subject").ToString  '2008/1/24小羅來電,檔案局確定A+取消這個只留網站名稱

                        'Me.Label1.Text = dr("CateGoryName").ToString
                        'Me.Label2.Text = dr("Subject").ToString
                        'Me.Label3.Text = dr("Content").ToString
                        '20090417 Chris Chu add 標示關鍵字
                        '''''''''''''''''''''''
                        Me.Label1.Text = markKeyWord((dr("CateGoryName")).ToString, "red")
                        Me.Label2.Text = markKeyWord((dr("Subject")).ToString, "red")
                        Me.Label3.Text = markKeyWord((dr("Content")).ToString(), "red")
                        '''''''''''''''''''''''
                        Me.Label4.Text = "國家發展委員會檔案管理局" 'System.Configuration.ConfigurationManager.AppSettings("SiteName").ToString


                        Dim PublishDate As Date = CType(dr("PublishDate"), Date)
                        Dim UpdateDateTime As Date = CType(dr("UpdateDateTime"), Date)

                        If GlobalResourcesCulture.GetCurrentCulture() = "zhTW" Then
                            PublishDate = PublishDate.AddYears(-1911)
                            UpdateDateTime = UpdateDateTime.AddYears(-1911)
                            Me.lbPostDate.Text = PublishDate.ToString("yyy.MM.dd", System.Globalization.DateTimeFormatInfo.InvariantInfo)
                            'Me.lbLastUpdate.Text = UpdateDateTime.ToString("yyy.MM.dd tt hh:mm:ss", System.Globalization.DateTimeFormatInfo.InvariantInfo).Replace("AM", "上午").Replace("PM", "下午")
                            Me.lbLastUpdate.Text = UpdateDateTime.ToString("yyy.MM.dd", System.Globalization.DateTimeFormatInfo.InvariantInfo) 'Chris 20090522
                        Else
                            Me.lbPostDate.Text = PublishDate.ToString
                            Me.lbLastUpdate.Text = UpdateDateTime.ToString
                        End If
                        Me.lbViewCount.Text = (Convert.ToInt32(dr("PageViewCount")).ToString)
                        If Not CheckSqlInjectionWording(dr.Item("AttFiles")) Then
                            FileManager1.doDataBind(dr.Item("AttFiles").ToString)
                        End If
                    End If
                End If
            Catch ex As Exception
                WriteErrLog(ex, Me)
            Finally

            End Try
        End Using
        'SELECT CateGoryID, Subject, Content, PublishDate, UpdateDateTime, PageViewCount, AttFiles, PublicID FROM Faq WHERE (PublicID = @PublicID)

        '更新計數器
        Try
            sql = Me.sds_Detail.UpdateCommand '.Replace("@PublicID", PublicID)
            'Me.ltSubject.Text = sql
            ClassDB.UpdateDBText(sql, New SqlParameter("@PublicID", PublicID))
        Catch ex As Exception
            WriteErrLog(ex, Me)
        End Try
    End Sub
    Private Function markKeyWord(ByVal drContent As String, ByVal FontColor As String) As String
        '20090417 Chris Chu add 標示關鍵字
        Dim kWord As String = Me.txtSearch.Text
        Dim Contents As String = ""
        Dim ContentsOrg As String = drContent
        Dim startIndex As Integer = 0
        If kWord IsNot Nothing And kWord <> "" Then
            While startIndex < ContentsOrg.Length
                Dim kwordDistance As Integer = ContentsOrg.Substring(startIndex).IndexOf(kWord)
                If kwordDistance < 0 Then
                    Contents += ContentsOrg.Substring(startIndex)
                    Exit While
                Else
                    Contents += ContentsOrg.Substring(startIndex, kwordDistance)
                    Contents += "<font Color=""" & FontColor & """>" & (kWord) & "</font>"
                End If
                startIndex += kwordDistance + kWord.Length
            End While
        Else
            Contents = ContentsOrg
        End If
        Return Contents
    End Function
#End Region



    End Class
