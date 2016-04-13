using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImmigrantBlog.WEB.Models
{
    public class CategoryModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UrlSlug { get; set; }
        public string Description { get; set; }
    }
}