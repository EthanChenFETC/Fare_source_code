<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Publisher4.ascx.vb" Inherits="common_Publisher4" %>
<%@ Register Assembly="CutePager" Namespace="CutePager" TagPrefix="cc1" %>
<%@ Register Src="FileManager.ascx" TagName="FileManager" TagPrefix="uc1" %>
<%@ Register Src="FileManagerCounter.ascx" TagName="FileManagerCounter" TagPrefix="uc2" %>

    <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" id="tblOne" runat="server" >
  <tr>
		<td width="1%" height="1%" align="left" valign="top">
			<img src="images/box_01.png" alt=" " width="16" height="15" border="0"></td>
		<td align="left" valign="top" background="images/box_02.png"></td>
		<td width="1%" align="left" valign="top">
			<img src="images/box_03.png" alt=" " width="18" height="15" border="0"></td>
	</tr>
	<tr>
		<td align="left" valign="top" width="16px" background="images/box_04.png">&nbsp;</td>
		<td align="left" valign="top" background="images/box_05.png"><p class="title18"><asp:Literal ID="ltSubject" runat="server"></asp:Literal><asp:Literal ID="ltUpdateDateTime" runat="server" visible="false" ></asp:Literal></p>
            <div align="right" style="padding-bottom:5px;">
                    <a href="javascript: void(window.open('http://www.facebook.com/share.php?u='.concat(encodeURIComponent(location.href)) ));"><img src="images/link_fb.gif" alt="推文到fackbook" width="22" height="22" border="0" align="top" title="推文到fackbook"></a>
                <!-- a href="javascript:desc='';if(window.getSelection)desc=window.getSelection();if(document.getSelection)desc=document.getSelection();if(document.selection)desc=document.selection.createRange().text;void(open('http://www.google.com/bookmarks/mark?op=add&amp;bkmk='+encodeURIComponent(location.href)+'&amp;title='+encodeURIComponent(document.title)+'&amp;annotation='+encodeURIComponent(desc)));">
                    <img src="images/link_google.gif" alt="新增Google Bookmarks" width="22" height="22" border="0" align="top" title="新增Google Bookmarks"></!-->
                <a href="javascript: void(window.open('https://plus.google.com/share?url=' + encodeURIComponent(location.href), '', 'menubar=no,toolbar=no,scrollbars=yes,resizable=yes,height=600,width=600'));"><img src="images/link_google.gif" alt="在 Google+ 分享" width="22" height="22" border="0" align="top" title="在 Google+ 分享"></a>
                
                <a href="javascript: void(window.open('http://www.plurk.com/?qualifier=shares&amp;status=' .concat(encodeURIComponent(location.href)) .concat(' ') .concat('(') .concat(encodeURIComponent(document.title)) .concat(')')));"><img src="images/link_plurk.gif" alt="推文到plurk" width="22" height="22" border="0" align="top" title="推文到plur"></a><a href="javascript: void(window.open('http://twitter.com/home/?status='.concat(encodeURIComponent(document.title)) .concat(' ') .concat(encodeURIComponent(location.href))));"><img src="images/link_twitter.gif" alt="推文到Twitter" width="22" height="22" border="0" align="top" title="推文到Twitter"></a></div>
            <div class="FCKContent">
                <asp:Literal ID="ltContent" runat="server"></asp:Literal>
            </div>
            <uc1:FileManager ID="FileManager1" runat="server"  counterenable="1" />
            <table border="0" cellpadding="2" cellspacing="0" width="100%" style ="display:none" >
                <tr>
                    <td class="font_1">
                        <asp:Label ID="LabelForPublishDate" runat="server" Visible="False"></asp:Label>
                        <asp:Label ID="lbPostDate" runat="server" Visible="False"></asp:Label></td>
                </tr>
                <tr>
                    <td class="font_1">
                    </td>
                </tr>
                <tr>
                    <td class="font_1">
                        <asp:Label ID="LabelForViewCount" runat="server"></asp:Label>
                        <asp:Label ID="lbViewCount" runat="server" Visible ="false" ></asp:Label></td>
                </tr>
            </table>
            <span class="updatedTime">
                <asp:Literal ID="LabelForLastUpdate" runat="server"></asp:Literal>
                <asp:Literal ID="lbLastUpdate" runat="server"></asp:Literal>
            </span>
        </td>
		<td align="left" valign="top" width="18px" background="images/box_06.png">&nbsp;</td>
	</tr>
	<tr>
		<td align="left" valign="top">
			<img src="images/box_07.png" alt=" " width="16" height="19" border="0"></td>
		<td align="left" valign="top" background="images/box_08.png"></td>
		<td align="left" valign="top">
			<img src="images/box_09.png" alt=" " width="18" height="19" border="0"></td>
	</tr>
    </table>
    <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" id="tblList" runat="server" >
  <tr>
		<td width="1%" height="1%" align="left" valign="top">
			<img src="images/box_01.png" alt=" " width="16" height="15" border="0"></td>
		<td align="left" valign="top" background="images/box_02.png"></td>
		<td width="1%" align="left" valign="top">
			<img src="images/box_03.png" alt=" " width="18" height="15" border="0"></td>
	</tr>
	<tr>
		<td align="left" valign="top" width="16px" background="images/box_04.png">&nbsp;</td>
		<td align="left" valign="top" background="images/box_05.png">
<asp:GridView ID="gv" runat="server" CssClass="newslist" AutoGenerateColumns="False"
    PageSize="10" BorderStyle="None" PagerSettings-LastPageText=">>" 
    PagerSettings-Mode="NumericFirstLast"  BorderWidth="0px"  Width="100%"
    ShowFooter="false" GridLines="None"  AllowPaging="true" PagerSettings-Visible="false"
    showheader = "true"  PagerStyle-CssClass ="scott" OnRowCommand="GridView1_RowCommand">
    <Columns>
        <asp:TemplateField HeaderText="功能" Visible="false"></asp:TemplateField>
        <asp:TemplateField HeaderText="消息標題" ShowHeader="true"  ItemStyle-Width="88%" HeaderStyle-VerticalAlign="Top" HeaderStyle-Wrap="false" HeaderStyle-BackColor ="#dfe7be" HeaderStyle-HorizontalAlign="Center">
            <ItemStyle Wrap="False" HorizontalAlign="left" VerticalAlign ="top" />
            <ItemTemplate>
                <asp:Literal ID="LiteralSubject" runat="server"></asp:Literal>
                <asp:HyperLink ID="hl_Subject" runat="server" ></asp:HyperLink>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="PublishDate" HeaderText="發布日期"  ShowHeader="true" ItemStyle-Width="12%" HeaderStyle-VerticalAlign="Top" HeaderStyle-Wrap="false" HeaderStyle-BackColor ="#dfe7be" HeaderStyle-HorizontalAlign="Center">
            <ItemStyle Wrap="False" HorizontalAlign="center" VerticalAlign ="top" />
        </asp:BoundField>
        <asp:TemplateField HeaderText="附件" Visible="false">
        <ItemStyle CssClass="dg_List_cell1"  />
            <ItemTemplate>
                <uc1:FileManager ID="FileManager2" runat="server" AttFile_IsShowTitle="false" AttFile_IsShowEmptyWord="false" AttFile_WordLimit="12" />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="下載次數" Visible="false">
        <ItemStyle CssClass="dg_List_cell1" />
            <ItemTemplate>
                <uc2:FileManagerCounter ID="FileManagerCounter2" runat="server" AttFile_IsShowTitle="false" AttFile_IsShowEmptyWord="false" AttFile_WordLimit="8"  CounterEnable="0"/>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:ButtonField ButtonType="Button" CommandName="ViewItem" Text ="Test" Visible="false"  />
    </Columns>

</asp:GridView>
<DIV align="right"><asp:Literal ID="ltlPageCount" runat="server"></asp:Literal></DIV>
<div align="right" class="scott" id="LitPage"><asp:Literal ID="ltlPager" runat="server"></asp:Literal></div> 
<asp:Literal ID="ltl_dgMessage" runat="server"></asp:Literal>
        </td>
		<td align="left" valign="top" width="18px" background="images/box_06.png">&nbsp;</td>
	</tr>
	<tr>
		<td align="left" valign="top">
			<img src="images/box_07.png" alt=" " width="16" height="19" border="0"></td>
		<td align="left" valign="top" background="images/box_08.png"></td>
		<td align="left" valign="top">
			<img src="images/box_09.png" alt=" " width="18" height="19" border="0"></td>
	</tr>
    </table>

<asp:Panel ID="PublisherDetail" runat="server" ></asp:Panel> 


