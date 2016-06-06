using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _170516.Models
{
    public class ProductIndexModel
    {
        // menu
        public List<MenuCategoryItem> Menu { get; set; }

        // main information
    }

    public class MenuCategoryItem
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public bool HasSubMenu { get; set; }
    }
}