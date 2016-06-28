$(document).ready(function () {
    // enable tiny mce
    tinymce.init({
        selector: '.tiny_mce_input',  // change this value according to your HTML
        plugins: ['advlist autolink lists link image charmap preview hr anchor pagebreak',
        'searchreplace wordcount visualblocks visualchars code',
        'insertdatetime nonbreaking save table contextmenu directionality',
        'paste textcolor colorpicker textpattern imagetools'
        ],
        toolbar1: 'insertfile undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image',
        toolbar2: 'preview | forecolor backcolor',
        formats: {
            alignleft: { selector: 'p,h1,h2,h3,h4,h5,h6,td,th,div,ul,ol,li,table', styles: { textAlign: 'left' } },
            aligncenter: { selector: 'p,h1,h2,h3,h4,h5,h6,td,th,div,ul,ol,li,table', styles: { textAlign: 'center' } },
            alignright: { selector: 'p,h1,h2,h3,h4,h5,h6,td,th,div,ul,ol,li,table', styles: { textAlign: 'right' } }
        },
        a_plugin_option: true,
        a_configuration_option: 400,
        height: "480"
    });

    $('#AnswerButton').on('click', function() {
        var isValid = true;
        var content = tinyMCE.get('ReplyContent').getContent();
        var subject = $('#ReplySubject').val();

        // validate
        if (subject == null || subject.trim() == "") {
            isValid = false;
            // show message
            $('#ReplySubjectValidation').text("Vui lòng nhập tiêu đề trả lời");
        }

        if (content == null || content.trim() == "") {
            isValid = false;
            // show message
            $('#ReplyContentValidation').text("Vui lòng nhập nội dung trả lời");
        }

        if (isValid) {

            // prepare model
            var answerModel = {
                RequestId: $('#RequestIdInput').val(),
                ReplyContent: content,
                ReplySubject: subject
            };

            // call ajax
            $.ajax({
                url: staticUrl.answerRequest,
                method: "POST",
                data: answerModel,
                success: function(data) {
                    if (data.isResult) {
                        toastr.success(data.result);
                    } else {
                        toastr.error(data.result);
                    }
                },
                error: function(e) {
                    toastr.error('Có lỗi xảy ra trong quá trình lưu. Vui lòng thử lại.');
                }
            });
        }
    });

    $('#EmailTemplateForReply').on('change', function () {
        if ($(this).val() == "")
        {
            $('#ReplySubject').val('');
            tinyMCE.get('ReplyContent').setContent('');
        }
        else
        {
            // call ajax
            $.ajax({
                url: staticUrl.getEmailTemplateById + '/' + $('#EmailTemplateForReply').val(),
                method: "GET",              
                success: function (data) {
                    if (data.isResult) {
                        $('#ReplySubject').val(data.replySubject);
                        tinyMCE.get('ReplyContent').setContent(data.replyContent);
                        
                    } else {
                        toastr.error(data.result);
                    }
                },
                error: function (e) {
                    toastr.error('Có lỗi xảy ra.');
                }
            });
        }
    });
});