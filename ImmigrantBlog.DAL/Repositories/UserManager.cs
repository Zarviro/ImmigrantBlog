using ImmigrantBlog.DAL.EF;
using ImmigrantBlog.DAL.Entities;
using ImmigrantBlog.DAL.Identity;
using ImmigrantBlog.DAL.Interfaces;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ImmigrantBlog.DAL.Repositories
{
    public class UserManager : IUserManager
    {
        private BlogContext db;
        private ApplicationUserManager identityUserManager;

        public UserManager(BlogContext context)
        {
            this.db = context;
            identityUserManager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));
        }

        public IEnumerable<User> GetAll()
        {
            IEnumerable<User> users = db.Users;
            foreach(var user in users.ToList())
                user.Roles = identityUserManager.GetRoles(user.Id);
            return users;
        }

        public User Get(string id)
        {
            User user = db.Users.Find(id);
            if(user != null)
                user.Roles = identityUserManager.GetRoles(user.Id);
            return user;
        }

        public async Task<User> GetAsync(string id)
        {
            User user = await db.Users.FindAsync(id);
            user.Roles = await identityUserManager.GetRolesAsync(user.Id);
            return user;
        }

        public User FindByLogin(string login)
        {
            var user = identityUserManager.FindByName(login);
            if (user != null)
                return Get(user.Id);
            else
                return null;
        }

        public async Task<User> FindByLoginAsync(string login)
        {
            var user = await identityUserManager.FindByNameAsync(login);
            if (user != null)
                return Get(user.Id);
            else
                return null;
        }

        public User FindByEmail(string email)
        {
            var user = identityUserManager.FindByEmail(email);
            if (user != null)
                return Get(user.Id);
            else
                return null;
        }

        public async Task<User> FindByEmailAsync(string email)
        {
            var user = await identityUserManager.FindByEmailAsync(email);
            if (user != null)
                return Get(user.Id);
            else
                return null;
        }

        public void Create(User user, string password)
        {
            identityUserManager.Create(user.ApplicationUser, password);
            // добавляем роли
            foreach (var role in user.Roles)
                identityUserManager.AddToRole(user.Id, role);
            // добавляем claim
            var isBlockedClaim = new IdentityUserClaim { ClaimType = "IsBlocked", ClaimValue = "false" };
            user.ApplicationUser.Claims.Add(isBlockedClaim);
            db.Users.Add(user);
        }

        public async Task CreateAsync(User user, string password)
        {
            await identityUserManager.CreateAsync(user.ApplicationUser, password);
            // добавляем роли
            foreach (var role in user.Roles)
                await identityUserManager.AddToRoleAsync(user.ApplicationUser.Id, role);
            // добавляем claim
            var isBlockedClaim = new IdentityUserClaim { ClaimType = "IsBlocked", ClaimValue = "false" };
            user.ApplicationUser.Claims.Add(isBlockedClaim);
            db.Users.Add(user);
        }

        public void Update(User user)
        {
            // удаляем все роли
            foreach (string role in GetAllRoles().ToList())
            {
                if (identityUserManager.IsInRole(user.Id, role))
                    identityUserManager.RemoveFromRole(user.Id, role);
            }
            // добавляем роли
            foreach (string role in user.Roles)
            {
                identityUserManager.AddToRole(user.Id, role);
            }
            // обновляем claim
            user.ApplicationUser.Claims.FirstOrDefault(c => c.ClaimType == "IsBlocked").ClaimValue = user.IsBlocked.ToString();
            db.Entry(user).State = EntityState.Modified;
        }

        public IEnumerable<User> Find(Func<User, Boolean> predicate)
        {
            return db.Users.Where(predicate).ToList();
        }

        public void Delete(string id)
        {
            User user = db.Users.Find(id);
            ApplicationUser userApplication = identityUserManager.FindById(id);
            if (user != null)
                db.Users.Remove(user);
            if(userApplication != null)
                identityUserManager.Delete(userApplication);
        }

        public async Task DeleteAsync(string id)
        {
            User user = db.Users.Find(id);
            if (user != null)
                await identityUserManager.DeleteAsync(user.ApplicationUser);
            db.Users.Remove(user);
        }

        public IEnumerable<string> GetAllRoles()
        {
            return db.Roles.Select(n => n.Name).ToList();
        }

        public IEnumerable<string> GetRoles(string id)
        {
            return identityUserManager.GetRoles(id);
        }

        public async Task<ClaimsIdentity> AuthenticateAsync(string login, string password)
        {
            ClaimsIdentity claim = null;
            // находим пользователя
            ApplicationUser user = await identityUserManager.FindAsync(login, password);
            // авторизуем его и возвращаем объект ClaimsIdentity
            if (user != null)
            {
                claim = await identityUserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
                // добавляем claim IsBlocked
                var isBlockedClaim = user.Claims.FirstOrDefault(c => c.ClaimType == "IsBlocked");
                if (isBlockedClaim != null)
                    claim.AddClaim(new Claim(isBlockedClaim.ClaimType, isBlockedClaim.ClaimValue));
            }
            return claim;
        }

        public void Dispose()
        {
            identityUserManager.Dispose();
        }
    }
}
