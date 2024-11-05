using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class StudentApplicationPartOneDTO : CommonParamDTO
    {
        public long pasr_id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public string PASR_FirstName { get; set; }
        public string PASR_MiddleName { get; set; }
        public string PASR_LastName { get; set; }
        public string PASR_Date { get; set; }
        public string PASR_RegistrationNo { get; set; }
        public long AMC_Id { get; set; }
        public string PASR_Sex { get; set; }
        public string PASR_DOB { get; set; }
        public int PASR_Age { get; set; }
        public long ASMCL_Id { get; set; }
        public string PASR_BloodGroup { get; set; }
        public string PASR_MotherTongue { get; set; }
        public long Religion_Id { get; set; }
        public long CasteCategory_Id { get; set; }
        public long Caste_Id { get; set; }
        public string PASR_PerStreet { get; set; }
        public string PASR_PerArea { get; set; }
        public string PASR_PerCity { get; set; }
        public string PASR_PerState { get; set; }
        public string PASR_PerCountry { get; set; }
        public int PASR_PerPincode { get; set; }
        public string PASR_ConStreet { get; set; }
        public string PASR_ConArea { get; set; }
        public string PASR_ConCity { get; set; }
        public string PASR_ConState { get; set; }
        public string PASR_ConCountry { get; set; }
        public int PASR_ConPincode { get; set; }
        public int PASR_AadharNo { get; set; }
        public int PASR_MobileNo { get; set; }
        public string PASR_emailId { get; set; }
        public string PASR_MaritalStatus { get; set; }
        public int PASR_FatherAliveFlag { get; set; }
        public string PASR_FatherName { get; set; }
        public int PASR_FatherAadharNo { get; set; }
        public string PASR_FatherSurname { get; set; }
        public string PASR_FatherEducation { get; set; }
        public string PASR_FatherOccupation { get; set; }
        public string PASR_FatherDesignation { get; set; }
        public decimal PASR_FatherIncome { get; set; }
        public int PASR_FatherMobleNo { get; set; }
        public string PASR_FatheremailId { get; set; }
        public int PASR_MotherAliveFlag { get; set; }
        public string PASR_MotherName { get; set; }
        public int PASR_MotherAadharNo { get; set; }
        public string PASR_MotherSurname { get; set; }
        public string PASR_MotherEducation { get; set; }
        public string PASR_MotherOccupation { get; set; }
        public string PASR_MotherDesignation { get; set; }
        public decimal PASR_MotherIncome { get; set; }
        public int PASR_MotherMobleNo { get; set; }
        public string PASR_MotheremailId { get; set; }
        public decimal PASR_TotalIncome { get; set; }
        public string PASR_BirthPlace { get; set; }
        public string PASR_Nationality { get; set; }
        public int PASR_HostelReqdFlag { get; set; }
        public int PASR_TransportReqdFlag { get; set; }
        public int PASR_GymReqdFlag { get; set; }
        public int PASR_ECSFlag { get; set; }
        public int PASR_PaymentFlag { get; set; }
        public int PASR_AmountPaid { get; set; }
        public string PASR_PaymentType { get; set; }
        public DateTime? PASR_PaymentDate { get; set; }
        public string PASR_ReceiptNo { get; set; }
        public int PASR_ActiveFlag { get; set; }
        public string PASR_ApplStatus { get; set; }
        public int PASR_FinalpaymentFlag { get; set; }
        // New Fields
        public int? PASR_UndertakingFlag { get; set; }
        public string PASR_MotherOfficeAddr { get; set; }
        public long? PASR_MotherNationality { get; set; }
        public string PASR_FatherOfficeAddr { get; set; }
        public long? PASR_FatherNationality { get; set; }
        public string PASR_OtherPermanentAddr { get; set; }
        public string PASR_OtherResidential_Addr { get; set; }
        public string PASR_ExtraActivity { get; set; }
        public string PASR_LastPlayGrndAttnd { get; set; }
    }
}
