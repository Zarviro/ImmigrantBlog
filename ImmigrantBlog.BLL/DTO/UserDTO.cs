using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImmigrantBlog.BLL.DTO
{
    public class UserDTO
    {
        public string Id { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<string> Roles { get; set; }

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
