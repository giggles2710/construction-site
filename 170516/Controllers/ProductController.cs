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
            for (var i = 0; i < 10; i++)
            {
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
            }

            return View(indexModel);
        }

        //public ActionResult ViewCategory(int id, int? page, int? itemsPerPage)
        //{
        //    var pageNo = page.GetValueOrDefault();
        //    var pageSize = itemsPerPage.GetValueOrDefault();
        [HttpGet]
        public ActionResult ViewCategory(int id, int? page, int? itemsPerPage, string searchText, string sortField, bool? isAsc)
        {
            var category = dbContext.Categories.FirstOrDefault(c => c.CategoryID == id);
            var model = new ViewCategoryModel();

            if (category != null)
            {
                var pageNo = page.GetValueOrDefault();
                var pageSize = itemsPerPage.GetValueOrDefault();

                if (pageNo == 0) pageNo = 1;
                if (pageSize == 0) pageSize = 10;
                if (isAsc == null) isAsc = true;
                if (string.IsNullOrEmpty(searchText)) searchText = null;
                if (string.IsNullOrEmpty(sortField)) sortField = "Latest";

                IQueryable<Product> products;

                switch (sortField)
                {
                    case "ProductName":
                        if (isAsc.GetValueOrDefault())
                            products = dbContext.Products
                                .Where(p => p.CategoryID == id && p.IsAvailable)
                                .OrderBy(p => p.Name);
                        else
                            products = dbContext.Products
                                .Where(p => p.CategoryID == id && p.IsAvailable)
                                .OrderBy(p => p.Name);
                        break;
                    case "Price":
                        if (isAsc.GetValueOrDefault())
                            products = dbContext.Products
                                .Where(p => p.CategoryID == id && p.IsAvailable)
                                .OrderBy(p => p.UnitPrice);
                        else
                            products = dbContext.Products
                                .Where(p => p.CategoryID == id && p.IsAvailable)
                                .OrderBy(p => p.UnitPrice);
                        break;
                    default:
                        if (isAsc.GetValueOrDefault())
                            products = dbContext.Products
                                .Where(p => p.CategoryID == id && p.IsAvailable)
                                .OrderBy(p => p.DateModified);
                        else
                            products = dbContext.Products
                                .Where(p => p.CategoryID == id && p.IsAvailable)
                                .OrderBy(p => p.DateModified);
                        break;
                }

                // do the query
                var productsModel = dbContext.Products
                    .Where(p => p.CategoryID == id)
                    .Select(p => new ShowcaseProductItem
                    {
                        ProductID = p.ProductID,
                        ProductName = p.Name,
                        Discount = (double)p.Discount,
                        IsAvailable = p.IsAvailable,
                        IsDiscount = p.IsDiscountAvailable,
                        Price = (double)p.UnitPrice,
                        UnitInStock = p.UnitsInStock,
                        ImageByte = p.Image,
                        ImageType = p.ImageType,
                        Summary = "Every piece meets the highest grading standards, Can be primed then painted or sealed then stained and For basic interior or exterior structural applications"
                    })
                    .OrderBy(p => p.Price)
                    .Skip(pageSize * (pageNo - 1))
                    .Take(pageSize).ToList();

                // prepare image
                for (var i = 0; i < productsModel.Count; i++)
                {
                    if (productsModel[i].ImageByte != null && productsModel[i].ImageByte.Count() > 0)
                        productsModel[i].ImageSrc = string.Format(Constant.ImageSourceFormat, productsModel[i].ImageType, Convert.ToBase64String(productsModel[i].ImageByte));
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
                model.MenuOnMainPage = productsModel;
                model.SortField = sortField;
                model.IsAsc = isAsc.GetValueOrDefault();
                model.CategoryName = category.Name;
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
    }
}