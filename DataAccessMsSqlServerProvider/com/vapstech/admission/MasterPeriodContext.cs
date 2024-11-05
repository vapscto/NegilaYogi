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
    public class MasterPeriodContext : DbContext
    {
        public MasterPeriodContext(DbContextOptions<MasterPeriodContext> options) :base(options)
        { }

        public DbSet<MasterPeriodDMO> MasterPeriodDMO { get; set; }
        public DbSet<MasterPeriodCategoryDMO> MasterPeriodCategoryDMO { get; set; }
        public DbSet<MasterAcademic> year { get; set; }
        public DbSet<AdmissionClass> AdmClass { get; set; }

        public DbSet<MasterCategory> MasterCategory { get; set; }
        public DbSet<Institution> Institute { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<MasterPeriodDMO>().HasKey(m => m.IMP_Id);

            builder.Entity<MasterPeriodCategoryDMO>().HasKey(m => m.IMPCM_Id);
        }

        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();

            updateUpdatedProperty<MasterPeriodDMO>();

            updateUpdatedProperty<MasterPeriodCategoryDMO>();

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
