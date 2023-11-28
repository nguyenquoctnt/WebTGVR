function isEmpty(c) {
    return (c == undefined || c == null || c == "" || c.toString().trim() == "");
}
// định dạng tiền #,###,###
function formatMoney($this) {
    if ($this == undefined || $this == null || isEmpty($this.val()))
        return 0;
    var money = convertToMoney($this.val());
    $this.val(money);
}
function convertToMoney(str) {
    if (isEmpty(str))
        return 0;
    str = str.toString();
    str = str.replace(/[^0-9\.]/g, ''); // number only
    str = str.replace(/\D+/g, '');
    if (isEmpty(str))
        return 0;
    str = parseInt(str);
    return str.toString().replace(/\d(?=(?:\d{3})+(?!\d))/g, '$&,');
}
var isEmail = function (email) {
    let patt = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    if (email != "undefined")
        return patt.test(email.toLowerCase().trim());
    return true;
}
var parseDate = function (str) {// 'day/month/year'
    let mdy = str.split('/');
    return new Date(mdy[2], mdy[1] - 1, mdy[0]);
}
var isChildBirthday = function (bdate) {
    let d = new Date();
    let bd = parseDate(bdate);
    if ((d.getFullYear() - bd.getFullYear()) < 18 && (d.getFullYear() - bd.getFullYear()) >= 2)
        return true;
    return false;
}
var isInfantBirthday = function (bday) {
    let d = new Date();
    let bd = parseDate(bdate);
    if ((d.getFullYear() - bd.getFullYear()) < 2)
        return true;
    return false;
}