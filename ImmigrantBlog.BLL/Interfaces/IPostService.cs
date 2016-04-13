using ImmigrantBlog.BLL.DTO;
using ImmigrantBlog.BLL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImmigrantBlog.BLL.Interfaces
{
    public interface IPostService
    {
        PostDTO GetPost(int? id);
        IEnumerable<PostDTO> GetPosts();
        IEnumerable<PostDTO> GetPosts(string category, int pageNo, int pageSize);
        IEnumerable<PostDTO> SearchPosts(string searchText, SearchType searchType, int pageNo, int pageSize);
        int TotalItems { get; }
        void AddPost(PostDTO postDOT);
        void UpdatePost(PostDTO postDOT);
        void DeletePost(int id);
        void RaiseCountHit(int id);
        void Dispose();
    }
}
