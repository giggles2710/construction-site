$(document).ready(function () {
    // check to toast when delete success
    if (window.sessionStorage.DeletedStatus == "true") {
        toastr.success(window.sessionStorage.DeletedMessage);
        window.sessionStorage.DeletedStatus = null;
    } else if (window.sessionStorage.DeletedStatus == "false") {
        toastr.error(window.sessionStorage.DeletedMessage)
        window.sessionStorage.DeletedStatus = null;
    }

    $('#SubmitUpdateOrder').on('click', function () {
        var form = $("#updateOrderForm");
        form.validate();
        if (form.valid()) {
            $.ajax({
                url: staticUrl.updateOrder,
                data: $('#updateOrderForm').serialize(),
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
                            toastr.success('Đơn hàng đã được chỉnh sữa thành công.');
                        }
                    }
                }, error: function (e) {
                    toastr.error('Có lỗi xảy ra trong quá trình chỉnh sữa. Vui lòng thử lại.');
                }
            });
        }
    });

    // before delete, show confirmation dialog
    $('.action-link.remove-link.order').on('click', function () {
        var removeUrl = $(this).data('url');
        var currentViewUrl = orderSupportModel.GetCurrentViewOrderUrl();

        bootbox.dialog({
            message: "Bạn có chắc là muốn hủy đơn hàng này không?",
            title: "Hủy đơn hàng",
            buttons: {
                okAction: {
                    label: "Có",
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
                                        window.sessionStorage.DeletedMessage = 'Hủy đơn hàng thành công.';
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
                    label: "Không",
                    className: "btn btn-default btn-sm",
                    callback: function () {

                    }
                }
            }
        });
    });

    //search section
    $("#dataTable_order th").on('click', function () {
        var sortStr = $(this).data('sort');
        var direction = $(this).data('direction');

        if (!Util.IsNullOrWhiteSpace(sortStr) && !Util.IsNullOrWhiteSpace(direction)) {
            // do sort
            orderSupportModel.SortOrder(sortStr, direction == "asc");
        }
    });
    
    // btn search
    $('#dataTables_searchOrder').on('click', function () {
        orderSupportModel.ViewOrder();
    });

    // when change value of number of item shown in the grid
    $('#dataTables_showNumberSelectOrder').on('change', function () {
        orderSupportModel.ViewOrder();
    });

});

var orderSupportModel = {
    SortOrder: function (sortField, isAsc) {
        var $activatePage = $('.pageinate_button.active');
        var page = 1; // page
        if ($activatePage.length > 0)
            page = $activatePage[0].text;
        var itemsOnPage = $('#dataTables_showNumberSelectOrder').val(); // items on page
        var searchText = $('#dataTables_show_item_search input[type="search"]').val(); // search text

        window.location.href = staticUrl.viewOrder + "?page=" + page + "&itemsPerPage="
            + itemsOnPage + "&searchText=" + searchText + "&sortField=" + sortField + "&isAsc=" + isAsc;
    },
    ViewOrder: function () {
        window.location.href = orderSupportModel.GetCurrentViewOrderUrl();
    },
    GetCurrentViewOrderUrl: function () {
        var $activatePage = $('.pageinate_button.active');
        var page = 1; // page
        if ($activatePage.length > 0)
            page = $activatePage[0].text;

        var itemsOnPage = $('#dataTables_showNumberSelectOrder').val(); // items on page
        var searchText = $('#dataTables_show_item_search input[type="search"]').val(); // search text
        var sortField = $('#dataTables_sort_field_hidden').val(); // sort field
        var directionField = $('#dataTables_sort_direction_hidden').val(); // direction field

        return staticUrl.viewOrder + "?page=" + page + "&itemsPerPage="
            + itemsOnPage + "&searchText=" + searchText + "&sortField=" + sortField + "&isAsc=" + directionField;
    }
};