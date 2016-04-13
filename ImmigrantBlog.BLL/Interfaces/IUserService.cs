using ImmigrantBlog.BLL.DTO;
using ImmigrantBlog.BLL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ImmigrantBlog.BLL.Interfaces
{
    public interface IUserService : IDisposable
    {
        IEnumerable<UserDTO> GetUsers();
        UserDTO GetUser(string id);
        UserDTO FindUserByLogin(string login);
        Task<UserDTO> FindUserByLoginAsync(string login);
        Task<UserDTO> FindUserByEmailAsync(string email);
        Task<OperationDetails> CreateAsync(UserDTO userDto);
        OperationDetails UpdateUser(UserDTO userDto);
        Task<OperationDetails> UpdateUserAsync(UserDTO userDto);
        Task<OperationDetails> DeleteUserAsync(string id);
        OperationDetails DeleteUser(string id);
        Task<ClaimsIdentity> Authenticate(UserDTO userDto);
        IEnumerable<string> GetAllRoles();
    }
}
