
/* Vietnamese initialisation for the jQuery UI date picker plugin. */
/* Translated by Le Thanh Huy (lthanhhuy@cit.ctu.edu.vn). */
jQuery(function ($) {
    $.datepickerlunar.regional['vi'] = {
        closeText: 'ÄĂ³ng',
        prevText: '&#x3c;TrÆ°á»›c',
        nextText: 'Tiáº¿p&#x3e;',
        currentText: 'HĂ´m nay',
        monthNames: ['ThĂ¡ng Má»™t', 'ThĂ¡ng Hai', 'ThĂ¡ng Ba', 'ThĂ¡ng TÆ°', 'ThĂ¡ng NÄƒm', 'ThĂ¡ng SĂ¡u',
		'ThĂ¡ng Báº£y', 'ThĂ¡ng TĂ¡m', 'ThĂ¡ng ChĂ­n', 'ThĂ¡ng MÆ°á»i', 'ThĂ¡ng MÆ°á»i Má»™t', 'ThĂ¡ng MÆ°á»i Hai'],
        monthNamesShort: ['ThĂ¡ng 1', 'ThĂ¡ng 2', 'ThĂ¡ng 3', 'ThĂ¡ng 4', 'ThĂ¡ng 5', 'ThĂ¡ng 6',
		'ThĂ¡ng 7', 'ThĂ¡ng 8', 'ThĂ¡ng 9', 'ThĂ¡ng 10', 'ThĂ¡ng 11', 'ThĂ¡ng 12'],
        dayNames: ['Chá»§ Nháº­t', 'Thá»© Hai', 'Thá»© Ba', 'Thá»© TÆ°', 'Thá»© NÄƒm', 'Thá»© SĂ¡u', 'Thá»© Báº£y'],
        dayNamesShort: ['CN', 'T2', 'T3', 'T4', 'T5', 'T6', 'T7'],
        dayNamesMin: ['CN', 'T2', 'T3', 'T4', 'T5', 'T6', 'T7'],
        weekHeader: 'Tu',
        dateFormat: 'dd/mm/yy',
        firstDay: 0,
        isRTL: false,
        showMonthAfterYear: false,
        yearSuffix: ''
    };
    $.datepickerlunar.setDefaults($.datepickerlunar.regional['vi']);
});
