using ImmigrantBlog.DAL.EF;
using ImmigrantBlog.DAL.Entities;
using ImmigrantBlog.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace ImmigrantBlog.DAL.Repositories
{
    public class PostCountHitRepository : IRepository<PostCountHit>
    {
        private BlogContext db;

        public PostCountHitRepository(BlogContext context)
        {
            db = context;
        }

        public IEnumerable<PostCountHit> GetAll()
        {
            return db.PostCounHits;
        }

        public PostCountHit Get(int id)
        {
            return db.PostCounHits.Find(id);
        }

        public IEnumerable<PostCountHit> Find(Func<PostCountHit, Boolean> predicate)
        {
            return db.PostCounHits.Where(predicate).ToList();
        }

        public void Create(PostCountHit postCountHit)
        {
            db.PostCounHits.Add(postCountHit);
        }

        public void Update(PostCountHit postCountHit)
        {
            db.Entry(postCountHit).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            PostCountHit postCountHit = db.PostCounHits.Find(id);
            if (postCountHit != null)
                db.PostCounHits.Remove(postCountHit);
        }
    }
}
