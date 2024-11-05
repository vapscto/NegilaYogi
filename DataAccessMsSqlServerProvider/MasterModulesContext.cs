using DomainModel.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessMsSqlServerProvider
{
    public class MasterModulesContext : DbContext
    {

        public MasterModulesContext(DbContextOptions<MasterModulesContext> options) :base(options)
        { }

        public MasterModulesContext()
        {
        }

        public DbSet<MasterModules> MasterModules { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<MasterModules>().HasKey(m => m.IVRMM_Id);

            builder.Entity<MasterModules>()
               .ToTable("IVRM_Module");

            base.OnModelCreating(builder);
        }

        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();

            updateUpdatedProperty<MasterModules>();

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
