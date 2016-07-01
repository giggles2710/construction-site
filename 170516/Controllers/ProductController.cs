using _170516.Entities;
using _170516.Models;
using _170516.Models.Administrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _170516.Controllers
{
    public class ProductController : BaseController
    {
        //
        // GET: /Product/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetSubCategory(int id)
        {
            var model = new SubCategoryModel()
            {
                SubCategoryMenu = new List<MenuCategoryItem>()
            };

            model.SubCategoryMenu = dbContext.Categories
                .Where(c => c.ParentID == id && c.IsActive)
                .Select(ca => new MenuCategoryItem
                {
                    CategoryId = ca.CategoryID,
                    CategoryName = ca.Name
                })
                .ToList();

            foreach (var subCategory in model.SubCategoryMenu)
            {
                subCategory.SubCategoryList = dbContext.Categories.Where(c => c.ParentID == subCategory.CategoryId && c.IsActive).Select(ca => new MinimalCategoryItem
                {
                    CategoryId = ca.CategoryID,
                    CategoryName = ca.Name
                }).ToList();
            }

            return PartialView("_PartialSubCategory", model);
        }

        [HttpGet]
        public ActionResult Showcase()
        {
            var indexModel = new ProductIndexModel();

            // prepare menu
            indexModel.Menu = new List<MenuCategoryItem>();
            indexModel.MenuOnMainPage = new List<MenuCategoryItem>();

            // get all categories
            indexModel.Menu = dbContext.Categories.Where(c => c.ParentID == null && c.IsActive).Select(ca => new MenuCategoryItem
            {
                CategoryId = ca.CategoryID,
                CategoryName = ca.Name,
                Description = ca.Description,
                ImageByte = ca.Image,
                ImageType = ca.ImageType
            }).ToList();

            // fill up missing information
            foreach (var item in indexModel.Menu)
            {
                item.SubCategoryList = dbContext.Categories.Where(c => c.ParentID == item.CategoryId && c.IsActive).Select(ca => new MinimalCategoryItem
                {
                    CategoryId = ca.CategoryID,
                    CategoryName = ca.Name
                }).ToList();

                // image
                if (item.ImageByte != null)
                {
                    item.ImageSrc = string.Format(Constant.ImageSourceFormat, item.ImageType, Convert.ToBase64String(item.ImageByte));
                }

                if (item.SubCategoryList.Any())
                {
                    indexModel.MenuOnMainPage.Add(item);
                }
            }

            return View(indexModel);
        }

        //public ActionResult ViewCategory(int id, int? page, int? itemsPerPage)
        //{
        //    var pageNo = page.GetValueOrDefault();
        //    var pageSize = itemsPerPage.GetValueOrDefault();
        [HttpGet]
        public ActionResult ViewCategory(int id, int? page, int? itemsPerPage, string sortField)
        {
            var category = dbContext.Categories.FirstOrDefault(c => c.CategoryID == id);
            var model = new ViewCategoryModel();

            if (category != null)
            {
                var pageNo = page.GetValueOrDefault();
                var pageSize = itemsPerPage.GetValueOrDefault();

                if (pageNo == 0) pageNo = 1;
                if (pageSize == 0) pageSize = 10;
                if (string.IsNullOrEmpty(sortField)) sortField = "latest";

                IEnumerable<ShowcaseProductItem> products = dbContext.Products
                    .Where(p => p.CategoryID == id && p.IsAvailable)
                    .Select(p => new ShowcaseProductItem
                    {
                        ProductID = p.ProductID,
                        ProductName = p.Name,
                        Discount = p.Discount,
                        IsAvailable = p.IsAvailable,
                        IsDiscount = p.IsDiscountAvailable,
                        Price = p.UnitPrice,
                        UnitInStock = p.UnitsInStock,
                        ImageByte = p.Image,
                        ImageType = p.ImageType,
                        Summary = p.Summary
                    });

                List<ShowcaseProductItem> sortedProducts;

                switch (sortField)
                {
                    case "priceAsc":
                        sortedProducts = products.OrderBy(p => p.DiscountedPrice).Skip(pageSize * (pageNo - 1)).Take(pageSize).ToList();
                        break;
                    case "priceDesc":
                        sortedProducts = products.OrderByDescending(p => p.DiscountedPrice).Skip(pageSize * (pageNo - 1)).Take(pageSize).ToList();
                        break;
                    case "nameAsc":
                        sortedProducts = products
                                .OrderBy(p => p.ProductName).Skip(pageSize * (pageNo - 1)).Take(pageSize).ToList();
                        break;
                    case "nameDesc":
                        sortedProducts = products
                                .OrderByDescending(p => p.ProductName).Skip(pageSize * (pageNo - 1)).Take(pageSize).ToList();
                        break;
                    default:
                        sortedProducts = products
                                .OrderByDescending(p => p.DateModified)
                                .Skip(pageSize * (pageNo - 1)).Take(pageSize).ToList();
                        break;
                }

                // prepare image
                for (var i = 0; i < sortedProducts.Count; i++)
                {
                    if (sortedProducts[i].ImageByte != null && sortedProducts[i].ImageByte.Count() > 0)
                        sortedProducts[i].ImageSrc = string.Format(Constant.ImageSourceFormat, sortedProducts[i].ImageType, Convert.ToBase64String(sortedProducts[i].ImageByte));
                }

                // prepare menu
                model.Menu = dbContext.Categories.Where(c => c.ParentID == category.ParentID && c.IsActive).Select(a => new MenuCategoryItem
                {
                    CategoryId = a.CategoryID,
                    CategoryName = a.Name
                }).ToList();

                // prepare model                
                model.CurrentPage = pageNo;
                model.ItemOnPage = pageSize;
                model.TotalNumber = products.Count();
                model.TotalPage = (int)Math.Ceiling((double)model.TotalNumber / pageSize);
                model.MenuOnMainPage = sortedProducts;
                model.SortField = sortField;
                model.CategoryName = category.Name;
                model.CategoryId = category.CategoryID;
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult ViewProductDetail(int id)
        {
            var product = dbContext.Products.FirstOrDefault(p => p.ProductID == id);

            if (product != null)
            {
                var model = new ShowcaseProductDetail();
                model.ProductID = product.ProductID;
                model.ProductName = product.Name;
                model.ProductDescription = product.Description;
                model.ProductSummary = product.Summary;
                model.Price = (double)product.UnitPrice;
                model.Discount = (double)product.Discount;

                // image
                if (product.Image != null)
                {
                    model.ImageSrc =
                        string.Format(Constant.ImageSourceFormat, product.ImageType, Convert.ToBase64String(product.Image));
                }

                // specification
                if (product.ProductDetails.Any())
                {
                    var i = 0;
                    model.SpecificationList = new List<ShowcaseProductSpecification>();

                    foreach (var detail in product.ProductDetails)
                    {
                        var specification = new ShowcaseProductSpecification();
                        specification.Index = ++i;
                        specification.Name = detail.Name;
                        specification.Value = detail.Value;
                        specification.IsSize = Constant.SpecDimensionCode == detail.Type;

                        model.SpecificationList.Add(specification);
                    }
                }

                return View(model);
            }

            return View();
        }

        [HttpGet]
        public ActionResult ViewCart()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ViewCartMinimal()
        {
            return PartialView("_PartialCart");
        }

        [HttpGet]
        public ActionResult Checkout()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetLatestProductInCategory(int id)
        {
            var model = new AdvertiseProductModel()
            {
                Items = new List<AdvertiseProductItem>()
            };

            var category = dbContext.Categories.FirstOrDefault(c => c.CategoryID == id);

            if (category != null && category.Products.Any())
            {
                model.PageTitle = "Mới Nhất";
                model.Items = category.Products.OrderByDescending(p => p.DateModified).Take(5).Select(p => new AdvertiseProductItem
                {
                    Id = p.ProductID,
                    Image = p.Image,
                    ImageType = p.ImageType,
                    Name = p.Name
                }).ToList();
            }

            return PartialView("_PartialAdvertiseProduct", model);
        }

        [HttpGet]
        public ActionResult GetTopRatedProductInCategory(int id)
        {
            var model = new AdvertiseProductModel()
            {
                Items = new List<AdvertiseProductItem>()
            };

            var category = dbContext.Categories.FirstOrDefault(c => c.CategoryID == id);

            if (category != null && category.Products.Any())
            {
                model.PageTitle = "Đánh Giá Cao Nhất";
                model.Items = category.Products.OrderByDescending(p => p.Rate).Take(5).Select(p => new AdvertiseProductItem
                {
                    Id = p.ProductID,
                    Image = p.Image,
                    ImageType = p.ImageType,
                    Name = p.Name
                }).ToList();
            }

            return PartialView("_PartialAdvertiseProduct", model);
        }

        [HttpGet]
        public ActionResult GetBestSellerProductInCategory(int id)
        {
            var model = new AdvertiseProductModel()
            {
                Items = new List<AdvertiseProductItem>()
            };

            var category = dbContext.Categories.FirstOrDefault(c => c.CategoryID == id);

            if (category != null && category.Products.Any())
            {
                model.PageTitle = "Bán Chạy";
                model.Items = category.Products.OrderByDescending(p => p.OrderCount).Take(5).Select(p => new AdvertiseProductItem
                {
                    Id = p.ProductID,
                    Image = p.Image,
                    ImageType = p.ImageType,
                    Name = p.Name
                }).ToList();
            }

            return PartialView("_PartialAdvertiseProduct", model);
        }

        [HttpGet]
        public ActionResult GetSimilarProductPartial(int id)
        {
            var model = new AdvertiseProductModel
            {
                Items = new List<AdvertiseProductItem>()
            };

            var product = dbContext.Products.FirstOrDefault(p => p.ProductID == id);

            if (product != null && product.Category != null)
            {
                model.PageTitle = "Sản Phẩm Tương Tự";
                model.Items = dbContext.Products.Where(p => p.CategoryID == product.CategoryID)
                    .OrderByDescending(p => p.Rate)
                    .Take(4).Select(p => new AdvertiseProductItem
                    {
                        Id = p.ProductID,
                        Image = p.Image,
                        ImageType = p.ImageType,
                        Name = p.Name,
                        Rate = p.Rate,
                        Price = p.UnitPrice,
                        Discount = p.Discount
                    }).ToList();
            }

            return PartialView("_PartialGetSimiliarProduct", model);
        }
    }
}