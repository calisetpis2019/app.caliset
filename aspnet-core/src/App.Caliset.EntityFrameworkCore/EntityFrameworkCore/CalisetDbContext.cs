using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using App.Caliset.Authorization.Roles;
using App.Caliset.Authorization.Users;
using App.Caliset.MultiTenancy;
using System.Linq;

namespace App.Caliset.EntityFrameworkCore
{
    public class CalisetDbContext : AbpZeroDbContext<Tenant, Role, User, CalisetDbContext>
    {



        public CalisetDbContext(DbContextOptions<CalisetDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Models.OperationTypes.OperationType> OperationTypes { get; set; }
        public virtual DbSet<Models.OperationStates.OperationState> OperationStates { get; set; }
        public virtual DbSet<Models.Locations.Location> Locations { get; set; }
        public virtual DbSet<Models.Clients.Client> Clients { get; set; }
        public virtual DbSet<Models.Samples.Sample> Samples { get; set; }
        public virtual DbSet<Models.Comments.Comment> Comments { get; set; }
        public virtual DbSet<Models.Operations.Operation> Operations { get; set; }
        public virtual DbSet<Models.Assignations.Assignation> Assignations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            foreach (var relationship in modelbuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            base.OnModelCreating(modelbuilder);
        }
    }
}
