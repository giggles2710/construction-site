using _170516.Entities;
using _170516.Models;
using _170516.Models.Administrator;
using _170516.Utility;
using Newtonsoft.Json;
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

                // comment list
                

                return View(model);
            }

            return View();
        }        

        public void AddCartToCookie(HttpContextBase context, CartViewModel cart)
        {
            HttpCookie cookie = new HttpCookie(Constant.CartCookie);
            cookie[Constant.ProductInCartCookie] = JsonConvert.SerializeObject(cart);
            cookie.Expires.AddDays(365);

            context.Response.Cookies.Add(cookie);
        }

        [HttpGet]
        public ActionResult ViewCart()
        {
            CartViewModel cart = GetCart(this.HttpContext);

            return View(cart);
        }

        [HttpPost]
        public JsonResult UpdateCart(CartViewModel model)
        {
            Product product;
            List<ProductError> errors = new List<ProductError>();
            model.Products.ForEach(p => {
                product = dbContext.Products.FirstOrDefault(ob => ob.ProductID == p.ProductId);

                if (product.UnitsInStock == 0)
                {
                    ProductError err = new ProductError { ProductId = product.ProductID, Error = "Sản phẩm này đã hết hàng" };
                    errors.Add(err);
                }
                else
                if (product.UnitsInStock < p.Quantity)
                {
                    ProductError err = new ProductError { ProductId = product.ProductID, Error = "Chỉ còn " + product.UnitsInStock + " sản phẩm trong kho" };
                    errors.Add(err);
                }
            });

            if (errors.Count > 0)
            {
                return Json(new { isResult = false, errors = errors }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                model.GrandTotal = 0;

                model.Products.ForEach(p => {
                    p.Total = p.Price * p.Quantity;
                    model.GrandTotal += p.Total;
                });                

                AddCartToCookie(this.HttpContext, model);

                return Json(new { isResult = true }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult DeleteProductInCart(int id)
        {
            CartViewModel cart = GetCart(this.HttpContext);

            var p = cart.Products.FirstOrDefault(ob => ob.ProductId == id);

            if (p != null)
            {
                cart.GrandTotal -= p.Price * p.Quantity;

                cart.Products.Remove(p);

                AddCartToCookie(this.HttpContext, cart);
            }

            return View("ViewCart", cart);
        }

        [HttpGet]
        public ActionResult ViewCartMinimal()
        {
            CartViewModel cart = GetCart(this.HttpContext);

            return PartialView("_PartialCart", cart);
        }

        [HttpGet]
        public ActionResult Checkout()
        {
            CartViewModel cart = GetCart(this.HttpContext);

            if (cart.Products.Count == 0)
            {
                return RedirectToAction("ViewCart", cart);
            }

            var model = new CheckoutModel { Cart = cart, Customer = new CustomerCheckoutModel()};            

            return View(model);
        }

        [HttpPost]
        public ActionResult Checkout(CheckoutModel model)
        {
            Product product;
            List<ProductError> errors = new List<ProductError>();
            model.Cart.Products.ForEach(p => {
                product = dbContext.Products.FirstOrDefault(ob => ob.ProductID == p.ProductId);

                if (product.UnitsInStock == 0)
                {
                    ProductError err = new ProductError { ProductId = product.ProductID, Error = "Sản phẩm này đã hết hàng" };
                    errors.Add(err);
                }
                else
                if (product.UnitsInStock < p.Quantity)
                {
                    ProductError err = new ProductError { ProductId = product.ProductID, Error = "Chỉ còn " + product.UnitsInStock + " sản phẩm trong kho" };
                    errors.Add(err);
                }
            });

            if (errors.Count > 0)
            {
                return Json(new { isResult = false, errors = errors }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                model.Cart.GrandTotal = 0;

                model.Cart.Products.ForEach(p => {
                    p.Total = p.Price * p.Quantity;
                    model.Cart.GrandTotal += p.Total;
                });

                AddCartToCookie(this.HttpContext, model.Cart);

                if (model.Customer.SameInfoForShipping)
                {
                    model.Customer.ShipAddress = model.Customer.Address;
                    model.Customer.ShipDistrict = model.Customer.District;
                    model.Customer.ShipCity = model.Customer.City;
                    model.Customer.ShipPhone = model.Customer.Phone;
                }

                TempData["ConfirmationModel"] = model;
                return Json(new { isResult = true }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult CheckoutConfirmation()
        {            
            var model = TempData["ConfirmationModel"] as CheckoutModel;

            if (model == null || model.Cart.Products.Count == 0)
            {
                return RedirectToAction("ViewCart", GetCart(this.HttpContext));
            }

            return View(model);
        }

        [HttpPost]
        public JsonResult CheckoutConfirmation(CheckoutModel model)
        {
            //create customer
            var customer = new Customer
            {
                Fullname = model.Customer.FullName,
                EmailAddress = model.Customer.EmailAddress,
                Phone = model.Customer.Phone,
                Address = model.Customer.Address,
                District = model.Customer.District,
                City = model.Customer.City,
                ShipAddress = model.Customer.ShipAddress,
                ShipDistrict = model.Customer.ShipDistrict,
                ShipCity = model.Customer.ShipCity,
                ShipPhone = model.Customer.ShipPhone
            };

            //create order details
            List<OrderDetail> listProduct = new List<OrderDetail>();

            model.Cart.Products.ForEach(p => {
                OrderDetail orderDetails = new OrderDetail();
                orderDetails.ProductID = p.ProductId;
                orderDetails.Price = p.Price;
                orderDetails.Quantity = p.Quantity;
                orderDetails.Total = p.Total;
                orderDetails.Size = 0;
                orderDetails.IsFulfilled = false;
                listProduct.Add(orderDetails);
            });

            //create order
            var order = new Order
            {
                Customer = customer,
                OrderDetails = listProduct,
                Freight = 0,
                SalesTax = 0,
                OrderStatus = (int)OrderStatuses.OrderIsCreated,
                OrderNumber = string.Format("DH-{0}", DateTime.Now.ToShortDateString()),
                OrderDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
            };

            try
            {
                dbContext.Orders.Add(order);
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                return Json(new { isResult = false, result = "Lỗi khi lưu thông tin giỏ hàng. Vui lòng thử lại sau" }, JsonRequestBehavior.AllowGet);
            }

            //clear cookie
            HttpCookie cookie = new HttpCookie(Constant.CartCookie);
            cookie.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(cookie);

            //Response.Cookies.Remove(Constant.CartCookie);

            //var emailToSend = EmailMergingHelper.MergeOrderConfirmationEmail(order.OrderID);
            //emailToSend.SendTo = "doanhhnqt74@gmail.com";

            //if (emailToSend != null)
            //{
            //    bool isSuccess = EmailServiceHelper.Send(emailToSend);
            //    if (!isSuccess)
            //        return Json(new { isResult = false, result = "Có lỗi xảy ra khi gửi mail" }, JsonRequestBehavior.AllowGet);
            //}

            return Json(new { isResult = true }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult AddProductToCart(ProductInCart model)
        {
            if (model.ProductId == 0)
            {
                return Json(new { isResult = false, result = "Sản phẩm không tồn tại" }, JsonRequestBehavior.AllowGet);
            }

            var product = dbContext.Products.FirstOrDefault(p => p.ProductID == model.ProductId);

            if (product == null)
            {
                return Json(new { isResult = false, result = "Sản phẩm không tồn tại" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //add Product to Cart Cookie
                CartViewModel cart = GetCart(this.HttpContext);

                var productInCart = cart.Products.FirstOrDefault(p => p.ProductId == model.ProductId);

                int numberOfQuantity = productInCart == null ? 0 : productInCart.Quantity;
                
                if (product.UnitsInStock < (model.Quantity + numberOfQuantity))
                {
                    if (product.UnitsInStock == 0)
                    {
                        return Json(new { isResult = false, result = "Sản phẩm này đã hết hàng. Vui lòng quay lại sau" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        if (numberOfQuantity == 0)
                        {
                            return Json(new { isResult = false, result = "Chỉ còn " + product.UnitsInStock + " sản phẩm trong kho." }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(new { isResult = false, result = "Bạn đã có "+ numberOfQuantity+" sản phẩm trong giỏ hàng. Chỉ còn " +
                                                                        + product.UnitsInStock + " sản phẩm trong kho." }, 
                                JsonRequestBehavior.AllowGet);
                        }                        
                    }
                }
                else
                {
                    if (productInCart == null)
                    {
                        ProductInCart p = new ProductInCart();
                        p.ProductId = product.ProductID;
                        p.ProductName = product.Name;
                        p.Quantity = model.Quantity;
                        p.Price = product.UnitPrice * (100 - product.Discount) / 100;
                        p.Total = p.Price * p.Quantity;
                        cart.Products.Add(p);
                        cart.GrandTotal += p.Total;
                    }
                    else
                    {
                        productInCart.Quantity = productInCart.Quantity + model.Quantity;
                    }

                    AddCartToCookie(this.HttpContext, cart);

                    return Json(new { isResult = true, result = "Thêm sản phẩm vào giỏ hàng thành công" }, JsonRequestBehavior.AllowGet);
                }
            }
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