using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NSocial.Controllers
{
    public class ReactionController : Controller
    {
        public void Like()
        {
            ViewBag.Message = "Your liked this photo.";
            return View();
        }

        public ActionResult Dislike()
        {
            ViewBag.Message = "Your disliked this photo.";
            return View();
        }


    }
}