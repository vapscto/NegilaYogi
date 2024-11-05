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
    public class GovernmentBondContext : DbContext
    {
        public GovernmentBondContext(DbContextOptions<GovernmentBondContext> options) :base(options)
        { }

        
        public DbSet<GovernmentBondDMO> GovernmentBondDMO { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            
            base.OnModelCreating(builder);
            builder.Entity<GovernmentBondDMO>().HasKey(m => m.IMGB_Id);
        }
        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();

            updateUpdatedProperty<GovernmentBondDMO>();

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
