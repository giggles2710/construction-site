using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _170516.Models.Administrator
{
    public class ViewProductCategoryModel
    {
        public List<ViewProductCategoryItem> Categories { get; set; }
        public int TotalNumber { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPage { get; set; }
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
        public int StartPage { get; set; }
        public int ItemOnPage { get; set; }
        public string SearchText { get; set; }
        public string SortField { get; set; }
        public bool IsAsc { get; set; }
    }

    public class ViewProductCategoryItem
    {
        public int CategoryID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }        
    }
}