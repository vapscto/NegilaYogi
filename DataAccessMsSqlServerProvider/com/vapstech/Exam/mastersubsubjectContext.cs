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
    public class mastersubsubjectContext : DbContext
    {
        public mastersubsubjectContext(DbContextOptions<mastersubsubjectContext> options) :base(options)
        { }

        public mastersubsubjectContext()
        {
        }
        public DbSet<mastersubsubjectDMO> mastersubsubjectDMO { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            
            base.OnModelCreating(builder);
            builder.Entity<mastersubsubjectDMO>().HasKey(m => m.EMSS_Id);
        }
        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();

            updateUpdatedProperty<mastersubsubjectDMO>();

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
