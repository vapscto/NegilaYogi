using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.Portals.Employee
{
    [Table("HR_Master_Employee_Update_Request")]
    public class HR_Master_Employee_Update_RequestDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HRMEREQ_Id { get; set; }
        public long HRME_Id { get; set; }
        public long MI_Id { get; set; }
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
        public long? CasteCategoryId { get; set; }
        public long? CasteId { get; set; }
        public long? ReligionId { get; set; }
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
        public List<HR_Master_Employee_Update_Request_EmailIdDMO> HR_Master_Employee_Update_Request_EmailIdDMO { get;set;}
        public List<HR_Master_Employee_Update_Request_MobileNoDMO> HR_Master_Employee_Update_Request_MobileNoDMO { get;set;}
    }
}
