using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.VMS.HRMS
{
    public class HR_Candidate_DetailsDTO : CommonParamDTO
    {
        public long HRCD_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRMPT_Id { get; set; }
        public long HRMC_Id { get; set; }
        public long? ASMAY_Id { get; set; }
        public long? ID { get; set; }
        public long? roleId { get; set; }
        public long HRCD_MRFNO { get; set; }
        public string HRCD_FullName { get; set; }
        public string HRCD_FirstName { get; set; }
        public string HRCD_MiddleName { get; set; }
        public string HRCD_LastName { get; set; }
        public Array paydet { get; set; }
        public long HRMJ_Id { get; set; }
        public string HRCD_Skills { get; set; }
        public DateTime HRCD_DOB { get; set; }
        public long IVRMMG_Id { get; set; }
        public long HRCD_MobileNo { get; set; }
        public string HRCD_EmailId { get; set; }
        public decimal HRCD_ExpFrom { get; set; }
        public decimal HRCD_ExpTo { get; set; }
        public string HRCD_CurrentCompany { get; set; }
        public string HRCD_ResumeSource { get; set; }
        public string HRCD_JobPortalName { get; set; }
        public string HRCD_RefCode { get; set; }
        //public long HRCD_RefCode { get; set; }
        public decimal HRCD_LastCTC { get; set; }
        public decimal HRCD_ExpectedCTC { get; set; }
        public DateTime HRCD_AppDate { get; set; }
        public DateTime HRCD_InterviewDate { get; set; }
        public long HRCD_NoticePeriod { get; set; }
        public string HRCD_Remarks { get; set; }
        public string HRCD_Resume { get; set; }
        public string HRCD_RecruitmentStatus { get; set; }
        public bool HRCD_ActiveFlg { get; set; }
        public long HRCD_CreatedBy { get; set; }
        public long HRCD_UpdatedBy { get; set; }
        //public DateTime CreatedDate { get; set; }
        //public DateTime UpdatedDate { get; set; }
        public string applydate { get; set; }
        public string HRCD_Department { get; set; }
        public long HRME_ID { get; set; }

        public string retrunMsg { get; set; }
        public string HRMPT_Name { get; set; }
        public string HRMC_QulaificationName { get; set; }
        public string IVRMMG_GenderName { get; set; }
        public string HRMJ_JobCode { get; set; }
        public string HRMJ_JobTiTle { get; set; }

        public Array VMSCandidateList { get; set; }
        public Array MasterPosTypeList { get; set; }
        public Array MasterQualification { get; set; }
        public Array MasterGender { get; set; }
        public Array VMSEditValue { get; set; }
        public Array VMSMRFList { get; set; }
        public Array masterjob { get; set; }
        public Array earningList { get; set; }
        public Array detectionList { get; set; }
        public Array arrearList { get; set; }
        public Array grossList { get; set; }
        public Array institutionlist { get; set; }
        public Array employeeEarningsDeductionsDetails { get; set; }
        public HR_Candidate_EarningsDeductionsDTO[] EarningDTO { get; set; }
        public HR_Candidate_EarningsDeductionsDTO[] DeductionDTO { get; set; }
        public HR_Candidate_EarningsDeductionsDTO[] ArrearDTO { get; set; }
        public string onlinepaygteway { get; set; }      
        public HR_Candidate_DetailsDTO Employeedto { get; set; }
        public Array CandidateDetails { get; set; }
        public Array companydetails { get; set; }
        public Array MasterEmployeetype { get; set; }
        public Array GroupTypeList { get; set; }
        public Array GradeList { get; set; }
        public Array maritalstatuslist { get; set; }
        public Array castecategorylist { get; set; }
        public Array castelist { get; set; }
        public Array masterreligionlist { get; set; }
        public Array employeedepartmentlist { get; set; }
        public Array employeedesignationlist { get; set; }
        public Array mastercountry { get; set; }
        public Array masterReligion { get; set; }
        public Array masterCaste { get; set; }
        public Array mastermaritalstatus { get; set; }
        public Array companylist { get; set; }
        public Array letterdetails { get; set; }

        public long HRMD_Id { get; set; }
        public string HRMD_DepartmentName { get; set; }
        public Int32? HRMD_Order { get; set; }
        public Array departmenlist { get; set; }
        public Array candidatelist { get; set; }

        public Array desgnationlist { get; set; }
        public Array departmentlist { get; set; }
        public string HRMDES_DesignationName { get; set; }
        public long HRMDES_Id { get; set; }
        public Int32? HRMDES_Order { get; set; }
        public Array earingdeductionlist { get; set; }
        public string HRMED_Name { get; set; }
        public string HRMED_AmountPercentFlag { get; set; }
        public long HRMED_Id { get; set; }
        public string percentOff { get; set; }

        //SALARY PART
        public long HRMEDT_Id { get; set; }
        public string HRMED_EDTypeFlag { get; set; }
        public string HRMED_AmountPercent { get; set; }
        public bool HRMED_ActiveFlag { get; set; }
        public string HRMED_RoundOffFlag { get; set; }
        public string HRMED_EarnDedFlag { get; set; }
        public long HRMED_Order { get; set; }
        public long HRCED_Id { get; set; }
        public decimal HRCED_Amount { get; set; }
        public string HRCED_Percentage { get; set; }
        public bool HRCED_ActiveFlag { get; set; }
        //SALARY PART


        public string HRCD_Photo { get; set; }
        public string HRCD_FatherName { get; set; }
        public long? HRCD_AadharNo { get; set; }
        public string HRCD_PAN { get; set; }
        public DateTime? HRCD_JoiningDate { get; set; }
        public string HRCD_Designation { get; set; }
        public long? HRCD_BondDuration { get; set; }
        public string HRCD_AddressLocal { get; set; }
        public string HRCD_AddressPermanent { get; set; }
        public string HRCD_NatureOfWork { get; set; }
        public string HRCD_ScopeOfService { get; set; }
        public string HRCD_SHName { get; set; }
        public string HRCD_SHAddress { get; set; }
        public long? HRCD_SHGender { get; set; }
        public string HRCD_SHGenderName { get; set; }
        public long? HRCD_SHContactNo { get; set; }
        public string HRCD_Place { get; set; }
        public long? HRCD_PINCode { get; set; }
        public long? HRMET_Id { get; set; }
        public long? HRMGT_Id { get; set; }
        public long? HRMG_Id { get; set; }
        public long? IVRMMMS_Id { get; set; }
        public long? IMCC_Id { get; set; }
        public long? IMC_Id { get; set; }
        public long? IVRMMR_Id { get; set; }
        public string HRMET_EmployeeType { get; set; }
        public string HRMGT_EmployeeGroupType { get; set; }
        public string HRMG_GradeName { get; set; }
        public string IVRMMMS_MaritalStatus { get; set; }
        public string IMCC_CategoryName { get; set; }
        public string IMC_CasteName { get; set; }
        public string IVRMMR_Name { get; set; }
        public int? HRMGT_Order { get; set; }
        public string HRME_EmployeeCode { get; set; }
        public DateTime HRCISC_InterviewDateTime { get; set; }
        public string HRCISC_InterviewRounds { get; set; }
        public string HRCISC_InterviewVenue { get; set; }
        public string HRME_employeename { get; set; }
        public string HRCIS_InterviewFeedBack { get; set; }
        public string HRCIS_Status { get; set; }
        public Array interviewlist { get; set; }
        public long casteCategoryId { get; set; }
        public long casteId { get; set; }
        public long religionId { get; set; }
        public long UserId { get; set; }

        public HR_Candidate_QualificationsDTO[] QualificationsDTO { get; set; }
        public HR_Candidate_ExperienceDTO[] ExperienceDTO { get; set; }
        public HR_Candidate_LanguagesDTO[] LanguagesDTO { get; set; }
        public HR_Candidate_FamilyDTO[] FamilyDTO { get; set; }

        public Array qualificationlist { get; set; }
        public Array experiencelist { get; set; }
        public Array languagelist { get; set; }
        public Array familylist { get; set; }

        public long HRCD_NationalityId { get; set; }
        public long HRCD_Religion { get; set; }
        public long HRCD_MaritalStatus { get; set; }
        public string HRCD_BloodGroup { get; set; }
        public long HRCD_CasteId { get; set; }
        public string Template { get; set; }
        public bool welcomenotice { get; set; }
        public bool thanksnotice { get; set; }
        public string HRCIS_CandidateStatus { get; set; }

        public string HRCD_AddressLocal2 { get; set; }
        public string HRCD_AddLocalPlace { get; set; }
        public long HRCD_AddLocalPIN { get; set; }
        public string HRCD_AddressPermanent2 { get; set; }
        public string HRCD_AddPermanentPlace { get; set; }
        public long HRCD_AddPermanentPIN { get; set; }
        public string HRCD_SHAddress2 { get; set; }

        public string HRCA_FirstDocName { get; set; }
        public string HRCA_SecDocName { get; set; }
        public DateTime HRCA_FirstDocDate { get; set; }
        public DateTime HRCA_SecDocDate { get; set; }
        public decimal HRCA_AnnualCTC { get; set; }
        public decimal HRCA_MonthlyCTC { get; set; }
        public string HRCA_AppointmentRefNo { get; set; }
        public string HRCA_AcknowledgementRefNo { get; set; }
        public long HRCA_Id { get; set; }
    }

}