using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _170516.Models.Administrator
{
    public class ViewProductModel
    {
        public List<ViewProductItem> Products { get; set; }
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

    public class ViewProductItem
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int QuantityInStock { get; set; }
        public string CategoryName { get; set; }
        public DateTime DateModified { get; set; }
        public string ModifiedUser { get; set; }
    }
}