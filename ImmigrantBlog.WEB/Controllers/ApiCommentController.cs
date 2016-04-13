using AutoMapper;
using ImmigrantBlog.BLL.DTO;
using ImmigrantBlog.BLL.Infrastructure;
using ImmigrantBlog.BLL.Interfaces;
using ImmigrantBlog.WEB.Models;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ImmigrantBlog.WEB.Controllers
{
    public class ApiCommentController : ApiController
    {
        private ICommentService commentService;

        [Inject]
        public ICommentService CommentService
        {
            get { return this.commentService; }
            set { this.commentService = value; }
        }


        // GET: api/ApiComment/IsPublished/true
        [Route("api/ApiComment/IsPublished/{isPublished}")]
        public List<CommentModel> GetComments(bool? isPublished)
        {
            List<CommentModel> comments = Mapper.Map<IEnumerable<CommentDTO>, List<CommentModel>>(commentService.GetAll());
            if (isPublished == true)
                comments = comments.Where(p => p.IsPublished == true).ToList();
            if (isPublished == false)
                comments = comments.Where(p => p.IsPublished == false).ToList();
            return comments;
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

        // PUT: api/ApiComment/SetPublished/5
        [Route("api/ApiComment/SetPublished/{id}")]
        public void PutPublished(int id, [FromBody]bool value)
        {
            CommentDTO comment = commentService.Get(id);
            comment.IsPublished = value;
            commentService.Update(comment);
        }

        // DELETE: api/ApiComment/5
        public void Delete(int id)
        {
            commentService.Delete(id);
        }

        protected override void Dispose(bool disposing)
        {
            commentService.Dispose();
            base.Dispose(disposing);
        }
    }
}
