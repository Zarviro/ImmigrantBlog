using ImmigrantBlog.BLL.DTO;
using ImmigrantBlog.BLL.Infrastructure;
using ImmigrantBlog.BLL.Interfaces;
using ImmigrantBlog.WEB.Models;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ImmigrantBlog.WEB.Filters
{
    public class LastDateUserVisitAttribute : ActionFilterAttribute
    {
        IUserService userService;

        [Inject]
        public IUserService UserService
        {
            get { return this.userService; }
            set { this.userService = value; }
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            UserDTO currentUser = userService.FindUserByLogin(HttpContext.Current.User.Identity.Name);
            if(currentUser != null)
            {
                currentUser.DateLastVisit = DateTime.Now;
                currentUser.LastIP = HttpContext.Current.Request.UserHostAddress;
                OperationDetails operation = userService.UpdateUser(currentUser);
            }
        }
    }
}