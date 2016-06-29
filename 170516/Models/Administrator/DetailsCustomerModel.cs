using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace _170516.Models.Administrator
{
    public class DetailsCustomerModel
    {
        public int CustomerID { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập họ và tên")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập địa chỉ")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập thành phố/tỉnh")]
        public string City { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập quận/huyện")]
        public string District { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        public string Phone { get; set; }
        [EmailAddress(ErrorMessage ="Email không hợp lệ")]
        [Required(ErrorMessage = "Vui lòng nhập email")]
        public string EmailAddress { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập địa chỉ ship đến")]
        public string ShipAddress { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập thành phố/tỉnh ship đến")]
        public string ShipCity { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập thành quận/huyện ship đến")]
        public string ShipDistrict { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập số điện thoại khi sản phẩm ship đến")]
        public string ShipPhone { get; set; }
        public string AdditionalInformation { get; set; }
        public Nullable<System.DateTime> DateEntered { get; set; }
    }
}