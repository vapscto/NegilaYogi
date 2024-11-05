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
using DomainModel.Model.com.vapstech.admission;
using DomainModel.Model.com.vaps.Exam;
using DomainModel.Model.com.vapstech.Fee.Tally;
using DomainModel.Model.com.vapstech.PDA;

using DomainModel.Model.com.vapstech.College.Fees;

using PreadmissionDTOs.com.vaps.Fees.Tally;
using DomainModel.Model.com.vapstech.Hostel;
using DomainModel.Model.com.vapstech.Fee.FinancialAccounting;
using DomainModel.Model.com.vapstech.Inventory;

namespace DataAccessMsSqlServerProvider
{
    public class FeeGroupContext : DbContext
    {
        public FeeGroupContext(DbContextOptions<FeeGroupContext> options) : base(options)
        { Database.SetCommandTimeout(300000000); }

        public DbSet<Fee_Student_Defaulter_Remarks_DMO> Fee_Student_Defaulter_Remarks_DMO_con { get; set; }
        public DbSet<v_studentPendingsavedconcessionDMO> v_studentPendingsavedconcessionDMO { get; set; }
        public DbSet<v_studentPendingconcessionDMO> v_studentPendingconcessionDMO { get; set; }
        public DbSet<Fee_Master_ConcessionDMO> Fee_Master_ConcessionDMO { get; set; }
        public DbSet<Institution> master_institution { get; set; }
        public DbSet<FeeGroupDMO> feeGroup { get; set; }
        public DbSet<MasterAcademic> AcademicYear { get; set; }
        public DbSet<IVRM_Master_FinancialYear> IVRM_Master_FinancialYear { get; set; }
        public DbSet<FeeYearGroupDMO> Yearlygroups { get; set; }
        //extra
        public DbSet<Fee_FeeGroup_CompanyMappingDMO> Fee_FeeGroup_CompanyMappingDMO { get; set; }
        public DbSet<Staff_User_Login> Staff_User_Login { get; set; }
        public DbSet<FeeSpecialFeeGroupDMO> feespecialHead { get; set; }
        public DbSet<FeeHeadDMO> feehead { get; set; }
        public DbSet<FeeGroupMappingDMO> feegm { get; set; }
        public DbSet<FeeClassCategoryDMO> feeCC { get; set; }
        public DbSet<School_M_Class> admissioncls { get; set; }
        public DbSet<School_M_Section> school_M_Section { get; set; }
        public DbSet<Fee_Y_Payment_PaymentModeDMO> Fee_Y_Payment_PaymentModeDMO { get; set; }
        public DbSet<FeeYearlyClassCategoryDMO> feeYCC { get; set; }
        public DbSet<FeeFineSlabDMO> feeFS { get; set; }
        public DbSet<FeeTermDMO> feeTr { get; set; }
        public DbSet<FeeInstallmentDMO> feeMI { get; set; }
        public DbSet<FeeInstallmentsyearlyDMO> feeMIY { get; set; }
        public DbSet<FeeInstallmentDueDateDMO> feeIDDD { get; set; }
        public DbSet<FeeMasterConfigurationDMO> feemastersettings { get; set; }
        public DbSet<FeeMasterTermHeadsDMO> feeMTH { get; set; }
        public DbSet<MasterTermFeeHeadsDueDateDMO> feeTHDDD { get; set; }
        public DbSet<Pre_Adm_Syllabus> Pre_Adm_Syllabus { get; set; }
        public DbSet<FeeGroupGroupingDMO> feeGGG { get; set; }
        public DbSet<FeeSpecialFeeGroupsGroupingDMO> feeSGGG { get; set; }        
       
        public DbSet<HR_Master_Room_DMO> HR_Master_Room_DMO { get; set; }
        public DbSet<HL_Master_Hostel_DMO> HL_Master_Hostel_DMO { get; set; }
        public DbSet<HR_Master_Floor_DMO> HR_Master_Floor_DMO { get; set; }
        public DbSet<FeeYearlygroupHeadMappingDMO> FeeYearlygroupHeadMappingDMO { get; set; }
        public DbSet<FeeInstallmentDueDateDMO> Feeduedateinstall { get; set; }
        public DbSet<FeeGroupDMO> FeeGroupDMO { get; set; }
        public DbSet<FeeHeadDMO> FeeHeadDMO { get; set; }
        public DbSet<MasterCompanyDMO> MasterCompanyDMO { get; set; }
        public DbSet<FeeInstallmentDMO> FeeInstallmentDMO { get; set; }
        public DbSet<FeeStudentEnablePartialPaymentDMO> FeeStudentEnablePartialPaymentDMO { get; set; }
        public DbSet<FeeAmountEntryDMO> FeeAmountEntryDMO { get; set; }
        public DbSet<FeeClassCategoryDMO> FeeClassCategoryDMO { get; set; }
        public DbSet<FeeInstallmentsyearlyDMO> FeeInstallmentsyearlyDMO { get; set; }
        public DbSet<StudentHelthcertificateDMO> StudentHelthcertificate { get; set; }
        public DbSet<School_M_Class> School_M_Class { get; set; }
        public DbSet<AdmissionAreaDMO> AdmissionAreaDMO { get; set; }
        public DbSet<FeeStudentGroupMappingDMO> FeeStudentGroupMappingDMO { get; set; }
        public DbSet<Adm_M_Student> AdmissionStudentDMO { get; set; }
        public DbSet<School_Adm_Y_StudentDMO> School_Adm_Y_StudentDMO { get; set; }
        public DbSet<FeeStudentGroupInstallmentMappingDMO> FeeStudentGroupInstallmentMappingDMO { get; set; }
        public DbSet<FeeStudentTransactionDMO> FeeStudentTransactionDMO { get; set; }
        public DbSet<FeePaymentDetailsDMO> FeePaymentDetailsDMO { get; set; }
        public DbSet<FeeTransactionPaymentDMO> FeeTransactionPaymentDMO { get; set; }
        public DbSet<MasterCategory> masterCategory { get; set; }
        public DbSet<FeeChequeBounceDMO> FeeChequeBounceDMO { get; set; }
        public DbSet<FeeMasterRefundDMO> FeeMasterRefundDMO { get; set; }
        public DbSet<SMSEmailSetting> sMSEmailSetting { get; set; }
        public DbSet<FeeTDueDateECSDMO> feeTDueDateECSDMO { get; set; }
        public DbSet<FeeTDueDateRegularDMO> feeTDueDateRegularDMO { get; set; }
        public DbSet<MasterMonthDMO> masterMonthDMO { get; set; }
        public DbSet<MasterMonthECSDMO> masterMonthECSDMO { get; set; }
        public DbSet<FeeTFineSlabDMO> feeTFineSlabDMO { get; set; }
        public DbSet<FeeTFineSlabECSDMO> feeTFineSlabECSDMO { get; set; }
        public DbSet<V_StudentPendingDMO> V_StudentPendingDMO { get; set; }
        public DbSet<Masterclasscategory> masterclasscategory { get; set; }
        public DbSet<MasterYearlyClassCategoryClassDMO> feeYCCC { get; set; }
        public DbSet<StudentApplication> stuapp { get; set; }
        public DbSet<Fee_Y_Payment_Preadmission_RegistrationDMO> Fee_Y_Payment_Preadmission_RegistrationDMO { get; set; }
        public DbSet<Fee_Y_Payment_Preadmission_ApplicationDMO> Fee_Y_Payment_Preadmission_ApplicationDMO { get; set; }
        public DbSet<Fee_Y_Payment_School_StudentDMO> Fee_Y_Payment_School_StudentDMO { get; set; }
        public DbSet<Institution> Institutionds { get; set; }
        public DbSet<ApplUser> applicationUser { get; set; }
        public DbSet<UserRoleWithInstituteDMO> UserRoleWithInstituteDMO { get; set; }
        public DbSet<Adm_Student_Transport_ApplicationDMO> Adm_Student_Transport_ApplicationDMO { get; set; }
        public DbSet<FeeClassCategoryDMO> Class_Category { get; set; }
        //  public DbSet<AdmissionClass> AdmissionClass { get; set; }

        public DbSet<FeePaymentDetailsDMO> Fee_Payment { get; set; }
        public DbSet<FeeConcessionDMO> FeeConcessionDMO { get; set; }
        public DbSet<FeeConcessionInstallmentsDMO> FeeConcessionInstallmentsDMO { get; set; }
        public DbSet<FeeOpeningBalanceDMO> feeOpeningBalance { get; set; }
        //radha
        public DbSet<Prospepaymentamount> Prospepaymentamount { get; set; }
        public DbSet<FeeBankDetailsDMO> FeeBankDetailsDMO { get; set; }
        public DbSet<FEeGroupLoginPreviledgeDMO> FEeGroupLoginPreviledgeDMO { get; set; }
        public DbSet<Fee_Master_ConcessionDMO> catergory { get; set; }
        public DbSet<Fee_Groupwise_AutoReceiptDMO> Fee_Groupwise_AutoReceiptDMO { get; set; }
        public DbSet<Fee_Groupwise_AutoReceipt_GroupsDMO> Fee_Groupwise_AutoReceipt_GroupsDMO { get; set; }
        public DbSet<FeeStudentAdjustment> FeeStudentAdjustment { get; set; }
        public DbSet<ApplicationUserRole> ApplicationUserRole { get; set; }
        public DbSet<MasterRoleType> IVRM_Role_Type { get; set; }
        public DbSet<FeeStudentAdjustment> feeStudentAdjustment { get; set; }
        public DbSet<FeeStudentWaivedOffDMO> feeStudentWaivedOff { get; set; }
        public DbSet<Fee_OnlinePayment_MappingDMO> Fee_OnlinePayment_Mapping { get; set; }
        public DbSet<Fee_PaymentGateway_DetailsDMO> Fee_PaymentGateway_Details { get; set; }
        public DbSet<Enquiry> Enquiry { get; set; }
        public DbSet<Master_Numbering> Master_Numbering { get; set; }
        public DbSet<MasterEmployee> MasterEmployee { get; set; }
        public DbSet<HR_Master_Department> HR_Master_Department { get; set; }
        public DbSet<HR_Master_Designation> HR_Master_Designation { get; set; }
        public DbSet<Fee_Staff_Status> Fee_Staff_Status { get; set; }
        public DbSet<Fee_Master_Staff_GroupHead> Fee_Master_Staff_GroupHead { get; set; }
        public DbSet<Fee_Master_Staff_GroupHead_Installments> Fee_Master_Staff_GroupHead_Installments { get; set; }
        public DbSet<Fee_Y_Payment_PA_RegistrationDMO> Fee_Y_Payment_PA_RegistrationDMO { get; set; }
        public DbSet<MasterConfiguration> mstConfig { get; set; }
        public DbSet<AdmissionStatus> AdmissionStatus { get; set; }
        public DbSet<AdmissionStandardDMO> AdmissionStandardDMO { get; set; }
        public DbSet<Fee_Master_Amount_OthStaffs> Fee_Master_Amount_OthStaffs { get; set; }
        public DbSet<Fee_T_Fine_Slabs_OthStaffs> Fee_T_Fine_Slabs_OthStaffs { get; set; }
        public DbSet<Fee_T_Due_Date_OthStaffs> Fee_T_Due_Date_OthStaffs { get; set; }

        //Date:02-11-2017 by Sripad Joshi.
        public DbSet<FeeMasterOtherStudentDMO> FeeMasterOtherStudentDMO { get; set; }
        public DbSet<FEE_MASTER_TERMWISE_PERIOD_DMO> FEE_MASTER_TERMWISE_PERIOD_DMO { get; set; }     
        public DbSet<FeeCardDetailsEntryDMO> feeCardDetailsEntry { get; set; }
        //public DbSet<Fee_Master_OtherStudentsDMO> Fee_Master_OtherStudentsDMO { get; set; }
        public DbSet<Fee_Master_OthStudents_GHDMO> Fee_Master_OthStudents_GHDMO { get; set; }
        public DbSet<Fee_Master_OthStudents_GH_InstlDMO> Fee_Master_OthStudents_GH_InstlDMO { get; set; }
        public DbSet<Fee_Student_Status_OthStuDMO> Fee_Student_Status_OthStuDMO { get; set; }
        public DbSet<Fee_Y_Payment_OthStuDMO> Fee_Y_Payment_OthStuDMO { get; set; }
        public DbSet<Fee_Y_Payment_StaffDMO> Fee_Y_Payment_StaffDMO { get; set; }
        public DbSet<Fee_T_Payment_OthStaffDMO> Fee_T_Payment_OthStaffDMO { get; set; }
        public DbSet<Fee_Student_Status_StaffDMO> Fee_Student_Status_StaffDMO { get; set; }
        public DbSet<Masterclasscategory> Masterclasscategory { get; set; }
        public DbSet<IVRM_Table_AuditTrailDMO> IVRM_Table_AuditTrailDMO { get; set; }
        public DbSet<IVRM_AuditTrail_DeatilsDMO> IVRM_AuditTrail_DeatilsDMO { get; set; }
        public DbSet<Fee_M_Online_TransactionDMO> Fee_M_Online_TransactionDMO { get; set; }
        public DbSet<Fee_T_Online_TransactionDMO> Fee_T_Online_TransactionDMO { get; set; }
        //MB
        public DbSet<MasterRouteDMO> MasterRouteDMO { get; set; }
        public DbSet<MasterLocationDMO> MasterLocationDMO { get; set; }
        public DbSet<TR_Route_ScheduleDMO> TR_Route_ScheduleDMO { get; set; }
        public DbSet<TR_Student_RouteDMO> TR_Student_RouteDMO { get; set; }
        public DbSet<MasterAreaDMO> MasterAreaDMO { get; set; }
        public DbSet<castecategoryDMO> castecategoryDMO { get; set; }
        public DbSet<Route_Location> Route_Location { get; set; }
        //MB
        public DbSet<IVRM_Storage_path_Details> IVRM_Storage_path_Details { get; set; }
        public DbSet<AreaGroupMappingDMO> areaGroupMappingDMO { get; set; }
        public DbSet<StudentSiblingDMO> Adm_M_Sibling { get; set; }
        public DbSet<Fee_Master_Stream_Group_MappingDMO> Fee_Master_Stream_Group_MappingDMO { get; set; }
        public DbSet<Fee_Employee_ConcessionDMO> Fee_Employee_ConcessionDMO { get; set; }
        public DbSet<Fee_Employee_Concession_InstallmentsDMO> Fee_Employee_Concession_InstallmentsDMO { get; set; }
        public DbSet<Prospectus> Prospectus { get; set; }
        public DbSet<Payment_PA_Prospectus> Fee_Y_Payment_Preadmission_ProspectusDMO { get; set; }
        public DbSet<GeneralConfigDMO> GenConfig { get; set; }
        public DbSet<AdmSchoolMasterClassCatSec> AdmSchoolMasterClassCatSec { get; set; }
        public DbSet<Fee_Payment_Overall_Settlement_DetailsDMO> Fee_Payment_Overall_Settlement_DetailsDMO { get; set; }
        public DbSet<Fee_Payment_Settlement_DetailsDMO> Fee_Payment_Settlement_DetailsDMO { get; set; }
        public DbSet<Month> month { get; set; }
        public DbSet<StudentAppUserLoginDMO> StudentAppUserLoginDMO { get; set; }
        public DbSet<FeeMasterTermHeadsDMO> FeeMasterTermHeadsDMO { get; set; }
        public DbSet<Fee_Y_Payment_ThirdPartyDMO> Fee_Y_Payment_ThirdPartyDMO { get; set; }
        public DbSet<PAYUDETAILS> PAYUDETAILS { get; set; }
        public DbSet<MsterSessionDMO> MsterSessionDMO { get; set; }
        public DbSet<TR_Student_Route_FeeGroupDMO> TR_Student_Route_FeeGroupDMO { get; set; }
        public DbSet<Exm_Login_PrivilegeDMO> Exm_Login_Privilege { get; set; }
        public DbSet<Exm_Login_Privilege_SubjectsDMO> Exm_Login_Privilege_Subjects { get; set; }
        public DbSet<ClassTeacherMappingDMO> ClassTeacherMappingDMO { get; set; }
        public DbSet<HeadLedgerMappingDMO> HeadLedgerMappingDMO { get; set; }
        public DbSet<MasterConfiguration> MasterConfiguration { get; set; }
        public DbSet<IVRM_ModeOfPayment> IVRM_ModeOfPayment { get; set; }
        public DbSet<Fee_Y_Payment_PaymentModeSchool> Fee_Y_Payment_PaymentModeSchool { get; set; }
        public DbSet<Fee_Others_ConcessionDMO> Fee_Others_ConcessionDMO { get; set; }
        public DbSet<Fee_Others_Concession_InstallmentsDMO> Fee_Others_Concession_InstallmentsDMO { get; set; }
        public DbSet<PDA_StatusDMO> PDA_StatusDMO { get; set; }
        public DbSet<Adm_M_Employee_StudentDMO> Adm_M_Employee_StudentDMO { get; set; }
        public DbSet<TallyMTransactionDMO> TallyMTransactionDMO { get; set; }
        public DbSet<MOBILE_INSTITUTION> MOBILE_INSTITUTION { get; set; }
        public DbSet<FEE_RAZOR_TRANSFER_API_DETAILS> FEE_RAZOR_TRANSFER_API_DETAILS { get; set; }
        public DbSet<FEE_ECS_DETAILSDMO> FEE_ECS_DETAILSDMO { get; set; }
        public DbSet<School_Adm_Y_StudentDMO> SchoolYearWiseStudent { get; set; }
        public DbSet<Adm_M_Student> Adm_M_Student { get; set; }
        public DbSet<School_M_Section> AdmSection { get; set; }
        public DbSet<Institution> Institution { get; set; }
        //shilpa       
        public DbSet<Fee_Master_Concession_Group> Fee_Master_Concession_Group { get; set; }
        public DbSet<Adm_Master_Activities> Adm_Master_Activities { get; set; }
        public DbSet<Adm_Student_Activities> Adm_Student_Activities { get; set; }
        public DbSet<Fee_Master_Concession_DetailsDMO> Fee_Master_Concession_DetailsDMO { get; set; }
        public DbSet<Fee_Master_AutoConcession_GroupDMO> Fee_Master_AutoConcession_GroupDMO { get; set; }      
        public DbSet<HlMasterRoom_FeeGroupDMO> HlMasterRoom_FeeGroupDMO { get; set; }
        //public DbSet<FeeGroupClgDMO> FeeGroupClgDMO { get; set; }

        public DbSet<FAMaster_GroupDMO> FAMaster_GroupDMO { get; set; }
        public DbSet<FAUser_GroupDMO> FAUser_GroupDMO { get; set; }
        public DbSet<FACompanyMasterDMO> FACompanyMasterDMO { get; set; }
        public DbSet<FA_M_Ledger_DetailsDMO> FA_M_Ledger_DetailsDMO { get; set; }
        public DbSet<FA_M_LedgerDMO> FA_M_LedgerDMO { get; set; }
        public DbSet<FA_M_VoucherDMO> FA_M_VoucherDMO { get; set; }
        public DbSet<FA_T_VoucherDMO> FA_T_VoucherDMO { get; set; }
        public DbSet<FAUserCompanyMappingDMO> FAUserCompanyMappingDMO { get; set; }
        public DbSet<FACompanyFYMappingDMO> FACompanyFYMappingDMO { get; set; }
        public DbSet<ApplRole> ApplRole{get; set;}
        //FAUser_GroupDMO
        //added
        public DbSet<FeeMasterConfigurationDMO> FeeMasterConfiguration { get; set; }
        //
        public DbSet<Fee_Tally_Master_CompanyDMO> Fee_Tally_Master_CompanyDMO { get; set; }

        public DbSet<TR_Area_AmountDMO> TR_Area_AmountDMO { get; set; }

        public DbSet<MobileApplAuthenticationDMO> MobileApplAuthenticationDMO { get; set; }

        public DbSet<Adm_School_Master_Stream> Adm_School_Master_Stream { get; set; }
        public DbSet<FeeTermWiseRebateSettingDMO> FeeTermWiseRebateSettingDMO { get; set; }
        public DbSet<FeeYearlyRebateSettingDMO> FeeYearlyRebateSettingDMO { get; set; }
       
        public DbSet<Fee_T_Payment_OnlineDMO> Fee_T_Payment_OnlineDMO { get; set; }
        public DbSet<Fee_PaymentGateway_RateDMO> Fee_PaymentGateway_RateDMO { get; set; }
        public DbSet<FeePDCDMO> FeePDCDMO { get; set; }
        public DbSet<Fee_Master_BankDMO> Fee_Master_BankDMO { get; set; }

        public DbSet<FeeStudentRebate> FeeStudentRebateDMO { get; set; }
        public DbSet<MasterNarrationDMO> MasterNarrationDMO { get; set; }

        public DbSet<INV_Master_ItemDMO> INV_Master_ItemDMO { get; set; }
        public DbSet<INV_Master_SupplierDMO> INV_Master_SupplierDMO { get; set; }
        public  DbSet<FeeFineSlabDMO> FeeFineSlabDMO { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<FEeGroupLoginPreviledgeDMO>().ToTable("Fee_Group_Login_Previledge");

            base.OnModelCreating(builder);
            builder.Entity<FeeGroupDMO>().ToTable("Fee_Master_Group");
            base.OnModelCreating(builder);
            builder.Entity<FeeYearGroupDMO>().ToTable("Fee_Yearly_Group");
            base.OnModelCreating(builder);
            builder.Entity<FeeHeadDMO>().ToTable("Fee_Master_Head");
            base.OnModelCreating(builder);
            builder.Entity<FeeSpecialFeeGroupDMO>().ToTable("Fee_Master_SpecialFeeHead");
            base.OnModelCreating(builder);
            builder.Entity<FeeGroupMappingDMO>().ToTable("Fee_Master_Group_Grouping");
            base.OnModelCreating(builder);
            builder.Entity<FeeClassCategoryDMO>().ToTable("Fee_Master_Class_Category");
            base.OnModelCreating(builder);
            builder.Entity<School_M_Class>().ToTable("Adm_School_M_Class");
            base.OnModelCreating(builder);
            builder.Entity<FeeYearlyClassCategoryDMO>().ToTable("Fee_Yearly_Class_Category");
            base.OnModelCreating(builder);
            builder.Entity<FeeFineSlabDMO>().ToTable("Fee_Master_Fine_Slabs");
            base.OnModelCreating(builder);
            builder.Entity<FeeTermDMO>().ToTable("Fee_Master_Terms");
            base.OnModelCreating(builder);
            builder.Entity<FeeInstallmentDMO>().ToTable("Fee_Master_Installment");
            base.OnModelCreating(builder);
            builder.Entity<FeeInstallmentsyearlyDMO>().ToTable("Fee_T_Installment");
            base.OnModelCreating(builder);
            builder.Entity<FeeInstallmentDueDateDMO>().ToTable("Fee_T_Installment_DueDate");
            base.OnModelCreating(builder);
            builder.Entity<FeeMasterConfigurationDMO>().ToTable("Fee_Master_Configuration");
            base.OnModelCreating(builder);
            builder.Entity<FeeMasterTermHeadsDMO>().ToTable("Fee_Master_Terms_FeeHeads");
            base.OnModelCreating(builder);
            builder.Entity<MasterTermFeeHeadsDueDateDMO>().ToTable("Fee_Master_Terms_FeeHeads_DueDate");


            base.OnModelCreating(builder);
            builder.Entity<V_StudentPendingDMO>().ToTable("V_StudentPending");
            base.OnModelCreating(builder);
            builder.Entity<Fee_Groupwise_AutoReceiptDMO>().ToTable("Fee_Groupwise_AutoReceipt");
            base.OnModelCreating(builder);
            builder.Entity<Fee_Groupwise_AutoReceipt_GroupsDMO>().ToTable("Fee_Groupwise_AutoReceipt_Groups");
            base.OnModelCreating(builder);
            builder.Entity<FeeStudentAdjustment>().ToTable("Fee_Student_Adjustment");
            base.OnModelCreating(builder);
            builder.Entity<Fee_Student_Status_StaffDMO>().ToTable("Fee_Student_Status_Staff");
            base.OnModelCreating(builder);
            builder.Entity<Fee_Master_Amount_OthStaffs>().ToTable("Fee_Master_Amount_OthStaffs");
            base.OnModelCreating(builder);
            builder.Entity<FeeMasterOtherStudentDMO>().ToTable("Fee_Master_OtherStudents");
            base.OnModelCreating(builder);
            builder.Entity<Fee_Master_OthStudents_GHDMO>().ToTable("Fee_Master_OthStudents_GH");
            base.OnModelCreating(builder);
            builder.Entity<Fee_Master_OthStudents_GH_InstlDMO>().ToTable("Fee_Master_OthStudents_GH_Instl");
            base.OnModelCreating(builder);
            builder.Entity<Fee_Student_Status_OthStuDMO>().ToTable("Fee_Student_Status_OthStu");
            base.OnModelCreating(builder);
            builder.Entity<Fee_Y_Payment_OthStuDMO>().ToTable("Fee_Y_Payment_OthStu");
            base.OnModelCreating(builder);
            builder.Entity<Fee_Y_Payment_StaffDMO>().ToTable("Fee_Y_Payment_Staff");
            base.OnModelCreating(builder);
            builder.Entity<Fee_T_Payment_OthStaffDMO>().ToTable("Fee_T_Payment_OthStaff");
            base.OnModelCreating(builder);
            builder.Entity<Fee_T_Due_Date_OthStaffs>().ToTable("Fee_T_Due_Date_OthStaffs");

            base.OnModelCreating(builder);
            builder.Entity<IVRM_Table_AuditTrailDMO>().ToTable("IVRM_Table_AuditTrail");
            base.OnModelCreating(builder);
            builder.Entity<IVRM_AuditTrail_DeatilsDMO>().ToTable("IVRM_AuditTrail_Deatils");

            base.OnModelCreating(builder);
            builder.Entity<Fee_Y_Payment_Preadmission_ApplicationDMO>().ToTable("Fee_Y_Payment_PA_Application");
            base.OnModelCreating(builder);
            builder.Entity<AreaGroupMappingDMO>().ToTable("Fee_Group_Area_Mapping");
            base.OnModelCreating(builder);
            builder.Entity<Fee_Y_Payment_ThirdPartyDMO>().ToTable("Fee_Y_Payment_ThirdParty");

            base.OnModelCreating(builder);
            builder.Entity<Fee_Y_Payment_PaymentModeSchool>().ToTable("Fee_Y_Payment_PaymentMode");

            base.OnModelCreating(builder);
            builder.Entity<TR_Area_AmountDMO>().ToTable("TR_Area_Amount");

            builder.Entity<TR_Area_AmountDMO>().HasKey(m => m.TRMAAMT_Id);

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
