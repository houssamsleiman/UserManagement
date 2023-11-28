using UserManagement.Common;
using UserManagement.Manager.Interfaces;

namespace UserManagement.Manager.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        #region Members
        public UserManagementContext _context { get; }
        #endregion

        #region Constructor 
        public UnitOfWork(UserManagementContext context)
        {
            _context = context;
        }
        #endregion

        #region Methods
        public void Commit()
        {
            _context.SaveChanges();
        }

        public void CommitAsync()
        {
            _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
        #endregion

    }
}
