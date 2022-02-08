using NSocial.Models;
using NSocial.ModelBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using NSocial.DataAccess;

namespace NSocial.Security
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (string.IsNullOrEmpty(SessionPersister.Email))
                filterContext.Result = new RedirectToRouteResult(new
                    RouteValueDictionary(new
                    {
                        controller = "LoginUser",
                        action = "Login"
                    }));
            else
            {
                // USER LOGGED IN BUT NOT AUTHORISED
                User user = new User();
                //user = UserDAL.Methods.GetByEmail(SessionPersister.Email);
                user = new User();
                CustomPrincipal mp = new CustomPrincipal(user);
                if (!mp.IsInRole(Roles))
                    filterContext.Result = new RedirectToRouteResult(new
                        RouteValueDictionary(new
                        {
                            controller = "User",
                            action = "AuthorizationFailed"
                        }));
            }
        }
    }
}