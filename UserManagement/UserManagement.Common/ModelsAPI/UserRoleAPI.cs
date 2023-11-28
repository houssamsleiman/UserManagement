using UserManagement.Common.Models;

namespace UserManagement.Common.ModelsAPI
{
    public class UserRoleAPI : BaseEntity
    {
        public string Name { get; set; }
        public int UserId { get; set; }
    }
}
