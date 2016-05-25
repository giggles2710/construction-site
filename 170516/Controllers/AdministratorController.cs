using _170516.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _170516.Controllers
{
    public class AdministratorController : Controller
    {
        //
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

        public ActionResult ViewProductCategory()
        {
            return View("ViewProductCategory", "_AdminLayout");
        }

        [HttpGet]
        public ActionResult AddProductCategory()
        {
            return PartialView("_AddProductCategoryPartial");
        }
    }
}