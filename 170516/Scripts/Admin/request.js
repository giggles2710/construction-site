$(document).ready(function () {
    // enable tiny mce
    tinymce.init({
        selector: '.tiny_mce_input',  // change this value according to your HTML
        plugin: 'a_tinymce_plugin',
        a_plugin_option: true,
        a_configuration_option: 400,
        height: "480"
    });

    $('#AnswerButton').on('click', function() {
        var isValid = true;
        var content = tinyMCE.get('ReplyContent').getContent();

        // validate
        if (content == null || content.trim() == "") {
            isValid = false;
            // show message
            alert('khong duoc bo trong noi dung');
        }

        if (isValid) {

            // prepare model
            var answerModel = {
                RequestId: $('#RequestIdInput').val(),
                ReplyContent: content
            };

            // call ajax
            $.ajax({
                url: staticUrl.answerRequest,
                method: "POST",
                data: answerModel,
                success: function(data) {
                    if (data.isResult) {
                        alert(data.result);
                    } else {
                        alert(data.result);
                    }
                },
                error: function(e) {
                    toastr.error('Có lỗi xảy ra trong quá trình lưu. Vui lòng thử lại.');
                }
            });
        }
    });
});