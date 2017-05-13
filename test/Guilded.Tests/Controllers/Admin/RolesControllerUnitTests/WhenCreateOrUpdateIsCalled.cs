﻿using Guilded.Controllers.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guilded.Identity;
using Guilded.Areas.Admin.ViewModels.Roles;
using Moq;
using Xunit;
using DataModel = Guilded.Identity.ApplicationRole;

namespace Guilded.Tests.Controllers.Admin.RolesControllerUnitTests
{
    public class WhenCreateOrUpdateIsCalled : RolesControllerUnitTestBase
    {
        [Fact]
        public async Task IfRoleIdDoesntExistThenCreateNewRole()
        {
            ApplicationRoleViewModel roleViewModelToCreate = new ApplicationRoleViewModel()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "New Role",
            };
            _mockAdminContext.Setup(db => db.GetRoleById(It.IsAny<string>())).Returns((DataModel) null);
            _mockAdminContext.Setup(db => db.CreateRoleAsync(
                It.IsAny<string>(),
                It.IsAny<IEnumerable<Permission>>()
            )).ReturnsAsync(new DataModel
            {
                Id = Guid.NewGuid().ToString(),
                Name = roleViewModelToCreate.Name
            });

            var result = await Controller.CreateOrUpdate(roleViewModelToCreate);

            var createdRole = AssertResultIsRoleViewModel(result);
            _mockAdminContext.Verify(db => db.CreateRoleAsync(
                It.Is<string>(str => str == roleViewModelToCreate.Name),
                It.IsAny<IEnumerable<Permission>>()
            ));
            _mockAdminContext.Verify(db => db.UpdateRoleAsync(It.IsAny<DataModel>()), Times.Never());
        }

        [Fact]
        public async Task IfRoleIdExistsThenUpdateRole()
        {
            DataModel dbRole = new DataModel
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Existing Role",
                ConcurrencyStamp = Guid.NewGuid().ToString(),
            };
            _mockAdminContext.Setup(db => db.GetRoleById(It.IsAny<string>()))
                .Returns(dbRole);
            _mockAdminContext.Setup(db => db.UpdateRoleAsync(It.IsAny<DataModel>()))
                .Returns((Func<DataModel, Task<DataModel>>)(role => Task.FromResult(role)));
            ApplicationRoleViewModel roleViewModelToUpdate = new ApplicationRoleViewModel(dbRole);
            roleViewModelToUpdate.Name = "Updated Role Name";

            var result = await Controller.CreateOrUpdate(roleViewModelToUpdate);

            var updatedRole = AssertResultIsRoleViewModel(result);
            Assert.Equal(roleViewModelToUpdate.Name, updatedRole.Name);
            _mockAdminContext.Verify(db => db.CreateRoleAsync(
                It.IsAny<string>(),
                It.IsAny<IEnumerable<Permission>>()
            ), Times.Never());
            _mockAdminContext.Verify(db => db.UpdateRoleAsync(
                It.Is<DataModel>(role => role.Id == roleViewModelToUpdate.Id)
            ));
        }

        [Fact]
        public async Task IfConcurrencyStampIsInvalidThenReturnCurrentRole()
        {
            DataModel dbRole = new DataModel
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Existing Role",
                ConcurrencyStamp = Guid.NewGuid().ToString(),
            };
            _mockAdminContext.Setup(db => db.GetRoleById(It.IsAny<string>()))
                .Returns(dbRole);
            ApplicationRoleViewModel roleViewModelToUpdate = new ApplicationRoleViewModel
            {
                Id = dbRole.Id,
                Name = "Updated Role Name",
                ConcurrencyStamp = Guid.Empty.ToString(),
            };

            var result = await Controller.CreateOrUpdate(roleViewModelToUpdate);

            var currentRole = AssertResultIsRoleViewModel(result);
            Assert.Equal(dbRole.Name, currentRole.Name);
            _mockAdminContext.Verify(db => db.CreateRoleAsync(
                It.IsAny<string>(),
                It.IsAny<IEnumerable<Permission>>()
            ), Times.Never());
            _mockAdminContext.Verify(db => db.UpdateRoleAsync(It.IsAny<DataModel>()), Times.Never());
        }
    }
}
