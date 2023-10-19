function isEmpty(obj) {
    if (typeof obj == 'undefined' || obj === null || obj === '') return true;
    if (typeof obj == 'number' && isNaN(obj)) return true;
    if (obj instanceof Date && isNaN(Number(obj))) return true;
    return false;
}
function ChangeTime(isDomestic) {
    if (isDomestic == true) {
        if ($('input[value="roundtrip"]:checked').length > 0) {
            var depDay = $('.drpDepDay').val();
            var depMonth = $('.drpDepMonth').val().split('/');
            var depDate = new Date((depMonth[1]), depMonth[0] - 1, depDay);

            var arrDate = new Date(depDate);
            arrDate.setDate(arrDate.getDate() + 1);
            $('.drpArrDay').val(arrDate.getDate());
            var arrMonth = '';
            if ((arrDate.getMonth() + 1).toString().length == 1) {
                arrMonth = "0" + (arrDate.getMonth() + 1).toString();
            }
            else {
                arrMonth = arrDate.getMonth() + 1;
            }
            var arrMonthResult = arrMonth + "/" + arrDate.getFullYear();

            $('.drpArrMonth').val(arrMonthResult);
        }
    }
    else {
        if ($('input[value="qt_roundtrip"]:checked').length > 0) {
            var depQtDay = $('.drpQtDepDay').val();
            var depQtMonth = $('.drpQtDepMonth').val().split('/');
            var depQtDate = new Date((depQtMonth[1]), depQtMonth[0] - 1, depQtDay);

            var arrQtDate = new Date(depQtDate);

            arrQtDate.setDate(arrQtDate.getDate() + 1);
            $('.drpQtArrDay').val(arrQtDate.getDate());
            var arrQtMonth = '';
            if ((arrQtDate.getMonth() + 1).toString().length == 1) {
                arrQtMonth = "0" + (arrQtDate.getMonth() + 1).toString();
            }
            else {
                arrQtMonth = arrQtDate.getMonth() + 1;
            }
            var arrQtMonthResult = arrQtMonth + "/" + arrQtDate.getFullYear();

            $('.drpQtArrMonth').val(arrQtMonthResult);
        }
    }
}
function FormatDateTime(day,month, year,classDay,classMonth)
{
    var date = new Date(year, month, day);
    if (date.getDay() != day)
    {
        var month = '';
        if ((date.getMonth() + 1).toString().length == 1) {
            month = "0" + (date.getMonth() + 1).toString();
        }
        else {
            month = (date.getMonth() + 1).toString();
        }
       
         month = month + "/" + date.getFullYear();
       
        $('.' + classDay).val(date.getDate());
        $('.' + classMonth).val(month);
    }
 
}

$(document).ready(function ()
{
   
    if ($('input[value="roundtrip"]:checked').length <= 0) {
        $('input[value="oneway"]').attr("checked", "checked");
        $(".xs-return-date").hide();
    }
    if ($('input[value="qt_roundtrip"]:checked').length <= 0) {
        $('input[value="qt_oneway"]').attr("checked", "checked");
        $(".xs-qt-return-date").hide();
    }
  
    $("#clientScreenWidth").val($('body').innerWidth());
    $(".bt-search-sm").attr("disabled", true);
    $(".bt-search-sm").attr('style', 'background:#ccc');
   
});
$(window).load(function () {
    $(".bt-search-sm").attr("disabled", false);
    $(".bt-search-sm").attr('style', 'background:#0ea8e8');
});
$(function ()
{
    $(window).resize(function () {
        $("#clientScreenWidth").val($('body').innerWidth());
     
    });
    $('input[value="oneway"]').click(function () {
        $("#div-ReturnDate,.xs-return-date").hide();
    });
    $('input[value="qt_oneway"]').click(function ()
    {
        $("#qt_ReturnDate,.xs-qt-return-date").hide();
    });
    $('input[value="roundtrip"]').click(function () {
        $("#div-ReturnDate,.xs-return-date").show();
        if ($('input[value="roundtrip"]:checked').length > 0) {
            ChangeTime(true);
        }
    });
    $('input[value="qt_roundtrip"]').click(function () {
        $("#qt_ReturnDate,.xs-qt-return-date").show();
        if ($('input[value="qt_roundtrip"]:checked').length > 0) {
            ChangeTime(false);
        }
    });
    $("#div-loading").hide();

    $("#search-flights").click(function () {
        $("#div-loading").show();
    });
    $(".sl-city").find('input[type=text]').click(function (e) {
        $('.sl-city').removeClass('active');
        var parent = $(this).closest('.sl-city');
        parent.addClass('active');
        parent.find('.txtFlightCity').focus();
        e.stopPropagation();
    });
    $('body').click(function (e) {
        $('.sl-city').removeClass('active');
        $(".startdate").datepicker('hide');
        $(".enddate").datepicker('hide');

   
    });
    $(".startplace,.endplace").focus(function () {
        $(this).addClass('focus-input');
        dateType = $(this).attr('datetype');
        var deOffset = $('.startplace').offset();
        var arrOffset = $('.endplace').offset();
        var interCityInput = '';
        $(".txtFlightCity").keyup(function () {
            interCityInput = $(this).val();
        });
        var depCity = $('.startplace').val();
        var arrCity = $('.endplace').val();
        if (isEmpty(interCityInput)) {
            $(".submit").click(function () {
                var father = $(this).closest('.sl-city');
                var val = father.find(".txtFlightCity").val().trim();
                if (val == '') {
                    father.find('.error').text('Xin hãy nhập tên thành phố hoặc sân bay để tiếp tục.');
                    father.find('.txtFlightCity').attr('value', val);
                    return false;
                }
                else {
                    father.find('.error').text(' ');
                    father.find('.city').attr('value', val);
                    father.removeClass('active');
                }

            });
        }
        // get data from dialog when click
        $('.list-city a').click(function () {
            var father = $(this).closest('.sl-city');
            father.find('.city').attr('value', $(this).text());
            father.removeClass('active');
            return false;
        });
     
    });
    $(".ac_input").autocomplete("Elines/Search/AutoSuggestV2.aspx", {
        max: 50,
        highlight: false,
        matchSubset: false,
        scrollHeight: 260,
        width: 355,
        formatItem: function (item, index, total, value) {
            return value.split("{")[0];
        },
        formatResult: function (item, value) {
            return value.split("{")[1];
        }
    });
    $('.drpDepDay,.drpDepMonth').change(function () {
        var depDay = $('.drpDepDay').val();
        var depMonth = $('.drpDepMonth').val().split('/');
        FormatDateTime(depDay, depMonth[0] - 1, (depMonth[1]), 'drpDepDay', 'drpDepMonth');
        ChangeTime(true);
    });
    $('.drpQtDepDay,.drpQtDepMonth').change(function () {
        var depDay = $('.drpQtDepDay').val();
        var depMonth = $('.drpQtDepMonth').val().split('/');
        FormatDateTime(depDay, depMonth[0] - 1, (depMonth[1]), 'drpQtDepDay', 'drpQtDepMonth');
        ChangeTime(false);
    });
    $('.drpArrDay,.drpArrMonth').change(function () {
        var depDay = $('.drpArrDay').val();
        var depMonth = $('.drpArrMonth').val().split('/');
        FormatDateTime(depDay, depMonth[0] - 1, (depMonth[1]), 'drpArrDay', 'drpArrMonth');
    });
    $('.drpQtArrDay,.drpQtArrMonth').change(function () {
        var depDay = $('.drpQtArrDay').val();
        var depMonth = $('.drpQtArrMonth').val().split('/');
        FormatDateTime(depDay, depMonth[0] - 1, (depMonth[1]), 'drpQtArrDay', 'drpQtArrMonth');
    });
 


})
