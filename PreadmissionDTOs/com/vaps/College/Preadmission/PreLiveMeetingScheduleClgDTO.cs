﻿using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.College.Preadmission
{
   public class PreLiveMeetingScheduleClgDTO
    {
        public object amstid;
        public long RoleId { get; set; }
        public long interval { get; set; }
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
        public string returnvalue { get; set; }
        public bool returnval { get; set; }

        public string remarks { get; set; }
        public string grade { get; set; }
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
        public long staffuserid { get; set; }
        public long ASMAY_Id { get; set; }
        public long PACA_Id { get; set; }
        public string rtype { get; set; }
        public string rtype2 { get; set; }
        public string ASMCL_ClassCode { get; set; }
        public Array deviceArray { get; set; }
        public int ASMCL_Order { get; set; }
       // public clsiddto[] selectedClasslist { get; set; }
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
        public string PlannedDate { get; set; }
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
    }
}