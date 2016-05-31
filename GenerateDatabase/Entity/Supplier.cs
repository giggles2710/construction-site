//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GenerateDatabase.Entity
{
    using System;
    using System.Collections.Generic;
    
    public partial class Supplier
    {
        public Supplier()
        {
            this.Products = new HashSet<Product>();
        }
    
        public int SupplierID { get; set; }
        public string CompanyName { get; set; }
        public string ContactFName { get; set; }
        public string ContactLName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string EmailAddress { get; set; }
        public double Discount { get; set; }
        public string ProductType { get; set; }
        public Nullable<bool> IsDiscountAvailable { get; set; }
    
        public virtual ICollection<Product> Products { get; set; }
    }
}
