using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ImmigrantBlog.WEB.Models
{
    public class UserModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public List<string> Roles { get; set; }

        public string AvatarPath { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public bool IsBlocked { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime DateRegister { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime DateLastVisit { get; set; }
        public string LastIP { get; set; }
    }
}