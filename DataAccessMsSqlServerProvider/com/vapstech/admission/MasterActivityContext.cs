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
    public class MasterActivityContext :DbContext
    {
        public MasterActivityContext(DbContextOptions<MasterActivityContext> options) :base(options)
        { }

        public MasterActivityContext()
        {
        }

        public DbSet<MasterActivityDMO> MasterActivityDMO { get; set; }
        public DbSet<StudentActitvityDMO> StudentActitvityDMO { get; set; }
        public DbSet<MasterModule> MasterModule { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            
            base.OnModelCreating(builder);
            builder.Entity<MasterActivityDMO>().HasKey(m => m.AMA_Id);
        }
        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();

            updateUpdatedProperty<MasterActivityDMO>();

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
