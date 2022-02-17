using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NSocial.Security;
using NSocial.DataAccess;
using NSocial.Models;

namespace NSocial.Controllers
{
    public class HomeController : Controller
    {
        [CustomAuthorize(Roles="user,admin,superadmin")]
        public ActionResult Index()
        {
            User activeUser = UserDAL.Methods.Find(SessionPersister.ID);
            ViewBag.activeUser = activeUser;

            // TODO: Burası takip edilenlere göre düzenlenecek.
            ICollection<Post> posts = PostDAL.Methods.List();



            List<User> followRequests = new List<User>();
            List<User> IDList = FollowDAL.Methods.WaitingRequestsList();
            foreach (var item in IDList)
            {
                followRequests.Add(UserDAL.Methods.Find(item.ID));
            }
            ViewBag.followRequests = followRequests;

            return View(posts);
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
    }
}