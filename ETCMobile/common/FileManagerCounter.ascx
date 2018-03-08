<%@ Control Language="VB" AutoEventWireup="false" CodeFile="FileManagerCounter.ascx.vb"
    Inherits="common_FileManagerCounter" %>
<div id="AttFile">
            <div >
                <asp:Label ID="lbDownloadText" runat="server"></asp:Label></div>
            <div> 
    <asp:DataList ID="dl" runat="server" ShowFooter="False" ShowHeader="False" GridLines="None" RepeatLayout="flow">
        <ItemTemplate>
            <asp:Label ID="lbDlCount" runat="server"></asp:Label>
        </ItemTemplate>
    </asp:DataList></div>
</div>
