using ImmigrantBlog.DAL.Entities;
using ImmigrantBlog.DAL.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImmigrantBlog.DAL.EF
{
    // DropCreateDatabaseIfModelChanges
    // DropCreateDatabaseAlways
    public class ImmigrantBlogDbInitializer : DropCreateDatabaseIfModelChanges<BlogContext>
    {
        protected override void Seed(BlogContext db)
        {
            // добавляем пользователя
            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            // создаем роли
            var role1 = new IdentityRole { Name = "admin" };
            var role2 = new IdentityRole { Name = "moderator" };
            var role3 = new IdentityRole { Name = "user" };
            // добавляем роли в бд
            roleManager.Create(role1);
            roleManager.Create(role2);
            roleManager.Create(role3);
            // создаем пользователей
            // админ 1
            var admin1 = new ApplicationUser { Email = "admin1@mail.ru", UserName = "admin1" };
            var result = userManager.Create(admin1, "aaa111");
            User userAdmin1 = new User();
            // если создание пользователя прошло успешно
            if (result.Succeeded)
            {
                // добавляем для пользователя роль
                userManager.AddToRole(admin1.Id, role1.Name);
                userManager.AddToRole(admin1.Id, role2.Name);
                userManager.AddToRole(admin1.Id, role3.Name);
                userAdmin1.Id = admin1.Id;
                userAdmin1.Name = "Имя Админ1";
                userAdmin1.Surname = "Фамилия Админ1";
                userAdmin1.Country = "Страна Админ1";
                userAdmin1.City = "Город Админ1";
                userAdmin1.ApplicationUser = admin1;
                // создаем claim
                var identityClaimAdmin1 = new IdentityUserClaim { ClaimType = "IsBlocked", ClaimValue = "false" };
                // добавляем claim
                userAdmin1.ApplicationUser.Claims.Add(identityClaimAdmin1);
                db.Users.Add(userAdmin1);
            }
            // модератор 1
            var moderator1 = new ApplicationUser { Email = "moderator1@mail.ru", UserName = "moderator1" };
            result = userManager.Create(moderator1, "mmm111");
            if (result.Succeeded)
            {
                userManager.AddToRole(moderator1.Id, role2.Name);
                userManager.AddToRole(moderator1.Id, role3.Name);
                var identityClaimModerator1 = new IdentityUserClaim { ClaimType = "IsBlocked", ClaimValue = "false" };
                moderator1.Claims.Add(identityClaimModerator1);
                db.Users.Add(new User { Id = moderator1.Id, ApplicationUser = moderator1, Name = "Имя Модератор1", Country = "Страна 2" });
            }
            // пользователь 1
            var user1 = new ApplicationUser { Email = "user1@mail.ru", UserName = "user1" };
            result = userManager.Create(user1, "uuu111");
            if (result.Succeeded)
            {
                userManager.AddToRole(user1.Id, role3.Name);
                var identityClaimUser1 = new IdentityUserClaim { ClaimType = "IsBlocked", ClaimValue = "false" };
                user1.Claims.Add(identityClaimUser1);
                db.Users.Add(new User { Id = user1.Id, ApplicationUser = user1, Name = "Имя Пользователь1", Country = "Страна 3" });
            }
            // пользователь 2
            var user2 = new ApplicationUser { Email = "user2@mail.ru", UserName = "user2" };
            result = userManager.Create(user2, "uuu222");
            if (result.Succeeded)
            {
                userManager.AddToRole(user2.Id, role3.Name);
                var identityClaimUser2 = new IdentityUserClaim { ClaimType = "IsBlocked", ClaimValue = "false" };
                user2.Claims.Add(identityClaimUser2);
                db.Users.Add(new User { Id = user2.Id, ApplicationUser = user2, Name = "Имя Пользователь2", Country = "Страна 4" });
            }

            Category c1 = new Category { Name = "Категория 1", Description = "category descr 1", UrlSlug = "url-category-1" };
            Category c2 = new Category { Name = "Категория 2", Description = "category descr 2", UrlSlug = "url-category-2" };
            Category c3 = new Category { Name = "Категория 3", Description = "category descr 3", UrlSlug = "url-category-3" };
            db.Categories.Add(c1);
            db.Categories.Add(c2);
            db.Categories.Add(c3);

            Post p1 = new Post { Title = "Post1", ShortDescription = "post descr 1", Description = "long descr 1", Meta = "meta 1", IsPublished = true, PostedOn = DateTime.Now.AddDays(-1), Modified = DateTime.Now.AddDays(-1), Category = c1, User = userAdmin1 };
            Post p2 = new Post { Title = "Post2", ShortDescription = "post descr 2", Description = "long descr 2", Meta = "meta 2", IsPublished = true, PostedOn = DateTime.Now.AddDays(-2), Modified = DateTime.Now.AddDays(-2), Category = c2, User = userAdmin1 };
            Post p3 = new Post { Title = "Post3", ShortDescription = "post descr 3", Description = "long descr 3", Meta = "meta 3", IsPublished = true, PostedOn = DateTime.Now.AddDays(-3), Modified = DateTime.Now.AddDays(-3), Category = c3, User = userAdmin1 };
            Post p4 = new Post { Title = "Post4", ShortDescription = "post descr 4", Description = "long descr 4", Meta = "meta 4", IsPublished = true, PostedOn = DateTime.Now.AddDays(-4), Modified = DateTime.Now.AddDays(-4), Category = c1, User = userAdmin1 };
            Post p5 = new Post { Title = "Post5", ShortDescription = "post descr 5", Description = "long descr 5", Meta = "meta 5", IsPublished = true, PostedOn = DateTime.Now.AddDays(-5), Modified = DateTime.Now.AddDays(-5), Category = c2, User = userAdmin1 };
            Post p6 = new Post { Title = "Post6", ShortDescription = "post descr 6", Description = "long descr 6", Meta = "meta 6", IsPublished = true, PostedOn = DateTime.Now.AddDays(-6), Modified = DateTime.Now.AddDays(-6), Category = c3, User = userAdmin1 };
            Post p7 = new Post { Title = "Post7", ShortDescription = "post descr 7", Description = "long descr 7", Meta = "meta 7", IsPublished = true, PostedOn = DateTime.Now.AddDays(-7), Modified = DateTime.Now.AddDays(-7), Category = c1, User = userAdmin1 };
            Post p8 = new Post { Title = "Post8", ShortDescription = "post descr 8", Description = "long descr 8", Meta = "meta 8", IsPublished = true, PostedOn = DateTime.Now.AddDays(-8), Modified = DateTime.Now.AddDays(-8), Category = c1, User = userAdmin1 };
            Post p9 = new Post { Title = "Post9", ShortDescription = "post descr 9", Description = "long descr 9", Meta = "meta 9", IsPublished = true, PostedOn = DateTime.Now.AddDays(-9), Modified = DateTime.Now.AddDays(-9), Category = c2, User = userAdmin1 };
            Post p10 = new Post { Title = "Post10", ShortDescription = "post descr 10", Description = "long descr 10", Meta = "meta 10", IsPublished = true, PostedOn = DateTime.Now.AddDays(-10), Modified = DateTime.Now.AddDays(-10), Category = c3, User = userAdmin1 };
            Post p11 = new Post { Title = "Post11", ShortDescription = "post descr 11", Description = "long descr 11", Meta = "meta 11", IsPublished = true, PostedOn = DateTime.Now.AddDays(-11), Modified = DateTime.Now.AddDays(-11), Category = c1, User = userAdmin1 };
            Post p12 = new Post { Title = "Post12", ShortDescription = "post descr 12", Description = "long descr 12", Meta = "meta 12", IsPublished = true, PostedOn = DateTime.Now.AddDays(-12), Modified = DateTime.Now.AddDays(-12), Category = c2, User = userAdmin1 };
            Post p13 = new Post { Title = "Post13", ShortDescription = "post descr 13", Description = "long descr 13", Meta = "meta 13", IsPublished = true, PostedOn = DateTime.Now.AddDays(-13), Modified = DateTime.Now.AddDays(-13), Category = c3, User = userAdmin1 };
            Post p14 = new Post { Title = "Post14", ShortDescription = "post descr 14", Description = "long descr 14", Meta = "meta 14", IsPublished = true, PostedOn = DateTime.Now.AddDays(-14), Modified = DateTime.Now.AddDays(-14), Category = c1, User = userAdmin1 };
            db.Posts.Add(p1);
            db.Posts.Add(p2);
            db.Posts.Add(p3);
            db.Posts.Add(p4);
            db.Posts.Add(p5);
            db.Posts.Add(p6);
            db.Posts.Add(p7);
            db.Posts.Add(p8);
            db.Posts.Add(p9);
            db.Posts.Add(p10);
            db.Posts.Add(p11);
            db.Posts.Add(p12);
            db.Posts.Add(p13);
            db.Posts.Add(p14);

            Tag t1 = new Tag { Name = "Tag1", Description = "tag descr 1", UrlSlug = "url 1", Posts = new List<Post>() { p1, p2, p4, p11, p12 } };
            Tag t2 = new Tag { Name = "Tag2", Description = "tag descr 2", UrlSlug = "url 2", Posts = new List<Post>() { p1, p2, p3, p6, p9, p10 } };
            Tag t3 = new Tag { Name = "Tag3", Description = "tag descr 3", UrlSlug = "url 3", Posts = new List<Post>() { p1, p5, p7,p8, p13, p14 } };
            db.Tags.Add(t1);
            db.Tags.Add(t2);
            db.Tags.Add(t3);

            Comment comment1 = new Comment { Description = "this post good", PostedOn = DateTime.Now.AddDays(-2), IsPublished = true, ResponsToCommentId = 1, User = userAdmin1, Post = p1 };
            Comment comment2 = new Comment { Description = "this post bad", PostedOn = DateTime.Now.AddDays(-1), IsPublished = true, ResponsToCommentId = 2, User = userAdmin1, Post = p1 };
            Comment comment3 = new Comment { Description = "this post good or bad", PostedOn = DateTime.Now, IsPublished = true, ResponsToCommentId = 2, User = userAdmin1, Post = p1 };
            db.Comments.Add(comment1);
            db.Comments.Add(comment2);
            db.Comments.Add(comment3);

            PostCountHit count1 = new PostCountHit { Date = DateTime.Now, Count = 10, Post = p1 };
            PostCountHit count2 = new PostCountHit { Date = DateTime.Now.AddDays(-14), Count = 100, Post = p1 };
            db.PostCounHits.Add(count1);
            db.PostCounHits.Add(count2);

            db.SaveChanges();

            // set table db Posts.Category ON DELETE SET NULL
            db.Database.ExecuteSqlCommand("ALTER TABLE dbo.Posts ADD CONSTRAINT Posts_Categories FOREIGN KEY (CategoryId) REFERENCES dbo.Categories (Id) ON DELETE SET NULL");
        }
    }
}
