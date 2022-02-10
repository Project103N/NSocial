using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NSocial.DataAccess;
using NSocial.ModelBase;

namespace NSocial.Models
{
    public class User:UserBase
    {

        public int ID { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Password2 { get; set; }
        public HttpPostedFileBase ProfileImage { get; set; }
        public int RoleID { get; set; }

        private Role _Role { get; set; }
        public Role Role {
            get { return RoleDAL.Methods.GetByID(RoleID); }
            set { this._Role = value; } 
        }
        public DateTime RegisterDate { get; set; }
        public ICollection<User> Followers { get; set; }
        public ICollection<User> Followings { get; set; }
        public bool isActive;

    }
}