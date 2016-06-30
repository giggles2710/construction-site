$(document).ready(function () {
    $('#addToCartBtn').click(function () {
        var cartData = {
            ProductId : $('#ProductID').val(),
            Quantity: $('#productInCartQuantity').val()            
        };

        $.ajax({
            url: staticUrl.AddProductToCart,
            data: cartData,
            method: "POST",
            success: function (data) {
                if (data.isResult) {
                    toastr.success('Đã thêm sản phẩm vào giỏ hàng.');
                }
                else {
                    toastr.error(data.result);
                }
            },
            error: function (e) {
                toastr.error('Có lỗi xảy ra khi thêm sản phẩm vào giỏ hàng. Vui lòng thử lại');
            }
        });
    });

    $('#btnViewCartPopup').click(function () {
        $.ajax({
            type: "GET",
            url: staticUrl.ViewPartialCart,            
            success: function (data) {               
                $("#cartPartialView").html(data); //the HTML I returned from the controller
            },
            error: function (errorData) {

            }
        });
    });

    $('.deleteProductInCartBtn').click(function () {
        $.ajax({
            type: "POST",
            url: $(this).attr('data-url'),
            success: function (data) {
                window.location.href = staticUrl.ViewCart; //the HTML I returned from the controller
            },
            error: function (errorData) {

            }
        });
    });

    $('#UpdateProductCartBtn').click(function () {        
        var form = $("#updateCartForm");
        var isValid = true;

        $('#updateCartForm').find('input[type=text]').each(function () {
            value = $(this).val();
            if (value == null || value == '' || value == undefined) {
                $(this).parent().addClass('has-error');
                $(this).nextAll('span.input-error-box').text("Không được bỏ trống");
                isValid = false;
            } else {
                // test regex
                if (!sampleRegex.integerRegex.test(value)) {
                    $(this).parent().addClass('has-error');
                    $(this).nextAll('span.input-error-box').text("Làm ơn chỉ nhập số lớn hơn 0");
                    isValid = false;
                } else {
                    $(this).parent().removeClass('has-error');
                    $(this).nextAll('span.input-error-box').text("");
                }
            }
        });

        if (isValid)
        {
            $.ajax({
                url: staticUrl.UpdateCart,
                data: form.serialize(),
                async: true,
                method: "POST",
                dataType: "json",
                cache: false,
                success: function (data) {
                    if (data.isResult == false)
                    {
                        data.errors.forEach(function (item) {                            
                            var pro = $('#error-pro-'+item.ProductId);
                            pro.parent().addClass('has-error');
                            pro.text(item.Error);
                        });
                    }
                    else
                    {
                        window.location.href = staticUrl.ViewCart;
                    }
                },
                error: function () {                    
                }
            });
        }
    });
});