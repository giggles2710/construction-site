using _170516.Entities;
using _170516.Models;
using Newtonsoft.Json;
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

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            // now, get the system variable, then apply to view
            // system variables
            Constant.SystemVariables = this.GetSystemVariable();

            foreach (var variable in Constant.SystemVariables)
            {
                ViewData[variable.Key] = variable.Value;
            }

            base.OnActionExecuted(filterContext);
        }

        //protected override void OnException(ExceptionContext filterContext)
        //{
        //    Exception ex = filterContext.Exception;
        //    filterContext.ExceptionHandled = true;

        //    var model = new HandleErrorInfo(filterContext.Exception, "Controller", "Action");

        //    filterContext.Result = new ViewResult()
        //    {
        //        ViewName = "Error",
        //        ViewData = new ViewDataDictionary(model)
        //    };

        //}

        protected string GetCurrentUserId()
        {
            var user = dbContext.Accounts.FirstOrDefault(a => a.Username.Equals(User.Identity.Name));

            if (user != null)
            {
                return user.AccountID;
            }

            return string.Empty;
        }

        protected CartViewModel GetCart(HttpContextBase context)
        {
            CartViewModel cart;

            if (context.Request.Cookies[Constant.CartCookie] == null)
            {
                cart = new CartViewModel();
            }
            else
            {
                HttpCookie cookie = context.Request.Cookies[Constant.CartCookie];

                try
                {
                    string siteCart = cookie[Constant.ProductInCartCookie];
                    cart = JsonConvert.DeserializeObject<CartViewModel>(siteCart);

                    // recalculating grand total
                    if (cart.Products != null && cart.Products.Any())
                    {
                        cart.GrandTotal = 0;
                        foreach (var product in cart.Products)
                        {
                            cart.GrandTotal += product.Price * product.Quantity;
                        }
                    }
                }
                catch
                {
                    cart = new CartViewModel();
                }
            }

            return cart;
        }

        protected Dictionary<string, string> GetSystemVariable()
        {
            // get system variables
            var systemVariables = new Dictionary<string, string>();

            foreach(var variable in dbContext.SystemVariables)
            {
                systemVariables.Add(variable.Code, variable.Value);
            }

            return systemVariables;
        }
    }
}