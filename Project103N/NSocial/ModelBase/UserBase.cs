using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NSocial.ModelBase
{
    public class UserBase
    {
        //public string PostImagePath { get; set; }  
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Nickname { get; set; }
        public string ProfileImagePath { get; set; } 
        public int FollowersCount { get; set; }
        public int FollowingsCount { get; set; }
    }
}