using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _170516.Models
{
    public class CategoryIndexModel
    {
        public List<CategoryProductItem> categoryProducts { get; set; }
    }

    public class CategoryProductItem
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public string ImageType { get; set; }
        public string ImageSrc { get; set; }
        public byte[] ImageBytes { get; set; }
    }
}