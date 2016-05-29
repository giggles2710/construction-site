using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _170516.Controllers
{
    public class HomeController : Controller
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

        [HttpGet]
        public ActionResult AddColor()
        {
            return PartialView("_AddColorPartial");
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

            return Json(new { base64Thumbnail = Convert.ToBase64String(data), fileTye = fileType }, JsonRequestBehavior.AllowGet);
        }
    }
}