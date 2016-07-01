using _170516.Models.Administrator;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace _170516.Models
{
    public class CheckoutModel
    {
        public CartViewModel Cart { get; set; }
        public CustomerCheckoutModel Customer { get; set; }
    }

    public class CustomerCheckoutModel
    {
        public CustomerCheckoutModel()
        {
            SameInfoForShipping = true;
        }
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
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [Required(ErrorMessage = "Vui lòng nhập email")]
        public string EmailAddress { get; set; }
        public string AdditionalInformation { get; set; }
        public bool SameInfoForShipping { get; set; }
        public string ShipAddress { get; set; }        
        public string ShipCity { get; set; }        
        public string ShipDistrict { get; set; }        
        public string ShipPhone { get; set; }        
    }
}