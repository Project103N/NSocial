using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NSocial.ModelBase;

namespace NSocial.Models
{
    public class User:UserBase
    {
        public int ID { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public HttpPostedFileBase ProfileImage { get; set; }
        public int RoleID { get; set; } = 1;
        public Role Role { get; set; }
        private DateTime _RegisterDate { get; set; }
        public DateTime RegisterDate
        {
            get
            {
                return _RegisterDate;
            }
            set
            {
                _RegisterDate = DateTime.Now;
            }
        }

        public ICollection<User> Followers { get; set; }
        public ICollection<User> Followings { get; set; }
    }
}