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

        //    if (pageNo == 0) pageNo = 1;
        //    if (pageSize == 0) pageSize = 10;

        //    IQueryable<Product> products;

        //    // do the query
        //    var productsModel = dbContext.Products
        //        .Select(p => new ViewProductItem
        //        {
        //            ProductID = p.ProductID,
        //            ProductName = p.Name,
        //            QuantityInStock = p.UnitsInStock,
        //            CategoryName = p.Category != null ? p.Category.Name : Constant.ProductEmptyCategory,
        //            DateModified = p.DateModified,
        //            ModifiedUser = p.CreatedUserID
        //        })
        //        .Skip(pageSize * (pageNo - 1))
        //        .Take(pageSize).ToList();
        //}
    }
}