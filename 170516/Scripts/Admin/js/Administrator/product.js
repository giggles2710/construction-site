$(document).ready(function () {
    // add product category
    $('#addColorLink').on('click', function (e) {
        // show modal
        addProductModel.showAddColorModal($(this).data('modal'));
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

    }).on('fileuploaddone', function (e, data) {
        // upload success
        if (data.result.base64Thumbnail != null && data.result.fileType != null) {
            var fileSrc = "data:image/" + data.result.fileType + ";base64, " + data.result.base64Thumbnail;
            $('#ImageBox').attr('src', fileSrc);
        }
    }).on('fileuploadfail', function (e, data) {
        // upload fail
    });
});

var staticUrl = {
    uploadFile: "/Home/UploadImage"
}

var addProductModel = {
    showAddColorModal: function (modalName) {
        // get add product category modal
        var modalID = '#' + modalName + 'Modal';
        var url = $(modalID).data('url');
        var $addColorModal = $(modalID);

        $.ajax({
            url: url,
            method: "GET",
            success: function (data) {
                $addColorModal.html(data);
                $addColorModal.on('shown.bs.modal', function () {
                    // apply binding
                    addColorModel.ApplyScriptBinding();
                });
                // then show it
                $addColorModal.modal('show');
            }
        });
    }
}

var addColorModel = {
    ApplyScriptBinding: function () {
        // button
        $('#ColorFileUploadButton').on('click', function () {
            $('#ColorFileUpload').trigger('click');

            $(this).attr('style', 'display:none;');
        });

        // binding form
        $('.modal-footer button[type="submit"]').on('click', function () {
            $('#AddColorForm').submit();
        });

        // initialize file upload plugin
        $('#ColorFileUpload').fileupload({
            url: staticUrl.uploadFile,
            dataType: 'json',
            autoUpload: false,
            acceptFileTypes: /(\.|\/)(jpe?g|png)$/i,
            maxFileSize: 999000,
            disableImageResize: /Android(?!.*Chrome)|Opera/
                .test(window.navigator.userAgent)
        }).on('fileuploadadd', function (e, data) {
            if (data.files.length > 0) {
                // image file name
                $('#ColorImagePreview .fileName').text(data.files[0].name);
                // image file preview
                $("#ColorImagePreview button[type='button']").data(data);
                // binding cancel button
                $("#ColorImagePreview button[type='reset']").on('click', function () {
                    $('#ColorImagePreview').attr('style', 'display:none;');
                    // clear the other input
                    $('#ColorBase64String').val('');
                    $('#ColorFileUpload').val('');
                    $('#ColorImagePreview .fileName').text('');
                    $('#ColorImagePreview .filePreview').html('');
                    $('#ColorImagePreview').attr('style', 'display:none;');
                    $('#ColorFileUploadButton').show();
                });
                // binding submit button
                $("#ColorImagePreview button[type='button']").on('click', function () {
                    $(this).data().submit().always(function () {
                        // clear the other inputs
                        $("#ColorImagePreview button[type='button']").prop('disabled', true);
                    });
                });
                // show image box
                $('#ColorImagePreview').show();
            }
        }).on('fileuploadprocessalways', function (e, data) {
            var file = data.files[0];

            if (file != null) {
                $('#ColorImagePreview .filePreview').append(file.preview);
            }
        }).on('fileuploaddone', function (e, data) {
            // upload success
            if (data.result.base64Thumbnail != null) {
                $('#ColorBase64String').val(data.result.base64Thumbnail);
            }
        }).on('fileuploadfail', function (e, data) {
            // upload fail
        });
    }
}