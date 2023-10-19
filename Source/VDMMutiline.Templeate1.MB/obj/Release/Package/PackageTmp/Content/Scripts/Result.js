function Toggle(targetId, controlId) {
    if (document.getElementById) {
        target = document.getElementById(targetId);
        control = document.getElementById(controlId);
        if ($(target).is(':visible')) {
            $(control).addClass('details ');
            
            $(control).removeClass('details_selected FlightItemActive1');
        }
        else {
            $(control).addClass('details_selected FlightItemActive1');
            $(control).removeClass('details ');
        }
        $(target).toggle('blind');
    }
}

function ToggleFare(targetId, controlId) {
    if (document.getElementById) {
        target = document.getElementById(targetId);
        control = document.getElementById(controlId);
        if ($(target).is(':visible')) {
            $(control).addClass('details');
            $(control).removeClass('details_selected');
        }
        else {
            $(control).addClass('details_selected');
            $(control).removeClass('details');
        }
        $(target).toggle('blind');
    }
}

function SelectFlight(value, control) {
    var flight = document.getElementById(control);
    flight.value = value;
}

function SwitchViewMode(mode) {
    if (mode == 'base') {
        $(".view-base-fare").show();
        $(".view-total-fare").hide();
    }
    else {
        $(".view-base-fare").hide();
        $(".view-total-fare").show();
    }
}

function SetPrice(fareAdt, taxAdt, fareChd, taxChd, fareInf, taxInf) {
    var adt = Number($("#lblTotalAdt").text());
    var chd = Number($("#lblTotalChd").text());
    var inf = Number($("#lblTotalInf").text());

    //Set price for each passenger type
    $("#lblFareAdt").text(format("#.##0,####", fareAdt));
    $("#lblFareChd").text(format("#.##0,####", fareChd));
    $("#lblFareInf").text(format("#.##0,####", fareInf));

    $("#lblTaxAdt").text(format("#.##0,####", taxAdt));
    $("#lblTaxChd").text(format("#.##0,####", taxChd));
    $("#lblTaxInf").text(format("#.##0,####", taxInf));

    //Set total price for each passenger type
    $("#lblTotalPriceAdt").text(format("#.##0,####", adt * (fareAdt + taxAdt)));
    $("#lblTotalPriceChd").text(format("#.##0,####", chd * (fareChd + taxChd)));
    $("#lblTotalPriceInf").text(format("#.##0,####", inf * (fareInf + taxInf)));

    //Grand total price
    var grandTotal = adt * (fareAdt + taxAdt) + chd * (fareChd + taxChd) + inf * (fareInf + taxInf);
    $("#lblGrandTotal").text(format("#.##0,####", grandTotal));
}


$(function () {
    function FreezeSortBox() {
        //var control = $('#SortBar');
        //var footer = $('#CheapFlight').position();
        //if ($(window).scrollTop() > 160 && $(window).scrollTop() < footer.top - 300) {
        //    control.addClass('FreezeBox');
        //    var width = $('#grid_9_left').width();
        //    control.width(width - 10);
        //}
        //else {
        //    control.removeClass('FreezeBox');
        //}
    }
    $(window).scroll(FreezeSortBox);
    FreezeSortBox();
});

$(function () {
    function FreezeBasketBox() {
        //var $cache = $('#Basket');
        //var spliter = $('#Spliter').position();
        //var footer = $('#CheapFlight').position();
        //if ($(window).scrollTop() > spliter.top - 50 && $(window).scrollTop() < footer.top - 520) {
        //    $cache.addClass('FreezeBoxNoShadow');
        //    var width = $('#Filter').width();
        //    $cache.width(width);
        //}
        //else {
        //    $cache.removeClass('FreezeBoxNoShadow');
        //}
    }
    $(window).scroll(FreezeBasketBox);
    FreezeBasketBox();
});

$(function () {
    //Toggle show filter button
    var currPosition = 0;
    $(window).scroll(function (event) {
        var filterStatus = $('#Filter').css('left');
        var filterHight = $('#pnShowFilter').css('height');
        if (filterStatus == '0px' && filterHight != '0px') {
            var position = $(this).scrollTop();
            if (position > currPosition) {
                if (position > 100) {
                    $("#pnShowFilter").addClass('hide');
                    $("#pnShowFilter").removeClass('show');
                }
            }
            else {
                if ($(window).scrollTop() < currPosition) {
                    $("#pnShowFilter").addClass('show');
                }
            }
            currPosition = position;
        }
    });
});


function ScrollToControlById(controlId) {
    control = document.getElementById(controlId);
    var targetOffset = $(control).offset().top;
     window.scrollTo(0, targetOffset); return false;
}

