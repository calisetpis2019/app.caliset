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
            ISettingManager settingManager
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
                .WhereIf(!keyword.IsNullOrWhiteSpace(), x => x.UserName.Contains(keyword) || x.Name.Contains(keyword) || x.EmailAddress.Contains(keyword))
                .WhereIf(active.HasValue, x => x.IsActive == active); ;
        }

       
    }
}
