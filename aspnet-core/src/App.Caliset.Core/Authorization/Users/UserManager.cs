using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Configuration;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Organizations;
using Abp.Runtime.Caching;
using App.Caliset.Authorization.Roles;
using Abp.Collections.Extensions;
using Abp.Extensions;
using System.Linq.Dynamic.Core;
using App.Caliset.Models.UserDeviceTokens;

namespace App.Caliset.Authorization.Users
{
    public class UserManager : AbpUserManager<Role, User>
    {
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<UserRole, long> _userRoleRepository;

        public UserManager(
            RoleManager roleManager,
            UserStore store, 
            IOptions<IdentityOptions> optionsAccessor, 
            IPasswordHasher<User> passwordHasher, 
            IEnumerable<IUserValidator<User>> userValidators, 
            IEnumerable<IPasswordValidator<User>> passwordValidators,
            ILookupNormalizer keyNormalizer, 
            IdentityErrorDescriber errors, 
            IServiceProvider services, 
            ILogger<UserManager<User>> logger, 
            IPermissionManager permissionManager, 
            IUnitOfWorkManager unitOfWorkManager, 
            ICacheManager cacheManager, 
            IRepository<OrganizationUnit, long> organizationUnitRepository, 
            IRepository<UserOrganizationUnit, long> userOrganizationUnitRepository, 
            IOrganizationUnitSettings organizationUnitSettings, 
            ISettingManager settingManager,
            IRepository<UserRole, long> userRoleRepository
            )
            : base(
                roleManager, 
                store, 
                optionsAccessor, 
                passwordHasher, 
                userValidators, 
                passwordValidators, 
                keyNormalizer, 
                errors, 
                services, 
                logger, 
                permissionManager, 
                unitOfWorkManager, 
                cacheManager,
                organizationUnitRepository, 
                userOrganizationUnitRepository, 
                organizationUnitSettings, 
                settingManager)
        {
            _userRepository = store.UserRepository;
            _userRoleRepository = userRoleRepository;
        }

        public IEnumerable<User> GetAll()
        {
            return _userRepository.GetAllIncluding(x => x.Roles);
        }
   
        public void SetLastLoginTime(long idUser)
        {
            _userRepository.FirstOrDefault(x => x.Id == idUser).LastLoginTime = DateTime.Now;
        }

        public IEnumerable<User> GetAllFilter(string keyword, bool? active)
        {
            return _userRepository.GetAllIncluding(x => x.Roles)
                .WhereIf(!keyword.IsNullOrWhiteSpace(), x => x.UserName.ToUpper().Contains(keyword.ToUpper()) || x.Name.ToUpper().Contains(keyword.ToUpper()) || x.Surname.ToUpper().Contains(keyword.ToUpper()) || x.EmailAddress.ToUpper().Contains(keyword.ToUpper()) )
                .WhereIf(active.HasValue, x => x.IsActive == active); ;
        }

        public void SetUserRole(long IdUSer, int IdRole)
        {
        
            IEnumerable<UserRole> Todos = _userRoleRepository.GetAll();
            
            foreach (UserRole usr_role in Todos)
            {
                if (usr_role.UserId == IdUSer){
                    _userRoleRepository.Delete(usr_role.Id);
                }
            }

            UserRole coso = new UserRole
            {
                RoleId = IdRole,
                UserId = IdUSer
            };

            _userRoleRepository.InsertAsync(coso);
        }
    
    }
}
