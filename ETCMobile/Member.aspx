<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Member.aspx.vb" Inherits="Member" %>



<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="X-UA-Compatible" content="IE=EmulateIE8" />

    <title></title>
<asp:Literal ID="LitCss" runat="server"></asp:Literal>
<%--    <script src="js/jquery.js" language="javascript" type="text/javascript" ></script>--%>
<link rel="image_src" type="image/jpeg" href="images/100x100.jpg" />

   <%-- <script src="//ajax.googleapis.com/ajax/libs/jquery/2.0.0/jquery.min.js"></script>--%>
  <link rel="stylesheet" href="style.css" />
  

</head>

<body>
    <label id="testmessage"></label>
    <form id="form1" runat="server">
帳號：<asp:TextBox ID="TxtUID" runat="server" CssClass="id" Text="身分證字號或護照號碼"   ></asp:TextBox>
        <asp:Button ID="BtnLog" runat="server" AlternateText="登入" border="0" align="top" Text="登入"/>
       
    </form>
</body>
</html>
