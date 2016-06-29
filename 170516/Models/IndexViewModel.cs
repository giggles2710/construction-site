using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _170516.Models
{
    public class IndexViewModel
    {
        public List<ProductThumbnailViewModel> LatestProducts { get; set; }
        public List<ProductThumbnailViewModel> BestSellerProducts { get; set; }
        public List<ProductThumbnailViewModel> TopRatedProducts { get; set; }
    }

    public class ProductThumbnailViewModel
    {
        public byte[] Image { get; set; }
        public string ImageType { get; set; }
        public string ImageSrc
        {
            get
            {
                if (Image != null && !string.IsNullOrWhiteSpace(ImageType))
                {
                    var src = string.Format(Constant.ImageSourceFormat, this.ImageType, Convert.ToBase64String(this.Image));
                    return src;
                }

                return string.Empty;
            }
        }
        public string Name { get; set; }
        public string Summary { get; set; }
        public decimal DiscountedPrice
        {
            get
            {
                return Price - (Price * (decimal)Discount.GetValueOrDefault() / 100);
            }
        }
        public decimal Price { get; set; }
        public double? Discount { get; set; }
        public int ProductId { get; set; }
        public int? Rate { get; set; }
        public int? View { get; set; }

    }
}