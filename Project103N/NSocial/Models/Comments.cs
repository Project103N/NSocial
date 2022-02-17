using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NSocial.DataAccess;

namespace NSocial.Models
{
    public class Comments
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int PostID { get; set; }
        public string Text { get; set; }

        private User _User { get; set; }
        public User User
        {
            get
            {
                if (_User == null)
                    _User = UserDAL.Methods.Find(UserID);
                return _User;
            }
            set
            {
                this._User = value;
            }

        }
    }
}