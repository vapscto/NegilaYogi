using Newtonsoft.Json.Linq;
using PreadmissionDTOs.com.vaps.LeaveManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Portals.Employee
{
    public class LiveMeetingScheduleDTO
    {
        public object amstid;
        public long RoleId { get; set; }
        public long interval  { get; set; }
        public long LMSLMEET_Id { get; set; }
        public long EMER_Id { get; set; }
        public string LMSLMEET_MeetingId { get; set; }
        public string AMST_Photoname { get; set; }
        public Array Staffmobileappprivileges { get; set; }
        public long roleid { get; set; }
        public long PaymentNootificationStaff { get; set; }
        public string moderatorurl { get; set; }
        public string vcflag { get; set; }
        public bool staffstudentflag { get; set; }
        public bool adminstaffflag { get; set; }
        public bool joined { get; set; }
        public bool createduser { get; set; }

        public bool meetingstatus { get; set; }
        public bool empexammapping { get; set; }
        public string mobileprivileges { get; set; }
        public string ASMC_SectionCode { get; set; }
        public string returnvalue  { get; set; }
        public string remarks  { get; set; }
        public string grade   { get; set; }
        public Array stafflist { get; set; }
        public Array duplicatestudentlist { get; set; }
        public Array joinedmeeting { get; set; }
        public Array meetingliststaff { get; set; }
        public DateTime? LMSLMEET_MeetingDate { get; set; }
        public string LMSLMEET_StartedTime { get; set; }
        public string roletype { get; set; }
        public string LMSLMEET_EndTime { get; set; }
        public bool LMSLMEET_ActiveFlg { get; set; }
        public DateTime LMSLMEET_CreatedDate { get; set; }
        public DateTime LMSLMEET_UpdatedDate { get; set; }
        public long LMSLMEET_CreatedBy { get; set; }
        public long LMSLMEET_UpdatedBy { get; set; }
        public string LMSLMEET_MeetingURL { get; set; }
        public bool LMSLMEET_Recordflag { get; set; }
        public string LMSLMEET_RecordId { get; set; }
        public string LMSLMEET_internalMeetingID { get; set; }

        public Array editlist { get; set; }
        public Array meetinglist { get; set; }
        public bool duplicatemeeting { get; set; }
        public Array duplicatemeetingemp { get; set; }
        public Array duplicatemeetingclass { get; set; }
        public Array teacherslist { get; set; }
        public Array allstafflist { get; set; }
        public Array recordedmeetinglist { get; set; }
        public Array totalmeetingsofday { get; set; } 
        public Array salarylist { get; set; }
        public Array salaryDetailslist { get; set; }
        public Array salaryEarningDlist { get; set; }
        public Array allperiods { get; set; }
        public Array joinmeetinglist { get; set; }
        public Array editedmeetinglist { get; set; }
        public Array periods { get; set; }
        public Array empdetails { get; set; }
        public Array class_sectons { get; set; }
        public Array TT_final_generationDetails { get; set; }
        public Array TT_final_generation { get; set; }
        public string monthName { get; set; }
        public string HRME_Photo { get; set; }
        public string IVRMMG_GenderName { get; set; }
        public string LMSLMEET_MeetingTopic { get; set; }
        public string roleflg { get; set; }
        public decimal? salary { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public long staffuserid  { get; set; }
        public long ASMAY_Id { get; set; }
        public long PASR_ID  { get; set; }
        public string rtype { get; set; }
        public string rtype2 { get; set; }
        public string ASMCL_ClassCode { get; set; }
        public Array deviceArray { get; set; }
        public int ASMCL_Order { get; set; }
        public clsiddto[] selectedClasslist { get; set; }
        public string IVRMSTAUL_UserName { get; set; }
        public string DayName { get; set; }
        public string LMSLMEET_PlannedStartTime { get; set; }
        public string LMSLMEET_PlannedEndTime { get; set; }
        public string ClassName { get; set; }
        public string SectionName { get; set; }
        public string SubjectName { get; set; }
        public int PeriodCount { get; set; }
        public string studentnameorder { get; set; }
        public string TTMDPT_StartTime { get; set; }
        public string TTMDPT_EndTime { get; set; }
        public bool TTMDPT_ActiveFlag { get; set; }
        public string mobiledeviceid { get; set; }
        public Array mobile { get; set; }
        public Array email { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public string ASMCL_ClassName { get; set; }
        public string ASMC_SectionName { get; set; }
        public string ISMS_SubjectName { get; set; }
        public bool ASMCL_ActiveFlag { get; set; }
        public string Period { get; set; }
        public string P_Days { get; set; }
        public Array datalst { get; set; }
        public int dayCount { get; set; }
        public long TTMD_Id { get; set; }
        //   public long userid { get; set; }
        public string ASMAY_Year { get; set; }
        public Array fillstudent { get; set; }
        public int studentcount { get; set; }
        public Array fillstudentalldetails { get; set; }
        public long Amst_Id { get; set; }
        public string AMST_FirstName { get; set; }
        public string AMST_MiddleName { get; set; }
        public string AMST_LastName { get; set; }
        public string amst_FirstName { get; set; }
        public string amst_MiddleName { get; set; }
        public string amst_LastName { get; set; }
        public string amst_RegistrationNo { get; set; }
        public string amst_AdmNo { get; set; }
        public string amst_sex { get; set; }
        public DateTime amst_dob { get; set; }
        public string amst_emailid { get; set; }
        public long amay_RollNo { get; set; }
        public string classname { get; set; }
        public string sectionname { get; set; }
        public long rollno { get; set; }
        public string admno { get; set; }
        public long amst_mobile { get; set; }
        public string fathername { get; set; }
        public string mothername { get; set; }
        public string bloodgroup { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string address3 { get; set; }
        public DateTime studentdob { get; set; }
        public long? fathermobileno { get; set; }
        public string asma_year { get; set; }
        public Array examlist { get; set; }
        public string exam_name { get; set; }
        public decimal? totalmarks { get; set; }
        public decimal? obtainmarks { get; set; }
        public decimal? persentage { get; set; }
        public string result { get; set; }
        public Array yearlist { get; set; }
        public decimal? hrmed_Amount { get; set; }
        public string hrmed_Name { get; set; }
        public string HRMLY_LeaveYear { get; set; }
        public int Id { get; set; }
        public long HRES_Id { get; set; }
        public long hres_id { get; set; }
        public string HRES_Year { get; set; }
        public Array TotalEarning { get; set; }
        public Array totalDeduction { get; set; }
        public Array salarylistD { get; set; }
        public Array salarylistE { get; set; }
        public long UserId { get; set; }
        //---------------Leave Name---------------------
        public string HRML_LeaveName { get; set; }
        public string HRML_LeaveCode { get; set; }
        public string HRML_LeaveDetails { get; set; }
        public bool HRML_LeaveCreditFlg { get; set; }
        public bool studflag { get; set; }
        public bool principalflg { get; set; }
        public bool hodflg { get; set; }
        public bool managerflg { get; set; }
        public bool stafflag { get; set; }
        public string HRML_LeaveType { get; set; }

        //--------------- Month---------------------
        public long IVRM_Month_Id { get; set; }
        public string IVRM_Month_Name { get; set; }
        public bool Is_Active { get; set; }
        public int IVRM_Month_Max_Days { get; set; }
        public string HRES_Month { get; set; }
        //--------------------------------------------------------------------------------------------------

        public string HRME_EmployeeFirstName { get; set; }
        public string HRME_EmployeeMiddleName { get; set; }
        public string HRME_EmployeeLastName { get; set; }
        public string HRME_EmployeeCode { get; set; }
        public string HRME_BiometricCode { get; set; }
        public long? HRME_RFCardId { get; set; }
        public string HRME_PerStreet { get; set; }
        public string HRME_PerArea { get; set; }
        public string HRME_PerCity { get; set; }
        public string HRME_PerAdd4 { get; set; }
        public long? HRME_PerStateId { get; set; }
        public long? HRME_PerCountryId { get; set; }
        public long? HRME_PerPincode { get; set; }
        public string HRME_LocStreet { get; set; }
        public string HRME_LocArea { get; set; }
        public string HRME_LocCity { get; set; }
        public string HRME_LocAdd4 { get; set; }
        public long? HRME_LocStateId { get; set; }
        public long? HRME_LocCountryId { get; set; }
        public long? HRME_LocPincode { get; set; }
        public long? IVRMMMS_Id { get; set; }
        public long? IVRMMG_Id { get; set; }
        public long? CasteCategoryId { get; set; }
        public long? CasteId { get; set; }
        public long? ReligionId { get; set; }
        public string HRME_FatherName { get; set; }
        public string HRME_MotherName { get; set; }
        public string HRME_SpouseName { get; set; }
        public string HRME_SpouseOccupation { get; set; }
        public long? HRME_SpouseMobileNo { get; set; }
        public string HRME_SpouseEmailId { get; set; }
        public string HRME_SpouseAddress { get; set; }
        public DateTime? HRME_DOB { get; set; }
        public DateTime? HRME_DOJ { get; set; }
        public DateTime? HRME_ExpectedRetirementDate { get; set; }
        public DateTime? HRME_PFDate { get; set; }
        public DateTime? HRME_ESIDate { get; set; }
        public DateTime LMSLMEET_PlannedDate { get; set; }
        public string PlannedDate  { get; set; }
        public long? HRME_MobileNo { get; set; }
        public string HRME_EmailId { get; set; }
        public string HRME_BloodGroup { get; set; }
        public string HRME_PaymentType { get; set; }
        public string HRME_BankAccountNo { get; set; }
        public string HRME_PFApplicableFlag { get; set; }
        public string HRME_PFMaxFlag { get; set; }
        public string HRME_PFFixedFlag { get; set; }
        public string HRME_PFAccNo { get; set; }
        public string HRME_ESIAccNo { get; set; }
        public string HRME_GratuityAccNo { get; set; }
        public string HRME_ESIApplicableFlag { get; set; }
        public string HRME_PhotoNo { get; set; }
        public string HRME_LeftFlag { get; set; }
        public DateTime HRME_DOL { get; set; }
        public string HRME_LeavingReason { get; set; }
        public string HRME_Height { get; set; }
        public string HRME_HeightUOM { get; set; }
        public int? HRME_Weight { get; set; }
        public string HRME_WeightUOM { get; set; }
        public string HRME_IdentificationMark { get; set; }
        public string HRME_ApprovalNo { get; set; }
        public string HRME_PANCardNo { get; set; }
        public string HRME_AadharCardNo { get; set; }
        public string HRME_SubstituteFlag { get; set; }
        public string HRME_NationalSSN { get; set; }
        public string HRME_SalaryType { get; set; }
        public string HRME_EmployeeOrder { get; set; }

        public string studentName { get; set; }

        //------HR_Emp_OB_Leave-------------------------------------------------------

        public long HREOBL_Id { get; set; }
        public long HRMLY_Id { get; set; }
        public DateTime HREOBL_Date { get; set; }
        public int HREOBL_OBLeaves { get; set; }
        public string leave_ob_name { get; set; }


        //----------------------[HR_Emp_Leave_Trans]-----------------------

        public long HRELT_Id { get; set; }
        public long HRELT_LeaveId { get; set; }
        public DateTime HRELT_FromDate { get; set; }
        public DateTime HRELT_ToDate { get; set; }
        public double HRELT_TotDays { get; set; }
        public DateTime HRELT_Reportingdate { get; set; }
        public string HRELT_LeaveReason { get; set; }
        public string HRELT_Status { get; set; }
        public bool HRELT_ActiveFlag { get; set; }

        //----------------------------HR_Master_Leave_Details_CFMonth-----------------------------------------

        public long HRMLDCFM_Id { get; set; }
        public long HRMLDCF_Id { get; set; }
        public int HRMLDCFM_MonthId { get; set; }

        ////-----------------------------HR_Leave_Transaction-----------------------------------------------------------
        //public long HRLT_Id { get; set; }
        //public float HRLT_Availed { get; set; }
        //public DateTime HRLT_From_Date { get; set; }
        //public char HRLT_LOP_Flag { get; set; }
        //public DateTime HRLT_To_Date { get; set; }
        //public string HRLT_REMARKS { get; set; }

        //--------------------------[HR_Emp_Leave_Credit]---------------------------------------------
        public long HRELC_Id { get; set; }
        public DateTime HRELC_Date { get; set; }
        public string HRELC_LCMonth { get; set; }
        public int HRELC_CrLeaves { get; set; }

        //-----------------------------[HR_Master_Grade]----------------------------------------------

        public string HRMG_GradeName { get; set; }
        public string HRMG_GradeDisplayName { get; set; }
        public string HRMG_PayScaleRange { get; set; }
        public decimal HRMG_PayScaleFrom { get; set; }
        public decimal HRMG_IncrementOf { get; set; }
        public decimal HRMG_PayScaleTo { get; set; }
        public int HRMG_Order { get; set; }
        public bool HRMG_ActiveFlag { get; set; }

        //----------------------------HR_Leave_Authorisation----------------------------------------------------------
        public long HRLA_Id { get; set; }
        public string HRLA_EmailTo { get; set; }
        public string HRLA_EmailCC { get; set; }

        //------------------------HR_Leave_Auth_OrderNo------------------------------------

        public long HRLAON_Id { get; set; }

        public long HRLAON_SanctionLevelNo { get; set; }
        public bool HRLAON_FinalFlg { get; set; }

        //-------------------------[HR_Emp_Leave_Trans_Details]--------------------------------------
        public long HRELTD_Id { get; set; }
        public DateTime HRELTD_FromDate { get; set; }
        public DateTime HRELTD_ToDate { get; set; }
        public double? HRELTD_TotDays { get; set; }
        public bool HRELTD_LWPFlag { get; set; }
        public tempDTO[] emplist { get; set; }

        //online Leave 
        public long HRELS_Id { get; set; }
        public decimal HRELS_OBLeaves { get; set; }
        public decimal HRELS_CreditedLeaves { get; set; }
        public decimal HRELS_TotalLeaves { get; set; }
        public decimal HRELS_TransLeaves { get; set; }
        public int HRELS_EncashedLeaves { get; set; }
        public decimal HRELS_CBLeaves { get; set; }

        //--save online leave application
        public long HRELAP_Id { get; set; }
        public long HRELAP_ApplicationID { get; set; }
        public DateTime? HRELAP_ApplicationDate { get; set; }
        public DateTime? HRELAP_FromDate { get; set; }
        public DateTime? HRELAP_ToDate { get; set; }
        public int HRELAP_TotalDays { get; set; }
        public string HRELAP_LeaveReason { get; set; }
        public long HRELAP_ContactNoOnLeave { get; set; }
        public DateTime? HRELAP_ReportingDate { get; set; }
        public string HRELAP_ApplicationStatus { get; set; }
        public Array master_eventlist { get; set; }
        //MB
        public LeaveCreditDTO[] temp_table_data { get; set; }
        //
        public string HRELAP_SanctioningLevel { get; set; }
        public bool HRELAP_FinalFlag { get; set; }
        public bool HRELAP_ActiveFlag { get; set; }

        //-----------------[HR_Master_LeaveYear]-------------------------------

        public DateTime HRMLY_FromDate { get; set; }
        public DateTime HRMLY_ToDate { get; set; }
        public bool HRMLY_ActiveFlag { get; set; }

        //----------------------HR_Master_Leave_Details_CreditMonth-------------------------------
        public long HRMLDCM_Id { get; set; }
        public string HRMLDCM_LCMonthCode { get; set; }
        //----------------------HR_Master_Leave_Details_CF-------------------------------        
        public bool HRMLDCF_MaxLeaveAplFlg { get; set; }
        public int HRMLDCF_MaxLeaveCF { get; set; }

        //----------------------[HR_Master_Leave_Details_EnCash]-------------------------------   
        public long HRMLDEC_Id { get; set; }
        public bool HRMLDEC_ServiceAplFlg { get; set; }
        public string HRMLDEC_ServiceYear { get; set; }
        public string HRMLDEC_ServiceMonth { get; set; }
        public string HRMLDEC_ServiceDays { get; set; }
        public bool HRMLDEC_MaxLeaveFlg { get; set; }
        public int HRMLDEC_MaxLeaves { get; set; }
        public bool HRMLDEC_MinLeaveFlg { get; set; }
        public int HRMLDEC_MinLeaves { get; set; }
        public string HRMLDEC_ScheduleFlg { get; set; }
        public string HRMLDEC_ScheduleYear { get; set; }
        public string HRMLDEC_ScheduleMonth { get; set; }
        public bool HRMLDEC_VariableFixedFlg { get; set; }
        public decimal HRMLDEC_FixedAmount { get; set; }

        //--------------------------Leave Report Store Procedure---------------------------
        public string multipleEmp { get; set; }
        public long Month { get; set; }
        public long Year { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }


        //--------------------[HR_Emp_Leave_Appl_Authorisation]---------------------------

        public long HRELAPA_Id { get; set; }
        public string HRELAPA_SanctioningLevel { get; set; }
        public string HRELAPA_Remarks { get; set; }
        public bool HRELAPA_FinalFlag { get; set; }

        //-----------------END-------------------------------

        //********************************************************************
        public long IGA_Id { get; set; }
        public string IGA_GalleryName { get; set; }
        public DateTime IGA_Date { get; set; }
        public bool IGA_CommonGalleryFlg { get; set; }
        public string IGA_Time { get; set; }
        public long IGAP_Id { get; set; }
        public string IGAP_Photos { get; set; }
        public bool IGAP_CoverPhotoFlag { get; set; }
        public Array imagegallery { get; set; }
        public Array videogallery { get; set; }
        public long HRML_Id { get; set; }
        public string HRMDES_DesignationName { get; set; }
        public LeaveCreditDTO[] auth_array { get; set; }
        public Array emp_trans { get; set; }
        public Array earnded { get; set; }
        public Array Designation_types { get; set; }
        public Array Department_types { get; set; }
        public Array credit_month { get; set; }
        public Array leave_name { get; set; }
        public Array leave_code { get; set; }
        public Array get_emp { get; set; }
        public Array get_emp_lop { get; set; }
        public Array get_emp_other { get; set; }
        public Array get_auth { get; set; }
        public Array leavearrayyear { get; set; }
        public Array get_leaveDetails { get; set; }
        public LeaveCreditDTO[] emptypes { get; set; }
        public LeaveCreditDTO[] emplop { get; set; }
        public LeaveCreditDTO[] desgtypes { get; set; }
        public long leavecode { get; set; }
        public Array online_leave { get; set; }
        public LeaveCreditDTO[] leaveobtype { get; set; }
        public LeaveCreditDTO[] empdept { get; set; }
        public LeaveCreditDTO[] empdesg { get; set; }
        public int count { get; set; }
        public bool returnval { get; set; }
        public bool Edit_flag { get; set; }
        public LeaveCreditMonthDTO[] leaveCM { get; set; }
        public LeaveDetailsCFDTO[] leaveCf { get; set; }
        public Array activityIds { get; set; }
        public Array activitycf { get; set; }
        public string returnduplicatestatus { get; set; }
        public Array master_loplist { get; set; }
        public Array edit_auth { get; set; }
        public Array grade_name { get; set; }
        public Array authData { get; set; }
        public Array get_year { get; set; }
        public LeaveCreditDTO[] selectedMonthList { get; set; }
        public LeaveCreditDTO[] selectedcarryMonthList { get; set; }
        //  public LeaveCreditDTO[] selectedreport { get; set; }
        public long[] selectedreport { get; set; }
        public LeaveCreditDTO[] get_leave_status { get; set; }
        public Array get_leavestatus { get; set; }
        public Array leave_year_id { get; set; }
        public string message { get; set; }
        public LeaveCreditDTO[] frmToDates { get; set; }
        public GroupType_TempDto[] GroupTypeTempDto { get; set; }
        public Department_TempDto[] DepartmentTempDto { get; set; }
        public Designation_TempDto[] DesignationTempDto { get; set; }
        public Grade_TempDto[] GradeTempDto { get; set; }
        public LeaveCreditDTO[] Emp_types { get; set; }
        public LeaveCreditDTO[] dept_types { get; set; }
        public LeaveCreditDTO[] desig_types { get; set; }
        public LeaveCreditDTO[] grade_types { get; set; }
        public Array loplist { get; set; }
        // public string message { get; set; }
        public Array SectionList { get; set; }
        public Array academicList { get; set; }
        public Array classlist { get; set; }
        public Array studentList { get; set; }
        public long miid { get; set; }
        public Array academicListdefault { get; set; }

        ////////////////////////////for punch and attendence
        public Array Emp_punchDetails { get; set; }
        public Array filldepartment { get; set; }
        public Array filldesignation { get; set; }
        public Array fillemployee { get; set; }
        public Array filldata { get; set; }
        public string multipletype { get; set; }
        public string multipledep { get; set; }
        public string multipledes { get; set; }
        public string multipleemp { get; set; }
        public long HRMD_Id { get; set; }
        public string HRME_TechNonTeachingFlg { get; set; }
        public string HRMD_DepartmentName { get; set; }
        public long HRMDES_Id { get; set; }
        public string ename { get; set; }
        public DateTime? fromdate { get; set; }
        public DateTime? todate { get; set; }
        public Array columnnames { get; set; }
        public string multiplehrmeid { get; set; }
        public string Lateby { get; set; }
        public string FOEP_PunchDate { get; set; }
        public string punchtype { get; set; }
        public string selectdate { get; set; }
        public string selectmonth { get; set; }
        public string selectyear { get; set; }
        ///////////////////////////////end
        public string empFname { get; set; }
        public string empMname { get; set; }
        public string empLname { get; set; }
        public DateTime? punchdate { get; set; }
        public string punchtime { get; set; }
        public string InOutFlg { get; set; }
        public Array remarklist { get; set; }
        public Array remarklistS { get; set; }
        public Array exmstdlist { get; set; }
        public Array clstchname { get; set; }
        public Array savelist { get; set; }
        public Array savelisttot { get; set; }
        public Array subjlist { get; set; }
        public Array grade_details { get; set; }
        public long EME_Id { get; set; }
        public Decimal present { get; set; }
        public Decimal classheld { get; set; }
        public int type { get; set; }
        public Decimal perc { get; set; }
        public Array attendencelist { get; set; }
        public Array fillmonths { get; set; }
        public string studentname { get; set; }
        public Int64 monthid { get; set; }
        public Array allstudent { get; set; }
        public Array coedata { get; set; }
        public string eventName { get; set; }
        public string eventDesc { get; set; }
        public DateTime? COEE_EStartDate { get; set; }
        public DateTime? COEE_EEndDate { get; set; }
        public string COEME_EventName { get; set; }
        public string COEME_EventDesc { get; set; }
        public DateTime? COEE_ReminderDate { get; set; }
        public string TXTRemark { get; set; }
        public bool EMER_ActiveFlag { get; set; }
        public Array calenderlist { get; set; }
        public long IVRMUL_Id { get; set; }
        public string version { get; set; }
        public studentArrayDTO[] studentArray { get; set; }
        public Array vmspaymentsubsctiptiondto { get; set; }
        public Array getpaymentnotificationdetails { get; set; }
        public string DeviceID { get; set; }
        public string subscriptionremarks { get; set; }
        public Array getreportdetails { get; set; }

        public seciddto[] secids { get; set; }
        public subiddto[] subids { get; set; }
        public stfiddto[] stfids { get; set; }
        public stuiddto[] stids { get; set; }

        public savedto[] saveaarylist  { get; set; }

    }

    public class clsiddto {
        public long ASMCL_Id { get; set; }
        public string ASMCL_ClassCode { get; set; }

    }
    public class savedto
    {
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long ISMS_Id { get; set; }

    }
    public class seciddto {
        public long ASMS_Id { get; set; }
        public string ASMC_SectionCode { get; set; }

    }
    public class subiddto {
        public long ISMS_Id { get; set; }
        public string ISMS_SubjectCode { get; set; }

    }
    public class stfiddto {
        public long UserId { get; set; }
        public long HRME_Id { get; set; }
     
    }
    public class stuiddto {
        public long amst_Id { get; set; }
       
     
    }
    public class jasonresult
    {
       public JArray response { get; set; }

    }
    public class jasonresultchild
    {

        public jasonresultchildatt[] attendees { get; set; }
    }
    public class jasonresultchildatt
    {
        public string fullName { get; set; }
    }


}