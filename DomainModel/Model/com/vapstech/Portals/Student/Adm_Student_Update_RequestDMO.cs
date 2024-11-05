using DomainModel.Model.com.vapstech.admission;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Portals.Student
{
    [Table("Adm_Student_Update_Request")]
    public class Adm_Student_Update_RequestDMO 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ASTUREQ_Id { get; set; }
        public long ASTUREQ_ConformBy { get; set; }
  public long MI_Id { get; set; }
 public long AMST_Id { get; set; }
 public string ASTUREQ_FirstName { get; set; }
 public string ASTUREQ_ReqStatus { get; set; }
public string ASTUREQ_MiddleName { get; set; }
public string ASTUREQ_LastName { get; set; }
public DateTime ASTUREQ_Date { get; set; }
        public string ASTUREQ_RegistrationNo { get; set; }
public string ASTUREQ_AdmNo { get; set; }
public long? AMC_Id { get; set; }
public string ASTUREQ_Sex { get; set; }
public DateTime? ASTUREQ_DOB { get; set; }
        public string ASTUREQ_DOBinwords { get; set; }
public long? ASTUREQ_Age { get; set; }
public long? ASMCL_Id { get; set; }
public string ASTUREQ_BloodGroup { get; set; }
public long? ASMST_Id { get; set; }
public string ASTUREQ_MotherTongue { get; set; }
public string ASTUREQ_HomeLaguage { get; set; }
public string ASTUREQ_BirthCertNo { get; set; }
public long? IVRMMR_Id { get; set; }
public long? IMCC_Id { get; set; }
public long? IMC_Id { get; set; }
public string ASTUREQ_StudentSubCaste { get; set; }
public string ASTUREQ_PerStreet { get; set; }
public string ASTUREQ_PerArea { get; set; }
public string ASTUREQ_PerCity { get; set; }
public string ASTUREQ_PerAdd3 { get; set; }
public long? ASTUREQ_PerState { get; set; }
public long? IVRMMC_Id { get; set; }
public long? ASTUREQ_PerPincode { get; set; }
public string ASTUREQ_ConStreet { get; set; }
public string ASTUREQ_ConArea { get; set; }
public string ASTUREQ_ConAdd3 { get; set; }
public string ASTUREQ_ConCity { get; set; }
public string ASTUREQ_Village { get; set; }
public string ASTUREQ_Taluk { get; set; }
public string ASTUREQ_District { get; set; }
public long? ASTUREQ_ConState { get; set; }
public long? ASTUREQ_ConCountryId { get; set; }
public long? ASTUREQ_ConPincode { get; set; }
public long? ASTUREQ_AadharNo { get; set; }
public string ASTUREQ_StuBankAccNo { get; set; }
public string ASTUREQ_StudentPANCard { get; set; }
public string ASTUREQ_StuBankIFSCCode { get; set; }
public string ASTUREQ_StuCasteCertiNo { get; set; }
public long? ASTUREQ_MobileNo { get; set; }
public string ASTUREQ_EmailId { get; set; }
public string ASTUREQ_FatherAliveFlag { get; set; }
public string ASTUREQ_FatherMaritalStatusFlg { get; set; }
public string ASTUREQ_FatherName { get; set; }
public long? ASTUREQ_FatherAadharNo { get; set; }
public string ASTUREQ_FatherSurname { get; set; }
public string ASTUREQ_FatherEducation { get; set; }
public string ASTUREQ_FatherOccupation { get; set; }
public string ASTUREQ_FatherOfficeAdd { get; set; }
public string ASTUREQ_FatherDesignation { get; set; }
public decimal? ASTUREQ_FatherMonIncome { get; set; }
        public decimal? ASTUREQ_FatherAnnIncome { get; set; }
        public long? ASTUREQ_FatherNationality   { get; set; }
public string ASTUREQ_FatherReligion  { get; set; }
public string ASTUREQ_FatherCaste { get; set; }
public string ASTUREQ_FatherSubCaste  { get; set; }
public long? ASTUREQ_FatherMobleNo   { get; set; }
public string ASTUREQ_FatheremailId   { get; set; }
public string ASTUREQ_FatherBankAccNo { get; set; }
public string ASTUREQ_FatherBankIFSCCode  { get; set; }
public string ASTUREQ_FatherCasteCertiNo  { get; set; }
public string ASTUREQ_FatherPhoto { get; set; }
public string ASTUREQ_FatherSign  { get; set; }
public string ASTUREQ_FatherFingerprint   { get; set; }
public string ASTUREQ_FatherPANCardNo { get; set; }
public string ASTUREQ_MotherAliveFlag { get; set; }
public string ASTUREQ_MotherName  { get; set; }
public long? ASTUREQ_MotherAadharNo  { get; set; }
public string ASTUREQ_MotherSurname   { get; set; }
public string ASTUREQ_MotherEducation { get; set; }
public string ASTUREQ_MotherOccupation    { get; set; }
public string ASTUREQ_MotherOfficeAdd { get; set; }
public string ASTUREQ_MotherDesignation   { get; set; }
public decimal? ASTUREQ_MotherMonIncome { get; set; }
        public decimal? ASTUREQ_MotherAnnIncome { get; set; }
        public long? ASTUREQ_MotherNationality   { get; set; }
public string ASTUREQ_MotherReligion  { get; set; }
public string ASTUREQ_MotherCaste { get; set; }
public string ASTUREQ_MotherSubCaste  { get; set; }
public long? ASTUREQ_MotherMobleNo   { get; set; }
public string ASTUREQ_MotheremailId   { get; set; }
public string ASTUREQ_MotherBankAccNo { get; set; }
public string ASTUREQ_MotherBankIFSCCode  { get; set; }
public string ASTUREQ_MotherCasteCertiNo  { get; set; }
public string ASTUREQ_MotherPANCardNo { get; set; }
public decimal? ASTUREQ_TotalIncome { get; set; }
        public string ASTUREQ_MotherSign  { get; set; }
public string ASTUREQ_MotherPhoto { get; set; }
public string ASTUREQ_MotherFingerprint   { get; set; }
public string ASTUREQ_BirthPlace  { get; set; }
public long? ASTUREQ_Nationality { get; set; }
public int? ASTUREQ_BPLCardFlag { get; set; }
public string ASTUREQ_BPLCardNo   { get; set; }
public int? ASTUREQ_HostelReqdFlag  { get; set; }
public int? ASTUREQ_TransportReqdFlag   { get; set; }
public int? ASTUREQ_GymReqdFlag { get; set; }
public int? ASTUREQ_ECSFlag { get; set; }
public int? ASTUREQ_PaymentFlag { get; set; }
public decimal? ASTUREQ_AmountPaid { get; set; }
        public string ASTUREQ_PaymentType { get; set; }
public DateTime? ASTUREQ_PaymentDate { get; set; }
        public string ASTUREQ_ReceiptNo   { get; set; }
public string ASTUREQ_EMSINo  { get; set; }
public string ASTUREQ_ApplStatus  { get; set; }
public int? ASTUREQ_FinalpaymentFlag    { get; set; }
public string ASTUREQ_StudentPhoto    { get; set; }
public string ASTUREQ_StudentSign { get; set; }
public string ASTUREQ_StudentFingerprint  { get; set; }
public int? ASTUREQ_NoofSiblingsSchool  { get; set; }
public int? ASTUREQ_NoofSiblings    { get; set; }
public long? ASTUREQ_NoOfBrothers    { get; set; }
public long? ASTUREQ_NoOfSisters { get; set; }
public long? ASTUREQ_NoOfElderBrothers   { get; set; }
public long? ASTUREQ_NoOfYoungerBrothers { get; set; }
public long? ASTUREQ_NoOfElderSisters    { get; set; }
public long? ASTUREQ_NoOfYoungerSisters  { get; set; }
public long? ASTUREQ_NoOfDependencies    { get; set; }
public string ASTUREQ_TPINNO  { get; set; }
public string ASTUREQ_ConcessionCategory  { get; set; }
public string ASTUREQ_MOInstruction   { get; set; }
public string ASTUREQ_GPSTrackingId   { get; set; }
public string ASTUREQ_AppDownloadedDeviceId   { get; set; }
public string ASTUREQ_SecretCode  { get; set; }
public string ASTUREQ_BiometricId { get; set; }
public string ASTUREQ_RFCardNo    { get; set; }
public string ASTUREQ_FatherChurchAffiliation { get; set; }
public string ASTUREQ_MotherChurchAffiliation { get; set; }
public string ASTUREQ_ChangeConfirmFlg { get; set; }
public bool? ASTUREQ_FatherSelfEmployedFlg   { get; set; }
public bool? ASTUREQ_MotherSelfEmployedFlg   { get; set; }
public DateTime? ASTUREQ_ChurchBaptisedDate { get; set; }
        public bool? ASTUREQ_ApprovedFlg { get; set; }
public long? ASTUREQ_ApprovedBy  { get; set; }
public bool? ASTUREQ_ActiveFlag  { get; set; }
public DateTime? ASTUREQ_CreatedDate { get; set; }
        public long? ASTUREQ_CreatedBy   { get; set; }
public long? ASTUREQ_UpdatedBy   { get; set; }
public DateTime ASTUREQ_UpdatedDate { get; set; }
        public string ASTUREQ_GuardianMobileNo    { get; set; }
public string ASTUREQ_GuardianEmailId { get; set; }
public long ASMAY_Id    { get; set; }
public long? AMSTG_Id { get; set; }
public bool ASTUREQ_ConformFlg { get; set; }


    }
}

