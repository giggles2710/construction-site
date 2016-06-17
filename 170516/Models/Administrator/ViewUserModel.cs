using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _170516.Models.Administrator
{
    public class ViewUserModel
    {
        public List<ViewUserItem> Users { get; set; }
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

    public class ViewUserItem
    {
        public string AccountID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public bool IsActive { get; set; }
    }
}