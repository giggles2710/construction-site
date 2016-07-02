using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace _170516.Models.Administrator
{
    public class ViewOrderDetailsModel
    {
        public List<ViewOrderDetailsItem> OrderDetails { get; set; }
        public int OrderID { get; set; }
        public int TotalNumber { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPage { get; set; }
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
        public int ItemOnPage { get; set; }
        public string SearchText { get; set; }
        public string SortField { get; set; }
        public bool IsAsc { get; set; }

        //order 
        public ViewOrderItem Order { get; set; }
    }

    public class ViewOrderDetailsItem
    {
        public int OrderDetailID { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int OrderID { get; set; }
        public string OrderNumber { get; set; }
        [DisplayFormat(DataFormatString = "{0:C0}", ApplyFormatInEditMode = true)]
        public double Price { get; set; }
        public int Quantity { get; set; }
        [DisplayFormat(DataFormatString = "{0:C0}", ApplyFormatInEditMode = true)]
        public double Discount { get; set; }
        [DisplayFormat(DataFormatString = "{0:C0}", ApplyFormatInEditMode = true)]
        public double Total { get; set; }
        public int Size { get; set; }
        public bool IsFulfilled { get; set; }
        public string FullFillStr { get; set; }

        public Nullable<System.DateTime> ShipDate { get; set; }
        public Nullable<System.DateTime> PaidDate { get; set; }
    }
}