using UserManagement.Common.Models;
using UserManagement.Manager.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace UserManagement.Manager.Implementations
{
    public class ApplicationRoleStore : IRoleStore<UserRole>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRepository _userRepository;

        public ApplicationRoleStore(
            IRoleRepository roleRepository,
            IUserRepository userRepository)
        { 
            _roleRepository = roleRepository;
            _userRepository = userRepository;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        public async Task<IdentityResult> CreateAsync(UserRole role, CancellationToken cancellationToken)
        {
            var roleId = _roleRepository.AddItem(role);
            return await Task.FromResult(roleId > 0 ? IdentityResult.Success : IdentityResult.Failed());
        }

        public async Task<IdentityResult> UpdateAsync(UserRole role, CancellationToken cancellationToken)
        {
            var success = _roleRepository.UpdateItem(role.Id,role);  
            return await Task.FromResult(success ? IdentityResult.Success : IdentityResult.Failed());
        }

        public async Task<IdentityResult> DeleteAsync(UserRole role, CancellationToken cancellationToken)
        {
            var success = _roleRepository.DeleteItem(role.Id);
            return await Task.FromResult(success ? IdentityResult.Success : IdentityResult.Failed());
        }

        public Task<string> GetRoleIdAsync(UserRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Id.ToString());
        }

        public Task<string> GetRoleNameAsync(UserRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Name);
        }

        public Task SetRoleNameAsync(UserRole role, string roleName, CancellationToken cancellationToken)
        {
            role.Name = roleName;

            return Task.FromResult((object)null);
        }

        public Task<string> GetNormalizedRoleNameAsync(UserRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Name.ToUpper());
        }

        public Task SetNormalizedRoleNameAsync(UserRole role, string normalizedName, CancellationToken cancellationToken)
        {
            return Task.FromResult((object)null);
        }

        public async Task<UserRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            if (int.TryParse(roleId, out var id))
                return await Task.FromResult(_roleRepository.Get(x=>x.Id==id).FirstOrDefault());

            return await Task.FromResult(default(UserRole));
        }

        public async Task<UserRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            return await Task.FromResult(_roleRepository.Get(x => x.Name == normalizedRoleName).FirstOrDefault());
        }

        protected virtual void Dispose(bool disposing)
        {
        }
    }
}
