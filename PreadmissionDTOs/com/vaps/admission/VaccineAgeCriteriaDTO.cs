using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.admission
{
    public class VaccineAgeCriteriaDTO
    {
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASVAC_Id { get; set; }
        public long ASVAC_AgeStartNo { get; set; }
        public long ASVAC_AgeEndNo { get; set; }
        public bool ASVAC_ActiveFlag { get; set; }
        public long ASVACD_Id { get; set; }       
        public string ASVACD_VaccineName { get; set; }
        public string ASVACD_VaccineType { get; set; }
        public bool ASVACD_ActiveFlag { get; set; }
        public Array GetAgeCriteriaDetails { get; set; }
        public Array ViewVaccineDetails { get; set; }
        public Array GetEditAgeCriteriaDetails { get; set; }
        public Array GetEditViewVaccineDetails { get; set; }
        public Array GetViewVaccineDetails { get; set; }
        public bool ReturnValue { get; set; }
        public string Message { get; set; }
        public VaccineAgeCriteriaDetails[] VaccineAgeCriteriaDetails { get; set; }
        // Vaccine Student Details
        public Array GetAccademicYear { get; set; }
        public Array GetStudentSearchList { get; set; }
        public Array GetAgeCriteriaStudentDetails { get; set; }
        public Array GetAgeCriteriaVaccineDetails { get; set; }
        public Array GetSavedStudentVaccineDetails { get; set; }
        public Array Getstudentvaccinedetails { get; set; }
        public Array GetViewstudentvaccinedetails { get; set; }
        public string searchfilter { get; set; }
        public string StudentName { get; set; }
        public string AMST_AdmNo { get; set; }
        public string vaccineagecriteria { get; set; }
        public long AMST_Id { get; set; }
        public SaveStudentVaccineDetails_Temp[] SaveStudentVaccineDetails_Temp { get; set; }
        public long ASWVD_Id { get; set; } 
        public DateTime? ASWVD_DateGiven { get; set; }
        public string ASWVD_AdministeredBy { get; set; }
        public DateTime? ASWVD_NextDoseDate { get; set; }
        public bool ASWVD_ActiveFlag { get; set; }

        //WebJobs
        public long? MobileNo { get; set; }
        public string EmailId { get; set; }
        public string VaccinationDueDate { get; set; }
    }

    public class VaccineAgeCriteriaDetails
    {
        public long ASVACD_Id { get; set; }
        public long ASVAC_Id { get; set; }
        public string ASVACD_VaccineName { get; set; }
        public string ASVACD_VaccineType { get; set; }       
    }

    public class SaveStudentVaccineDetails_Temp
    {
        public long ASVACD_Id { get; set; }
        public long ASWVD_Id { get; set; }
        public long ASVAC_Id { get; set; }
        public DateTime? ASWVD_NextDoseDate { get; set; }
        public DateTime? ASWVD_DateGiven { get; set; }
        public string ASWVD_AdministeredBy { get; set; }
    }
}
