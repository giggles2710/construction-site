//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace _170516.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class Account
    {
        public Account()
        {
            this.Account1 = new HashSet<Account>();
            this.Orders = new HashSet<Order>();
            this.Products = new HashSet<Product>();
            this.Requests = new HashSet<Request>();
            this.Roles = new HashSet<Role>();
        }
    
        public string AccountID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public Nullable<int> RoleID { get; set; }
        public string HashToken { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public string CreatedUserID { get; set; }
        public string Note { get; set; }
        public byte[] Image { get; set; }
        public string ImageType { get; set; }
        public Nullable<int> FailedLoginCount { get; set; }
        public Nullable<System.DateTime> EndDeactiveTime { get; set; }
    
        public virtual ICollection<Account> Account1 { get; set; }
        public virtual Account Account2 { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<Request> Requests { get; set; }
        public virtual ICollection<Role> Roles { get; set; }
    }
}
