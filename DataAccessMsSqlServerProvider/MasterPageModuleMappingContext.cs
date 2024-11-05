using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainModel.Model;
using Microsoft.EntityFrameworkCore;

namespace DataAccessMsSqlServerProvider
{
    public class MasterPageModuleMappingContext : DbContext
    {
        public MasterPageModuleMappingContext(DbContextOptions<MasterPageModuleMappingContext> options) :base(options)
        {
        }
        public DbSet<MasterPageModuleMapping> masterPageModuleMapping { get; set; }
        public DbSet<MasterPage> masterPage { get; set; }
        public DbSet<MasterModule> masterModule { get; set; }

        public DbSet<MasterRolePreviledgeDMO> MasterRolePreviledgeDMO { get; set; }

        //  public DbSet<pagemodulemapping> fetchdata { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<MasterPageModuleMapping>().HasKey(m => m.IVRMMP_Id);


            

            //one to one mapping
            builder.Entity<MasterPageModuleMapping>()
              .HasOne(p => p.masterPage);

            builder.Entity<MasterPageModuleMapping>()
             .HasOne(p => p.mastermodule);


        }

        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();
            updateUpdatedProperty<MasterPageModuleMapping>();
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
