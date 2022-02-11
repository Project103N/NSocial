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
        public int MyProperty { get; set; }
    }
}