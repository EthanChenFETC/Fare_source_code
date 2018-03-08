<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SystemErrors.aspx.vb" Inherits="SystemErrors" %>
<%@ Register TagPrefix="blocks" TagName="Styles" Src="~/common/styles.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>未命名頁面</title>
    <blocks:Styles ID="Styles" runat="server"></blocks:Styles>
    
</head>
<body>
    <form id="form1" runat="server">
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
             
             
             <div style="border:#0066CC dashed 1px;margin:auto;font-size:13px;width:420px;height:180px;background:#fff; padding-right: 0px; padding-left: 0px; padding-bottom: 0px; padding-top: 0px;">
                 <div style="padding-right: 8px; padding-left: 8px; font-weight: bold; font-size: 17px; padding-bottom: 8px; color: #ffffff; padding-top: 8px; background-color: #0033cc">錯誤！</div>
                 <div style="padding-right: 8px; padding-left: 8px; padding-bottom: 8px; line-height: 24px; padding-top: 8px; overflow: visible;">非常抱歉，系統運作出現錯誤！<br />
                 系統已記錄錯誤事件資料，並交由相關人員進行偵錯與修復工作。<br />
                     1.或點選
                 <asp:LinkButton ID="lkbHome" runat="server" OnClick="lkbHome_Click">回到管理系統</asp:LinkButton>
                 首頁重新操作。
                 <br />
                     2.<asp:HyperLink ID="hyLogout" runat="server">登出系統</asp:HyperLink>。</div></div>
    </form>
</body>
</html>
