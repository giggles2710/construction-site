$(document).ready(function () {    
    // check to toast when delete success
    if (window.sessionStorage.DeletedStatus == "true") {
        toastr.success(window.sessionStorage.DeletedMessage);
        window.sessionStorage.DeletedStatus = null;
    } else if (window.sessionStorage.DeletedStatus == "false") {
        toastr.error(window.sessionStorage.DeletedMessage)
        window.sessionStorage.DeletedStatus = null;
    } else if (window.sessionStorage.UpdatedStatus == "true") {
        toastr.success(window.sessionStorage.UpdatedMesssage)
        window.sessionStorage.UpdatedStatus = null;
    }

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
                            var redirectUrl = staticUrl.viewProductCategoryDetail + '/' + $('#CategoryID').val();
                            window.sessionStorage.UpdatedStatus = true;
                            window.sessionStorage.UpdatedMesssage = 'Danh mục đã chỉnh sữa thành công.';
                            window.location.href = redirectUrl;                                                     
                        }
                    }
                }, error: function (e) {
                    toastr.error('Có lỗi xảy ra trong quá trình chỉnh sữa. Vui lòng thử lại.');
                }
            });
        }
    });

    $('#btnDeleteProductCategory').on('click', function () {        

        bootbox.dialog({
            message: "Bạn có chắc là muốn xóa danh mục sản phẩm này không?",
            title: "Xóa danh mục",
            buttons: {
                okAction: {
                    label: "Xóa",
                    className: "btn btn-primary btn-sm",
                    callback: function () {
                        // ajax - remove
                        $.ajax({
                            url: '/Administrator/RemoveProductCategory',
                            data: {
                                id: $('#CategoryID').val()
                            },
                            traditional: true,
                            async: true,
                            method: "POST",
                            cache: false,
                            success: function (data) {
                                if (data != null) {
                                    if (data.isResult == true) {
                                        window.sessionStorage.DeletedStatus = true;
                                        window.sessionStorage.DeletedMessage = 'Xóa danh mục thành công.';
                                    } else {
                                        window.sessionStorage.DeletedStatus = false;
                                        window.sessionStorage.DeletedMessage = data.result;
                                    }
                                    
                                    window.location.href = staticUrl.viewProductCategory;
                                }

                            }, error: function (data) {
                                toastr.error(data.result);

                                window.sessionStorage.IsDeletedStatus = null;
                            }
                        });
                    }
                },
                noAction: {
                    label: "Hủy",
                    className: "btn btn-default btn-sm",
                    callback: function () {

                    }
                }
            }
        });
    });

    // before delete, show confirmation dialog
    $('.action-link.remove-link.category').on('click', function () {
        var removeUrl = $(this).data('url');
        var currentViewUrl = productCategoryModel.GetCurrentViewProductCategoryUrl();

        bootbox.dialog({
            message: "Bạn có chắc là muốn xóa danh mục này không?",
            title: "Xóa danh mục",
            buttons: {
                okAction: {
                    label: "Xóa",
                    className: "btn btn-primary btn-sm",
                    callback: function () {
                        // ajax - remove
                        $.ajax({
                            url: removeUrl,
                            method: "POST",
                            success: function (data) {
                                if (data != null) {
                                    if (data.isResult == true) {
                                        window.sessionStorage.DeletedStatus = true;
                                        window.sessionStorage.DeletedMessage = 'Xóa danh mục thành công.';
                                    } else {
                                        window.sessionStorage.DeletedStatus = false;
                                        window.sessionStorage.DeletedMessage = data.result;
                                    }

                                    window.location.href = currentViewUrl;
                                }

                            }, error: function (data) {
                                toastr.error(data.result);

                                window.sessionStorage.IsDeletedStatus = null;
                            }
                        });
                    }
                },
                noAction: {
                    label: "Hủy",
                    className: "btn btn-default btn-sm",
                    callback: function () {

                    }
                }
            }
        });
    });

    //search section
    $("#dataTable_category th").on('click', function () {
        var sortStr = $(this).data('sort');
        var direction = $(this).data('direction');

        if (!Util.IsNullOrWhiteSpace(sortStr) && !Util.IsNullOrWhiteSpace(direction)) {
            // do sort
            productCategoryModel.SortCategory(sortStr, direction == "asc");
        }
    });

    // btn search
    $('#dataTables_searchCategory').on('click', function () {
        productCategoryModel.ViewProductCategory();
    });

    // when change value of number of item shown in the grid
    $('#dataTables_showNumberSelectCategory').on('change', function () {
        productCategoryModel.ViewProductCategory();
    });

    $('#ImageBox').on('click', function (e) {
        $('#ImageUpload').trigger('click');
    });

    // initialize file upload plugin
    $('#ImageUpload').fileupload({
        url: staticUrl.uploadFile,
        dataType: 'json',
        autoUpload: true,
        acceptFileTypes: /(\.|\/)(jpe?g|png)$/i,
        maxFileSize: 999000,
        disableImageResize: /Android(?!.*Chrome)|Opera/
            .test(window.navigator.userAgent)
    }).on('fileuploadadd', function (e, data) {
        // replace it with a canvas
        $('#ImageName').text(data.files[0].name)

        // put data to upload button
        $('#ImageSubmitBtn').data(data);

    }).on('fileuploaddone', function (e, data) {
        // upload success
        if (data.result.base64Thumbnail != null && data.result.fileType != null) {
            var fileSrc = "data:image/" + data.result.fileType + ";base64, " + data.result.base64Thumbnail;
            $('#ImageBox').attr('src', fileSrc);
            $('#ProductImage').val(data.result.fileType + ":" + data.result.base64Thumbnail);
        }
    }).on('fileuploadfail', function (e, data) {
        // upload fail
        toastr.error('Tải ảnh không thành công. Vui lòng thử lại.');
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
    SortCategory: function (sortField, isAsc) {
        var $activatePage = $('.pageinate_button.active');
        var page = 1; // page
        if ($activatePage.length > 0)
            page = $activatePage[0].text;
        var itemsOnPage = $('#dataTables_showNumberSelectCategory').val(); // items on page
        var searchText = $('#dataTables_show_item_search').val(); // search text

        window.location.href = staticUrl.viewProductCategory + "?page=" + page + "&itemsPerPage="
            + itemsOnPage + "&searchText=" + searchText + "&sortField=" + sortField + "&isAsc=" + isAsc;
    },
    ViewProductCategory: function () {
        window.location.href = productCategoryModel.GetCurrentViewProductCategoryUrl();
    },
    GetCurrentViewProductCategoryUrl: function () {
        var $activatePage = $('.pageinate_button.active');
        var page = 1; // page
        if ($activatePage.length > 0)
            page = $activatePage[0].text;

        var itemsOnPage = $('#dataTables_showNumberSelectCategory').val(); // items on page
        var searchText = $('#dataTables_show_item_search').val(); // search text
        var sortField = $('#dataTables_sort_field_hidden').val(); // sort field
        var directionField = $('#dataTables_sort_direction_hidden').val(); // direction field

        return staticUrl.viewProductCategory + "?page=" + page + "&itemsPerPage="
            + itemsOnPage + "&searchText=" + searchText + "&sortField=" + sortField + "&isAsc=" + directionField;
    }

}