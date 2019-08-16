using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using App.Caliset.Authorization.Roles;
using App.Caliset.Authorization.Users;
using App.Caliset.MultiTenancy;

namespace App.Caliset.EntityFrameworkCore
{
    public class CalisetDbContext : AbpZeroDbContext<Tenant, Role, User, CalisetDbContext>
    {
        /* Define a DbSet for each entity of the application */
        
        public CalisetDbContext(DbContextOptions<CalisetDbContext> options)
            : base(options)
        {
        }
    }
}
