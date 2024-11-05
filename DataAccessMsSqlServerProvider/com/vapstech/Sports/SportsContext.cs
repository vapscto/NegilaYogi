using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vaps.Exam;
using DomainModel.Model.com.vapstech.admission;
using DomainModel.Model.com.vapstech.FrontOffice;
using DomainModel.Model.com.vapstech.HRMS;
using DomainModel.Model.com.vapstech.Sports;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessMsSqlServerProvider.com.vapstech.Sports
{
    public class SportsContext : DbContext
    {
        public SportsContext(DbContextOptions<SportsContext> options) : base(options)
        {
            Database.SetCommandTimeout(300000);
        }


        public DbSet<MasterAcademic> year { get; set; }
        public DbSet<MasterSponserDMO> MasterSponserDMO { get; set; }
        public DbSet<ClassTeacherMappingDMO> ClassTeacherMappingDMO { get; set; }
        public DbSet<SportMasterDivisionDMO> SportMasterDivisionDMO { get; set; }
        public DbSet<SportMasterHouseDMO> SportMasterHouseDMO { get; set; }
        public DbSet<SportMasterHouseDessignationDMO> SportMasterHouseDessignationDMO { get; set; }
        public DbSet<MasterEventVenueDMO> MasterEventVenueDMO { get; set; }
        public DbSet<MasterEventsDMO> MasterEventsDMO { get; set; }
        public DbSet<EventsMappingDMO> EventsMappingDMO { get; set; }
        public DbSet<EventsSponsorDMO> EventsSponsorDMO { get; set; }
        public DbSet<EventsStudentRecordDMO> EventsStudentRecordDMO { get; set; }
        public DbSet<SportMasterCompitionLevelDMO> SportMasterCompitionLevelDMO { get; set; }
        public DbSet<MasterCompitionCategoryDMO> MasterCompitionCategoryDMO { get; set; }
        public DbSet<MasterSportsCCNameDMO> MasterSportsCCNameDMO { get; set; }
        public DbSet<SportMasterUOMDMO> SportMasterUOMDMO { get; set; }
        public DbSet<Adm_M_Student> Adm_M_Student { get; set; }
        public DbSet<MasterSportsCCGroupDMO> MasterSportsCCGroupDMO { get; set; }
        public DbSet<MasterSportsCCNameUOM_DMO> MasterSportsCCNameUOM_DMO { get; set; }
        public DbSet<Institution> Institution { get; set; }
        public DbSet<SportMasterHouseCommitteDMO> SportMasterHouseCommitteDMO { get; set; }
        public DbSet<SportStudentHouseDivisionDMO> SportStudentHouseDivisionDMO { get; set; }
        public DbSet<Adm_M_Student> admissionStduent { get; set; }
        public DbSet<School_Adm_Y_StudentDMO> admissionyearstudent { get; set; }
        public DbSet<School_M_Class> admissionClass { get; set; }
        public DbSet<School_M_Class> School_M_Class { get; set; }
        public DbSet<School_M_Section> masterSection { get; set; }
        public DbSet<MasterAcademic> AcademicYear { get; set; }
        public DbSet<Institution_MobileNo> Institution_MobileNo { get; set; }
        public DbSet<Institution_EmailId> Institution_EmailId { get; set; }
        public DbSet<StudentAgeCalcDMO> StudentAgeCalcDMO { get; set; }
        public DbSet<BMICalculationDMO> BMICalculationDMO { get; set; }
        public DbSet<ProgramMasterDMO> ProgramMasterDMO { get; set; }
        public DbSet<SPCC_Events_Students_DMO> SPCC_Events_Students_DMO { get; set; }
        public DbSet<FO_Master_HolidayWorkingDay_DatesDMO> holidaydate { get; set; }
        public DbSet<SPCC_Master_House_Staff_DMO> SPCC_Master_House_Staff_DMO { get; set; }
        public DbSet<MasterEmployee> MasterEmployee { get; set; }
        public DbSet<Multiple_Mobile_DMO> Emp_MobileNo { get; set; }
        public DbSet<Multiple_Email_DMO> Emp_Email_Id { get; set; }
        public DbSet<HR_Master_Department> HR_Master_Department { get; set; }
        public DbSet<HR_Master_Designation> HR_Master_Designation { get; set; }
        public DbSet<AdmSchoolMasterClassCatSec> AdmSchoolMasterClassCatSec { get; set; }
        public DbSet<Staff_User_Login> Staff_User_Login { get; set; }
        public DbSet<Exm_Login_PrivilegeDMO> Exm_Login_PrivilegeDMO { get; set; }
        public DbSet<Exm_Login_Privilege_SubjectsDMO> Exm_Login_Privilege_SubjectsDMO { get; set; }
        public DbSet<Exm_Login_Privilege_SubSubjectsDMO> Exm_Login_Privilege_SubSubjectsDMO { get; set; }
        public DbSet<Masterclasscategory> Masterclasscategory { get; set; }
        public DbSet<GeneralConfigDMO> GenConfig { get; set; }
        public DbSet<exammasterDMO> exammasterDMO { get; set; }

        public DbSet<EventsTeamDMO> EventsTeamDMO { get; set; }
        public DbSet<EventsTeamStudentsDMO> EventsTeamStudentsDMO { get; set; }
        public DbSet<EventsTeamScheduleDMO> EventsTeamScheduleDMO { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //base.OnModelCreating(builder);
            base.OnModelCreating(builder);
            builder.Entity<MasterSponserDMO>().HasKey(m => m.SPCCMSP_Id);

            base.OnModelCreating(builder);
            builder.Entity<SportMasterDivisionDMO>().HasKey(m => m.SPCCMD_Id);

            base.OnModelCreating(builder);
            builder.Entity<SportMasterHouseDMO>().HasKey(m => m.SPCCMH_Id);

            base.OnModelCreating(builder);
            builder.Entity<SportMasterCompitionLevelDMO>().HasKey(m => m.SPCCMCL_Id);

            base.OnModelCreating(builder);
            builder.Entity<SportMasterUOMDMO>().HasKey(m => m.SPCCMUOM_Id);

            base.OnModelCreating(builder);
            builder.Entity<SportMasterHouseDessignationDMO>().HasKey(m => m.SPCCMHD_Id);

            base.OnModelCreating(builder);
            builder.Entity<SportMasterHouseCommitteDMO>().HasKey(m => m.SPCCMHC_Id);

            builder.Entity<MasterEventVenueDMO>().HasKey(m => m.SPCCMEV_Id);
            builder.Entity<MasterEventsDMO>().HasKey(m => m.SPCCME_Id);
            builder.Entity<EventsMappingDMO>().HasKey(m => m.SPCCE_Id);
            builder.Entity<EventsSponsorDMO>().HasKey(m => m.SPCCESP_Id);

            builder.Entity<SPCC_Events_Students_DMO>().HasKey(m => m.SPCCEST_Id);

            builder.Entity<MasterCompitionCategoryDMO>().HasKey(m => m.SPCCMCC_Id);
            builder.Entity<MasterSportsCCNameDMO>().HasKey(m => m.SPCCMSCC_Id);
            builder.Entity<MasterSportsCCGroupDMO>().HasKey(m => m.SPCCMSCCG_Id);
            builder.Entity<MasterSportsCCNameUOM_DMO>().HasKey(m => m.SPCCMSCCUOM_Id);
            builder.Entity<StudentAgeCalcDMO>().HasKey(m => m.SPCCAC_Id);

        }
        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();
            //updateUpdatedProperty<Adm_M_Student>();
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
