$(document).ready(function() {
    //setup tinymce menu for the first time
    
    tinymce.init({
        selector: '.tiny_mce_input',  // change this value according to your HTML
        plugin: 'a_tinymce_plugin',
        a_plugin_option: true,
        a_configuration_option: 400,
        height: "480",        
    });

    $('#SubmitAddEmailTemp').on('click', function () {        
        var form = $("#addEmailTemplateForm");

        var isValid = true;
        var htmlContent = tinyMCE.get('txtA_htmlVersion').getContent();

        if ($('#ddlEmailFormat').val() == 'HTML') {
            if (htmlContent == "") {
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
            var dataToSend = {
                EmailTemplateName : $('#EmailTemplateName').val(),
                EmailSubject : $('#EmailSubject').val(),
                IsEnable : $('#IsEnable').val(),
                IsHTML: $('#IsHTML').val(),
                HtmlTextContent: htmlContent,
                PlainTextContent: $('#txtA_textVersion').val()
            }
            $.ajax({
                url: staticUrl.addEmailTemplate,
                data: dataToSend,
                method: "POST",
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

    $('#SubmitUpdateEmailTemp').on('click', function () {
        var form = $("#updateEmailTemplateForm");

        var isValid = true;
        var htmlContent = tinyMCE.get('txtA_htmlVersion').getContent();

        if ($('#ddlEmailFormat').val() == 'HTML') {
            if (htmlContent == "") {
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
            var dataToSend = {
                EmailTemplateId: $('#EmailTemplateId').val(),
                EmailTemplateName: $('#EmailTemplateName').val(),
                EmailSubject: $('#EmailSubject').val(),
                IsEnable: $('#IsEnable').val(),
                IsHTML: $('#IsHTML').val(),
                HtmlTextContent: htmlContent,
                PlainTextContent: $('#txtA_textVersion').val()
            }
            $.ajax({
                url: staticUrl.updateEmailTemplate,
                data: dataToSend,
                method: "POST",
                cache: false,
                success: function (data) {
                    if (data != null) {
                        if (data.isResult == false) {
                            toastr.error('Có lỗi xảy ra trong quá trình chỉnh sửa. Vui lòng thử lại.');
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

    $('.merge-item').on('click', function () {
        if ($('#ddlEmailFormat').val() == 'HTML')
        {
            tinymce.activeEditor.execCommand('mceInsertContent', false, '['+ $(this).text() +']');
        }
        else 
        {
            emailTemplateSupport.addToPlainText('[' + $(this).text() + ']');
        }
    });

    //search section
    $("#dataTable_emailTemplate th").on('click', function () {
        var sortStr = $(this).data('sort');
        var direction = $(this).data('direction');

        if (!Util.IsNullOrWhiteSpace(sortStr) && !Util.IsNullOrWhiteSpace(direction)) {
            // do sort
            emailTemplateSupport.SortEmailTemplate(sortStr, direction == "asc");
        }
    });

    // btn search
    $('#dataTables_searchEmailTemplate').on('click', function () {
        emailTemplateSupport.ViewEmailTemplate();
    });

    // when change value of number of item shown in the grid
    $('#dataTables_showNumberSelectEmailTemplate').on('change', function () {
        emailTemplateSupport.ViewEmailTemplate();
    });
});


var emailTemplateSupport = {
    addToPlainText: function (text)
    {
        var $txt = jQuery("#txtA_textVersion");
        var caretPos = $txt[0].selectionStart;
        var textAreaTxt = $txt.val();
        var txtToAdd = text;
        $txt.val(textAreaTxt.substring(0, caretPos) +txtToAdd + textAreaTxt.substring(caretPos));
    },
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
    },

    SortEmailTemplate: function (sortField, isAsc) {
        var $activatePage = $('.pageinate_button.active');
        var page = 1; // page
        if ($activatePage.length > 0)
            page = $activatePage[0].text;
        var itemsOnPage = $('#dataTables_showNumberSelectEmailTemplate').val(); // items on page
        var searchText = $('#dataTables_show_item_search').val(); // search text

        window.location.href = staticUrl.viewEmailTemplate + "?page=" + page + "&itemsPerPage="
            + itemsOnPage + "&searchText=" + searchText + "&sortField=" + sortField + "&isAsc=" + isAsc;
    },
    ViewEmailTemplate: function () {
        window.location.href = emailTemplateSupport.GetCurrentViewEmailTemplateUrl();
    },
    GetCurrentViewEmailTemplateUrl: function () {
        var $activatePage = $('.pageinate_button.active');
        var page = 1; // page
        if ($activatePage.length > 0)
            page = $activatePage[0].text;

        var itemsOnPage = $('#dataTables_showNumberSelectEmailTemplate').val(); // items on page
        var searchText = $('#dataTables_show_item_search').val(); // search text
        var sortField = $('#dataTables_sort_field_hidden').val(); // sort field
        var directionField = $('#dataTables_sort_direction_hidden').val(); // direction field

        return staticUrl.viewEmailTemplate + "?page=" + page + "&itemsPerPage="
            + itemsOnPage + "&searchText=" + searchText + "&sortField=" + sortField + "&isAsc=" + directionField;
    }
    
};