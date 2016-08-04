using System;
using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using _170516.Models;
using _170516.Utility;

namespace _170516.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {
        public AccountController()
        {
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            try
            {
                if (this.Request.IsAuthenticated)
                {
                    return RedirectToLocal(returnUrl);
                }
            }
            catch (Exception ex)
            {
                // log
            }

            //ViewBag.ReturnUrl = returnUrl;
            return View("Login", "_LayoutPlain");
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            try
            {
                // first of all, try to get the user id and user hashed token
                var user = dbContext.GetHashTokenByEmailAddress(model.Email.ToLower());

                if (user != null)
                {
                    var foundUser = user.ToList();

                    if (foundUser.Any())
                    {
                        // hashed the inputed password
                        var hashedPassword = EncryptionHelper.HashPassword(model.Password, foundUser.First().HashToken);
                        var loginInfo = dbContext.LoginByEmailAdress(model.Email.Trim(), hashedPassword.Trim());

                        if (loginInfo != null)
                        {
                            var loginDetails = loginInfo.ToList();

                            if (loginDetails.Any())
                            {
                                var loginDetail = loginDetails.First();
                                // log him in
                                SignInUser(loginDetail.Username, false);
                            }
                        }
                    }

                    ModelState.AddModelError(string.Empty, Constant.InvalidLogin);
                }
            }
            catch (Exception ex)
            {
                // log error
            }

            return Json(new { isResult = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LogOff()
        {
            try
            {
                // Setting.    
                var ctx = Request.GetOwinContext();
                var authenticationManager = ctx.Authentication;
                // Sign Out.    
                authenticationManager.SignOut();
            }
            catch (Exception ex)
            {
                // Info    
                throw ex;
            }
            // Info.    
            return this.RedirectToAction("Login", "Account");
        }

        private void SignInUser(string username, bool isPersistent)
        {
            var claims = new List<Claim>();

            try
            {
                claims.Add(new Claim(ClaimTypes.Name, username));

                var claimIdenties = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);
                var ctx = Request.GetOwinContext();
                var authenticationManager = ctx.Authentication;

                authenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, claimIdenties);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            try
            {
                // Verification.    
                if (Url.IsLocalUrl(returnUrl))
                {
                    // Info.    
                    return this.Redirect(returnUrl);
                }
            }
            catch (Exception ex)
            {
                // Info    
                throw ex;
            }
            // Info.    
            return this.RedirectToAction("Index", "Home");
        }
    }
}