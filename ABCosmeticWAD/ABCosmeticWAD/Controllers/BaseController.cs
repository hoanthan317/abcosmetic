using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ABCosmeticWAD.Models;
using ABCosmeticWAD.Common;
using System.Web.Routing;

namespace ABCosmeticWAD.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var session = (Staff)Session[CommonConstants.USER_SESSION];
            if (session == null)
            {
                filterContext.Result = new RedirectToRouteResult(new
                    RouteValueDictionary(new { controller = "Login", action = "Login" }));
            }
            base.OnActionExecuting(filterContext);
        }
    }
}