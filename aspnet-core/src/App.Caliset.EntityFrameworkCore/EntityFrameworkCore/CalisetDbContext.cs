using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using App.Caliset.Authorization.Roles;
using App.Caliset.Authorization.Users;
using App.Caliset.MultiTenancy;
using System.Linq;
using System;

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
        public virtual DbSet<Models.UserFile.UserFile> UserFiles { get; set; }
        public virtual DbSet<Models.UserDeviceTokens.UserDeviceToken> UserDeviceTokens { get; set; }
        public virtual DbSet<Models.HoursRecords.HourRecord> HoursRecord { get; set; }
        public virtual DbSet<Models.LocationRecord.LocationRecord> LocationRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            foreach (var relationship in modelbuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            base.OnModelCreating(modelbuilder);

            modelbuilder.Entity<Models.OperationStates.OperationState>().HasData(
                new { Id = 1, Name = "Futura", CreationTime = DateTime.Today, IsDeleted = false},
                new { Id = 2, Name = "Activa", CreationTime = DateTime.Today, IsDeleted = false },
                new { Id = 3, Name = "Finalizada", CreationTime = DateTime.Today, IsDeleted = false }
            );



            modelbuilder.Entity<Models.OperationStates.OperationState>().HasIndex(e => e.Name).IsUnique();
            modelbuilder.Entity<Models.OperationTypes.OperationType>().HasIndex(e => e.Name).IsUnique();
            modelbuilder.Entity<Models.Locations.Location>().HasIndex(e => e.Name).IsUnique();
            modelbuilder.Entity<Models.Clients.Client>().HasIndex(e => e.Name).IsUnique();
        }
    }
}
