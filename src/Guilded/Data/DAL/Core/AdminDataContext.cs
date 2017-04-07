using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Guilded.Data.DAL.Abstract;
using Guilded.ViewModels.Core;
using Microsoft.AspNetCore.Identity;
using ApplicationRole = Guilded.Identity.ApplicationRole;

namespace Guilded.Data.DAL.Core
{
    public class AdminDataContext : ReadWriteDataContextBase, IAdminDataContext
    {
        #region Properties
        #region Public properties
        public IPermissionsRepository Permissions => _permissions;
        #endregion

        #region Private Properties
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IPermissionsRepository _permissions;
        #endregion
        #endregion
        

        public AdminDataContext(ApplicationDbContext context,
            RoleManager<ApplicationRole> roleManager,
            IPermissionsRepository permissions) : base(context)
        {
            _roleManager = roleManager;
            _permissions = permissions;
        }

        public IQueryable<ApplicationRole> GetRoles()
        {
            return _roleManager.Roles;
        }

        public ApplicationRole GetRoleById(string id)
        {
            return _roleManager.Roles.FirstOrDefault(r => r.Id == id);
        }

        public Task<ApplicationRole> CreateRoleAsync(string roleName, IEnumerable<Permission> permissions)
        {
            throw new NotImplementedException();
        }

        public async Task<ApplicationRole> UpdateRoleAsync(ApplicationRole roleToUpdate)
        {
            throw new NotImplementedException();
        }
    }
}