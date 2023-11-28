using System;
using UserManagement.Common;

namespace UserManagement.Manager.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        UserManagementContext _context { get; }
        void Commit();
    }
}
