using NSocial.DataAccess;
using NSocial.Models;
using NSocial.Security;
using NSocial.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NSocial.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        
        public ActionResult Login()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult Login(LoginUser loginUser)
        {

            User user = UserDAL.Methods.FindX(loginUser.Email);
            if (user != null)
            {
                if (user.Password == loginUser.Password)
                {
                    // Session oluştur!!!
                    SessionPersister.Email = user.Email;
                    SessionPersister.ID = user.ID;
                    return Redirect("/User/Index");
                    // returnUrl eklenecek.
                }
            }

            ViewBag.Error = "Login failed.";
            return View("Login");

        }

        public ActionResult Logout()
        {
            SessionPersister.Email = string.Empty;
            return Redirect("/Home/Index");
        }

        public ActionResult AuthorizationFailed()
        {
            return View();
        }
    }
}
