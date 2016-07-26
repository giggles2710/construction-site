$(document).ready(function () {
    // before delete, show confirmation dialog
    $('.action-link.remove-link.product').on('click', function () {
        var removeUrl = $(this).data('url');

        bootbox.dialog({
            message: "Bạn có chắc là muốn xóa thông tin này không?",
            title: "Xóa thông tin hệ thống",
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
                                        window.sessionStorage.DeletedMessage = 'Xóa thông tin hệ thống thành công.';
                                    } else {
                                        window.sessionStorage.DeletedStatus = false;
                                        window.sessionStorage.DeletedMessage = data.result;
                                    }

                                    window.location.href = staticUrl.ViewSystemVariable;
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

    $('.add-update-modal').on('click', function () {
        var $link = $(this);

        $('#add-system-variable-modal').on('shown.bs.modal', function (e) {
            // update variable
            if ($link.data('id') != undefined && $link.data('id') != null && $link.data('id') !== "")
                $('#VariableId').val($link.data('id'));

            if ($link.data('code') != undefined && $link.data('code') != null && $link.data('code') !== "")
                $('#Code').val($link.data('code'));

            if ($link.data('name') != undefined && $link.data('name') != null && $link.data('name') !== "")
                $('#Name').val($link.data('name'));

            if ($link.data('value') != undefined && $link.data('value') != null && $link.data('value') !== "")
                $('#Value').val($link.data('value'));

            $('#SubmitSystemVariable').on('click', function () {
                var isValid = true;

                var $code = $('#Code'), $name = $('#Name'), $value = $('#Value'), $variableId = $('#VariableId');

                // validate
                if ($code.val() == null || $code.val() == undefined || $code.val().trim() == "") {
                    isValid = false;
                    // show message
                    $code.next().text('Làm ơn nhập mã thông tin.').show();
                }

                if ($name.val() == null || $name.val() == undefined || $name.val().trim() == "") {
                    isValid = false;
                    // show message
                    $name.next().text('Làm ơn nhập tên thông tin.').show();
                }

                if ($value.val() == null || $value.val() == undefined || $value.val().trim() == "") {
                    isValid = false;
                    // show message
                    $value.next().text('Làm ơn nhập nội dung thông tin.').show();
                }

                if (isValid) {
                    // hide all error message of this form
                    $code.next().text('').attr('style', 'display:none;');
                    $name.next().text('').attr('style', 'display:none;');
                    $value.next().text('').attr('style', 'display:none;');

                    // prepare model
                    var addSystemVariableModel = {
                        VariableId: $variableId.val(),
                        Name: $name.val(),
                        Code: $code.val(),
                        Value: $value.val()
                    };

                    // call ajax
                    $.ajax({
                        url: staticUrl.AddUpdateSystemVariable,
                        method: "POST",
                        data: addSystemVariableModel,
                        success: function (data) {
                            if (data != null) {
                                if (data.isResult) {
                                    // reset forms
                                    window.location = staticUrl.ViewSystemVariable;

                                    toastr.success('Thêm thông tin sản phẩm thành công.');
                                    return;
                                } else {
                                    toastr.error(data.result);
                                }
                            } else {
                                toastr.error('Có lỗi xảy ra trong quá trình lưu. Vui lòng thử lại.');
                            }
                        },
                        error: function (e) {
                            toastr.error('Có lỗi xảy ra trong quá trình lưu. Vui lòng thử lại.');
                        }
                    });
                }
            })
        })
        // open modal
        $('#add-system-variable-modal').modal('show');
    });


});