using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using App.Caliset.Authorization.Roles;
using App.Caliset.Authorization.Users;
using App.Caliset.MultiTenancy;
using App.Caliset.Operations;

namespace App.Caliset.EntityFrameworkCore
{
    public class CalisetDbContext : AbpZeroDbContext<Tenant, Role, User, CalisetDbContext>
    {
        public DbSet<OperationType> OperationTypes { get; set; }
        
        public CalisetDbContext(DbContextOptions<CalisetDbContext> options)
            : base(options)
        {
        }
    }
}
