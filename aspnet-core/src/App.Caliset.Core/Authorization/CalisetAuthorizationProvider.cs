using Abp.Authorization;
using Abp.Localization;
using Abp.MultiTenancy;

namespace App.Caliset.Authorization
{
    public class CalisetAuthorizationProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            /*context.CreatePermission(PermissionNames.Pages_Users, L("Users"));
            context.CreatePermission(PermissionNames.Pages_Roles, L("Roles"));
            context.CreatePermission(PermissionNames.Pages_Tenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);*/

            context.CreatePermission(PermissionNames.Administrador, L("Administrador"));
            context.CreatePermission(PermissionNames.Operador, L("Operador"));
            context.CreatePermission(PermissionNames.Inspector, L("Inspector"));
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, CalisetConsts.LocalizationSourceName);
        }
    }
}
