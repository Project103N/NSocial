using NSocial.DataAccess;
using NSocial.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NSocial.Security;
namespace NSocial.Controllers
{
    public class CommentsController : Controller
    {
        static string strConnection = ConfigurationManager.ConnectionStrings["NSocialCS"].ConnectionString;
        public SqlConnection con = new SqlConnection(strConnection);

        public ActionResult Index()
        {
            List<Comments> commentss = CommentsDAL.Methods.List();

            return View(commentss);
        }
        public ActionResult Details(int id)
        {
            List<Comments> com = CommentsDAL.Methods.GetByPostID(id);
            return View(com);
        }
        public ActionResult Add(int id)
        {
            Comments newComments = new Comments();
            newComments.PostID = id;
            return View(newComments);
        }
        [HttpPost]
        public int Add(Comments comments)
        {
            TempData["insertedID"] = CommentsDAL.Methods.Add(comments);
            ViewBag.comment = comments.Text;
            return 1;
        }
        
    }
}