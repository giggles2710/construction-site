$(document).ready(function () {
    // add product category
    $('#addBtn').on('click', function () {
        // show modal
        productCategoryModel.getAddProductCategory();
    });
});

var productCategoryModel = {
    getAddProductCategory: function () {
        // get add product category modal
        var url = $('#addProductCategoryModal').data('url');

        $.ajax({
            url: url,
            method: "GET",
            success: function (data) {
                $('#addProductCategoryModal').html(data);

                // then show it
                $('#addProductCategoryModal').modal('show');
            }
        });
    }
}