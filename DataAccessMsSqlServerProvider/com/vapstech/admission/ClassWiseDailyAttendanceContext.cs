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
    public class ClassWiseDailyAttendanceContext : DbContext
    {
        public ClassWiseDailyAttendanceContext(DbContextOptions<ClassWiseDailyAttendanceContext> options) : base(options)
        { }

        public ClassWiseDailyAttendanceContext()
        {
        }

        public DbSet<SchoolYearWiseStudent> SchoolYearWiseStudent { get; set; }
        public DbSet<AcademicYear> academicyr { get; set; }
        public DbSet<School_M_Class> classs { get; set; }
        public DbSet<School_M_Section> section { get; set; }

        //added by vishnu
        public DbSet<Adm_SchoolAttendanceLoginUser> Adm_SchAttLoginUser { get; set; }
        public DbSet<Adm_SchoolAttendanceLoginUserClass> Adm_SchAttLoginUserClass { get; set; }
        public DbSet<Staff_User_Login> Staff_User_Login { get; set; }
        public DbSet<MasterRoleType> MasterRoleType { get; set; }
        public DbSet<Masterclasscategory> Masterclasscategory { get; set; }
        public DbSet<AdmSchoolMasterClassCatSec> AdmSchoolMasterClassCatSec { get; set; }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<SMS_MAIL_SAVED_PARAMETER_DMO> SMS_MAIL_SAVED_PARAMETER_DMO { get; set; }
        public DbSet<SMS_DETAILS_DMO> SMS_DETAILS_DMO { get; set; }
        public DbSet<Institution> Institution { get; set; }
        public DbSet<SMS_MAIL_PARAMETER_DMO> SMS_MAIL_PARAMETER_DMO { get; set; }
        public DbSet<SMSEmailSetting> smsEmailSetting { get; set; }
        public DbSet<Institution_Module> Institution_Module { get; set; }
        public DbSet<MasterModule> masterModule { get; set; }
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
