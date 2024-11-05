using DomainModel.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace DataAccessMsSqlServerProvider
{
    public class StudentDetailsContext : DbContext
    {
        public StudentDetailsContext(DbContextOptions<StudentDetailsContext> options) :base(options)
        { }

        public StudentDetailsContext()
        {
        }

        public DbSet<StudentDetailsDMO> StudentDetailsDMO { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<StudentDetailsDMO>().HasKey(m => m.PASR_Id);

            builder.Entity<StudentDetailsDMO>()
               .ToTable("Preadmission_School_Registration");

            base.OnModelCreating(builder);
        }

        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();

            updateUpdatedProperty<StudentDetailsDMO>();

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
