$(document).ready(function(){
   $('.menu_toggle').on('click',function(){
        $('.left.dropdow_mobile').toggleClass('active');
   }); 
   $('.arrow_sup').on('click', function(){
     $(this).toggleClass('open').next().toggleClass('open');
     
   });
   
   
   
   $('.close_popup').on('click',function(){
     $('.popup_overlay').addClass('hidden');
   });
   
   $('.show_popup_ticker').on('click',function(){
     $('.popup_overlay_ticket').addClass('active');
   });
   
   $('.close_popup_ticker').on('click',function(){
     $('.popup_overlay_ticket').removeClass('active');
   });
   
   $('.button_checkbox').on('click', function(){
     $(this).toggleClass('checked');
     $('.attack_up_file').toggleClass('active');
   })
   
});