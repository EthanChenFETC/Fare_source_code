<%@ Control Language="VB" AutoEventWireup="false" CodeFile="FileManager.ascx.vb"
    Inherits="common_FileManager" %>
	    <div class="boxDownload">
                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                      <tr>
                        <td width="8%" valign="top" nowrap="nowrap"><strong><asp:Literal ID="lbDownloadText" runat="server"></asp:Literal></strong></td>
                        <td width="92%" valign="top">
    <asp:DataList ID="dl" runat="server" ShowFooter="False" ShowHeader="False" GridLines="None" RepeatLayout="flow">
        <ItemTemplate>
            <asp:HyperLink ID="lb_AttFile" runat="server" 
                CausesValidation="False" 
                />
            <asp:Label ID="lbDlCountTxt" runat="server" Visible="false"></asp:Label>
            <asp:Label ID="lbDlCount" runat="server" Visible="false"></asp:Label>
           
        </ItemTemplate>
    </asp:DataList><asp:Label ID="lbNoFile" runat="server"></asp:Label>                        </td>
                      </tr>
                    </table>
         </div>

