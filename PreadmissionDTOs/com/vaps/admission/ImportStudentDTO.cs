using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.admission
{
    public class ImportStudentDTO
    {

        // added on 21/02/2017
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string AMSTRegistrationNo { get; set; }
        public string AMSTAdmNo { get; set; }  //  public long? AMC_Id { get; set; }
        public string Gender { get; set; } //AMST_Sex
        public string DOB { get; set; }  //  public string PASR_Age { get; set; }
        public string AMSTDOBWords { get; set; }
        public string Class { get; set; }  //long?
        public string BloodGroup { get; set; }
        public string MotherTongue { get; set; }
        public string BirthCertificateNo { get; set; }
        public string Religion { get; set; }
        public string CasteCategory { get; set; }
        public string Caste { get; set; }
        public string Permanentadd3 { get; set; }
        public string PermanentStreet { get; set; }
        public string PermanentArea { get; set; }
        public string PermanentCity { get; set; }
        public string PermanentCountry { get; set; }  //changed
        public string PermanentPincode { get; set; }
        public string Permanentstate { get; set; }        
        public string PresentStreet { get; set; }
        public string PresentArea { get; set; }
        public string PresentCity { get; set; }
        public string PresentState { get; set; }
        public string persentcity { get; set; }
        public string persentstate { get; set; }
        public string PresentCountry { get; set; }
        public string PresentPincode { get; set; }
        public string AadharNo { get; set; }
        public string BankAccountNo { get; set; }
        public string IFSCCode { get; set; }
        public string CasteCertificateNo { get; set; }
        public string MobileNo { get; set; }
        public string EmailID { get; set; }
        public string FatherAlive { get; set; }
        public string FatherName { get; set; }
        public string FatherAadharNo { get; set; }
        public string FatherSurname { get; set; }
        public string FatherOccupation { get; set; }
        public string FatherEducation { get; set; }
        public string FatherOfficeAddress { get; set; }
        public string FatherDesignation { get; set; }
        public string FatherMonthlyIncome { get; set; }
        public string FatherAnnualIncome { get; set; }
        public string FatherNationality { get; set; }
        public string Fathermobileno { get; set; }
        public string FatherEmailId { get; set; }
        public string FatherAccountNo { get; set; }
        public string FatherIFSCcode { get; set; }
        public string FathercastecertificateNo { get; set; }
        public string FatherPhoto { get; set; }
        public string MotherAlive { get; set; }
        public string MotherName { get; set; }
        public string MotherAadharNo { get; set; }
        public string MotherSurname { get; set; }
        public string MotherEducation { get; set; }
        public string MotherOcupation { get; set; }
        public string MotherOccupation { get; set; }
        public string MotherDesignation { get; set; }
        public string MotherOfficesAddress { get; set; }
        public string MotherMonthlyIncome { get; set; }
        public string MotherAnnualIncome { get; set; }
        public string MotherNationality { get; set; }
        public string MotherMobileNo { get; set; }
        public string MotherEmailId { get; set; }
        public string MotherBankAccountNo { get; set; }
        public string MotherIFSCcode { get; set; }
        public string MotherCasteCertificateNo { get; set; }
        public string TotalIncome { get; set; }
        public string MotherPhoto { get; set; }
        public string StudentBirthPlace { get; set; }
        public string StudentNationality { get; set; }
        public string BPLCardFlag { get; set; }
        public string BPLCardNo { get; set; }
        public string HostelFacility { get; set; }
        public string TransportFacility { get; set; }
        public string GymFacility { get; set; }
        public string ECSFlag { get; set; }
        public string GymReqdFlag { get; set; }
        public string TransportReqdFlag { get; set; }
        public string HostelReqdFlag { get; set; }
        public string PaymentFlag { get; set; }
        public string AmountPaid { get; set; }
        public string PaymentType { get; set; }
        public string PaymentDate { get; set; }
        public string ReceiptNo { get; set; }
        public string ActiveFlag { get; set; }
        public string Applicationstatus { get; set; }
        public string FinalPaymentFlag { get; set; }
        public string PhotoName { get; set; }
        public string Noofsisters { get; set; }
        public string NoOfBrothers { get; set; }
        public int ASMAY_Id { get; set; }
        public int MI_Id { get; set; }
        public string amstdate { get; set; }
        public string stuStatus { get; set; }
        public string year { get; set; }
        public string NoOfYoungerSisters { get; set; }
        public string NoOfElderSisters { get; set; }
        public string NoOfYoungerBrothers { get; set; }
        public string NoOfElderBrothers { get; set; }
        public string LanguageSpoken { get; set; }
        public string MotherPANNo { get; set; }
        public string FatherPANNo { get; set; }
        public string StudentPANNo { get; set; }
        public string MotherSubCaste { get; set; }
        public string MotherCaste { get; set; }
        public string MotherReligion { get; set; }
        public string FatherSubCaste { get; set; }
        public string FatherCaste { get; set; }
        public string FatherReligion { get; set; }
        public string StudentSubCaste { get; set; }
        public string MotherOfficeAddress { get; set; }
        public string BirthPlace { get; set; }
        public string MotherCasteCertiNo { get; set; }
        public string MotherBankIFSC_Code { get; set; }
        public string MotherBankAccNo { get; set; } 
        public string FatherCasteCertiNo { get; set; } 
        public string FatherBankIFSC_Code { get; set; } 
        public string FatherBankAccNo { get; set; } 
        public string FatherOfficeAdd { get; set; } 
        public string StuCasteCertiNo { get; set; } 
        public string StudentBankIFSC_Code { get; set; } 
        public string StudentBankAccNo { get; set; } 
        public string BirthCertNO { get; set; } 
        public string CurrentYear { get; set; } 
        public string CurrentClass { get; set; } 
        public string CurrentSection { get; set; } 
        public string Status { get; set; } 
        public List<ImportStudentDTO> newlstget { get; set; }
    }
}
