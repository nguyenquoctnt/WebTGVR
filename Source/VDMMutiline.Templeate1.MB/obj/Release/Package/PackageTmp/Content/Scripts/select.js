﻿
$(function () {
    $(".menuRight").click(function (evt) {
         
        $(".hidden1").hide();
        $(".showmenu").show();
    });

    $(".menuLeft").click(function () {
        $(".showmenu").hide();
        $(".hidden1").show();
        $(".selected").show();
    });

 
    
});
