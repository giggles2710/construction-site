using _170516.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _170516.Controllers
{
    public class BaseController : Controller
    {
        protected ConstructionSiteEntities dbContext = new ConstructionSiteEntities();

        protected string GetCurrentUserId()
        {
            var user = dbContext.Accounts.FirstOrDefault(a => a.Username.Equals(User.Identity.Name));

            if(user != null)
            {
                return user.AccountID;
            }

            return string.Empty;
        }
    }
}