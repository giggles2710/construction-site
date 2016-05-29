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
        public ActionResult ViewProduct()
        {
            return View("ViewProduct", "_AdminLayout");
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

        [HttpGet]
        public ActionResult AddColor()
        {
            return PartialView("_AddColorPartial");
        }
    }
}