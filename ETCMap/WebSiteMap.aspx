<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="WebSiteMap.aspx.vb" Inherits="WebSiteMap" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHolder1" Runat="Server">
<table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
    <tr>
		<td width="1%" height="1%" align="left" valign="top">
			<img src="<%=Page.ResolveUrl("~/images/box_01.png") %>" alt=" " width="16" height="15" border="0"/></td>
		<td align="left" valign="top" background="<%=Page.ResolveUrl("~/images/box_02.png") %>"></td>
		<td width="1%" align="left" valign="top">
			<img src="<%=Page.ResolveUrl("~/images/box_03.png")%>" alt=" " width="18" height="15" border="0"/></td>
	</tr>
	<tr>
		<td align="left" valign="top" width="16px" background="<%=Page.ResolveUrl("~/images/box_04.png") %>">&nbsp;</td>
		<td align="left" valign="top" background="<%=Page.ResolveUrl("~/images/box_05.png") %>">
            <div class="sitemap">
                <asp:Literal id="ltl_Sitemap1" runat="server" Visible="True"></asp:Literal>
            </div>
		</td>
		<td align="left" valign="top" width="18px" background="<%=Page.ResolveUrl("~/images/box_06.png") %>">&nbsp;</td>
	</tr>
	<tr>
		<td align="left" valign="top">
			<img src="<%=Page.ResolveUrl("~/images/box_07.png") %>" alt=" " width="16" height="19" border="0"/></td>
		<td align="left" valign="top" background="<%=Page.ResolveUrl("~/images/box_08.png")%>"></td>
		<td align="left" valign="top">
			<img src="<%=Page.ResolveUrl("~/images/box_09.png")%>" alt=" " width="18" height="19" border="0"/></td>
	</tr>
</table>
</asp:Content>

