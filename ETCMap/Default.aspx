<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Default.aspx.vb" Inherits="_Default" %>
<%@ Register Src="common/Top.ascx" TagName="TopArea" TagPrefix="uc1" %>
<%@ Register Src="common/footer.ascx" TagName="footer" TagPrefix="uc2" %>
<%@ Register Src="common/HomePageBlock.ascx" TagName="HomePageBlock" TagPrefix="uc3" %>
<%@ Register Src="common/HomeMarqeeBlock.ascx" TagName="HomeMarqeeBlock" TagPrefix="uc4" %>
<%@ Register Src="common/PublishBlock.ascx" TagName="PublishBlock" TagPrefix="uc5" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="HEAD" runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <link href="css/style.css" rel="stylesheet" type="text/css" />
    <link href="css/collapse.css" rel="stylesheet" type="text/css" />
<script src="http://code.jquery.com/jquery-1.12.3.js" type="text/javascript"></script>
<script src="http://code.jquery.com/ui/1.11.4/jquery-ui.js" type="text/javascript"></script>
<script src="css/MSClass.js" type="text/javascript"></script>
<title>計程費率試算</title>
  
<script type="text/javascript">
if ("<%=availableTagst.Value %>" != ""){
    
var browser={
    versions:function(){
        var u = navigator.userAgent, app = navigator.appVersion;
            return {       
                trident: u.indexOf('Trident') > -1, //IE?
                presto: u.indexOf('Presto') > -1, //opera?
                webKit: u.indexOf('AppleWebKit') > -1, 
                gecko: u.indexOf('Gecko') > -1 && u.indexOf('KHTML') == -1, 
                mobile: !!u.match(/AppleWebKit.*Mobile.*/), 
                ios: !!u.match(/\(i[^;]+;( U;)? CPU.+Mac OS X/), //ios
                android: u.indexOf('Android') > -1 || u.indexOf('Linux') > -1, 
                iPhone: u.indexOf('iPhone') > -1 , 
                iPad: u.indexOf('iPad') > -1  , //iPad
                webApp: u.indexOf('Safari') == -1, 
                opera: u.indexOf('Opera') > -1,
                chrome: u.indexOf('Chrome') > -1,
                firefox: u.indexOf('Opera') == -1 && u.indexOf('Firefox') > -1,
                tablet: u.indexOf('Tablet') > -1,
                msie: u.indexOf('Opera') == -1 && u.indexOf('Trident') > -1 && u.indexOf('MS IE')
                };
         }(),
         language:(navigator.browserLanguage || navigator.language).toLowerCase()
        }

if (browser.versions.android == true || browser.versions.mobile == true || browser.versions.iPhone == true) {
        location.href="<%=availableTagst.Value %>";
    }
}

    $(function () {
        var $marqueeUl = $('div#news_marquee ul'),
			$marqueeli = $marqueeUl.append($marqueeUl.html()).children(),
			_height = $('div#news_marquee').height() * -1,
			scrollSpeed = 600,
			timer,
			speed = 3000 + scrollSpeed;
        $marqueeli.hover(function () {
            clearTimeout(timer);
        }, function () {
            timer = setTimeout(showad, speed);
        });
        function showad() {
            var _now = $marqueeUl.position().top / _height;
            _now = (_now + 1) % $marqueeli.length;
            $marqueeUl.animate({
                top: _now * _height
            }, scrollSpeed, function () {
                if (_now == $marqueeli.length / 2) {
                    $marqueeUl.css('top', 0);
                }
            });
            timer = setTimeout(showad, speed);
        }
        timer = setTimeout(showad, speed);

        $('a').focus(function () {
            this.blur();
        });
    });
</script>
</head>
<body id="body"  >
    <a name="top"></a>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:HiddenField ID="availableTagst" runat="server" />
        <!-- 上方區塊開始 -->
        <uc1:TopArea ID="TopArea1" runat="server" />
        <uc4:HomeMarqeeBlock ID="HomeMarqeeBlock1" runat="server" />
<!-- 上方區塊結束 -->
<!-- content區塊開始 -->
        <div id="content">
        <!-- 左區塊開始 -->
            <div id="leftside">
                <uc3:HomePageBlock ID="HomePageBlock1" runat="server" />
                
                <uc5:PublishBlock ID="PublishBlock3" runat="server" />
                <div id="nonbox">
                    <uc5:PublishBlock ID="PublishBlock1" runat="server" />
                </div>
            </div>
        <!-- 左區塊結束 -->
        <!-- 右區塊開始 -->
        <div id="rightside">
            <uc5:PublishBlock ID="PublishBlock2" runat="server" />
            
        </div>
        <!-- 右區塊結束 -->
    </div>
<!-- content區塊結束 -->
<!-- footer區塊開始 -->
    <div id="footer">
        <uc2:footer ID="footer1" runat="server" />
    </div>
<!-- footer區塊結束 -->
</form> 
</body>
</html>
