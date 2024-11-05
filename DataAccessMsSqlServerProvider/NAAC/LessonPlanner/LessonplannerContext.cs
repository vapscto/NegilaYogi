using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vaps.Exam;
using DomainModel.Model.com.vapstech.admission;
using DomainModel.Model.com.vapstech.College.Admission;
using DomainModel.Model.com.vapstech.College.Exam;
using DomainModel.Model.com.vapstech.College.Exam.LessonPlanner;
using DomainModel.Model.com.vapstech.Exam;
using DomainModel.Model.com.vapstech.Exam.LessonPlanner;
using DomainModel.Model.com.vapstech.TT;
using DomainModel.Model.NAAC.LessonPlanner;
using DomainModel.Model.NAAC.LP_OnlineExam;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccessMsSqlServerProvider.com.vapstech.Exam
{
    public class LessonplannerContext : DbContext
    {
        public LessonplannerContext(DbContextOptions<LessonplannerContext> options) : base(options)
        { }

        public DbSet<MasterSchoolTopicDMO> MasterSchoolTopicDMO { get; set; }
        public DbSet<LP_Students_Exam_AnswersheetDMO> LP_Students_Exam_AnswersheetDMO { get; set; }
        public DbSet<SchoolSubjectWithMasterTopicMappingDMO> SchoolSubjectWithMasterTopicMapping { get; set; }
        public DbSet<IVRM_School_Master_SubjectsDMO> IVRM_School_Master_SubjectsDMO { get; set; }
        public DbSet<MasterAcademic> AcademicYear { get; set; }
        public DbSet<AdmissionClass> AdmissionClass { get; set; }
        public DbSet<HR_Master_Employee_DMO> HR_Master_Employee_DMO { get; set; }
        public DbSet<School_M_Section> School_M_Section { get; set; }
        public DbSet<Exm_Login_PrivilegeDMO> Exm_Login_PrivilegeDMO { get; set; }
        public DbSet<Exm_Login_Privilege_SubjectsDMO> Exm_Login_Privilege_SubjectsDMO { get; set; }
        public DbSet<Exm_Login_Privilege_SubSubjectsDMO> Exm_Login_Privilege_SubSubjectsDMO { get; set; }
        public DbSet<Exm_Yearly_Category_GroupDMO> Exm_Yearly_Category_GroupDMO { get; set; }
        public DbSet<Exm_Yearly_Category_Group_SubjectsDMO> Exm_Yearly_Category_Group_SubjectsDMO { get; set; }
        public DbSet<Staff_User_Login> Staff_User_Login { get; set; }
        public DbSet<Institution> Institution { get; set; }
        public DbSet<SchoolStaffperiodmappingDMO> SchoolStaffperiodmappingDMO { get; set; }
        public DbSet<Masterclasscategory> Masterclasscategory { get; set; }
        public DbSet<AdmSchoolMasterClassCatSec> AdmSchoolMasterClassCatSec { get; set; }
        public DbSet<MasterRoleType> MasterRoleType { get; set; }
        public DbSet<SchoolMasterUnitDMO> SchoolMasterUnitDMO { get; set; }
        public DbSet<SchoolMasterTopicUnitDMO> SchoolMasterTopicUnitDMO { get; set; }
        public DbSet<School_Topic_Resource_MappingDMO> School_Topic_Resource_MappingDMO { get; set; }
        public DbSet<CollegeStaffPeriodMappingDMO> CollegeStaffPeriodMappingDMO { get; set; }
        public DbSet<ClgMasterBranchDMO> ClgMasterBranchDMO { get; set; }
        public DbSet<MasterCourseDMO> MasterCourseDMO { get; set; }
        public DbSet<CLG_Adm_Master_SemesterDMO> CLG_Adm_Master_SemesterDMO { get; set; }
        public DbSet<Adm_College_Master_SectionDMO> Adm_College_Master_SectionDMO { get; set; }
        public DbSet<Adm_College_Atten_Login_UserDMO> Adm_College_Atten_Login_UserDMO { get; set; }
        public DbSet<Adm_College_Atten_Login_DetailsDMO> Adm_College_Atten_Login_DetailsDMO { get; set; }
        public DbSet<CLG_Adm_College_AY_CourseDMO> CLG_Adm_College_AY_CourseDMO { get; set; }
        public DbSet<CLG_Adm_College_AY_Course_BranchDMO> CLG_Adm_College_AY_Course_BranchDMO { get; set; }
        public DbSet<CLG_Adm_College_AY_Course_Branch_SemesterDMO> CLG_Adm_College_AY_Course_Branch_SemesterDMO { get; set; }
        public DbSet<Adm_College_Yearly_StudentDMO> Adm_College_Yearly_StudentDMO { get; set; }
        public DbSet<Adm_Master_College_StudentDMO> Adm_Master_College_StudentDMO { get; set; }
        public DbSet<StudentAppUserLoginDMO> StudentAppUserLoginDMO { get; set; }
        public DbSet<CollegeStudentlogin> CollegeStudentlogin { get; set; }
        public DbSet<Exm_Col_Studentwise_SubjectsDMO> Exm_Col_Studentwise_SubjectsDMO { get; set; }
        public DbSet<StudentMappingDMO> StudentMappingDMO { get; set; }
        public DbSet<School_Adm_Y_StudentDMO> School_Adm_Y_StudentDMO { get; set; }
        public DbSet<Adm_M_Student> Adm_M_Student { get; set; }
        public DbSet<Exm_Category_ClassDMO> Exm_Category_ClassDMO { get; set; }
        public DbSet<Exm_Yearly_CategoryDMO> Exm_Yearly_CategoryDMO { get; set; }
        public DbSet<Exm_Yearly_Category_ExamsDMO> Exm_Yearly_Category_ExamsDMO { get; set; }
        public DbSet<Exm_ConfigurationDMO> Exm_ConfigurationDMO { get; set; }
        public DbSet<ExamMarksDMO> ExamMarksDMO { get; set; }
        public DbSet<exammasterDMO> exammasterDMO { get; set; }
        public DbSet<Exm_Master_GradeDMO> Exm_Master_GradeDMO { get; set; }
        public DbSet<Exm_Master_Grade_DetailsDMO> Exm_Master_Grade_DetailsDMO { get; set; }
        public DbSet<Exm_Yrly_Cat_Exams_SubwiseDMO> Exm_Yrly_Cat_Exams_SubwiseDMO { get; set; }
        public DbSet<LP_Master_MainTopic_CollegeDMO> LP_Master_MainTopic_CollegeDMO { get; set; }
        public DbSet<LP_Master_Topic_Resources_CollegeDMO> LP_Master_Topic_Resources_CollegeDMO { get; set; }
        public DbSet<LP_Master_Topic_CollegeDMO> LP_Master_Topic_CollegeDMO { get; set; }

        //*************** Lesson Planner Online Exam *************************//

        public DbSet<LP_Master_OE_SettingDMO> LP_Master_OE_SettingDMO { get; set; }
        public DbSet<LP_Master_OE_QuestionsDMO> LP_Master_OE_QuestionsDMO { get; set; }
        public DbSet<LP_Master_OE_Questions_FilesDMO> LP_Master_OE_Questions_FilesDMO { get; set; }
        public DbSet<LP_Master_OE_QNS_OptionsDMO> LP_Master_OE_QNS_OptionsDMO { get; set; }
        public DbSet<LP_Students_ExamDMO> LP_Students_ExamDMO { get; set; }
        public DbSet<LP_Students_Exam_AnswerDMO> LP_Students_Exam_AnswerDMO { get; set; }
        public DbSet<LP_Master_OE_ExamDMO> LP_Master_OE_ExamDMO { get; set; }
        public DbSet<LP_Master_OE_Exam_QuestionsDMO> LP_Master_OE_Exam_QuestionsDMO { get; set; }
        public DbSet<LP_Master_OE_Exam_TopicsDMO> LP_Master_OE_Exam_TopicsDMO { get; set; }
        public DbSet<LP_Students_Exam_SubjectiveAnswerDMO> LP_Students_Exam_SubjectiveAnswerDMO { get; set; }
        public DbSet<LP_Master_OE_Exam_LevelsDMO> LP_Master_OE_Exam_LevelsDMO { get; set; }
        public DbSet<LP_Master_OE_QNS_Options_FilesDMO> LP_Master_OE_QNS_Options_FilesDMO { get; set; }
        public DbSet<LP_Master_ComplexitiesDMO> LP_Master_ComplexitiesDMO { get; set; }
        public DbSet<LP_Students_Exam_Answersheet_StaffDMO> LP_Students_Exam_Answersheet_StaffDMO { get; set; }
        public DbSet<LP_Master_OE_QNS_Options_MFDMO> LP_Master_OE_QNS_Options_MFDMO { get; set; }
        public DbSet<LP_Master_OE_Exam_Questions_OptionsDMO> LP_Master_OE_Exam_Questions_OptionsDMO { get; set; }
        public DbSet<LP_Master_OE_Exam_Questions_Options_MFDMO> LP_Master_OE_Exam_Questions_Options_MFDMO { get; set; }
        public DbSet<IVRM_Storage_path_Details> IVRM_Storage_path_Details { get; set; }
        public DbSet<LP_Students_Exam_SubjectiveAnswer_FilesDMO> LP_Students_Exam_SubjectiveAnswer_FilesDMO { get; set; }
        public DbSet<LP_Master_OE_Exam_Questions_FilesDMO> LP_Master_OE_Exam_Questions_FilesDMO { get; set; }
        public DbSet<LP_Master_OE_Exam_Questions_Options_FilesDMO> LP_Master_OE_Exam_Questions_Options_FilesDMO { get; set; }
        public DbSet<LP_Students_Exam_SubjectiveAnswer_Staff_FilesDMO> LP_Students_Exam_SubjectiveAnswer_Staff_FilesDMO { get; set; }

        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            base.OnModelCreating(modelbuilder);
            modelbuilder.Entity<MasterSchoolTopicDMO>().HasKey(m => m.LPMMT_Id);
        }
        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();

            updateUpdatedProperty<MasterSchoolTopicDMO>();

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
