$(document).ready(function () {
    // check to toast when delete success
    if (window.sessionStorage.CheckoutStatus == "true") {
        toastr.success(window.sessionStorage.ThankyouMessage);
        window.sessionStorage.CheckoutStatus = null;
    } else if (window.sessionStorage.ErrorStatus == "true") {
        toastr.error(window.sessionStorage.ErrorMessage)
        window.sessionStorage.ErrorStatus = null;
    }

    $('#addToCartBtn').click(function () {
        var isValid = true;

        value = $('#productInCartQuantity').val();
        if (value == null || value == '' || value == undefined) {
            $('#productInCartQuantity').parent().addClass('has-error');
            $('#productInCartQuantity').nextAll('span.input-error-box').text("Vui lòng nhập số lượng");
            isValid = false;
        } else {
            // test regex
            if (!sampleRegex.integerRegex.test(value)) {
                $('#productInCartQuantity').parent().addClass('has-error');
                $('#productInCartQuantity').nextAll('span.input-error-box').text("Làm ơn chỉ nhập số lớn hơn 0");
                isValid = false;
            } else {
                $('#productInCartQuantity').parent().removeClass('has-error');
                $('#productInCartQuantity').nextAll('span.input-error-box').text("");
            }
        }

        if (isValid)
        {
            var cartData = {
                ProductId: $('#ProductID').val(),
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
        }
    });

    $('.addToCartFrCategory').click(function () {
        var cartData = {
            ProductId: $(this).attr('data-content'),
            Quantity: 1
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

    $('#shipping-info-check').on('change', function () {
        if ($('#shipping-info-check').is(":checked"))
        {
            $('#shipping-information').hide();
        }
        else
        {
            $('#shipping-information').show();
        }
    });

    $('#btnCheckout').click(function () {
        var form = $("#checkoutForm");
        var isValid = true;
        form.validate();

        if (!$('#shipping-info-check').is(":checked"))
        {
            //address
            if ($('#Customer_ShipAddress').val() == null || $('#Customer_ShipAddress').val() == '' || $('#Customer_ShipAddress').val() == undefined) {
                //$('#Customer_ShipAddress').parent().addClass('has-error');
                $('#Customer_ShipAddress').nextAll('span.input-error-box').text("Vui lòng nhập địa chỉ chuyển hàng đến.");
                isValid = false;
            } else {
                //$('#Customer_ShipAddress').parent().removeClass('has-error');
                $('#Customer_ShipAddress').nextAll('span.input-error-box').text("");
            }

            //district
            if ($('#Customer_ShipDistrict').val() == null || $('#Customer_ShipDistrict').val() == '' || $('#Customer_ShipDistrict').val() == undefined) {
                //$('#Customer_ShipDistrict').parent().addClass('has-error');
                $('#Customer_ShipDistrict').nextAll('span.input-error-box').text("Vui lòng nhập quận/huyện chuyển hàng đến.");
                isValid = false;
            } else {
                //$('#Customer_ShipDistrict').parent().removeClass('has-error');
                $('#Customer_ShipDistrict').nextAll('span.input-error-box').text("");
            }

            //city
            if ($('#Customer_ShipCity').val() == null || $('#Customer_ShipCity').val() == '' || $('#Customer_ShipCity').val() == undefined) {
                //$('#Customer_ShipCity').parent().addClass('has-error');
                $('#Customer_ShipCity').nextAll('span.input-error-box').text("Vui lòng nhập thành phố/tỉnh chuyển hàng đến.");
                isValid = false;
            } else {
                //$('#Customer_ShipCity').parent().removeClass('has-error');
                $('#Customer_ShipCity').nextAll('span.input-error-box').text("");
            }

            //phone
            if ($('#Customer_ShipPhone').val() == null || $('#Customer_ShipPhone').val() == '' || $('#Customer_ShipPhone').val() == undefined) {
                //$('#Customer_ShipPhone').parent().addClass('has-error');
                $('#Customer_ShipPhone').nextAll('span.input-error-box').text("Vui lòng nhập số điện thoại khi chuyển hàng đến.");
                isValid = false;
            } else {
                ///$('#Customer_ShipPhone').parent().removeClass('has-error');
                $('#Customer_ShipPhone').nextAll('span.input-error-box').text("");
            }
        }
        
        if (form.valid() && isValid)
        {
            $.ajax({
                url: staticUrl.CheckOut,
                data: form.serialize(),
                async: true,
                method: "POST",
                dataType: "json",
                cache: false,
                success: function (data) {
                    if (data.isResult == false) {                        
                        toastr.error(data.result);
                    }
                    else {                        
                        window.sessionStorage.CheckoutStatus = true;
                        window.sessionStorage.ThankyouMessage = "Cảm ơn bạn đã đặt hàng. Hãy kiểm tra email để có thông tin chi tiết.";
                        window.location.href = staticUrl.HomePage;
                    }
                },
                error: function (e) {                    
                    toastr.error('Có lỗi xảy ra trong quá trình thanh toán. Vui lòng thử lại.');
                }
            });
        }
    });
});