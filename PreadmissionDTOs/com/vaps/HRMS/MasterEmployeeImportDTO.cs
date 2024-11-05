using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.HRMS
{
    public class MasterEmployeeImportDTO :CommonParamDTO
    {
        // public long HRME_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRMET_Id { get; set; }
        public long HRMGT_Id { get; set; }
        public long HRMD_Id { get; set; }
        public long HRMDES_Id { get; set; }
        public long HRMG_Id { get; set; }
        public string EmployeeFirstName { get; set; }
        public string EmployeeMiddleName { get; set; }
        public string EmployeeLastName { get; set; }
        public string EmployeeCode { get; set; }
        public string BiometricCode { get; set; }
        public string RFCardId { get; set; }
        public string Permanent_Street { get; set; }
        public string Permanent_Area { get; set; }
        public string Permanent_City { get; set; }
        public string Permanent_Add4 { get; set; }
        public long PermanentState { get; set; }
        public long PermanentCountryName { get; set; }
        public long Permanent_Pincode { get; set; }
        public string Local_Street { get; set; }
        public string Local_Area { get; set; }
        public string Local_City { get; set; }
        public string Local_Add4 { get; set; }
        public long LocalStateName { get; set; }
        public long HRME_LocCountryId { get; set; }
        public long Local_Pincode { get; set; }
        public long MaritalStatus { get; set; }
        public long GenderName { get; set; }
        public long CasteCategoryName { get; set; }
        public long CasteName { get; set; }
        public long ReligionName { get; set; }
        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public string SpouseName { get; set; }
        public string SpouseOccupation { get; set; }
        public long? SpouseMobileNo { get; set; }
        public string SpouseEmailId { get; set; }
        public string SpouseAddress { get; set; }
        public string Date_Of_Birth { get; set; }
        public string Date_Of_Joining { get; set; }
        public string ExpectedRetirementDate { get; set; }
        public string PFDate { get; set; }
        public string ESIDate { get; set; }
        public string Date_Of_Leaving { get; set; }
        public long MobileNo { get; set; }
        public string EmailId { get; set; }
        public string BloodGroup { get; set; }
        public string PaymentType { get; set; }
        public string BankAccountNo { get; set; }
        public bool? HRME_PFApplicableFlag { get; set; }
        public bool? HRME_PFMaxFlag { get; set; }
        public bool? HRME_PFFixedFlag { get; set; }
        public string PFAccNo { get; set; }
        public string ESIAccNo { get; set; }
        public string GratuityAccNo { get; set; }
        public bool? HRME_ESIApplicableFlag { get; set; }
        public string HRME_Photo { get; set; }
        public bool? HRME_LeftFlag { get; set; }

        public string LeavingReason { get; set; }
        public string Height { get; set; }
        public string HeightUOM { get; set; }
        public int Weight { get; set; }
        public string WeightUOM { get; set; }
        public string IdentificationMark { get; set; }
        public string ApprovalNo { get; set; }
        public string PANCardNo { get; set; }
        public string AadharCardNo { get; set; }
        public bool? HRME_SubstituteFlag { get; set; }
        public string NationalSSN { get; set; }
        public string SalaryType { get; set; }
        public int? EmployeeOrder { get; set; }
        public bool? HRME_ActiveFlag { get; set; }



        //added by sudeep
        public string EmployeeType { get; set; }
        public string EmployeeGroupType { get; set; }

        public string DepartmentName { get; set; }

        public string DesignationName { get; set; }
        public string GradeName { get; set; }
        public string Permanent_State { get; set; }

        public string LocalState_Name { get; set; }
        public string Local_Country_Name { get; set; }

        public string PermanentCountry_Name { get; set; }
        public string Marital_Status { get; set; }

        public string Gender_Name { get; set; }
        public string Caste_Name { get; set; }
        public string CasteCategory_Name { get; set; }
        public string Religion_Name { get; set; }
        public string Emp_type { get; set; }


        public MasterEmployeeImportDTO[] newlstget { get; set; }

        public string stuStatus { get; set; }
        public string returnMsg { get; set; }

        public Array failedlist { get; set; }



        public long? IVRMMMS_Id { get; set; }
        public long? IVRMMG_Id { get; set; }
        public long? CasteCategoryId { get; set; }
        public long? CasteId { get; set; }
        public long? ReligionId { get; set; }


        }
}
