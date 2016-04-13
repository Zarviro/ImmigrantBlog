using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImmigrantBlog.DAL.Entities
{
    public class User
    {
        [Key]
        [ForeignKey("ApplicationUser")]
        public string Id { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public IEnumerable<string> Roles { get; set; }

        public string AvatarPath { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public bool IsBlocked { get; set; }
        public DateTime? DateRegister { get; set; }
        public DateTime? DateLastVisit { get; set; }
        public string LastIP { get; set; }
    }
}
