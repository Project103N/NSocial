using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NSocial.DataAccess;
using NSocial.Models;
using System.IO;

namespace NSocial.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        // GET: User/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: User/Create
        public ActionResult Register()
        {
            return View();
        }

        // POST: User/Create
        // TODO: We will add short register form later.
        [HttpPost]
        public ActionResult Register(User user)
        {
            try
            {
                user.RegisterDate = DateTime.Now; // auto registerdate
                

                if (user.RoleID == 0)
                    user.RoleID = 1; // default role: user
                user.ProfileImagePath = "default.png";

                int insertedID = UserDAL.Methods.Insert(user);

                if(insertedID != -1)
                {
                    user.ID = insertedID;

                    if (user.ProfileImage != null)
                    {
                        string path = PhotoUpload(user.ID, user.ProfileImage);
                        if (path != "")
                        {
                            return Content(path);
                            // TODO: Update with image path
                        }
                    }
                }
                string html = "<p>" + insertedID + "</p>";
                return Content(html);
            }
            catch(Exception e)
            {
                throw(e);
                //return Content("Olmadı yar!\");
            }
        }
        public string PhotoUpload(int userID,HttpPostedFileBase userPhoto)
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
                throw;
            }
            return shortPath;
        }

        // GET: User/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: User/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

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
            return View();
        }

        // POST: User/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
