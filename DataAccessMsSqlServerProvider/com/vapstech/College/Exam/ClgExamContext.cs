using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vaps.Exam;
using DomainModel.Model.com.vapstech.admission;
using DomainModel.Model.com.vapstech.College.Admission;
using DomainModel.Model.com.vapstech.College.Exam;
using DomainModel.Model.com.vapstech.Exam;
using DomainModel.Model.com.vapstech.TT;
using DomainModel.Model.NAAC.Admission;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccessMsSqlServerProvider.com.vapstech.College.Exam
{
    public class ClgExamContext : DbContext
    {
        public ClgExamContext(DbContextOptions<ClgExamContext> options) : base(options)
        { Database.SetCommandTimeout(30000); }
        public DbSet<Adm_Master_College_StudentDMO> Adm_Master_College_StudentDMO { get; set; }
        public DbSet<Exm_Col_Yearly_SchemeDMO> Exm_Col_Yearly_SchemeDMO { get; set; }
        public DbSet<Exm_Col_Yearly_Scheme_GroupDMO> Exm_Col_Yearly_Scheme_GroupDMO { get; set; }
        public DbSet<Exm_Col_Yearly_Scheme_Group_SubjectsDMO> Exm_Col_Yearly_Scheme_Group_SubjectsDMO { get; set; }
        public DbSet<Adm_College_Yearly_StudentDMO> Adm_College_Yearly_StudentDMO { get; set; }
        public DbSet<Exm_Col_Yearly_Scheme_ExamsDMO> Exm_Col_Yearly_Scheme_ExamsDMO { get; set; }
        public DbSet<Exm_Col_Yrly_Sch_Exams_SubwiseDMO> Exm_Col_Yrly_Sch_Exams_SubwiseDMO { get; set; }
        public DbSet<Exm_Col_Yrly_Sch_Exams_Subwise_SubDMO> Exm_Col_Yrly_Sch_Exams_Subwise_SubDMO { get; set; }
        public DbSet<Exm_Col_Student_MarksDMO> Exm_Col_Student_MarksDMO { get; set; }
        public DbSet<Exm_Col_Student_Marks_SubSubjectDMO> Exm_Col_Student_Marks_SubSubjectDMO { get; set; }
        public DbSet<AdmCourseBranchSemesterMappingDMO> AdmCourseBranchSemesterMappingDMO { get; set; }
        public DbSet<Adm_Course_Branch_MappingDMO> ClgMasterCourseBranchMap { get; set; }
        public DbSet<exammasterDMO> col_exammasterDMO { get; set; }
        public DbSet<Exm_Master_GradeDMO> col_Exm_Master_GradeDMO { get; set; }
        public DbSet<Exm_Master_Grade_DetailsDMO> col_Exm_Master_Grade_DetailsDMO { get; set; }
        public DbSet<Exm_Master_GroupDMO> col_Exm_Master_GroupDMO { get; set; }
        public DbSet<Exm_Master_Group_SubjectsDMO> col_Exm_Master_Group_SubjectsDMO { get; set; }
        public DbSet<Exm_Yearly_Category_ExamsDMO> Exm_Yearly_Category_ExamsDMO { get; set; }
        public DbSet<Exm_Master_GradeDMO> Exm_Master_GradeDMO { get; set; }
        public DbSet<ExamMarksDMO> ExamMarksDMO { get; set; }
        public DbSet<Exm_M_Prom_Subj_Group_ExamsDMO> Exm_M_Prom_Subj_Group_ExamsDMO { get; set; }
        public DbSet<ExmStudentMarksProcessDMO> ExmStudentMarksProcessDMO { get; set; }
        public DbSet<ExmStudentMarksProcessSubjectwiseDMO> ExmStudentMarksProcessSubjectwiseDMO { get; set; }
        public DbSet<Exm_Master_Grade_DetailsDMO> Exm_Master_Grade_DetailsDMO { get; set; }
        public DbSet<Exm_M_PromotionDMO> Exm_M_PromotionDMO { get; set; }
        public DbSet<Exm_M_Promotion_SubjectsDMO> Exm_M_Promotion_SubjectsDMO { get; set; }
        public DbSet<Exm_Master_GroupDMO> Exm_Master_GroupDMO { get; set; }
        public DbSet<IVRM_School_Master_SubjectsDMO> IVRM_School_Master_SubjectsDMO { get; set; }
        public DbSet<Exm_Col_Master_Group_SubjectsDMO> Exm_Col_Master_Group_SubjectsDMO { get; set; }
        public DbSet<Exm_Col_Studentwise_SubjectsDMO> Exm_Col_Studentwise_SubjectsDMO { get; set; }
        public DbSet<IVRM_School_Master_SubjectsDMO> IVRM_Master_SubjectsDMO { get; set; }
        public DbSet<mastersubexamDMO> clg_mastersubexam { get; set; }
        public DbSet<Exm_Master_CategoryDMO> clg_Exm_Master_CategoryDMO { get; set; }
        public DbSet<Exm_Category_ClassDMO> clg_Exm_Category_ClassDMO { get; set; }
        public DbSet<CLG_Adm_Master_SemesterDMO> CLG_Adm_Master_SemesterDMO { get; set; }
        public DbSet<MasterAcademic> AcademicYear { get; set; }
        public DbSet<Month> Month { get; set; }
        public DbSet<ClgMasterBranchDMO> ClgMasterBranchDMO { get; set; }
        public DbSet<MasterCourseDMO> MasterCourseDMO { get; set; }
        public DbSet<AdmissionClass> AdmissionClass { get; set; }
        public DbSet<Adm_College_Master_SectionDMO> Adm_College_Master_SectionDMO { get; set; }
        public DbSet<Masterclasscategory> clg_Masterclasscategory { get; set; }
        public DbSet<AdmSchoolMasterClassCatSec> clg_AdmSchoolMasterClassCatSec { get; set; }
        public DbSet<Exm_Yearly_CategoryDMO> clg_Exm_Yearly_CategoryDMO { get; set; }
        public DbSet<mastersubsubjectDMO> clg_mastersubsubject { get; set; }
        public DbSet<Exm_Login_Privilege_SubSubjectsDMO> Exm_Login_Privilege_SubSubjectsDMO { get; set; }
        public DbSet<Exm_ConfigurationDMO> Exm_ConfigurationDMO { get; set; }
        public DbSet<TT_Restricting_Period_SubjectDMO> TT_Restricting_Period_SubjectDMO { get; set; }
        public DbSet<TT_Restricting_Period_Staff_ClassSectionDMO> TT_Restricting_Period_Staff_ClassSectionDMO { get; set; }
        public DbSet<TT_Restricting_Day_SubjectDMO> TT_Restricting_Day_SubjectDMO { get; set; }
        public DbSet<TT_Restricting_Day_Staff_ClassSectionDMO> TT_Restricting_Day_Staff_ClassSectionDMO { get; set; }
        public DbSet<TT_Restricting_Day_PeriodDMO> TT_Restricting_Day_PeriodDMO { get; set; }
        public DbSet<TT_Master_Subject_AbbreviationDMO> TT_Master_Subject_AbbreviationDMO { get; set; }
        public DbSet<TT_LABLIB_DetailsDMO> TT_LABLIB_DetailsDMO { get; set; }
        public DbSet<TT_Fixing_Period_SubjectDMO> TT_Fixing_Period_SubjectDMO { get; set; }
        public DbSet<TT_Fixing_Period_Staff_ClassSectionDMO> TT_Fixing_Period_Staff_ClassSectionDMO { get; set; }
        public DbSet<TT_Fixing_Day_SubjectDMO> TT_Fixing_Day_SubjectDMO { get; set; }
        public DbSet<TT_Fixing_Day_Staff_ClassSectionDMO> TT_Fixing_Day_Staff_ClassSectionDMO { get; set; }
        public DbSet<TT_Fixing_Day_PeriodDMO> TT_Fixing_Day_PeriodDMO { get; set; }
        public DbSet<TT_Final_Period_Distribution_DetailedDMO> TT_Final_Period_Distribution_DetailedDMO { get; set; }
        public DbSet<TT_Final_Generation_DetailedDMO> TT_Final_Generation_DetailedDMO { get; set; }
        public DbSet<TT_ConsecutiveDMO> TT_ConsecutiveDMO { get; set; }
        public DbSet<TT_Bifurcation_Details_DMO> TT_Bifurcation_Details_DMO { get; set; }
        public DbSet<WIrttenTestSubjectWiseMarksDMO> WIrttenTestSubjectWiseMarksDMO { get; set; }
        public DbSet<MasterSubjectDMO> masterSubject { get; set; }
        public DbSet<AdmCollegeSubjectSchemeDMO> AdmCollegeSubjectSchemeDMO { get; set; }
        public DbSet<AdmCollegeSchemeTypeDMO> AdmCollegeSchemeTypeDMO { get; set; }
        public DbSet<CLG_Adm_College_AY_CourseDMO> CLG_Adm_College_AY_CourseDMO { get; set; }
        public DbSet<CLG_Adm_College_AY_Course_BranchDMO> CLG_Adm_College_AY_Course_BranchDMO { get; set; }
        public DbSet<CLG_Adm_College_AY_Course_Branch_SemesterDMO> CLG_Adm_College_AY_Course_Branch_SemesterDMO { get; set; }
        public DbSet<MasterRoleType> MasterRoleType { get; set; }
        public DbSet<Staff_User_Login> Staff_User_Login { get; set; }
        public DbSet<Adm_College_Atten_Login_UserDMO> Adm_College_Atten_Login_UserDMO { get; set; }
        public DbSet<Adm_College_Atten_Login_DetailsDMO> Adm_College_Atten_Login_DetailsDMO { get; set; }
        public DbSet<Clg_Exm_M_RuleSettingDMO> Clg_Exm_M_RuleSettingDMO { get; set; }
        public DbSet<Clg_Exm_M_RuleSetting_SubjectsDMO> Clg_Exm_M_RuleSetting_SubjectsDMO { get; set; }
        public DbSet<Clg_Exm_M_RS_Subj_GroupDMO> Clg_Exm_M_RS_Subj_GroupDMO { get; set; }
        public DbSet<Clg_Exm_M_RS_Subj_Group_ExamsDMO> Clg_Exm_M_RS_Subj_Group_ExamsDMO { get; set; }
        public DbSet<CLG_Exm_Col_Student_Marks_ProcessDMO> CLG_Exm_Col_Student_Marks_ProcessDMO { get; set; }
        public DbSet<CLG_Exm_Col_Student_Marks_Process_SubjectwiseDMO> CLG_Exm_Col_Student_Marks_Process_SubjectwiseDMO { get; set; }
        public DbSet<IVRM_Master_Subjects_Branch_DMO> IVRM_Master_Subjects_Branch_DMO { get; set; }
        public DbSet<Institution> Institution_master { get; set; }
        public DbSet<ApplUser> ApplUser { get; set; }

        public DbSet<Adm_Course_Branch_MappingDMO> Adm_Course_Branch_MappingDMO { get; set; }
        public DbSet<Exm_HallTicketCollegeDMO> Exm_HallTicketCollegeDMO { get; set; }
        public DbSet<IVRM_Storage_path_Details> IVRM_Storage_path_Details { get; set; }
        public DbSet<Exm_TT_M_SessionDMO> Exm_TT_M_SessionDMO { get; set; }
        public DbSet<Exm_TimeTable_College_SubjectsDMO> Exm_TimeTable_College_SubjectsDMO { get; set; }
        public DbSet<Exm_TimeTable_CollegeDMO> Exm_TimeTable_CollegeDMO { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {

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
