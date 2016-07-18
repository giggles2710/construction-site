$(document).ready(function () {
    $("#owl-demo").owlCarousel({
        slideSpeed: 300,
        paginationSpeed: 400,
        singleItem: true,
        itemsScaleUp: true,
        responsive: true,
        theme: "owl-theme",
        transitionStyle: "fade"
    });

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

    $('#product_itemNumber_select .selectpicker').on('change', function () {
        viewProductDetail();
    });

    $('#product_filter_select .selectpicker').on('change', function () {
        viewProductDetail();
    });
});

var viewProductDetail = function () {
    window.location.href = getViewProductDetailUrl();
}

var getViewProductDetailUrl = function () {
    var $activatePage = $('.paginate_button.active a');
    var page = 1; // page
    if ($activatePage.length > 0)
        page = $activatePage[0].text;

    var id = $('#category_id').val();
    var itemsOnPage = $('#product_itemNumber_select .selectpicker').val(); // items on page
    var sortField = $('#product_filter_select .selectpicker').val(); // sort field
        
    return staticUrl.ViewCategory + "?page=" + page + "&itemsPerPage="
        + itemsOnPage + "&sortField=" + (sortField == undefined ? "" : sortField);
}