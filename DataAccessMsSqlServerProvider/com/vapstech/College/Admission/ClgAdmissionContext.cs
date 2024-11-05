using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vaps.Fee;
using DomainModel.Model.com.vapstech.admission;
using DomainModel.Model.com.vapstech.Alumni;
using DomainModel.Model.com.vapstech.College.Admission;
using DomainModel.Model.com.vapstech.College.Exam;
using DomainModel.Model.com.vapstech.College.Fees;
using DomainModel.Model.com.vapstech.College.Preadmission;
using DomainModel.Model.com.vapstech.HRMS;
using DomainModel.Model.com.vapstech.TT;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessMsSqlServerProvider.com.vapstech.College.Admission
{
    public class ClgAdmissionContext : DbContext
    {
        public ClgAdmissionContext(DbContextOptions<ClgAdmissionContext> options) : base(options)
        {
            Database.SetCommandTimeout(300000);
        }
        public DbSet<IVRM_MediumOfInstructionDMO> IVRM_MediumOfInstructionDMO { get; set; }
        public DbSet<ClgMasterBranchDMO> ClgMasterBranchDMO { get; set; }
        public DbSet<MasterConfiguration> masterConfig { get; set; }
        public DbSet<PA_College_Student_AchivementsTypeDMO> PA_College_Student_AchivementsTypeDMO { get; set; }
        public DbSet<PA_College_Student_PrevExtracurricularDMO> PA_College_Student_PrevExtracurricularDMO { get; set; }
        public DbSet<Month> mnth { get; set; }
        public DbSet<ClgMasterAcademicYearDMO> ClgMasterAcademicYearDMO { get; set; }
        public DbSet<PA_College_Student_SubjectDMO> PA_College_Student_SubjectDMO { get; set; }
        public DbSet<MasterAcademic> AcademicYear { get; set; }
        public DbSet<MasterCourseDMO> MasterCourseDMO { get; set; }
        public DbSet<Adm_College_Yearly_StudentDMO> Adm_College_Yearly_StudentDMO { get; set; }
        public DbSet<CLG_Adm_Master_SemesterDMO> CLG_Adm_Master_SemesterDMO { get; set; }
        public DbSet<Adm_College_Master_SectionDMO> Adm_College_Master_SectionDMO { get; set; }
        public DbSet<IVRM_School_Master_SubjectsDMO> IVRM_School_Master_SubjectsDMO { get; set; }
        public DbSet<Adm_College_Atten_Subject_MaxPeriodDMO> Adm_College_Atten_Subject_MaxPeriodDMO { get; set; }
        public DbSet<CLG_Adm_College_AY_CourseDMO> CLG_Adm_College_AY_CourseDMO { get; set; }
        public DbSet<CLG_Adm_College_AY_Course_BranchDMO> CLG_Adm_College_AY_Course_BranchDMO { get; set; }
        public DbSet<CLG_Adm_College_AY_Course_Branch_SemesterDMO> CLG_Adm_College_AY_Course_Branch_SemesterDMO { get; set; }
        public DbSet<Clg_Adm_College_QuotaDMO> Clg_Adm_College_QuotaDMO { get; set; }
        public DbSet<Clg_Adm_College_Quota_CategoryDMO> Clg_Adm_College_Quota_CategoryDMO { get; set; }
        public DbSet<Clg_Adm_College_Quota_Category_MappingDMO> Clg_Adm_College_Quota_Category_MappingDMO { get; set; }
        public DbSet<AdmCollegeMasterBatchDMO> AdmCollegeMasterBatchDMO { get; set; }
        public DbSet<AdmCollegeSchemeTypeDMO> AdmCollegeSchemeTypeDMO { get; set; }
        public DbSet<AdmCollegeSubjectSchemeDMO> AdmCollegeSubjectSchemeDMO { get; set; }

        public DbSet<CLGAlumniUserRegistrationDMO> CLGAlumniUserRegistrationDMO { get; set; }

        public DbSet<CLGAlumni_M_StudentDMO> CLGAlumni_M_StudentDMO  { get; set; }

        public DbSet<PA_College_Student_SubjectMarks> PA_College_Student_SubjectMarks { get; set; }
        public DbSet<Country> Country { get; set; }
        public DbSet<State> State { get; set; }
        public DbSet<MasterReligionDMO> Religion { get; set; }
        public DbSet<CollegemastercasteDMO> Caste { get; set; }
        public DbSet<CollegecastecaegoryDMO> CasteCategory { get; set; }
        public DbSet<MasterReference> MasterReference { get; set; }
        public DbSet<MasterSource> MasterSource { get; set; }
        public DbSet<MasterActivityDMO> MasterActivityDMO { get; set; }
        public DbSet<CollegeDocumentMasterDMO> MasterDocumentDMO { get; set; }
        public DbSet<Adm_Master_College_StudentDMO> Adm_Master_College_StudentDMO { get; set; }
        public DbSet<AdmCourseBranchSemesterMappingDMO> AdmCourseBranchSemesterMappingDMO { get; set; }      
        public DbSet<ClgMasterCategoryDMO> mastercategory { get; set; }
        public DbSet<ClgMasterCourseCategoryMapDMO> ClgMasterCourseCategorycategoryMap { get; set; }
        public DbSet<Adm_Course_Branch_MappingDMO> ClgMasterCourseBranchMap { get; set; }       
        public DbSet<Adm_Course_Branch_MappingDMO> Adm_Course_Branch_MappingDMO { get; set; }  
        public DbSet<IVRM_College_ReportDMO> IVRM_College_ReportDMO { get; set; }        
        public DbSet<Master_Numbering> Master_Numbering { get; set; }
        public DbSet<AdmCollegeStudentSMSNoDMO> AdmCollegeStudentSMSNoDMO { get; set; }
        public DbSet<AdmCollegeStudentEmailIdDMO> AdmCollegeStudentEmailIdDMO { get; set; }
        public DbSet<AdmCollegeStudentParentsEmailIdDMO> AdmCollegeStudentParentsEmailIdDMO { get; set; }
        public DbSet<AdmCollegeStudentParentsMobileNoDMO> AdmCollegeStudentParentsMobileNoDMO { get; set; }
        public DbSet<MasterBorad> MasterBorad { get; set; }
        public DbSet<MasterSchoolType> MasterSchoolType { get; set; }
        public DbSet<AdmCollegeStudentReferenceDMO> AdmCollegeStudentReferenceDMO { get; set; }
        public DbSet<AdmCollegeStudentSourceDMO> AdmCollegeStudentSourceDMO { get; set; }
        public DbSet<AdmCollegeStudentPrevSchoolDMO> AdmCollegeStudentPrevSchoolDMO { get; set; }
        public DbSet<AdmCollegeStudentGuardianDMO> AdmCollegeStudentGuardianDMO { get; set; }
        public DbSet<AdmCollegeStudentSiblingsDetailsDMO> AdmCollegeStudentSiblingsDetailsDMO { get; set; }
        public DbSet<AdmCollegeStudentDocumentsDMO> AdmCollegeStudentDocumentsDMO { get; set; }
        public DbSet<Adm_College_Atten_Login_UserDMO> Adm_College_Atten_Login_UserDMO { get; set; }
        public DbSet<Adm_College_Atten_Login_DetailsDMO> Adm_College_Atten_Login_DetailsDMO { get; set; }
        public DbSet<HR_Master_Employee_DMO> HR_Master_Employee_DMO { get; set; }
        public DbSet<Staff_User_Login> Staff_User_Login { get; set; }
        public DbSet<Adm_College_Attendance_BatchDMO> Adm_College_Attendance_BatchDMO { get; set; }
        public DbSet<Adm_College_Atten_Batch_SubjectsDMO> Adm_College_Atten_Batch_SubjectsDMO { get; set; }
        public DbSet<Adm_College_Atten_Batch_Subject_StudentsDMO> Adm_College_Atten_Batch_Subject_StudentsDMO { get; set; }
        public DbSet<Clg_Adm_College_Seat_DistributionDMO> Clg_Adm_College_Seat_DistributionDMO { get; set; }
        public DbSet<CLGAdm_College_RegNo_FormatDMO> CLGAdm_College_RegNo_FormatDMO { get; set; }
        public DbSet<Adm_M_Category> Adm_M_Category { get; set; }
        public DbSet<TT_Master_PeriodDMO> TT_Master_PeriodDMO { get; set; }
        public DbSet<CollegeAdmissionStandardDMO> AdmissionStandardDMO { get; set; }
        public DbSet<MasterRoleType> MasterRoleType { get; set; }
        public DbSet<Adm_Prv_Sch_CombinationDMO> Adm_Prv_Sch_CombinationDMO { get; set; }
        public DbSet<Institution> Institution { get; set; }
        public DbSet<Adm_College_Student_AttendanceDMO> Adm_College_Student_AttendanceDMO { get; set; }
        public DbSet<Adm_College_Student_Attendance_PeriodwiseDMO> Adm_College_Student_Attendance_PeriodwiseDMO { get; set; }
        public DbSet<Adm_College_Student_Attendance_StudentsDMO> Adm_College_Student_Attendance_StudentsDMO { get; set; }
        public DbSet<AdmissionRegisterDMO> AdmissionRegisterDMO { get; set; }
        public DbSet<Month> Month { get; set; }
        public DbSet<HR_Master_Designation> HR_Master_Designation { get; set; }
        public DbSet<HR_Master_Department> HR_Master_Department { get; set; }
        public DbSet<Multiple_Mobile_DMO> Multiple_Mobile_DMO { get; set; }
        public DbSet<Multiple_Email_DMO> Multiple_Email_DMO { get; set; }
        public DbSet<FeeGroupDMO> FeeGroupDMO { get; set; }
        public DbSet<ApplUser> ApplUser { get; set; }
        public DbSet<CollegeStudentlogin> CollegeStudentlogin { get; set; }
        public DbSet<CollegeQuotaCourseBranchDocumentMappingDMO> CollegeQuotaCourseBranchDocumentMappingDMO { get; set; }
        public DbSet<BranchChangeDMO> BranchChangeDMO { get; set; }
        public DbSet<CollegeActiveDeactiveStudentsReasonDMO> CollegeActiveDeactiveStudentsReasonDMO { get; set; }
        public DbSet<UserRoleWithInstituteDMO> UserRoleWithInstituteDMO { get; set; }
        public DbSet<CollegeCancellationConfigurationDMO> CollegeCancellationConfigurationDMO { get; set; }
        public DbSet<Adm_College_Student_SubjectMarksDMO> Adm_College_Student_SubjectMarksDMO { get; set; }
        public DbSet<CollegeStudenttctransactionDMO> CollegeStudenttctransactionDMO { get; set; }
        public DbSet<Exm_Col_Studentwise_SubjectsDMO> Exm_Col_Studentwise_SubjectsDMO { get; set; }
        //added by roopa
        public DbSet<Adm_Master_College_Student_PA_DMO> Adm_Master_College_Student_PA_DMO { get; set; }
        public DbSet<Master_Competitive_AdmExamsClgDMO> Master_Competitive_AdmExamsClgDMO { get; set; }

        public DbSet<Master_CompetitiveExamsSubjectsAdmClgDMO> Master_CompetitiveExamsSubjectsAdmClgDMO { get; set; }

        public DbSet<PA_College_Student_CEMarksClgDMO> PA_College_Student_CEMarksClgDMO { get; set; }
        public DbSet<PA_College_Student_CEMarks_SubjectClgDMO> PA_College_Student_CEMarks_SubjectClgDMO { get; set; }
        public DbSet<Adm_College_Student_CEMarksDMO> Adm_College_Student_CEMarksDMO { get; set; }
        public DbSet<Adm_College_Student_CEMarks_SubjectDMO> Adm_College_Student_CEMarks_SubjectDMO { get; set; }

        //
        public DbSet<CollegeConcessionDMO> CollegeConcessionDMO { get; set; }
        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            base.OnModelCreating(modelbuilder);
            modelbuilder.Entity<ClgMasterBranchDMO>().HasKey(m => m.AMB_Id);

            base.OnModelCreating(modelbuilder);
            modelbuilder.Entity<ClgMasterAcademicYearDMO>().HasKey(m => m.ACMAY_Id);
            base.OnModelCreating(modelbuilder);
            modelbuilder.Entity<Adm_College_Atten_Subject_MaxPeriodDMO>().ToTable("Adm_College_Atten_Subject_MaxPeriod", "CLG");
            base.OnModelCreating(modelbuilder);
            modelbuilder.Entity<Adm_College_Atten_Login_UserDMO>().ToTable("Adm_College_Atten_Login_User", "CLG");
            base.OnModelCreating(modelbuilder);
            modelbuilder.Entity<Adm_College_Atten_Login_DetailsDMO>().ToTable("Adm_College_Atten_Login_Details", "CLG");
            base.OnModelCreating(modelbuilder);
            modelbuilder.Entity<Adm_College_Attendance_BatchDMO>().ToTable("Adm_College_Attendance_Batch", "CLG");
            base.OnModelCreating(modelbuilder);
            modelbuilder.Entity<Adm_College_Atten_Batch_SubjectsDMO>().ToTable("Adm_College_Atten_Batch_Subjects", "CLG");
            base.OnModelCreating(modelbuilder);
            modelbuilder.Entity<Adm_College_Atten_Batch_Subject_StudentsDMO>().ToTable("Adm_College_Atten_Batch_Subject_Students", "CLG");

            modelbuilder.Entity<Adm_Master_College_StudentDMO>().HasKey(m => m.AMCST_Id);

        }
        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();

            updateUpdatedProperty<ClgMasterBranchDMO>();
            updateUpdatedProperty<Adm_College_Atten_Subject_MaxPeriodDMO>();
            updateUpdatedProperty<Adm_Master_College_StudentDMO>();
            updateUpdatedProperty<Adm_College_Atten_Login_UserDMO>();
            updateUpdatedProperty<Adm_College_Atten_Login_DetailsDMO>();
            updateUpdatedProperty<Adm_College_Attendance_BatchDMO>();
            updateUpdatedProperty<Adm_College_Atten_Batch_SubjectsDMO>();
            updateUpdatedProperty<Adm_College_Atten_Batch_Subject_StudentsDMO>();

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
