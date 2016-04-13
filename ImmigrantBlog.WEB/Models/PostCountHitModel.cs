using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImmigrantBlog.WEB.Models
{
    public class PostCountHitModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Count { get; set; }

        public int PostId { get; set; }
    }
}