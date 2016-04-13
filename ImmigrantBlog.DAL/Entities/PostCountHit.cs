using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImmigrantBlog.DAL.Entities
{
    public class PostCountHit
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Count { get; set; }

        public int PostId { get; set; }
        public virtual Post Post { get; set; }
    }
}
