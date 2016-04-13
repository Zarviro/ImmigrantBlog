using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImmigrantBlog.BLL.Enums
{
    public enum SearchType : byte
    {
        Tag,
        Title,
        TitleAndCategory,
        User
    }
}
