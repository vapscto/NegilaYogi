using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
namespace DomainModel.Model.com.vapstech.College.Admission
{
    [Table("Adm_Master_College_Student", Schema = "CLG")]
    public class Adm_Master_College_StudentDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long AMCST_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public string AMCST_FirstName { get; set; }
        public string AMCST_MiddleName { get; set; }
        public string AMCST_LastName { get; set; }
        public DateTime AMCST_Date { get; set; }
        public string AMCST_RegistrationNo { get; set; }
        public string AMCST_AdmNo { get; set; }
        public long? AMCOC_Id { get; set; }
        public long AMCO_Id { get; set; }
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
        public string AMCST_Urban_Rural { get; set; }
        public string AMCST_District { get; set; }
        public long? AMCST_ConState { get; set; }
        public long? AMCST_ConCountryId { get; set; }
        public long? AMCST_ConPincode { get; set; }
        public long? AMCST_AadharNo { get; set; }
        public long? AMCST_StuBankAccNo { get; set; }
        public string AMCST_StuBankIFSCCode { get; set; }
        public string AMCST_StuCasteCertiNo { get; set; }
        public long AMCST_MobileNo { get; set; }
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
        public long? AMCST_Nationality { get; set; }
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
        public string AMCST_TPINNO { get; set; }
        public bool AMCST_ActiveFlag { get; set; }
        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }
        public string AMCST_SOL { get; set; }
        public long ACMB_Id { get; set; }
        public long ACQ_Id { get; set; }

        //biometric

        public string AMCST_RFCardNo { get; set; }
        public string AMCST_BiometricId { get; set; }

        public long ACQC_Id { get; set; }
        public long ACSS_Id { get; set; }
        public long ACST_Id { get; set; }
        public string AMCST_CoutryCode { get; set; }
        public string AMCST_PassportNo { get; set; }
        public string AMCST_PassportIssuedAt { get; set; }
        public DateTime? AMCST_PassportIssueDate { get; set; }
        public long? AMCST_PassportIssuedCounrty { get; set; }
        public string AMCST_PassportIssuedPlace { get; set; }
        public DateTime? AMCST_PassportExpiryDate { get; set; }
        public string AMCST_VisaIssuedBy { get; set; }
        public DateTime? AMCST_VISAValidFrom { get; set; }
        public DateTime? AMCST_VISAValidTo { get; set; }
        public bool? AMCST_Divyangjan { get; set; }

        public long? AMCST_WalletPIN { get; set; }
        public string AMCST_AppDownloadedDeviceId { get; set; }
        //public decimal? AMCST_NEETMarks { get; set; }
        //public decimal? AMCST_JEEMainMarks { get; set; }
        //public decimal? AMCST_JEEAdvancedMarks { get; set; }
        public List<AdmCollegeStudentSMSNoDMO> AdmCollegeStudentSMSNo { get; set; }
        public List<AdmCollegeStudentEmailIdDMO> AdmCollegeStudentEmailId { get; set; }
        public List<AdmCollegeStudentParentsEmailIdDMO> AdmCollegeStudentParentsEmailId { get; set; }
        public List<AdmCollegeStudentParentsMobileNoDMO> AdmCollegeStudentParentsMobileNo { get; set; }
        public List<AdmCollegeStudentReferenceDMO> referenceDMOList { get; set; }
        public List<AdmCollegeStudentSourceDMO> sourceDMOList { get; set; }
        public List<AdmCollegeStudentSiblingsDetailsDMO> siblingDMOList { get; set; }
        public List<AdmCollegeStudentPrevSchoolDMO> prevSchoolDMOList { get; set; }
        public List<AdmCollegeStudentGuardianDMO> guardianDMOList { get; set; }
        public List<AdmCollegeStudentDocumentsDMO> documentDMOList { get; set; }
        public List<Adm_College_Yearly_StudentDMO> Adm_College_Yearly_StudentDMO { get; set; }

        //public List<AdmCollegeMasterBatchDMO> AdmCollegeMasterBatchDMO { get; set; }
       // public List<Clg_Adm_College_QuotaDMO> Clg_Adm_College_QuotaDMO { get; set; }

     
    }
}