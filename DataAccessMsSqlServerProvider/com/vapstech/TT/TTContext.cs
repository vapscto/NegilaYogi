using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainModel.Model;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs;
using DomainModel.Model.com.vapstech.TT;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vapstech.HRMS;
using DomainModel.Model.com.vapstech.admission;
using DomainModel.Model.com.vapstech.College.Admission;

namespace DataAccessMsSqlServerProvider.com.vapstech.TT
{
    public class TTContext:DbContext
    {
        public TTContext(DbContextOptions<TTContext> options) :base(options)
        { }
        public DbSet<Institution> institution { get; set; }

        public DbSet<Month> month { get; set; }
        public DbSet<TTMasterCategoryDMO> TTMasterCategoryDMO { get; set; }

        public DbSet<TT_Category_Class_DMO> TT_Category_Class_DMO { get; set; }

        public DbSet<AcademicYear> AcademicYear { get; set; }

        public DbSet<TT_Bifurcation_DMO> TT_Bifurcation_DMO { get; set; }
        public DbSet<TT_Bifurcation_Details_DMO> TT_Bifurcation_Details_DMO { get; set; }

        public DbSet<TT_Master_BuildingDMO> TT_Master_BuildingDMO { get; set; }

        public DbSet<TT_Master_Building_Class_SectionDMO> TT_Master_Building_Class_SectionDMO { get; set; }

        public DbSet <TT_Master_Day_Period_TimeDMO> TT_Master_Day_Period_TimeDMO{ get; set; }
        //-------------//
        public DbSet<MobileApplAuthenticationDMO> MobileApplAuthenticationDMO { get; set; }
        public DbSet <StaffMaxMinDaySettingDMO> StaffMaxMinDaySettingDMO { get; set; }

        public DbSet<TT_Master_Period_ClasswiseDMO> TT_Master_Period_ClasswiseDMO { get; set; }
        public DbSet<TT_Fixing_DayDMO> TT_Fixing_DayDMO { get; set; }
        public DbSet<TT_Fixing_PeriodDMO> TT_Fixing_PeriodDMO { get; set; }
        public DbSet<TT_Fixing_Day_PeriodDMO> TT_Fixing_Day_PeriodDMO { get; set; }
        public DbSet<IVRM_School_Master_SubjectsDMO> IVRM_School_Master_SubjectsDMO { get; set; }
        public DbSet<TT_Final_Period_DistributionDMO> TT_Final_Period_DistributionDMO { get; set; }
        public DbSet<TT_Final_Period_Distribution_DetailedDMO> TT_Final_Period_Distribution_DetailedDMO { get; set; }
        public DbSet<TT_Fixing_Day_StaffDMO> TT_Fixing_Day_StaffDMO { get; set; }
        public DbSet<TT_Fixing_Day_Staff_ClassSectionDMO> TT_Fixing_Day_Staff_ClassSectionDMO { get; set; }
        public DbSet<TT_Fixing_Day_SubjectDMO> TT_Fixing_Day_SubjectDMO { get; set; }
        public DbSet<TT_Fixing_Day_Subject_ClassSectionDMO> TT_Fixing_Day_Subject_ClassSectionDMO { get; set; }
        public DbSet<TT_Fixing_Period_StaffDMO> TT_Fixing_Period_StaffDMO { get; set; }
        public DbSet<TT_Fixing_Period_Staff_ClassSectionDMO> TT_Fixing_Period_Staff_ClassSectionDMO { get; set; }
        public DbSet<TT_Fixing_Period_SubjectDMO> TT_Fixing_Period_SubjectDMO { get; set; }
        public DbSet<TT_Fixing_Period_Subject_ClassSectionDMO> TT_Fixing_Period_Subject_ClassSectionDMO { get; set; }

        public DbSet<TT_ConfigurationDMO> TT_ConfigurationDMO { get; set; }
        //------------//
        public DbSet<TT_Master_Day_ClasswiseDMO> TT_Master_Day_ClasswiseDMO { get; set; }
        public DbSet<TT_Master_Subject_AbbreviationDMO> TT_Master_Subject_AbbreviationDMO { get; set; }

        public DbSet<TTBreakTimeSettingsDMO> TTBreakTimeSettingsDMO { get; set; }
        public DbSet<School_M_Class> School_M_Class { get; set; }
        public DbSet<TT_Master_DayDMO> TT_Master_DayDMO { get; set; }
        public DbSet<TT_Master_Staff_AbbreviationDMO> TT_Master_Staff_AbbreviationDMO { get; set; }
        public DbSet<TT_ConsecutiveDMO> TT_ConsecutiveDMO { get; set; }
        public DbSet<School_M_Section> School_M_Section { get; set; }
        public DbSet<HR_Master_Employee_DMO> HR_Master_Employee_DMO { get; set; }
        public DbSet<TT_LABLIB_DMO> TT_LABLIB_DMO { get; set; }
        public DbSet<TT_LABLIB_DetailsDMO> TT_LABLIB_DetailsDMO { get; set; }
        public DbSet<TT_Master_PeriodDMO> TT_Master_PeriodDMO { get; set; }
        public DbSet<TT_Master_Break_AftPeriodsDMO> TT_Master_Break_AftPeriodsDMO { get; set; }
        public DbSet<TT_Master_Break_BefPeriodsDMO> TT_Master_Break_BefPeriodsDMO { get; set; }
        public DbSet<TT_Final_GenerationDMO> TT_Final_GenerationDMO { get; set; }
        public DbSet<TT_Final_Generation_DetailedDMO> TT_Final_Generation_DetailedDMO { get; set; }

        public DbSet<TT_Final_generation_tempDMO> TT_Final_generation_tempDMO { get; set; }
        public DbSet<TT_Restricting_Day_PeriodDMO> TT_Restricting_Day_PeriodDMO { get; set; }
        public DbSet<TT_Restricting_Day_StaffDMO> TT_Restricting_Day_StaffDMO { get; set; }
        public DbSet<TT_Restricting_Day_Staff_ClassSectionDMO> TT_Restricting_Day_Staff_ClassSectionDMO { get; set; }
        public DbSet<TT_Restricting_Day_SubjectDMO> TT_Restricting_Day_SubjectDMO { get; set; }
        public DbSet<TT_Restricting_Day_Subject_ClassSectionDMO> TT_Restricting_Day_Subject_ClassSectionDMO { get; set; }
        public DbSet<TT_Restricting_Period_StaffDMO> TT_Restricting_Period_StaffDMO { get; set; }
        public DbSet<TT_Restricting_Period_Staff_ClassSectionDMO> TT_Restricting_Period_Staff_ClassSectionDMO { get; set; }
        public DbSet<TT_Restricting_Period_SubjectDMO> TT_Restricting_Period_SubjectDMO { get; set; }
        public DbSet<TT_Restricting_Period_Subject_ClassSectionDMO> TT_Restricting_Period_Subject_ClassSectionDMO { get; set; }
        public DbSet<TT_Staff_DeputationDMO> TT_Staff_DeputationDMO { get; set; }     
        public DbSet<Multiple_Mobile_DMO> Emp_MobileNo { get; set; }
        public DbSet<Multiple_Email_DMO> Emp_Email_Id { get; set; }
        public DbSet<PN_Sent_Details_DMO> PN_Sent_Details_DMO { get; set; }
        public DbSet<PN_Sent_Details_Staff_DMO> PN_Sent_Details_Staff_DMO { get; set; }
        public DbSet<PN_Sent_Details_Student_DMO> PN_Sent_Details_Student_DMO { get; set; }
        public DbSet<PN_Sent_Details_Devicewise_DMO> PN_Sent_Details_Devicewise_DMO { get; set; }

        //college Context
        public DbSet<MasterCourseDMO> MasterCourseDMO { get; set; }
        public DbSet<ClgMasterBranchDMO> ClgMasterBranchDMO { get; set; }
        public DbSet<CLG_Adm_College_AY_CourseDMO> CLG_Adm_College_AY_CourseDMO { get; set; }
        public DbSet<CLG_Adm_College_AY_Course_BranchDMO> CLG_Adm_College_AY_Course_BranchDMO { get; set; }
        public DbSet<CLGTT_Category_CourseBranchDMO> CLGTT_Category_CourseBranchDMO { get; set; }
        public DbSet<CLG_Adm_Master_SemesterDMO> CLG_Adm_Master_SemesterDMO { get; set; }
        public DbSet<CLG_Adm_College_AY_Course_Branch_SemesterDMO> CLG_Adm_College_AY_Course_Branch_SemesterDMO { get; set; }
        public DbSet<CLGTT_Master_Day_CourseBranchDMO> CLGTT_Master_Day_CourseBranchDMO { get; set; }
        public DbSet<CLGTT_PRDDistributionDetailsDMO> CLGTT_PRDDistributionDetailsDMO { get; set; }
        public DbSet<Adm_College_Master_SectionDMO> Adm_College_Master_SectionDMO { get; set; }
        public DbSet<CLGTT_Master_BreakDMO> CLGTT_Master_BreakDMO { get; set; }
        public DbSet<CLGTT_Master_Break_AftPeriodsDMO> CLGTT_Master_Break_AftPeriodsDMO { get; set; }
        public DbSet<CLGTT_Master_Break_BefPeriodsDMO> CLGTT_Master_Break_BefPeriodsDMO { get; set; }
        public DbSet<ClgPeriodAllocation_Course_DMO> ClgPeriodAllocation_Course_DMO { get; set; }
        public DbSet<CLGMasterBuilding_DMO> CLGMasterBuilding_DMO { get; set; }
        public DbSet<CLGBifurcationDetailsDMO> CLGBifurcationDetailsDMO { get; set; }
        public DbSet<CLGTT_ConsecutiveDMO> CLGTT_ConsecutiveDMO { get; set; }
        public DbSet<CLGLabDetailsDMO> CLGLabDetailsDMO { get; set; }
        public DbSet<CLGTT_Restricting_Day_PeriodDMO> CLGTT_Restricting_Day_PeriodDMO { get; set; }
        public DbSet<CLGTT_Fixing_Day_PeriodDMO> CLGTT_Fixing_Day_PeriodDMO { get; set; }
        public DbSet<CLGTT_Fixing_Day_StaffDMO> CLGTT_Fixing_Day_StaffDMO { get; set; }
        public DbSet<CLGTT_Fixing_Day_SubjectDMO> CLGTT_Fixing_Day_SubjectDMO { get; set; }
        public DbSet<CLGTT_Fixing_Period_StaffDMO> CLGTT_Fixing_Period_StaffDMO { get; set; }
        public DbSet<CLGTT_Fixing_Period_SubjectDMO> CLGTT_Fixing_Period_SubjectDMO { get; set; }
        public DbSet<CLGTT_Restricting_Day_StaffDMO> CLGTT_Restricting_Day_StaffDMO { get; set; }
        public DbSet<CLGTT_Restricting_Day_SubjectDMO> CLGTT_Restricting_Day_SubjectDMO { get; set; }
        public DbSet<CLGTT_Restricting_Period_StaffDMO> CLGTT_Restricting_Period_StaffDMO { get; set; }
        public DbSet<CLGTT_Restricting_Period_SubjectDMO> CLGTT_Restricting_Period_SubjectDMO { get; set; }
        public DbSet<CLGTT_Final_Generation_DetailedDMO> CLGTT_Final_Generation_DetailedDMO { get; set; }
        public DbSet<CLGDeputationDMO> CLGDeputationDMO { get; set; }
        public DbSet<SMSEmailSetting> smsEmailSetting { get; set; }
        public DbSet<SMS_MAIL_SAVED_PARAMETER_DMO> SMS_MAIL_SAVED_PARAMETER_DMO { get; set; }
        public DbSet<SMS_DETAILS_DMO> SMS_DETAILS_DMO { get; set; }
        public DbSet<SMS_MAIL_PARAMETER_DMO> SMS_MAIL_PARAMETER_DMO { get; set; }
        public DbSet<Institution_Module> Institution_Module { get; set; }
        public DbSet<MasterModule> masterModule { get; set; }
        public DbSet<EMAIL_DETAILS_DMO> EMAIL_DETAILS_DMO { get; set; }
        public DbSet<IVRM_EMAIL_ATT_DMO> IVRM_EMAIL_ATT_DMO { get; set; }
        public DbSet<TT_Master_FacilitiesDMO> TT_Master_FacilitiesDMO { get; set; }
        public DbSet<TT_Master_RoomDMO> TT_Master_RoomDMO { get; set; }
        public DbSet<TT_Master_Room_FacilitiesDMO> TT_Master_Room_FacilitiesDMO { get; set; }
        public DbSet<CLGTT_Course_Subject_RoomDMO> CLGTT_Course_Subject_RoomDMO { get; set; }
        public DbSet<TT_Class_Subject_RoomDMO> TT_Class_Subject_RoomDMO { get; set; }
        public DbSet<Adm_Master_College_StudentDMO> Adm_Master_College_StudentDMO { get; set; }
        public DbSet<Staff_User_Login> Staff_User_Login { get; set; }
        public DbSet<Adm_College_Yearly_StudentDMO> Adm_College_Yearly_StudentDMO { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);
            builder.Entity<TTMasterCategoryDMO>().HasKey(m => m.TTMC_Id);
            builder.Entity<TT_Category_Class_DMO>().HasKey(m => m.TTCC_Id);
            builder.Entity<TT_Bifurcation_DMO>().HasKey(m => m.TTB_Id);
            builder.Entity<TT_Bifurcation_Details_DMO>().HasKey(m => m.TTBD_Id);


            base.OnModelCreating(builder);
            builder.Entity<TT_Master_BuildingDMO>().ToTable("TT_Master_Building");
            base.OnModelCreating(builder);
            builder.Entity<TT_Master_Day_Period_TimeDMO>().ToTable("TT_Master_Day_Period_Time");
            base.OnModelCreating(builder);
            builder.Entity<HR_Master_Employee_DMO>().ToTable("HR_Master_Employee");
            base.OnModelCreating(builder);
            builder.Entity<TT_Master_PeriodDMO>().ToTable("TT_Master_Period");
            base.OnModelCreating(builder);
            builder.Entity<TT_Master_Period_ClasswiseDMO>().ToTable("TT_Master_Period_Classwise");
            base.OnModelCreating(builder);
            builder.Entity<TT_Master_Staff_AbbreviationDMO>().ToTable("TT_Master_Staff_Abbreviation");
            base.OnModelCreating(builder);
            builder.Entity<TT_Fixing_DayDMO>().ToTable("TT_Fixing_Day");
            base.OnModelCreating(builder);
            builder.Entity<TT_Fixing_PeriodDMO>().ToTable("TT_Fixing_Period");
            base.OnModelCreating(builder);
            builder.Entity<TT_Fixing_Day_PeriodDMO>().ToTable("TT_Fixing_Day_Period");
            base.OnModelCreating(builder);
            builder.Entity<IVRM_School_Master_SubjectsDMO>().ToTable("IVRM_Master_Subjects");
            base.OnModelCreating(builder);
            builder.Entity<TT_Final_Period_DistributionDMO>().ToTable("TT_Final_Period_Distribution");
            base.OnModelCreating(builder);
            builder.Entity<TT_Final_Period_Distribution_DetailedDMO>().ToTable("TT_Final_Period_Distribution_Detailed");
            base.OnModelCreating(builder);
            builder.Entity<TT_Fixing_Day_StaffDMO>().ToTable("TT_Fixing_Day_Staff");
            base.OnModelCreating(builder);
            builder.Entity<TT_Fixing_Day_Staff_ClassSectionDMO>().ToTable("TT_Fixing_Day_Staff_ClassSection");
            base.OnModelCreating(builder);
            builder.Entity<TT_Fixing_Day_SubjectDMO>().ToTable("TT_Fixing_Day_Subject");
            base.OnModelCreating(builder);
            builder.Entity<TT_Fixing_Day_Subject_ClassSectionDMO>().ToTable("TT_Fixing_Day_Subject_ClassSection");
            base.OnModelCreating(builder);
            builder.Entity<TT_Fixing_Period_StaffDMO>().ToTable("TT_Fixing_Period_Staff");
            base.OnModelCreating(builder);
            builder.Entity<TT_Fixing_Period_Staff_ClassSectionDMO>().ToTable("TT_Fixing_Period_Staff_ClassSection");
            base.OnModelCreating(builder);
            builder.Entity<TT_Fixing_Period_SubjectDMO>().ToTable("TT_Fixing_Period_Subject");
            base.OnModelCreating(builder);
            builder.Entity<TT_Fixing_Period_Subject_ClassSectionDMO>().ToTable("TT_Fixing_Period_Subject_ClassSection");
            base.OnModelCreating(builder);
            builder.Entity<TTBreakTimeSettingsDMO>().ToTable("TT_Master_Break");
            base.OnModelCreating(builder);
            builder.Entity<TT_Master_DayDMO>().ToTable("TT_Master_Day");
            base.OnModelCreating(builder);
            builder.Entity<TT_Master_Subject_AbbreviationDMO>().ToTable("TT_Master_Subject_Abbreviation");
            base.OnModelCreating(builder);
            builder.Entity<TT_ConsecutiveDMO>().ToTable("TT_Consecutive");
            base.OnModelCreating(builder);
            builder.Entity<TT_LABLIB_DMO>().ToTable("TT_LABLIB");
            base.OnModelCreating(builder);
            builder.Entity<TT_LABLIB_DetailsDMO>().ToTable("TT_LABLIB_Details");
            base.OnModelCreating(builder);
            builder.Entity<TT_Master_Break_AftPeriodsDMO>().ToTable("TT_Master_Break_AftPeriods");
            base.OnModelCreating(builder);
            builder.Entity<TT_Master_Break_BefPeriodsDMO>().ToTable("TT_Master_Break_BefPeriods");
            base.OnModelCreating(builder);
            builder.Entity<TT_Final_GenerationDMO>().ToTable("TT_Final_Generation");
            base.OnModelCreating(builder);
            builder.Entity<TT_Final_Generation_DetailedDMO>().ToTable("TT_Final_Generation_Detailed");
            base.OnModelCreating(builder);
            builder.Entity<TT_Restricting_Day_PeriodDMO>().ToTable("TT_Restricting_Day_Period");
            base.OnModelCreating(builder);
            builder.Entity<TT_Restricting_Day_StaffDMO>().ToTable("TT_Restricting_Day_Staff");
            base.OnModelCreating(builder);
            builder.Entity<TT_Restricting_Day_Staff_ClassSectionDMO>().ToTable("TT_Restricting_Day_Staff_ClassSection");
            base.OnModelCreating(builder);
            builder.Entity<TT_Restricting_Day_SubjectDMO>().ToTable("TT_Restricting_Day_Subject");
            base.OnModelCreating(builder);
            builder.Entity<TT_Restricting_Day_Subject_ClassSectionDMO>().ToTable("TT_Restricting_Day_Subject_ClassSection");
            base.OnModelCreating(builder);
            builder.Entity<TT_Restricting_Period_StaffDMO>().ToTable("TT_Restricting_Period_Staff");
            base.OnModelCreating(builder);
            builder.Entity<TT_Restricting_Period_Staff_ClassSectionDMO>().ToTable("TT_Restricting_Period_Staff_ClassSection");
            base.OnModelCreating(builder);
            builder.Entity<TT_Restricting_Period_SubjectDMO>().ToTable("TT_Restricting_Period_Subject");
            base.OnModelCreating(builder);
            builder.Entity<TT_Restricting_Period_Subject_ClassSectionDMO>().ToTable("TT_Restricting_Period_Subject_ClassSection");
            base.OnModelCreating(builder);
            builder.Entity<TT_Staff_DeputationDMO>().ToTable("TT_Staff_Deputation");

        }
        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();
            updateUpdatedProperty<TTMasterCategoryDMO>();
            updateUpdatedProperty<TT_Category_Class_DMO>();
            updateUpdatedProperty<TT_Bifurcation_DMO>();
            updateUpdatedProperty<TT_Bifurcation_Details_DMO>();
            //----//
            updateUpdatedProperty<TT_Master_BuildingDMO>();
            updateUpdatedProperty<TT_Master_Day_Period_TimeDMO>();
            updateUpdatedProperty<HR_Master_Employee_DMO>();
            updateUpdatedProperty<TT_Master_PeriodDMO>();
            updateUpdatedProperty<TT_Master_Period_ClasswiseDMO>();
            updateUpdatedProperty<TT_Master_Staff_AbbreviationDMO>();
            updateUpdatedProperty<TT_Fixing_DayDMO>();
            updateUpdatedProperty<TT_Fixing_PeriodDMO>();
            updateUpdatedProperty<TT_Fixing_Day_PeriodDMO>();
            updateUpdatedProperty<IVRM_School_Master_SubjectsDMO>();
            updateUpdatedProperty<TT_Final_Period_Distribution_DetailedDMO>();
            updateUpdatedProperty<TT_Fixing_Day_StaffDMO>();
            updateUpdatedProperty<TT_Fixing_Day_Staff_ClassSectionDMO>();
            updateUpdatedProperty<TT_Fixing_Day_SubjectDMO>();
            updateUpdatedProperty<TT_Fixing_Day_Subject_ClassSectionDMO>();
            updateUpdatedProperty<TT_Fixing_Period_StaffDMO>();
            updateUpdatedProperty<TT_Fixing_Period_Staff_ClassSectionDMO>();
            updateUpdatedProperty<TT_Fixing_Period_SubjectDMO>();
            updateUpdatedProperty<TT_Fixing_Period_Subject_ClassSectionDMO>();
            updateUpdatedProperty<TTBreakTimeSettingsDMO>();
            updateUpdatedProperty<TT_Master_DayDMO>();
            updateUpdatedProperty<TT_Master_Subject_AbbreviationDMO>();
            updateUpdatedProperty<TT_ConsecutiveDMO>();
            updateUpdatedProperty<TT_LABLIB_DMO>();
            updateUpdatedProperty<TT_LABLIB_DetailsDMO>();
            updateUpdatedProperty<TT_Master_Break_AftPeriodsDMO>();
            updateUpdatedProperty<TT_Master_Break_BefPeriodsDMO>();
            updateUpdatedProperty<TT_Final_Period_DistributionDMO>();
            updateUpdatedProperty<TT_Final_GenerationDMO>();
            updateUpdatedProperty<TT_Final_Generation_DetailedDMO>();

            updateUpdatedProperty<TT_Restricting_Day_PeriodDMO>();
            updateUpdatedProperty<TT_Restricting_Day_StaffDMO>();
            updateUpdatedProperty<TT_Restricting_Day_Staff_ClassSectionDMO>();
            updateUpdatedProperty<TT_Restricting_Day_SubjectDMO>();
            updateUpdatedProperty<TT_Restricting_Day_Subject_ClassSectionDMO>();
            updateUpdatedProperty<TT_Restricting_Period_StaffDMO>();
            updateUpdatedProperty<TT_Restricting_Period_Staff_ClassSectionDMO>();
            updateUpdatedProperty<TT_Restricting_Period_SubjectDMO>();
            updateUpdatedProperty<TT_Restricting_Period_Subject_ClassSectionDMO>();
            updateUpdatedProperty<TT_Staff_DeputationDMO>();

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
