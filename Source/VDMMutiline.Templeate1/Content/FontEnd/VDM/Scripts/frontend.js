function setFocusToControl(controlNextId, e) {
    var key; if (window.event)
        key = window.event.keyCode; else
        key = e.which; if (key == 13) { var btn = document.getElementById(controlNextId); if (btn != null) { btn.click(); event.keyCode = 0 } }
}
function PopupCenter(pageURL, h, w) { var left = (screen.width / 2) - (w / 2); var top = (screen.height / 2) - (h / 2); var targetWin = window.open(pageURL, '_blank', 'toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=1, resizable=1, copyhistory=no, width=' + w + ', height=' + h + ', top=' + top + ', left=' + left); }
function popup(url, _height, _width) {
    var newwindow; newwindow = window.open(url, '_blank', 'height=' + _height + ',width=' + _width + ',scrollbars=1,resizable=1'); if (window.focus) { newwindow.focus() }
    return false;
}
function popup(url, _height, _width, _top, _left) {
    var newwindow; newwindow = window.open(url, '_blank', 'height=' + _height + ',width=' + _width + ',top=' + _top + ',left=' + _left + ',scrollbars=1,resizable=1'); if (window.focus) { newwindow.focus() }
    return false;
}
function querySt(paramName) {
    var value = ''; paramName = paramName.toLowerCase(); hu = window.location.search.substring(1); hu = hu.toLowerCase(); gy = hu.split("&"); for (i = 0; i < gy.length; i++) { ft = gy[i].split("="); if (ft[0] == paramName) { value = ft[1]; } }
    return value;
}
function SelectHotel(btnSearchHotelId, message) { var elem = document.getElementById(btnSearchHotelId); elem.innerHTML = message; }
function SelectFlightTravelFusion(blockOutBound, blockInBound, message, BtnSelectId) {
    var outwardId; var returnId; for (var i = 0; i < document.getElementsByName(blockOutBound).length; i++) {
        if (document.getElementsByName(blockOutBound)[i].checked)
            outwardId = document.getElementsByName(blockOutBound)[i].value;
    }
    for (var i = 0; i < document.getElementsByName(blockInBound).length; i++) {
        if (document.getElementsByName(blockInBound)[i].checked)
            returnId = document.getElementsByName(blockInBound)[i].value;
    }
    var url = window.location.toString().toLowerCase(); var amount = document.getElementById(blockOutBound).value; if (returnId === undefined) {
        if (outwardId === undefined) { } else {
            url = url.replace("=301&", "=302&")
            url = url + "&outwardId=" + outwardId + "&amount=" + amount; window.location = url;
        }
    }
    else {
        url = url.replace("=301&", "=302&")
        url = url + "&outwardId=" + outwardId + "&returnId=" + returnId + "&amount=" + amount; window.location = url;
    }
}
function SelectFlight(blockOutBound, blockInBound, message, BtnSelectId) {
    var elem = document.getElementById(BtnSelectId); elem.setAttribute("value", message); var outBoundRef; var inBoundRef; for (var i = 0; i < document.getElementsByName(blockOutBound).length; i++) {
        if (document.getElementsByName(blockOutBound)[i].checked)
            outBoundRef = document.getElementsByName(blockOutBound)[i].value;
    }
    for (var i = 0; i < document.getElementsByName(blockInBound).length; i++) {
        if (document.getElementsByName(blockInBound)[i].checked)
            inBoundRef = document.getElementsByName(blockInBound)[i].value;
    }
    var url = window.location.toString().toLowerCase(); url = url.replace(("&orderid=" + querySt("orderid")).toLowerCase(), ''); url = url.replace(("&outbound=" + querySt("outbound")).toLowerCase(), ''); url = url.replace(("&inbound=" + querySt("inbound")).toLowerCase(), ''); if (inBoundRef === undefined)
        url = url + "&outbound=" + outBoundRef
    else
        url = url + "&outbound=" + outBoundRef + "&inbound=" + inBoundRef; url = url.replace("pageid=42&", "pageid=43&"); url = url.replace("pageid=10002&", "pageid=43&"); window.location = url;
}
function SelectFlight2(blockOutBound, blockInBound) {
    var outBoundRef; var inBoundRef; for (var i = 0; i < document.getElementsByName(blockOutBound).length; i++) {
        if (document.getElementsByName(blockOutBound)[i].checked)
            outBoundRef = document.getElementsByName(blockOutBound)[i].value;
    }
    for (var i = 0; i < document.getElementsByName(blockInBound).length; i++) {
        if (document.getElementsByName(blockInBound)[i].checked)
            inBoundRef = document.getElementsByName(blockInBound)[i].value;
    }
    var url = window.location.toString().toLowerCase(); url = url.replace(("&outbound=" + querySt("outbound")).toLowerCase(), ''); url = url.replace(("&inbound=" + querySt("inbound")).toLowerCase(), ''); url = url.replace(("&orderid=" + querySt("orderid")).toLowerCase(), ''); if (inBoundRef === undefined)
        url = url + "&OutBound=" + outBoundRef
    else
        url = url + "&OutBound=" + outBoundRef + "&InBound=" + inBoundRef; url = url.replace("=423&", "=623&")
    window.location = url;
}
function selectHotel(hotelId) { document.getElementById(hotelId).innerHTML = "Processing"; }