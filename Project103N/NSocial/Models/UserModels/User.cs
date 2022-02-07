using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NSocial.Models.UserModels
{
    public class User
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public HttpPostedFileBase ProfileImage { get; set; }
        public string ProfileImagePath { get; set; }
        public string Phone { get; set; }
        public int RoleID { get; set; }

        public Role Role
        {
            get
            {
                return new Role();
                //TODO: return RoleDAL.Methods.GetByID(this.RoleID);
            }
        }

        public bool Login()
        {
            // TODO: Burada UserDAL kullanılarak oturum kontrolü yapılacak.
            return true;
        }

    }
}