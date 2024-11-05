using Microsoft.EntityFrameworkCore;
using System.Linq;
using DomainModel.Model.com.vapstech.VMS.HRMS;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.HRMS;
using DomainModel.Model.com.vapstech.College.Fees;

using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vapstech.VMS.Training;
using DomainModel.Model.com.vapstech.COE;
using DomainModel.Model.com.vapstech.admission;
using DomainModel.Model.com.vapstech.VMS.Exit;
using DomainModel.Model.com.vapstech.VMS.Sales;

using DomainModel.Model.com.vapstech.FrontOffice;
using DomainModel.Model.com.vapstech.VMS;

namespace DataAccessMsSqlServerProvider
{
    public class VMSContext : DbContext
    {

        public VMSContext(DbContextOptions<VMSContext> options) : base(options)
        { }

        public DbSet<OT_Candidates_Exam_SubjectiveAnswerDMO> OT_Candidates_Exam_SubjectiveAnswerDMO { get; set; }
        public DbSet<OT_Candidates_Exam_AnswerDMO> OT_Candidates_Exam_AnswerDMO { get; set; }
        public DbSet<OT_Candidates_ExamDMO> OT_Candidates_ExamDMO { get; set; }
        public DbSet<OT_QuestionPaperTypeDMO> OT_QuestionPaperTypeDMO { get; set; }
        public DbSet<OT_Master_OE_QNS_OptionsDMO> OT_Master_OE_QNS_OptionsDMO { get; set; }
        public DbSet<OT_Master_OE_QuestionsDMO> OT_Master_OE_QuestionsDMO { get; set; }
        public DbSet<ISM_Client_Project_Docs_DMO> ISM_Client_Project_Docs_DMO_con { get; set; }
        public DbSet<ISM_Client_Project_Master_Docs_DMO> ISM_Client_Project_Master_Docs_DMO_con { get; set; }
        public DbSet<ISM_Client_Project_ManPower_DMO> ISM_Client_Project_ManPower_DMO_con { get; set; }
        public DbSet<ISM_Client_Master_Components_DMO> ISM_Client_Master_Components_DMO_con { get; set; }
        public DbSet<ISM_Client_Project_BOM_DMO> ISM_Client_Project_BOM_DMO_con { get; set; }
        public DbSet<HR_Candidate_DetailsDMO> HR_Candidate_DetailsDMO { get; set; }
        public DbSet<HR_Candidate_ExperienceDMO> HR_Candidate_ExperienceDMO { get; set; }
        public DbSet<HR_Candidate_FamilyDMO> HR_Candidate_FamilyDMO { get; set; }
        public DbSet<HR_Candidate_LanguagesDMO> HR_Candidate_LanguagesDMO { get; set; }
        public DbSet<HR_Candidate_QualificationsDMO> HR_Candidate_QualificationsDMO { get; set; }
        public DbSet<EMAIL_DETAILS_DMO> EMAIL_DETAILS_DMO { get; set; }
        public DbSet<HR_CandidateInterviewScheduleDMO> HR_CandidateInterviewScheduleDMO { get; set; }
        public DbSet<HR_Candidate_Master_GradeDMO> HR_Candidate_Master_GradeDMO { get; set; }
        public DbSet<HR_Candidate_InterviewStatusDMO> HR_Candidate_InterviewStatusDMO { get; set; }
        public DbSet<HR_InterviewDMO> HR_InterviewDMO { get; set; }
        public DbSet<HR_InvitationToCandidateDMO> HR_InvitationToCandidateDMO { get; set; }
        public DbSet<HR_JobApplicationDMO> HR_JobApplicationDMO { get; set; }
        public DbSet<HR_JobDetailsDMO> HR_JobDetailsDMO { get; set; }
        public DbSet<HR_Master_CandidateTypeDMO> HR_Master_CandidateTypeDMO { get; set; }
        public DbSet<HR_Master_JobsDMO> HR_Master_JobsDMO { get; set; }
        public DbSet<HR_Master_NEmployeeDMO> HR_Master_NEmployeeDMO { get; set; }
        public DbSet<HR_Master_PositionDMO> HR_Master_PositionDMO { get; set; }
        public DbSet<HR_Master_PostionTypeDMO> HR_Master_PostionTypeDMO { get; set; }
        public DbSet<HR_Master_PriorityDMO> HR_Master_PriorityDMO { get; set; }
        public DbSet<HR_Master_Qual_CategoryDMO> HR_Master_Qual_CategoryDMO { get; set; }
        public DbSet<HR_Master_Qual_SubCategoryDMO> HR_Master_Qual_SubCategoryDMO { get; set; }
        public DbSet<HR_Master_Qual_SubjectDMO> HR_Master_Qual_SubjectDMO { get; set; }
        public DbSet<HR_Master_ReferenceDMO> HR_Master_ReferenceDMO { get; set; }
        public DbSet<HR_MRF_ListDMO> HR_MRF_ListDMO { get; set; }
        public DbSet<HR_MRFJOB_DetailsDMO> HR_MRFJOB_DetailsDMO { get; set; }
        public DbSet<HR_MRFRequisitionDMO> HR_MRFRequisitionDMO { get; set; }
        public DbSet<HR_Master_CourseDMO> HR_Master_Course { get; set; }
        public DbSet<State> IVRM_Master_State { get; set; }
        public DbSet<Country> IVRM_Master_Country { get; set; }
        public DbSet<HR_Master_Department> HR_Master_Department { get; set; }
        public DbSet<HR_Master_Designation> HR_Master_Designation { get; set; }
        public DbSet<UserLogin> userLogin { get; set; }
        public DbSet<ApplicationUserDMO> ApplicationUserDMO { get; set; }
        public DbSet<HR_Master_LocationDMO> HR_Master_LocationDMO { get; set; }
        public DbSet<IVRM_Master_Gender> IVRM_Master_Gender { get; set; }
        public DbSet<HR_PROCESSDMO> HR_PROCESSDMO { get; set; }
        public DbSet<HR_Process_Auth_OrderNoDMO> HR_Process_Auth_OrderNoDMO { get; set; }
        public DbSet<HR_Master_EarningsDeductions> HR_Master_EarningsDeductions { get; set; }
        public DbSet<HR_Master_EarningsDeductionsPer> HR_Master_EarningsDeductionsPer { get; set; }
        public DbSet<HR_Candidate_EarningsDeductionsDMO> HR_Candidate_EarningsDeductionsDMO { get; set; }
        public DbSet<HR_Employee_EarningsDeductions> HR_Employee_EarningsDeductions { get; set; }
        public DbSet<HR_Master_DepartmentCode_HeadDMO> HR_Master_DepartmentCode_HeadDMO { get; set; }
        public DbSet<ISM_Sales_Master_Source_DMO> ISM_Sales_Master_Source_DMO { get; set; }
        public DbSet<Month> month { get; set; }
        public DbSet<SecurityDetailsDMO> SecurityDetailsDMO { get; set; }
        public DbSet<SecurityRoasterDMO> SecurityRoasterDMO { get; set; }
        public DbSet<HR_Master_DepartmentCodeDMO> HR_Master_DepartmentCodeDMO { get; set; }
        public DbSet<Institution> Institution { get; set; }
        public DbSet<MasterEmployee> HR_Master_Employee_DMO { get; set; }
        public DbSet<MasterAcademic> AcademicYear { get; set; }
        public DbSet<Staff_User_Login> Staff_User_Login { get; set; }
        //public DbSet<ApplUser> ApplUser { get; set; }

        public DbSet<ISM_Master_Client_DMO> ISM_Master_Client_DMO { get; set; }
        public DbSet<SMSEmailSetting> SMSEmailSetting { get; set; }
        public DbSet<Staff_User_Login> IVRM_Staff_User_Login { get; set; }
        public DbSet<VMS_CallLetter_sentDMO> VMS_CallLetter_sentDMO { get; set; }
        public DbSet<HR_Candidate_AppointmentDMO> HR_Candidate_AppointmentDMO { get; set; }

        public DbSet<Master_Employee_Qulaification> Master_Employee_Qulaification_con { get; set; }
        public DbSet<MasterEmployee> Hr_Master_Employee_con { get; set; }
        public DbSet<HR_Training_Employee_DMO> HR_Training_Employee_DMO_con { get; set; }
        public DbSet<HR_Training_Status_DMO> HR_Training_Status_DMO_con { get; set; }
        public DbSet<Staff_User_Login> Staff_User_Login_con { get; set; }
        public DbSet<HR_Master_EmployeeType> HR_Master_EmployeeType { get; set; }
        public DbSet<HR_Master_GroupTypeDMO> HR_Master_GroupTypeDMO { get; set; }
        public DbSet<HR_Master_Grade> HR_Master_Grade { get; set; }
        public DbSet<IVRM_Master_Marital_Status> IVRM_Master_Marital_Status { get; set; }
        public DbSet<castecategoryDMO> castecategoryDMO { get; set; }
        public DbSet<mastercasteDMO> mastercasteDMO { get; set; }
        public DbSet<MasterReligionDMO> MasterReligionDMO { get; set; }


        //praveen
        public DbSet<HR_Master_Amenities_DMO> HR_Master_Amenities_DMO { get; set; }
        public DbSet<HR_Master_Room_FilesDMO> HR_Master_Room_FilesDMO { get; set; }
        public DbSet<HR_Master_Room_AmenitiesDMO> HR_Master_Room_AmenitiesDMO { get; set; }
        public DbSet<HR_Master_Room_ContactsDMO> HR_Master_Room_ContactsDMO { get; set; }
        public DbSet<Multiple_Mobile_DMO> Multiple_Mobile_DMO { get; set; }
        public DbSet<Multiple_Email_DMO> Multiple_Email_DMO { get; set; }
        public DbSet<Institution_Module_Page> Institution_Module_Page { get; set; }
        public DbSet<Institution_Module> Institution_Module { get; set; }
        public DbSet<MasterPage> MasterPage { get; set; }
        public DbSet<MasterModule> masterModule { get; set; }

        //shivu

        //Training
        public DbSet<MasterRoleType> IVRM_Role_Type { get; set; }
        public DbSet<GeneralConfigDMO> GeneralConfigDMO { get; set; }
        public DbSet<HR_Master_Building> HR_Master_Building_con { get; set; }
        public DbSet<HR_Master_Floor> HR_Master_Floor_con { get; set; }
        public DbSet<HR_Master_Room> HR_Master_Room_con { get; set; }
        public DbSet<HR_Training_Create_DMO> HR_Training_Create_DMO_con { get; set; }
        public DbSet<HR_Master_External_Trainer_Creation_DMO> HR_Master_External_Trainer_Creation_DMO_con { get; set; }
        public DbSet<HR_Training_Create_Participants_DMO> HR_Training_Create_Participants_DMO_con { get; set; }
        public DbSet<HR_Training_Create_IntTrainer_DMO> HR_Training_Create_IntTrainer_DMO_con { get; set; }
        public DbSet<HR_Training_Create_ExtTrainer_DMO> HR_Training_Create_ExtTrainer_DMO_con { get; set; }
        public DbSet<HR_Master_Feedback_Qns_DMO> HR_Master_Feedback_Qns_DMO_con { get; set; }
        public DbSet<HR_Master_Feedback_Option_DMO> HR_Master_Feedback_Option_DMO_con { get; set; }
        public DbSet<HR_Master_Question_Option_DMO> HR_Master_Question_Option_DMO_con { get; set; }
        public DbSet<HR_Training_Question_DMO> HR_Training_Question_DMO_con { get; set; }
        public DbSet<HR_Training_Feedback_DMO> HR_Training_Feedback_DMO_con { get; set; }
        public DbSet<Master_External_TrainingTypeDMO> Master_External_TrainingTypeDMO { get; set; }
        public DbSet<Master_External_TrainingCentersDMO> Master_External_TrainingCentersDMO { get; set; }
        public DbSet<External_TrainingDMO> External_TrainingDMO { get; set; }

        //Exit
        public DbSet<ISM_Resignation_ChecKLists_DMO> ISM_Resignation_ChecKLists_DMO_con { get; set; }
        public DbSet<ISM_Resignation_DMO> ISM_Resignation_DMO_con { get; set; }
        public DbSet<ISM_Resignation_Master_CheckLists_DMO> ISM_Resignation_Master_CheckLists_DMO_con { get; set; }
        public DbSet<ISM_Resignation_Master_Reasons_DMO> ISM_Resignation_Master_Reasons_DMO_con { get; set; }
        public DbSet<ISM_Resignation_RelievingLetter_DMO> ISM_Resignation_RelievingLetter_DMO_con { get; set; }

        public DbSet<HR_Master_Designation> HR_Master_Designation_con { get; set; }
        //----------------------------------------------------------------
        //Sales
        public DbSet<ISM_Sales_Master_Product_DMO> ISM_Sales_Master_Product_DMO_con { get; set; }
        public DbSet<ISM_Sales_Master_Category_DMO> ISM_Sales_Master_Category_DMO_con { get; set; }
        public DbSet<ISM_Sales_Master_Source_DMO> ISM_Sales_Master_Source_DMO_con { get; set; }
        public DbSet<ISM_Sales_Master_Status_DMO> ISM_Sales_Master_Status_DMO_con { get; set; }
        public DbSet<ISM_Sales_Lead_DMO> ISM_Sales_Lead_DMO_con { get; set; }
        public DbSet<ISM_Sales_Lead_Products_DMO> ISM_Sales_Lead_Products_DMO_con { get; set; }
        public DbSet<ISM_Sales_Lead_Demo_DMO> ISM_Sales_Lead_Demo_DMO_con { get; set; }
        public DbSet<ISM_Sales_Lead_Demo_Products_DMO> ISM_Sales_Lead_Demo_Products_DMO_con { get; set; }
        public DbSet<HR_Master_TrainingTopicDMO> HR_Master_TrainingTopicDMO { get; set; }
        
        public DbSet<External_Training_ApprovalDMO> External_Training_ApprovalDMO { get; set; }
        public DbSet<IVRM_Training_TransactionDMO> IVRM_Training_TransactionDMO { get; set; }
        public DbSet<IVRM_Training_MasterTrainerDMO> IVRM_Training_MasterTrainerDMO { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);
            //  builder.Entity<Staff_User_Login>().HasKey(m => m.IVRMSTAUL_Id);
            // builder.Entity<HR_Master_EmployeeType>().ToTable("HR_Master_EmployeeType");
        }

        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();

            updateUpdatedProperty<HR_Candidate_DetailsDMO>();


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
