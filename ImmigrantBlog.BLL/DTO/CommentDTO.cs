using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImmigrantBlog.BLL.DTO
{
    public class CommentDTO
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public bool IsPublished { get; set; }
        public DateTime PostedOn { get; set; }
        public int? ResponsToCommentId { get; set; }

        public UserDTO User { get; set; }

        public int PostId { get; set; }
        public PostDTO Post { get; set; }
    }
}
