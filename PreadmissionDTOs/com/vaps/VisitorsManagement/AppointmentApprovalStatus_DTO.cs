using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.VisitorsManagement
{
    public class AppointmentApprovalStatus_DTO
    {
        public long VMAP_Id { get; set; }
        public long MI_Id { get; set; }
        public string VMAP_VisitorName { get; set; }
        public DateTime? VMAP_EntryDateTime { get; set; }
        public string VMMV_CkeckedInOutStatus { get; set; }
        public DateTime? VMAP_MeetingDateTime { get; set; }
        public int fhrors { get; set; }
        public int fminutes { get; set; }
        public string VMMV_ExitDateTime { get; set; }
        public string VMAP_Remarks { get; set; }
        public string HRME_AppDownloadedDeviceId { get; set; }
        public long? VMMV_UpdatedBy { get; set; }
        public long UserId { get; set; }
        public string VMAP_ReminderSchedule { get; set; }
        public string type { get; set; }
        public string VMAP_FromAddress { get; set; }
        public string VMAP_ReminderFlag { get; set; }
        public string VMAP_RepeatFlag { get; set; }
        public string flag { get; set; }
        public long? VMAP_HRME_Id { get; set; }
        public string VMAP_Status { get; set; }
        public string VMAP_MeetingTiming { get; set; }
        public string VMAP_MeetingToTime { get; set; }
        public DateTime? VMAP_ReminderDate { get; set; }
        public long VMAP_VisitorContactNo { get; set; }
        public long HRME_Id { get; set; }
        public string VMAP_VisitorEmailid { get; set; }
        public string VMAP_FromPlace { get; set; }
        public string VMAP_MeetingPurpose { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
        public bool VMAP_ActiveFlag { get; set; }
        public Array editfiles { get; set; }
        public Array visitorlist { get; set; }
        public Array editDetails { get; set; }
        public string returnVal { get; set; }
        public Array vis_list { get; set; }
        public string empname { get; set; }
        public Array emplist { get; set; }
        public Array griddata { get; set; }
        public Array institution { get; set; }
        public Array institutionlist { get; set; }
        public vmsappdocDTOq[] filelist { get; set; }
        public string VMAP_Feedback { get; set; }
        public string VMAP_RescheduleReason { get; set; }
    }


    public class vmsappdocDTOq
    {
        public long gridid { get; set; }
        public long cfileid { get; set; }
        public string cfilename { get; set; }
        public string cfilepath { get; set; }
        public string cfiledesc { get; set; }
        public string name { get; set; }
    }
}