using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessMsSqlServerProvider
{
    public class MasterSectionContext:DbContext
    {
        public MasterSectionContext(DbContextOptions<MasterSectionContext> options) :base(options)
        { }
        public DbSet<School_M_Section> masterSection { get; set; }
        public DbSet<School_Adm_Y_StudentDMO> ystudent { get; set; }
        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            base.OnModelCreating(modelbuilder);
           // modelbuilder.Entity<MasterSection>().HasKey(m => m.MO_Id);

           //// modelbuilder.Entity<>().HasKey(m => m.MOE_Id);
        }

        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();

            updateUpdatedProperty<School_M_Section>();

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
