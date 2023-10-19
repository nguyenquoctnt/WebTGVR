function getParameterByName(name) {
    name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
    var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
        results = regex.exec(location.search);
    return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
}

function commaSeparateNumber(val) {
    while (/(\d+)(\d{3})/.test(val.toString())) {
        val = val.toString().replace(/(\d+)(\d{3})/, '$1' + ',' + '$2');
    }
    return val;
}

function GetDayLeft(date)
{
    var weekdays = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"];
    var weekday = weekdays[date.getDay()];
    var dayLeft = 0;
    switch (weekday)
    {
        case "Monday":
            dayLeft = 1;
            break;
        case "Tuesday":
            dayLeft = 2;
            break;
        case "Wednesday":
            dayLeft = 3;
            break;
        case "Thursday":
            dayLeft = 4;
            break;
        case "Friday":
            dayLeft = 5;
            break;
        case "Saturday":
            dayLeft = 6;
            break;
        case "Sunday":
            dayLeft = 0;
            break;
    }
    return dayLeft;
}

var sid = getParameterByName('sessionId');
var check = $('#ipRoundTrip').val();
if (check == 'False') {
    $('.flightResultInBound').hide();
}
else {
    $('.flightResultInBound').show();
}
if (sid != '')
{
    $('.loading').removeClass('hidden').addClass('show');
    $('.notice').text("* Giá vé chưa gồm thuế phí.");
    $('.select-flight-outer,.flightResultOutBound,.search_form').css('display', 'block');
    $('.flight-month-search').css('display', 'none');

    var bl = $.ajax({
        type: "POST",
        url: "VeGiaReTheoThang.aspx/SearchJetStarByMonth",
        data: '{sessionId: "' + sid + '",tripType:"1" }',
        contentType: "application/json; charset=utf-8",
        cache: false,
        dataType: "json",
        error: function () { }
    });
    bl.done(function (data) {
        var rows = data.d;
        var depDateMonth = rows.depDate.split('/')[1];
        var date = new Date();
        var lastDay = new Date(date.getFullYear(), depDateMonth, 0);
        var dayInPrevMonth = new Date(date.getFullYear(), (parseInt(depDateMonth) - 1),1);
        var dayLeft = GetDayLeft(dayInPrevMonth);
        var i = 30;
        $.each(rows.ListCheapestflightByMonthOutBound, function (idex, item)
        {
                var result = '';
                var day = item.startDate.split('/')[0];
                var month = item.startDate.split('/')[1];
                var dateArr = item.startDate.split('/');
                var strDate = dateArr[0] + "/" + dateArr[1];
                var city = item.depCity + " - " + item.arrCity;
                if (day <= lastDay.getDate() && month == (lastDay.getMonth() + 1)) {
                    var td = $('.calendar-outbound').find('.d_price[cell-number="' + (parseInt(day) + parseInt(dayLeft)) + '"]');
                    td.removeClass('disable');
                    td.find('.BL').append(commaSeparateNumber(item.price) + " VND").removeClass('disable');
                    td.find('.ngay').text(strDate);
                    td.find('.hanh_trinh').text(city);
                    td.attr('data-date', item.startDate);
                    var ip = td.find('input[type="radio"]');
                    ip.attr('data-date', item.startDate);
                    ip.attr('adult', rows.adult);
                    ip.attr('child', rows.child);
                    ip.attr('infant', rows.infant);

                }
                else if (day <= lastDay.getDate() && month > (lastDay.getMonth() + 1)) {
                    i++;
                    if (i <= 35) {
                        var tdDay = $('.calendar-outbound').find('.d_price[cell-number="' + (parseInt(day) + parseInt(lastDay.getDate() + parseInt(dayLeft))) + '"]');
                        tdDay.find('.ngay').text(strDate);
                        tdDay.removeClass('disable');
                        tdDay.find('.BL').append(commaSeparateNumber(item.price) + " VND").removeClass('disable');
                        tdDay.find('.ngay').text(strDate);
                        tdDay.find('.hanh_trinh').text(city);
                        tdDay.attr('data-date', item.startDate);
                        var ip = tdDay.find('input[type="radio"]');

                        ip.attr('data-date', item.startDate);
                        ip.attr('adult', rows.adult);
                        ip.attr('child', rows.child);
                        ip.attr('infant', rows.infant);
                    }
                }
        })
  
        if (check == 'True')
        {
           
            var retDateMonth = rows.arrDate.split('/')[1];
            var retlastDay = new Date(date.getFullYear(), retDateMonth, 0);
            var dayInPrevRetMonth = new Date(date.getFullYear(), (parseInt(retDateMonth) - 1), 1);
            var retDayLeft = GetDayLeft(dayInPrevRetMonth);
            var j = 30;
            $.each(rows.ListCheapestflightByMonthInBound, function (idex, item) {
              
                    var result = '';
                    var day = item.startDate.split('/')[0];
                    var month = item.startDate.split('/')[1];
                    var td = $('.calendar-inbound').find('.d_price[cell-number="' + (parseInt(day) + parseInt(retDayLeft)) + '"]');
                    var dateArr = item.startDate.split('/');
                    var strDate = dateArr[0] + "/" + dateArr[1];
                    var city = item.depCity + " - " + item.arrCity;
                    if (day <= retlastDay.getDate() && month == (retlastDay.getMonth() + 1)) {
                        td.removeClass('disable');
                        td.find('.ngay').text(strDate);
                        td.find('.BL').append(commaSeparateNumber(item.price) + " VND").removeClass('disable');
                        td.find('.hanh_trinh').text(city);
                        td.attr('data-date', item.startDate);
                        var ip = td.find('.rdoPlane');
                        ip.attr('data-date', item.startDate);
                        ip.attr('adult', rows.adult);
                        ip.attr('child', rows.child);
                        ip.attr('infant', rows.infant);
                    }
                    else if (day <= retlastDay.getDate() && month > (retlastDay.getMonth() + 1)) {
                        j++;
                        
                        if (j <= 35) {
                            var tdDay = $('.calendar-inbound').find('.d_price[cell-number="' + (parseInt(day) + parseInt(retlastDay.getDate()) + parseInt(retDayLeft)) + '"]');
                            tdDay.find('.ngay').text(strDate);
                            tdDay.removeClass('disable');
                            tdDay.find('.BL').append(commaSeparateNumber(item.price) + " VND").removeClass('disable');
                            tdDay.find('.ngay').text(strDate);
                            tdDay.find('.hanh_trinh').text(city);
                            tdDay.attr('data-date', item.startDate);
                            var ip = tdDay.find('.rdoPlane');
                            ip.attr('data-date', item.startDate);
                            ip.attr('adult', rows.adult);
                            ip.attr('child', rows.child);
                            ip.attr('infant', rows.infant);
                        }
                    }
                
            })
        }
    });
    var vj = $.ajax({
        type: "POST",
        url: "VeGiaReTheoThang.aspx/GetVietJetByMonthResult",
        data: '{sessionId:"' + sid + '",tripType:"1" }',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        error: function () {
        }
    });
    vj.done(function (data) {
        var rows = data.d;
        var date = new Date();
        console.log(rows.depDate);
        var depDateMonth = rows.depDate.split('/')[1];
        var lastDay = new Date(date.getFullYear(), depDateMonth, 0);
        var dayInPrevMonth = new Date(date.getFullYear(), (parseInt(depDateMonth) - 1), 1);
        var dayLeft = GetDayLeft(dayInPrevMonth);
        $.each(rows.ListCheapestflightByMonthOutBound, function (idex, item) {
            if (item.price > 0) {
                var result = '';
                var day = item.startDate.split('/')[0];
                var month = item.startDate.split('/')[1];
                var td = $('.calendar-outbound').find('.d_price[cell-number="' + (parseInt(day) + parseInt(dayLeft)) + '"]');
                var dateArr = item.startDate.split('/');
                var strDate = dateArr[0] + "/" + dateArr[1];
                var city = item.depCity + " - " + item.arrCity;
                if (day <= lastDay.getDate() && month == (lastDay.getMonth() + 1)) {

                    td.removeClass('disable');
                    td.find('.VJ').append(commaSeparateNumber(item.price) + " VND").removeClass('disable');
                    td.find('.hanh_trinh').text(city);
                    td.find('.ngay').text(strDate);
                    td.attr('data-date', item.startDate);
                    var ip = td.find('input[type=radio]');
                    ip.attr('data-date', item.startDate);
                    ip.attr('adult', rows.adult);
                    ip.attr('child', rows.child);
                    ip.attr('infant', rows.infant);


                }
                else if (day <= lastDay.getDate() && month > (lastDay.getMonth() + 1)) {

                    var tdDay = $('.calendar-outbound').find('.d_price[cell-number="' + (parseInt(day) + parseInt(lastDay.getDate()) + parseInt(dayLeft)) + '"]');
                    tdDay.find('.ngay').text(strDate);
                    tdDay.removeClass('disable');
                    tdDay.find('.VJ').append(commaSeparateNumber(item.price) + " VND").removeClass('disable');
                    tdDay.find('.ngay').text(strDate);
                    tdDay.find('.hanh_trinh').text(city);
                    tdDay.attr('data-date', item.startDate);
                    var ip = tdDay.find('input[type="radio"]');
                    ip.attr('data-date', item.startDate);
                    ip.attr('adult', rows.adult);
                    ip.attr('child', rows.child);
                    ip.attr('infant', rows.infant);
                }
            }
            })

        if (check == 'True') {
            var retDateMonth = rows.arrDate.split('/')[1];
            var retlastDay = new Date(date.getFullYear(), retDateMonth, 0);
            var dayInPrevRetMonth = new Date(date.getFullYear(), (parseInt(retDateMonth) - 1), 1);
            var retDayLeft = GetDayLeft(dayInPrevRetMonth);
            $.each(rows.ListCheapestflightByMonthInBound, function (idex, item) {
                if (item.price > 0) {
                    var result = '';
                    var day = item.startDate.split('/')[0];
                    var month = item.startDate.split('/')[1];
                    var td = $('.calendar-inbound').find('.d_price[cell-number="' + (parseInt(day) + parseInt(retDayLeft)) + '"]');
                    var dateArr = item.startDate.split('/');
                    var strDate = dateArr[0] + "/" + dateArr[1];
                    var city = item.depCity + " - " + item.arrCity;
                    if (day <= retlastDay.getDate() && month == (retlastDay.getMonth() + 1)) {
                            td.removeClass('disable');
                            td.find('.VJ').append(commaSeparateNumber(item.price) + " VND").removeClass('disable');
                            td.find('.hanh_trinh').text(city);
                            td.find('.ngay').text(strDate);
                            td.attr('data-date', item.startDate);
                            var ip = td.find('input[type=radio]');
                            ip.attr('data-date', item.startDate);
                            ip.attr('adult', rows.adult);
                            ip.attr('child', rows.child);
                            ip.attr('infant', rows.infant);
                    }
                    else if (day <= retlastDay.getDate() && month > (retlastDay.getMonth() + 1))
                    {
                        var tdDay = $('.calendar-inbound').find('.d_price[cell-number="' + (parseInt(day) + parseInt(lastDay.getDate()) + parseInt(retDayLeft)) + '"]');
                        tdDay.find('.ngay').text(strDate);
                        tdDay.removeClass('disable');
                        tdDay.find('.VJ').append(commaSeparateNumber(item.price) + " VND").removeClass('disable');
                        tdDay.find('.ngay').text(strDate);
                        tdDay.find('.hanh_trinh').text(city);
                        tdDay.attr('data-date', item.startDate);
                        var ip = tdDay.find('input[type="radio"]');
                        ip.attr('data-date', item.startDate);
                        ip.attr('adult', rows.adult);
                        ip.attr('child', rows.child);
                        ip.attr('infant', rows.infant);
                    }
                }
            })
        }
    });
    $.when(bl, vj).done(function ()
    {

        $('.calendar-outbound').find('.d_price').each(function (idex,item)
        {
            var bl = $(this).find('.BL').first();
            var vj = $(this).find('.VJ').first();
            var bl_price = parseInt(bl.text().replace(/,/g, ''));
        
            var vj_price = parseInt(vj.text().replace(/,/g, ''));
            var ip = $(this).find('.rdoPlane');
            if (!isNaN(bl_price) && !isNaN(vj_price)) {
           
                if (bl_price > vj_price) { bl.css('display', 'none'); vj.css('display', 'block'); 
                    $(this).attr('data-price', vj_price);
                    ip.attr('data-price', vj_price);
                    ip.attr('data-airline', "VJ");
                }
                else
                {
                    bl.css('display', 'block'); vj.css('display', 'none');
                    var ip = $(this).find('input[type=radio]');
                    ip.attr('data-price', bl_price);
                    ip.attr('data-airline', "BL");
                }
            }
            else {
                if (!isNaN(bl_price) && isNaN(vj_price)) {
                    bl.css('display', 'block');
                    vj.css('display', 'none');
                    $(this).attr('data-price', bl_price);
                    ip.attr('data-price', bl_price);
                    ip.attr('data-airline', "BL");
                }
                else if (isNaN(bl_price) && !isNaN(vj_price))
                {
                    bl.css('display', 'none');
                    vj.css('display', 'block');
                    $(this).attr('data-price', vj_price);
                    ip.attr('data-price', vj_price);
                    ip.attr('data-airline', "VJ");
                }
            }

        })
        if (check == 'True')
        {
            $('.calendar-inbound').find('.d_price').each(function (idex, item) {
                var bl = $(this).find('.BL').first();
                var vj = $(this).find('.VJ').first();
                var bl_price = parseFloat(bl.text().replace(/,/g, ''));
                var vj_price = parseFloat(vj.text().replace(/,/g, ''));
                var ip = $(this).find('.rdoPlane');
                if (!isNaN(bl_price) && !isNaN(vj_price)) {

                    if (bl_price > vj_price) {
                        bl.css('display', 'none'); vj.css('display', 'block');
                        $(this).attr('data-price', vj_price);
                        ip.attr('data-price', vj_price);
                        ip.attr('data-airline', "VJ");
                    }
                    else {
                        bl.css('display', 'block'); vj.css('display', 'none');
                        $(this).attr('data-price', bl_price);
                        ip.attr('data-price', bl_price);
                        ip.attr('data-airline', "BL");
                    }
                }
                else {
                    if (!isNaN(bl_price) && isNaN(vj_price)) { bl.css('display', 'block'); vj.css('display', 'none'); $(this).attr('data-price', bl_price); ip.attr('data-price', bl_price); ip.attr('data-airline', "BL"); }
                    else if (isNaN(bl_price) && !isNaN(vj_price)) { bl.css('display', 'none'); vj.css('display', 'block'); $(this).attr('data-price', vj_price); ip.attr('data-price', vj_price); ip.attr('data-airline', "VJ"); }
                }
               

            })
        }
        $('.loading').removeClass('show').addClass('hidden');
    })
}

function isEmpty(obj) {
    if (typeof obj == 'undefined' || obj === null || obj === '') return true;
    if (typeof obj == 'number' && isNaN(obj)) return true;
    if (obj instanceof Date && isNaN(Number(obj))) return true;
    return false;
}
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
$(".ac_input").autocomplete("../Elines/Search/AutoSuggestV2.aspx", {
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
