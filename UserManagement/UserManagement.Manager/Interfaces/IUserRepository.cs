using UserManagement.Common.Models;
using UserManagement.Common.ModelsAPI;
using System.Collections.Generic;

namespace UserManagement.Manager.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        bool UpdateItem(int id, User entity);
        bool DeleteItem(int id);
        bool ItemExists(int id);
        void SaveTransaction(User entity);
        int AddItem(User item);
        bool IsUserInRole(int id, string roleName);
        IEnumerable<User> GetUsersInRole(string roleName);
        UserAPI Authenticate(string username, string password);
        IEnumerable<UserAPI> GetAll();
        User GetById(int id);
        User UpdateUserStatus(UserAPI user);
        UserAPI SaveUser(UserAPI user);
    }
}
