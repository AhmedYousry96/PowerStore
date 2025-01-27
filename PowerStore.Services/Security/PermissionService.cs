using PowerStore.Core;
using PowerStore.Core.Caching;
using PowerStore.Core.Caching.Constants;
using PowerStore.Domain.Customers;
using PowerStore.Domain.Data;
using PowerStore.Domain.Security;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PowerStore.Services.Security
{
    /// <summary>
    /// Permission service
    /// </summary>
    public partial class PermissionService : IPermissionService
    {
        #region Fields

        private readonly IRepository<PermissionRecord> _permissionRecordRepository;
        private readonly IRepository<PermissionAction> _permissionActionRepository;
        private readonly IWorkContext _workContext;
        private readonly ICacheBase _cacheBase;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="permissionRecordRepository">Permission repository</param>
        /// <param name="permissionActionRepository">Permission action repository</param>
        /// <param name="workContext">Work context</param>
        /// <param name="cacheManager">Cache manager</param>
        public PermissionService(
            IRepository<PermissionRecord> permissionRecordRepository,
            IRepository<PermissionAction> permissionActionRepository,
            IWorkContext workContext,
            ICacheBase cacheManager)
        {
            _permissionRecordRepository = permissionRecordRepository;
            _permissionActionRepository = permissionActionRepository;
            _workContext = workContext;
            _cacheBase = cacheManager;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Authorize permission
        /// </summary>
        /// <param name="permissionRecordSystemName">Permission record system name</param>
        /// <param name="customerRole">Customer role</param>
        /// <returns>true - authorized; otherwise, false</returns>
        protected virtual async Task<bool> Authorize(string permissionRecordSystemName, CustomerRole customerRole)
        {
            if (string.IsNullOrEmpty(permissionRecordSystemName))
                return false;

            string key = string.Format(CacheKey.PERMISSIONS_ALLOWED_KEY, customerRole.Id, permissionRecordSystemName);
            return await _cacheBase.GetAsync(key, async () =>
            {
                var permissionRecord = await _permissionRecordRepository.Table.FirstOrDefaultAsync(x => x.SystemName == permissionRecordSystemName);
                return permissionRecord?.CustomerRoles.Contains(customerRole.Id) ?? false;
            });
        }

        #endregion

        #region Methods

        /// <summary>
        /// Delete a permission
        /// </summary>
        /// <param name="permission">Permission</param>
        public virtual async Task DeletePermissionRecord(PermissionRecord permission)
        {
            if (permission == null)
                throw new ArgumentNullException("permission");

            await _permissionRecordRepository.DeleteAsync(permission);

            await _cacheBase.RemoveByPrefix(CacheKey.PERMISSIONS_PATTERN_KEY);
        }

        /// <summary>
        /// Gets a permission
        /// </summary>
        /// <param name="permissionId">Permission identifier</param>
        /// <returns>Permission</returns>
        public virtual Task<PermissionRecord> GetPermissionRecordById(string permissionId)
        {
            return _permissionRecordRepository.GetByIdAsync(permissionId);
        }

        /// <summary>
        /// Gets a permission
        /// </summary>
        /// <param name="systemName">Permission system name</param>
        /// <returns>Permission</returns>
        public virtual async Task<PermissionRecord> GetPermissionRecordBySystemName(string systemName)
        {
            if (String.IsNullOrWhiteSpace(systemName))
                return await Task.FromResult<PermissionRecord>(null);

            var query = from pr in _permissionRecordRepository.Table
                        where pr.SystemName == systemName
                        orderby pr.Id
                        select pr;

            return await query.FirstOrDefaultAsync();
        }

        /// <summary>
        /// Gets all permissions
        /// </summary>
        /// <returns>Permissions</returns>
        public virtual async Task<IList<PermissionRecord>> GetAllPermissionRecords()
        {
            var query = from pr in _permissionRecordRepository.Table
                        orderby pr.Name
                        select pr;
            return await query.ToListAsync();
        }

        /// <summary>
        /// Inserts a permission
        /// </summary>
        /// <param name="permission">Permission</param>
        public virtual async Task InsertPermissionRecord(PermissionRecord permission)
        {
            if (permission == null)
                throw new ArgumentNullException("permission");

            await _permissionRecordRepository.InsertAsync(permission);

            await _cacheBase.RemoveByPrefix(CacheKey.PERMISSIONS_PATTERN_KEY);
        }

        /// <summary>
        /// Updates the permission
        /// </summary>
        /// <param name="permission">Permission</param>
        public virtual async Task UpdatePermissionRecord(PermissionRecord permission)
        {
            if (permission == null)
                throw new ArgumentNullException("permission");

            await _permissionRecordRepository.UpdateAsync(permission);

            await _cacheBase.RemoveByPrefix(CacheKey.PERMISSIONS_PATTERN_KEY);
        }

        /// <summary>
        /// Authorize permission
        /// </summary>
        /// <param name="permission">Permission record</param>
        /// <returns>true - authorized; otherwise, false</returns>
        public virtual async Task<bool> Authorize(PermissionRecord permission)
        {
            return await Authorize(permission, _workContext.CurrentCustomer);
        }

        /// <summary>
        /// Authorize permission
        /// </summary>
        /// <param name="permission">Permission record</param>
        /// <param name="customer">Customer</param>
        /// <returns>true - authorized; otherwise, false</returns>
        public virtual async Task<bool> Authorize(PermissionRecord permission, Customer customer)
        {
            if (permission == null)
                return false;

            if (customer == null)
                return false;

            return await Authorize(permission.SystemName, customer);
        }

        /// <summary>
        /// Authorize permission
        /// </summary>
        /// <param name="permissionRecordSystemName">Permission record system name</param>
        /// <returns>true - authorized; otherwise, false</returns>
        public virtual async Task<bool> Authorize(string permissionRecordSystemName)
        {
            return await Authorize(permissionRecordSystemName, _workContext.CurrentCustomer);
        }

        /// <summary>
        /// Authorize permission
        /// </summary>
        /// <param name="permissionRecordSystemName">Permission record system name</param>
        /// <param name="customer">Customer</param>
        /// <returns>true - authorized; otherwise, false</returns>
        public virtual async Task<bool> Authorize(string permissionRecordSystemName, Customer customer)
        {
            if (String.IsNullOrEmpty(permissionRecordSystemName))
                return false;

            var customerRoles = customer.CustomerRoles.Where(cr => cr.Active);
            foreach (var role in customerRoles)
                if (await Authorize(permissionRecordSystemName, role))
                    //yes, we have such permission
                    return true;

            //no permission found
            return false;
        }

        /// <summary>
        /// Gets a permission action
        /// </summary>
        /// <param name="systemName">Permission system name</param>
        /// <param name="customeroleId">Customer role ident</param>
        /// <returns>Permission action</returns>
        public virtual async Task<IList<PermissionAction>> GetPermissionActions(string systemName, string customeroleId)
        {
            return await _permissionActionRepository.Table
                    .Where(x => x.SystemName == systemName && x.CustomerRoleId == customeroleId).ToListAsync();
        }

        /// <summary>
        /// Inserts a permission action record
        /// </summary>
        /// <param name="permission">Permission</param>
        public virtual async Task InsertPermissionActionRecord(PermissionAction permissionAction)
        {
            if (permissionAction == null)
                throw new ArgumentNullException("permissionAction");

            //insert
            await _permissionActionRepository.InsertAsync(permissionAction);
            //clear cache
            await _cacheBase.RemoveByPrefix(CacheKey.PERMISSIONS_PATTERN_KEY);
        }

        /// <summary>
        /// Inserts a permission action record
        /// </summary>
        /// <param name="permission">Permission</param>
        public virtual async Task DeletePermissionActionRecord(PermissionAction permissionAction)
        {
            if (permissionAction == null)
                throw new ArgumentNullException("permissionAction");

            //delete
            await _permissionActionRepository.DeleteAsync(permissionAction);
            //clear cache
            await _cacheBase.RemoveByPrefix(CacheKey.PERMISSIONS_PATTERN_KEY);
        }

        /// <summary>
        /// Authorize permission for action
        /// </summary>
        /// <param name="permissionRecordSystemName">Permission record system name</param>
        /// <param name="permissionActionName">Permission action name</param>
        /// <returns>true - authorized; otherwise, false</returns>
        public virtual async Task<bool> AuthorizeAction(string permissionRecordSystemName, string permissionActionName)
        {
            if (string.IsNullOrEmpty(permissionRecordSystemName) || string.IsNullOrEmpty(permissionActionName))
                return false;

            if (!await Authorize(permissionRecordSystemName))
                return false;

            var customerRoles = _workContext.CurrentCustomer.CustomerRoles.Where(cr => cr.Active);
            foreach (var role in customerRoles)
            {
                if (!await Authorize(permissionRecordSystemName, role))
                    continue;

                var key = string.Format(CacheKey.PERMISSIONS_ALLOWED_ACTION_KEY, role.Id, permissionRecordSystemName, permissionActionName);
                var permissionAction = await _cacheBase.GetAsync(key, async () =>
                {
                    return await _permissionActionRepository.Table
                        .FirstOrDefaultAsync(x => x.SystemName == permissionRecordSystemName && x.CustomerRoleId == role.Id && x.Action == permissionActionName);
                });
                if (permissionAction != null)
                    return false;
            }

            return true;
        }

        #endregion
    }
}