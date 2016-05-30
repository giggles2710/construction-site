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
    public class AdministratorController : BaseController
    {
        // GET: /Administrator/
        public ActionResult Index()
        {
            return View("Index", "_AdminLayout");
        }

        public ActionResult AccountMenu()
        {
            var accountModel = new AccountModel()
            {
                Username = "Thuan Nguyen",
                UserId = 1
            };

            return PartialView("_PartialAccount", accountModel);
        }

        [HttpGet]
        public ActionResult ViewProductCategory()
        {
            return View("ViewProductCategory", "_AdminLayout");
        }

        [HttpGet]
        public ActionResult AddProductCategory()
        {
            return PartialView("_AddProductCategoryPartial");
        }

        [HttpGet]
        public ActionResult ViewProduct(int? page, int? itemsPerPage, string searchText, string sortField, bool? isAsc)
        {
            var pageNo = 0;
            var pageSize = 0;

            if (page == null) pageNo = 1;
            if (itemsPerPage == null) pageSize = 10;
            if (isAsc == null) isAsc = true;

            //IQueryable<Product> products = dbContext.Products
            //    .Where(p => string.IsNullOrWhiteSpace(searchText) || searchText.Equals(p.Name));

            var temp = dbContext.Products;

            
            IQueryable<Product> products = dbContext.Products
                .Where(p => p.IsAvailable);

            switch (sortField)
            {
                case "QuantityInStock":
                    if (isAsc.GetValueOrDefault())
                        products = products.OrderBy(p => p.UnitsInStock);
                    else
                        products = products.OrderByDescending(p => p.UnitsInStock);
                    break;
                case "CategoryName":
                    if (isAsc.GetValueOrDefault())
                        products = products.OrderBy(p => p.Category.Name);
                    else
                        products = products.OrderByDescending(p => p.Category.Name);
                    break;
                case "DateModified":
                    if (isAsc.GetValueOrDefault())
                        products = products.OrderBy(p => p.DateModified);
                    else
                        products = products.OrderByDescending(p => p.DateModified);
                    break;
                case "ModifiedUser":
                    if (isAsc.GetValueOrDefault())
                        products = dbContext.Products.OrderBy(p => p.CreatedUserID);
                    else
                        products = dbContext.Products.OrderByDescending(p => p.CreatedUserID);
                    break;
                default:
                    if (isAsc.GetValueOrDefault())
                        products = dbContext.Products.OrderBy(p => p.Name);
                    else
                        products = dbContext.Products.OrderByDescending(p => p.Name);
                    break;
            }

            // do the query
            var productsModel = products
                .Select(p => new ViewProductItem
                {
                    ProductID = p.ProductID,
                    ProductName = p.Name,
                    QuantityInStock = p.UnitsInStock,
                    CategoryName = p.Category != null ? p.Category.Name : string.Empty,
                    DateModified = p.DateModified,
                    ModifiedUser = "0"
                })
                .Skip(pageSize * pageNo)
                .Take(pageSize).ToList();

            var model = new ViewProductModel();
            model.CurrentPage = pageNo;
            model.SearchText = searchText;
            model.ItemOnPage = pageSize;
            model.StartIndex = pageSize * pageNo - pageSize + 1;
            model.EndIndex = model.StartIndex + pageSize;
            model.TotalNumber = dbContext.Products.Count();
            model.TotalPage = (int)Math.Ceiling((double)model.TotalNumber / pageSize);
            model.Products = productsModel;

            return View("ViewProduct", "_AdminLayout", model);
        }

        [HttpGet]
        public ActionResult AddProduct()
        {
            var model = new CreateProductModel();

            // get all category items
            model.CategoryList = dbContext.Categories
                .Where(c => c.IsActive)
                .Select(c => new CreateProductCategoryListItem
            {
                CategoryID = c.CategoryID,
                CategoryName = c.Name
            }).ToList();

            // get all existing supplier
            model.SupplierList = dbContext.Suppliers.Select(s => new CreateProductSupplierListItem
            {
                SupplierID = s.SupplierID,
                SupplierName = s.CompanyName
            }).ToList();

            return View("AddProduct", "_AdminLayout", model);
        }

        [HttpPost]
        public JsonResult AddProduct(CreateProductModel model)
        {
            var product = new Product
            {
                DateModified = DateTime.Now,
                Description = model.ProductDescription,
                Discount = model.ProductDiscount,
                IsAvailable = true,
                IsDiscountAvailable = model.ProductDiscount > 0,
                Name = model.ProductName,
                Rating = 0,
                Size = model.ProductSize,
                UnitPrice = (decimal)model.ProductPrice,
                UnitsInStock = model.ProductQuantity,
                UnitsOnOrder = 0,
                UnitWeight = model.ProductWeight,
                UnitName = model.ProductUnit
            };

            // product supplier id
            if (model.ProductSupplierID == 0)
                product.SupplierID = null;
            else
                product.SupplierID = model.ProductSupplierID;

            // product category id
            if (model.ProductCategoryID == 0)
                product.CategoryID = null;
            else
                product.CategoryID = model.ProductCategoryID;

            // product image
            if (!string.IsNullOrWhiteSpace(model.ProductImage))
            {
                var imageInfos = model.ProductImage.Split(':');

                if (imageInfos.Length > 0)
                {
                    product.ImageType = imageInfos[0]; // file type
                    product.Image = Convert.FromBase64String(imageInfos[1]); // base 64 string
                }
            }

            dbContext.Products.Add(product);

            try
            {
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                return Json(new { isResult = false, result = ex.Message }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { isResult = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult AddColor()
        {
            return PartialView("_AddColorPartial");
        }

        //[HttpPost]
        //public JsonResult AddColor(CreateColorModel model)
        //{
        //    Color color = null;

        //    color = new Color
        //    {
        //        ColorName = model.ColorName,
        //        Image = Convert.FromBase64String(model.ColorBase64String),
        //        ColorDescription = model.ColorDescription,
        //        Extension = model.Extension
        //    };

        //    dbContext.Colors.Add(color);

        //    try
        //    {
        //        dbContext.SaveChanges();

        //        // update color id for model
        //        model.ColorID = color.ColorID;
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { isError = true, result = ex.Message }, JsonRequestBehavior.AllowGet);
        //    }

        //    return Json(new { isError = false, result = model }, JsonRequestBehavior.AllowGet);
        //}
    }
}