using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace _170516.Models.Administrator
{
    public class CreateSupplierModel
    {
        public int SupplierID { get; set; }

        [Required (ErrorMessage ="Vui lòng nhập tên công ty")]
        public string CompanyName { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập họ người đại diện")]
        public string ContactFName { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên người đại diện")]
        public string ContactLName { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập địa chỉ")]
        public string Address1 { get; set; }

        public string Address2 { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập thành phố/tỉnh")]
        public string City { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [Phone]
        public string Phone { get; set; }
        
        public string Fax { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập địa chỉ email")]
        [EmailAddress(ErrorMessage ="Địa chỉ email không hợp lệ")]
        public string EmailAddress { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập số giảm giá. 0 nếu không có giảm giá")]
        [Range(typeof(double), "0", "100", ErrorMessage = "Giảm giá phải từ 0 đến 100")]
        public double Discount { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập loại sản phẩm")]
        public string ProductType { get; set; }
        public string Logo { get; set; }
        public string ImageType { get; set; }
        public Nullable<bool> IsDiscountAvailable { get; set; }
    }
}