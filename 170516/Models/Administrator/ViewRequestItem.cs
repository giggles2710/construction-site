using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _170516.Models.Administrator
{
    public class ViewRequestItem
    {
        public int RequestId { get; set; }
        public string FullName { get; set; }
        public string Content { get; set; }
        public DateTime DateCreated { get; set; }
        public string ReplyUser { get; set; }
        public bool IsNew { get; set; }
    }

    public class ViewRequestModel
    {
        public List<ViewRequestItem> Requests { get; set; }
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

    public class DetailRequestModel
    {
        public int RequestID { get; set; }
        public string FullName { get; set; }
        public string EmailAddress { get; set; }
        public string Content { get; set; }
        public string ReplyUser { get; set; }
        public string Reply { get; set; }
        public string DateCreated { get; set; }
    }

    public class AnswerRequestModel
    {
        public int RequestID { get; set; }
        public string ReplyContent { get; set; }
    }
}