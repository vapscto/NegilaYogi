using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.VisitorsManagement
{
   public class V_AppointmentApprovalReport_DTO
    {
        
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public DateTime VMAP_MeetingDateTime { get; set; }
        public Array institute { get; set; }
        public Array institutionlist { get; set; }
        public Array viewlist { get; set; }
        public Array month_list { get; set; }
        public int monthid { get; set; }
        public string monthname { get; set; }
        public string month_id { get; set; }
        public string fromdate { get; set; }
        public string todate { get; set; }
        public Array yarname { get; set; }
        public string ASMAY_Year { get; set; }
        public long ASMAY_Id { get; set; }
        public string all1 { get; set; }
        public string radiotype { get; set; }
        public long roleId { get; set; }
        public V_AppointmentApprovalReport_DTO[] selected_Inst { get; set; }
    }
}
