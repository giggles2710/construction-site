using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _170516.Models
{
    public static class Constant
    {
        public const string ProductEmptyCategory = "Chưa có dữ liệu";
        public const string ImageSourceFormat = "data:image/{0};base64, {1}";
        public const string StringEmpty = "STRING_EMPTY";
        public const string ProductNotFound = "Sản phẩm này không còn tồn tại trong hệ thống.";
        public const string SupplierNotFound = "Nhà cung cấp này không còn tồn tại trong hệ thống.";
        public const string ShipperNotFound = "Shipper này không còn tồn tại trong hệ thống.";
        public const string EmailTemplateNotFound = "EmailTemplate này không tồn tại trong hệ thống.";
        public const string OrderNotFound = "Đơn hàng này không còn tồn tại trong hệ thống.";
        public const string OrderDetailsNotFound = "Sản phẩm này không còn tồn tại trong đơn hàng.";
        public const string UserNotFound = "Người dùng này không còn tồn tại trong hệ thống.";
        public const string UsernameExists = "Tên người dùng đã tồn tại.";
        public const string EmailExists = "Email đã được dùng.";
        public const string ErrorOccur = "Có lỗi xảy ra trong quá trình cập nhật dữ liệu.";
        public const string InvalidLogin = "Địa chỉ email hoặc mật khẩu không đúng.";

        public const string OrderCanceledStatus = "Canceled";
        public const string OrderDeliveredStatus = "Delivered";
        public const string OrderFulfilledStatus = "Fulfilled";
        public const string OrderIsProcessingtatus = "Processing";

        public const int SpecDimensionCode = 1;
        public const int SpecDetailCode = 2;

        public static readonly Tuple<int, string>[] SpecificationType = new Tuple<int, string>[] {
            new Tuple<int, string>(1, "Kích cỡ"),
            new Tuple<int, string>(2, "Chi tiết")
        };

        public const string SessionSpecification = "AddProduct_Specification";

        public const string CustomerNameField = "[T&ecirc;n kh&aacute;ch h&agrave;ng]";
        public const string OrderNumberField = "[M&atilde; đơn h&agrave;ng]";
        public const string OrderDateField = "[Ng&agrave;y giờ đặt h&agrave;ng]";
        public const string OrderDetailsField = "[Chi tiết đơn h&agrave;ng]";
        public const string OrderSummaryField = "[Tổng đơn h&agrave;ng]";
        public const string CustomerEmailField = "[Email kh&aacute;ch h&agrave;ng]";
        public const string CustomerPhoneField = "[Số điện thoại kh&aacute;ch h&agrave;ng]";
        public const string CustomerAddressField = "[Địa chỉ kh&aacute;ch h&agrave;ng]";
        public const string CustomerShipToAddressField = "[Địa chỉ chuyển h&agrave;ng]";
        public const string CustomerPostalCodeField = "[M&atilde; Postal]";

        //public const string OrderNumberField = "[M&atilde; &#273;&#417;n h&agrave;ng]";
        //public const string OrderDetailsField = "[Chi ti&#7871;t &#273;&#417;n h&agrave;ng]";
        //public const string OrderSummaryField = "[T&#7893;ng &#273;&#417;n h&agrave;ng]";
        //public const string OrderDateField = "[Ng&agrave;y gi&#7901; &#273;&#7863;t h&agrave;ng]";
        //public const string CustomerPhoneField = "[S&#7889; &#273;i&#7879;n tho&#7841;i kh&aacute;ch h&agrave;ng]";
        //public const string CustomerAddressField = "[&#272;&#7883;a ch&#7881; kh&aacute;ch h&agrave;ng]";
        //public const string CustomerShipToAddressField = "[&#272;&#7883;a ch&#7881; chuy&#7875;n h&agrave;ng]";

        public const string CompanyNameField = "[T&ecirc;n c&ocirc;ng ty]";
        public const string CompanyWebsiteField = "[Website c&ocirc;ng ty]";

        //Store Session
        public const string Cart = "Cart";
        public const string CartCookie = "ConstructionSiteCart";
        public const string ProductInCartCookie = "ConstructionSiteProducts";
    }

    public enum OrderStatuses
    {
        OrderIsCreated = 0,
        OrderIsBeingProcessing = 1,
        OrderIsDelivered = 2,
        OrderIsFulfilled = 3,
        OrderIsCanceled = 4
    }
}