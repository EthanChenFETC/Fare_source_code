
    var fadeInTimer = 0, fadeOutTimer = 0;
    var baseX = 0; //document.getElementById("draggable").offsetLeft;
    var baseY = 0; // document.getElementById("draggable").offsetTop;
var tile_width = 916;
var tile_height = 550;
var map_width = 916;
var map_height = 3510;
var map_scale = 10;
    var x=0;
    var y=0;
function ShowRouteLine() {
    // 設定canvas
    var draggable_select_canvas = document.getElementById("draggable_select");
    //var draggable_canvas = document.getElementById("draggable");
    /*// IE才要處理
    if (document.all && !window.opera) {
    draggable_select_canvas = G_vmlCanvasManager.initElement(draggable_select_canvas);
    }*/
    //如果不支援html5
    //if (!draggable_canvas || !draggable_canvas.getContext) {
//    if (!draggable_canvas || !draggable_canvas.getContext) {
//        setTimeout(function() { ShowRouteLine(); }, 10);
//        return false;
    //    }
    
    if (!draggable_select_canvas || !draggable_select_canvas.getContext) {
        setTimeout(function () { ShowRouteLine(); }, 10);
        return false;
    }
    var browserName = navigator.appName;
    var navigatorAgent = navigator.userAgent;
    var nVer = navigator.appVersion;
    baseX = -document.getElementById("draggable").offsetLeft - 90 -5;
    baseY = -document.getElementById("draggable").offsetTop - 160 ;
    
    var draggable_select_ctx = draggable_select_canvas.getContext("2d");
    draggable_select_ctx.clearRect(0, 0, $("#draggable").width(), $("#draggable").height());
    var i, j, pos = 3, arr = routeline_array; 
    var lines = Number(arr[1]);
    for (i = 0; i < lines; i++) {
        var selected = false;
        if (i == Number(arr[2]))
            selected = true;
        var points = Number(arr[pos++]);
        var first_point = true;
        for (j = 0; j < points; j++) {
         
            var x2 = Number(arr[pos++]) ; //* map_scale;
            var y2 = Number(arr[pos++]) ; //* map_scale;       
            if (x2 == 0) {
                if (!first_point) {
                    if(selected){
                       draggable_select_ctx.stroke();
                    }
                }
                first_point = true;
                continue;
            }
            x2 -= baseX + x;
            y2 -= baseY + y;
            if (!first_point) {
                if (selected) {
                    //$("#draggable_select").drawLine(x1, y1, x2, y2, { color: '#ff0000', stroke: 6 });
                    draggable_select_ctx.lineTo(x2, y2);
                }
            } else {
                if (selected) {
                    draggable_select_ctx.beginPath();
                    draggable_select_ctx.strokeStyle = '#ff0000'; //$("#draggable").css.color; //
                    draggable_select_ctx.lineWidth = 6;
                    draggable_select_ctx.moveTo(x2, y2);
                }
            }
            first_point = false;
        }
        
        if (!first_point) {
            if (selected) {
                draggable_select_ctx.stroke();
                
            }
        }
    }
}
    function onFadeOut() {
        $("#draggable_select").css("visibility", "hidden");
        $("#draggable_select").css("zIndex", -1);
        fadeOutTimer = setTimeout(function () { onFadeIn(); clearTimeout(fadeOutTimer); }, 1000);
    }
    function onFadeIn() {
        fadeInTimer = setTimeout(function() { onFadeOut(); clearTimeout(fadeInTimer); }, 1000);
        $("#draggable_select").css("visibility", "visible");
        $("#draggable_select").css("zIndex", 2);
    }
 
   function MapShowConnectLine() {
    $("#draggable").css("visibility", "visible");
    $("#draggable_select").css("visibility", "visible");
    $("#draggable_select").css("zIndex", 2);

    setTimeout(function () { ShowRouteLine(); }, 20);
    //ShowRouteLine();
    ShowMap();
    clearTimeout(fadeInTimer);
    clearTimeout(fadeOutTimer);
    onFadeIn();
}
//           0   1    2    3     4      5     6    7  8
var xAxis = [0, 0, 0, 0, 0, 0, 0, 0, 0];
var yAxis = [0, 160, 838, 1633, 2254, 2658, 3094, 3670, 3710];
function ShowMap() {
    var divW = parseFloat(document.getElementById("SuggestedMap").offsetWidth);
    var divH = parseFloat(document.getElementById("SuggestedMap").offsetHeight);
    var divdW = parseFloat(document.getElementById("draggable").offsetWidth);
    var divdH = parseFloat(document.getElementById("draggable").offsetHeight);

    var divL = parseFloat(document.getElementById("SuggestedMap").offsetLeft);
    var divT = parseFloat(document.getElementById("SuggestedMap").offsetTop);
    var dleft = parseFloat(document.getElementById("draggable").offsetLeft);
    var dtop = parseFloat(document.getElementById("draggable").offsetTop);
    var xlt = 0 - dleft;
    var xrt = xlt + divW;
    var ylt = 0 - dtop ;
    var yrt = ylt;
    var xlb = xlt;
    var xrb = xrt;
    var ylb = ylt + divH;
    var yrb = ylb;
    var i = 0;
    var j = 0;
    var k = 0;
    var l = 0;
    for (i = 1; i < xAxis.length; i++) {
        if (xAxis[i] > xlt) {
            break;
        }
    }
    for (j = 1; j < xAxis.length; j++) {
        if (xAxis[j] > xrt) {
            break;
        }
    }
    for (k = 1; k < yAxis.length; k++) {
        if (yAxis[k] > ylt) {
            break;
        }
    }
    for (l = 1; l < yAxis.length; l++) {
        if (yAxis[l] > ylb) {
            break;
        }
    } 

    var idl = "";
    var m = i;
   
    for (m = i ; m < j+1; m++) {
        if (m < 1) m = 1;
        for (n = k-1; n < l+1; n++) {
            if (n < 1) n = 1;
            var mapid = (n -1) * 1 + m;
            if (mapid == 0) mapid = 1;
            var mapname = mapid.toString();
            if (mapid > 20) mapid = mapid - 1;
            if (mapid > 10) mapid = mapid - 1;
            if (mapid > 6) mapid = 6;
            if (mapid < 7) mapname = "0" + mapid.toString();
            else mapname = mapid.toString();
            document.getElementById("img" + mapid).src = "images/map/emap" + mapname + ".gif";
            idl += "," + n.toString() + "-" + m.toString();
        }
    }
}