using ImmigrantBlog.DAL.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImmigrantBlog.DAL.EF
{
    public class BlogContext : IdentityDbContext<ApplicationUser>
    {
        static BlogContext()
        {
            Database.SetInitializer<BlogContext>(new ImmigrantBlogDbInitializer());
        }

        public BlogContext(string connectionString)
            : base(connectionString)
        {}

        public DbSet<Post> Posts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<PostCountHit> PostCounHits { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
