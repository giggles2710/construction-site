using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace _170516.Models.Administrator
{
    public class CreateShipperModel
    {
        public int ShipperID { get; set; }
        [Required(ErrorMessage ="Vui lòng nhập tên công ty")]
        public string CompanyName { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        public string Phone { get; set; }
        [EmailAddress(ErrorMessage ="Địa chỉ email không hợp lệ")]
        [Required(ErrorMessage = "Vui lòng nhập địa chỉ email")]
        public string EmailAddress { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập địa chỉ công ty")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tên tỉnh/thành phố")]
        public string City { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tên huyện/phường")]
        public string District { get; set; }
    }
}