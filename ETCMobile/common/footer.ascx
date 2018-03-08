<%@ Control Language="VB" AutoEventWireup="false" CodeFile="footer.ascx.vb" Inherits="footer" %>
        <div class="footer">
            計算人次總數：<asp:Label ID="lblCount" runat="server"></asp:Label><br />
            <asp:HyperLink ID="hypc" runat="server" Text="電腦版" NavigateUrl="http://fare.fetc.net.tw/Default.aspx"></asp:HyperLink>
        </div>