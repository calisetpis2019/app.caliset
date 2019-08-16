using Abp.Authorization;
using App.Caliset.Authorization.Roles;
using App.Caliset.Authorization.Users;

namespace App.Caliset.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}
