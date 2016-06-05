$(document).ready(function () {
    // check to toast when delete success
    if (window.sessionStorage.DeletedStatus == "true") {
        toastr.success(window.sessionStorage.DeletedMessage);
        window.sessionStorage.DeletedStatus = null;
    } else if (window.sessionStorage.DeletedStatus == "false") {
        toastr.error(window.sessionStorage.DeletedMessage)
        window.sessionStorage.DeletedStatus = null;
    }

    $('#btnUpdateShipperLink').on('click', function () {
        window.location = '/Administrator/UpdateShipper/' + $('#ShipperID').val();
    });

    $('#SubmitAddShipper').on('click', function () {
        var form = $("#addShipperForm");
        form.validate();
        if (form.valid()) {
            $.ajax({
                url: staticUrl.addShipper,
                data: $('#addShipperForm').serialize(),
                async: true,
                method: "POST",
                dataType: "json",
                cache: false,
                success: function (data) {
                    if (data != null) {
                        if (data.isResult == false) {
                            toastr.error('Có lỗi xảy ra trong quá trình lưu. Vui lòng thử lại.');
                        } else {
                            $('#addShipperForm')[0].reset();
                            // Display an info toast with no title
                            toastr.success('Shipper mới được tạo thành công.')
                        }
                    }
                }, error: function (e) {
                    toastr.error('Có lỗi xảy ra trong quá trình lưu. Vui lòng thử lại.');
                }
            });
        }
    });

    $('#SubmitUpdateShipper').on('click', function () {
        var form = $("#updateShipperForm");
        form.validate();
        if (form.valid()) {
            $.ajax({
                url: staticUrl.updateShipper,
                data: $('#updateShipperForm').serialize(),
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
                            toastr.success('Shipper đã được chỉnh sữa thành công.');
                        }
                    }
                }, error: function (e) {
                    toastr.error('Có lỗi xảy ra trong quá trình chỉnh sữa. Vui lòng thử lại.');
                }
            });
        }
    });

    $('#btnDeleteShipper').on('click', function () {

        bootbox.dialog({
            message: "Bạn có chắc là muốn xóa nhà Shipper này không?",
            title: "Xóa Shipper",
            buttons: {
                okAction: {
                    label: "Xóa",
                    className: "btn btn-primary btn-sm",
                    callback: function () {
                        // ajax - remove
                        $.ajax({
                            url: '/Administrator/RemoveShipper',
                            data: {
                                id: $('#ShipperID').val()
                            },
                            traditional: true,
                            async: true,
                            method: "POST",
                            cache: false,
                            success: function (data) {
                                if (data != null) {
                                    if (data.isResult == true) {
                                        window.sessionStorage.DeletedStatus = true;
                                        window.sessionStorage.DeletedMessage = 'Xóa Shipper thành công.';
                                    } else {
                                        window.sessionStorage.DeletedStatus = false;
                                        window.sessionStorage.DeletedMessage = data.result;
                                    }

                                    window.location.href = staticUrl.viewShipper;
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
    $('.action-link.remove-link.shipper').on('click', function () {
        var removeUrl = $(this).data('url');
        var currentViewUrl = shipperSupportModel.GetCurrentViewShipperUrl();

        bootbox.dialog({
            message: "Bạn có chắc là muốn xóa Shipper này không?",
            title: "Xóa Shipper",
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
                                        window.sessionStorage.DeletedMessage = 'Xóa Shipper thành công.';
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
    $("#dataTable_shipper th").on('click', function () {
        var sortStr = $(this).data('sort');
        var direction = $(this).data('direction');

        if (!Util.IsNullOrWhiteSpace(sortStr) && !Util.IsNullOrWhiteSpace(direction)) {
            // do sort
            shipperSupportModel.SortShipper(sortStr, direction == "asc");
        }
    });

    // btn search
    $('#dataTables_searchShipper').on('click', function () {
        shipperSupportModel.ViewShipper();
    });

    // when change value of number of item shown in the grid
    $('#dataTables_showNumberSelectShipper').on('change', function () {
        shipperSupportModel.ViewShipper();
    });

});

var shipperSupportModel = {
    SortShipper: function (sortField, isAsc) {
        var $activatePage = $('.pageinate_button.active');
        var page = 1; // page
        if ($activatePage.length > 0)
            page = $activatePage[0].text;
        var itemsOnPage = $('#dataTables_showNumberSelectShipper').val(); // items on page
        var searchText = $('#dataTables_show_item_search input[type="search"]').val(); // search text

        window.location.href = staticUrl.viewShipper + "?page=" + page + "&itemsPerPage="
            + itemsOnPage + "&searchText=" + searchText + "&sortField=" + sortField + "&isAsc=" + isAsc;
    },
    ViewShipper: function () {
        window.location.href = shipperSupportModel.GetCurrentViewShipperUrl();
    },
    GetCurrentViewShipperUrl: function () {
        var $activatePage = $('.pageinate_button.active');
        var page = 1; // page
        if ($activatePage.length > 0)
            page = $activatePage[0].text;

        var itemsOnPage = $('#dataTables_showNumberSelectShipper').val(); // items on page
        var searchText = $('#dataTables_show_item_search input[type="search"]').val(); // search text
        var sortField = $('#dataTables_sort_field_hidden').val(); // sort field
        var directionField = $('#dataTables_sort_direction_hidden').val(); // direction field

        return staticUrl.viewShipper + "?page=" + page + "&itemsPerPage="
            + itemsOnPage + "&searchText=" + searchText + "&sortField=" + sortField + "&isAsc=" + directionField;
    }
};