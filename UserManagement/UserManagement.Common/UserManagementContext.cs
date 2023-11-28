using Microsoft.EntityFrameworkCore;
using UserManagement.Common.Models;

namespace UserManagement.Common
{
    public class UserManagementContext : DbContext
    {
        #region Members  
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        #endregion

        #region Constructor
        public UserManagementContext(DbContextOptions<UserManagementContext> options) : base(options)
        { }
        #endregion

        #region Methods  
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
        #endregion


    }
}
