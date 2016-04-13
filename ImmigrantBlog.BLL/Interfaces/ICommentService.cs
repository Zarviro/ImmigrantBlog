using ImmigrantBlog.BLL.DTO;
using ImmigrantBlog.BLL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImmigrantBlog.BLL.Interfaces
{
    public interface ICommentService
    {
        IEnumerable<CommentDTO> GetAll();
        CommentDTO Get(int? id);
        void Add(CommentDTO item);
        void Update(CommentDTO item);
        void Delete(int id);
        int TotalItems { get; }
        IEnumerable<CommentDTO> SearchComments(string searchText, SearchType searchType, int pageNo, int pageSize);
        void Dispose();
    }
}
