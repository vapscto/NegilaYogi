using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.VisitorsManagement
{
   public class Visitor_Management_Appointment_DTO:CommonParamDTO
    {
        public long VMAP_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRMEEM_Id { get; set; }
        public long IVRMUL_Id { get; set; }
        public long VMAPVF_Id { get; set; }
        public long? VMAP_HRME_Id { get; set; }
        public string HRME_EmailId { get; set; }
        public string VMAP_VisitorName { get; set; }
        public long VMAP_VisitorContactNo { get; set; }
        public string VMAP_VisitorEmailid { get; set; }
        public string VMAP_FromPlace { get; set; }
        public string VMAP_MeetingTiming { get; set; }
        public string VMAP_Status { get; set; }
        public string VMAP_FromAddress { get; set; }
        public string VMAP_MeetingDuration { get; set; }
        public string VMAP_MeetingLocation { get; set; }
        public string VMAP_MeetingDateTime { get; set; }
        public string VMAP_MeetingPurpose { get; set; }
        public string VMAP_PersonsAccompanying { get; set; }
        public string VMAP_AuthorisationBy { get; set; }
        public string VMAP_ToMeet { get; set; }
        public DateTime? VMAP_EntryDateTime { get; set; }
        public string VMAP_VisitTypeFlg { get; set; }
        public string VMAP_Remarks { get; set; }
        public string requestedby { get; set; }
        public bool VMAP_ActiveFlag { get; set; }
        public long? VMAP_CreatedBy { get; set; }
        public long? VMAP_UpdatedBy { get; set; }

        public string VMAP_MeetingFromTime { get; set; }
        public string VMAP_MeetingToTime { get; set; }
        public long UserId { get; set; }
        public Array extravisitor { get; set; }
        public Array editfiles { get; set; }
        public Array getdata { get; set; }
        public Array editDetails { get; set; }
        public bool returnval { get; set; }
        public string returnval21 { get; set; }
        public bool duplicate { get; set; }
        public Array emplist { get; set; }
        public vmsappdocDTO[] filelist { get; set; }
        public visitordto[] visitordto { get; set; }

        public string HRME_EmployeeFirstName { get; set; }
        public long HRME_Id { get; set; }

        public long VMVAP_Id { get; set; }       
        public long? VMMV_Id { get; set; }
        public string VMAP_RequestFromTime { get; set; }
        public string VMAP_RequestToTime { get; set; }
        public Array institution { get; set; }
    }

    public class vmsappdocDTO
    {
        public long gridid { get; set; }
        public long cfileid { get; set; }
        public string cfilename { get; set; }
        public string cfilepath { get; set; }
        public string cfiledesc { get; set; }
        public string name { get; set; }
    }

    public class visitordto
    {
        public long VMAPVI_Id { get; set; }
        public string VMAPVI_VisitorName { get; set; }
        public string VMAPVI_VisitorContactNo { get; set; }
        public string VMAPVI_VisitorEmailId { get; set; }
        public string VMAPVI_FromPlace { get; set; }
        public string VMAPVI_VisitorAddress { get; set; }
        public string VMAPVI_Remarks { get; set; }
    }
}
