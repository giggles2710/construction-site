$(document).ready(function () {
    $('#LoginBtn').on('click', function () {
        // validate before login
        if (accountModel.ValidateLogin()) {
            $.ajax({
                url: staticUrl.login,
                data: $('#LoginForm').serialize(),
                async: true,
                method: "POST",
                dataType: "json",
                cache: false,
                success: function (data) {
                    if (data != null) {
                        if (data.isResult == false) {
                            toastr.error('Có lỗi xảy ra trong quá trình lưu. Vui lòng thử lại.');
                        } else {
                            $('#AddProductForm')[0].reset();
                            // Display an info toast with no title
                            toastr.success('Sản phẩm mới lưu thành công.')
                        }
                    }
                }, error: function (e) {
                    toastr.error('Có lỗi xảy ra trong quá trình lưu. Vui lòng thử lại.');
                }
            });
        }
    })
})

var accountModel = {
    ValidateLogin: function(){
        return true;
    }
}