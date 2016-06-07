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