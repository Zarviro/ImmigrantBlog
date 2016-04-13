using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImmigrantBlog.BLL.DTO
{
    public class PostDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public string Meta { get; set; }
        public bool IsPublished { get; set; }
        public DateTime PostedOn { get; set; }
        public DateTime? Modified { get; set; }

        public CategoryDTO Category { get; set; }
        public UserDTO User { get; set; }

        public ICollection<TagDTO> Tags { get; set; }
        public ICollection<CommentDTO> Comments { get; set; }
        public ICollection<PostCountHitDTO> CountHits { get; set; }
    }
}
