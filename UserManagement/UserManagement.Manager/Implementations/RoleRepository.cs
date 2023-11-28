using UserManagement.Common.Models;
using UserManagement.Manager.Interfaces;
using System;
using System.Linq;

namespace UserManagement.Manager.Implementations
{
    public class RoleRepository : Repository<UserRole>, IRoleRepository
    {
        #region Members 
        private readonly IUnitOfWork _unitOfWork;
        #endregion

        #region Constructor 
        public RoleRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        #endregion

        #region Methods 
        public bool UpdateItem(int id, UserRole entity)
        {
            try
            {
                var item = FillItem(id, entity);
            base.Update(item);
            _unitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
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

        public void SaveTransaction(UserRole item)
        {

            using (var transaction = _unitOfWork._context.Database.BeginTransaction())
            {
                try
                {
                    UserRole role = FillItem(item.Id, item);
                    if (item.Id == 0)
                    {
                        _unitOfWork._context.Add(role);
                    }
                    else
                    {
                        _unitOfWork._context.Update(role);
                    }

                    _unitOfWork._context.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public int AddItem(UserRole item)
        {
            _unitOfWork._context.Add(item);
            _unitOfWork._context.SaveChanges();
            return item.Id;
        }

        private UserRole FillItem(int id, UserRole entity)
        {
            UserRole item;
            if (id != 0)
                item = Get(x => x.Id == id).FirstOrDefault();
            else
                item = new UserRole();
             
            item.Name = entity.Name; 

            return item;
        }

        public void AddRole(int userId, string name)
        {
            _unitOfWork._context.Add(new UserRole() { UserId = userId, Name = name });
            _unitOfWork._context.SaveChanges();
        }
        #endregion
    }
} 
