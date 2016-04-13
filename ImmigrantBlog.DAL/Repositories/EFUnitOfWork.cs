using ImmigrantBlog.DAL.EF;
using ImmigrantBlog.DAL.Entities;
using ImmigrantBlog.DAL.Identity;
using ImmigrantBlog.DAL.Interfaces;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImmigrantBlog.DAL.Repositories
{
    public class EFUnitOfWork : IUnitOfWork
    {
        private BlogContext db;

        private PostRepository postRepository;
        private CategoryRepository categoryRepository;
        private TagRepository tagRepository;
        private CommentRepository commentRepository;
        private PostCountHitRepository postCountViewRepository;
        private UserManager userManager;

        public EFUnitOfWork(string connectionString)
        {
            db = new BlogContext(connectionString);
        }

        public IRepository<Post> Posts
        {
            get
            {
                if (postRepository == null)
                    postRepository = new PostRepository(db);
                return postRepository;
            }
        }

        public IRepository<Category> Categories
        {
            get
            {
                if (categoryRepository == null)
                    categoryRepository = new CategoryRepository(db);
                return categoryRepository;
            }
        }

        public IRepository<Tag> Tags
        {
            get
            {
                if (tagRepository == null)
                    tagRepository = new TagRepository(db);
                return tagRepository;
            }
        }

        public IRepository<Comment> Comments
        {
            get
            {
                if (commentRepository == null)
                    commentRepository = new CommentRepository(db);
                return commentRepository;
            }
        }

        public IRepository<PostCountHit> PostCountHits
        {
            get
            {
                if (postCountViewRepository == null)
                    postCountViewRepository = new PostCountHitRepository(db);
                return postCountViewRepository;
            }
        }

        public IUserManager UserManager
        {
            get
            {
                if (userManager == null)
                    userManager = new UserManager(db);
                return userManager;
            }
        }

        public void Save()
        {
            db.SaveChanges();
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
