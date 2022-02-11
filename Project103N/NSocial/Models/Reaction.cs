using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NSocial.Models
{
    public class Reaction
    {
        public int ID { get; set; }
        public int PostID { get; set; }
        public int UserID { get; set; }
        public int ReactionType { get; set; }
    }
}