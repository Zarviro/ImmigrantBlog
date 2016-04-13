using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ImmigrantBlog.WEB.Models
{
    public class CommentModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage="*Поле не должно быть пустым")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        public bool IsPublished { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime PostedOn { get; set; }
        public int? ResponsToCommentId { get; set; }

        public UserModel User { get; set; }

        public int PostId { get; set; }
        public string PostTitle { get; set; }
    }
}