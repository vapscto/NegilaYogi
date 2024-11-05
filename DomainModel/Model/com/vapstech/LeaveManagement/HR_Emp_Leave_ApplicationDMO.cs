using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.LeaveManagement
{
    [Table("HR_Emp_Leave_Application")]
    public class HR_Emp_Leave_ApplicationDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        
        public long HRELAP_Id { get; set; }
        public long  MI_Id {get;set;}
        public long HRME_Id  {get;set;}
        public string HRELAP_ApplicationID  {get;set;}
        public  DateTime HRELAP_ApplicationDate  {get;set;}
        public DateTime? HRELAP_FromDate  {get;set;}
        public DateTime? HRELAP_ToDate  {get;set;}
        public  decimal  HRELAP_TotalDays  {get;set;}
        public  string HRELAP_LeaveReason  {get;set;}
        public long HRELAP_ContactNoOnLeave { get; set; }
        public DateTime  HRELAP_ReportingDate  {get;set;}
        public  string HRELAP_ApplicationStatus  {get;set;}
        public string HRELAP_SanctioningLevel { get; set; }
        public bool HRELAP_FinalFlag { get; set; }
        public bool HRELAP_ActiveFlag { get; set; }
        public bool? HRELAP_CompOffCreditApplFlg { get; set; }
        public long? HRELAP_CreatedBy { get; set; }
        public long? HRELAP_UpdatedBy { get; set; }
       // public long HR_Emp_OB_Leave_DMOHREOBL_Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
       public string HRELAP_SupportingDocument { get; set; }
    }
}
