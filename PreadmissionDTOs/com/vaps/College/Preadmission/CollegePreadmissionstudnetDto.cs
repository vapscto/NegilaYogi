using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.College.Preadmission
{
    public class CollegePreadmissionstudnetDto : CommonParamDTO
    {
        public long PACA_Id { get; set; }
        public long MI_Id { get; set; }
        public long userid { get; set; }
        public long ASMAY_Id { get; set; }
        public string PACA_FirstName { get; set; }
        public string PACA_MiddleName { get; set; }
        public string PACA_LastName { get; set; }
        public DateTime PACA_Date { get; set; }
        public Array mediumlist { get; set; }
        public Array statuslist { get; set; }
        public Array achievementdata { get; set; }
        public Array subjectlist { get; set; }
        public Array subjectlistlag { get; set; }
        public Array studentpreviouscountry { get; set; }
        public long? PACA_FatherOfficeAddPincode { get; set; }
        public long? PACA_FatherResidentialAddPinCode { get; set; }
        public long? PACA_MotherOfficeAddPinCode { get; set; }
        public long? PACA_MotherResidentialAddPinCode { get; set; }
        public Array activitydetails { get; set; }
        public Array studentpasissuecountry { get; set; }
        public string status_type { get; set; }
        public long PAMST_Id { get; set; }
        public int ID { get; set; }
        public string PACA_RegistrationNo { get; set; }
        public string PACA_ApplicationNo { get; set; }
        public long AMC_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public string PACA_Sex { get; set; }
        public DateTime PACA_DOB { get; set; }
        public string PACA_DOB_inwords { get; set; }
        public string PACA_PhysicallyChallenged { get; set; }
        public long PACA_Age { get; set; }
        public string PACA_BloodGroup { get; set; }
        public string PACA_MotherTongue { get; set; }
        public string PACA_BirthCertNo { get; set; }
        public long IVRMMR_Id { get; set; }
        public long IMCC_Id { get; set; }
        public bool? PACA_IncomeCertificateFlg { get; set; }
        public string onlinepaygteway { get; set; }
        public long IMC_Id { get; set; }
        public Array mstConfig { get; set; }
        public long PACA_AdmStatus { get; set; }
        public string PACA_StudentSubCaste { get; set; }
        public string PACA_PerStreet { get; set; }
        public string PACA_PerArea { get; set; }
        public string PACA_PerCity { get; set; }
        public string PACA_PerAdd3 { get; set; }
        public long PACA_PerState { get; set; }
        public long IVRMMC_Id { get; set; }
        public long PACA_PerPincode { get; set; }
        public string PACA_ConStreet { get; set; }
        public string PACA_ConArea { get; set; }
        public string PACA_ConAdd3 { get; set; }
        public string PACA_ConCity { get; set; }
        public string PACA_Village { get; set; }
        public string PACA_Taluk { get; set; }
        public string PACA_District { get; set; }
        public long PACA_ConState { get; set; }
        public long PACA_ConCountryId { get; set; }
        public long PACA_ConPincode { get; set; }
        public long PACA_AadharNo { get; set; }
        public long PACA_StuBankAccNo { get; set; }
        public long PACA_StuBankIFSCCode { get; set; }
        public string PACA_StuCasteCertiNo { get; set; }
        public long PACA_MobileNo { get; set; }
        public string PACA_emailId { get; set; }
        public string PACA_AlterNativeEmialId { get; set; }
        public bool PACA_FatherAliveFlag { get; set; }
        public string PACA_FatherName { get; set; }
        public long PACA_FatherAadharNo { get; set; }
        public string PACA_FatherSurname { get; set; }
        public string PACA_FatherEducation { get; set; }
        public string PACA_FatherOccupation { get; set; }
        public string PACA_FatherOfficeAdd { get; set; }
        public string PACA_FatherDesignation { get; set; }
        public decimal PACA_FatherMonIncome { get; set; }
        public decimal PACA_FatherAnnIncome { get; set; }
        public string PACA_FatherNationality { get; set; }
        public long PACA_FatherReligion { get; set; }
        public long PACA_FatherCaste { get; set; }
        public long PACA_FatherSubCaste { get; set; }
        public long PACA_FatherMobleNo { get; set; }
        public string PACA_FatherEmailId { get; set; }
        public long PACA_FatherBankAccNo { get; set; }
        public long PACA_FatherBankIFSCCode { get; set; }
        public string PACA_FatherCasteCertiNo { get; set; }
        public string PACA_FatherPhoto { get; set; }
        public string PACA_FatherSign { get; set; }
        public string PACA_FatherFingerprint { get; set; }
        public bool PACA_MotherAliveFlag { get; set; }
        public string PACA_MotherName { get; set; }


        public string PACA_FatherOfficeTelPhno { get; set; }
        public string PACA_FatherResTelPhno { get; set; }
        public string PACA_FatherResAdd { get; set; }
       

        public string PACA_MotherOfficeTelPhno { get; set; }
        public string PACA_MotherResTelPhno { get; set; }

        public string PACA_MotherOfficeAdd { get; set; }

        public string PACA_MotherResAdd { get; set; } 
        public string precutdate { get; set; }
        public long PACA_MotherAadharNo { get; set; }
        public string PACA_MotherSurname { get; set; }
        public string PACA_MotherEducation { get; set; }
        public string PACA_MotherOccupation { get; set; }
        public string PACSTPS_Result { get; set; }
        public string PACA_MotherDesignation { get; set; }
        public Decimal PACA_MotherMonIncome { get; set; }
        public Decimal PACA_MotherAnnIncome { get; set; }
        public string PACA_MotherNationality { get; set; }
        public long PACA_MotherReligion { get; set; }
        public long PACA_MotherCaste { get; set; }
        public long PACA_MotherSubCaste { get; set; }
        public long PACA_MotherMobleNo { get; set; }
        public string PACA_MotherEmailId { get; set; }
        public long PACA_MotherBankAccNo { get; set; }
        public long PACA_MotherBankIFSCCode { get; set; }
        public string PACA_MotherCasteCertiNo { get; set; }
        public Decimal PACA_TotalIncome { get; set; }
        public string PACA_MotherSign { get; set; }
        public string PACA_MotherPhoto { get; set; }
        public string PACA_MotherFingerprint { get; set; }
        public string PACA_BirthPlace { get; set; }
        public string PACA_Nationality { get; set; }
        public bool PACA_BPLCardFlag { get; set; }
        public string PACA_BPLCardNo { get; set; }
        public bool PACA_HostelReqdFlag { get; set; }
        public bool PACA_TransportReqdFlag { get; set; }
        public bool PACA_GymReqdFlag { get; set; }
        public bool PACA_ECSFlag { get; set; }
        public bool PACA_PaymentFlag { get; set; }
        public Decimal PACA_AmountPaid { get; set; }
        public string PACA_PaymentType { get; set; }
        public DateTime? PACA_PaymentDate { get; set; }
        public string PACA_ReceiptNo { get; set; }
        public string PACA_EMSINo { get; set; }
        public string PACA_ApplStatus { get; set; }
        public bool PACA_FinalpaymentFlag { get; set; }
        public string PACA_StudentPhoto { get; set; }
        public string PACA_StudentSign { get; set; }
        public string PACA_StudentFingerprint { get; set; }
        public string PACA_NoofSiblingsSchool { get; set; }
        public long PACA_NoofSiblings { get; set; }
        public long PACA_NoOfBrothers { get; set; }
        public long PACA_NoOfSisters { get; set; }
        public long PACA_NoofDependencies { get; set; }
        public string PACA_TPINNO { get; set; }
        public string PACA_MedOfInstruction { get; set; }
        public long ACST_Id { get; set; }
        public long ACSS_Id { get; set; }
        public string PACA_PassportNo { get; set; }
        public string PACA_PassportIssuedAt { get; set; }
        public DateTime? PACA_PassportIssueDate { get; set; }
        public long PACA_PassportIssuedCounrty { get; set; }
        public string PACA_PassportIssuedPlace { get; set; }
        public string PACA_Place { get; set; }
        public DateTime? PACA_PassportExpiryDate { get; set; }
        public string PACA_VISAIssuedBy { get; set; }
        public string PACA_VISANo { get; set; }
        public string PACA_VISAValidFrom { get; set; }
        public string PACA_VISAValidTo { get; set; }
        public string yearname { get; set; }
        public bool PACA_ActiveFlag { get; set; }

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
        public Array StudentGuardianDetails { get; set; }

        public Array Studentsubjectmarksarry { get; set; }
        public Array StudentSiblingDetails { get; set; }
        public Array StudentReferenceDetails { get; set; }
        public Array StudentSourceDetails { get; set; }
        public Array DocumentList { get; set; }
        public Array combinationlist { get; set; }

        public Master_NumberingDTO transnumconfigsettings { get; set; }
        public Master_NumberingDTO admissionNumbering { get; set; }
     
        
        public subiddto[] langsubids { get; set; }
        public subiddto[] optsubids { get; set; }

        public string message { get; set; }

        public string pay  { get; set; }

        public string ApplicationNo { get; set; }

        public string manualAdmFlag { get; set; }
        public long AMSMD_Id { get; set; }

        public long remarkcount { get; set; }

        public CollegeStudentPrevSchoolDTO[] SelectedPrevSchoolDetails { get; set; }

        public CollegeStudentGuardianDTO[] SelectedGuardianDetails { get; set; }

        public string countryName { get; set; }

        public CollegeDocumentDTO[] Uploaded_documentList { get; set; }

        public string courseName { get; set; }

        public string branchName { get; set; }

        public string semesterName { get; set; }

        public long AMCOC_Id { get; set; }
        public string AMCOC_Name { get; set; }

        public string IMC_CasteName { get; set; }

        public string studpreviousstate { get; set; }
        public Array studentcourse { get; set; }

        public string AMCO_CourseName { get; set; }

        public string IVRMMR_Name { get; set; }

        public Array studentReligion { get; set; }

        public Array studentcastecate { get; set; }

        public Array studentpreviousstate { get; set; }

        public Array studentperstate { get; set; }

        public Array studentconstate { get; set; }

        public string studperstate { get; set; }

        public string studconstate { get; set; }

        public Array studentpercountry { get; set; }
        public Array CasteCategoryName { get; set; }

        public Array studentpreffredbranch { get; set; }

        public Array studentcurrenrtbranch { get; set; }
        public Array admissioncongigurationList { get; set; }
        
        public Array studentconcountry { get; set; }

        public string studpercountry { get; set; }

        public string studconcountry { get; set; }

        public Array Adm_College_Student_SubjectMarksDTO { get; set; }

        public Adm_College_Student_SubjectMarksDTO[] Adm_College_Student_SubjectMarksTempDTO { get; set; }
        public PA_College_Student_PrevExtracurricularDTO[] activitydto { get; set; }
        public sportsachievementdto[] achievedto { get; set; }
        public PA_College_Student_CBPreferenceDTO[] PA_College_Student_CBPreference { get; set; }
        public string countrycode { get; set; }

        public string statecode { get; set; }
        public string CategoryName { get; set; }

        public string branchname { get; set; }

        public string studentbranchname { get; set; }

        public int? PACA_CompleteFillflag { get; set; }

        public DateTime ASMAY_PreAdm_F_Date { get; set; }

        public string roleName { get; set; }

        public long roleId { get; set; }

        public bool countrole { get; set; }
        public Array academicdrp { get; set; }

        public long ACAYC_Id { get; set; }

        public MasterConfigurationDTO configurationsettings { get; set; }

        public int configurationsettings1 { get; set; }

        public int payementcheck { get; set; }

        public Array paydet { get; set; }

        public Master_NumberingDTO transnumbconfigurationsettingsss { get; set; }

        public long FCMAS_Id { get; set; }
        public decimal FCMAS_Amount { get; set; }

        public Array groupwisefmaids { get; set; }
        public long FMH_Id { get; set; }

        public Array prospectusPaymentlist { get; set; }

        public long ACAYCB_Id { get; set; }

        public long AMSE_Id { get; set; }

        //added by roopa

        public Array fillyear { get; set; }
        public Array fillcourse { get; set; }
        public DateTime prestartdate { get; set; }

        public Array totalcountDetails { get; set; }

        public int academicorder { get; set; }
        public DateTime? academicyearstratdate { get; set; }
        public long acedemicyear { get; set; }

        public string From_Date { get; set; }
        public string To_Date { get; set; }
        public int ReportType { get; set; }
        public string type { get; set; }
        public Array SearchstudentDetails { get; set; }

        public DateTime presenddate { get; set; }
        public int count { get; set; }

        public string PACA_Statusremark { get; set; }

        public string statusName { get; set; }
        public string statusFlag { get; set; }

        public long PACSTD_Id { get; set; }

        public string ACSTD_Doc_Path { get; set; }

        public string AMSMD_DocumentName { get; set; }

        

        public Array allcourse { get; set; }
        public Array registrationList { get; set; }

        public Array studentDetailsHelth { get; set; }

        public CollegePreadmissionstudnetDto[] ddoc { get; set; }
        public Array docdownload { get; set; }
        public Array admissioncatdrp { get; set; }

        public Array admissioncatdrpall { get; set; }

        public string htmldata { get; set; }

        public string AMB_BranchName { get; set; }
        public string AMSE_SEMName { get; set; }
        public courselsttwo1[] courselsttwo { get; set; }

        public branchlisttwo1[] branchlisttwo { get; set; }

        public semesterlisttwo1[] semesterlisttwo { get; set; }

        public Array branchlist { get; set; }
        public Array semesterlist { get; set; }

        public long? ACQC_Id { get; set; }
        public long? ACQ_Id { get; set; }
        public Decimal PACSTCEMS_MaxMarks { get; set; }
        public Array compExamarray { get; set; }

        public Array compSubarray { get; set; }
        public long PAMCEXM_Id { get; set; }
        public bool subflg { get; set; }
        public Array compSubList { get; set; }
        public long? PAMCEXMSUB_Id { get; set; }
        public Array compExamList { get; set; }
        public Array studentCompDetails { get; set; }
        public Array studentCompSubDetails { get; set; }
        public string fee_status { get; set; }
        public Array doclist { get; set; }
        public Array remarkslist { get; set; }
        
        public Array fillpaymentgateway { get; set; }

        public string PACA_CoutryCode { get; set; }
        public string PACA_MotherCountryCode { get; set; }
        public string PACA_FatherCountryCode { get; set; }
        public string PACSTG_CoutryCode { get; set; }
        public string PACA_Urban_Rural { get; set; }
        
        public bool PAMCEXM_CompulsoryFlg { get; set; }
        public tempidlist1[] tempidlist { get; set; }

        public PA_College_Student_CEMarksClgDTO[] PA_College_Student_CEMarksClgDTO { get; set; }
        public PA_College_Student_CEMarks_SubjectClgDTO[] PA_College_Student_CEMarks_SubjectClgDTO { get; set; }

        //

    }

  
    public class tempidlist1
    {
        public long PAMCEXMSUB_Id { get; set; }
    }
    public class PA_College_Student_CEMarks_SubjectClgDTO : CommonParamDTO
    {
        public long PACSTCEM_Id { get; set; }
        public long PACSTCEMS_Id { get; set; }
        public long PACA_Id { get; set; }
        public long PAMCEXM_Id { get; set; }
        public long PAMCEXMSUB_Id { get; set; }
        public Decimal PACSTCEMS_MaxMarks { get; set; }
        public Decimal PACSTCEMS_SubjectMarks { get; set; }
        public bool PACSTCEMS_ActiveFlg { get; set; }
        public string PAMCEXM_CompetitiveExams { get; set; }

        public string PAMCEXMSUB_SubjectName { get; set; }

        public Decimal? PAMCEXMSUB_MaxMarks { get; set; }

    }
    public class PA_College_Student_CEMarksClgDTO : CommonParamDTO
    {
        public long PACSTCEM_Id { get; set; }
        public long PACA_Id { get; set; }
        public long PAMCEXM_Id { get; set; }
        public string PACSTCEM_RegistrationId { get; set; }
        public string PACSTCEM_RollNo { get; set; }
        public string PACSTCEM_MeritNo { get; set; }
        
        public int? PACSTCEM_ALLIndiaRank { get; set; }
        public int? PACSTCEM_CategoryRank { get; set; }
        public Decimal PACSTCEM_TotalMaxMarks { get; set; }
        public Decimal PACSTCEM_ObtdMarks { get; set; }
        public Decimal PACSTCEM_Percentile { get; set; }
        public Decimal PACSTCEM_Percentage { get; set; }
        public bool PACSTCEM_ActiveFlg { get; set; }
        public string PAMCEXM_CompetitiveExams { get; set; }

    }
    public class courselsttwo1
    {
        public long AMCO_Id { get; set; }
      

    }
    public class branchlisttwo1
    {
        public long AMB_Id { get; set; }


    }
    public class semesterlisttwo1
    {
        public long AMSE_Id { get; set; }


    }
    public class PA_College_Student_PrevExtracurricularDTO 
    {
        public long PACSPER_Id { get; set; }
        public long  PACA_Id { get; set; }
        public string PACSPER_Type { get; set; }
        public string PACSPER_ActivityName { get; set; }

    }

    public class Adm_College_Student_SubjectMarksDTO : CommonParamDTO
    {
        public long PACSTSUM_Id { get; set; }
        public long PACA_Id { get; set; }
        public string PACSTSUM_SubjectName { get; set; }
        public decimal PACSTSUM_MaxMarks { get; set; }
        public decimal? PACSTSUM_MinMarks { get; set; }
        public decimal PACSTSUM_SubjectMarks { get; set; }
        public decimal PACSTSUM_Percentage { get; set; }
        public string PACSTSUM_LangFlg { get; set; }

     

    }

    public class PA_College_Student_CBPreferenceDTO : CommonParamDTO
    {
        public long PACSTCBO_Id { get; set; }
        public long PACA_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public int PACSTCBO_Order { get; set; }
        public bool PACSTCBO_ActiveFlg { get; set; }


    }
    public class sportsachievementdto 
    {
        public long PACA_Id { get; set; }
        public string PACSAT_AchivementsName { get; set; }
        public string PACSAT_type { get; set; }
        public string PACSAT_Filename { get; set; }
        public string PACSAT_Filepath { get; set; }


    }
    public class subiddto 
    {
        public long ISMS_Id { get; set; }
   

    }
}
