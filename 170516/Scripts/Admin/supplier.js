$(document).ready(function () {
    // check to toast when delete success
    if (window.sessionStorage.DeletedStatus == "true") {
        toastr.success(window.sessionStorage.DeletedMessage);
        window.sessionStorage.DeletedStatus = null;
    } else if (window.sessionStorage.DeletedStatus == "false") {
        toastr.error(window.sessionStorage.DeletedMessage)
        window.sessionStorage.DeletedStatus = null;
    }

    $('#btnUpdateSupplierLink').on('click', function () {
        window.location = '/Administrator/UpdateSupplier/' + $('#SupplierID').val();
    });

    $('#SubmitAddSupplier').on('click', function () {
        var form = $("#addSupplierForm");
        form.validate();
        if (form.valid()) {
            $.ajax({
                url: staticUrl.addSupplier,
                data: $('#addSupplierForm').serialize(),
                async: true,
                method: "POST",
                dataType: "json",
                cache: false,
                success: function (data) {
                    if (data != null) {
                        if (data.isResult == false) {
                            toastr.error('Có lỗi xảy ra trong quá trình lưu. Vui lòng thử lại.');
                        } else {
                            $('#addSupplierForm')[0].reset();
                            // Display an info toast with no title
                            toastr.success('Nhà cung cấp mới được tạo thành công.')
                        }
                    }
                }, error: function (e) {
                    toastr.error('Có lỗi xảy ra trong quá trình lưu. Vui lòng thử lại.');
                }
            });
        }
    });
    
    $('#CancelUpdateSupplier').on('click', function () {
        window.location.href = staticUrl.viewSupplier;
    });

    $('#SubmitUpdateSupplier').on('click', function () {
        var form = $("#updateSupplierForm");
        form.validate();
        if (form.valid()) {
            $.ajax({
                url: staticUrl.updateSupplier,
                data: $('#updateSupplierForm').serialize(),
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
                            toastr.success('Nhà cung cấp đã được chỉnh sữa thành công.');
                        }
                    }
                }, error: function (e) {
                    toastr.error('Có lỗi xảy ra trong quá trình chỉnh sữa. Vui lòng thử lại.');
                }
            });
        }
    });

    $('#btnDeleteSupplier').on('click', function () {

        bootbox.dialog({
            message: "Bạn có chắc là muốn xóa nhà cung cấp này không?",
            title: "Xóa nhà cung cấp",
            buttons: {
                okAction: {
                    label: "Xóa",
                    className: "btn btn-primary btn-sm",
                    callback: function () {
                        // ajax - remove
                        $.ajax({
                            url: '/Administrator/RemoveSupplier',
                            data: {
                                id: $('#SupplierID').val()
                            },
                            traditional: true,
                            async: true,
                            method: "POST",
                            cache: false,
                            success: function (data) {
                                if (data != null) {
                                    if (data.isResult == true) {
                                        window.sessionStorage.DeletedStatus = true;
                                        window.sessionStorage.DeletedMessage = 'Xóa nhà cung cấp thành công.';
                                    } else {
                                        window.sessionStorage.DeletedStatus = false;
                                        window.sessionStorage.DeletedMessage = data.result;
                                    }

                                    window.location.href = staticUrl.viewSupplier;
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
    $('.action-link.remove-link.supplier').on('click', function () {
        var removeUrl = $(this).data('url');
        var currentViewUrl = supplierSupportModel.GetCurrentViewSupplierUrl();

        bootbox.dialog({
            message: "Bạn có chắc là muốn xóa nhà cung cấp này không?",
            title: "Xóa nhà cung cấp",
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
                                        window.sessionStorage.DeletedMessage = 'Xóa nhà cung cấp thành công.';
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
    $("#dataTable_supplier th").on('click', function () {
        var sortStr = $(this).data('sort');
        var direction = $(this).data('direction');

        if (!Util.IsNullOrWhiteSpace(sortStr) && !Util.IsNullOrWhiteSpace(direction)) {
            // do sort
            supplierSupportModel.SortSupplier(sortStr, direction == "asc");
        }
    });

    // btn search
    $('#dataTables_searchSupplier').on('click', function () {
        supplierSupportModel.ViewSupplier();
    });

    // when change value of number of item shown in the grid
    $('#dataTables_showNumberSelectSupplier').on('change', function () {
        supplierSupportModel.ViewSupplier();
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
            $('#SupplierImage').val(data.result.fileType + ":" + data.result.base64Thumbnail);
        }
    }).on('fileuploadfail', function (e, data) {
        // upload fail
        toastr.error('Tải ảnh không thành công. Vui lòng thử lại.');
    });
});

var supplierSupportModel = {
    SortSupplier: function (sortField, isAsc) {
        var $activatePage = $('.pageinate_button.active');
        var page = 1; // page
        if ($activatePage.length > 0)
            page = $activatePage[0].text;
        var itemsOnPage = $('#dataTables_showNumberSelectSupplier').val(); // items on page
        var searchText = $('#dataTables_show_item_search input[type="search"]').val(); // search text

        window.location.href = staticUrl.viewSupplier + "?page=" + page + "&itemsPerPage="
            + itemsOnPage + "&searchText=" + searchText + "&sortField=" + sortField + "&isAsc=" + isAsc;
    },
    ViewSupplier: function () {
        window.location.href = supplierSupportModel.GetCurrentViewSupplierUrl();
    },
    GetCurrentViewSupplierUrl: function () {
        var $activatePage = $('.pageinate_button.active');
        var page = 1; // page
        if ($activatePage.length > 0)
            page = $activatePage[0].text;

        var itemsOnPage = $('#dataTables_showNumberSelectSupplier').val(); // items on page
        var searchText = $('#dataTables_show_item_search input[type="search"]').val(); // search text
        var sortField = $('#dataTables_sort_field_hidden').val(); // sort field
        var directionField = $('#dataTables_sort_direction_hidden').val(); // direction field

        return staticUrl.viewSupplier + "?page=" + page + "&itemsPerPage="
            + itemsOnPage + "&searchText=" + searchText + "&sortField=" + sortField + "&isAsc=" + directionField;
    }
};