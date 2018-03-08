$(function () {
    $("#draggable").draggable();
    $("#Canvas1").draggable();
    //$("#draggable_select").draggable();
    $("#apDiv9").draggable({ containment: "#apDiv8" });
});


function MM_showHideLayers() { //v9.0
    var i, p, v, obj, args = MM_showHideLayers.arguments;
    for (i = 0; i < (args.length - 3) ; i += 4)
        with (document) if (args[i] != '' && getElementById && ((obj = getElementById(args[i + 1])) != null)) {
            v = args[i + 3];
            if (obj.style) { obj = obj.style; v = (v == 'show') ? 'visible' : (v == 'hide') ? 'hidden' : v; }
            obj.visibility = v;
            if (v == 'visible') { obj.zIndex = 2; } else { obj.zIndex = -1; }
            if (args[i + 1] == 'apDiv3' && document.getElementById('apDiv3').style.visibility == 'visible') {
                if (navigator.appName.indexOf("Explorer") > -1) {
                    document.getElementById("ICName").innerText = args[i + 2];
                } else {
                    document.getElementById("ICName").textContent = args[i + 2];
                }
                obj.zIndex = 3;
                var divW = document.getElementById("SuggestedMap").offsetWidth;
                var divH = document.getElementById("SuggestedMap").offsetHeight;
                var divW3 = document.getElementById("apDiv3").offsetWidth;
                var divH3 = document.getElementById("apDiv3").offsetHeight;
                document.getElementById("apDiv3").style.left = (axisX[args[i]] - (divW3 / 2) + 90+6).toString() + "px";
                document.getElementById("apDiv3").style.top = (axisY[args[i]] - 173 - 67 + 24 + 160+1).toString() + "px";

                document.getElementById("draggable").style.left = (0 - axisX[args[i]] + (divW / 2)).toString() + "px";
                document.getElementById("draggable").style.top = (0 - axisY[args[i]] + (divH / 2) - 50 - 160).toString() + "px";
                mousep();
            } else if (args[i + 1] == 'apDiv2' && document.getElementById('apDiv2').style.visibility == 'visible') {

                document.getElementById("apDiv2").style.left = (axisX[args[i]] - 9 + 90).toString() + "px";
                document.getElementById("apDiv2").style.top = (axisY[args[i]] - 11 + 160).toString() + "px";
                hidIC = args[i];
                hidName = args[i + 2];

            }
        }
    SycapDiv();
}
function apDiv2Click(t) {

    if (t == 2) {
        MM_showHideLayers(hidIC, 'apDiv2', hidName, 'show')
    } else if (t == 3) {
        MM_showHideLayers(hidIC, 'apDiv2', hidName, 'hide')
    } else {
        MM_showHideLayers(hidIC, 'apDiv3', hidName, 'show');
    }
}

function mousedragend() {
    if (document.getElementById("draggable").offsetLeft < -200) document.getElementById("draggable").style.left = "-200px";
    if (document.getElementById("draggable").offsetLeft > 0) document.getElementById("draggable").style.left = "0px";
    if (document.getElementById("draggable").offsetTop > 0) document.getElementById("draggable").style.top = "0px";
    if (document.getElementById("draggable").offsetTop < document.getElementById("SuggestedMap").offsetHeight - document.getElementById("draggable").offsetHeight + 2)
        document.getElementById("draggable").style.top = (document.getElementById("SuggestedMap").offsetHeight - document.getElementById("draggable").offsetHeight + 2).toString() + "px";
    MapShowConnectLine();
    debug();

}
function mousep() {
    SycapDiv();
    document.getElementById("apDiv8").style.left = (document.getElementById("SuggestedMap").offsetLeft + document.getElementById("SuggestedMap").offsetWidth - document.getElementById("apDiv8").offsetWidth).toString() + "px";
    document.getElementById("apDiv8").style.top = (document.getElementById("SuggestedMap").offsetTop) + "px";

    if (document.getElementById("draggable").offsetLeft < -200) document.getElementById("draggable").style.left = "-200px";
    if (document.getElementById("draggable").offsetLeft > 0) document.getElementById("draggable").style.left = "0px";
    if (document.getElementById("draggable").offsetTop > 0) document.getElementById("draggable").style.top = "0px";
    if (document.getElementById("draggable").offsetTop < document.getElementById("SuggestedMap").offsetHeight - document.getElementById("draggable").offsetHeight + 2)
        document.getElementById("draggable").style.top = (document.getElementById("SuggestedMap").offsetHeight - document.getElementById("draggable").offsetHeight + 2).toString() + "px";
    document.getElementById("apDiv9").style.left = parseFloat(document.getElementById("apDiv8").offsetLeft + 5 - document.getElementById("draggable").offsetLeft * (document.getElementById("apDiv8").offsetWidth - 10) / document.getElementById("draggable").offsetWidth).toString() + "px";
    document.getElementById("apDiv9").style.top = parseFloat(document.getElementById("apDiv8").offsetTop + 5 - document.getElementById("draggable").offsetTop * (document.getElementById("apDiv8").offsetHeight - 10) / document.getElementById("draggable").offsetHeight-5).toString() + "px";

    mousedragend();
} 
function mouses() {
    SycapDiv();
    document.getElementById("apDiv8").style.left = (document.getElementById("SuggestedMap").offsetLeft + document.getElementById("SuggestedMap").offsetWidth - document.getElementById("apDiv8").offsetWidth).toString() + "px";
    document.getElementById("apDiv8").style.top = (document.getElementById("SuggestedMap").offsetTop) + "px";
    if (document.getElementById("apDiv9").offsetLeft < document.getElementById("apDiv8").offsetLeft + 5) document.getElementById("apDiv9").style.left = parseFloat(document.getElementById("apDiv8").offsetLeft + 5).toString() + "px";
    if (document.getElementById("apDiv9").offsetTop > document.getElementById("apDiv8").offsetTop + 5 + document.getElementById("apDiv8").offsetHeight - 10 - document.getElementById("apDiv9").offsetHeight)
        document.getElementById("apDiv9").style.Top = (document.getElementById("apDiv8").offsetTop + 5 + document.getElementById("apDiv8").offsetHeight - 10 - document.getElementById("apDiv9").offsetHeight).toString() + "px";
    document.getElementById("draggable").style.left = parseFloat(-(document.getElementById("apDiv9").offsetLeft - document.getElementById("apDiv8").offsetLeft - 5) * (document.getElementById("draggable").offsetWidth) / (document.getElementById("apDiv8").offsetWidth - 10)).toString() + "px";
    document.getElementById("draggable").style.top = parseFloat(-(document.getElementById("apDiv9").offsetTop - document.getElementById("apDiv8").offsetTop - 5) * (document.getElementById("draggable").offsetHeight+15) / (document.getElementById("apDiv8").offsetHeight - 17)).toString() + "px";
    MapShowConnectLine();
    debug();
}
function mouseinit() {
    document.getElementById("apDiv8").style.left = (document.getElementById("SuggestedMap").offsetLeft + document.getElementById("SuggestedMap").offsetWidth - document.getElementById("apDiv8").offsetWidth).toString() + "px";
    document.getElementById("apDiv8").style.top = (document.getElementById("SuggestedMap").offsetTop) + "px";
    document.getElementById("MapBtn").style.left = (document.getElementById("SuggestedMap").offsetLeft + document.getElementById("SuggestedMap").offsetWidth / 2 - document.getElementById("MapBtn").offsetWidth / 2).toString() + "px";
    document.getElementById("MapBtn").style.top = (document.getElementById("SuggestedMap").offsetTop + document.getElementById("SuggestedMap").offsetHeight - 35).toString() + "px";
    document.getElementById("draggable").style.left = "0px";
    document.getElementById("draggable").style.top = "-160px";
    document.getElementById("apDiv9").style.left = parseFloat(document.getElementById("apDiv8").offsetLeft + 5 - document.getElementById("draggable").offsetLeft * (document.getElementById("apDiv8").offsetWidth - 10) / document.getElementById("draggable").offsetWidth).toString() + "px";
    document.getElementById("apDiv9").style.top = parseFloat(document.getElementById("apDiv8").offsetTop + 5 - document.getElementById("draggable").offsetTop * (document.getElementById("apDiv8").offsetHeight-17) / document.getElementById("draggable").offsetHeight).toString() + "px";
    ShowMap();
    debug();
}
function movemap(t) {
    var moveleft = 0;
    var movedown = 0;
    var upmore = 550;
    var leftmore = 90;
    if (t == 0) {
        mouseinit();
        MapShowConnectLine();
        return;
    } else if (t == 1) {
        movedown = upmore;
    } else if (t == 2) {
        movedown = -upmore;
    } else if (t == 3) {
        moveleft = leftmore;
    } else if (t == 4) {
        moveleft = -leftmore;
    }
    document.getElementById("draggable").style.left = parseInt(document.getElementById("draggable").offsetLeft + moveleft) + "px";
    document.getElementById("draggable").style.top = parseInt(document.getElementById("draggable").offsetTop + movedown) + "px";
    mousep();
    mousedragend();
}
function mapmoveto(t) { //v9.0
    var i, p, v, obj;
    var divW = document.getElementById("SuggestedMap").offsetWidth-4;
    var divH = document.getElementById("SuggestedMap").offsetHeight-4;
    document.getElementById("draggable").style.left = (0 - axisX[t] + (divW / 2)).toString() + "px";
    document.getElementById("draggable").style.top = (0 - axisY[t] + (divH / 2) ).toString() + "px";
    SycapDiv();
    //if (document.getElementById("draggable").offsetLeft != 0) document.getElementById("draggable").style.left = "0px";
    //if (document.getElementById("draggable").offsetTop > 0) document.getElementById("draggable").style.top = "0px";
    //if (document.getElementById("draggable").offsetTop < document.getElementById("SuggestedMap").offsetHeight - document.getElementById("draggable").offsetHeight + 2)
    //    document.getElementById("draggable").style.top = (document.getElementById("SuggestedMap").offsetHeight - document.getElementById("draggable").offsetHeight + 2) + "px";
    setTimeout(function () { mousep(); }, 10);
}

mouseinit();
function debug() {
    SycapDiv();
    //document.getElementById("SuggestedMap").offsetLeft = document.getElementById("SuggestedMap").offsetLeft 
    document.getElementById("debuginfo").style.display = "none"
    document.getElementById("debuginfo").innerHTML = "SuggestedMapTop:" + document.getElementById("SuggestedMap").style.top + "  Left:" + document.getElementById("SuggestedMap").style.left + " offsetTop:" + document.getElementById("SuggestedMap").offsetTop + "  offsetLeft:" + document.getElementById("SuggestedMap").offsetLeft + " offsetHeight:" + document.getElementById("SuggestedMap").offsetHeight + "  offsetWidth:" + document.getElementById("SuggestedMap").offsetWidth + "<br>"
    document.getElementById("debuginfo").innerHTML += "dragableTop:" + document.getElementById("draggable").style.top + "  Left:" + document.getElementById("draggable").style.left + " offsetTop:" + document.getElementById("draggable").offsetTop + "  offsetLeft:" + document.getElementById("draggable").offsetLeft + " offsetHeight:" + document.getElementById("draggable").offsetHeight + "  offsetWidth:" + document.getElementById("draggable").offsetWidth + "<br>"
    document.getElementById("debuginfo").innerHTML += "apDiv8Top:" + document.getElementById("apDiv8").style.top + "  Left:" + document.getElementById("apDiv8").style.left + " offsetTop:" + document.getElementById("apDiv8").offsetTop + "  offsetLeft:" + document.getElementById("apDiv8").offsetLeft + " offsetHeight:" + document.getElementById("apDiv8").offsetHeight + "  offsetWidth:" + document.getElementById("apDiv8").offsetWidth + "<br>"
    document.getElementById("debuginfo").innerHTML += "apDiv9Top:" + document.getElementById("apDiv9").style.top + "  Left:" + document.getElementById("apDiv9").style.left + " offsetTop:" + document.getElementById("apDiv9").offsetTop + "  offsetLeft:" + document.getElementById("apDiv9").offsetLeft + " offsetHeight:" + document.getElementById("apDiv9").offsetHeight + "  offsetWidth:" + document.getElementById("apDiv9").offsetWidth + "<br>"
    document.getElementById("debuginfo").innerHTML += "apDiv5Top:" + document.getElementById("apDiv5").style.top + "  Left:" + document.getElementById("apDiv5").style.left + " offsetTop:" + document.getElementById("apDiv5").offsetTop + "  offsetLeft:" + document.getElementById("apDiv5").offsetLeft + " offsetHeight:" + document.getElementById("apDiv5").offsetHeight + "  offsetWidth:" + document.getElementById("apDiv5").offsetWidth + "<br>"
    document.getElementById("debuginfo").innerHTML += "sUM:" + (document.getElementById("apDiv9").offsetTop - document.getElementById("apDiv8").offsetTop - 5) * (document.getElementById("draggable").offsetHeight) / (document.getElementById("apDiv8").offsetHeight - 10) + "<br>"
    document.getElementById("debuginfo").innerHTML += "sUM:" + (document.getElementById("apDiv8").offsetTop + document.getElementById("apDiv8").offsetHeight - 10 - document.getElementById("apDiv9").offsetHeight - 5) + "<br>" + (document.getElementById("apDiv8").offsetTop + document.getElementById("apDiv8").offsetHeight - 5 - document.getElementById("apDiv9").offsetHeight)
}
