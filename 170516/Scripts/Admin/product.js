$(document).ready(function () {
    // check to toast when delete success
    if (window.sessionStorage.DeletedStatus == "true") {
        toastr.success(window.sessionStorage.DeletedMessage);
        window.sessionStorage.DeletedStatus = null;
    } else if (window.sessionStorage.DeletedStatus == "false") {
        toastr.error(window.sessionStorage.DeletedMessage)
        window.sessionStorage.DeletedStatus = null;
    }

    // enable tiny mce
    tinymce.init({
        selector: '.tiny_mce_input',  // change this value according to your HTML
        plugin: 'a_tinymce_plugin',
        a_plugin_option: true,
        a_configuration_option: 400,
        height : "480"
    });

    // add product specification
    $('#AddSpecificationBtn').on('click', function () {
        var isValid = true;
        var $name = $('#SpecificationName'),
            $value = $('#SpecificationValue'),
            $type = $('#SpecificationTypeID');

        // validate
        if ($name.val() == null || $name.val() == undefined || $name.val().trim() == "") {
            isValid = false;
            // show message
            $name.next().text('Tên đặc điểm không được bỏ trống.').show();
        }

        if ($value.val() == null || $value.val() == undefined || $value.val().trim() == "") {
            isValid = false;
            // show message
            $value.next().text('Giá trị của đặc điểm không được bỏ trống.').show();
        }

        if ($type.val() == null || $type.val() == undefined || $type.val().trim() == "") {
            isValid = false;
            // show message
            $type.next().text('Loại đặc điểm không được bỏ trống.').show();
        }

        if (isValid) {
            // hide all error message of this form
            $name.next().text('').attr('style', 'display:none;');
            $value.next().text('').attr('style', 'display:none;');
            $type.next().text('').attr('style', 'display:none;');

            // prepare model
            var specificationModel = {
                Name: $name.val(),
                Type: $type.val(),
                Value: $value.val()
            };

            // call ajax
            $.ajax({
                url: staticUrl.addSpecification,
                method: "POST",
                data: specificationModel,
                beforeSend: function () {
                    // add loading
                },
                success: function (data) {
                    if (data != null && data != undefined) {
                        // add it to div
                        $('#AddProductSpecificationTable').html(data);

                        // reset forms
                        $name.val('');
                        $value.val('');

                        toastr.success('Thêm đặc điểm cho sản phẩm thành công.');
                    } else {
                        toastr.error('Có lỗi xảy ra trong quá trình lưu. Vui lòng thử lại.');
                    }                    
                },
                error: function (e) {
                    toastr.error('Có lỗi xảy ra trong quá trình lưu. Vui lòng thử lại.');
                }
            })
        }
    })

    // sorting header
    $(".dataTable th").on('click', function () {
        var sortStr = $(this).data('sort');
        var direction = $(this).data('direction');

        if (!Util.IsNullOrWhiteSpace(sortStr) && !Util.IsNullOrWhiteSpace(direction)) {
            // do sort
            addProductModel.SortProduct(sortStr, direction == "asc");
        }
    });

    // btn search
    $('#dataTables_search').on('click', function () {
        addProductModel.ViewProduct();
    });

    // when change value of number of item shown in the grid
    $('#dataTables_showNumberSelect').on('change', function () {
        addProductModel.ViewProduct();
    });

    // before delete, show confirmation dialog
    $('.action-link.remove-link.product').on('click', function () {
        var removeUrl = $(this).data('url');
        var currentViewUrl = addProductModel.GetCurrentViewProductUrl();

        bootbox.dialog({
            message: "Bạn có chắc là muốn xóa sản phẩm này không?",
            title: "Xóa sản phẩm",
            buttons: {
                okAction: {
                    label: "Xóa",
                    className: "btn btn-primary btn-sm",
                    callback: function () {
                        // ajax - remove
                        $.ajax({
                            url: removeUrl,
                            method: "POST",
                            success: function (data) {
                                if (data != null) {
                                    if (data.isResult == true) {
                                        window.sessionStorage.DeletedStatus = true;
                                        window.sessionStorage.DeletedMessage = 'Xóa sản phẩm thành công.';
                                    } else {
                                        window.sessionStorage.DeletedStatus = false;
                                        window.sessionStorage.DeletedMessage = data.result;
                                    }

                                    window.location.href = currentViewUrl;
                                }

                            }, error: function (data) {
                                toastr.error(data.result);

                                window.sessionStorage.IsDeletedStatus = null;
                            }
                        });
                    }
                },
                noAction: {
                    label: "Hủy",
                    className: "btn btn-default btn-sm",
                    callback: function () {

                    }
                }
            }
        });
    });

    // submit form
    $('#SubmitUpdateProduct').on('click', function () {
        // validate before submit form
        if (addProductModel.ValidateAddProduct()) {
            var updateProductFormModel = $('#UpdateProductForm').serializeObject();

            // prepare description
            updateProductFormModel.ProductDescription = tinyMCE.activeEditor.getContent();

            // prepare specification list
            var specificationTrs = $('#UpdateProductSpecificationTable table tbody tr');
            if (specificationTrs.length > 0) {
                var specificationList = [];

                for (var i = 0; i < specificationTrs.length; i++) {
                    var specificationTds = $(specificationTrs[i]).find('td');

                    var specification = {
                        Id: $(specificationTds[0]).text(),
                        Type: $(specificationTds[1]).text(),
                        Name: $(specificationTds[2]).text(),
                        Value: $(specificationTds[3]).text()
                    };

                    if (specification != null)
                        specificationList.push(specification);
                }

                // add the specification list into main model
                updateProductFormModel.specificationList = specificationList;
            }

            $.ajax({
                url: staticUrl.updateProduct,
                data: updateProductFormModel,
                async: true,
                method: "POST",
                dataType: "json",
                cache: false,
                success: function (data) {
                    if (data != null) {
                        if (data.isResult == false) {
                            toastr.error('Có lỗi xảy ra trong quá trình lưu. Vui lòng thử lại.');
                        } else {
                            $('#UpdateProductForm')[0].reset();
                            // Display an info toast with no title
                            toastr.success('Sản phẩm mới lưu thành công.')
                        }
                    } else {
                        toastr.error('Có lỗi xảy ra trong quá trình lưu. Vui lòng thử lại.');
                    }
                }, error: function (e) {
                    toastr.error('Có lỗi xảy ra trong quá trình lưu. Vui lòng thử lại.');
                }
            });
        }
    });

    // submit form
    $('#SubmitAddProduct').on('click', function () {
        // validate before submit form
        if (addProductModel.ValidateAddProduct()) {
            var addProductFormModel = $('#AddProductForm').serializeObject();

            // prepare description
            addProductFormModel.ProductDescription = tinyMCE.activeEditor.getContent();

            // prepare specification list
            var specificationTrs = $('#AddProductSpecificationTable table tbody tr');
            if (specificationTrs.length > 0) {
                var specificationList = [];

                for (var i = 0; i < specificationTrs.length; i++) {
                    var specificationTds = $(specificationTrs[i]).find('td');

                    var specification = {
                        Id: $(specificationTds[0]).text(),
                        Type: $(specificationTds[1]).text(),
                        Name: $(specificationTds[2]).text(),
                        Value: $(specificationTds[3]).text()
                    };

                    if (specification != null)
                        specificationList.push(specification);
                }

                // add the specification list into main model
                addProductFormModel.specificationList = specificationList;
            }

            $.ajax({
                url: staticUrl.addProduct,
                data: addProductFormModel,
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
                    } else {
                        toastr.error('Có lỗi xảy ra trong quá trình lưu. Vui lòng thử lại.');
                    }
                }, error: function (e) {
                    toastr.error('Có lỗi xảy ra trong quá trình lưu. Vui lòng thử lại.');
                }
            });
        }
    });

    // add product category
    $('#addColorLink').on('click', function (e) {
        // show modal
        addProductModel.ShowAddColorModal($(this).data('modal'));
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
            $('#ProductImage').val(data.result.fileType + ":" + data.result.base64Thumbnail);
        }
    }).on('fileuploadfail', function (e, data) {
        // upload fail
        toastr.error('Tải ảnh không thành công. Vui lòng thử lại.');
    });
});

var addProductModel = {
    ShowAddColorModal: function (modalName) {
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
                }).on('hidden.bs.modal', function () {
                    // scroll to addColor link 
                });
                // then show it
                $addColorModal.modal('show');
            }
        });
    },
    ValidateAddProduct: function () {
        // validate add color screen
        var $productName = $('#ProductName');
        var $productQuantity = $('#ProductQuantity');
        var $productPrice = $('#ProductPrice');
        var isValid = true;

        // product name
        if ($productName.val() == null || $productName.val() == '' || $productName.val() == undefined) {
            $productName.parent().addClass('has-error');
            $productName.nextAll('span.input-error-box').text("Làm ơn nhập tên sản phẩm.");
            isValid = false;
        } else {
            $productName.parent().removeClass('has-error');
            $productName.nextAll('span.input-error-box').text("");
        }

        // product quantity
        if ($productQuantity.val() == null || $productQuantity.val() == '' || $productQuantity.val() == undefined) {
            $productQuantity.parent().addClass('has-error');
            $productQuantity.nextAll('span.input-error-box').text("Làm ơn nhập số lượng sản phẩm trong kho.");
            isValid = false;
        } else {
            $productQuantity.parent().removeClass('has-error');
            $productQuantity.nextAll('span.input-error-box').text("");
        }

        // product price
        if ($productPrice.val() == null || $productPrice.val() == '' || $productPrice.val() == undefined) {
            $productPrice.parent().addClass('has-error');
            $productPrice.nextAll('span.input-error-box').text("Làm ơn nhập giá của sản phẩm.");
            isValid = false;
        } else {
            // test regex
            if (sampleRegex.floatRegex.test($productPrice.val())) {
                $productPrice.parent().addClass('has-error');
                $productPrice.nextAll('span.input-error-box').text("Làm ơn chỉ nhập chữ số và dấu phẩy (,).");
                isValid = false;
            } else {
                $productPrice.parent().removeClass('has-error');
                $productPrice.nextAll('span.input-error-box').text("");
            }
        }

        return isValid;
    },
    ViewProduct: function () {
        window.location.href = addProductModel.GetCurrentViewProductUrl();
    },
    GetCurrentViewProductUrl: function () {
        var $activatePage = $('.pageinate_button.active');
        var page = 1; // page
        if ($activatePage.length > 0)
            page = $activatePage[0].text;

        var itemsOnPage = $('#dataTables_showNumberSelect').val(); // items on page
        var searchText = $('#dataTables_show_item_search input[type="search"]').val(); // search text
        var sortField = $('#dataTables_sort_field_hidden').val(); // sort field
        var directionField = $('#dataTables_sort_direction_hidden').val(); // direction field

        return staticUrl.viewProduct + "?page=" + page + "&itemsPerPage="
            + itemsOnPage + "&searchText=" + searchText + "&sortField=" + sortField + "&isAsc=" + directionField;
    },
    SortProduct: function (sortField, isAsc) {
        var $activatePage = $('.pageinate_button.active');
        var page = 1; // page
        if ($activatePage.length > 0)
            page = $activatePage[0].text;
        var itemsOnPage = $('#dataTables_showNumberSelect').val(); // items on page
        var searchText = $('#dataTables_show_item_search input[type="search"]').val(); // search text

        window.location.href = staticUrl.viewProduct + "?page=" + page + "&itemsPerPage="
            + itemsOnPage + "&searchText=" + searchText + "&sortField=" + sortField + "&isAsc=" + isAsc;
    }
}

var addColorModel = {
    ApplyScriptBinding: function () {
        // decide to show table or form
        // get the list of available colors
        //$.ajax({
        //    url: staticUrl.getAvailableColors,
        //    async: true,
        //    method: "GET",
        //    dataType: "json",
        //    cache: false,
        //    success: function (data) {
        //        if (data != null && data.length > 0) {
        //            // so show the available color list with our data
        //            $('#AddColorForm').attr('style', 'display:none;');
        //            $('#colorAvailableList').show();
        //        } else {
        //            // show the form so everybody can insert data
        //            $('#colorAvailableList').attr('style', 'display:none;');
        //            $('#AddColorForm').show();
        //        }
        //    }
        //});


        // button
        $('#ColorFileUploadButton').on('click', function () {
            $('#ColorFileUpload').trigger('click');

            $(this).attr('style', 'display:none;');
        });

        // binding form
        $('.modal-footer button[type="submit"]').on('click', function () {
            // validate before submit form
            if (addColorModel.ValidateAddColor()) {
                $.ajax({
                    url: staticUrl.addColor,
                    data: $('#AddColorForm').serialize(),
                    async: true,
                    method: "POST",
                    dataType: "json",
                    cache: false,
                    success: function (data) {
                        if (data != null) {
                            if (!data.isError) {
                                // create a row in color list
                                var $colorGroup = $('ColorGroup');

                                if ($colorGroup.find('ul').length < 0) {
                                    // create list item
                                    var ul = $('<ul/>').addClass('list-group');
                                    $colorGroup.append(ul);
                                }

                                var $ul = $(ul);
                                var li = $('<li/>').addClass('list-group-item');
                                var codeIdInput = $("<input type='hidden' />").val(data.result.ColorID).appendTo(li);
                                var removeLink = $('<a href="#"><i class="glyphicon glyphicon-remove"></i></a>').on('click', function () {
                                    // call remove
                                }).appendTo(li);
                                var span = $('<span/>').text(data.result.ColorName).appendTo(li);
                                var img = $('<img/>').attr('src', Util.getBase64Url(data.result.Extension, data.result.Base64String)).appendTo(li);

                                var $ul = $(ul).append(li);

                                // success
                                $('#AddColorModal').modal('hide');
                            } else {
                                // error
                                $('#AddColorForm .error-summary').text(data.result);
                            }
                        }
                    }, error: function (e) {

                    }
                });
            }
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
                    $("#ColorImagePreview button[type='button']").prop('disabled', false);
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
                $('#Extension').val(data.result.fileType);
            }
        }).on('fileuploadfail', function (e, data) {
            // upload fail
        });
    },
    ValidateAddColor: function () {
        // validate add color screen
        var $colorName = $('#ColorName');
        var $colorDescription = $('#ColorDescription');
        var $colorBase64String = $('#ColorBase64String');
        var isValid = true;

        if ($colorName.val() == null || $colorName.val() == '' || $colorName.val() == undefined) {
            $colorName.parent().addClass('has-error');
            $colorName.nextAll('span.input-error-box').text("Tên màu không được bỏ trống.");
            isValid = false;
        } else {
            $colorName.parent().removeClass('has-error');
            $colorName.nextAll('span.input-error-box').text("");
        }

        if ($colorBase64String.val() == null || $colorBase64String.val() == '' || $colorBase64String.val() == undefined) {
            $colorBase64String.parent().addClass('has-error');
            $colorBase64String.parent().nextAll('span.input-error-box').text("Ảnh đại diện cho màu không được bỏ trống.");
            isValid = false;
        } else {
            $colorBase64String.parent().removeClass('has-error');
            $colorBase64String.parent().nextAll('span.input-error-box').text("");
        }

        return isValid;
    }
}