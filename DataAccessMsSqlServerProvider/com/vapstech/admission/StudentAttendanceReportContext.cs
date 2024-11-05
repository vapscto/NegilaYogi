using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainModel;
using Microsoft.EntityFrameworkCore;
using DomainModel.Model;
using DataAccessMsSqlServerProvider;
using DomainModel.Model.com.vaps.admission;
using PreadmissionDTOs;
using DomainModel.Model.com.vapstech.admission;
using DomainModel.Model.com.vaps.Fee;
using DomainModel.Model.com.vapstech.HRMS;
using DomainModel.Model.com.vaps.Exam;

namespace DataAccessMsSqlServerProvider
{
    public class StudentAttendanceReportContext : DbContext
    {
        public StudentAttendanceReportContext(DbContextOptions<StudentAttendanceReportContext> options) : base(options)
        { }

        public DbSet<MasterAcademic> academicYear { get; set; }
        public DbSet<School_M_Section> masterSection { get; set; }
        public DbSet<School_M_Class> admissionClass { get; set; }
        public DbSet<MasterMonthDMO> masterMonth { get; set; }
        public DbSet<Adm_M_Student> admissionStduent { get; set; }
        public DbSet<School_Adm_Y_StudentDMO> admissionyearstudent { get; set; }
        public DbSet<Adm_studentAttendance> attendancelist { get; set; }
        public DbSet<Adm_studentAttendanceStudents> attendanceStudentlist { get; set; }
        public DbSet<StudentTC> StudentTcList { get; set; }
        public DbSet<Religion> religion { get; set; }
        public DbSet<mastercasteDMO> caste { get; set; }
        public DbSet<MasterCompanyDMO> companyname { get; set; }
        //added by roopa//
        public DbSet<State> StateDMO { get; set; }
        public DbSet<Country> CountryDMO { get; set; }

        public DbSet<MasterCategory> category { get; set; }
        public DbSet<Adm_Student_Attendance_Shortage> Adm_Student_Attendance_Shortage { get; set; }
        public DbSet<Adm_Student_Attendance_Shortage_Students> Adm_Student_Attendance_Shortage_Students { get; set; }

        //
        //added by vishnu
        public DbSet<Adm_SchoolAttendanceLoginUser> Adm_SchAttLoginUser { get; set; }
        public DbSet<Adm_SchoolAttendanceLoginUserClass> Adm_SchAttLoginUserClass { get; set; }
        public DbSet<Adm_SchoolAttendanceLoginUserClassSubject> Adm_SchoolAttendanceLoginUserClassSubject { get; set; }
        public DbSet<Staff_User_Login> Staff_User_Login { get; set; }
        public DbSet<MasterRoleType> MasterRoleType { get; set; }
        public DbSet<AdmissionStandardDMO> standarad { get; set; }
        public DbSet<Attendance_Students_SmartCard> Attendance_Students_SmartCard { get; set; }
        public DbSet<Adm_studentAttendance> Adm_studentAttendance { get; set; }
        public DbSet<Adm_studentAttendanceStudents> Adm_studentAttendanceStudents { get; set; }
        public DbSet<AdmissionStandardDMO> AdmissionStandardDMO { get; set; }
        public DbSet<StudentAppUserLoginDMO> StudentAppUserLoginDMO { get; set; }
        public DbSet<ApplRole> applicationRole { get; set; }
        public DbSet<ApplicationUserRole> appUserRole { get; set; }
        public DbSet<StudentUserLoginDMO> StudentUserLoginDMO { get; set; }
        public DbSet<StudentUserLogin_Institutionwise> StudentUserLogin_Institutionwise { get; set; }
        public DbSet<Masterclasscategory> Masterclasscategory { get; set; }
        public DbSet<AdmSchoolMasterClassCatSec> AdmSchoolMasterClassCatSec { get; set; }
        public DbSet<MasterEmployee> masteremployee { get; set; }
        public DbSet<Institution> Institution { get; set; }
        public DbSet<Exm_Category_ClassDMO> Exm_Category_ClassDMO { get; set; }
        public DbSet<StudentMappingDMO> StudentMappingDMO { get; set; }
        public DbSet<IVRM_Master_SubjectsDMO> IVRM_Master_SubjectsDMO { get; set; }
        public DbSet<SMSEmailSetting> SMSEmailSetting { get; set; }
        public DbSet<SMS_MAIL_SAVED_PARAMETER_DMO> SMS_MAIL_SAVED_PARAMETER_DMO { get; set; }
        public DbSet<SMS_MAIL_PARAMETER_DMO> SMS_MAIL_PARAMETER_DMO { get; set; }
        public DbSet<SMS_DETAILS_DMO> SMS_DETAILS_DMO { get; set; }
        public DbSet<Institution_Module> Institution_Module { get; set; }
        public DbSet<MasterModule> masterModule { get; set; }
        public DbSet<EMAIL_DETAILS_DMO> EMAIL_DETAILS_DMO { get; set; }
        public DbSet<IVRM_EMAIL_ATT_DMO> IVRM_EMAIL_ATT_DMO { get; set; }
        public DbSet<GeneralConfigDMO> GenConfig { get; set; }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }

        public DbSet<AttendanceEntryTypeDMO> AttendanceEntryTypeDMO { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Adm_M_Student>().HasKey(m => m.AMST_Id);
            builder.Entity<readmitstudentDMO>().HasKey(m => m.ARS_Id);
            builder.Entity<School_Adm_Y_StudentDMO>().HasKey(m => m.ASYST_Id);
            builder.Entity<StudentUserLoginDMO>().HasKey(m => m.IVRMSTUUL_Id);
            builder.Entity<StudentUserLogin_Institutionwise>().HasKey(m => m.IVRMSTUULI_Id);
            builder.Entity<StudentAppUserLoginDMO>().HasKey(m => m.IVRMUSLAPP_ID);
            builder.Entity<MasterStudentBondDMO>().HasKey(m => m.AMSTB_Id);
            builder.Entity<ApplicationUserRole>().HasKey(m=>m.UserId);
            builder.Entity<ApplRole>().HasKey(m => m.Id);
        }

        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();

            updateUpdatedProperty<Adm_M_Student>();
            updateUpdatedProperty<School_Adm_Y_StudentDMO>();
            updateUpdatedProperty<StudentAppUserLoginDMO>();
            updateUpdatedProperty<StudentUserLogin_Institutionwise>();
            updateUpdatedProperty<StudentUserLoginDMO>();
            updateUpdatedProperty<ApplicationUserRole>();
            updateUpdatedProperty<ApplRole>();

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
