using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessMsSqlServerProvider
{
    public class MasterReferenceContext:DbContext
    {
        public MasterReferenceContext(DbContextOptions<MasterReferenceContext> options) :base(options)
        { }
        public DbSet<MasterReference> masterRefernce { get; set; }
        public DbSet<StudentReferenceDMO> StudentReferenceDMO { get; set; }
        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            base.OnModelCreating(modelbuilder);
            // modelbuilder.Entity<MasterSection>().HasKey(m => m.MO_Id);

            //// modelbuilder.Entity<>().HasKey(m => m.MOE_Id);
        }

        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();

            updateUpdatedProperty<MasterReference>();

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
