using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using DomainModel.Model.com.vapstech.TT;

namespace DataAccessMsSqlServerProvider
{
    public class AdmissionRegisterContext : DbContext
    {
        public AdmissionRegisterContext(DbContextOptions<AdmissionRegisterContext> options) : base(options)
        { }

        public AdmissionRegisterContext()
        {
        }

        public DbSet<School_Adm_Y_StudentDMO> SchoolYearWiseStudent { get; set; }
        public DbSet<AcademicYear> academicyr { get; set; }
        public DbSet<Adm_M_Student> Adm_M_Student { get; set; }
        public DbSet<UserRoleWithInstituteDMO> UserRoleWithInstituteDMO { get; set; }
        public DbSet<Institution> master_institution { get; set; }
        public DbSet<School_M_Class> classs { get; set; }
        public DbSet<IVRM_COLOUMN_REPORT> section { get; set; }

        public DbSet<HR_Master_Employee_DMO> Employee { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);
            builder.Entity<castecategoryDMO>().HasKey(m => m.IMCC_Id);
            base.OnModelCreating(builder);
            builder.Entity<AcademicYear>().HasKey(m => m.ASMAY_Id);
        }
        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();

            updateUpdatedProperty<castecategoryDMO>();
            updateUpdatedProperty<AcademicYear>();

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
