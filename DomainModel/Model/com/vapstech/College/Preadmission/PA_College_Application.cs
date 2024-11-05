using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.College.Preadmission
{
    [Table("PA_College_Application", Schema = "CLG")]
    public class PA_College_Application : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PACA_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public string PACA_FirstName { get; set; }
        public string PACA_MiddleName { get; set; }
        public string PACA_LastName { get; set; }
        public DateTime PACA_Date { get; set; }

        public string PACA_RegistrationNo { get; set; }
        public string PACA_ApplicationNo { get; set; }
  public bool? PACA_IncomeCertificateFlg { get; set; }
        public long AMCOC_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public string PACA_Sex { get; set; }
        public DateTime PACA_DOB { get; set; }
        public string PACA_DOB_inwords { get; set; }
        public long PACA_Age { get; set; }
        public string PACA_BloodGroup { get; set; }
        public string PACA_MotherTongue { get; set; }
        public string PACA_BirthCertNo { get; set; }
        public long IVRMMR_Id { get; set; }
        public long IMCC_Id { get; set; }
        public long IMC_Id { get; set; }
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
        public Decimal PACA_FatherMonIncome { get; set; }
        public Decimal PACA_FatherAnnIncome { get; set; }
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
        public long PACA_MotherAadharNo { get; set; }
        public string PACA_MotherSurname { get; set; }
        public string PACA_MotherEducation { get; set; }
        public string PACA_MotherOccupation { get; set; }
        public string PACA_MotherOfficeAdd { get; set; }
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
        public long PACA_AdmStatus { get; set; }
        public string PACA_PassportIssuedPlace { get; set; }
        public string PACA_Place { get; set; }
        public DateTime? PACA_PassportExpiryDate { get; set; }
        public string PACA_VISAIssuedBy { get; set; }
        public string PACA_VISANo { get; set; }
        public string PACA_VISAValidFrom { get; set; }
        public string PACA_VISAValidTo { get; set; }
        public bool PACA_ActiveFlag { get; set; }

        public int? PACA_CompleteFillflag { get; set; }

        public int ID { get; set; }

        public long AMSE_Id { get; set; }
        public string PACA_FatherResAdd { get; set; }
        public string PACA_MotherResAdd { get; set; }
        public string PACA_PhysicallyChallenged { get; set; }
        public string PACA_FatherResTelPhno { get; set; }
        public string PACA_FatherOfficeTelPhno { get; set; }
        public string PACA_MotherResTelPhno { get; set; }
        public string PACA_MotherOfficeTelPhno { get; set; }

        public long? PACA_FatherOfficeAddPincode { get; set; }
        public long? PACA_FatherResidentialAddPinCode { get; set; }
        public long? PACA_MotherOfficeAddPinCode { get; set; }
        public long? PACA_MotherResidentialAddPinCode { get; set; }

        public string PACA_Statusremark { get; set; }

        public long? ACQ_Id { get; set; }
        public long? ACQC_Id { get; set; }

        public string PACA_CoutryCode { get; set; }
        public string PACA_MotherCountryCode { get; set; }
        public string PACA_FatherCountryCode { get; set; }
        public string PACA_Urban_Rural { get; set; }
    }
}
