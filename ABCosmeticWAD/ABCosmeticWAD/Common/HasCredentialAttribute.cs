using ABCosmeticWAD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ABCosmeticWAD.Common
{
    public class HasCredentialAttribute : AuthorizeAttribute
    {
        public string Action { get; set; }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var session = (Staff)HttpContext.Current.Session[CommonConstants.USER_SESSION];
            if(session == null)
            {
                return false;
            }
            else
            {
                List<string> privilegeLevels = this.GetCredentialByLoggedInUser();
                if (privilegeLevels.Contains(this.Action))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new ViewResult
            {
                ViewName = "~/Views/Shared/401.cshtml"
            };
        }
        private List<string> GetCredentialByLoggedInUser()
        {
            var credential = (List<string>)HttpContext.Current.Session[CommonConstants.PERMISSION_SESSION];
            return credential;
        }
    }
}