using _170516.Models.Administrator;
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

    public class ViewCategoryModel
    {
        public List<MenuCategoryItem> Menu { get; set; }
        public List<ShowcaseProductItem> MenuOnMainPage { get; set; }

        public int TotalNumber { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPage { get; set; }
        public int ItemOnPage { get; set; }
        public string SortField { get; set; }
        public bool IsAsc { get; set; }

        public string CategoryName { get; set; }
        public int CategoryId { get; set; }

        public List<AdvertiseProductItem> LatestItems { get; set; }
        public List<AdvertiseProductItem> TopRatedItems { get; set; }
        public List<AdvertiseProductItem> BestSellerItems { get; set; }
    }

    public class AdvertiseProductModel
    {
        public AdvertiseProductModel()
        {
            UniqueGuid = Guid.NewGuid().ToString();
        }

        public string UniqueGuid
        {
            get; set;
        }
        public string PageTitle { get; set; }
        public List<AdvertiseProductItem> Items { get; set; }
    }

    public class AdvertiseProductItem
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public string ImageSrc
        {
            get
            {
                if (string.IsNullOrEmpty(this.ImageType) && this.Image != null)
                {
                    return String.Format(Constant.ImageSourceFormat, this.ImageType, Convert.ToBase64String(this.Image));
                }

                return string.Empty;
            }
        }
        public byte[] Image { get; set; }
        public string ImageType { get; set; }
    }
}