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
            // Esra homepage olusturacak ve oraya göndereceğiz, profileview
            return View();
        }


        //PostADD
        public ActionResult PostAdd(Post post)
        {
            try
            {
                //if (user.RoleID == 0)
                //    user.RoleID = 1; // default role: user
                //user.ProfileImagePath = "default.png";
                //user.isActive = true;
                //tekrar bakilacak
                post.PostDate = DateTime.Now;
                int insertedID = PostDAL.Methods.Add(post);
                if (insertedID != -1)
                {
                    post.ID = insertedID;
                    if (post.PostImage != null)
                    {
                        string path = PostUpload(post.ID, post.PostImage);
                        if (path != "")
                        {
                            post.PostImagePath = path;
                            PostDAL.Methods.SaveChanges(post);
                            return Content(path);
                        }
                    }
                }
                string html = "<p>" + insertedID + "</p>";
                return Content(html);
            }
            catch (Exception e)
            {
                throw (e);
            }
        }

        public string PostUpload(int PostID, HttpPostedFileBase PostPhoto)
        {
            string PostPath = Server.MapPath($"~/UploadedFiles/Post/");
            string shortPath = $"{DateTime.Now.Year}/{DateTime.Now.Month}/{DateTime.Now.Day}/{PostID}/";
            string directory = PostPath + shortPath;
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
            shortPath += PostPhoto.FileName;

            string postPath = PostPath + shortPath;
            try
            {
                PostPhoto.SaveAs(postPath);
            }
            catch (Exception)
            {
                shortPath = "";
            }
            return shortPath;
        }

        // GET: Post/Edit/5

        [CustomAuthorize(Roles = "User, superadmin")]
        public ActionResult Edit(int id)
        {
            Post currentPost = PostDAL.Methods.Find(SessionPersister.ID);

            Post editPost = PostDAL.Methods.FindX(id);
            if (currentPost.ID == 1)
            { 
                if (SessionPersister.ID != editPost.ID) 
                    return RedirectToAction("AuthorizationFailed");
            }
            return View(PostDAL.Methods.Find(id));
        }

        // POST: Post/Edit/5
        [HttpPost]
        public ActionResult Edit(Post Post)
        {
            if (Post.PostImagePath != null)
            {
                string path = PostUpload(Post.ID, Post.PostImage);
                if (path != "")
                {
                    string oldPhotoPath = Server.MapPath($"~/UploadedFiles/Post/") + Post.PostImagePath;
                    FileInfo f = new FileInfo(oldPhotoPath);
                    if (f.Exists)
                        f.Delete();

                    Post.PostImagePath = path;
                }
            }

            try
            {
                PostDAL.Methods.SaveChanges(Post);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }


        // GET: Post/Delete/5
        public ActionResult Delete(int id)
        {
            return View(PostDAL.Methods.FindX(id));
        }

        // POST: Post/Delete/5
        [HttpPost]
        public ActionResult Delete(Post Post)
        {
            try
            {
                Post = PostDAL.Methods.Find(Post.ID);
               // Post.isActive = false;
                PostDAL.Methods.SaveChanges(Post);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }



    }
}
