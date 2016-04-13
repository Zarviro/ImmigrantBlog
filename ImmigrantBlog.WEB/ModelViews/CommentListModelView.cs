using ImmigrantBlog.WEB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImmigrantBlog.WEB.ModelViews
{
    public class CommentListModelView
    {
        public IEnumerable<CommentModel> Comments { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public string CurrentSearch { get; set; }
    }
}