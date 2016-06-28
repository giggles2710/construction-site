$(document).ready(function () {
    // check to toast when delete success
    if (window.sessionStorage.DeletedStatus == "true") {
        toastr.success(window.sessionStorage.DeletedMessage);
        window.sessionStorage.DeletedStatus = null;
    } else if (window.sessionStorage.DeletedStatus == "false") {
        toastr.error(window.sessionStorage.DeletedMessage)
        window.sessionStorage.DeletedStatus = null;
    } else if (window.sessionStorage.UpdateStatus == "true") {
        toastr.success(window.sessionStorage.UpdatedMessage);
        window.sessionStorage.UpdateStatus = null;
    }


    $('#SubmitUpdateOrder').on('click', function () {
        var form = $("#updateOrderForm");
        //form.validate();
        if (orderSupportModel.ValidateUpdateProduct()) {
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
                            window.sessionStorage.UpdateStatus = true;
                            window.sessionStorage.UpdatedMessage = 'Đơn hàng đã được chỉnh sữa thành công.';
                            window.location.href = staticUrl.viewOrderDetails + "/" + $('#OrderID').val();
                        }
                    }
                }, error: function (e) {
                    toastr.error('Có lỗi xảy ra trong quá trình chỉnh sữa. Vui lòng thử lại.');
                }
            });
        }
    });

    $('#CancelUpdateOrder').click(function () {
        window.location.href = staticUrl.viewOrder;
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

    $('.action-link.update-link.orderDetails').on('click', function () {
        var dad = $(this).parent().parent();
        //show button
        dad.find('.action-link.conduct-link.orderDetails').show();
        dad.find('.action-link.back-link.orderDetails').show();
        $(this).hide();

        $price = dad.find('#Price');
        $price.hide();
        $price.next().val($price.text()).show();

        $quantity = dad.find('#Quantity');
        $quantity.hide();
        $quantity.next().val($quantity.text()).show();

        $discount = dad.find('#Discount');
        $discount.hide();
        $discount.next().val($discount.text()).show();

        $size = dad.find('#Size');
        $size.hide();
        $size.next().val($size.text()).show();

        $fulfilled = dad.find('#IsFulfilled');
        if ($fulfilled.text() == 'Đã xong')
        {
            $fulfilled.next().val('true');
        }
        else
        {
            $fulfilled.next().val('false');
        }
        $fulfilled.hide();
        $fulfilled.next().show();
    });

    $('.action-link.back-link.orderDetails').on('click', function () {        
        var dad = $(this).parent().parent();
        $(this).hide();
        dad.find('.action-link.conduct-link.orderDetails').hide();
        dad.find('.action-link.update-link.orderDetails').show();

        $price = dad.find('#Price');
        $price.show();
        $price.next().hide();

        $quantity = dad.find('#Quantity');
        $quantity.show();
        $quantity.next().hide();

        $discount = dad.find('#Discount');
        $discount.show();
        $discount.next().hide();

        $size = dad.find('#Size');
        $size.show();
        $size.next().hide();

        $fulfilled = dad.find('#IsFulfilled');        
        $fulfilled.show();
        $fulfilled.next().hide();
    });

    $('.action-link.conduct-link.orderDetails').on('click', function () {        
        var id = $(this).data('url');

        var dad = $(this).parent().parent();

        $price = dad.find('#Price');
        var iprice = $price.next().val();

        $quantity = dad.find('#Quantity');
        var iquantity = $quantity.next().val();

        $discount = dad.find('#Discount');
        var idiscount = $discount.next().val();

        $size = dad.find('#Size');
        var isize = $size.next().val();

        var isOrderFullfilled = dad.find('#IsFulfilled').next().val();
        
        if (iprice * iquantity < idiscount)
        {
            toastr.error('Giảm giá không thể lớn hơn giá trị của giao dịch trên sản phẩm');
        }
        else
        {
            $.ajax({
                url: '/Administrator/UpdateOrderDetails',
                data: {
                    orderDetaisId: id,
                    price: iprice,
                    quantity: iquantity,
                    discount: idiscount,
                    size: isize,
                    isFulfilled: isOrderFullfilled
                },
                traditional: true,
                async: true,
                method: "POST",
                cache: false,
                success: function (data) {
                    if (data != null) {
                        if (data.isResult == false) {
                            toastr.error('Có lỗi xảy ra trong quá trình lưu. Vui lòng thử lại.');
                        } else {
                            // Display an info toast with no title
                            $total = dad.find('#Total');
                            $total.text(data.result);
                            $price.show();
                            $price.next().hide();
                            $quantity.show();
                            $quantity.next().hide();
                            $discount.show();
                            $discount.next().hide();
                            $size.show();
                            $size.next().hide();

                            if (isOrderFullfilled == 'true' || isOrderFullfilled == 'True') {
                                $fulfilled.text('Đã xong');
                            }
                            else {
                                $fulfilled.text('Chưa giao');
                            }
                            $fulfilled.show();
                            $fulfilled.next().hide();


                            dad.find('.action-link.conduct-link.orderDetails').hide();
                            dad.find('.action-link.back-link.orderDetails').hide();
                            dad.find('.action-link.update-link.orderDetails').show();
                        }
                    }
                }, error: function (e) {
                    toastr.error('Có lỗi xảy ra trong quá trình chỉnh sữa. Vui lòng thử lại.');
                }
            });
        }
    });

    $('input[type=text]').focusout(function () {
        $(this).prev().text($(this).val());
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


    // before delete, show confirmation dialog
    $('.action-link.remove-link.orderDetails').on('click', function () {
        var removeUrl = $(this).data('url');
        var currentViewUrl = orderSupportModel.GetCurrentViewOrderDetailsUrl();

        bootbox.dialog({
            message: "Bạn có chắc là muốn xóa sản phẩm trong đơn hàng này không?",
            title: "Xóa",
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
                                        window.sessionStorage.DeletedMessage = 'Xóa sản phẩm thành công.';
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
    $("#dataTable_orderDetails th").on('click', function () {
        var sortStr = $(this).data('sort');
        var direction = $(this).data('direction');

        if (!Util.IsNullOrWhiteSpace(sortStr) && !Util.IsNullOrWhiteSpace(direction)) {
            // do sort
            orderSupportModel.SortOrderDetails(sortStr, direction == "asc");
        }
    });

    // btn search
    $('#dataTables_searchOrderDetails').on('click', function () {
        orderSupportModel.ViewOrderDetails();
    });

    // when change value of number of item shown in the grid
    $('#dataTables_showNumberSelectOrderDetails').on('change', function () {
        orderSupportModel.ViewOrderDetails();
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
    SortOrderDetails: function (sortField, isAsc) {
        var $activatePage = $('.pageinate_button.active');
        var id = $('#OrderID').val();
        var page = 1; // page
        if ($activatePage.length > 0)
            page = $activatePage[0].text;
        var itemsOnPage = $('#dataTables_showNumberSelectOrderDetails').val(); // items on page
        var searchText = $('#dataTables_show_item_search input[type="search"]').val(); // search text
        window.location.href = staticUrl.viewOrderDetails + "?id=" + id + "&page=" + page + "&itemsPerPage="
            + itemsOnPage + "&searchText=" + searchText + "&sortField=" + sortField + "&isAsc=" + isAsc;
    },
    ViewOrder: function () {
        window.location.href = orderSupportModel.GetCurrentViewOrderUrl();
    },
    ViewOrderDetails: function () {
        window.location.href = orderSupportModel.GetCurrentViewOrderDetailsUrl();
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
    },
    GetCurrentViewOrderDetailsUrl: function () {
        var $activatePage = $('.pageinate_button.active');
        var id = $('#OrderID').val();
        var page = 1; // page
        if ($activatePage.length > 0)
            page = $activatePage[0].text;

        var itemsOnPage = $('#dataTables_showNumberSelectOrderDetails').val(); // items on page
        var searchText = $('#dataTables_show_item_search input[type="search"]').val(); // search text
        var sortField = $('#dataTables_sort_field_hidden').val(); // sort field
        var directionField = $('#dataTables_sort_direction_hidden').val(); // direction field

        return staticUrl.viewOrderDetails + "?id=" + id + "&page=" + page + "&itemsPerPage="
            + itemsOnPage + "&searchText=" + searchText + "&sortField=" + sortField + "&isAsc=" + directionField;
    },
    ValidateUpdateProduct: function () {        
        var $freight = $('#Freight');
        var $saleTax = $('#SalesTax');
        var $paid = $('#Paid');
        var $shipDate = $('#ShipDate');
        var $paymentDate = $('#PaymentDate');
        var isValid = true;

        //feight
        if ($freight.val() == null || $freight.val() == '' || $freight.val() == undefined) {
            $freight.parent().addClass('has-error');
            $freight.nextAll('span.input-error-box').text("Làm ơn nhập số tiền cước.");
            isValid = false;
        } else {
            // test regex
            if (!sampleRegex.integerRegex.test($freight.val())) {
                $freight.parent().addClass('has-error');
                $freight.nextAll('span.input-error-box').text("Làm ơn chỉ nhập chữ số.");
                isValid = false;
            } else {
                $freight.parent().removeClass('has-error');
                $freight.nextAll('span.input-error-box').text("");
            }
        }

        //saleTax
        if ($saleTax.val() == null || $saleTax.val() == '' || $saleTax.val() == undefined) {
            $saleTax.parent().addClass('has-error');
            $saleTax.nextAll('span.input-error-box').text("Làm ơn nhập số tiền thuế.");
            isValid = false;
        } else {
            // test regex
            if (!sampleRegex.integerRegex.test($saleTax.val())) {
                $saleTax.parent().addClass('has-error');
                $saleTax.nextAll('span.input-error-box').text("Làm ơn chỉ nhập chữ số.");
                isValid = false;
            } else {
                $saleTax.parent().removeClass('has-error');
                $saleTax.nextAll('span.input-error-box').text("");
            }
        }

        //paid
        if ($paid.val() == null || $paid.val() == '' || $paid.val() == undefined) {
            $paid.parent().addClass('has-error');
            $paid.nextAll('span.input-error-box').text("Làm ơn nhập số tiền đã trả.");
            isValid = false;
        } else {
            // test regex
            if (!sampleRegex.integerRegex.test($paid.val())) {
                $paid.parent().addClass('has-error');
                $paid.nextAll('span.input-error-box').text("Làm ơn chỉ nhập chữ số.");
                isValid = false;
            } else {
                $paid.parent().removeClass('has-error');
                $paid.nextAll('span.input-error-box').text("");
            }
        }

        /*
        //shipDate
        if ($shipDate.val() == null || $shipDate.val() == '' || $shipDate.val() == undefined) {
            $shipDate.parent().addClass('has-error');
            $shipDate.nextAll('span.input-error-box').text("Làm ơn chọn ngày ship.");
            isValid = false;
        } else {
            // test regex
            if (sampleRegex.dateVNRegex.test($shipDate.val())) {
                $shipDate.parent().addClass('has-error');
                $shipDate.nextAll('span.input-error-box').text("Làm ơn nhập ngày đúng định dạng (ngày/tháng/năm)");
                isValid = false;
            } else {
                $shipDate.parent().removeClass('has-error');
                $shipDate.nextAll('span.input-error-box').text("");
            }
        }


        //paymentDate
        if ($paymentDate.val() == null || $paymentDate.val() == '' || $paymentDate.val() == undefined) {
            $paymentDate.parent().addClass('has-error');
            $paymentDate.nextAll('span.input-error-box').text("Làm ơn chọn ngày trả tiền.");
            isValid = false;
        } else {
            // test regex
            if (sampleRegex.dateVNRegex.test($paymentDate.val())) {
                $paymentDate.parent().addClass('has-error');
                $paymentDate.nextAll('span.input-error-box').text("Làm ơn nhập ngày đúng định dạng (ngày/tháng/năm)");
                isValid = false;
            } else {
                $paymentDate.parent().removeClass('has-error');
                $paymentDate.nextAll('span.input-error-box').text("");
            }
        }
        */

        //validate order details values
        var num = $('#OrderDetaisCount').val();

        var value = 0;
        $('#detailsOrdersTable').find('input[type=text]').each(function () {
            value = $(this).val();
            if (value == null || value == '' || value == undefined) {
                $(this).parent().addClass('has-error');
                $(this).nextAll('span.input-error-box').text("Không được bỏ trống");
                isValid = false;
            } else {
                // test regex
                if (!sampleRegex.integerRegex.test(value)) {
                    $(this).parent().addClass('has-error');
                    $(this).nextAll('span.input-error-box').text("Không hợp lệ. Làm ơn chỉ nhập số");
                    isValid = false;
                } else {
                    $(this).parent().removeClass('has-error');
                    $(this).nextAll('span.input-error-box').text("");
                }
            }
        });

        return isValid;
    }

};