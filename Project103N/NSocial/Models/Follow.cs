using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NSocial.Models
{
    public class Follow
    {
        public int FollowerID { get; set; }
        public int FollowingID { get; set; }
        public bool IsAccepted
        {
            get
            {
                return false;
            }
            set { }
        
        }
        public User Follower { get; set; } 
        public User Followed { get; set; }
    }
}