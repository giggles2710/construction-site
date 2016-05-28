$(document).ready(function () {
    // add product category
    $('#addColorLink').on('click', function (e) {
        // show modal
        productCategoryModel.getAddProductCategory($('#addColorLink').data("modal"));
    });
});

var productModel = {
    showModal: function (modalName) {
        // get add product category modal
        var modalID = '#' + modalName + 'Modal';
        var url = $(modalID).data('url');

        $.ajax({
            url: url,
            method: "GET",
            success: function (data) {
                $(modalID).html(data);

                // then show it
                $(modalID).modal('show');
            }
        });
    }
}