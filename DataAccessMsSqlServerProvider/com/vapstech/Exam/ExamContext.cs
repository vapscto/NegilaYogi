using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainModel.Model;
using DomainModel.Model.com.vaps.Exam;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using DomainModel.Model.com.vapstech.TT;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vaps.Fee;
using DomainModel.Model.com.vapstech.admission;
using DomainModel.Model.com.vapstech.LeaveManagement;
using DomainModel.Model.com.vapstech.Exam;
using DomainModel.Model.com.vapstech.HRMS;
using DomainModel.Model.com.vapstech.MobileApp;
using DomainModel.Model.com.vapstech.Portals.Student;
using DomainModel.Model.com.vapstech.Portals.Employee;
using DomainModel.Model.com.vapstech.Sports;

namespace DataAccessMsSqlServerProvider.com.vapstech.Exam
{
    public class ExamContext : DbContext
    {
        public ExamContext(DbContextOptions<ExamContext> options) : base(options)
        {
            Database.SetCommandTimeout(300000000);
        }
        public ExamContext()
        {
        }
        public DbSet<Adm_TC_Fee_Approval_DMO> Adm_TC_Fee_Approval_DMO_con { get; set; }
        public DbSet<Adm_TC_PDA_Approval_DMO> Adm_TC_PDA_Approval_DMO_con { get; set; }
        public DbSet<Adm_TC_Library_Approval_DMO> Adm_TC_Library_Approval_DMO_con { get; set; }
        public DbSet<Adm_TC_CT_Approval_DMO> Adm_TC_CT_Approval_DMO_con { get; set; }
        public DbSet<Adm_Students_Certificate_Apply_DMO> Adm_Students_Certificate_Apply_DMO_con { get; set; }
        public DbSet<ClassTeacherMappingDMO> ClassTeacherMappingDMO { get; set; }
        public DbSet<ExmStudentMarksProcessSubjectwiseDMO> ExmStudentMarksProcessSubjectwiseDMO { get; set; }
        public DbSet<ExmStudentMarksProcessDMO> ExmStudentMarksProcessDMO { get; set; }
        public DbSet<Staff_User_Login> Staff_User_Login { get; set; }
        public DbSet<Masterclasscategory> Masterclasscategory { get; set; }
        public DbSet<AdmSchoolMasterClassCatSec> AdmSchoolMasterClassCatSec { get; set; }
        public DbSet<exammasterDMO> exammasterDMO { get; set; }
        public DbSet<SMSEmailSetting> SMSEmailSetting { get; set; }
        public DbSet<Institution_Module_Page> Institution_Module_Page { get; set; }
        public DbSet<MasterPage> MasterPage { get; set; }
        public DbSet<MasterAcademic> AcademicYear { get; set; }
        public DbSet<AdmissionClass> AdmissionClass { get; set; }
        public DbSet<IVRM_School_Master_SubjectsDMO> IVRM_School_Master_SubjectsDMO { get; set; }
        public DbSet<HR_Master_Employee_DMO> HR_Master_Employee_DMO { get; set; }
        public DbSet<Multiple_Email_DMO> Multiple_Email_DMO { get; set; }
        public DbSet<Multiple_Mobile_DMO> Multiple_Mobile_DMO { get; set; }
        public DbSet<Exm_Master_CategoryDMO> Exm_Master_CategoryDMO { get; set; }
        public DbSet<Exm_Category_ClassDMO> Exm_Category_ClassDMO { get; set; }
        public DbSet<Exm_Master_GradeDMO> Exm_Master_GradeDMO { get; set; }
        public DbSet<Exm_Master_Grade_DetailsDMO> Exm_Master_Grade_DetailsDMO { get; set; }
        public DbSet<Exm_Master_GroupDMO> Exm_Master_GroupDMO { get; set; }
        public DbSet<Exm_Master_Group_SubjectsDMO> Exm_Master_Group_SubjectsDMO { get; set; }
        public DbSet<School_M_Section> School_M_Section { get; set; }
        public DbSet<mastersubsubjectDMO> mastersubsubject { get; set; }
        public DbSet<exammasterDMO> masterexam { get; set; }
        public DbSet<mastersubexamDMO> mastersubexam { get; set; }
        public DbSet<School_Adm_Y_StudentDMO> School_Adm_Y_Student { get; set; }
        public DbSet<Adm_M_Student> Adm_M_Student { get; set; }
        public DbSet<Exm_Yearly_CategoryDMO> Exm_Yearly_CategoryDMO { get; set; }
        public DbSet<Exm_Yearly_Category_GroupDMO> Exm_Yearly_Category_GroupDMO { get; set; }
        public DbSet<Exm_Yearly_Category_Group_SubjectsDMO> Exm_Yearly_Category_Group_SubjectsDMO { get; set; }       
        public DbSet<StudentMappingDMO> StudentMappingDMO { get; set; }
        public DbSet<FEeGroupLoginPreviledgeDMO> FEeGroupLoginPreviledgeDMO { get; set; }
        public DbSet<ApplUser> ApplUser { get; set; }
        public DbSet<Exm_Login_PrivilegeDMO> Exm_Login_PrivilegeDMO { get; set; }
        public DbSet<Exm_Login_Privilege_SubjectsDMO> Exm_Login_Privilege_SubjectsDMO { get; set; }
        public DbSet<Exm_Login_Privilege_SubSubjectsDMO> Exm_Login_Privilege_SubSubjectsDMO { get; set; }
        public DbSet<School_Adm_Y_StudentDMO> School_Adm_Y_StudentDMO { get; set; }
        public DbSet<Exm_Yearly_Category_ExamsDMO> Exm_Yearly_Category_ExamsDMO { get; set; }
        public DbSet<Exm_Yrly_Cat_Exams_SubwiseDMO> Exm_Yrly_Cat_Exams_SubwiseDMO { get; set; }
        public DbSet<Exm_Yrly_Cat_Exams_Subwise_SubSubjectsDMO> Exm_Yrly_Cat_Exams_Subwise_SubSubjectsDMO { get; set; }
        public DbSet<Exm_Yrly_Cat_Exams_Subwise_SubExamsDMO> Exm_Yrly_Cat_Exams_Subwise_SubExamsDMO { get; set; }
        public DbSet<ExamMarksDMO> ExamMarksDMO { get; set; }
        public DbSet<Exm_M_PromotionDMO> Exm_M_PromotionDMO { get; set; }
        public DbSet<Exm_M_Promotion_SubjectsDMO> Exm_M_Promotion_SubjectsDMO { get; set; }
        public DbSet<Exm_M_Prom_Subj_GroupDMO> Exm_M_Prom_Subj_GroupDMO { get; set; }
        public DbSet<Exm_M_Prom_Subj_Group_ExamsDMO> Exm_M_Prom_Subj_Group_ExamsDMO { get; set; }
        public DbSet<Exm_ConfigurationDMO> Exm_ConfigurationDMO { get; set; }       
        public DbSet<UserLoginEmployee> UserLoginEmployee { get; set; }        
        public DbSet<Exm_Student_Marks_SubSubjectDMO> Exm_Student_Marks_SubSubjectDMO { get; set; }
        public DbSet<Exm_Student_MP_PromotionDMO> Exm_Student_MP_PromotionDMO { get; set; }
        public DbSet<Exm_Stu_MP_Promo_SubjectwiseDMO> Exm_Stu_MP_Promo_SubjectwiseDMO { get; set; }
        public DbSet<Exm_Stu_MP_Promo_Subjectwise_GroupwiseDMO> Exm_Stu_MP_Promo_Subjectwise_GroupwiseDMO { get; set; }        
        public DbSet<Country> Country { get; set; }
        public DbSet<State> State { get; set; }
        public DbSet<Exm_PersonalityDMO> Exm_PersonalityDMO { get; set; }
        public DbSet<Exm_Student_PersonalityDMO> Exm_Student_PersonalityDMO { get; set; }
        public DbSet<IVRM_Month_DMO> IVRM_Month_DMO { get; set; }
        public DbSet<Exm_ProgressCard_RemarksDMO> Exm_ProgressCard_RemarksDMO { get; set; }        
        public DbSet<Exm_Student_CoCurricularDMO> Exm_Student_CoCurricularDMO { get; set; }
        public DbSet<Exm_Student_Marks_Pro_Sub_SubSubjectDMO> Exm_Student_Marks_Pro_Sub_SubSubjectDMO { get; set; }        
        public DbSet<ExamsubjectGroupMappingDMO> ExamsubjectGroupMappingDMO { get; set; }
        public DbSet<ExamSubjectGroupMappingExamsDMO> ExamSubjectGroupMappingExamsDMO { get; set; }
        public DbSet<ExamSubjectGroupMappingSubjectsDMO> ExamSubjectGroupMappingSubjectsDMO { get; set; }
        public DbSet<Exm_HallTicketDMO> Exm_HallTicketDMO { get; set; }
        public DbSet<CCE_Master_Life_SkillsDMO> CCE_Master_Life_SkillsDMO { get; set; }
        public DbSet<CCE_Master_Life_Skill_AreasDMO> CCE_Master_Life_Skill_AreasDMO { get; set; }
        public DbSet<CCE_Master_Life_Skill_Areas_MappingDMO> CCE_Master_Life_Skill_Areas_MappingDMO { get; set; }
        public DbSet<CCE_M_CoScholasticActivitiesDMO> CCE_M_CoScholasticActivitiesDMO { get; set; }
        public DbSet<CCE_Exam_M_TermsDMO> CCE_Exam_M_TermsDMO { get; set; }
        public DbSet<CCE_M_Scholastic_AreasDMO> CCE_M_Scholastic_AreasDMO { get; set; }
        public DbSet<exammasterCoCulrricularDMO> exammasterCoCulrricularDMO { get; set; }
        public DbSet<CCE_Exam_Term_MappingDMO> CCE_Exam_Term_MappingDMO { get; set; }
        public DbSet<exammasterpersonalityDMO> exammasterpersonalityDMO { get; set; }
        public DbSet<exammasterPointDMO> exammasterPointDMO { get; set; }
        public DbSet<exammasterRemarkDMO> exammasterRemarkDMO { get; set; }
        public DbSet<Institution> Institution_master { get; set; }
        public DbSet<Exm_CCE_ActivitiesDMO> Exm_CCE_ActivitiesDMO { get; set; }
        public DbSet<EXM_CCE_Activities_AREADMO> EXM_CCE_Activities_AREADMO { get; set; }
        public DbSet<Exm_CCE_TERMS_MP_EXAMSDMO> Exm_CCE_TERMS_MP_EXAMSDMO { get; set; }
        public DbSet<Exm_CCE_TERMS_EXAMSDMO> Exm_CCE_TERMS_EXAMSDMO { get; set; }
        public DbSet<Exm_Condition_MasterDMO> Exm_Condition_MasterDMO { get; set; }
        public DbSet<Exm_PassFailRank_ConditionDMO> Exm_PassFailRank_ConditionDMO { get; set; }
        public DbSet<Exm_CCE_SKILLS_TransactionDMO> Exm_CCE_SKILLS_TransactionDMO { get; set; }        
        public DbSet<HR_Master_Designation> HR_Master_Designation { get; set; }
        public DbSet<HR_Master_Department> HR_Master_Department { get; set; }
        public DbSet<Exm_CCE_Activities_TransactionDMO> Exm_CCE_Activities_TransactionDMO { get; set; }
        public DbSet<Exm_CCE_Activities_AREA_MappingDMO> Exm_CCE_Activities_AREA_MappingDMO { get; set; }
        public DbSet<Exm_TT_M_SessionDMO> Exm_TT_M_SessionDMO { get; set; }       
        public DbSet<Exm_TimeTableDMO> Exm_TimeTableDMO { get; set; }
        public DbSet<Exm_TimeTable_SubjectsDMO> Exm_TimeTable_SubjectsDMO { get; set; }       
        public DbSet<ExamPromotionRemarksDMO> ExamPromotionRemarksDMO { get; set; }
        public DbSet<ExamPromotionGroupwiseRemarksDMO> ExamPromotionGroupwiseRemarksDMO { get; set; }
        public DbSet<MasterRoleType> MasterRoleType { get; set; }       
        public DbSet<UserPromotion_DMO> UserPromotion_DMO { get; set; }
        public DbSet<Exm_Calculation_LogDMO> Exm_Calculation_LogDMO { get; set; }
        public DbSet<ExamTermWiseRemarksDMO> ExamTermWiseRemarksDMO { get; set; }
        public DbSet<Exm_StudentWiseSubjectGroupMarksDMO> Exm_StudentWiseSubjectGroupMarksDMO { get; set; }
        public DbSet<IVRM_MobileApp_Page> IVRM_MobileApp_Page { get; set; }
        public DbSet<IVRM_Role_MobileApp_Privileges> IVRM_Role_MobileApp_Privileges { get; set; }
        public DbSet<IVRM_User_MobileApp_Login_Privileges> IVRM_User_MobileApp_Login_Privileges { get; set; }
        public DbSet<Exm_Stu_MP_Promo_Subject_Groupwise_ExamDMO> Exm_Stu_MP_Promo_Subject_Groupwise_ExamDMO { get; set; }
        public DbSet<Exm_Student_TermAchievementsDMO> Exm_Student_TermAchievementsDMO { get; set; }
        public DbSet<SportMasterHouseDMO> SportMasterHouseDMO { get; set; }
        public DbSet<SportStudentHouseDivisionDMO> SportStudentHouseDivisionDMO { get; set; }
        public DbSet<EXM_ProgressCard_FormatsDMO> EXM_ProgressCard_FormatsDMO { get; set; }
        public DbSet<FeeTermDMO> FeeTermDMO { get; set; }
        public DbSet<FeeGroupDMO> FeeGroupDMO { get; set; }
        public DbSet<Exm_Student_ProgressCard_SubjectWise_RemarksDMO> Exm_Student_ProgressCard_SubjectWise_RemarksDMO { get; set; }
        public DbSet<Exm_Master_PaperTypeDMO> Exm_Master_PaperTypeDMO { get; set; }
        public DbSet<Exm_Student_Examwise_PTDMO> Exm_Student_Examwise_PTDMO { get; set; }
        public DbSet<Exm_Yrly_Cat_Exams_Subwise_PTDMO> Exm_Yrly_Cat_Exams_Subwise_PTDMO { get; set; }
        public DbSet<IVRM_Storage_path_Details> IVRM_Storage_path_Details { get; set; }
        public DbSet<Exm_Master_slabDMO> Exm_Master_slabDMO { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {

            // base.OnModelCreating(builder);
            //builder.Entity<exammasterDMO>().HasKey(m => m.EME_Id);


            base.OnModelCreating(builder);
            builder.Entity<Exm_Master_CategoryDMO>().ToTable("Exm_Master_Category", "Exm");
            base.OnModelCreating(builder);
            builder.Entity<Exm_Category_ClassDMO>().ToTable("Exm_Category_Class", "Exm");
            base.OnModelCreating(builder);
            builder.Entity<Exm_Master_GradeDMO>().ToTable("Exm_Master_Grade", "Exm");
            base.OnModelCreating(builder);
            builder.Entity<Exm_Master_Grade_DetailsDMO>().ToTable("Exm_Master_Grade_Details", "Exm");
            base.OnModelCreating(builder);
            builder.Entity<Exm_Master_GroupDMO>().ToTable("Exm_Master_Group", "Exm");
            base.OnModelCreating(builder);
            builder.Entity<Exm_Master_Group_SubjectsDMO>().ToTable("Exm_Master_Group_Subjects", "Exm");
            base.OnModelCreating(builder);
            builder.Entity<Exm_Yearly_CategoryDMO>().ToTable("Exm_Yearly_Category", "Exm");
            base.OnModelCreating(builder);
            builder.Entity<Exm_Yearly_Category_GroupDMO>().ToTable("Exm_Yearly_Category_Group", "Exm");
            base.OnModelCreating(builder);
            builder.Entity<Exm_Yearly_Category_Group_SubjectsDMO>().ToTable("Exm_Yearly_Category_Group_Subjects", "Exm");            
            base.OnModelCreating(builder);
            builder.Entity<Exm_Yearly_Category_ExamsDMO>().ToTable("Exm_Yearly_Category_Exams", "Exm");
            base.OnModelCreating(builder);
            builder.Entity<Exm_Yrly_Cat_Exams_SubwiseDMO>().ToTable("Exm_Yrly_Cat_Exams_Subwise", "Exm");
            base.OnModelCreating(builder);
            builder.Entity<Exm_Yrly_Cat_Exams_Subwise_SubSubjectsDMO>().ToTable("Exm_Yrly_Cat_Exams_Subwise_SubSubjects", "Exm");
            base.OnModelCreating(builder);
            builder.Entity<Exm_Yrly_Cat_Exams_Subwise_SubExamsDMO>().ToTable("Exm_Yrly_Cat_Exams_Subwise_SubExams", "Exm");
            base.OnModelCreating(builder);
            builder.Entity<Exm_M_PromotionDMO>().ToTable("Exm_M_Promotion", "Exm");
            base.OnModelCreating(builder);
            builder.Entity<Exm_M_Promotion_SubjectsDMO>().ToTable("Exm_M_Promotion_Subjects", "Exm");
            base.OnModelCreating(builder);
            builder.Entity<Exm_M_Prom_Subj_GroupDMO>().ToTable("Exm_M_Prom_Subj_Group", "Exm");
            base.OnModelCreating(builder);
            builder.Entity<Exm_M_Prom_Subj_Group_ExamsDMO>().ToTable("Exm_M_Prom_Subj_Group_Exams", "Exm");
            base.OnModelCreating(builder);
            builder.Entity<Exm_ConfigurationDMO>().ToTable("Exm_Configuration", "Exm");
            base.OnModelCreating(builder);
            builder.Entity<exammasterDMO>().ToTable("Exm_Master_Exam", "Exm");
            base.OnModelCreating(builder);
            builder.Entity<mastersubsubjectDMO>().ToTable("Exm_Master_SubSubject", "Exm");
            base.OnModelCreating(builder);
            builder.Entity<mastersubexamDMO>().ToTable("Exm_Master_SubExam", "Exm");
            base.OnModelCreating(builder);
            builder.Entity<StudentMappingDMO>().ToTable("Exm_Studentwise_Subjects", "Exm");
            base.OnModelCreating(builder);
            builder.Entity<ExamMarksDMO>().ToTable("Exm_Student_Marks", "Exm");
            base.OnModelCreating(builder);
            builder.Entity<Exm_Login_PrivilegeDMO>().ToTable("Exm_Login_Privilege", "Exm");
            base.OnModelCreating(builder);
            builder.Entity<Exm_Login_Privilege_SubjectsDMO>().ToTable("Exm_Login_Privilege_Subjects", "Exm");
            base.OnModelCreating(builder);
            builder.Entity<Exm_Login_Privilege_SubSubjectsDMO>().ToTable("Exm_Login_Privilege_SubSubjects", "Exm");            
            base.OnModelCreating(builder);
            builder.Entity<Exm_Student_Marks_SubSubjectDMO>().ToTable("Exm_Student_Marks_SubSubject", "Exm");
            base.OnModelCreating(builder);
            builder.Entity<Exm_Student_MP_PromotionDMO>().ToTable("Exm_Student_MP_Promotion", "Exm");
            base.OnModelCreating(builder);
            builder.Entity<Exm_Stu_MP_Promo_SubjectwiseDMO>().ToTable("Exm_Stu_MP_Promo_Subjectwise", "Exm");
            base.OnModelCreating(builder);
            builder.Entity<Exm_Stu_MP_Promo_Subjectwise_GroupwiseDMO>().ToTable("Exm_Stu_MP_Promo_Subjectwise_Groupwise", "Exm");            
            base.OnModelCreating(builder);
            builder.Entity<ExamsubjectGroupMappingDMO>().ToTable("Exm_Subject_Group", "Exm");
            base.OnModelCreating(builder);
            builder.Entity<ExamSubjectGroupMappingExamsDMO>().ToTable("Exm_Subject_Group_Exams", "Exm");
            base.OnModelCreating(builder);
            builder.Entity<ExamSubjectGroupMappingSubjectsDMO>().ToTable("Exm_Subject_Group_Subjects", "Exm");
            base.OnModelCreating(builder);
            builder.Entity<exammasterpersonalityDMO>().ToTable("Exm_M_Personality", "Exm");
            base.OnModelCreating(builder);
            builder.Entity<exammasterCoCulrricularDMO>().ToTable("Exm_CoCurricular", "Exm");
            base.OnModelCreating(builder);
            builder.Entity<exammasterPointDMO>().ToTable("Exm_M_Progresscard_Point", "Exm");
            base.OnModelCreating(builder);
            builder.Entity<Exm_Student_Marks_Pro_Sub_SubSubjectDMO>().ToTable("Exm_Student_Marks_Pro_Sub_SubSubject", "Exm");
            base.OnModelCreating(builder);
            builder.Entity<CCE_Master_Life_SkillsDMO>().ToTable("EXM_CCE_SKILLS");
            base.OnModelCreating(builder);
            builder.Entity<CCE_Master_Life_Skill_AreasDMO>().ToTable("Exm_CCE_SKILLS_AREA");
            base.OnModelCreating(builder);
            builder.Entity<CCE_Master_Life_Skill_Areas_MappingDMO>().ToTable("EXM_CCE_SKILLS_AREA_Mapping");
            base.OnModelCreating(builder);
            builder.Entity<CCE_M_CoScholasticActivitiesDMO>().ToTable("CCE_M_CoScholasticActivities");
            base.OnModelCreating(builder);
            builder.Entity<CCE_M_Scholastic_AreasDMO>().ToTable("CCE_M_Scholastic_Areas");
            base.OnModelCreating(builder);
            builder.Entity<CCE_Exam_M_TermsDMO>().ToTable("Exm_CCE_TERMS");
            base.OnModelCreating(builder);
            builder.Entity<CCE_Exam_Term_MappingDMO>().ToTable("Exm_CCE_TERMS_MP");
            base.OnModelCreating(builder);
            builder.Entity<Exm_CCE_Activities_TransactionDMO>().ToTable("Exm_CCE_Activities_Transaction");
            base.OnModelCreating(builder);
            builder.Entity<Exm_CCE_SKILLS_TransactionDMO>().ToTable("Exm_CCE_SKILLS_Transaction");
        }
        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();

            updateUpdatedProperty<exammasterDMO>();            
            updateUpdatedProperty<Exm_ConfigurationDMO>();
            updateUpdatedProperty<Exm_Master_CategoryDMO>();
            updateUpdatedProperty<Exm_Category_ClassDMO>();
            updateUpdatedProperty<Exm_Master_GradeDMO>();
            updateUpdatedProperty<Exm_Master_Grade_DetailsDMO>();
            updateUpdatedProperty<Exm_Master_GroupDMO>();
            updateUpdatedProperty<Exm_Master_Group_SubjectsDMO>();
            updateUpdatedProperty<Exm_Yearly_CategoryDMO>();
            updateUpdatedProperty<Exm_Yearly_Category_GroupDMO>();
            updateUpdatedProperty<Exm_Yearly_Category_Group_SubjectsDMO>();
            updateUpdatedProperty<StudentMappingDMO>();
            updateUpdatedProperty<Exm_Yearly_Category_ExamsDMO>();
            updateUpdatedProperty<Exm_Yrly_Cat_Exams_SubwiseDMO>();
            updateUpdatedProperty<Exm_Yrly_Cat_Exams_Subwise_SubSubjectsDMO>();
            updateUpdatedProperty<Exm_Yrly_Cat_Exams_Subwise_SubExamsDMO>();
            updateUpdatedProperty<Exm_M_PromotionDMO>();
            updateUpdatedProperty<Exm_M_Promotion_SubjectsDMO>();
            updateUpdatedProperty<Exm_M_Prom_Subj_GroupDMO>();
            updateUpdatedProperty<Exm_M_Prom_Subj_Group_ExamsDMO>();
            updateUpdatedProperty<ExamMarksDMO>();
            updateUpdatedProperty<mastersubexamDMO>();
            updateUpdatedProperty<Exm_Login_PrivilegeDMO>();
            updateUpdatedProperty<Exm_Login_Privilege_SubjectsDMO>();
            updateUpdatedProperty<Exm_Login_Privilege_SubSubjectsDMO>();
            updateUpdatedProperty<Exm_Student_Marks_SubSubjectDMO>();
            updateUpdatedProperty<Exm_Student_MP_PromotionDMO>();
            updateUpdatedProperty<Exm_Stu_MP_Promo_SubjectwiseDMO>();
            updateUpdatedProperty<Exm_Stu_MP_Promo_Subjectwise_GroupwiseDMO>();
            updateUpdatedProperty<ExamsubjectGroupMappingDMO>();
            updateUpdatedProperty<ExamSubjectGroupMappingExamsDMO>();
            updateUpdatedProperty<ExamSubjectGroupMappingSubjectsDMO>();
            updateUpdatedProperty<exammasterpersonalityDMO>();
            updateUpdatedProperty<exammasterCoCulrricularDMO>();
            updateUpdatedProperty<exammasterRemarkDMO>();
            updateUpdatedProperty<exammasterPointDMO>();
            updateUpdatedProperty<CCE_Master_Life_SkillsDMO>();
            updateUpdatedProperty<CCE_Master_Life_Skill_AreasDMO>();
            updateUpdatedProperty<CCE_Master_Life_Skill_Areas_MappingDMO>();
            updateUpdatedProperty<CCE_M_CoScholasticActivitiesDMO>();
            updateUpdatedProperty<CCE_Exam_M_TermsDMO>();
            updateUpdatedProperty<CCE_M_Scholastic_AreasDMO>();
            updateUpdatedProperty<CCE_Exam_Term_MappingDMO>();
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
