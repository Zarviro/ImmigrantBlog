using ImmigrantBlog.DAL.Entities;
using ImmigrantBlog.DAL.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImmigrantBlog.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Post> Posts { get; }
        IRepository<Category> Categories { get; }
        IRepository<Tag> Tags { get; }
        IRepository<Comment> Comments { get; }
        IRepository<PostCountHit> PostCountHits { get; }
        // User
        IUserManager UserManager { get; }
        //ApplicationUserManager UserManager { get; }
        //ApplicationRoleManager RoleManager { get; }

        void Save();
    }
}
