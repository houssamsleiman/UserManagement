using UserManagement.Common.Models;
using UserManagement.Common.ModelsAPI;
using UserManagement.Manager.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace UserManagement.Manager.Implementations
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        #region Members 
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRoleRepository _roleRepository; 
        private readonly AppSettings _appSettings;
        #endregion

        #region Constructor 
        public UserRepository(IUnitOfWork unitOfWork,
            IRoleRepository roleRepository,
            IOptions<AppSettings> appSettings) : base(unitOfWork)
        {
            _appSettings = appSettings.Value;
            _unitOfWork = unitOfWork;
            _roleRepository = roleRepository;
        }
        #endregion

        #region Methods 
        public bool UpdateItem(int id, User entity)
        {
            try
            {
                var item = FillItem(id, entity);
                base.Update(item);
                _unitOfWork.Commit();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public bool DeleteItem(int id)
        {
            var item = Get(x => x.Id == id).FirstOrDefault();

            if (item != null)
            {
                Delete(item);
                _unitOfWork.Commit();
                return true;
            }
            return false;
        }

        public bool ItemExists(int id)
        {
            return Get(e => e.Id == id).Any();
        }

        public void SaveTransaction(User item)
        {
            using (var transaction = _unitOfWork._context.Database.BeginTransaction())
            {
                try
                {
                    var user = FillItem(item.Id, item);
                    if (item.Id == 0)
                    {
                        _unitOfWork._context.Add(user);
                    }
                    else
                    {
                        _unitOfWork._context.Update(user);
                    }

                    _unitOfWork._context.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public int AddItem(User item)
        {
            _unitOfWork._context.Add(item);
            _unitOfWork._context.SaveChanges();
            return item.Id;
        }

        private User FillItem(int id, User entity)
        {
            User item;
            if (id != 0)
                item = Get(x => x.Id == id).FirstOrDefault();
            else
                item = new User();

            item.Email = entity.Email;
            item.FirstName = entity.FirstName;
            item.Id = entity.Id;
            item.LastName = entity.LastName;
            item.Password = entity.Password;
            item.PasswordHash = entity.PasswordHash;
            item.Status = entity.Status;
            item.Token = entity.Token;
            item.UserName = entity.UserName;

            return item;
        }

        public bool IsUserInRole(int id, string roleName)
        {
            return _roleRepository.Get(r => r.UserId == id && r.Name == roleName).Any();
        }

        public IEnumerable<User> GetUsersInRole(string roleName)
        {
            var role = _roleRepository.Get(x => x.Name.Contains(roleName)).FirstOrDefault();
            return  Get(x => x.Id == role.UserId);
        }
        #endregion

        #region user service
        public UserAPI Authenticate(string username, string password)
        {
            var user = Get(x => x.UserName == username && x.Password == password).SingleOrDefault();
            var role = _roleRepository.Get(r => r.UserId == user.Id).FirstOrDefault();

            // return null if user not found
            if (user == null)
                return null;
            else if (user.Status == Status.InProgress || user.Status == Status.Reject ||
                user.UserRole.Any(x => x.Name == Role.User))
                return EntityToModel(new List<User> { user }).FirstOrDefault(); 

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, role.Name)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);

            // remove password before returning
            user.Password = null;
            
            return EntityToModel(new List<User> { user }).FirstOrDefault();
        }

        public IEnumerable<UserAPI> GetAll()
        {
            var role = _roleRepository.Get(r => r.Name == Role.User).ToList();
            // return users without passwords
            var users = Get(x => role.Any(r => r.UserId == x.Id)).ToList().Select(x => {
                x.Password = null;
                return x;
            }).ToList();
            return EntityToModel(users);
        }

        public User GetById(int id)
        {
            var user = Get(x => x.Id == id).FirstOrDefault();

            // return user without password
            if (user != null)
                user.Password = null;

            return user;
        }

        public User UpdateUserStatus(UserAPI user)
        {
            var item = GetUser(user.Id);
            item.Status = user.Status;
            _unitOfWork._context.Update(item);
            _unitOfWork._context.SaveChanges();

            // return user without password
            if (item != null)
                item.Password = null;

            return item;
        }

        public UserAPI SaveUser(UserAPI user)
        {
            User item;
            if (user.Id != 0)
                item = GetUser(user.Id);
            else
                item = new User();
            item.Status = user.Status;
            item.FirstName = user.FirstName;
            item.LastName = user.LastName;
            item.UserName = user.UserName;
            item.Email = user.Email;
            item.Password = user.Password; 

            if (user.Id != 0)
                _unitOfWork._context.Update(item);
            else
                _unitOfWork._context.Add(item);

            _unitOfWork.Commit();

            _roleRepository.AddRole(item.Id, Role.User);

            _unitOfWork.Commit();

            // return user without password
            if (item != null)
                item.Password = null;

            return EntityToModel(new List<User> { item }).FirstOrDefault();
        }

        public User GetUser(int id)
        {
            return Get(x => x.Id == id).FirstOrDefault();
        }

        private List<UserAPI> EntityToModel(List<User> users)
        {
            var model = new List<UserAPI>();
            int i = 0;
            foreach (var x in users)
            {
                model.Add(new UserAPI
                {
                    Id=x.Id,
                    FirstName=x.FirstName,
                    LastName=x.LastName,
                    Email=x.Email,
                    Password=x.Password,
                    Token=x.Token,
                    Status =x.Status,
                    UserName=x.UserName, 
                });
                if (x.UserRole?.Count > 0)
                {
                    model[i].UserRole = new List<UserRoleAPI>();
                    foreach(var r in x.UserRole)
                    {
                        model[i].UserRole.Add(new UserRoleAPI {
                            Id = r.Id,
                            Name=r.Name,
                            UserId=r.UserId
                        });
                    }
                }
                i++;
            }
            return model;
        }
        #endregion
    }

}
