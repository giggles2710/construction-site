$(document).ready(function () {
    // enable tiny mce
    tinymce.init({
        selector: '.tiny_mce_input',  // change this value according to your HTML
        plugin: 'a_tinymce_plugin',
        a_plugin_option: true,
        a_configuration_option: 400,
        height: "480"
    });

    $('#AnswerButton').on('click', function () {
        var isValid = true;
        var $content = $('#ReplyContent');

        // validate
        if ($content.val() == null || $content.val() == undefined || $content.val().trim() == "") {
            isValid = false;
            // show message
            $content.next().text('Nội dung trả lời không được bỏ trống.').show();
        }

        if (isValid) {
            // hide all error message of this form
            $content.next().text('').attr('style', 'display:none;');

            // prepare model
            var answerModel = {
                RequestId: $('#RequestIdInput').val(),
                ReplyContent: $content.val()
            };

            // call ajax
            $.ajax({
                url: staticUrl.answerRequest,
                method: "POST",
                data: answerModel,
                error: function (e) {
                    toastr.error('Có lỗi xảy ra trong quá trình lưu. Vui lòng thử lại.');
                }
            });
        }
    })
});