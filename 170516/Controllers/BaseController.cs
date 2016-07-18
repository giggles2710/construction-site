﻿using _170516.Entities;
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

        protected string GetCurrentUserId()
        {
            var user = dbContext.Accounts.FirstOrDefault(a => a.Username.Equals(User.Identity.Name));

            if(user != null)
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
                }
                catch
                {
                    cart = new CartViewModel();
                }
            }

            return cart;
        }
    }
}