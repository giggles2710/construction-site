using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _170516.Models.Administrator
{
    public class ViewEmailTemplateModel
    {
        public List<ViewEmailTemplateItem> Templates { get; set; }
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

    public class ViewEmailTemplateItem
    {
        public int EmailTemplateId { get; set; }
        public string EmailTemplateName { get; set; }
        public string CreatedByUsername { get; set; }
        public string CreatedByAccountID { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}