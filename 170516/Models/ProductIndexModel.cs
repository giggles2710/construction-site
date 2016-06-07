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
        public List<MenuCategoryItem> MenuOnMainPage { get; set; }

        // main information
    }

    public class MenuCategoryItem
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public string ImageSrc { get; set; }
        public string ImageType { get; set; }
        public byte[] ImageByte { get; set; }
        public List<MinimalCategoryItem> SubCategoryList { get; set; }
    }

    public class MinimalCategoryItem
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}