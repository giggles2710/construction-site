using _170516.Entities;
using _170516.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _170516.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult SendRequestContact(RequestContactModel model)
        {
            var requestContact = new Request();
            requestContact.EmailAddress = model.EmailAddress;
            requestContact.FullName = model.FullName;
            requestContact.RequestContent = model.Content;
            requestContact.IsNew = true;
            requestContact.DateCreated = DateTime.Now;

            dbContext.Requests.Add(requestContact);

            try
            {
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                return Json(new { isValid = false }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { isValid = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UploadImage()
        {
            HttpPostedFileBase myFile = Request.Files["ImageUpload"];

            byte[] data;
            using (Stream inputStream = myFile.InputStream)
            {
                MemoryStream memoryStream = inputStream as MemoryStream;
                if (memoryStream == null)
                {
                    memoryStream = new MemoryStream();
                    inputStream.CopyTo(memoryStream);
                }
                data = memoryStream.ToArray();
            }

            var names = myFile.FileName.Split('.');
            var fileType = names[names.Length - 1];

            return Json(new { base64Thumbnail = Convert.ToBase64String(data), fileType = fileType }, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult ProductMenu(int? id)
        //{
        //    var categories = new List<ProductMenuItem>();

        //    if (id.GetValueOrDefault() > 0)
        //    {
        //        var categoryId = id.GetValueOrDefault();
        //        // get the sub item of this id
        //        categories = dbContext.Categories.Where(ca => ca.IsActive && ca.ParentID == categoryId).Select(c => new ProductMenuItem
        //        {
        //            CategoryId = c.CategoryID,
        //            CategoryName = c.Name
        //        }).ToList();
        //    }
        //    else
        //    {
        //        // get all categories
        //        categories = dbContext.Categories.Where(c => c.ParentID == null && c.IsActive).Select(ca => new ProductMenuItem
        //        {
        //            CategoryId = ca.CategoryID,
        //            CategoryName = ca.Name
        //        }).ToList();
        //    }

        //    // fill up missing information
        //    foreach (var item in categories)
        //    {
        //        item.HasSubMenu = dbContext.Categories.Where(c => c.ParentID == item.CategoryId).Count() > 0;
        //    }

        //    return View("_PartialProductMenu", categories);
        //}
    }
}