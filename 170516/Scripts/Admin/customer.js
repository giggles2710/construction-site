$(document).ready(function () {
    $('#SubmitUpdateCustomer').on('click', function () {
        var form = $("#updateCustomerForm");
        form.validate();
        if (form.valid()) {
            $.ajax({
                url: staticUrl.updateCustomer,
                data: $('#updateCustomerForm').serialize(),
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
                            toastr.success('Khách hàng đã được chỉnh sữa thành công.');
                        }
                    }
                }, error: function (e) {
                    toastr.error('Có lỗi xảy ra trong quá trình chỉnh sữa. Vui lòng thử lại.');
                }
            });
        }
    });

});