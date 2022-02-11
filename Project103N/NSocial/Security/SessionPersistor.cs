using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

// https://github.com/stefaleon/Custom-Auth-in-.NET-MVC

namespace NSocial.Security
{
    public static class SessionPersister
    {
        static string emailSessionVar = "email";
        static int IDSessionVar = -1;

        public static int ID
        {
            get
            {
                if (HttpContext.Current == null)
                {
                    return -1; // return "";
                }
                int sessionVar = Convert.ToInt32(HttpContext.Current.Session[IDSessionVar]);
                if (sessionVar != -1)
                    return sessionVar;
                return -1;
            }
            set
            {
                HttpContext.Current.Session[IDSessionVar] = value;
            }

        }

        public static string Email
        {
            get
            {
                if (HttpContext.Current == null)
                {
                    return string.Empty; // return "";
                }
                var sessionVar = HttpContext.Current.Session[emailSessionVar];
                if (sessionVar != null)
                    return sessionVar as string;
                return null;
            }
            set
            {
                HttpContext.Current.Session[emailSessionVar] = value;
            }
        }
    }
}
