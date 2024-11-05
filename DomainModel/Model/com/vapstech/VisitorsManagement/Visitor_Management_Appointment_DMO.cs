using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.VisitorsManagement
{
    [Table("Visitor_Management_Appointment", Schema = "VM")]
    public class Visitor_Management_Appointment_DMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long VMAP_Id { get; set; }
        public long MI_Id { get; set; }
        public string VMAP_VisitorName { get; set; }
        public long VMAP_VisitorContactNo { get; set; }
        public string VMAP_VisitorEmailid { get; set; }
        public string VMAP_FromPlace { get; set; }
        public string VMAP_FromAddress { get; set; }
        public string VMAP_MeetingDuration { get; set; }
        public string VMAP_MeetingLocation { get; set; }
        public DateTime? VMAP_MeetingDateTime { get; set; }
        public string VMAP_MeetingPurpose { get; set; }
        public string VMAP_PersonsAccompanying { get; set; }
        public string VMAP_AuthorisationBy { get; set; }
        public string VMAP_ToMeet { get; set; }
        public DateTime? VMAP_EntryDateTime { get; set; }
        public string VMAP_VisitTypeFlg { get; set; }
        public string VMAP_Remarks { get; set; }
        public bool VMAP_ActiveFlag { get; set; }
        public long? VMAP_CreatedBy { get; set; }
        public long? VMAP_UpdatedBy { get; set; }
        public string VMAP_ReminderSchedule { get; set; }
        public string VMAP_ReminderFlag { get; set; }
        public string VMAP_RepeatFlag { get; set; }
        public long? VMAP_HRME_Id { get; set; }
        public string VMAP_Status { get; set; }
        public string VMAP_MeetingTiming { get; set; }
        public DateTime? VMAP_ReminderDate { get; set; }
        public string VMAP_ChekInOutStatus { get; set; }
        public string VMAP_ExitDateTime { get; set; }
        public string VMAP_MeetingFromTime { get; set; }
        public string VMAP_MeetingToTime { get; set; }
        public string VMAP_RequestFromTime { get; set; }
        public string VMAP_RequestToTime { get; set; }
        public string VMAP_Feedback { get; set; }
        public string VMAP_RescheduleReason { get; set; }
        public List<Visitor_Management_Visitor_Appointment_DMO> Visitor_Management_Visitor_Appointment_DMO { get; set; }
        public List<Visitor_Management_Appointment_FilesDMO> Visitor_Management_Appointment_FilesDMO { get; set; }
        public List<Visitor_Management_Appointment_VisitorsDMO> Visitor_Management_Appointment_VisitorsDMO { get; set; }

    }
}
