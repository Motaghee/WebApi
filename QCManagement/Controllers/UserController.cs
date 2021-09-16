using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using QCManagement.Models;
using System.Security.Claims;

namespace QCManagement.Controllers
{
    public class UserController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Login(UserModels UM, string ReturnUrl = "")
        {
            if ((UM != null) && (ModelState.IsValid))
            {

                var isValidUser = Membership.ValidateUser(UM.USERNAME, UM.PSW);
                if (isValidUser)
                {
                    FormsAuthentication.SetAuthCookie(UM.USERNAME.ToString(), false);
                    //string a = System.Web.HttpContext.Current.User.Identity.Name;
                    MyMembershipUser msUser =(MyMembershipUser) Membership.GetUser(UM.USERNAME);
                    Session["USERNAME"] = msUser.UserName;
                    Session["FNAME"] = msUser.FName;
                    Session["LNAME"] = msUser.LName;
                    Session["SRL"] = msUser.SRL;

                    if (Url.IsLocalUrl(ReturnUrl))
                    {
                        ModelState.Remove("Password");
                        return Redirect(ReturnUrl);
                    }
                    else
                    {
                        ModelState.Remove("Password");
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.Remove("Password");
                    return View("Login");
                }
            }
            else
            {
                //ModelState.Remove("Password");
                return View(UM);
            }
        }
        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Login", "User");
        }

        public ActionResult AccessDenied()
        {
            return View();
        }
    }
}