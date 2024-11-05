using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainModel.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vapstech.admission;
using DomainModel.Model.com.vaps.Fee;
using DomainModel.Model.com.vapstech.TT;
using DomainModel.Model.com.vaps.Exam;
using DomainModel.Model.com.vapstech.Birthday;
using DomainModel.Model.com.vapstech.Fee;
using DomainModel.Model.com.vapstech.HRMS;
using DomainModel.Model.com.vapstech.Portals.Chairman;
using DomainModel.Model.com.vapstech.LeaveManagement;
using DomainModel.Model.com.vapstech.Transport;
using DomainModel.Model.com.vapstech.Portals.HOD;
using DomainModel.Model.com.vapstech.FrontOffice;
using DomainModel.Model.com.vapstech.Alumni;
using DomainModel.Model.com.vapstech.Portals.Employee;
using DomainModel.Model.com.vapstech.Portals.Student;
using DomainModel.Model.com.vapstech.Inventory;
using DomainModel.Model.com.vapstech.MobileApp;
using DomainModel.Model.com.vapstech.College.Admission;
using DomainModel;
using DomainModel.Model.com.vapstech.College.Preadmission;
using DomainModel.Model.com.vapstech.College.Fees;
using DomainModel.Model.com.vapstech.OnlineExam;
using DomainModel.Model.com.vapstech.Portals.IVRM;
using DomainModel.Model.com.vapstech.VisitorsManagement;
using DomainModel.Model.com.vapstech.Library;
using DomainModel.Model.com.vapstech.Purchase.Inventory;
using DomainModel.Model.com.vapstech.OnlineProgram;
using DomainModel.Model.com.vapstech.Portals.IVRS;
using DomainModel.Model.com.vapstech.PDA;
using DomainModel.Model.com.vapstech.Hostel;
using DomainModel.Model.com.vapstech.Portals;
using DomainModel.Model.NAAC.LP_OnlineExam;
using DomainModel.Model.com.vapstech.Fee.FinancialAccounting;
using PreadmissionDTOs;
using DomainModel.Model.com.vapstech.BirthDay;
using static DomainModel.Model.com.vapstech.admission.QRCode_GenerationDMO;

namespace DataAccessMsSqlServerProvider
{
    public class DomainModelMsSqlServerContext : DbContext
    {
        public DomainModelMsSqlServerContext(DbContextOptions<DomainModelMsSqlServerContext> options) : base(options) { Database.SetCommandTimeout(30000000); }

        public IEnumerable<object> masterConfig;
        public DbSet<SMS_Email_Setting_RoleTypeDMO> SMS_Email_Setting_RoleTypeDMO { get; set; }
        public DbSet<MasterRouteDMO> MasterRouteDMO { get; set; }
        
        public DbSet<Master_VideoConferencing_InstituitionDMO> Master_VideoConferencing_InstituitionDMO { get; set; }
        public DbSet<LMS_Live_Meeting_PAStudentDMO> LMS_Live_Meeting_PAStudentDMO_con { get; set; }
        public DbSet<Fee_Y_Payment_AlumniDMO> Fee_Y_Payment_AlumniDMO_con { get; set; }
        public DbSet<DataEventRecord> DataEventRecords { get; set; }
        public DbSet<FeeAmountEntryDMO> Fee_Master_AmountDMO_con{ get; set; }


        //Ganesh added HRMS ---QR code
        public DbSet<HR_PROCESSDMO> HR_PROCESSDMO { get; set; }
        public DbSet<Master_Employee_Qulaification> Master_Employee_Qulaification { get; set; }
        public DbSet<HR_Master_CourseDMO> HR_Master_Course { get; set; }
        public DbSet<Master_Employee_Documents> Master_Employee_Documents { get; set; }
        public DbSet<TT_Final_GenerationDMO> TT_Final_GenerationDMO { get; set; }
        //public DbSet<subjectmasterDMO> subjectmasterDMO { get; set; }
        public DbSet<TT_Final_Generation_DetailedDMO> TT_Final_Generation_DetailedDMO { get; set; }
        public DbSet<Multiple_Mobile_DMO> Emp_MobileNo { get; set; }
        public DbSet<Multiple_Email_DMO> Emp_Email_Id { get; set; }
        public DbSet<MasterEmployee> MasterEmployee { get; set; }
        public DbSet<mastercasteDMO> Caste { get; set; }
        public DbSet<MasterReligionDMO> Religion { get; set; }
        public DbSet<HRGroupDeptDessgDMO> HRGroupDeptDessgDMO { get; set; }

        public DbSet<HR_Process_Auth_OrderNoDMO> HR_Process_Auth_OrderNoDMO { get; set; }
        public DbSet<FeeHeadDMO> Fee_Master_HeadDMO_con{ get; set; }
        public DbSet<Adm_Master_Student_CE> Adm_Master_Student_CE { get; set; }
        public DbSet<Preadmission_Dashboard_PageDMO> Preadmission_Dashboard_PageDMO { get; set; }
        public DbSet<ivrm_email_sentbox> ivrm_email_sentbox { get; set; }

        public DbSet<HR_Master_GroupType> HR_Master_GroupType { get; set; }

        public DbSet<IVRM_COLOUMN_REPORT> IVRM_COLOUMN { get; set; }
        public DbSet<IVRM_Auto_RollNo_Configuration> IVRM_Auto_RollNo_Configuration { get; set; }
        
        public DbSet<Adm_College_Yearly_StudentDMO> Adm_College_Yearly_StudentDMO { get; set; }
        public DbSet<PN_Sent_Details_DMO> PN_Sent_Details_DMO_con { get; set; }
        public DbSet<Alumni_Student_Qulaification_DMO> Alumni_Student_Qulaification_DMO_con { get; set; }
        public DbSet<Alumni_Student_Profession_DMO> Alumni_Student_Profession_DMO_con { get; set; }
        public DbSet<Alumni_Student_Achivement_DMO> Alumni_Student_Achivement_DMO_con { get; set; }
        public DbSet<PN_Sent_Details_Devicewise_DMO> PN_Sent_Details_Devicewise_DMO_con { get; set; }
        public DbSet<PN_Sent_Details_Staff_DMO> PN_Sent_Details_Staff_DMO_con { get; set; }
        public DbSet<PN_Sent_Details_Student_DMO> PN_Sent_Details_Student_DMO_con { get; set; }
        public DbSet<IVRM_ClassWorkDMO> IVRM_ClassWorkDMO_con { get; set; }
        public DbSet<GeneralConfigDMO> GenConfig { get; set; }
        public DbSet<StudentAppUserLoginDMO> StudentAppUserLoginDMO { get; set; }
        public DbSet<SourceInfo> SourceInfos { get; set; }
        public DbSet<Registration> Registration { get; set; }
        public DbSet<Master_VideoConferencingDMO> Master_VideoConferencingDMO { get; set; }
        public DbSet<MobileApplAuthenticationDMO> MobileApplAuthenticationDMO { get; set; }
        public DbSet<userDetails> userDetails { get; set; }
        public DbSet<Enquiry> Enquiry { get; set; }
        public DbSet<CasteCategory> castecategory { get; set; }
        public DbSet<Multiple_Email_DMO> Multiple_Email_DMO { get; set; }
        public DbSet<Multiple_Mobile_DMO> Multiple_Mobile_DMO { get; set; }
        public DbSet<School_M_Class> admissioncls { get; set; }
        public DbSet<MasterAcademic> AcademicYear { get; set; }
        public DbSet<Institution> Institute { get; set; }
        public DbSet<PDA_StatusDMO> PDA_StatusDMO { get; set; }
        public DbSet<Alumni_M_StudentDMO> Alumni_M_StudentDMO { get; set; }
        public DbSet<CLGAlumniUserRegistrationDMO> CLGAlumniUserRegistrationDMO { get; set; }
        public DbSet<CLGAlumni_User_LoginDMO> CLGAlumni_User_LoginDMO { get; set; }
        public DbSet<CLGADM_master_courseDMO> CLGADM_master_courseDMO { get; set; }
        public DbSet<StudentSibling> StudentSibling { get; set; }
        public DbSet<AdmissionStatus> AdmissionStatus { get; set; }
        public DbSet<Organisation> Organisation { get; set; }
        public DbSet<MasterConfiguration> mstConfig { get; set; }
        public DbSet<MasterTemplate> mstTemplate { get; set; }
        public DbSet<InstituteTemplate> InstitutionTemplate { get; set; }
        public DbSet<AdmissionStatus> status { get; set; }
        public DbSet<PA_Student_Sibblings_Details> PA_Student_Sibblings_Details { get; set; }
        public DbSet<PAStudentEmployee> PAStudentEmployee { get; set; }
        public DbSet<Adm_School_Master_Stream> Master_stream { get; set; }
        public DbSet<Adm_School_Master_CE> Adm_School_Master_CE { get; set; }
        public DbSet<Adm_School_Stream_Class_CE> Adm_School_Stream_Class_CE { get; set; }
        public DbSet<PA_School_Application_CE> PA_School_Application_CE { get; set; }
        public DbSet<Adm_School_Stream_Class> Master_stream_class { get; set; }
        public DbSet<TR_student_LocMappingDMO> TR_student_LocMappingDMO { get; set; }
        public DbSet<PA_Student_Transport_ApplicationDMO> PA_Student_Transport_ApplicationDMO { get; set; }
        public DbSet<TR_Student_RouteDMO> TR_Student_RouteDMO { get; set; }
        public DbSet<TR_Student_Route_FeeGroupDMO> TR_Student_Route_FeeGroupDMO { get; set; }
        public DbSet<StudentSiblingDMO> StudentSiblingDMO { get; set; }
        public DbSet<PA_Student_Sibblings> PA_Student_Sibblings { get; set; }
        public DbSet<Adm_M_Employee_StudentDMO> Adm_M_Employee_StudentDMO { get; set; }
        public DbSet<IVRM_Master_FinancialYear> IVRM_Master_FinancialYear { get; set; }
        public DbSet<PA_College_Application> PA_College_Application { get; set; }
        public DbSet<TR_Location_FeeGroup_MappingDMO> TR_Location_FeeGroup_MappingDMO { get; set; }
        public DbSet<PA_Master_Vaccines> PA_Master_Vaccines { get; set; }
        public DbSet<Adm_Master_College_StudentDMO> Adm_Master_College_StudentDMO { get; set; }
        public DbSet<CLGAlumni_M_StudentDMO> CLGAlumni_M_StudentDMO { get; set; }
        public DbSet<CollegeStudentlogin> CollegeStudentlogin { get; set; }
        public DbSet<MOBILE_INSTITUTION> MOBILE_INSTITUTION { get; set; }
        public DbSet<API_MOBILE> API_MOBILE { get; set; }
        public DbSet<FEE_RAZOR_TRANSFER_API_DETAILS> FEE_RAZOR_TRANSFER_API_DETAILS { get; set; }
        public DbSet<UserLoginPrivileges> UserLoginPrivileges { get; set; }
        public DbSet<IVRM_IVRS_ConfigurationDMO> IVRS_ConfigurationDMO { get; set; }
        public DbSet<IVRM_Configuration_URLDMO> IVRM_Configuration_URLDMO { get; set; }
        public DbSet<IVRM_User_MobileApp_Login_Privileges> IVRM_User_MobileApp_Login_Privileges { get; set; }
        public DbSet<PreadmissionSchoolRegistrationAdmissionNoticeDMO> PreadmissionSchoolRegistrationAdmissionNoticeDMO { get; set; }
        public DbSet<PreadmissionSchoolRegistrationAdmNoticeStudentsDMO> PreadmissionSchoolRegistrationAdmNoticeStudentsDMO { get; set; }
        public DbSet<IVRM_MobileApp_Page> IVRM_MobileApp_Page { get; set; }
        public DbSet<IVRM_Role_MobileApp_Privileges> IVRM_Role_MobileApp_Privileges { get; set; }
        public DbSet<IVRM_Mandatory_Setting> IVRM_Mandatory_Setting { get; set; }
        public DbSet<IVRM_Mandatory_Setting_IW> IVRM_Mandatory_Setting_IW { get; set; }
        public DbSet<StudentStatusHistory> studentstatushistory { get; set; }
        public DbSet<TransactionNumbering> TransactionNumbering { get; set; }
        public DbSet<Transaction_Numbering_Type> Transaction_Numbering_Type { get; set; }
        public DbSet<AlumniUserRegistrationDMO> AlumniUserRegistrationDMO { get; set; }
        public DbSet<Alumni_User_LoginDMO> Alumni_User_LoginDMO { get; set; }
        public DbSet<MasterRolePreviledgeDMO> Role_Privileges { get; set; }
        public DbSet<HR_Master_LeaveYearDMO> HR_Master_LeaveYearDMO { get; set; }
        public DbSet<Institution> Institution { get; set; }
        public DbSet<Institution_Module_Page> Institution_Module_Page { get; set; }
        public DbSet<MasterModule> masterModule { get; set; }
        public DbSet<MasterRoleType> MasterRoleType { get; set; }
        public DbSet<Institution_Module> Institution_Module { get; set; }
        public DbSet<MasterPageModuleMapping> masterPageModuleMapping { get; set; }
        public DbSet<MasterPage> masterPage { get; set; }
        public DbSet<MasterTemplate> MasterTemplate { get; set; }
        public DbSet<MasterCategory> mastercategory { get; set; }

        //public DbSet<ClgMasterCategoryDMO> clgmastercategory { get; set; }
        public DbSet<MasterSchoolType> masterSchoolType { get; set; }
        public DbSet<MasterBorad> masterBorad { get; set; }
        public DbSet<ApplicationUserRole> appUserRole { get; set; }
        public DbSet<ApplRole> applicationRole { get; set; }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<OrganisationEmail> OrganisationEmail { get; set; }
        public DbSet<OrganisationPhone> OrganisationPhone { get; set; }
        public DbSet<Institution_Phone_No> Institution_Phone_No { get; set; }
        public DbSet<Institution_MobileNo> Institution_MobileNo { get; set; }
        public DbSet<Institution_EmailId> Institution_EmailId { get; set; }
        public DbSet<Country> country { get; set; }
        public DbSet<City> city { get; set; }
        public DbSet<State> State { get; set; }
        public DbSet<Preadmission_SeatBlocked_Student> Preadmission_SeatBlocked_Student { get; set; }
        public DbSet<School_M_Class> School_M_Class { get; set; }
        public DbSet<Staff_User_Login> Staff_User_Login { get; set; }
        public DbSet<StudentApplication> StudentApplication { get; set; }
        public DbSet<IVRM_Email_sentBoxDMO> IVRM_Email_sentBoxDMO { get; set; }
        public DbSet<Adm_Student_Transport_ApplicationDMO> Adm_Student_Transport_ApplicationDMO { get; set; }
        public DbSet<HlMasterRoom_FeeGroupDMO> HL_Master_Room_FeeGroup_DMO { get; set; }
        public DbSet<MasterPage> masterpage { get; set; }
        public DbSet<VirtaulSchool> VirtualSchool { get; set; }
        public DbSet<MandatoryFields> mandatory { get; set; }
        public DbSet<ModulePage_Category> modulepagecoategory { get; set; }
        public DbSet<Master_Numbering> Master_Numbering { get; set; }
        public DbSet<MasterMenu> Mastermenu { get; set; }
        //public DbSet<MasterMenuPageMapping> Mastermenupagemapping { get; set; }
        public DbSet<MasterMenuInstitutionWise> Mastermenuinstitutewise { get; set; }
        public DbSet<MasterMenuPageMappingInstituteWise> Mastermenupagemappinginstitutewise { get; set; }
        public DbSet<InstitutionRolePrivileges> InstitutionRolePrivileges { get; set; }
        
         public DbSet<LMS_Live_Meeting_PAStudent_CollegeDMO> LMS_Live_Meeting_PAStudent_CollegeDMO { get; set; }
        public DbSet<IVRM_Custom_UserName_PasswordDMO> IVRM_Custom_UserName_PasswordDMO { get; set; }


        // public DbSet<IVRM_Master_SubjectsDMO> Subject { get; set; }
        public DbSet<School_M_Section> Section { get; set; }
        public DbSet<Adm_SchoolAttendanceLoginUser> Adm_SchAttLoginUser { get; set; }
        public DbSet<Adm_SchoolAttendanceLoginUserClass> Adm_SchAttLoginUserClass { get; set; }
        public DbSet<Adm_SchoolAttendanceLoginUserClassSubject> Adm_schAttLoginUserClassSubjects { get; set; }
        public DbSet<SMSEmailSetting> smsEmailSetting { get; set; }
        public DbSet<IVRM_SMS_Email_Setting_ParameterDMO> IVRM_SMS_Email_Setting_Parameter { get; set; }

        public DbSet<SmsEmailHeader> smsEmailHeader { get; set; }
        public DbSet<IVRM_Master_HTMLTemplatesDMO> IVRM_Master_HTMLTemplatesDMO { get; set; }
        public DbSet<Month> month { get; set; }
        public DbSet<MasterClassHeld> masterclassHeld { get; set; }
        public DbSet<School_M_Section> School_M_Section { get; set; }
        public DbSet<School_Adm_Y_StudentDMO> SchoolYearWiseStudent { get; set; }
        public DbSet<AdmSchoolAttendanceSubjectBatch> AdmSchoolAttendanceSubjectBatch { get; set; }
        public DbSet<AdmSchoolAttendanceSubjectBatchStudents> AdmSchoolAttendanceSubjectBatchStudents { get; set; }
        public DbSet<MasterSubjectDMO> MasterSubject { get; set; }
        public DbSet<Adm_M_Student> Adm_M_Student { get; set; }
        public DbSet<School_Adm_Y_StudentDMO> School_Adm_Y_StudentDMO { get; set; }
        public DbSet<Adm_SchoolAttendanceLoginUser> attloginuser { get; set; }
        public DbSet<Adm_studentAttendance> Adm_studentAttendance { get; set; }
        public DbSet<Adm_studentAttendanceStudents> Adm_studentAttendanceStudents { get; set; }
        public DbSet<Adm_studentAttendanceSubjects> Adm_studentAttendanceSubjects { get; set; }
        public DbSet<Adm_StudentAttendancePeriodwiseDMO> Adm_StudentAttendancePeriodwiseDMO { get; set; }
        public DbSet<AdditionalField> additional_field_context { get; set; }
        public DbSet<SMS_MAIL_SAVED_PARAMETER_DMO> SMS_MAIL_SAVED_PARAMETER_DMO { get; set; }
        public DbSet<SMS_DETAILS_DMO> SMS_DETAILS_DMO { get; set; }

        public DbSet<DistrictDMO> DistrictDMO { get; set; }
        public DbSet<SMS_MAIL_PARAMETER_DMO> SMS_MAIL_PARAMETER_DMO { get; set; }
        public DbSet<EMAIL_DETAILS_DMO> EMAIL_DETAILS_DMO { get; set; }
        public DbSet<UserRoleWithInstituteDMO> UserRoleWithInstituteDMO { get; set; }
        public DbSet<Preadmission_Special_Registration> Preadmission_Special_Registration { get; set; }
        public DbSet<Master_Institution_SubscriptionValidity> Master_Institution_SubscriptionValidity { get; set; }
        public DbSet<Masterclasscategory> Masterclasscategory { get; set; }
        public DbSet<MasterReligionDMO> masterReligion { get; set; }
        public DbSet<IVRM_Master_Menu_Page_MappingDMO> IVRM_Master_Menu_Page_MappingDMO { get; set; }
        public DbSet<MasterMenuPageMappingInstituteWise> IVRM_Master_Menu_Page_Mapping_InstitutionwiseDMO { get; set; }
        public DbSet<MasterMenuInstitutionWise> MasterMenuInstitutionWise { get; set; }
        public DbSet<HR_Master_Department> HR_Master_Department { get; set; }
        public DbSet<HR_Master_Designation> HR_Master_Designation { get; set; }
        public DbSet<StudentAchivementDMO> Achivement { get; set; }
        public DbSet<StudentTC> Student_TC { get; set; }
        public DbSet<AttendanceEntryTypeDMO> AttendanceEntryTypeDMO { get; set; }
        public DbSet<ProspectusFilePath> ProspectusFilePath { get; set; }
        public DbSet<Prospectus> prospectus { get; set; }
        public DbSet<FeeMasterConfigurationDMO> FeeMasterConfigurationDMO { get; set; }
        public DbSet<readmitstudentDMO> readmitstudentDMO { get; set; }
        public DbSet<Adm_Master_Batch> Adm_Master_Batch { get; set; }

        //public DbSet<MasterEmployee> masterStaff { get; set; }
        public DbSet<Receipt_Numbering> Receipt_Numbering { get; set; }
        public DbSet<Voucher_Numbering> Voucher_Numbering { get; set; }
        public DbSet<SmsEmailParameterMappingDMO> SmsEmailParameterMappingDMO { get; set; }
        public DbSet<IVRM_School_Master_SubjectsDMO> MasterSubjectList { get; set; }
        public DbSet<AdmissionStandardDMO> AdmissionStandardDMO { get; set; }
        public DbSet<FeeStudentTransactionDMO> feestudentstatus { get; set; }
        public DbSet<StaffLoginDMO> StaffLoginDMO { get; set; }
        public DbSet<MasterEmployee> HR_Master_Employee_DMO { get; set; }
        public DbSet<FeeStudentGroupMappingDMO> studentgroupmapping { get; set; }
        public DbSet<FeeStudentGroupInstallmentMappingDMO> studentgroupinstallmapping { get; set; }
        public DbSet<FeePaymentDetailsDMO> FeePaymentDetails { get; set; }
        public DbSet<Fee_Y_Payment_School_StudentDMO> Fee_Y_Payment_School_StudentDMO { get; set; }
        public DbSet<Fee_Y_PaymentDMO> FeePaymentDetailsCollege { get; set; }
        public DbSet<IVRM_sms_sentBoxDMO> IVRM_sms_sentBoxDMO { get; set; }
        public DbSet<Adm_M_Student_FatherMobileNo> Adm_M_Student_FatherMobileNo { get; set; }
        public DbSet<Adm_Master_Father_Email> Adm_Master_Father_Email { get; set; }
        public DbSet<Adm_M_Mother_MobileNo> Adm_M_Mother_MobileNo { get; set; }
        public DbSet<Adm_M_Mother_Emailid> Adm_M_Mother_Emailid { get; set; }
        public DbSet<Adm_M_Student_MobileNo> Adm_M_Student_MobileNo { get; set; }
        public DbSet<Adm_M_Student_Email_Id> Adm_M_Student_Email_Id { get; set; }
        public DbSet<Adm_Student_EcsDetailsDMO> Adm_Student_EcsDetailsDMO { get; set; }
        public DbSet<StudentUserLoginDMO> StudentUserLoginDMO { get; set; }
        public DbSet<StudentUserLogin_Institutionwise> StudentUserLogin_Institutionwise { get; set; }        
        public DbSet<AdmSchoolMasterClassCatSec> AdmSchoolMasterClassCatSec { get; set; }
        public DbSet<FeeMasterRefundDMO> FeeMasterRefundDMO { get; set; }
        public DbSet<PreadmisionLoginHistory> PreadmisionLoginHistory { get; set; }
        public DbSet<IVRM_Storage_path_Details> IVRM_Storage_path_Details { get; set; }
        public DbSet<TT_Master_PeriodDMO> TT_Master_PeriodDMO { get; set; }       
        public DbSet<Attendance_Students_SmartCard> Attendance_Students_SmartCard { get; set; }
        public DbSet<Attendance_Lunch_Students_SmartCardDMO> Attendance_Lunch_Students_SmartCardDMO { get; set; }
        public DbSet<ClassTeacherMappingDMO> ClassTeacherMappingDMO { get; set; }
        public DbSet<Fee_M_Online_TransactionDMO> Fee_M_Online_TransactionDMO { get; set; }
        public DbSet<Dashboard_page_mapping> Dashboard_page_mapping { get; set; }
        public DbSet<StudycertificateReportDMO> StudycertificateReportDMO { get; set; }
        public DbSet<ivrm_email_sentbox> IVRM_Email_sentBox { get; set; }
        public DbSet<IVRM_Master_Gender> IVRM_Master_Gender { get; set; }

        // public DbSet<IVRM_Master_SubjectsDMO> IVRM_Master_Subjects { get; set; }
        public DbSet<Payment_PA_Prospectus> Payment_PA_ProspectusDMO { get; set; }
        public DbSet<HR_Emp_Leave_ApplicationDMO> HR_Emp_Leave_ApplicationDMO { get; set; }
        public DbSet<HR_Emp_OB_Leave_DMO> HR_Emp_OB_Leave_DMO { get; set; }
        public DbSet<PA_School_Application_ProspectusDMO> PA_School_Application_ProspectusDMO { get; set; }
        //Bus Hire related DMO's.
        public DbSet<TripOnlineBookingDMO> TripOnlineBookingDMO { get; set; }
        public DbSet<MasterHirerGroupDMO> MasterHirerGroupDMO { get; set; }
        public DbSet<TripDMO> TripDMO { get; set; }
        public DbSet<IVRM_ModeOfPaymentDMO> IVRM_ModeOfPaymentDMO { get; set; }
        public DbSet<TR_ServiceDMO> TR_ServiceDMO { get; set; }

        //Trip Payment.
        public DbSet<TR_Trip_PaymentDMO> TR_Trip_PaymentDMO { get; set; }
        public DbSet<TR_Trip_Payment_TripsDMO> TR_Trip_Payment_TripsDMO { get; set; }
        public DbSet<TR_Hirer_OB_DMO> TR_Hirer_OB_DMO { get; set; }
        public DbSet<MasterHirerDMO> MasterHirerDMO { get; set; }
        public DbSet<Adm_Master_Student_PA> Adm_Master_Student_PA { get; set; }
        public DbSet<MasterAreaDMO> MasterAreaDMO { get; set; }
        public DbSet<SMS_Sent_Details> SMS_Sent_Details { get; set; }
        public DbSet<IVRM_EMAIL_ATT_DMO> IVRM_EMAIL_ATT_DMO { get; set; }
        public DbSet<IVRM_HOD_Class_DMO> IVRM_HOD_Class_DMO { get; set; }
        public DbSet<HOD_DMO> HOD_DMO { get; set; }
        public DbSet<SMS_Sent_Details_LoginUserDMO> SMS_Sent_Details_LoginUserDMO { get; set; }
        public DbSet<SMS_Sent_Details_NowiseDMO> SMS_Sent_Details_NowiseDMO { get; set; }

        //========Message Interaction 
        public DbSet<IVRM_Interactions_StudentDMO> IVRM_Interactions_StudentDMO { get; set; }
        public DbSet<IVRM_Interactions_Student_StaffDMO> IVRM_Interactions_Student_StaffDMO { get; set; }
        public DbSet<IVRM_School_Master_InteractionsDMO> IVRM_School_Master_InteractionsDMO { get; set; }
        public DbSet<IVRM_School_Transaction_InteractionsDMO> IVRM_School_Transaction_InteractionsDMO { get; set; }

        //===============Push IVRM_PushNotification
        public DbSet<IVRM_PushNotificationDMO> IVRM_PushNotificationDMO { get; set; }

        //===============INVENTORY       
        public DbSet<INV_M_PurchaseRequisitionDMO> INV_M_PurchaseRequisitionDMO { get; set; }
        public DbSet<INV_M_PurchaseIndentDMO> INV_M_PurchaseIndentDMO { get; set; }
        public DbSet<INV_M_SupplierQuotationDMO> INV_M_SupplierQuotationDMO { get; set; }
        public DbSet<INV_M_PurchaseOrderDMO> INV_M_PurchaseOrderDMO { get; set; }
        public DbSet<INV_M_GRNDMO> INV_M_GRNDMO { get; set; }
        public DbSet<INV_M_SalesDMO> INV_M_SalesDMO { get; set; }
        public DbSet<Attendance_Students_SmartCard_Timings> Attendance_Students_SmartCard_Timings { get; set; }
        public DbSet<HR_Master_BankDeatils> HR_Master_BankDeatils { get; set; }

        //Visitor Management
        public DbSet<FO_Inward_DMO> FO_Inward_DMO { get; set; }
        public DbSet<FO_Outward_DMO> FO_Outward_DMO { get; set; }
        public DbSet<Gate_Pass_Student_DMO> Gate_Pass_Student_DMO { get; set; }
        public DbSet<Gate_Pass_Staff_DMO> Gate_Pass_Staff_DMO { get; set; }
        public DbSet<LIB_Master_Subscription_DMO> LIB_Master_Subscription_DMO { get; set; }

        //adde praveen
        public DbSet<SMSMasterApprovalDMO> SMSMasterApprovalDMO { get; set; }
        public DbSet<SMSApprovalStatusDMO> SMSApprovalStatusDMO { get; set; }
        //online exam
        public DbSet<LMS_Master_OE_QNS_OptionsDMO> LMS_Master_OE_QNS_OptionsDMO { get; set; }
        public DbSet<LMS_Master_OE_Questions_FilesDMO> LMS_Master_OE_Questions_FilesDMO { get; set; }
        public DbSet<LMS_Master_OE_Questions_ClassDMO> LMS_Master_OE_Questions_ClassDMO { get; set; }
        public DbSet<LMS_Master_OE_QuestionsDMO> LMS_Master_OE_QuestionsDMO { get; set; }
        public DbSet<CLG_Adm_College_AY_CourseDMO> CLG_Adm_College_AY_CourseDMO { get; set; }
        public DbSet<ClgMasterBranchDMO> ClgMasterBranchDMO { get; set; }
        public DbSet<LMS_Master_OE_Questions_BranchDMO> LMS_Master_OE_Questions_BranchDMO { get; set; }
        public DbSet<LMS_Students_Exam_Answer_CollegeDMO> LMS_Students_Exam_Answer_CollegeDMO { get; set; }
        public DbSet<LMS_Students_Exam_CollegeDMO> LMS_Students_Exam_CollegeDMO { get; set; }
        public DbSet<LMS_Master_OE_SettingDMO> LMS_Master_OE_SettingDMO { get; set; }
        public DbSet<LMS_Students_Exam_AnswerDMO> LMS_Students_Exam_AnswerDMO { get; set; }
        public DbSet<LMS_Students_ExamDMO> LMS_Students_ExamDMO { get; set; }
        public DbSet<CLG_Adm_College_AY_Course_BranchDMO> CLG_Adm_College_AY_Course_BranchDMO { get; set; }
        public DbSet<CLG_Adm_Master_SemesterDMO> CLG_Adm_Master_SemesterDMO { get; set; }
        public DbSet<Adm_College_Master_SectionDMO> Adm_College_Master_SectionDMO { get; set; }
        public DbSet<CLG_Adm_College_AY_Course_Branch_SemesterDMO> CLG_Adm_College_AY_Course_Branch_SemesterDMO { get; set; }
        public DbSet<MasterCourseDMO> MasterCourseDMO { get; set; }
        public DbSet<SMSMail_HeaderDMO> SMSMail_HeaderDMO { get; set; }

        //online program
        public DbSet<ProgramsYearlyDMO> ProgramsYearlyDMO { get; set; }
        public DbSet<ProgramsYearlyActivitiesDMO> ProgramsYearlyActivitiesDMO { get; set; }
        public DbSet<ProgramsYearlyGuestDMO> ProgramsYearlyGuestDMO { get; set; }
        public DbSet<ProgramsYearlyPhotosDMO> ProgramsYearlyPhotosDMO { get; set; }
        public DbSet<ProgramsYearlyVideosDMO> ProgramsYearlyVideosDMO { get; set; }
        public DbSet<Fee_Master_ConcessionDMO> Fee_Master_ConcessionDMO { get; set; }
        public DbSet<PAYUDETAILS> PAYUDETAILS { get; set; }
        public DbSet<ProgramsMasterLevelDMO> ProgramsMasterLevelDMO { get; set; }
        public DbSet<LMS_Live_Meeting_Student_CollegeDMO> LMS_Live_Meeting_Student_CollegeDMO { get; set; }
        public DbSet<ProgramsMasterTypeDMO> ProgramsMasterTypeDMO { get; set; }
        public DbSet<LMS_Live_Meeting_CourseBranchDMO> LMS_Live_Meeting_CourseBranchDMO { get; set; }
        public DbSet<LMS_Live_MeetingDMO> LMS_Live_MeetingDMO { get; set; }
        public DbSet<LMS_Live_Meeting_StudentDMO> LMS_Live_Meeting_StudentDMO { get; set; }
        public DbSet<LMS_Live_Meeting_PAStudentDMO> LMS_Live_Meeting_PAStudentDMO { get; set; }
        public DbSet<LMS_Live_Meeting_ClassDMO> LMS_Live_Meeting_ClassDMO { get; set; }
        public DbSet<LMS_Live_Meeting_StaffOthersDMO> LMS_Live_Meeting_StaffOthersDMO { get; set; }
        public DbSet<Fee_PaymentGateway_DetailsDMO> Fee_PaymentGateway_Details { get; set; }
        public DbSet<ProgramsYearlyFileDMO> ProgramsYearlyFileDMO { get; set; }
        public DbSet<StudentDetailsupdateDMO> StudentDetailsupdateDMO { get; set; }
        public DbSet<HR_Master_Room_DMO> HR_Master_Room_DMO { get; set; }
        public DbSet<BankDetailsDMO> BankDetailsDMO { get; set; }
        public DbSet<Adm_Certificates_Apply_DMO> Adm_Certificates_Apply_DMO { get; set; }
        public DbSet<Adm_Students_Certificate_Apply_DMO> Adm_Students_Certificate_Apply_DMO { get; set; }
        public DbSet<Adm_Students_Certificate_Approve_DMO> Adm_Students_Certificate_Approve_DMO { get; set; }

        // PA ONLIN EXAM
        public DbSet<PA_Master_OE_SettingDMO> PA_Master_OE_SettingDMO { get; set; }
        public DbSet<PA_Master_OE_QuestionsDMO> PA_Master_OE_QuestionsDMO { get; set; }
        public DbSet<PA_Master_OE_ANS_OptionsDMO> PA_Master_OE_ANS_OptionsDMO { get; set; }
        public DbSet<PA_Master_OE_QNS_OptionsDMO> PA_Master_OE_QNS_OptionsDMO { get; set; }
        public DbSet<PA_Master_OE_Questions_ClassDMO> PA_Master_OE_Questions_ClassDMO { get; set; }
        public DbSet<PA_Master_OE_Questions_FilesDMO> PA_Master_OE_Questions_FilesDMO { get; set; }
        public DbSet<PA_Students_Exam_AnswerDMO> PA_Students_Exam_AnswerDMO { get; set; }
        public DbSet<PA_Students_ExamDMO> PA_Students_ExamDMO { get; set; }
        public DbSet<IVRM_Payment_User_MappingDMO> IVRM_Payment_User_MappingDMO { get; set; }
        public DbSet<Exm_Category_ClassDMO> Exm_Category_ClassDMO { get; set; }
        public DbSet<StudentMappingDMO> StudentMappingDMO { get; set; }
        public DbSet<IVRM_Payment_Subscription_Login_RemarksDMO> IVRM_Payment_Subscription_Login_RemarksDMO { get; set; }
        public DbSet<ExmStudentMarksProcessDMO> ExmStudentMarksProcessDMO { get; set; }
        public DbSet<FeeClassCategoryDMO> FeeClassCategoryDMO { get; set; }
        public DbSet<FeeYearlyClassCategoryDMO> FeeYearlyClassCategoryDMO { get; set; }
        public DbSet<MasterYearlyClassCategoryClassDMO> FeeYearlyClassCategoryClassDMO { get; set; }
        public DbSet<Adm_School_Student_COCDMO> Adm_School_Student_COCDMO { get; set; }
        public DbSet<SMSEmailSettingUserMapping> SMSEmailSettingUserMapping { get; set; }
        public DbSet<LP_Students_ExamDMO> LP_Students_ExamDMO { get; set; }
        public DbSet<ClgMasterCategoryDMO> ClgMasterCategoryDMO { get; set; }
        public DbSet<ClgMasterCourseCategoryMapDMO> ClgMasterCourseCategoryMapDMO { get; set; }
        public DbSet<Adm_Master_College_Student_PA_DMO> Adm_Master_College_Student_PA_DMO { get; set; }
        public DbSet<Fee_Y_Payment_PA_Application> Fee_Y_Payment_PA_Application { get; set; }
        public DbSet<IVRM_MobileApp_LoginDetailsDMO> IVRM_MobileApp_LoginDetailsDMO { get; set; }
        public DbSet<QR_Code_GenerateDMO> QR_Code_GenerateDMO { get; set; }
        public DbSet<Staff_QRCode_Generation_DetailsDMO> Staff_QRCode_Generation_DetailsDMO { get; set; }
        public DbSet<PDA_ExpenditureDMO> PDA_ExpenditureDMO { get; set; }
        public  DbSet<TalukDMO> TalukDMO { get; set; }
        public  DbSet<ExamMarksDMO> ExamMarksDMO { get; set; }
        public  DbSet<FA_M_VoucherDMO> FA_M_VoucherDMO { get; set; }
        public DbSet<Master_ExamQualified_ClassDMO> Master_ExamQualified_ClassDMO { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ProspectusFilePath>().HasKey(m => m.IPPC_Id);
            builder.Entity<DataEventRecord>().HasKey(m => m.DataEventRecordId);
            builder.Entity<SourceInfo>().HasKey(m => m.SourceInfoId);
            //builder.Entity<logindata>().HasKey(m => m.userid);
            builder.Entity<Registration>().HasKey(m => m.AOAD_ID);
            builder.Entity<userDetails>().HasKey(m => m.AMPOT_Id);
            builder.Entity<Enquiry>().HasKey(m => m.PASE_Id);
            builder.Entity<Organisation>().HasKey(m => m.MO_Id);
            builder.Entity<MasterTemplate>().HasKey(m => m.IVRMT_Id);
            builder.Entity<MasterCategory>().HasKey(m => m.AMC_Id);
            builder.Entity<MasterBorad>().HasKey(m => m.IVRMMB_Id);
            builder.Entity<MasterSchoolType>().HasKey(m => m.IVRMMTYP_Id);
            builder.Entity<SMSEmailSetting>().HasKey(m => m.ISES_Id);
            builder.Entity<MasterClassHeld>().HasKey(m => m.ASCH_Id);
            builder.Entity<AdditionalField>().HasKey(m => m.IPAF_Id);
            builder.Entity<Masterclasscategory>().HasKey(m => m.ASMCC_Id);

            builder.Entity<MasterReligionDMO>().HasKey(m => m.IVRMMR_Id);
            builder.Entity<StudentTC>().HasKey(m => m.ASTC_Id);

            builder.Entity<IVRM_sms_sentBoxDMO>().HasKey(m => m.IVRM_SSB_ID);
            builder.Entity<AdmSchoolMasterClassCatSec>().HasKey(m => m.ASMCCS_Id);
            builder.Entity<StudentUserLoginDMO>().HasKey(m => m.IVRMSTUUL_Id);
            builder.Entity<StudentUserLogin_Institutionwise>().HasKey(m => m.IVRMSTUULI_Id);
            builder.Entity<StudycertificateReportDMO>().HasKey(m => m.ASC_Id);
            // shadow properties
            builder.Entity<DataEventRecord>().Property<DateTime>("UpdatedTimestamp");
            builder.Entity<SourceInfo>().Property<DateTime>("UpdatedTimestamp");

            // builder.Entity<users>().Property<DateTime>("UpdatedTimestamp");

            builder.Entity<School_M_Class>().ToTable("Adm_School_M_Class");

            builder.Entity<School_Adm_Y_StudentDMO>().ToTable("Adm_School_Y_Student");
            builder.Entity<AdmSchoolAttendanceSubjectBatch>().ToTable("Adm_School_Attendance_Subject_Batch");
            builder.Entity<AdmSchoolAttendanceSubjectBatchStudents>().ToTable("Adm_School_Attendance_Subject_Batch_Students");
            builder.Entity<PreadmisionLoginHistory>().ToTable("Preadmission_Student_Login_History");
            builder.Entity<Payment_PA_Prospectus>().HasKey(m => m.FYPPP_Id);
            builder.Entity<HR_Emp_OB_Leave_DMO>().HasKey(m => m.HREOBL_Id);

            //Bus hire.
            builder.Entity<TripOnlineBookingDMO>().HasKey(d => d.TRTOB_Id);
            builder.Entity<TripDMO>().HasKey(d => d.TRTP_Id);
            builder.Entity<IVRM_ModeOfPaymentDMO>().HasKey(d => d.IVRMMOD_Id);
            builder.Entity<TR_Trip_PaymentDMO>().HasKey(d => d.TRTPP_Id);
            builder.Entity<TR_Trip_Payment_TripsDMO>().HasKey(d => d.TRTPPT_Id);
            builder.Entity<TR_Hirer_OB_DMO>().HasKey(d => d.TRHOB_Id);
            builder.Entity<MasterHirerDMO>().HasKey(d => d.TRMH_Id);

            base.OnModelCreating(builder);
        }

        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();

            updateUpdatedProperty<SourceInfo>();
            updateUpdatedProperty<DataEventRecord>();
            updateUpdatedProperty<DataEventRecord>();            
            updateUpdatedProperty<Institution>();
            updateUpdatedProperty<Institution_Phone_No>();
            updateUpdatedProperty<Institution_MobileNo>();
            updateUpdatedProperty<Institution_EmailId>();
            // updateUpdatedProperty<users>();
            updateUpdatedProperty<Preadmission_SeatBlocked_Student>();
            updateUpdatedProperty<School_M_Class>();
            updateUpdatedProperty<SMSEmailSetting>();
            updateUpdatedProperty<MasterClassHeld>();
            updateUpdatedProperty<StudycertificateReportDMO>();
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
