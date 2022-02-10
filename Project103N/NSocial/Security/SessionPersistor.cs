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
