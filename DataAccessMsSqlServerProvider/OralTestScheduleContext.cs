using DomainModel.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessMsSqlServerProvider
{
    public class OralTestScheduleContext :DbContext
    {
        public OralTestScheduleContext(DbContextOptions<OralTestScheduleContext> options) :base(options)
        { }

        public OralTestScheduleContext()
        {
        }

        public DbSet<OralTestScheduleDMO> OralTestScheduleDMO { get; set; }
        public DbSet<StudentApplication> StudentApplication { get; set; }

        public DbSet<AdmissionStatus> status { get; set; }

        public DbSet<OralTestScheduleStudentInsertDMO> OralTestScheduleStudentInsertDMO { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<OralTestScheduleDMO>().HasKey(m => m.PAOTS_Id);

           
        }

        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();

            updateUpdatedProperty<OralTestScheduleDMO>();

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
