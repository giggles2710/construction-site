using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _170516.Models
{
    public class ProductMenuItem
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public bool HasSubMenu { get; set; }
    }
}