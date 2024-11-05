
using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace PreadmissionDTOs
{
    public class StudentApplicationDTO : CommonParamDTO
    {
        //public long pasR_Id { get; set; }
        public long pasR_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public string PASR_FirstName { get; set; }
        public string PASR_MiddleName { get; set; }
        public string PASR_LastName { get; set; }
        public string PASR_Date { get; set; }
        public string PASR_RegistrationNo { get; set; }
        public Array electivegrouplist { get; set; }
        public Preadmissionelectives[] elesubsubject ;
        public Array electivesubgrouplist  { get; set; }
        public Array DocumentList { get; set; }
        public Array caste_doc_maplist { get; set; }
        public long AMC_Id { get; set; }
        public string PASR_Sex { get; set; }
        public DateTime PASR_DOB { get; set; }
        public int? PASR_Age { get; set; }
        public long? ASMCL_Id { get; set; }
        public Array streamCEprint { get; set; }
        public string PASR_BloodGroup { get; set; }
        public string PASR_MotherTongue { get; set; }
        public long Religion_Id { get; set; }
        public Array classcategoryList { get; set; }
        public string applicationhtml { get; set; }
        public long? CasteCategory_Id { get; set; }
        public long? Caste_Id { get; set; }
        public string ASMST_StreamName { get; set; }
        public long? ASMST_Id { get; set; }
        public string ASMCE_CEName { get; set; }
        public string studentstream  { get; set; }
        public long? ASMCE_Id  { get; set; }
        public long? ASMCEId { get; set; }
        public string PASR_PerStreet { get; set; }
        public string PASR_PerArea { get; set; }
        public string PASR_PerCity { get; set; }
        public string PASR_PerState { get; set; }

        public long? PASR_ConDistrict { get; set; }
        public long? PASR_PerDistrict { get; set; }
        public string PASR_PerCountry { get; set; }
        public int PASR_PerPincode { get; set; }
        public string PASR_ConStreet { get; set; }
        public string PASR_ConArea { get; set; }
        public string PASR_ConCity { get; set; }
        public long PASR_ConState { get; set; }
        public string PASR_ConCountry { get; set; }
        public int PASR_ConPincode { get; set; }
        public long? PASR_AadharNo { get; set; }
        public long PASR_MobileNo { get; set; }
        public string PASR_emailId { get; set; }
        public string PASR_MaritalStatus { get; set; }
        public int PASR_FatherAliveFlag { get; set; }
        public string PASR_FatherName { get; set; }
        public long? PASR_FatherAadharNo { get; set; }
        public string PASR_FatherSurname { get; set; }
        public string PASR_FatherEducation { get; set; }
        public string PASR_FatherOccupation { get; set; }
        public string PASR_FatherDesignation { get; set; }
        public float? PASR_FatherIncome { get; set; }
        public long? PASR_FatherMobleNo { get; set; }
        public string PASR_FatheremailId { get; set; }
        public int PASR_MotherAliveFlag { get; set; }
        public string PASR_MotherName { get; set; }
        public long? PASR_MotherAadharNo { get; set; }
        public string PASR_MotherSurname { get; set; }
        public string PASR_MotherEducation { get; set; }
        public string PASR_MotherOccupation { get; set; }
        public string PASR_MotherDesignation { get; set; }
        public decimal? PASR_MotherIncome { get; set; }
        public long? PASR_MotherMobleNo { get; set; }
        public string PASR_MotheremailId { get; set; }
        public decimal? PASR_TotalIncome { get; set; }
        public string PASR_BirthPlace { get; set; }
        public string PASR_Nationality { get; set; }
        public int? PASR_HostelReqdFlag { get; set; }
        public int? PASR_TransportReqdFlag { get; set; }
        public int? PASR_GymReqdFlag { get; set; }
        public int? PASR_ECSFlag { get; set; }
        public int PASR_PaymentFlag { get; set; }
        public int? PASR_AmountPaid { get; set; }

        public Array StudentReferenceDetails { get; set; }
        public Array StudentSourceDetails { get; set; }

        public Array edit_StudentReferenceDetails { get; set; }
        public Array edit_StudentSourceDetails { get; set; }
        public string PASR_PaymentType { get; set; }
        public DateTime? PASR_PaymentDate { get; set; }
        public string PASR_ReceiptNo { get; set; }
        public int? PASR_ActiveFlag { get; set; }
        public string PASR_ApplStatus { get; set; }
        public int PASR_FinalpaymentFlag { get; set; }
        public bool PASA_TransferrableJobFlg { get; set; }
        public string PASR_Taluk { get; set; }
        public string PASR_Distirct { get; set; }
        public string PASR_Medium { get; set; }
        public string PASR_Village { get; set; }
        public Array syllabuslist { get; set; }
        public Array syllabuslistoth { get; set; }
        public Array streamexams  { get; set; }
        public Array streamexamsprint { get; set; }
        public string PASR_Stayingwith { get; set; }
        // New Fields
        public int PASR_UndertakingFlag { get; set; }
        public string PASR_MotherOfficeAddr { get; set; }
        public long? PASR_MotherNationality { get; set; }
        public string PASR_FatherOfficeAddr { get; set; }
        public long? PASR_FatherNationality { get; set; }
        public string PASR_OtherPermanentAddr { get; set; }
        public string PASR_OtherResidential_Addr { get; set; }
        public string PASR_ExtraActivity { get; set; }
        public string PASR_LastPlayGrndAttnd { get; set; }
        public long? trmA_Id { get; set; }
        public long? trmR_Idp { get; set; }
        public long? trmL_Idp { get; set; }
        // Guardian
        public long? PASRG_Id { get; set; }
        public string PASRG_GuardianName { get; set; }
        public string PASRG_GuardianAddress { get; set; }
        public long? PASRG_GuardianPhoneNo { get; set; }
        public string PASRG_emailid { get; set; }
        // Guardian ends
        // Document
        //public string PASMD_DocumentName { get; set; }
        // Document ends
        // Siblings
        public long? PASRS_Id { get; set; }
        public string PASRS_SiblingsName { get; set; }
        public string PASRS_SiblingsClass { get; set; }
        public int PASRS_SiblingsAdmissionNo { get; set; }
        public string PASRS_SiblingsSection { get; set; }
        // Sibling ends        
        // transport details
        public long? PASRT_Id { get; set; }
        public string PASRT_ivrmmcT_Id { get; set; }
        public long PASRT_cmR_Id { get; set; }
        public long PASRT_cmL_Id { get; set; }
        public long PASRT_consession_type_Id { get; set; }
        public int? PASRT_Daughter { get; set; }
        public int? PASRT_Son { get; set; }
        public int? PASRT_Heared_Friend_Colleague { get; set; }
        public int? PASRT_Internet { get; set; }
        public int? PASRT_Media { get; set; }
        public int? PASRT_Other { get; set; }
        //
        // Image
        public string PASMD_DocumentName { get; set; }
        public string PASMD_Path { get; set; }
        public long PASMD_Id { get; set; }
        public long PAMST_Id { get; set; }
        public string SearchData { get; set; }
        public int searchType { get; set; }      
        public ICollection<StateDTO> state { get; set; }
        public Array StudentReg_DTObj { get; set; }
        public Array StudentGuardian_DTObj { get; set; }
        public Array StudentPrevSch_DTObj { get; set; }
        public Array StudentSbling_DTObj { get; set; }
        public Array StudentSubjects_DTObj { get; set; }
        public Array StudentTrns_DTObj { get; set; }
        public Array Studentsource_DTObj  { get; set; }
        public Array studentEmploye { get; set; }
        //14-11-2016
        public Array registrationList { get; set; }
        // Adddedon 9-11-2016 to hold the message
        public string message { get; set; }
        // Adddedon 9-11-2016 to hold the message
        //
        public Array academicdrp { get; set; }
        public long Id { get; set; }
        public StudentSiblingDTO[] siblingsDetails;
        public long? PASR_AltContactNo { get; set; }
        public string PASR_AltContactEmail { get; set; }
        public string PASR_Student_Pic_Path { get; set; }
        public PreadmissionSchoolRegistrationDocumentsDTO[] selectedDocuments;
        public Array studentDocuments_DTObj { get; set; }
        public string PASR_BirthCertificateNo { get; set; }
        public long PASL_ID { get; set; }
        //Date: 21-12-2016
        public string remark { get; set; }
        public long Repeat_Class_Id { get; set; }
        public string className { get; set; }
        public string statusName { get; set; }
        public string statusFlag { get; set; }
        // MasterConfigurations
        public Array fillclass { get; set; }
        public long? PASR_Noofbrothers { get; set; }
        public long? PASR_Noofsisters { get; set; }
        public string PASR_lastclassperc { get; set; }
        public StudentPrevSchoolDTO[] PreviousSchoolList;
        public bool? PASR_SibblingConcessionFlag { get; set; }
        public bool? PASR_ParentConcessionFlag { get; set; }
        public Master_NumberingDTO transnumconfigsettings { get; set; }
        public long? FMCC_ID { get; set; }
        public Array studentDetailsTEmp { get; set; }
        public MasterConfigurationDTO configurationsettings { get; set; }
        public int payementcheck { get; set; }
        public Array paydet { get; set; }
        public bool? PASR_Adm_Confirm_Flag { get; set; }
        public Array studentCategory { get; set; }
        public string admclassCapacity { get; set; }
        public string admflag { get; set; }
        public DateTime dateString { get; set; }
        public string ASMCL_ClassName { get; set; }
        public Array areaList { get; set; }
        public Array routelist { get; set; }
        public string studentarea { get; set; }
        public string studentroue  { get; set; }
        public string studentlocation { get; set; }
        public string studentsource { get; set; }
        public string studenconncessioncat  { get; set; }
        public Array studentareaList { get; set; }
        public Array studentroutelist{ get; set; }
        public Array locationlist { get; set; }
        public Array sourcedropDown { get; set; }
        public long ASMAYid { get; set; }
        public string IVRMMR_Name { get; set; }
        public string IMC_CasteName { get; set; }
        public string IMCC_CategoryName { get; set; }
        public string PASR_PerStaten { get; set; }
        public string PASR_PerCountryn { get; set; }
        public string PASR_ConStaten { get; set; }
        public string PASR_ConCountryn { get; set; }
        public string studentnationality { get; set; }
        public string fathernationality { get; set; }
        public string mothernationality { get; set; }
        public Array studentClass { get; set; }
        public Array studentReligion { get; set; }
        public Array studentcaste { get; set; }
        public Array fatherreligion { get; set; }
        public Array fathercaste { get; set; }
        public Array fathersubcaste { get; set; }
        public Array mothersubcaste { get; set; }
        public Array subcaste  { get; set; }
        public Array sylabusss { get; set; }
        public Array motherreligion { get; set; }
        public Array mothercaste { get; set; }
        public Array studentcastecategory { get; set; }
        public Array studentperstate { get; set; }
        public Array studentpercountry { get; set; }
        public Array studentconstate { get; set; }
        public Array studentconcountry { get; set; }
        public Array studentnationalitys { get; set; }
        public Array fathernationalitys { get; set; }
        public Array mothernationalitys { get; set; }
        public Array concessioncategory  { get; set; }
        public Array studenthelthDTO { get; set; }
        public Array vaccines { get; set; }
        public Master_NumberingDTO transnumbconfigurationsettingsss { get; set; }
        public DateTime Dateofb { get; set; }
        public long asmcl_id { get; set; }
        public string asmcl_name { get; set; }
        public Array Class_list { get; set; }
        public Array Class_list2 { get; set; }
        public DateTime? _cut_of_date1 { get; set; }
        public Array fill_asy { get; set; }
        public long PASRAPS_ID { get; set; }
        public string remarks { get; set; }
        public long PAMS_Id { get; set; }
        public long PAMSId { get; set; }
        public Array transnumconfig { get; set; }
        public string PASR_DOBWords { get; set; }
        public DateTime maxdate { get; set; }
        public DateTime mindate { get; set; }
        public Array prospectusPaymentlist { get; set; }
        public string editflag { get; set; }
        public bool concessionconfirm { get; set; }
        public string PASRSTUL_MAACAdd { get; set; }
        public string PASRSTUL_IPAdd { get; set; }
        public string PASRSTUL_NetIp { get; set; }
        public string htmldata { get; set; }
        public string dashboardpage { get; set; }
        public string PASRPS_Address { get; set; }
        public string PASR_ChurchName { get; set; }
        public string PASR_ChurchAddress { get; set; }
        public int? PASR_MotherPassingYear { get; set; }
        public int? PASR_FatherPassingYear { get; set; }
        public string PASR_FatherOfficePhNo { get; set; }
        public string PASR_MotherOfficePhNo { get; set; }
        public string PASR_FatherHomePhNo { get; set; }
        public string PASR_MotherHomePhNo { get; set; }
        public string PASRG_GuardianRelation { get; set; }
        public string paymentapplicable { get; set; }
        public string PASR_Languagespeaking { get; set; }
        public string PASR_FatherPanno { get; set; }
        public string PASR_MotherPanno { get; set; }
        public int? PASR_FatherReligion { get; set; }
        public int? PASR_FatherCaste { get; set; }
        public int? PASR_MotherReligion { get; set; }
        public int? PASR_MotherCaste { get; set; }
        public string PASR_Tribe { get; set; }
        public string PASR_FatherTribe { get; set; }
        public string PASR_MotherTribe { get; set; }
        public string PASR_FirstLanguage { get; set; }
        public string PASR_SecondLanguage { get; set; }
        public string PASR_Thirdlanguage { get; set; }
        public string PASR_FatherPhoto { get; set; }
        public string PASR_MotherPhoto { get; set; }
        public string PASRG_Occupation { get; set; }
        public long? PASRG_PhoneOffice { get; set; }
        public Array fillstudent { get; set; }
        public string multiplegroups { get; set; }
        public bool Extraforms { get; set; }
        public string concessioncats { get; set; }
        public string PASR_Emisno { get; set; }
      public string PASR_Boarding { get; set; }
        public long roleid { get; set; }
        public bool roletypefind { get; set; }
        public string PASR_Fathersubcaste { get; set; }
        public string PASR_Mothersubcatse { get; set; }
        public string PASR_Subcaste { get; set; }
        public string manualAdmFlag { get; set; }
        public string Usercreatonflag { get; set; } 
        public string ApplicationNo { get; set; }      
        public string PASR_Applicationno { get; set; }
        public int IVRMGC_Healthapp { get; set; }
        public bool healthflag { get; set; }
        //srkvs deepak
        public Array prospectusDatalist { get; set; }
        public long PASP_Id { get; set; }
        public string PASP_ProspectusNo { get; set; }
        public Array mstConfig { get; set; }
        public Array prospectuslist { get; set; }
        public int PASR_UnderAge { get; set; }
        public int PASR_OverAge { get; set; }
        public string PASR_AccountNo  { get; set; }
        public string PASR_BankName { get; set; }
        public string PASR_BranchName { get; set; }
        public string PASR_IFSCCode { get; set; }
        public string PASR_Domicile { get; set; }
        public string PASR_SchoolDISECode { get; set; }
        public string PASR_MedicalComplaints { get; set; }
        public string PASR_OtherInformations  { get; set; }
        public bool PASR_NewlyAdmisstedFlg { get; set; }
        public bool PASR_VaccinatedFlg  { get; set; }
        public bool PASR_Tcflag  { get; set; }
        public int? PASR_NoOfSiblings { get; set; }
        public int? PASR_NoOfSiblingsSchool { get; set; }
        public int? PASR_NoOfElderBrothers { get; set; }
        public int? PASR_NoOfYoungerBrothers { get; set; }
        public int? PASR_NoOfElderSisters { get; set; }
        public int? PASR_NoOfYoungerSisters { get; set; }
        public int? PASR_NoOfDependencies { get; set; }
        public string onlinepaygteway { get; set; }
        public Array fillstaff { get; set; }
        public long? Staffid { get; set; }
        public Array admissioncatdrp { get; set; }
        public string stusername { get; set; }
        public bool updateform { get; set; }
        public int? offon { get; set; }
        public bool ASSTCLCE_CompulsoryFlg { get; set; }
        public int ASMST_Order { get; set; }
        public DateTime? PASR_ChurchBaptisedDate { get; set; }
        public string PASR_FatherChurchAffiliation { get; set; }
        public string PASR_MotherChurchAffiliation { get; set; }
        public bool PASR_FatherSelfEmployedFlg { get; set; }
        public bool PASR_MotherSelfEmployedFlg { get; set; }
        public bool PASRG_FeeUndertakeFlg { get; set; }

        public string PASR_CatseCertificateNo { get; set; }
        public string PASR_Illnessdetails { get; set; }
        public string PASR_AdmissionReason { get; set; }
        public string PASR_FatherPresentAddress { get; set; }
        public string PASR_FatherPresentCity { get; set; }
        public string PASR_FatherPresentPS { get; set; }
        public string PASR_FatherPresentPO { get; set; }
        public string PASR_FatherPermanentAddress { get; set; }
        public string PASR_FatherPermanentCity { get; set; }
        public string PASR_FatherPermanentPS { get; set; }
        public string PASR_FatherPermanentPO { get; set; }
        public string PASR_FatherMaritalStatus { get; set; }
        public string PASR_FatherBankName { get; set; }
        public string PASR_FatherBankBranch { get; set; }
        public string PASR_FatherIFSC { get; set; }
        public string PASR_MotherPresentAddress { get; set; }
        public string PASR_MotherPresentCity { get; set; }
        public string PASR_MotherPresentPS { get; set; }
        public string PASR_MotherPresentPO { get; set; }
        public string PASR_MotherPermanentAddress { get; set; }
        public string PASR_MotherPermanentCity { get; set; }
        public string PASR_MotherPermanentPS { get; set; }
        public string PASR_MotherPermanentPO { get; set; }
        public string PASR_MotherBankName { get; set; }
        public string PASR_MotherBankBranch { get; set; }
        public string PASR_MotherIFSC { get; set; }
        public string PASR_PovertyLine { get; set; }
        public string PASR_JointPhoto { get; set; }
        public long? PASR_FatherPresentState { get; set; }
        public long? PASR_FatherPresentPinCode { get; set; }
        public long? PASR_FatherPermanentState { get; set; }
        public long? PASR_FatherPermanentPinCode { get; set; }
        public long? PASR_FatherBankAccount { get; set; }
        public long? PASR_MotherPresentState { get; set; }
        public long? PASR_MotherPresentPinCode { get; set; }
        public long? PASR_MotherPermanentState { get; set; }
        public long? PASR_MotherPermanentPinCode { get; set; }
        public long? PASR_MotherBankAccount { get; set; }
        public decimal? PASR_MotherAnnIncome { get; set; }
        public decimal? PASR_FatherAnnIncome { get; set; }
        public decimal? PASR_FatherMonIncome { get; set; }
        public decimal? PASR_MotherMonIncome { get; set; }
        public bool? PASR_Studentillness { get; set; }
        public string PASR_MotherMaritalStatus { get; set; }
        public string PASR_NickName { get; set; }
        public string PASR_FatherHobbies { get; set; }
        public string PASR_MotherHobbies { get; set; }
        public string PASR_NeighbourPhoneNo { get; set; }
        public string PASR_NeighbourAddr1 { get; set; }
        public string PASR_NeighbourAddr2 { get; set; }
        public string PASR_PhysicalDisability { get; set; }

        public string PASR_NeighbourName { get; set; }

        public bool concessionconfirmsibling { get; set; }
        public Array referenceIds { get; set; }
        public Array sourceIds { get; set; }
  
        public PA_StudentReferenceDTO[] SelectedRefrenceDetails { get; set; }
        public PA_StudentSourceDTO[] SelectedSourceDetails { get; set; }

        public ICollection<IFormFile> File { get; set; }

    }

  
    public class classdto
    {
        public long ASMCL_Id { get; set; }
        public string ASMCL_ClassName { get; set; }
        public int ASMCL_Order { get; set; }
        public long MI_Id { get; set; }
        public int ASMCL_MinAgeYear { get; set; }
        public int ASMCL_MinAgeMonth { get; set; }
        public int ASMCL_MinAgeDays { get; set; }
        public int ASMCL_MaxAgeYear { get; set; }
        public int ASMCL_MaxAgeMonth { get; set; }
        public int ASMCL_MaxAgeDays { get; set; }
        public int ASMCL_MaxCapacity { get; set; }
        public bool ASMCL_ActiveFlag { get; set; }
        public int ASMCL_PreadmFlag { get; set; }
        public DateTime maxdate { get; set; }
        public DateTime mindate { get; set; }       

    }   
}
