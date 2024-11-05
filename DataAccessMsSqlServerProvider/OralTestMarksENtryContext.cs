using DomainModel.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessMsSqlServerProvider
{
    public class OralTestMarksENtryContext : DbContext
    {
        public OralTestMarksENtryContext(DbContextOptions<OralTestMarksENtryContext> options) :base(options)
        { }

        public OralTestMarksENtryContext()
        {
        }

        public DbSet<OralTestScheduleDMO> ScheduleNameDMO { get; set; }
        public DbSet<StudentDetailsDMO> StudentDetailsDMO { get; set; }
        public DbSet<MasterSubjectDMO> MasterSubjectDMO { get; set; }
        public DbSet<MasterConfiguration> MasterConfiguration { get; set; }

        public DbSet<OralTestOralByMarksDMO> OralTestOralByMarksDMO { get; set; }
        public DbSet<OralTestScheduleMarksMapDMO> OralTestScheduleMarksMapDMO { get; set; }
        public DbSet<OralTestStudentWiseMarksDMO> OralTestStudentWiseMarksDMO { get; set; }      
        public DbSet<OralTestStudentStatusDMO> OralTestStudentStatusDMO { get; set; }

        public DbSet<OralTestScheduleStudentInsertDMO> OralTestScheduleStudentInsertDMO { get; set; }
        public DbSet<MasterAcademic> AcademicYear { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<OralTestOralByMarksDMO>().HasKey(m => m.PAOTM_Id);
            builder.Entity<OralTestScheduleMarksMapDMO>().HasKey(m => m.PASHOM_Id);
            builder.Entity<OralTestStudentWiseMarksDMO>().HasKey(m => m.PAOTMS_Id);
            builder.Entity<OralTestStudentStatusDMO>().HasKey(m => m.PAOTSS_Id);
        }

        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();

            updateUpdatedProperty<OralTestOralByMarksDMO>();

            updateUpdatedProperty<OralTestScheduleMarksMapDMO>();

            updateUpdatedProperty<OralTestStudentWiseMarksDMO>();

            updateUpdatedProperty<OralTestStudentStatusDMO>();

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
