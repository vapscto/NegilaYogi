using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.VisitorsManagement
{
    public class AppointmentStatusDTO
    {
        //public long AMVM_Id { get; set; }
        //public long MI_Id { get; set; }
        //public string AMVM_Name { get; set; }
        //public DateTime? Date_Visit { get; set; }
        //public string Time_Visit { get; set; }
        //public string AMVM_Status { get; set; }
        //public string AMVM_Out_Time { get; set; }
        //public Array visitorlist { get; set; }
        //public Array editDetails { get; set; }
        //public string returnVal { get; set; }
        //public Array vis_list { get; set; }


        public long VMMV_Id { get; set; }
        public long MI_Id { get; set; }
        public string VMMV_VisitorName { get; set; }
        public DateTime VMMV_MeetingDateTime { get; set; }
        public DateTime? VMMV_ExitDate { get; set; }
        public string VMMV_CkeckedInOutStatus { get; set; }
        public string VMMV_EntryDateTime { get; set; }
        public string VMMV_ExitDateTime { get; set; }
        public string VMMV_Remarks { get; set; }
        public long? VMMV_UpdatedBy { get; set; }
        public long UserId { get; set; }
        public Array visitorlist { get; set; }
        public Array editDetails { get; set; }
        public string returnVal { get; set; }
        public Array vis_list { get; set; }
        public Array FloreList { get; set; }
        public string empname { get; set; }
    }
}