using System;
using System.Collections.Generic;
using Abp.Authorization.Users;
using Abp.Extensions;
using App.Caliset.Models.Assignations;
using App.Caliset.Models.Operations;

namespace App.Caliset.Authorization.Users
{
    public class User : AbpUser<User>
    {
        public const string DefaultPassword = "123qwe";
        public string LastName { get; set; }
        public int Document { get; set; }
        public string Phone { get; set; }
        public DateTime BirthDate { get; set; }
        public string City { get; set; }
        public string Adress { get; set; }

        public bool FirstLogin { get; set; }

        public virtual ICollection<Assignation> Assignations { get; set; }

        public static string CreateRandomPassword()
        {
            return Guid.NewGuid().ToString("N").Truncate(16);
        }

        public static User CreateTenantAdminUser(int tenantId, string emailAddress)
        {
            var user = new User
            {
                TenantId = tenantId,
                UserName = AdminUserName,
                Name = AdminUserName,
                Surname = AdminUserName,
                EmailAddress = emailAddress,
                Roles = new List<UserRole>()
            };

            user.SetNormalizedNames();

            return user;
        }
    }
}
