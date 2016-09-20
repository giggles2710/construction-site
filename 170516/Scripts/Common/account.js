$(document).ready(function () {
    $('#LoginBtn').on('click', function () {
        // validate before login
        if (accountModel.ValidateLogin()) {
            $.ajax({
                url: staticUrl.login,
                data: $('#LoginForm').serialize(),
                async: false,
                method: "POST",
                dataType: "json",
                cache: false,
                success: function (data) {
                    if (data != null) {
                        if (data.isResult == false) {
                            $('#Password').val('');

                            toastr.error('Có lỗi xảy ra trong quá trình lưu. Vui lòng thử lại.');
                        } else {
                            location.reload();
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