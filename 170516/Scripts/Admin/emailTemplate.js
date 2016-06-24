$(document).ready(function() {
    //setup tinymce menu for the first time
    tinymce.editors = [];
    tinymce.init({
        mode: "exact",
        elements: "txtA_htmlVersion",
        theme: "modern",
        relative_urls: false,
        remove_script_host: false,
        plugins: [
            "advlist autolink lists charmap preview hr",
            "wordcount visualblocks code",
            "insertdatetime save contextmenu directionality",
            "textcolor colorpicker textpattern imagetools link"
        ],
        toolbar1: "insertfile undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | autolink",
        toolbar2: "preview | forecolor backcolor emoticons | fontselect link",
        image_advtab: true,
        templates: [
            { title: 'Test template 1', content: 'Test 1' },
            { title: 'Test template 2', content: 'Test 2' }
        ],
        setup: function (editor) {
            
        }

    });

    $('#SubmitAddEmailTemp').on('click', function () {        
        var form = $("#addEmailTemplateForm");
        tinyMCE.get("txtA_htmlVersion").save();

        var isValid = true;
        if ($('#ddlEmailFormat').val() == 'HTML') {
            if (tinyMCE.get('txtA_htmlVersion').getContent() == "") {
                isValid = false;
                $('#contentValidation').text('Nội dung không được bỏ trống');
            } else {
                $('#contentValidation').text('');
            }

        } else {
            if ($('#txtA_textVersion').val() == "") {
                isValid = false;
                $('#contentValidation').text('Nội dung không được bỏ trống');
            } else {
                $('#contentValidation').text('');
            }
        }



        form.validate();
        if (form.valid() && isValid) {
           
            $.ajax({
                url: staticUrl.addEmailTemplate,
                data: $('#addEmailTemplateForm').serialize(),                
                method: "POST",
                dataType: "json",
                cache: false,
                success: function (data) {
                    if (data != null) {
                        if (data.isResult == false) {
                            toastr.error('Có lỗi xảy ra trong quá trình lưu. Vui lòng thử lại.');
                        } else {
                            toastr.success(data.result);
                        }
                    }
                }, error: function (e) {
                    toastr.error('Có lỗi xảy ra trong quá trình chỉnh sữa. Vui lòng thử lại.');
                }
            });
        }
    });
});


var emailTemplateSupport = {
    //Binding data to IsHTML field
    ChangeHtmlToText: function (obj) {
        var value = $(obj).val();
        if (value == "HTML") {
            $('#IsHTML').prop('checked', true);            
            $('#emailTemplate-area-html').show();
            $('#emailTemplate-area-text').hide();
        }
        else {
            $('#IsHTML').prop('checked', false);            
            $('#emailTemplate-area-text').show();
            $('#emailTemplate-area-html').hide();
        }
    }
};