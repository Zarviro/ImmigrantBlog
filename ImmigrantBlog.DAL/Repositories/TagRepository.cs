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
    public class TagRepository : IRepository<Tag>
    {
        private BlogContext db;

        public TagRepository(BlogContext context)
        {
            this.db = context;
        }
 
        public IEnumerable<Tag> GetAll()
        {
            return db.Tags.Include(p => p.Posts);
        }

        public Tag Get(int id)
        {
            return db.Tags.Find(id);
        }

        public void Create(Tag tag)
        {
            db.Tags.Add(tag);
        }

        public void Update(Tag tag)
        {
            db.Entry(tag).State = EntityState.Modified;
        }

        public IEnumerable<Tag> Find(Func<Tag, Boolean> predicate)
        {
            return db.Tags.Include(p => p.Posts).Where(predicate).ToList();
        }
 
        public void Delete(int id)
        {
            Tag tag = db.Tags.Find(id);
            if (tag != null)
                db.Tags.Remove(tag);
        }
    }
}
