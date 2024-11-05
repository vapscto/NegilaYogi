using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainModel.Model;
using Microsoft.EntityFrameworkCore;
using DomainModel.Model.com.vaps.admission;

namespace DataAccessMsSqlServerProvider
{
    public class MasterSourceContext : DbContext
    {

        public MasterSourceContext(DbContextOptions<MasterSourceContext> options) :base(options)
        { }

        public DbSet<MasterSource> MasterSource { get; set; }
        public DbSet<StudentSourceDMO> StudentSourceDMO { get; set; }
        public DbSet<StudentApplication> StudentApplication { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<MasterSource>()
               .ToTable("Preadmission_Master_Source");

            // base.OnModelCreating(builder);

            //builder.Entity<Organisation>()
            //    .HasOne(s => s.IVRM_Master_City)
            //    .WithOne()
            //   .HasForeignKey<City>(s => s.IVRMMC_Id);
        }

        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();

            updateUpdatedProperty<MasterSource>();

            return base.SaveChanges();
        }

        private void updateUpdatedProperty<T>() where T : class
        {
            var modifiedSourceInfo =
                ChangeTracker.Entries<T>()
                    .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entry in modifiedSourceInfo)
            {
                //entry.Property("UpdatedTimestamp").CurrentValue = DateTime.UtcNow;;;;;
            }
        }
    }
}
