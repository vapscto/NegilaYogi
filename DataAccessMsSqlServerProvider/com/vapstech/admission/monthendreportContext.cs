using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using DomainModel.Model.com.vapstech.admission;

namespace DataAccessMsSqlServerProvider
{
    public class monthendreportContext : DbContext
    {
        public monthendreportContext(DbContextOptions<monthendreportContext> options) :base(options)
        { }

        public monthendreportContext()
        {
        }

        public DbSet<mastercasteDMO> mastercasteDMO { get; set; }
        public DbSet<MasterAcademic> AcademicYear { get; set; }
        public DbSet<castecategoryDMO> castecategoryDMO { get; set; }

        public DbSet<Month> mnth { get; set; }

        public DbSet<GeneralConfigDMO> GenConfig { get; set; }
        public DbSet<MasterCategory> mastercategory { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            
            base.OnModelCreating(builder);
            builder.Entity<mastercasteDMO>().HasKey(m => m.IMC_Id);
        }
        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();

            updateUpdatedProperty<mastercasteDMO>();

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
