using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace ImmigrantBlog.WEB.Filters
{
    public class AuthorizeClaimsAttribute : AuthorizeAttribute
    {
        public bool IsBlocked { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            ClaimsIdentity claimsIdentity;
            if (!httpContext.User.Identity.IsAuthenticated)
                return false;

            claimsIdentity = httpContext.User.Identity as ClaimsIdentity;
            var isBlockedClaims = claimsIdentity.FindFirst("IsBlocked");
            if (isBlockedClaims == null)
                return false;

            bool isBlocked; // получаем значение
            if (!Boolean.TryParse(isBlockedClaims.Value, out isBlocked))
                return false;

            // проверяем значение
            if (isBlocked != IsBlocked)
                return false;

            // обращаемся к методу базового класса
            return base.AuthorizeCore(httpContext);
        }
    }
}