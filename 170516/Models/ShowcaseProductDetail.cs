using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _170516.Models
{
    public class ShowcaseProductDetail
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string ImageSrc { get; set; }
        public byte[] ImageByte { get; set; }
        public string ImageType { get; set; }
        public string ProductDescription { get; set; }
        public string ProductSummary { get; set; }
        public double Price { get; set; }
        public double DiscountedPrice
        {
            get
            {
                return Price * (100 - Discount) / 100;
            }
        }
        public double Discount { get; set; }

        public List<ShowcaseProductSpecification> SpecificationList { get; set; }
    }

    public class ShowcaseProductSpecification
    {
        public int Index { get; set; }
        public string Value { get; set; }
        public string Name { get; set; }
        public bool IsSize { get; set; }
    }
}