using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Transport
{
    public class RouteSessionScheduleDTO
    {
        public long TRRSC_Id { get; set; }
        public long MI_Id { get; set; }
        public DateTime TRRSC_Date { get; set; }
        public string TRRSC_ScheduleName { get; set; }
        public bool TRRSC_ActiveFlag { get; set; }
        public long TRMR_Id { get; set; }
        public Array getroute { get; set; }
        public Array getdata { get; set; }
        public bool returnval { get; set; }
        public string message { get; set; }
        public string TRMR_RouteName { get; set; }
        public Array geteditdata { get; set; }

        public long TRRSCS_Id { get; set; }
       
        public long TRMS_Id { get; set; }
      
        public long ASMAY_Id { get; set; }
        public string TRRSCS_Day { get; set; }
        public string TRRSCS_FromTime { get; set; }
        public string TRRSCS_ToTime { get; set; }
        public Array YearList { get; set; }
        public Array sessionlist { get; set; }
        public Array schdulelist { get; set; }
        public Array getlocationlist { get; set; }
        public Array getpopupdata { get; set; }
        public DateTime TRRSCS_Date { get; set; }
        public string TRML_LocationName { get; set; }
        public long TRML_Id { get; set; }
        public int TRMRL_Order { get; set; }
        public string ASMAY_Year { get; set; }
        public weekdays[] weekdays { get; set; }
        public long TRRSCSL_Id { get; set; }
        public string TRRSCSL_ArrivalTime { get; set; }
        public string TRRSCSL_DepartureTime { get; set; }
        public int TRRSCSL_Order { get; set; }
        public string TRMS_SessionName { get; set; }
        public RouteSessionScheduleDTO[] loclixt { get; set; }
        public bool TRRSCS_ActiveFlg { get; set; }
        public bool TRRSCSL_ActiveFlg { get; set; }
    }

    public class weekdays
    {
        public int id { get; set; }
        public string type { get; set; }
    }
}
