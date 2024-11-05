using DomainModel.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessMsSqlServerProvider
{
    public class InstituteMainMenuContext : DbContext
    {
        public InstituteMainMenuContext(DbContextOptions<InstituteMainMenuContext> options) : base(options)
        { }
        public DbSet<InstituteMainMenuDMO> InstituteMainMenuDMO { get; set; }
        public DbSet<MasterMainMenuDMO> MasterMainMenuDMO { get; set; }
        public DbSet<MasterModule> MasterModule { get; set; }
        public DbSet<Institution> Institute { get; set; }

        public DbSet<MasterMenuPageMappingInstituteWise> submenupageinst { get; set; }
        public DbSet<Institution_Module> modulemap { get; set; }

        public DbSet<MasterRoleType> MasterRoleType { get; set; }
        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            base.OnModelCreating(modelbuilder);
            modelbuilder.Entity<InstituteMainMenuDMO>().HasKey(m => m.IVRMMMI_Id);
        }
        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();

            updateUpdatedProperty<InstituteMainMenuDMO>();

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
