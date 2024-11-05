using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainModel.Model;
using DomainModel.Model.com.vaps.Exam;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;


namespace DataAccessMsSqlServerProvider
{
    public class exammasterContext : DbContext
    {
        public exammasterContext(DbContextOptions<exammasterContext> options) :base(options)
        { }
        public exammasterContext()
        {
        }

        public DbSet<exammasterDMO> exammasterDMO { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            
            base.OnModelCreating(builder);
            builder.Entity<exammasterDMO>().HasKey(m => m.EME_Id);
        }
        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();
            updateUpdatedProperty<exammasterDMO>();
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
