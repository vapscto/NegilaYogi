using DomainModel.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessMsSqlServerProvider
{
    public class MasterMainMenuContext : DbContext
    {
        public MasterMainMenuContext(DbContextOptions<MasterMainMenuContext> options) : base(options)
        { }
        public DbSet<MasterMainMenuDMO> MasterMainMenuDMO { get; set; }
        public DbSet<InstituteMainMenuDMO> InstituteMainMenuDMO { get; set; }
        public DbSet<MasterModule> MasterModule { get; set; }
        public DbSet<IVRM_Master_Menu_Page_MappingDMO> IVRM_Master_Menu_Page_MappingDMO { get; set; }

        public DbSet<MasterMenuPageMappingInstituteWise> submenupageinst { get; set; }
        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            base.OnModelCreating(modelbuilder);
            modelbuilder.Entity<MasterMainMenuDMO>().HasKey(m => m.IVRMMM_Id);
        }
        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();

            updateUpdatedProperty<MasterMainMenuDMO>();

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
