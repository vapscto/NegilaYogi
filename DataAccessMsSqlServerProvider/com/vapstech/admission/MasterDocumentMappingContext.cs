using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessMsSqlServerProvider
{
    public class MasterDocumentMappingContext : DbContext
    {
        public MasterDocumentMappingContext(DbContextOptions<MasterDocumentMappingContext> options) :base(options)
        { }

        public DbSet<MasterDocumentMappingDMO> MasterDocumentMappingDMO { get; set; }

        public DbSet<MasterDocumentDMO> MasterDocumentDMO { get; set; }

        public DbSet<MasterCategory> MasterCategory { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<MasterDocumentMappingDMO>().HasKey(m => m.PASCD_Id);
        }

        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();

            updateUpdatedProperty<MasterDocumentMappingDMO>();

            return base.SaveChanges();
        }

        private void updateUpdatedProperty<T>() where T : class
        {
            var modifiedSourceInfo =
                ChangeTracker.Entries<T>()
                    .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entry in modifiedSourceInfo)
            {
                //entry.Property("UpdatedTimestamp").CurrentValue = DateTime.UtcNow;
            }
        }
    }
}
