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
        public const string OrderNotFound = "Đơn hàng này không còn tồn tại trong hệ thống.";
        public const string OrderDetailsNotFound = "Sản phẩm này không còn tồn tại trong đơn hàng.";
        public const string ErrorOccur = "Có lỗi xảy ra trong quá trình cập nhật dữ liệu.";
        public const string InvalidLogin = "Địa chỉ email hoặc mật khẩu không đúng.";

        public const string OrderCanceledStatus = "Canceled";
        public const string OrderDeliveredStatus = "Delivered";
        public const string OrderFulfilledStatus = "Fulfilled";
        public const string OrderIsProcessingtatus = "Processing";

        public static readonly Tuple<int, string>[] SpecificationType = new Tuple<int, string>[] {
            new Tuple<int, string>(1, "Kích cỡ"),
            new Tuple<int, string>(2, "Chi tiết")
        };
    }
}