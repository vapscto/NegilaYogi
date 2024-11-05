using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.HRMS
{
   public class HealthCardDetailsDTO
    {
        public long MI_Id { get; set; }
        public string HMTPD_MemberId { get; set; }
        public long UserId { get; set; }
        public Array getsavedetails { get; set; }
        public Array getemployeelist { get; set; }
        public long HRME_Id { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
        public Array policydeatail { get; set; }
        public Array masterdetails { get; set; }
        public Array getemployeedetails { get; set; }
        public string HRME_EmployeeCode { get; set; }
        public string HRMD_DepartmentName { get; set; }
        public string HRMDES_DesignationName { get; set; }
        public Array getreport { get; set; }

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
        public string HMTRSCD_Symptomspresented { get; set; }
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
        public string return_val { get; set; }
        public Array getemployeelistttt { get; set; }


    }
}
