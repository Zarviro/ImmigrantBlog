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
    public class ApiCategoryController : ApiController
    {
        private IService<CategoryDTO> categoryService;

        [Inject]
        public IService<CategoryDTO> CategoryService
        {
            get { return this.categoryService; }
            set { this.categoryService = value; }
        }

        // GET: api/ApiCategory
        public List<CategoryModel> Get()
        {
            List<CategoryModel> posts = Mapper.Map<IEnumerable<CategoryDTO>, List<CategoryModel>>(categoryService.GetAll().ToList());
            return posts;
        }

        // GET: api/ApiCategory/5
        public string Get(int id)
        {
            return null;
        }

        // POST: api/ApiCategory
        public HttpResponseMessage Post([FromBody]CategoryModel value)
        {
            CategoryDTO category = Mapper.Map<CategoryModel, CategoryDTO>(value);
            try 
            {
                categoryService.Add(category);
            }
            catch (ValidationException ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, "Error: " + ex.Property + " " + ex.Message);
            }
            return Request.CreateResponse(HttpStatusCode.OK, "Success");
        }

        // PUT: api/ApiCategory/5
        public HttpResponseMessage Put(int id, [FromBody]CategoryModel value)
        {
            value.Id = id;
            CategoryDTO category = Mapper.Map<CategoryModel, CategoryDTO>(value);
            try
            {
                categoryService.Update(category);
            }
            catch (ValidationException ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, String.Format("Error: {0} {1}", ex.Property, ex.Message));
            }
            return Request.CreateResponse(HttpStatusCode.OK, "Success");
        }

        // DELETE: api/ApiCategory/5
        public HttpResponseMessage Delete(int id)
        {
            categoryService.Delete(id);
            return Request.CreateResponse(HttpStatusCode.OK, "Success");
        }
    }
}
