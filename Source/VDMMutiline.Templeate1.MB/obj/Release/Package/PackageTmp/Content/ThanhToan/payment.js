$(document).ready(function () {
    $(".paymenthinhanh").click(function (e) {
        $('.paymenthinhanh p').removeClass('choose-bank');
        $('.content-payment-bank').addClass('hidden');
        $('.paymenthinhanh').addClass('blur');
        $('.paymenthinhanh img').removeClass("border");
        $(this).find("p").addClass('choose-bank');
        $(this).find("img").addClass("border")
        $(".content-payment-bank[bank='" + $(this).attr("bank") + "']").removeClass('hidden');
        $(this).removeClass('blur');
        Getthongtinthanhtoan($(this));

    });
    $(".paymenthinhanhVTC").click(function (e) {
        $('.paymenthinhanhVTC p').removeClass('choose-bank');
        $('.paymenthinhanhVTC').addClass('blur');
        $('.paymenthinhanhVTC img').removeClass("border");
        $(this).find("p").addClass('choose-bank');
        $(this).find("img").addClass("border");
        $(this).removeClass('blur');
        $('#NganHangChon').val($(this).attr("bank"));
    });
    $(".paymenthinhanhVNPAY").click(function (e) {
        $('.paymenthinhanhVNPAY p').removeClass('choose-bank');
        $('.paymenthinhanhVNPAY').addClass('blur');
        $('.paymenthinhanhVNPAY img').removeClass("border");
        $(this).find("p").addClass('choose-bank');
        $(this).find("img").addClass("border");
        $(this).removeClass('blur');
        $('#NganHangChon').val($(this).attr("bank"));
    });
    $('.ttck').click(function (e) {
        $('#IdHinhthucthanhtoan').val($(this).data("idhinhthuc"));
        $('.ttck').addClass('choose-menthod');
        $('.VTCPAY').removeClass('choose-menthod');
        $('.VNPAY').removeClass('choose-menthod');
        $('.TaiVanPhong').removeClass('choose-menthod');
        $('.GiaoVeTanNha').removeClass('choose-menthod');

        $('.menthod-VNPAY').addClass('hidden');
        $('.menthod-VTCPAY').addClass('hidden');
        $('.menthod-TaiVanPhong').addClass('hidden');
        $('.menthod-GiaoVeTanNha').addClass('hidden');

        $('.menthod-ttck').removeClass('hidden');
        $('.menthod-Stripe').removeClass('hidden');
        $('.Stripe').removeClass('choose-menthod');

        return false;
    });
    $('.VNPAY').click(function (e) {
        $('#IdHinhthucthanhtoan').val($(this).data("idhinhthuc"));

        $('.paymenthinhanhVNPAY p').removeClass('choose-bank');
        $('.paymenthinhanhVNPAY').addClass('blur');
        $('.paymenthinhanhVNPAY img').removeClass("border");

        $(".paymenthinhanhVNPAY[bank='VIETCOMBANK']").find("p").addClass('choose-bank');
        $(".paymenthinhanhVNPAY[bank='VIETCOMBANK']").find("img").addClass("border");
        $(".paymenthinhanhVNPAY[bank='VIETCOMBANK']").removeClass('blur');

        $(".thanhtoan").find("strong").html("THANH TOÁN");
        $('.VNPAY').addClass('choose-menthod');
        $('.ttck').removeClass('choose-menthod');
        $('.VTCPAY').removeClass('choose-menthod');
        $('.TaiVanPhong').removeClass('choose-menthod');
        $('.GiaoVeTanNha').removeClass('choose-menthod');

        $('.menthod-VNPAY').addClass('hidden');
        $('.menthod-VTCPAY').addClass('hidden');
        $('.menthod-TaiVanPhong').addClass('hidden');
        $('.menthod-GiaoVeTanNha').addClass('hidden');
        $('.menthod-ttck').addClass('hidden');
        $('.menthod-VNPAY').removeClass('hidden');
        $('#NganHangChon').val("Vietcombank");
        $('.menthod-Stripe').removeClass('hidden');
        $('.Stripe').removeClass('choose-menthod');

    });
    $('.VTCPAY ').click(function (e) {
        $('.paymenthinhanhVTC p').removeClass('choose-bank');
        $('.paymenthinhanhVTC').addClass('blur');
        $('.paymenthinhanhVTC img').removeClass("border");

        $(".paymenthinhanhVTC[bank='Vietcombank']").find("p").addClass('choose-bank');
        $(".paymenthinhanhVTC[bank='Vietcombank']").find("img").addClass("border");
        $(".paymenthinhanhVTC[bank='Vietcombank']").removeClass('blur');



        $('#IdHinhthucthanhtoan').val($(this).data("idhinhthuc"));
        $(".thanhtoan").find("strong").html("THANH TOÁN");
        $('.VTCPAY ').addClass('choose-menthod');
        $('.ttck').removeClass('choose-menthod');
        $('.VNPAY').removeClass('choose-menthod');
        $('.TaiVanPhong').removeClass('choose-menthod');
        $('.GiaoVeTanNha').removeClass('choose-menthod');

        $('.menthod-VNPAY').addClass('hidden');
        $('.menthod-VTCPAY').addClass('hidden');
        $('.menthod-TaiVanPhong').addClass('hidden');
        $('.menthod-GiaoVeTanNha').addClass('hidden');
        $('.menthod-ttck').addClass('hidden');
        $('.menthod-VTCPAY').removeClass('hidden');
        $('#NganHangChon').val("VIETCOMBANK");
        $('.menthod-Stripe').removeClass('hidden');
        $('.Stripe').removeClass('choose-menthod');
    });
    $('.TaiVanPhong ').click(function (e) {
        $('#IdHinhthucthanhtoan').val($(this).data("idhinhthuc"));
        $(".thanhtoan").find("strong").html("THANH TOÁN");
        $('.VTCPAY ').removeClass('choose-menthod');
        $('.ttck').removeClass('choose-menthod');
        $('.VNPAY').removeClass('choose-menthod');
        $('.TaiVanPhong').addClass('choose-menthod');
        $('.GiaoVeTanNha').removeClass('choose-menthod');

        $('.menthod-VNPAY').addClass('hidden');
        $('.menthod-VTCPAY').addClass('hidden');
        $('.menthod-TaiVanPhong').removeClass('hidden');
        $('.menthod-GiaoVeTanNha').addClass('hidden');
        $('.menthod-ttck').addClass('hidden');
        $('.menthod-VTCPAY').addClass('hidden');
        $('.menthod-Stripe').removeClass('hidden');
        $('.Stripe').removeClass('choose-menthod');
    });
    $('.GiaoVeTanNha ').click(function (e) {
        $('#IdHinhthucthanhtoan').val($(this).data("idhinhthuc"));
        $(".thanhtoan").find("strong").html("THANH TOÁN");
        $('.VTCPAY ').removeClass('choose-menthod');
        $('.ttck').removeClass('choose-menthod');
        $('.VNPAY').removeClass('choose-menthod');
        $('.TaiVanPhong').removeClass('choose-menthod');
        $('.GiaoVeTanNha').addClass('choose-menthod');

        $('.menthod-VNPAY').addClass('hidden');
        $('.menthod-VTCPAY').addClass('hidden');
        $('.menthod-TaiVanPhong').addClass('hidden');
        $('.menthod-GiaoVeTanNha').removeClass('hidden');
        $('.menthod-ttck').addClass('hidden');
        $('.menthod-VTCPAY').addClass('hidden');
        $('.menthod-Stripe').removeClass('hidden');
        $('.Stripe').removeClass('choose-menthod');
    });


    $('.Stripe ').click(function (e) {
        $('#IdHinhthucthanhtoan').val($(this).data("idhinhthuc"));
        $(".thanhtoan").find("strong").html("THANH TOÁN");
        $('.VTCPAY ').removeClass('choose-menthod');
        $('.ttck').removeClass('choose-menthod');
        $('.VNPAY').removeClass('choose-menthod');
        $('.TaiVanPhong').removeClass('choose-menthod');
        $('.GiaoVeTanNha').removeClass('choose-menthod');

        $('.menthod-VNPAY').addClass('hidden');
        $('.menthod-VTCPAY').addClass('hidden');
        $('.menthod-TaiVanPhong').addClass('hidden');
        $('.menthod-GiaoVeTanNha').addClass('hidden');
        $('.menthod-ttck').addClass('hidden');
        $('.menthod-VTCPAY').addClass('hidden');
        $('.menthod-Stripe').removeClass('hidden');
        $('.Stripe').addClass('choose-menthod');
    });
    $('#NganHangChon').val($(this).attr("bank"));
});
function disableAllowHold() {
    $('.ttck').addClass('hidden');
    chooseATM();
}
function chooseATM() {
    $(".thanhtoan").find("strong").html("THANH TOÁN");
    $('.VTCPAY').addClass('choose-menthod');
    $('.ttck').removeClass('choose-menthod');
    $('.VNPAY').removeClass('choose-menthod');
    $('.VNPAY2').removeClass('choose-menthod');
    $('.ttvnpay').removeClass('choose-menthod');

    $('.menthod-ttvnpay').addClass('hidden');
    $('.menthod-VNPAY').addClass('hidden');
    $('.menthod-ttck').addClass('hidden');
    $('.menthod-VNPAY2').addClass('hidden');
    $('.menthod-VTCPAY').removeClass('hidden');


    $(".checkboxselect").removeClass("active");
    $(".checkboxselect").find(".fa").removeClass("fa-circle");
    $(".VTCPAY").find(".checkboxselect").addClass("active");
    $(".VTCPAY").find(".checkboxselect").find(".fa").addClass("fa-circle");

}
function addCommas(nStr) {
    nStr += '';
    x = nStr.split('.');
    x1 = x[0];
    x2 = x.length > 1 ? '.' + x[1] : '';
    var rgx = /(\d+)(\d{3})/;
    while (rgx.test(x1)) {
        x1 = x1.replace(rgx, '$1' + ',' + '$2');
    }
    return x1 + x2;
}
$(".item-menthod").click(function () {
    $(".checkboxselect").removeClass("active");
    $(".checkboxselect").find(".fa").removeClass("fa-circle");
    $(this).find(".checkboxselect").addClass("active");
    $(this).find(".checkboxselect").find(".fa").addClass("fa-circle");
})
function Getthongtinthanhtoan(control) {
    $('#SoTaiKhoanChon').val(control.data("stk"));
    $('#NganHangChon').val(control.data("name"));
    $('#ChuTheChon').val(control.data("chuthe"));
    $('#ChiNhanhChon').val(control.data("chinhanh"));
}