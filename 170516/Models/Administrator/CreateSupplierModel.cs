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

        [Required (ErrorMessage ="Company name is required")]
        public string CompanyName { get; set; }

        [Required(ErrorMessage = "Contact First Name is required")]
        public string ContactFName { get; set; }

        [Required(ErrorMessage = "Contact Last Name is required")]
        public string ContactLName { get; set; }

        [Required(ErrorMessage = "Address 1 is required")]
        public string Address1 { get; set; }

        public string Address2 { get; set; }

        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }

        [Required(ErrorMessage = "Phone is required")]
        [Phone]
        public string Phone { get; set; }
        
        public string Fax { get; set; }

        [Required(ErrorMessage = "Email Address is required")]
        [EmailAddress]
        public string EmailAddress { get; set; }
        [Required]
        [Range(typeof(decimal), "0", "100")]
        public double Discount { get; set; }
        [Required(ErrorMessage = "Product type is required")]
        public string ProductType { get; set; }
        public string Logo { get; set; }
        public string ImageType { get; set; }
        public Nullable<bool> IsDiscountAvailable { get; set; }
    }
}