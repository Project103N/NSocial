using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NSocial.Controllers
{
    public class ReactionController : Controller
    {
        static int likeCount = 0;
        static int dislikeCount = 0;

        [HttpPost]
        public int Like(string value)
        {
            likeCount += int.Parse(value);
            ViewBag.Message = "Your liked this photo.";
            return likeCount;
        }
        public ActionResult Like()
        {
            return View();
        }

        public ActionResult Dislike(int value)
        {
            dislikeCount += value;
            ViewBag.Message = "Your disliked this photo.";
            return View();
        }
        [HttpPost]
        public int Dislike(string value)
        {
            dislikeCount += int.Parse(value);
            ViewBag.Message = "Your disliked this photo.";
            return dislikeCount;
        }


    }
}