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
    public class ApiTagController : ApiController
    {
        private IService<TagDTO> tagService;

        [Inject]
        public IService<TagDTO> TagService
        {
            get { return this.tagService; }
            set { this.tagService = value; }
        }

        // GET: api/ApiTag
        public List<TagModel> Get()
        {
            List<TagModel> tags = Mapper.Map<IEnumerable<TagDTO>, List<TagModel>>(tagService.GetAll().ToList());
            return tags;
        }

        // GET: api/ApiTag/5
        public string Get(int id)
        {
            return null;
        }

        // POST: api/ApiTag
        public HttpResponseMessage Post([FromBody]TagModel value)
        {
            TagDTO category = Mapper.Map<TagModel, TagDTO>(value);
            try 
            {
                tagService.Add(category);
            }
            catch (ValidationException ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, "Error: " + ex.Property + " " + ex.Message);
            }
            return Request.CreateResponse(HttpStatusCode.OK, "Success");
        }

        // PUT: api/ApiTag/5
        public HttpResponseMessage Put(int id, [FromBody]TagModel value)
        {
            value.Id = id;
            TagDTO category = Mapper.Map<TagModel, TagDTO>(value);
            try
            {
                tagService.Update(category);
            }
            catch (ValidationException ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, String.Format("Error: {0} {1}", ex.Property, ex.Message));
            }
            return Request.CreateResponse(HttpStatusCode.OK, "Success");
        }

        // DELETE: api/ApiTag/5
        public HttpResponseMessage Delete(int id)
        {
            tagService.Delete(id);
            return Request.CreateResponse(HttpStatusCode.OK, "Success");
        }
    }
}
