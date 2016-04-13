using ImmigrantBlog.WEB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImmigrantBlog.WEB.ModelViews
{
    public class CommentEditModelView
    {
        public CommentModel Comment { get; set; }
        public List<CommentModel> Responses { get; set; }
    }
}