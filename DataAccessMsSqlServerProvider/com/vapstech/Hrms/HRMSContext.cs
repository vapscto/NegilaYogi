using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.HRMS;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vapstech.admission;
using DomainModel.Model.com.vapstech.LeaveManagement;
using DomainModel.Model.com.vapstech.Birthday;
using DomainModel.Model.com.vapstech.TT;
using DomainModel.Model.com.vaps.Exam;

namespace DataAccessMsSqlServerProvider
{
    public class HRMSContext : DbContext
    {
        public HRMSContext(DbContextOptions<HRMSContext> options) : base(options)
        { }

        public DbSet<HR_Master_EmployeeType> HR_Master_EmployeeType { get; set; }       
        public DbSet<masterSpecialisationDMO> masterSpecialisationDMO { get; set; }
        public DbSet<masterLeavingReasonDMO> masterLeavingReasonDMO { get; set; }       
        public DbSet<Month> Month { get; set; }
        public DbSet<IVRM_Master_Marital_Status> IVRM_Master_Marital_Status { get; set; }
        public DbSet<IVRM_Master_Gender> IVRM_Master_Gender { get; set; }
        public DbSet<HR_Master_BankDeatils> HR_Master_BankDeatils { get; set; }
        public DbSet<HR_Master_GroupType> HR_Master_GroupType { get; set; }
        public DbSet<HR_Master_Department> HR_Master_Department { get; set; }
        public DbSet<HR_Master_Designation> HR_Master_Designation { get; set; }
        public DbSet<HM_T_ReimbursementClaim_DetailsDMO> HM_T_ReimbursementClaim_DetailsDMO { get; set; }
        public DbSet<HM_T_PolicyDetailsDMO> HM_T_PolicyDetailsDMO { get; set; }
        public DbSet<HR_Master_EarningsDeductions> HR_Master_EarningsDeductions { get; set; }
        public DbSet<HR_Master_Grade> HR_Master_Grade { get; set; }
        public DbSet<HR_Master_PFVPF_InterestDMO> HR_Master_PFVPF_InterestDMO { get; set; }
        public DbSet<HR_Resume_UploadDMO> HR_Resume_UploadDMO { get; set; }
        public DbSet<NAACHRMasterEmpFullTimeDMO> NAACHRMasterEmpFullTimeDMO { get; set; }        
        public DbSet<HR_Master_CourseDMO> HR_Master_Course { get; set; }
        public DbSet<HR_Master_IncomeTax_CessDMO> HR_Master_IncomeTaxCess { get; set; }
        public DbSet<Institution> Institution { get; set; }
        public DbSet<TT_Final_GenerationDMO> TT_Final_GenerationDMO { get; set; }
        public DbSet<TT_Final_Generation_DetailedDMO> TT_Final_Generation_DetailedDMO { get; set; }
        public DbSet<AdmissionClass> AdmissionClass { get; set; }
        public DbSet<School_M_Section> School_M_Section { get; set; }
        public DbSet<subjectmasterDMO> subjectmasterDMO { get; set; }
        public DbSet<ReligionCategory_MappingDMO> ReligionCategory_MappingDMO { get; set; }
        public DbSet<HRGroupDeptDessgDMO> HRGroupDeptDessgDMO { get; set; }
        public DbSet<HR_Master_LeaveYearDMO> HR_MasterLeaveYear { get; set; }
        public DbSet<HR_Master_ProfessionalTaxDMO> HR_MasterProfessionalTax { get; set; }
        public DbSet<HR_Master_IncomeTaxDMO> HR_MasterIncomeTax { get; set; }
        public DbSet<HR_Master_IncomeTax_DetailsDMO> HR_MasterIncomeTaxDetails { get; set; }
        public DbSet<HR_Master_IncomeTax_Details_CessDMO> HR_MasterIncomeTaxDetailsCess { get; set; }
        public DbSet<HR_MasterExam_GroupBDMO> HR_MasterExam_GroupBDMO { get; set; }
        public DbSet<HR_MasterExam_GroupADMO> HR_MasterExam_GroupADMO { get; set; }
        public DbSet<MasterEmployee> MasterEmployee { get; set; }
        public DbSet<MasterReligionDMO> Religion { get; set; }
        public DbSet<mastercasteDMO> Caste { get; set; }
        public DbSet<CasteCategory> CasteCategory { get; set; }
        public DbSet<Master_Employee_Experience> Master_Employee_Experience { get; set; }
        public DbSet<Master_Employee_Qulaification> Master_Employee_Qulaification { get; set; }
        public DbSet<Master_Employee_Documents> Master_Employee_Documents { get; set; }
        public DbSet<HR_Master_Employee_Bank> HR_Master_Employee_Bank { get; set; }
        public DbSet<HR_Employee_Salary> HR_Employee_Salary { get; set; }
        public DbSet<HR_Master_EarningsDeductions_Type> HR_Master_EarningsDeductions_Type { get; set; }
        public DbSet<HR_Configuration> HR_Configuration { get; set; }
        public DbSet<HR_Master_EarningsDeductionsPer> HR_Master_EarningsDeductionsPer { get; set; }

        public DbSet<HR_Master_Employee_IncrementDetails> HR_Master_Employee_IncrementDetails { get; set; }
        public DbSet<HR_Employee_EarningsDeductions> HR_Employee_EarningsDeductions { get; set; }
        public DbSet<HR_Employee_Salary_Details> HR_Employee_Salary_Details { get; set; }
        public DbSet<HR_Emp_Leave_Trans_Details_DMO> HR_Emp_Leave_Trans_Details { get; set; }
        public DbSet<HR_Master_Leave_DMO> HR_Master_Leave { get; set; }
        public DbSet<HR_Emp_Leave_StatusDMO> HR_Emp_Leave_StatusDMO { get; set; }
        public DbSet<IVRM_Master_FinancialYear> IVRM_Master_FinancialYear { get; set; }
        public DbSet<Multiple_Mobile_DMO> Emp_MobileNo { get; set; }
        public DbSet<Multiple_Email_DMO> Emp_Email_Id { get; set; }
        public DbSet<State> IVRM_Master_State { get; set; }
        public DbSet<Country> IVRM_Master_Country { get; set; }
        public DbSet<HR_Employee_Arrear_Salary> HR_Employee_Arrear_Salary { get; set; }
        public DbSet<HR_Emp_SalaryAdvance> HR_Emp_SalaryAdvance { get; set; }
        public DbSet<HRMasterLoan> HRMasterLoan { get; set; }
        public DbSet<HR_Emp_Loan> HR_Emp_Loan { get; set; }
        public DbSet<HR_Emp_Leave_Trans_DMO> HR_Emp_Leave_Trans { get; set; }
        public DbSet<IVRM_ModeOfPayment> IVRM_ModeOfPayment { get; set; }
        public DbSet<HR_PROCESSDMO> HR_PROCESSDMO { get; set; }
        public DbSet<HR_Process_Auth_OrderNoDMO> HR_Process_Auth_OrderNoDMO { get; set; }
        public DbSet<UserLogin> userLogin { get; set; }
        public DbSet<HR_PROCESS_PRIVILEGE> HR_PROCESS_PRIVILEGEDTO { get; set; }
        public DbSet<HR_Emp_Loan_TransactionDMO> HR_Emp_Loan_Transaction { get; set; }
        public DbSet<HR_Emp_Loan_ApprovalDMO> HR_Emp_Loan_Approval { get; set; }
        public DbSet<HR_Emp_SalaryAdvance_ApprovalDMO> HR_Emp_SalaryAdvanceApproval { get; set; }

        //    public DbSet<ApplicationUser> application { get; set; }
        public DbSet<ApplicationUserRole> appUserRole { get; set; }
        public DbSet<ApplRole> applicationRole {get; set;}
        public DbSet<Staff_User_Login> Staff_User_Login { get; set; }
        public DbSet<HRMasterPAN> HRMasterPAN { get; set; }
        public DbSet<HR_Emp_Salary_Approval> HR_Emp_Salary_ApprovalDMO { get; set; }
        public DbSet<ivrm_email_sentbox> ivrm_email_sentbox { get; set; }
        public DbSet<IVRM_sms_sentBoxDMO> IVRM_sms_sentBoxDMO { get; set; }
        public DbSet<HR_Employee_Assesment_Points> HR_Employee_Assementpoint { get; set; }
        public DbSet<HR_Employee_Assesment_Parameter> HR_Employee_Assementparameter { get; set; }
        public DbSet<HR_ECRDMO> HR_ECRDMO { get; set; }
        public DbSet<UserRoleWithInstituteDMO> UserRoleWithInstituteDMO { get; set; }
        public DbSet<ApplUser> applicationuser { get; set; }
        public DbSet<HR_Master_Quarter> HR_Master_quarter { get; set; }
        public DbSet<HR_Master_80C> HR_Master_80C { get; set; }
        public DbSet<HR_Emp_TDS> HR_Master_TDS { get; set; }
        public DbSet<HR_Master_Quarter_Month> HR_Master_quarter_month { get; set; }
        public DbSet<HR_Master_Allowance> HR_Master_Allowance { get; set; }
        public DbSet<HR_Emp_Allowance> HR_Master_Emp_Allowance { get; set; }
        public DbSet<HR_Emp_OtherIncome> HR_Master_Other_Income { get; set; }
        public DbSet<HR_Master_OtherIncome> HR_Master_OtherIncome { get; set; }
        public DbSet<HR_Emp_ChapterVI> HR_Employee_ChapterVI { get; set; }
        public DbSet<HR_Master_ChapterVI> HR_master_ChapterVI { get; set; }
        public DbSet<HR_Employee_Investment> HR_Employee_Investment { get; set; }
        public DbSet<HR_Employee_Subsection_Investment> HR_Employee_Subsection_Investment { get; set; }
        public DbSet<HR_Employee_Subsection_Investment_other> HR_Employee_Subsection_Investment_other { get; set; }
        public DbSet<HR_Employee_TDS_Quarter> HR_Employee_TDS_Quarter { get; set; }
        public DbSet<HR_Employee_Increment> HR_Employee_IncrementDMO { get; set; }
        public DbSet<HR_Employee_Increment_EDHeads> HR_Employee_Increment_EDHeadsDMO { get; set; }
        public DbSet<Fee_Master_ConcessionDMO> Fee_Master_ConcessionDMO { get; set; }
        public DbSet<HR_Employee_Awards_DMO> HR_Employee_Awards_DMO { get; set; }
        public DbSet<AcademicYear> AcademicYear { get; set; }
        public DbSet<HR_Employee_RemarksDMO> HR_Employee_RemarksDMO { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);
            //  builder.Entity<Staff_User_Login>().HasKey(m => m.IVRMSTAUL_Id);
            // builder.Entity<HR_Master_EmployeeType>().ToTable("HR_Master_EmployeeType");
        }

        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();

            updateUpdatedProperty<MasterEmployee>();


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
