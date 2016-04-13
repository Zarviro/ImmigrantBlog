using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImmigrantBlog.DAL.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public bool IsPublished { get; set; }
        public DateTime PostedOn { get; set; }
        public int? ResponsToCommentId { get; set; }

        public string UserId { get; set; }
        public virtual User User { get; set; }

        public int PostId { get; set; }
        public virtual Post Post { get; set; }
    }
}
