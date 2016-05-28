$(document).ready(function () {
    // add product category
    $('#addColorLink').on('click', function (e) {
        // show modal
        productCategoryModel.getAddProductCategory($('#addColorLink').data("modal"));
    });

    $('#ImageBox').on('click', function (e) {
        $('#ImageUpload').trigger('click');
    });

    // initialize file upload plugin
    $('#ImageUpload').fileupload({
        url: staticUrl.uploadFile,
        dataType: 'json',
        autoUpload: true,
        acceptFileTypes: /(\.|\/)(jpe?g|png)$/i,
        maxFileSize: 999000,
        disableImageResize: /Android(?!.*Chrome)|Opera/
            .test(window.navigator.userAgent)
    }).on('fileuploadadd', function (e, data) {
        // replace it with a canvas
        $('#ImageName').text(data.files[0].name)

        // put data to upload button
        $('#ImageSubmitBtn').data(data);

        //data.context = $('<div/>').appendTo('#ImagePreview')
        //var node = $('<p/>').append($('<span/>').text(data.files[0].name));
        //node.appendTo(data.context);
    }).on('fileuploaddone', function (e, data) {
        if (data.result.base64Thumbnail != null) {
            $('#ImageBox').attr('src', data.result.base64Thumbnail);
        }
    }).on('fileuploadfail', function (e, data) {
        $.each(data.files, function (index) {
            var error = $('<span class="text-danger"/>').text('File upload failed.');
            $(data.context.children()[index])
                .append('<br>')
                .append(error);
        });
    }).prop('disabled', !$.support.fileInput)
        .parent().addClass($.support.fileInput ? undefined : 'disabled');
});

var staticUrl = {
    uploadFile: "/Home/UploadImage"
}

var productModel = {
    showModal: function (modalName) {
        // get add product category modal
        var modalID = '#' + modalName + 'Modal';
        var url = $(modalID).data('url');

        $.ajax({
            url: url,
            method: "GET",
            success: function (data) {
                $(modalID).html(data);

                // then show it
                $(modalID).modal('show');
            }
        });
    }
}