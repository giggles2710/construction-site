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
        [Required(ErrorMessage = "Vui lòng nhập họ")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tên")]
        public string LastName { get; set; }               
        public string Address { get; set; }

        public string City { get; set; }
        public string District { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        public string Phone { get; set; }
        [EmailAddress]
        [Required(ErrorMessage = "Vui lòng nhập email")]
        public string EmailAddress { get; set; }
        public string ShipAddress { get; set; }
        public string ShipCity { get; set; }
        public string ShipDistrict { get; set; }
        public string ShipPhone { get; set; }
        public Nullable<System.DateTime> DateEntered { get; set; }
    }
}