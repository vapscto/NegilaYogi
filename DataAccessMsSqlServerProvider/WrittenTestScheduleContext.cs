using DomainModel.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessMsSqlServerProvider
{
    public class WrittenTestScheduleContext : DbContext
    {
        public WrittenTestScheduleContext(DbContextOptions<WrittenTestScheduleContext> options) :base(options)
        { }

        public WrittenTestScheduleContext()
        {
        }

        public DbSet<WrittenTestScheduleDMO> WrittenTestScheduleDMO { get; set; }
      // public DbSet<StudentDetailsDMO> StudentDetailsDMO { get; set; }
        public DbSet<StudentApplication> StudentApplication { get; set; }

        public DbSet<WrittenTestScheduleStudentInsertDMO> WrittenTestScheduleStudentInsertDMO { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<WrittenTestScheduleDMO>().HasKey(m => m.PAWTS_Id);

            //builder.Entity<WrittenTestScheduleDMO>().HasKey(m => m.PAWTS_Id);

            //builder.Entity<WrittenTestScheduleDMO>()
            //   .ToTable("Preadmission_WrittenTest_Schedule");

            //base.OnModelCreating(builder);
        }


        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();

            updateUpdatedProperty<WrittenTestScheduleDMO>();

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
