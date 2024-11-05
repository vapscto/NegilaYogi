using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.HRMS
{
    [Table("HM_T_ReimbursementClaim_Details")]
    public class HM_T_ReimbursementClaim_DetailsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HMTRSCD_Id { get; set; }
        public long HMTPD_Id { get; set; }
        public string HMTRSCD_MemberId { get; set; }
        public string HMTRSCD_CompanyName { get; set; }
        public DateTime? HMTRSCD_DOB { get; set; }
        public string HMTRSCD_Patientname { get; set; }
        public string HMTRSCD_Gender { get; set; }
        public string HMTRSCD_PatientPhNo { get; set; }
        public DateTime? HMTRSCD_DateOfTreatment { get; set; }
        public DateTime? HMTRSCD_DateOfAdmission { get; set; }
        public DateTime? HMTRSCD_DateOfDischarge { get; set; }
        public string HMTRSCD_Symptomspresented { get;set; }
        public string HMTRSCD_RlptoPrimaryinsured { get; set; }
        public bool HMTRSCD_Self { get; set; }
        public bool HMTRSCD_Spouse { get; set; }
        public bool HMTRSCD_Child { get; set; }
        public bool HMTRSCD_Father { get; set; }
        public bool HMTRSCD_Mother { get; set; }
        public bool HMTRSCD_Other { get; set; }
        public string HMTRSCD_Occupation { get; set; }
        public string HMTRSCD_NameofHospital { get; set; }
        public string HMTRSCD_RoomCategory { get; set; }
        public decimal HMTRSCD_hospitalizationexpenses { get; set; }
        public string HMTRSCD_Address { get; set; }
        public string HMTRSCD_Pincode { get; set; }
        public string HMTRSCD_EmailId { get; set; }
        public string HMTRSCD_ClaimDocFilePath { get; set; }
        public bool HMTRSCD_CurrentlyCoveredInsuranceFlag { get; set; }
        public string HMTRSCD_RClaimNo { get; set; }
        public decimal HMTRSCD_SumOfInsuredAmt { get; set; }
        public string HMTRSCD_RCCompanyName { get; set; }
        public string HMTRSCD_Diagnosis { get; set; }
        public bool HMTRSCD_ActiveFLag { get; set; }
        public DateTime? HMTRSCD_CreatedDate { get; set; }
        public DateTime? HMTRSCD_UpdatedDate { get; set; }
    }
}
