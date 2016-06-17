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
    
    public partial class Order
    {
        public Order()
        {
            this.OrderDetails = new HashSet<OrderDetail>();
        }
    
        public int OrderID { get; set; }
        public int CustomerID { get; set; }
        public string OrderNumber { get; set; }
        public System.DateTime OrderDate { get; set; }
        public System.DateTime ShipDate { get; set; }
        public System.DateTime RequiredDate { get; set; }
        public int ShipperID { get; set; }
        public double Freight { get; set; }
        public decimal SalesTax { get; set; }
        public string OrderStatus { get; set; }
        public bool IsFulfilled { get; set; }
        public bool IsCanceled { get; set; }
        public Nullable<decimal> Paid { get; set; }
        public Nullable<System.DateTime> PaymentDate { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public string ModifiedUserID { get; set; }
    
        public virtual Account Account { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Shipper Shipper { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
