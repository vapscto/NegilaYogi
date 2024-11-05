using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainModel.Model;
using Microsoft.EntityFrameworkCore;

namespace DataAccessMsSqlServerProvider
{
    public class MasterRoleContext : DbContext
    {
        public MasterRoleContext(DbContextOptions<MasterRoleContext> options) :base(options)
        { }

        public DbSet<MasterRole> masterRole { get; set; }

        public DbSet<ApplRole> ApplicationRole { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<MasterRole>()
               .ToTable("master_role");

            // base.OnModelCreating(builder);

            //builder.Entity<Organisation>()
            //    .HasOne(s => s.IVRM_Master_City)
            //    .WithOne()
            //   .HasForeignKey<City>(s => s.IVRMMC_Id);
        }

        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();

            updateUpdatedProperty<MasterRole>();

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
