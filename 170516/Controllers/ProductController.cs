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
        public ActionResult ViewCategory(int id, int? page, int? itemsPerPage)
        {
            var pageNo = page.GetValueOrDefault();
            var pageSize = itemsPerPage.GetValueOrDefault();

            if (pageNo == 0) pageNo = 1;
            if (pageSize == 0) pageSize = 10;

            IQueryable<Product> products;

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
                    ImageType = p.ImageType
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

            // prepare model
            var model = new ShowcaseProductModel();
            model.CurrentPage = pageNo;
            model.TotalNumber = productsModel.Count();
            model.TotalPage = (int)Math.Ceiling((double)model.TotalNumber / pageSize);
            model.Products = productsModel;

            //ViewBag.Title = ;

            return PartialView("_PartialViewCategory", model);
        }
    }
}