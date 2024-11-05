using DomainModel.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessMsSqlServerProvider
{
    public class MasterCategoryContext:DbContext
    {
        public MasterCategoryContext(DbContextOptions<MasterCategoryContext> options) : base(options)
        { }
        public DbSet<MasterCategory> category { get; set; }

        public DbSet<Institution> institution { get; set; }


        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            base.OnModelCreating(modelbuilder);
            modelbuilder.Entity<MasterCategory>().HasKey(m => m.AMC_Id);
        }
        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();

            updateUpdatedProperty<MasterCategory>();

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
