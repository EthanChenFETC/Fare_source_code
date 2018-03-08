<%@ Page Language="VB" AutoEventWireup="false" CodeFile="PublishPreview.aspx.vb" Inherits="Publish_PublishPreview" EnableEventValidation="false" ValidateRequest="false"  %>
<%@ Register Src="../common/FileManager.ascx" TagName="FileManager" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>未命名頁面</title>
    <link href="images/style.css" type="text/css" rel="stylesheet" />
</head>
<body>
    <table id="ctl00_CPHolder1_Publisher1_tblOne" width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
	<tr>
		<td width="1%" height="1%" align="left" valign="top">
			<img src="images/box_01.png" alt=" " width="16" height="15" border="0"></td>
		<td align="left" valign="top" background="images/box_02.png"></td>
		<td width="1%" align="left" valign="top">
			<img src="images/box_03.png" alt=" " width="18" height="15" border="0"></td>
	</tr>
	<tr>
		<td align="left" valign="top" width="16px" background="images/box_04.png">&nbsp;</td>
		<td align="left" valign="top" background="images/box_05.png"><p class="title18"><asp:Literal ID="ltlSubject" runat="server"></asp:Literal></p>
            <div align="right" style="padding-bottom:5px;">
                    <a href="javascript: void(window.open('http://www.facebook.com/share.php?u='.concat(encodeURIComponent(location.href)) ));"><img src="../images/link_fb.gif" alt="推文到fackbook" width="22" height="22" border="0" align="top" title="推文到fackbook"></a><a href="javascript:desc='';if(window.getSelection)desc=window.getSelection();if(document.getSelection)desc=document.getSelection();if(document.selection)desc=document.selection.createRange().text;void(open('http://www.google.com/bookmarks/mark?op=add&amp;bkmk='+encodeURIComponent(location.href)+'&amp;title='+encodeURIComponent(document.title)+'&amp;annotation='+encodeURIComponent(desc)));"><img src="../images/link_google.gif" alt="新增Google Bookmarks" width="22" height="22" border="0" align="top" title="新增Google Bookmarks"></a><a href="javascript: void(window.open('http://www.plurk.com/?qualifier=shares&amp;status=' .concat(encodeURIComponent(location.href)) .concat(' ') .concat('(') .concat(encodeURIComponent(document.title)) .concat(')')));"><img src="../images/link_plurk.gif" alt="推文到plurk" width="22" height="22" border="0" align="top" title="推文到plur"></a><a href="javascript: void(window.open('http://twitter.com/home/?status='.concat(encodeURIComponent(document.title)) .concat(' ') .concat(encodeURIComponent(location.href))));"><img src="../images/link_twitter.gif" alt="推文到Twitter" width="22" height="22" border="0" align="top" title="推文到Twitter"></a></div>
            <div class="FCKContent" style="border:hidden">
                <asp:Literal ID="ltlContent" runat="server"></asp:Literal>
            </div>
	    <div class="boxDownload" id="divdownload" runat="server">
                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                      <tr>
                        <td width="8%" valign="top" nowrap="nowrap"><strong><asp:Literal ID="lbDownloadText" runat="server"></asp:Literal></strong></td>
                        <td width="92%" valign="top">
    <asp:DataList ID="dl" runat="server" ShowFooter="False" ShowHeader="False" GridLines="None" RepeatLayout="flow">
        <ItemTemplate>
            <asp:LinkButton ID="lb_AttFile" runat="server"       CausesValidation="False"  >
                <asp:Image ID="img_AttFile" runat="server" ImageAlign="AbsMiddle" ToolTip="Attach File Image!" AlternateText="Attach File Image!"></asp:Image>
            </asp:LinkButton>
          
            <asp:Label ID="lbDlCountTxt" runat="server" Visible="false"></asp:Label>
            <asp:Label ID="lbDlCount" runat="server" Visible="false"></asp:Label>
           
        </ItemTemplate>
    </asp:DataList><asp:Label ID="lbNoFile" runat="server"></asp:Label>                        </td>
                      </tr>
                    </table>
         </div>
            <span class="updatedTime">
                本頁最後更新日期：<%=(Date.Now.Year - 1911) & "-" & Date.Now.Month.ToString & "-" & Date.Now.Day.ToString %>
            </span>
        </td>
		<td align="left" valign="top" width="18px" background="images/box_06.png">&nbsp;</td>
	</tr>
	<tr>
		<td align="left" valign="top" width="16">
			<img src="images/box_07.png" alt=" " width="16" height="19" border="0"></td>
		<td align="left" valign="top" background="images/box_08.png"></td>
		<td align="left" valign="top" width="18" >
			<img src="images/box_09.png" alt=" " width="18" height="19" border="0"></td>
	</tr>
</table>
    <div align="center" ><input type="button" class="Btn" value="關閉視窗" onclick="window.close();" onkeypress="window.close();" /></div>
</body>
</html>
