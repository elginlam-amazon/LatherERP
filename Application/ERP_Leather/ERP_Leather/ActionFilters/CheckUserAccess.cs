using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ERP_Leather.ActionFilters
{
    public class CheckUserAccess : ActionFilterAttribute
    {
        private readonly string _urlString;

        public CheckUserAccess(string urlString)
        {
            _urlString = urlString;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var session = filterContext.HttpContext.Session;
            var userUrlPermission = session.GetString("userUrlPermission");

            if (userUrlPermission == null)
            {
                filterContext.Result = new RedirectResult("~/Home/SessionOutMsg");
            }
            else
            {
                var userUrlList = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<string>>(userUrlPermission);

                if (userUrlList != null)
                {
                    var isExists = userUrlList.Any(v => v.ToUpper().Equals(_urlString.ToUpper()));
                    if (isExists) return;
                    filterContext.Result = new RedirectResult("~/Home/login");
                }
                else
                {
                    filterContext.Result = new RedirectResult("~/Home/login");
                }
            }
        }
    }
}
