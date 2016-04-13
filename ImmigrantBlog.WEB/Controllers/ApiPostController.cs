using AutoMapper;
using ImmigrantBlog.BLL.DTO;
using ImmigrantBlog.BLL.Infrastructure;
using ImmigrantBlog.BLL.Interfaces;
using ImmigrantBlog.WEB.Models;
using Newtonsoft.Json;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ImmigrantBlog.WEB.Controllers
{
    public class ApiPostController : ApiController
    {
        private IPostService postService;

        [Inject]
        public IPostService PostService
        {
            get { return this.postService; }
            set { this.postService = value; }
        }


        // GET: api/ApiPost/IsPublished/true
        public List<PostModel> GetPosts(bool? isPublished)
        {
            List<PostModel> posts = Mapper.Map<IEnumerable<PostDTO>, List<PostModel>>(postService.GetPosts());
            if (isPublished == true)
                posts = posts.Where(p => p.IsPublished == true).ToList();
            if (isPublished == false)
                posts = posts.Where(p => p.IsPublished == false).ToList();
            return posts;
        }

        // GET: api/ApiPost/5
        public string Get(int id)
        {
            return null;
        }

        // POST: api/ApiPost
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/ApiPost/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // PUT: api/ApiPost/SetPublished/5
        [Route("api/ApiPost/SetPublished/{id}")]
        public void PutPublished(int id, [FromBody]bool value)
        {
            PostDTO post = postService.GetPost(id);
            post.IsPublished = value;
            postService.UpdatePost(post);
        }

        // DELETE: api/ApiPost/5
        public void Delete(int id)
        {
            postService.DeletePost(id);
        }

        protected override void Dispose(bool disposing)
        {
            postService.Dispose();
            base.Dispose(disposing);
        }
    }
}
