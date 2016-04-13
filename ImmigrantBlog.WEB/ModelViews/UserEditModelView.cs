using ImmigrantBlog.WEB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ImmigrantBlog.WEB.ModelViews
{
    public class UserEditModelView
    {
        public UserModel User { get; set; }
        public IEnumerable<SelectListItem> Roles { get; set; }
    }
}