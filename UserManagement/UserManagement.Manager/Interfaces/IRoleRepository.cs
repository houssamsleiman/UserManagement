using UserManagement.Common.Models;

namespace UserManagement.Manager.Interfaces
{
    public interface IRoleRepository :IRepository<UserRole>
    {
        bool UpdateItem(int id, UserRole entity);
        bool DeleteItem(int id);
        bool ItemExists(int id);
        void SaveTransaction(UserRole entity);
        void AddRole(int userId, string name);
        int AddItem(UserRole item);
    }
}
