<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="Faq.aspx.vb" Inherits="Faq_Default" %>
<%@ Register Src="~/common/FileManager.ascx" TagName="FileManager" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHolder1" Runat="Server">
<table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
  <tr>
		<td width="1%" height="1%" align="left" valign="top">
			<img src="../images/box_01.png" alt=" " width="16" height="15" border="0"></td>
		<td align="left" valign="top" background="../images/box_02.png"></td>
		<td width="1%" align="left" valign="top">
			<img src="../images/box_03.png" alt=" " width="18" height="15" border="0"></td>
	</tr>
	<tr>
		<td align="left" valign="top" width="16px" background="../images/box_04.png">&nbsp;</td>
		<td align="left" valign="top" background="../images/box_05.png">
    <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
        <asp:View ID="View1" runat="server">
<div class="search"><label for="select"></label>
    <asp:DropDownList ID="ddl1" runat="server" AppendDataBoundItems="True" DataSourceID="sds_FaqCatgry_List"
        DataTextField="CategoryName" DataValueField="CateGoryID" AutoPostBack="True">
        <asp:ListItem Value="0">請選擇分類</asp:ListItem>
    </asp:DropDownList><asp:TextBox ID="txtSearch" runat="server" size="50" ></asp:TextBox>&nbsp;<asp:Button ID="ibtnSearch" runat="server"  AlternateText="搜尋按鈕" Text="搜尋"  CssClass="btn btn-success"/>&nbsp;<asp:Button
        ID="ibtnClear" runat="server" AlternateText="清除按鈕" Text="清除" CssClass="btn  btn-success" /><asp:SqlDataSource ID="sds_FaqCatgry_List" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
        ProviderName="<%$ ConnectionStrings:AdminModuleConnectionString.ProviderName %>"
        SelectCommand="SELECT FaqCatgry.CateGoryID, FaqCatgry.CateGoryName FROM FaqCatgry INNER JOIN SitesApRelation ON FaqCatgry.CateGoryID = SitesApRelation.ApUID WHERE (FaqCatgry.IsOnline = 1) AND (SitesApRelation.SiteID = @SiteID) AND (SitesApRelation.ApKeyword = 'Faq')">
        <SelectParameters>
            <asp:Parameter Name="SiteID" />
        </SelectParameters>
    </asp:SqlDataSource>
    </div> 
    <asp:GridView ID="GridView1" runat="server" DataSourceID="sds_Faq_List"
         AutoGenerateColumns="False" CssClass="qalist" AllowSorting="false" 
        BorderStyle="None" PagerSettings-LastPageText=">>" PagerSettings-Mode="NumericFirstLast"  BorderWidth="0px"  Width="100%"
        ShowFooter="false" GridLines="None"  AllowPaging="true" PagerSettings-Visible="false"
        showheader = "true"  PagerStyle-CssClass ="scott" OnRowCommand="GridView1_RowCommand"
        PageSize="10" EmptyDataText="目前沒有資料，或您的查詢沒有資料。" EmptyDataRowStyle-BorderColor="Red" >
        <HeaderStyle  wrap="false"  BackColor="#b1d37a" HorizontalAlign="Center"   />
        <Columns>
            <asp:BoundField DataField="CateGoryName" HeaderText="類別" SortExpression="CateGoryName">
                <ItemStyle Width="20%" />
            </asp:BoundField>
            <asp:TemplateField HeaderText="問題" SortExpression="Subject">
                <EditItemTemplate>
                    &nbsp;
                </EditItemTemplate>
                <ItemTemplate><asp:Label ID="lblIndex" runat="server" CssClass ="color14592f" Visible="false" ></asp:Label>
                    <span class="color000000"><asp:LinkButton ID="lbtn1" runat="server"
                        CommandName="ViewItem"></asp:LinkButton></span> 
                </ItemTemplate>
                <ItemStyle Width="80%" />
            </asp:TemplateField>
            <asp:ButtonField ButtonType="Button" CommandName="ViewPager" Text ="Test" Visible="false"  />
        </Columns>
        <EmptyDataRowStyle BorderColor="Red" />
        <EmptyDataTemplate >
            <font color=red><b>目前沒有資料，或您的查詢沒有資料。</b></font>
        </EmptyDataTemplate>
    </asp:GridView>
<DIV align="right"><asp:Literal ID="ltlPageCount" runat="server"></asp:Literal></DIV>
<div align="right" class="scott" id="LitPage"><asp:Literal ID="ltlPager" runat="server"></asp:Literal></div> 

    <asp:SqlDataSource ID="sds_Faq_List" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
        SelectCommand="SELECT Faq.PublicID, Faq.Subject, Faq.Content, FaqCatgry.CateGoryName, Faq.PublishDate, Faq.PublishExpireDate FROM Faq INNER JOIN FaqCatgry ON Faq.CateGoryID = FaqCatgry.CateGoryID INNER JOIN SitesApRelation ON FaqCatgry.CateGoryID = SitesApRelation.ApUID WHERE (Faq.Subject LIKE N'%' + @Keyword + N'%') AND (@CateGoryID = 0 or Faq.CateGoryID = @CateGoryID) AND (SitesApRelation.SiteID = @SiteID) AND (SitesApRelation.ApKeyword = 'Faq') AND ((DATEDIFF(day, PublishDate, GETDATE()) >= 0) AND (DATEDIFF(day, GETDATE(), PublishExpireDate) >= 0) AND (DATEDIFF(day, PublishDate, GETDATE()) >= 0) AND (DATEDIFF(day, GETDATE(), PublishExpireDate) >= 0)) ORDER BY Faq.PublishDate DESC, FaqCatgry.CateGoryID, Faq.PublicID DESC">
        <SelectParameters>
            <asp:ControlParameter ControlID="ddl1" Name="CateGoryID" PropertyName="SelectedValue"
                Type="String" DefaultValue="0" />
            <asp:ControlParameter ControlID="txtSearch" DefaultValue="%" Name="Keyword" PropertyName="Text" />
            <asp:Parameter Name="SiteID" />
        </SelectParameters>
    </asp:SqlDataSource>
        </asp:View>
        <asp:View ID="View2" runat="server">
    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="qalist">
        <tr>
            <th width="10%" nowrap="nowrap" bgcolor="#C6DE9C"><p>
                <asp:Literal ID="Label5" runat="server"></asp:Literal></p> </td>
            <td class="linetd" style="width:90%">
                <asp:Literal ID="Label1" runat="server"></asp:Literal></td>
        </tr>
        <tr>
            <th style="width: 10%; background-color: #C6DE9C;"><p>
                <asp:Literal ID="Label6" runat="server"></asp:Literal></p></th>
            <td class="linetd" style="width:90%">
                <asp:Literal ID="Label2" runat="server"></asp:Literal></td>
        </tr>
        <tr>
            <th style="width: 10%; background-color: #C6DE9C;"><p>
                <asp:Literal ID="Label7" runat="server"></asp:Literal></p></th>
            <td class="linetd" style="width:90%">
                <asp:Literal ID="Label3" runat="server"></asp:Literal></td>
        </tr>
        <tr style="display:none">
            <td class="linetd" style="width: 60px; background-color: #C6DE9C;">
                <asp:Literal ID="Label8" runat="server"></asp:Literal></td>
            <td class="linetd" style="width:90%">
                <asp:Literal ID="Label4" runat="server"></asp:Literal></td>
        </tr>
    </table>
        <br />
          <uc1:FileManager ID="FileManager1" runat="server" />
     
        <div class="FCKdown" style="display:none">
        <p><asp:Label ID="LabelForPublishDate" runat="server"></asp:Label>
                    <asp:Label ID="lbPostDate" runat="server"></asp:Label></p>
        <p><asp:Label ID="LabelForLastUpdate" runat="server"></asp:Label>
                    <asp:Label ID="lbLastUpdate" runat="server"></asp:Label></p>
        <p><asp:Label ID="LabelForViewCount" runat="server"></asp:Label>
                    <asp:Label ID="lbViewCount" runat="server"></asp:Label></div>
<div class="line"></div>
<div align="center"><button type="button" class="btn btn-success" onclick="javascript:location.href='Faq.aspx';">返回Q&amp;A清單</button><div>
    <asp:SqlDataSource ID="sds_Detail" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
        ProviderName="<%$ ConnectionStrings:AdminModuleConnectionString.ProviderName %>"
        SelectCommand="SELECT Faq.CateGoryID, Faq.Subject, Faq.Content, Faq.PublishDate, Faq.UpdateDateTime, Faq.PageViewCount, Faq.AttFiles, Faq.PublicID, FaqCatgry.CateGoryName FROM Faq INNER JOIN FaqCatgry ON Faq.CateGoryID = FaqCatgry.CateGoryID WHERE (Faq.PublicID = @PublicID)"
        UpdateCommand="UPDATE Faq SET PageViewCount = PageViewCount + 1 WHERE (PublicID = @PublicID)">
        <UpdateParameters>
            <asp:Parameter Name="PublicID" />
        </UpdateParameters>
        <SelectParameters>
            <asp:QueryStringParameter Name="PublicID" QueryStringField="p" />
        </SelectParameters>
    </asp:SqlDataSource></asp:View>
    </asp:MultiView>
    </td>
		<td align="left" valign="top" width="18px" background="../images/box_06.png">&nbsp;</td>
	</tr>
	<tr>
		<td align="left" valign="top">
			<img src="../images/box_07.png" alt=" " width="16" height="19" border="0"></td>
		<td align="left" valign="top" background="../images/box_08.png"></td>
		<td align="left" valign="top">
			<img src="../images/box_09.png" alt=" " width="18" height="19" border="0"></td>
	</tr>
</table>
</asp:Content>

