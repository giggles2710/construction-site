$(document).ready(function () {    

    $('#btnCreateCategory').on('click', function () {
        productCategoryModel.createCategory();
    });

    $('#btnUpdateCategoryLink').on('click', function () {
        window.location = '/Administrator/UpdateProductCategory/' + $('#CategoryID').val();
    });
    

    $('#SubmitAddProductCategory').on('click', function () {
        var form = $("#addProductCategoryForm");
        form.validate();        
        if (form.valid())
        {            
            $.ajax({
                url: staticUrl.addProductCategory,
                data: $('#addProductCategoryForm').serialize(),
                async: true,
                method: "POST",
                dataType: "json",
                cache: false,
                success: function (data) {                    
                    if (data != null) {
                        if (data.isResult == false) {
                            toastr.error('Có lỗi xảy ra trong quá trình lưu. Vui lòng thử lại.');
                        } else {
                            $('#addProductCategoryForm')[0].reset();
                            // Display an info toast with no title
                            toastr.success('Danh mục mới lưu thành công.')
                        }
                    }
                }, error: function (e) {
                    toastr.error('Có lỗi xảy ra trong quá trình lưu. Vui lòng thử lại.');
                }
            });
        }                                              
    });
    
    $('#SubmitUpdateProductCategory').on('click', function () {
        var form = $("#updateProductCategoryForm");
        form.validate();
        if (form.valid()) {            
            $.ajax({
                url: staticUrl.updateProductCategory,
                data: $('#updateProductCategoryForm').serialize(),
                async: true,
                method: "POST",
                dataType: "json",
                cache: false,
                success: function (data) {                    
                    if (data != null) {
                        if (data.isResult == false) {
                            toastr.error('Có lỗi xảy ra trong quá trình lưu. Vui lòng thử lại.');
                        } else {                            
                            // Display an info toast with no title
                            toastr.success('Danh mục đã chỉnh sữa thành công.');                            
                        }
                    }
                }, error: function (e) {
                    toastr.error('Có lỗi xảy ra trong quá trình chỉnh sữa. Vui lòng thử lại.');
                }
            });
        }
    });

    $('#btnDeleteProductCategory').on('click', function () {
        $.ajax({
            url: '/Administrator/DeleteProductCategory',
            data: {
                id: $('#CategoryID').val()
            },
            traditional: true,
            async: true,
            method: "POST",            
            cache: false,
            success: function (data) {
                if (data != null) {
                    if (data.isResult == false) {
                        toastr.error(data.result);
                    } else {
                        // Display an info toast with no title
                        toastr.success('Danh mục đã được xóa thành công.');
                        window.location = staticUrl.viewProductCategory;
                    }
                }
            }, error: function (e) {
                toastr.error('Có lỗi xảy ra trong quá trình xóa. Vui lòng thử lại.');
            }
        });
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
}