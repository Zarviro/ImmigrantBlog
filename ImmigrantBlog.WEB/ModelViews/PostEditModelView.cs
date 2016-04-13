using ImmigrantBlog.WEB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ImmigrantBlog.WEB.ModelViews
{
    public class PostEditModelView
    {
        public PostModel Post { get; set; }
        public SelectList Categories { get; set; }
        public List<SelectListItem> Tags { get; set; }
    }
}