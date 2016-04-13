using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImmigrantBlog.BLL.DTO
{
    public class PostCountHitDTO
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Count { get; set; }

        public int PostId { get; set; }
        //public PostDTO Post { get; set; }
    }
}
