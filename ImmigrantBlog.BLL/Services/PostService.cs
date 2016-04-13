using AutoMapper;
using ImmigrantBlog.BLL.DTO;
using ImmigrantBlog.BLL.Enums;
using ImmigrantBlog.BLL.Infrastructure;
using ImmigrantBlog.BLL.Interfaces;
using ImmigrantBlog.DAL.Entities;
using ImmigrantBlog.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImmigrantBlog.BLL.Services
{
    public class PostService : IPostService
    {
        IUnitOfWork Database { get; set; }

        public PostService(IUnitOfWork uow)
        {
            Database = uow;
            AutoMapperConfig.RegisterMappings();
        }

        public int TotalItems { get; private set;}

        public PostDTO GetPost(int? id)
        {
            if (id == null)
            {
                throw new ValidationException("Не установлено id поста", "");
            }
            var post = Database.Posts.Get(id.Value);
            if (post == null)
            {
                throw new ValidationException("Пост не найден", "");
            }
            post.Comments = post.Comments.OrderBy(d => d.PostedOn).ToList();
            return Mapper.Map<Post, PostDTO>(post);
        }

        public IEnumerable<PostDTO> GetPosts()
        {
            List<Post> postsDb = Database.Posts.GetAll().OrderByDescending(p => p.PostedOn).ToList();
            List<PostDTO> posts = Mapper.Map<IEnumerable<Post>, List<PostDTO>>(postsDb);
            return posts;
        }

        public IEnumerable<PostDTO> GetPosts(string category, int pageNo, int pageSize)
        {
            List<Post> postsDb = Database.Posts.Find(p => p.IsPublished && (category == null || p.Category.UrlSlug == category)).ToList();
            TotalItems = postsDb.Count();
            postsDb = postsDb
                .OrderByDescending(p => p.PostedOn)
                .Skip((pageNo - 1)*pageSize)
                .Take(pageSize)
                .ToList();
            List<PostDTO> posts = Mapper.Map<IEnumerable<Post>, List<PostDTO>>(postsDb);
            return posts;
        }

        public IEnumerable<PostDTO> SearchPosts(string search, SearchType searchType, int pageNo, int pageSize)
        {
            List<Post> postsDb = new List<Post>();
            switch(searchType)
            {
                case SearchType.Title:
                    postsDb = Database.Posts.Find(p => p.IsPublished && (p.Title.ToLower().Contains(search.ToLower()))).ToList();
                    break;
                case SearchType.TitleAndCategory:
                    postsDb = Database.Posts.Find(p => p.IsPublished && (p.Title.ToLower().Contains(search.ToLower()) || p.Category.Name.ToLower().Equals(search.ToLower()))).ToList();
                    break;
                case SearchType.Tag:
                    Tag tagDb = Database.Tags.Find(t => t.UrlSlug.ToLower().Equals(search.ToLower())).FirstOrDefault();
                    postsDb = Database.Posts.Find(p => p.IsPublished && p.Tags.Contains(tagDb)).ToList();
                    break;
                case SearchType.User:
                    postsDb = Database.Posts.Find(p => p.UserId == search).ToList();
                    break;
                default:
                    break;
            }
            TotalItems = postsDb.Count();
            postsDb = postsDb
                .OrderByDescending(p => p.PostedOn)
                .Skip((pageNo - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            List<PostDTO> posts = Mapper.Map<IEnumerable<Post>, List<PostDTO>>(postsDb);
            return posts;
        }

        public void AddPost(PostDTO postDTO)
        {
            if (postDTO.Tags == null || postDTO.Tags.Count <= 0 || postDTO.Tags.Count > 5)
            {
                throw new ValidationException("", "Tags");
            }
            Post post = Mapper.Map<PostDTO, Post>(postDTO);
            List<Tag> tags = Database.Tags.GetAll().ToList();
            var ids = postDTO.Tags.Select(s => s.Id);
            post.Tags = tags.Where(t => ids.Contains(t.Id)).ToList();
            post.PostedOn = DateTime.Now;
            Database.Posts.Create(post);
            Database.Save();
        }

        public void UpdatePost(PostDTO postDto)
        {
            if (postDto.Tags == null || postDto.Tags.Count <= 0 || postDto.Tags.Count > 5)
            {
                throw new ValidationException("", "Tags");
            }
            Post post = Database.Posts.Get(postDto.Id);
            if (postDto.Description != post.Description)
                post.Modified = DateTime.Now;
            post.Title = postDto.Title;
            post.ShortDescription = postDto.ShortDescription;
            post.Description = postDto.Description;
            post.Meta = postDto.Meta;
            post.IsPublished = postDto.IsPublished;
            post.CategoryId = postDto.Category.Id;
            List<Tag> tags = Database.Tags.GetAll().ToList();
            var ids = postDto.Tags.Select(s => s.Id);
            post.Tags = tags.Where(t => ids.Contains(t.Id)).ToList();
            Database.Posts.Update(post);
            Database.Save();
        }

        public void DeletePost(int id)
        {
            Database.Posts.Delete(id);
            Database.Save();
        }

        public void RaiseCountHit(int id)
        {
            PostCountHit countHits = Database.PostCountHits.Find(c => c.Post.Id == id && c.Date.ToShortDateString() == DateTime.Now.ToShortDateString()).FirstOrDefault();
            if (countHits == null)
            {
                Database.PostCountHits.Create(new PostCountHit() { PostId = id, Date = DateTime.Now, Count = 1 });
                Database.Save();
            }
            else
            {
                countHits.Count++;
                Database.PostCountHits.Update(countHits);
                Database.Save();
            }
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
