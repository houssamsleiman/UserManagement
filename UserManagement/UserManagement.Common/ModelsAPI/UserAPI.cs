using UserManagement.Common.Models;
using System.Collections.Generic;

namespace UserManagement.Common.ModelsAPI
{
    public class UserAPI : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Status Status { get; set; }
        public string Token { get; set; }
        public List<UserRoleAPI> UserRole { get; set; }
    }
}
