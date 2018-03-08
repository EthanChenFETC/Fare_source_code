<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Default.aspx.vb" Inherits="_Default" %>
<%@ Register Src="common/Top.ascx" TagName="TopArea" TagPrefix="uc1" %>
<%@ Register Src="common/footer.ascx" TagName="footer" TagPrefix="uc2" %>
<%@ Register Src="common/HomePageBlock.ascx" TagName="HomePageBlock" TagPrefix="uc3" %>
<%@ Register Src="common/HomeMarqeeBlock.ascx" TagName="HomeMarqeeBlock" TagPrefix="uc4" %>
<%@ Register Src="common/PublishBlock.ascx" TagName="PublishBlock" TagPrefix="uc5" %>
<%@ Register Src="common/LeftMenu.ascx" TagName="LeftMenu" TagPrefix="uc6" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="HEAD" runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width; initial-scale=1.0; user-scalable=1.0;">
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <link href="css/style.css" rel="stylesheet" type="text/css" />
    <link href="css/collapse.css" rel="stylesheet" type="text/css" />
<script src="http://code.jquery.com/jquery-1.12.3.js" type="text/javascript"></script>
<script src="http://code.jquery.com/ui/1.11.4/jquery-ui.js" type="text/javascript"></script>
<script src="css/MSClass.js" type="text/javascript"></script>
<title>國道高速公路局-計程通行費試算</title>

<script type="text/javascript">
    $(function () {
        $('body').on('click', '.btn_menu a', function (ev) {
            ev.preventDefault();
            var $pane = $('.pane');
            if ($pane.hasClass('slide')) {
                $pane.animate({
                    left: '0',
                }, 500, function () {
                    $pane.removeClass('slide');
                });
            } else {
                $pane.animate({
                    left: '-80%',
                }, 500, function () {
                    $pane.addClass('slide');
                    $pane.find('.left_slide').height($('body').height());
                });
            }
            return false;
        });

        $('.pane .left_slide').height($('body').height());
    });

    
  
</script>
</head>
<body id="body"  >
    <a name="top"></a>
        <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
<div class="pane cfx">
    <div class="wrapper">
        <div class="main">
            <uc1:TopArea ID="TopArea1" runat="server" />
            <uc4:HomeMarqeeBlock ID="HomeMarqeeBlock1" runat="server" />
            <uc3:HomePageBlock ID="HomePageBlock1" runat="server" />
            <uc5:PublishBlock ID="PublishBlock1" runat="server" BlockID="MobileHome" />
        </div>
        <uc2:footer ID="footer1" runat="server" />
    </div>
    <uc6:LeftMenu ID="LeftMenu1" runat="server" />
</div>
            </form>
</body>
</html>