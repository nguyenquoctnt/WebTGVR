$(document).ready(function () {
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
    if ($('.oneway input:checked').length > 0) {
        $('.retBlock').css("display", "none");
        $('.retDate').val(" ");
    }
    if ($('.round-trip input:checked').length > 0) {
        $('.retBlock').css("display", "block");
    }
    $(".oneway input").click(function () {
        $(".retBlock").css("display", "none");
        $('.retDate').val(" ");
    });
    $(".round-trip input").click(function () {
        $(".retBlock").css("display", "block");
    });
    $('.selector select').each(function (index, value) {
        if ($(this).val() > 0) $(this).addClass('focus-input');
    });
    $('.sub-form-search input[type=text]').each(function (index, value) {
        if ($(this).val() != '') $(this).addClass('focus-input');
    });
    $('.sub-form-search input[type=text]').keyup(function () {
        if ($(this).val() != '') $(this).addClass('focus-input');
        else $(this).removeClass('focus-input');
    });
    $('.selector select').change(function () {
        if ($(this).val() > 0) $(this).addClass('focus-input');
        else $(this).removeClass('focus-input');
    });
    $('.waytype input').click(function () {
        $('.waytype label').removeClass('selected');
        $(this).parent().next().addClass('selected');
    });
    $('.waytype input:checked').each(function (index, value) {
        $(this).parent().next().addClass('selected');
    });
    $('.waytype input').click(function () {
        $('.waytype label').removeClass('selected');
        $(this).parent().next().addClass('selected');
    });

    $('.depDate').datepicker({
        onSelect: function (date) {
            if ($('input[value="radioRoundTrip"]').attr('checked') == 'checked') {
                var rtdatetext = $('.retDate').val().split('/');
                var depdatetext = date.split('/')
                var rtdate = new Date(rtdatetext[2], (rtdatetext[1] - 1), rtdatetext[0]);
                var depdate = new Date(depdatetext[2], (depdatetext[1] - 1), depdatetext[0]);
                var oneday = 24 * 60 * 60 * 1000;
                var count = Math.round((rtdate - depdate) / (oneday));
                if (count <= 0) {
                    var newdate = new Date(depdate.setDate(depdate.getDate() + 3));
                    var strnewdate = newdate.getDate() + "/" + (newdate.getMonth() + 1) + "/" + newdate.getFullYear();
                    $('.retDate').val(strnewdate);

                }

            }
        },
        dateFormat: "dd/mm/yy",
        changeMonth: true,
        minDate: 0
    })
    $('.retDate').datepicker({
        showOn: 'both',
        buttonImage: '../Images/ThemeForest/icon/blank.png',
        buttonText: '',
        buttonImageOnly: true,
        changeYear: false,
        dateFormat: "dd/mm/yy",
        changeMonth: true,
        minDate: 0
    })
});