using ImmigrantBlog.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ImmigrantBlog.DAL.Interfaces
{
    public interface IUserManager : IDisposable
    {
        IEnumerable<User> GetAll();
        User Get(string id);
        Task<User> GetAsync(string id);
        IEnumerable<User> Find(Func<User, Boolean> predicate);
        void Create(User user, string password);
        Task CreateAsync(User user, string password);
        void Update(User user);
        void Delete(string id);
        Task DeleteAsync(string id);
        User FindByLogin(string login);
        Task<User> FindByLoginAsync(string login);
        User FindByEmail(string email);
        Task<User> FindByEmailAsync(string email);
        Task<ClaimsIdentity> AuthenticateAsync(string login, string password);
        IEnumerable<string> GetAllRoles();
        IEnumerable<string> GetRoles(string id);
    }
}
