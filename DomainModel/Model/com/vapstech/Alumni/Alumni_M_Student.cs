using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Alumni
{
    [Table("Alumni_Master_Student", Schema = "ALU")]
    public class Alumni_M_StudentDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long ALMST_Id { get; set; }
        public long MI_Id { get; set; }
      
        public long? AMST_ID { get; set; }
        public long? ASMAY_Id_Join { get; set; }
        public long? ASMAY_Id_Left { get; set; }
        public long? ASMCL_Id_Join { get; set; }
        public long? ASMCL_Id_Left { get; set; }
        public long? IVRMMR_Id { get; set; }
        public long? IMCC_Id { get; set; }
        public long? IMC_Id { get; set; }
        public long? IVRMMC_Id { get; set; }
        public long? IVRMMB_Id { get; set; }
        
        public string ALMST_FirstName { get; set; }
        public string ALMST_MiddleName { get; set; }
        public string ALMST_LastName { get; set; }
        public DateTime? ALMST_Date { get; set; }
        public string ALMST_RegistrationNo { get; set; }
        public string ALMST_AdmNo { get; set; }
        public string ALMST_Sex { get; set; }
        public DateTime ALMST_DOB { get; set; }
        public string ALMST_DOBinwords { get; set; }
        public int? ALMST_Age { get; set; }
        public string ALMST_BloodGroup { get; set; }
        public string ALMST_MotherTongue { get; set; }
        public string ALMST_HomeLaguage { get; set; }
        public string ALMST_BirthCertNo { get; set; }
        public string ALMST_StudentSubCaste { get; set; }
        public string ALMST_PerStreet { get; set; }
        public string ALMST_PerArea { get; set; }
        public string ALMST_PerCity { get; set; }
        public string ALMST_PerAdd3 { get; set; }
        public long? ALMST_PerState { get; set; }
        public long? ALMST_PerDistrict { get; set; }
        public long? ALMST_ConDistrict { get; set; }
        public long? ALMST_PerPincode { get; set; }
        public string ALMST_ConStreet { get; set; }
        public string ALMST_ConArea { get; set; }
        public string ALMST_ConAdd3 { get; set; }
        public string ALMST_ConCity { get; set; }
        public string ALMST_Village { get; set; }
        public string ALMST_Taluk { get; set; }
        public string ALMST_District { get; set; }
        public long? ALMST_ConState { get; set; }
        public long? ALMST_ConCountryId { get; set; }
        public long? ALMST_ConPincode { get; set; }
        public long? ALMST_AadharNo { get; set; }
        public string ALMST_StuBankAccNo { get; set; }
        public string ALMST_StudentPANCard { get; set; }
        public string ALMST_StuBankIFSCCode { get; set; }
        public string ALMST_StuCasteCertiNo { get; set; }
        public long? ALMST_MobileNo { get; set; }
        public string ALMST_emailId { get; set; }
        public string ALMST_FatherAliveFlag { get; set; }
        public string ALMST_FatherName { get; set; }
        public long? ALMST_FatherAadharNo { get; set; }
        public string ALMST_FatherSurname { get; set; }
        public string ALMST_FatherEducation { get; set; }
        public string ALMST_FatherOccupation { get; set; }
        public string ALMST_FatherOfficeAdd { get; set; }
        public string ALMST_FatherDesignation { get; set; }
        public decimal? ALMST_FatherMonIncome { get; set; }
        public decimal? ALMST_FatherAnnIncome { get; set; }
        public long? ALMST_FatherNationality { get; set; }
        public long? ALMST_FatherReligion { get; set; }
        public long? ALMST_FatherCaste { get; set; }
        public string ALMST_FatherSubCaste { get; set; }
        public long? ALMST_FatherMobleNo { get; set; }
        public string ALMST_FatheremailId { get; set; }
        public string ALMST_FatherBankAccNo { get; set; }
        public string ALMST_FatherBankIFSCCode { get; set; }
        public string ALMST_FatherCasteCertiNo { get; set; }
        public string ALMST_FatherPhoto { get; set; }
        public string ALMST_FatherSign { get; set; }
        public string ALMST_FatherFingerprint { get; set; }
        public long? ALMST_FatherPANCardNo { get; set; }
        public string ALMST_MotherAliveFlag { get; set; }
        public string ALMST_MotherName { get; set; }
        public long? ALMST_MotherAadharNo { get; set; }
        public string ALMST_MotherSurname { get; set; }
        public string ALMST_MotherEducation { get; set; }
        public string ALMST_MotherOccupation { get; set; }
        public string ALMST_MotherOfficeAdd { get; set; }
        public string ALMST_MotherDesignation { get; set; }
        public decimal? ALMST_MotherMonIncome { get; set; }
        public decimal? ALMST_MotherAnnIncome { get; set; }
        public long? ALMST_MotherNationality { get; set; }
        public long? ALMST_MotherReligion { get; set; }
        public long? ALMST_MotherCaste { get; set; }
        public string ALMST_MotherSubCaste { get; set; }
        public long? ALMST_MotherMobleNo { get; set; }
        public string ALMST_MotheremailId { get; set; }
        public string ALMST_MotherBankAccNo { get; set; }
        public string ALMST_MotherBankIFSCCode { get; set; }
        public string ALMST_MotherCasteCertiNo { get; set; }
        public string ALMST_MotherPANCardNo { get; set; }
        public decimal? ALMST_TotalIncome { get; set; }
        public string ALMST_MotherSign { get; set; }
        public string ALMST_MotherPhoto { get; set; }
        public string ALMST_MotherFingerprint { get; set; }
        public string ALMST_BirthPlace { get; set; }
        public string ALMST_Nationality { get; set; }
        public int? ALMST_BPLCardFlag { get; set; }
        public string ALMST_BPLCardNo { get; set; }
        public int? ALMST_HostelReqdFlag { get; set; }
        public int? ALMST_TransportReqdFlag { get; set; }
        public int? ALMST_GymReqdFlag { get; set; }
        public int? ALMST_ECSFlag { get; set; }
        public int? ALMST_PaymentFlag { get; set; }
        public int? ALMST_AmountPaid { get; set; }
        public string ALMST_PaymentType { get; set; }
        public string ALMST_FullAddess { get; set; }
        public DateTime? ALMST_PaymentDate { get; set; }
        public string ALMST_ReceiptNo { get; set; }
        public string ALMST_EMSINo { get; set; }
        public string ALMST_ApplStatus { get; set; }
        public Boolean? ALMST_FinalpaymentFlag { get; set; }
        public string ALMST_StudentPhoto { get; set; }
        public string ALMST_StudentSign { get; set; }
        public string ALMST_StudentFingerprint { get; set; }
        public int? ALMST_NoofSiblingsSchool { get; set; }
        public int? ALMST_NoofSiblings { get; set; }
        public int? ALMST_NoOfBrothers { get; set; }
        public int? ALMST_NoOfSisters { get; set; }
        public int? ALMST_NoofDependencies { get; set; }
        public string ALMST_TPINNO { get; set; }
        public string ALMST_MOInstruction { get; set; }
        public long? ALMST_GPSTrackingId { get; set; }
        public string ALMST_AppDownloadedDeviceId { get; set; }
        public bool ALMST_ActiveFlag { get; set; }
        public long? ALMST_CreatedBy { get; set; }   
        public long? ALMST_UpdatedBy { get; set; }
        public long? ALMST_PerCountry { get; set; }
        public string ALMST_Marital_Status { get; set; }
        public string ALMST_PhoneNo { get; set; }
        public string ALMST_FamilyPhoto { get; set; }
        public string ALMST_MembershipId { get; set; }
        public string ALMST_SpouseContactNo { get; set; }
        public string ALMST_NickName { get; set; }
        public string ALMST_SpouseEmailId { get; set; }
        public string ALMST_SpouseQulaification { get; set; }
        public string ALMST_SpouseProfession { get; set; }
        public string ALMST_SpouseName { get; set; }
        public long? ALMST_MembershipCategory { get; set; }
        public long? ALSREG_Id { get; set; }
        public DateTime? ALMST_WeddingAnniversaryDate { get; set; }
        

        public List<AlumniUserRegistrationDMO> AlumniUserRegistrationDMO { get; set; }
      


    }
}
