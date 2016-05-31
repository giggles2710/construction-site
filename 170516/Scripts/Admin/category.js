$(document).ready(function () {
    // add product category
    $('#addBtn').on('click', function () {
        // show modal
        productCategoryModel.getAddProductCategory();
    });

    $('#btnCreateCategory').on('click', function () {
        alert('abc');
        productCategoryModel.createCategory();
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
    },

    createCategory: function () {
        $.ajax({
            url: '@Url.Action("AddProductCategory", "Administrator")',
            type: 'POST',
            data: $("#createCategoryForm").serialize(),
            success: function (data) {
                if (data.isResult) {
                    alert('created');
                } else {
                    alert('error');
                }
            }
        });
    }
}