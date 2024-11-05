using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainModel.Model;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs;
using DomainModel.Model.com.vaps.Fee;
using PreadmissionDTOs.com.vaps.Fees;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vapstech.Fee;
using DomainModel.Model.com.vapstech.HRMS;
using DomainModel.Model.com.vapstech.Transport;
using DomainModel.Model.com.vapstech.College.Admission;
using DomainModel.Model.com.vapstech.College.Fees;
using DomainModel.Model.com.vapstech.College.Preadmission;

namespace DataAccessMsSqlServerProvider.com.vapstech.College.Fee
{
    public class CollFeeGroupContext : DbContext
    {
       

        public CollFeeGroupContext(DbContextOptions<CollFeeGroupContext> options) :base(options)
        { Database.SetCommandTimeout(30000); }

        public DbSet<Fee_PaymentGateway_DetailsDMO> Fee_PaymentGateway_Details { get; set; }
       
        public DbSet<PAYUDETAILS> PAYUDETAILS { get; set; }
        public DbSet<MasterCourseDMO> MasterCourseDMO { get; set; }

        public DbSet<ClgMasterBranchDMO> ClgMasterBranchDMO { get; set; }

        public DbSet<Adm_Course_Branch_MappingDMO> coursebranch { get; set; }

        public DbSet<AdmCourseBranchSemesterMappingDMO> Coursebranchsem { get; set; }

        public DbSet<Adm_College_Master_SectionDMO> Adm_College_Master_SectionDMO { get; set; }

        public DbSet<MasterAcademic> AcademicYear { get; set; }

        public DbSet<CLG_Adm_College_AY_CourseDMO> CLG_Adm_College_AY_CourseDMO { get; set; }

        public DbSet<CLG_Adm_College_AY_Course_BranchDMO> CLG_Adm_College_AY_Course_BranchDMO { get; set; }
        public DbSet<Fee_Student_EnablePartialPayment_CollegeDMO> Fee_Student_EnablePartialPayment_CollegeDMO { get; set; }

        public DbSet<CLG_Adm_College_AY_Course_Branch_SemesterDMO> CLG_Adm_College_AY_Course_Branch_SemesterDMO { get; set; }

        public DbSet<CLG_Adm_Master_SemesterDMO> CLG_Adm_Master_SemesterDMO { get; set; }

        public DbSet<FeeGroupClgDMO> FeeGroupClgDMO { get; set; }
        public DbSet<FeeYearGroupClgDMO> FeeYearGroupDMO { get; set; }

        public DbSet<Fee_Master_Amount_OthStaffs> CLG_Adm_College_Seat_Distribution { get; set; }

        public DbSet<CLG_Fee_Yearly_Group_Head_Mapping> CLG_Fee_Yearly_Group_Head_Mapping { get; set; }

        // public DbSet<FeeGroupClgDMO> FeeGroupClgDMO { get; set; }
        public DbSet<FeeHeadClgDMO> FeeHeadClgDMO { get; set; }
        public DbSet<Clg_Fee_Installment_DMO> Clg_Fee_Installment_DMO { get; set; }
        public DbSet<Clg_Fee_Installments_Yearly_DMO> Clg_Fee_Installments_Yearly_DMO { get; set; }

        public DbSet<Clg_Fee_AmountEntry_DMO> Clg_Fee_AmountEntry_DMO { get; set; }

        public DbSet<CLG_Fee_College_Master_Amount_Semesterwise> CLG_Fee_College_Master_Amount_Semesterwise { get; set; }

        public DbSet<CLG_Fee_College_T_Due_DateDMO> CLG_Fee_College_T_Due_DateDMO { get; set; }
        public DbSet<CLG_Fee_College_T_Fine_Slabs> CLG_Fee_College_T_Fine_Slabs { get; set; }

        public DbSet<FeeFineSlabDMO> feeFS { get; set; }

        public DbSet<MasterMonthDMO> IVRM_Month { get; set; }

        public DbSet<Fee_College_Student_StatusDMO> Fee_College_Student_StatusDMO { get; set; }

        public DbSet<Fee_Y_Payment_PA_Application> Fee_Y_Payment_PA_Application { get; set; }
        public DbSet<Adm_College_Yearly_StudentDMO> Adm_College_Yearly_StudentDMO { get; set; }

        public  DbSet<Adm_Master_College_StudentDMO> Adm_Master_College_StudentDMO { get; set; }

        public DbSet<Fee_College_Master_Student_GroupHeadDMO> Fee_College_Master_Student_GroupHeadDMO { get; set; }

        public DbSet<Fee_C_Master_Student_GroupHead_InstallmentsDMO> Fee_C_Master_Student_GroupHead_InstallmentsDMO { get; set; }

        public  DbSet <Clg_Fee_Installment_Due_Date_DMO> Clg_Fee_Installment_Due_Date_DMO { get; set; }

        public DbSet<FEeGroupLoginPreviledgeDMO> FEeGroupLoginPreviledgeDMO { get; set; }

        public DbSet<MasterCompanyDMO> MasterCompanyDMO { get; set; }
       
        public DbSet<Fee_Groupwise_AutoReceiptDMO> Fee_Groupwise_AutoReceiptDMO { get; set; }
        public DbSet<Fee_Groupwise_AutoReceipt_GroupsDMO> Fee_Groupwise_AutoReceipt_GroupsDMO { get; set; }

        public DbSet<FeeMasterConfigurationDMO> feemastersettings { get; set; }
        public DbSet<Master_Numbering> Master_Numbering { get; set; }
        public DbSet<MasterRoleType> IVRM_Role_Type { get; set; }
        //public DbSet<FeePaymentDetailsDMO> FeePaymentDetailsDMO { get; set; }
        public DbSet<FeeTermDMO> feeTr { get; set; }
        public DbSet<FeeMasterTermHeadsDMO> feeMTH { get; set; }
        public DbSet<FeeSpecialFeeGroupDMO> feespecialHead { get; set; }
        public DbSet<FeeSpecialFeeGroupsGroupingDMO> feeSGGG { get; set; }
      
        public DbSet<CollegeConcessionDMO> CollegeConcessionDMO { get; set; }
        public DbSet<CollegeConcessionInstallmentDMO> CollegeConcessionInstallmentDMO { get; set; }
        public DbSet<Fee_Master_ConcessionDMO> Fee_Master_ConcessionDMO { get; set; }
        public DbSet<Fee_Y_PaymentDMO> Fee_Y_PaymentDMO { get; set; }
        public DbSet<Fee_Y_Payment_PaymentModeDMO> Fee_Y_Payment_PaymentModeDMO { get; set; }
        public DbSet<Fee_Y_Payment_College_StudentDMO> Fee_Y_Payment_College_StudentDMO { get; set; }
        public DbSet<Fee_T_College_PaymentDMO> Fee_T_College_PaymentDMO { get; set; }

        public DbSet<FeeGroupGroupingDMO> FeeGroupGroupingDMO { get; set; }
        public DbSet<FeeGroupMappingDMO> FeeGroupMappingDMO { get; set; }
        public DbSet<v_studentPendingconcessionDMO> v_studentPendingconcessionDMO { get; set; }

        public DbSet<Clg_Fee_Studentwise_DMO> Clg_Fee_Studentwise_DMO { get; set; }

        public DbSet<ApplicationUserDMO> applicationUser { get; set; }
        public DbSet<ApplicationUserRoleDMO> ApplicationUserRole { get; set; }
        public DbSet<IVRM_User_Login_InstitutionwiseDMO> UserRoleWithInstituteDMO { get; set; }

        public DbSet<Fee_College_Master_Opening_BalanceDMO> Fee_College_Master_Opening_BalanceDMO { get; set; }
        public DbSet<Fee_College_RefundDMO> Fee_College_RefundDMO { get; set; }
        public DbSet<Fee_College_Student_AdjustmentDMO> Fee_College_Student_AdjustmentDMO { get; set; }
        public DbSet<Fee_College_Cheque_BounceDMO> Fee_College_Cheque_BounceDMO { get; set; }
        public DbSet<Fee_College_Student_WaivedOffDMO> Fee_College_Student_WaivedOffDMO { get; set; }
        public DbSet<Clg_Adm_College_QuotaDMO> Clg_Adm_College_QuotaDMO { get; set; }
        public DbSet<ClgMasterCategoryDMO> mastercategory { get; set; }
        public DbSet<ClgMasterCourseCategoryMapDMO> ClgMasterCourseCategoryMapDMO { get; set; }

        public DbSet<Fee_M_Online_TransactionDMO> Fee_M_Online_TransactionDMO { get; set; }
        public DbSet<Fee_T_Online_TransactionDMO> Fee_T_Online_TransactionDMO { get; set; }

        public DbSet<Fee_Y_Payment_Preadmission_ApplicationDMO> Fee_Y_Payment_Preadmission_ApplicationDMO { get; set; }
        public DbSet<Fee_OnlinePayment_MappingDMO> Fee_OnlinePayment_Mapping { get; set; }
        public DbSet<Fee_OnlinePayment_MappingDMO> GenConfig { get; set; }
        public DbSet<AdmCollegeSchemeTypeDMO> AdmCollegeSchemeTypeDMO { get; set; }
        public DbSet<SMS_DETAILS_DMO> SMS_DETAILS_DMO { get; set; }
        public DbSet<SMSEmailSetting> smsEmailSetting { get; set; }
        public DbSet<MasterModule> masterModule { get; set; }
        public DbSet<Institution_Module> Institution_Module { get; set; }
        public DbSet<EMAIL_DETAILS_DMO> EMAIL_DETAILS_DMO { get; set; }
        public DbSet<Institution> Institution { get; set; }
        public DbSet<Institution_EmailId> Institution_EmailId { get; set; }
        public DbSet<SMS_MAIL_SAVED_PARAMETER_DMO> SMS_MAIL_SAVED_PARAMETER_DMO { get; set; }
        public DbSet<SMS_MAIL_PARAMETER_DMO> SMS_MAIL_PARAMETER_DMO { get; set; }
        public DbSet<Institution> master_institution { get; set; }
        public DbSet<CLG_Fee_OnlinePayment_MappingDMO> CLG_Fee_OnlinePayment_MappingDMO { get; set; }

        public DbSet<MOBILE_INSTITUTION> MOBILE_INSTITUTION { get; set; }

        public DbSet<FEE_RAZOR_TRANSFER_API_DETAILS> FEE_RAZOR_TRANSFER_API_DETAILS { get; set; }
        public DbSet<PA_College_Application> PA_College_Application { get; set; }
        public DbSet<ClgQuotaFeeGroupDMO> ClgQuotaFeeGroupDMO { get; set; }
        public DbSet<Clg_Adm_College_Quota_CategoryDMO> Clg_Adm_College_Quota_CategoryDMO { get; set; }

        public DbSet<CollegePaymentApprovalDMO> CollegePaymentApprovalDMO { get; set; }
        public DbSet<Staff_User_Login> Staff_User_Login { get; set; }
        public DbSet<Fee_Master_BankDMO> Fee_Master_BankDMO { get; set; }
        public DbSet<IVRM_ModeOfPayment> IVRM_ModeOfPayment { get; set; }
        public DbSet<HR_PROCESSDMO> HR_PROCESSDMO { get; set; }

        public DbSet<IVRM_EMAIL_ATT_DMO> IVRM_EMAIL_ATT_DMO { get; set; }
        public DbSet<Fee_College_Studentwise_PDCDMO> Fee_College_Studentwise_PDCDMO { get; set; }
        public DbSet<GeneralConfigDMO> GenConfiguration { get; set; }
        public DbSet<Fee_Payment_Settlement_Details_CollegeDMO> Fee_Payment_Settlement_Details_CollegeDMO { get; set; }
        //added//
        public DbSet<FeeAmountEntryDMO> FeeAmountEntryDMO { get; set; }

        public DbSet<FeeTDueDateRegularDMO> feeTDueDateRegularDMO { get; set; }
        public DbSet<FeeTDueDateECSDMO> feeTDueDateECSDMO { get; set; }
        public DbSet<FeeTFineSlabDMO> feeTFineSlabDMO { get; set; }
        public DbSet<FeeTFineSlabECSDMO> feeTFineSlabECSDMO { get; set; }
        //other
        public DbSet<Fee_Master_Amount_OthStaffs> Fee_Master_Amount_OthStaffs { get; set; }
        public DbSet<Fee_T_Fine_Slabs_OthStaffs> Fee_T_Fine_Slabs_OthStaffs { get; set; }
        public DbSet<Fee_T_Due_Date_OthStaffs> Fee_T_Due_Date_OthStaffs { get; set; }

        public DbSet<MasterMonthDMO> masterMonthDMO { get; set; }
        public DbSet<MasterMonthECSDMO> masterMonthECSDMO { get; set; }

        public DbSet<Fee_Others_ConcessionDMO> Fee_Others_ConcessionDMO { get; set; }
        public DbSet<Fee_Others_Concession_InstallmentsDMO> Fee_Others_Concession_InstallmentsDMO { get; set; }

        public DbSet<Fee_Employee_ConcessionDMO> Fee_Employee_ConcessionDMO { get; set; }
        public DbSet<Fee_Employee_Concession_InstallmentsDMO> Fee_Employee_Concession_InstallmentsDMO { get; set; }
        public DbSet<FeeMasterOtherStudentDMO> FeeMasterOtherStudentDMO { get; set; }
        public DbSet<Fee_Student_Status_StaffDMO> Fee_Student_Status_StaffDMO { get; set; }

        public DbSet<Fee_Student_Status_OthStuDMO> Fee_Student_Status_OthStuDMO { get; set; }
        public DbSet<v_studentPendingsavedconcessionDMO> v_studentPendingsavedconcessionDMO { get; set; }
        public DbSet<MasterEmployee> MasterEmployee { get; set; }
        public DbSet<HR_Master_Department> HR_Master_Department { get; set; }
        public DbSet<HR_Master_Designation> HR_Master_Designation { get; set; }

        public DbSet<Fee_Master_Staff_GroupHead> Fee_Master_Staff_GroupHead { get; set; }

        public DbSet<Fee_Master_OthStudents_GHDMO> Fee_Master_OthStudents_GHDMO { get; set; }

        public DbSet<Fee_Master_Staff_GroupHead_Installments> Fee_Master_Staff_GroupHead_Installments { get; set; }

        public DbSet<Fee_Master_OthStudents_GH_InstlDMO> Fee_Master_OthStudents_GH_InstlDMO { get; set; }

        public DbSet<Fee_College_Student_Status_Staff> Fee_College_Student_Status_Staff { get; set; }

        public DbSet<Fee_Master_College_Staff_GroupHeadDMO> Fee_Master_College_Staff_GroupHeadDMO { get; set; }

        public DbSet<Fee_Master_College_Staff_GroupHead_InstallmentsDMO> Fee_Master_College_Staff_GroupHead_InstallmentsDMO { get; set; }

        public DbSet<Fee_College_Student_Status_OthStuDMO> Fee_College_Student_Status_OthStuDMO { get; set; }

        public DbSet<Fee_Master_College_OthStudents_GHDMO> Fee_Master_College_OthStudents_GHDMO { get; set; }

        public DbSet<Fee_Master_College_OthStudents_GH_InstlDMO> Fee_Master_College_OthStudents_GH_InstlDMO { get; set; }

        public DbSet<Fee_Master_College_OtherStudents> Fee_Master_College_OtherStudents { get; set; }

        public DbSet<Fee_Employee_Concession_CollegeDMO> Fee_Employee_Concession_CollegeDMO { get; set; }

        public DbSet<Fee_Employee_Concession_Installments_CollegeDMO> Fee_Employee_Concession_Installments_CollegeDMO { get; set; }

        public DbSet<Fee_Others_Concession_CollegeDMO> Fee_Others_Concession_CollegeDMO { get; set; }

        public DbSet<Fee_Others_Concession_Installments_CollegeDMO> Fee_Others_Concession_Installments_CollegeDMO { get; set; }

        public DbSet<Fee_Y_Payment_College_StaffDMO> Fee_Y_Payment_College_StaffDMO { get; set; }

        public DbSet<Fee_T_Payment_OthStaffDMO> Fee_T_Payment_OthStaffDMO { get; set; }
        public DbSet<Fee_Y_Payment_OthStu_CollegeDMO> Fee_Y_Payment_OthStu_CollegeDMO { get; set; }

        public DbSet<Fee_T_Payment_OthStaff_College_CollegeDMO> Fee_T_Payment_OthStaff_College_CollegeDMO { get; set; }

        public DbSet<Adm_College_Student_CEMarksDMO> Adm_College_Student_CEMarksDMO { get; set; }

        public DbSet<Fee_Master_College_Amount_OthStaffs> Fee_Master_College_Amount_OthStaffs { get; set; }

        public DbSet<Fee_T_Due_Date_CollegeOthStaffs> Fee_T_Due_Date_CollegeOthStaffs { get; set; }

        public DbSet<Fee_T_Fine_Slabs_CollegeOthStaffs> Fee_T_Fine_Slabs_CollegeOthStaffs { get; set; }
        public DbSet<MasterConfiguration> MasterConfiguration { get; set; }
        public DbSet<Prospepaymentamount> Prospepaymentamount { get; set; }
        public DbSet<AdmissionStatus> AdmissionStatus { get; set; }

        public DbSet<PA_College_Student_CEMarks_SubjectClgDMO> PA_College_Student_CEMarks_SubjectClgDMO { get; set; }

        public DbSet<PA_College_Student_CEMarksClgDMO> PA_College_Student_CEMarksClgDMO { get; set; }


        

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









