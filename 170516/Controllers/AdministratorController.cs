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
        public ActionResult ViewProductCategory(int? page, int? itemsPerPage, string searchText, string sortField, bool? isAsc)
        {
            var pageNo = 0;
            var pageSize = 0;

            if (page == null) pageNo = 1;
            if (itemsPerPage == null) pageSize = 10;
            if (isAsc == null) isAsc = true;

            // do the query
            var categoriesModel = dbContext.Categories
                .Where(c => c.IsActive)
                .Select(c => new ViewProductCategoryItem
                {
                    CategoryID = c.CategoryID,
                    Name = c.Name,
                    Description = c.Description
                })
                .OrderBy(c => c.Name)
                .Skip(pageSize * (pageNo - 1))
                .Take(pageSize).ToList();

            var model = new ViewProductCategoryModel();
            model.CurrentPage = pageNo;
            model.SearchText = searchText;
            model.ItemOnPage = pageSize;
            model.StartIndex = pageSize * pageNo - pageSize + 1;
            model.EndIndex = model.StartIndex + pageSize;
            model.TotalNumber = dbContext.Products.Count();
            model.TotalPage = (int)Math.Ceiling((double)model.TotalNumber / pageSize);
            model.Categories = categoriesModel;

            return View("ViewProductCategory", "_AdminLayout", model);
        }

        [HttpGet]
        public ActionResult AddProductCategory()
        {
            var model = new CreateProductCategoryModel();

            // get all category items
            model.CategoryList = dbContext.Categories
                .Where(c => c.IsActive)
                .Select(c => new CreateProductCategoryListItem
                {
                    CategoryID = c.CategoryID,
                    CategoryName = c.Name
                }).ToList();

            return View(model);
        }

        [HttpPost]
        public JsonResult AddProductCategory(CreateProductCategoryModel model)
        {
            var category = new Category
            {
                Name = model.Name,
                Description = model.Description,
                IsActive = true,
                DateModified = DateTime.Now,
                ParentID = model.ParentID
            };

            dbContext.Categories.Add(category);

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
        public ActionResult UpdateProductCategory(int id)
        {
            var model = new CreateProductCategoryModel();

            var category = dbContext.Categories.FirstOrDefault(c => c.CategoryID == id);

            if (category == null)
            {
                return RedirectToAction("Index");
            }

            model.CategoryID = category.CategoryID;
            model.ParentID = category.ParentID;
            model.Name = category.Name;
            model.Description = category.Description;

            // get all category items
            model.CategoryList = dbContext.Categories
                .Where(c => c.IsActive)
                .Select(c => new CreateProductCategoryListItem
                {
                    CategoryID = c.CategoryID,
                    CategoryName = c.Name
                }).ToList();

            return View(model);
        }

        [HttpPost]
        public JsonResult UpdateProductCategory(CreateProductCategoryModel model)
        {
            var category = dbContext.Categories.FirstOrDefault(c => c.CategoryID == model.CategoryID);
            category.Name = model.Name;
            category.Description = model.Description;
            category.ParentID = model.ParentID;                        

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

        public ActionResult ViewProductCategoryDetail(int id)
        {
            var category = dbContext.Categories.FirstOrDefault(c => c.CategoryID == id);
            if (category == null)
            {
                return RedirectToAction("Index");
            }

            var categoryDetail = new CreateProductCategoryModel
            {
                CategoryID = category.CategoryID,
                Name = category.Name,
                Description = category.Description,
            };

            if (category.ParentID != null)
            {
                var parentCategory = dbContext.Categories.FirstOrDefault(c => c.CategoryID == category.ParentID);
                if (parentCategory != null)
                {
                    categoryDetail.ParentCategoryName = parentCategory.Name;
                }
            }

            return View(categoryDetail);
        }

        [HttpGet]
        public ActionResult UpdateProduct(int id)
        {
            var product = dbContext.Products.FirstOrDefault(p => p.ProductID == id);

            if (product != null)
            {
                var model = new CreateProductModel
                {
                    ProductDescription = product.Description,
                    ProductDiscount = product.Discount.GetValueOrDefault(),
                    ProductPrice = (double)product.UnitPrice,
                    ProductName = product.Name,
                    ProductUnit = product.UnitName,
                    ProductQuantity = product.UnitsInStock,
                    ProductSize = product.Size,
                    ProductWeight = product.UnitWeight.GetValueOrDefault()
                };

                // category
                if (product.Category != null)
                    model.SelectedCategoryID = product.Category.CategoryID;

                // supplier 
                if (product.Supplier != null)
                    model.SelectedSupplierID = product.Supplier.SupplierID;

                return View("UpdateProduct", "_AdminLayout", model);
                }

            // should be throw error
            return View("UpdateProduct", "_AdminLayout", new DetailProductModel());
        }

        [HttpGet]
        public ActionResult ViewProductDetail(int id)
        {
            var product = dbContext.Products.FirstOrDefault(p => p.ProductID == id);

            if (product != null)
            {
                var model = new DetailProductModel
                {
                    Description = product.Description,
                    Discount = product.Discount.GetValueOrDefault(),
                    Price = (double)product.UnitPrice,
                    ProductName = product.Name,
                    ProductUnit = product.UnitName,
                    QuantityUnit = product.UnitsInStock,
                    Size = product.Size,
                    Weight = product.UnitWeight.GetValueOrDefault()
                };

                // category
                if (product.Category != null)
                {
                    model.CategoryID = product.Category.CategoryID;
                    model.CategoryName = product.Category.Name;
                }

                // supplier 
                if (product.Supplier != null)
                {
                    model.SupplierID = product.Supplier.SupplierID;
                    model.SupplierName = product.Supplier.CompanyName;
                }

                // image
                if (product.Image != null && !string.IsNullOrEmpty(product.ImageType))
                {
                    model.ImageSrc = string.Format(Constant.ImageSourceFormat, product.ImageType, Convert.ToBase64String(product.Image));
                }

                return View("ViewProductDetail", "_AdminLayout", model);
            }

            // should be throw error
            return View("ViewProductDetail", "_AdminLayout", new DetailProductModel());
        }

        [HttpGet]
        public ActionResult ViewProduct(int? page, int? itemsPerPage, string searchText, string sortField, bool? isAsc)
        {
            var pageNo = page.GetValueOrDefault();
            var pageSize = itemsPerPage.GetValueOrDefault();

            if (pageNo == 0) pageNo = 1;
            if (pageSize == 0) pageSize = 10;
            if (isAsc == null) isAsc = true;
            if (string.IsNullOrEmpty(searchText)) searchText = null;
            if (string.IsNullOrEmpty(sortField)) sortField = "QuantityInStock";

            IQueryable<Product> products;

            switch (sortField)
            {
                case "QuantityInStock":
                    if (isAsc.GetValueOrDefault())
                        products = dbContext.Products
                            .Where(p => string.IsNullOrEmpty(searchText) || searchText.Equals(p.Name))
                            .OrderBy(p => p.UnitsInStock);
                    else
                        products = dbContext.Products
                            .Where(p => string.IsNullOrEmpty(searchText) || searchText.Equals(p.Name))
                            .OrderByDescending(p => p.UnitsInStock);
                    break;
                case "CategoryName":
                    if (isAsc.GetValueOrDefault())
                        products = dbContext.Products
                            .Where(p => string.IsNullOrEmpty(searchText) || searchText.Equals(p.Name))
                            .OrderBy(p => p.Category.Name);
                    else
                        products = dbContext.Products
                            .Where(p => string.IsNullOrEmpty(searchText) || searchText.Equals(p.Name))
                            .OrderByDescending(p => p.Category.Name);
                    break;
                case "DateModified":
                    if (isAsc.GetValueOrDefault())
                        products = dbContext.Products
                            .Where(p => string.IsNullOrEmpty(searchText) || searchText.Equals(p.Name))
                            .OrderBy(p => p.DateModified);
                    else
                        products = dbContext.Products
                            .Where(p => string.IsNullOrEmpty(searchText) || searchText.Equals(p.Name))
                            .OrderByDescending(p => p.DateModified);
                    break;
                case "ModifiedUser":
                    if (isAsc.GetValueOrDefault())
                        products = dbContext.Products
                            .Where(p => string.IsNullOrEmpty(searchText) || searchText.Equals(p.Name))
                            .OrderBy(p => p.CreatedUserID);
                    else
                        products = dbContext.Products
                            .Where(p => string.IsNullOrEmpty(searchText) || searchText.Equals(p.Name))
                            .OrderByDescending(p => p.CreatedUserID);
                    break;
                default:
                    if (isAsc.GetValueOrDefault())
                        products = dbContext.Products
                            .Where(p => string.IsNullOrEmpty(searchText) || searchText.Equals(p.Name))
                            .OrderBy(p => p.Name);
                    else
                        products = dbContext.Products
                            .Where(p => string.IsNullOrEmpty(searchText) || searchText.Equals(p.Name))
                            .OrderByDescending(p => p.Name);
                    break;
            }

            // do the query
            var productsModel = products
                .Select(p => new ViewProductItem
                {
                    ProductID = p.ProductID,
                    ProductName = p.Name,
                    QuantityInStock = p.UnitsInStock,
                    CategoryName = p.Category != null ? p.Category.Name : Constant.ProductEmptyCategory,
                    DateModified = p.DateModified,
                    ModifiedUser = p.CreatedUserID == 0 ? string.Empty : "0"
                })
                .Skip(pageSize * (pageNo - 1))
                .Take(pageSize).ToList();

            var model = new ViewProductModel();
            model.CurrentPage = pageNo;
            model.SearchText = searchText;
            model.ItemOnPage = pageSize;
            model.StartIndex = pageSize * pageNo - pageSize + 1;
            model.EndIndex = model.StartIndex + pageSize - 1;
            model.TotalNumber = products.Count();
            model.TotalPage = (int)Math.Ceiling((double)model.TotalNumber / pageSize);
            model.Products = productsModel;
            model.SortField = sortField;

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

        [HttpGet]
        public ActionResult ViewCustomerDetails(int id)
        {
            var customer = dbContext.Customers.FirstOrDefault(c => c.CustomerID == id);

            if (customer == null)
            {
                return RedirectToAction("Index");
            }

            var customerView = new DetailsCustomerModel
            {
                CustomerID = customer.CustomerID,
                Address = customer.Address,
                City = customer.City,
                District = customer.District,
                EmailAddress = customer.EmailAddress,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Phone = customer.Phone,
                ShipAddress = customer.ShipAddress,
                ShipCity = customer.ShipCity,
                ShipDistrict = customer.ShipDistrict,
                ShipPhone = customer.ShipPhone,
                DateEntered = customer.DateEntered
            };

            return View(customerView);
        }

        [HttpGet]
        public ActionResult UpdateCustomer(int id)
        {
            var model = new DetailsCustomerModel();

            var customer = dbContext.Customers.FirstOrDefault(c => c.CustomerID == id);

            model.CustomerID = customer.CustomerID;
            model.FirstName = customer.FirstName;
            model.LastName = customer.LastName;
            model.Address = customer.Address;
            model.City = customer.City;
            model.District = customer.District;
            model.Phone = customer.Phone;
            model.EmailAddress = customer.EmailAddress;
            model.ShipAddress = customer.ShipAddress;
            model.ShipCity = customer.ShipCity;
            model.ShipDistrict = customer.ShipDistrict;
            model.ShipPhone = customer.ShipPhone;

            if (customer == null)
            {
                return RedirectToAction("Index");
            }
            
            return View(model);
        }

        [HttpPost]
        public JsonResult UpdateCustomer(DetailsCustomerModel model)
        {
            var customer = dbContext.Customers.FirstOrDefault(c => c.CustomerID == model.CustomerID);

            customer.FirstName = model.FirstName;
            customer.LastName = model.LastName;
            customer.Address = model.Address;
            customer.City = model.City;
            customer.District = model.District;
            customer.Phone = model.Phone;
            customer.EmailAddress = model.EmailAddress;
            customer.ShipAddress = model.ShipAddress;
            customer.ShipCity = model.ShipCity;
            customer.ShipDistrict = model.ShipDistrict;
            customer.ShipPhone = model.ShipPhone;            

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

    }
}