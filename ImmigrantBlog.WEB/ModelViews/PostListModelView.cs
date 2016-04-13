using ImmigrantBlog.WEB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImmigrantBlog.WEB.ModelViews
{
    public class PostListModelView
    {
        public IEnumerable<PostModel> Posts { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public string CurrentSearch { get; set; }
    }
}