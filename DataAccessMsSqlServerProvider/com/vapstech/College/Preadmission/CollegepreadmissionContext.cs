using DomainModel.Model;
using DomainModel.Model.com.vaps.Fee;
using DomainModel.Model.com.vapstech.College.Admission;
using DomainModel.Model.com.vapstech.College.Fees;
using DomainModel.Model.com.vapstech.College.Preadmission;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DataAccessMsSqlServerProvider.com.vapstech.College.Preadmission
{
    public class CollegepreadmissionContext : DbContext
    {
        public CollegepreadmissionContext(DbContextOptions<CollegepreadmissionContext> options) : base(options)
        {
            Database.SetCommandTimeout(300000);
        }

        public DbSet<PA_College_Application> PA_College_Application { get; set; }

        public DbSet<Religion> religion { get; set; }

        public DbSet<Caste> caste { get; set; }

        public DbSet<State> state { get; set; }

        public DbSet<Country> country { get; set; }

        public DbSet<PA_College_Student_PrevSchool> PA_College_Student_PrevSchool { get; set; }

        public DbSet<PA_College_Student_Guardian> PA_College_Student_Guardian { get; set; }

        public DbSet<PA_College_Student_CBPreference> PA_College_Student_CBPreference { get; set; }
        //added by roopa//


        public DbSet<PA_Student_Status_History_College> PA_Student_Status_History_College { get; set; }
        public DbSet<Adm_Course_Branch_MappingDMO> Adm_Course_Branch_MappingDMO { get; set; }
        public DbSet<AdmCourseBranchSemesterMappingDMO> AdmCourseBranchSemesterMappingDMO { get; set; }
        public DbSet<MasterConfiguration> masterConfig { get; set; }
        public DbSet<PA_College_Student_CEMarksClgDMO> PA_College_Student_CEMarksClgDMO { get; set; }
        public DbSet<PA_College_Student_CEMarks_SubjectClgDMO> PA_College_Student_CEMarks_SubjectClgDMO { get; set; }

        public DbSet<Master_Competitive_ExamsClgDMO> Master_Competitive_ExamsClgDMO { get; set; }

        public DbSet<Master_CompetitiveExamsSubjectsClgDMO> Master_CompetitiveExamsSubjectsClgDMO { get; set; }


        public DbSet<OralTestScheduleClgDMO> OralTestScheduleClgDMO { get; set; }

        public DbSet<OralTestScheduleStudentInsertClgDMO> OralTestScheduleStudentInsertClgDMO { get; set; }

        public DbSet<LMS_Live_Meeting_PAStudent_CollegeDMO> LMS_Live_Meeting_PAStudent_CollegeDMO { get; set; }

        //
        public DbSet<AdmissionStatus> AdmissionStatus { get; set; }
        public DbSet<PA_College_Student_Documents> PA_College_Student_Documents { get; set; }

        public DbSet<MasterCourseDMO> MasterCourseDMO { get; set; }

        public DbSet<ClgMasterBranchDMO> ClgMasterBranchDMO { get; set; }

        public DbSet<CLG_Adm_Master_SemesterDMO> CLG_Adm_Master_SemesterDMO { get; set; }

        public DbSet<AdmCollegeMasterBatchDMO> AdmCollegeMasterBatchDMO { get; set; }

        public DbSet<CollegeDocumentMasterDMO> MasterDocumentDMO { get; set; }

        public DbSet<ClgMasterCategoryDMO> mastercategory { get; set; }
        public DbSet<ClgMasterCourseCategoryMapDMO> ClgMasterCourseCategorycategoryMap { get; set; }
        public DbSet<CasteCategory> castecategory { get; set; }

        public DbSet<AcademicYear> AcademicYear { get; set; }

        public DbSet<MasterRoleType> IVRM_Role_Type { get; set; }

        public DbSet<Fee_Y_Payment_Preadmission_ApplicationDMO> Fee_Y_Payment_Preadmission_ApplicationDMO { get; set; }
        //added by roopa
        public DbSet<Fee_Y_Payment_PA_Application> Fee_Y_Payment_PA_Application { get; set; }
        //
        public DbSet<Prospepaymentamount> Prospepaymentamount { get; set; }

        public DbSet<FeePaymentDetailsDMO> FeePaymentDetailsDMO { get; set; }

        public DbSet<FeeYearlyClassCategoryDMO> feeYCC { get; set; }

        public DbSet<MasterYearlyClassCategoryClassDMO> feeYCCC { get; set; }

        public DbSet<FeeHeadDMO> feehead { get; set; }

        public DbSet<FeeAmountEntryDMO> FeeAmountEntryDMO { get; set; }

        public DbSet<FeeHeadDMO> FeeHeadDMO { get; set; }

        public DbSet<FeeYearlygroupHeadMappingDMO> FeeYearlygroupHeadMappingDMO { get; set; }

        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();

            updateUpdatedProperty<StudentApplication>();
            updateUpdatedProperty<StudentGuardian>();
            updateUpdatedProperty<StudentSibling>();
            updateUpdatedProperty<StudentPreviousSchool>();
            updateUpdatedProperty<StudentUploadImage>();
            updateUpdatedProperty<StudentTrnxDoc>();
            updateUpdatedProperty<StudentTransport>();


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
