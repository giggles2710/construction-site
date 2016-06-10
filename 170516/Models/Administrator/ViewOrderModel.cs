using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace _170516.Models.Administrator
{
    public class ViewOrderModel
    {
        public List<ViewOrderItem> Orders { get; set; }
        public int TotalNumber { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPage { get; set; }
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
        public int ItemOnPage { get; set; }
        public string SearchText { get; set; }
        public string SortField { get; set; }
        public bool IsAsc { get; set; }
    }

    public class ViewOrderItem
    {
        public int OrderID { get; set; }
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string OrderNumber { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public System.DateTime OrderDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public System.DateTime ShipDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public System.DateTime RequiredDate { get; set; }
        public int ShipperID { get; set; }
        public string ShipperCompanyName { get; set; }
        public double Freight { get; set; }
        public decimal SalesTax { get; set; }
        public string OrderStatus { get; set; }
        public string OrderStatusToUser { get; set; }
        public bool IsFulfilled { get; set; }
        public bool IsCanceled { get; set; }
        public Nullable<decimal> Paid { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public Nullable<System.DateTime> PaymentDate { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public string ModifiedUserID { get; set; }
        public string ModifiedUserName { get; set; }

        public decimal TotalIncome { get; set; }
    }
}