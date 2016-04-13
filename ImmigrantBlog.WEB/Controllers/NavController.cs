using AutoMapper;
using ImmigrantBlog.BLL.DTO;
using ImmigrantBlog.BLL.Interfaces;
using ImmigrantBlog.WEB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ImmigrantBlog.WEB.Controllers
{
    public class NavController : Controller
    {
        private IPostService postService;
        private IService<CategoryDTO> categoryService;
        private IService<TagDTO> tagService;

        public NavController(IPostService postService, IService<CategoryDTO> categoryService, IService<TagDTO> tagService)
        {
            this.postService = postService;
            this.categoryService = categoryService;
            this.tagService = tagService;
        }

        // GET: Nav/Categories/categoryUrlSlug
        public PartialViewResult Categories(string category = null)
        {
            ViewBag.SelectedCategory = category;
            List<CategoryModel> categories = Mapper.Map<IEnumerable<CategoryDTO>, List<CategoryModel>>(categoryService.GetAll());
            return PartialView(categories);
        }

        // GET: Nav/Tags/tagUrlSlug
        public PartialViewResult Tags(string tag = null)
        {
            ViewBag.SelectedTag = tag;
            List<TagModel> tags = Mapper.Map<IEnumerable<TagDTO>, List<TagModel>>(tagService.GetAll());
            return PartialView(tags);
        }

        // GET: Nav/LatestPosts
        public PartialViewResult LatestPosts()
        {
            List<PostModel> posts = Mapper.Map<IEnumerable<PostDTO>, List<PostModel>>(postService.GetPosts());
            posts = posts.OrderByDescending(p => p.PostedOn).Take(5).ToList();
            return PartialView(posts);
        }

        // GET: Nav/MostReadPosts
        public PartialViewResult MostReadPosts()
        {
            List<PostModel> posts = Mapper.Map<IEnumerable<PostDTO>, List<PostModel>>(postService.GetPosts());
            posts = posts.Where(p => p.IsPublished == true).OrderByDescending(p => p.CountHits.Select(c => c.Count).Sum()).Take(10).ToList();
            return PartialView(posts);
        }
    }
}