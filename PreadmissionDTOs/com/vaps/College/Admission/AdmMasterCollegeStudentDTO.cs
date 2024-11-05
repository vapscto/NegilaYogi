using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.College.Admission
{
    public class AdmMasterCollegeStudentDTO
    {
        public string msgreturn { get; set; }
        public string Message { get; set; }
        public long AMCST_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public string AMCST_FirstName { get; set; }
        public string AMCST_MiddleName { get; set; }
        public string AMCST_LastName { get; set; }
        public DateTime? AMCST_Date { get; set; }
        public string AMCST_RegistrationNo { get; set; }
        public string AMCST_AdmNo { get; set; }
        public long? AMCOC_Id { get; set; }
        public long? AMCO_Id { get; set; }
        public string AMCST_Sex { get; set; }
        public DateTime? AMCST_DOB { get; set; }
        public string AMCST_DOBin_words { get; set; }
        public int? AMCST_Age { get; set; }
        public string AMCST_BloodGroup { get; set; }
        public string AMCST_MotherTongue { get; set; }
        public string AMCST_BirthCertNo { get; set; }
        public long? IVRMMR_Id { get; set; }
        public long? IMCC_Id { get; set; }
        public long? IMC_Id { get; set; }
        public string AMCST_StudentSubCaste { get; set; }
        public string AMCST_PerStreet { get; set; }
        public string AMCST_PerArea { get; set; }
        public string AMCST_PerCity { get; set; }
        public string AMCST_PerAdd3 { get; set; }
        public long? AMCST_PerState { get; set; }
        public long? IVRMMC_Id { get; set; }
        public long? AMCST_PerPincode { get; set; }
        public string AMCST_ConStreet { get; set; }
        public string AMCST_ConArea { get; set; }
        public string AMCST_ConAdd3 { get; set; }
        public string AMCST_ConCity { get; set; }
        public string AMCST_Village { get; set; }
        public string AMCST_Taluk { get; set; }
        public string AMCST_District { get; set; }
        public long? AMCST_ConState { get; set; }
        public string AMCST_Urban_Rural { get; set; }
        public long? AMCST_ConCountryId { get; set; }
        public long? AMCST_ConPincode { get; set; }
        public long? AMCST_AadharNo { get; set; }
        public long? AMCST_StuBankAccNo { get; set; }
        public string AMCST_StuBankIFSCCode { get; set; }
        public string AMCST_StuCasteCertiNo { get; set; }
        public long? AMCST_MobileNo { get; set; }
        public string AMCST_emailId { get; set; }
        public bool? AMCST_FatherAliveFlag { get; set; }
        public string AMCST_FatherName { get; set; }
        public long? AMCST_FatherAadharNo { get; set; }
        public string AMCST_FatherSurname { get; set; }
        public string AMCST_FatherEducation { get; set; }
        public string AMCST_FatherOccupation { get; set; }
        public string AMCST_FatherOfficeAdd { get; set; }
        public string AMCST_FatherDesignation { get; set; }
        public decimal? AMCST_FatherMonIncome { get; set; }
        public decimal? AMCST_FatherAnnIncome { get; set; }
        public string AMCST_FatherNationality { get; set; }
        public string AMCST_FatherReligion { get; set; }
        public string AMCST_FatherCaste { get; set; }
        public string AMCST_FatherSubCaste { get; set; }
        public long? AMCST_FatherMobleNo { get; set; }
        public string AMCST_FatheremailId { get; set; }
        public long? AMCST_FatherBankAccNo { get; set; }
        public string AMCST_FatherBankIFSCCode { get; set; }
        public string AMCST_FatherCasteCertiNo { get; set; }
        public string AMCST_FatherPhoto { get; set; }
        public string AMCST_FatherSign { get; set; }
        public string AMCST_FatherFingerprint { get; set; }
        public bool? AMCST_MotherAliveFlag { get; set; }
        public string AMCST_MotherName { get; set; }
        public long? AMCST_MotherAadharNo { get; set; }
        public string AMCST_MotherSurname { get; set; }
        public string AMCST_MotherEducation { get; set; }
        public string AMCST_MotherOccupation { get; set; }
        public string AMCST_MotherOfficeAdd { get; set; }
        public string AMCST_MotherDesignation { get; set; }
        public decimal? AMCST_MotherMonIncome { get; set; }
        public decimal? AMCST_MotherAnnIncome { get; set; }
        public string AMCST_MotherNationality { get; set; }
        public string AMCST_MotherReligion { get; set; }
        public string AMCST_MotherCaste { get; set; }
        public string AMCST_MotherSubCaste { get; set; }
        public long? AMCST_MotherMobleNo { get; set; }
        public string AMCST_MotheremailId { get; set; }
        public long? AMCST_MotherBankAccNo { get; set; }
        public string AMCST_MotherBankIFSCCode { get; set; }
        public string AMCST_MotherCasteCertiNo { get; set; }
        public decimal? AMCST_TotalIncome { get; set; }
        public string AMCST_MotherSign { get; set; }
        public string AMCST_MotherPhoto { get; set; }
        public string AMCST_MotherFingerprint { get; set; }
        public string AMCST_BirthPlace { get; set; }
        public long AMCST_Nationality { get; set; }
        public bool? AMCST_BPLCardFlag { get; set; }
        public string AMCST_BPLCardNo { get; set; }
        public bool? AMCST_HostelReqdFlag { get; set; }
        public bool? AMCST_TransportReqdFlag { get; set; }
        public bool? AMCST_GymReqdFlag { get; set; }
        public bool? AMCST_ECSFlag { get; set; }
        public bool? AMCST_PaymentFlag { get; set; }
        public decimal? AMCST_AmountPaid { get; set; }
        public string AMCST_PaymentType { get; set; }
        public DateTime? AMCST_PaymentDate { get; set; }
        public string AMCST_ReceiptNo { get; set; }
        public string AMCST_EMSINo { get; set; }
        public string AMCST_ApplStatus { get; set; }
        public bool? AMCST_FinalpaymentFlag { get; set; }
        public string AMCST_StudentPhoto { get; set; }
        public string AMCST_StudentSign { get; set; }
        public string AMCST_StudentFingerprint { get; set; }
        public int? AMCST_NoofSiblingsSchool { get; set; }
        public int? AMCST_NoofSiblings { get; set; }
        public int? AMCST_NoOfBrothers { get; set; }
        public int? AMCST_NoOfSisters { get; set; }
        public int? AMCST_NoofDependencies { get; set; }
        public long? AMCST_TPINNO { get; set; }
        public bool AMCST_ActiveFlag { get; set; }
        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }
        public string AMCST_SOL { get; set; }
        public long ACMB_Id { get; set; }
        public long ACQ_Id { get; set; }
        public long ACQC_Id { get; set; }
        public long ACSS_Id { get; set; }
        public long ACST_Id { get; set; }
        public bool Edit_flag { get; set; }

        public string AMCST_BiometricId { get; set; }
        public string AMCST_RFCardNo { get; set; }
        

        //Arrays.
        public Array academicYearOnLoad { get; set; }
        public Array AllAcademicYear { get; set; }
        public Array branches { get; set; }
        public Array AllSources { get; set; }
        public Array AllRefrence { get; set; }
        public Array AllcasteCategory { get; set; }
        public Array AllCaste { get; set; }
        public Array AllReligion { get; set; }
        public Array AllCountry { get; set; }

        public Array AllCountrycode { get; set; }
        
        public Array AllState { get; set; }
        public Array studentCategory { get; set; }
        public Array courses { get; set; }
        public Array semesters { get; set; }
        public Array batches { get; set; }
        public Array quotas { get; set; }
        public Array quotaCategory { get; set; }
        public Array subjectScheme { get; set; }
        public Array schemeType { get; set; }
        public Array admTransNumSetting { get; set; }
        public Array StudentList { get; set; }
        public Array boardList { get; set; }
        public Array Schooltypelist { get; set; }
        public Array prevStateList { get; set; }
        public Array referenceIds { get; set; }
        public Array sourceIds { get; set; }
        public Array PrevSchoolDetails { get; set; }
        public Array Adm_College_Student_SubjectMarksDTO { get; set; }
        public Array StudentGuardianDetails { get; set; }
        public Array StudentSiblingDetails { get; set; }
        public Array StudentReferenceDetails { get; set; }
        public Array StudentSourceDetails { get; set; }
        public Array DocumentList { get; set; }
        public Array combinationlist { get; set; }
        public Array mastersectionlist { get; set; }
        public Array currentyearstudentdetails { get; set; }
        public Adm_College_Student_SMSNoDTO[] Adm_College_Student_SMSNoDTO { get; set; }
        public Adm_College_Student_EmailIdDTO[] Adm_College_Student_EmailIdDTO { get; set; }
        public Adm_M_Student_TempMobileNo[] stdmobile { get; set; }
        public Adm_M_Student_TempEmailId[] stdemail { get; set; }
        public FatherMultipleEmailIdDTO[] FatherMultipleEmailIdDTO { get; set; }
        public FatherMultipleMobileNoDTO[] FatherMultipleMobileNoDTO { get; set; }
        public MotherMultipleEmailIdDTO[] MotherMultipleEmailIdDTO { get; set; }
        public MotherMultipleMobileNoDTO[] MotherMultipleMobileNoDTO { get; set; }
        public AdmCollegeStudentReferenceDTO[] SelectedRefrenceDetails { get; set; }
        public AdmCollegeStudentSourceDTO[] SelectedSourceDetails { get; set; }
        public AdmCollegeStudentPrevSchoolDTO[] SelectedPrevSchoolDetails { get; set; }
        public AdmCollegeStudentGuardianDTO[] SelectedGuardianDetails { get; set; }
        public AdmCollegeStudentSiblingsDetailsDTO[] SelectedSiblingDetails { get; set; }
        public CollegeMasterDocumentDTO[] Uploaded_documentList { get; set; }
        public Master_NumberingDTO transnumconfigsettings { get; set; }
        public Master_NumberingDTO admissionNumbering { get; set; }
        public Adm_College_Student_SubjectMarksDTO[] Adm_College_Student_SubjectMarksTempDTO { get; set; }
        public retuen_student_DTO[] retuen_student_DTO { get; set; }
        public string courseName { get; set; }
        public string branchName { get; set; }
        public string semesterName { get; set; }
        public string batchName { get; set; }
        public string messagesection { get; set; }
        public long ACAYC_Id { get; set; }
        public long ACAYCB_Id { get; set; }
        public long ACAYCBS_Id { get; set; }
        public string IMC_CasteName { get; set; }
        public string AMCOC_Name { get; set; }
        public string stdmobilenumber { get; set; }
        public string admRegManualFlag { get; set; }
        public string admAdmManualFlag { get; set; }
        public string preventdupRegNo { get; set; }
        public string preventdupAdmNo { get; set; }
        public int duplicateRegNo { get; set; }
        public int duplicateAdmNo { get; set; }
        public int duplicateEmailId { get; set; }
        public int duplicateAdharNo { get; set; }
        public string countryName { get; set; }
        public string SearchColumn { get; set; }
        public string EnteredData { get; set; }
        public int count { get; set; }
        public string AMCST_PassportNo { get; set; }
        public string AMCST_PassportIssuedAt { get; set; }
        public DateTime? AMCST_PassportIssueDate { get; set; }
        public long? AMCST_PassportIssuedCounrty { get; set; }
        public string AMCST_PassportIssuedPlace { get; set; }
        public DateTime? AMCST_PassportExpiryDate { get; set; }
        public string AMCST_VisaIssuedBy { get; set; }
        public DateTime? AMCST_VISAValidFrom { get; set; }
        public DateTime? AMCST_VISAValidTo { get; set; }
        public bool AMCST_Divyangjan { get; set; }
        public long ACMS_Id { get; set; }
        public long ACYST_RollNo { get; set; }
        public long? LoginId { get; set; }
        public string sectionname { get; set; }
        public bool returnval { get; set; }
        
       public string studentname { get; set; }

        public string ASMAY_Year { get; set; }
        public string AMCO_CourseName { get; set; }
        public Array viewstudentjoineddetails { get; set; }
        public Array viewstudentdetails { get; set; }
        public Array viewstudentacademicyeardetails { get; set; }


        public string AMB_BranchName { get; set; }
        public string Status_Flag { get; set; }
        public long order { get; set; }
        public Array viewstudentguardiandetails { get; set; }

        public long subjorder { get; set; }
        public long ISMS_Id { get; set; }
        public string ISMS_SubjectName { get; set; }
        public bool ESTSU_ElecetiveFlag { get; set; }
        public bool ECSTSU_ElectiveFlag { get; set; }
        public bool ECSTSU_ActiveFlg { get; set; }
        public Array viewstudentsubjectdetails { get; set; }
        public Array viewstudentattendancetails { get; set; }
        public string AMSE_SEMName { get; set; }

        public Array compExamarray { get; set; }

        public Array compSubarray { get; set; }
        public long AMCEXM_Id { get; set; }
        public bool subflg { get; set; }
        public Array compSubList { get; set; }
        public long? AMCEXMSUB_Id { get; set; }
        public Array compExamList { get; set; }
        public long PACA_Id { get; set; }
        public decimal ACSTCEMS_MaxMarks { get; set; }

        public Array studentCompDetails { get; set; }
        public Array attendanceArray { get; set; }
        public Array studentCompSubDetails { get; set; }
        public tempidlist1[] tempidlist { get; set; }

        public string htmldata { get; set; }
        public string IVRMMR_Name { get; set; }
        public string studpreviousstate { get; set; }

        public string statecode { get; set; }
        public int ID { get; set; }

        public string studperstate { get; set; }
        public string studconstate { get; set; }
        public string studconcountry { get; set; }
        public string studpercountry { get; set; }
        public string countrycode { get; set; }
        public string CategoryName { get; set; }
        public string yearname { get; set; }
        public string studentbranchname { get; set; }
        public Array studentcurrenrtbranch { get; set; }
        public Array admissioncongigurationList { get; set; }
        
        public Array CasteCategoryName { get; set; }
        public Array studentpercountry { get; set; }
        public Array studentconcountry { get; set; }
        public Array studentpasissuecountry { get; set; }
        public Array studentpreviouscountry { get; set; }
        public Array studentperstate { get; set; }
        public Array studentconstate { get; set; }
        public Array studentpreviousstate { get; set; }
        public Array studentcastecate { get; set; }
        public Array studentReligion { get; set; }
        public Array studentcourse { get; set; }
        public Adm_College_Student_CEMarksDTO[] Adm_College_Student_CEMarksDTO { get; set; }
        public Adm_College_Student_CEMarks_SubjectDTO[] Adm_College_Student_CEMarks_SubjectDTO { get; set; }
    }
    public class tempidlist1
    {
        public long AMCEXMSUB_Id { get; set; }
    }
    public class Adm_College_Student_CEMarks_SubjectDTO : CommonParamDTO
    {
        public long ACSTCEMS_Id { get; set; }
        public long AMCST_Id { get; set; }
        public long AMCEXM_Id { get; set; }
        
        public decimal ACSTCEMS_MaxMarks { get; set; }
        public decimal ACSTCEMS_SubjectMarks { get; set; }
        public bool ACSTCEMS_ActiveFlg { get; set; }
        public long AMCEXMSUB_Id { get; set; }
      
        
        public string AMCEXM_CompetitiveExams { get; set; }

        public string AMCEXMSUB_SubjectName { get; set; }

        public decimal? AMCEXMSUB_MaxMarks { get; set; }

    }
    public class Adm_College_Student_CEMarksDTO : CommonParamDTO
    {
        public long ACSTCEM_Id { get; set; }
        public long AMCST_Id { get; set; }
        public long AMCEXM_Id { get; set; }
       
        public string ACSTCEM_RegistrationId { get; set; }
        public string ACSTCEM_RollNo { get; set; }
        public string ACSTCEM_MeritNo { get; set; }
        public decimal ACSTCEM_TotalMaxMarks { get; set; }
        public decimal ACSTCEM_ObtdMarks { get; set; }
        public int ACSTCEM_ALLIndiaRank { get; set; }
        public int ACSTCEM_CategoryRank { get; set; }
        public decimal ACSTCEM_Percentage { get; set; }
        public decimal ACSTCEM_Percentile { get; set; }
        public bool ACSTCEM_ActiveFlg { get; set; }
        public string AMCEXM_CompetitiveExams { get; set; }

    }
    public class Adm_College_Student_SMSNoDTO
    {
        public long ACSTSMS_Id { get; set; }
        public long AMCST_Id { get; set; }
        public string AMCST_MobileNo { get; set; }
        public string ACSTSMS_CountryCode { get; set; }

      
    }
    public class Adm_College_Student_EmailIdDTO
    {
        public long ACSTE_Id { get; set; }
        public long AMCST_Id { get; set; }
        public string AMCST_emailId { get; set; }
    }
    public class Adm_M_Student_TempMobileNo
    {
        public long UserName { get; set; }
        public string Role { get; set; }
    }
    public class Adm_M_Student_TempEmailId
    {
        public long UserNameemail { get; set; }
        public string Roleemail { get; set; }
    }
    public class FatherMultipleMobileNoDTO
    {
        public long ACSTPMN_Id { get; set; }
        public long? AMCST_FatherMobleNo { get; set; }
        public long AMCST_Id { get; set; }
        public string ACSTPMN_Flag { get; set; }
        public string ACSTPMN_CountryCode { get; set; }
    }
    public class MotherMultipleMobileNoDTO
    {
        public long ACSTPMN_Id { get; set; }
        public long? AMCST_MotherMobleNo { get; set; }
        public long AMCST_Id { get; set; }
        public string ACSTPMN_Flag { get; set; }
        public string ACSTPMN_CountryCode { get; set; }
    }
    public class FatherMultipleEmailIdDTO
    {
        public long ACSTPE_Id { get; set; }
        public string AMCST_FatheremailId { get; set; }
        public long AMCST_Id { get; set; }
        public string ACSTPE_Flag { get; set; }
    }
    public class MotherMultipleEmailIdDTO
    {
        public long ACSTPE_Id { get; set; }
        public string AMCST_MotheremailId { get; set; }
        public long AMCST_Id { get; set; }
        public string ACSTPE_Flag { get; set; }
    }
    public class Adm_College_Student_SubjectMarksDTO
    {
        public long ACSTSUM_Id { get; set; }
        public long AMCST_Id { get; set; }
        public string ACSTSUM_SubjectName { get; set; }
        public decimal ACSTSUM_MaxMarks { get; set; }
        public decimal ACSTSUM_SubjectMarks { get; set; }
        public decimal ACSTSUM_Percentage { get; set; }
        public string ACSTSUM_LangFlg { get; set; }
    }

    public class retuen_student_DTO
    {
        public string returnmsg { get; set; }
        public bool returnval { get; set; }
        public long AMCST_Id { get; set; }
    }
    public class save_firsttab_details
    {
        public long AMCST_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public string AMCST_FirstName { get; set; }
        public string AMCST_MiddleName { get; set; }
        public string AMCST_LastName { get; set; }
        public DateTime? AMCST_Date { get; set; }
        public string AMCST_RegistrationNo { get; set; }
        public string AMCST_AdmNo { get; set; }
        public long? AMCOC_Id { get; set; }
        public long AMCO_Id { get; set; }
        public string AMCST_Sex { get; set; }
        public DateTime AMCST_DOB { get; set; }
        public string AMCST_DOBin_words { get; set; }
        public int? AMCST_Age { get; set; }
        public string AMCST_BloodGroup { get; set; }
        public string AMCST_MotherTongue { get; set; }
        public string AMCST_BirthCertNo { get; set; }
        public long? IVRMMR_Id { get; set; }
        public long? IMCC_Id { get; set; }
        public long? IMC_Id { get; set; }
        public string AMCST_StudentSubCaste { get; set; }
        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }
        public long ACMB_Id { get; set; }
        public string AMCST_BirthPlace { get; set; }
        public string AMCST_StuCasteCertiNo { get; set; }
        public string AMCST_StudentPhoto { get; set; }
        public long AMCST_Nationality { get; set; }
        public Adm_College_Student_SMSNoDTO[] Adm_College_Student_SMSNoDTO { get; set; }
        public Adm_College_Student_EmailIdDTO[] Adm_College_Student_EmailIdDTO { get; set; }
        public long? AMCST_AadharNo { get; set; }
        public long ACQ_Id { get; set; }
        public long ACQC_Id { get; set; }
        public long ACSS_Id { get; set; }
        public long ACST_Id { get; set; }
        public bool? Edit_flag { get; set; }
        public long AMCST_StuBankAccNo { get; set; }
        public bool? AMCST_BPLCardFlag { get; set; }
        public string AMCST_BPLCardNo { get; set; }
        public bool? AMCST_ECSFlag { get; set; }
        public long? AMCST_MobileNo { get; set; }
        public string AMCST_emailId { get; set; }
        public string AMCST_BiometricId { get; set; }
        public string AMCST_RFCardNo { get; set; }


        public Master_NumberingDTO transnumconfigsettings { get; set; }
        public Master_NumberingDTO admissionNumbering { get; set; }
        public string AMCST_Village { get; set; }
        public string AMCST_Taluk { get; set; }
        public string AMCST_District { get; set; }
        public string AMCST_Urban_Rural { get; set; }
        public bool? AMCST_Divyangjan { get; set; }
        public long ACMS_Id { get; set; }
        public string msgreturn { get; set; }
        public string Message { get; set; }
        public string Messagesection { get; set; }
        public string AMCST_StuBankIFSCCode { get; set; }
        public long ACYST_RollNo { get; set; }
        public string AMCST_CoutryCode { get; set; }
        
        public long? LoginId { get; set; }
        public Array attendanceArray { get; set; }
        //public decimal? AMCST_NEETMarks { get; set; }
        //public decimal? AMCST_JEEMainMarks { get; set; }
        //public decimal? AMCST_JEEAdvancedMarks { get; set; }


    }
}