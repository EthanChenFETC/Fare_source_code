function Marquee(){if(this.ID=document.getElementById(arguments[0])){this.Direction=this.Width=this.Height=this.DelayTime=this.WaitTime=this.CTL=this.StartID=this.Stop=this.MouseOver=0;this.Step=1;this.Timer=30;this.DirectionArray={top:0,up:0,bottom:1,down:1,left:2,right:3};if(typeof arguments[1]=="number"||typeof arguments[1]=="string")this.Direction=arguments[1];if(typeof arguments[2]=="number")this.Step=arguments[2];if(typeof arguments[3]=="number")this.Width=arguments[3];if(typeof arguments[4]==
"number")this.Height=arguments[4];if(typeof arguments[5]=="number")this.Timer=arguments[5];if(typeof arguments[6]=="number")this.DelayTime=arguments[6];if(typeof arguments[7]=="number")this.WaitTime=arguments[7];if(typeof arguments[8]=="number")this.ScrollStep=arguments[8];this.ID.style.overflow=this.ID.style.overflowX=this.ID.style.overflowY="hidden";this.ID.noWrap=true;this.IsNotOpera=navigator.userAgent.toLowerCase().indexOf("opera")==-1;arguments.length>=7&&this.Start()}else{alert('\u60a8\u8981\u8a2d\u7f6e\u7684"'+
arguments[0]+'"\u521d\u59cb\u5316\u932f\u8aa4\r\n\u8acb\u6aa2\u67e5\u6a19\u7c3dID\u8a2d\u7f6e\u662f\u5426\u6b63\u78ba!');this.ID=-1}}
Marquee.prototype.Start=function(){if(this.ID!=-1){if(this.WaitTime<800)this.WaitTime=800;if(this.Timer<20)this.Timer=20;if(this.Width==0)this.Width=parseInt(this.ID.style.width);if(this.Height==0)this.Height=parseInt(this.ID.style.height);if(typeof this.Direction=="string")this.Direction=this.DirectionArray[this.Direction.toString().toLowerCase()];this.HalfWidth=Math.round(this.Width/2);this.HalfHeight=Math.round(this.Height/2);this.BakStep=this.Step;this.ID.style.width=this.Width+"px";this.ID.style.height=
this.Height+"px";if(typeof this.ScrollStep!="number")this.ScrollStep=this.Direction>1?this.Width:this.Height;var a=this;a.tempHTML=a.ID.innerHTML;if(a.Direction<=1)a.ID.innerHTML="<table cellspacing='0' cellpadding='0' style='border-collapse:collapse;'><tr><td>MSCLASS_TEMP_HTML</td></tr><tr><td>MSCLASS_TEMP_HTML</td></tr></table>".replace(/MSCLASS_TEMP_HTML/g,a.ID.innerHTML);else if(a.ScrollStep==0&&a.DelayTime==0)a.ID.innerHTML+=a.ID.innerHTML;else a.ID.innerHTML="<table cellspacing='0' cellpadding='0' style='border-collapse:collapse;display:inline;'><tr><td noWrap=true style='white-space: nowrap;word-break:keep-all;'>MSCLASS_TEMP_HTML</td><td noWrap=true style='white-space: nowrap;word-break:keep-all;'>MSCLASS_TEMP_HTML</td></tr></table>".replace(/MSCLASS_TEMP_HTML/g,
a.ID.innerHTML);var c=this.Timer,d=this.DelayTime,e=this.WaitTime;a.StartID=function(){a.Scroll()};a.Continue=function(){if(a.MouseOver==1)setTimeout(a.Continue,d);else{clearInterval(a.TimerID);a.CTL=a.Stop=0;a.TimerID=setInterval(a.StartID,c)}};a.Pause=function(){a.Stop=1;clearInterval(a.TimerID);setTimeout(a.Continue,d)};a.Begin=function(){a.ClientScroll=a.Direction>1?a.ID.scrollWidth/2:a.ID.scrollHeight/2;if(a.Direction<=1&&a.ClientScroll<=a.Height+a.Step||a.Direction>1&&a.ClientScroll<=a.Width+
a.Step){a.ID.innerHTML=a.tempHTML;delete a.tempHTML}else{delete a.tempHTML;a.TimerID=setInterval(a.StartID,c);if(!(a.ScrollStep<0)){a.ID.onmousemove=function(b){if(a.ScrollStep==0&&a.Direction>1){b=b||window.event;if(window.event)if(a.IsNotOpera)a.EventLeft=b.srcElement.id==a.ID.id?b.offsetX-a.ID.scrollLeft:b.srcElement.offsetLeft-a.ID.scrollLeft+b.offsetX;else{a.ScrollStep=null;return}else a.EventLeft=b.layerX-a.ID.scrollLeft;a.Direction=a.EventLeft>a.HalfWidth?3:2;a.AbsCenter=Math.abs(a.HalfWidth-
a.EventLeft);a.Step=Math.round(a.AbsCenter*a.BakStep*2/a.HalfWidth)}};a.ID.onmouseover=function(){if(a.ScrollStep!=0){a.MouseOver=1;clearInterval(a.TimerID)}};a.ID.onmouseout=function(){if(a.ScrollStep==0){if(a.Step==0)a.Step=1}else{a.MouseOver=0;if(a.Stop==0){clearInterval(a.TimerID);a.TimerID=setInterval(a.StartID,c)}}}}}};setTimeout(a.Begin,e)}};
Marquee.prototype.Scroll=function(){switch(this.Direction){case 0:this.CTL+=this.Step;if(this.CTL>=this.ScrollStep&&this.DelayTime>0){this.ID.scrollTop+=this.ScrollStep+this.Step-this.CTL;this.Pause();return}else{if(this.ID.scrollTop>=this.ClientScroll)this.ID.scrollTop-=this.ClientScroll;this.ID.scrollTop+=this.Step}break;case 1:this.CTL+=this.Step;if(this.CTL>=this.ScrollStep&&this.DelayTime>0){this.ID.scrollTop-=this.ScrollStep+this.Step-this.CTL;this.Pause();return}else{if(this.ID.scrollTop<=
0)this.ID.scrollTop+=this.ClientScroll;this.ID.scrollTop-=this.Step}break;case 2:this.CTL+=this.Step;if(this.CTL>=this.ScrollStep&&this.DelayTime>0){this.ID.scrollLeft+=this.ScrollStep+this.Step-this.CTL;this.Pause();return}else{if(this.ID.scrollLeft>=this.ClientScroll)this.ID.scrollLeft-=this.ClientScroll;this.ID.scrollLeft+=this.Step}break;case 3:this.CTL+=this.Step;if(this.CTL>=this.ScrollStep&&this.DelayTime>0){this.ID.scrollLeft-=this.ScrollStep+this.Step-this.CTL;this.Pause();return}else{if(this.ID.scrollLeft<=
0) this.ID.scrollLeft += this.ClientScroll; this.ID.scrollLeft -= this.Step
} break
}
}; function MarqueeStop(a) { if (this.ID = document.getElementById(a)) { a = this.DelayTime; var c = this.WaitTime; this.Stop = 1; clearInterval(this.TimerID); setTimeout(this.Continue, a); setTimeout(this.Begin, c) } else { alert('\u60a8\u8981\u8a2d\u7f6e\u7684"' + a + '"\u521d\u59cb\u5316\u932f\u8aa4\r\n\u8acb\u6aa2\u67e5\u6a19\u7c3dID\u8a2d\u7f6e\u662f\u5426\u6b63\u78ba!'); this.ID = -1 } };


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