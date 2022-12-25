using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DemoApplication.Attributes
{
    public class IsAuthenticated : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                RedirectToDashboard(context);
            }
        }

        private void RedirectToDashboard(ActionExecutingContext filterContext)
        {
            var redirectTarget = new RouteValueDictionary
            {
                {"controller" , "Account" },
                {"action" , "Dashboard" }
            };

            filterContext.Result = new RedirectToRouteResult(redirectTarget);
        }
    }
}