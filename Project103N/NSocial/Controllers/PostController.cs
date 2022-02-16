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
    //[CustomAuthorize (Roles ="user,superadmin")]
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
        public ActionResult Profile(int id)
        {
           
            return View(PostDAL.Methods.GetByUserID(id));
        }

        public ActionResult Add()
        {
            Post newPost = new Post();
            return View(newPost);
        }

        [HttpPost]
        public ActionResult Add(Post post)
        {

            post.PostDate = DateTime.Now;

            post.PostImagePath = "photo1.jpg";
            post.LikesCount = 0;
            post.CommentsCount = 0;
            post.Comments = "";

            int insertedID = PostDAL.Methods.Add(post);
            if (insertedID != -1)
            {
                post.ID = insertedID;
                if (post.PostImage != null)
                {
                    string path = PhotoUpload(post.ID, post.PostImage);
                    if (path != "")
                    {
                        post.PostImagePath = path;
                        PostDAL.Methods.Edit(post);
                        return Content(path);
                    }
                }
            }
            return RedirectToAction("Index");
        }
        public string PhotoUpload(int ID, HttpPostedFileBase postPhoto)
        {
            string userPath = Server.MapPath($"~/UploadedFiles/Post/PostPhoto/");
            string shortPath = $"{DateTime.Now.Year}/{DateTime.Now.Month}/{DateTime.Now.Day}/{ID}/";
            string directory = userPath + shortPath;
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
            shortPath += postPhoto.FileName;

            string photoPath = userPath + shortPath;
            try
            {
                postPhoto.SaveAs(photoPath);
            }
            catch (Exception)
            {
                shortPath = "";
            }
            return shortPath;
        }
        public ActionResult Details(int id)
        {
            return View(PostDAL.Methods.GetByID(id));
        }

        //------------------------
        //------------------------EDİT------------------------------------
        public ActionResult Edit(int id)
        {
            return View(PostDAL.Methods.GetByID(id));
        }

        [HttpPost]
        public ActionResult Edit(Post post)
        {
            if (post.PostImage != null)
            {
                string path = PhotoUpload(post.ID, post.PostImage);
                if (path != "")
                {
                    string oldPhotoPath = Server.MapPath($"~/UploadedFiles/Post/PostPhoto/") + post.PostImagePath;
                    FileInfo f = new FileInfo(oldPhotoPath);
                    if (f.Exists)
                        f.Delete();
                    post.PostImagePath = path;

                    //eskiyi sil
                }
            }
            try
            {
                PostDAL.Methods.Edit(post);
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");//return View();
            }
            int affectedRows = PostDAL.Methods.Edit(post);
            if (affectedRows > 0)
                TempData["editmessage"] = "Edit successfull!!!";
            else
                TempData["editmessage"] = "Error on edit!!!";
            return RedirectToAction("Index");
        }

        //------------------------DELETE------------------------------------
        public ActionResult Delete(int id)
        {
            return View(PostDAL.Methods.GetByID(id));
        }

        [HttpPost]
        public ActionResult Delete(Post post)
        {
            int affectedRows = PostDAL.Methods.Delete(post.ID);
            if (affectedRows > 0)
                TempData["deletemessage"] = "Delete successfull!!!";
            else
                TempData["deletemessage"] = "Error on delete!!!";
            return RedirectToAction("Index");
        }

    }
}
