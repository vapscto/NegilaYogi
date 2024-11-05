using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vaps.Exam;
using DomainModel.Model.com.vapstech.TT;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessMsSqlServerProvider
{
    public class SubjectwisePeriodSettingsContext : DbContext
    {
        public SubjectwisePeriodSettingsContext(DbContextOptions<SubjectwisePeriodSettingsContext> options) :base(options)
        { }

        public DbSet<SubjectwisePeriodSettingsDMO> SubjectwisePeriodSettingsDMO { get; set; }
        public DbSet<MasterAcademic> year { get; set; }
        public DbSet<School_M_Class> AdmClass { get; set; }

        public DbSet<School_M_Section> AdmSection { get; set; }
        public DbSet<IVRM_Master_SubjectsDMO> allSubject { get; set; }

      //public DbSet<subjectmasterDMO> allSubject { get; set; }

        public DbSet<Institution> Institute { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<SubjectwisePeriodSettingsDMO>().HasKey(m => m.ASASMP_Id);
        }

        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();

            updateUpdatedProperty<SubjectwisePeriodSettingsDMO>();

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
