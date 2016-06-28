using _170516.Entities;
using _170516.Models;
using _170516.Models.Administrator;
using _170516.Utility;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _170516.Controllers
{
    public class AdministratorController : BaseController
    {
        // GET: /Administrator/
        [Authorize]
        public ActionResult Index()
        {
            return View("Index", "_AdminLayout");
        }

        [Authorize]
        public ActionResult AccountMenu()
        {
            var accountModel = new AccountModel()
            {
                Username = "Thuan Nguyen",
                UserId = "abcdwef"
            };

            return PartialView("_PartialAccount", accountModel);
        }

        [HttpGet]
        [Authorize]
        public ActionResult ViewProductCategory(int? page, int? itemsPerPage, string searchText, string sortField, bool? isAsc)
        {
            var pageNo = page.GetValueOrDefault();
            var pageSize = itemsPerPage.GetValueOrDefault();

            if (page == null) pageNo = 1;
            if (itemsPerPage == null) pageSize = 10;
            if (isAsc == null) isAsc = true;
            if (string.IsNullOrEmpty(searchText)) searchText = null;
            if (string.IsNullOrEmpty(sortField)) sortField = "CategoryName";

            IQueryable<Category> categories;

            switch (sortField)
            {
                case "CategoryName":
                    if (isAsc.GetValueOrDefault())
                        categories = dbContext.Categories
                            .Where(p => p.IsActive && (string.IsNullOrEmpty(searchText) || searchText.Equals(p.Name)))
                            .OrderBy(p => p.Name);
                    else
                        categories = dbContext.Categories
                          .Where(p => p.IsActive && (string.IsNullOrEmpty(searchText) || searchText.Equals(p.Name)))
                            .OrderByDescending(p => p.Name);
                    break;
                case "CategoryDescription":
                    if (isAsc.GetValueOrDefault())
                        categories = dbContext.Categories
                          .Where(p => p.IsActive && (string.IsNullOrEmpty(searchText) || searchText.Equals(p.Name)))
                            .OrderBy(p => p.Description);
                    else
                        categories = dbContext.Categories
                          .Where(p => p.IsActive && (string.IsNullOrEmpty(searchText) || searchText.Equals(p.Name)))
                            .OrderByDescending(p => p.Description);
                    break;
                case "DateModified":
                    if (isAsc.GetValueOrDefault())
                        categories = dbContext.Categories
                          .Where(p => p.IsActive && (string.IsNullOrEmpty(searchText) || searchText.Equals(p.Name)))
                            .OrderBy(p => p.DateModified);
                    else
                        categories = dbContext.Categories
                          .Where(p => p.IsActive && (string.IsNullOrEmpty(searchText) || searchText.Equals(p.Name)))
                            .OrderByDescending(p => p.DateModified);
                    break;
                case "CreatedUser":
                    if (isAsc.GetValueOrDefault())
                        categories = dbContext.Categories
                            .Where(p => p.IsActive && (string.IsNullOrEmpty(searchText) || searchText.Equals(p.Name)))
                            .OrderBy(p => p.CreatedUserID);
                    else
                        categories = dbContext.Categories
                           .Where(p => p.IsActive && (string.IsNullOrEmpty(searchText) || searchText.Equals(p.Name)))
                            .OrderByDescending(p => p.CreatedUserID);
                    break;
                default:
                    if (isAsc.GetValueOrDefault())
                        categories = dbContext.Categories
                           .Where(p => p.IsActive && (string.IsNullOrEmpty(searchText) || searchText.Equals(p.Name)))
                            .OrderBy(p => p.Name);
                    else
                        categories = dbContext.Categories
                            .Where(p => p.IsActive && (string.IsNullOrEmpty(searchText) || searchText.Equals(p.Name)))
                            .OrderByDescending(p => p.Name);
                    break;
            }

            // do the query
            var categoriesModel = categories
                .Select(c => new ViewProductCategoryItem
                {
                    CategoryID = c.CategoryID,
                    Name = c.Name,
                    Description = c.Description,
                    DateModified = c.DateModified,
                    CreatedUserID = c.CreatedUserID == 0 ? string.Empty : "0"
                })
                .Skip(pageSize * (pageNo - 1))
                .Take(pageSize).ToList();

            var model = new ViewProductCategoryModel();
            model.CurrentPage = pageNo;
            model.SearchText = searchText;
            model.ItemOnPage = pageSize;
            model.StartIndex = pageSize * pageNo - pageSize + 1;
            model.EndIndex = model.StartIndex + pageSize - 1;
            model.TotalNumber = categories.Count();
            model.TotalPage = (int)Math.Ceiling((double)model.TotalNumber / pageSize);
            model.Categories = categoriesModel;
            model.SortField = sortField;
            model.IsAsc = isAsc.GetValueOrDefault();

            return View("ViewProductCategory", "_AdminLayout", model);
        }

        [HttpGet]
        [Authorize]
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
        [Authorize]
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

            // image
            if (!string.IsNullOrWhiteSpace(model.CategoryImage))
            {
                var imageInfos = model.CategoryImage.Split(':');

                if (imageInfos.Length > 0)
                {
                    category.ImageType = imageInfos[0]; // file type
                    category.Image = Convert.FromBase64String(imageInfos[1]); // base 64 string
                }
            }

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
        [Authorize]
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

            // image
            if (category.Image != null && !string.IsNullOrEmpty(category.ImageType))
            {
                model.CategoryImage = string.Format(Constant.ImageSourceFormat, category.ImageType, Convert.ToBase64String(category.Image));
            }

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
        [Authorize]
        public JsonResult UpdateProductCategory(CreateProductCategoryModel model)
        {
            var category = dbContext.Categories.FirstOrDefault(c => c.CategoryID == model.CategoryID);
            category.Name = model.Name;
            category.Description = model.Description;
            category.ParentID = model.ParentID;

            // product image
            if (!string.IsNullOrWhiteSpace(model.CategoryImage))
            {
                var imageInfos = model.CategoryImage.Split(':');

                if (imageInfos.Length > 0)
                {
                    category.ImageType = imageInfos[0]; // file type
                    category.Image = Convert.FromBase64String(imageInfos[1]); // base 64 string
                }
            }

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

        [Authorize]
        public ActionResult ViewProductCategoryDetail(int id)
        {
            var category = dbContext.Categories.FirstOrDefault(c => c.CategoryID == id);
            if (category == null)
            {
                return RedirectToAction("Index");
            }

            var categoryDetail = new ViewProductCategoryItem
            {
                CategoryID = category.CategoryID,
                Name = category.Name,
                Description = category.Description,
                DateModified = category.DateModified,
                CreatedUserID = category.CreatedUserID == 0 ? string.Empty : "0"

            };

            if (category.ParentID != null)
            {
                var parentCategory = dbContext.Categories.FirstOrDefault(c => c.CategoryID == category.ParentID);
                if (parentCategory != null)
                {
                    categoryDetail.ParentCategoryName = parentCategory.Name;
                }
            }

            // image
            if (category.Image != null && !string.IsNullOrEmpty(category.ImageType))
            {
                categoryDetail.CategoryImage = string.Format(Constant.ImageSourceFormat, category.ImageType, Convert.ToBase64String(category.Image));
            }



            return View(categoryDetail);
        }

        [HttpGet]
        [Authorize]
        public ActionResult UpdateProduct(int id)
        {
            var product = dbContext.Products.FirstOrDefault(p => p.ProductID == id);

            if (product != null)
            {
                var model = new UpdateProductModel
                {
                    ProductID = product.ProductID,
                    ProductDescription = product.Description,
                    ProductSummary = product.Summary,
                    ProductDiscount = product.Discount.GetValueOrDefault(),
                    ProductPrice = (double)product.UnitPrice,
                    ProductName = product.Name,
                    ProductQuantity = product.UnitsInStock
                };

                // category
                if (product.Category != null)
                    model.SelectedCategoryID = product.Category.CategoryID;

                // supplier 
                if (product.Supplier != null)
                    model.SelectedSupplierID = product.Supplier.SupplierID;

                // image
                if (product.Image != null && !string.IsNullOrEmpty(product.ImageType))
                {
                    model.ProductImage = string.Format(Constant.ImageSourceFormat, product.ImageType, Convert.ToBase64String(product.Image));
                }

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

                // get specification list
                model.SpecificationList = new List<SpecificationsTableModel>();

                if (product.ProductDetails.Any())
                {
                    var i = 0;
                    foreach (var productDetail in product.ProductDetails)
                    {
                        var specification = new SpecificationsTableModel();
                        specification.Id = ++i;
                        specification.Name = productDetail.Name;
                        specification.Type = SpecificationsTableModel.GetTypeNameStatic(productDetail.Type);
                        specification.Value = productDetail.Value;

                        model.SpecificationList.Add(specification);
                    }
                }

                // update session
                Session[Constant.SessionSpecification] = model.SpecificationList;

                return View("UpdateProduct", "_AdminLayout", model);
            }

            // should be throw error
            return View("UpdateProduct", "_AdminLayout", new UpdateProductModel());
        }

        [HttpPost]
        [Authorize]
        [ValidateInput(false)]
        public ActionResult UpdateProduct(UpdateProductModel model)
        {
            try
            {
                var product = dbContext.Products.FirstOrDefault(p => p.ProductID == model.ProductID);

                if (product != null)
                {
                    product.DateModified = DateTime.Now;
                    product.Description = model.ProductDescription;
                    product.Summary = model.ProductSummary;
                    product.Discount = model.ProductDiscount;
                    product.IsAvailable = true;
                    product.IsDiscountAvailable = model.ProductDiscount > 0;
                    product.Name = model.ProductName;
                    product.UnitPrice = (decimal)model.ProductPrice;
                    product.UnitsInStock = model.ProductQuantity;

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

                    dbContext.Entry(product).State = EntityState.Modified;
                    dbContext.SaveChanges();
                }
                else
                {
                    return Json(new { isResult = false, result = Constant.ProductNotFound }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }

                return Json(new { isResult = false, result = Constant.ErrorOccur }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { isResult = false, result = Constant.ErrorOccur }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { isResult = true, result = Constant.ErrorOccur }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Authorize]
        public ActionResult ViewProductDetail(int id)
        {
            var product = dbContext.Products.FirstOrDefault(p => p.ProductID == id);

            if (product != null)
            {
                var model = new DetailProductModel
                {
                    Description = product.Description,
                    Summary = product.Summary,
                    Discount = product.Discount.GetValueOrDefault(),
                    Price = (double)product.UnitPrice,
                    ProductName = product.Name,
                    QuantityUnit = product.UnitsInStock,
                    DateModified = product.DateModified
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

                // user modified
                if (product.Account != null)
                {
                    model.UserModified = string.Format("{0} {1}", product.Account.LastName, product.Account.FirstName);
                }

                // specification list
                model.SpecificationList = new List<SpecificationsTableModel>();

                if (product.ProductDetails.Any())
                {
                    var i = 0;
                    foreach (var productDetail in product.ProductDetails)
                    {
                        var specification = new SpecificationsTableModel();
                        specification.Id = ++i;
                        specification.Name = productDetail.Name;
                        specification.Type = SpecificationsTableModel.GetTypeNameStatic(productDetail.Type);
                        specification.Value = productDetail.Value;

                        model.SpecificationList.Add(specification);
                    }
                }

                return View("ViewProductDetail", "_AdminLayout", model);
            }

            // should be throw error
            return View("ViewProductDetail", "_AdminLayout", new DetailProductModel());
        }

        [HttpGet]
        [Authorize]
        public ActionResult ViewProduct(int? page, int? itemsPerPage, string searchText, string sortField, bool? isAsc)
        {
            var pageNo = page.GetValueOrDefault();
            var pageSize = itemsPerPage.GetValueOrDefault();

            if (pageNo == 0) pageNo = 1;
            if (pageSize == 0) pageSize = 10;
            if (isAsc == null) isAsc = true;
            if (string.IsNullOrWhiteSpace(searchText)) searchText = null;
            if (string.IsNullOrWhiteSpace(sortField)) sortField = "ProductName";

            IQueryable<Product> products;

            switch (sortField)
            {
                case "QuantityInStock":
                    if (isAsc.GetValueOrDefault())
                        products = dbContext.Products
                            .Where(p => string.IsNullOrEmpty(searchText) || p.Name.Contains(searchText))
                            .OrderBy(p => p.UnitsInStock);
                    else
                        products = dbContext.Products
                            .Where(p => string.IsNullOrEmpty(searchText) || p.Name.Contains(searchText))
                            .OrderByDescending(p => p.UnitsInStock);
                    break;
                case "CategoryName":
                    if (isAsc.GetValueOrDefault())
                        products = dbContext.Products
                            .Where(p => string.IsNullOrEmpty(searchText) || p.Name.Contains(searchText))
                            .OrderBy(p => p.Category.Name);
                    else
                        products = dbContext.Products
                            .Where(p => string.IsNullOrEmpty(searchText) || p.Name.Contains(searchText))
                            .OrderByDescending(p => p.Category.Name);
                    break;
                case "DateModified":
                    if (isAsc.GetValueOrDefault())
                        products = dbContext.Products
                            .Where(p => string.IsNullOrEmpty(searchText) || p.Name.Contains(searchText))
                            .OrderBy(p => p.DateModified);
                    else
                        products = dbContext.Products
                            .Where(p => string.IsNullOrEmpty(searchText) || p.Name.Contains(searchText))
                            .OrderByDescending(p => p.DateModified);
                    break;
                case "ModifiedUser":
                    if (isAsc.GetValueOrDefault())
                        products = dbContext.Products
                            .Where(p => string.IsNullOrEmpty(searchText) || p.Name.Contains(searchText))
                            .OrderBy(p => p.CreatedUserID);
                    else
                        products = dbContext.Products
                            .Where(p => string.IsNullOrEmpty(searchText) || p.Name.Contains(searchText))
                            .OrderByDescending(p => p.CreatedUserID);
                    break;
                default:
                    if (isAsc.GetValueOrDefault())
                        products = dbContext.Products
                            .Where(p => string.IsNullOrEmpty(searchText) || p.Name.Contains(searchText))
                            .OrderBy(p => p.Name);
                    else
                        products = dbContext.Products
                            .Where(p => string.IsNullOrEmpty(searchText) || p.Name.Contains(searchText))
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
                    ModifiedUser = p.CreatedUserID
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
            model.IsAsc = isAsc.GetValueOrDefault();

            return View("ViewProduct", "_AdminLayout", model);
        }

        [HttpGet]
        [Authorize]
        public ActionResult AddProduct()
        {
            var model = new CreateProductModel();

            // get all category items
            model.CategoryList = dbContext.Categories
                .Where(c => c.IsActive && !c.Category1.Any())
                .Select(c => new CreateProductCategoryListItem
                {
                    CategoryID = c.CategoryID,
                    CategoryName = c.Name,
                    ParentID = c.ParentID == null ? 0 : (int)c.ParentID
                }).OrderBy(c => c.ParentID).ToList();

            // get all existing supplier
            model.SupplierList = dbContext.Suppliers.Select(s => new CreateProductSupplierListItem
            {
                SupplierID = s.SupplierID,
                SupplierName = s.CompanyName
            }).ToList();

            // reset specification type session
            Session[Constant.SessionSpecification] = null;

            return View("AddProduct", "_AdminLayout", model);
        }

        [HttpPost]
        [Authorize]
        [ValidateInput(false)]
        public JsonResult AddProduct(CreateProductModel model)
        {
            var product = new Product
            {
                DateModified = DateTime.Now,
                Description = model.ProductDescription,
                Summary = model.ProductSummary,
                Discount = model.ProductDiscount,
                IsAvailable = true,
                IsDiscountAvailable = model.ProductDiscount > 0,
                Name = model.ProductName,
                UnitPrice = (decimal)model.ProductPrice,
                UnitsInStock = model.ProductQuantity
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

            // product specification
            if (model.SpecificationList != null && model.SpecificationList.Any())
            {
                foreach (var specification in model.SpecificationList)
                {
                    var productDetail = new ProductDetail
                    {
                        Name = specification.Name,
                        Type = specification.GetTypeCode(),
                        Value = specification.Value
                    };

                    product.ProductDetails.Add(productDetail);
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
        [Authorize]
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
        [Authorize]
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
                FullName = customer.Fullname,
                EmailAddress = customer.EmailAddress,
                Phone = customer.Phone,
                ShipAddress = customer.ShipAddress,
                ShipCity = customer.ShipCity,
                ShipDistrict = customer.ShipDistrict,
                ShipPhone = customer.ShipPhone,
                DateEntered = customer.DateEntered,
                AdditionalInformation = customer.AdditionalInformation
            };

            return View(customerView);
        }

        [HttpGet]
        [Authorize]
        public ActionResult UpdateCustomer(int id)
        {
            var model = new DetailsCustomerModel();

            var customer = dbContext.Customers.FirstOrDefault(c => c.CustomerID == id);

            model.CustomerID = customer.CustomerID;
            model.FullName = customer.Fullname;
            model.Address = customer.Address;
            model.City = customer.City;
            model.District = customer.District;
            model.Phone = customer.Phone;
            model.EmailAddress = customer.EmailAddress;
            model.ShipAddress = customer.ShipAddress;
            model.ShipCity = customer.ShipCity;
            model.ShipDistrict = customer.ShipDistrict;
            model.ShipPhone = customer.ShipPhone;
            model.AdditionalInformation = customer.AdditionalInformation;

            if (customer == null)
            {
                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public JsonResult UpdateCustomer(DetailsCustomerModel model)
        {
            var customer = dbContext.Customers.FirstOrDefault(c => c.CustomerID == model.CustomerID);

            customer.Fullname = model.FullName;
            customer.Address = model.Address;
            customer.City = model.City;
            customer.District = model.District;
            customer.Phone = model.Phone;
            customer.EmailAddress = model.EmailAddress;
            customer.ShipAddress = model.ShipAddress;
            customer.ShipCity = model.ShipCity;
            customer.ShipDistrict = model.ShipDistrict;
            customer.ShipPhone = model.ShipPhone;
            customer.AdditionalInformation = model.AdditionalInformation;

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

        [HttpPost]
        [Authorize]
        public JsonResult RemoveProduct(int id)
        {
            try
            {
                var product = dbContext.Products.FirstOrDefault(p => p.ProductID == id);

                if (product != null)
                {
                    // remove it
                    dbContext.Products.Remove(product);
                    dbContext.SaveChanges();
                }
                else
                {
                    return Json(new { isResult = false, result = Constant.ProductNotFound }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { isResult = false, result = Constant.ErrorOccur }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { isResult = true, result = string.Empty }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize]
        public JsonResult RemoveProductCategory(int id)
        {
            var category = dbContext.Categories.FirstOrDefault(c => c.CategoryID == id);

            if (category == null)
            {
                return Json(new { isResult = false, result = "Không tìm thấy Danh mục. Vui lòng thử lại sau" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                category.IsActive = false;
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

        [HttpPost]
        [Authorize]
        public ActionResult AddSpecification(SpecificationsTableModel model)
        {
            if (Session[Constant.SessionSpecification] == null)
                Session[Constant.SessionSpecification] = new List<SpecificationsTableModel>();

            var specificationTableModel = Session[Constant.SessionSpecification] as List<SpecificationsTableModel>;

            // convert from specification type code into the specification type description
            foreach (var specificationType in Constant.SpecificationType)
            {
                if (specificationType.Item1 == int.Parse(model.Type))
                {
                    model.Type = specificationType.Item2;
                    break;
                }
            }

            // refactor index
            model.Id = specificationTableModel.Count + 1;

            if (specificationTableModel != null)
            {
                specificationTableModel.Add(model);
            }

            Session[Constant.SessionSpecification] = specificationTableModel;

            return PartialView("_PartialSpecificationTable", specificationTableModel);
        }

        #region Supplier
        [Authorize]
        public ActionResult ViewSupplier(int? page, int? itemsPerPage, string searchText, string sortField, bool? isAsc)
        {
            var pageNo = page.GetValueOrDefault();
            var pageSize = itemsPerPage.GetValueOrDefault();

            if (pageNo == 0) pageNo = 1;
            if (pageSize == 0) pageSize = 10;
            if (isAsc == null) isAsc = true;
            if (string.IsNullOrEmpty(searchText)) searchText = null;
            if (string.IsNullOrEmpty(sortField)) sortField = "CompanyName";

            IQueryable<Supplier> suppliers;
            suppliers = dbContext.Suppliers
                            .Where(p => string.IsNullOrEmpty(searchText) || p.CompanyName.Contains(searchText));

            var suppliersModel = suppliers.ToList().Select(s => new ViewSupplierItem
            {
                SupplierID = s.SupplierID,
                SupplierCompanyName = s.CompanyName,
                SupplierContactName = string.Format("{0} {1}", s.ContactFName, s.ContactLName),
                Address = s.Address1,
                Email = s.EmailAddress,
                ProductType = s.ProductType
            });

            IEnumerable<ViewSupplierItem> result;

            switch (sortField)
            {
                case "ProductType":
                    if (isAsc.GetValueOrDefault())
                        result = suppliersModel.OrderBy(s => s.SupplierCompanyName);
                    else
                        result = suppliersModel.OrderByDescending(s => s.SupplierCompanyName);
                    break;
                case "SupplierContactName":
                    if (isAsc.GetValueOrDefault())
                        result = suppliersModel.OrderBy(s => s.SupplierContactName);
                    else
                        result = suppliersModel.OrderByDescending(s => s.SupplierContactName);
                    break;
                case "Address":
                    if (isAsc.GetValueOrDefault())
                        result = suppliersModel.OrderBy(s => s.Address);
                    else
                        result = suppliersModel.OrderByDescending(s => s.Address);
                    break;
                case "Email":
                    if (isAsc.GetValueOrDefault())
                        result = suppliersModel.OrderBy(s => s.Email);
                    else
                        result = suppliersModel.OrderByDescending(s => s.Email);
                    break;
                default:
                    if (isAsc.GetValueOrDefault())
                        result = suppliersModel.OrderBy(s => s.SupplierCompanyName);
                    else
                        result = suppliersModel.OrderByDescending(s => s.SupplierCompanyName);
                    break;
            }

            int totalRecord = result.Count();

            result = result.Select(s => s).Skip(pageSize * (pageNo - 1)).Take(pageSize);

            var model = new ViewSupplierModel();
            model.CurrentPage = pageNo;
            model.SearchText = searchText;
            model.ItemOnPage = pageSize;
            model.StartIndex = pageSize * pageNo - pageSize + 1;
            model.EndIndex = model.StartIndex + pageSize - 1;
            model.TotalNumber = totalRecord;
            model.TotalPage = (int)Math.Ceiling((double)model.TotalNumber / pageSize);
            model.Suppliers = result.ToList();
            model.SortField = sortField;
            model.IsAsc = isAsc.GetValueOrDefault();

            return View(model);
        }

        [Authorize]
        public ActionResult ViewSupplierDetails(int id)
        {
            var supplier = dbContext.Suppliers.FirstOrDefault(s => s.SupplierID == id);
            if (supplier == null)
            {
                return RedirectToAction("ViewSupplier");
            }

            var supplierDetailsModel = new CreateSupplierModel
            {
                SupplierID = supplier.SupplierID,
                CompanyName = supplier.CompanyName,
                ContactFName = supplier.ContactFName,
                ContactLName = supplier.ContactLName,
                Address1 = supplier.Address1,
                Address2 = supplier.Address2,
                City = supplier.City,
                Phone = supplier.Phone,
                Fax = supplier.Fax,
                EmailAddress = supplier.EmailAddress,
                Discount = supplier.Discount,
                ProductType = supplier.ProductType,
                IsDiscountAvailable = supplier.IsDiscountAvailable,
            };

            // image
            if (supplier.Logo != null && !string.IsNullOrEmpty(supplier.ImageType))
            {
                supplierDetailsModel.Logo = string.Format(Constant.ImageSourceFormat, supplier.ImageType, Convert.ToBase64String(supplier.Logo));
            }

            return View(supplierDetailsModel);
        }

        [HttpGet]
        [Authorize]
        public ActionResult AddSupplier()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public JsonResult AddSupplier(CreateSupplierModel model)
        {
            var supplier = new Supplier
            {
                CompanyName = model.CompanyName,
                ContactFName = model.ContactFName,
                ContactLName = model.ContactLName,
                Address1 = model.Address1,
                Address2 = model.Address2,
                City = model.City,
                Phone = model.Phone,
                Fax = model.Fax,
                EmailAddress = model.EmailAddress,
                Discount = model.Discount,
                ProductType = model.ProductType,
                IsDiscountAvailable = model.Discount > 0
            };


            // image
            if (!string.IsNullOrWhiteSpace(model.Logo))
            {
                var imageInfos = model.Logo.Split(':');

                if (imageInfos.Length > 0)
                {
                    supplier.ImageType = imageInfos[0]; // file type
                    supplier.Logo = Convert.FromBase64String(imageInfos[1]); // base 64 string
                }
            }

            dbContext.Suppliers.Add(supplier);

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
        [Authorize]
        public ActionResult UpdateSupplier(int id)
        {
            var supplier = dbContext.Suppliers.FirstOrDefault(s => s.SupplierID == id);

            if (supplier == null)
            {
                return RedirectToAction("ViewSupplier");
            }

            var supplierUpdateModel = new CreateSupplierModel
            {
                SupplierID = supplier.SupplierID,
                CompanyName = supplier.CompanyName,
                ContactFName = supplier.ContactFName,
                ContactLName = supplier.ContactLName,
                Address1 = supplier.Address1,
                Address2 = supplier.Address2,
                City = supplier.City,
                Phone = supplier.Phone,
                Fax = supplier.Fax,
                EmailAddress = supplier.EmailAddress,
                Discount = supplier.Discount,
                ProductType = supplier.ProductType,
                IsDiscountAvailable = supplier.IsDiscountAvailable,
            };

            // image
            if (supplier.Logo != null && !string.IsNullOrEmpty(supplier.ImageType))
            {
                supplierUpdateModel.Logo = string.Format(Constant.ImageSourceFormat, supplier.ImageType, Convert.ToBase64String(supplier.Logo));
            }

            return View(supplierUpdateModel);
        }

        [HttpPost]
        [Authorize]
        public JsonResult UpdateSupplier(CreateSupplierModel model)
        {
            var supplier = dbContext.Suppliers.FirstOrDefault(s => s.SupplierID == model.SupplierID);

            supplier.CompanyName = model.CompanyName;
            supplier.ContactFName = model.ContactFName;
            supplier.ContactLName = model.ContactLName;
            supplier.Address1 = model.Address1;
            supplier.Address2 = model.Address2;
            supplier.City = model.City;
            supplier.Phone = model.Phone;
            supplier.Fax = model.Fax;
            supplier.EmailAddress = model.EmailAddress;
            supplier.Discount = model.Discount;
            supplier.ProductType = model.ProductType;
            supplier.IsDiscountAvailable = model.Discount > 0;

            // image
            if (!string.IsNullOrWhiteSpace(model.Logo))
            {
                var imageInfos = model.Logo.Split(':');

                if (imageInfos.Length > 0)
                {
                    supplier.ImageType = imageInfos[0]; // file type
                    supplier.Logo = Convert.FromBase64String(imageInfos[1]); // base 64 string
                }
            }

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


        [HttpPost]
        [Authorize]
        public JsonResult RemoveSupplier(int id)
        {
            try
            {
                var supplier = dbContext.Suppliers.FirstOrDefault(p => p.SupplierID == id);

                if (supplier != null)
                {
                    // remove it
                    dbContext.Suppliers.Remove(supplier);
                    dbContext.SaveChanges();
                }
                else
                {
                    return Json(new { isResult = false, result = Constant.SupplierNotFound }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { isResult = false, result = Constant.ErrorOccur }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { isResult = true, result = string.Empty }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        # region Shipper

        [Authorize]
        public ActionResult ViewShipper(int? page, int? itemsPerPage, string searchText, string sortField, bool? isAsc)
        {
            var pageNo = page.GetValueOrDefault();
            var pageSize = itemsPerPage.GetValueOrDefault();

            if (pageNo == 0) pageNo = 1;
            if (pageSize == 0) pageSize = 10;
            if (isAsc == null) isAsc = true;
            if (string.IsNullOrEmpty(searchText)) searchText = null;
            if (string.IsNullOrEmpty(sortField)) sortField = "CompanyName";

            IQueryable<Shipper> shipper;
            shipper = dbContext.Shippers
                            .Where(p => string.IsNullOrEmpty(searchText) || p.CompanyName.Contains(searchText));

            switch (sortField)
            {
                case "Phone":
                    if (isAsc.GetValueOrDefault())
                        shipper = shipper.OrderBy(s => s.Phone);
                    else
                        shipper = shipper.OrderByDescending(s => s.Phone);
                    break;
                case "EmailAddress":
                    if (isAsc.GetValueOrDefault())
                        shipper = shipper.OrderBy(s => s.EmailAddress);
                    else
                        shipper = shipper.OrderByDescending(s => s.EmailAddress);
                    break;
                case "Address":
                    if (isAsc.GetValueOrDefault())
                        shipper = shipper.OrderBy(s => s.Address);
                    else
                        shipper = shipper.OrderByDescending(s => s.Address);
                    break;
                case "City":
                    if (isAsc.GetValueOrDefault())
                        shipper = shipper.OrderBy(s => s.City);
                    else
                        shipper = shipper.OrderByDescending(s => s.City);
                    break;
                case "District":
                    if (isAsc.GetValueOrDefault())
                        shipper = shipper.OrderBy(s => s.District);
                    else
                        shipper = shipper.OrderByDescending(s => s.District);
                    break;
                default:
                    if (isAsc.GetValueOrDefault())
                        shipper = shipper.OrderBy(s => s.CompanyName);
                    else
                        shipper = shipper.OrderByDescending(s => s.CompanyName);
                    break;
            }


            var shipperModel = shipper.Select(s => new ViewShipperItem
            {
                ShipperID = s.ShipperID,
                CompanyName = s.CompanyName,
                Phone = s.Phone,
                EmailAddress = s.EmailAddress,
                Address = s.Address,
                City = s.City,
                District = s.District
            }).Skip(pageSize * (pageNo - 1)).Take(pageSize).ToList();



            var model = new ViewShipperModel();
            model.CurrentPage = pageNo;
            model.SearchText = searchText;
            model.ItemOnPage = pageSize;
            model.StartIndex = pageSize * pageNo - pageSize + 1;
            model.EndIndex = model.StartIndex + pageSize - 1;
            model.TotalNumber = shipper.Count();
            model.TotalPage = (int)Math.Ceiling((double)model.TotalNumber / pageSize);
            model.Shippers = shipperModel;
            model.SortField = sortField;
            model.IsAsc = isAsc.GetValueOrDefault();

            return View(model);
        }

        [Authorize]
        public ActionResult ViewShipperDetails(int id)
        {
            var shipper = dbContext.Shippers.FirstOrDefault(s => s.ShipperID == id);
            if (shipper == null)
            {
                return RedirectToAction("ViewShipper");
            }

            var shipperDetailsModel = new CreateShipperModel
            {
                ShipperID = shipper.ShipperID,
                CompanyName = shipper.CompanyName,
                Phone = shipper.Phone,
                EmailAddress = shipper.EmailAddress,
                Address = shipper.Address,
                City = shipper.City,
                District = shipper.District
            };


            return View(shipperDetailsModel);
        }

        [HttpGet]
        [Authorize]
        public ActionResult AddShipper()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public JsonResult AddShipper(CreateShipperModel model)
        {
            var shipper = new Shipper
            {
                CompanyName = model.CompanyName,
                Phone = model.Phone,
                EmailAddress = model.EmailAddress,
                Address = model.Address,
                City = model.City,
                District = model.District
            };


            dbContext.Shippers.Add(shipper);

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
        [Authorize]
        public ActionResult UpdateShipper(int id)
        {
            var shipper = dbContext.Shippers.FirstOrDefault(s => s.ShipperID == id);

            if (shipper == null)
            {
                return RedirectToAction("ViewShipper");
            }

            var ShipperUpdateModel = new CreateShipperModel
            {
                ShipperID = shipper.ShipperID,
                CompanyName = shipper.CompanyName,
                Phone = shipper.Phone,
                EmailAddress = shipper.EmailAddress,
                Address = shipper.Address,
                City = shipper.City,
                District = shipper.District
            };


            return View(ShipperUpdateModel);
        }

        [HttpPost]
        [Authorize]
        public JsonResult UpdateShipper(CreateShipperModel model)
        {
            var shipper = dbContext.Shippers.FirstOrDefault(s => s.ShipperID == model.ShipperID);

            shipper.CompanyName = shipper.CompanyName;
            shipper.Phone = shipper.Phone;
            shipper.EmailAddress = shipper.EmailAddress;
            shipper.Address = shipper.Address;
            shipper.City = shipper.City;
            shipper.District = shipper.District;


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


        [HttpPost]
        [Authorize]
        public JsonResult RemoveShipper(int id)
        {
            try
            {
                var shipper = dbContext.Shippers.FirstOrDefault(p => p.ShipperID == id);

                if (shipper != null)
                {
                    // remove it
                    dbContext.Shippers.Remove(shipper);
                    dbContext.SaveChanges();
                }
                else
                {
                    return Json(new { isResult = false, result = Constant.ShipperNotFound }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { isResult = false, result = Constant.ErrorOccur }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { isResult = true, result = string.Empty }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Orders

        [Authorize]
        public ActionResult ViewOrder(int? page, int? itemsPerPage, string searchText, string sortField, bool? isAsc)
        {
            var pageNo = page.GetValueOrDefault();
            var pageSize = itemsPerPage.GetValueOrDefault();

            if (pageNo == 0) pageNo = 1;
            if (pageSize == 0) pageSize = 10;
            if (isAsc == null) isAsc = true;
            if (string.IsNullOrEmpty(searchText)) searchText = null;
            if (string.IsNullOrEmpty(sortField)) sortField = "OrderDate";

            List<Order> orders;
            orders = dbContext.Orders
                            .Where(p => string.IsNullOrEmpty(searchText) ||
                            (p.Customer.Fullname.Contains(searchText)))
                            .ToList();


            var ordersModel = orders.Select(o => new ViewOrderItem
            {
                OrderID = o.OrderID,
                OrderNumber = o.OrderNumber,
                OrderStatusToUser = GetOrderStatusToUser(o.OrderStatus),
                OrderStatus = o.OrderStatus,                
                IsFulfilled = o.IsFulfilled,
                IsCanceled = o.IsCanceled,
                CustomerID = o.CustomerID,
                CustomerName = o.Customer.Fullname,
                ShipperID = o.ShipperID ?? 0,
                ShipperCompanyName = o.Shipper == null ? "":o.Shipper.CompanyName,
                OrderDate = o.OrderDate,
                ShipDate = o.ShipDate,
                PaymentDate = o.PaymentDate,
                TotalIncome = o.OrderDetails.Sum(d => d.Total),
                Freight = o.Freight,
                SalesTax = o.SalesTax,
                Paid = o.Paid
            }).ToList();




            IEnumerable<ViewOrderItem> result;

            switch (sortField)
            {
                case "Customer":
                    if (isAsc.GetValueOrDefault())
                        result = ordersModel.OrderBy(s => s.CustomerName);
                    else
                        result = ordersModel.OrderByDescending(s => s.CustomerName);
                    break;
                case "Shipper":
                    if (isAsc.GetValueOrDefault())
                        result = ordersModel.OrderBy(s => s.ShipperCompanyName);
                    else
                        result = ordersModel.OrderByDescending(s => s.ShipperCompanyName);
                    break;
                case "Paid":
                    if (isAsc.GetValueOrDefault())
                        result = ordersModel.OrderBy(s => s.Paid);
                    else
                        result = ordersModel.OrderByDescending(s => s.Paid);
                    break;
                case "Freight":
                    if (isAsc.GetValueOrDefault())
                        result = ordersModel.OrderBy(s => s.Freight);
                    else
                        result = ordersModel.OrderByDescending(s => s.Freight);
                    break;
                case "SalesTax":
                    if (isAsc.GetValueOrDefault())
                        result = ordersModel.OrderBy(s => s.SalesTax);
                    else
                        result = ordersModel.OrderByDescending(s => s.SalesTax);
                    break;
                case "Total":
                    if (isAsc.GetValueOrDefault())
                        result = ordersModel.OrderBy(s => s.TotalIncome);
                    else
                        result = ordersModel.OrderByDescending(s => s.TotalIncome);
                    break;
                case "ShipDate":
                    if (isAsc.GetValueOrDefault())
                        result = ordersModel.OrderBy(s => s.ShipDate);
                    else
                        result = ordersModel.OrderByDescending(s => s.ShipDate);
                    break;
                case "PaymentDate":
                    if (isAsc.GetValueOrDefault())
                        result = ordersModel.OrderBy(s => s.PaymentDate);
                    else
                        result = ordersModel.OrderByDescending(s => s.PaymentDate);
                    break;
                case "OrderStatus":
                    if (isAsc.GetValueOrDefault())
                        result = ordersModel.OrderBy(s => s.OrderStatusToUser);
                    else
                        result = ordersModel.OrderByDescending(s => s.OrderStatusToUser);
                    break;
                default:
                    if (isAsc.GetValueOrDefault())
                        result = ordersModel.OrderBy(s => s.OrderDate);
                    else
                        result = ordersModel.OrderByDescending(s => s.OrderDate);
                    break;
            }

            int totalRecord = result.Count();

            result = result.Select(s => s).Skip(pageSize * (pageNo - 1)).Take(pageSize);

            var model = new ViewOrderModel();
            model.CurrentPage = pageNo;
            model.SearchText = searchText;
            model.ItemOnPage = pageSize;
            model.StartIndex = pageSize * pageNo - pageSize + 1;
            model.EndIndex = model.StartIndex + pageSize - 1;
            model.TotalNumber = totalRecord;
            model.TotalPage = (int)Math.Ceiling((double)model.TotalNumber / pageSize);
            model.Orders = result.ToList();
            model.SortField = sortField;
            model.IsAsc = isAsc.GetValueOrDefault();

            return View(model);
        }

        [Authorize]
        private string GetOrderStatusToUser(string orderStatus)
        {
            string result = string.Empty;
            switch (orderStatus)
            {
                case Constant.OrderCanceledStatus:
                    result = "Đơn hàng đã hủy";
                    break;
                case Constant.OrderFulfilledStatus:
                    result = "Đơn hàng đã xong";
                    break;
                case Constant.OrderIsProcessingtatus:
                    result = "Đang được xử lý";
                    break;
                case Constant.OrderDeliveredStatus:
                    result = "Đã được chuyển đến";
                    break;
                default:
                    break;
            }

            return result;
        }

        [HttpGet]
        [Authorize]
        public ActionResult UpdateOrder(int id)
        {
            var order = dbContext.Orders.FirstOrDefault(o => o.OrderID == id);

            if (order == null)
            {
                return RedirectToAction("ViewOrder");
            }

            var updateOrderModel = new UpdateOrderModel
            {
                OrderID = order.OrderID,
                OrderNumber = order.OrderNumber,
                CustomerID = order.CustomerID,
                CustomerName = order.Customer.Fullname,
                ShipperID = order.ShipperID??0,
                Freight = order.Freight,
                SalesTax = order.SalesTax,
                Paid = order.Paid,
                ShipDate = order.ShipDate,
                PaymentDate = order.PaymentDate,
                OrderDate = order.OrderDate,
                IsFulfilled = order.IsFulfilled,
                IsCanceled = order.IsCanceled,
                OrderStatus = order.OrderStatus,
                RequiredDate = order.RequiredDate,
            };

            updateOrderModel.OrderDetails = order.OrderDetails.Select(o => new ViewOrderDetailsItem
            {
                OrderDetailID = o.OrderDetailID,
                ProductID = o.ProductID,
                ProductName = o.Product.Name,
                OrderID = o.OrderID,
                OrderNumber = o.OrderNumber,
                Price = o.Price,
                Quantity = o.Quantity,
                Discount = o.Discount,
                Total = o.Total,
                Size = o.Size,
                IsFulfilled = o.IsFulfilled,
                FullFillStr = o.IsFulfilled.ToString(),
                ShipDate = o.ShipDate,
                PaidDate = o.PaidDate
            }).ToList();


            updateOrderModel.Shippers = dbContext.Shippers.Select(s => new ViewShipperItem
            {
                ShipperID = s.ShipperID,
                CompanyName = s.CompanyName
            }).ToList();

            return View(updateOrderModel);
        }

        [HttpPost]
        [Authorize]
        public JsonResult UpdateOrder(UpdateOrderModel model)
        {
            var order = dbContext.Orders.FirstOrDefault(o => o.OrderID == model.OrderID);

            order.ShipperID = model.ShipperID;
            order.Freight = model.Freight;
            order.SalesTax = model.SalesTax;
            order.Paid = model.Paid;
            order.ShipDate = model.ShipDate;
            order.RequiredDate = model.RequiredDate;
            order.PaymentDate = model.PaymentDate;
            order.OrderStatus = model.OrderStatus;

            order.IsCanceled = order.OrderStatus == Constant.OrderCanceledStatus;
            order.IsFulfilled = order.OrderStatus == Constant.OrderFulfilledStatus;
            order.ModifiedDate = DateTime.Now;

            for (int i = 0; i < order.OrderDetails.Count; i++)
            {
                order.OrderDetails.ElementAt(i).Price = model.OrderDetails[i].Price;
                order.OrderDetails.ElementAt(i).Quantity = model.OrderDetails[i].Quantity;
                order.OrderDetails.ElementAt(i).Size = model.OrderDetails[i].Size;
                order.OrderDetails.ElementAt(i).Discount = model.OrderDetails[i].Discount;
                order.OrderDetails.ElementAt(i).Total = (model.OrderDetails[i].Price * model.OrderDetails[i].Quantity) - (decimal)model.OrderDetails[i].Discount;
                order.OrderDetails.ElementAt(i).IsFulfilled = model.OrderDetails[i].IsFulfilled;
            }

            try
            {
                dbContext.SaveChanges();

                var emailToSend = EmailMergingHelper.MergeOrderConfirmationEmail(model.OrderID);
                emailToSend.SendTo = "giggles2710@gmail.com";

                if (emailToSend != null)
                {
                    bool isSuccess = EmailServiceHelper.Send(emailToSend);
                    if (!isSuccess)
                        return Json(new { isResult = false, result = "Có lỗi xảy ra khi gửi mail" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { isResult = false, result = ex.Message }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { isResult = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize]
        public JsonResult CancelOrder(int id)
        {
            var order = dbContext.Orders.FirstOrDefault(o => o.OrderID == id);

            try
            {
                if (order != null)
                {
                    // remove it
                    order.IsCanceled = true;
                    order.OrderStatus = Constant.OrderCanceledStatus;
                    dbContext.SaveChanges();
                }
                else
                {
                    return Json(new { isResult = false, result = Constant.OrderNotFound }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { isResult = false, result = Constant.ErrorOccur }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { isResult = true, result = string.Empty }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Authorize]
        public ActionResult ViewOrderDetails(int id, int? page, int? itemsPerPage, string searchText, string sortField, bool? isAsc)
        {
            if (id == 0)
            {
                return RedirectToAction("ViewOrder");
            }

            var order = dbContext.Orders.FirstOrDefault(o => o.OrderID == id);

            if (order == null)
            {
                return RedirectToAction("ViewOrder");
            }

            var pageNo = page.GetValueOrDefault();
            var pageSize = itemsPerPage.GetValueOrDefault();

            if (pageNo == 0) pageNo = 1;
            if (pageSize == 0) pageSize = 10;
            if (isAsc == null) isAsc = true;
            searchText = null;
            if (string.IsNullOrEmpty(sortField)) sortField = "OrderID";

            var orderDetailsModel = order.OrderDetails.ToList().Select(o => new ViewOrderDetailsItem
            {
                OrderDetailID = o.OrderDetailID,
                ProductID = o.ProductID,
                ProductName = o.Product.Name,
                OrderID = o.OrderID,
                OrderNumber = o.OrderNumber,
                Price = o.Price,
                Quantity = o.Quantity,
                Discount = o.Discount,
                Total = o.Total,
                Size = o.Size,
                IsFulfilled = o.IsFulfilled,
                ShipDate = o.ShipDate,
                PaidDate = o.PaidDate
            });

            IEnumerable<ViewOrderDetailsItem> result;

            switch (sortField)
            {
                case "ProductName":
                    if (isAsc.GetValueOrDefault())
                        result = orderDetailsModel.OrderBy(s => s.ProductName);
                    else
                        result = orderDetailsModel.OrderByDescending(s => s.ProductName);
                    break;
                case "Price":
                    if (isAsc.GetValueOrDefault())
                        result = orderDetailsModel.OrderBy(s => s.Price);
                    else
                        result = orderDetailsModel.OrderByDescending(s => s.Price);
                    break;
                case "Quantity":
                    if (isAsc.GetValueOrDefault())
                        result = orderDetailsModel.OrderBy(s => s.Quantity);
                    else
                        result = orderDetailsModel.OrderByDescending(s => s.Quantity);
                    break;
                case "Discount":
                    if (isAsc.GetValueOrDefault())
                        result = orderDetailsModel.OrderBy(s => s.Discount);
                    else
                        result = orderDetailsModel.OrderByDescending(s => s.Discount);
                    break;
                case "Size":
                    if (isAsc.GetValueOrDefault())
                        result = orderDetailsModel.OrderBy(s => s.Size);
                    else
                        result = orderDetailsModel.OrderByDescending(s => s.Size);
                    break;
                case "Total":
                    if (isAsc.GetValueOrDefault())
                        result = orderDetailsModel.OrderBy(s => s.Total);
                    else
                        result = orderDetailsModel.OrderByDescending(s => s.Total);
                    break;
                case "IsFulfilled":
                    if (isAsc.GetValueOrDefault())
                        result = orderDetailsModel.OrderBy(s => s.IsFulfilled);
                    else
                        result = orderDetailsModel.OrderByDescending(s => s.IsFulfilled);
                    break;
                case "PaymentDate":
                    if (isAsc.GetValueOrDefault())
                        result = orderDetailsModel.OrderBy(s => s.PaidDate);
                    else
                        result = orderDetailsModel.OrderByDescending(s => s.PaidDate);
                    break;
                case "ShipDate":
                    if (isAsc.GetValueOrDefault())
                        result = orderDetailsModel.OrderBy(s => s.ShipDate);
                    else
                        result = orderDetailsModel.OrderByDescending(s => s.ShipDate);
                    break;
                default:
                    if (isAsc.GetValueOrDefault())
                        result = orderDetailsModel.OrderBy(s => s.OrderID);
                    else
                        result = orderDetailsModel.OrderByDescending(s => s.OrderID);
                    break;
            }

            int totalRecord = result.Count();
            pageSize = totalRecord;

            result = result.Select(s => s).Skip(pageSize * (pageNo - 1)).Take(pageSize);

            var model = new ViewOrderDetailsModel();

            model.Order = new ViewOrderItem
            {
                OrderID = order.OrderID,
                OrderNumber = order.OrderNumber,
                OrderStatus = order.OrderStatus,
                OrderStatusToUser = GetOrderStatusToUser(order.OrderStatus),
                IsFulfilled = order.IsFulfilled,
                IsCanceled = order.IsCanceled,
                CustomerID = order.CustomerID,
                CustomerName = order.Customer.Fullname,
                ShipperID = order.ShipperID??0,
                ShipperCompanyName = order.Shipper == null? "" : order.Shipper.CompanyName,
                OrderDate = order.OrderDate,
                ShipDate = order.ShipDate,
                PaymentDate = order.PaymentDate,
                TotalIncome = order.OrderDetails.Sum(d => d.Total),
                Freight = order.Freight,
                SalesTax = order.SalesTax,
                Paid = order.Paid
            };

            model.OrderID = id;
            model.CurrentPage = pageNo;
            model.SearchText = searchText;
            model.ItemOnPage = pageSize;
            model.StartIndex = pageSize * pageNo - pageSize + 1;
            model.EndIndex = model.StartIndex + pageSize - 1;
            model.TotalNumber = totalRecord;
            model.TotalPage = (int)Math.Ceiling((double)model.TotalNumber / pageSize);
            model.OrderDetails = result.ToList();
            model.SortField = sortField;
            model.IsAsc = isAsc.GetValueOrDefault();

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public JsonResult CancelOrderProduct(int id)
        {
            var orderDetails = dbContext.OrderDetails.FirstOrDefault(o => o.OrderDetailID == id);

            try
            {
                if (orderDetails != null)
                {
                    // remove it
                    dbContext.OrderDetails.Remove(orderDetails);
                    dbContext.SaveChanges();
                }
                else
                {
                    return Json(new { isResult = false, result = Constant.OrderDetailsNotFound }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { isResult = false, result = Constant.ErrorOccur }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { isResult = true, result = string.Empty }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize]
        public JsonResult UpdateOrderDetails(int orderDetaisId, decimal price, int quantity, double discount, int size, bool isFulfilled)
        {
            var orderDetails = dbContext.OrderDetails.FirstOrDefault(o => o.OrderDetailID == orderDetaisId);

            try
            {
                if (orderDetails != null)
                {
                    // remove it
                    orderDetails.Price = price;
                    orderDetails.Quantity = quantity;
                    orderDetails.Discount = discount;
                    orderDetails.Size = size;
                    orderDetails.IsFulfilled = isFulfilled;
                    orderDetails.Total = price * quantity - (decimal)discount;
                    dbContext.SaveChanges();
                }
                else
                {
                    return Json(new { isResult = false, result = Constant.OrderDetailsNotFound }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { isResult = false, result = Constant.ErrorOccur }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { isResult = true, result = orderDetails.Total.ToString() }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Users

        [Authorize]
        public ActionResult ViewUser(int? page, int? itemsPerPage, string searchText, string sortField, bool? isAsc)
        {
            var pageNo = page.GetValueOrDefault();
            var pageSize = itemsPerPage.GetValueOrDefault();

            if (pageNo == 0) pageNo = 1;
            if (pageSize == 0) pageSize = 10;
            if (isAsc == null) isAsc = true;
            if (string.IsNullOrEmpty(searchText)) searchText = null;
            if (string.IsNullOrEmpty(sortField)) sortField = "Username";

            IQueryable<Account> users;

            switch (sortField)
            {
                case "Username":
                    if (isAsc.GetValueOrDefault())
                        users = dbContext.Accounts
                            .Where(p => string.IsNullOrEmpty(searchText) || p.Username.Contains(searchText))
                            .OrderBy(p => p.Username);
                    else
                        users = dbContext.Accounts
                            .Where(p => string.IsNullOrEmpty(searchText) || p.Username.Contains(searchText))
                            .OrderByDescending(p => p.Username);
                    break;
                case "FirstName":
                    if (isAsc.GetValueOrDefault())
                        users = dbContext.Accounts
                            .Where(p => string.IsNullOrEmpty(searchText) || p.Username.Contains(searchText))
                            .OrderBy(p => p.FirstName);
                    else
                        users = dbContext.Accounts
                            .Where(p => string.IsNullOrEmpty(searchText) || p.Username.Contains(searchText))
                            .OrderByDescending(p => p.Username);
                    break;
                case "LastName":
                    if (isAsc.GetValueOrDefault())
                        users = dbContext.Accounts
                            .Where(p => string.IsNullOrEmpty(searchText) || p.Username.Contains(searchText))
                            .OrderBy(p => p.LastName);
                    else
                        users = dbContext.Accounts
                            .Where(p => string.IsNullOrEmpty(searchText) || p.Username.Contains(searchText))
                            .OrderByDescending(p => p.LastName);
                    break;

                case "EmailAddress":
                    if (isAsc.GetValueOrDefault())
                        users = dbContext.Accounts
                            .Where(p => string.IsNullOrEmpty(searchText) || p.Username.Contains(searchText))
                            .OrderBy(p => p.EmailAddress);
                    else
                        users = dbContext.Accounts
                            .Where(p => string.IsNullOrEmpty(searchText) || p.Username.Contains(searchText))
                            .OrderByDescending(p => p.EmailAddress);
                    break;
                case "IsActive":
                    if (isAsc.GetValueOrDefault())
                        users = dbContext.Accounts
                            .Where(p => string.IsNullOrEmpty(searchText) || p.Username.Contains(searchText))
                            .OrderBy(p => p.IsActive);
                    else
                        users = dbContext.Accounts
                            .Where(p => string.IsNullOrEmpty(searchText) || p.Username.Contains(searchText))
                            .OrderByDescending(p => p.IsActive);
                    break;
                default:
                    if (isAsc.GetValueOrDefault())
                        users = dbContext.Accounts
                            .Where(p => string.IsNullOrEmpty(searchText) || p.Username.Contains(searchText))
                            .OrderBy(p => p.Username);
                    else
                        users = dbContext.Accounts
                            .Where(p => string.IsNullOrEmpty(searchText) || p.Username.Contains(searchText))
                            .OrderByDescending(p => p.Username);
                    break;
            }

            //do the query
            var usersModel = users.Select(u => new ViewUserItem
            {
                AccountID = u.AccountID,
                Username = u.Username,
                FirstName = u.FirstName,
                LastName = u.LastName,
                EmailAddress = u.EmailAddress,
                IsActive = u.IsActive
            }).Skip(pageSize * (pageNo - 1))
            .Take(pageSize).ToList();

            var model = new ViewUserModel();
            model.CurrentPage = pageNo;
            model.SearchText = searchText;
            model.ItemOnPage = pageSize;
            model.StartIndex = pageSize * pageNo - pageSize + 1;
            model.EndIndex = model.StartIndex + pageSize - 1;
            model.TotalNumber = users.Count();
            model.TotalPage = (int)Math.Ceiling((double)model.TotalNumber / pageSize);
            model.Users = usersModel;
            model.SortField = sortField;
            model.IsAsc = isAsc.GetValueOrDefault();

            return View(model);
        }

        [Authorize]
        public JsonResult ChangeUserStatus(string guid)
        {
            var user = dbContext.Accounts.FirstOrDefault(u => u.AccountID == guid);

            if (user == null)
            {
                return Json(new { isResult = false, result = Constant.UserNotFound }, JsonRequestBehavior.AllowGet);
            }

            user.IsActive = !user.IsActive;

            try
            {
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                return Json(new { isResult = false, result = Constant.ErrorOccur }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { isResult = true, result = string.Empty }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Authorize]
        public ActionResult AddUser()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public JsonResult AddUser(UpdateUserModel model)
        {            
            bool isExist = dbContext.Accounts.Any(u => u.AccountID != model.AccountID && u.Username == model.Username);

            if (isExist)
            {
                return Json(new { isResult = false, result = Constant.UsernameExists }, JsonRequestBehavior.AllowGet);
            }

            isExist = dbContext.Accounts.Any(u => u.AccountID != model.AccountID && u.EmailAddress == model.EmailAddress);

            if (isExist)
            {
                return Json(new { isResult = false, result = Constant.EmailExists }, JsonRequestBehavior.AllowGet);
            }

            var user = new Account();
            user.AccountID = Guid.NewGuid().ToString();
            user.IsActive = true;
            user.HashToken = UtilityHelper.RandomString(10);
            user.ModifiedDate = DateTime.Now;

            user.Username = model.Username;
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.PhoneNumber = model.PhoneNumber;
            user.EmailAddress = model.EmailAddress;
            user.Password = EncryptionHelper.HashPassword(model.Password, user.HashToken);

            //image
            if (!string.IsNullOrWhiteSpace(model.UserImage))
            {
                var imageInfos = model.UserImage.Split(':');

                if (imageInfos.Length > 0)
                {
                    user.ImageType = imageInfos[0]; // file type
                    user.Image = Convert.FromBase64String(imageInfos[1]); // base 64 string
                }
            }

            try
            {
                dbContext.Accounts.Add(user);
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                return Json(new { isResult = false, result = Constant.ErrorOccur }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { isResult = true, result = string.Empty }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Authorize]
        public ActionResult UpdateUser(string userid)
        {
            var user = dbContext.Accounts.FirstOrDefault(u => u.AccountID == userid);

            if (user == null)
            {
                return RedirectToAction("ViewUser");
            }

            var updateUserModel = new UpdateUserModel
            {
                AccountID = user.AccountID,
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                EmailAddress = user.EmailAddress,                
                PhoneNumber = user.PhoneNumber
            };

           
            // image
            if (user.Image != null && !string.IsNullOrEmpty(user.ImageType))
            {
                updateUserModel.UserImage = string.Format(Constant.ImageSourceFormat, user.ImageType, Convert.ToBase64String(user.Image));
            }

            return View(updateUserModel);
        }

        [HttpPost]
        [Authorize]
        public JsonResult UpdateUser(UpdateUserModel model)
        {
            var user = dbContext.Accounts.FirstOrDefault(u => u.AccountID == model.AccountID);

            if (user == null)
            {
                return Json(new { isResult = false, result = Constant.UserNotFound }, JsonRequestBehavior.AllowGet);
            }

            bool isExist = dbContext.Accounts.Any(u => u.AccountID != model.AccountID && u.Username == model.Username);

            if (isExist)
            {
                return Json(new { isResult = false, result = Constant.UsernameExists }, JsonRequestBehavior.AllowGet);
            }

            isExist = dbContext.Accounts.Any(u => u.AccountID != model.AccountID && u.EmailAddress == model.EmailAddress);

            if (isExist)
            {
                return Json(new { isResult = false, result = Constant.EmailExists }, JsonRequestBehavior.AllowGet);
            }

            user.Username = model.Username;
            user.Username = model.Username;
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.PhoneNumber = model.PhoneNumber;
            user.EmailAddress = model.EmailAddress;
            user.ModifiedDate = DateTime.Now;

            //password
            if (!string.IsNullOrEmpty(model.PasswordInUpdate))
            {
                user.HashToken = UtilityHelper.RandomString(10);
                user.Password = EncryptionHelper.HashPassword(model.PasswordInUpdate, user.HashToken);
            }
           
            //image
            if (!string.IsNullOrWhiteSpace(model.UserImage))
            {
                var imageInfos = model.UserImage.Split(':');

                if (imageInfos.Length > 0)
                {
                    user.ImageType = imageInfos[0]; // file type
                    user.Image = Convert.FromBase64String(imageInfos[1]); // base 64 string
                }
            }
            
            try
            {
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                return Json(new { isResult = false, result = Constant.ErrorOccur }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { isResult = true, result = string.Empty }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Request
        

        [HttpGet]
        [Authorize]
        public ActionResult ViewRequest(int? page, int? itemsPerPage, string searchText, string sortField, bool? isAsc)
        {
            var pageNo = page.GetValueOrDefault();
            var pageSize = itemsPerPage.GetValueOrDefault();

            if (pageNo == 0) pageNo = 1;
            if (pageSize == 0) pageSize = 10;
            if (isAsc == null) isAsc = false;
            if (string.IsNullOrEmpty(searchText)) searchText = null;
            if (string.IsNullOrEmpty(sortField)) sortField = "DateCreated";

            IQueryable<Request> requests;

            switch (sortField)
            {
                case "FullName":
                    if (isAsc.GetValueOrDefault())
                        requests = dbContext.Requests
                            .Where(p => string.IsNullOrEmpty(searchText) || searchText.Contains(p.FullName) || searchText.Contains(p.RequestContent) || searchText.Contains(p.ReplyUser))
                            .OrderBy(p => p.FullName);
                    else
                        requests = dbContext.Requests
                            .Where(p => string.IsNullOrEmpty(searchText) || searchText.Contains(p.FullName) || searchText.Contains(p.RequestContent) || searchText.Contains(p.ReplyUser))
                            .OrderByDescending(p => p.FullName);
                    break;
                case "RequestContent":
                    if (isAsc.GetValueOrDefault())
                        requests = dbContext.Requests
                            .Where(p => string.IsNullOrEmpty(searchText) || searchText.Contains(p.FullName) || searchText.Contains(p.RequestContent) || searchText.Contains(p.ReplyUser))
                            .OrderBy(p => p.RequestContent);
                    else
                        requests = dbContext.Requests
                            .Where(p => string.IsNullOrEmpty(searchText) || searchText.Contains(p.FullName) || searchText.Contains(p.RequestContent) || searchText.Contains(p.ReplyUser))
                            .OrderByDescending(p => p.RequestContent);
                    break;
                case "IsNew":
                    if (isAsc.GetValueOrDefault())
                        requests = dbContext.Requests
                            .Where(p => string.IsNullOrEmpty(searchText) || searchText.Contains(p.FullName) || searchText.Contains(p.RequestContent) || searchText.Contains(p.ReplyUser))
                            .OrderBy(p => p.IsNew);
                    else
                        requests = dbContext.Requests
                            .Where(p => string.IsNullOrEmpty(searchText) || searchText.Contains(p.FullName) || searchText.Contains(p.RequestContent) || searchText.Contains(p.ReplyUser))
                            .OrderByDescending(p => p.IsNew);
                    break;
                case "ReplyUser":
                    if (isAsc.GetValueOrDefault())
                        requests = dbContext.Requests
                            .Where(p => string.IsNullOrEmpty(searchText) || searchText.Contains(p.FullName) || searchText.Contains(p.RequestContent) || searchText.Contains(p.ReplyUser))
                            .OrderBy(p => p.Account.Username);
                    else
                        requests = dbContext.Requests
                            .Where(p => string.IsNullOrEmpty(searchText) || searchText.Contains(p.FullName) || searchText.Contains(p.RequestContent) || searchText.Contains(p.ReplyUser))
                            .OrderByDescending(p => p.Account.Username);
                    break;
                default:
                    if (isAsc.GetValueOrDefault())
                        requests = dbContext.Requests
                            .Where(p => string.IsNullOrEmpty(searchText) || searchText.Contains(p.FullName) || searchText.Contains(p.EmailAddress) || searchText.Contains(p.RequestContent) || searchText.Contains(p.ReplyUser))
                            .OrderBy(p => p.DateCreated);
                    else
                        requests = dbContext.Requests
                            .Where(p => string.IsNullOrEmpty(searchText) || searchText.Contains(p.FullName) || searchText.Contains(p.EmailAddress) || searchText.Contains(p.RequestContent) || searchText.Contains(p.ReplyUser))
                            .OrderByDescending(p => p.DateCreated);
                    break;
            }

            // do the query
            var requestModel = requests
                .Select(p => new ViewRequestItem
                {
                    Content = p.RequestContent,
                    FullName = p.FullName,
                    DateCreated = p.DateCreated,
                    ReplyUser = p.Account != null ? p.Account.Username : string.Empty,
                    IsNew = p.IsNew,
                    RequestId = p.RequestID
                })
                .Skip(pageSize * (pageNo - 1))
                .Take(pageSize).ToList();

            var model = new ViewRequestModel();
            model.CurrentPage = pageNo;
            model.SearchText = searchText;
            model.ItemOnPage = pageSize;
            model.StartIndex = pageSize * pageNo - pageSize + 1;
            model.EndIndex = model.StartIndex + pageSize - 1;
            model.TotalNumber = requests.Count();
            model.TotalPage = (int)Math.Ceiling((double)model.TotalNumber / pageSize);
            model.Requests = requestModel;
            model.SortField = sortField;
            model.IsAsc = isAsc.GetValueOrDefault();

            return View("ViewRequest", "_AdminLayout", model);
        }

        [HttpGet]
        [Authorize]
        public ActionResult ViewRequestDetail(int id)
        {
            var request = dbContext.Requests.FirstOrDefault(p => p.RequestID == id);

            if (request != null)
            {
                var model = new DetailRequestModel
                {
                    Content = request.RequestContent,
                    DateCreated = request.DateCreated.ToString("dd/MM/yyyy hh:mm:ss"),
                    RequestID = request.RequestID,
                    ReplyUser = request.Account != null ? request.Account.Username : string.Empty,
                    Reply = request.Reply,
                    EmailAddress = request.EmailAddress,
                    FullName = request.FullName
                };

                model.ListEmailTemplates = dbContext.EmailTemplates
                    .Select(e => new SelectListItem { Value = e.EmailTemplateId.ToString(), Text = e.EmailTemplateName }).ToList();

                return View("ViewRequestDetail", "_AdminLayout", model);
            }

            // should be throw error
            return View("ViewRequestDetail", "_AdminLayout", new DetailRequestModel());
        }

        [HttpPost]
        [ValidateInput(false)]
        [Authorize]
        public JsonResult AnswerRequest(AnswerRequestModel model)
        {
            if(model.RequestID > 0)
            {
                var request = dbContext.Requests.FirstOrDefault(r => r.RequestID == model.RequestID);

                //var ob = new EmailDeliveryModel
                //{
                //    IsBodyHtml = true,
                //    Subject = model.ReplySubject,                    
                //    Body = model.ReplyContent,
                //    SendTo = "bac.bear@gmail.com"
                //};

                var ob = EmailMergingHelper.MergeFeedbackEmail(model);
                ob.SendTo = "doanhhnqt74@gmail.com";

                bool isSuccess = EmailServiceHelper.Send(ob);

                if (isSuccess)
                {
                    request.Reply = model.ReplyContent;
                    request.ReplyUser = GetCurrentUserId();
                    request.DateCreated = DateTime.Now;

                    try
                    {
                        dbContext.Entry(request).State = EntityState.Modified;
                        dbContext.SaveChanges();

                        return Json(new { isResult = true, result = "Đã trả lời" }, JsonRequestBehavior.AllowGet);
                    }
                    catch (Exception ex)
                    {
                        return Json(new { isResult = true, result = "Có lỗi xảy ra trong khi ghi dữ liệu. Vui lòng thử lại sau" }, JsonRequestBehavior.AllowGet);
                    }                    
                }
                else
                {
                    return Json(new { isResult = false, result = "Có lỗi xảy ra trong khi gửi email. Vui lòng thử lại sau" }, JsonRequestBehavior.AllowGet);
                }                
            }

            return Json(new { isResult = false, result = "Không tìm thấy yêu cầu trong cơ sở dữ liệu. Vui lòng kiểm tra lại" }, JsonRequestBehavior.AllowGet);
        }
       
        #endregion

        #region EmailTemplate

        [Authorize]
        [HttpGet]
        public ActionResult ViewEmailTemplate(int? page, int? itemsPerPage, string searchText, string sortField, bool? isAsc)
        {
            var pageNo = page.GetValueOrDefault();
            var pageSize = itemsPerPage.GetValueOrDefault();

            if (pageNo == 0) pageNo = 1;
            if (pageSize == 0) pageSize = 10;
            if (isAsc == null) isAsc = false;
            if (string.IsNullOrEmpty(searchText)) searchText = null;
            if (string.IsNullOrEmpty(sortField)) sortField = "CreatedDate";

            IQueryable<EmailTemplate> templates;

            switch (sortField)
            {
                case "EmailTemplateName":
                    if (isAsc.GetValueOrDefault())
                        templates = dbContext.EmailTemplates
                            .Where(p => string.IsNullOrEmpty(searchText) || p.EmailTemplateName.Contains(searchText))
                            .OrderBy(p => p.EmailTemplateName);
                    else
                        templates = dbContext.EmailTemplates
                            .Where(p => string.IsNullOrEmpty(searchText) || p.EmailTemplateName.Contains(searchText))
                            .OrderByDescending(p => p.EmailTemplateName);
                    break;
               
                case "CreatedByUsername":
                    if (isAsc.GetValueOrDefault())
                        templates = dbContext.EmailTemplates
                            .Where(p => string.IsNullOrEmpty(searchText) || p.EmailTemplateName.Contains(searchText))
                            .OrderBy(p => p.Account.Username);
                    else
                        templates = dbContext.EmailTemplates
                            .Where(p => string.IsNullOrEmpty(searchText) || p.EmailTemplateName.Contains(searchText))
                            .OrderByDescending(p => p.Account.Username);
                    break;
                default:
                    if (isAsc.GetValueOrDefault())
                        templates = dbContext.EmailTemplates
                            .Where(p => string.IsNullOrEmpty(searchText) || p.EmailTemplateName.Contains(searchText))
                            .OrderBy(p => p.CreatedDate);
                    else
                        templates = dbContext.EmailTemplates
                            .Where(p => string.IsNullOrEmpty(searchText) || p.EmailTemplateName.Contains(searchText))
                            .OrderByDescending(p => p.CreatedDate);
                    break;
            }

            // do the query
            var templateModel = templates
                .Select(p => new ViewEmailTemplateItem
                {
                    EmailTemplateId = p.EmailTemplateId,
                    EmailTemplateName = p.EmailTemplateName,
                    CreatedByUsername = p.Account != null ? p.Account.Username : string.Empty,
                    CreatedDate = p.CreatedDate
                })
                .Skip(pageSize * (pageNo - 1))
                .Take(pageSize).ToList();

            var model = new ViewEmailTemplateModel();
            model.CurrentPage = pageNo;
            model.SearchText = searchText;
            model.ItemOnPage = pageSize;
            model.StartIndex = pageSize * pageNo - pageSize + 1;
            model.EndIndex = model.StartIndex + pageSize - 1;
            model.TotalNumber = templates.Count();
            model.TotalPage = (int)Math.Ceiling((double)model.TotalNumber / pageSize);
            model.Templates = templateModel;
            model.SortField = sortField;
            model.IsAsc = isAsc.GetValueOrDefault();

            return View(model);
        }

        [HttpGet]
        [Authorize]
        public JsonResult GetEmailTemplateContent(int id)
        {
            var emailTem = dbContext.EmailTemplates.FirstOrDefault(e => e.EmailTemplateId == id);

            if (emailTem == null)
            {
                return Json(new { isResult = false, result = "Không tìm thấy email mẫu. Vui lòng kiểm tra lại" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { isResult = true, replySubject = emailTem.EmailSubject, replyContent = emailTem.HtmlBody }, JsonRequestBehavior.AllowGet);
                
            }
        }


        [HttpGet]
        [Authorize]
        public ActionResult AddEmailTemplate()
        {
            var model = new EmailTemplateModel();
            model.MergeFields = dbContext.MergeFields.Where(m => m.FieldType == (int)FieldTypes.Common || m.FieldType == (int)FieldTypes.ReplyRequest)
                .Select(m => m.FieldName).ToList();

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public JsonResult AddEmailTemplate(EmailTemplateModel model)
        {
            var temp = new EmailTemplate
            {
                EmailType = (int)FieldTypes.ReplyRequest,
                EmailTemplateName = model.EmailTemplateName,
                EmailSubject = model.EmailSubject,
                HtmlBody = model.HtmlTextContent,
                CreatedBy = GetCurrentUserId(),
                CreatedDate = DateTime.Now
            };

            try
            {
                dbContext.EmailTemplates.Add(temp);
                dbContext.SaveChanges();
                return Json(new { isResult = true, result = "Lưu email mẫu thành công" }, JsonRequestBehavior.AllowGet);
            }            
            catch (Exception ex)
            {
                return Json(new { isResult = false, result = "Có lỗi xảy ra trong qua trình lưu. Vui lòng thử lại sau" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [Authorize]
        public ActionResult UpdateEmailTemplate(int id)
        {
            var emailTemp = dbContext.EmailTemplates.FirstOrDefault(e => e.EmailTemplateId == id);

            if (emailTemp == null)
            {
                return RedirectToAction("ViewEmailTemplate");
            }

            var model = new EmailTemplateModel
            {
                EmailTemplateId = id,
                EmailTemplateName = emailTemp.EmailTemplateName,
                EmailSubject = emailTemp.EmailSubject,
                HtmlTextContent = emailTemp.HtmlBody,
            };

            model.MergeFields = dbContext.MergeFields.Where(m => m.FieldType == emailTemp.EmailType)
                .Select(m => m.FieldName).ToList();

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public JsonResult UpdateEmailTemplate(EmailTemplateModel model)
        {
            var emailTemp = dbContext.EmailTemplates.FirstOrDefault(e => e.EmailTemplateId == model.EmailTemplateId);

            if (emailTemp == null)
            {
                return Json(new { isResult = false, result = "Không tìm thấy email mẫu. Vui lòng thử lại" }, JsonRequestBehavior.AllowGet);
            }

            emailTemp.EmailTemplateName = model.EmailTemplateName;
            emailTemp.EmailSubject = model.EmailSubject;           
            emailTemp.HtmlBody = model.HtmlTextContent;       
            emailTemp.LastUpdatedBy = GetCurrentUserId();
            emailTemp.LastUpdatedDate = DateTime.Now;

            try
            {
                dbContext.SaveChanges();
                return Json(new { isResult = true, result = "Cập nhật email mẫu thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { isResult = false, result = "Có lỗi xảy ra trong qua trình lưu. Vui lòng thử lại sau" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [Authorize]
        public JsonResult RemoveEmailTemplate(int id)
        {
            try
            {
                var emailTemplate = dbContext.EmailTemplates.FirstOrDefault(p => p.EmailTemplateId == id);

                if (emailTemplate != null)
                {
                    // remove it
                    dbContext.EmailTemplates.Remove(emailTemplate);
                    dbContext.SaveChanges();
                }
                else
                {
                    return Json(new { isResult = false, result = Constant.EmailTemplateNotFound }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { isResult = false, result = Constant.ErrorOccur }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { isResult = true, result = string.Empty }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
