using Abp.MultiTenancy;
using Abp.Zero.Configuration;

namespace App.Caliset.Authorization.Roles
{
    public static class AppRoleConfig
    {
        public static void Configure(IRoleManagementConfig roleManagementConfig)
        {
            // Static host roles

            roleManagementConfig.StaticRoles.Add(
                new StaticRoleDefinition(
                    StaticRoleNames.Host.Administrador,
                    MultiTenancySides.Host
                )
            );

            roleManagementConfig.StaticRoles.Add(
                new StaticRoleDefinition(
                    StaticRoleNames.Host.Operador,
                    MultiTenancySides.Host
                )
            );

            roleManagementConfig.StaticRoles.Add(
                new StaticRoleDefinition(
                    StaticRoleNames.Host.Inspector,
                    MultiTenancySides.Host
                )
            );

            // Static tenant roles

            roleManagementConfig.StaticRoles.Add(
                new StaticRoleDefinition(
                    StaticRoleNames.Tenants.Administrador,
                    MultiTenancySides.Tenant
                )
            );

            roleManagementConfig.StaticRoles.Add(
                new StaticRoleDefinition(
                    StaticRoleNames.Tenants.Operador,
                    MultiTenancySides.Tenant
                )
            );

            roleManagementConfig.StaticRoles.Add(
                new StaticRoleDefinition(
                    StaticRoleNames.Tenants.Inspector,
                    MultiTenancySides.Tenant
                )
            );
        }
    }
}
