using NSocial.DataAccess;
using NSocial.Models;
using NSocial.Security;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NSocial.Controllers
{
    public class PostController : Controller
    {
        // GET: Post
        static string strConnection = ConfigurationManager.ConnectionStrings["NSocialCS"].ConnectionString;
        public SqlConnection con = new SqlConnection(strConnection);





        // GET: Post
        public ActionResult Index()
        {
            List<Post> posts = PostDAL.Methods.List();

            return View(posts);
        }
        [HttpPost]
        public ActionResult Index(string searchterm)
        {
            List<Post> searchedStudents = PostDAL.Methods.Search(searchterm);
            return View(searchedStudents);
        }


        public ActionResult Add()
        {
            Post newPost = new Post();
            return View(newPost);
        }

        [HttpPost]
        public ActionResult Add(Post post)
        {

            TempData["insertedID"] = PostDAL.Methods.Add(post);
            return RedirectToAction("Index");
        }
        public ActionResult Details(int id)
        {
            return View(PostDAL.Methods.GetByID(id));
        }
    }
}
