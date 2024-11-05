using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;


namespace DataAccessMsSqlServerProvider
{
    public class mastercasteContext : DbContext
    {
        public mastercasteContext(DbContextOptions<mastercasteContext> options) :base(options)
        { }

        public mastercasteContext()
        {
        }

        public DbSet<mastercasteDMO> mastercasteDMO { get; set; }

        public DbSet<castecategoryDMO> castecategoryDMO { get; set; }
        public DbSet<Adm_M_Student> adm_m_student { get; set; }
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
