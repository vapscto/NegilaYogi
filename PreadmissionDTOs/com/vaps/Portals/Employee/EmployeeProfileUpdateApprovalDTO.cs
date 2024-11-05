using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Portals.Employee
{
    public class EmployeeProfileUpdateApprovalDTO
    {
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public long UserId { get; set; }
        public long? ReligionId { get; set; }
        public long? CasteCategoryId { get; set; }
        public Array GetCountryList { get; set; }        
        public Array GetLocalStateList { get; set; }
        public Array GetPerStateList { get; set; }
        public Array GetMartialStatusList { get; set; }
        public Array GetGenderList { get; set; }
        public Array GetEmployeeDetails { get; set; }
        public Array GetEmployeMobileNoDetails { get; set; }
        public Array GetEmployeEmailIdDetails { get; set; }
        public Array GetEmployeRequestedDetails { get; set; }
        public Array GetEmployeRequestedMobileNoDetails { get; set; }
        public Array GetEmployeRequestedEmailDetails { get; set; }
        public Array GetReligionList { get; set; }
        public Array GetCasteList { get; set; }
        public Array GetCasteCategoryList { get; set; }
        public long HRMEREQ_Id { get; set; }        
        public string HRMEREQ_EmployeeFirstName { get; set; }
        public string HRMEREQ_EmployeeMiddleName { get; set; }
        public string HRMEREQ_EmployeeLastName { get; set; }
        public string HRMEREQ_PerStreet { get; set; }
        public string HRMEREQ_PerArea { get; set; }
        public string HRMEREQ_PerCity { get; set; }
        public string HRMEREQ_PerAdd4 { get; set; }
        public long? HRMEREQ_PerStateId { get; set; }
        public long? HRMEREQ_PerCountryId { get; set; }
        public long? HRMEREQ_PerPincode { get; set; }
        public string HRMEREQ_LocStreet { get; set; }
        public string HRMEREQ_LocArea { get; set; }
        public string HRMEREQ_LocCity { get; set; }
        public string HRMEREQ_LocAdd4 { get; set; }
        public long? HRMEREQ_LocStateId { get; set; }
        public long? HRMEREQ_LocCountryId { get; set; }
        public long? HRMEREQ_LocPincode { get; set; }
        public long? IVRMMMS_Id { get; set; }
        public long? IVRMMG_Id { get; set; }       
        public long? CasteId { get; set; }        
        public string HRMEREQ_FatherName { get; set; }
        public string HRMEREQ_MotherName { get; set; }
        public string HRMEREQ_SpouseName { get; set; }
        public string HRMEREQ_SpouseOccupation { get; set; }
        public long? HRMEREQ_SpouseMobileNo { get; set; }
        public string HRMEREQ_SpouseEmailId { get; set; }
        public string HRMEREQ_SpouseAddress { get; set; }
        public DateTime? HRMEREQ_DOB { get; set; }
        public long? HRMEREQ_MobileNo { get; set; }
        public string HRMEREQ_EmailId { get; set; }
        public string HRMEREQ_BloodGroup { get; set; }
        public string HRMEREQ_Photo { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? HRMEREQREQ_ApprovedFlg { get; set; }
        public long? HRMEREQREQ_ApprovedBy { get; set; }
        public bool? HRMEREQREQ_ConformFlg { get; set; }
        public long? HRMEREQREQ_ConformBy { get; set; }
        public string HRMEREQREQ_ReqStatus { get; set; }
        public string HRMEREQREQ_ChangeConfirmFlg { get; set; }
        public Temp_MobileNo[] Temp_MobileNo { get; set; }
        public Temp_EmailId[] Temp_EmailId { get; set; }
        public bool returnval { get; set; }
        public string message { get; set; }
        public string username { get; set; }
        public string employeename { get; set; }

        public Array GetRequestedData { get; set; }
        public Array GetRequestedDataList { get; set; }
        public Array GetRequestedEmailData { get; set; }
        public Array GetRequestedMobileData { get; set; }

        public long? HRMEREQMN_MobileNo { get; set; }
        public string HRMEREQMN_Flag { get; set; }
        public string HRMEREQEM_EmailId { get; set; }
        public string HRMEREQEM_Flag { get; set; }

    }

    public class Temp_MobileNo
    {
        public long HRMEREQMN_Id { get; set; }
        public long HRMEREQ_Id { get; set; }
        public long? HRMEREQMN_MobileNo { get; set; }
        public string HRMEREQMN_Flag { get; set; }
        public bool HRMEREQMN_ActiveFlg { get; set; }

    }

    public class Temp_EmailId
    {
        public long HRMEREQEM_Id { get; set; }
        public long HRMEREQ_Id { get; set; }
        public string HRMEREQEM_EmailId { get; set; }
        public string HRMEREQEM_Flag { get; set; }
        public bool HRMEREQEM_ActiveFlg { get; set; }
    }
}
