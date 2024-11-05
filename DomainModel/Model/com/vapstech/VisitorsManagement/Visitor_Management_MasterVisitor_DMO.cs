using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.VisitorsManagement
{
    [Table("Visitor_Management_MasterVisitor", Schema = "VM")]
    public class Visitor_Management_MasterVisitor_DMO:CommonParamDMO
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long VMMV_Id { get; set; }
        public long MI_Id { get; set; }
        public string VMMV_VisitorName { get; set; }
        public long VMMV_VisitorContactNo { get; set; }
        public string VMMV_VisitorEmailid { get; set; }
        public string VMMV_IdentityCardType { get; set; }
        public string VMMV_CardNo { get; set; }
        public string VMMV_CardImage { get; set; }
        public string VMMV_FromPlace { get; set; }
        public string VMMV_FromAddress { get; set; }
        public string VMMV_MeetingDuration { get; set; }
        public string VMMV_MeetingLocation { get; set; }
        public DateTime VMMV_MeetingDateTime { get; set; }
        public DateTime? VMMV_ExitDate { get; set; }
        public string VMMV_MeetingPurpose { get; set; }
        public string VMMV_PersonsAccompanying { get; set; }
        public string VMMV_AuthorisationBy { get; set; }
        public long VMMV_ToMeet { get; set; }
        public string VMMV_PersonToMeet { get; set; }
        public string VMMV_EntryDateTime { get; set; }
        public string VMMV_ExitDateTime { get; set; }
        public string VMMV_VehicleNo { get; set; }
        public string VMMV_VisitTypeFlg { get; set; }
        public string VMMV_CkeckedInOutStatus { get; set; }       
        public string VMMV_Remarks { get; set; }
        public string VMMV_VisitorPhoto { get; set; }
        public string VMMV_VisitorFingerPrint { get; set; }
        public string VMMV_DocumentUpload { get; set; }
        public bool VMMV_ActiveFlag { get; set; }      
        public bool? VMMV_ExternalFlg { get; set; }      
        public long? VMMV_UpdatedBy { get; set; }
        public long? VMMV_CreatedBy { get; set; }
        public string VMMV_IDCardNo { get; set; }
        public bool? VMMV_IDCardReturnedFlg { get; set; }
        public bool? VMMV_BlocekFlg { get; set; }
        public DateTime? VMMV_IDCardReturnedDateTime { get; set; }
        public List<Visitor_Management_Visitor_Appointment_DMO> Visitor_Management_Visitor_Appointment_DMO { get; set; } 
        public List<Visitor_Management_Visitor_ToMeetDMO> Visitor_Management_Visitor_ToMeetDMO { get; set; }
        public List<Multiple_VisitorDMO> Multiple_VisitorDMO { get; set; }
        public List<VM_Master_Visitor_FileDMO> VM_Master_Visitor_FileDMO { get; set; }
    }
}