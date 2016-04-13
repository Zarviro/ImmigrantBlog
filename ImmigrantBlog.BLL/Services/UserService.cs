using AutoMapper;
using ImmigrantBlog.BLL.DTO;
using ImmigrantBlog.BLL.Infrastructure;
using ImmigrantBlog.BLL.Interfaces;
using ImmigrantBlog.DAL.Entities;
using ImmigrantBlog.DAL.Interfaces;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ImmigrantBlog.BLL.Services
{
    public class UserService : IUserService
    {
        IUnitOfWork Database { get; set; }

        public UserService(IUnitOfWork uow)
        {
            Database = uow;
        }

        public IEnumerable<UserDTO> GetUsers()
        {
            List<User> users = Database.UserManager.GetAll().ToList();
            List<UserDTO> usersDto = Mapper.Map<IEnumerable<User>, List<UserDTO>>(users);
            return usersDto;
        }

        public UserDTO GetUser(string id)
        {
            if (id == null)
            {
                throw new ValidationException("Не установлено id пользователя", "");
            }
            var user = Database.UserManager.Get(id);
            if (user == null)
            {
                throw new ValidationException("Пользователь не найден", "");
            }
            UserDTO userDto = Mapper.Map<User, UserDTO>(user);
            return userDto;
        }

        public UserDTO FindUserByLogin(string login)
        {
            User user = Database.UserManager.FindByLogin(login);
            if (user != null)
            {
                UserDTO userDto = Mapper.Map<User, UserDTO>(user);
                return userDto;
            }
            return null;
        }

        public async Task<UserDTO> FindUserByLoginAsync(string login)
        {
            User user = await Database.UserManager.FindByLoginAsync(login);
            if (user != null)
            {
                UserDTO userDto = Mapper.Map<User, UserDTO>(user);
                return userDto;
            }
            return null;
        }


        public async Task<UserDTO> FindUserByEmailAsync(string email)
        {
            User user = await Database.UserManager.FindByEmailAsync(email);
            if (user != null)
            {
                UserDTO userDto = Mapper.Map<User, UserDTO>(user);
                return userDto;
            }
            return null;
        }

        public async Task<OperationDetails> CreateAsync(UserDTO userDto)
        {
            User user = await Database.UserManager.FindByEmailAsync(userDto.Email);
            if (user == null)
            {
                user = await Database.UserManager.FindByLoginAsync(userDto.Login);
                if (user == null)
                {
                    // проверяем IP, что бы не был заблокированным
                    if(Database.UserManager.GetAll().FirstOrDefault(u => u.LastIP == userDto.LastIP).IsBlocked)
                    {
                        return new OperationDetails(false, "Вы не можете зарегестрироваться. Ваш IP: " + userDto.LastIP.ToString() + " заблокирован.", "");
                    }
                    else
                    {
                        user = Mapper.Map<UserDTO, User>(userDto);
                        user.ApplicationUser = new ApplicationUser() { Email = userDto.Email, UserName = userDto.Login };
                        user.DateRegister = DateTime.Now;
                        user.DateLastVisit = DateTime.Now;
                        await Database.UserManager.CreateAsync(user, userDto.Password);
                        Database.Save();
                        return new OperationDetails(true, "Регистрация успешно пройдена", "");
                    }
                }
                else
                {
                    return new OperationDetails(false, "Пользователь с таким логином уже существует", "Login");
                }
            }
            else
            {
                return new OperationDetails(false, "Пользователь с таким email уже существует", "Email");
            }
        }

        public OperationDetails UpdateUser(UserDTO userDto)
        {
            User user = Database.UserManager.FindByLogin(userDto.Login);
            if (user != null)
            {
                user.AvatarPath = userDto.AvatarPath;
                user.Name = userDto.Name;
                user.Surname = userDto.Surname;
                user.Country = userDto.Country;
                user.City = userDto.City;
                user.IsBlocked = userDto.IsBlocked;
                user.Roles = userDto.Roles;
                user.DateLastVisit = userDto.DateLastVisit;
                user.LastIP = userDto.LastIP;
                Database.UserManager.Update(user);
                Database.Save();
                return new OperationDetails(true, "Изменения сохранены", "");
            }
            else
            {
                return new OperationDetails(false, "Ошибка обновления пользователя", "");
            }
        }

        public async Task<OperationDetails> UpdateUserAsync(UserDTO userDto)
        {
            User user = await Database.UserManager.FindByLoginAsync(userDto.Login);
            if (user != null)
            {
                user.AvatarPath = userDto.AvatarPath;
                user.Name = userDto.Name;
                user.Surname = userDto.Surname;
                user.Country = userDto.Country;
                user.City = userDto.City;
                user.IsBlocked = userDto.IsBlocked;
                user.Roles = userDto.Roles;
                user.DateLastVisit = userDto.DateLastVisit;
                user.LastIP = userDto.LastIP;
                Database.UserManager.Update(user);
                Database.Save();
                return new OperationDetails(true, "Изменения сохранены", "");
            }
            else
            {
                return new OperationDetails(false, "Ошибка обновления пользователя", "");
            }
        }

        public OperationDetails DeleteUser(string id)
        {
            User user = Database.UserManager.Get(id);
            if (user != null)
            {
                Database.UserManager.Delete(id);
                Database.Save();
                return new OperationDetails(true, "Пользователь удален", "");
            }
            else
            {
                return new OperationDetails(false, "Ошибка удаления пользователя", "");
            }
        }

        public async Task<OperationDetails> DeleteUserAsync(string id)
        {
            User user = await Database.UserManager.GetAsync(id);
            if (user != null)
            {
                await Database.UserManager.DeleteAsync(id);
                Database.Save();
                return new OperationDetails(true, "Пользователь удален", "");
            }
            else
            {
                return new OperationDetails(false, "Ошибка удаления пользователя", "");
            }
        }

        public async Task<ClaimsIdentity> Authenticate(UserDTO userDto)
        {

            ClaimsIdentity claim = await Database.UserManager.AuthenticateAsync(userDto.Login, userDto.Password);
            return claim;
        }

        public IEnumerable<string> GetAllRoles()
        {
            return Database.UserManager.GetAllRoles();
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
