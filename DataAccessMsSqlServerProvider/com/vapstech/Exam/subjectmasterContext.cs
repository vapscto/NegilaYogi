using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainModel.Model;
using DomainModel.Model.com.vaps.Exam;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using DomainModel.Model.com.vapstech.TT;

namespace DataAccessMsSqlServerProvider
{
    public class subjectmasterContext : DbContext
    {
        public subjectmasterContext(DbContextOptions<subjectmasterContext> options) :base(options)
        { }

        public subjectmasterContext()
        {
        }
        public DbSet<IVRM_School_Master_SubjectsDMO> subjectmasterDMO { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            
            base.OnModelCreating(builder);
            builder.Entity<IVRM_School_Master_SubjectsDMO>().HasKey(m => m.ISMS_Id);
        }
        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();
            updateUpdatedProperty<IVRM_School_Master_SubjectsDMO>();
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
