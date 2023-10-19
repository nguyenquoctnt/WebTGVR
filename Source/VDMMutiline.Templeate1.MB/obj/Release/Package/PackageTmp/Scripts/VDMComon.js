function CKupdate() {
   
}

var _lwfunc = {};

_lwfunc.toTitleCase = function (s) {
    return s.replace(/([^\s:\-])([^\s:\-]*)/g, function ($0, $1, $2) {
        return $1.toUpperCase() + $2.toLowerCase();
    });
}
_lwfunc.SetTitleCase = function (item) {
    window.$(item).val(_lwfunc.toTitleCase(window.$(item).val()));
}
_lwfunc.replaceAll = function (find, replace) {
    var str = this;
    return str.replace(new RegExp(find.replace(/[-\/\\^$*+?.()|[\]{}]/g, '\\$&'), 'g'), replace);
};
_lwfunc.formatDateTime = function (data, type, row) {
    return moment(data).format('DD/MM/YYYY HH:mm:ss');
}

_lwfunc.formatDate = function (data, type, row) {
    return window.moment(data).format('DD/MM/YYYY');
}
_lwfunc.formatNumber = function (data, type, row) {
    return data.toString().replace(
        /\B(?=(\d{3})+(?!\d))/g, ","
    );
}

_lwfunc.showLoading = function (isShow) {
    if (isShow)
        jQuery("#loadingpanel").show();
    else
        jQuery("#loadingpanel").hide();
}

jQuery(document).ajaxStart(function () {
    _lwfunc.showLoading(true);
});

jQuery(document).ajaxComplete(function () {
    _lwfunc.showLoading(false);
});
_lwfunc.AjaxNoprocessData = function (url, data, callBackFunction) {
    showpopub();
   
    jQuery.ajax({
        url: url,
        data: data,
        type: "POST",
        cache: false,
        contentType: false,
        processData: false,
        success: function (data) {
            callBackFunction(data);
            closepopub();
            ion.sound.play("metal_plate_2");
            //toastr.success(data.mess, "Thông báo thành công!");
        },
        error: function (jqXhr, errorCode, error) {
            closepopub();
            toastr.error("Lỗi không xác định " + jqXhr.status + " " + jqXhr.statusText,"Thông báo!");
           
        }
    });
    
}
_lwfunc.Ajax = function (url, data, callBackFunction) {
    showpopub();
    CKupdate();
    jQuery.ajax({
        url: url,
        data: data,
        type: "POST",
        success: function (data) {
            callBackFunction(data);
            closepopub();
            ion.sound.play("metal_plate_2");
           // toastr.success(data.mess, "Thông báo thành công!");
        },
        error: function (jqXhr, errorCode, error) {
            closepopub();
            toastr.error("Lỗi không xác định " + jqXhr.status + " " + jqXhr.statusText, "Thông báo!");
        }
    });
}
_lwfunc.Ajax = function (url, data, callBackFunction) {
    showpopub();
    CKupdate();
    jQuery.ajax({
        url: url,
        data: data,
        type: "POST",
        success: function (data) {
            callBackFunction(data);
            closepopub();
        },
        error: function (jqXhr, errorCode, error) {
            closepopub();
            toastr.error("Lỗi không xác định " + jqXhr.status + " " + jqXhr.statusText, "Thông báo!");
        }
    });
}


_lwfunc.AjaxGet = function (url, callBackFunction) {
    showpopub();
    CKupdate();
    jQuery.ajax({
        url: url,
        type: "GET",
        success: function (data) {
            callBackFunction(data);
            _lwfunc.bindingControl();
            closepopub();
        },
        error: function (jqXhr, errorCode, error) {
            closepopub();
            toastr.error("Lỗi không xác định " + jqXhr.status + " " + jqXhr.statusText, "Thông báo!");
        }
    });
}

_lwfunc.SyncAjaxGet = function (url, callBackFunction) {
    showpopub();
    CKupdate();
    jQuery.ajax({
        async: false,
        url: url,
        type: "GET",
        success: function (data) {
            callBackFunction(data);
            closepopub();
            ion.sound.play("metal_plate_2");
            //toastr.success(data.mess, "Thông báo thành công!");
        },
        error: function (jqXhr, errorCode, error) {
            closepopub();
            toastr.error("Lỗi không xác định " + jqXhr.status + " " + jqXhr.statusText, "Thông báo!");
        }
    });
}

_lwfunc.SyncAjaxPost = function (url, data, callBackFunction) {
    jQuery.ajax({
        async: false,
        url: url,
        data: data,
        type: "POST",
        success: function (data) {
            callBackFunction(data);
            ion.sound.play("metal_plate_2");
            //toastr.success(data.mess, "Thông báo thành công!");
        },
        error: function (jqXhr, errorCode, error) {
            toastr.error("Lỗi không xác định " + jqXhr.status + " " + jqXhr.statusText, "Thông báo!");
        }
    });
}
_lwfunc.Shownotification = function (conten) {
    $.gritter.add({
        title: 'Thông báo thành công!',
        text: conten,
        sticky: false,
        time: '',
        class_name: 'gritter-success'
    });
}
_lwfunc.notificationSuccess = function (conten) {
    $.gritter.add({
        title: 'Thông báo thành công!',
        text: conten,
        sticky: true,
        time: '',
        class_name: 'gritter-success'
    });

}
_lwfunc.notificationError = function (conten) {
    ion.sound.play("metal_plate_2");
    $.gritter.add({
        title: 'Thông báo lỗi!',
        text: conten,
        sticky: true,
        time: 'gritter-error',
        class_name: ''
    });

}
_lwfunc.notificationWarning = function (conten) {
    $.gritter.add({
        title: 'Cảnh báo!',
        text: conten,
        sticky: true,
        time: 'gritter-warning',
        class_name: ''
    });

}
var datas;
_lwfunc.datajax = (function addRequestVerificationToken(data) {
    data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
    datas = data;
    return data;
});

_lwfunc.AjaxLoadDrop = function (url, datas, control) {
    var target = $(control);
    jQuery.ajax({
        url: url,
        data: datas,
        type: "POST",
        success: function (data) {
            target.empty();
            target.append("<option value=''>[Chọn]</option>");
            for (var i = 0; i < data.listvalue.length; i++) {
                var item = data.listvalue[i];
                target.append("<option value='" + item.Value + "'>" + item.Text + "</option>");
            }
        },
        error: function (jqXhr, errorCode, error) {
            closepopub();
            toastr.error("Lỗi không xác định " + jqXHR.status + " " + jqXHR.statusText, "Thông báo!");
        }
    });
}

_lwfunc.UpdateMessage = function (urlGetNewMessage, urlOpenMessage, imageUrl) {
    _lwfunc.AjaxGet(urlGetNewMessage + "/?token=" + new Date().getTime(), function (data) {
        var list = $('#message-ul');
        list.empty();
        if (data.total > 0) {
            jQuery("#header_inbox_bar .message-count").text(data.total);
        }

        $(data.entities).each(function () {
            var liElement = $(document.createElement('li'));
            liElement.append($(document.createElement('a')).html("<img src='" + this.Sender.Avatar + "' class='msg-photo' alt='" + this.TenNguoigui + "'>" +
                "<span class='msg-body'><span class='msg-title'><span class='blue'>" + this.Sender.UserName + ":</span></span>" + this.ContentChat +
                "<span class='msg-time'><i class='ace-icon fa fa-clock-o'></i><span class='local-datetime' title='" + moment(this.NgayGui).format('DD/MM/YYYY HH:mm:ss') + "' data-utc='" + this.Ticks +"'>" + moment(this.NgayGui).format('DD/MM/YYYY HH:mm:ss') +"</span></span></span>")
                .attr("class", "clearfix")
            );
            liElement.appendTo(list);
        });
    });
}

_lwfunc.bindingControl = function () {
    SetstyleIcheck();
    //$('.date-picker').datetimepicker({
    //    weekStart: 1,
    //    todayBtn: 1,
    //    autoclose: 1,
    //    todayHighlight: 1,
    //    startView: 2,
    //    minView: 2,
    //    forceParse: 0
    //});
  
    $(".uppercase").on("blur", function (index) {
        var reslut = $(this).val();
        $(this).val(reslut.toUpperCase());
    });
    $(".lowercase").on("blur", function (index) {
        var reslut = $(this).val();
        $(this).val(reslut.toLowerCase());
    });
    $(".titlecase").on("blur", function (index) {
        var reslut = $(this).val();
        $(this).val(_lwfunc.toTitleCase(reslut));
    });
    $(".local-datetime").each(function () {

        var $this = $(this), utcDate = parseInt($this.attr('data-utc'), 10) || 0;

        if (!utcDate) {

            return;

        }
        var local = moment.utc(utcDate).local();

        var formattedDate = "";

        var days = moment().diff(local, "days");

        if (days > 2) {

            formattedDate = local.locale("en").format("DD/MM/YYYY HH:mm");

        }

        else {

            formattedDate = local.locale("vi").fromNow();

        }
        $this.text(formattedDate);

    });
}

function showpopub() {
    $('#myModalloading').bPopup({
        modalClose: false,
        opacity: 0.6,
        positionStyle: 'fixed'
    });

}
function closepopub() {
    $('#myModalloading').bPopup().close();

}
$(document).ready(function () {
    _lwfunc.bindingControl();
});

function SetstyleIcheck() {
   
}

