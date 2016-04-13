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
    public class ApiUserController : ApiController
    {
        private IUserService userService;

        [Inject]
        public IUserService UserService
        {
            get { return this.userService; }
            set { this.userService = value; }
        }


        // GET: api/ApiUser/true
        [Route("api/ApiUser/IsBlocked/{isBlocked}")]
        public List<UserModel> GetUsers(bool? isBlocked)
        {
            List<UserModel> users = Mapper.Map<IEnumerable<UserDTO>, List<UserModel>>(userService.GetUsers());
            if (isBlocked == true)
                users = users.Where(p => p.IsBlocked == true).ToList();
            if (isBlocked == false)
                users = users.Where(p => p.IsBlocked == false).ToList();
            return users;
        }

        // GET: api/ApiUser/5
        public string Get(int id)
        {
            return null;
        }

        // POST: api/ApiUser
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/ApiUser/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // PUT: api/ApiUser/SetBlocked/5
        [Route("api/ApiUser/SetBlocked/{id}")]
        public string PutBlocked(string id, [FromBody]bool value)
        {
            string errorMsg = string.Empty;
            UserDTO user = userService.GetUser(id);
            user.IsBlocked = value;
            try
            {
                userService.UpdateUser(user);
            }
            catch (ValidationException ex)
            {
                errorMsg = "Property: " + ex.Property + "\nError: " + ex.Message;
            }
            return errorMsg;
        }

        //// PUT: api/ApiUser/EditRoles/5
        //[Route("api/ApiUser/EditRoles/{id}")]
        //public string PutRoles(string id, [FromBody]string[] selectedRoles)
        //{
        //    string errorMsg = string.Empty;
        //    UserDTO user = userService.GetUser(id);
        //    if (selectedRoles != null)
        //        user.Roles = selectedRoles.ToList();
        //    try
        //    {
        //        userService.UpdateUser(user);
        //    }
        //    catch (ValidationException ex)
        //    {
        //        errorMsg = "Property: " + ex.Property + "\nError: " + ex.Message;
        //    }
        //    return errorMsg;
        //}

        // DELETE: api/ApiUser/5
        public void Delete(string id)
        {
            userService.DeleteUser(id);
        }

        protected override void Dispose(bool disposing)
        {
            userService.Dispose();
            base.Dispose(disposing);
        }
    }
}
