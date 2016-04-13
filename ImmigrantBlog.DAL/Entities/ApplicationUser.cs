using Microsoft.AspNet.Identity.EntityFramework;

namespace ImmigrantBlog.DAL.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public virtual User User { get; set; }
    }
}
