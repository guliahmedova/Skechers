// plus minus
$(document).ready(function() {
    $('.minus').click(function () {
        var $input = $(this).parent().find('input');
        var count = parseInt($input.val()) - 1;
        count = count < 1 ? 1 : count;
        $input.val(count);
        $input.change();
        return false;
    });
    $('.plus').click(function () {
        var $input = $(this).parent().find('input');
        $input.val(parseInt($input.val()) + 1);
        $input.change();
        return false;
    });
});




//beden size dropdown 
$(function() {
  
    $('.dropdown > .caption').on('click', function() {
      $(this).parent().toggleClass('open');
    });
    
    $('.dropdown > .list > .item').on('click', function() {
      $('.dropdown > .list > .item').removeClass('selected');
      $(this).addClass('selected').parent().parent().removeClass('open').children('.caption').text( $(this).text() );
    });
    
    $(document).on('keyup', function(evt) {
      if ( (evt.keyCode || evt.which) === 27 ) {
        $('.dropdown').removeClass('open');
      }
    });
    
    $(document).on('click', function(evt) {
      if ( $(evt.target).closest(".dropdown > .caption").length === 0 ) {
        $('.dropdown').removeClass('open');
      }
    });
    
  });


