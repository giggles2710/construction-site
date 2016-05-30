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

        [HttpPost]
        public JsonResult AddColor(CreateColorModel model)
        {
            Color color = null;

            if (ModelState.IsValid)
            {
                color = new Color
                {
                    ColorName = model.ColorName,
                    Image = Convert.FromBase64String(model.ColorBase64String),
                    ColorDescription = model.ColorDescription,
                    Extension = model.Extension
                };

                dbContext.Colors.Add(color);

                try
                {
                    dbContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    return Json(new { isError = true, result = ex.Message }, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(new { isError = false, result = color }, JsonRequestBehavior.AllowGet);
        }
    }
}