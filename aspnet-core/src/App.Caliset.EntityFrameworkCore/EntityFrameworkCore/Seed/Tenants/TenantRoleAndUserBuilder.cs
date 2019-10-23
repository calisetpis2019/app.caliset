using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Abp.Authorization;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.MultiTenancy;
using App.Caliset.Authorization;
using App.Caliset.Authorization.Roles;
using App.Caliset.Authorization.Users;

namespace App.Caliset.EntityFrameworkCore.Seed.Tenants
{
    public class TenantRoleAndUserBuilder
    {
        private readonly CalisetDbContext _context;
        private readonly int _tenantId;

        public TenantRoleAndUserBuilder(CalisetDbContext context, int tenantId)
        {
            _context = context;
            _tenantId = tenantId;
        }

        public void Create()
        {
            CreateRolesAndUsers();
        }

        private void CreateRolesAndUsers()
        {
            // Admin role

            var adminRole = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == _tenantId && r.Name == StaticRoleNames.Tenants.Administrador);
            if (adminRole == null)
            {
                adminRole = _context.Roles.Add(new Role(_tenantId, StaticRoleNames.Tenants.Administrador, StaticRoleNames.Tenants.Administrador) { IsStatic = true }).Entity;
                _context.SaveChanges();
            }
            var operRole = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == _tenantId && r.Name == StaticRoleNames.Tenants.Operador);
            if (operRole == null)
            {
                operRole = _context.Roles.Add(new Role(_tenantId, StaticRoleNames.Tenants.Operador, StaticRoleNames.Tenants.Operador) { IsStatic = true }).Entity;
                _context.SaveChanges();
                
            }
            var inspRole = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == _tenantId && r.Name == StaticRoleNames.Tenants.Inspector);
            if (inspRole == null)
            {
                inspRole = _context.Roles.Add(new Role(_tenantId, StaticRoleNames.Tenants.Inspector, StaticRoleNames.Tenants.Inspector) { IsStatic = true }).Entity;
                _context.SaveChanges();
            }
            // Grant all permissions to admin role

            var grantedPermissions = _context.Permissions.IgnoreQueryFilters()
                .OfType<RolePermissionSetting>()
                .Where(p => p.TenantId == _tenantId && p.RoleId == adminRole.Id)
                .Select(p => p.Name)
                .ToList();

            var permissions = PermissionFinder
                .GetAllPermissions(new CalisetAuthorizationProvider())
                .Where(p => p.MultiTenancySides.HasFlag(MultiTenancySides.Tenant) &&
                            !grantedPermissions.Contains(p.Name))
                .ToList();

            if (permissions.Any())
            {
                _context.Permissions.AddRange(
                    permissions.Select(permission => new RolePermissionSetting
                    {
                        TenantId = _tenantId,
                        Name = permission.Name,
                        IsGranted = true,
                        RoleId = adminRole.Id
                    })
                );
                _context.Permissions.Add(new RolePermissionSetting
                    {
                        TenantId = _tenantId,
                        Name = "Pages.Operador",
                        IsGranted = true,
                        RoleId = operRole.Id
                    }
                );
                _context.Permissions.Add( new RolePermissionSetting
                    {
                        TenantId = _tenantId,
                        Name = "Pages.Inspector",
                        IsGranted = true,
                        RoleId = operRole.Id
                    }
                );
                _context.Permissions.Add(new RolePermissionSetting
                    {
                        TenantId = _tenantId,
                        Name = "Pages.Inspector",
                        IsGranted = true,
                        RoleId = inspRole.Id
                    }
                );
                _context.SaveChanges();
            }

            // Admin user

            var adminUser = _context.Users.IgnoreQueryFilters().FirstOrDefault(u => u.TenantId == _tenantId && u.UserName == AbpUserBase.AdminUserName);
            if (adminUser == null)
            {
                adminUser = User.CreateTenantAdminUser(_tenantId, "admin@caliset.com");
                adminUser.Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(adminUser, "123qwe");
                adminUser.IsEmailConfirmed = true;
                adminUser.IsActive = true;

                _context.Users.Add(adminUser);
                _context.SaveChanges();

                // Assign Admin role to admin user
                _context.UserRoles.Add(new UserRole(_tenantId, adminUser.Id, adminRole.Id));
                _context.SaveChanges();
            }
        }
    }
}
