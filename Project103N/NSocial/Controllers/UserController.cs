using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NSocial.DataAccess;
using NSocial.Models;
using System.IO;
using System.Data.SqlClient;
using System.Configuration;
using NSocial.Security;
using NSocial.ViewModels;

namespace NSocial.Controllers
{
    //[CustomAuthorize(Roles = "superadmin")]
    public class UserController : Controller
    {
        static string strConnection = ConfigurationManager.ConnectionStrings["NSocialCS"].ConnectionString;
        public SqlConnection con = new SqlConnection(strConnection);
        // GET: User
        public ActionResult Index()
        {
            // UserDAL.Methods.All() 'dan dönen List<User> ı döndürür.
            return View(UserDAL.Methods.All());
        }

        // GET: User/Details/5
        public ActionResult Details(int id)
        {
            return View(UserDAL.Methods.Find(id));
        }

        [AllowAnonymous]
        // GET: User/Create
        public ActionResult Register()
        {
            return View();
        }

        [AllowAnonymous]
        // POST: User/Create
        // TODO: We will add short register form later.
        [HttpPost] // From Form
        public ActionResult Register(User user)
        {
            try
            {
                user.RegisterDate = DateTime.Now; // auto registerdate
                if (user.RoleID == 0)
                    user.RoleID = 1; // default role: user
                user.ProfileImagePath = "default.png";
                user.isActive = true;
                int insertedID = UserDAL.Methods.Add(user);
                if (insertedID != -1)
                {
                    user.ID = insertedID;
                    if (user.ProfileImage != null)
                    {
                        string path = PhotoUpload(user.ID, user.ProfileImage);
                        if (path != "")
                        {
                            user.ProfileImagePath = path;
                            UserDAL.Methods.SaveChanges(user);
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
                //return Content("Olmadı yar!\");
            }
        }
        public string PhotoUpload(int userID, HttpPostedFileBase userPhoto)
        {
            string userPath = Server.MapPath($"~/UploadedFiles/User/");
            string shortPath = $"{DateTime.Now.Year}/{DateTime.Now.Month}/{DateTime.Now.Day}/{userID}/";
            string directory = userPath + shortPath;
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
            shortPath += userPhoto.FileName;

            string photoPath = userPath + shortPath;
            try
            {
                userPhoto.SaveAs(photoPath);
            }
            catch (Exception)
            {
                shortPath = "";
            }
            return shortPath;
        }

        // GET: User/Edit/5

        [CustomAuthorize(Roles = "user,superadmin")]
        public ActionResult Edit(int id)
        {
            User currentUser = UserDAL.Methods.FindX(SessionPersister.Email);

            User editUser = UserDAL.Methods.FindX(id);
            if (currentUser.RoleID == 1)
            { // rolü user ise sadece kendi hesabını düzenleyebilir.
                if (SessionPersister.Email != editUser.Email) // kendisi mi?
                    return RedirectToAction("AuthorizationFailed");
            }
            return View(UserDAL.Methods.Find(id));
        }

        // POST: User/Edit/5
        [HttpPost]
        public ActionResult Edit(User user)
        {
            if (user.ProfileImage != null)
            {
                string path = PhotoUpload(user.ID, user.ProfileImage);
                if (path != "")
                {
                    string oldPhotoPath = Server.MapPath($"~/UploadedFiles/User/") + user.ProfileImagePath;
                    FileInfo f = new FileInfo(oldPhotoPath);
                    if (f.Exists)
                        f.Delete();
                    user.ProfileImagePath = path;

                    //eskiyi sil
                }
            }

            user.isActive = true;
            try
            {
                UserDAL.Methods.SaveChanges(user);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }


        // GET: User/Delete/5
        public ActionResult Delete(int id)
        {
            return View(UserDAL.Methods.Find(id));
        }

        // POST: User/Delete/5
        [HttpPost]
        public ActionResult Delete(User user)
        {
            try
            {
                user = UserDAL.Methods.Find(user.ID);
                user.isActive = false;
                UserDAL.Methods.SaveChanges(user);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

<<<<<<< Updated upstream
<<<<<<< Updated upstream
        public ActionResult FollowList()
        {
            return View();
        }

        public ActionResult GetFollower()
        {
            return View(UserDAL.Methods.All());
        }

        public ActionResult Follow(User user)
        {
            try
            {
                FollowDAL.Methods.FriendRequest(user.ID);
               
  
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }

            
        }

        public ActionResult FriendList()
        {
            return View();
        }


=======
=======
>>>>>>> Stashed changes
       public ActionResult Profile()
        {
            return View();
        }
<<<<<<< Updated upstream
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
    }
}
