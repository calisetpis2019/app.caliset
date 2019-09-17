using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using App.Caliset.Authorization.Roles;
using App.Caliset.Authorization.Users;
using App.Caliset.MultiTenancy;


namespace App.Caliset.EntityFrameworkCore
{
    public class CalisetDbContext : AbpZeroDbContext<Tenant, Role, User, CalisetDbContext>
    {



        public CalisetDbContext(DbContextOptions<CalisetDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Models.Clients.Client> Clients { get; set; }
    }
}
