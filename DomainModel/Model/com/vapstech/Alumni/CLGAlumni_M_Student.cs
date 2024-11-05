using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Alumni
{
    [Table("Alumni_College_Master_Student", Schema = "CLG")]
    public class CLGAlumni_M_StudentDMO :CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long ALCMST_Id { get; set; }
        public long MI_Id { get; set; }
        public long? AMCST_Id { get; set; }
        public long? AMCO_JOIN_Id { get; set; }
        public long? AMB_JOIN_Id { get; set; }
        public long? AMSE_JOIN_Id { get; set; }
        public long? ASMAY_Id_Join { get; set; }
        public long? AMB_Id_Left { get; set; }
        public long? ASMAY_Id_Left { get; set; }
        public long? AMCO_Left_Id { get; set; }
        public long? AMSE_Id_Left { get; set; }
        public string ALCMST_FirstName { get; set; }
        public string ALCMST_MiddleName { get; set; }
        public string ALCMST_LastName { get; set; }
        public DateTime? ALCMST_Date { get; set; }
        public string ALCMST_RegistrationNo { get; set; }
        public string ALCMST_AdmNo { get; set; }
        public string ALCMST_Sex { get; set; }
        public DateTime? ALCMST_DOB { get; set; }
        public string ALCMST_DOBinwords { get; set; }
        public int? ALCMST_Age { get; set; }
        public long? ASMCL_Id_Join { get; set; }
        public long? ASMCL_Id_Left { get; set; }
        public string ALCMST_BloodGroup { get; set; }
        public string ALCMST_MotherTongue { get; set; }
        public string ALCMST_HomeLaguage { get; set; }
        public string ALCMST_BirthCertNo { get; set; }
        public long? IVRMMR_Id { get; set; }
        public long? IMCC_Id { get; set; }
        public long? IMC_Id { get; set; }
        public string ALCMST_StudentSubCaste { get; set; }
        public string ALCMST_PerStreet { get; set; }
        public string ALCMST_PerArea { get; set; }
        public string ALCMST_PerCity { get; set; }
        public string ALCMST_PerAdd3 { get; set; }
        public long? ALCMST_PerState { get; set; }
        public long? IVRMMC_Id { get; set; }
        public long? ALCMST_PerPincode { get; set; }
        public string ALCMST_ConStreet { get; set; }
        public string ALCMST_ConArea { get; set; }
        public string ALCMST_ConAdd3 { get; set; }
        public string ALCMST_ConCity { get; set; }
        public string ALCMST_Village { get; set; }
        public string ALCMST_Taluk { get; set; }
        public string ALCMST_District { get; set; }
        public long? ALCMST_ConState { get; set; }
        public long? ALCMST_ConCountryId { get; set; }
        public long? ALCMST_ConPincode { get; set; }
        public long? ALCMST_AadharNo { get; set; }
        public long? ALCMST_StuBankAccNo { get; set; }
        public string ALCMST_StudentPANCard { get; set; }
        public string ALCMST_StuBankIFSCCode { get; set; }
        public string ALCMST_StuCasteCertiNo { get; set; }
        public long? ALCMST_MobileNo { get; set; }
        public string ALCMST_emailId { get; set; }
        public string ALCMST_FatherAliveFlag { get; set; }
        public string ALCMST_FatherName { get; set; }
        public long? ALCMST_FatherAadharNo { get; set; }
        public string ALCMST_FatherSurname { get; set; }
        public string ALCMST_FatherEducation { get; set; }
        public string ALCMST_FatherOccupation { get; set; }
        public string ALCMST_FatherOfficeAdd { get; set; }
        public string ALCMST_FatherDesignation { get; set; }
        public Decimal? ALCMST_FatherMonIncome { get; set; }
        public Decimal? ALCMST_FatherAnnIncome { get; set; }
        public long? ALCMST_FatherNationality { get; set; }
        public long? ALCMST_FatherReligion { get; set; }
        public string ALCMST_FatherCaste { get; set; }
        public string ALCMST_FatherSubCaste { get; set; }
        public long? ALCMST_FatherMobleNo { get; set; }
        public string ALCMST_FatheremailId { get; set; }
        public string ALCMST_FatherBankAccNo { get; set; }
        public string ALCMST_FatherBankIFSCCode { get; set; }
        public string ALCMST_FatherCasteCertiNo { get; set; }
        public string ALCMST_FatherPhoto { get; set; }
        public string ALCMST_FatherSign { get; set; }
        public string ALCMST_FatherFingerprint { get; set; }
        public string ALCMST_FatherPANCardNo { get; set; }
        public string ALCMST_MotherAliveFlag { get; set; }
        public string ALCMST_MotherName { get; set; }
        public string ALCMST_MotherAadharNo { get; set; }
        public string ALCMST_MotherSurname { get; set; }
        public string ALCMST_MotherEducation { get; set; }
        public string ALCMST_MotherOccupation { get; set; }
        public string ALCMST_MotherOfficeAdd { get; set; }
        public string ALCMST_MotherDesignation { get; set; }
        public Decimal? ALCMST_MotherMonIncome { get; set; }
        public Decimal? ALCMST_MotherAnnIncome { get; set; }
        public long? ALCMST_MotherNationality { get; set; }
        public long? ALCMST_MotherReligion { get; set; }
        public long? ALCMST_MotherCaste { get; set; }
        public string ALCMST_MotherSubCaste { get; set; }
        public long? ALCMST_MotherMobleNo { get; set; }
        public string ALCMST_MotheremailId { get; set; }
        public string ALCMST_MotherBankAccNo { get; set; }
        public string ALCMST_MotherBankIFSCCode { get; set; }
        public string ALCMST_MotherCasteCertiNo { get; set; }
        public string ALCMST_MotherPANCardNo { get; set; }
        public Decimal? ALCMST_TotalIncome { get; set; }
        public string ALCMST_MotherSign { get; set; }
        public string ALCMST_MotherPhoto { get; set; }
        public string ALCMST_MotherFingerprint { get; set; }
        public string ALCMST_BirthPlace { get; set; }
        public long? ALCMST_Nationality { get; set; }
        public int? ALCMST_BPLCardFlag { get; set; }
        public string ALCMST_BPLCardNo { get; set; }
        public int? ALCMST_HostelReqdFlag { get; set; }
        public int? ALCMST_TransportReqdFlag { get; set; }
        public int? ALCMST_GymReqdFlag { get; set; }
        public int? ALCMST_ECSFlag { get; set; }
        public int? ALCMST_PaymentFlag { get; set; }
        public int? ALCMST_AmountPaid { get; set; }
        public string ALCMST_PaymentType { get; set; }
        public DateTime? ALCMST_PaymentDate { get; set; }
        public string ALCMST_ReceiptNo { get; set; }
        public string ALCMST_EMSINo { get; set; }
        public string ALCMST_ApplStatus { get; set; }
        public Boolean? ALCMST_FinalpaymentFlag { get; set; }
        public string ALCMST_StudentPhoto { get; set; }
        public string ALCMST_StudentSign { get; set; }
        public string ALCMST_StudentFingerprint { get; set; }
        public int? ALCMST_NoofSiblingsSchool { get; set; }
        public int? ALCMST_NoofSiblings { get; set; }
        public int? ALCMST_NoOfBrothers { get; set; }
        public int? ALCMST_NoOfSisters { get; set; }
        public int? ALCMST_NoofDependencies { get; set; }
        public string ALCMST_TPINNO { get; set; }
        public long? IVRMMB_Id { get; set; }
        public string ALCMST_MOInstruction { get; set; }
        public long? ALCMST_GPSTrackingId { get; set; }
        public string ALCMST_AppDownloadedDeviceId { get; set; }
        public Boolean? ALCMST_ActiveFlag { get; set; }
        public long? ALCMST_CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public long? ALCMST_UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

    }
}
