using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _170516.Models.Administrator
{
    public class ViewProductModel
    {
        public List<ViewProductItem> Products
        {
            get;
            set;
        }
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

    public class ShowcaseProductModel
    {
        public List<ShowcaseProductItem> Products { get; set; }
        public int TotalNumber { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPage { get; set; }
        public string SortField { get; set; }
        public bool IsAsc { get; set; }
    }

    public class ViewProductItem
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int QuantityInStock { get; set; }
        public string CategoryName { get; set; }
        public DateTime DateModified { get; set; }
        public string ModifiedUser { get; set; }
    }

    public class ShowcaseProductItem
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int UnitInStock { get; set; }
        public double? Discount { get; set; }
        public decimal Price { get; set; }
        public bool IsDiscount { get; set; }
        public bool IsAvailable { get; set; }
        public string ImageSrc { get; set; }
        public string ImageType { get; set; }
        public byte[] ImageByte { get; set; }
        public string Summary { get; set; }
    }
}