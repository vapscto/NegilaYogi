﻿using DomainModel.Model.com.vapstech.admission;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.admission
{
    [Table("Adm_M_Student")]
    public class Adm_M_Student : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long AMST_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public string AMST_FirstName { get; set; }
        public string AMST_MiddleName { get; set; }
        public string AMST_LastName { get; set; }
        public DateTime AMST_Date { get; set; }
        public string AMST_RegistrationNo { get; set; }
        public string AMST_AdmNo { get; set; }
        public long? AMC_Id { get; set; }
        public string AMST_Sex { get; set; }
        public DateTime AMST_DOB { get; set; }
        public string AMST_DOB_Words { get; set; }
        public int? PASR_Age { get; set; }
        public long? ASMCL_Id { get; set; }
        public string AMST_BloodGroup { get; set; }
        public string AMST_MotherTongue { get; set; }
        public string AMST_BirthCertNO { get; set; }
        public long? IVRMMR_Id { get; set; }
        public long? IMCC_Id { get; set; }
        public long? IC_Id { get; set; }
        public string AMST_PerStreet { get; set; }
        public string AMST_PerArea { get; set; }
        public string AMST_PerCity { get; set; }
        public string AMST_PerAdd3 { get; set; }
        public long? AMST_PerState { get; set; }
        public long? AMST_PerDistrict { get; set; }
        public long? AMST_ConDistrict { get; set; }
        public long? AMST_PerCountry { get; set; }
        public int? AMST_PerPincode { get; set; }
        public string AMST_ConStreet { get; set; }
        public string AMST_ConArea { get; set; }
        public string AMST_ConCity { get; set; }
        public long? AMST_ConState { get; set; }
        public long? AMST_ConCountry { get; set; }
        public int? AMST_ConPincode { get; set; }
        public long? AMST_AadharNo { get; set; }
        public string AMCST_WalletPIN { get; set; }
        public string AMST_StuBankAccNo { get; set; }
        public string AMST_StuBankIFSC_Code { get; set; }
        public string AMST_StuCasteCertiNo { get; set; }
        public long AMST_MobileNo { get; set; }
        public string AMST_emailId { get; set; }
        public string AMST_FatherAliveFlag { get; set; }
        public string AMST_FatherName { get; set; }
        public long? AMST_FatherAadharNo { get; set; }
        public string AMST_FatherSurname { get; set; }
        public string AMST_FatherEducation { get; set; }
        public string AMST_FatherOccupation { get; set; }
        public string AMST_FatherOfficeAdd { get; set; }
        public string AMST_FatherDesignation { get; set; }
        public decimal? AMST_FatherMonIncome { get; set; }
        public decimal? AMST_FatherAnnIncome { get; set; }
        public long? AMST_FatherNationality { get; set; }
        public long? AMST_FatherMobleNo { get; set; }
        public string AMST_FatheremailId { get; set; }
        public string AMST_FatherBankAccNo { get; set; }
        public string AMST_FatherBankIFSC_Code { get; set; }
        public string AMST_FatherCasteCertiNo { get; set; }
        public string ANST_FatherPhoto { get; set; }
        public string AMST_MotherAliveFlag { get; set; }
        public string AMST_MotherName { get; set; }
        public long? AMST_MotherAadharNo { get; set; }
        public string AMST_MotherSurname { get; set; }
        public string AMST_MotherEducation { get; set; }
        public string AMST_MotherOfficeAdd { get; set; }
        public string AMST_MotherOccupation { get; set; }
        public string AMST_MotherDesignation { get; set; }
        public decimal? AMST_MotherMonIncome { get; set; }
        public decimal? AMST_MotherAnnIncome { get; set; }
        public long? AMST_MotherNationality { get; set; }
        public long? AMST_MotherMobileNo { get; set; }
        public string AMST_MotherEmailId { get; set; }
        public string AMST_MotherBankAccNo { get; set; }
        public string AMST_MotherBankIFSC_Code { get; set; }
        public string AMST_MotherCasteCertiNo { get; set; }
        public decimal? AMST_TotalIncome { get; set; }
        public string ANST_MotherPhoto { get; set; }
        public string AMST_BirthPlace { get; set; }
        public long? AMST_Nationality { get; set; }
        public int? AMST_BPLCardFlag { get; set; }
        public string AMST_BPLCardNo { get; set; }
        public int? AMST_HostelReqdFlag { get; set; }
        public int? AMST_TransportReqdFlag { get; set; }
        public int? AMST_GymReqdFlag { get; set; }
        public int? AMST_ECSFlag { get; set; }
        public int? AMST_PaymentFlag { get; set; }
        public int? AMST_AmountPaid { get; set; }
        public string AMST_PaymentType { get; set; }
        public DateTime? AMST_PaymentDate { get; set; }
        public string AMST_ReceiptNo { get; set; }
        public int? AMST_ActiveFlag { get; set; }
        public string AMST_ApplStatus { get; set; }
        public int? AMST_FinalpaymentFlag { get; set; }
        public string AMST_Photoname { get; set; }
        public string AMST_SOL { get; set; }
        public long? AMST_Noofbrothers { get; set; }
        public long? AMST_Noofsisters { get; set; }
        public string AMST_Father_Signature { get; set; }
        public string AMST_Father_FingerPrint { get; set; }
        public string AMST_Mother_Signature { get; set; }
        public string AMST_Mother_FingerPrint { get; set; }
        public long? AMST_Concession_Type { get; set; }
        public List<StudentActitvityDMO> activityDMOList { get; set; }
        public List<StudentReferenceDMO> referenceDMOList { get; set; }
        public List<StudentSourceDMO> sourceDMOList { get; set; }
        public List<MasterStudentBondDMO> masterbondDMOList { get; set; }
        public List<StudentSiblingDMO> siblingDMOList { get; set; }
        public StudentAchivementDMO achievementDMO { get; set; }
        public List<StudentPrevSchoolDMO> prevSchoolDMOList { get; set; }
        public List<StudentGuardianDMO> guardianDMOList { get; set; }
        public List<StudentDocumentDMO> documentDMOList { get; set; }
        public List<Adm_M_Student_FatherMobileNo> studentFatherMobileNo { get; set; }
        public List<Adm_Master_Father_Email> studentFatherEmail { get; set; }
        public List<Adm_M_Mother_MobileNo> studentMotherMobileNo { get; set; }
        public List<Adm_M_Mother_Emailid> studentMotherEmailid { get; set; }
        public List<Adm_M_Student_Email_Id> studentemailid { get; set; }
        public List<Adm_M_Student_MobileNo> studentmobileid { get; set; }
        public Adm_Student_EcsDetailsDMO Adm_Student_EcsDetailsDMO { get; set; }
        public School_Adm_Y_StudentDMO School_Adm_Y_StudentDMO { get; set; }
        public string AMST_SubCasteIMC_Id { get; set; }
        public string AMST_FatherReligion { get; set; }
        public string AMST_FatherCaste { get; set; }
        public string AMST_FatherSubCaste { get; set; }
        public string AMST_MotherReligion { get; set; }
        public string AMST_MotherCaste { get; set; }
        public string AMST_MotherSubCaste { get; set; }
        public string AMST_Tpin { get; set; }
        public string AMST_GovtAdmno { get; set; }
        public string AMST_StudentPANNo { get; set; }
        public string AMST_FatherPANNo { get; set; }
        public string AMST_MotherPANNo { get; set; }
        public string AMST_LanguageSpoken { get; set; }
        public string AMST_GPSTrackingId { get; set; }
        public string AMST_AppDownloadedDeviceId { get; set; }
        public int? AMST_NoOfElderBrothers { get; set; }
        public int? AMST_NoOfYoungerBrothers { get; set; }
        public int? AMST_NoOfElderSisters { get; set; }
        public int? AMST_NoOfYoungerSisters { get; set; }
        public long? ASMST_Id { get; set; }
        public string AMST_SecretCode { get; set; }
        public string AMST_BiometricId { get; set; }
        public string AMST_MOInstruction { get; set; }
        public string AMST_RFCardNo { get; set; }
        public string AMST_FatherChurchAffiliation { get; set; }
        public string AMST_MotherChurchAffiliation { get; set; }
        public bool? AMST_FatherSelfEmployedFlg { get; set; }
        public bool? AMST_MotherSelfEmployedFlg { get; set; }
        public DateTime? AMST_ChurchBaptisedDate { get; set; }
        public string AMST_Studentillness { get; set; }
        public string AMST_Illnessdetails { get; set; }
        public string AMST_AdmissionReason { get; set; }
        public string AMST_FatherPresentAddress { get; set; }
        public string AMST_FatherPresentCity { get; set; }
        public long? AMST_FatherPresentState { get; set; }
        public string AMST_FatherPresentPS { get; set; }
        public string AMST_FatherPresentPO { get; set; }
        public long? AMST_FatherPresentPinCode { get; set; }
        public string AMST_FatherPermanentAddress { get; set; }
        public string AMST_FatherPermanentCity { get; set; }
        public long? AMST_FatherPermanentState { get; set; }
        public string AMST_FatherPermanentPS { get; set; }
        public string AMST_FatherPermanentPO { get; set; }
        public long? AMST_FatherPermanentPinCode { get; set; }
        public string AMST_FatherMaritalStatus { get; set; }
        public string AMST_FatherBankName { get; set; }
        public string AMST_FatherBankBranch { get; set; }
        public string AMST_MotherPresentAddress { get; set; }
        public string AMST_MotherPresentCity { get; set; }
        public long? AMST_MotherPresentState { get; set; }
        public string AMST_MotherPresentPS { get; set; }
        public string AMST_MotherPresentPO { get; set; }
        public long? AMST_MotherPresentPinCode { get; set; }
        public string AMST_MotherPermanentAddress { get; set; }
        public string AMST_MotherPermanentCity { get; set; }
        public long? AMST_MotherPermanentState { get; set; }
        public string AMST_MotherPermanentPS { get; set; }
        public string AMST_MotherPermanentPO { get; set; }
        public long? AMST_MotherPermanentPinCode { get; set; }
        public string AMST_MotherMaritalStatus { get; set; }
        public string AMST_MotherBankName { get; set; }
        public string AMST_MotherBankBranch { get; set; }
        public string AMST_MaritalStatus { get; set; }
        public string AMST_LastPlayGrndAttnd { get; set; }
        public string AMST_ExtraActivity { get; set; }
        public string AMST_OtherResidential_Addr { get; set; }
        public string AMST_OtherPermanentAddr { get; set; }
        public string AMST_ChurchName { get; set; }
        public string AMST_ChurchAddress { get; set; }
        public long? AMST_MotherPassingYear { get; set; }
        public long? AMST_FatherPassingYear { get; set; }
        public string AMST_FatherOfficePhNo { get; set; }
        public string AMST_MotherOfficePhNo { get; set; }
        public string AMST_FatherHomePhNo { get; set; }
        public string AMST_MotherHomePhNo { get; set; }
        public string AMST_Tribe { get; set; }
        public string AMST_FatherTribe { get; set; }
        public string AMST_MotherTribe { get; set; }
        public string AMST_FirstLanguage { get; set; }
        public string AMST_SecondLanguage { get; set; }
        public string AMST_Thirdlanguage { get; set; }
        public string AMST_Boarding { get; set; }
        public string AMST_Town { get; set; }
        public string AMST_Taluk { get; set; }
        public string AMST_Distirct { get; set; }
        public string AMST_Village { get; set; }
        public long? AMST_PlaceOfBirthState { get; set; }
        public long? AMST_PlaceOfBirthCountry { get; set; }
        public string AMST_Stayingwith { get; set; }
        public bool? AMST_TransferrableJobFlg { get; set; }
        public int? AMST_UnderAge { get; set; }
        public int? AMST_OverAge { get; set; }
        public string AMST_BankName { get; set; }
        public string AMST_BranchName { get; set; }
        public string AMST_MedicalComplaints { get; set; }
        public string AMST_OtherInformations { get; set; }
        public bool? AMST_VaccinatedFlg { get; set; }
        public string AMST_SchoolDISECode { get; set; }
        public string AMST_Domicile { get; set; }
        public bool? AMST_Tcflag { get; set; }
        public int? AMST_NoOfSiblings { get; set; }
        public int? AMST_NoOfSiblingsSchool { get; set; }
        public int? AMST_NoOfDependencies { get; set; }
        public long? AMST_State { get; set; }
        public long? AMST_CreatedBy { get; set; }
        public long? AMST_UpdatedBy { get; set; }

        public string AMST_CoutryCode { get; set; }

        //public string AMST_PENNo { get; set; }
    }
}
