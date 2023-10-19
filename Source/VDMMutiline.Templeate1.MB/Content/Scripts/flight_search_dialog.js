
$(function () {
   
    $("#radiobutt input[type=radio]").each(function (i) {
        $(this).click(function () {

            if ($('input[id*=oneway]').is(":checked")) {
                $("[id$=dateto]").val("");
                $("[id$=dateto]").attr("disabled", "disabled");
            } else {
                $("[id$=dateto]").removeAttr("disabled", "disabled");
            }
        });

    });
    // Hien thi textbox

    var $list_departure = $("#list-departure");
    var $list_arrival = $("#list-arrival");
    var $txtDeparture = $("[id$=txtDeparture]");
    var $txtArrival = $("[id$=txtArrival]");

    //$(document).live("click", function (e) {
    //    $list_departure.hide();
    //    $list_arrival.hide();
    //});


   


    $list_departure.click(function (e) {
        e.stopPropagation();
    });
    $list_arrival.click(function (e) {
        e.stopPropagation();
    });
    $(".domestic-col-departure a").click(function () {
        $txtDeparture.val($(this).text());
        $("[id$=txtNoiDi]").val($(this).text());
        $list_departure.hide();
      //  $list_arrival.show();
      //  $txtArrival.focus();
    });
    $(".domestic-col-arrival a").click(function () {
        $txtArrival.val($(this).text());
        $("[id$=txtNoiDen]").val($(this).text());
        $list_arrival.hide();
       // $("[id$=datefrom]").focus();
    });
    $("#btnMaDi").click(function () {
        //var e = document.getElementById("ddlQuocGia_Di_SanBay");
        //var value = e.options[e.selectedIndex].value;
        //var text = e.options[e.selectedIndex].text;

        //if (value != null && text != null) {
        //    $txtDeparture.val(value + text);

            $list_departure.hide();
        //}
    });
    
    //nhap ma thanh pho - begin
    

    //nhap ma thanh pho - end
    //    khu hoi , mot chieu - beg
   
    //    khu hoi , mot chieu - end
    $("#trans").click(function () {
        //alert(1);
        var from = $txtDeparture.val();
        var to = $txtArrival.val();
        $txtDeparture.val(to);
        $txtArrival.val(from);
    })

    //minus & plus passenger -beg
    var number = 1;
    $(".minus").click(function () {
        if ($("[id$=adt]").val() == "1") {
            $("[id$=adt]").val("1");
        }
        else {
            number = number - 1;
            $("[id$=adt]").val(number);
        }
    })
    $(".plus").click(function () {
        if ($("[id$=adt]").val() == "10") {
            $("[id$=adt]").val("10");
        }
        else {
            number = number + 1;
            $("[id$=adt]").val(number);
        }

       
    })

            var number1 = 0;
            $(".minus1").click(function () {
                if ($("[id$=chd]").val() == "0") {
                    $("[id$=chd]").val("0");
                }
                else {
                    number1 = number1 - 1;
                    $("[id$=chd]").val(number1);
                }
            })
            $(".plus1").click(function () {
                if ($("[id$=chd]").val() == "5") {
                    $("[id$=chd]").val("5");
                }
                else {
                    number1 = number1 + 1;
                    $("[id$=chd]").val(number1);
                }


            })

                    var number2 = 0;
                    $(".minus2").click(function () {
                        if ($("[id$=infant]").val() == "0") {
                            $("[id$=infant]").val("0");
                        }
                        else {
                            number2 = number2 - 1;
                            $("[id$=infant]").val(number2);
                        }
                    })
                    $(".plus2").click(function () {
                        if ($("[id$=infant]").val() == "2") {
                            $("[id$=infant]").val("2");
                        }
                        else {
                            number2 = number2 + 1;
                            $("[id$=infant]").val(number2);
                        }


                    })
    //minus & plus passenger -end

    $("#Close_Di").click(function () {
        $list_departure.hide();
    });
    $("#Close_Den").click(function () {
        $list_arrival.hide();
    });

    //passenger-begin
    $(".minusp1").click(function () {
        $(".minusp1").toggleClass("addp11");
        $(".addp1").toggleClass("hide");
    })
    
     
    $(".minusp2").click(function () {
        $(".minusp2").toggleClass("addp11");
        $(".addp2").toggleClass("hide");
    })
      
     
    //passenger-end

    //news detail

    $(".share4").click(function () {
        $(".NewShare").show();
        $(".hidden1").addClass("hidden3");
        $(".Comment").hide();
    });

    $(".share3").click(function () {
        $(".NewShare").hide();
        $(".hidden1").removeClass("hidden3");
        
    });
    

    $(".comm1").click(function () {
        $(".Comment").show();
        $(".hidden1").addClass("hidden3");
        $(".NewShare").hide();
    });
    $(".comm2").click(function () {
        $(".Comment").hide();
        $(".hidden1").removeClass("hidden3");

    });
    ///news detail

});

