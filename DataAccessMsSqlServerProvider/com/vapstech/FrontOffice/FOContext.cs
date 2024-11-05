using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainModel.Model;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs;
using DomainModel.Model.com.vapstech.TT;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vapstech.FrontOffice;
using DomainModel.Model.com.vapstech.HRMS;
using DomainModel.Model.com.vapstech.admission;
using DomainModel.Model.com.vaps.Exam;
using DomainModel.Model.com.vapstech.College.Admission;
using DomainModel.Model.com.vapstech.Hostel;

namespace DataAccessMsSqlServerProvider.com.vapstech.FrontOffice
{
    public class FOContext : DbContext
    {
        public FOContext(DbContextOptions<FOContext> options) :base(options)
        {
            Database.SetCommandTimeout(300000);
        }
        //public DbSet<TTMasterCategoryDMO> TTMasterCategoryDMO { get; set; }
        public DbSet<MasterShiftsTimingsDMO> masterShiftsTimings { get; set; }
     
        public DbSet<MasterTimeSettingDMO> MasterTimeSetting { get; set; }

        public DbSet<MasterShiftsDMO> masterShifts { get; set; }
        public DbSet<HolidayWorkingDayTypeDMO> holidayWorkingDayType { get; set; }

        public DbSet<SMSEmailSetting> smsEmailSetting { get; set; }
        public DbSet<Month> Month { get; set; }

        public DbSet<HR_Master_GroupType> HR_Master_GroupType_DMO { get; set; }
        public DbSet<HR_Master_Department> HR_Master_Department_DMO { get; set; }
        public DbSet<HR_Master_Designation> HR_Master_Designation_DMO { get; set; }

        public DbSet<IVRM_Storage_path_Details> IVRM_Storage_path_Details { get; set; }
        public DbSet<FO_Biometric_DeviceDMO> FO_Biometric_DeviceDMO { get; set; }
        public DbSet<FO_Biometric_DeviceType_InstitutoinwiseDMO> FO_Biometric_DeviceType_InstitutoinwiseDMO { get; set; }
        public DbSet<FO_Biometric_DeviceTypeDMO> FO_Biometric_DeviceTypeDMO { get; set; }
        public DbSet<FO_Master_Employee_HolidaysDMO> FO_Master_Employee_HolidaysDMO { get; set; }
        public DbSet<ClassTeacherMappingDMO> ClassTeacherMappingDMO { get; set; }
        public DbSet<School_Adm_Y_StudentDMO> Adm_School_Y_StudentDMO { get; set; }
        public DbSet<School_M_Class> Adm_School_M_ClassDMO { get; set; }
        public DbSet<School_M_Section> school_M_Section { get; set; }
        public DbSet<Exm_Login_PrivilegeDMO> Exm_Login_PrivilegeDMO { get; set; }
        public DbSet<Exm_Login_Privilege_SubjectsDMO> Exm_Login_Privilege_SubjectsDMO { get; set; }
        public DbSet<Staff_User_Login> Staff_User_Login { get; set; }

        public DbSet<HR_Master_Employee_DMO> HR_Master_Employee_DMO { get; set; }
        public DbSet<Adm_M_Student> Adm_M_Student { get; set; }
        public DbSet<HL_Hostel_GatePass_DMO> HL_Hostel_GatePass_DMO { get; set; }
        public DbSet<Adm_Master_College_StudentDMO> Adm_Master_College_StudentDMO { get; set; }
        public DbSet<Adm_Student_PunchDMO> Adm_Student_PunchDMO { get; set; }
        public DbSet<Adm_Student_Punch_DetailsDMO> Adm_Student_Punch_DetailsDMO { get; set; }

        public DbSet<EmployeeShiftMappingDMO> EmployeeShiftMapping { get; set; }

        public DbSet<MasterAcademic> AcademicYear { get; set; }
        public DbSet<MasterHolidayDMO> Master_holiday { get; set; }
        public DbSet<FO_Master_HolidayWorkingDay_DatesDMO> holidaydate { get; set; }
        public DbSet<TT_Master_DayDMO> TT_Master_DayDMO { get; set; }
        public DbSet<FODayNameDMO> FODayNameDMO { get; set; }
        public DbSet<FO_Emp_Punch_DetailsDMO> FO_Emp_Punch_Details { get; set; }
        public DbSet<FO_Emp_PunchDMO> FO_Emp_Punch { get; set; }
        public DbSet<HR_Master_LeaveYearDMO> HR_Master_LeaveYearDMO { get; set; }
        public DbSet<Multiple_Mobile_DMO> Multiple_Mobile_DMO { get; set; }
        public DbSet<Multiple_Email_DMO> Multiple_Email_DMO { get; set; }
        public DbSet<FO_FileDownloadedLogsDMO> FO_FileDownloadedLogsDMO { get; set; }
        public DbSet<FO_Biometric_VAPS_IEMapping_DMO> FO_Biometric_VAPS_IEMapping_DMO { get; set; }
        public DbSet<HRGroupDeptDessgDMO> HRGroupDeptDessgDMO { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<MasterShiftsTimingsDMO>().ToTable("FO_Master_Shifts_Timings", "FO");

            builder.Entity<FODayNameDMO>().ToTable("FO_Master_Day", "FO");

            builder.Entity<FO_Emp_PunchDMO>().ToTable("FO_Emp_Punch", "FO");
            builder.Entity<FO_Emp_Punch_DetailsDMO>().ToTable("FO_Emp_Punch_Details", "FO");
            builder.Entity<MasterShiftsDMO>().ToTable("FO_Master_Shifts", "FO");

            builder.Entity<HolidayWorkingDayTypeDMO>().ToTable("FO_HolidayWorkingDay_Type", "FO");
            builder.Entity<MasterTimeSettingDMO>()
           .ToTable("FO_Master_TimeSettings", "FO");
            builder.Entity<EmployeeShiftMappingDMO>()
              .ToTable("FO_Emp_Shifts_Timings", "FO");
            builder.Entity<MasterHolidayDMO>().ToTable("FO_Master_HolidayWorkingDay", "FO");
            builder.Entity<FO_Master_HolidayWorkingDay_DatesDMO>().ToTable("FO_Master_HolidayWorkingDay_Dates", "FO");
            base.OnModelCreating(builder);
        }
        public override int SaveChanges()
        {
            return base.SaveChanges();
        }
        private void updateUpdatedProperty<T>() where T : class
        {
            var modifiedSourceInfo =
                ChangeTracker.Entries<T>()
                    .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entry in modifiedSourceInfo)
            {
                //entry.Property("UpdatedTimestamp").CurrentValue = DateTime.UtcNow;;;;;
            }
        }



    }
}
