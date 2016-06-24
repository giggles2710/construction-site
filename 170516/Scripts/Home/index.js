$(document).ready(function () {
    // initialize dl menu
    $('#dl-menu').dlmenu();

    // initialize list group
    $('#list').click(function (event) {
        event.preventDefault();
        $('#products .item').addClass('list-group-item');
    });
    $('#grid').click(function (event) {
        event.preventDefault();
        $('#products .item').removeClass('list-group-item');
        $('#products .item').addClass('grid-group-item');
    });

    // initialize select
    $('.selectpicker').selectpicker({
        maxOptions: 1
    });

    $('.da-slider').cslider();

    // initialize for slick
    $('.responsive-slick').slick({
        dots: true,
        infinite: false,
        speed: 300,
        slidesToShow: 4,
        slidesToScroll: 4,
        prevArrow: false,
        nextArrow: false,
        lazyLoad: 'ondemand',   
        responsive: [
          {
              breakpoint: 1024,
              settings: {
                  slidesToShow: 3,
                  slidesToScroll: 3,
                  infinite: true,
                  dots: true
              }
          },
          {
              breakpoint: 600,
              settings: {
                  slidesToShow: 2,
                  slidesToScroll: 2
              }
          },
          {
              breakpoint: 480,
              settings: {
                  slidesToShow: 1,
                  slidesToScroll: 1
              }
          }
          // You can unslick at a given breakpoint now by adding:
          // settings: "unslick"
          // instead of a settings object
        ]
    });
});