using ImmigrantBlog.DAL.EF;
using ImmigrantBlog.DAL.Entities;
using ImmigrantBlog.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImmigrantBlog.DAL.Repositories
{
    public class PostRepository : IRepository<Post>
    {
        private BlogContext db;

        public PostRepository(BlogContext context)
        {
            this.db = context;
        }
 
        public IEnumerable<Post> GetAll()
        {
            return db.Posts.Include(c => c.Category).Include(t => t.Tags).Include(u => u.User).Include(c => c.Comments).Include(c => c.CountHits);
        }
 
        public Post Get(int id)
        {
            return db.Posts.Find(id);
        }

        public void Create(Post post)
        {
            db.Posts.Add(post);
        }

        public void Update(Post post)
        {
            db.Entry(post).State = EntityState.Modified;
        }

        public IEnumerable<Post> Find(Func<Post, Boolean> predicate)
        {
            return db.Posts.Include(c => c.Category).Include(t => t.Tags).Include(u => u.User).Where(predicate);
        }
 
        public void Delete(int id)
        {
            Post post = db.Posts.Find(id);
            if (post != null)
                db.Posts.Remove(post);
        }
    }
}
