<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Login.aspx.vb" Inherits="Login" ValidateRequest="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>�޲z�t��</title>
    <link href="common/Template2/login2.css" rel="stylesheet" type="text/css" />
</head>
<body>
<div class="Content">
    <form id="form1" runat="server">
		<div class="InputArea" style="text-align: center">
            <table cellspacing="3" cellpadding="0" width="100%" border="0">
                <tr>
                    <td valign="middle" Class="c12">
                        <asp:RequiredFieldValidator ID="RValidatorUserID" runat="server" ErrorMessage="�п�J�b���I"
                            Display="Dynamic" ControlToValidate="txtUserName" CssClass="c12" ValidationGroup="1"></asp:RequiredFieldValidator><asp:RequiredFieldValidator
                                ID="RValidatorUserPw" runat="server" ErrorMessage="�п�J�K�X�I" Display="Dynamic"
                                ControlToValidate="txtP" CssClass="c12" ValidationGroup="1"></asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtP"
                            CssClass="c12" Display="Dynamic" ErrorMessage="�п�J�K�X�I" ValidationGroup="3"></asp:RequiredFieldValidator></td>
              </tr>
                <tr align="center">
                    <td valign="middle" class="labeltxt">
                        �п�J�z���n�J���
                    </td>
              </tr>
                <tr align="center" >
                    <td valign="middle" class="labeltxt">
                        �b���G<asp:TextBox ID="txtUserName" runat="server" CssClass="txtUserName" MaxLength="15" TabIndex="0"
                            ToolTip="Input Your User Name!"></asp:TextBox></td>
              </tr>
                <tr align="center" >
                    <td valign="middle" class="labeltxt">
                        �K�X�G<asp:TextBox ID="txtP" runat="server" CssClass="txtPassword" MaxLength="15"
                            ToolTip="Input Your Password!" TextMode="Password"></asp:TextBox></td>
              </tr>
                <tr>
                    <td valign="middle" class="labeltxt">
                    <asp:ImageButton ID = "btnLogin" runat="server" imageUrl="common/Template2/images/btn_login.jpg" height="25" width="67" border="0" AlternateText ="�n�J" style="margin-left:40px" ValidationGroup="1" />
                    <br />
                    <asp:Label ID="lblMessage" runat="server" CssClass="c12" ForeColor="Red"></asp:Label>
              </tr>
            </table>
      <asp:Label ID="lblTime" runat="server" CssClass="labeltxt"></asp:Label></div>
        <input id="hrecive1" name="hrecive1" type="hidden" />
    </form>
	</div>
    <div class="footer"></div>
</body>
</html>
