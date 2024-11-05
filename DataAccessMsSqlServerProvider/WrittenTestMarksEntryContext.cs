using DomainModel.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessMsSqlServerProvider
{
    public class WrittenTestMarksEntryContext : DbContext
    {
        public WrittenTestMarksEntryContext(DbContextOptions<WrittenTestMarksEntryContext> options) : base(options)
        { }

        public WrittenTestMarksEntryContext()
        {
        }

     
        public DbSet<AdmissionClass> AdmissionClass { get; set; }
        public DbSet<WrittenTestScheduleDMO> WrittenTestScheduleDMO { get; set; }
        //public DbSet<StudentDetailsDMO> StudentDetailsDMO { get; set; }

        public DbSet<StudentApplication> StudentDetailsDMO { get; set; }
        public DbSet<MasterSubjectDMO> MasterSubjectDMO { get; set; }
        public DbSet<MasterConfiguration> MasterConfiguration { get; set; }

        public DbSet<WIrttenTestSubjectWiseMarksDMO> WIrttenTestSubjectWiseMarksDMO { get; set; }
        public DbSet<WrittenTestStudentSubjectWiseMarksDMO> WrittenTestStudentSubjectWiseMarksDMO { get; set; }
        public DbSet<WrittenTestStudentWiseTotalMarksDMO> WrittenTestStudentWiseTotalMarksDMO { get; set; }
        public DbSet<WrittenTestScheduleMarksDMO> WrittenTestScheduleMarksDMO { get; set; }

        public DbSet<WrittenTestScheduleStudentInsertDMO> WrittenTestScheduleStudentInsertDMO { get; set; }

        public DbSet<IVRM_Master_SubjectsDMO> allSubject { get; set; }

        public DbSet<MasterAcademic> AcademicYear { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<WIrttenTestSubjectWiseMarksDMO>().HasKey(m => m.PASWM_Id);
            builder.Entity<WrittenTestStudentSubjectWiseMarksDMO>().HasKey(m => m.PASWMS_Id);
            builder.Entity<WrittenTestStudentWiseTotalMarksDMO>().HasKey(m => m.PAWMS_Id);
            builder.Entity<WrittenTestScheduleMarksDMO>().HasKey(m => m.PASHWTM_Id);
        }

        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();

            updateUpdatedProperty<WIrttenTestSubjectWiseMarksDMO>();

            updateUpdatedProperty<WrittenTestStudentSubjectWiseMarksDMO>();

            updateUpdatedProperty<WrittenTestStudentWiseTotalMarksDMO>();

            updateUpdatedProperty<WrittenTestScheduleMarksDMO>();

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
