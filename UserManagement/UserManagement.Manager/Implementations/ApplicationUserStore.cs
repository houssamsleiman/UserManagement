using UserManagement.Common.Models;
using UserManagement.Manager.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace UserManagement.Manager.Implementations
{

    /// <inheritdoc cref="IUserRoleStore{TUser}" />
    /// <summary>
    ///     Application user store
    /// </summary>
    public class ApplicationUserStore : IUserRoleStore<User>, IUserPasswordStore<User>, IUserEmailStore<User>
    {

        private readonly IRoleRepository _roleRepository;
        private readonly IUserRepository _userRepository;

        public ApplicationUserStore( 
            IRoleRepository roleRepository,
            IUserRepository userRepository)
        {
            _roleRepository = roleRepository;
            _userRepository = userRepository;
        }

        public Task SetEmailAsync(User user, string email, CancellationToken cancellationToken)
        {
            user.Email = email;

            return Task.FromResult((object)null);
        }

        public Task<string> GetEmailAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(true);
        }

        public Task SetEmailConfirmedAsync(User user, bool confirmed, CancellationToken cancellationToken)
        {
            _userRepository.SaveTransaction(user); 
            return Task.FromResult((object)null);
        }

        public async Task<User> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            var user = _userRepository.Get(x => x.Email == normalizedEmail).FirstOrDefault();
            return await Task.FromResult(user);
        }

        public Task<string> GetNormalizedEmailAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Email.ToUpper());
        }

        public Task SetNormalizedEmailAsync(User user, string normalizedEmail, CancellationToken cancellationToken)
        {
            return Task.FromResult((object)null);
        }

        public Task SetPasswordHashAsync(User user, string passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;
            return Task.FromResult((object)null);
        }

        public Task<string> GetPasswordHashAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(!string.IsNullOrWhiteSpace(user.PasswordHash));
        }

        public async Task AddToRoleAsync(User user, string roleName, CancellationToken cancellationToken)
        {
            await Task.Run(() => _roleRepository.AddRole(user.Id, roleName), cancellationToken);
        } 

        public async Task RemoveFromRoleAsync(User user, string roleName, CancellationToken cancellationToken)
        { 
            await Task.Run(() => _roleRepository.DeleteItem(user.Id), cancellationToken);
        } 

        public async Task<IList<string>> GetRolesAsync(User user, CancellationToken cancellationToken)
        {
            
            return await Task.FromResult(_roleRepository.Get(x => x.Id == user.Id).Select(x => x.Name).ToList());
        }

        public async Task<bool> IsInRoleAsync(User user, string roleName, CancellationToken cancellationToken)
        {
            return await Task.FromResult(_userRepository.IsUserInRole(user.Id, roleName));
        }

        public async Task<IList<User>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            return await Task.FromResult(_userRepository.GetUsersInRole(roleName).ToList());
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id.ToString());
        }

        public Task<string> GetUserNameAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        public Task SetUserNameAsync(User user, string userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;

            return Task.FromResult((object)null);
        }

        public Task<string> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName.ToUpper());
        }

        public Task SetNormalizedUserNameAsync(User user, string normalizedName, CancellationToken cancellationToken)
        {
            return Task.FromResult((object)null);
        }

        public async Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken)
        {
            var userId = _userRepository.AddItem(user);
            return await Task.FromResult(userId > 0 ? IdentityResult.Success : IdentityResult.Failed());
        }

        public async Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken)
        {
            var success = _userRepository.UpdateItem(user.Id,user);
            return await Task.FromResult(success ? IdentityResult.Success : IdentityResult.Failed());
        }

        public async Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken)
        {
            var success = _userRepository.DeleteItem(user.Id);
            return await Task.FromResult(success ? IdentityResult.Success : IdentityResult.Failed());
        }

        public async Task<User> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            if (int.TryParse(userId, out var id))
                return await Task.FromResult(_userRepository.Get(x=>x.Id== id).FirstOrDefault());

            return await Task.FromResult(default(User));
        }

        public async Task<User> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            return await Task.FromResult(_userRepository.Get(x => x.UserName == normalizedUserName).FirstOrDefault());
        }

        protected virtual void Dispose(bool disposing)
        {
        }
    }
}
